namespace SIA.UI.Controls.Dialogs
{
    partial class DlgCalibrationValueInput
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.nudLogicalLength = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.nudRealLength = new System.Windows.Forms.NumericUpDown();
            this.lbUnit = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudLogicalLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRealLength)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(76, 93);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(157, 93);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Length:";
            // 
            // nudLogicalLength
            // 
            this.nudLogicalLength.DecimalPlaces = 3;
            this.nudLogicalLength.Enabled = false;
            this.nudLogicalLength.Location = new System.Drawing.Point(100, 12);
            this.nudLogicalLength.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.nudLogicalLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.nudLogicalLength.Name = "nudLogicalLength";
            this.nudLogicalLength.Size = new System.Drawing.Size(120, 20);
            this.nudLogicalLength.TabIndex = 6;
            this.nudLogicalLength.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(232, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "(pixel)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Real Length:";
            // 
            // nudRealLength
            // 
            this.nudRealLength.DecimalPlaces = 3;
            this.nudRealLength.Location = new System.Drawing.Point(100, 47);
            this.nudRealLength.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.nudRealLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.nudRealLength.Name = "nudRealLength";
            this.nudRealLength.Size = new System.Drawing.Size(120, 20);
            this.nudRealLength.TabIndex = 9;
            this.nudRealLength.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lbUnit
            // 
            this.lbUnit.AutoSize = true;
            this.lbUnit.Location = new System.Drawing.Point(232, 49);
            this.lbUnit.Name = "lbUnit";
            this.lbUnit.Size = new System.Drawing.Size(44, 13);
            this.lbUnit.TabIndex = 10;
            this.lbUnit.Text = "(micron)";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(-30, 77);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(573, 8);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            // 
            // DlgCalibrationValueInput
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(309, 125);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbUnit);
            this.Controls.Add(this.nudRealLength);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nudLogicalLength);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DlgCalibrationValueInput";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Calibration";
            ((System.ComponentModel.ISupportInitialize)(this.nudLogicalLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRealLength)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudLogicalLength;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudRealLength;
        private System.Windows.Forms.Label lbUnit;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}