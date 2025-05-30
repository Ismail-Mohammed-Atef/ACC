namespace DataLayer.Models
{
    public class IfcFile
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public int ProjectId { get; set; }
        public string? UploadedBy { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
