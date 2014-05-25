using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BlackMesa.MyStack.Main.DataLayer;
using BlackMesa.MyStack.Main.Models;
using BlackMesa.MyStack.Main.Utilities;
using BlackMesa.MyStack.Main.ViewModels.Test;

namespace BlackMesa.MyStack.Main.Controllers
{
    [Authorize]
    public class TestController : BaseController
    {
        private readonly MyStackRepository _myStackRepo = new MyStackRepository(new MyStackDbContext());

        public ActionResult Setup(string folderId, bool selectAll = false)
        {
            var folder = _myStackRepo.GetFolder(folderId);

            if (selectAll)
                _myStackRepo.SelectFolder(folder);

            var numberOfSelectedCards = 0;
             _myStackRepo.GetCardCount(folder, ref numberOfSelectedCards, true, true, false);


            var viewModel = new SetupTestViewModel
            {
                FolderId = folder.Id.ToString(),
                TestOnlyDueCards = true,
                ReverseSides = false,
                NumberOfSelectedCards = numberOfSelectedCards,
                OrderType = OrderType.Ordered,
                TestType = TestType.Normal,
            };

            return View(viewModel);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Setup(SetupTestViewModel viewModel)
        {

            var folder = _myStackRepo.GetFolder(viewModel.FolderId);

            // Selection

            var cardsToTest = new List<Card>();
            _myStackRepo.GetAllCardsInFolder(folder, ref cardsToTest, true);

            if (viewModel.TestOnlyDueCards)
            {
                cardsToTest = cardsToTest.Where(c => c.IsDue).ToList();
            }

            // Ordering
            if (viewModel.OrderType == OrderType.Shuffled)
                cardsToTest.Shuffle();


            // Create Test

            var test = new Test
            {
                Id = Guid.NewGuid(),
                TestOnlyDueCards = viewModel.TestOnlyDueCards,
                ReverseSides = viewModel.ReverseSides,
                OrderType = viewModel.OrderType,
                TestType = viewModel.TestType,
                CardsToTest = cardsToTest,
                TestStatus = TestStatus.InProgress,
                StartTime = DateTime.Now,
            };

            // Save Test to User Session

            Session["Test"] = test;

            return RedirectToAction("GetTestItem",
                new
                {
                    testId = test.Id.ToString(),
                    position = 0,
                    folderId = viewModel.FolderId
                });
        }

        public ActionResult GetTestItem(string testId, string folderId, int positionOffset = 0)
        {
            var viewModel = GetTestItemViewModel(testId, folderId, positionOffset);
            if (viewModel == null)
                return RedirectToAction("Completed", "Test", new {testId = testId, folderId = folderId});

            return View(viewModel);
        }


        private TestItemViewModel GetTestItemViewModel(string testId, string folderId, int positionOffset = 0)
        {
            var test = Session["Test"] as Test;
            Card card = null;
            int cardsLeft = 0;
            if (positionOffset == 0)
            {
                var dbCardsToTest = test.CardsToTest.Select(cardToTest => _myStackRepo.GetCard(cardToTest.Id.ToString()));
                var untestedCards = dbCardsToTest.Where(c => !c.TestItems.Exists(i => i.TestId == testId)); ;

                if (test.TestType == TestType.Normal)
                    cardsLeft = dbCardsToTest.Count() - dbCardsToTest.Count(c => c.TestItems.Exists(i => i.TestId == testId && i.Result == TestResult.Correct));
                else if (test.TestType == TestType.SinglePass)
                    cardsLeft = untestedCards.Count();

                if (untestedCards.Any())
                {
                    card = untestedCards.First();
                }
                else if (test.TestType == TestType.Normal)
                {
                    var wrongCardsToRetest = dbCardsToTest
                        .Where(
                            c =>
                                c.TestItems.Last(i => i.TestId == testId).Result == TestResult.PartlyCorrect
                                || c.TestItems.Last(i => i.TestId == testId).Result == TestResult.Wrong);
                    if (wrongCardsToRetest.Any())
                    {
                        card = wrongCardsToRetest.OrderBy(c => c.TestItems.Last(i => i.TestId == testId).StartTime).First();
                    }
                }
            }
            else
            {
                var lastTestedCardsOrderedByStartTime = test.CardsToTest.Where(c => c.TestItems.Exists(i => i.TestId == testId))
                    .OrderBy(c => c.TestItems.Single(i => i.TestId == testId).StartTime);
                card = lastTestedCardsOrderedByStartTime.ElementAt(lastTestedCardsOrderedByStartTime.Count() - positionOffset + 1);

            }
            if (card != null)
            {
                var frontSide = (test.ReverseSides ? card.BackSide : card.FrontSide);
                var backSide = (test.ReverseSides ? card.FrontSide : card.BackSide);

                return new TestItemViewModel
                {
                    FolderId = folderId,
                    TestId = testId,
                    CardId = card.Id.ToString(),
                    FrontSide = frontSide,
                    BackSide = backSide != null ? backSide.AddLinkTags() : null,
                    StartTime = DateTime.Now,
                    Result = TestResult.Correct,
                    CardsLeft = cardsLeft,
                };
            }
            return null;
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveTestItem(TestItemViewModel resultViewModel)
        {
            var currentTime = DateTime.Now;

            var test = Session["Test"] as Test;
            var testedCard = _myStackRepo.GetCard(resultViewModel.CardId);

            _myStackRepo.AddTestItem(testedCard.Id.ToString(), resultViewModel.TestId,
                resultViewModel.StartTime, currentTime, resultViewModel.Result);
            
            return RedirectToAction("GetTestItem",
                new
                {
                    testId = resultViewModel.TestId,
                    folderId = resultViewModel.FolderId
                });
        }


        public ActionResult Aborted(string testId, string folderId)
        {
            Session["Test"] = null;

            return RedirectToAction("Details", "Folder", new {id = folderId});
        }


        public ActionResult Completed(string testId, string folderId)
        {
            var test = Session["Test"] as Test;

            var viewModel = new TestCompletedViewModel
            {
                FolderId = folderId,
                Duration = DateTime.Now - test.StartTime,
                TotalCount = test.CardsToTest.Count,
                CorrectCount = test.CardsToTest.Count(c => c.TestItems.Any(i => i.TestId == testId && i.Result == TestResult.Correct)),
                PartlyCorrectCount = test.CardsToTest.Count(c => c.TestItems.Any(i => i.TestId == testId && i.Result == TestResult.PartlyCorrect)),
                WrongCount = test.CardsToTest.Count(c => c.TestItems.Any(i => i.TestId == testId && i.Result == TestResult.Wrong)),
            };

            Session["Test"] = null;

            return View(viewModel);
        }

    }
}