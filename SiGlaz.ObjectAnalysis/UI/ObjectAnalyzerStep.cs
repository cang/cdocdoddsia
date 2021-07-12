using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SiGlaz.ObjectAnalysis.UI
{
    public class ObjectAnalyzerStep : Control
    {
        protected System.Windows.Forms.Panel pnHeader;
        protected System.Windows.Forms.Panel pnFooter;
        protected System.Windows.Forms.Panel pnBody;
        protected System.Windows.Forms.CheckBox chkStatus;

        public ObjectAnalyzerStep() : base()
        {
            InitializeComponent();
        }

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
            this.pnHeader = new System.Windows.Forms.Panel();
            this.pnFooter = new System.Windows.Forms.Panel();
            this.pnBody = new System.Windows.Forms.Panel();
            this.chkStatus = new System.Windows.Forms.CheckBox();
            this.pnHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnHeader
            // 
            this.pnHeader.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pnHeader.Controls.Add(this.chkStatus);
            this.pnHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnHeader.Location = new System.Drawing.Point(0, 0);
            this.pnHeader.Name = "pnHeader";
            this.pnHeader.Size = new System.Drawing.Size(486, 32);
            this.pnHeader.TabIndex = 0;
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
            this.pnBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnBody.Location = new System.Drawing.Point(0, 32);
            this.pnBody.Name = "pnBody";
            this.pnBody.Size = new System.Drawing.Size(486, 245);
            this.pnBody.TabIndex = 2;
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
            // ucObjectAnalyzerStep
            // 
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

        private void chkStatus_CheckedChanged(object sender, EventArgs e)
        {
            OnCheckBoxStatus_CheckedChanged();
        }

        protected virtual void OnCheckBoxStatus_CheckedChanged()
        {
        }
    }
}
