using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;

namespace EFCoreBankAccounts {
    public class LoginRequiredAttribute : ActionFilterAttribute {
        private static bool IsNotLoggedIn(ActionExecutingContext context)
        {
            return (
                context.HttpContext.Session.GetInt32("UserId") == null || 
                context.HttpContext.Session.GetInt32("AccountId") == null
            );
        }
        public override void OnActionExecuting(ActionExecutingContext context) {            
            if(IsNotLoggedIn(context)){
                var route = new RouteValueDictionary(new {
                    controller = "Home",
                    Action= "Index"
                });
                context.Result = new RedirectToRouteResult(route);
            } 
            else if(Convert.ToInt32(context.RouteData.Values["AccountId"] ?? 0) != context.HttpContext.Session.GetInt32("AccountId"))
            {
                var route = new RouteValueDictionary(new {
                    controller = "Accounts",
                    Action = "Index",
                    AccountId = context.HttpContext.Session.GetInt32("AccountId")
                }); 
                context.Result = new RedirectToRouteResult(route);
            }
            
            //Console.WriteLine("Session: {0}", context.HttpContext.Session.GetInt32("AccountId"));
            //Console.WriteLine("RouteData: {0}", context.RouteData.Values["AccountId"]);
            //Console.WriteLine((int?)context.RouteData.Values["AccountId"] == context.HttpContext.Session.GetInt32("AccountId"));
            base.OnActionExecuting(context);
        }
    }
}