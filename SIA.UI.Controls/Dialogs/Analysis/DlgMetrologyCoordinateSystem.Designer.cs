namespace SIA.UI.Controls.Dialogs
{
    partial class DlgMetrologyCoordinateSystem
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgMetrologyCoordinateSystem));
            SiGlaz.Common.ImageAlignment.MetrologyUnitBase metrologyUnitBase1 = new SiGlaz.Common.ImageAlignment.MetrologyUnitBase();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnRefine = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCreateReferenceFile = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.pnRegions = new System.Windows.Forms.Panel();
            this.btnRegionEditor = new System.Windows.Forms.Button();
            this.menu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.applyToWorkspaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createReferenceFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.calibrateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.hintMarkersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.createRegionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ucMarkerList1 = new SIA.UI.Controls.UserControls.ucMarkerList();
            this.ucCoordinateSystem1 = new SIA.UI.Controls.UserControls.ucCoordinateSystem();
            this.ucUnitConfiguration1 = new SIA.UI.Controls.UserControls.ucUnitConfiguration();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(12, 7);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(272, 17);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "Auto re-fine coordinate system when defining marker";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkBox2);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 311);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(462, 55);
            this.panel1.TabIndex = 4;
            this.panel1.Visible = false;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(12, 30);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(88, 17);
            this.checkBox2.TabIndex = 4;
            this.checkBox2.Text = "Show marker";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel2.Controls.Add(this.btnRefine);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Controls.Add(this.btnCreateReferenceFile);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.btnLoad);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 506);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(462, 40);
            this.panel2.TabIndex = 5;
            this.panel2.Visible = false;
            // 
            // btnRefine
            // 
            this.btnRefine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefine.Location = new System.Drawing.Point(301, 8);
            this.btnRefine.Name = "btnRefine";
            this.btnRefine.Size = new System.Drawing.Size(75, 23);
            this.btnRefine.TabIndex = 4;
            this.btnRefine.Text = "Refine";
            this.btnRefine.UseVisualStyleBackColor = true;
            this.btnRefine.Visible = false;
            this.btnRefine.Click += new System.EventHandler(this.btnRefine_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(382, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCreateReferenceFile
            // 
            this.btnCreateReferenceFile.Location = new System.Drawing.Point(166, 8);
            this.btnCreateReferenceFile.Name = "btnCreateReferenceFile";
            this.btnCreateReferenceFile.Size = new System.Drawing.Size(129, 23);
            this.btnCreateReferenceFile.TabIndex = 2;
            this.btnCreateReferenceFile.Text = "Create Reference File";
            this.btnCreateReferenceFile.UseVisualStyleBackColor = true;
            this.btnCreateReferenceFile.Click += new System.EventHandler(this.btnCreateReferenceFile_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(85, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(4, 8);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // pnRegions
            // 
            this.pnRegions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnRegions.Location = new System.Drawing.Point(0, 467);
            this.pnRegions.Name = "pnRegions";
            this.pnRegions.Size = new System.Drawing.Size(462, 39);
            this.pnRegions.TabIndex = 7;
            this.pnRegions.Visible = false;
            // 
            // btnRegionEditor
            // 
            this.btnRegionEditor.Location = new System.Drawing.Point(301, 201);
            this.btnRegionEditor.Name = "btnRegionEditor";
            this.btnRegionEditor.Size = new System.Drawing.Size(105, 23);
            this.btnRegionEditor.TabIndex = 0;
            this.btnRegionEditor.Text = "Region Editor";
            this.btnRegionEditor.UseVisualStyleBackColor = true;
            this.btnRegionEditor.Visible = false;
            this.btnRegionEditor.Click += new System.EventHandler(this.btnRegionEditor_Click);
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(462, 24);
            this.menu.TabIndex = 8;
            this.menu.Text = "menu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripMenuItem5,
            this.applyToWorkspaceToolStripMenuItem,
            this.toolStripMenuItem2,
            this.importToolStripMenuItem,
            this.createReferenceFileToolStripMenuItem,
            this.toolStripMenuItem3,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.openToolStripMenuItem.Text = "&Open";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(181, 6);
            // 
            // applyToWorkspaceToolStripMenuItem
            // 
            this.applyToWorkspaceToolStripMenuItem.Name = "applyToWorkspaceToolStripMenuItem";
            this.applyToWorkspaceToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.applyToWorkspaceToolStripMenuItem.Text = "Apply to Workspace";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(181, 6);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.importToolStripMenuItem.Text = "&Import";
            // 
            // createReferenceFileToolStripMenuItem
            // 
            this.createReferenceFileToolStripMenuItem.Name = "createReferenceFileToolStripMenuItem";
            this.createReferenceFileToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.createReferenceFileToolStripMenuItem.Text = "Create Reference File";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(181, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.exitToolStripMenuItem.Text = "&Close";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.calibrateToolStripMenuItem,
            this.toolStripMenuItem1,
            this.hintMarkersToolStripMenuItem,
            this.refineToolStripMenuItem,
            this.toolStripMenuItem4,
            this.createRegionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // calibrateToolStripMenuItem
            // 
            this.calibrateToolStripMenuItem.Name = "calibrateToolStripMenuItem";
            this.calibrateToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.calibrateToolStripMenuItem.Text = "&Calibrate";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(150, 6);
            // 
            // hintMarkersToolStripMenuItem
            // 
            this.hintMarkersToolStripMenuItem.Name = "hintMarkersToolStripMenuItem";
            this.hintMarkersToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.hintMarkersToolStripMenuItem.Text = "&Hint Markers";
            // 
            // refineToolStripMenuItem
            // 
            this.refineToolStripMenuItem.Name = "refineToolStripMenuItem";
            this.refineToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.refineToolStripMenuItem.Text = "Re&fine";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(150, 6);
            this.toolStripMenuItem4.Visible = false;
            // 
            // createRegionsToolStripMenuItem
            // 
            this.createRegionsToolStripMenuItem.Name = "createRegionsToolStripMenuItem";
            this.createRegionsToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.createRegionsToolStripMenuItem.Text = "Create &Regions";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // ucMarkerList1
            // 
            this.ucMarkerList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucMarkerList1.Location = new System.Drawing.Point(0, 366);
            this.ucMarkerList1.MetrologySystem = null;
            this.ucMarkerList1.Name = "ucMarkerList1";
            this.ucMarkerList1.SelectedMarkerIndex = -1;
            this.ucMarkerList1.Size = new System.Drawing.Size(462, 101);
            this.ucMarkerList1.TabIndex = 6;
            // 
            // ucCoordinateSystem1
            // 
            this.ucCoordinateSystem1.CoordinateSystem = null;
            this.ucCoordinateSystem1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucCoordinateSystem1.Location = new System.Drawing.Point(0, 131);
            this.ucCoordinateSystem1.Name = "ucCoordinateSystem1";
            this.ucCoordinateSystem1.OriginLocation = ((System.Drawing.PointF)(resources.GetObject("ucCoordinateSystem1.OriginLocation")));
            this.ucCoordinateSystem1.Size = new System.Drawing.Size(462, 180);
            this.ucCoordinateSystem1.TabIndex = 2;
            // 
            // ucUnitConfiguration1
            // 
            this.ucUnitConfiguration1.CurrentUnit = metrologyUnitBase1;
            this.ucUnitConfiguration1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucUnitConfiguration1.Location = new System.Drawing.Point(0, 24);
            this.ucUnitConfiguration1.Name = "ucUnitConfiguration1";
            this.ucUnitConfiguration1.PixelVal = 1F;
            this.ucUnitConfiguration1.Size = new System.Drawing.Size(462, 107);
            this.ucUnitConfiguration1.TabIndex = 1;
            this.ucUnitConfiguration1.UnitVal = 1F;
            // 
            // DlgMetrologyCoordinateSystem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 546);
            this.Controls.Add(this.btnRegionEditor);
            this.Controls.Add(this.ucMarkerList1);
            this.Controls.Add(this.pnRegions);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ucCoordinateSystem1);
            this.Controls.Add(this.ucUnitConfiguration1);
            this.Controls.Add(this.menu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MainMenuStrip = this.menu;
            this.MinimumSize = new System.Drawing.Size(465, 580);
            this.Name = "DlgMetrologyCoordinateSystem";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Metrology System";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SIA.UI.Controls.UserControls.ucUnitConfiguration ucUnitConfiguration1;
        private SIA.UI.Controls.UserControls.ucCoordinateSystem ucCoordinateSystem1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCreateReferenceFile;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoad;
        private SIA.UI.Controls.UserControls.ucMarkerList ucMarkerList1;
        private System.Windows.Forms.Button btnRefine;
        private System.Windows.Forms.Panel pnRegions;
        private System.Windows.Forms.Button btnRegionEditor;
        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createReferenceFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem calibrateToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem hintMarkersToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem createRegionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refineToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem applyToWorkspaceToolStripMenuItem;
    }
}