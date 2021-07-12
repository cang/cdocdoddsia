using System;
using System.Windows.Forms;

using SiGlaz.Common;

namespace SIA.UI.MaskEditor.DrawTools
{
	/// <summary>
	/// Ellipse tool
	/// </summary>
	public class ToolEllipse : DrawTools.ToolRectangle
	{
		public ToolEllipse(IMaskEditor editor) : base(editor)
		{
            Cursor = Cursors.Arrow;
		}

		public override void Reset()
		{
			base.Reset ();
			Cursor = Cursors.Arrow;
		}


        public override void OnMouseDown(DrawArea drawArea, MouseEventArgsF e)
        {
            AddNewObject(drawArea, new SIA.UI.MaskEditor.DrawTools.DrawEllipse(e.X, e.Y, 1, 1));
        }

		public override void OnMouseMove(DrawArea drawArea, MouseEventArgsF e)
		{		
			base.OnMouseMove (drawArea, e);

			// apply draw ellipse cursor
			drawArea.Cursor = Resources.ApplicationCursors.DrawEllipse;
		}


	}
}
