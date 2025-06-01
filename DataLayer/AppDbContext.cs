using DataLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;

using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions options):base(options)
        {
            
        }

        protected AppDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Define Composite Primary Key for Many-to-Many
            builder.Entity<ProjectMembers>()
                .HasKey(pm => new { pm.ProjectId, pm.MemberId });

            // Configure Relationships
            builder.Entity<ProjectMembers>()
                .HasOne(pm => pm.Project)
                .WithMany(p => p.Members)
                .HasForeignKey(pm => pm.ProjectId);

            builder.Entity<ProjectActivities>()
                .HasOne(pm => pm.project)
                .WithMany(p => p.Activities)
                .HasForeignKey(pm => pm.projectId);

            builder.Entity<ProjectMembers>()
                .HasOne(pm => pm.Member)
                .WithMany(u => u.Projects)
                .HasForeignKey(pm => pm.MemberId);

            ///////////////// ProjectCompany ///////////////////////////

            builder.Entity<ProjectCompany>()
        .HasKey(pc => new { pc.ProjectId, pc.CompanyId }); // Composite Key

            builder.Entity<ProjectCompany>()
                .HasOne(pc => pc.Project)
                .WithMany(p => p.ProjectCompany)
                .HasForeignKey(pc => pc.ProjectId);

            builder.Entity<ProjectCompany>()
                .HasOne(pc => pc.Company)
                .WithMany(c => c.ProjectCompany)
                .HasForeignKey(pc => pc.CompanyId);

  /// issue
            builder.Entity<Issue>()
                .HasOne(i => i.Project)
                .WithMany(p => p.Issues)
                .HasForeignKey(i => i.ProjectId);

            builder.Entity<Issue>()
           .HasOne(i => i.Document)
           .WithMany()
           .HasForeignKey(i => i.DocumentId)
           .OnDelete(DeleteBehavior.SetNull);

               // .HasForeignKey(pc => pc.CompanyId);         
           
        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentVersion> DocumentVersions { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMembers> ProjectMembers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ProjectActivities> ProjectActivities { get; set; }
        public DbSet<ProjectCompany> ProjectCompany { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<IfcFile> IfcFiles { get; set; }


    }
}
