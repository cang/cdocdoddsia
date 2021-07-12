using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

using SIA.SystemFrameworks;
using SIA.UI.Controls.Commands;

namespace SIA.UI.Controls.Automation.Commands
{
	/// <summary>
	/// Summary description for CustConvolutionCommandSettings.
	/// </summary>
	[Serializable]
	public class CustConvolutionCommandSettings : AutoCommandSettings
	{
		#region members
		// [XmlArray(ElementName = "Item", IsNullable = true)]
		[XmlElement(typeof(ArrayList))]
		public ArrayList Matrix = null;
		public int nCols = 1;
		//public int [,] Matrix;
		public int NumPass = 1;		

		#endregion members

		#region Constructors and Destructors
		public CustConvolutionCommandSettings()
		{			
		}

		public CustConvolutionCommandSettings(int [,]matrix, int numPass)
		{						
			int nRows = matrix.GetLength(0);
			nCols = matrix.GetLength(1);
			Matrix = new ArrayList(nRows*nCols);			
			for (int i=0; i<nRows; i++)
			{
				for (int j=0; j<nCols; j++)
				{
					Matrix.Add(matrix[i, j]);
				}
			}			
			//Matrix = matrix;
			NumPass = numPass;
		}
		#endregion Constructors and Destructors

		#region Properties
		#endregion Properties

		#region Internal Helpers
		public int [,] GetMatrix()
		{
			int nRows = Matrix.Count/nCols;
			int [,] matrix = new int[nRows, nCols];
			int index = 0;
			for (int i=0; i<nRows; i++)
			{
				for (int j=0; j< nCols; j++)
				{
					matrix[i, j] = (int)Matrix[index++];
				}
			}
			return matrix;
		}
		#endregion Internal Helpers

		#region Methods
		
		public override void Copy(RasterCommandSettings settings)
		{
			if (settings is CustConvolutionCommandSettings == false)
				throw new ArgumentException("Settings is not an instance of CustConvolutionCommandSettings", "settings");

			base.Copy(settings);

			CustConvolutionCommandSettings cmdSettings = (CustConvolutionCommandSettings)settings;
			if (this.Matrix != null)
				this.Matrix = (ArrayList)cmdSettings.Matrix.Clone();//(int [,])cmdSettings.Matrix.Clone();
			else
				this.Matrix = null;
			this.NumPass = cmdSettings.NumPass;
		}
		
		public override void Validate()
		{
		}	
		#endregion Methods
		
		#region Serializable && Deserialize

		public void Serialize(String filename)
		{
			
		}

		public void Deserialize(String filename)
		{
			
		}

		#endregion Serializable && Deserialize
	}
}
