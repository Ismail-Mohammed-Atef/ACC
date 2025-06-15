using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

public class HasGlobalRole : Attribute, IAsyncAuthorizationFilter
{
    private readonly string _roleName;
    private readonly string _projectIdRouteKey;

    public HasGlobalRole(string roleName)
    {
        _roleName = roleName;
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

        var GlobalAccessLevels = await db.Set<ApplicationUserRole>()
            .Include(ur => ur.Role)
            .Where(ur => ur.UserId == userId && ur.Role.GloblaAccesLevel==true)
            .ToListAsync();

        var hasPermission = GlobalAccessLevels.Any(ur => ur.Role.Name == _roleName);

        if (!hasPermission)
        {
            context.Result = new ForbidResult();
        }
    }
}
