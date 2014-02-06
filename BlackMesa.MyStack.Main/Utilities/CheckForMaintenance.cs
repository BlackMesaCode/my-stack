using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace BlackMesa.MyStack.Main.Utilities
{
    public sealed class CheckForMaintenance : ActionFilterAttribute
    {

        private static List<string> _allowedIps = new List<string>
                                                      {
                                                          "134.3.100.237",
//                                                          "46.128.19.14"
                                                      };

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (ConfigurationManager.AppSettings["Maintenance"] == "true" && !filterContext.RouteData.Values["controller"].Equals("Maintenance"))
            {
                if (!filterContext.HttpContext.Request.IsLocal && !_allowedIps.Contains(filterContext.HttpContext.Request.UserHostAddress))
                {
                    filterContext.HttpContext.Response.RedirectToRoute(new { controller = "Maintenance", action = "Index" });
                }
            }

            base.OnActionExecuting(filterContext);
        }


    }
}