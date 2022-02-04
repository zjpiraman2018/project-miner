using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProjMiner
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void nebraskaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Nebraska nebraska = new Nebraska();
            nebraska.MdiParent = this;
            nebraska.Show();
        }

        private void nebraskaUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
       
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNebraskaUsers nebraska = new frmNebraskaUsers();
            nebraska.MdiParent = this;
            nebraska.Show();
        }

        private void proxySetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmFresnoProxy fresnoProxy = new frmFresnoProxy();
            fresnoProxy.MdiParent = this;
            fresnoProxy.Show();
        }

        private void fresnoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmFresno fresno = new frmFresno();
            fresno.MdiParent = this;
            fresno.Show();
        }
    }
}
