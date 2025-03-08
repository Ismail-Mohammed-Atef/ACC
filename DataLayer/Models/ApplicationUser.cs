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
        public int CompanyId { get; set; }
        public Company? Company { get; set; }
        public ICollection<ProjectMembers>? Projects { get; set; }
        public DateTime AddedOn { get; set; } = DateTime.Now;
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
