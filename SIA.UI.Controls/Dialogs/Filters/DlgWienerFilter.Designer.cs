namespace SIA.UI.Controls.Dialogs
{
    partial class DlgWienerFilter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgWienerFilter));
            this.label1 = new System.Windows.Forms.Label();
            this.nudKernelSize = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.rdAuto = new System.Windows.Forms.RadioButton();
            this.rdManual = new System.Windows.Forms.RadioButton();
            this.nudNoiseLevel = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudKernelSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNoiseLevel)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Kernel size:";
            // 
            // nudKernelSize
            // 
            this.nudKernelSize.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudKernelSize.Location = new System.Drawing.Point(102, 15);
            this.nudKernelSize.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudKernelSize.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nudKernelSize.Name = "nudKernelSize";
            this.nudKernelSize.Size = new System.Drawing.Size(67, 20);
            this.nudKernelSize.TabIndex = 1;
            this.nudKernelSize.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.nudKernelSize.ValueChanged += new System.EventHandler(this.nudKernelSize_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(175, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "(pixels)";
            // 
            // rdAuto
            // 
            this.rdAuto.AutoSize = true;
            this.rdAuto.Checked = true;
            this.rdAuto.Location = new System.Drawing.Point(14, 20);
            this.rdAuto.Name = "rdAuto";
            this.rdAuto.Size = new System.Drawing.Size(47, 17);
            this.rdAuto.TabIndex = 3;
            this.rdAuto.TabStop = true;
            this.rdAuto.Text = "Auto";
            this.rdAuto.UseVisualStyleBackColor = true;
            this.rdAuto.CheckedChanged += new System.EventHandler(this.rdAuto_CheckedChanged);
            // 
            // rdManual
            // 
            this.rdManual.AutoSize = true;
            this.rdManual.Location = new System.Drawing.Point(14, 43);
            this.rdManual.Name = "rdManual";
            this.rdManual.Size = new System.Drawing.Size(60, 17);
            this.rdManual.TabIndex = 4;
            this.rdManual.Text = "Manual";
            this.rdManual.UseVisualStyleBackColor = true;
            this.rdManual.CheckedChanged += new System.EventHandler(this.rdManual_CheckedChanged);
            // 
            // nudNoiseLevel
            // 
            this.nudNoiseLevel.DecimalPlaces = 3;
            this.nudNoiseLevel.Enabled = false;
            this.nudNoiseLevel.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudNoiseLevel.Location = new System.Drawing.Point(78, 43);
            this.nudNoiseLevel.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudNoiseLevel.Name = "nudNoiseLevel";
            this.nudNoiseLevel.Size = new System.Drawing.Size(67, 20);
            this.nudNoiseLevel.TabIndex = 5;
            this.nudNoiseLevel.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudNoiseLevel.ValueChanged += new System.EventHandler(this.nudNoiseLevel_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdAuto);
            this.groupBox1.Controls.Add(this.nudNoiseLevel);
            this.groupBox1.Controls.Add(this.rdManual);
            this.groupBox1.Location = new System.Drawing.Point(24, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(190, 75);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Global noise level";
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(-28, 122);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(367, 7);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(41, 143);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(122, 143);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // DlgWienerFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(238, 178);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nudKernelSize);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DlgWienerFilter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Adaptive Filter";
            ((System.ComponentModel.ISupportInitialize)(this.nudKernelSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNoiseLevel)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudKernelSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdAuto;
        private System.Windows.Forms.RadioButton rdManual;
        private System.Windows.Forms.NumericUpDown nudNoiseLevel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}