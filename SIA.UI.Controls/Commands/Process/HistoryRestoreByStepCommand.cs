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
using SIA.UI.Controls.Helpers;

namespace SIA.UI.Controls.Commands
{
	/// <summary>
	/// Summary description for HistoryRestoreByStepCommand.
	/// </summary>
	public class HistoryRestoreByStepCommand : RasterCommand
	{
		public HistoryRestoreByStepCommand(IProgressCallback callback) : base(callback)
		{
		}

		protected override void ValidateArguments(params object[] args)
		{
			if (args == null)
				throw new ArgumentNullException("arguments");
			if (args.Length < 2)
				throw new ArgumentException("Not enough arguments", "arguments");
			if (args[0] is HistoryHelper == false)
				throw new ArgumentException("Argument type does not match. Arguments[0] must be HistoryHelper", "arguments");
			if (args[1] is int == false)
				throw new ArgumentException("Argument type does not match. Arguments[1] must be integer", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
			HistoryHelper helper = (HistoryHelper)args[0];
			int stepIndex = (int)args[1];

			this.SetStatusRange(0, 100);
			this.SetStatusText("Restoring from history...");

			helper.RestoreFromHistory(stepIndex);
		}
	}
}
