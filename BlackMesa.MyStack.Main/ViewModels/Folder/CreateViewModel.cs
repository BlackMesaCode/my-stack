using System.ComponentModel.DataAnnotations;

namespace BlackMesa.MyStack.Main.ViewModels.Folder
{
    public class CreateViewModel
    {
        public string Id { get; set; }

        public string ParentFolderId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }


    }
}