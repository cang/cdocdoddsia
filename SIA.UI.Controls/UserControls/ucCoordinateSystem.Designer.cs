namespace SIA.UI.Controls.UserControls
{
    partial class ucCoordinateSystem
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.nudRotationAngle = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.nudY = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.nudX = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.picOrientation = new System.Windows.Forms.PictureBox();
            this.cmbOrientation = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRotationAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOrientation)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnApply);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.nudRotationAngle);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.nudY);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.nudX);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.picOrientation);
            this.groupBox1.Controls.Add(this.cmbOrientation);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(374, 166);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Coordinate System";
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(261, 130);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(105, 23);
            this.btnApply.TabIndex = 13;
            this.btnApply.Text = "Apply Changes";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(204, 134);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "(degree)";
            // 
            // nudRotationAngle
            // 
            this.nudRotationAngle.DecimalPlaces = 2;
            this.nudRotationAngle.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudRotationAngle.Location = new System.Drawing.Point(112, 132);
            this.nudRotationAngle.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.nudRotationAngle.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.nudRotationAngle.Name = "nudRotationAngle";
            this.nudRotationAngle.Size = new System.Drawing.Size(86, 20);
            this.nudRotationAngle.TabIndex = 11;
            this.nudRotationAngle.Value = new decimal(new int[] {
            629,
            0,
            0,
            -2147287040});
            this.nudRotationAngle.ValueChanged += new System.EventHandler(this.InfoChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(24, 135);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Axis-X Rotation:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(204, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "(pixel)";
            // 
            // nudY
            // 
            this.nudY.DecimalPlaces = 2;
            this.nudY.Location = new System.Drawing.Point(112, 92);
            this.nudY.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nudY.Minimum = new decimal(new int[] {
            100000000,
            0,
            0,
            -2147483648});
            this.nudY.Name = "nudY";
            this.nudY.Size = new System.Drawing.Size(86, 20);
            this.nudY.TabIndex = 8;
            this.nudY.Value = new decimal(new int[] {
            1747,
            0,
            0,
            0});
            this.nudY.ValueChanged += new System.EventHandler(this.InfoChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(89, 94);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(26, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Y = ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(204, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "(pixel)";
            // 
            // nudX
            // 
            this.nudX.DecimalPlaces = 2;
            this.nudX.Location = new System.Drawing.Point(112, 66);
            this.nudX.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nudX.Minimum = new decimal(new int[] {
            100000000,
            0,
            0,
            -2147483648});
            this.nudX.Name = "nudX";
            this.nudX.Size = new System.Drawing.Size(86, 20);
            this.nudX.TabIndex = 5;
            this.nudX.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudX.ValueChanged += new System.EventHandler(this.InfoChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(89, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "X = ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Origin:";
            // 
            // picOrientation
            // 
            this.picOrientation.Image = global::SIA.UI.Controls.Properties.Resources.coordinate_system;
            this.picOrientation.Location = new System.Drawing.Point(207, 20);
            this.picOrientation.Name = "picOrientation";
            this.picOrientation.Size = new System.Drawing.Size(24, 24);
            this.picOrientation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picOrientation.TabIndex = 2;
            this.picOrientation.TabStop = false;
            // 
            // cmbOrientation
            // 
            this.cmbOrientation.Enabled = false;
            this.cmbOrientation.FormattingEnabled = true;
            this.cmbOrientation.Items.AddRange(new object[] {
            "Northeast",
            "Northwest",
            "Southwest",
            "Southeast"});
            this.cmbOrientation.Location = new System.Drawing.Point(112, 23);
            this.cmbOrientation.Name = "cmbOrientation";
            this.cmbOrientation.Size = new System.Drawing.Size(86, 21);
            this.cmbOrientation.TabIndex = 1;
            this.cmbOrientation.Text = "Northeast";
            this.cmbOrientation.SelectedIndexChanged += new System.EventHandler(this.InfoChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Orientation:";
            // 
            // ucCoordinateSystem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "ucCoordinateSystem";
            this.Size = new System.Drawing.Size(397, 180);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRotationAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOrientation)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudY;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudX;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox picOrientation;
        private System.Windows.Forms.ComboBox cmbOrientation;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nudRotationAngle;
        private System.Windows.Forms.Button btnApply;
    }
}
