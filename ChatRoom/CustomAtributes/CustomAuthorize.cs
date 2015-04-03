using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChatRoom.Helpers;

namespace ChatRoom.CustomAtributes
{
    
    public class CustomAuthorize : ActionFilterAttribute
    {
       
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //get current session token
            if(!Validation.SessionToken())
            {
                filterContext.Result = new RedirectResult("/auth/LoggedOutRedirect");
            }
        }
    }
}