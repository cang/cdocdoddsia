using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;			
using System.IO;				

using SIA.Common;
using SIA.SystemLayer;
using SIA.UI.Controls.UserControls;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Utilities;

using SIA.Common.IPLFacade;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class DlgImageProperties : DialogBase
	{
		private System.Windows.Forms.TabControl tapctr;
		private System.Windows.Forms.TabPage tapSummary;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtLocation;
		private System.Windows.Forms.TextBox txtSize;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox txtCreated;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtModified;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtAccessed;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox chbReadOnly;
		private System.Windows.Forms.CheckBox chbHidden;
		private System.Windows.Forms.CheckBox chbArchive; 
		private System.Windows.Forms.CheckBox chbSystem;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox txtImageSize;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TabPage tapFits;
		private PropertyListView listViewProperties;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnModify;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnDeleteAll;
		private System.Windows.Forms.ContextMenu ctMenu;
		private System.Windows.Forms.MenuItem mnAdd;
		private System.Windows.Forms.MenuItem mnModify;
		private System.Windows.Forms.MenuItem mnDelete;
		private System.Windows.Forms.MenuItem mnDeleteAll;
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button btnImport;

		private CommonImage _image = null;

		public DlgImageProperties(CommonImage image)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();			
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this._image = image;
			this.listViewProperties.Properties = image.Properties;

			string filePath = image.FilePath;			
			FileInfo fileInfo = new FileInfo(filePath);
			long size = fileInfo.Length;		
			float sizeInKB = fileInfo.Length / 1024.0F;
            				
			CheckAttribute(File.GetAttributes(image.FilePath));

			txtSize.Text = sizeInKB.ToString("#,###.# ") + "KB (" + size.ToString("#,###") + " Bytes)";
			txtLocation.Text = filePath;
			txtCreated.Text = fileInfo.CreationTime.ToLongDateString();
			txtAccessed.Text = fileInfo.LastAccessTime.ToLongDateString();
			txtModified.Text = fileInfo.LastWriteTime.ToLongDateString();

			txtImageSize.Text = String.Format("{0} x {1} (pixels)", image.Width, image.Height);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgImageProperties));
            this.tapctr = new System.Windows.Forms.TabControl();
            this.tapSummary = new System.Windows.Forms.TabPage();
            this.txtImageSize = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chbSystem = new System.Windows.Forms.CheckBox();
            this.chbArchive = new System.Windows.Forms.CheckBox();
            this.chbHidden = new System.Windows.Forms.CheckBox();
            this.chbReadOnly = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtAccessed = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtModified = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCreated = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tapFits = new System.Windows.Forms.TabPage();
            this.listViewProperties = new SIA.UI.Controls.UserControls.PropertyListView();
            this.ctMenu = new System.Windows.Forms.ContextMenu();
            this.mnAdd = new System.Windows.Forms.MenuItem();
            this.mnModify = new System.Windows.Forms.MenuItem();
            this.mnDelete = new System.Windows.Forms.MenuItem();
            this.mnDeleteAll = new System.Windows.Forms.MenuItem();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnModify = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnDeleteAll = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tapctr.SuspendLayout();
            this.tapSummary.SuspendLayout();
            this.tapFits.SuspendLayout();
            this.SuspendLayout();
            // 
            // tapctr
            // 
            this.tapctr.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tapctr.Controls.Add(this.tapSummary);
            this.tapctr.Controls.Add(this.tapFits);
            this.tapctr.Location = new System.Drawing.Point(8, 8);
            this.tapctr.Name = "tapctr";
            this.tapctr.SelectedIndex = 0;
            this.tapctr.Size = new System.Drawing.Size(596, 306);
            this.tapctr.TabIndex = 0;
            this.tapctr.SelectedIndexChanged += new System.EventHandler(this.tapctr_SelectedIndexChanged);
            // 
            // tapSummary
            // 
            this.tapSummary.Controls.Add(this.txtImageSize);
            this.tapSummary.Controls.Add(this.label8);
            this.tapSummary.Controls.Add(this.groupBox2);
            this.tapSummary.Controls.Add(this.chbSystem);
            this.tapSummary.Controls.Add(this.chbArchive);
            this.tapSummary.Controls.Add(this.chbHidden);
            this.tapSummary.Controls.Add(this.chbReadOnly);
            this.tapSummary.Controls.Add(this.label6);
            this.tapSummary.Controls.Add(this.txtAccessed);
            this.tapSummary.Controls.Add(this.label5);
            this.tapSummary.Controls.Add(this.txtModified);
            this.tapSummary.Controls.Add(this.label4);
            this.tapSummary.Controls.Add(this.txtCreated);
            this.tapSummary.Controls.Add(this.label3);
            this.tapSummary.Controls.Add(this.groupBox1);
            this.tapSummary.Controls.Add(this.txtSize);
            this.tapSummary.Controls.Add(this.label2);
            this.tapSummary.Controls.Add(this.txtLocation);
            this.tapSummary.Controls.Add(this.label1);
            this.tapSummary.Location = new System.Drawing.Point(4, 24);
            this.tapSummary.Name = "tapSummary";
            this.tapSummary.Size = new System.Drawing.Size(588, 278);
            this.tapSummary.TabIndex = 0;
            this.tapSummary.Text = "Summary";
            // 
            // txtImageSize
            // 
            this.txtImageSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtImageSize.Location = new System.Drawing.Point(88, 244);
            this.txtImageSize.Name = "txtImageSize";
            this.txtImageSize.ReadOnly = true;
            this.txtImageSize.Size = new System.Drawing.Size(492, 21);
            this.txtImageSize.TabIndex = 20;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(16, 244);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 23);
            this.label8.TabIndex = 19;
            this.label8.Text = "Image Size:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Location = new System.Drawing.Point(16, 232);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(572, 4);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            // 
            // chbSystem
            // 
            this.chbSystem.Enabled = false;
            this.chbSystem.Location = new System.Drawing.Point(200, 200);
            this.chbSystem.Name = "chbSystem";
            this.chbSystem.Size = new System.Drawing.Size(104, 24);
            this.chbSystem.TabIndex = 15;
            this.chbSystem.Text = "&System";
            // 
            // chbArchive
            // 
            this.chbArchive.Enabled = false;
            this.chbArchive.Location = new System.Drawing.Point(88, 200);
            this.chbArchive.Name = "chbArchive";
            this.chbArchive.Size = new System.Drawing.Size(104, 24);
            this.chbArchive.TabIndex = 14;
            this.chbArchive.Text = "Ar&chive";
            // 
            // chbHidden
            // 
            this.chbHidden.Enabled = false;
            this.chbHidden.Location = new System.Drawing.Point(200, 172);
            this.chbHidden.Name = "chbHidden";
            this.chbHidden.Size = new System.Drawing.Size(104, 24);
            this.chbHidden.TabIndex = 13;
            this.chbHidden.Text = "Hid&den";
            // 
            // chbReadOnly
            // 
            this.chbReadOnly.Enabled = false;
            this.chbReadOnly.Location = new System.Drawing.Point(88, 172);
            this.chbReadOnly.Name = "chbReadOnly";
            this.chbReadOnly.Size = new System.Drawing.Size(104, 24);
            this.chbReadOnly.TabIndex = 12;
            this.chbReadOnly.Text = "&Read only";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(16, 172);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 23);
            this.label6.TabIndex = 11;
            this.label6.Text = "Attributes:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtAccessed
            // 
            this.txtAccessed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAccessed.Location = new System.Drawing.Point(88, 144);
            this.txtAccessed.Name = "txtAccessed";
            this.txtAccessed.ReadOnly = true;
            this.txtAccessed.Size = new System.Drawing.Size(492, 21);
            this.txtAccessed.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(16, 144);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 23);
            this.label5.TabIndex = 9;
            this.label5.Text = "Accessed:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtModified
            // 
            this.txtModified.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtModified.Location = new System.Drawing.Point(88, 116);
            this.txtModified.Name = "txtModified";
            this.txtModified.ReadOnly = true;
            this.txtModified.Size = new System.Drawing.Size(492, 21);
            this.txtModified.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(16, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 23);
            this.label4.TabIndex = 7;
            this.label4.Text = "Modified:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCreated
            // 
            this.txtCreated.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCreated.Location = new System.Drawing.Point(88, 88);
            this.txtCreated.Name = "txtCreated";
            this.txtCreated.ReadOnly = true;
            this.txtCreated.Size = new System.Drawing.Size(492, 21);
            this.txtCreated.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(16, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "Created:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(16, 76);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(572, 4);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // txtSize
            // 
            this.txtSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSize.Location = new System.Drawing.Point(88, 44);
            this.txtSize.Name = "txtSize";
            this.txtSize.ReadOnly = true;
            this.txtSize.Size = new System.Drawing.Size(492, 21);
            this.txtSize.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Size:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtLocation
            // 
            this.txtLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLocation.Location = new System.Drawing.Point(88, 16);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.ReadOnly = true;
            this.txtLocation.Size = new System.Drawing.Size(492, 21);
            this.txtLocation.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Location:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tapFits
            // 
            this.tapFits.Controls.Add(this.listViewProperties);
            this.tapFits.Controls.Add(this.btnAdd);
            this.tapFits.Controls.Add(this.btnModify);
            this.tapFits.Controls.Add(this.btnDelete);
            this.tapFits.Controls.Add(this.btnDeleteAll);
            this.tapFits.Controls.Add(this.btnImport);
            this.tapFits.Location = new System.Drawing.Point(4, 24);
            this.tapFits.Name = "tapFits";
            this.tapFits.Size = new System.Drawing.Size(588, 278);
            this.tapFits.TabIndex = 2;
            this.tapFits.Text = "Header information";
            // 
            // listViewProperties
            // 
            this.listViewProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewProperties.ContextMenu = this.ctMenu;
            this.listViewProperties.FullRowSelect = true;
            this.listViewProperties.GridLines = true;
            this.listViewProperties.Location = new System.Drawing.Point(8, 8);
            this.listViewProperties.Name = "listViewProperties";
            this.listViewProperties.Properties = null;
            this.listViewProperties.Size = new System.Drawing.Size(576, 225);
            this.listViewProperties.TabIndex = 0;
            this.listViewProperties.UseCompatibleStateImageBehavior = false;
            this.listViewProperties.View = System.Windows.Forms.View.Details;
            this.listViewProperties.SelectedIndexChanged += new System.EventHandler(this.listViewProperties_SelectedIndexChanged);
            this.listViewProperties.DoubleClick += new System.EventHandler(this.listViewProperties_DoubleClick);
            // 
            // ctMenu
            // 
            this.ctMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnAdd,
            this.mnModify,
            this.mnDelete,
            this.mnDeleteAll});
            // 
            // mnAdd
            // 
            this.mnAdd.Index = 0;
            this.mnAdd.Text = "&Add";
            this.mnAdd.Click += new System.EventHandler(this.mnAdd_Click);
            // 
            // mnModify
            // 
            this.mnModify.Index = 1;
            this.mnModify.Text = "&Modify";
            this.mnModify.Click += new System.EventHandler(this.mnModify_Click);
            // 
            // mnDelete
            // 
            this.mnDelete.Index = 2;
            this.mnDelete.Text = "&Delete";
            this.mnDelete.Click += new System.EventHandler(this.mnDelete_Click);
            // 
            // mnDeleteAll
            // 
            this.mnDeleteAll.Index = 3;
            this.mnDeleteAll.Text = "&Delete all";
            this.mnDeleteAll.Click += new System.EventHandler(this.mnDeleteAll_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(8, 242);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 24);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "&Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnModify
            // 
            this.btnModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnModify.Enabled = false;
            this.btnModify.Location = new System.Drawing.Point(90, 242);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(75, 24);
            this.btnModify.TabIndex = 3;
            this.btnModify.Text = "&Modify";
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(172, 242);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 24);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteAll.Enabled = false;
            this.btnDeleteAll.Location = new System.Drawing.Point(254, 242);
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.Size = new System.Drawing.Size(75, 24);
            this.btnDeleteAll.TabIndex = 5;
            this.btnDeleteAll.Text = "Delete &All";
            this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(504, 242);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 24);
            this.btnImport.TabIndex = 5;
            this.btnImport.Text = "&Import";
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(442, 322);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(530, 322);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            // 
            // DlgImageProperties
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(612, 352);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tapctr);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DlgImageProperties";
            this.Text = "Image Properties";
            this.tapctr.ResumeLayout(false);
            this.tapSummary.ResumeLayout(false);
            this.tapSummary.PerformLayout();
            this.tapFits.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region UI Commnad

		private void AddNewKey()
		{
			using (DlgCustomHeaderInfo dlgCustom= new DlgCustomHeaderInfo())
			{
				if (dlgCustom.ShowDialog(this) == DialogResult.OK)
				{
					RasterImagePropertyItem property = dlgCustom.Property;
					string key = property.Key;
					int dataType = property.DataType;
					string value = "";
					if (property.Value != null)
						value = property.Value.ToString();
					string comment = property.Comment;

					if (!CheckInfo(key))
					{
						ListViewItem newItem = new ListViewItem(new string[] {key, GetType(dataType), value, comment});
						newItem.Tag = property;
						listViewProperties.Items.Add(newItem);
					}
					else
						MessageBoxEx.Error("The name '{0}' is exist.", key);
				}
			}
		}
		

		private void ModifyKey()
		{
			if (listViewProperties.SelectedItems.Count > 0)
			{
				ListViewItem item = listViewProperties.SelectedItems[0];
				RasterImagePropertyItem property = (RasterImagePropertyItem)item.Tag;
				using (DlgCustomHeaderInfo dlgCustom= new DlgCustomHeaderInfo(property))
				{
					if (dlgCustom.ShowDialog(this) == DialogResult.OK)
					{			
						property = dlgCustom.Property;
						string key = property.Key;
						int dataType = property.DataType;
						string value = "";
						if (property.Value != null)
							value = property.Value.ToString();
						string comment = property.Comment;					

						if (!CheckInfoModify(key))
						{
							item.Text = key;
							item.SubItems[1].Text = GetType(dataType);
							item.SubItems[2].Text = value;
							item.SubItems[3].Text = comment;
							item.Tag = property;					
						}
						else
							MessageBoxEx.Error("The name '{0}' is exist.", key);
					}
				}
			}
		}


		private void RemoveSelectedKeys()
		{
			if (MessageBoxEx.ConfirmYesNo("Are you sure you want to permanently delete the selected item(s) ?"))
			{
				foreach (ListViewItem item in listViewProperties.SelectedItems)
					listViewProperties.Items.Remove(item);
			}	
		}


		private void RemoveAllKeys()
		{
			if (MessageBoxEx.ConfirmYesNo("Are you sure you want to permanently delete all item(s) ?"))
			{
				foreach (ListViewItem item in listViewProperties.Items)
				{
					//if (item.Text=="SIMPLE" || item.Text=="BITPIX" || item.Text=="NAXIS"||item.Text=="NAXIS1"||item.Text=="NAXIS2"||item.Text=="BSCALE")
					RasterImagePropertyItem prop = item.Tag as RasterImagePropertyItem;
					//if(!(item.Text=="SIMPLE" || item.Text=="BITPIX" || item.Text=="NAXIS"||item.Text=="NAXIS1"||item.Text=="NAXIS2"||item.Text=="BSCALE"))					
					if (prop != null && prop.ReadOnly == false)
						listViewProperties.Items.Remove(item);
				}
			}	
		}

		private bool CheckInfo(string name)
		{			
			foreach (ListViewItem item in listViewProperties.Items)
			{
				if (item.Text == name)				
					return true;				
			}
			return false;
		}


		private bool CheckInfoModify(string name)
		{
			ListViewItem itemSelect = listViewProperties.SelectedItems[0];
			foreach (ListViewItem item in listViewProperties.Items)
			{
				if (item.Text == name && item.Text != itemSelect.Text)				
					return true;				
			}
			return false;
		}

		
		private void SetTapSummary()
		{
			
		}


		public int GetType(string datatype)
		{

			switch(datatype)       
			{         
				case "Byte":   
					return 11;					
				case "Boolean":            
					return 14;
				case "Short":            
					return 21;
				case "UShort":            
					return 20;
				case "UInt":
					return 30;
				case "Int":
					return 31;
				case "ULong":
					return 40;
				case "Long":
					return 41;
				case "Float":
					return 42;
				case "Double":
					return 82;
				case "String":
					return 16;
				default:            
					return -1;
			}		

		}


		public string GetType(int datavalue)
		{

			switch(datavalue)       
			{         
				case 11:   
					return "Byte";					
				case 14:            
					return "Boolean";
				case 20:            
					return "UShort";
				case 30:
					return "UInt";
				case 31:
					return "Int";
				case 40:
					return "ULong";
				case 41:
					return "Long";
				case 42:
					return "Float";
				case 82:
					return "Double";
				case 16:
					return "String";
				case 21:            
					return "Short";
				default:            
					return string.Empty;
			}		

		}


		private void CheckAttribute(FileAttributes fileAtri)
		{
			switch (fileAtri.ToString())
			{
				case "Archive":
					chbArchive.Checked=true;
					break;
				case "System":
					chbSystem.Checked=true;
					break;
				case "Hidden":
					chbHidden.Checked=true;
					break;
				case "ReadOnly":
					chbReadOnly.Checked=true;
					break;
			}
		}


		#endregion

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			listViewProperties.UpdateData(true);
			RasterImagePropertyCollection properties = listViewProperties.Properties;
			
			this._image.Properties.Clear();
			foreach (RasterImagePropertyItem property in properties)
				this._image.Properties.Add(property);
			this._image.Modified = true;
		}


		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}


		private void tapctr_SelectedIndexChanged(object sender, System.EventArgs e)
		{			
			if (tapctr.SelectedIndex==1)
			{
				btnAdd.Enabled=true;
				btnDelete.Enabled=true;
				btnDeleteAll.Enabled=true;
				btnModify.Enabled=true;
			}
			else
			{
				btnAdd.Enabled=false;
				btnDelete.Enabled=false;
				btnDeleteAll.Enabled=false;
				btnModify.Enabled=false;
			}
		}


		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			RemoveSelectedKeys();
		}


		private void btnDeleteAll_Click(object sender, System.EventArgs e)
		{
			RemoveAllKeys();
		}


		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			AddNewKey();
		}

		
		private void btnModify_Click(object sender, System.EventArgs e)
		{
			ModifyKey();
		}


		private void mnAdd_Click(object sender, System.EventArgs e)
		{
			AddNewKey();
		}


		private void mnModify_Click(object sender, System.EventArgs e)
		{
			ModifyKey();	
		}


		private void mnDelete_Click(object sender, System.EventArgs e)
		{
			RemoveSelectedKeys();
		}


		private void mnDeleteAll_Click(object sender, System.EventArgs e)
		{
			RemoveAllKeys();
		}

		private void btnImport_Click(object sender, System.EventArgs e)
		{
			using (OpenFileDialog dlg = CommonDialogs.OpenImageFileDialog("Select a FITS file...", CommonDialogs.ImageFileFilter.FITImage))
			{
				if (DialogResult.OK == dlg.ShowDialog(this))
				{
					try
					{
						using (CommonImage image = CommonImage.FromFile(dlg.FileName))
						{
							if (image.Properties.Count <= 0)
							{
								MessageBoxEx.Error("The image does not contain header information.");
							}
							else
							{
                                RasterImagePropertyCollection properties = this.listViewProperties.Properties.Clone() as RasterImagePropertyCollection;

								foreach (RasterImagePropertyItem prop in image.Properties)
								{
									bool addProp = true;

                                    if (properties[prop.Key] != null)
									{
										addProp = MessageBoxEx.ConfirmYesNo(String.Format("The item {0} is already exists. " +
											"Do you want to override the existing values?", prop.Key));
									}

									if (addProp)
									{
                                        IImagePropertyItem oldProp = properties[prop.Key];
										if ( oldProp != null )
                                            properties.Remove(oldProp);
                                        properties.Add(prop);
									}
								}

								// update list view properties
                                this.listViewProperties.Properties = properties;

                                // clean up temporary properties
                                properties.Clear();
							}
						}
					}
					catch (Exception exp)
					{
						MessageBoxEx.Error("Failed to import header from FITS file: " + exp.Message);
					}
				}
			}
		}	

		private void listViewProperties_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (listViewProperties.SelectedItems.Count > 0)
			{
				ListViewItem item = listViewProperties.SelectedItems[0];
				//if (item.Text=="SIMPLE" || item.Text=="BITPIX" || item.Text=="NAXIS"||item.Text=="NAXIS1"||item.Text=="NAXIS2"||item.Text=="BSCALE")
				RasterImagePropertyItem prop = item.Tag as RasterImagePropertyItem;
				if (prop != null)
				{
					if (prop.ReadOnly)
					{
						btnModify.Enabled = mnModify.Enabled = false;
						btnDelete.Enabled = mnDelete.Enabled = false;
					}
					else
					{
						btnModify.Enabled = mnModify.Enabled = true;
						btnDelete.Enabled = mnDelete.Enabled = true;
					}
				}
			}			
		}

		private void listViewProperties_DoubleClick(object sender, System.EventArgs e)
		{
			if (listViewProperties.SelectedItems.Count > 0)
			{
				ListViewItem item = listViewProperties.SelectedItems[0];
				RasterImagePropertyItem prop = item.Tag as RasterImagePropertyItem;
				if (prop != null && prop.ReadOnly == false)
					this.ModifyKey();
			}		
		}

		
	}
}

