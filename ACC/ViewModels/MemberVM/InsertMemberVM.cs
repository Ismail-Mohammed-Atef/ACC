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
        public int? RoleId { get; set; }
        public bool adminAccess { get; set; }
        public bool standardAccess { get; set; }
        public bool excutive { get; set; }
        public DateTime? AddedOn { get; set; }

    }
}
