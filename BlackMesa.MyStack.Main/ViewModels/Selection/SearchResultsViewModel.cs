using System.Collections.Generic;

namespace BlackMesa.MyStack.Main.ViewModels.Selection
{
    public class SearchResultsViewModel
    {
        public string Id { get; set; }
        public string SearchText { get; set; }
        public List<SearchResultViewModel> SearchResults { get; set; }
    }
}