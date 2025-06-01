using System.Text.Json.Serialization;
using BusinessLogic.Repository.RepositoryClasses;
using BusinessLogic.Repository.RepositoryInterfaces;
using BusinessLogic.Services;
using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace ACC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IProjetcRepository, ProjectRepository>();
            builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IProjectActivityRepository, ProjectActivityRepository>();
            builder.Services.AddSingleton<Helpers.FileHelper>();
            builder.Services.AddScoped<IfcFileRepository>();
            builder.Services.AddScoped<IfcFileService>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".js"] = "application/javascript";
            provider.Mappings[".mjs"] = "application/javascript";
            provider.Mappings[".wasm"] = "application/wasm";
            provider.Mappings[".ifc"] = "application/octet-stream";
            provider.Mappings[".json"] = "application/json";
            provider.Mappings[".bin"] = "application/octet-stream";
            provider.Mappings[".dwg"] = "application/octet-stream";


            // Serve normal static files from wwwroot/
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
                ContentTypeProvider = provider,
                ServeUnknownFileTypes = true,
                OnPrepareResponse = ctx =>
                {
                    var ext = Path.GetExtension(ctx.File.Name).ToLowerInvariant();

                    if (ext == ".js" || ext == ".mjs")
                    {
                        ctx.Context.Response.Headers["Content-Type"] = "application/javascript";
                    }
                    else if (ext == ".wasm")
                    {
                        ctx.Context.Response.Headers["Content-Type"] = "application/wasm";
                    }
                    else if (ext == ".ifc")
                    {
                        ctx.Context.Response.Headers["Content-Type"] = "application/octet-stream";
                    }

                    ctx.Context.Response.Headers["Access-Control-Allow-Origin"] = "*";
                    ctx.Context.Response.Headers["Cache-Control"] = "public, max-age=31536000";
                }
            });

            // Serve static files from /dist
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dist")),
                RequestPath = "/dist",
                ContentTypeProvider = provider,
                ServeUnknownFileTypes = true,
                OnPrepareResponse = ctx =>
                {
                    var ext = Path.GetExtension(ctx.File.Name).ToLowerInvariant();

                    if (ext == ".js" || ext == ".mjs")
                    {
                        ctx.Context.Response.Headers["Content-Type"] = "application/javascript";
                    }
                    else if (ext == ".wasm")
                    {
                        ctx.Context.Response.Headers["Content-Type"] = "application/wasm";
                    }
                    else if (ext == ".ifc")
                    {
                        ctx.Context.Response.Headers["Content-Type"] = "application/octet-stream";
                    }

                    ctx.Context.Response.Headers["Access-Control-Allow-Origin"] = "*";
                    ctx.Context.Response.Headers["Cache-Control"] = "public, max-age=31536000";
                }
            });

            app.UseRouting();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}