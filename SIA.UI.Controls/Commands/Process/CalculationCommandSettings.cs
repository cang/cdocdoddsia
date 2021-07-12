using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using SIA.Common;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Automation;

namespace SIA.UI.Controls.Commands
{	
	/// <summary>
	/// Represents the calculation command settings
	/// </summary> 
	[Serializable]
	public class CalculationCommandSettings 
        : AutoCommandSettings
	{
		#region Member Fields
		
		/// <summary>
		/// Gets or sets type of calculation
		/// </summary>
		public eCalculationType Type = eCalculationType.Monadic;

		/// <summary>
		/// Gets or sets type of calculation with a constant value
		/// </summary>
		public String MonadicType = "";

        /// <summary>
        /// Gets or sets the value for calculating (add, subtract, multiply, divide)
        /// </summary>
		public float Value = 0;
		
		/// <summary>
		/// Gets or sets type of calculation with an image
		/// </summary>
		public String DyadicType = "";

        /// <summary>
        /// Gets ors sets the location of the image
        /// </summary>
		public String FileName = "";

		#endregion

		#region Constructor and Destructors

		public CalculationCommandSettings()
		{
		}		

		public CalculationCommandSettings(String monadicType, float value)
		{
			Type = eCalculationType.Monadic;
			MonadicType = monadicType;
			Value = value;
		}

		public CalculationCommandSettings(String dyadicType, string fileName)
		{
			Type = eCalculationType.Dyadic;
			DyadicType = dyadicType;
			FileName = fileName;
		}
		#endregion

		#region Methods

		public override void Copy(RasterCommandSettings settings)
		{
			if (settings is CalculationCommandSettings == false)
				throw new ArgumentException("Settings is not an instance of CalculationCommandSettings", "settings");

			base.Copy(settings);

			CalculationCommandSettings cmdSettings = (CalculationCommandSettings)settings;

			this.Type = cmdSettings.Type;

			if (cmdSettings.MonadicType != null)			
				this.MonadicType = (String)cmdSettings.MonadicType.Clone();
			else
				this.MonadicType = null;

			this.Value = cmdSettings.Value;

			if (cmdSettings.DyadicType != null)
				this.DyadicType = (String)cmdSettings.DyadicType.Clone();
			else
				this.DyadicType = null;

			if (cmdSettings.FileName != null)
				this.FileName = (String)cmdSettings.FileName.Clone();
			else
				this.FileName = null;
		}

		public override void Validate()
		{			
		}				
		
		#endregion
	}
}
