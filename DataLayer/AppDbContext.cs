using DataLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Define Composite Primary Key for Many-to-Many
            builder.Entity<ProjectMembers>().HasKey(pm => new { pm.ProjectId, pm.MemberId });

            // Configure Relationships
            builder.Entity<ProjectMembers>()
                .HasOne(pm => pm.Project)
                .WithMany(p => p.Members)
                .HasForeignKey(pm => pm.ProjectId);

            builder.Entity<ProjectMembers>()
                .HasOne(pm => pm.Member)
                .WithMany(u => u.Projects)
                .HasForeignKey(pm => pm.MemberId);

        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMembers> ProjectMembers { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
