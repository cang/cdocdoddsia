using System;
using System.Xml.Serialization;

using SIA.Common.KlarfExport;
using SIA.SystemFrameworks;
using SIA.UI.Controls.Commands;

namespace SIA.UI.Controls.Automation.Commands
{
	/// <summary>
	/// Summary description for DetectObjectCommandSettings.
	/// </summary>
	[Serializable]
	public class DetectObjectCommandSettings : AutoCommandSettings
	{
		#region member fields
		private ObjectDetectionSettings _settings = null;		
		#endregion member fields

		#region Constructors ans Deconstructors
		public DetectObjectCommandSettings()
		{			
		}

		public DetectObjectCommandSettings(ObjectDetectionSettings settings)
		{
			_settings = settings;
		}
		#endregion Constructors ans Deconstructors

		#region Properties

		[XmlElement(typeof(ObjectDetectionSettings))]
		public ObjectDetectionSettings ObjectDetectionSettings
		{
			get
			{
				return  _settings;
			}
			set
			{
				_settings = value;
			}
		}

		#endregion Properties

		#region Methods
		
		public override void Copy(RasterCommandSettings settings)
		{
			if (settings is DetectObjectCommandSettings == false)
				throw new ArgumentException("Settings is not an instance of DetectObjectCommandSettings", "settings");

			base.Copy(settings);
			
			DetectObjectCommandSettings cmdSettings = (DetectObjectCommandSettings)settings;
			if (cmdSettings._settings != null)
				this._settings = (ObjectDetectionSettings)cmdSettings._settings.Clone();
			else
				this._settings = null;
		}

		public override void Validate()
		{
			if (_settings == null)
				throw new ArgumentException("ObjectDetectionSettings is not set.",  "ObjectDetectionSettings");
		}			

		#endregion Methods
		
		#region Serializable && Deserialize
		public void Serialize(String filename)
		{
			this._settings = new ObjectDetectionSettings();
			this._settings.Serialize(filename);	
		}

		public void Deserialize(String filename)
		{
			if (this._settings != null)
				this._settings.Deserialize(filename);
		}
		#endregion Serializable && Deserialize
	}
}
