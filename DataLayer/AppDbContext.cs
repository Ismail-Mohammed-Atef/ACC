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

            builder.Entity<ProjectActivities>()
                .HasOne(pm => pm.project)
                .WithMany(p => p.Activities)
                .HasForeignKey(pm => pm.projectId);

            builder.Entity<ProjectMembers>()
                .HasOne(pm => pm.Member)
                .WithMany(u => u.Projects)
                .HasForeignKey(pm => pm.MemberId);


            ///////////////// ProjectCompany////////////////////////////////////////////

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

            builder.Entity<Review>()
              .HasOne(wf => wf.WorkflowTemplate)
              .WithMany(u => u.Reviews)
              .HasForeignKey(pm => pm.WorkflowTemplateId);

            builder.Entity<ReviewDocument>()
                 .HasKey(rd => new { rd.ReviewId, rd.DocumentId });

            builder.Entity<ReviewDocument>()
                .HasOne(rd => rd.Review)
                .WithMany(r => r.ReviewDocuments)
                .HasForeignKey(rd => rd.ReviewId);

            builder.Entity<ReviewDocument>()
                .HasOne(rd => rd.Document)
                .WithMany(d => d.ReviewDocuments)
                .HasForeignKey(rd => rd.DocumentId);

            builder.Entity<ReviewFolder>()
               .HasKey(rd => new { rd.ReviewId, rd.FolderId });

            builder.Entity<ReviewFolder>()
                .HasOne(rd => rd.Review)
                .WithMany(r => r.ReviewFolders)
                .HasForeignKey(rd => rd.ReviewId);

            builder.Entity<ReviewFolder>()
                .HasOne(rd => rd.Folder)
                .WithMany(d => d.ReviewFolders)
                .HasForeignKey(rd => rd.FolderId);

            builder.Entity<WorkflowStepUser>()
               .HasKey(w => new { w.UserId, w.StepId  });

            builder.Entity<WorkflowStepUser>()
                .HasOne(rd => rd.Step)
                .WithMany(r => r.workflowStepUsers)
                .HasForeignKey(rd => rd.StepId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<WorkflowStepUser>()
                .HasOne(rd => rd.User)
                .WithMany(d => d.workflowStepUsers)
                .HasForeignKey(rd => rd.UserId)
                .OnDelete(DeleteBehavior.Restrict);



            builder.Entity<ReviewStepUser>()
              .HasKey(w => new { w.UserId, w.StepId , w.ReviewId });

            builder.Entity<ReviewStepUser>()
                .HasOne(rd => rd.Step)
                .WithMany(r => r.ReviewStepUsers)
                .HasForeignKey(rd => rd.StepId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<ReviewStepUser>()
                .HasOne(rd => rd.User)
                .WithMany(d => d.ReviewStepUsers)
                .HasForeignKey(rd => rd.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ReviewStepUser>()
                .HasOne(rd => rd.Review)
                .WithMany(d => d.ReviewStepUsers)
                .HasForeignKey(rd => rd.ReviewId)
                .OnDelete(DeleteBehavior.Restrict);



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
        public DbSet<Transmittal> Transmittals { get; set; }
        public DbSet<TransmittalDocument> TransmittalDocuments { get; set; }

        public DbSet<IfcFile> IfcFiles { get; set; }

        public DbSet<Review> Reviews { get; set; }
        public DbSet<WorkflowTemplate> WorkflowTemplates { get; set; }
        public DbSet<WorkflowStepTemplate> WorkflowStepTemplates { get; set; }
        public DbSet<ReviewFolder> ReviewsFolders { get; set; }
        public DbSet<ReviewDocument> ReviewDocuments { get; set; }
        public DbSet<ReviewStepUser> ReviewStepUsers { get; set; }
        public DbSet<WorkflowStepUser> WorkflowStepUsers { get; set; }





    }
}
