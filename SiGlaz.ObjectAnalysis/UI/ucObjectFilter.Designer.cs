namespace SiGlaz.ObjectAnalysis.UI
{
    partial class ucObjectFilter
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
            this.unverControl = new SiGlaz.ObjectAnalysis.UI.OptionsSelectionControl();
            this.SuspendLayout();
            // 
            // unverControl
            // 
            this.unverControl.ConditionExpression = "";
            this.unverControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.unverControl.Location = new System.Drawing.Point(0, 0);
            this.unverControl.Name = "unverControl";
            this.unverControl.Size = new System.Drawing.Size(556, 331);
            this.unverControl.TabIndex = 30;
            this.unverControl.UseOrAnd = true;
            // 
            // ucObjectFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.unverControl);
            this.Name = "ucObjectFilter";
            this.Size = new System.Drawing.Size(556, 331);
            this.ResumeLayout(false);

        }

        #endregion

        public OptionsSelectionControl unverControl;
    }
}
