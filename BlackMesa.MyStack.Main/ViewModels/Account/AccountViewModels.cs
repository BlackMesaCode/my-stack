using System.ComponentModel.DataAnnotations;
using BlackMesa.MyStack.Main.Resources;

namespace BlackMesa.MyStack.Main.ViewModels.Account
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(ResourceType = typeof(Strings), Name = "UserName")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Strings), Name = "CurrentPassword")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "PasswordLengthInvalid", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Strings), Name = "NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Strings), Name = "ConfirmPassword")]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName= "PasswordsDontMatch")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(ResourceType = typeof(Strings), Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Strings), Name = "Password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "StaySignedIn")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(ResourceType = typeof(Strings), Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "PasswordLengthInvalid", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Strings), Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Strings), Name = "ConfirmPassword")]
        [Compare("Password", ErrorMessageResourceType = typeof(Strings), ErrorMessageResourceName = "PasswordsDontMatch")]
        public string ConfirmPassword { get; set; }
    }
}
