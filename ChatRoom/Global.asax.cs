using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ChatRoom.Helpers;
using System.Web.SessionState;
using System.Web.Configuration;
using System.Data;
using ChatRoom.Models;

namespace ChatRoom
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //bad hacky code
           // GlobalVariables.LoggedIn = false;
            
        }

        
        //Check to see if user has a valid token on every request
        /*
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //load db
            ChatDBEntities db = new ChatDBEntities();
            
            //see if there is a current session
            HttpContext context = HttpContext.Current;
            if (context != null && context.Session == null)
            {
                Response.Redirect("/home/LoggedOutRedirect");
            }
                // If session token isn't valid redirect to loggoutRedirect page
                
            else if (TokenValidation.ValidateUserToken(Session["token"].ToString()))
            {
                //update last used property on token
                db.Database.ExecuteSqlCommand("update UserTokens set LastUsed = CURRENT_TIMESTAMP where Token = " + Session["token"].ToString());
                //db.Database.SqlQuery(null, "update UserTokens set LastUsed = CURRENT_TIMESTAMP where Token = " + Session["token"].ToString());
            }
            else
            {
                Response.RedirectToRoute("LoggedOutRedirect", "Home");
            }
        }*/
    }
}
