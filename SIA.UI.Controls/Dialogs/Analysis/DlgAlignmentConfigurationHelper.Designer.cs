namespace SIA.UI.Controls.Dialogs
{
    partial class DlgAlignmentConfigurationHelper
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnABSRegionDefault = new System.Windows.Forms.Button();
            this.btnHint = new System.Windows.Forms.Button();
            this.nudOrientation = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.nudRegionHeight = new System.Windows.Forms.NumericUpDown();
            this.nudRegionWidth = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.nudY = new System.Windows.Forms.NumericUpDown();
            this.nudX = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSampleSizeDefault = new System.Windows.Forms.Button();
            this.nudSampleHeight = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.nudSampleWidth = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.listView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.thumbnails = new System.Windows.Forms.ImageList(this.components);
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnDeleteAll = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdInsertion = new System.Windows.Forms.RadioButton();
            this.rdSelection = new System.Windows.Forms.RadioButton();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDefault = new System.Windows.Forms.Button();
            this.btnExportRegion = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOrientation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRegionHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRegionWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudX)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSampleHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSampleWidth)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnExportRegion);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.btnABSRegionDefault);
            this.groupBox1.Controls.Add(this.btnHint);
            this.groupBox1.Controls.Add(this.nudOrientation);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.nudRegionHeight);
            this.groupBox1.Controls.Add(this.nudRegionWidth);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.nudY);
            this.groupBox1.Controls.Add(this.nudX);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(443, 102);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Region of Interest";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(283, 73);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(46, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "(degree)";
            // 
            // btnABSRegionDefault
            // 
            this.btnABSRegionDefault.Location = new System.Drawing.Point(339, 69);
            this.btnABSRegionDefault.Name = "btnABSRegionDefault";
            this.btnABSRegionDefault.Size = new System.Drawing.Size(87, 23);
            this.btnABSRegionDefault.TabIndex = 11;
            this.btnABSRegionDefault.Text = "Apply Changes";
            this.btnABSRegionDefault.UseVisualStyleBackColor = true;
            this.btnABSRegionDefault.Click += new System.EventHandler(this.btnABSRegionApplyChanges_Click);
            // 
            // btnHint
            // 
            this.btnHint.Location = new System.Drawing.Point(339, 17);
            this.btnHint.Name = "btnHint";
            this.btnHint.Size = new System.Drawing.Size(87, 23);
            this.btnHint.TabIndex = 10;
            this.btnHint.Text = "Hint";
            this.btnHint.UseVisualStyleBackColor = true;
            this.btnHint.Click += new System.EventHandler(this.btnHint_Click);
            // 
            // nudOrientation
            // 
            this.nudOrientation.DecimalPlaces = 3;
            this.nudOrientation.Location = new System.Drawing.Point(196, 71);
            this.nudOrientation.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.nudOrientation.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.nudOrientation.Name = "nudOrientation";
            this.nudOrientation.Size = new System.Drawing.Size(81, 20);
            this.nudOrientation.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(129, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Orientation:";
            // 
            // nudRegionHeight
            // 
            this.nudRegionHeight.DecimalPlaces = 2;
            this.nudRegionHeight.Location = new System.Drawing.Point(196, 45);
            this.nudRegionHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudRegionHeight.Name = "nudRegionHeight";
            this.nudRegionHeight.Size = new System.Drawing.Size(81, 20);
            this.nudRegionHeight.TabIndex = 7;
            this.nudRegionHeight.Value = new decimal(new int[] {
            1380,
            0,
            0,
            0});
            // 
            // nudRegionWidth
            // 
            this.nudRegionWidth.DecimalPlaces = 2;
            this.nudRegionWidth.Location = new System.Drawing.Point(196, 19);
            this.nudRegionWidth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudRegionWidth.Name = "nudRegionWidth";
            this.nudRegionWidth.Size = new System.Drawing.Size(81, 20);
            this.nudRegionWidth.TabIndex = 6;
            this.nudRegionWidth.Value = new decimal(new int[] {
            2237,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(152, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Height:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(152, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Width:";
            // 
            // nudY
            // 
            this.nudY.DecimalPlaces = 2;
            this.nudY.Location = new System.Drawing.Point(46, 45);
            this.nudY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudY.Name = "nudY";
            this.nudY.Size = new System.Drawing.Size(81, 20);
            this.nudY.TabIndex = 3;
            // 
            // nudX
            // 
            this.nudX.DecimalPlaces = 2;
            this.nudX.Location = new System.Drawing.Point(46, 19);
            this.nudX.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudX.Name = "nudX";
            this.nudX.Size = new System.Drawing.Size(81, 20);
            this.nudX.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Y:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "X:";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(460, 124);
            this.panel2.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel3.Controls.Add(this.btnSampleSizeDefault);
            this.panel3.Controls.Add(this.nudSampleHeight);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.nudSampleWidth);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 124);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(460, 71);
            this.panel3.TabIndex = 4;
            // 
            // btnSampleSizeDefault
            // 
            this.btnSampleSizeDefault.Location = new System.Drawing.Point(352, 34);
            this.btnSampleSizeDefault.Name = "btnSampleSizeDefault";
            this.btnSampleSizeDefault.Size = new System.Drawing.Size(87, 23);
            this.btnSampleSizeDefault.TabIndex = 17;
            this.btnSampleSizeDefault.Text = "Apply Changes";
            this.btnSampleSizeDefault.UseVisualStyleBackColor = true;
            this.btnSampleSizeDefault.Click += new System.EventHandler(this.btnSampleSizeApplyChanges_Click);
            // 
            // nudSampleHeight
            // 
            this.nudSampleHeight.Location = new System.Drawing.Point(209, 37);
            this.nudSampleHeight.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudSampleHeight.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudSampleHeight.Name = "nudSampleHeight";
            this.nudSampleHeight.Size = new System.Drawing.Size(81, 20);
            this.nudSampleHeight.TabIndex = 10;
            this.nudSampleHeight.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(164, 39);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Height:";
            // 
            // nudSampleWidth
            // 
            this.nudSampleWidth.Location = new System.Drawing.Point(209, 9);
            this.nudSampleWidth.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudSampleWidth.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudSampleWidth.Name = "nudSampleWidth";
            this.nudSampleWidth.Size = new System.Drawing.Size(81, 20);
            this.nudSampleWidth.TabIndex = 8;
            this.nudSampleWidth.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(164, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Width:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(110, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Size of sample region:";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.listView);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 195);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(460, 275);
            this.panel4.TabIndex = 5;
            // 
            // listView
            // 
            this.listView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.LargeImageList = this.thumbnails;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(326, 273);
            this.listView.SmallImageList = this.thumbnails;
            this.listView.TabIndex = 1;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            this.listView.DoubleClick += new System.EventHandler(this.listView_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Index";
            this.columnHeader1.Width = 75;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "X-Controid";
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Y-Centroid";
            this.columnHeader3.Width = 80;
            // 
            // thumbnails
            // 
            this.thumbnails.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.thumbnails.ImageSize = new System.Drawing.Size(48, 48);
            this.thumbnails.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.SystemColors.Control;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.btnDeleteAll);
            this.panel5.Controls.Add(this.btnDelete);
            this.panel5.Controls.Add(this.groupBox2);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(326, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(132, 273);
            this.panel5.TabIndex = 0;
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteAll.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnDeleteAll.Location = new System.Drawing.Point(13, 238);
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.Size = new System.Drawing.Size(107, 23);
            this.btnDeleteAll.TabIndex = 15;
            this.btnDeleteAll.Text = "Delete All";
            this.btnDeleteAll.UseVisualStyleBackColor = true;
            this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnDelete.Location = new System.Drawing.Point(13, 209);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(107, 23);
            this.btnDelete.TabIndex = 14;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.rdInsertion);
            this.groupBox2.Controls.Add(this.rdSelection);
            this.groupBox2.Location = new System.Drawing.Point(13, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(105, 74);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Interactive Mode";
            // 
            // rdInsertion
            // 
            this.rdInsertion.AutoSize = true;
            this.rdInsertion.Location = new System.Drawing.Point(23, 43);
            this.rdInsertion.Name = "rdInsertion";
            this.rdInsertion.Size = new System.Drawing.Size(65, 17);
            this.rdInsertion.TabIndex = 1;
            this.rdInsertion.Text = "Insertion";
            this.rdInsertion.UseVisualStyleBackColor = true;
            this.rdInsertion.CheckedChanged += new System.EventHandler(this.InteractiveModeChanged);
            // 
            // rdSelection
            // 
            this.rdSelection.AutoSize = true;
            this.rdSelection.Checked = true;
            this.rdSelection.Location = new System.Drawing.Point(23, 20);
            this.rdSelection.Name = "rdSelection";
            this.rdSelection.Size = new System.Drawing.Size(69, 17);
            this.rdSelection.TabIndex = 0;
            this.rdSelection.TabStop = true;
            this.rdSelection.Text = "Selection";
            this.rdSelection.UseVisualStyleBackColor = true;
            this.rdSelection.CheckedChanged += new System.EventHandler(this.InteractiveModeChanged);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(12, 10);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(61, 23);
            this.btnLoad.TabIndex = 11;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(79, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(61, 23);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(377, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(71, 23);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(146, 10);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(61, 23);
            this.btnExport.TabIndex = 14;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.btnDefault);
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnLoad);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 470);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(460, 40);
            this.panel1.TabIndex = 0;
            // 
            // btnDefault
            // 
            this.btnDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDefault.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnDefault.Location = new System.Drawing.Point(300, 10);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(71, 23);
            this.btnDefault.TabIndex = 15;
            this.btnDefault.Text = "Default";
            this.btnDefault.UseVisualStyleBackColor = true;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // btnExportRegion
            // 
            this.btnExportRegion.Location = new System.Drawing.Point(339, 43);
            this.btnExportRegion.Name = "btnExportRegion";
            this.btnExportRegion.Size = new System.Drawing.Size(87, 23);
            this.btnExportRegion.TabIndex = 13;
            this.btnExportRegion.Text = "Export ROI";
            this.btnExportRegion.UseVisualStyleBackColor = true;
            this.btnExportRegion.Click += new System.EventHandler(this.btnExportRegion_Click);
            // 
            // DlgAlignmentConfigurationHelper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 510);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DlgAlignmentConfigurationHelper";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Alignment Configuration";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOrientation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRegionHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRegionWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudX)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSampleHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSampleWidth)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown nudRegionHeight;
        private System.Windows.Forms.NumericUpDown nudRegionWidth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudY;
        private System.Windows.Forms.NumericUpDown nudX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnHint;
        private System.Windows.Forms.NumericUpDown nudOrientation;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.NumericUpDown nudSampleHeight;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nudSampleWidth;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.Button btnDeleteAll;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.RadioButton rdInsertion;
        private System.Windows.Forms.RadioButton rdSelection;
        private System.Windows.Forms.Button btnABSRegionDefault;
        private System.Windows.Forms.Button btnSampleSizeDefault;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.ImageList thumbnails;
        private System.Windows.Forms.Button btnExportRegion;
    }
}