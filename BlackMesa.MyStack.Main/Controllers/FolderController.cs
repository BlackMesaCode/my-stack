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

            var cardManagementFolderId = _myStackRepo.AddFolder("Karteikartenverwaltung", User.Identity.GetUserId(), tutorialFolderId);

            _myStackRepo.AddCard(cardManagementFolderId, User.Identity.GetUserId(), "Wie lege ich eine Karteikarte an ?",
                "Schritt 1: Verwende die Hinzufügen Schaltfläche (+ Symbol) am rechten Rand deines Bildschirms\rSchritt 2: Wähle im Hinzufügen-Menü den Punkt Karteikarte\rSchritt 3: Beschrifte deine Karteikarte mit Vorder- und Rückseite.\rSchritt 4: Speichere deine Arbeit ab.\r\rBeachte dabei, dass du dich in dem Ordner befindest indem du die Kartei anlegen möchtest!"
                , DateTime.Now,
                null);
            _myStackRepo.AddCard(cardManagementFolderId, User.Identity.GetUserId(), "Wie verändere ich meine angelegten Karteikarten ?",
                "Schritt 1: Drücke mit der Linken-Maustaste auf die Kartei, welche du editieren möchtest.\rSchritt 2: Speichere deine Änderungen ab."
                , DateTime.Now,
                null);
            _myStackRepo.AddCard(cardManagementFolderId, User.Identity.GetUserId(), "Wie verändere ich die Reihenfolge meiner Karteikarten ?",
                "Schritt 1: Markiere die gewünschte Kartei, indem du ein Häkchen am Ende der Zeile setzt. (Mehrfachauswahl möglich)\rSchritt 2: Öffne die Optionen für deine Auswahl.\rSchritt 3: Wähle in den Optionen den Menüpunkt „Reihenfolge verändern“. \rSchritt 4: Wähle die Karte aus, unter der du deine Auswahl einfügen möchtest."
                , DateTime.Now,
                null);
            _myStackRepo.AddCard(cardManagementFolderId, User.Identity.GetUserId(), "Wie werde ich Karteikarten wieder los ?",
                "Schritt 1: Markiere die gewünschte Kartei, indem du ein Häkchen am Ende der Zeile setzt. (Mehrfachauswahl möglich)\rSchritt 2: Öffne die Optionen für deine Auswahl. \rSchritt 3: Wähle in den Optionen den Menüpunkt „Löschen“.\rSchritt 4: Bestätige den Löschvorgang."
                , DateTime.Now,
                null);
            _myStackRepo.AddCard(cardManagementFolderId, User.Identity.GetUserId(), "Kann ich auch Bilder in Karteikarten einfügen ?",
                "Das direkte einfügen von Bildern ist nicht möglich, du hast aber die Möglichkeit auf Bilder zu verlinken. Dazu fügst du einfach deinen gewünschten Link auf die Rückseite deiner Karteikarte mit an."
                , DateTime.Now,
                null);
            _myStackRepo.AddCard(cardManagementFolderId, User.Identity.GetUserId(), "Wie lege ich Ordner an ?",
                "Schritt 1: Verwende die Hinzufügen (+) Schaltfläche am rechten Rand deines Bildschirms. \rSchritt 2: Wähle im Hinzufügen-Menü den Punkt Verzeichnis. \rSchritt 3: Gib deinen Ordner einen Namen. \rSchritt 4: Speichere diesen ab."
                , DateTime.Now,
                null);
            _myStackRepo.AddCard(cardManagementFolderId, User.Identity.GetUserId(), "Wie verändere ich meine angelegten Ordner ?",
                "Schritt 1: Markiere den gewünschten Ordner, indem du ein Häkchen am Ende der Zeile setzt. \rSchritt 2: Öffne die Optionen für deine Auswahl. \rSchritt 3: Wähle in den Optionen den Menüpunkt „Bearbeiten“. \rSchritt 4: Verändere den Namen deines Ordners. \rSchritt 5: Speichere deine Änderung ab."
                , DateTime.Now,
                null);
            _myStackRepo.AddCard(cardManagementFolderId, User.Identity.GetUserId(), "Wie werde ich Ordner wieder los ?",
                "Schritt 1: Markiere den gewünschten Ordner, indem du ein Häkchen am Ende der Zeile setzt. \rSchritt 2: Öffne die Optionen für deine Auswahl. \rSchritt 3: Wähle in den Optionen den Menüpunkt „Löschen“. \rSchritt 4: Bestätige den Löschvorgang."
                , DateTime.Now,
                null);
            _myStackRepo.AddCard(cardManagementFolderId, User.Identity.GetUserId(), "Wie finde ich Inhalte in meinen Karteikarten wieder (suchen) ?",
                "Schritt 1: Markiere den/die gewünschte(n) Ordner/Kartei, indem du ein Häkchen am Ende der Zeile setzt. (Mehrfachauswahl möglich) \rSchritt 2: Öffne die Optionen für deine Auswahl. \rSchritt 3: Wähle in den Optionen den Menüpunkt „Durchsuchen“. \rSchritt 4: Gib deinen Suchbegriff ein. \rSchritt 5: Wähle aus wo du suchen möchtest (Vorderseite/Rückseite/Beides). \rSchritt 6: Bestätige deine Suche. \r"
                , DateTime.Now,
                null);
            _myStackRepo.AddCard(cardManagementFolderId, User.Identity.GetUserId(), "Wie kann ich Karteikarten verschieben ?",
                "Schritt 1: Markiere die gewünschte Kartei, indem du ein Häkchen am Ende der Zeile setzt. (Mehrfachauswahl möglich) \rSchritt 2: Öffne die Optionen für deine Auswahl. \rSchritt 3: Wähle in den Optionen den Menüpunkt „Verschieben“. \rSchritt 4: Wähle den Ordner in den du die Kartei verschieben möchtest."
                , DateTime.Now,
                null);
            _myStackRepo.AddCard(cardManagementFolderId, User.Identity.GetUserId(), "Wie exportiere ich Karteikarten ?",
                "Schritt 1: Markiere die gewünschte Kartei, indem du ein Häkchen am Ende der Zeile setzt. (Mehrfachauswahl möglich) \rSchritt 2: Öffne die Optionen für deine Auswahl. \rSchritt 3: Wähle zwischen den Optionen: \r\r Schritt 3.1: Auswahl: „Als XML-Datei exportieren“:\r Schritt 3.1.1: Lade die XML auf deinen Computer. („Herunterladen“)\r Schritt 3.1.2: Wähle ein Verzeichnis für die XML-Datei.\r Schritt 3.1.3: Bestätige mit Speichern das exportieren.\r\rSchritt 3.2: Auswahl: „Als CVS-Datei exportieren“.\r Schritt 3.2.1: Wähle die Trennzeichen zwischen Vorder- und Rückseite und zwischen den Karteikarten.\r Schritt 3.2.2: Generiere die CVS-Datei.\r Schritt 3.2.3: Lade die Datei analog der XML-Datei auf deinen Computer.\r\rHinweis: \rUnterschied CVS- zu XML-Datei: \rBeachte, dass bei einem CVS-Export keine Ordnerstrukturen mit exportiert werden können. Bei einer Auswahl von mehreren Ordnern, exportierst du lediglich die darin enthaltenen Karteikarten. \r\rBeim XML-Export können ganze Ordner-Strukturen exportiert werden."
                , DateTime.Now,
                null);
            _myStackRepo.AddCard(cardManagementFolderId, User.Identity.GetUserId(), "Wie importiere ich Karteikarten ?",
                "Schritt 1: Gehe in den Ordner in den du die Datei importieren möchtest. \rSchritt 2: Verwende die Hinzufügen (+) Schaltfläche am rechten Rand deines Bildschirms. \rSchritt 3: Wähle zwischen den Optionen: \r\rSchritt 3.1: Auswahl: Aus XML-Datei importieren. \r Schritt 3.1.1: Kopiere den XML Code in das dafür vorgegebene Textfeld. \r Schritt 3.1.2: Importiere den XML Code mit der Schaltfläche „Importieren“. \r\rSchritt 3.2: Auswahl: Aus CVS-Datei importieren. \r Schritt 3.2.1: Kopiere den CVS Code in das dafür vorgegebene Textfeld. \r Schritt 3.2.2: Gib die Trennzeichen zwischen Vorder- und Rückseite und zwischen den Karteikarten an. \r Schritt 3.2.3:  Importiere den CVS Code mit der Schaltfläche „Importieren“. \r"
                , DateTime.Now,
                null);

            var testCardsFolderId = _myStackRepo.AddFolder("Kartenabfrage", User.Identity.GetUserId(), tutorialFolderId);

            _myStackRepo.AddCard(testCardsFolderId, User.Identity.GetUserId(), "Wie starte ich eine Abfrage ?",
                "Schritt 1: Markiere den gewünschten Ordner, indem du ein Häkchen am Ende der Zeile setzt. (Mehrfachauswahl möglich) \rSchritt 2: Starte eine Abfrage mit dem Raketen-Symbol. \rSchritt 3: Wähle deine gewünschten Optionen für eine Abfrage aus. \rSchritt 4: Beginne die Abfrage mit dem „Abfrage beginnen“ Button. \r"
                , DateTime.Now,
                null);
            _myStackRepo.AddCard(testCardsFolderId, User.Identity.GetUserId(), "Welche Auswahlmöglichkeiten habe ich, mich abfragen zu lassen ?",
                "Wenn du eine Abfrage starten möchtest, kannst du dir die Abfrage mit folgenden Auswahlmöglichkeiten einrichten: \r\rAllgemeine Angaben:\r\rHier hast du die Auswahl zwischen (du kannst auch beide auswählen):\rNur fällige Karten abfragen\rEs werden nur Karten abgefragt, welche nachdem Karteikartensystem nach Leitner, zu wiederholen sind. \rVorder- und Rückseite vertauschen\rHier hast du die Option dich invertiert abfragen zu lassen.\r\rBestimmen der Reihenfolge:\r\rHier hast du die Auswahl zwischen: \rSortiert:\rDie Karten werden in der von dir angelegten Reihenfolge abgefragt. \rZufällig:\rDie Karten werden in einer zufälligen Reihenfolge abgefragt. \r\rBestimmen der Abfrageart:\r\rHier hast du die Auswahl zwischen: \rNormal:\rEs werden alle Karteikarten solange abgefragt, bis alle Karten richtig beantwortet wurden.\rEinmaliger Durchlauf:\rEs werden alle Karteikarten nur ein einziges mal durchlaufen, egal ob diese Richtig oder Falsch beantwortet wurden. "
                , DateTime.Now,
                null);
            _myStackRepo.AddCard(testCardsFolderId, User.Identity.GetUserId(), "Kann ich auch lernen, ohne mich abfragen zu lassen ?",
                 "Du hast die Möglichkeit deine Karteikarten auch einfach nur „durchzublättern“. Gehe dabei wie folgt vor: \r\rSchritt 1: Markiere den gewünschten Ordner, indem du ein Häkchen am Ende der Zeile setzt. (Mehrfachauswahl möglich)\rSchritt 2: Wähle die Option am rechten Bildschirmrand „Durchblättern“ (Symbol einfügen)\rSchritt 3: Blättere mit den Symbolen (<) und (>) durch deine Karteikarten. \r"
                 , DateTime.Now,
                 null);
            _myStackRepo.AddCard(testCardsFolderId, User.Identity.GetUserId(), "Kann ich irgendwo meinen Lernfortschritt einsehen ?",
                "Schritt 1: Markiere den gewünschten Ordner, indem du ein Häkchen am Ende der Zeile setzt. (Mehrfachauswahl möglich)\rSchritt 2: Öffne die Optionen für deine Auswahl.\rSchritt 3: Wähle in den Optionen den Menüpunkt „Statistik“ und lasse dir so die Statistik zu deinem ausgewählten Ordner anzeigen. Diese gibt dir einen Rückschluss auf deine Lernerfolge.\r\rHinweis: Du kannst deine Abfrageergebnisse jederzeit zurücksetzen. Gehe dabei wie folgt vor:\r\rSchritt 1: Markiere den gewünschten Ordner, indem du ein Häkchen am Ende der Zeile setzt. (Mehrfachauswahl möglich)\rSchritt 2: Öffne die Optionen für deine Auswahl.\rSchritt 3: Wähle in den Optionen den Menüpunkt „Abfrageergebnisse zurücksetzen“ \rSchritt 4: Bestätige die Durchführung der Rücksetzung. \r"
                , DateTime.Now,
                null);
            _myStackRepo.AddCard(testCardsFolderId, User.Identity.GetUserId(), "Wann muss ich eine Karteikarte wiederholen ?",
                "Sobald dir die Glocke neben einer Kartei angezeigt wird, solltest du diese wiederholen. Wann genau eine Kartei fällig wird, ermitteln wir auf Grundlage des sogenannten Leitner Algorithmus. Wird dir das (Daumen hoch) Symbol angezeigt, dann hast du die Kartei bereits gelernt, und ein erneutes Wiederholen steht noch nicht an. Bei Ordnern steht neben dem Symbol zusätzlich noch eine Zahl, die angibt wie viele fällige Karteien sich im Ordner befinden. "
                , DateTime.Now,
                null);

            var accountManagementFolderId = _myStackRepo.AddFolder("Accountverwaltung", User.Identity.GetUserId(), tutorialFolderId);

            _myStackRepo.AddCard(accountManagementFolderId, User.Identity.GetUserId(), "Gibt es die Seite auch in anderen Sprachen ?",
                "Schritt 1: Gehe in die Accountverwaltung.\rSchritt 2: Wähle die Option „Sprache wechseln“.\rSchritt 3: Wähle deine gewünschte Sprache aus.\rSchritt 4: Speichere deine Auswahl ab."
                , DateTime.Now,
                null);
            _myStackRepo.AddCard(accountManagementFolderId, User.Identity.GetUserId(), "Wie ändere ich mein Passwort ?",
                "Schritt 1: Gehe in die Accountverwaltung.\rSchritt 2: Wähle die Option „Passwort ändern“.\rSchritt 3: Gib dein Derzeitiges Passwort ein.\rSchritt 4: Gib dein neues Passwort ein und bestätige dieses nochmal im darunter liegenden Feld.\rSchritt 5: Bestätige deine Änderung durch das klicken auf den Button „Passwort ändern“.\r"
                , DateTime.Now,
                null);
            _myStackRepo.AddCard(accountManagementFolderId, User.Identity.GetUserId(), "Wie lösche ich meinen Account ?",
                "Du möchtest dein Account bei MyStack löschen?\r Solltest du mit irgendwas unzufrieden sein, teile uns doch bitte deinen Grund für das löschen deines Accounts mit. \rE-Mail: admin@blackmesa.eu \rWir freuen uns immer über Feedback!\r\rSchritt 1: Gehe in die Accountverwaltung.\rSchritt 2: Wähle die Option „Account schließen“. \rSchritt 3: Bestätige die Löschung deines Accounts. \r"
                , DateTime.Now,
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


        public ActionResult Edit(string id, bool returnToDetailsView = false)
        {
            var folder = _myStackRepo.GetFolder(id);
            var viewModel = new EditViewModel
            {
                Id = folder.Id.ToString(),
                ParentFolderId = folder.ParentFolder.Id.ToString(),
                Name = folder.Name,
                ReturnToDetailsView = returnToDetailsView,
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

