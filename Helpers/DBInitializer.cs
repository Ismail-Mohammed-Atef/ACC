using DataLayer.Models.ClassHelper;

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class DBInitializer
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var roles = new[]
            {
        Roles.AccountAdmin,
        Roles.ProjectAdmin,
        Roles.BIMManager,
        Roles.DisciplineLead,
        Roles.DocumentController,
        Roles.QualityControl,
        Roles.GeneralMember
    };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

    }
}
