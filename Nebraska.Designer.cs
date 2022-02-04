namespace ProjMiner
{
    partial class Nebraska
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cboCourtType = new System.Windows.Forms.ComboBox();
            this.cboCaseType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.NumUpDownCaseYear = new System.Windows.Forms.NumericUpDown();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.cboUserAccount = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.cboCounty = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDownCaseYear)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(428, 160);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(142, 24);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Output Path:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(78, 13);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(461, 20);
            this.textBox1.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(532, 11);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(38, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(204, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Court Type:";
            // 
            // cboCourtType
            // 
            this.cboCourtType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCourtType.FormattingEnabled = true;
            this.cboCourtType.Items.AddRange(new object[] {
            "[ C ] - Country ",
            "[ D ] - District"});
            this.cboCourtType.Location = new System.Drawing.Point(263, 39);
            this.cboCourtType.Name = "cboCourtType";
            this.cboCourtType.Size = new System.Drawing.Size(101, 21);
            this.cboCourtType.TabIndex = 6;
            // 
            // cboCaseType
            // 
            this.cboCaseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCaseType.FormattingEnabled = true;
            this.cboCaseType.Items.AddRange(new object[] {
            "[ CI ] - Civil ",
            "[ SC ] - Small Claims"});
            this.cboCaseType.Location = new System.Drawing.Point(79, 68);
            this.cboCaseType.Name = "cboCaseType";
            this.cboCaseType.Size = new System.Drawing.Size(98, 21);
            this.cboCaseType.TabIndex = 8;
            this.cboCaseType.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Case Type:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(204, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Case Year:";
            // 
            // NumUpDownCaseYear
            // 
            this.NumUpDownCaseYear.Location = new System.Drawing.Point(263, 69);
            this.NumUpDownCaseYear.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.NumUpDownCaseYear.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.NumUpDownCaseYear.Name = "NumUpDownCaseYear";
            this.NumUpDownCaseYear.Size = new System.Drawing.Size(66, 20);
            this.NumUpDownCaseYear.TabIndex = 10;
            this.NumUpDownCaseYear.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numericUpDown2);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Location = new System.Drawing.Point(15, 95);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(282, 45);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(169, 16);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            1410065407,
            2,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(97, 20);
            this.numericUpDown2.TabIndex = 21;
            this.numericUpDown2.Value = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(134, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 20;
            this.label7.Text = "End:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Start:";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(41, 15);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(87, 20);
            this.numericUpDown1.TabIndex = 18;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cboUserAccount
            // 
            this.cboUserAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUserAccount.FormattingEnabled = true;
            this.cboUserAccount.Location = new System.Drawing.Point(400, 110);
            this.cboUserAccount.Name = "cboUserAccount";
            this.cboUserAccount.Size = new System.Drawing.Size(170, 21);
            this.cboUserAccount.TabIndex = 24;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(324, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "User Account:";
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(583, 190);
            this.shapeContainer1.TabIndex = 26;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.BorderColor = System.Drawing.SystemColors.ScrollBar;
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 8;
            this.lineShape1.X2 = 569;
            this.lineShape1.Y1 = 152;
            this.lineShape1.Y2 = 152;
            // 
            // cboCounty
            // 
            this.cboCounty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCounty.FormattingEnabled = true;
            this.cboCounty.Items.AddRange(new object[] {
            "Douglas",
            "Lan Caster"});
            this.cboCounty.Location = new System.Drawing.Point(78, 40);
            this.cboCounty.Name = "cboCounty";
            this.cboCounty.Size = new System.Drawing.Size(101, 21);
            this.cboCounty.TabIndex = 28;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 43);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 13);
            this.label8.TabIndex = 27;
            this.label8.Text = "County";
            // 
            // Nebraska
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 190);
            this.Controls.Add(this.cboCounty);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cboUserAccount);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.NumUpDownCaseYear);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboCaseType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboCourtType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.shapeContainer1);
            this.Name = "Nebraska";
            this.Load += new System.EventHandler(this.Nebraska_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NumUpDownCaseYear)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboCourtType;
        private System.Windows.Forms.ComboBox cboCaseType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown NumUpDownCaseYear;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.ComboBox cboUserAccount;
        private System.Windows.Forms.Label label5;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private System.Windows.Forms.ComboBox cboCounty;
        private System.Windows.Forms.Label label8;
    }
}