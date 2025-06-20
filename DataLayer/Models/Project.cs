﻿using DataLayer.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Project : BaseEntity
    {
        public string? Name { get; set; }
        public string? ProjectNumber { get; set; }
        public ProjectType? ProjectType { get; set; }
        public string? Address { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CreationDate { get; set; }
        public double? ProjectValue { get; set; }
        public Currency? Currency { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public bool IsArchived { get; set; } = false;
        public ICollection<ProjectMembers>? Members { get; set; }
        public ICollection<ProjectActivities>? Activities { get; set; }
        public ICollection<ProjectCompany>? ProjectCompany { get; set; }
        public List<Issue> Issues { get; set; }
        public ICollection<ApplicationUserRole> UserRoles { get; set; }


    }
}
