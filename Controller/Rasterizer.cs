using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

// required Ghostscript.NET namespaces
using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;

namespace Ghostscript.NET.Samples
{
    public class RasterizerSample 
    {
        private GhostscriptVersionInfo _lastInstalledVersion = null;
        private GhostscriptRasterizer _rasterizer = null;

        public void Start(string inputPdfPath, string outputPath)
        {
            int desired_x_dpi = 96;
            int desired_y_dpi = 96;

        

            _lastInstalledVersion =
                GhostscriptVersionInfo.GetLastInstalledVersion(
                        GhostscriptLicense.GPL | GhostscriptLicense.AFPL,
                        GhostscriptLicense.GPL);

            _rasterizer = new GhostscriptRasterizer();

            _rasterizer.Open(inputPdfPath, _lastInstalledVersion, false);

            for (int pageNumber = 1; pageNumber <= _rasterizer.PageCount; pageNumber++)
            {
                string pageFilePath = Path.Combine(outputPath, "Page-" + pageNumber.ToString() + ".png");

                Image img = _rasterizer.GetPage(desired_x_dpi, desired_y_dpi, pageNumber);
                img.Save(pageFilePath, ImageFormat.Png);

                Console.WriteLine(pageFilePath);
            }
        }
    }
}