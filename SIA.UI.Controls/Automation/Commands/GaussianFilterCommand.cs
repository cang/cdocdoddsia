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

using SIA.UI.Controls.Commands;
using SIA.Algorithms;

namespace  SIA.UI.Controls.Automation.Commands
{
	/// <summary>
	/// Summary description for GaussianFilterCommand.
	/// </summary>
	public class GaussianFilterCommand : AutoCommand
	{
		public GaussianFilterCommand(IProgressCallback callback) : base(callback)
		{
		}

		protected override void ValidateArguments(params object[] args)
		{
			if (args == null)
				throw new ArgumentNullException("arguments");
			if (args.Length < 1)
				throw new ArgumentException("Not enough arguments", "arguments");
			if (args[0] is CommonImage == false)
				throw new ArgumentException("Argument type does not match. Arguments[0] must be CommonImage", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;

			this.SetStatusRange(0, 100);
			this.SetStatusText("Gaussian filtering...");

#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif
			image.GaussianSmoothFilter();

#if DEBUG_METETIME
            dm.AddLine("GaussianFilterCommand:Run");
            dm.Write2Debug(true);
#endif
		}

#if GPU_SUPPORTED
		public override void AutomationRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;

            //this code will be removed in feature
            bool bCreateBuff = !image.HasDeviceBuffer;
            if (bCreateBuff)
                image.CreateDeviceBuffer();

			image.GaussianSmoothFilterGPU();

            //this code will be removed in feature
            image.ReadDataFromDeviceBuffer();
            if (bCreateBuff)
                image.DisposeDeviceBuffer();

		}
#else
        public override void AutomationRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;

			image.GaussianSmoothFilter();
		}
#endif
	}
}
