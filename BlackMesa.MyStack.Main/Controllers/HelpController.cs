using System;
using System.IO;
using System.Web.Mvc;
using BlackMesa.MyStack.Main.DataLayer;
using BlackMesa.MyStack.Main.ViewModels.Help;
using Microsoft.AspNet.Identity;

namespace BlackMesa.MyStack.Main.Controllers
{
    public class HelpController : BaseController
    {
        //private readonly MyStackRepository _myStackRepo = new MyStackRepository(new MyStackDbContext());

        public ActionResult Leitner(string folderId)
        {
            var viewModel = new LeitnerViewModel
            {
                FolderId = folderId,
            };
            return View(viewModel);
        }

    }
}