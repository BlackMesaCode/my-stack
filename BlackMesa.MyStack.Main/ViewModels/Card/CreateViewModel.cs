using System.ComponentModel.DataAnnotations;

namespace BlackMesa.MyStack.Main.ViewModels.Card
{
    public class CreateViewModel
    {
        public string FolderId { get; set; }

        [StringLength(255)]
        public string FrontSide { get; set; }

        [StringLength(10000)]
        [DataType(DataType.MultilineText)]
        public string BackSide { get; set; }

    }
}