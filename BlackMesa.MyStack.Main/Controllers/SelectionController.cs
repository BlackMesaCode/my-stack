using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using BlackMesa.MyStack.Main.DataLayer;
using BlackMesa.MyStack.Main.Models;
using BlackMesa.MyStack.Main.Resources;
using BlackMesa.MyStack.Main.Utilities;
using BlackMesa.MyStack.Main.ViewModels.Selection;
using Microsoft.AspNet.Identity;


namespace BlackMesa.MyStack.Main.Controllers
{
    [Authorize]
    public class SelectionController : BaseController
    {

        private readonly MyStackRepository _myStackRepo = new MyStackRepository(new MyStackDbContext());


        public ActionResult AddFolder(string folderId, string returnFolderId)
        {
            var folder = _myStackRepo.GetFolder(folderId);
            _myStackRepo.SelectFolder(folder);
            return RedirectToAction("Details", "Folder", new {id = returnFolderId, deSelect = false });
        }

        public ActionResult RemoveFolder(string folderId, string returnFolderId)
        {
            var folder = _myStackRepo.GetFolder(folderId);
            _myStackRepo.DeSelectFolder(folder);
            return RedirectToAction("Details", "Folder", new { id = returnFolderId, deSelect = false });
        }


        public ActionResult AddFolderAjax(string folderId)
        {
            var folder = _myStackRepo.GetFolder(folderId);
            _myStackRepo.SelectFolder(folder);
            return PartialView("_AddFolder", folderId);
        }

        public ActionResult RemoveFolderAjax(string folderId)
        {
            var folder = _myStackRepo.GetFolder(folderId);
            _myStackRepo.DeSelectFolder(folder);
            return PartialView("_RemoveFolder", folderId);
        }


        public ActionResult AddCard(string cardId, string returnFolderId)
        {
            var card = _myStackRepo.GetCard(cardId);
            _myStackRepo.SelectCard(card);
            return RedirectToAction("Details", "Folder", new { id = returnFolderId, deSelect = false });
        }

        public ActionResult RemoveCard(string cardId, string returnFolderId)
        {
            var card = _myStackRepo.GetCard(cardId);
            _myStackRepo.DeSelectCard(card);
            return RedirectToAction("Details", "Folder", new { id = returnFolderId, deSelect = false });
        }

        public ActionResult AddCardAjax(string cardId)
        {
            var card = _myStackRepo.GetCard(cardId);
            _myStackRepo.SelectCard(card);
            return PartialView("_AddCard", cardId);
        }

        public ActionResult RemoveCardAjax(string cardId)
        {
            var card = _myStackRepo.GetCard(cardId);
            _myStackRepo.DeSelectCard(card);
            return PartialView("_RemoveCard", cardId);
        }


        public ActionResult Delete(string folderId)
        {
            var folder = _myStackRepo.GetFolder(folderId);

            int affectedCardsCount = 0;
            _myStackRepo.GetCardCount(folder, ref affectedCardsCount, true, true);

            var viewModel = new DeleteViewModel
            {
                Id = folder.Id.ToString(),
                AffectedCardsCount = affectedCardsCount,
            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(DeleteViewModel viewModel)
        {
            var folder = _myStackRepo.GetFolder(viewModel.Id);
            var parentFolderId = !folder.IsRootFolder ? folder.ParentFolder.Id.ToString() : String.Empty;
            if (folder.IsSelected && !folder.IsRootFolder)
            {
                _myStackRepo.RemoveFolder(folder.Id.ToString());
                if (!String.IsNullOrEmpty(parentFolderId))
                    return RedirectToAction("Details", "Folder", new {id = parentFolderId});
                else
                    return RedirectToAction("Index", "Folder");
            }
            else
            {
                var selectedCards = folder.Cards.Where(u => u.IsSelected).ToList();
                foreach (var selectedCard in selectedCards)
                {
                    _myStackRepo.RemoveCard(selectedCard.Id.ToString());
                }

                var selectedSubFolders = folder.SubFolders.Where(f => f.IsSelected).ToList();
                foreach (var selectedSubFolder in selectedSubFolders)
                {
                    _myStackRepo.RemoveFolder(selectedSubFolder.Id.ToString());
                }
                return RedirectToAction("Details", "Folder", new { id = folder.Id.ToString() });
            }
        }

        public ActionResult SetMoveTarget(string sourceFolderId)
        {
            var folder = _myStackRepo.GetFolder(sourceFolderId);
            var viewModel = new SetMoveTargetViewModel
            {
                SourceFolder = folder,
                SourceFolderId = folder.Id.ToString(),
                RootFolder = _myStackRepo.GetRootFolder(User.Identity.GetUserId()),
            };
            return View(viewModel);
        }


        public ActionResult Move(string sourceFolderId, string targetFolderId)
        {
            if (sourceFolderId == targetFolderId)
                return RedirectToAction("Details", "Folder", new { id = sourceFolderId });

            var sourceFolder = _myStackRepo.GetFolder(sourceFolderId); 

            // move sourcefolder
            if (sourceFolder.IsSelected)
            {
                _myStackRepo.MoveFolder(sourceFolderId, targetFolderId);
                _myStackRepo.DeSelectFolder(sourceFolder);

                return RedirectToAction("Details", "Folder", new { id = targetFolderId });
            }
            else
            {
                // move subfolders
                var selectedSubfolders = sourceFolder.SubFolders.Where(s => s.IsSelected).ToList();
                foreach (var subFolder in selectedSubfolders)
                {
                    _myStackRepo.MoveFolder(subFolder.Id.ToString(), targetFolderId);
                    _myStackRepo.DeSelectFolder(subFolder);
                }

                // move cards
                var cards = sourceFolder.Cards.Where(c => c.IsSelected).ToList();
                foreach (var card in cards)
                {
                    _myStackRepo.MoveCard(card.Id.ToString(), targetFolderId);
                    _myStackRepo.DeSelectCard(card);
                }
                return RedirectToAction("Details", "Folder", new { id = targetFolderId });
            }
            
        }


        public ActionResult SetInsertAfterCard(string sourceFolderId)
        {
            var folder = _myStackRepo.GetFolder(sourceFolderId);
            var viewModel = new SetNewOrderViewModel
            {
                SourceFolder = folder,
                SourceFolderId = folder.Id.ToString(),
                Cards = folder.Cards.Where(u => !u.IsSelected).OrderBy(c => c.Position).ToList(),
            };
            return View(viewModel);
        }

        public ActionResult ChangeOrder(string sourceFolderId, string insertAfterCardId)
        {
            _myStackRepo.ChangeCardOrder(sourceFolderId, insertAfterCardId);

            return RedirectToAction("Details", "Folder", new { id = sourceFolderId });
        }


        public ActionResult Search(string folderId)
        {
            var folder = _myStackRepo.GetFolder(folderId);

            var viewModel = new SearchViewModel
            {
                FolderId = folder.Id.ToString(),
                SearchFrontSide = true,
                SearchBackSide = true,
            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(SearchViewModel viewModel)
        {
            var folder = _myStackRepo.GetFolder(viewModel.FolderId);
            var searchResult = new List<SearchResultViewModel>();

            if (!String.IsNullOrEmpty(viewModel.SearchText) && (viewModel.SearchFrontSide || viewModel.SearchBackSide))
            { 
                SearchInFolder(folder, viewModel.SearchText.ToLower(), viewModel.SearchFrontSide, viewModel.SearchBackSide,
                       ref searchResult, true);
            }

            var result = new SearchResultsViewModel
            {
                Id = viewModel.FolderId,
                SearchText = viewModel.SearchText,
                SearchResults = searchResult,
            };
            return View("SearchResults", result);
        }

        public void SearchInFolder(Folder folderToSearch, string searchText, bool searchFrontSide, bool searchBackSide, ref List<SearchResultViewModel> searchResults, bool onlySelected = false)
        {
            var path = new List<Folder>();
            _myStackRepo.GetFolderPath(folderToSearch, ref path);
            path.Reverse();

            foreach (var card in folderToSearch.Cards.Where(u => (onlySelected && u.IsSelected) || !onlySelected))
            {
                var frontSide = card.FrontSide;
                var frontSideLowered = frontSide == null ? String.Empty : card.FrontSide.ToLower();
                var backSide = card.BackSide;
                var backSideLowered = backSide == null ? String.Empty : card.BackSide.ToLower();

                if ((searchFrontSide && frontSideLowered.Contains(searchText)) || (searchBackSide && backSideLowered.Contains(searchText)))
                {

                    const string prefix = "<span class=\"marked-search-text\">";
                    const string suffix = "</span>";

                    if (searchFrontSide && frontSideLowered.Contains(searchText))
                    {
                        var matches = Regex.Matches(frontSideLowered, searchText).Cast<Match>();
                        var offset = 0;
                        foreach (var match in matches)
                        {
                            frontSide = frontSide.Insert(match.Index + offset, prefix);
                            offset += prefix.Length;
                            frontSide = frontSide.Insert(match.Index + match.Length + offset, suffix);
                            offset += suffix.Length;
                        }
                    }


                    if (searchBackSide && backSideLowered.Contains(searchText))
                    {
                        var matches = Regex.Matches(backSideLowered, searchText).Cast<Match>();
                        var offset = 0;
                        foreach (var match in matches)
                        {
                            backSide = backSide.Insert(match.Index + offset, prefix);
                            offset += prefix.Length;
                            backSide = backSide.Insert(match.Index + match.Length + offset, suffix);
                            offset += suffix.Length;
                        }
                    }


                    var searchResult = new SearchResultViewModel
                    {
                        Id = card.Id.ToString(),
                        Path = path,
                        FrontSide = frontSide,
                        BackSide = backSide,
                    };
                    searchResults.Add(searchResult);
                }
            }

            foreach (var subFolder in folderToSearch.SubFolders.Where(f => (onlySelected && f.IsSelected) || !onlySelected))
            {
                SearchInFolder(subFolder, searchText, searchFrontSide, searchBackSide, ref searchResults);
            }
        }



        public ActionResult Options(string id)
        {
            var folder = _myStackRepo.GetFolder(id);

            var hasOnlyCardsSelected = !folder.IsSelected && !folder.SubFolders.Any(f => f.IsSelected);
            var hasOneCardSelected = (folder.Cards.Count(c => c.IsSelected) == 1);

            var hasOnlyFoldersSelected = !folder.Cards.Any(c => c.IsSelected);
            var hasOneFolderSelected = ((folder.SubFolders.Count(c => c.IsSelected) + (folder.IsSelected ? 1 : 0)) == 1);

            var numberOfSelectedCardsIncludingSubfolders = 0;
            _myStackRepo.GetCardCount(folder, ref numberOfSelectedCardsIncludingSubfolders, true, true, false);

            var viewModel = new OptionsViewModel
            {
                Id = folder.Id.ToString(),
                Folder = folder,
                HasAnyFolderSelected = (folder.IsSelected || folder.SubFolders.Any(f => f.IsSelected)),
                HasRootFolderSelected = (folder.IsRootFolder && folder.IsSelected),
                HasOnlyCardsSelected = hasOnlyCardsSelected,
                HasOnlyOneCardSelected = hasOnlyCardsSelected && hasOneCardSelected,
                CardId = (folder.Cards.Count(c => c.IsSelected) == 1) ? folder.Cards.Find(c => c.IsSelected).Id.ToString() : String.Empty,
                HasOnlyFoldersSelected = hasOnlyFoldersSelected,
                HasOnlyOneFolderSelected = hasOneFolderSelected && hasOnlyFoldersSelected,
                NumberOfSelectedCardsIncludingSubfolders = numberOfSelectedCardsIncludingSubfolders,
                FolderId = ((folder.SubFolders.Count(c => c.IsSelected) + (folder.IsSelected ? 1 : 0)) == 1) ? (folder.IsSelected ? folder.Id.ToString() : folder.SubFolders.Find(f => f.IsSelected).Id.ToString()) : String.Empty,
            };

            return View(viewModel);
        }

        public ActionResult Statistic(string id)
        {
            var folder = _myStackRepo.GetFolder(id);

            var cards = new List<Card>();
            _myStackRepo.GetAllCardsInFolder(folder, ref cards, true);

            //var result = cards.Sum(c => c.TestItems.Count) == 0;
            //var correctPercentage = cards.Sum(c => c.TestItems.Count) == 0
            //    ? 0d
            //    : (cards.Sum(c => c.TestItems.Count(t => t.Result == TestResult.Correct))/
            //       cards.Sum(c => c.TestItems.Count))*100;
            //var result2 = (cards.Sum(c => c.TestItems.Count(t => t.Result == TestResult.Correct))/
            //               cards.Sum(c => c.TestItems.Count))*100;
            //var result3 = cards.Sum(c => c.TestItems.Count(t => t.Result == TestResult.Correct));
            //var result4 = cards.Sum(c => c.TestItems.Count);
            //var result5 = ((double)result3 / (double)result4) * 100;

            var viewModel = new StatisticViewModel
            {
                FolderId = id,

                SelectedCount = cards.Count,

                AverageAnwserTime = cards.Average(c => c.TestItems.Select(t => t.Duration.Seconds).DefaultIfEmpty().Average()),
                AverageLevel = cards.Average(c => c.Level),
                //AverageAge = cards.Average(c => c.DateCreated.Ticks),  // causes exception - maybe tick numbers are too big for Average()
                AverageRepititions = cards.Average(c => c.TestItems.Count),

                TestItemsCount = cards.Sum(c => c.TestItems.Count),

                CorrectPercentage = cards.Sum(c => c.TestItems.Count) == 0 ? 0d :
                    ((double)cards.Sum(c => c.TestItems.Count(t => t.Result == TestResult.Correct)) / (double)cards.Sum(c => c.TestItems.Count)) * 100,
                CorrectCount = cards.Sum(c => c.TestItems.Count(t => t.Result == TestResult.Correct)),

                PartlyCorrectPercentage = cards.Sum(c => c.TestItems.Count) == 0 ? 0d :
                    ((double)cards.Sum(c => c.TestItems.Count(t => t.Result == TestResult.PartlyCorrect)) / (double)cards.Sum(c => c.TestItems.Count)) * 100,
                PartlyCorrectCount = cards.Sum(c => c.TestItems.Count(t => t.Result == TestResult.PartlyCorrect)),

                WrongPercentage = cards.Sum(c => c.TestItems.Count) == 0 ? 0d :
                    ((double)cards.Sum(c => c.TestItems.Count(t => t.Result == TestResult.Wrong)) / (double)cards.Sum(c => c.TestItems.Count)) * 100,
                WrongCount = cards.Sum(c => c.TestItems.Count(t => t.Result == TestResult.Wrong)),

                DuePercentage = cards.Count == 0 ? 0d : ((double)cards.Count(c => c.IsDue) / (double)cards.Count) * 100,
                DueCount = cards.Count(c => c.IsDue),

                LatestCard = cards.OrderByDescending(c => c.DateCreated).First(),
                OldestCard = cards.OrderBy(c => c.DateCreated).First(),

                TopCards = cards.OrderByDescending(c => c.Level).ThenBy(c => c.TestItems.Count(t => t.Result == TestResult.Correct)).Take(10),
                FlopCards = cards.OrderBy(c => c.Level).ThenBy(c => c.TestItems.Count(t => t.Result == TestResult.Wrong)).Take(10),

            };
            return View(viewModel);
        }


        public ActionResult Import(string folderId)
        {
            var viewModel = new ImportViewModel
            {
                FolderId = folderId,
            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Import(ImportViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var xDoc = new XDocument();
                try
                {
                    xDoc = XDocument.Parse(viewModel.SerializationResult);
                }
                catch
                {
                    ModelState.AddModelError(String.Empty, Strings.XmlParsingError);
                    return View(viewModel);
                }
                var rootElement = xDoc.Root;
                
                Deserialize(rootElement, viewModel.FolderId);

                return RedirectToAction("Details", "Folder", new { id = viewModel.FolderId });
            }
            return View(viewModel);
        }


        private void Deserialize(XElement folder, string parentFolderId)
        {
            var folderId = parentFolderId;
            if (!string.IsNullOrEmpty(folder.Attribute("Name").Value))
                folderId = _myStackRepo.AddFolder(folder.Attribute("Name").Value, User.Identity.GetUserId(), parentFolderId);

            foreach (var card in folder.Elements("Card"))
            {
                var dateCreated = card.Attribute("DateCreated") == null ? DateTime.Now : DateTime.Parse(card.Attribute("DateCreated").Value);
                var dateEdited = card.Attribute("DateEdited") == null ? DateTime.Now : DateTime.Parse(card.Attribute("DateEdited").Value);

                var cardId = _myStackRepo.AddCard(folderId, User.Identity.GetUserId(), card.Attribute("FrontSide").Value,
                    card.Attribute("BackSide").Value, dateCreated, dateEdited, int.Parse(card.Attribute("Level").Value), bool.Parse(card.Attribute("IsDue").Value));

                if (card.Elements("TestItems").Any())
                {
                    foreach (var testItem in card.Elements("TestItems").Single().Elements("TestItem"))
                    {
                        _myStackRepo.AddTestItem(cardId, String.Empty, DateTime.Parse(testItem.Attribute("StartTime").Value),
                            DateTime.Parse(testItem.Attribute("EndTime").Value),
                            (TestResult)Enum.Parse(typeof(TestResult), testItem.Attribute("Result").Value));
                    }
                }
            }

            foreach (var subFolder in folder.Elements("Folder"))
            {
                Deserialize(subFolder, folderId);
            }
        }


        public ActionResult ImportCsv(string folderId)
        {
            var viewModel = new ImportCsvViewModel
            {
                FolderId = folderId,
                SideDelimiter = Delimiter.Tab,
                CardDelimiter = Delimiter.NewLine,
                TextInQuotationMarks = true
            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult ImportCsv(ImportCsvViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ParseCSV(viewModel.SerializationResult, GetDelimiterString(viewModel.SideDelimiter),
                        GetDelimiterString(viewModel.CardDelimiter), viewModel.FolderId, viewModel.TextInQuotationMarks);
                }
                catch (Exception)
                {
                    ModelState.AddModelError(String.Empty, Strings.CSVParsingError);
                    return View(viewModel);
                }
                
                return RedirectToAction("Details", "Folder", new { id = viewModel.FolderId });
            }
            return View(viewModel);
        }


        private void ParseCSV(string stringToParse, string sideDelimiter, string cardDelimiter, string folderId, bool textInQuotationMarks)
        {
            var newFolderId = _myStackRepo.AddFolder("ImportedCSV", User.Identity.GetUserId(), folderId);

            if (!textInQuotationMarks)
            {
                var cards = stringToParse.Split(new string[] { cardDelimiter }, StringSplitOptions.None);
                if (cards == null)
                    throw new Exception();
                foreach (var card in cards)
                {
                    var sides = card.Split(new string[] { sideDelimiter }, StringSplitOptions.None);
                    if (sides.Length == 2)
                        _myStackRepo.AddCard(newFolderId, User.Identity.GetUserId(), sides[0], sides[1], DateTime.Now);
                    else
                        throw new Exception();
                }
            }
            else
            {
                var sides = new string[] { "", "" };
                var currentSide = 0;
                var isTextInsideQuotation = false;

                for (int i = 0; i < stringToParse.Length; i++)
                {
                    var c = stringToParse[i];
                    var sideDelimiterTmp = string.Empty;
                    var cardDelimiterTmp = string.Empty;

                    if (i + sideDelimiter.Length < stringToParse.Length)
                    {
                        sideDelimiterTmp = stringToParse.Substring(i, sideDelimiter.Length);
                    }
                    if (i + cardDelimiter.Length < stringToParse.Length)
                    {
                        cardDelimiterTmp = stringToParse.Substring(i, cardDelimiter.Length);
                    }

                    if (i == 0 && c != '"')
                    {
                        throw new Exception();
                    }

                    if (!isTextInsideQuotation && string.Equals(sideDelimiter, sideDelimiterTmp))
                    {
                        currentSide = currentSide == 0 ? 1 : 0;
                        i += sideDelimiter.Length - 1;
                    }
                    else if (!isTextInsideQuotation && string.Equals(cardDelimiter, cardDelimiterTmp))
                    {
                        _myStackRepo.AddCard(newFolderId, User.Identity.GetUserId(), sides[0], sides[1], DateTime.Now);
                        Array.Clear(sides, 0, 2);
                        currentSide = 0;
                        i += cardDelimiter.Length - 1;
                    }
                    else if (c == '"')
                    {
                        if (!isTextInsideQuotation)
                        {
                            isTextInsideQuotation = true;
                        }
                        else if (i < stringToParse.Length - 1 && stringToParse[i + 1] != '"')
                        {
                            isTextInsideQuotation = false;
                        }
                        else if (i < stringToParse.Length - 1 && stringToParse[i + 1] == '"')
                        {
                            sides[currentSide] += stringToParse[++i];
                        }
                        else
                        {
                            isTextInsideQuotation = false;
                        }
                    }
                    else
                    {
                        sides[currentSide] += c;
                    }
                }

                // add last card, if its ending correctly
                if (!isTextInsideQuotation)
                {
                    _myStackRepo.AddCard(newFolderId, User.Identity.GetUserId(), sides[0], sides[1], DateTime.Now);
                }
            }
        }


        public ActionResult Export(string folderId)
        {
            var folder = _myStackRepo.GetFolder(folderId);
            
            var stringBuilder = new StringBuilder();
            var settings = new XmlWriterSettings
            {
                Encoding = Encoding.Unicode,
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };
            using (var writer = XmlWriter.Create(stringBuilder, settings))
            {
                var doc = new XmlDocument();
                SerializeFolder(folder, null, ref doc, true, true);
                doc.Save(writer);
            }

            var viewModel = new ExportViewModel
            {
                FolderId = folderId,
                SerializationResult = stringBuilder.ToString(),
            };

            return View(viewModel);
        }


        public ActionResult ExportCsvOptions(string folderId)
        {
            var viewModel = new ExportCsvOptionsViewModel
            {
                FolderId = folderId,
                SideDelimiter = Delimiter.Tab,
                CardDelimiter = Delimiter.NewLine,
                TextInQuotationMarks = true
            };

            return View(viewModel);
        }


        private string GetDelimiterString(Delimiter delimiter)
        {
            var result = String.Empty;
            switch (delimiter)
            {
                case Delimiter.Comma:
                    result = ",";
                    break;
                case Delimiter.Tab:
                    result = "\t";
                    break;
                case Delimiter.NewLine:
                    result = "\r\n";
                    break;
                case Delimiter.Semicolon:
                    result = ";";
                    break;
                default:
                    result = ",";
                    break;
            }
            return result;
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult ExportCsv(ExportCsvOptionsViewModel model)
        {
            var folder = _myStackRepo.GetFolder(model.FolderId);

            var stringBuilder = new StringBuilder();

            var cards = new List<Card>();
            _myStackRepo.GetAllCardsInFolder(folder, ref cards, true);

            var i = 0;
            string frontSide, backSide;

            foreach (var card in cards)
            {
                i++;

                frontSide = string.IsNullOrEmpty(card.FrontSide) ? string.Empty : card.FrontSide;
                backSide = string.IsNullOrEmpty(card.BackSide) ? string.Empty : card.BackSide;

                if (model.TextInQuotationMarks)
                {
                    frontSide = string.Format("\"{0}\"", frontSide.Replace("\"", "\"\""));
                    backSide = string.Format("\"{0}\"", backSide.Replace("\"", "\"\""));
                }

                stringBuilder.Append(frontSide);
                stringBuilder.Append(GetDelimiterString(model.SideDelimiter));
                stringBuilder.Append(backSide);
                if (i != cards.Count)
                    stringBuilder.Append(GetDelimiterString(model.CardDelimiter));
            }

            var viewModel = new ExportCsvViewModel
            {
                FolderId = model.FolderId,
                SerializationResult = stringBuilder.ToString(),
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DownloadCsvExport(string folderId, string serializationResult)
        {
            var fileName = "MyStack-" + DateTime.Now.ToString("dd.MM.yyyy-HH:mm:ss") + ".csv";
            return File(System.Text.Encoding.UTF8.GetBytes(serializationResult), "text/csv", fileName);
        }


        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DownloadExport(string folderId, string serializationResult)
        {
            //return Content(serializationResult, "application/octet-stream", Encoding.UTF8);  // Alternative solution to display and browse xml result in browser
            var fileName = "MyStack-" + DateTime.Now.ToString("dd.MM.yyyy-HH:mm:ss") + ".xml";
            return new XmlActionResult(serializationResult, fileName, EncodingType.UTF16);
        }


        private void SerializeFolder(Folder folder, XmlElement parentFolderElement, ref XmlDocument doc, bool onlySelected = false, bool includeTestItems = true)
        {
            if (parentFolderElement == null)
            {
                parentFolderElement = doc.CreateElement("Folder");
                if (folder.IsSelected)
                    parentFolderElement.SetAttribute("Name", folder.Name);
                else
                    parentFolderElement.SetAttribute("Name", "");
                doc.AppendChild(parentFolderElement);
            }

            foreach (var selectedSubFolder in folder.SubFolders.Where(f => (onlySelected && f.IsSelected || !onlySelected)).OrderBy(f => f.Name))
            {
                var subFolderElement = doc.CreateElement("Folder");
                subFolderElement.SetAttribute("Name", selectedSubFolder.Name);
                parentFolderElement.AppendChild(subFolderElement);
                SerializeFolder(selectedSubFolder, subFolderElement, ref doc, false, includeTestItems);
            }

            foreach (var selectedCard in folder.Cards.Where(c => (onlySelected && c.IsSelected || !onlySelected)).OrderBy(f => f.Position))
            {
                var cardNode = doc.CreateElement("Card");

                cardNode.SetAttribute("FrontSide", selectedCard.FrontSide);
                cardNode.SetAttribute("BackSide", selectedCard.BackSide);
                cardNode.SetAttribute("DateCreated", selectedCard.DateCreated.ToString());
                cardNode.SetAttribute("DateEdited", selectedCard.DateEdited.ToString());
                cardNode.SetAttribute("Level", selectedCard.Level.ToString());
                cardNode.SetAttribute("IsDue", selectedCard.IsDue.ToString());

                if (includeTestItems && selectedCard.TestItems.Any())
                {
                    var testItemsNode = doc.CreateElement("TestItems");
                    cardNode.AppendChild(testItemsNode);
                    foreach (var testItem in selectedCard.TestItems)
                    {
                        var testItemNode = doc.CreateElement("TestItem");
                        testItemNode.SetAttribute("Result", testItem.Result.ToString());
                        testItemNode.SetAttribute("StartTime", testItem.StartTime.ToString());
                        testItemNode.SetAttribute("EndTime", testItem.EndTime.ToString());
                        testItemNode.SetAttribute("Result", testItem.Result.ToString());
                        testItemsNode.AppendChild(testItemNode);
                    }
                }
                parentFolderElement.AppendChild(cardNode);
            }
        }




        public ActionResult Browse(string folderId, int position, bool doInit = false, bool selectAll = false, bool returnToDetailsView = false)
        {

            if (doInit)
            {
                var cards = new List<Card>();
                var folder = _myStackRepo.GetFolder(folderId);

                if(selectAll)
                    _myStackRepo.SelectFolder(folder);

                _myStackRepo.GetAllCardsInFolder(folder, ref cards, true);

                var newBrowseList = new BrowseList
                {
                    Cards = cards,
                    CardsCount = cards.Count,
                };

                Session["BrowseList"] = newBrowseList;
            }


            var browseList = Session["BrowseList"] as BrowseList;

            string frontSide = null;
            string backSide = null;

            if (browseList.CardsCount > 0)
            {
                var elementAtCurrentPosition = browseList.Cards.ElementAt(position);

                frontSide = elementAtCurrentPosition != null ? elementAtCurrentPosition.FrontSide : null;
                backSide = elementAtCurrentPosition != null ? elementAtCurrentPosition.BackSide : null;

            }

            var viewModel = new BrowseViewModel
            {
                FolderId = folderId,
                FrontSide = frontSide,
                BackSide = backSide != null ? backSide.AddLinkTags() : null,
                CardsCount = browseList.CardsCount,
                Position = position,
                ReturnToDetailsView = returnToDetailsView,
            };
            return View(viewModel);
        }


        public ActionResult ResetTestResults(string folderId)
        {
            var folder = _myStackRepo.GetFolder(folderId);

            var cardCount = 0;
            _myStackRepo.GetCardCount(folder, ref cardCount, true, true);

            var viewModel = new ResetTestResultsViewModel
            {
                FolderId = folderId,
                AffectedCardsCount = cardCount,
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetTestResults(ResetTestResultsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var cards = new List<Card>();
                var folder = _myStackRepo.GetFolder(viewModel.FolderId);
                _myStackRepo.GetAllCardsInFolder(folder, ref cards, true);

                foreach (var card in cards)
                {
                    _myStackRepo.RemoveTestItems(card.Id.ToString(), card);
                    _myStackRepo.UpdateCardLevel(card);
                    _myStackRepo.UpdateCardIsDue(card);
                }
                return RedirectToAction("Details", "Folder", new {id = viewModel.FolderId});
            }
            return View(viewModel);
        }


    }
}
