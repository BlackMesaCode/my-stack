using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;
using BlackMesa.MyStack.Main.DataLayer;
using Microsoft.AspNet.Identity;

namespace BlackMesa.MyStack.Main.Controllers
{

    public abstract class BaseController : Controller
    {

        //Executes after the controller is created
        protected override void Initialize(RequestContext requestContext)
        {
            //Call the Initialize method from the base class
            base.Initialize(requestContext);

            //Make sure Ajax requests are not cached client-side (Chrome fix)
            if (requestContext.HttpContext.Request.IsAjaxRequest())
                requestContext.HttpContext.Response.Cache.SetNoStore();


            //Language and Culture Handling
            //The name of the chosen culture
            string culture;
            


            // Try to determine the user culture in different ways
            // Try to get the culture from the route data (url)
            if (RouteData.Values["culture"] != null &&
                !String.IsNullOrWhiteSpace(RouteData.Values["culture"].ToString()))
                culture = SetCulture(RouteData.Values["culture"].ToString());
            // Try to get the prefered culture from users account settings
            else
            {
                var _dbContext = new MyStackDbContext();
                var currentUser = _dbContext.Users.Find(User.Identity.GetUserId());
                if (currentUser != null && !String.IsNullOrEmpty(currentUser.Culture))
                {
                    culture = SetCulture(currentUser.Culture);
                }
                // Try to set the culture based on the browser settings
                else if (HttpContext.Request.UserLanguages != null)
                    culture = SetCulture(HttpContext.Request.UserLanguages[0]);

                //If nothing works, take the default culture
                else
                    culture = SetCulture("");

            }




            // set the culture value into route data
            RouteData.Values["culture"] = culture;

            // fill viewBag with culture info
            ViewBag.AllowedCultures = Global.AllowedCultures;
            ViewBag.CurrentCulture = culture;

            // get language out of culture infos
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);
            var language = ci.IsNeutralCulture ? ci.TwoLetterISOLanguageName : ci.Parent.TwoLetterISOLanguageName;

            // set the language value into route data
            RouteData.Values["language"] = language;

            // fill viewBag with language info
            ViewBag.CurrentLanguage = language;

        }


        /// <summary>
        /// Sets and returns the CurrentCulture and CurrentUiCulture of the current thread to the culture specified 
        /// by cultureName. The name is checked against a list of allowed cultures.
        /// </summary>
        /// <param name="cultureName">The culture identifier, e.g. "en-us"</param>
        public static string SetCulture(string cultureName)
        {
            //Check if languange is allowed, otherwise replace it with the default
            var selectedCultureName = Global.AllowedCultures.Any(c => c.ToLower() == cultureName.ToLower())
                ? cultureName
                : Global.AllowedCultures.First();

            CultureInfo culture = CultureInfo.CreateSpecificCulture(selectedCultureName);
            //The CurrentCulture is responsible for the renderung of DateTimes and floating point numbers
            Thread.CurrentThread.CurrentCulture = culture;
            //The CurrentUiCulture is responsible for selecting the right ressources
            Thread.CurrentThread.CurrentUICulture = culture;

            return selectedCultureName;
        }
    }
}



