using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ChatRoom.Helpers;
using ChatRoom.Models;

namespace ChatRoom.Controllers
{
    public class AuthController : Controller
    {
        ChatDBEntities db = new ChatDBEntities();
        // GET: Auth
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult LoggedOutRedirect()
        {
            return View();
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost, AllowAnonymous]
        public ActionResult Login(string userName, string password)
        {

            User user = db.Users.Find(userName);

            if (GlobalVariables.LoggedIn != false)
            {
                return Content("You are already signed in. If this is not your account loggout and sign in with your account.");
            }
            else if (user.Password == password)
            {
                //try to create token
                var createTokenResults = CreateDBObject.CreateUserToken(userName);
                UserToken tokenCreated = db.UserTokens.Find(createTokenResults.Identifier);
                //add user and token to session
                Session["token"] = createTokenResults.Identifier;
                Session["user"] = user.UserName;
                //Session["user"] = user.UserName;

                string test = Session["token"].ToString();
                //Authenticate 
                FormsAuthentication.SetAuthCookie(createTokenResults.Identifier, false);

                //set global logged in variable to true
                //GlobalVariables.LoggedIn = true;
                
                //Redirect logged in user to home page
                return RedirectToAction("index", "home");
            } 
            else
            {
                return Content("Login failed. Invalid username or password.");
            }

        }

        

        [HttpPost]
        public ActionResult UserSignOut()
        {
            UserToken tokenToRemove = db.UserTokens.Find(Session["userToken"]);
            db.UserTokens.Remove(tokenToRemove);
            db.SaveChanges();

            UserToken removedToken = db.UserTokens.Find(Session["userToken"]);
            Session.Clear();

            FormsAuthentication.SignOut();

            return RedirectToAction("LoggedOutRedirect", "Auth");
        }
    }
}