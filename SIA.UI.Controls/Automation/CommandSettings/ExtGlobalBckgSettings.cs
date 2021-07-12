using System;
using System.Xml.Serialization;

using SIA.Common.KlarfExport;
using SIA.SystemFrameworks;
using SIA.UI.Controls.Commands;

namespace SIA.UI.Controls.Automation.Commands
{
    public enum eExtGlobalBckgMethod
    {
        FFT = 0,
        Erosion = 1
    }

    [Serializable]
    public class ExtGlobalBckgSettings : AutoCommandSettings
    {
        #region member fields
        public eExtGlobalBckgMethod Method = eExtGlobalBckgMethod.Erosion;
        public int NumberOfErosionFilters = 1;
        public float FFT_Threshold = 1.0f;
        public float FFT_CutOff = 2.0f;
        #endregion member fields

        #region Constructors ans Deconstructors
        public ExtGlobalBckgSettings()
        {
        }

        public ExtGlobalBckgSettings(int numberOfErsionFilters)
        {
            Method = eExtGlobalBckgMethod.Erosion;
            NumberOfErosionFilters = numberOfErsionFilters;
        }

        public ExtGlobalBckgSettings(float fft_threshold, float fft_cutoff)
        {
            Method = eExtGlobalBckgMethod.FFT;
            FFT_Threshold = fft_threshold;
            FFT_CutOff = fft_cutoff;
        }
        #endregion Constructors ans Deconstructors

        #region Properties
        #endregion Properties

        #region Methods
        public override void Copy(RasterCommandSettings settings)
        {
            if (settings is ExtGlobalBckgSettings == false)
                throw new ArgumentException("Settings is not an instance of ExtGlobalBckgSettings", "settings");

            base.Copy(settings);

            ExtGlobalBckgSettings cmdSettings = (ExtGlobalBckgSettings)settings;
            Method = cmdSettings.Method;
            NumberOfErosionFilters = cmdSettings.NumberOfErosionFilters;
            FFT_Threshold = cmdSettings.FFT_Threshold;
            FFT_CutOff = cmdSettings.FFT_CutOff;
        }

        public override void Validate()
        {            
        }

        public static ExtGlobalBckgSettings Deserialize(string filePath)
        {
            return (RasterCommandSettingsSerializer.Deserialize(filePath, typeof(ExtGlobalBckgSettings)) as ExtGlobalBckgSettings);
        }

        public void Serialize(string filePath)
        {
            RasterCommandSettingsSerializer.Serialize(filePath, this);
        }
        #endregion Methods
    }
}
