namespace BlackMesa.MyStack.Main.ViewModels.Selection
{
    public class SetMoveTargetViewModel
    {
        public string SourceFolderId { get; set; }
        public Models.Folder SourceFolder { get; set; }
        public Models.Folder RootFolder { get; set; }
    }
}