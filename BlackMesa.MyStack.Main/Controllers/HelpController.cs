﻿using System;
using System.IO;
using System.Web.Mvc;
using BlackMesa.MyStack.Main.DataLayer;
using BlackMesa.MyStack.Main.ViewModels.Help;
using Microsoft.AspNet.Identity;

namespace BlackMesa.MyStack.Main.Controllers
{
    public class HelpController : BaseController
    {

        public ActionResult Leitner(string folderId)
        {
            var viewModel = new LeitnerViewModel
            {
                FolderId = folderId,
            };
            return View(viewModel);
        }


        public ActionResult TestTypes(string folderId)
        {
            var viewModel = new TestTypesViewModel
            {
                FolderId = folderId,
            };
            return View(viewModel);
        }


        public ActionResult FirstSteps()
        {
            return View();
        }

    }
}