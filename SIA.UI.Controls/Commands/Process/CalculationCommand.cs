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
using SIA.Common;

using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Automation;
using SIA.UI.Controls.Automation.Commands;
using SIA.Algorithms;

namespace SIA.UI.Controls.Commands
{
	/// <summary>
	/// Implements the calculation function
	/// </summary>
	public class CalculationCommand 
        : AutoCommand
	{
		public CalculationCommand(IProgressCallback callback) 
            : base(callback)
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
			if (args[1] is CalculationCommandSettings == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be CalculationCommandSettings", "arguments");			
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
			CalculationCommandSettings cmdSettings = (CalculationCommandSettings)args[1];

			eCalculationType calculationType = cmdSettings.Type;

#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif


			if (calculationType == eCalculationType.Monadic)
			{				
				string type = cmdSettings.MonadicType;
				float value = cmdSettings.Value;

				this.Monadic(image, type, value);	
			}
			else if (calculationType == eCalculationType.Dyadic)
			{
				string type = cmdSettings.DyadicType;
				string filename = cmdSettings.FileName;

				this.Dyadic(image, type, filename);			
			}

#if DEBUG_METETIME
            dm.AddLine("CalculationCommand:Run");
            dm.Write2Debug(true);
#endif

		}

		public override void AutomationRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
			CalculationCommandSettings cmdSettings = (CalculationCommandSettings)args[1];

            using (CommandProgressLocker locker = new CommandProgressLocker())
            {
                eCalculationType calculationType = cmdSettings.Type;
                if (calculationType == eCalculationType.Monadic)
                {
                    string type = cmdSettings.MonadicType;
                    float value = cmdSettings.Value;

                    this.Monadic(image, type, value);
                }
                else if (calculationType == eCalculationType.Dyadic)
                {
                    string type = cmdSettings.DyadicType;
                    string filename = cmdSettings.FileName;

                    this.Dyadic(image, type, filename);
                }
            }
		}

#if GPU_SUPPORTED
        private void Monadic(CommonImage image, string type, float value)
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

        private void Dyadic(CommonImage image, string type, String filename)
        {
            //this code will be removed in feature
            bool bCreateBuff = !image.HasDeviceBuffer;
            if (bCreateBuff)
                image.CreateDeviceBuffer();


            using (CommonImage refImage = CommonImage.FromFile(filename))
                image.kDyadicOperationGPU(type, refImage);

            //this code will be removed in feature
            image.ReadDataFromDeviceBuffer();
            if (bCreateBuff)
                image.DisposeDeviceBuffer();
        }
#else
        private void Monadic(CommonImage image, string type, float value)
        {
            //#if USING_IPL
            //            image.kMonadicOperation(type, value, false);
            //#else
            //            using (IplCommonImage iplImage = new IplCommonImage(image))
            //            {
            //                using (Calculation iplProcess = new Calculation(IplProgressCallback.Instance))
            //                {
            //                    switch (type)
            //                    {
            //                        case "ADD":
            //                            iplProcess.Add(iplImage, value, iplImage);
            //                            break;
            //                        case "SUB":
            //                            iplProcess.Subtract(iplImage, value, iplImage);
            //                            break;
            //                        case "MUL":
            //                            iplProcess.Multiply(iplImage, value, iplImage);
            //                            break;
            //                        case "DIV":
            //                            iplProcess.Divide(iplImage, value, iplImage);
            //                            break;
            //                        default:
            //                            throw new ArgumentException();
            //                    }
            //                }
            //            }
            //#endif


            image.kMonadicOperation(type, value, false);
        }

        private void Dyadic(CommonImage image, string type, String filename)
        {
            using (CommonImage refImage = CommonImage.FromFile(filename))
                image.kDyadicOperation(type, refImage, false);
        }
#endif
	}
}
