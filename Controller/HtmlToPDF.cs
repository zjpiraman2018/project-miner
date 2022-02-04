/*
 * Created by SharpDevelop.
 * User: CyBerONE
 * Date: 9/10/2015
 * Time: 7:19 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Xml;
using Sgml;

namespace ProjMiner.Controller
{
	/// <summary>
	/// Description of HtmlToPDF.
	/// </summary>
	public class HtmlToPDF
	{
		public HtmlToPDF()
		{
		}

        public string ConvertToXHtml(string strInputHtml)
        {
            string strOutputXhtml = String.Empty;
            SgmlReader reader = new SgmlReader();
            reader.DocType = "HTML";
            StringReader sr = new System.IO.StringReader(strInputHtml);
            reader.InputStream = sr;
            StringWriter sw = new StringWriter();
            XmlTextWriter w = new XmlTextWriter(sw);
            reader.Read();
            while (!reader.EOF)
            {
                w.WriteNode(reader, true);
            }
            w.Flush();
            w.Close();
            return sw.ToString();
        }


        public string Convert(string Html,string outputFile){
				
				Byte[] bytes;
				
			
				using (var ms = new MemoryStream()) {
				
				   
				    using (var doc = new Document()) {
				
				
				        using (var writer = PdfWriter.GetInstance(doc, ms)) {
				
				            //Open the document for writing
				            doc.Open();
				
				            //Our sample HTML and CSS
				            //var example_html = @"<p>This <em>is </em><span class=""headline"" style=""text-decoration: underline;"">some</span> <strong>sample <em> text</em></strong><span style=""color: red;"">!!!</span></p>";
				            var example_css = Settings1.Default.FresnoCss;
				

				            using (var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_css))) {
				                using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(ConvertToXHtml(Html)))) {
				
				                    //Parse the HTML
				                    iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml, msCss);
				                }
				            }
				
				
				            doc.Close();
				        }
				    }
				
				    bytes = ms.ToArray();
				}



                if (File.Exists(outputFile)) File.Delete(outputFile);
				System.IO.File.WriteAllBytes(outputFile, bytes);

			return "";
		}
		
	}
}
