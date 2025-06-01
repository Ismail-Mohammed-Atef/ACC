using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ACC.Controllers.Viewers
{
    public class DwgController : Controller
    {
        private readonly string uploadsPath = Path.Combine("wwwroot", "uploads");
        private readonly string convertedPath = Path.Combine("wwwroot", "converted");

        [HttpGet]
        public IActionResult Upload() => View();

        [HttpPost]
        public async Task<IActionResult> UploadDWG(IFormFile file)
        {
            try
            {
                if (file == null || Path.GetExtension(file.FileName).ToLower() != ".dwg")
                    return BadRequest("Only .dwg files are supported.");

                Directory.CreateDirectory(uploadsPath);
                Directory.CreateDirectory(convertedPath);

                // Clear old files to avoid naming conflicts
                foreach (var oldFile in Directory.GetFiles(uploadsPath)) System.IO.File.Delete(oldFile);
                foreach (var oldFile in Directory.GetFiles(convertedPath)) System.IO.File.Delete(oldFile);

                // Generate a unique filename
                var baseName = Path.GetFileNameWithoutExtension(file.FileName);
                var uniqueName = baseName + "_" + Guid.NewGuid().ToString("N");
                var dwgFile = Path.Combine(uploadsPath, uniqueName + ".dwg");

                // Save uploaded DWG file
                using (var stream = new FileStream(dwgFile, FileMode.Create))
                    await file.CopyToAsync(stream);

                // External tools paths
                var odaPath = @"C:\Tools\ODAFileConverter\ODAFileConverter.exe";
                var inkscapePath = @"C:\Program Files\Inkscape\bin\inkscape.exe";

                if (!System.IO.File.Exists(odaPath))
                    return StatusCode(500, "ODA converter not found at: " + odaPath);

                if (!System.IO.File.Exists(inkscapePath))
                    return StatusCode(500, "Inkscape not found at: " + inkscapePath);

                // Step 1: Convert DWG to DXF (folder-based)
                var odaArgs = $"\"{Path.GetFullPath(uploadsPath)}\" \"{Path.GetFullPath(convertedPath)}\" ACAD2018 DXF 0 1";

                var odaProcess = Process.Start(new ProcessStartInfo
                {
                    FileName = odaPath,
                    Arguments = odaArgs,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                });
                odaProcess?.WaitForExit();

                var dxfFile = Path.Combine(convertedPath, uniqueName + ".dxf");
                if (!System.IO.File.Exists(dxfFile))
                    return StatusCode(500, "DXF file not generated. ODA conversion failed.");

                // Step 2: Convert DXF to PDF
                var pdfFile = Path.Combine(convertedPath, uniqueName + ".pdf");
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
                    return StatusCode(500, "PDF file not created. Inkscape may have failed.");

                return RedirectToAction("ViewFile", new { filename = Path.GetFileName(pdfFile) });
            }
            catch (Exception ex)
            {
                // Optional log
                System.IO.File.AppendAllText("log.txt", $"[{DateTime.Now}] Error: {ex}\n");

                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }
        }

        public IActionResult ViewFile(string filename)
        {
            var fileUrl = $"/converted/{filename}";
            return View("ViewFile", model: fileUrl);
        }
    }
}
