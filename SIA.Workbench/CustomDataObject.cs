using System;
using System.Collections;
using System.Windows.Forms;

namespace SIA.Workbench
{
	/// <summary>
	/// Summary description for CustomDataObject.
	/// </summary>
	internal class CustomDataObject : DataObject
	{
		private string[] _formats = null;

		public CustomDataObject(string format)
		{
			_formats = new string[] {format};
		}

		public CustomDataObject(string[] formats)
		{
			_formats = (string[])formats.Clone();
		}

		public override string[] GetFormats()
		{
			return _formats;
		}

		public override string[] GetFormats(bool autoConvert)
		{
			return _formats;
		}

		public override object GetData(Type format)
		{
			return base.GetData (format);
		}

		public override object GetData(string format)
		{
			return base.GetData (format);
		}

		public override object GetData(string format, bool autoConvert)
		{
			return base.GetData (format, autoConvert);
		}


	}
}

