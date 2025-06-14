using DataLayer.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ACC.ViewModels.MemberVM.MemberVM
{
    public class InsertMemberVM
    {
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public Status? Status { get; set; } = DataLayer.Models.Enums.Status.NotInvited;
        public int? CompanyId { get; set; }
        public string? GlobalAccessLevelId { get; set; }
        public string? PositionId { get; set; }
        public string? ProjectAccessLevelId { get; set; }





        public string? currentCompany {  get; set; }
        public string? currentGlobalAccessLevelId {  get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }


    }
}
