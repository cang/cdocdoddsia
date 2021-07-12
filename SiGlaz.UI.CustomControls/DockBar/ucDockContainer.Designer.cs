namespace SiGlaz.UI.CustomControls.DockBar
{
    partial class ucDockContainer
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucDockContainer));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this._pTab = new System.Windows.Forms.Panel();
            this._ucTabStrip = new ucTabStrip();
            this._dockbarButton = new System.Windows.Forms.ToolStripButton();
            this._pSpace2 = new System.Windows.Forms.Panel();
            this._pSpace1 = new System.Windows.Forms.Panel();
            this._ucDockbar = new ucDockbar();
            this._pTab.SuspendLayout();
            this._ucTabStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "auto_hide.png");
            // 
            // _pTab
            // 
            this._pTab.BackColor = System.Drawing.SystemColors.Control;
            this._pTab.Controls.Add(this._ucTabStrip);
            this._pTab.Controls.Add(this._pSpace2);
            this._pTab.Controls.Add(this._pSpace1);
            this._pTab.Dock = System.Windows.Forms.DockStyle.Left;
            this._pTab.Location = new System.Drawing.Point(0, 0);
            this._pTab.Name = "_pTab";
            this._pTab.Size = new System.Drawing.Size(29, 547);
            this._pTab.TabIndex = 0;
            // 
            // _ucTabStrip
            // 
            this._ucTabStrip.BackColor = System.Drawing.SystemColors.ControlLight;
            this._ucTabStrip.DockbarStyle = System.Windows.Forms.DockStyle.Left;
            this._ucTabStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._ucTabStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._dockbarButton});
            this._ucTabStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this._ucTabStrip.Location = new System.Drawing.Point(0, 5);
            this._ucTabStrip.Name = "_ucTabStrip";
            this._ucTabStrip.ShowItemToolTips = false;
            this._ucTabStrip.Size = new System.Drawing.Size(26, 76);
            this._ucTabStrip.TabIndex = 4;
            // 
            // _dockbarButton
            // 
            this._dockbarButton.ForeColor = System.Drawing.Color.Gray;
            this._dockbarButton.Image = ((System.Drawing.Image)(resources.GetObject("_dockbarButton.Image")));
            this._dockbarButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._dockbarButton.Name = "_dockbarButton";
            this._dockbarButton.Size = new System.Drawing.Size(24, 71);
            this._dockbarButton.Text = "caption 1";
            this._dockbarButton.TextDirection = System.Windows.Forms.ToolStripTextDirection.Vertical90;
            this._dockbarButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this._dockbarButton.MouseHover += new System.EventHandler(this._dockbarButton_MouseHover);
            // 
            // _pSpace2
            // 
            this._pSpace2.Dock = System.Windows.Forms.DockStyle.Right;
            this._pSpace2.Location = new System.Drawing.Point(26, 5);
            this._pSpace2.Name = "_pSpace2";
            this._pSpace2.Size = new System.Drawing.Size(3, 542);
            this._pSpace2.TabIndex = 9;
            // 
            // _pSpace1
            // 
            this._pSpace1.Dock = System.Windows.Forms.DockStyle.Top;
            this._pSpace1.Location = new System.Drawing.Point(0, 0);
            this._pSpace1.Name = "_pSpace1";
            this._pSpace1.Size = new System.Drawing.Size(29, 5);
            this._pSpace1.TabIndex = 5;
            // 
            // _ucDockbar
            // 
            this._ucDockbar.Caption = "caption 1";
            this._ucDockbar.CaptionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this._ucDockbar.CaptionForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this._ucDockbar.CurrentHeight = 200;
            this._ucDockbar.CurrentWidth = 300;
            this._ucDockbar.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ucDockbar.DockbarStyle = System.Windows.Forms.DockStyle.Left;
            this._ucDockbar.IsAutoHide = true;
            this._ucDockbar.Location = new System.Drawing.Point(29, 0);
            this._ucDockbar.Name = "_ucDockbar";
            this._ucDockbar.Size = new System.Drawing.Size(289, 547);
            this._ucDockbar.TabIndex = 8;
            // 
            // ucDockContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this._ucDockbar);
            this.Controls.Add(this._pTab);
            this.Name = "ucDockContainer";
            this.Size = new System.Drawing.Size(318, 547);
            this.Load += new System.EventHandler(this.ucDockContainer_Load);
            this._pTab.ResumeLayout(false);
            this._pTab.PerformLayout();
            this._ucTabStrip.ResumeLayout(false);
            this._ucTabStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel _pTab;
        private ucTabStrip _ucTabStrip;
        private System.Windows.Forms.ToolStripButton _dockbarButton;
        private System.Windows.Forms.Panel _pSpace1;
        private ucDockbar _ucDockbar;
        private System.Windows.Forms.Panel _pSpace2;

    }
}
