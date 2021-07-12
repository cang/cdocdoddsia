namespace SIA.UI.Controls.UserControls
{
    partial class ucMarkerList
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnHintMarkers = new System.Windows.Forms.Button();
            this.btnDeleteAll = new System.Windows.Forms.Button();
            this.btnDeleteItem = new System.Windows.Forms.Button();
            this.btnEditItem = new System.Windows.Forms.Button();
            this.markerListView = new System.Windows.Forms.ListView();
            this.colIndex = new System.Windows.Forms.ColumnHeader();
            this.colXCentroidMicron = new System.Windows.Forms.ColumnHeader();
            this.colYCentroidMicron = new System.Windows.Forms.ColumnHeader();
            this.colXCentroidPixel = new System.Windows.Forms.ColumnHeader();
            this.colYCentroidPixel = new System.Windows.Forms.ColumnHeader();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnHintMarkers);
            this.panel1.Controls.Add(this.btnDeleteAll);
            this.panel1.Controls.Add(this.btnDeleteItem);
            this.panel1.Controls.Add(this.btnEditItem);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 288);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(420, 32);
            this.panel1.TabIndex = 0;
            // 
            // btnHintMarkers
            // 
            this.btnHintMarkers.Location = new System.Drawing.Point(3, 4);
            this.btnHintMarkers.Name = "btnHintMarkers";
            this.btnHintMarkers.Size = new System.Drawing.Size(64, 23);
            this.btnHintMarkers.TabIndex = 3;
            this.btnHintMarkers.Text = "Hint";
            this.btnHintMarkers.UseVisualStyleBackColor = true;
            this.btnHintMarkers.Click += new System.EventHandler(this.btnHintMarkers_Click);
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteAll.Location = new System.Drawing.Point(341, 4);
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.Size = new System.Drawing.Size(74, 23);
            this.btnDeleteAll.TabIndex = 2;
            this.btnDeleteAll.Text = "Delete All";
            this.btnDeleteAll.UseVisualStyleBackColor = true;
            this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
            // 
            // btnDeleteItem
            // 
            this.btnDeleteItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteItem.Location = new System.Drawing.Point(278, 4);
            this.btnDeleteItem.Name = "btnDeleteItem";
            this.btnDeleteItem.Size = new System.Drawing.Size(57, 23);
            this.btnDeleteItem.TabIndex = 1;
            this.btnDeleteItem.Text = "Delete";
            this.btnDeleteItem.UseVisualStyleBackColor = true;
            this.btnDeleteItem.Click += new System.EventHandler(this.btnDeleteItem_Click);
            // 
            // btnEditItem
            // 
            this.btnEditItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditItem.Location = new System.Drawing.Point(215, 4);
            this.btnEditItem.Name = "btnEditItem";
            this.btnEditItem.Size = new System.Drawing.Size(57, 23);
            this.btnEditItem.TabIndex = 0;
            this.btnEditItem.Text = "Edit";
            this.btnEditItem.UseVisualStyleBackColor = true;
            this.btnEditItem.Click += new System.EventHandler(this.btnEditItem_Click);
            // 
            // markerListView
            // 
            this.markerListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colIndex,
            this.colXCentroidMicron,
            this.colYCentroidMicron,
            this.colXCentroidPixel,
            this.colYCentroidPixel});
            this.markerListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.markerListView.FullRowSelect = true;
            this.markerListView.GridLines = true;            
            this.markerListView.Location = new System.Drawing.Point(0, 0);
            this.markerListView.MultiSelect = false;
            this.markerListView.Name = "markerListView";
            this.markerListView.Size = new System.Drawing.Size(420, 288);
            this.markerListView.TabIndex = 1;
            this.markerListView.UseCompatibleStateImageBehavior = false;
            this.markerListView.View = System.Windows.Forms.View.Details;
            this.markerListView.SelectedIndexChanged += new System.EventHandler(this.markerListView_SelectedIndexChanged);
            this.markerListView.DoubleClick += new System.EventHandler(this.markerListView_DoubleClick);
            // 
            // colIndex
            // 
            this.colIndex.Text = "Index";
            this.colIndex.Width = 64;
            // 
            // colXCentroidMicron
            // 
            this.colXCentroidMicron.Text = "X-Coordinate";
            this.colXCentroidMicron.Width = 81;
            // 
            // colYCentroidMicron
            // 
            this.colYCentroidMicron.Text = "Y-Coordinate";
            this.colYCentroidMicron.Width = 81;
            // 
            // colXCentroidPixel
            // 
            this.colXCentroidPixel.Text = "X-Centroid (pixel)";
            this.colXCentroidPixel.Width = 94;
            // 
            // colYCentroidPixel
            // 
            this.colYCentroidPixel.Text = "Y-Centroid (pixel)";
            this.colYCentroidPixel.Width = 93;
            // 
            // ucMarkerList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.markerListView);
            this.Controls.Add(this.panel1);
            this.Name = "ucMarkerList";
            this.Size = new System.Drawing.Size(420, 320);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView markerListView;
        private System.Windows.Forms.ColumnHeader colIndex;
        private System.Windows.Forms.ColumnHeader colXCentroidMicron;
        private System.Windows.Forms.ColumnHeader colYCentroidMicron;
        private System.Windows.Forms.ColumnHeader colXCentroidPixel;
        private System.Windows.Forms.ColumnHeader colYCentroidPixel;
        private System.Windows.Forms.Button btnDeleteItem;
        private System.Windows.Forms.Button btnEditItem;
        private System.Windows.Forms.Button btnDeleteAll;
        private System.Windows.Forms.Button btnHintMarkers;
    }
}
