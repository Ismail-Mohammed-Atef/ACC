using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

namespace Helpers
{
    public static class DwgConverter
    {
        public static void ConvertToPng(string inputPath, string outputPath)
        {
            using (var image = Image.Load(inputPath))
            {
                var cadImage = (CadImage)image;

                foreach (var layout in cadImage.Layouts)
                {
                    System.Diagnostics.Debug.WriteLine($"Layout name: {layout.Key}");
                }

                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 4000,
                    PageHeight = 4000,
                    AutomaticLayoutsScaling = true,
                    NoScaling = false,
                    DrawType = CadDrawTypeMode.UseObjectColor,
                    BackgroundColor = Color.White,
                    Layouts = new[] { "Layout2" } 
                };

                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                image.Save(outputPath, pngOptions);
            }

        }
    }
}
