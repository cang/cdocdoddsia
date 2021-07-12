namespace SIA.UI.Controls.UserControls
{
    partial class ucObjectFilterItem
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
            this.pnFilterContents = new System.Windows.Forms.Panel();
            this.chkMax = new System.Windows.Forms.CheckBox();
            this.nudMax = new System.Windows.Forms.NumericUpDown();
            this.chkMin = new System.Windows.Forms.CheckBox();
            this.nudMin = new System.Windows.Forms.NumericUpDown();
            this.pnFilterStates = new System.Windows.Forms.Panel();
            this.chkState = new System.Windows.Forms.CheckBox();
            this.pnFilterContents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMin)).BeginInit();
            this.pnFilterStates.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnFilterContents
            // 
            this.pnFilterContents.Controls.Add(this.chkMax);
            this.pnFilterContents.Controls.Add(this.nudMax);
            this.pnFilterContents.Controls.Add(this.chkMin);
            this.pnFilterContents.Controls.Add(this.nudMin);
            this.pnFilterContents.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnFilterContents.Location = new System.Drawing.Point(0, 22);
            this.pnFilterContents.Name = "pnFilterContents";
            this.pnFilterContents.Size = new System.Drawing.Size(359, 26);
            this.pnFilterContents.TabIndex = 3;
            // 
            // chkMax
            // 
            this.chkMax.AutoSize = true;
            this.chkMax.Location = new System.Drawing.Point(197, 4);
            this.chkMax.Name = "chkMax";
            this.chkMax.Size = new System.Drawing.Size(73, 17);
            this.chkMax.TabIndex = 4;
            this.chkMax.Text = "Maximum:";
            this.chkMax.UseVisualStyleBackColor = true;
            // 
            // nudMax
            // 
            this.nudMax.Location = new System.Drawing.Point(274, 2);
            this.nudMax.Name = "nudMax";
            this.nudMax.Size = new System.Drawing.Size(79, 20);
            this.nudMax.TabIndex = 3;
            // 
            // chkMin
            // 
            this.chkMin.AutoSize = true;
            this.chkMin.Location = new System.Drawing.Point(22, 5);
            this.chkMin.Name = "chkMin";
            this.chkMin.Size = new System.Drawing.Size(70, 17);
            this.chkMin.TabIndex = 2;
            this.chkMin.Text = "Minimum:";
            this.chkMin.UseVisualStyleBackColor = true;
            // 
            // nudMin
            // 
            this.nudMin.Location = new System.Drawing.Point(95, 3);
            this.nudMin.Name = "nudMin";
            this.nudMin.Size = new System.Drawing.Size(79, 20);
            this.nudMin.TabIndex = 1;
            // 
            // pnFilterStates
            // 
            this.pnFilterStates.Controls.Add(this.chkState);
            this.pnFilterStates.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnFilterStates.Location = new System.Drawing.Point(0, 0);
            this.pnFilterStates.Name = "pnFilterStates";
            this.pnFilterStates.Size = new System.Drawing.Size(359, 22);
            this.pnFilterStates.TabIndex = 2;
            // 
            // chkState
            // 
            this.chkState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chkState.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.chkState.Location = new System.Drawing.Point(3, 3);
            this.chkState.Name = "chkState";
            this.chkState.Size = new System.Drawing.Size(353, 17);
            this.chkState.TabIndex = 0;
            this.chkState.Text = "checkBox1";
            this.chkState.UseVisualStyleBackColor = true;
            // 
            // ucObjectFilterItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnFilterContents);
            this.Controls.Add(this.pnFilterStates);
            this.Name = "ucObjectFilterItem";
            this.Size = new System.Drawing.Size(359, 22);
            this.pnFilterContents.ResumeLayout(false);
            this.pnFilterContents.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMin)).EndInit();
            this.pnFilterStates.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnFilterContents;
        private System.Windows.Forms.CheckBox chkMax;
        private System.Windows.Forms.NumericUpDown nudMax;
        private System.Windows.Forms.CheckBox chkMin;
        private System.Windows.Forms.NumericUpDown nudMin;
        private System.Windows.Forms.Panel pnFilterStates;
        private System.Windows.Forms.CheckBox chkState;
    }
}
