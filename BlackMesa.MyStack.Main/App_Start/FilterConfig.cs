using System.Web.Mvc;
using BlackMesa.MyStack.Main.Utilities;

namespace BlackMesa.MyStack.Main.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CheckForMaintenance());
        }
    }
}