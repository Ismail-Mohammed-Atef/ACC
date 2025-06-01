using DataLayer.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class ApplicationUser : IdentityUser
    {
        

        [ForeignKey(nameof(Company))]
        public int? CompanyId { get; set; }
        public Company? Company { get; set; }

        [ForeignKey(nameof(Role))]
        public int? RoleId { get; set; }
        public Role? Role { get; set; }
        public Status? Status { get; set; }
        public IList<AccessLevel>? AccessLevel { get; set; }
        public IList<ProjectMembers>? Projects { get; set; }
        public DateTime? AddedOn { get; set; } = DateTime.Now;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

<<<<<<< HEAD
        public List<WorkflowStepTemplate>? WorkflowSteps { get; set; }

        public List<WorkflowStepUser> workflowStepUsers { get; set; }

        public List<ReviewStepUser> ReviewStepUsers { get; set; } = new List<ReviewStepUser> { };
=======
        public List<IssueReviwers>? IssueReviwers { get; set; }
>>>>>>> ibrahim-isuue


    }
}
