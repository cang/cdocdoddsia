namespace SIA.UI.Controls.Dialogs
{
    partial class DlgObjectListEx2
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgObjectListEx2));
            this.objectList = new SiGlaz.UI.CustomControls.ListViewEx.ObjectListView();
            this.idColumn = new SiGlaz.UI.CustomControls.ListViewEx.OLVColumn();
            this.thumbnailColumn = new SiGlaz.UI.CustomControls.ListViewEx.OLVColumn();
            this.typeColumn = new SiGlaz.UI.CustomControls.ListViewEx.OLVColumn();
            this.xColumn = new SiGlaz.UI.CustomControls.ListViewEx.OLVColumn();
            this.yColumn = new SiGlaz.UI.CustomControls.ListViewEx.OLVColumn();
            this.widthColumn = new SiGlaz.UI.CustomControls.ListViewEx.OLVColumn();
            this.heightColumn = new SiGlaz.UI.CustomControls.ListViewEx.OLVColumn();
            this.pxCountColumn = new SiGlaz.UI.CustomControls.ListViewEx.OLVColumn();
            this.largeImageList = new System.Windows.Forms.ImageList(this.components);
            this.smallImageList = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.objectList)).BeginInit();
            this.SuspendLayout();
            // 
            // objectList
            // 
            this.objectList.AllColumns.Add(this.idColumn);
            this.objectList.AllColumns.Add(this.thumbnailColumn);
            this.objectList.AllColumns.Add(this.typeColumn);
            this.objectList.AllColumns.Add(this.xColumn);
            this.objectList.AllColumns.Add(this.yColumn);
            this.objectList.AllColumns.Add(this.widthColumn);
            this.objectList.AllColumns.Add(this.heightColumn);
            this.objectList.AllColumns.Add(this.pxCountColumn);
            this.objectList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.idColumn,
            this.thumbnailColumn,
            this.typeColumn,
            this.xColumn,
            this.yColumn,
            this.widthColumn,
            this.heightColumn,
            this.pxCountColumn});
            this.objectList.FullRowSelect = true;
            this.objectList.GridLines = true;
            this.objectList.GroupWithItemCountFormat = "{0} ({1} objects)";
            this.objectList.GroupWithItemCountSingularFormat = "{0} ({1} object)";
            this.objectList.LargeImageList = this.largeImageList;
            this.objectList.Location = new System.Drawing.Point(12, 24);
            this.objectList.Name = "objectList";
            this.objectList.ShowImagesOnSubItems = true;
            this.objectList.ShowItemCountOnGroups = true;
            this.objectList.Size = new System.Drawing.Size(540, 409);
            this.objectList.SmallImageList = this.smallImageList;
            this.objectList.SpaceBetweenGroups = 20;
            this.objectList.TabIndex = 0;
            this.objectList.UseCompatibleStateImageBehavior = false;
            this.objectList.View = System.Windows.Forms.View.Details;
            // 
            // idColumn
            // 
            this.idColumn.AspectName = "";
            this.idColumn.IsEditable = false;
            this.idColumn.Text = "ID";
            // 
            // thumbnailColumn
            // 
            this.thumbnailColumn.IsEditable = false;
            this.thumbnailColumn.Text = "Thumbnail";
            // 
            // typeColumn
            // 
            this.typeColumn.AspectName = "ObjectTypeId";
            this.typeColumn.CheckBoxes = true;
            this.typeColumn.GroupWithItemCountFormat = "{0} ({1} objects)";
            this.typeColumn.GroupWithItemCountSingularFormat = "{0} ({1} object)";
            this.typeColumn.IsEditable = false;
            this.typeColumn.Text = "Type";
            this.typeColumn.TriStateCheckBoxes = true;
            this.typeColumn.UseInitialLetterForGroup = true;
            // 
            // xColumn
            // 
            this.xColumn.IsEditable = false;
            this.xColumn.Text = "Left";
            // 
            // yColumn
            // 
            this.yColumn.IsEditable = false;
            this.yColumn.Text = "Top";
            // 
            // widthColumn
            // 
            this.widthColumn.IsEditable = false;
            this.widthColumn.Text = "Width";
            // 
            // heightColumn
            // 
            this.heightColumn.IsEditable = false;
            this.heightColumn.Text = "Height";
            // 
            // pxCountColumn
            // 
            this.pxCountColumn.AspectName = "NumPixels";
            this.pxCountColumn.IsEditable = false;
            this.pxCountColumn.Text = "Pixel Count";
            // 
            // largeImageList
            // 
            this.largeImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.largeImageList.ImageSize = new System.Drawing.Size(16, 16);
            this.largeImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // smallImageList
            // 
            this.smallImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.smallImageList.ImageSize = new System.Drawing.Size(32, 32);
            this.smallImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // DlgObjectListEx2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 474);
            this.Controls.Add(this.objectList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DlgObjectListEx2";
            this.ShowInTaskbar = false;
            this.Text = "DlgObjectListViewEx2";
            ((System.ComponentModel.ISupportInitialize)(this.objectList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private SiGlaz.UI.CustomControls.ListViewEx.ObjectListView objectList;
        private SiGlaz.UI.CustomControls.ListViewEx.OLVColumn idColumn;
        private SiGlaz.UI.CustomControls.ListViewEx.OLVColumn thumbnailColumn;
        private SiGlaz.UI.CustomControls.ListViewEx.OLVColumn typeColumn;
        private SiGlaz.UI.CustomControls.ListViewEx.OLVColumn xColumn;
        private SiGlaz.UI.CustomControls.ListViewEx.OLVColumn yColumn;
        private SiGlaz.UI.CustomControls.ListViewEx.OLVColumn widthColumn;
        private SiGlaz.UI.CustomControls.ListViewEx.OLVColumn heightColumn;
        private SiGlaz.UI.CustomControls.ListViewEx.OLVColumn pxCountColumn;
        private System.Windows.Forms.ImageList smallImageList;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.ImageList largeImageList;
    }
}