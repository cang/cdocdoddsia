namespace SiGlaz.ObjectAnalysis.UI
{
    partial class ucObjectAnalyzerStep
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        protected System.ComponentModel.IContainer components = null;

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
            this.pnHeader = new System.Windows.Forms.Panel();
            this.btnSaveStepSettings = new System.Windows.Forms.Button();
            this.btnLoadStepSettings = new System.Windows.Forms.Button();
            this.chkStatus = new System.Windows.Forms.CheckBox();
            this.pnFooter = new System.Windows.Forms.Panel();
            this.pnBody = new System.Windows.Forms.Panel();
            this.pnHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnHeader
            // 
            this.pnHeader.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnHeader.Controls.Add(this.btnSaveStepSettings);
            this.pnHeader.Controls.Add(this.btnLoadStepSettings);
            this.pnHeader.Controls.Add(this.chkStatus);
            this.pnHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnHeader.Location = new System.Drawing.Point(0, 0);
            this.pnHeader.Name = "pnHeader";
            this.pnHeader.Size = new System.Drawing.Size(486, 32);
            this.pnHeader.TabIndex = 0;
            // 
            // btnSaveStepSettings
            // 
            this.btnSaveStepSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveStepSettings.Enabled = false;
            this.btnSaveStepSettings.Location = new System.Drawing.Point(405, 5);
            this.btnSaveStepSettings.Name = "btnSaveStepSettings";
            this.btnSaveStepSettings.Size = new System.Drawing.Size(75, 23);
            this.btnSaveStepSettings.TabIndex = 2;
            this.btnSaveStepSettings.Text = "Save";
            this.btnSaveStepSettings.UseVisualStyleBackColor = true;
            this.btnSaveStepSettings.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoadStepSettings
            // 
            this.btnLoadStepSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadStepSettings.Enabled = false;
            this.btnLoadStepSettings.Location = new System.Drawing.Point(324, 5);
            this.btnLoadStepSettings.Name = "btnLoadStepSettings";
            this.btnLoadStepSettings.Size = new System.Drawing.Size(75, 23);
            this.btnLoadStepSettings.TabIndex = 1;
            this.btnLoadStepSettings.Text = "Load";
            this.btnLoadStepSettings.UseVisualStyleBackColor = true;
            this.btnLoadStepSettings.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // chkStatus
            // 
            this.chkStatus.AutoSize = true;
            this.chkStatus.Location = new System.Drawing.Point(12, 9);
            this.chkStatus.Name = "chkStatus";
            this.chkStatus.Size = new System.Drawing.Size(15, 14);
            this.chkStatus.TabIndex = 0;
            this.chkStatus.UseVisualStyleBackColor = true;
            this.chkStatus.CheckedChanged += new System.EventHandler(this.chkStatus_CheckedChanged);
            // 
            // pnFooter
            // 
            this.pnFooter.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pnFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnFooter.Location = new System.Drawing.Point(0, 277);
            this.pnFooter.Name = "pnFooter";
            this.pnFooter.Size = new System.Drawing.Size(486, 32);
            this.pnFooter.TabIndex = 1;
            // 
            // pnBody
            // 
            this.pnBody.AutoScroll = true;
            this.pnBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnBody.Enabled = false;
            this.pnBody.Location = new System.Drawing.Point(0, 32);
            this.pnBody.Name = "pnBody";
            this.pnBody.Size = new System.Drawing.Size(486, 245);
            this.pnBody.TabIndex = 2;
            // 
            // ucObjectAnalyzerStep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnBody);
            this.Controls.Add(this.pnFooter);
            this.Controls.Add(this.pnHeader);
            this.Name = "ucObjectAnalyzerStep";
            this.Size = new System.Drawing.Size(486, 309);
            this.pnHeader.ResumeLayout(false);
            this.pnHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Panel pnHeader;
        protected System.Windows.Forms.Panel pnFooter;
        protected System.Windows.Forms.Panel pnBody;
        protected System.Windows.Forms.CheckBox chkStatus;
        protected System.Windows.Forms.Button btnSaveStepSettings;
        protected System.Windows.Forms.Button btnLoadStepSettings;
    }
}
