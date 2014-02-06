using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BlackMesa.MyStack.Main.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{culture}/{controller}/{action}/{id}",
                defaults: new {culture = "de-DE", controller = "Home", action = "Index", id = UrlParameter.Optional},
                constraints: new {culture = Global.CultureConstraints},
                namespaces: new[] { "BlackMesa.MyStack.Main.Controllers" }
                );
        }
    }

    // FromValuesListConstraint can be added as a route constraint for cultures
    public class FromValuesListConstraint : IRouteConstraint
    {
        public FromValuesListConstraint(params string[] values)
        {
            this._values = values;
        }

        private string[] _values;

        public bool Match(HttpContextBase httpContext,
            Route route,
            string parameterName,
            RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            // Get the value called "parameterName" from the 
            // RouteValueDictionary called "value"
            string value = values[parameterName].ToString();

            // Return true is the list of allowed values contains 
            // this value.
            return _values.Contains(value);
        }
    }
}