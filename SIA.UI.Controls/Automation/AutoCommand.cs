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

using SIA.UI.Controls.Commands;

namespace SIA.UI.Controls.Automation
{
	/// <summary>
	/// Represents the command that will be used by RDE Monitor
	/// </summary>
	public abstract class AutoCommand 
        : RasterCommand
	{
		#region Constructor and destructor
		
		public AutoCommand()
		{
		}

		public AutoCommand(IProgressCallback callback) 
            : base(callback)
		{
		}
		
		#endregion

		#region Abstract Methods

		public abstract void AutomationRun(params object[] args);

		#endregion
	}
}
