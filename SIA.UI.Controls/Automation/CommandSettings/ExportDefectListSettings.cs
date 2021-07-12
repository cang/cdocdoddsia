using System;
using System.IO;
using System.Xml.Serialization;

using SIA.Common;
using SIA.Common.Imaging;

using SIA.UI.Controls.Commands;
using SIA.Common.Analysis;

namespace SIA.UI.Controls.Automation.Commands
{
    public enum eDefectListReportFileType
    {
        TXT = 0,
        CSV = 1
    }

    [Serializable]
    public class ExportDefectListSettings : AutoCommandSettings
	{
		#region member fields        
        public String FileNameFormat = "";
        public eDefectListReportFileType Format = eDefectListReportFileType.TXT;

        public bool KeepName = true;
        public string CustomizedName = "";
		#endregion member fields

		#region Constructors ans Deconstructors
		public ExportDefectListSettings()
		{			
		}

		public ExportDefectListSettings(string fileNameFormat)
		{
            FileNameFormat = fileNameFormat;
		}

        public ExportDefectListSettings(eDefectListReportFileType fileType, string fileNameFormat)
		{
            Format = fileType;
            FileNameFormat = fileNameFormat;
		}
		#endregion Constructors ans Deconstructors

		#region Properties
		#endregion Properties

		#region Methods
		
		public override void Copy(RasterCommandSettings settings)
		{
            if (settings is ExportDefectListSettings == false)
                throw new ArgumentException("Settings is not an instance of ExportDefectListSettings", "settings");

			base.Copy(settings);

            ExportDefectListSettings cmdSettings = (ExportDefectListSettings)settings;
            this.FileNameFormat = (String)cmdSettings.FileNameFormat.Clone();
			this.Format = cmdSettings.Format;
            this.KeepName = cmdSettings.KeepName;
            this.CustomizedName = cmdSettings.CustomizedName;
		}

		public override void Validate()
		{			
		}		
	
		#endregion Methods

		#region Serializable && Deserialize
		#endregion Serializable && Deserialize		
	}
}
