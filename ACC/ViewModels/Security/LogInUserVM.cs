using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ACC.ViewModels.Security
{
    public class LogInUserVM
    {
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display (Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
