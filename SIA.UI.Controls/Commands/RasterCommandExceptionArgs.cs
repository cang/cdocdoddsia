using System;
using System.Text;

namespace SIA.UI.Controls.Commands
{
	public delegate void RasterCommandExceptionHandler(object sender, RasterCommandExceptionArgs arg);
	
	/// <summary>
	/// Summary description for RasterCommandExceptionArgs.
	/// </summary>
	public class RasterCommandExceptionArgs	
	{
		private DateTime _atTime = DateTime.Now;
		private System.Exception _exception = null;
		private RasterCommand _command;

		public RasterCommand RasterCommand
		{
			get {return _command;}
		}

		public Exception Exception
		{
			get {return _exception;}
		}

		public RasterCommandExceptionArgs(RasterCommand command, System.Exception exp) 
		{
			_atTime = DateTime.Now;
			_exception = exp;
			_command = command;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("Date: " + _atTime.ToShortDateString());
			sb.Append("Time: " + _atTime.ToShortTimeString());
			sb.Append("\r\n");
			sb.Append("\t Raster Command : " + this._command != null ? this._command.GetType().Name : "Unknown");
			sb.Append("\r\n");
			sb.Append("\t Exception: " + this._exception != null ? this._exception.ToString() : "Unknown exception");
			return sb.ToString();
		}

	}
}
