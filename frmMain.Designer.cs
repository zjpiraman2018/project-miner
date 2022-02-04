namespace ProjMiner
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nebraskaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fresnoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nebraskaUsersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fresnoToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.proxySetupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(621, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuToolStripMenuItem
            // 
            this.menuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nebraskaToolStripMenuItem,
            this.fresnoToolStripMenuItem});
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            this.menuToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.menuToolStripMenuItem.Text = "Menu";
            // 
            // nebraskaToolStripMenuItem
            // 
            this.nebraskaToolStripMenuItem.Name = "nebraskaToolStripMenuItem";
            this.nebraskaToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.nebraskaToolStripMenuItem.Text = "Nebraska";
            this.nebraskaToolStripMenuItem.Click += new System.EventHandler(this.nebraskaToolStripMenuItem_Click);
            // 
            // fresnoToolStripMenuItem
            // 
            this.fresnoToolStripMenuItem.Name = "fresnoToolStripMenuItem";
            this.fresnoToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.fresnoToolStripMenuItem.Text = "Fresno";
            this.fresnoToolStripMenuItem.Click += new System.EventHandler(this.fresnoToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nebraskaUsersToolStripMenuItem,
            this.fresnoToolStripMenuItem1});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // nebraskaUsersToolStripMenuItem
            // 
            this.nebraskaUsersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.usersToolStripMenuItem,
            this.defaultPathToolStripMenuItem});
            this.nebraskaUsersToolStripMenuItem.Name = "nebraskaUsersToolStripMenuItem";
            this.nebraskaUsersToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.nebraskaUsersToolStripMenuItem.Text = "Nebraska";
            this.nebraskaUsersToolStripMenuItem.Click += new System.EventHandler(this.nebraskaUsersToolStripMenuItem_Click);
            // 
            // usersToolStripMenuItem
            // 
            this.usersToolStripMenuItem.Name = "usersToolStripMenuItem";
            this.usersToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.usersToolStripMenuItem.Text = "Users";
            this.usersToolStripMenuItem.Click += new System.EventHandler(this.usersToolStripMenuItem_Click);
            // 
            // defaultPathToolStripMenuItem
            // 
            this.defaultPathToolStripMenuItem.Name = "defaultPathToolStripMenuItem";
            this.defaultPathToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.defaultPathToolStripMenuItem.Text = "Default Path";
            // 
            // fresnoToolStripMenuItem1
            // 
            this.fresnoToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.proxySetupToolStripMenuItem});
            this.fresnoToolStripMenuItem1.Name = "fresnoToolStripMenuItem1";
            this.fresnoToolStripMenuItem1.Size = new System.Drawing.Size(123, 22);
            this.fresnoToolStripMenuItem1.Text = "Fresno";
            // 
            // proxySetupToolStripMenuItem
            // 
            this.proxySetupToolStripMenuItem.Name = "proxySetupToolStripMenuItem";
            this.proxySetupToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.proxySetupToolStripMenuItem.Text = "Proxy Setup";
            this.proxySetupToolStripMenuItem.Click += new System.EventHandler(this.proxySetupToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 386);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nebraskaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nebraskaUsersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem usersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defaultPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fresnoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fresnoToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem proxySetupToolStripMenuItem;
    }
}