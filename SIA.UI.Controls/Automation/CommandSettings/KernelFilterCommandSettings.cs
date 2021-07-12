using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using SIA.Common;
using SIA.UI.Controls.Commands;

namespace SIA.UI.Controls.Automation.Commands
{	
	/// <summary>
	/// Summary description for KernelFilterCommandSettings.
	/// </summary> 
	[Serializable]
	public class KernelFilterCommandSettings
        : AutoCommandSettings
	{
		#region Member Fields
		
		// type of filter used
		public eKernelFilterType Type = eKernelFilterType.Convolution;

		// Convolution Command Settings
		[XmlElement(typeof(ConvolutionCommandSettings))]
		public ConvolutionCommandSettings Convolution = null;

		// Cust Convolution Settings
		[XmlElement(typeof(CustConvolutionCommandSettings))]
		//[XmlIgnore]
		public CustConvolutionCommandSettings CustConvolution = null;

		// Morphology Settings
		[XmlElement(typeof(MorphologyCommandSettings))]
		public MorphologyCommandSettings MorphologySettings = null;

		#endregion

		#region Properties
		
		#endregion

		#region Constructor and Destructors

		public KernelFilterCommandSettings()
		{
		}		

		public KernelFilterCommandSettings(ConvolutionCommandSettings settings)
		{
			Type = eKernelFilterType.Convolution;
			Convolution = settings;			
		}

		public KernelFilterCommandSettings(CustConvolutionCommandSettings settings)
		{
			Type = eKernelFilterType.CustConvolution;
			CustConvolution = settings;			
		}

		public KernelFilterCommandSettings(MorphologyCommandSettings settings)
		{
			Type = eKernelFilterType.Morphology;
			MorphologySettings = settings;			
		}
		#endregion

		#region Methods

		public override void Copy(RasterCommandSettings settings)
		{
			if (settings is KernelFilterCommandSettings == false)
				throw new ArgumentException("Settings is not an instance of KernelFilterCommandSettings", "settings");

			base.Copy(settings);

			KernelFilterCommandSettings cmdSettings = (KernelFilterCommandSettings)settings;

			this.Type = cmdSettings.Type;
			if (cmdSettings.Convolution != null)
				this.Convolution = (ConvolutionCommandSettings)cmdSettings.Convolution.Clone();
			else
				this.Convolution = null;

			if (cmdSettings.CustConvolution != null)
				this.CustConvolution = (CustConvolutionCommandSettings)cmdSettings.CustConvolution.Clone();
			else
				this.CustConvolution = null;

			if (cmdSettings.MorphologySettings != null)
				this.MorphologySettings = (MorphologyCommandSettings)cmdSettings.MorphologySettings.Clone();
			else
				this.MorphologySettings = null;
		}

		public override void Validate()
		{
			switch (Type)
			{
				case eKernelFilterType.Convolution:
					if (Convolution == null)
						throw new ArgumentException("Setting cannot be null!", "Convolution Filter");
					break;
				case eKernelFilterType.CustConvolution:
					if (CustConvolution == null)
						throw new ArgumentException("Setting cannot be null!", "CustConvolution Filter");
					break;
				case eKernelFilterType.Morphology:
					if (MorphologySettings == null)
						throw new ArgumentException("Setting cannot be null!", "Morphology Filter");
					break;
				default:
					throw new ArgumentException("Kernel filter type doesn't match!");					
			}
		}				
		
		#endregion

		#region Serializable && Deserialize
		
		#endregion Serializable && Deserialize		
	}
}
