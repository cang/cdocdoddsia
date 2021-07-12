using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using SIA.Common.PatternRecognition;
using SIA.Common.Analysis;
using SIA.SystemLayer;
using SIA.Workbench.Common;

using SIA.Plugins.Common;
using SIA.Algorithms.FeatureProcessing;
using SiGlaz.Common.ImageAlignment;
using SIA.Algorithms.Preprocessing.Alignment;

namespace SIA.UI.Controls.Automation
{
    /// <summary>
    /// The WorkingSpace class works as a document workspace when executing a RDE Monitor script.
    /// This class contains all necessary stuff for executing and communicate with the host process.
    /// </summary>
	[Serializable]
	public class WorkingSpace 
        : IDisposable
	{
		#region Members

		private IScript _script = null;
		private CommonImage _image = null;
		private DetectedObjectCollection _detectedObjects = null;
		private string _processingFileName = string.Empty;
		public string ScriptName = null;
		private int _counter = 0;
		private ScriptData _scriptData = new ScriptData();
		private Hashtable _storage = null;
		private Hashtable _localStorage = new Hashtable();

        private MetrologySystemReference _refFile = null;
        private MetrologySystem _detectedSystem = null;
        private AlignmentResult _alignmentResult = null;
        private SiGlaz.Common.GraphicsList _detectedRegions = null;

        private FeatureSpace _featureSpace = null;

		#endregion

		#region Properties

		public IScript Script
		{
			get {return _script;}
			set {_script = value;}
		}

		public CommonImage Image
		{
			get
			{
				return _image;
			}
			set
			{
				_image = value;
			}
		}

		public DetectedObjectCollection DetectedObjects
		{
			get
			{
				return _detectedObjects;
			}
			set
			{
				_detectedObjects = value;
			}
		}

        public FeatureSpace FeatureSpace
        {
            get { return _featureSpace; }
            set { _featureSpace = value; }
        }

        public MetrologySystemReference RefFile
        {
            get { return _refFile; }
            set { _refFile = value; }
        }

        public MetrologySystem DetectedSystem
        {
            get { return _detectedSystem; }
            set 
            { 
                _detectedSystem = value;
                if (_detectedSystem != null)
                {
                    _detectedSystem.RebuildTransformer();
                }
            }
        }

        public AlignmentResult AlignmentResult
        {
            get { return _alignmentResult; }
            set { _alignmentResult = value; }
        }

        public SiGlaz.Common.GraphicsList DetectedRegions
        {
            get { return _detectedRegions; }
            set { _detectedRegions = value; }
        }

        public void InternalReset()
        {
            try
            {
                if (_detectedObjects != null)
                    _detectedObjects.Clear();

                this["BROKENWAFERS"] = null;

                this["DETECTEDOBJECTS"] = null;

                
            }
            catch
            {
                // nothing
            }
        }

		public string ProcessingFileName
		{
			get
			{
				return _processingFileName;
			}
			set
			{
				_processingFileName = value;
			}
		}

		public int Counter
		{
			get 
			{
				return _counter;
			}
			set 
			{
				_counter = value;
			}
		}

		public ScriptData ScriptData
		{
			get {return _scriptData;}
		}

		public Hashtable Storage
		{
			get {return _storage;}
		}

		public object this[object key]
		{
			get {return _localStorage[key];}
			set {_localStorage[key] = value;}
		}

		#endregion

		#region Constructors and Destructors
		
		public WorkingSpace()
		{
		}

		public WorkingSpace(IScript script)
		{
			_script = script;
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if (_image != null)
			{
				_image.Dispose();
				_image = null;
			}
			if (_detectedObjects != null)
			{
				_detectedObjects.Clear();
				_detectedObjects = null;
			}
		}

		#endregion

		#region Public Methods

		public void LoadStorage(string hostProcessID)
		{
			using (ScriptSharedMemory scriptSharedMemory = new ScriptSharedMemory(hostProcessID, false))
			{
				try
				{
					scriptSharedMemory.Lock();
					Hashtable storage = scriptSharedMemory.GetData() as Hashtable;
					if (storage != null)
						this._storage = storage;
				}
				finally
				{
					scriptSharedMemory.Unlock();
				}
			}
		}

		public void SaveStorage(string hostProcessID)
		{
			using (ScriptSharedMemory scriptSharedMemory = new ScriptSharedMemory(hostProcessID, false))
			{
                if (this._storage != null)
                {
                    try
                    {
                        scriptSharedMemory.Lock();
                        scriptSharedMemory.SetData(this._storage);
                    }
                    finally
                    {
                        scriptSharedMemory.Unlock();
                    }
                }
			}
		}

		#endregion

	}
}
