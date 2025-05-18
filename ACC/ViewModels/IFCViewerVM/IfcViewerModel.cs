using DataLayer.Models;

namespace ACC.ViewModels.IFCViewerVM
{
    public class IfcViewerModel
    {
        public int FileId { get; set; }
        public int ProjectId { get; set; }
        public List<IfcFile> AvailableFiles { get; set; } = new List<IfcFile>();
        public string ViewerJsFile { get; set; }
    }
}
