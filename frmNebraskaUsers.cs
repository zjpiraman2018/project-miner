using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace ProjMiner
{
    public partial class frmNebraskaUsers : Form
    {
        private DataSet dataSet;
        public frmNebraskaUsers()
        {
            InitializeComponent();
            dataSet = new DataSet();
        }

        private void frmNebraskaUsers_Load(object sender, EventArgs e)
        {
            StringReader sr = new StringReader(Settings1.Default.Users);
            dataSet.ReadXml(sr);
            dataGridView1.DataSource = dataSet.Tables[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Settings1.Default.Users = dataSet.GetXml();
            Settings1.Default.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
