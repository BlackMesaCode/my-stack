using System.Collections.Generic;

namespace BlackMesa.MyStack.Main.ViewModels.Selection
{
    public class SearchResultViewModel
    {
        public string Id { get; set; }
        public List<Models.Folder> Path { get; set; }
        public string FrontSide { get; set; }
        public string BackSide { get; set; }

    }
}