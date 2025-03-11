using DataLayer.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ACC.ViewModels.ProjectVMs
{
    public class AddProjectVM
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

        public SelectList ProjectTypes { get; set; }
        public SelectList Currencies { get; set; }
    }
}
