using System;
using System.Collections.Generic;
using System.Text;
using SIA.UI.Controls.Commands;

namespace SIA.UI.Controls.Automation.Commands
{
    [Serializable]
    public class DetectAnomaliesSettings : AutoCommandSettings
	{
		#region member fields
        public bool ApplyRegions = true;
		#endregion member fields

		#region Constructors ans Deconstructors
		public DetectAnomaliesSettings()
		{			
		}

		public DetectAnomaliesSettings(bool applyRegions)
		{
            ApplyRegions = applyRegions;
		}		
        #endregion Constructors ans Deconstructors

		#region Properties
		#endregion Properties

		#region Methods
		
		public override void Copy(RasterCommandSettings settings)
		{
            if (settings is DetectAnomaliesSettings == false)
                throw new ArgumentException("Settings is not an instance of DetectAnomaliesSettings", "settings");

			base.Copy(settings);

            DetectAnomaliesSettings cmdSettings = (DetectAnomaliesSettings)settings;
            this.ApplyRegions = cmdSettings.ApplyRegions;
		}

		public override void Validate()
		{			
		}		
	
		#endregion Methods

		#region Serializable && Deserialize
		#endregion Serializable && Deserialize		
	}
}
