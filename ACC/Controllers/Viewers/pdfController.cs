﻿using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace ACC.Controllers.Viewers
{
    public class PdfController : Controller
    {
        private readonly string _pdfFolder = Path.Combine("wwwroot", "pdfs");
        private readonly IWebHostEnvironment _env;

        public PdfController(IWebHostEnvironment env)
        {
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }
        //for issue it's important 
        [HttpGet]
        public IActionResult ViewPdf(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || Path.GetExtension(filePath).ToLower() != ".pdf")
            {
                return BadRequest("Invalid or missing PDF file path.");
            }

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("PDF file not found.");
            }

            // تحويل المسار إلى رابط نسبي
            var relativePath = filePath.Replace(_env.WebRootPath, "").Replace("\\", "/").TrimStart('/');
            return View("ViewPdf", model: "/" + relativePath);
        }




        [HttpPost]
        public IActionResult Upload(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || Path.GetExtension(filePath).ToLower() != ".pdf")
            {
                return BadRequest(new { message = "Invalid or missing PDF file path." });
            }

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new { message = "File not found." });
            }

            // Convert filePath to a relative URL
      
            var relativePath = filePath.Replace(_env.WebRootPath, "").Replace("\\", "/").TrimStart('/');
            return Json(new { fileUrl = $"/{relativePath}" });
        }


    }
}