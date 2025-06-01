using Microsoft.AspNetCore.Mvc;

namespace ACC.Controllers.Viewers
{
    public class ImageController : Controller
    {
        private readonly string _imageFolder = Path.Combine("wwwroot", "images");

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || !file.ContentType.StartsWith("image/"))
                return BadRequest("Only image files are allowed.");

            Directory.CreateDirectory(_imageFolder);

            var uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid():N}{Path.GetExtension(file.FileName)}";
            var path = Path.Combine(_imageFolder, uniqueFileName);

            using (var stream = new FileStream(path, FileMode.Create))
                await file.CopyToAsync(stream);

            return Json(new { filename = uniqueFileName });
        }
    }
}
