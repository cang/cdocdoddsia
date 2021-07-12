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
	/// Summary description for ResizeImageCommand.
	/// </summary>
	public class ResizeImageCommand : RasterCommand
	{
		public ResizeImageCommand(IProgressCallback callback) : base(callback)
		{
		}

		protected override void ValidateArguments(params object[] args)
		{
			if (args == null)
				throw new ArgumentNullException("arguments");
			if (args.Length < 2)
				throw new ArgumentException("Not enough arguments", "arguments");
			if (args[0] is CommonImage == false)
				throw new ArgumentException("Argument type does not match. Arguments[0] must be CommonImage", "arguments");
			if (args[1] is ResizeImageCommandSettings == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be ResizeImageCommandSettings", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
			ResizeImageCommandSettings settings = (ResizeImageCommandSettings)args[1];

			this.SetStatusRange(0, 100);
			this.SetStatusText("Resizing image...");

#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif

			this.ResizeImage(image, settings);

#if DEBUG_METETIME
            dm.AddLine("ResizeImageCommand:Run");
            dm.Write2Debug(true);
#endif
		}

//#if GPU_SUPPORTED
//        protected virtual void ResizeImage(CommonImage image, ResizeImageCommandSettings settings)
//        {
//            try
//            {
//                //this code will be removed in feature
//                bool bCreateBuff = !image.HasDeviceBuffer;
//                if (bCreateBuff)
//                    image.CreateDeviceBuffer();


//                if (settings.ResizeBy == ResizeBy.AbsoluteSize)
//                {
//                    image.ResizeGPU(settings.Width, settings.Height, settings.SamplingType);
//                }
//                else 
//                {
//                    int newWidth = (int)Math.Round(image.Width * settings.Percentage * 0.01F);
//                    int newHeight = (int)Math.Round(image.Height * settings.Percentage * 0.01F);

//                    image.ResizeGPU(newWidth, newHeight, settings.SamplingType);
//                }

//                //this code will be removed in feature
//                image.ReadDataFromDeviceBuffer();
//                if (bCreateBuff)
//                    image.DisposeDeviceBuffer();
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//            }
//        }
//#else
        protected virtual void ResizeImage(CommonImage image, ResizeImageCommandSettings settings)
		{
			try
			{
				if (settings.ResizeBy == ResizeBy.AbsoluteSize)
				{
					image.Resize(settings.Width, settings.Height, settings.SamplingType);
				}
				else 
				{
					int newWidth = (int)Math.Round(image.Width * settings.Percentage * 0.01F);
					int newHeight = (int)Math.Round(image.Height * settings.Percentage * 0.01F);
					image.Resize(newWidth, newHeight, settings.SamplingType);
				}
			}
			catch
			{
				throw;
			}
			finally
			{
			}
        }
//#endif


    }
}
