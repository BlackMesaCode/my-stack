using System;
using System.Collections.Generic;
using System.Linq;
using BlackMesa.MyStack.Main.Models;

namespace BlackMesa.MyStack.Main.DataLayer
{
    public class MyStackRepository
    {

        private readonly MyStackDbContext _dbContext;

        public MyStackRepository(MyStackDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        // ================================ Folders ================================ //

        public string AddFolder(string name, string ownerId, string parentFolderId)
        {

            // todo check for valid parentFolderId

            Folder parentFolder = null;
            if (!String.IsNullOrEmpty(parentFolderId))
                parentFolder = _dbContext.MyStack_Folders.Find(new Guid(parentFolderId));

            var owner = _dbContext.Users.Find(ownerId);
            var newFolder = new Folder
            {
                Name = name,
                OwnerId = ownerId,
                Owner = owner,
                ParentFolder = parentFolder,
                Level = parentFolder != null ? parentFolder.Level + 1 : 1,
            };

            _dbContext.MyStack_Folders.Add(newFolder);
            _dbContext.SaveChanges();

            return newFolder.Id.ToString();
        }



        public void EditFolder(string folderId, string newFolderName)
        {
            var folder = _dbContext.MyStack_Folders.Find(new Guid(folderId));

            if (folder.IsRootFolder)
                throw new Exception("Root folders name must not be changed.");

            folder.Name = newFolderName;
            _dbContext.SaveChanges();

        }


        public IEnumerable<Folder> GetFolders(string userId)
        {
            return _dbContext.MyStack_Folders.Where(f => f.Owner.Id == userId && f.Level == 1).AsEnumerable();
        }


        public void GetAllSubfolders(string folderId, ref List<Folder> subfolders)
        {
            var folder = GetFolder(folderId);
            foreach (var subfolder in folder.SubFolders)
            {
                subfolders.Add(subfolder);
                GetAllSubfolders(subfolder.Id.ToString(), ref subfolders);
            }
        }


        public void GetFolderCount(Folder folder, ref int folderCount, bool includeSubfolders = true, bool countOnlySelected = false)
        {
            if (countOnlySelected)
            {
                folderCount += folder.SubFolders.Count(f => f.IsSelected);
            }
            else
            {
                folderCount += folder.SubFolders.Count;
            }
            

            if (includeSubfolders)
            {
                foreach (var subFolder in folder.SubFolders)
                {
                    GetFolderCount(subFolder, ref folderCount, includeSubfolders, countOnlySelected);
                }
            }
        }


        public Folder GetRootFolder(string userId)
        {
            return _dbContext.MyStack_Folders.SingleOrDefault(f => f.Owner.Id == userId && f.ParentFolder == null);;
        }


        public void CreateRootFolder(string name, string ownerId)
        {
            var owner = _dbContext.Users.Find(ownerId);
            var newFolder = new Folder
            {
                Name = name,
                OwnerId = ownerId,
                Owner = owner,
                ParentFolder = null,
                Level = 1,
            };
            //_dbContext.Users.Find(ownerId).

            _dbContext.MyStack_Folders.Add(newFolder);
            _dbContext.SaveChanges();
        }


        public Folder GetFolder(string folderId)
        {
            return _dbContext.MyStack_Folders.SingleOrDefault(f => f.Id == new Guid(folderId));
        }


        public void GetFolderPath(Folder folder, ref  List<Folder> path)
        {
            path.Add(folder);
            if (!folder.IsRootFolder)
            {
                GetFolderPath(folder.ParentFolder, ref path);
            }
        }


        public void RemoveFolder(string folderId)
        {
            var folderToDelete = _dbContext.MyStack_Folders.Find(new Guid(folderId));

            if (folderToDelete.IsRootFolder)
                throw new Exception("Root folder must not be deleted.");

            for (var i = folderToDelete.Cards.Count-1; i >= 0; i--)
            {
                RemoveCard(folderToDelete.Cards[i].Id.ToString());
            }


            for (var i = folderToDelete.SubFolders.Count - 1; i >= 0; i--)
            {
                RemoveFolder(folderToDelete.SubFolders[i].Id.ToString());
            }

            _dbContext.MyStack_Folders.Remove(folderToDelete);
            _dbContext.SaveChanges();
        }


        public void MoveFolder(string folderId, string newParentFolderId)
        {
            var folder = _dbContext.MyStack_Folders.Find(new Guid(folderId));

            if (folder.IsRootFolder)
                throw new Exception("Root folder must not be moved.");

            var oldParentFolder = folder.ParentFolder;
            var newParentFolder = _dbContext.MyStack_Folders.Find(new Guid(newParentFolderId));

            // Remove Folder from the subfolders List of the old parent folder if there is one existing
            oldParentFolder.SubFolders.Remove(folder);

            // Add Folder to the subfolders list of the new parent folder 
            newParentFolder.SubFolders.Add(folder);

            // Adjust its new parent folder
            folder.ParentFolder = newParentFolder;  // todo check if null is assigned if there is no new parentFolder

            // Adjust its level
            folder.Level = !folder.IsRootFolder ? folder.ParentFolder.Level + 1 : 1;  //todo add level calculation to property getter


            _dbContext.SaveChanges();
        }


        // ================================ Cards ================================ //


        public Card GetCard(string cardId)
        {
            return _dbContext.MyStack_Cards.SingleOrDefault(f => f.Id == new Guid(cardId));
        }


        public void GetAllCardsInFolder(Folder folder, ref List<Card> cards, bool countOnlySelected = false)
        {
            cards.AddRange(folder.Cards.Where(c => (countOnlySelected && c.IsSelected) || !countOnlySelected).OrderBy(c => c.Position));
            
            foreach (var subfolder in folder.SubFolders.Where(f => (countOnlySelected && f.IsSelected) || !countOnlySelected).OrderBy(f => f.Name))
            {
                GetAllCardsInFolder(subfolder, ref cards);
            }
        }


        public void GetAllCardsIdsInFolder(Folder folder, ref List<string> cardIds, bool countOnlySelected = false)
        {
            cardIds.AddRange(folder.Cards.Where(c => (countOnlySelected && c.IsSelected) || !countOnlySelected).OrderBy(c => c.Position).Select(c => c.Id.ToString()));

            foreach (var subfolder in folder.SubFolders.Where(f => (countOnlySelected && f.IsSelected) || !countOnlySelected).OrderBy(f => f.Name))
            {
                GetAllCardsIdsInFolder(subfolder, ref cardIds);
            }
        }


        public void GetCardCount(Folder folder, ref int cardCount, bool includeSubfolders = true, bool countOnlySelected = false, bool countOnlyDue = false)
        {
            cardCount += folder.Cards.Count(u => ((countOnlySelected && u.IsSelected) || !countOnlySelected) && ((countOnlyDue && u.IsDue) || !countOnlyDue));

            if (includeSubfolders)
            {
                foreach (var subFolder in folder.SubFolders.Where(f => (countOnlySelected && f.IsSelected) || !countOnlySelected))
                {
                    GetCardCount(subFolder, ref cardCount);
                }
            }
        }


        public string AddCard(string folderId, string ownerId, string frontSide, string backSide, DateTime? dateCreated = null, DateTime? dateEdited = null)
        {

            var owner = _dbContext.Users.Find(ownerId);
            var folder = _dbContext.MyStack_Folders.Find(new Guid(folderId));

            var card = new Card
            {
                FolderId = new Guid(folderId),
                Folder = folder,
                OwnerId = ownerId,
                Owner = owner,
                IsSelected = false,
                Position = (folder.Cards != null ? folder.Cards.Count : 0),
                FrontSide = frontSide,
                BackSide = backSide,
                DateCreated = dateCreated ?? DateTime.Now,
                DateEdited = dateEdited ?? DateTime.Now,
            };

            _dbContext.MyStack_Cards.Add(card);

            _dbContext.SaveChanges();

            return card.Id.ToString();
        }


        public void EditCard(string id, string newFrontSide, string newBackSide)
        {
            var currentCard = _dbContext.MyStack_Cards.Find(new Guid(id));

            currentCard.FrontSide = newFrontSide;
            currentCard.BackSide = newBackSide;

            _dbContext.SaveChanges();
        }

        public void MoveCard(string cardId, string newFolderId)
        {
            var card = _dbContext.MyStack_Cards.Find(new Guid(cardId));
            var oldFolder = card.Folder;
            var newFolder = _dbContext.MyStack_Folders.Find(new Guid(newFolderId));

            DecreasePositionOfSubsequentCards(oldFolder, card, 1);

            card.FolderId = new Guid(newFolderId);
            card.Folder = newFolder;
            card.Position = newFolder.Cards.Count;

            // Remove card from the list of learning cards from the old folder
            oldFolder.Cards.Remove(card);

            // Add card to the list of learning cards of the new folder
            newFolder.Cards.Add(card);

            _dbContext.SaveChanges();
        }


        private void DecreasePositionOfSubsequentCards(Folder folder, Card card, int offset)
        {
            var affectedCards = folder.Cards.Where(c => c.Position > card.Position);
            foreach (var affectedCard in affectedCards)
            {
                affectedCard.Position = affectedCard.Position - offset;
            }
        }


        public void ChangeCardOrder(string sourceFolderId, string insertAfterCardId)
        {
            var folder = GetFolder(sourceFolderId);
            var insertAfterCard = GetCard(insertAfterCardId);
            var selectedCards = folder.Cards.Where(u => u.IsSelected).OrderBy(c => c.Position).ToList();

            for (int i = 0; i < selectedCards.Count(); i++)
            {
                ChangeCardPosition(folder.Cards.Single(c => c.Position == (insertAfterCard.Position + i)), selectedCards.ElementAt(i));
            }

            _dbContext.SaveChanges();
        }

        private void ChangeCardPosition(Card insertAfterCard, Card card)
        {
            card.Position = -1;
            var cardsBefore = insertAfterCard.Folder.Cards.Where(c => c.Position <= insertAfterCard.Position && c.Position >= 0).OrderBy(c => c.Position).ToList();
            var cardsAfter = insertAfterCard.Folder.Cards.Where(c => c.Position > insertAfterCard.Position).OrderBy(c => c.Position).ToList();

            int b = 0;
            foreach (var cardBefore in cardsBefore)
            {
                insertAfterCard.Folder.Cards.Find(c => c.Id == cardBefore.Id).Position = b;
                b++;
            }

            int a = insertAfterCard.Position + 2;
            foreach (var cardAfter in cardsAfter)
            {
                insertAfterCard.Folder.Cards.Find(c => c.Id == cardAfter.Id).Position = a;
                a++;
            }
            card.Position = insertAfterCard.Position + 1;

            _dbContext.SaveChanges();
        }


        public void RemoveCard(string id)
        {
            var card = _dbContext.MyStack_Cards.Find(new Guid(id));
            DecreasePositionOfSubsequentCards(card.Folder, card, 1);
            _dbContext.MyStack_Cards.Remove(card);
            _dbContext.SaveChanges();
        }


        // ================================ TestItems ================================ //


        public List<TestItem> GetTestItems(string cardId)
        {
            return _dbContext.MyStack_Cards.Find(new Guid(cardId)).TestItems;
        }

        public TestItem GetTestItem(string testItemId)
        {
            return _dbContext.MyStack_TestItems.Find(new Guid(testItemId));
        }

        public void AddTestItem(string cardId, string testId, DateTime startTime, DateTime endTime, TestResult result)
        {
            var testItem = new TestItem
            {
                CardId = new Guid(cardId),

                TestId = testId,

                StartTime = startTime,
                EndTime = endTime,
                Result = result,
            };

            _dbContext.MyStack_TestItems.Add(testItem);

            _dbContext.SaveChanges();
        }

        public void EditTestItem(string testItemId, DateTime? startTime, DateTime? endTime, TestResult result)
        {
            var testItem = GetTestItem(testItemId);

            if (startTime.HasValue)
                testItem.StartTime = startTime.Value;

            if (endTime.HasValue)
                testItem.EndTime = endTime.Value;

            testItem.Result = result;

            _dbContext.SaveChanges();
        }

        public void RemoveTestItem(string testItemId, TestItem testItem = null)
        {
            _dbContext.MyStack_TestItems.Remove(testItem ?? GetTestItem(testItemId));
            _dbContext.SaveChanges();
        }


        public void RemoveTestItems(string cardId, Card card = null)
        {
            _dbContext.MyStack_TestItems.RemoveRange(card != null ? card.TestItems : GetCard(cardId).TestItems);
            _dbContext.SaveChanges();
        }


        // ================================ Selections ================================ //


        public bool AllChildsSelected(Folder folder)
        {
            if (folder.SubFolders.Any(subFolder => !subFolder.IsSelected))
                return false;
            if (folder.Cards.Any(card => !card.IsSelected))
                return false;

            return true;
        }

        public void SelectCard(Card card)
        {
            card.IsSelected = true;

            //if (!card.Folder.IsSelected && AllChildsSelected(card.Folder))
            //    card.Folder.IsSelected = true;

            _dbContext.SaveChanges();
        }


        public void DeSelectCard(Card card)
        {
            card.IsSelected = false;

            var cardsFolderIsSelected = _dbContext.MyStack_Folders.Where(f => f.Id == card.FolderId).Select(f => f.IsSelected).Single();
            if (cardsFolderIsSelected)
                card.Folder.IsSelected = false;

            _dbContext.SaveChanges();
        }


        public void SelectFolder(Folder folder)
        {
            folder.IsSelected = true;
            foreach (var card in folder.Cards)
            {
                card.IsSelected = true;
            }
            foreach (var subfolder in folder.SubFolders.Where(f => !f.IsSelected))
            {
                subfolder.IsSelected = true;
            }

            //if (folder.ParentFolder!= null && !folder.ParentFolder.IsSelected && AllChildsSelected(folder.ParentFolder))
            //    folder.ParentFolder.IsSelected = true;

            _dbContext.SaveChanges();
        }


        public void DeSelectFolder(Folder folder)
        {
            folder.IsSelected = false;

            if (!folder.IsRootFolder)
            {
                if (folder.ParentFolder.IsSelected)
                    folder.ParentFolder.IsSelected = false;
            }

            foreach (var card in folder.Cards)
            {
                card.IsSelected = false;
            }

            foreach (var subfolder in folder.SubFolders.Where(f => f.IsSelected))
            {
                subfolder.IsSelected = false;
            }

            _dbContext.SaveChanges();
        }



    }
}