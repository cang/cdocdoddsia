using System;

namespace SIA.UI.Controls.Automation
{
	/// <summary>
	/// Provides detailed data when failed to validate script 
	/// </summary>
	public class ScriptValidationException 
        : System.Exception
	{
		public ScriptValidationException(string message) 
            : base(message)
		{
		}

		public ScriptValidationException(string message, System.Exception innerExp) 
            : base(message, innerExp)
		{
		}
	}
}
