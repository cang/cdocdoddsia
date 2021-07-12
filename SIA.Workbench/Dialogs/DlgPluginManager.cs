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
using SIA.UI.Controls.Automation;

using SIA.Plugins;
using SIA.Plugins.Common;

namespace SIA.Workbench.Dialogs
{
	/// <summary>
	/// Summary description for DlgPluginManager.
	/// </summary>
	internal class DlgPluginManager : DialogBase
	{
		#region Windows Form Members
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
		#endregion

		#region Internal Fields

		private ProcessStepInfo[] _externalSteps = null;

		#endregion

		#region Constructor and destructor
		
		public DlgPluginManager()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
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

		
		#endregion

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
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnClose.Location = new System.Drawing.Point(296, 32);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 2;
			this.btnClose.Text = "&Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAdd.Location = new System.Drawing.Point(296, 60);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.TabIndex = 3;
			this.btnAdd.Text = "&Add...";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnRemove
			// 
			this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRemove.Location = new System.Drawing.Point(296, 88);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.TabIndex = 4;
			this.btnRemove.Text = "&Remove";
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// listView
			// 
			this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
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
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DlgPluginManager";
			this.Text = "Plugins Manager";
			this.Load += new System.EventHandler(this.DlgPluginManager_Load);
			this.ResumeLayout(false);

		}
		#endregion

		#region Event Handlers
		private void DlgPluginManager_Load(object sender, System.EventArgs e)
		{
			
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
			listView.BeginUpdate();

			while (listView.SelectedItems.Count > 0)
			{
				ListViewItem item = listView.SelectedItems[0];
				ProcessStepInfo stepInfo = item.Tag as ProcessStepInfo;
				// unregister process step
				if (stepInfo != null)
					this.UnloadPlugin(stepInfo);
				// remove list view item out of the list
				this.RemoveListItem(item);
			}

			listView.EndUpdate();

			this.UpdateControls();
		}

		private void listView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.UpdateControls();
		}

		#endregion

		#region Override Routines

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			// initialize process step manager
			this.InitializeProcessStepManager();

			// initialize list view
			this.InitializeListView();

			// update user interface for the first time
			this.UpdateControls();
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed (e);

			// initialize list view
			this.UninitializeListView();

			// initialize process step manager
			this.UninitializeProcessStepManager();
		}

		#endregion

		#region Internal Helpers

		private void UpdateControls()
		{
			btnRemove.Enabled = listView.SelectedItems.Count > 0;			
		}

		private void InitializeListView()
		{
			// initialize list view
			listView.BeginUpdate();

			colName.Width = (int)(listView.Width * 0.5F);
			colLocation.Width = 2*listView.Width;

			foreach (ProcessStepInfo step in _externalSteps)
				this.AddListItem(step);

			listView.EndUpdate();
		}

		private void UninitializeListView()
		{
			this.listView.Items.Clear();
		}

		
		private void AddListItem(ProcessStepInfo procInfo)
		{
			string location = procInfo.Type.Assembly.Location;
			ListViewItem item = new ListViewItem();
			item.Text = procInfo.DisplayName;
			item.Tag = procInfo;
			item.SubItems.Add(location);
			listView.Items.Add(item); 
		}

		private ListViewItem GetListItemByDisplayName(string displayName)
		{
			foreach (ListViewItem item in listView.Items)
			{
				if (item.Text == displayName)
					return item;
			}
			return null;
		}

		private void RemoveListItem(ListViewItem item)
		{
			if (item == null)
				return;
			item.Tag = null;
			listView.Items.Remove(item);
		}

		private void InitializeProcessStepManager()
		{
			ArrayList result = new ArrayList();
			ProcessStepInfo[] steps = ProcessStepManager.GetRegistedProcessSteps();
			foreach (ProcessStepInfo step in steps)
			{
				if (step.IsBuiltIn == false)
					result.Add(step);
			}

			this._externalSteps = (ProcessStepInfo[])result.ToArray(typeof(ProcessStepInfo));
		}

		private void UninitializeProcessStepManager()
		{
			// empty external process steps
			this._externalSteps = null;
			
			// save modified to register
			ProcessStepManager.SaveExtendProcessSteps();

			// update script serializer
			ScriptSerializer.Update();
		}

		private void LoadPlugins(string[] fileNames)
		{
			listView.BeginUpdate();
			ArrayList steps = null;
				
			using (DlgPluginLoadFailed dlg = new DlgPluginLoadFailed())
			{
				foreach (string fileName in fileNames)
				{
					try
					{	
						steps = ProcessStepManager.LoadProcessStepWithPrompt(fileName, null);
						foreach (ProcessStepInfo stepInfo in steps)
						{
							ListViewItem item = this.GetListItemByDisplayName(stepInfo.DisplayName);
							if (item != null)
								this.RemoveListItem(item);
							this.AddListItem(stepInfo);
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

			// update registry
			ProcessStepManager.SaveExtendProcessSteps();

			// end update list view
			listView.EndUpdate();

			// update ui control status
			this.UpdateControls();
		}

		private void UnloadPlugin(ProcessStepInfo procInfo)
		{
			if (procInfo == null)
				return ;
			
			// call plugin manager to unload plugin
			ProcessStepManager.UnregistProcessType(procInfo);
		}
		
		#endregion
	}
}
