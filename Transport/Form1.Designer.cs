namespace Transport
{
    partial class Form1
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
            this.txtFileName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.btnChiaXe = new DevComponents.DotNetBar.ButtonX();
            this.btnImportSO = new DevComponents.DotNetBar.ButtonX();
            this.btnImport1 = new DevComponents.DotNetBar.ButtonX();
            this.btnImport = new DevComponents.DotNetBar.ButtonX();
            this.lblCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtFileName
            // 
            // 
            // 
            // 
            this.txtFileName.Border.Class = "TextBoxBorder";
            this.txtFileName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtFileName.Location = new System.Drawing.Point(12, 12);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.PreventEnterBeep = true;
            this.txtFileName.Size = new System.Drawing.Size(320, 20);
            this.txtFileName.TabIndex = 2;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView1.GridColor = System.Drawing.SystemColors.AppWorkspace;
            this.dataGridView1.Location = new System.Drawing.Point(0, 98);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(484, 352);
            this.dataGridView1.TabIndex = 4;
            // 
            // btnChiaXe
            // 
            this.btnChiaXe.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnChiaXe.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnChiaXe.Location = new System.Drawing.Point(146, 47);
            this.btnChiaXe.Name = "btnChiaXe";
            this.btnChiaXe.Size = new System.Drawing.Size(79, 28);
            this.btnChiaXe.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnChiaXe.TabIndex = 6;
            this.btnChiaXe.Text = "ChiaXe";
            this.btnChiaXe.Click += new System.EventHandler(this.btnChiaXe_Click);
            // 
            // btnImportSO
            // 
            this.btnImportSO.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnImportSO.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnImportSO.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnImportSO.Image = global::Transport.Properties.Resources.Download_icon;
            this.btnImportSO.Location = new System.Drawing.Point(259, 47);
            this.btnImportSO.Name = "btnImportSO";
            this.btnImportSO.Size = new System.Drawing.Size(73, 28);
            this.btnImportSO.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnImportSO.TabIndex = 7;
            this.btnImportSO.Text = "ImportSO";
            this.btnImportSO.Click += new System.EventHandler(this.btnImportSO_Click);
            // 
            // btnImport1
            // 
            this.btnImport1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnImport1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnImport1.Image = global::Transport.Properties.Resources.Download_icon;
            this.btnImport1.Location = new System.Drawing.Point(12, 47);
            this.btnImport1.Name = "btnImport1";
            this.btnImport1.Size = new System.Drawing.Size(86, 28);
            this.btnImport1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnImport1.TabIndex = 5;
            this.btnImport1.Text = "Import ASN";
            this.btnImport1.Click += new System.EventHandler(this.btnImport1_Click);
            // 
            // btnImport
            // 
            this.btnImport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnImport.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnImport.Image = global::Transport.Properties.Resources.images;
            this.btnImport.Location = new System.Drawing.Point(338, 12);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(20, 20);
            this.btnImport.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnImport.TabIndex = 3;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // lblCount
            // 
            this.lblCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCount.Location = new System.Drawing.Point(391, 12);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(66, 23);
            this.lblCount.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 450);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.btnImportSO);
            this.Controls.Add(this.btnChiaXe);
            this.Controls.Add(this.btnImport1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.txtFileName);
            this.Name = "Form1";
            this.Text = "Import Transport";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevComponents.DotNetBar.Controls.TextBoxX txtFileName;
        private DevComponents.DotNetBar.ButtonX btnImport;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private DevComponents.DotNetBar.ButtonX btnImport1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private DevComponents.DotNetBar.ButtonX btnImportSO;
        private DevComponents.DotNetBar.ButtonX btnChiaXe;
        private System.Windows.Forms.Label lblCount;
    }
}

