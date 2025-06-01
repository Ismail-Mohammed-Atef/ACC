using Microsoft.AspNetCore.Mvc;

namespace ACC.Controllers.Viewers
{
    public class PdfController : Controller
    {
        private readonly string _pdfFolder = Path.Combine("wwwroot", "pdfs");

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || Path.GetExtension(file.FileName).ToLower() != ".pdf")
                return BadRequest("Only PDF files are allowed.");

            Directory.CreateDirectory(_pdfFolder);

            var uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid():N}.pdf";
            var path = Path.Combine(_pdfFolder, uniqueFileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Json(new { filename = uniqueFileName });
        }

    }
}
