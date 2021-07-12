using System;
using System.Threading;
using System.Data;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using System.Text;
using System.Diagnostics;

using SIA.Common;
using SIA.UI.Controls;
using SIA.UI.Controls.Common;
using SIA.UI.Controls.Commands;

using SIA.SystemLayer;

using SIA.UI.Controls.Automation;
using SIA.UI.Controls.Automation.Steps;

using SIA.Workbench.Common;
using SIA.Workbench.Common.InterprocessCommunication.SharedMemory;

namespace SIA.UI.Controls.Automation
{
	/// <summary>
	/// Customized script
	/// </summary>
	public class ScriptV2 : Script
	{
		public ScriptV2()
		{
			// new version
			this.Version = 2;
		}
	}
}
