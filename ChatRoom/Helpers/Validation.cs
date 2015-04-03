using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChatRoom.Models;
using System.Web.SessionState;

namespace ChatRoom.Helpers
{
    //since Session is only availible in a controller class, create an instance a
    //controller so session variables can be passed into other classes
    public class FauxController : Controller
    {
        public string getSessionToken()
        {
            string token;

            if(GlobalVariables.LoggedIn != false)
            {
                token = Session["token"].ToString();
            }
            else{
                token = "invalid";
            }
            return token;
        }

        public FauxController()
        {

        }
    }

    public class Validation
    {
        public static bool SessionToken()
        {
            //get session token
            FauxController myController = new FauxController();
            string token = myController.getSessionToken();

            //load database
            ChatDBEntities db = new ChatDBEntities();
            //find the token that is being validated
            UserToken tokenToValidate = db.UserTokens.Find(token);
            if (tokenToValidate != null)
            {
                TimeSpan TokenLifeTime = DateTime.Now - tokenToValidate.DateCreated;
                TimeSpan TokenExpiration = new TimeSpan(0, 45, 0);
                //if token exists and is valid return true else return false
                if (TokenLifeTime < TokenExpiration)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
    }
}