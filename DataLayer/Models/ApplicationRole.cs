using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ICollection<ApplicationUserRole> UserRoles { get; set; }

        public bool GloblaAccesLevel { get; set; } = false;
        public bool ProjectAccessLevel { get; set; } = false;
        public bool ProjectPosition {  get; set; } = false;

        public bool Permission {  get; set; } = false;

    }
}
