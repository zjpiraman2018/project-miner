using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ProjMiner
{
    public partial class frmFresnoProxy : Form
    {
           private DataSet dataSet;
        public frmFresnoProxy()
        {
            InitializeComponent();
             dataSet = new DataSet();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            Settings1.Default.ProxyList = dataSet.GetXml();


            Settings1.Default.ProxyAddress = txtIPAddress.Text;
            Settings1.Default.ProxyName=txtProxyName.Text;
            Settings1.Default.ProxyPort = txtProxyPort.Text;

            Settings1.Default.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmFresnoProxy_Load(object sender, EventArgs e)
        {
            StringReader sr = new StringReader(Settings1.Default.ProxyList);
            dataSet.ReadXml(sr);
            dataGridView1.DataSource = dataSet.Tables[0];

            txtIPAddress.Text = Settings1.Default.ProxyAddress.ToString();
            txtProxyName.Text = Settings1.Default.ProxyName.ToString();
            txtProxyPort.Text = Settings1.Default.ProxyPort.ToString();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (((DataGridView)sender).SelectedRows.Count < 1) return;
             DataGridViewRow Row = ((DataGridView)sender).SelectedRows[0];
             txtProxyName.Text = Row.Cells[0].Value.ToString();
             txtIPAddress.Text = Row.Cells[1].Value.ToString();
             txtProxyPort.Text = Row.Cells[2].Value.ToString();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
