using ACC.Services;
using BusinessLogic.Repository.RepositoryClasses;
using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace ACC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<AppDbContext>((options) =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services
                .AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 3;
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                })
                .AddEntityFrameworkStores<AppDbContext>();



            #region Dependency injection
            builder.Services.AddControllers().AddJsonOptions(options =>
               {
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
            builder.Services.AddScoped<ITransmittalRepository, TransmittalRepository>();


            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
            builder.Services.AddScoped<IWorkflowRepository, WorkflowRepository>();
            builder.Services.AddScoped<IWorkFlowStepRepository, WorkflowStepRepository>();
            builder.Services.AddScoped<FolderService>();
            builder.Services.AddScoped<ReviewFolderService>();
            builder.Services.AddScoped<ReviewDocumentService>();
            builder.Services.AddScoped<WorkflowStepsUsersService>();
            builder.Services.AddScoped<ReviewStepUsersService>();


            #endregion


            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
