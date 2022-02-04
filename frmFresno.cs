using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Win32;
using ProjMiner.Controller;
using ProjMiner.Model;
using System.Diagnostics;
using Ghostscript.NET.Samples;

namespace ProjMiner
{
    public partial class frmFresno : Form
    {

        private DataSet dataSet;
        public static int rec;
        //        RegistryKey RegKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Internet Explorer\Main", true);
        public frmFresno()
        {
            InitializeComponent();
            dataSet = new DataSet();

            textBox1.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //            RegKey.SetValue("Display Inline Images", "no");
        }

        private int GetLastID(string BaseFile) {
            if (!File.Exists(BaseFile)) return 0;

            string txt = File.ReadAllText(BaseFile);
            string id = txt.Split('\n').ToList().Where(a => a.Trim() != "").Last().Split(',').Last().Trim();
           
            return Convert.ToInt32(id.Replace("\"","").Replace("\r",""));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //http://www.us-proxy.org/

            if (dataGridView1.SelectedRows.Count < 1)
            {
                MessageBox.Show("No type selected!");
                return;
            }


            DataGridViewRow Row = dataGridView1.SelectedRows[0];
            string outputDirectory = textBox1.Text;
            string template = Settings1.Default.FresnoHtmlTemplate;
            string htmlContent = "";
            string caseType = Row.Cells[0].Value.ToString();
            string prefix = Row.Cells[1].Value.ToString();
            int start = Convert.ToInt32(Row.Cells[2].Value.ToString());
            int end = Convert.ToInt32(Row.Cells[3].Value.ToString());
            rec = GetLastID(Path.Combine(outputDirectory, "base_" + caseType + ".txt"));

            Task.Factory.StartNew(() =>
            {

                this.Invoke((MethodInvoker)delegate
                {
                    this.Text = "Initializing...";
                    this.button1.Enabled = false;
                });

                FresnoMiner fm = new FresnoMiner(this.checkBox1.Checked);

                //int continueCount = 0;


                for (int i = start; i <= end; i++)
                {

                    //if (continueCount > 4) break;

                    var xxxxx = ("00000" + i.ToString()).Substring(("00000" + i.ToString()).Length - 5, 5);
                    var caseno = prefix + ("00000" + i.ToString()).Substring(("00000" + i.ToString()).Length - 5, 5);

                    FresnoBaseField field = fm.GetDetails(caseno, rec);




                    //if (field == null) continueCount += 1;

                    using (StreamWriter stream = File.AppendText(Path.Combine(outputDirectory, "base_" + caseType + ".txt")))
                    {
                        if (field != null)
                        {
                            stream.WriteLine(ConcatFieldsBASE(field));
                            htmlContent = "";
                            htmlContent = ConvertToHTML(template, field);
                            //File.WriteAllText(Path.Combine(outputDirectory, field.FilingNo + ".html"), htmlContent);
                            new HtmlToPDF().Convert(htmlContent, Path.Combine(outputDirectory, field.FilingNo.ToUpper() + ".pdf"));
                            //continueCount = 0;
                            rec += 1;
                        }
                    }

                    using (StreamWriter stream = File.AppendText(Path.Combine(outputDirectory, "party_" + caseType + ".txt")))
                    {

                        if (field != null)
                        {
                            foreach (var party in field.Parties)
                            {
                                if (party.PartyName != "") stream.WriteLine(ConcatParty(party));
                            }
                        }
                    }

                    ShowStatus(string.Format("Processing {0} of {1} / Saved: {2}", i, end, rec));
                    



                }

            }).ContinueWith(delegate
            {



                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        ShowStatus(string.Format("Done! {0} document saved", rec));
                        this.button1.Enabled = true;
                    });

                }
                else
                {
                    ShowStatus(string.Format("Done! {0} document saved", rec));
                    this.button1.Enabled = true;
                }

            });

        }




        public void ShowStatus(string msg)
		{
		    if (this.InvokeRequired) {
		        this.Invoke((MethodInvoker)delegate {
		            ShowStatus(msg);
		        });
		        return;
		    }
		    // do something with foo and bar
		    this.Text = msg;

		}
        
        private string ConcatParty(FresnoPartyField item)
        {
        	
	        			
				 var txt =   "\"" +item.CourtDuns + "\"," + 
				  "\"" + item.FileNumber + "\"," + 
				  "\"" + item.FilingDate.Replace("/","") + "\"," + 
				 "\"" + item.FilingType + "\"," + 
				 "\"" + item.PartyType + "\"," + 
				 "\"" + item.PartyName + "\"," +

                 "\"\"," + 
                 "\"\"," + 
                 "\"\"," + 
                 "\"\"," + 
				 "\"\"," + 
				 "\"\"," + 
				 "\"\"," + 
				 	"\"\"," +
                 "\"" + item.RecordNo + "\"";
				 
				 
				 
        	return txt;
        
        }
        
        
        private string ConcatFieldsBASE(FresnoBaseField field)
        {
 			   var txt =   "\"" + field.CourtDuns + "\"," + 
				"\"" + field.FilingNo.ToUpper()    + "\"," +
                "\"" + field.FilingDate.Replace("/", "") + "\"," +    
				"\"" + field.FilingType   + "\"," +       
				"\"" + field.DescCode   + "\"," +        
				"\"" + field.LienType+ "\"," + 
				"\"" + field.GovernmentType+ "\"," + 
				"\"" + field.Status.ToUpper() + "\"," + 
				"\"" + field.StatusDate      + "\"," +  
				"\"" + field.Amount+ "\"," + 
				"\"" + field.NoOfDefendants+ "\"," + 
				"\"" + field.NoOfPlaintiffs     + "\"," +         
				"\"" + field.CauseOfAction1     + "\"," +      
				"\"" + field.CauseOfAction2+ "\"," + 
				"\"" + field.CauseOfAction3+ "\"," + 
				"\"" + field.CauseOfActionAmount1+ "\"," + 
				"\"" + field.CauseOfActionAmount2+ "\"," + 
				"\"" + field.CauseOfActionAmount3+ "\"," + 
				"\"" + field.CaseRemedy1+ "\"," + 
				"\"" + field.CaseRemedy2+ "\"," + 
				"\"" + field.CaseRemedy3+ "\"," + 
				"\"" + field.CaseRemedyAmount1+ "\"," + 
				"\"" + field.CaseRemedyAmount2+ "\"," + 
				"\"" + field.CaseRemedyAmount3+ "\"," + 
				"\"" + field.CauseOfActionDescription+ "\"," + 
				"\"" + field.CollectedDate+ "\"," + 
				"\"" + field.ReasonCode1+ "\"," + 
				"\"" + field.ReasonCode2+ "\"," + 
				"\"" + field.ReasonCode3+ "\"," + 
				"\"" + field.JudgmentType+ "\"," + 
				"\"" + field.RecordNo + "\"";
			return  txt;
        }


        private string ConvertToHTML(string Template, FresnoBaseField field) {
            string res = "";

            //field.FinancialInformation = "";
            //field.DispositionEventInfo = "";

            res = String.Format(Template, field.Title, field.FilingNo , field.Court , field.FilingDate, field.CauseOfActionDescription , field.Status, field.partyRawHTML,field.EventInformation,field.FinancialInformation,field.DispositionEventInfo,field.DocumentsInfo,field.CauseOfActionInfo);

            return res;
        }
        
        private void frmFresno_Load(object sender, EventArgs e)
        {
            StringReader sr = new StringReader(Settings1.Default.TypeList);
            dataSet.ReadXml(sr);


            dataGridView1.DataSource = dataSet.Tables[0] ;
            dataGridView1.Refresh();


            //            HtmlToPDF htp = new HtmlToPDF();
            //            htp.Convert();
            //            
            //            PDFToImage pti = new PDFToImage();
            //            pti.Convert(@"C:\Users\Administrator\Desktop\test2.pdf",@"C:\Users\Administrator\Desktop\");
        }

        private void frmFresno_FormClosed(object sender, FormClosedEventArgs e)
        {
//            RegKey.SetValue("Display Inline Images", "yes");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

 

        private void dataGridView1_SelectionChanged_1(object sender, EventArgs e)
        {
            if (((DataGridView)sender).SelectedRows.Count < 1) return;
            DataGridViewRow Row = ((DataGridView)sender).SelectedRows[0];

            Settings1.Default.TypeList = dataSet.GetXml();
            Settings1.Default.Save();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Settings1.Default.TypeList  = dataSet.GetXml();
            Settings1.Default.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();

            if (folderBrowserDialog1.SelectedPath!="")
            textBox1.Text=folderBrowserDialog1.SelectedPath;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PdfToTiff(this.textBox1.Text);
        }


        public void PdfToTiff(string PDFDirectory)
        {
            if(!Directory.Exists(Path.Combine(PDFDirectory, "Converted"))){
                Directory.CreateDirectory(Path.Combine(PDFDirectory, "Converted"));
            }
            PDFToImage pdftoImage = new PDFToImage();
            int cnt = 0;
            var files = Directory.GetFiles(PDFDirectory, "*.pdf");
            foreach (var file in files)
            {
                cnt += 1;


                this.Text = (string.Format("Converting to pdf [ {0} / {1} ]", cnt, files.GetUpperBound(0)));
                this.Refresh();
                Application.DoEvents();
                

                string tifFile = Path.Combine(Path.GetDirectoryName(file), "Converted", Path.GetFileNameWithoutExtension(file));
                RasterizerSample rs = new RasterizerSample();
                rs.Start(file, Path.Combine(Path.GetDirectoryName(file), "Converted"));
                //pdftoImage.Convert(file, tifFile);
                
                //string ghostScriptPath = @"C:\Program Files\gs\gs9.16\bin\gswin32.exe";
                //String ars = "-dNOPAUSE -q -sDEVICE=tifflzw -sOutputFile=" + file + " " + tifFile + " -c quit";
                //System.Diagnostics.Process proc = new System.Diagnostics.Process();
                //proc.StartInfo.FileName = ghostScriptPath;
                //proc.StartInfo.Arguments = ars;
                //proc.StartInfo.CreateNoWindow = true;
                //proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                //proc.Start();
                //proc.WaitForExit();

                this.Text = (string.Format("Converting to pdf [ {0} / {1} ]", cnt, files.GetUpperBound(0)));
                this.Refresh();
                Application.DoEvents();
            }
        }

    }
}
