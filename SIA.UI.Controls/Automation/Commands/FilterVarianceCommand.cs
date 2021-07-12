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

namespace SIA.UI.Controls.Automation.Commands
{
	/// <summary>
	/// Summary description for FilterVarianceCommand.
	/// </summary>
	public class FilterVarianceCommand : AutoCommand
	{
		public FilterVarianceCommand(IProgressCallback callback) : base(callback)
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
			if (args[1] is FilterVarianceCommandSettings == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be FilterVarianceCommandSettings", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;

			FilterVarianceCommandSettings cmdSettings = (FilterVarianceCommandSettings)args[1];
			float radius = cmdSettings.Radius;

			this.SetStatusRange(0, 100);
			this.SetStatusText("Filtering image...");

			this.Variance(image, radius);			
		}

		public override void AutomationRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;

			FilterVarianceCommandSettings cmdSettings = (FilterVarianceCommandSettings)args[1];
			float radius = cmdSettings.Radius;

			this.Variance(image, radius);			
		}

		protected virtual void Variance(CommonImage image, float radius)
		{
			try
			{
				image.FilterVariance(radius);
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
