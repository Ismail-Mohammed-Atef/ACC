using DataLayer.Models.Enums;

namespace ACC.ViewModels.ProjectVMs
{
    public class DisplayProjectsVM
    {
        public int id { get; set; }
        public string? Name { get; set; }
        public string? ProjectNumber { get; set; }
        public ProjectType? ProjectType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CreationDate { get; set; }
        public double? ProjectValue { get; set; }
        public string? Address { get;  set; }
        public Currency? Currency { get;  set; }
    }
}
