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
	/// Summary description for DyadicCommand.
	/// </summary>
	public class DyadicCommand : RasterCommand
	{
		public DyadicCommand(IProgressCallback callback) : base(callback)
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
			if (args[1] is string == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be string", "arguments");
			if (args[2] is string == false)
				throw new ArgumentException("Argument type does not match. Arguments[2] must be string", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
			string type = (string)args[1];
			string filename = (string)args[2];

			this.SetStatusRange(0, 100);
			this.SetStatusText("Perform calculation operation...");

#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif
			this.Dyadic(image, type, filename);

#if DEBUG_METETIME
            dm.AddLine("DyadicCommand:Run");
            dm.Write2Debug(true);
#endif
		}

#if GPU_SUPPORTED
		protected virtual void Dyadic(CommonImage image, string type, String filename)
		{
			CommonImage refImage = null;

			try
			{
                //this code will be removed in feature
                bool bCreateBuff = !image.HasDeviceBuffer;
                if (bCreateBuff)
                    image.CreateDeviceBuffer();

				refImage = CommonImage.FromFile(filename);
				image.kDyadicOperationGPU(type, refImage);

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
				if (refImage != null)
					refImage.Dispose();
				refImage = null;
			}
		}
#else
        protected virtual void Dyadic(CommonImage image, string type, String filename)
		{
			CommonImage refImage = null;

			try
			{
				refImage = CommonImage.FromFile(filename);
				image.kDyadicOperation(type, refImage, false);
			}
			catch
			{
				throw;
			}
			finally
			{
				if (refImage != null)
					refImage.Dispose();
				refImage = null;
			}
		}
#endif
    }
}
