using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using SIA.UI.Components;

using SIA.UI.Controls;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Utilities;

using SIA.UI;

namespace SIA.UI.Dialogs
{
	/// <summary>
	/// Summary description for DlgHistory.
	/// </summary>
	internal class DlgHistory : FloatingFormBase
	{
		private HistoryListBox historyListBox;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ContextMenu contextMenu;
		private System.Windows.Forms.MenuItem mnuRestore;
		private System.Windows.Forms.MenuItem mnuSep01;
		private System.Windows.Forms.MenuItem mnuQuickView;

		private MainFrame _appWorkspace;

		public event EventHandler Restore = null;
		public event EventHandler QuickView = null;

		public MainFrame appWorkspace
		{
			get {return _appWorkspace;}
		}

		public HistoryListBox HistoryListBox
		{
			get {return historyListBox;}
		}

		public DlgHistory(MainFrame appWorkspace)
		{
			this._appWorkspace = appWorkspace;

			//
			// Required for Windows Form Designer support
			//
            InitializeComponent();
                     
            // register for events
            _appWorkspace.Load += new EventHandler(appWorkspace_Load);
            
            ImageWorkspace workspace = _appWorkspace.DocumentWorkspace as ImageWorkspace;
            workspace.WorkspaceCreated += new EventHandler(Workspace_WorkspaceCreated);
            workspace.WorkspaceDestroyed += new EventHandler(Workspace_WorkspaceDestroyed);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
            if (this._appWorkspace != null)
            {
                _appWorkspace.Load -= new EventHandler(appWorkspace_Load);

                ImageWorkspace workspace = _appWorkspace.DocumentWorkspace as ImageWorkspace;
                if (workspace != null)
                {
                    workspace.WorkspaceCreated -= new EventHandler(Workspace_WorkspaceCreated);
                    workspace.WorkspaceDestroyed -= new EventHandler(Workspace_WorkspaceDestroyed);
                }
            }

            this._appWorkspace = null;

            if (disposing)
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgHistory));
			this.historyListBox = new SIA.UI.Components.HistoryListBox();
			this.contextMenu = new System.Windows.Forms.ContextMenu();
			this.mnuRestore = new System.Windows.Forms.MenuItem();
			this.mnuSep01 = new System.Windows.Forms.MenuItem();
			this.mnuQuickView = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// historyListBox
			// 
			this.historyListBox.ContextMenu = this.contextMenu;
			this.historyListBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.historyListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.historyListBox.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.historyListBox.HistoryHelper = null;
			this.historyListBox.Location = new System.Drawing.Point(0, 0);
			this.historyListBox.Name = "historyListBox";
			this.historyListBox.Padding = 2;
			this.historyListBox.SelectedItemBackColor = System.Drawing.Color.Blue;
			this.historyListBox.SelectedItemFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.historyListBox.SelectedItemForeColor = System.Drawing.Color.White;
			this.historyListBox.Size = new System.Drawing.Size(232, 326);
			this.historyListBox.SkipItemFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.historyListBox.SkipItemForeColor = System.Drawing.Color.Gray;
			this.historyListBox.TabIndex = 0;
			this.historyListBox.DoubleClick += new System.EventHandler(this.historyListBox_DoubleClick);
			// 
			// contextMenu
			// 
			this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.mnuRestore,
																						this.mnuSep01,
																						this.mnuQuickView});
			// 
			// mnuRestore
			// 
			this.mnuRestore.Index = 0;
			this.mnuRestore.Text = "&Restore";
			this.mnuRestore.Click += new System.EventHandler(this.mnuRestore_Click);
			// 
			// mnuSep01
			// 
			this.mnuSep01.Index = 1;
			this.mnuSep01.Text = "-";
			// 
			// mnuQuickView
			// 
			this.mnuQuickView.Index = 2;
			this.mnuQuickView.Text = "&Quick View";
			this.mnuQuickView.Click += new System.EventHandler(this.mnuQuickView_Click);
			// 
			// DlgHistory
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(232, 326);
			this.Controls.Add(this.historyListBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DlgHistory";
			this.ShowInTaskbar = false;
			this.Text = "History";
			this.ResumeLayout(false);

		}
		#endregion

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

            // Cong commented:
            this.historyListBox.ContextMenu = null;
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			e.Cancel = true;
			this.Visible = false;

			base.OnClosing (e);
		}


		private void mnuRestore_Click(object sender, System.EventArgs e)
		{
			if (this.Restore != null)
				this.Restore(this, EventArgs.Empty);
		}

		private void historyListBox_DoubleClick(object sender, System.EventArgs e)
		{
			if (this.Restore != null)
				this.Restore(this, EventArgs.Empty);
		}

		private void mnuQuickView_Click(object sender, System.EventArgs e)
		{
			if (this.QuickView != null)
				this.QuickView(this, EventArgs.Empty);
		}

		private void appWorkspace_Load(object sender, EventArgs e)
		{
			// reset window screen layout
			this.ResetScreenLayout();

			// show me
			this.Show();


            // invisible me :-)
            this.Close();
		}

        private void Workspace_WorkspaceCreated(object sender, EventArgs e)
        {
            ImageWorkspace workspace = this._appWorkspace.DocumentWorkspace as ImageWorkspace;
            this.historyListBox.HistoryHelper = workspace.HistoryHelper;
        }

        private void Workspace_WorkspaceDestroyed(object sender, EventArgs e)
        {
            this.historyListBox.HistoryHelper = null;
        }

		public void ResetScreenLayout()
		{
            ImageWorkspace workspace = this.appWorkspace.DocumentWorkspace as ImageWorkspace;
            int offset = 5;
			int top = workspace.Top;
			int right = workspace.Right;

			Point pt = workspace.PointToScreen(new Point(right, top));
			
			int thisWidth = this.Width;
			int thisHeight = this.Height;

			this.StartPosition = FormStartPosition.Manual;
			this.Left = pt.X - this.Width - offset;
			this.Top = pt.Y + offset;
		}
	}
}
