//#define GPU_SUPPORTED
//#define DEBUG_METETIME

using System;
using System.Drawing;
using System.Data;
using System.IO;
using System.Threading;
using System.Diagnostics;

using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;

using SIA.IPEngine;
using SIA.SystemLayer;

using SIA.UI.Controls.Utilities;
using SIA.Algorithms;

namespace SIA.UI.Controls.Commands
{
	/// <summary>
	/// Summary description for FilterRankCommand.
	/// </summary>
	public class FilterRankCommand : RasterCommand
	{
		public FilterRankCommand(IProgressCallback callback) : base(callback)
		{
		}

		protected override void ValidateArguments(params object[] args)
		{
			if (args == null)
				throw new ArgumentNullException("arguments");
			if (args.Length < 3)
				throw new ArgumentException("Not enough arguments", "arguments");
			if (args[0] is CommonImage == false)
				throw new ArgumentException("Argument type does not match. Arguments[0] must be CommonImage", "arguments");
			if (args[1] is int == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be integer", "arguments");
			if (args[2] is int == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be integer", "arguments");
			if (args[3] is int == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be integer", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
			int typeFilter = (int)args[1];
			int kernel = (int)args[2];
			int pass = (int)args[3];			

			this.SetStatusRange(0, 100);
			this.SetStatusText("Filtering image...");

#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif

			this.FilterRank(image, typeFilter, kernel, pass);

#if DEBUG_METETIME
            dm.AddLine("FilterRankCommand:Run");
            dm.Write2Debug(true);
#endif
        }

#if GPU_SUPPORTED
		protected virtual void FilterRank(CommonImage image, int typeFilter, int kernel, int pass)
		{
			try
			{
                //this code will be removed in feature
                bool bCreateBuff = !image.HasDeviceBuffer;
                if (bCreateBuff)
                    image.CreateDeviceBuffer();

				image.FilterRankGPU(typeFilter, kernel, pass);

                //this code will be removed in feature
                image.ReadDataFromDeviceBuffer();
                if (bCreateBuff)
                    image.DisposeDeviceBuffer();
			}
			catch
			{
				throw;
			}
			finally
			{
			}
		}
#else
        protected virtual void FilterRank(CommonImage image, int typeFilter, int kernel, int pass)
		{
			try
			{				
				image.FilterRank(typeFilter, kernel, pass);
			}
			catch
			{
				throw;
			}
			finally
			{
			}
		}
#endif
    }
}
