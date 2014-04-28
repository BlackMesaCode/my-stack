using System.ComponentModel.DataAnnotations;
using BlackMesa.MyStack.Main.Resources;

namespace BlackMesa.MyStack.Main.ViewModels.Selection
{
    public class ImportCsvViewModel
    {
        public string FolderId { get; set; }
        public string SerializationResult { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "SideDelimiter")]
        public Delimiter SideDelimiter { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "CardDelimiter")]
        public Delimiter CardDelimiter { get; set; }
    }
}