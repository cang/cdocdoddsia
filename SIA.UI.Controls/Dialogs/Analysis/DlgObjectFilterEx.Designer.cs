namespace SIA.UI.Controls.Dialogs
{
    partial class DlgObjectFilterEx
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
            this.chkFilter1 = new System.Windows.Forms.CheckBox();
            this.btnFilter1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.btnFilter4 = new System.Windows.Forms.Button();
            this.chkFilter4 = new System.Windows.Forms.CheckBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnFilter3 = new System.Windows.Forms.Button();
            this.chkFilter3 = new System.Windows.Forms.CheckBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnFilter2 = new System.Windows.Forms.Button();
            this.chkFilter2 = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkFilter1
            // 
            this.chkFilter1.AutoSize = true;
            this.chkFilter1.Location = new System.Drawing.Point(11, 6);
            this.chkFilter1.Name = "chkFilter1";
            this.chkFilter1.Size = new System.Drawing.Size(157, 17);
            this.chkFilter1.TabIndex = 0;
            this.chkFilter1.Text = "Apply filter for \"Dark object\"";
            this.chkFilter1.UseVisualStyleBackColor = true;
            this.chkFilter1.CheckedChanged += new System.EventHandler(this.optFilter_CheckedChanged);
            // 
            // btnFilter1
            // 
            this.btnFilter1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFilter1.Location = new System.Drawing.Point(311, 3);
            this.btnFilter1.Name = "btnFilter1";
            this.btnFilter1.Size = new System.Drawing.Size(60, 21);
            this.btnFilter1.TabIndex = 1;
            this.btnFilter1.Text = "Settings";
            this.btnFilter1.UseVisualStyleBackColor = true;
            this.btnFilter1.Click += new System.EventHandler(this.btnFilter1_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.panel8);
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(374, 117);
            this.panel1.TabIndex = 3;
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.SystemColors.Control;
            this.panel8.Controls.Add(this.btnFilter4);
            this.panel8.Controls.Add(this.chkFilter4);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 87);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(374, 29);
            this.panel8.TabIndex = 6;
            this.panel8.Visible = false;
            // 
            // btnFilter4
            // 
            this.btnFilter4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFilter4.Location = new System.Drawing.Point(311, 3);
            this.btnFilter4.Name = "btnFilter4";
            this.btnFilter4.Size = new System.Drawing.Size(60, 21);
            this.btnFilter4.TabIndex = 2;
            this.btnFilter4.Text = "Settings";
            this.btnFilter4.UseVisualStyleBackColor = true;
            this.btnFilter4.Click += new System.EventHandler(this.btnFilter4_Click);
            // 
            // chkFilter4
            // 
            this.chkFilter4.AutoSize = true;
            this.chkFilter4.Location = new System.Drawing.Point(11, 6);
            this.chkFilter4.Name = "chkFilter4";
            this.chkFilter4.Size = new System.Drawing.Size(242, 17);
            this.chkFilter4.TabIndex = 0;
            this.chkFilter4.Text = "Apply filter for \"Bright object across boundary\"";
            this.chkFilter4.UseVisualStyleBackColor = true;
            this.chkFilter4.CheckedChanged += new System.EventHandler(this.optFilter_CheckedChanged);
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.SystemColors.Control;
            this.panel6.Controls.Add(this.btnFilter3);
            this.panel6.Controls.Add(this.chkFilter3);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 58);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(374, 29);
            this.panel6.TabIndex = 4;
            this.panel6.Visible = false;
            // 
            // btnFilter3
            // 
            this.btnFilter3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFilter3.Location = new System.Drawing.Point(311, 3);
            this.btnFilter3.Name = "btnFilter3";
            this.btnFilter3.Size = new System.Drawing.Size(60, 21);
            this.btnFilter3.TabIndex = 2;
            this.btnFilter3.Text = "Settings";
            this.btnFilter3.UseVisualStyleBackColor = true;
            this.btnFilter3.Click += new System.EventHandler(this.btnFilter3_Click);
            // 
            // chkFilter3
            // 
            this.chkFilter3.AutoSize = true;
            this.chkFilter3.Location = new System.Drawing.Point(11, 6);
            this.chkFilter3.Name = "chkFilter3";
            this.chkFilter3.Size = new System.Drawing.Size(238, 17);
            this.chkFilter3.TabIndex = 0;
            this.chkFilter3.Text = "Apply filter for \"Dark object across boundary\"";
            this.chkFilter3.UseVisualStyleBackColor = true;
            this.chkFilter3.CheckedChanged += new System.EventHandler(this.optFilter_CheckedChanged);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.Control;
            this.panel4.Controls.Add(this.btnFilter2);
            this.panel4.Controls.Add(this.chkFilter2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 29);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(374, 29);
            this.panel4.TabIndex = 2;
            // 
            // btnFilter2
            // 
            this.btnFilter2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFilter2.Location = new System.Drawing.Point(311, 3);
            this.btnFilter2.Name = "btnFilter2";
            this.btnFilter2.Size = new System.Drawing.Size(60, 21);
            this.btnFilter2.TabIndex = 2;
            this.btnFilter2.Text = "Settings";
            this.btnFilter2.UseVisualStyleBackColor = true;
            this.btnFilter2.Click += new System.EventHandler(this.btnFilter2_Click);
            // 
            // chkFilter2
            // 
            this.chkFilter2.AutoSize = true;
            this.chkFilter2.Location = new System.Drawing.Point(11, 6);
            this.chkFilter2.Name = "chkFilter2";
            this.chkFilter2.Size = new System.Drawing.Size(161, 17);
            this.chkFilter2.TabIndex = 0;
            this.chkFilter2.Text = "Apply filter for \"Bright object\"";
            this.chkFilter2.UseVisualStyleBackColor = true;
            this.chkFilter2.CheckedChanged += new System.EventHandler(this.optFilter_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this.btnFilter1);
            this.panel2.Controls.Add(this.chkFilter1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(374, 29);
            this.panel2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(-31, 136);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(479, 7);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button2.Location = new System.Drawing.Point(120, 153);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button3.Location = new System.Drawing.Point(201, 153);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 6;
            this.button3.Text = "Cancel";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(67, 152);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(48, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(10, 152);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(51, 23);
            this.btnLoad.TabIndex = 7;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Visible = false;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // DlgObjectFilterEx
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 183);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DlgObjectFilterEx";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Filter Configuration";
            this.panel1.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkFilter1;
        private System.Windows.Forms.Button btnFilter1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.CheckBox chkFilter4;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.CheckBox chkFilter3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.CheckBox chkFilter2;
        private System.Windows.Forms.Button btnFilter4;
        private System.Windows.Forms.Button btnFilter3;
        private System.Windows.Forms.Button btnFilter2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoad;
    }
}