﻿using DataLayer.Models.Enums;

namespace DataLayer.Models
{
    public class Company : BaseEntity
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public string? Website { get; set; }
        public string? PhoneNumber { get; set; }
        public CompanyType? CompanyType { get; set; }
        public Country? Country { get; set; }
        public ICollection<ProjectCompany>? ProjectCompany { get; set; }



    }
}