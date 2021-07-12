//#define GPU_SUPPORTED
//#define DEBUG_METETIME

#define USING_IPL

using System;
using System.Drawing;
using System.Data;
using System.IO;
using System.Threading;
using System.Diagnostics;

using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;
using SIA.Common.Imaging.Filters;

using SIA.IPEngine;
using SIA.SystemLayer;

using SIA.UI.Controls.Utilities;

using SIA.UI.Controls.Commands;
using SIA.Algorithms;

namespace SIA.UI.Controls.Automation.Commands
{
	/// <summary>
	/// Summary description for FlipRotateImageCommand.
	/// </summary>
	public class FlipRotateImageCommand : AutoCommand
	{
		public FlipRotateImageCommand(IProgressCallback callback) : base(callback)
		{
		}

		public override bool CanAbort
		{
			get {return true;}
		}


		protected override void ValidateArguments(params object[] args)
		{
			if (args == null)
				throw new ArgumentNullException("arguments");
			if (args.Length < 2)
				throw new ArgumentException("Not enough arguments", "arguments");
			if (args[0] is CommonImage == false)
				throw new ArgumentException("Argument type does not match. Arguments[0] must be CommonImage", "arguments");
			if (args[1] is FlipRotateImageCommandSettings == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be FlipRotateImageCommandSettings", "arguments");			
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
			FlipRotateImageCommandSettings cmdSettings = (FlipRotateImageCommandSettings)args[1];

			this.Execute(image, cmdSettings);
		}

		public override void AutomationRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
			FlipRotateImageCommandSettings cmdSettings = (FlipRotateImageCommandSettings)args[1];

			this.Execute(image, cmdSettings);
		}

#if GPU_SUPPORTED
		public void Execute(CommonImage image, FlipRotateImageCommandSettings cmdSettings)
		{
			float angle = cmdSettings.RotateAngle;
			FlipRotateImageCommandSettings.Actions type = (FlipRotateImageCommandSettings.Actions)cmdSettings.ActionType;

			// initialize progress range
			this.SetStatusRange(0, 100);

#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif

            //this code will be removed in feature
            bool bCreateBuff = !image.HasDeviceBuffer;
            if (bCreateBuff)
                image.CreateDeviceBuffer();

            switch (type)
            {
                case FlipRotateImageCommandSettings.Actions.FlipHorizontal:
                    this.SetStatusText("Flipping horizontal...");
                    image.FlipHozGPU();
                    break;
                case FlipRotateImageCommandSettings.Actions.FlipVertical:
                    this.SetStatusText("Flipping vertical...");
                    image.FlipVerGPU();
                    break;
                case FlipRotateImageCommandSettings.Actions.Rotate90CW:
                    this.SetStatusText("Rotating 90 Clockwise...");
                    image.RotateGPU(-90);
                    break;
                case FlipRotateImageCommandSettings.Actions.Rotate90CCW:
                    this.SetStatusText("Rotating 90 Counter-Clockwise...");
                    image.RotateGPU(90);
                    break;
                case FlipRotateImageCommandSettings.Actions.Rotate180:
                    this.SetStatusText("Rotating 180...");
                    image.RotateGPU(180);
                    break;
                case FlipRotateImageCommandSettings.Actions.RotateByAngle:
                    this.SetStatusText("Rotating image by a specified angle...");
                    image.RotateGPU(angle);
                    break;
                default:
                    break;
            }

            //this code will be removed in feature
            image.ReadDataFromDeviceBuffer();
            if (bCreateBuff)
                image.DisposeDeviceBuffer();


#if DEBUG_METETIME
            dm.AddLine("RotateImageCommand:Run");
            dm.Write2Debug(true);
#endif
		}
#else
        public void Execute(CommonImage image, FlipRotateImageCommandSettings cmdSettings)
		{
			float angle = cmdSettings.RotateAngle;
			FlipRotateImageCommandSettings.Actions type = (FlipRotateImageCommandSettings.Actions)cmdSettings.ActionType;

			// initialize progress range
			this.SetStatusRange(0, 100);

#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif


            switch (type)
            {
                case FlipRotateImageCommandSettings.Actions.FlipHorizontal:
                    this.SetStatusText("Flipping horizontal...");
                    image.FlipHoz();
                    break;
                case FlipRotateImageCommandSettings.Actions.FlipVertical:
                    this.SetStatusText("Flipping vertical...");
                    image.FlipVer();
                    break;
                case FlipRotateImageCommandSettings.Actions.Rotate90CW:
                    this.SetStatusText("Rotating 90 Clockwise...");
                    image.Rotate(-90);
                    break;
                case FlipRotateImageCommandSettings.Actions.Rotate90CCW:
                    this.SetStatusText("Rotating 90 Counter-Clockwise...");
                    image.Rotate(90);
                    break;
                case FlipRotateImageCommandSettings.Actions.Rotate180:
                    this.SetStatusText("Rotating 180...");
                    image.Rotate(180);
                    break;
                case FlipRotateImageCommandSettings.Actions.RotateByAngle:
                    this.SetStatusText("Rotating image by a specified angle...");
                    image.Rotate(angle);
                    break;
                default:
                    break;
            }

#if DEBUG_METETIME
            dm.AddLine("RotateImageCommand:Run");
            dm.Write2Debug(true);
#endif
		}
#endif
    }
}
