using DataLayer;
using DataLayer.Models;
using DataLayer.Models.ClassHelper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helpers
{
    public static class DBInitializer
    {
        public static async Task SeedAsync(RoleManager<ApplicationRole> roleManager, AppDbContext context , UserManager<ApplicationUser> userManager )
        {
            var projectPositionsList = new List<string>
            {
                ProjectPositions.Architect,
                ProjectPositions.BIMManager,
                ProjectPositions.ConstructionManager,
                ProjectPositions.DocumentManager,
                ProjectPositions.Engineer,
                ProjectPositions.Estimator,
                ProjectPositions.Executive,
                ProjectPositions.Foreman,
                ProjectPositions.IT,
                ProjectPositions.ProjectEngineer,
                ProjectPositions.ProjectManager,
                ProjectPositions.SafetyManager,
                ProjectPositions.Subcontractor,
                ProjectPositions.Superintendent
            };

            foreach (var roleName in projectPositionsList)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = roleName , ProjectPosition = true });
                }
            }

            var GloblalAccessLevelsList = new List<string>
            {
                GlobalAccessLevels.AccountAdmin,
                GlobalAccessLevels.Executive,
                GlobalAccessLevels.StandardAccess.Trim()
            };

            foreach (var roleName in GloblalAccessLevelsList)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = roleName, GloblaAccesLevel = true });
                }
            }


            var permissionsList = new List<string>
            {
                Permissions.ViewFiles,
                Permissions.UploadFiles,
                Permissions.EditFiles,
                Permissions.DeleteFiles,
                Permissions.MoveFiles,
                Permissions.CreateFolders,
                Permissions.RenameFolders,
                Permissions.DeleteFolders,
                Permissions.SetPermissions,

                Permissions.CreateReviews,
                Permissions.SubmitReviews,
                Permissions.ApproveRejectReviews,
                Permissions.ViewReviews,

                Permissions.ViewReports,
                Permissions.CreateReports,

                Permissions.CreateForms,
                Permissions.EditForms,
                Permissions.ViewForms,
                Permissions.DeleteForms,

                Permissions.ViewIssues,
                Permissions.CreateIssues,
                Permissions.EditIssues,
                Permissions.DeleteIssues,

                Permissions.ViewRFIs,
                Permissions.CreateRFIs,
                Permissions.RespondToRFIs,

                Permissions.ViewModels,
                Permissions.RunClashes,
                Permissions.AssignClashes,

                Permissions.ManageTemplates,
                Permissions.ManagePermissions
            };

            foreach (var roleName in permissionsList)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = roleName, Permission = true });
                }
            }

            var ProjectAccessLevelsList = new List<string>
            {
                ProjectAccessLevels.ProjectMember,
                ProjectAccessLevels.ProjectAdmin,
            };

           foreach (var roleName in ProjectAccessLevelsList)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = roleName, ProjectAccessLevel = true });
                }
            }

            var Admin = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@cde.com",
            };

            var result = await userManager.CreateAsync(Admin, "123");
            if (result.Succeeded)
            {


                var userRole = new ApplicationUserRole
                {
                    ProjectId = null,
                    RoleId = (await roleManager.FindByNameAsync(GlobalAccessLevels.AccountAdmin))?.Id,
                    UserId = Admin.Id
                };

                context.Set<ApplicationUserRole>().Add(userRole);

    
            }

            await context.SaveChangesAsync();
        }
    }
}
