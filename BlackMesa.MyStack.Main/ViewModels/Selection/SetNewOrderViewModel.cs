using System.Collections.Generic;

namespace BlackMesa.MyStack.Main.ViewModels.Selection
{
    public class SetNewOrderViewModel
    {
        public string SourceFolderId { get; set; }
        public Models.Folder SourceFolder { get; set; }
        public List<Models.Card> Cards { get; set; }
    }
}