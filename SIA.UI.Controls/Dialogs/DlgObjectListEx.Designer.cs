using System.Windows.Forms;
namespace SIA.UI.Controls.Dialogs
{
    partial class DlgObjectListEx
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        //private System.ComponentModel.IContainer components = null;

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
            SiGlaz.UI.CustomControls.GradientColor gradientColor1 = new SiGlaz.UI.CustomControls.GradientColor();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgObjectListEx));
            this.menu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolbar = new System.Windows.Forms.ToolStrip();
            this.statusbar = new System.Windows.Forms.StatusStrip();
            this.pnPreview = new System.Windows.Forms.Panel();
            this.splitter = new SiGlaz.UI.CustomControls.SplitterEx();
            this.pnListView = new System.Windows.Forms.Panel();
            this._listView = new SIA.UI.Controls.Dialogs.ObjectListView();
            this.tbShowHidePreview = new System.Windows.Forms.ToolStripButton();
            this.tbViewMode = new System.Windows.Forms.ToolStripSplitButton();
            this.mnViewLargeIcons = new System.Windows.Forms.ToolStripMenuItem();
            this.mnViewSmallIcons = new System.Windows.Forms.ToolStripMenuItem();
            this.mnViewList = new System.Windows.Forms.ToolStripMenuItem();
            this.mnViewDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.mnViewTiles = new System.Windows.Forms.ToolStripMenuItem();
            this.menu.SuspendLayout();
            this.toolbar.SuspendLayout();
            this.pnListView.SuspendLayout();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(673, 24);
            this.menu.TabIndex = 0;
            this.menu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // toolbar
            // 
            this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbShowHidePreview,
            this.tbViewMode});
            this.toolbar.Location = new System.Drawing.Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new System.Drawing.Size(468, 25);
            this.toolbar.TabIndex = 1;
            this.toolbar.Text = "toolStrip1";
            // 
            // statusbar
            // 
            this.statusbar.Location = new System.Drawing.Point(0, 480);
            this.statusbar.Name = "statusbar";
            this.statusbar.Size = new System.Drawing.Size(673, 22);
            this.statusbar.TabIndex = 2;
            this.statusbar.Text = "statusStrip1";
            // 
            // pnPreview
            // 
            this.pnPreview.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnPreview.Location = new System.Drawing.Point(473, 24);
            this.pnPreview.Name = "pnPreview";
            this.pnPreview.Size = new System.Drawing.Size(200, 456);
            this.pnPreview.TabIndex = 3;
            // 
            // splitter
            // 
            this.splitter.AnimationDelay = 20;
            this.splitter.AnimationStep = 20;
            gradientColor1.End = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(250)))));
            gradientColor1.Start = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(250)))));
            this.splitter.BkColor = gradientColor1;
            this.splitter.BorderStyle3D = System.Windows.Forms.Border3DStyle.Flat;
            this.splitter.ControlToHide = this.pnPreview;
            this.splitter.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter.ExpandParentForm = false;
            this.splitter.Location = new System.Drawing.Point(468, 24);
            this.splitter.Name = "splitter";
            this.splitter.Size = new System.Drawing.Size(5, 456);
            this.splitter.TabIndex = 4;
            this.splitter.TabStop = false;
            this.splitter.UseAnimations = false;
            this.splitter.VisualStyle = SiGlaz.UI.CustomControls.VisualStyles.Mozilla;
            // 
            // pnListView
            // 
            this.pnListView.Controls.Add(this._listView);
            this.pnListView.Controls.Add(this.toolbar);
            this.pnListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnListView.Location = new System.Drawing.Point(0, 24);
            this.pnListView.Name = "pnListView";
            this.pnListView.Size = new System.Drawing.Size(468, 456);
            this.pnListView.TabIndex = 5;
            // 
            // _listView
            // 
            this._listView.AllowColumnReorder = true;
            this._listView.CahcedItems = null;
            this._listView.DataItemController = null;
            this._listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._listView.FirstCachedItemIdx = 0;
            this._listView.FullRowSelect = true;
            this._listView.GridLines = true;
            this._listView.Location = new System.Drawing.Point(0, 25);
            this._listView.Name = "_listView";
            this._listView.Objects = null;
            this._listView.Size = new System.Drawing.Size(468, 431);
            this._listView.TabIndex = 0;
            this._listView.UseCompatibleStateImageBehavior = false;
            this._listView.View = System.Windows.Forms.View.Details;
            this._listView.VirtualMode = true;
            // 
            // tbShowHidePreview
            // 
            this.tbShowHidePreview.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tbShowHidePreview.CheckOnClick = true;
            this.tbShowHidePreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbShowHidePreview.Image = global::SIA.UI.Controls.Properties.Resources.show_preview;
            this.tbShowHidePreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbShowHidePreview.Name = "tbShowHidePreview";
            this.tbShowHidePreview.Size = new System.Drawing.Size(23, 22);
            this.tbShowHidePreview.Text = "Show/Hide Preview Panel";
            // 
            // tbViewMode
            // 
            this.tbViewMode.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tbViewMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnViewLargeIcons,
            this.mnViewSmallIcons,
            this.mnViewList,
            this.mnViewDetails,
            this.mnViewTiles});
            this.tbViewMode.Image = global::SIA.UI.Controls.Properties.Resources.detail;
            this.tbViewMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbViewMode.Name = "tbViewMode";
            this.tbViewMode.Size = new System.Drawing.Size(134, 22);
            this.tbViewMode.Text = "Change your view";
            this.tbViewMode.ToolTipText = "Change your view";
            // 
            // mnViewLargeIcons
            // 
            this.mnViewLargeIcons.Image = global::SIA.UI.Controls.Properties.Resources.extra_large;
            this.mnViewLargeIcons.Name = "mnViewLargeIcons";
            this.mnViewLargeIcons.Size = new System.Drawing.Size(152, 22);
            this.mnViewLargeIcons.Text = "Large Icons";
            this.mnViewLargeIcons.ToolTipText = "View as Large Icons";
            // 
            // mnViewSmallIcons
            // 
            this.mnViewSmallIcons.Image = global::SIA.UI.Controls.Properties.Resources.small;
            this.mnViewSmallIcons.Name = "mnViewSmallIcons";
            this.mnViewSmallIcons.Size = new System.Drawing.Size(152, 22);
            this.mnViewSmallIcons.Text = "Small Icons";
            this.mnViewSmallIcons.ToolTipText = "View as Small Icons";
            // 
            // mnViewList
            // 
            this.mnViewList.Image = global::SIA.UI.Controls.Properties.Resources.list;
            this.mnViewList.Name = "mnViewList";
            this.mnViewList.Size = new System.Drawing.Size(152, 22);
            this.mnViewList.Text = "List";
            this.mnViewList.ToolTipText = "View as List";
            // 
            // mnViewDetails
            // 
            this.mnViewDetails.Image = global::SIA.UI.Controls.Properties.Resources.detail;
            this.mnViewDetails.Name = "mnViewDetails";
            this.mnViewDetails.Size = new System.Drawing.Size(152, 22);
            this.mnViewDetails.Text = "Details";
            this.mnViewDetails.ToolTipText = "View as Details";
            // 
            // mnViewTiles
            // 
            this.mnViewTiles.Image = global::SIA.UI.Controls.Properties.Resources.title;
            this.mnViewTiles.Name = "mnViewTiles";
            this.mnViewTiles.Size = new System.Drawing.Size(152, 22);
            this.mnViewTiles.Text = "Tiles";
            this.mnViewTiles.ToolTipText = "View as Tiles";
            // 
            // DlgObjectListEx
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 502);
            this.Controls.Add(this.pnListView);
            this.Controls.Add(this.splitter);
            this.Controls.Add(this.pnPreview);
            this.Controls.Add(this.statusbar);
            this.Controls.Add(this.menu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menu;
            this.Name = "DlgObjectListEx";
            this.ShowInTaskbar = false;
            this.Text = "DlgObjectListEx";
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.pnListView.ResumeLayout(false);
            this.pnListView.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolbar;
        private System.Windows.Forms.StatusStrip statusbar;
        private System.Windows.Forms.Panel pnPreview;
        private SiGlaz.UI.CustomControls.SplitterEx splitter;
        private System.Windows.Forms.ToolStripButton tbShowHidePreview;
        private System.Windows.Forms.ToolStripSplitButton tbViewMode;
        private System.Windows.Forms.ToolStripMenuItem mnViewLargeIcons;
        private System.Windows.Forms.ToolStripMenuItem mnViewTiles;
        private System.Windows.Forms.ToolStripMenuItem mnViewSmallIcons;
        private System.Windows.Forms.ToolStripMenuItem mnViewDetails;
        private System.Windows.Forms.Panel pnListView;


        private ObjectListView _listView = null;
        private ImageList _smallImageList = null;
        private ImageList _largeImageList = null;
        private ToolStripMenuItem mnViewList;

    }
}