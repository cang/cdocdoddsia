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
	/// Summary description for MonadicCommand.
	/// </summary>
	public class MonadicCommand : RasterCommand
	{
		public MonadicCommand(IProgressCallback callback) : base(callback)
		{
		}

		protected override void ValidateArguments(params object[] args)
		{
			if (args == null)
				throw new ArgumentNullException("arguments");
			if (args.Length < 3)
				throw new ArgumentException("Not enough arguments", "arguments");
			
			for (int i=0; i<args.Length; i++)
				if (args[i] == null)
					throw new ArgumentNullException("Invalid parameter arg[" + i + "]");
			
			if (args[0] is CommonImage == false)
				throw new ArgumentException("Argument type does not match. Arguments[0] must be CommonImage", "arguments");
			if (args[1] is string == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be string", "arguments");
			if (args[2] is float == false)
				throw new ArgumentException("Argument type does not match. Arguments[2] must be float", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
			string type = (string)args[1];
			float value = (float)args[2];

			this.SetStatusRange(0, 100);
			this.SetStatusText("Perform calculation operation...");

#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif

			this.Monadic(image, type, value);

#if DEBUG_METETIME
            dm.AddLine("MonadicCommand:Run");
            dm.Write2Debug(true);
#endif
        }

#if GPU_SUPPORTED
		protected virtual void Monadic(CommonImage image, string type, float value)
		{
			try
			{
                //this code will be removed in feature
                bool bCreateBuff = !image.HasDeviceBuffer;
                if (bCreateBuff)
                    image.CreateDeviceBuffer();

				image.kMonadicOperationGPU(type, value);

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
        protected virtual void Monadic(CommonImage image, string type, float value)
		{
			try
			{
				image.kMonadicOperation(type, value, false);
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
