using BusinessLogic.Repository.RepositoryClasses;
using DataLayer.Models;
using Helpers;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Services
{
    public class IfcFileService
    {
        private readonly IfcFileRepository _ifcFileRepository;
        private readonly FileHelper _fileHelper;

        public IfcFileService(IfcFileRepository ifcFileRepository, FileHelper fileHelper)
        {
            _ifcFileRepository = ifcFileRepository;
            _fileHelper = fileHelper;
        }

        public async Task<List<IfcFile>> GetIfcFilesByProjectIdAsync(int projectId)
        {
            return await _ifcFileRepository.GetByProjectIdAsync(projectId);
        }

        public async Task<IfcFile> UploadIfcFileAsync(IFormFile file, int projectId, string userId)
        {
            var filePath = await _fileHelper.SaveIfcFileAsync(file);
            var ifcFile = new IfcFile
            {
                FileName = file.FileName,
                FilePath = filePath,
                ProjectId = projectId,
                UploadedBy = userId,
                UploadDate = DateTime.UtcNow
            };
            await _ifcFileRepository.AddAsync(ifcFile);
            return ifcFile;
        }

        public async Task<IfcFile> GetIfcFileByIdAsync(int id)
        {
            return await _ifcFileRepository.GetByIdAsync(id);
        }
    }
}