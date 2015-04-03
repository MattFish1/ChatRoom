using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChatRoom.Helpers;
using System.Web.Security;

namespace ChatRoom.Controllers
{
    public class AccountController : Controller
    {

        ChatDBEntities db = new ChatDBEntities();

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string userName, string password)
        {

            string result;

            User user = db.Users.Find(userName);

            if (user.Password == password && Session["user"] == null)
            {
                //try to create token
                var createTokenResults = CreateDBObject.CreateUserToken(userName);
                UserToken tokenCreated = db.UserTokens.Find(createTokenResults.Identifier);
                //add user and token to session
                Session["token"] = createTokenResults.Identifier;
                //Session["user"] = user.UserName;

                //Authenticate 
                FormsAuthentication.SetAuthCookie(createTokenResults.Identifier, false);

                //create result output
                result = tokenCreated.User.UserName + " login Successfull. Token: " + tokenCreated.Token; //Session["userToken"]
            }
            else if (Session["token"] != null)
                result = "Login failed. Another user is already logged in.";
            else
            {
                result = "Login failed. Invalid username or password.";
            }

            return Content(result);

        }
    }
}