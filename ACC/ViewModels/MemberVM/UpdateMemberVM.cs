using DataLayer.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ACC.ViewModels.MemberVM
{
    public class UpdateMemberVM
    {
        public string? name { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        public string? status { get; set; }
        public int? companyId { get; set; }
        public int? roleId { get; set; }
        public bool adminAccess { get; set; }
        public bool standardAccess { get; set; }
        public bool excutive { get; set; }
    }
}
