namespace BlackMesa.MyStack.Main.ViewModels.Selection
{
    public class BrowseViewModel
    {
        public string FolderId { get; set; }
        public string FrontSide { get; set; }
        public string BackSide { get; set; }
        public int Position { get; set; }
        public int CardsCount { get; set; }
        public bool ReturnToDetailsView { get; set; }
    }
}