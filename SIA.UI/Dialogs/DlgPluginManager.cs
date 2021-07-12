using System;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SIA.UI.Controls;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Utilities;

using SIA.Plugins;
using SIA.Plugins.Common;

namespace SIA.UI.Dialogs
{
	/// <summary>
	/// Summary description for DlgPluginManager.
	/// </summary>
	public class DlgPluginManager : DialogBase
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ColumnHeader colName;
		private System.Windows.Forms.ColumnHeader colLocation;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.ListView listView;

		private IPluginManager _pluginMgr = null;

		public DlgPluginManager(IPluginManager pluginMgr)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			_pluginMgr = pluginMgr;

			this.InitializeListView();
		}

		protected DlgPluginManager()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgPluginManager));
			this.label1 = new System.Windows.Forms.Label();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnRemove = new System.Windows.Forms.Button();
			this.listView = new System.Windows.Forms.ListView();
			this.colName = new System.Windows.Forms.ColumnHeader();
			this.colLocation = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(4, 4);
			this.label1.Name = "label1";
			this.label1.TabIndex = 0;
			this.label1.Text = "Plugins available:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnClose
			// 
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnClose.Location = new System.Drawing.Point(296, 32);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 2;
			this.btnClose.Text = "&Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(296, 60);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.TabIndex = 3;
			this.btnAdd.Text = "&Add...";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnRemove
			// 
			this.btnRemove.Location = new System.Drawing.Point(296, 88);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.TabIndex = 4;
			this.btnRemove.Text = "&Remove";
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// listView
			// 
			this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					   this.colName,
																					   this.colLocation});
			this.listView.FullRowSelect = true;
			this.listView.GridLines = true;
			this.listView.Location = new System.Drawing.Point(4, 32);
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size(288, 225);
			this.listView.TabIndex = 1;
			this.listView.View = System.Windows.Forms.View.Details;
			this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
			// 
			// colName
			// 
			this.colName.Text = "Name";
			this.colName.Width = 120;
			// 
			// colLocation
			// 
			this.colLocation.Text = "Location";
			this.colLocation.Width = 400;
			// 
			// DlgPluginManager
			// 
			this.AcceptButton = this.btnClose;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(374, 260);
			this.Controls.Add(this.listView);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.btnRemove);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DlgPluginManager";
			this.Text = "Plugins Manager";
			this.Load += new System.EventHandler(this.DlgPluginManager_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void DlgPluginManager_Load(object sender, System.EventArgs e)
		{
			colName.Width = (int)(listView.Width * 0.5F);
			colLocation.Width = 2*listView.Width;

			this.RefreshUI();
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
		
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			using (OpenFileDialog dlg = new OpenFileDialog())
			{
				try
				{
					string initialDir = Path.GetDirectoryName(this.GetType().Assembly.Location);
					dlg.InitialDirectory = initialDir;
					dlg.Title = "Add new plug in";
					dlg.Filter = "Assembly Files (*.dll)|*.dll|All Files (*.*)|*.*";
					dlg.FilterIndex = 0;
					dlg.Multiselect = true;

					if (DialogResult.OK == dlg.ShowDialog(this))
						this.LoadPlugins(dlg.FileNames);
				}
				catch (System.Exception exp)
				{
					Trace.WriteLine(exp);
				}
			}
			
		}

		private void btnRemove_Click(object sender, System.EventArgs e)
		{
			while (listView.SelectedItems.Count > 0)
			{
				ListViewItem item = listView.SelectedItems[0];
				this.UnloadPlugin(item.Tag as IPlugin);				
				item.Tag = null;
				listView.Items.Remove(item);
			}

			this.RefreshUI();
		}

		private void listView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.RefreshUI();
		}

		private void RefreshUI()
		{
			btnRemove.Enabled = listView.SelectedItems.Count > 0;			
		}

		private void InitializeListView()
		{
			// initialize list view
			foreach (IPlugin plugin in _pluginMgr.Plugins)
				this.AddListItem(plugin);
		}

		private void LoadPlugins(string[] fileNames)
		{
			using (DlgPluginLoadFailed dlg = new DlgPluginLoadFailed())
			{
				foreach (string fileName in fileNames)
				{
					try
					{
						PluginCollection plugins = this._pluginMgr.LoadPlugin(fileName, "");

						if (plugins.Count > 0)
						{
							foreach (IPlugin plugin in plugins)
								this.AddListItem(plugin);
						}
						else
						{
							dlg.AddItem(fileName);
						}
					}
					catch (System.Exception exp)
					{
						dlg.AddItem(fileName);

						Trace.WriteLine("Failed to load plugin " + fileName + "\n\nexception thrown : \n" + exp.Message);						
					}
					finally
					{

					}			
				}

				// show load result
				if (dlg.ItemCount > 0)
					dlg.ShowDialog(this);
				else
					MessageBoxEx.Info("Plugins were loaded successfully.");
			}

			this.RefreshUI();
		}

		private void UnloadPlugin(IPlugin plugin)
		{
			if (plugin == null)
				return ;
			
			// call plugin manager to unload plugin
			_pluginMgr.UnloadPlugin(plugin);
		}
		
		private void AddListItem(IPlugin plugin)
		{
			string location = plugin.GetType().Assembly.Location;
			ListViewItem item = new ListViewItem();
			item.Text = plugin.Name;
			item.Tag = plugin;
			item.SubItems.Add(location);
			listView.Items.Add(item);
		}

		private void RemoveListItem(string ID)
		{
			
		}
	}
}
