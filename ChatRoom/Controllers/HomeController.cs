using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChatRoom.Models;
using System.Data;
using System.Data.Entity;
using System.Web.Caching;
using System.Web.SessionState;
using System.Web.Configuration;
using ChatRoom.Helpers;
using ChatRoom.CustomAtributes;
using Newtonsoft.Json.Linq;

namespace ChatRoom.Controllers
{
    
    public class HomeController : Controller
    {
        
        //global variables
        //Cache cache = new Cache();
        ChatDBEntities db = new ChatDBEntities();
        //SessionStateItemCollection session = new SessionStateItemCollection();

        //[Authorize]
        //[CustomAuthorize]
        public ActionResult Index()
        {
            
            /*
            if(Session["token"] == null)
            {
                Session["token"] = "";
            }

            if (TokenValidation.ValidateUserToken(Session["token"].ToString()))
            {
                //db.Database.ExecuteSqlCommand("update UserTokens set LastUsed = CURRENT_TIMESTAMP where Token = " + Session["token"].ToString());
                //db.Database.SqlQuery(null, "update UserTokens set LastUsed = CURRENT_TIMESTAMP where Token = " + Session["token"].ToString());
                return View();
            }
            else
                return RedirectToAction("LoggedOutRedirect");  */
            ViewBag.CurrentUser = Session["user"];
            return View();
        }

        [HttpGet, AllowAnonymous]
        public ActionResult CreateAccount()
        {
            
            return View();
        }

        [HttpPost, AllowAnonymous]
        public ActionResult CreateAccount(string userName, string password)
        {
            string results;
            var createAccountAtempt = CreateDBObject.CreateUser(userName, password);
            
            if (createAccountAtempt.Result == true)
            {
                
                results = "User: " + createAccountAtempt.Identifier + " successfully created!";  
            }
            else
            {
                results = "User creation failed.";
            }

            return Content(results);
        }

        public ActionResult StartAConversation()
        {
            List<string> users = db.Users.Select(x => x.UserName).ToList<string>();
            users.Remove(Session["user"].ToString());
            ViewBag.Users = users;

            return View();
        }

        [HttpPost]
        public ActionResult SendMessage(string friend, string message, string conversationID)
        {         
            var attempCreatingConversation = CreateDBObject.CreateMessage(Session["user"].ToString(), friend, message, conversationID);
            if (attempCreatingConversation.Result)
            {
                Conversation createdConversation = db.Conversations.Find(attempCreatingConversation.Identifier);
                return RedirectToAction("MyConversations", "Home");
            }
            else
                return Content("Failed to send message");
            //return RedirectToAction("MyConversations", "Home");
        }

        public ActionResult MyConversations()
        {
            //make a list of all conversations of current user
            //List<Conversation> conversations =  db.Users.Find(Session["user"]).Conversations.ToList<Conversation>();
            string currentUser = Session["user"].ToString(); 
            List<Conversation> conversations = db.Conversations.Where(x => x.User1 == currentUser || x.User2 == currentUser).ToList<Conversation>();
            
            
            //add conversation list to viewbag
            ViewBag.Conversations = conversations;
            ViewBag.CurrentUser = currentUser;
            
            return View();
        }

        public string GetCurrentUser()
        {
            return Session["user"].ToString();
        }
        
    }
}