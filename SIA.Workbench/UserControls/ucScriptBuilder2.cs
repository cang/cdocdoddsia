using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;

using SIA.UI.Controls.Automation.Dialogs;
using SIA.UI.Controls.Automation.Commands;
using SIA.UI.Controls.Automation;

using SiGlaz.UI.CustomControls.XPTable;
using SiGlaz.UI.CustomControls.XPTable.Editors;
using SiGlaz.UI.CustomControls.XPTable.Models;
using SiGlaz.UI.CustomControls.XPTable.Events;

using SIA.UI.Controls;
using SIA.UI.Controls.Common;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Utilities;

using SIA.UI.Controls.Automation.Steps;
using SIA.UI.Controls.Commands;

using SIA.Plugins.Common;

using SIA.Workbench.Common;
using SiGlaz.UI.CustomControls;

namespace SIA.Workbench.UserControls
{
	/// <summary>
	/// Summary description for ucScriptBuilder2.
	/// </summary>
	internal class ucScriptBuilder2 : System.Windows.Forms.UserControl
	{
		#region Windows Form Members

		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.ContextMenu contextMenu;
		private System.Windows.Forms.MenuItem mnuCut;
		private System.Windows.Forms.MenuItem mnuCopy;
		private System.Windows.Forms.MenuItem mnuPaste;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem mnuDelete;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem mnuSettings;
		private System.ComponentModel.IContainer components;

		private ScriptBuilder scriptBuilder;
		
		#endregion
        
		#region Member Fields
		
		private WorkingSpace _workingSpace = null;

        private SIAWExplorerBarManager _explorerBarManager = null;
        private ExplorerBarContainer _explorerBarContainer = null;
        private SplitterEx _splitter = null;
		#endregion
		
		#region Constructors and Destructors

		public ucScriptBuilder2()
		{
            InitializeExplorerBar();

			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			scriptBuilder.ProcessStepInserted += new EventHandler(scriptBuilder_ProcessStepInserted);
			scriptBuilder.ProcessStepSettingsChanged += new EventHandler(scriptBuilder_ProcessStepSettingsChanged);
			scriptBuilder.SelectedProcessStepMoved += new EventHandler(scriptBuilder_SelectedProcessStepMoved);
			scriptBuilder.SelectedProcessStepsRemoved += new EventHandler(scriptBuilder_SelectedProcessStepsRemoved);
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

		#endregion Constructors and Destructors
		
		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.panel2 = new System.Windows.Forms.Panel();
            this.scriptBuilder = new SIA.Workbench.UserControls.ScriptBuilder();
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.mnuCut = new System.Windows.Forms.MenuItem();
            this.mnuCopy = new System.Windows.Forms.MenuItem();
            this.mnuPaste = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.mnuDelete = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.mnuSettings = new System.Windows.Forms.MenuItem();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.scriptBuilder);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(195, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(568, 550);
            this.panel2.TabIndex = 1;
            // 
            // scriptBuilder
            // 
            this.scriptBuilder.AllowDrop = true;
            this.scriptBuilder.ContextMenu = this.contextMenu;
            this.scriptBuilder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scriptBuilder.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.scriptBuilder.Items.AddRange(new object[] {
            "BEGIN",
            "END"});
            this.scriptBuilder.Location = new System.Drawing.Point(0, 0);
            this.scriptBuilder.Name = "scriptBuilder";
            this.scriptBuilder.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.scriptBuilder.Size = new System.Drawing.Size(568, 550);
            this.scriptBuilder.TabIndex = 0;
            // 
            // contextMenu
            // 
            this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuCut,
            this.mnuCopy,
            this.mnuPaste,
            this.menuItem4,
            this.mnuDelete,
            this.menuItem6,
            this.mnuSettings});
            this.contextMenu.Popup += new System.EventHandler(this.ContextMenu_PopUp);
            // 
            // mnuCut
            // 
            this.mnuCut.Index = 0;
            this.mnuCut.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
            this.mnuCut.Text = "&Cut";
            this.mnuCut.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // mnuCopy
            // 
            this.mnuCopy.Index = 1;
            this.mnuCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.mnuCopy.Text = "C&opy";
            this.mnuCopy.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // mnuPaste
            // 
            this.mnuPaste.Index = 2;
            this.mnuPaste.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
            this.mnuPaste.Text = "&Paste";
            this.mnuPaste.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 3;
            this.menuItem4.Text = "-";
            // 
            // mnuDelete
            // 
            this.mnuDelete.Index = 4;
            this.mnuDelete.Text = "&Delete";
            this.mnuDelete.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 5;
            this.menuItem6.Text = "-";
            // 
            // mnuSettings
            // 
            this.mnuSettings.Index = 6;
            this.mnuSettings.Text = "&Settings";
            this.mnuSettings.Click += new System.EventHandler(this.MenuItem_Click);            
            // 
            // ucScriptBuilder2
            // 
            this.Controls.Add(this.panel2);
            this.Controls.Add(_splitter);
            this.Controls.Add(_explorerBarContainer);
            this.Name = "ucScriptBuilder2";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(768, 560);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

		}

        private void InitializeExplorerBar()
        {
            _explorerBarContainer = new ExplorerBarContainer();
            _explorerBarContainer.Dock = DockStyle.Left;
            _explorerBarContainer.Width = 240;
            _explorerBarContainer.ExplorerBar.AllowDrag = true;

            if (_splitter != null)
                _splitter.Dispose();
            _splitter = new SplitterEx();
            _splitter.Dock = DockStyle.Left;

            _explorerBarManager = 
                new SIAWExplorerBarManager(_explorerBarContainer, _splitter);
        }
		#endregion

		#region Properties

		public WorkingSpace WorkingSpace
		{
			get
			{
				return _workingSpace;
			}
			set
			{
				_workingSpace = value;
				OnWorkingSpaceChanged();
			}
		}
		
		
		protected virtual void OnWorkingSpaceChanged()
		{
			this.scriptBuilder.Load(this._workingSpace.Script);
		}

		public bool IsModified
		{
			get {return _workingSpace.Modified;}
		}

		public Script Script
		{
			get {return _workingSpace.Script;}
		}

		public int StepCount
		{
			get
			{
				return scriptBuilder.StepCount;
			}
		}


		#endregion Properties

		#region Methods

		public bool ValidateStep(int stepIndex)
		{
			int numSteps = this.StepCount;
			if (stepIndex < 0 || stepIndex >= numSteps)
			{
				MessageBoxEx.Error("Invalid process step. Index was out of range.");
				return false;
			}

			IProcessStep step = null;

			try
			{
				step = this.scriptBuilder[stepIndex];
				if (step == null)
					throw new System.Exception("Internal error: step was not specified");
				
				// validate step by the current state of the working space 
				step.Validate(this.WorkingSpace);

				// if validation succeeded then add the output key of the current step
				if (step.OutputKeys != null && step.OutputKeys.Length > 0)
					this.WorkingSpace.AddKey(step.OutputKeys);
			}
			catch (System.Exception e)
			{
				Trace.WriteLine(e);

				string name = "";
				if (step != null)
					name = step.DisplayName;

				MessageBoxEx.Error( "An error has occurred at \"" + name + "\" Step : " + e.Message);
				
				return false;
			}

			return true;
		}

		public ProcessStep CreateStep(int stepIndex)
		{
			return this.scriptBuilder[stepIndex];
		}

		#endregion Methods

		#region Override Methods

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
		}

		#endregion

		#region Events Handlers
		
		
		#endregion 
		
		#region Internal Helpers

		#endregion 
		
		#region Commands

		public void DoCommandNewScript()
		{
		}

		public void DoCommandOpenScript()
		{
		}

		public void DoCommandSaveScript()
		{

		}

		public void DoCommandBuildScript()
		{	
		
		}

		public void ExportScript(string filePath)
		{
			ScriptSerializer.Export(this._workingSpace.FileName, filePath);
		}

		public void ImportScript(string filePath, string directory)
		{
			ScriptSerializer.Import(filePath, directory);
		}

		#endregion Commands

		private void scriptBuilder_ProcessStepInserted(object sender, EventArgs e)
		{
			this._workingSpace.Modified = true;
		}

		private void scriptBuilder_ProcessStepSettingsChanged(object sender, EventArgs e)
		{
			this._workingSpace.Modified = true;
		}

		private void scriptBuilder_SelectedProcessStepsRemoved(object sender, EventArgs e)
		{
			this._workingSpace.Modified = true;
		}

		private void scriptBuilder_SelectedProcessStepMoved(object sender, EventArgs e)
		{
			this._workingSpace.Modified = true;
		}

		private void MenuItem_Click(object sender, System.EventArgs e)
		{
			if (sender == mnuSettings)
				scriptBuilder.ShowStepSettings();
			else if (sender == mnuDelete)
				scriptBuilder.RemoveSelectedSteps();
			else if (sender == mnuCut)
				scriptBuilder.CutSelectedSteps();
			else if (sender == mnuCopy)
				scriptBuilder.CopySelectedSteps();
			else if (sender == mnuPaste)
				scriptBuilder.PasteStepsFromClipboard();
		}

		private void ContextMenu_PopUp(object sender, System.EventArgs e)
		{
			bool hasSelection = scriptBuilder.SelectedItems.Count > 0;
			bool isSingleSelection = scriptBuilder.SelectedItems.Count == 1;

			mnuSettings.Enabled = isSingleSelection && scriptBuilder.SelectionHasSettings;
			mnuCut.Enabled = mnuCopy.Enabled = mnuDelete.Enabled = hasSelection;
			mnuPaste.Enabled = scriptBuilder.CanPasteFromClipboard;
		}
	}
}
