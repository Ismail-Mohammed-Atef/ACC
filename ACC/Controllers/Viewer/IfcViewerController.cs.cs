using System.Security.Claims;
using ACC.ViewModels;
using ACC.ViewModels.IFCViewerVM;
using BusinessLogic.Services;
using Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ACC.Controllers.Viewer
{
    public class IfcViewerController : Controller
    {
        private readonly IfcFileService _ifcFileService;

        public IfcViewerController(IfcFileService ifcFileService)
        {
            _ifcFileService = ifcFileService;
        }

        public async Task<IActionResult> Index(int? fileId, int projectId = 1)
        {
            var model = new IfcViewerModel
            {
                FileId = fileId ?? 0,
                ProjectId = projectId,
                AvailableFiles = await _ifcFileService.GetIfcFilesByProjectIdAsync(projectId)
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadIfcFile(IFormFile file, int projectId)
        {
            if (!Helpers.FileHelper.IsValidIfcFile(file))
                return BadRequest("Invalid IFC file.");
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var ifcFile = await _ifcFileService.UploadIfcFileAsync(file, projectId, userId);
            return Json(new { success = true, fileId = ifcFile.Id });
        }

        [HttpGet]
        public async Task<IActionResult> GetIfcFile(int id)
        {
            var ifcFile = await _ifcFileService.GetIfcFileByIdAsync(id);
            if (ifcFile == null)
                return NotFound();
            var fileStream = System.IO.File.OpenRead(ifcFile.FilePath);
            return File(fileStream, "application/octet-stream", ifcFile.FileName);
        }
    }

   
}
