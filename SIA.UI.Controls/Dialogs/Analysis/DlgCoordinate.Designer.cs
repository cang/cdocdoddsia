namespace SIA.UI.Controls.Dialogs
{
    partial class DlgCoordinate
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
            this.label1 = new System.Windows.Forms.Label();
            this.nudX = new System.Windows.Forms.NumericUpDown();
            this.lbUnit1 = new System.Windows.Forms.Label();
            this.lbUnit2 = new System.Windows.Forms.Label();
            this.nudY = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudY)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "X:";
            // 
            // nudX
            // 
            this.nudX.DecimalPlaces = 3;
            this.nudX.Location = new System.Drawing.Point(48, 9);
            this.nudX.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudX.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.nudX.Name = "nudX";
            this.nudX.Size = new System.Drawing.Size(116, 20);
            this.nudX.TabIndex = 1;
            // 
            // lbUnit1
            // 
            this.lbUnit1.AutoSize = true;
            this.lbUnit1.Location = new System.Drawing.Point(170, 11);
            this.lbUnit1.Name = "lbUnit1";
            this.lbUnit1.Size = new System.Drawing.Size(44, 13);
            this.lbUnit1.TabIndex = 2;
            this.lbUnit1.Text = "(micron)";
            // 
            // lbUnit2
            // 
            this.lbUnit2.AutoSize = true;
            this.lbUnit2.Location = new System.Drawing.Point(170, 40);
            this.lbUnit2.Name = "lbUnit2";
            this.lbUnit2.Size = new System.Drawing.Size(44, 13);
            this.lbUnit2.TabIndex = 5;
            this.lbUnit2.Text = "(micron)";
            // 
            // nudY
            // 
            this.nudY.DecimalPlaces = 3;
            this.nudY.Location = new System.Drawing.Point(48, 38);
            this.nudY.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudY.Minimum = new decimal(new int[] {
            10000000,
            0,
            0,
            -2147483648});
            this.nudY.Name = "nudY";
            this.nudY.Size = new System.Drawing.Size(116, 20);
            this.nudY.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Y:";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(-12, 66);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(287, 7);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(58, 81);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(60, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(124, 81);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(60, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // DlgCoordinate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(242, 112);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbUnit2);
            this.Controls.Add(this.nudY);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbUnit1);
            this.Controls.Add(this.nudX);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DlgCoordinate";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Coordinate";
            ((System.ComponentModel.ISupportInitialize)(this.nudX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudY)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudX;
        private System.Windows.Forms.Label lbUnit1;
        private System.Windows.Forms.Label lbUnit2;
        private System.Windows.Forms.NumericUpDown nudY;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}