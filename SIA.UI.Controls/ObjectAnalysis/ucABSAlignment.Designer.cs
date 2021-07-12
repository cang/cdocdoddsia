namespace SIA.UI.Controls.ObjectAnalysis
{
    partial class ucABSAlignment
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupAlignments = new System.Windows.Forms.GroupBox();
            this.rdAlignPoleTipImage = new System.Windows.Forms.RadioButton();
            this.rdAlignABSImage = new System.Windows.Forms.RadioButton();
            this.rdAuto = new System.Windows.Forms.RadioButton();
            this.chkStatus = new System.Windows.Forms.CheckBox();
            this.groupAlignments.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupAlignments
            // 
            this.groupAlignments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupAlignments.Controls.Add(this.rdAlignPoleTipImage);
            this.groupAlignments.Controls.Add(this.rdAlignABSImage);
            this.groupAlignments.Controls.Add(this.rdAuto);
            this.groupAlignments.Location = new System.Drawing.Point(12, 6);
            this.groupAlignments.Name = "groupAlignments";
            this.groupAlignments.Size = new System.Drawing.Size(444, 97);
            this.groupAlignments.TabIndex = 0;
            this.groupAlignments.TabStop = false;
            // 
            // rdAlignPoleTipImage
            // 
            this.rdAlignPoleTipImage.AutoSize = true;
            this.rdAlignPoleTipImage.Location = new System.Drawing.Point(24, 69);
            this.rdAlignPoleTipImage.Name = "rdAlignPoleTipImage";
            this.rdAlignPoleTipImage.Size = new System.Drawing.Size(121, 17);
            this.rdAlignPoleTipImage.TabIndex = 2;
            this.rdAlignPoleTipImage.TabStop = true;
            this.rdAlignPoleTipImage.Text = "Align Pole Tip image";
            this.rdAlignPoleTipImage.UseVisualStyleBackColor = true;
            this.rdAlignPoleTipImage.CheckedChanged += new System.EventHandler(this.MethodAlignmentChanged);
            // 
            // rdAlignABSImage
            // 
            this.rdAlignABSImage.AutoSize = true;
            this.rdAlignABSImage.Location = new System.Drawing.Point(24, 46);
            this.rdAlignABSImage.Name = "rdAlignABSImage";
            this.rdAlignABSImage.Size = new System.Drawing.Size(103, 17);
            this.rdAlignABSImage.TabIndex = 1;
            this.rdAlignABSImage.TabStop = true;
            this.rdAlignABSImage.Text = "Align ABS image";
            this.rdAlignABSImage.UseVisualStyleBackColor = true;
            this.rdAlignABSImage.CheckedChanged += new System.EventHandler(this.MethodAlignmentChanged);
            // 
            // rdAuto
            // 
            this.rdAuto.AutoSize = true;
            this.rdAuto.Checked = true;
            this.rdAuto.Location = new System.Drawing.Point(24, 24);
            this.rdAuto.Name = "rdAuto";
            this.rdAuto.Size = new System.Drawing.Size(351, 17);
            this.rdAuto.TabIndex = 0;
            this.rdAuto.TabStop = true;
            this.rdAuto.Text = "Auto determine based on prefix of image file name (\"ABS\", \"Pole\", ...)";
            this.rdAuto.UseVisualStyleBackColor = true;
            this.rdAuto.CheckedChanged += new System.EventHandler(this.MethodAlignmentChanged);
            // 
            // chkStatus
            // 
            this.chkStatus.AutoSize = true;
            this.chkStatus.Location = new System.Drawing.Point(24, 4);
            this.chkStatus.Name = "chkStatus";
            this.chkStatus.Size = new System.Drawing.Size(101, 17);
            this.chkStatus.TabIndex = 3;
            this.chkStatus.Text = "Apply Alignment";
            this.chkStatus.UseVisualStyleBackColor = true;
            this.chkStatus.CheckedChanged += new System.EventHandler(this.chkStatus_CheckedChanged);
            // 
            // ucABSAlignment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkStatus);
            this.Controls.Add(this.groupAlignments);
            this.Name = "ucABSAlignment";
            this.Size = new System.Drawing.Size(466, 112);
            this.groupAlignments.ResumeLayout(false);
            this.groupAlignments.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupAlignments;
        private System.Windows.Forms.RadioButton rdAuto;
        private System.Windows.Forms.RadioButton rdAlignPoleTipImage;
        private System.Windows.Forms.RadioButton rdAlignABSImage;
        private System.Windows.Forms.CheckBox chkStatus;
    }
}
