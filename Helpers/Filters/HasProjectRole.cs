using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

public class HasProjectRole : Attribute, IAsyncAuthorizationFilter
{
    private readonly string _roleName;
    private readonly string _projectIdRouteKey;

    public HasProjectRole(string roleName, string projectIdRouteKey = "ProjectId")
    {
        _roleName = roleName;
        _projectIdRouteKey = projectIdRouteKey;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            context.Result = new ForbidResult();
            return;
        }

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

        var db = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();

        var userRoles =  await db.Set<ApplicationUserRole>()

            .Include(ur => ur.Role)
            .Where(ur => ur.UserId == userId && ur.ProjectId == projectId)
            .ToListAsync();

        var hasPermission = userRoles.Any(ur => ur.Role.Name == _roleName);

        if (!hasPermission)
        {
            context.Result = new ForbidResult();
        }
    }
}
