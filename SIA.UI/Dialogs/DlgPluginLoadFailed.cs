using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SIA.UI.Controls;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Utilities;
using SIA.Plugins;

namespace SIA.UI.Dialogs
{
	/// <summary>
	/// Summary description for DlgPluginLoadFailed.
	/// </summary>
	public class DlgPluginLoadFailed : DialogBase
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ColumnHeader colLocation;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.ListView listView;

		public DlgPluginLoadFailed()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgPluginLoadFailed));
			this.label1 = new System.Windows.Forms.Label();
			this.btnClose = new System.Windows.Forms.Button();
			this.listView = new System.Windows.Forms.ListView();
			this.colLocation = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(4, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(364, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "The following plugin(s) failed to load";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnClose
			// 
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnClose.Location = new System.Drawing.Point(296, 164);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 2;
			this.btnClose.Text = "&Continue";
			// 
			// listView
			// 
			this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					   this.colLocation});
			this.listView.FullRowSelect = true;
			this.listView.GridLines = true;
			this.listView.Location = new System.Drawing.Point(3, 32);
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size(368, 128);
			this.listView.TabIndex = 1;
			this.listView.View = System.Windows.Forms.View.Details;
			// 
			// colLocation
			// 
			this.colLocation.Text = "Location";
			this.colLocation.Width = 400;
			// 
			// DlgPluginLoadFailed
			// 
			this.AcceptButton = this.btnClose;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(374, 192);
			this.Controls.Add(this.listView);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DlgPluginLoadFailed";
			this.Text = "Plugins";
			this.Load += new System.EventHandler(this.DlgPluginManager_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void DlgPluginManager_Load(object sender, System.EventArgs e)
		{
			colLocation.Width = listView.Width;
		}

		public int ItemCount
		{
			get {return listView.Items.Count;}
		}

		public void AddItem(string location)
		{
			listView.Items.Add(location);
		}
	}
}
