using System.Web.Mvc;
using BlackMesa.MyStack.Main.DataLayer;
using BlackMesa.MyStack.Main.ViewModels.Card;
using Microsoft.AspNet.Identity;

namespace BlackMesa.MyStack.Main.Controllers
{
    [Authorize]
    public class CardController : BaseController
    {

        private readonly MyStackRepository _myStackRepo = new MyStackRepository(new MyStackDbContext());

        public ActionResult Create(string folderId)
        {
            var viewModel = new CreateViewModel()
            {
                FolderId = folderId,
            };

            return View(viewModel);
        }


        [HttpPost]
        public ActionResult Create(CreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _myStackRepo.AddCard(viewModel.FolderId, User.Identity.GetUserId(), viewModel.FrontSide, viewModel.BackSide);
                return RedirectToAction("Details", "Folder", new { id = viewModel.FolderId });
            }
            return View(viewModel);
        }


        public ActionResult Edit(string id)
        {
            var card = _myStackRepo.GetCard(id);
            var viewModel = new EditViewModel
            {
                Id = card.Id.ToString(),
                FolderId = card.FolderId.ToString(),
                FrontSide = card.FrontSide,
                BackSide = card.BackSide,
            };
            return View(viewModel);
        }


        [HttpPost]
        public ActionResult Edit(EditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var card = _myStackRepo.GetCard(viewModel.Id);
                _myStackRepo.EditCard(viewModel.Id, viewModel.FrontSide, viewModel.BackSide);
                return RedirectToAction("Details", "Folder", new  { id = card.FolderId });
            }
            return View(viewModel);
        }


    }
}
