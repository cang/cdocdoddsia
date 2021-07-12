using System;
using System.Collections.Generic;
using System.Text;
using SiGlaz.ObjectAnalysis.UI;

namespace SIA.UI.Controls.ObjectAnalysis
{
    public class ucABSPreprocessingStep : ucObjectAnalyzerStep
    {
        private ucABSAlignment alignment;

        public const int DefaultStepId = 0;

        public ucABSPreprocessingStep()
            : base(DefaultStepId)
        {
            InitializeComponent();

            chkStatus.Text = "Apply preprocessing";

            SupportSearialization = false;
        }

        private void InitializeComponent()
        {
            this.alignment = new SIA.UI.Controls.ObjectAnalysis.ucABSAlignment();
            this.pnHeader.SuspendLayout();
            this.pnBody.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnBody
            // 
            this.pnBody.Controls.Add(this.alignment);
            // 
            // btnSave
            // 
            this.btnSaveStepSettings.Location = new System.Drawing.Point(403, 5);
            // 
            // btnLoad
            // 
            this.btnLoadStepSettings.Location = new System.Drawing.Point(322, 5);
            // 
            // alignment
            // 
            this.alignment.Dock = System.Windows.Forms.DockStyle.Top;
            this.alignment.Location = new System.Drawing.Point(0, 0);
            this.alignment.MethodName = "Auto";
            this.alignment.Name = "alignment";
            this.alignment.Size = new System.Drawing.Size(486, 112);
            this.alignment.TabIndex = 0;
            // 
            // ucABSPreprocessingStep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "ucABSPreprocessingStep";
            this.pnHeader.ResumeLayout(false);
            this.pnHeader.PerformLayout();
            this.pnBody.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
