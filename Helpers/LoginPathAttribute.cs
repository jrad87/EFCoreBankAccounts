using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;

namespace EFCoreBankAccounts {
    public class LoginPathAttribute : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext context) {
            base.OnActionExecuting(context);
            if(context.HttpContext.Session.GetInt32("Id") != null){
                var route = new RouteValueDictionary(new {
                    controller = "Accounts",
                    Action = "Index"
                });
                context.Result = new RedirectToRouteResult(route);
            }
        }
    }
}