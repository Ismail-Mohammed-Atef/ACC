using System.ComponentModel.DataAnnotations;

namespace ACC.ViewModels.Security
{
    public class RegisterUserVM
    {
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name ="Confirm Password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

    }
}
