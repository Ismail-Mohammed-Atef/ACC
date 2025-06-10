using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ACC.Controllers.Viewers
{
    public class DwgController : Controller
    {
        private readonly string _uploadsPath = Path.Combine("wwwroot", "uploads");
        private readonly string _convertedPath = Path.Combine("wwwroot", "converted");
        private readonly IWebHostEnvironment _env;

        public DwgController(IWebHostEnvironment env)
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
            try
            {
                if (string.IsNullOrEmpty(filePath) || Path.GetExtension(filePath).ToLower() != ".dwg")
                {
                    return BadRequest("Invalid or missing DWG file path.");
                }

                var fullSourcePath = filePath;
                if (!System.IO.File.Exists(fullSourcePath))
                {
                    return NotFound("File not found.");
                }

                // Ensure directories exist
                Directory.CreateDirectory(_uploadsPath);
                Directory.CreateDirectory(_convertedPath);

                // Clear old files to avoid conflicts
                foreach (var oldFile in Directory.GetFiles(_uploadsPath))
                {
                    System.IO.File.Delete(oldFile);
                }
                foreach (var oldFile in Directory.GetFiles(_convertedPath))
                {
                    System.IO.File.Delete(oldFile);
                }

                // Generate unique filenames
                var baseName = Path.GetFileNameWithoutExtension(filePath);
                var uniqueName = $"{baseName}_{Guid.NewGuid():N}";
                var dwgFile = Path.Combine(_uploadsPath, uniqueName + ".dwg");

                // Copy DWG to uploads folder
                using (var sourceStream = new FileStream(fullSourcePath, FileMode.Open, FileAccess.Read))
                using (var destStream = new FileStream(dwgFile, FileMode.Create, FileAccess.Write))
                {
                    await sourceStream.CopyToAsync(destStream);
                }

                // External tools paths
                var odaPath = @"C:\Tools\ODAFileConverter\ODAFileConverter.exe";
                var inkscapePath = @"C:\Program Files\Inkscape\bin\inkscape.exe";

                if (!System.IO.File.Exists(odaPath))
                {
                    return StatusCode(500, $"ODA converter not found at: {odaPath}");
                }
                if (!System.IO.File.Exists(inkscapePath))
                {
                    return StatusCode(500, $"Inkscape not found at: {inkscapePath}");
                }

                // Step 1: Convert DWG to DXF
                var dxfFile = Path.Combine(_convertedPath, uniqueName + ".dxf");
                var odaArgs = $"\"{Path.GetFullPath(_uploadsPath)}\" \"{Path.GetFullPath(_convertedPath)}\" ACAD2018 DXF 0 1";

                var odaProcess = Process.Start(new ProcessStartInfo
                {
                    FileName = odaPath,
                    Arguments = odaArgs,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                });
                odaProcess?.WaitForExit();

                if (!System.IO.File.Exists(dxfFile))
                {
                    return StatusCode(500, "DXF file not generated. ODA conversion failed.");
                }

                // Step 2: Convert DXF to PDF
                var pdfFile = Path.Combine(_convertedPath, uniqueName + ".pdf");
                var inkscapeArgs = $"\"{dxfFile}\" --export-filename=\"{pdfFile}\" --export-area-drawing --export-type=pdf";

                var inkProcess = Process.Start(new ProcessStartInfo
                {
                    FileName = inkscapePath,
                    Arguments = inkscapeArgs,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });
                inkProcess?.WaitForExit();

                if (!System.IO.File.Exists(pdfFile))
                {
                    return StatusCode(500, "PDF file not created. Inkscape conversion failed.");
                }

                // Return the PDF filename
                return Json(new { filename = Path.GetFileName(pdfFile) });
            }
            catch (Exception ex)
            {
                // Log error (optional)
                System.IO.File.AppendAllText("log.txt", $"[{DateTime.Now}] Error: {ex}\n");
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }
        }
    }
}