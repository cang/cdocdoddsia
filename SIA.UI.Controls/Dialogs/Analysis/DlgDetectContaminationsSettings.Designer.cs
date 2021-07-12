namespace SIA.UI.Controls.Dialogs
{
    partial class DlgDetectContaminationsSettings
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtRegionFile = new System.Windows.Forms.TextBox();
            this.btnRegionFile = new System.Windows.Forms.Button();
            this.btnNNModelFile = new System.Windows.Forms.Button();
            this.txtNNModelFile = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Region file:";
            // 
            // txtRegionFile
            // 
            this.txtRegionFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRegionFile.Location = new System.Drawing.Point(91, 9);
            this.txtRegionFile.Name = "txtRegionFile";
            this.txtRegionFile.ReadOnly = true;
            this.txtRegionFile.Size = new System.Drawing.Size(432, 20);
            this.txtRegionFile.TabIndex = 1;
            this.txtRegionFile.Text = "\\\\sglserver\\SharedFull\\CongNguyen\\Xyratex\\ABS Inspection\\roi-1.msk";
            // 
            // btnRegionFile
            // 
            this.btnRegionFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRegionFile.Location = new System.Drawing.Point(529, 7);
            this.btnRegionFile.Name = "btnRegionFile";
            this.btnRegionFile.Size = new System.Drawing.Size(44, 23);
            this.btnRegionFile.TabIndex = 2;
            this.btnRegionFile.Text = "...";
            this.btnRegionFile.UseVisualStyleBackColor = true;
            this.btnRegionFile.Click += new System.EventHandler(this.btnRegionFile_Click);
            // 
            // btnNNModelFile
            // 
            this.btnNNModelFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNNModelFile.Location = new System.Drawing.Point(529, 38);
            this.btnNNModelFile.Name = "btnNNModelFile";
            this.btnNNModelFile.Size = new System.Drawing.Size(44, 23);
            this.btnNNModelFile.TabIndex = 5;
            this.btnNNModelFile.Text = "...";
            this.btnNNModelFile.UseVisualStyleBackColor = true;
            this.btnNNModelFile.Click += new System.EventHandler(this.btnNNModelFile_Click);
            // 
            // txtNNModelFile
            // 
            this.txtNNModelFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNNModelFile.Location = new System.Drawing.Point(91, 40);
            this.txtNNModelFile.Name = "txtNNModelFile";
            this.txtNNModelFile.ReadOnly = true;
            this.txtNNModelFile.Size = new System.Drawing.Size(432, 20);
            this.txtNNModelFile.TabIndex = 4;
            this.txtNNModelFile.Text = "\\\\sglserver\\SharedFull\\CongNguyen\\Xyratex\\ABS Inspection\\model.nns";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "NN Model file:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(-43, 70);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(651, 7);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(214, 89);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(295, 89);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // DlgDetectContaminationsSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 122);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnNNModelFile);
            this.Controls.Add(this.txtNNModelFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnRegionFile);
            this.Controls.Add(this.txtRegionFile);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DlgDetectContaminationsSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Detect Contaminations";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRegionFile;
        private System.Windows.Forms.Button btnRegionFile;
        private System.Windows.Forms.Button btnNNModelFile;
        private System.Windows.Forms.TextBox txtNNModelFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}