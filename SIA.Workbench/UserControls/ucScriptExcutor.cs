using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Reflection;

using SiGlaz.UI.CustomControls.XPTable;
using SiGlaz.UI.CustomControls.XPTable.Editors;
using SiGlaz.UI.CustomControls.XPTable.Models;
using SiGlaz.UI.CustomControls.XPTable.Events;

using SIA.SystemFrameworks.UI;

using SIA.Workbench.Common;
using SIA.Workbench.Common.InterprocessCommunication.SharedMemory;

using SIA.UI.Controls;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Automation;

using SIA.Workbench.Dialogs;


namespace SIA.Workbench.UserControls 
{
	/// <summary>
	/// Summary description for ucScriptExecutor.
	/// </summary>
	internal class ucScriptExecutor : System.Windows.Forms.UserControl 
	{
		#region  Window Member fields		
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Label labelProcessingScript;
		private System.Windows.Forms.Label labelProcessingFile;
		private System.Windows.Forms.Panel panel3;		
		private System.Windows.Forms.GroupBox groupBox1;		
		private System.Windows.Forms.Splitter splitter1;			
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TextBox txtProcessingScript;
		private System.Windows.Forms.TextBox txtProcessingFile;		
		private SIA.Workbench.Components.RichTextBoxEx  richTextLogs;
		private SIA.Workbench.ProgressBar _scriptProgressBar;
		private SIA.Workbench.ProgressBar _fileProgressBar;		  	
		
		#endregion Window Member fields

		#region Member fields

		// table control
		private StatusTable _statusTable;

		private bool bCanRun = false;
		private bool bCanStop = false;
		
		private int _iProcessingStep = 0;				
		private WorkingSpace _workingSpace = null;
		private string _processingFileName = string.Empty;

		private object _syncObject = new object();
		private AutoResetEvent _statusChangeWaitHandle = new AutoResetEvent(true);
		
		private int _clearBufferStep = 1024;
		private int _maxLogBuffer = 32768;

		private string _currentSubStep = string.Empty;
		private SortedList _incomingFiles = new SortedList();
		private int _counterIncomingFilesInFolder = 0;
		private int _totalIncomingFiles = 0;	
			
		#endregion Member fields

		#region Constructors and Destructors
		public ucScriptExecutor() 
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
			this.panel4 = new System.Windows.Forms.Panel();
			this.txtProcessingScript = new System.Windows.Forms.TextBox();
			this._fileProgressBar = new SIA.Workbench.ProgressBar();
			this._scriptProgressBar = new SIA.Workbench.ProgressBar();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this._statusTable = new SIA.Workbench.UserControls.StatusTable();
			this.labelProcessingScript = new System.Windows.Forms.Label();
			this.labelProcessingFile = new System.Windows.Forms.Label();
			this.txtProcessingFile = new System.Windows.Forms.TextBox();
			this.panel3 = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.richTextLogs = new SIA.Workbench.Components.RichTextBoxEx();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._statusTable)).BeginInit();
			this.panel3.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.txtProcessingScript);
			this.panel4.Controls.Add(this._fileProgressBar);
			this.panel4.Controls.Add(this._scriptProgressBar);
			this.panel4.Controls.Add(this.groupBox2);
			this.panel4.Controls.Add(this._statusTable);
			this.panel4.Controls.Add(this.labelProcessingScript);
			this.panel4.Controls.Add(this.labelProcessingFile);
			this.panel4.Controls.Add(this.txtProcessingFile);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel4.Location = new System.Drawing.Point(0, 0);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(670, 282);
			this.panel4.TabIndex = 16;
			// 
			// txtProcessingScript
			// 
			this.txtProcessingScript.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtProcessingScript.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtProcessingScript.Location = new System.Drawing.Point(104, 11);
			this.txtProcessingScript.Name = "txtProcessingScript";
			this.txtProcessingScript.ReadOnly = true;
			this.txtProcessingScript.Size = new System.Drawing.Size(560, 13);
			this.txtProcessingScript.TabIndex = 1;
			this.txtProcessingScript.Text = "";
			// 
			// _fileProgressBar
			// 
			this._fileProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this._fileProgressBar.BarOffset = 1;
			this._fileProgressBar.Caption = "Progress";
			this._fileProgressBar.CaptionColor = System.Drawing.Color.Black;
			this._fileProgressBar.CaptionMode = SIA.Workbench.ProgressCaptionMode.Percent;
			this._fileProgressBar.CaptionShadowColor = System.Drawing.Color.White;
			this._fileProgressBar.ChangeByMouse = false;
			this._fileProgressBar.DashSpace = 2;
			this._fileProgressBar.DashWidth = 5;
			this._fileProgressBar.Edge = SIA.Workbench.ProgressBarEdge.Rounded;
			this._fileProgressBar.EdgeColor = System.Drawing.Color.Gray;
			this._fileProgressBar.EdgeLightColor = System.Drawing.Color.LightGray;
			this._fileProgressBar.EdgeWidth = 1;
			this._fileProgressBar.FloodPercentage = 0.2F;
			this._fileProgressBar.FloodStyle = SIA.Workbench.ProgressFloodStyle.Standard;
			this._fileProgressBar.Invert = false;
			this._fileProgressBar.Location = new System.Drawing.Point(4, 80);
			this._fileProgressBar.MainColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(192)), ((System.Byte)(0)));
			this._fileProgressBar.Maximum = 100;
			this._fileProgressBar.Minimum = 0;
			this._fileProgressBar.Name = "_fileProgressBar";
			this._fileProgressBar.Orientation = SIA.Workbench.ProgressBarDirection.Horizontal;
			this._fileProgressBar.ProgressBackColor = System.Drawing.Color.White;
			this._fileProgressBar.ProgressBarStyle = SIA.Workbench.ProgressStyle.Dashed;
			this._fileProgressBar.SecondColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(192)), ((System.Byte)(0)));
			this._fileProgressBar.Shadow = true;
			this._fileProgressBar.ShadowOffset = 1;
			this._fileProgressBar.Size = new System.Drawing.Size(662, 16);
			this._fileProgressBar.Step = 1;
			this._fileProgressBar.TabIndex = 5;
			this._fileProgressBar.TextAntialias = true;
			this._fileProgressBar.Value = 0;
			// 
			// _scriptProgressBar
			// 
			this._scriptProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this._scriptProgressBar.BarOffset = 1;
			this._scriptProgressBar.Caption = "Progress";
			this._scriptProgressBar.CaptionColor = System.Drawing.Color.Black;
			this._scriptProgressBar.CaptionMode = SIA.Workbench.ProgressCaptionMode.Percent;
			this._scriptProgressBar.CaptionShadowColor = System.Drawing.Color.White;
			this._scriptProgressBar.ChangeByMouse = false;
			this._scriptProgressBar.DashSpace = 2;
			this._scriptProgressBar.DashWidth = 5;
			this._scriptProgressBar.Edge = SIA.Workbench.ProgressBarEdge.Rounded;
			this._scriptProgressBar.EdgeColor = System.Drawing.Color.Gray;
			this._scriptProgressBar.EdgeLightColor = System.Drawing.Color.LightGray;
			this._scriptProgressBar.EdgeWidth = 1;
			this._scriptProgressBar.FloodPercentage = 0.2F;
			this._scriptProgressBar.FloodStyle = SIA.Workbench.ProgressFloodStyle.Standard;
			this._scriptProgressBar.Invert = false;
			this._scriptProgressBar.Location = new System.Drawing.Point(4, 32);
			this._scriptProgressBar.MainColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(192)), ((System.Byte)(0)));
			this._scriptProgressBar.Maximum = 100;
			this._scriptProgressBar.Minimum = 0;
			this._scriptProgressBar.Name = "_scriptProgressBar";
			this._scriptProgressBar.Orientation = SIA.Workbench.ProgressBarDirection.Horizontal;
			this._scriptProgressBar.ProgressBackColor = System.Drawing.Color.White;
			this._scriptProgressBar.ProgressBarStyle = SIA.Workbench.ProgressStyle.Dashed;
			this._scriptProgressBar.SecondColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(192)), ((System.Byte)(0)));
			this._scriptProgressBar.Shadow = true;
			this._scriptProgressBar.ShadowOffset = 1;
			this._scriptProgressBar.Size = new System.Drawing.Size(662, 16);
			this._scriptProgressBar.Step = 1;
			this._scriptProgressBar.TabIndex = 2;
			this._scriptProgressBar.TextAntialias = true;
			this._scriptProgressBar.Value = 0;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Location = new System.Drawing.Point(0, 0);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(912, 4);
			this.groupBox2.TabIndex = 0;
			this.groupBox2.TabStop = false;
			// 
			// _statusTable
			// 
			this._statusTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this._statusTable.FullRowSelect = true;
			this._statusTable.GridColor = System.Drawing.SystemColors.ControlDark;
			this._statusTable.GridLines = SiGlaz.UI.CustomControls.XPTable.Models.GridLines.Both;
			this._statusTable.Location = new System.Drawing.Point(4, 100);
			this._statusTable.Name = "_statusTable";
			this._statusTable.NoItemsText = "";
			this._statusTable.SelectionStyle = SiGlaz.UI.CustomControls.XPTable.Models.SelectionStyle.Grid;
			this._statusTable.Size = new System.Drawing.Size(662, 172);
			this._statusTable.TabIndex = 6;
			// 
			// labelProcessingScript
			// 
			this.labelProcessingScript.Location = new System.Drawing.Point(4, 12);
			this.labelProcessingScript.Name = "labelProcessingScript";
			this.labelProcessingScript.Size = new System.Drawing.Size(96, 20);
			this.labelProcessingScript.TabIndex = 0;
			this.labelProcessingScript.Text = "Processing Script:";
			// 
			// labelProcessingFile
			// 
			this.labelProcessingFile.Location = new System.Drawing.Point(4, 56);
			this.labelProcessingFile.Name = "labelProcessingFile";
			this.labelProcessingFile.Size = new System.Drawing.Size(96, 20);
			this.labelProcessingFile.TabIndex = 3;
			this.labelProcessingFile.Text = "Processing File:";
			// 
			// txtProcessingFile
			// 
			this.txtProcessingFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtProcessingFile.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtProcessingFile.Location = new System.Drawing.Point(104, 56);
			this.txtProcessingFile.Name = "txtProcessingFile";
			this.txtProcessingFile.ReadOnly = true;
			this.txtProcessingFile.Size = new System.Drawing.Size(560, 13);
			this.txtProcessingFile.TabIndex = 4;
			this.txtProcessingFile.Text = "";
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.panel1);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 282);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(670, 168);
			this.panel3.TabIndex = 15;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.richTextLogs);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(670, 168);
			this.panel1.TabIndex = 9;
			// 
			// richTextLogs
			// 
			this.richTextLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.richTextLogs.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.richTextLogs.Location = new System.Drawing.Point(4, 0);
			this.richTextLogs.MaxLength = 200;
			this.richTextLogs.Name = "richTextLogs";
			this.richTextLogs.Size = new System.Drawing.Size(660, 164);
			this.richTextLogs.TabIndex = 2;
			this.richTextLogs.Text = "";
			this.richTextLogs.WordWrap = false;
			this.richTextLogs.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.Log_LinkClicked);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(0, -80);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(662, 8);
			this.groupBox1.TabIndex = 14;
			this.groupBox1.TabStop = false;
			// 
			// splitter1
			// 
			this.splitter1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter1.Location = new System.Drawing.Point(0, 279);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(670, 3);
			this.splitter1.TabIndex = 0;
			this.splitter1.TabStop = false;
			// 
			// ucScriptExecutor
			// 
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.panel4);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.groupBox1);
			this.Name = "ucScriptExecutor";
			this.Size = new System.Drawing.Size(670, 450);
			this.panel4.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._statusTable)).EndInit();
			this.panel3.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
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

		public string ProcessingScript 
		{
			get 
			{
				return this._workingSpace.FileName;
			}

			set 
			{
				//_processingScriptFileName = value;				
				this._workingSpace.FileName = value;

				txtProcessingScript.Text =  this._workingSpace.FileName;
			}
		}

		public string TemporaryScriptFileName 
		{
			get 
			{
				return Application.StartupPath + "tempScript.xml";
			}
		}

		public string ProcessingFile 
		{
			get 
			{
				return _processingFileName;
			}
			set 
			{
				_processingFileName = value;
				txtProcessingFile.Text = _processingFileName;
			}
		}

		public bool CanRun 
		{
			get 
			{
				return bCanRun;
			}
			set 
			{
				bCanRun = value;				
			}
		}

		public bool CanStop 
		{
			get 
			{
				return bCanStop;
			}
			set 
			{
				bCanStop = value;				
			}
		}

		public bool SwitchToRun 
		{
			set 
			{
				this.CanRun = value;
				this.CanStop = !this.CanRun;
			}
		}

		public bool TurnOff 
		{
			set 
			{
				this.CanRun = false;
				this.CanStop = false;
			}
		}
		
		public string Logs 
		{
			get 
			{
				return richTextLogs.Text;
			}
		}

		public int NumberOfSteps 
		{
			get 
			{
				return this._statusTable.StepCount;
			}
		}

		public int ProcessingStep 
		{
			get 
			{
				return this._iProcessingStep;
			}
			set 
			{
				this._iProcessingStep = value;
			}
		}

		public Script Script 
		{
			get 
			{				
				return this._workingSpace.Script;
			}
		}

		public int OverralProgress 
		{
			get 
			{
				return this._scriptProgressBar.Value;
			}

			set 
			{
				if (this._scriptProgressBar.Value != value)
					this._scriptProgressBar.Value = value;
			}
		}

		public int ProcessingFileProgress 
		{
			get 
			{
				return this._fileProgressBar.Value;
			}

			set 
			{
				if (this._fileProgressBar.Value != value)
					this._fileProgressBar.Value = value;
			}
		}

		public string CurrentSubStep 
		{
			get 
			{
				return _currentSubStep;
			}
		}
		
		#endregion Properties

		#region Methods	
		
		public void DoCommandOpenScriptFile() 
		{			
		}

		public void DoCommandSaveLogFile() 
		{
			this.SaveLogFile();
		}

		private void InternalInitialization() 
		{
			
		}


		protected virtual void OnWorkingSpaceChanged() 
		{
			// reset processing script fileName
			this.ProcessingScript = _workingSpace.FileName;

			// switch to run mode
			this.SwitchToRun = true;			

			// initializes hashtable contains name of the steps for updating status while executing
			Script script = this.Script;

			// reset progress bar
			this._statusTable.CreateProgressStatus(script);

			// reset processing file
			this.ProcessingFile = string.Empty;
		}

		#endregion Methods

		#region Override Methods
		protected override void OnLoad(EventArgs e) 
		{
			base.OnLoad(e);

			// init for progress bar						
			_scriptProgressBar.FloodStyle = SIA.Workbench.ProgressFloodStyle.Horizontal;
			_scriptProgressBar.FloodPercentage = 1.0f;			
			_scriptProgressBar.DashSpace = 0;
			_scriptProgressBar.Value = 0;
           
			_fileProgressBar.FloodStyle = SIA.Workbench.ProgressFloodStyle.Horizontal;
			_fileProgressBar.FloodPercentage = 1.0f;			
			_fileProgressBar.DashSpace = 0;						
			_fileProgressBar.Value = 0;

			// clear logs
			this.ClearLogs();
		}

		#endregion Override Methods

		#region Event Handlers
		#endregion Event Handlers

		#region Internal Helpers
		
		public void SaveLogFile() 
		{
			if (this.Logs == string.Empty) 
			{
				MessageBoxEx.Info("Logs is empty!");
				return;
			}

			using (SaveFileDialog dlg = CommonDialogs.SaveTextFileDialog("Save Logs As...")) 
			{
				if( dlg.ShowDialog() == DialogResult.OK ) 
				{					
					try 
					{
						if (File.Exists( dlg.FileName )) 
						{
							File.SetAttributes(dlg.FileName, FileAttributes.Normal );
						}
					}
					catch(Exception ex) 
					{
						MessageBoxEx.Error(ex.Message);
						return;
					}

					try 
					{
						using (StreamWriter writer = new StreamWriter(dlg.FileName, false, System.Text.Encoding.ASCII)) 
						{
							writer.Write(this.richTextLogs.Text);
						}						
					}
					catch(System.Exception exp) 
					{
						Trace.WriteLine(exp);
						MessageBoxEx.Error("Failed to save logs data");
					}
					finally 
					{
					}
				}
			}
		}


		#endregion Internal Helpers

		#region External Helpers

		#region update memory status
#if SHOW_MEMORY_STATUS
		public void SetMemStatus(string[] text)
		{
			try
			{
				if (listViewMemory.Items == null)
					return;
				if (listViewMemory.Items.Count <= 0)
				{
					ListViewItem item = new ListViewItem();
					item.SubItems[0].Text = string.Empty;
					item.SubItems.Add(string.Empty);
					item.SubItems.Add(string.Empty);
					item.SubItems.Add(string.Empty);
					item.SubItems.Add(string.Empty);					
					listViewMemory.Items.Add(item);
				}
				
				ListViewItem memItems = listViewMemory.Items[0];
				if (memItems == null)
					return;
				int memItemsCount = memItems.SubItems.Count;
				for (int i = 0; i < Math.Min(memItemsCount, text.Length); i++)
					memItems.SubItems[i].Text = text[i];
			}
			finally
			{
			}
		}

		public void ResetMemStatus()
		{
			SetMemStatus(new string[] {"", "", "", "", ""});
		}
#endif		
		#endregion update memory status

		#region update logs

		public void UpdateHeaderProcessingScriptLogs(bool bStart) 
		{
			string separator = "==================================================================================================\n";

			string logs = string.Empty;

			try 
			{
				if (bStart) 
				{								
					logs += separator;
					logs += "SUMMARY RUNNING SCRIPT FILE: " + this.ProcessingScript + "\n";
					logs += separator;
					logs += "Start Time: " + DateTime.Now.ToString() + "\n\n";
				}
				else 
				{
					logs += "\n\n";
					logs += "End Time: " + DateTime.Now.ToString() + "\n";
					logs += separator + "\n";
				}

				this.richTextLogs.AppendText(logs);
			}
			finally 
			{
			}
		}

		public void UpdateHeaderProcessingFileLogs(int globalCounter, bool bStart) 
		{
			string separator =   "****************************************************************************************************************\n";

			string logs = string.Empty;

			try 
			{
				if (bStart) 
				{								
					logs += separator;
					logs += globalCounter.ToString() + ". " + this.ProcessingFile + "\n";
					logs += "Start Time: " + DateTime.Now.ToString() + "\n";
				}
				else 
				{
					logs += "End Time: " + DateTime.Now.ToString() + "\n";
					logs += separator;
				}

				this.richTextLogs.AppendText(logs);
			}
			catch (System.Exception exp) 
			{
				Trace.WriteLine(exp.Message, exp.StackTrace);
			}
			finally 
			{
			}
		}

		public void UpdateDetailLogs() 
		{			
			string logs = string.Empty;

			try 
			{
				TableModel tableModel = _statusTable.TableModel;
				if (tableModel.Rows != null) 
				{	
					int count =  tableModel.Rows.Count;
					for (int i=0; i< count; i++) 
					{
						logs += "\t" + ((int)(i+1)).ToString() + ". " +  _statusTable.GetName(i) + "\n";
						logs += "\t\tStart Time: " + _statusTable.GetStartTime(i) + "\n";
						logs += "\t\tEnd Time: " + _statusTable.GetEndTime(i) + "\n";
					}				
				}

				this.richTextLogs.AppendText(logs);
			}
			catch (System.Exception exp) 
			{
				Trace.WriteLine(exp.Message, exp.StackTrace);
			}
			finally 
			{
			}
		}
		
		#endregion update logs

		public void LogsAppendText(string text) 
		{
			richTextLogs.AppendText(text);
		}		

		public void ClearLogs() 
		{
			this.richTextLogs.Text = string.Empty;
		}

		
		public void UpdateStep(int index, int percentage) 
		{
            this.UpdateStep(index, percentage, DateTime.MinValue, DateTime.MinValue);
		}

		public void UpdateStep(int index, DateTime startTime, DateTime endTime) 
		{
			this.UpdateStep(index, -1, startTime, endTime);
		}
		
		public void UpdateStep(int index, int percentage, DateTime startTime, DateTime endTime) 
		{
			TableModel tableModel = this._statusTable.TableModel;
			if (tableModel == null || tableModel.Rows == null || tableModel.Rows.Count == 0)
				return;

			try 
			{
				if (index<0 || index >= tableModel.Rows.Count)
					return;
				
				// update progress bar cell 
				if (percentage > 0)
					this._statusTable.SetProgress(index, percentage);
				
				// update start time cell
				if (startTime != DateTime.MinValue)
					this._statusTable.SetStartTime(index, startTime.ToShortTimeString());

				// update end time cell
                if (endTime != DateTime.MinValue)
					this._statusTable.SetEndTime(index, endTime.ToShortTimeString());

				// updated previous step				
				if (index > _iProcessingStep) 
				{
					_iProcessingStep = index;
					index--;
							 
					// update previous step progress bar
					if (this._statusTable.GetProgress(index) != 100)
						this._statusTable.SetProgress(index, 100);						
					
					// update previous end time cell
					if (this._statusTable.GetEndTime(index) == string.Empty)
						this._statusTable.SetEndTime(index, this._statusTable.GetStartTime(index));
				}
			}
			catch 
			{
			}
		}


		#endregion External Helpers

		public void OnStartProcessScript(StartProcessScriptEventArgs args) 
		{
			lock (_syncObject) 
			{
				try 
				{
					// reset table of process steps
					this._statusTable.ResetProgressStatus(true);

					// reset processing script
					this.txtProcessingScript.Text = args.ScriptFileName;

					// clear old logs
					this.richTextLogs.Text = "";	
				
					// write start process script
					this.AppendLogStartProcessScript(args);
				}
				catch (System.Exception exp) 
				{
					Trace.WriteLine(exp);
				}
			}
		}

		public void OnStopProcessScript(StopProcessScriptEventArgs args) 
		{
			lock (_syncObject) 
			{
				try 
				{
					// update status bar
					this.OverralProgress = 100;

					// write stop process script
					this.AppendLogStopProcessScript(args);
				}
				catch (System.Exception exp) 
				{
					Trace.WriteLine(exp);
				}
			}
		}

		public void OnBeginProcessFile(BeginProcessFileEventArgs args) 
		{
			lock (_syncObject) 
			{
				try 
				{
					// reset index of processing step
					this._iProcessingStep = 0;

					// reset table of process steps
					this._statusTable.ResetProgressStatus(true);

					// update processing file name
					this.txtProcessingFile.Text = args.FileName;

					// update logs
					this.AppendLogBeginProcessFile(args);
				}
				catch (System.Exception exp) 
				{
					Trace.WriteLine(exp);
				}
			}
		}

		public void OnEndProcessFile(EndProcessFileEventArgs args) 
		{
			lock (_syncObject) 
			{
				try 
				{
					// update current file progress bar 
					this.ProcessingFileProgress = 100;

					// update logs
					this.AppendLogEndProcessFile(args);
				}
				catch (System.Exception exp) 
				{
					Trace.WriteLine(exp);
				}
			}
		}

		public void OnStatusChanged(StatusChangedEventArgs args) 
		{
			if (Monitor.TryEnter(this._statusChangeWaitHandle, 1000)) 
			{
				#region Update status
				try 
				{
					_statusChangeWaitHandle.Reset();

					SharedData data = args.SharedData;
					if (data != null) 
					{
						// update step progress status
						this.UpdateStep(data.StepIndex, data.StepProgress, data.StartTime, data.EndTime);
			
						// update current file progress bar 
						int currentFileProgress = (100*this.ProcessingStep) / this.NumberOfSteps;
						this.ProcessingFileProgress = currentFileProgress;

						// update overal progress bar
						int interval = 0;
						if (args.Files!=null && args.Files.Length > 0) 
						{
							interval = 100/args.Files.Length;	
							this.OverralProgress = currentFileProgress*interval/100 + (100*args.CurrentFileIndex/args.Files.Length);
						}
					}

					if (args.MessageQueue != null) 
					{
#if DEBUG_
						if (_oldQueueLength != args.MessageQueue.Count)
						{
							this.AppendLogMessage(string.Format("Queue Length: {0} \n", args.MessageQueue.Count));
							_oldQueueLength = args.MessageQueue.Count;
						}
#endif
						UpdateStatusCommand command = null;
						while ((command = args.MessageQueue.Dequeue()) != null) 
						{
							if (command.StatusText != string.Empty && 
								command.StatusText != "")
								_currentSubStep = command.StatusText;
							if (command.Type == UpdateStatusCommandType.SharedData) 
							{
								data = command.SharedData;
								if (data != null) 
								{
									if (data.Done)
										_currentSubStep = string.Empty;

									// check for step changed 
									if (data.ActionType == SharedDataActionType.Begin)
										this.AppendLogBeginProcessStep(data);
									else if (data.ActionType == SharedDataActionType.End)
										this.AppendLogEndProcessStep(data);
									else if (data.ActionType == SharedDataActionType.Exception)
										this.AppendLogExceptionProcessStep(data);

									// update step progress status
									this.UpdateStep(data.StepIndex, data.StepProgress, data.StartTime, data.EndTime);
			
									// update current file progress bar 
									int currentFileProgress = (100*this.ProcessingStep) / this.NumberOfSteps;
									this.ProcessingFileProgress = currentFileProgress;

									// update overall progress bar
									int interval = 0;
									if (args.Files!=null && args.Files.Length > 0) 
									{
										interval = 100/args.Files.Length;	
										this.OverralProgress = currentFileProgress*interval/100 + (100*args.CurrentFileIndex/args.Files.Length);
									}
								}
							}
							else if (command.Type == UpdateStatusCommandType.PercentageOnly) 
							{
								// update step progress status
								this.UpdateStep(this.ProcessingStep, (int)command.Percentage);
							}
							else if (command.Type == UpdateStatusCommandType.TextOnly) 
							{
							}
						}
					}
				}
				catch (System.Exception exp) 
				{
					Trace.WriteLine(exp);
				}
				finally 
				{
					Monitor.Exit(this._statusChangeWaitHandle);
					this._statusChangeWaitHandle.Set();
				}
				#endregion
			}
			else 
			{ // it could not enter
				Trace.WriteLine(string.Format("{0}: It could not enter critical section", DateTime.Now));				
			}
		}


		private void AppendLogMessage(string message) 
		{			
			if (this.richTextLogs.TextLength > _maxLogBuffer) 
			{
				this.richTextLogs.Text.Remove(0,  _clearBufferStep);
				if (this.richTextLogs.TextLength > 0)
					this.richTextLogs.Text.Remove(0,  this.richTextLogs.TextLength);
				this.richTextLogs.Text = "";
			}

			int length = this.richTextLogs.Text.Length;
			this.richTextLogs.Select(length, 0);
			this.richTextLogs.SelectedText = message;
		}

		private void AppendLogStartProcessScript(StartProcessScriptEventArgs args) 
		{
			const string separator = "**********************************************************************************";
			const string header    = "SIA Workbench - Script Monitor \r\nVersion {0}\r\nStart processing script:\"{1}\"\r\n{2}\r\n";
			string str = String.Format(header, Application.ProductVersion, args.ScriptFileName, separator);
			
			this.AppendLogMessage(str);
		}

		private void AppendLogStopProcessScript(StopProcessScriptEventArgs args) 
		{
			const string separator = "**********************************************************************************";
			const string footer = "\n{0}\r\nStop processing script:\"{1}\"\r\n";
			string str = String.Format(footer, separator, args.ScriptFileName);

			this.AppendLogMessage(str);
		}

		private void AppendLogBeginProcessFile(BeginProcessFileEventArgs args) 
		{
			const string format = "\r\nBegin process file: {0}\r\n";
			string str = string.Format(format, args.FileName);
			this.AppendLogMessage(str);
		}

		private void AppendLogEndProcessFile(EndProcessFileEventArgs args) 
		{
			const string format = "\rEnd process file: {0}\r\n";
			string str = string.Format(format, args.FileName);
			this.AppendLogMessage(str);
		}

		private void AppendLogBeginProcessStep(SharedData args) 
		{
			const string format = "\tStep {0}: \r\n\t\t+ Start Time: {1}";
			string str = string.Format(format, args.StepName, args.StartTime.ToShortTimeString());
			this.AppendLogMessage(str);
		}

		private void AppendLogEndProcessStep(SharedData args) 
		{
			const string format = "\r\n\t\t+ End Time: {0}\r\n";
			string str = string.Format(format, args.EndTime.ToShortTimeString());
			this.AppendLogMessage(str);
		}

		private void AppendLogExceptionProcessStep(SharedData args) 
		{								
			if (args == null) return;			
			
			const string format = "\r\n\t\t+ Exception: Failed to process step \"{0}\". See log file for more details.";
			string str = string.Format(format, args.StepName);
			this.AppendLogMessage(str);
			
			if (args.LogFileName != null && args.LogFileName != string.Empty) 
			{
				this.richTextLogs.SelectedText += " \r\n\t\t+ Log file: ";
				this.richTextLogs.InsertLink(args.LogFileName);
			}
		}

		private void Log_LinkClicked(object sender, System.Windows.Forms.LinkClickedEventArgs e) 
		{
			System.Diagnostics.Process.Start(e.LinkText);
		}

		public void AppendStartedScanningFolder() 
		{
			this.AppendLogMessage("\n\n------------------------------------------------------------------------");
			this.AppendLogMessage("\nStart scanning files\n");

			if (_incomingFiles == null)
				_incomingFiles = new SortedList();
			else
				_incomingFiles.Clear();
			_counterIncomingFilesInFolder = -1;
			_totalIncomingFiles = 0;
		}

		public void AppendScanningFolder(string folder) 
		{
			if (_counterIncomingFilesInFolder > 0) 
			{
				this.AppendLogMessage("\n\n\t* Total incoming files: " + _counterIncomingFilesInFolder.ToString());
			}
			this.AppendLogMessage("\n- Scanning folder: " + folder);
			_counterIncomingFilesInFolder = 0;
		}

		public void AppendIncommingFileToLogWindow(string file) 
		{
			this.AppendLogMessage("\n\t+ Incoming file: " + file);
			_counterIncomingFilesInFolder++;
			_totalIncomingFiles++;
		}

		public void AppendEndedScanningFolder() 
		{
			if (_counterIncomingFilesInFolder > 0) 
			{
				this.AppendLogMessage("\n\n\t* Total incoming files: " + _counterIncomingFilesInFolder.ToString());
			}
			this.AppendLogMessage("\n\n- Total incoming files: " + _totalIncomingFiles.ToString());
			this.AppendLogMessage("\n\nEnd scanning files");
			this.AppendLogMessage("\n------------------------------------------------------------------------");
		}
	}
}
