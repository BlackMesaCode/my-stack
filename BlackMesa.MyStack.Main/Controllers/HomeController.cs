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

        public ActionResult ImportFromHD()
        {
            var rootDir = new DirectoryInfo(@"D:\RootFolder");
            WalkDirectoryTree(rootDir, true);
            return Content("Import completed");
        }

        public void WalkDirectoryTree(DirectoryInfo folder, bool isRootFolder = false)
        {
            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;

            var createdFolderId = "";
            if (!isRootFolder)
            {
                createdFolderId = _myStackRepo.AddFolder(folder.Name, User.Identity.GetUserId(),
                    _myStackRepo.GetRootFolder(User.Identity.GetUserId()).Id.ToString());
            }


            try
            {
                files = folder.GetFiles("*.*");
            }
            catch (UnauthorizedAccessException e)
            {

            }

            catch (System.IO.DirectoryNotFoundException e)
            {

            }


            if (files != null)
            {
                foreach (var file in files)
                {
                    var createdSubFolderId = _myStackRepo.AddFolder(file.Name.Remove(file.Name.IndexOf(".txt"), 4), User.Identity.GetUserId(),
                        createdFolderId);
                    //using ()
                    //{
                    var stream = file.OpenRead();
                    var reader = new StreamReader(stream);
                    var text = reader.ReadToEnd();
                    CreateNextCard(text, createdSubFolderId);
                    stream.Close();
                    //    stream.Dispose();
                    //}
                }


                subDirs = folder.GetDirectories();

                foreach (var subDir in subDirs)
                {
                    WalkDirectoryTree(subDir);
                }
            }
        }


        public void CreateNextCard(string text, string folderId)
        {
            var divider = "---------------------------------------------------------------------";
            var positionOfFirstDivider = text.IndexOf(divider);

            if (positionOfFirstDivider != -1)
            {
                var positionOfFrontSideStart = positionOfFirstDivider + divider.Length;
                var positionOfSecondDivider = text.IndexOf(divider, positionOfFirstDivider + divider.Length);
                var frontSide = text.Substring(positionOfFrontSideStart,
                    positionOfSecondDivider - positionOfFrontSideStart)
                    .Trim(new char[] { ' ', '\n', '\r', '\t' });
                var positionOfBackSideStart = positionOfSecondDivider + divider.Length;
                var positionOfThirdDivider = text.IndexOf(divider, positionOfSecondDivider + divider.Length);
                var backSide = String.Empty;
                if (positionOfThirdDivider == -1)
                {
                    backSide = text.Substring(positionOfBackSideStart).Trim(new char[] { ' ', '\n', '\r', '\t' });
                }
                else
                {
                    backSide = text.Substring(positionOfBackSideStart, positionOfThirdDivider - positionOfBackSideStart)
                        .Trim(new char[] { ' ', '\n', '\r', '\t' });
                }

                _myStackRepo.AddCard(folderId, User.Identity.GetUserId(), frontSide, backSide);

                if (positionOfThirdDivider != -1)
                    CreateNextCard(text.Substring(positionOfThirdDivider), folderId);
            }

        }

    }
}