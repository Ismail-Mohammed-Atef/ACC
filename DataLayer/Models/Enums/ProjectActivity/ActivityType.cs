using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models.Enums.ProjectActivity
{
    public enum ActivityType
    {
        [Display(Name = "Company Added")]
        CompanyAdded,

        [Display(Name = "Project Created")]
        ProjectCreated,

        [Display(Name = "Project Removed")]
        ProjectRemoved,

        [Display(Name = "Company Removed")]
        CompanyRemoved,
    }
}
