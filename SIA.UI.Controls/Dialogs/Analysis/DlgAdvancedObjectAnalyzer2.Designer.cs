namespace SIA.UI.Controls.Dialogs
{
    partial class DlgAdvancedObjectAnalyzer2
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
            this.ucAdvancedObjectAnalyzer1 = new SiGlaz.ObjectAnalysis.UI.ucAdvancedObjectAnalyzer();
            this.SuspendLayout();
            // 
            // ucAdvancedObjectAnalyzer1
            // 
            this.ucAdvancedObjectAnalyzer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucAdvancedObjectAnalyzer1.Location = new System.Drawing.Point(0, 0);
            this.ucAdvancedObjectAnalyzer1.Name = "ucAdvancedObjectAnalyzer1";
            this.ucAdvancedObjectAnalyzer1.Size = new System.Drawing.Size(667, 540);
            this.ucAdvancedObjectAnalyzer1.TabIndex = 0;
            // 
            // DlgAdvancedObjectAnalyzer2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 540);
            this.Controls.Add(this.ucAdvancedObjectAnalyzer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(683, 450);
            this.Name = "DlgAdvancedObjectAnalyzer2";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Advanced Object Analyzer";
            this.ResumeLayout(false);

        }

        #endregion

        private SiGlaz.ObjectAnalysis.UI.ucAdvancedObjectAnalyzer ucAdvancedObjectAnalyzer1;
    }
}