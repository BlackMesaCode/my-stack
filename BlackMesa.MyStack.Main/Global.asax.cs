using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using BlackMesa.MyStack.Main.App_Start;

namespace BlackMesa.MyStack.Main
{
    public static class Global
    {
        public static readonly string CultureConstraints = @"\w{2,3}(-\w{4})?(-\w{2,3})?";
        public static readonly string LanguageConstraints = @"\w{2}";
        public static readonly object IdConstraints = @"\d+";
        public static readonly List<string> AllowedCultures = new List<string> { "en-US", "de-DE" };
        public static readonly List<string> AllowedLanguages = new List<string> { "en", "de" };
        public static readonly TimeZoneInfo ApplicationTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
    }

    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }


}