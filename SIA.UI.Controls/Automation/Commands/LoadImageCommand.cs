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

namespace SIA.UI.Controls.Automation.Commands
{
	/// <summary>
	/// Summary description for LoadImageCommand.
	/// </summary>
	public class LoadImageCommand : AutoCommand
	{
		private CommonImage _image = null;
		public CommonImage Image 
		{
			get
			{
				return _image;
			}
		}

		public LoadImageCommand(IProgressCallback callback) : base(callback)
		{
		}

		protected override void UninitClass()
		{
			base.UninitClass ();

			// clear references
			_image = null;
		}


		protected override void ValidateArguments(params object[] args)
		{
			if (args == null)
				throw new ArgumentNullException("arguments");
			if (args.Length < 1)
				throw new ArgumentException("Not enough arguments", "arguments");
			if (args[0] is LoadImageCommandSettings == false)
				throw new ArgumentException("Argument type does not match. Arguments[0] must be LoadImageCommandSettings", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
			LoadImageCommandSettings cmdSettings = (LoadImageCommandSettings)args[0];
			
			string filename = cmdSettings.FileName;

			this.SetStatusRange(0, 100);
			this.SetStatusText("Loading image...");

			this._image = this.LoadImage(filename);			
		}

		public override void AutomationRun(params object[] args)
		{
			LoadImageCommandSettings cmdSettings = (LoadImageCommandSettings)args[0];
			
			string filename = cmdSettings.FileName;
			
			this._image = this.LoadImage(filename);			
		}

		public override object[] GetOutput()
		{
			return new object[] {_image};
		}

		protected virtual CommonImage LoadImage(string filename)
		{
			CommonImage image = null;

			try
			{
				image = CommonImage.FromFile(filename);		
			}
			catch
			{
				if (image != null)
					image.Dispose();
				image = null;

                string msg = string.Format("Cannot load image file: {0} due to: file is invalid format or incomplete.", filename);

                //Trace.WriteLine("Cannot load image file: {0}", filename);

                throw new System.Exception(msg);
			}
			finally
			{	
			}

			return image;
		}
	}
}
