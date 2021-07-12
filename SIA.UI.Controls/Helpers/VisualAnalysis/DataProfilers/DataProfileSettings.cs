using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Threading;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using SIA.Common;
using SIA.Common.Analysis;
using SIA.SystemLayer;

using SIA.UI.Controls;
using SIA.UI.Controls.Common;
//CONG using SIA.UI.Controls.Components;
using SIA.UI.Controls.UserControls;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Helpers;
using SIA.UI.Controls.Helpers.VisualAnalysis;
using SIA.UI.Controls.Utilities;

using SIA.UI.Components;
using SIA.UI.Components.Helpers;

namespace SIA.UI.Controls.Helpers.VisualAnalysis.DataProfilers
{
	/// <summary>
	/// Summary description for LineProfileSettings.
	/// </summary>
	public class LineProfileSettings : ICloneable
	{
		public bool AutoScale = true;
		public float Mininum = 0;
		public float Maximum = 10000;
		public bool	LogarithmicPlot = false;

		public LineProfileSettings()
		{
		}

		#region ICloneable Members

		public object Clone()
		{
			return this.MemberwiseClone();
		}

		#endregion

		public virtual void UpdateDataBySettings(float[] data)
		{
			if (this.LogarithmicPlot)
			{
				for (int i=0; i<data.Length; i++)
					data[i] = (float)(data[i] > 0 ? Math.Log10(data[i]) : 0);
			}
			
			if (!this.AutoScale)
			{
				float maxVal = Math.Max(this.Maximum, this.Mininum);
				float minVal = Math.Min(this.Maximum, this.Mininum);
                //float range = maxVal - minVal;
				
                //float dataMin = float.MaxValue, dataMax = float.MinValue;
                //float dataRange = 0;

                //for (int i=0; i<data.Length; i++)
                //{
                //    dataMin = Math.Min(data[i], dataMin);
                //    dataMax = Math.Max(data[i], dataMax);
                //}

                //dataRange = dataMax - dataMin;

                //for (int i=0; i<data.Length; i++)
                //    data[i] = minVal + (data[i]-dataMin)*range/dataRange;

                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = Math.Max(minVal, Math.Min(maxVal, data[i]));
                }
			}
		}
	}

	public class BoxProfileSettings : LineProfileSettings
	{
		public BoxProfileOptions Options = BoxProfileOptions.Maximum;

		public BoxProfileSettings()
		{
		}
	};

	public class AreaPlotSettings : LineProfileSettings
	{
		public RenderMode RenderMode;
		public float XRes = 1;
		public float YRes = 1;

		public AreaPlotSettings()
		{
		}
	};
}
