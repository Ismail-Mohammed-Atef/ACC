using ACC.ViewModels.ReviewsVM;
using DataLayer;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace ACC.Services
{
    public class FolderService
    {
        private readonly AppDbContext _context;

        public FolderService(AppDbContext context)
        {
            _context = context;
        }

        public  List<FolderWithDocumentsVM> GetFolderTree()
        {
            var allFolders = _context.Folders
                .Include(f => f.Documents)
                .ToList();

            return BuildFolderTree(allFolders, null);
        }

        private  List<FolderWithDocumentsVM> BuildFolderTree(List<Folder> allFolders, int? parentId)
        {
            return allFolders
                .Where(f => f.ParentFolderId == parentId)
                .Select(f => new FolderWithDocumentsVM
                {
                    Id = f.Id,
                    Name = f.Name,
                    Documents = f.Documents?.Select(d => new DocumentVM
                    {
                        Id = d.Id,
                        Name = d.Name
                    }).ToList() ?? new List<DocumentVM>(),
                    Children = BuildFolderTree(allFolders, f.Id)
                })
                .ToList();
        }
    }
}
