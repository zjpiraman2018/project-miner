using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ProjMiner.Controller;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using HtmlAgilityPack;
using ProjMiner.Model;
using System.Globalization;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ProjMiner
{
    public partial class Nebraska : Form
    {
        public static int rec;
        public static int noRec;
        public Nebraska()
        {
            InitializeComponent();


            // Load user accounts
            string[] accounts = NebraskaController.GetUserAccounts();
            foreach (var account in accounts) cboUserAccount.Items.Add(account);

        }


        public void ShowStatus(string msg)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    ShowStatus(msg);
                });
                return;
            }
            // do something with foo and bar
            this.Text = msg;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            noRec = 0;
            if (textBox1.Text.Trim()=="")
            {
                MessageBox.Show("Output Path is required!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Focus();
                return;
            }

            if (!Directory.Exists(textBox1.Text)) {
                MessageBox.Show("Output Path does not exist!", "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                textBox1.Focus();
                return;
            }


            if (cboCourtType.Text == "")
            {
                MessageBox.Show("Please select Court Type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cboCourtType.Focus();
                return;
            }


            if (cboCaseType.Text == "")
            {
                MessageBox.Show("Please select Case Type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cboCaseType.Focus();
                return;
            }



            if (cboUserAccount.Text == "")
            {
                MessageBox.Show("Please select User Account.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cboUserAccount.Focus();
                return;
            }

            int start = Convert.ToInt32(numericUpDown1.Value);
            int end = Convert.ToInt32(numericUpDown2.Value);

            Task.Factory.StartNew(() =>
            {
                rec = 0;
                this.Invoke((MethodInvoker)delegate
                {
                    this.Text = "Initializing...";
                    this.button1.Enabled = false;
                });
                int retryCount = 0;

                for (int i = start; i <= end; i++)
                {

                    NebraskaMiner miner = new NebraskaMiner();
                    string countyNum = "";
                    string courtType = "";
                    string caseType = "";
                    int caseYear = 0;
                    string user = "";
                    string output = "";
                    this.Invoke((MethodInvoker)delegate
                    {

                        if (cboCounty.Text == "Douglas")
                        {
                            countyNum = "01";
                        }
                        else
                        {
                            countyNum = "02";
                        }

                         courtType = NebraskaController.GetCourtType(cboCourtType.Text);// Court Type
                         caseType = NebraskaController.GetCaseType(cboCaseType.Text);// Case Type
                         caseYear = Convert.ToInt32(NumUpDownCaseYear.Value.ToString().Substring(2, 2));  // Case Year
                         user =  cboUserAccount.Text;// User Account
                         output =textBox1.Text;
                    });

                    retry:
                    var field = miner.Mine(countyNum, courtType,caseType,  caseYear ,user, i,  output );

                    if (field == null) {

                        // RETRY COUNT FOR NUMBER OF NULL DOCUMENT 
                        if(retryCount < 3) 
                        {
                            retryCount += 1;
                            goto retry;
                        }
                        

                        noRec += 1;
                        //if (noRec > 5) break; // CHANGE FROM 5 to 100
                        if (noRec > 100) break; 



                        ShowStatus(string.Format("Processing {0} of {1} / Saved: {2}", i, end, rec));
           
                        

                        continue;
                    }

                    retryCount = 0;

                    ExcelHelper xls = new ExcelHelper();

                    string xlsTemplate = Path.Combine(
                          Path.GetDirectoryName(Application.ExecutablePath),
                          "Files", "template.xlsx"
                        );

                    if (!File.Exists(Path.Combine(output, "base.xlsx")))
                    {
                        File.Copy(xlsTemplate, Path.Combine(output, "base.xlsx"));
                    }

                    if (isValidCOA(field.SuitCOA))
                    {

                        xls.InsertToExcel(field, Path.Combine(output, "base.xlsx"));

                        HtmlToPDF htp = new HtmlToPDF();
                        string html = miner.createHTML(field.NebraskaPDFField);
                        htp.Convert(html, Path.Combine(output, field.CaseNumber.Replace(" ", "") + ".pdf"));
                        rec += 1;
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


        private bool isValidCOA(string COA) {
            List<string> COAList = new List<string>();
            COAList.Add("Annulment");
            COAList.Add("Dissolution of Marriage");
            COAList.Add("Interstate Paternity Incoming");
            COAList.Add("Legal Separation");
            COAList.Add("Name Changes");
            COAList.Add("Order of Supp/Custody/Visit");
            COAList.Add("Order of Supp/Custody/Visit-Private Atty");
            COAList.Add("Paternity");
            COAList.Add("Paternity-Private Atty");
            COAList.Add("Protection Order-Harassment");
            COAList.Add("Reg of Foreign Supp Order");
            COAList.Add("Reg of Foreign Supp Order-Private Atty");
            COAList.Add("Uniform Child Custody Juris-Private Atty");
            COAList.Add("Visitation Rights-Grandparents");

            if (COAList.Contains(COA)) {
                return false;
            }
            return true;
        }








        private void Nebraska_Load(object sender, EventArgs e)
        {
            NumUpDownCaseYear.Value = DateTime.Now.Year;
            cboCounty.SelectedIndex = 0;
            cboCaseType.SelectedIndex = 0;
            cboCourtType.SelectedIndex = 0;
            cboUserAccount.SelectedIndex = 0;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath;
        }


     
    }
}
