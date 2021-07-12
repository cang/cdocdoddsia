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
	/// Summary description for HistoryRedoCommand.
	/// </summary>
	public class HistoryRedoCommand : RasterCommand
	{
		public HistoryRedoCommand(IProgressCallback callback) : base(callback)
		{
		}

		protected override void ValidateArguments(params object[] args)
		{
			if (args == null)
				throw new ArgumentNullException("arguments");
			if (args.Length < 1)
				throw new ArgumentException("Not enough arguments", "arguments");
			if (args[0] is HistoryHelper == false)
				throw new ArgumentException("Argument type does not match. Arguments[0] must be HistoryHelper", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
			HistoryHelper helper = (HistoryHelper)args[0];

			this.SetStatusRange(0, 100);
			this.SetStatusText("Redo action...");

			helper.Redo();
		}
	}
}
