using System;
using System.Xml.Serialization;

using SIA.UI.Controls.Commands;

namespace SIA.UI.Controls.Automation.Commands
{
	public enum eThresholdType
	{
		Intensity = 0,
		Percentile = 1
	}

	/// <summary>
	/// Summary description for ThresholdCommandSettings.
	/// </summary>
	[Serializable]
	public class ThresholdCommandSettings : AutoCommandSettings
	{
		#region member fields
		public eThresholdType Type = eThresholdType.Intensity;
		
		public bool UseMinimum = false;
		public int Minimum = 0;
		public bool ZeroMinimum = false;
		public bool UseMaximum = false;
		public int Maximum = 0;
		public bool ZeroMaximum = false;

		public bool RemoveDeadPixel = false;
		public float From = 0;
		public bool ZeroFrom = false;
		public bool RemoveHotPixel = false;
		public float To = 100;
		public bool ZeroTo = false;
		#endregion member fields

		#region Constructors ans Deconstructors
		public ThresholdCommandSettings()
		{
		}

		public ThresholdCommandSettings(bool useMin, int minimum, bool zeroMinimum, bool useMax, int maximum, bool zeroMaximum)
		{
			Type = eThresholdType.Intensity;
			UpdateSettings_ThresholdByIntensity(useMin, minimum, zeroMinimum, useMax, maximum, zeroMaximum);
		}

		public ThresholdCommandSettings(bool removeDead, float from, bool zeroFrom, bool removeHot, float to, bool zeroTo)
		{
			Type = eThresholdType.Percentile;
			UpdateSettings_ThresholdByPercentile(removeDead, from, zeroFrom, removeHot, to, zeroTo);
		}
		#endregion Constructors ans Deconstructors

		#region Methods
		
		public void UpdateSettings_ThresholdByIntensity(bool useMin, int minimum, bool zeroMinimum, bool useMax, int maximum, bool zeroMaximum)
		{
			UseMinimum = useMin;
			UseMaximum = useMax;
			Minimum = minimum;
			ZeroMinimum = zeroMaximum;
			Maximum = maximum;
			ZeroMaximum = zeroMaximum;
		}

		public void UpdateSettings_ThresholdByPercentile(bool removeDead, float from, bool zeroFrom, bool removeHot, float to, bool zeroTo)
		{
			RemoveHotPixel = removeHot;
			RemoveDeadPixel = removeDead;
			From = from;
			ZeroFrom = zeroFrom;
			To = to;
			ZeroTo = zeroTo;
		}

		public override void Copy(RasterCommandSettings settings)
		{
			if (settings is ThresholdCommandSettings == false)
				throw new ArgumentException("Settings is not an instance of ThresholdCommandSettings", "settings");

			base.Copy(settings);

			ThresholdCommandSettings cmdSettings = (ThresholdCommandSettings)settings;

			this.Type = cmdSettings.Type;

			this.UseMinimum = cmdSettings.UseMinimum;
			this.UseMaximum = cmdSettings.UseMaximum;
			this.Minimum = cmdSettings.Minimum;
			this.Maximum = cmdSettings.Maximum;
			this.ZeroMinimum = cmdSettings.ZeroMinimum;
			this.ZeroMaximum = cmdSettings.ZeroMaximum;

			this.RemoveDeadPixel = cmdSettings.RemoveDeadPixel;
			this.RemoveHotPixel = cmdSettings.RemoveHotPixel;
			this.From = cmdSettings.From;
			this.To  = cmdSettings.To;
			this.ZeroFrom = cmdSettings.ZeroFrom;
			this.ZeroTo = cmdSettings.ZeroTo;
		}

		public override void Validate()
		{			
		}
		#endregion Methods
	}
}
