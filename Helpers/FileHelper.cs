using Microsoft.AspNetCore.Http;

namespace Helpers
{
    public class FileHelper
    {
        public static bool IsValidIfcFile(IFormFile file)
        {
            return file != null && file.Length > 0 && Path.GetExtension(file.FileName).ToLower() == ".ifc";
        }

        public async Task<string> SaveIfcFileAsync(IFormFile file)
        {
            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ifc-files");
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            var filePath = Path.Combine(uploadsDir, $"{Guid.NewGuid()}_{file.FileName}");
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return filePath;
        }
    }
}
