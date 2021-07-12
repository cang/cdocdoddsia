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

namespace  SIA.UI.Controls.Automation.Commands
{
	/// <summary>
	/// Summary description for SmoothFilterCommand.
	/// </summary>
	public class SmoothFilterCommand : AutoCommand
	{
		public SmoothFilterCommand(IProgressCallback callback) : base(callback)
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
			this.SetStatusText("Smooth filtering...");
			
			this.SmoothFilter(image);
		}

		public override void AutomationRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
			
			this.SmoothFilter(image);
		}

		private void SmoothFilter(CommonImage image)
		{
			try
			{
				// [20070618 Cong];  wrapper kFilter function
				eMaskType maskType = eMaskType.kMASK_SMOOTH;
				eMatrixType matrixType = eMatrixType.kMATRIX_3x3;
				int num_pass = 1;
				float threshold = 0.0f;
				image.kApplyFilter(maskType, matrixType, num_pass, threshold);
			}
			catch
			{
				throw;
			}
			finally
			{
			}
		}
	}
}
