using System;
using System.Collections.Generic;
using System.Text;
using SiGlaz.ObjectAnalysis.UI;

namespace SIA.UI.Controls.ObjectAnalysis
{
    public class ucABSPrimitiveObjectDetectionStep : ucObjectAnalyzerStep
    {
        private ucABSPrimitiveObjectDetection ucABSPrimitiveObjectDetection1;
        public const int DefaultStepId = 1;

        public ucABSPrimitiveObjectDetectionStep()
            : base(DefaultStepId)
        {
            InitializeComponent();

            chkStatus.Text = "Apply detecting primitive object(s)";
            
            SupportSearialization = false;
        }

        private void InitializeComponent()
        {
            SIA.UI.Controls.Automation.Commands.ObjectClassificationSettings objectClassificationSettings6 = new SIA.UI.Controls.Automation.Commands.ObjectClassificationSettings();
            this.ucABSPrimitiveObjectDetection1 = new SIA.UI.Controls.ObjectAnalysis.ucABSPrimitiveObjectDetection();
            this.pnHeader.SuspendLayout();
            this.pnBody.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnHeader
            // 
            this.pnHeader.Size = new System.Drawing.Size(538, 32);
            // 
            // pnFooter
            // 
            this.pnFooter.Location = new System.Drawing.Point(0, 302);
            this.pnFooter.Size = new System.Drawing.Size(538, 32);
            // 
            // pnBody
            // 
            this.pnBody.Controls.Add(this.ucABSPrimitiveObjectDetection1);
            this.pnBody.Size = new System.Drawing.Size(538, 270);
            // 
            // btnSave
            // 
            this.btnSaveStepSettings.Location = new System.Drawing.Point(454, 5);
            // 
            // btnLoad
            // 
            this.btnLoadStepSettings.Location = new System.Drawing.Point(373, 5);
            // 
            // ucABSPrimitiveObjectDetection1
            // 
            this.ucABSPrimitiveObjectDetection1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucABSPrimitiveObjectDetection1.Location = new System.Drawing.Point(0, 0);
            this.ucABSPrimitiveObjectDetection1.Name = "ucABSPrimitiveObjectDetection1";
            objectClassificationSettings6.Version = 1;
            this.ucABSPrimitiveObjectDetection1.Settings = objectClassificationSettings6;
            this.ucABSPrimitiveObjectDetection1.Size = new System.Drawing.Size(538, 88);
            this.ucABSPrimitiveObjectDetection1.TabIndex = 0;
            // 
            // ucABSPrimitiveObjectDetectionStep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "ucABSPrimitiveObjectDetectionStep";
            this.Size = new System.Drawing.Size(538, 334);
            this.pnHeader.ResumeLayout(false);
            this.pnHeader.PerformLayout();
            this.pnBody.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
