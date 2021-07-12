#define SIAW_SCIENTECH

using System;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

using SIA.UI.Controls.Automation;
using SIA.UI.Controls.Automation.Commands;
using SIA.UI.Controls.Automation.Steps;
using SIA.Workbench.Utilities;


namespace SIA.Workbench 
{

	/// <summary>
	/// Summary description for DataProvider.
	/// </summary>
    internal class DataProvider : SIA.Workbench.Common.IDataProvider, IDisposable 
	{
		#region Member Fields
		private Script _script = null;
		private ArrayList _processedFiles = new ArrayList();
		private ArrayList _dataFiles = new ArrayList();

		private ScanFolderEngine _folderScanner = null;
		private ScanFolderSettings _folderScannerSettings = null;
		private IDataProviderStep _dataProviderStep = null;
		
		public event EventHandler StartedScanningFolder = null;
		public event EventHandler ScanningFolder = null;
		public event EventHandler EndedScanningFolder = null;

		#region Cong: input file filter
		
		private bool _bFilter = false;
		private string _fileNameFilter = string.Empty;
		private string[] _fileTypeFilter = null;
		private Regex _regEngine = null;
		
		#endregion Cong - 2007 - 07 - 23

		#endregion

		#region Properties

		public bool IsListening
		{
			get {return _folderScanner != null;}
		}

		public bool HasLoadImageStep
		{
			get {return _dataProviderStep != null;}
		}

		#endregion
			
		#region Constructor and destructor

		public DataProvider(Script script) 
		{		

			if (script == null)
				throw new ArgumentNullException("script", "Invalid script reference");
			_script = script;
			
			this.InitClass();
		}

		private void InitClass() 
		{
			this._processedFiles = new ArrayList();
			this._dataFiles = new ArrayList();
			this._folderScanner = null;
			this._folderScannerSettings = null;

			// scan the process step in the script and find for load image step
			this._dataProviderStep = null;
			foreach (ProcessStep step in _script.ProcessSteps)
			{
				if (step is IDataProviderStep)
				{
					_dataProviderStep = step as IDataProviderStep;
					break;
				}
			}			
		}

		#region IDisposable Members

		public void Dispose() 
		{
			_regEngine = null;

			if (this._folderScanner != null)
				this._folderScanner.Dispose();
			this._folderScanner = null;
		}

		#endregion

		#endregion

		#region IDataProvider Members
		
		public ArrayList BeginGetFiles() 
		{
			if (this._script == null)
				throw new ArgumentNullException("_script", "Script was not set to a reference.");
			if (this._script.ProcessSteps.Count <= 0)
				throw new ArgumentException("Script.ProcessSteps", "Process step collection is empty");
			//if (!(this._script.ProcessSteps[0] is IDataProviderStep))
			//	throw new ArgumentException("Script.ProcessSteps[0]", "IDataProviderStep was not found.First step must be Load image step.");

			// initialize data file buffers
			_dataFiles.Clear();

			// initialize processed file buffers
			_processedFiles.Clear();

			// retrieve load image step settings
			IDataProviderStep dataProviderStep = this._dataProviderStep;
			if (dataProviderStep != null)
			{
				bool bScanSubFolder = false, bClearProcessedFileHistory = false;
				if (dataProviderStep != null) 
				{
					bScanSubFolder = dataProviderStep.ScanSubFolder;
					bClearProcessedFileHistory = dataProviderStep.ClearProcessedFileHistory;
				}
			
				// retrieve running file from load image step
				string[] fileList = dataProviderStep.GetFiles();
				if (fileList != null)
					this._dataFiles.AddRange(fileList);


				// retrieve scan folders from load image step
				string[] scanFolders = dataProviderStep.GetScanFolders();
				string assemblyPath = Path.GetDirectoryName(typeof(DataProvider).Assembly.Location);

				if (scanFolders != null && scanFolders.Length > 0) 
				{			

					// initialize folder scanner
					_folderScannerSettings = new ScanFolderSettings();
					_folderScannerSettings.AutoDeleteOldFileFromDatabase = true;
					foreach (string folder in scanFolders)
						_folderScannerSettings.AddFolder(folder);
					_folderScannerSettings.IncludeSubFolders = bScanSubFolder;
					_folderScannerSettings.DBFileName = assemblyPath + @"\ScanFileDb.mdb";
					//_folderScannerSettings.MaxDays = int.MaxValue;
					_folderScannerSettings.Pattern = dataProviderStep.GetSearchPatterns();

					_folderScanner = new ScanFolderEngine(_folderScannerSettings);
					_folderScanner.StartedScanningFolder += new EventHandler(OnStartedScanningFolder);
					_folderScanner.ScanningFolder += new EventHandler(OnScanningFolder);
					_folderScanner.EndedScanningFolder += new EventHandler(OnEndedScanningFolder);
                    
#if !SIAW_SCIENTECH
					if (bClearProcessedFileHistory)
						_folderScanner.ClearDatabase();
#endif
				}
			}

			return (ArrayList)this._dataFiles.Clone();
		}

		/// <summary>
		/// Gets next file in the file queue
		/// </summary>
		/// <returns>File path of the next file in the queue</returns>
		public string GetNextFile() 
		{
			string filePath = null;

			if (_dataFiles.Count > 0) // normal mode
			{
				filePath = (string)_dataFiles[0];
				if (filePath != null)
					_processedFiles.Add(filePath);
				_dataFiles.RemoveAt(0);
			}
			else if (this._folderScanner != null) // if listening mode
			{
				filePath = _folderScanner.GetNewFile();
				if (filePath != null) 
				{
					_processedFiles.Add(filePath);
					//this._folderScanner.SetFileAsProcessed(filePath);
				}
			}

			if (filePath == null && this._folderScanner == null)
				filePath = string.Empty;
			return filePath;
		}

		public void EndGetFiles() 
		{

			if (this._folderScanner != null)
				this._folderScanner.Dispose();
			this._folderScanner = null;
			
			if (this._folderScannerSettings != null)
				this._folderScannerSettings.Dispose();
			this._folderScannerSettings = null;

			_dataFiles.Clear();
			_processedFiles.Clear();
		}

		public void PrepareProcessInputFileFilter() 
		{
			if (this._dataProviderStep == null)
				return;

			try 
			{
				IDataProviderStep step = this._dataProviderStep;
                _bFilter = step.UseFilter;
				if (!_bFilter)
					return;
                if (step.Filter.FilterInputFileName)
                    _fileNameFilter = step.Filter.FileNameFormat;
				else
					_fileNameFilter = string.Empty;
                if (step.Filter.FilterInputFileFormat) 
				{
                    _fileTypeFilter = step.Filter.GetExtensions();
				}
				else
					_fileTypeFilter = null;

				if (_fileNameFilter == null || _fileNameFilter == string.Empty) 
				{
					_regEngine = null;
				}
				else 
				{
					string  tmpFilter = _fileNameFilter.Replace("*",".*");
					if (tmpFilter[0] != '^')
						tmpFilter = "^" + tmpFilter;
					_regEngine = new Regex(tmpFilter);
				}
			}
			finally 
			{
			}
		}

		public bool ProcessFilterInputFile(string fileName) 
		{
			if (!_bFilter)
				return true;

			if (_fileTypeFilter != null) 
			{
				string ext = Path.GetExtension(fileName).ToLower();
				bool bValid = false;
				for (int i=0; i<_fileTypeFilter.Length; i++)
					if (ext == _fileTypeFilter[i]) 
					{
						bValid = true;
						break;
					}
				if (!bValid) return false;				
			}

			if (_fileNameFilter != null && _fileNameFilter != string.Empty) 
			{
				if (_regEngine == null)
					return true;
				string name = Path.GetFileNameWithoutExtension(fileName);
				bool result = _regEngine.IsMatch(name);
				return result;
			}			
			return true;
		}

		#endregion

		#region Event handlers
		private void OnStartedScanningFolder(object sender, EventArgs args) 
		{
			if (this.StartedScanningFolder != null)
				this.StartedScanningFolder(this, args);
		}

		private void OnScanningFolder(object sender, EventArgs args) 
		{
			if (this.ScanningFolder != null)
				this.ScanningFolder(this, args);
		}

		private void OnEndedScanningFolder(object sender, EventArgs args) 
		{
			if (this.EndedScanningFolder != null)
				this.EndedScanningFolder(this, args);
		}
		#endregion Event handlers
	}
}
