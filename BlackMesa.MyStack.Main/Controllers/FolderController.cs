using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BlackMesa.MyStack.Main.DataLayer;
using BlackMesa.MyStack.Main.Models;
using BlackMesa.MyStack.Main.Resources;
using BlackMesa.MyStack.Main.ViewModels.Folder;
using Microsoft.AspNet.Identity;

namespace BlackMesa.MyStack.Main.Controllers
{
    [Authorize]
    public class FolderController : BaseController
    {

        private readonly MyStackRepository _myStackRepo = new MyStackRepository(new MyStackDbContext());

        public ActionResult Index()
        {
            var rootFolder = _myStackRepo.GetRootFolder(User.Identity.GetUserId());

            if (rootFolder == null)
            {
                _myStackRepo.CreateRootFolder(Strings.Root, User.Identity.GetUserId());
                rootFolder = _myStackRepo.GetRootFolder(User.Identity.GetUserId());
                CreateTutorialFolder(rootFolder.Id.ToString());
            }

            return RedirectToAction("Details", "Folder", new {id = rootFolder.Id.ToString()});
        }


        private void CreateTutorialFolder(string parentFolderId)
        {
            var tutorialFolderId = _myStackRepo.AddFolder("Tutorial", User.Identity.GetUserId(), parentFolderId);

            _myStackRepo.AddCard(tutorialFolderId, User.Identity.GetUserId(), "FS Content", "BS Content", DateTime.Now,
                null);
        }


        public ActionResult Details(string id, bool deSelect = true)
        {
            var folder = _myStackRepo.GetFolder(id);
            var path = new List<Folder>();

            _myStackRepo.GetFolderPath(folder, ref path);
            path.Reverse();
            path.Remove(path.Last());

            if (deSelect)
                _myStackRepo.DeSelectFolder(folder);

            var dueCardsPerFolder = new Dictionary<string, int>();
            foreach (var subFolder in folder.SubFolders)
            {
                var cardCount = 0;
                _myStackRepo.GetCardCount(subFolder, ref cardCount, true, false, true);
                dueCardsPerFolder.Add(subFolder.Id.ToString(), cardCount);
            }

            var dueCards = 0;
            _myStackRepo.GetCardCount(folder, ref dueCards, false, false, true);
            dueCards += dueCardsPerFolder.Values.Sum();

            var numberOfSelectedCardsIncludingSubfolders = 0;
            _myStackRepo.GetCardCount(folder, ref numberOfSelectedCardsIncludingSubfolders, true, true, false);

            var viewModel = new DetailsViewModel
            {
                Id = folder.Id.ToString(),
                Name = folder.Name,
                IsSelected = folder.IsSelected,
                IsRootFolder = (folder.ParentFolder == null),
                Level = folder.Level,
                HasAnySelection = (folder.IsSelected || folder.SubFolders.Any(f => f.IsSelected) || folder.Cards.Any(u => u.IsSelected)),
                SubFolders = folder.SubFolders.OrderBy(f => f.Name),
                Cards = folder.Cards.OrderBy(c => c.Position),
                Path = path,
                ParentFolderId = (!folder.IsRootFolder ? folder.ParentFolder.Id.ToString() : String.Empty),
                DueCards = dueCards,
                DueCardsPerSubfolder = dueCardsPerFolder,
                NumberOfSelectedCardsIncludingSubfolders = numberOfSelectedCardsIncludingSubfolders,
            };
            return View(viewModel);
        }


        public ActionResult Create(string parentFolderId)
        {
            var viewModel = new CreateViewModel()
            {
                ParentFolderId = parentFolderId,
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _myStackRepo.AddFolder(viewModel.Name, User.Identity.GetUserId(), viewModel.ParentFolderId);
                if (!String.IsNullOrEmpty(viewModel.ParentFolderId))
                    return RedirectToAction("Details", "Folder", new { id = viewModel.ParentFolderId });
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }


        public ActionResult Edit(string id)
        {
            var folder = _myStackRepo.GetFolder(id);
            var viewModel = new EditViewModel
            {
                Id = folder.Id.ToString(),
                Name = folder.Name,
            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var folder = _myStackRepo.GetFolder(viewModel.Id);
                _myStackRepo.EditFolder(viewModel.Id, viewModel.Name);
                if (!folder.IsRootFolder)
                    return RedirectToAction("Details", "Folder", new { id = folder.ParentFolder.Id });
                return RedirectToAction("Index");
            }
        return View();
        }


        public ActionResult AddOptions(string folderId)
        {
            var viewModel = new AddOptionsViewModel
            {
                FolderId = folderId,
            };
            return View(viewModel);
        }

    }
}
