using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ACC.Controllers.Viewers
{
    public class ImageController : Controller
    {
        private readonly string _imageFolder = Path.Combine("wwwroot", "images");
        private readonly IWebHostEnvironment _env;

        public ImageController(IWebHostEnvironment env)
        {
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }

        [HttpGet]
        public IActionResult Upload()
        {
            var filePath = TempData["filePath"] as string;

            if (string.IsNullOrEmpty(filePath))
            {
                return BadRequest("No file path provided.");
            }

            if (!filePath.StartsWith(_env.WebRootPath, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Invalid file path.");
            }

            return View(model: filePath);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return BadRequest("Invalid or missing file path.");
            }

            // Validate image extension
            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            var extension = Path.GetExtension(filePath).ToLower();
            if (!validExtensions.Contains(extension))
            {
                return BadRequest("Only image files (jpg, jpeg, png, gif, bmp) are allowed.");
            }

            var fullSourcePath = filePath;
            if (!System.IO.File.Exists(fullSourcePath))
            {
                return NotFound("File not found.");
            }

            // Ensure the images folder exists
            Directory.CreateDirectory(_imageFolder);

            // Generate a unique filename
            var uniqueFileName = $"{Path.GetFileNameWithoutExtension(filePath)}_{Guid.NewGuid():N}{extension}";
            var destinationPath = Path.Combine(_imageFolder, uniqueFileName);

            // Copy the file
            using (var sourceStream = new FileStream(fullSourcePath, FileMode.Open, FileAccess.Read))
            using (var destinationStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write))
            {
                await sourceStream.CopyToAsync(destinationStream);
            }

            // Return the filename for the client to use in the image viewer
            return Json(new { filename = uniqueFileName });
        }
    }
}