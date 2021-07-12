using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using SIA.UI.Controls.Commands;
using System.IO;
using SIA.SystemLayer.ImageProcessing;
using SIA.Algorithms.ReferenceFile;

namespace SIA.UI.Controls.Automation.Commands
{
    /// <summary>
    /// Summary description for DetectObjectUsingGoldenImageSettings.
    /// </summary>
    [Serializable]
    public class DetectObjectUsingGoldenImageSettings : AutoCommandSettings
    {
        #region member fields
        public string GoldenImageFilePath = "";
        public eGoldenImageMethod Method = eGoldenImageMethod.None;
        public double DarkThreshold = 110;
        public double BrightThreshold = 150;
		#endregion member fields

		#region Constructors ans Deconstructors
		public DetectObjectUsingGoldenImageSettings()
		{			
		}
		#endregion Constructors ans Deconstructors

		#region Properties
		
		#endregion Properties

		#region Methods
		
		public override void Copy(RasterCommandSettings settings)
		{
            if (settings is DetectObjectUsingGoldenImageSettings == false)
                throw new ArgumentException("Settings is not an instance of DetectObjectUsingGoldenImageSettings", "settings");

			base.Copy(settings);

            DetectObjectUsingGoldenImageSettings cmdSettings = (DetectObjectUsingGoldenImageSettings)settings;
            GoldenImageFilePath = cmdSettings.GoldenImageFilePath;
            Method = cmdSettings.Method;
            DarkThreshold = cmdSettings.DarkThreshold;
            BrightThreshold = cmdSettings.BrightThreshold;
		}

		public override void Validate()
		{
            if (GoldenImageFilePath == "" )
                throw new ArgumentException("GoldenImageFilePath is not set.", "DetectObjectUsingGoldenImageSettings");
            
            if (!File.Exists(GoldenImageFilePath))
                throw new ArgumentException(
                    string.Format("GoldenImageFilePath: {0} does not exist.", GoldenImageFilePath), 
                    "DetectObjectUsingGoldenImageSettings");
		}

		#endregion Methods
		
		#region Serializable && Deserialize
        public static DetectObjectUsingGoldenImageSettings Deserialize(string fileName)
        {
            try
            {
                return RasterCommandSettingsSerializer.Deserialize(
                    fileName, typeof(DetectObjectUsingGoldenImageSettings)) as DetectObjectUsingGoldenImageSettings;
            }
            catch
            {
                // nothting                
            }

            return new DetectObjectUsingGoldenImageSettings();
        }

        public void Serialize(string fileName)
        {
            try
            {
                RasterCommandSettingsSerializer.Serialize(fileName, this);
            }
            catch
            {
                // nothting                
            }
        }
		#endregion Serializable && Deserialize
    }
}
