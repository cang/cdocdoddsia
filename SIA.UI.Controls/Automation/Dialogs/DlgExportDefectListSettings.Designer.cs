namespace SIA.UI.Controls.Automation.Dialogs
{
    partial class DlgExportDefectListSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgExportDefectListSettings));
            this.ucFileNameFormat = new SIA.UI.Controls.Automation.Dialogs.ucMaskStringEditor();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdKeepName = new System.Windows.Forms.RadioButton();
            this.rdCustomizeName = new System.Windows.Forms.RadioButton();
            this.txtCustomizeName = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucFileNameFormat
            // 
            this.ucFileNameFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ucFileNameFormat.BrowseFileDialogTitle = "Save file as...";
            this.ucFileNameFormat.FileNameFormat = "";
            this.ucFileNameFormat.FileTypes = null;
            this.ucFileNameFormat.Location = new System.Drawing.Point(5, 19);
            this.ucFileNameFormat.Name = "ucFileNameFormat";
            this.ucFileNameFormat.SaveFileDialog = true;
            this.ucFileNameFormat.SelectedFileType = null;
            this.ucFileNameFormat.SelectedFileTypeIndex = 0;
            this.ucFileNameFormat.Size = new System.Drawing.Size(684, 88);
            this.ucFileNameFormat.SupportOnlyTextFile = false;
            this.ucFileNameFormat.SupportTextCsvFile = false;
            this.ucFileNameFormat.TabIndex = 3;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(278, 252);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "&OK";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(365, 252);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "&Cancel";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Location = new System.Drawing.Point(-10, 242);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(750, 4);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtCustomizeName);
            this.groupBox1.Controls.Add(this.rdCustomizeName);
            this.groupBox1.Controls.Add(this.rdKeepName);
            this.groupBox1.Location = new System.Drawing.Point(12, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(695, 100);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Classify Anomaly";
            // 
            // rdKeepName
            // 
            this.rdKeepName.AutoSize = true;
            this.rdKeepName.Checked = true;
            this.rdKeepName.Location = new System.Drawing.Point(20, 20);
            this.rdKeepName.Name = "rdKeepName";
            this.rdKeepName.Size = new System.Drawing.Size(121, 17);
            this.rdKeepName.TabIndex = 0;
            this.rdKeepName.TabStop = true;
            this.rdKeepName.Text = "Keep anomaly name";
            this.rdKeepName.UseVisualStyleBackColor = true;
            this.rdKeepName.CheckedChanged += new System.EventHandler(this.classifyAnomalyNameChanged);
            // 
            // rdCustomizeName
            // 
            this.rdCustomizeName.AutoSize = true;
            this.rdCustomizeName.Location = new System.Drawing.Point(20, 43);
            this.rdCustomizeName.Name = "rdCustomizeName";
            this.rdCustomizeName.Size = new System.Drawing.Size(144, 17);
            this.rdCustomizeName.TabIndex = 1;
            this.rdCustomizeName.Text = "Customize anomaly name";
            this.rdCustomizeName.UseVisualStyleBackColor = true;
            this.rdCustomizeName.CheckedChanged += new System.EventHandler(this.classifyAnomalyNameChanged);
            // 
            // txtCustomizeName
            // 
            this.txtCustomizeName.Location = new System.Drawing.Point(39, 66);
            this.txtCustomizeName.Name = "txtCustomizeName";
            this.txtCustomizeName.Size = new System.Drawing.Size(247, 20);
            this.txtCustomizeName.TabIndex = 2;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.ucFileNameFormat);
            this.groupBox3.Location = new System.Drawing.Point(12, 117);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(695, 112);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Specify output file path";
            // 
            // DlgExportDefectListSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 280);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(725, 308);
            this.Name = "DlgExportDefectListSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export Anomaly List Settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ucMaskStringEditor ucFileNameFormat;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtCustomizeName;
        private System.Windows.Forms.RadioButton rdCustomizeName;
        private System.Windows.Forms.RadioButton rdKeepName;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}