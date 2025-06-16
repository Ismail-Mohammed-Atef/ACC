using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

public class HasRoles : Attribute, IAsyncAuthorizationFilter
{
    private readonly string? _globalRole;
    private readonly List<string>? _projectRoles;
    private readonly string _projectIdRouteKey;

    public HasRoles(string? globalRole = null, string projectIdRouteKey = "ProjectId", params string[] projectRoles)
    {
        _globalRole = globalRole;
        _projectIdRouteKey = projectIdRouteKey;
        _projectRoles = projectRoles?.ToList();
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            context.Result = new ForbidResult();
            return;
        }

        var db = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();

        bool hasGlobalRole = false;
        if (!string.IsNullOrEmpty(_globalRole))
        {
            var globalRoles = await db.Set<ApplicationUserRole>()
                .Include(ur => ur.Role)
                .Where(ur => ur.UserId == userId && ur.Role.GloblaAccesLevel == true)
                .ToListAsync();

            hasGlobalRole = globalRoles.Any(ur => ur.Role.Name == _globalRole);
        }

        if (hasGlobalRole)
            return;

        if (!context.RouteData.Values.TryGetValue(_projectIdRouteKey, out var projectIdObj) ||
            !int.TryParse(projectIdObj?.ToString(), out int projectId))
        {
            projectId = int.TryParse(context.HttpContext.Request.Query[_projectIdRouteKey], out var pid) ? pid : -1;
        }

        if (projectId == -1)
        {
            context.Result = new BadRequestObjectResult("Missing projectId");
            return;
        }

        bool hasProjectRole = false;
        if (_projectRoles != null && _projectRoles.Any())
        {
            var projectRoles = await db.Set<ApplicationUserRole>()
                .Include(ur => ur.Role)
                .Where(ur => ur.UserId == userId && ur.ProjectId == projectId)
                .ToListAsync();

            hasProjectRole = projectRoles.Any(ur => _projectRoles.Contains(ur.Role.Name));
        }

        if (!hasProjectRole)
        {
            context.Result = new ForbidResult();
        }
    }
}
