using System;

using SiGlaz.Common;

namespace SIA.UI.MaskEditor
{
	/// <summary>
	/// Summary description for MaskEditorApplyMaskEventArgs.
	/// </summary>
	public class MaskEditorApplyMaskEventArgs : System.EventArgs
	{
		private GraphicsList _objects;

		public GraphicsList GraphicsList
		{
			get {return _objects;}
		}

		public MaskEditorApplyMaskEventArgs(GraphicsList objects)
		{
			_objects = objects;
		}
	}
}
