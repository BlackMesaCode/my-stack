using System.Collections.Generic;

namespace BlackMesa.MyStack.Main.ViewModels.Folder
{
    public class DetailsViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public bool IsRootFolder { get; set; }
        public int Level { get; set; }
        public bool HasAnySelection { get; set; }
        public string ParentFolderId { get; set; }
        public List<Models.Folder> Path { get; set; }
        public IEnumerable<Models.Folder> SubFolders { get; set; }
        public IEnumerable<Models.Card> Cards { get; set; }
        public int DueCards { get; set; }
        public Dictionary<string, int> DueCardsPerSubfolder{ get; set; }
    }
}