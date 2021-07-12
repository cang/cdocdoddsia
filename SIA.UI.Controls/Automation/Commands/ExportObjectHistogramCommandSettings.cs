using System;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Xml.Serialization;

using SIA.SystemFrameworks;
using SIA.UI.Controls.Commands;
//CONG using SIA.UI.Controls.Components;

namespace SIA.UI.Controls.Automation.Commands
{
	/// <summary>
	/// Summary description for ExportObjectHistogramCommandSettings.
	/// </summary>
	public class ExportObjectHistogramCommandSettings : AutoCommandSettings
	{
		#region member fields

		private String _fileName = "";

		private int _dataSourceType = 0;
		private HistogramType _histogramType = HistogramType.Linear;
		private DisplayType _displayType = DisplayType.Automatic;
		private HistogramCustomOptions _customOptions = new HistogramCustomOptions();
		private Size _imageSize = new Size(640, 280);
				
		#endregion member fields

		#region Properties

		[XmlIgnore]
		public string FileName
		{
			get {return _fileName;}
			set {_fileName = value;}
		}

		public int DataSourceType
		{
			get {return _dataSourceType;}
			set {_dataSourceType = value;}
		}
		
		public HistogramType HistogramType
		{
			get {return _histogramType;}
			set {_histogramType = value;}
		}

		public DisplayType DisplayType
		{
			get {return _displayType;}
			set {_displayType = value;}
		}

		public HistogramCustomOptions CustomOptions
		{
			get {return _customOptions;}
			set {_customOptions = value;}
		}

		public Size ImageSize 
		{
			get {return _imageSize;}
			set {_imageSize = value;}
		}
		
		#endregion
		
		#region Constructors ans Deconstructors

		public ExportObjectHistogramCommandSettings()
		{			
		}
		
		#endregion
		
		#region Methods

		public override void Copy(RasterCommandSettings settings)
		{
			if (settings is ExportObjectHistogramCommandSettings == false)
				throw new ArgumentException("Settings is not an instance of ExportObjectHistogramCommandSettings", "settings");

			base.Copy(settings);

			ExportObjectHistogramCommandSettings cmdSettings = settings as ExportObjectHistogramCommandSettings;
			this._fileName = cmdSettings._fileName;

			this._dataSourceType = cmdSettings._dataSourceType;
			this._histogramType = cmdSettings._histogramType;
			this._displayType = cmdSettings._displayType;
			this._customOptions = cmdSettings._customOptions;
			this._imageSize = cmdSettings._imageSize;
		}

		public override void Validate()
		{
		}		

		#endregion Methods
		
		#region Serializable && Deserialize
		
		#endregion Serializable && Deserialize
	}
}
