/*
 * Created by SharpDevelop.
 * User: CyBerONE
 * Date: 9/10/2015
 * Time: 7:33 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using ImageMagick;
using iTextSharp.text.pdf.parser;

namespace ProjMiner.Controller
{
	/// <summary>
	/// Description of PDFToImage.
	/// </summary>
	public class PDFToImage
	{
		public PDFToImage()
		{
		}
		
		public string Convert(string PDFFile,string OutputPath){
			MagickReadSettings settings = new MagickReadSettings();
            int ret = 0;
        retry:

            if (ret > 4) return "";

            try
            {

           
			
			// Settings the density to 300 dpi will create an image with a better quality
			settings.Density = new PointD(150,150);
			
			using (MagickImageCollection images = new MagickImageCollection())
			{
			  // Add all the pages of the pdf file to the collection
			  images.Read(PDFFile, settings);
			  
			  int page = 1;

			  
			  foreach (MagickImage image in images)
			  {
		
			    // Writing to a specific format works the same as for a single image
			    image.Format = MagickFormat.Tif;
			    image.Write(System.IO.Path.Combine(OutputPath + "_" + page.ToString()  + ".tif") );
			    page++;
			  }
			}
            }
            catch (Exception ex)
            {
                ret += 1;
                goto retry;
            }
			return "";
		}
	}
}
