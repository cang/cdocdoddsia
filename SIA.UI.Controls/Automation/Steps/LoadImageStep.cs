using System;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;

using SIA.Common.Analysis;
using SIA.SystemLayer;
using SIA.Workbench.Common;

using SIA.UI.Controls.Automation.Commands;
using SIA.UI.Controls.Automation.Dialogs;


namespace SIA.UI.Controls.Automation.Steps 
{
	/// <summary>
	/// Summary description for LoadImageStep.
	/// </summary>
	public class LoadImageStep 
        : ProcessStep3, IDataProviderStep
	{
		private const string displayName = "Load Image From File";
		private const string description = "Load image from file";

		#region Constructors and Destructors

		public LoadImageStep()
            : base(Guid.NewGuid().ToString(), displayName, description, ProcessStepCategories.Input,
			typeof(LoadImageCommand), typeof(LoadImageCommandSettings), 
			null, ValidationKeys.SingleImage)
		{			
		}

		public LoadImageStep(LoadImageCommandSettings settings)
			: this()
		{			
			this._settings = settings;
		}

        protected LoadImageStep(string id, string displayName, string description, string category, 
			Type commandType, Type settingsType, string inputKey, string outputKey)
			: base (id, displayName, description, category, commandType, settingsType, inputKey, outputKey)
		{
		}

		#endregion

		#region Properties	
		
		[XmlElement(typeof(LoadImageCommandSettings))]
		public virtual LoadImageCommandSettings SelfSettings 
		{
			get 
			{
				return (LoadImageCommandSettings)_settings;
			}
			set 
			{
				_settings = value;
			}
		}
		
		#endregion

		#region Methods

		public string GetSearchPatterns() 
		 {
			if (this.SelfSettings.Filter == null)
				return "*";
			string[] exts = this.SelfSettings.Filter.GetExtensions();
			if (exts == null || exts.Length == 0)
				return "*";

			string patterns = "";
			foreach (string ext in exts)
				patterns += "*" + ext + ";";
			patterns = patterns.Remove(patterns.Length-1, 1);

			return patterns;
		}

		public string[] GetRunFiles() 
		{
			if (SelfSettings.InputFileType == InputFileType.File)
				return new string[]{SelfSettings.FileName};
			else if (SelfSettings.InputFileType == InputFileType.Path) 
			{
				if (!Directory.Exists(SelfSettings.FilePath))
					return null;
				return Directory.GetFiles(SelfSettings.FilePath);
			}
			else 
				return null;
		}

		/// <summary>
		/// Get files in folder and sub-folders
		/// </summary>
		/// <param name="fileList"></param>
		/// <param name="folder"></param>
		private void GetFiles(ArrayList fileList, string folder, bool bSubFolder) 
		{
			if (folder == null || !Directory.Exists(folder))
				return;
			
			if (!bSubFolder) 
			{
				string []files = Directory.GetFiles(folder);				
				if (files != null && files.Length > 0)
					fileList.AddRange(files);
			}
			else 
			{
				ArrayList folders = GetSubFolders(folder);
				foreach (string subFolder in folders) 
				{
					if ( !Directory.Exists(subFolder) )
						continue;
					string[] files = Directory.GetFiles(subFolder);
					if (files != null && files.Length > 0)
						fileList.AddRange(files);
				}
			}
		}


		/// <summary>
		/// Get  files to run without conitnuous mode
		/// </summary>
		/// <returns> the files have to run without continuous mode</returns>
		public string[] GetFiles() 
		{
			ProcessingDataCollection data = SelfSettings.Data;
			if (data == null || data.Count == 0)
				return null;

			ArrayList fileList = new ArrayList();
			foreach (ProcessingData processingData in data) 
			{
				eProcessingDataType dataType = processingData.Type;
				if (dataType == eProcessingDataType.File) 
				{
					fileList.Add(processingData.FilePath);
				}
				else if (dataType == eProcessingDataType.NoScanFolder) 
				{
					this.GetFiles(fileList, processingData.FilePath, false);
				}
				else if (dataType == eProcessingDataType.SubFolderAndNoScan) 
				{
					this.GetFiles(fileList, processingData.FilePath, true);
				}
			}

			string []results = null;
			if (fileList != null && fileList.Count > 0) 
			{
				results = (string[])fileList.ToArray(typeof(string));
			}
			return results;
		}

		/// <summary>
		/// Get sub-folders of input folder
		/// </summary>
		/// <param name="rootFolder">Input folder</param>
		/// <returns>List of sub-folders</returns>
		private ArrayList GetSubFolders(string rootFolder) 
		{
			ArrayList folderList = new ArrayList();

			folderList.Add(rootFolder);

			Queue folderQueue = new Queue(1);						

			folderQueue.Enqueue(rootFolder);

			while ( folderQueue.Count > 0 ) 
			{
				String folder = (String)folderQueue.Dequeue();

				if ( !Directory.Exists(folder) ) continue;

				try 
				{
					String[] subFolders = Directory.GetDirectories(folder);

					foreach ( String tmpFolder in subFolders ) 
					{
						folderQueue.Enqueue(tmpFolder);

						folderList.Add(tmpFolder);
					}
				}
				catch ( System.UnauthorizedAccessException ) 
				{
				}
			}

			return folderList;
		}

		/// <summary>
		/// Get scan folders to scan and run as continuous mode
		/// </summary>
		/// <returns>the folders to scan</returns>
		public string[] GetScanFolders() 
		{
			ProcessingDataCollection data = SelfSettings.Data;
			if (data == null || data.Count == 0)
				return null;

			ArrayList fileList = new ArrayList();
			foreach (ProcessingData processingData in data) 
			{
				eProcessingDataType dataType = processingData.Type;
				if (dataType == eProcessingDataType.ScanFolder) 
				{
					fileList.Add(processingData.FilePath);
				}				
			}

			string []results = null;
			if (fileList != null && fileList.Count > 0) 
			{
				results = (string[])fileList.ToArray(typeof(string));
			}
			return results;
		}


		public override void Validate(SIA.Plugins.Common.IAutomationWorkspace workspace)
		{
			base.Validate(workspace);			

			if (this.Index > 0)
				throw new ArgumentException(String.Format("The \"{0}\" step must be the first step in the script", this.DisplayName));
		}


		public override void Execute(WorkingSpace workingSpace) 
		{
			if (workingSpace == null || workingSpace.ProcessingFileName == null)
				return;
			
			if (_settings == null)
				_settings = new LoadImageCommandSettings();
			
			SelfSettings.FileName = workingSpace.ProcessingFileName;

			using (LoadImageCommand command = new LoadImageCommand(null)) 
			{
				command.AutomationRun(new object[]{_settings});				
				
                // update working space image
                workingSpace.Image = command.Image;
                workingSpace["SINGLEIMAGE"] = command.Image;
			}
		}

		public override bool ShowSettingsDialog(System.Windows.Forms.IWin32Window owner) 
		{
			bool bSettingsIsNull = false;
						
			if (this._settings == null) 
			{
				bSettingsIsNull = true;
				this._settings = new LoadImageCommandSettings("", null);
			}
			
			using (DlgLoadImageStepSettings2 dlg = new DlgLoadImageStepSettings2((LoadImageCommandSettings)this._settings)) 
			{
				if (DialogResult.OK == dlg.ShowDialog(owner)) 
				{			
					// save modified settings
					this._settings = dlg.Settings;

					return true;
				}
				else 
				{
					if (bSettingsIsNull)
						this._settings = (SIA.UI.Controls.Commands.RasterCommandSettings)null;
				}
			}

			return false;
		}

		#endregion		
	
        #region IDataProviderStep Members

        public bool ScanSubFolder
        {
            get 
            {
                if (this.SelfSettings == null)
                    return false;
                return this.SelfSettings.ScanSubFolder;
            }
        }

        public bool ClearProcessedFileHistory
        {
            get
            {
                if (this.SelfSettings == null)
                    return false;
                return this.SelfSettings.ClearProcessedFileHistory;
            }
        }

        public bool UseFilter
        {
            get
            {
                if (this.SelfSettings == null)
                    return false;
                return this.SelfSettings.UseFilter;
            }
        }
        
        public InputFileFilter Filter
        {
            get
            {
                if (this.SelfSettings == null)
                    return null;
                return this.SelfSettings.Filter;
            }
        }

        #endregion
    }
}
