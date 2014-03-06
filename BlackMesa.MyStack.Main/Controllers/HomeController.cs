using System;
using System.IO;
using System.Web.Mvc;
using BlackMesa.MyStack.Main.DataLayer;
using Microsoft.AspNet.Identity;

namespace BlackMesa.MyStack.Main.Controllers
{
    public class HomeController : BaseController
    {
        private readonly MyStackRepository _myStackRepo = new MyStackRepository(new MyStackDbContext());

        public ActionResult Index()
        {
            return View();
        }

    }
}