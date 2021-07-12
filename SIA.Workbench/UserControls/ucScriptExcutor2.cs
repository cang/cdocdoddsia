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
	/// Summary description for ucScriptExecutor2.
	/// </summary>
	internal class ucScriptExecutor2 : System.Windows.Forms.UserControl 
	{
		#region  Windows Form Member fields		

		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Panel panel3;		
		private System.Windows.Forms.GroupBox groupBox1;	
		private System.Windows.Forms.Splitter vSplitter;
		private SIA.Workbench.Components.RichTextBoxEx richTextLogs;
		private System.Windows.Forms.Panel panelOutput;
		private System.Windows.Forms.Panel panelScript;
		private ScriptExecution scriptExecution;	  	
		
		#endregion 

		#region Member fields

		private bool bCanRun = false;
		private bool bCanStop = false;

		private WorkingSpace _workingSpace = null;
		private string _processingFileName = string.Empty;

		private object _syncObject = new object();
		private AutoResetEvent _statusChangeWaitHandle = new AutoResetEvent(true);
		
		private int _clearBufferStep = 1024;
		private int _maxLogBuffer = 32768;

		private SortedList _incomingFiles = new SortedList();
		private int _counterIncomingFilesInFolder = 0;
		private int _totalIncomingFiles = 0;	
			
		#endregion Member fields

		#region Constructors and Destructors
		public ucScriptExecutor2() 
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
			this.panelScript = new System.Windows.Forms.Panel();
			this.scriptExecution = new SIA.Workbench.UserControls.ScriptExecution();
			this.panel3 = new System.Windows.Forms.Panel();
			this.panelOutput = new System.Windows.Forms.Panel();
			this.richTextLogs = new SIA.Workbench.Components.RichTextBoxEx();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.vSplitter = new System.Windows.Forms.Splitter();
			this.panel4.SuspendLayout();
			this.panelScript.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panelOutput.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.panelScript);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel4.Location = new System.Drawing.Point(0, 0);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(708, 436);
			this.panel4.TabIndex = 16;
			// 
			// panelScript
			// 
			this.panelScript.Controls.Add(this.scriptExecution);
			this.panelScript.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelScript.DockPadding.All = 4;
			this.panelScript.Location = new System.Drawing.Point(0, 0);
			this.panelScript.Name = "panelScript";
			this.panelScript.Size = new System.Drawing.Size(708, 436);
			this.panelScript.TabIndex = 0;
			// 
			// scriptExecution
			// 
			this.scriptExecution.AllowDrop = true;
			this.scriptExecution.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scriptExecution.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.scriptExecution.Location = new System.Drawing.Point(4, 4);
			this.scriptExecution.Name = "scriptExecution";
			this.scriptExecution.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.scriptExecution.Size = new System.Drawing.Size(700, 428);
			this.scriptExecution.TabIndex = 0;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.panelOutput);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 436);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(708, 168);
			this.panel3.TabIndex = 15;
			// 
			// panelOutput
			// 
			this.panelOutput.Controls.Add(this.richTextLogs);
			this.panelOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelOutput.DockPadding.All = 4;
			this.panelOutput.Location = new System.Drawing.Point(0, 0);
			this.panelOutput.Name = "panelOutput";
			this.panelOutput.Size = new System.Drawing.Size(708, 168);
			this.panelOutput.TabIndex = 9;
			// 
			// richTextLogs
			// 
			this.richTextLogs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextLogs.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.richTextLogs.Location = new System.Drawing.Point(4, 4);
			this.richTextLogs.MaxLength = 200;
			this.richTextLogs.Name = "richTextLogs";
			this.richTextLogs.Size = new System.Drawing.Size(700, 160);
			this.richTextLogs.TabIndex = 3;
			this.richTextLogs.Text = "";
			this.richTextLogs.WordWrap = false;
            this.richTextLogs.LinkClicked += new LinkClickedEventHandler(RichTextLogs_LinkClicked);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(0, -80);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(700, 8);
			this.groupBox1.TabIndex = 14;
			this.groupBox1.TabStop = false;
			// 
			// vSplitter
			// 
			this.vSplitter.BackColor = System.Drawing.SystemColors.ControlDark;
			this.vSplitter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.vSplitter.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.vSplitter.Location = new System.Drawing.Point(0, 434);
			this.vSplitter.Name = "vSplitter";
			this.vSplitter.Size = new System.Drawing.Size(708, 2);
			this.vSplitter.TabIndex = 0;
			this.vSplitter.TabStop = false;
			// 
			// ucScriptExecutor2
			// 
			this.Controls.Add(this.vSplitter);
			this.Controls.Add(this.panel4);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.groupBox1);
			this.Name = "ucScriptExecutor2";
			this.Size = new System.Drawing.Size(708, 604);
			this.panel4.ResumeLayout(false);
			this.panelScript.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panelOutput.ResumeLayout(false);
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


		public bool IsLogEmpty
		{
			get {return richTextLogs.Text != null && richTextLogs.Text.Length > 0;}
		}
		
		public bool SwitchToRun 
		{
			set 
			{
				this.CanRun = value;
				this.CanStop = !this.CanRun;
			}
		}

				
		#endregion

		#region Methods	
		
		public void DoCommandOpenScriptFile() 
		{			
		}

		public void DoCommandSaveLogFile() 
		{
			this.SaveLogFile();
		}

		

		protected virtual void OnWorkingSpaceChanged() 
		{
			// switch to run mode
			this.SwitchToRun = true;			

			// initializes hashtable contains name of the steps for updating status while executing
			Script script = this._workingSpace.Script;

			// load script from working space
			scriptExecution.Load(script);
			
			// reset processing file
			this._processingFileName = string.Empty;
		}

		#endregion Methods

		#region Override Methods
		
		protected override void OnLoad(EventArgs e) 
		{
			base.OnLoad(e);

			// clear logs
			this.ClearLogs();
		}

		#endregion

		#region Event Handlers
		#endregion Event Handlers

		#region Internal Helpers
		
		public void SaveLogFile() 
		{
			string str = richTextLogs.Text;
			
			if (str == string.Empty) 
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
			string scriptFileName = this._workingSpace.FileName;

			if (bStart) 
			{								
				logs += separator;
				logs += "SUMMARY RUNNING SCRIPT FILE: " + scriptFileName + "\n";
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

		public void UpdateHeaderProcessingFileLogs(int globalCounter, bool bStart) 
		{
			string separator =   "****************************************************************************************************************\n";
			string logs = string.Empty;

			if (bStart) 
			{								
				logs += separator;
				logs += globalCounter.ToString() + ". " + this._processingFileName + "\n";
				logs += "Start Time: " + DateTime.Now.ToString() + "\n";
			}
			else 
			{
				logs += "End Time: " + DateTime.Now.ToString() + "\n";
				logs += separator;
			}

			this.richTextLogs.AppendText(logs);
		}

		private void UpdateDetailLogs() 
		{			
			string logs = string.Empty;
			this.richTextLogs.AppendText(logs);
		}
		
		#endregion 

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
			this.UpdateStep(index, string.Empty, percentage, DateTime.MinValue, DateTime.MinValue);
		}

        public void UpdateStep(int index, DateTime startTime, DateTime endTime) 
		{
			this.UpdateStep(index, string.Empty, -1, startTime, endTime);
		}

        public void UpdateStep(int index, string status, int percentage, DateTime startTime, DateTime endTime) 
		{			
            scriptExecution.UpdateProcessStepStatus(index, status, percentage, startTime, endTime);
		}


		#endregion External Helpers

		public void OnStartProcessScript(StartProcessScriptEventArgs args) 
		{
			lock (_syncObject) 
			{
				try 
				{
					// clear old logs
					this.richTextLogs.Text = "";	

					// update script execution
					scriptExecution.BeginExecution(args.ScriptFileName);
				
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
					// update script execution
					scriptExecution.EndExecution(args.ScriptFileName);

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
					// update script execution
					scriptExecution.BeginProcessFile(args.FileName);

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
					// update script execution
					scriptExecution.EndProcessFile(args.FileName);

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

					// update step progress status
					SharedData data = args.SharedData;
					if (data != null) 
						this.UpdateStep(data.StepIndex, string.Empty, data.StepProgress, data.StartTime, data.EndTime);

					if (args.MessageQueue != null) 
					{
						UpdateStatusCommand command = null;
						while ((command = args.MessageQueue.Dequeue()) != null) 
						{
							if (command.Type == UpdateStatusCommandType.SharedData) 
							{
								data = command.SharedData;
								if (data != null) 
								{
									// check for step changed 
									if (data.ActionType == SharedDataActionType.Begin)
										this.AppendLogBeginProcessStep(data);
									else if (data.ActionType == SharedDataActionType.End)
										this.AppendLogEndProcessStep(data);
									else if (data.ActionType == SharedDataActionType.Exception)
										this.AppendLogExceptionProcessStep(data);

									// update step progress status
									this.UpdateStep(data.StepIndex, command.StatusText, data.StepProgress, data.StartTime, data.EndTime);
								}
							}
							else if (command.Type == UpdateStatusCommandType.PercentageOnly) 
							{
								// update step progress status
								this.UpdateStep(-1, command.StatusText, (int)command.Percentage, DateTime.MinValue, DateTime.MinValue);
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
			else // it could not enter
			{ 
				Trace.WriteLine(string.Format("{0}: It could not enter critical section", DateTime.Now));				
			}
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
			string str = string.Format(format, args.StepName, args.StartTime);
			this.AppendLogMessage(str);
		}

		private void AppendLogEndProcessStep(SharedData args) 
		{
			const string format = "\r\n\t\t+ End Time: {0}\r\n\t\t+ Duration: {1}\r\n";
            TimeSpan duration = args.EndTime - args.StartTime;
            string str = string.Format(format, args.EndTime, duration);
			this.AppendLogMessage(str);
		}

		private void AppendLogExceptionProcessStep(SharedData args) 
		{								
			if (args == null) return;

            const string format = "\r\n\t\t\t+ Exception: Failed to process step \"{0}\".\r\n\t\t\t  Message: {1}";
			string str = string.Format(format, 
                args.StepName, args.ExceptionMessage);
			this.AppendLogMessage(str);
			
			if (args.LogFileName != null && args.LogFileName != string.Empty) 
			{
                this.richTextLogs.SelectedText += " \r\n\t\t\t+ Log file: ";
				this.richTextLogs.InsertLink(args.LogFileName);
			}
		}

		private void RichTextLogs_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }
	}
}
