using System.ComponentModel.DataAnnotations;
using BlackMesa.MyStack.Main.Resources;

namespace BlackMesa.MyStack.Main.ViewModels.Selection
{
    public class ExportCsvOptionsViewModel
    {
        public string FolderId { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "SideDelimiter")]
        public Delimiter SideDelimiter { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "CardDelimiter")]
        public Delimiter CardDelimiter { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "TextInQuotationMarks")]
        public bool TextInQuotationMarks { get; set; }
    }
}


public enum Delimiter
{
    [Display(Name = "Comma", ResourceType = typeof(Strings))]
    Comma,
    [Display(Name = "Tab", ResourceType = typeof(Strings))]
    Tab,
    [Display(Name = "Semicolon", ResourceType = typeof(Strings))]
    Semicolon,
    [Display(Name = "NewLine", ResourceType = typeof(Strings))]
    NewLine,
}

//public enum SideDelimiter
//{
//    [Display(Name = "Comma", ResourceType = typeof(Strings))]
//    Comma,
//    [Display(Name = "Tab", ResourceType = typeof(Strings))]
//    Tab,
//}

//public enum CardDelimiter
//{
//    [Display(Name = "Semicolon", ResourceType = typeof(Strings))]
//    Semicolon,
//    [Display(Name = "NewLine", ResourceType = typeof(Strings))]
//    NewLine,
//}