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

namespace SIA.Workbench.UserControls
{
	/// <summary>
	/// Summary description for ucScriptBuilder.
	/// </summary>
	internal class ucScriptBuilder : System.Windows.Forms.UserControl
	{
		#region internal structs and enumeration

		private enum CMD_INDEX
		{
			ADDSTEP = 0,
			REMOVESTEP,
			MOVEUP,
			MOVEDOWN
		}

		#endregion
		
		#region Windows Form Members
		
		private System.Windows.Forms.ImageList imageListMain;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ToolBar toolBarScriptBuilder;
		private System.Windows.Forms.ToolBarButton cmdAddStep;
		private System.Windows.Forms.ToolBarButton cmdRemove;
		private System.Windows.Forms.ToolBarButton cmdMoveUp;
		private System.Windows.Forms.ToolBarButton cmdMoveDown;		  

		private ScriptTable _scriptTable = null;
		
		#endregion

		#region Member Fields
		
		WorkingSpace _workingSpace = null;
		private int _selectedRowIndex = -1;

		#region Process handling		
		#endregion

		#endregion
		
		#region Constructors and Destructors

		public ucScriptBuilder()
		{
			// This call is required by the Windows.Forms Form Designer.
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

		#endregion Constructors and Destructors
		
		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ucScriptBuilder));
			this._scriptTable = new SIA.Workbench.UserControls.ScriptTable();
			this.imageListMain = new System.Windows.Forms.ImageList(this.components);
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.toolBarScriptBuilder = new System.Windows.Forms.ToolBar();
			this.cmdAddStep = new System.Windows.Forms.ToolBarButton();
			this.cmdRemove = new System.Windows.Forms.ToolBarButton();
			this.cmdMoveUp = new System.Windows.Forms.ToolBarButton();
			this.cmdMoveDown = new System.Windows.Forms.ToolBarButton();
			((System.ComponentModel.ISupportInitialize)(this._scriptTable)).BeginInit();
			this.SuspendLayout();
			// 
			// _scriptTable
			// 
			this._scriptTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this._scriptTable.FullRowSelect = true;
			this._scriptTable.GridColor = System.Drawing.SystemColors.ControlDark;
			this._scriptTable.GridLines = SiGlaz.UI.CustomControls.XPTable.Models.GridLines.Both;
			this._scriptTable.Location = new System.Drawing.Point(5, 33);
			this._scriptTable.Name = "_scriptTable";
			//this._scriptTable.NoItemsText = "";
			//this._scriptTable.SelectionStyle = SiGlaz.UI.CustomControls.XPTable.Models.SelectionStyle.Grid;
			this._scriptTable.Size = new System.Drawing.Size(650, 354);
			this._scriptTable.TabIndex = 1;
			this._scriptTable.SettingsModified += new System.EventHandler(this.table_SettingsModified);
			this._scriptTable.SelectionChanged += new SiGlaz.UI.CustomControls.XPTable.Events.SelectionEventHandler(this.tableScriptBuilder_SelectionChanged);
			this._scriptTable.CellCheckChanged += new SiGlaz.UI.CustomControls.XPTable.Events.CellCheckBoxEventHandler(this.checkBoxRemovable_CheckChanged);
			this._scriptTable.CellButtonClicked += new SiGlaz.UI.CustomControls.XPTable.Events.CellButtonEventHandler(this.buttonSettings_Clicked);
			// 
			// imageListMain
			// 
			this.imageListMain.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imageListMain.ImageSize = new System.Drawing.Size(16, 16);
			this.imageListMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMain.ImageStream")));
			this.imageListMain.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(5, 5);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(646, 4);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			// 
			// toolBarScriptBuilder
			// 
			this.toolBarScriptBuilder.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																									this.cmdAddStep,
																									this.cmdRemove,
																									this.cmdMoveUp,
																									this.cmdMoveDown});
			this.toolBarScriptBuilder.ButtonSize = new System.Drawing.Size(90, 18);
			this.toolBarScriptBuilder.DropDownArrows = true;
			this.toolBarScriptBuilder.ImageList = this.imageListMain;
			this.toolBarScriptBuilder.Location = new System.Drawing.Point(5, 5);
			this.toolBarScriptBuilder.Name = "toolBarScriptBuilder";
			this.toolBarScriptBuilder.ShowToolTips = true;
			this.toolBarScriptBuilder.Size = new System.Drawing.Size(650, 28);
			this.toolBarScriptBuilder.TabIndex = 8;
			this.toolBarScriptBuilder.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
			// 
			// cmdAddStep
			// 
			this.cmdAddStep.ImageIndex = 2;
			this.cmdAddStep.Text = "Add";
			this.cmdAddStep.ToolTipText = "Add step";
			// 
			// cmdRemove
			// 
			this.cmdRemove.ImageIndex = 3;
			this.cmdRemove.Text = "Remove";
			this.cmdRemove.ToolTipText = "Remove selected steps";
			// 
			// cmdMoveUp
			// 
			this.cmdMoveUp.ImageIndex = 0;
			this.cmdMoveUp.Text = "Move Up";
			this.cmdMoveUp.ToolTipText = "Move Up";
			// 
			// cmdMoveDown
			// 
			this.cmdMoveDown.ImageIndex = 1;
			this.cmdMoveDown.Text = "Move Down";
			this.cmdMoveDown.ToolTipText = "Move Down";
			// 
			// ucScriptBuilder
			// 
			this.Controls.Add(this.toolBarScriptBuilder);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this._scriptTable);
			this.DockPadding.All = 5;
			this.Name = "ucScriptBuilder";
			this.Size = new System.Drawing.Size(660, 392);
			((System.ComponentModel.ISupportInitialize)(this._scriptTable)).EndInit();
			this.ResumeLayout(false);

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
			// reset user interface
			this.InitToolBarStatus();
			// fill _scriptTable with data from script
			this._scriptTable.LoadFromScript(this._workingSpace.Script);
			// refresh user interface
			this.RefreshUI();
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
				return _scriptTable.StepCount;
			}
		}


		#endregion Properties

		#region Methods

		public bool ValidateStep(int index)
		{
			int numSteps = this._scriptTable.StepCount;
			if (index<0 || index >= numSteps)
			{
				MessageBoxEx.Error("Invalid process step. Index was out of range.");
				return false;
			}

			IProcessStep step = null;
			try
			{
				step = this._scriptTable.GetProcessStep(index);
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

		public ProcessStep CreateStep(int index)
		{
			ProcessStep step = this._scriptTable.GetProcessStep(index) as ProcessStep;
			
			// update step index
			if (step != null) 
				step.Index = index;

			return step;
		}


		#endregion Methods

		#region Override Methods
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			try
			{
				// add events
				this.toolBarScriptBuilder.ButtonClick += new ToolBarButtonClickEventHandler(this.ToolBarItem_Click);

				// update toolbar status
				this.InitToolBarStatus();
			}
			catch (System.Exception exp)
			{
				Trace.WriteLine(exp.Message + exp.StackTrace);
			}
			finally
			{				
			}			
		}
		#endregion Override Methods

		#region Events Handlers
		
		private void ToolBarItem_Click(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			CMD_INDEX cmd = (CMD_INDEX)toolBarScriptBuilder.Buttons.IndexOf(e.Button);

			switch (cmd)
			{
				case CMD_INDEX.ADDSTEP:
					this.DoCommandAddStep();
					break;
				case CMD_INDEX.REMOVESTEP:
					this.DoCommandRemoveSelectedSteps();
					break;
				case CMD_INDEX.MOVEUP:
					this.DoCommandMoveUp();					
					break;
				case CMD_INDEX.MOVEDOWN:
					this.DoCommandMoveDown();					
					break;				
			}		
		}

		private void table_SettingsModified(object sender, EventArgs e)
		{
			this.WorkingSpace.Modified = true;
		}

		private void tableScriptBuilder_SelectionChanged(object sender, SelectionEventArgs e)
		{									
			if (e.NewSelectedIndicies == null || e.NewSelectedIndicies.Length <= 0)
				return;
			int nRows = this._scriptTable.StepCount;
			int index = e.NewSelectedIndicies[0];
			cmdMoveUp.Enabled = index > 0;
			cmdMoveDown.Enabled = index < nRows-1;
		}


		private void buttonSettings_Clicked(object sender, CellButtonEventArgs e)
		{
			// save selected cell
			this._selectedRowIndex = e.Cell.Row.Index;
		}

		private void checkBoxRemovable_CheckChanged(object sender, CellCheckBoxEventArgs e)
		{			
			int numCheckedRow = 0;

			int nRows = this._scriptTable.StepCount;
			for (int i=0; i<nRows; i++)
			{
				if (this._scriptTable.GetCell(i, ScriptTable.colSelected).Checked)
				{
					numCheckedRow++;
					break;
				}
			}

			// refresh remove button
			cmdRemove.Enabled = numCheckedRow > 0;
		}

		
		#endregion Event Handlers
		
		#region Internal Helpers

		private void InitToolBarStatus()
		{
			cmdRemove.Enabled = false;
			cmdMoveUp.Enabled = false;
			cmdMoveDown.Enabled = false;
		}

		private void RefreshUI()
		{
			
		}

		#endregion Internal Helpers
		
		#region Commands

		#region internal
		private void DoCommandAddStep()
		{			
			ProcessStep newStep = new DetectObjectStep();
			this._scriptTable.AddProcessStep(newStep);

			// signal workspace modified 
			this.WorkingSpace.Modified = true;
		}

		private void DoCommandRemoveSelectedSteps()
		{
			// forward command to _scriptTable
			this._scriptTable.RemoveSelectedProcessSteps();

			// refresh UI
			cmdRemove.Enabled = false;

			// signal workspace modified 
			this.WorkingSpace.Modified = true;
		}

		private void DoCommandMoveUp()
		{
			// forward command to _scriptTable
			this._scriptTable.MoveSelStepUp();

			// signal workspace modified 
			this.WorkingSpace.Modified = true;
		}

		private void DoCommandMoveDown()
		{
			// forward command to _scriptTable
			this._scriptTable.MoveSelStepDown();

			// signal workspace modified 
			this.WorkingSpace.Modified = true;
		}

		#endregion internal

		#region external
		
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

		#endregion external

		#endregion Commands

		
	}
}
