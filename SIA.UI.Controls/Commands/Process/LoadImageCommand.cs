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

namespace SIA.UI.Controls.Commands
{
	/// <summary>
	/// Summary description for LoadImageCommand.
	/// </summary>
	public class LoadImageCommand : RasterCommand
	{
		private CommonImage _image = null;

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
            if (args[0] is String == false && args[0] is FileStream == false && args[0] is MemoryStream == false)
				throw new ArgumentException("Argument type does not match. Arguments[0] must be String or FileStream", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
            if (args[0] is String)
            {
                string filename = args[0] as String;

                this.SetStatusRange(0, 100);
                this.SetStatusText("Loading image...");

                this._image = this.LoadImage(filename);
            }
            else if (args[0] is FileStream)
            {
                FileStream fs = args[0] as FileStream;

                this.SetStatusRange(0, 100);
                this.SetStatusText("Loading image...");

                this._image = this.LoadImage(fs);
            }
            else if (args[0] is MemoryStream)
            {
                MemoryStream fs = args[0] as MemoryStream;

                this.SetStatusRange(0, 100);
                this.SetStatusText("Loading image...");

                this._image = this.LoadImage(fs);
            }
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

				throw;
			}
			finally
			{	
			}

			return image;
		}

        protected virtual CommonImage LoadImage(FileStream fs)
        {
            CommonImage image = null;

            try
            {
                image = CommonImage.FromFileStream(fs);
            }
            catch
            {
                if (image != null)
                    image.Dispose();
                image = null;

                throw;
            }
            finally
            {
            }

            return image;
        }

        protected virtual CommonImage LoadImage(MemoryStream fs)
        {
            CommonImage image = null;

            try
            {
                image = CommonImage.FromMemoryStream(fs);
            }
            catch
            {
                if (image != null)
                    image.Dispose();
                image = null;

                throw;
            }
            finally
            {
            }

            return image;
        }
	}
}
