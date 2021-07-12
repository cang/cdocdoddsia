using System;
using System.Windows.Forms;
using System.Drawing;

using SiGlaz.Common;

namespace SIA.UI.MaskEditor.DrawTools
{
	/// <summary>
	/// Line tool
	/// </summary>
	public class ToolLine : DrawTools.ToolObject
	{
        public ToolLine(IMaskEditor editor) : base(editor)
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
            AddNewObject(drawArea, new DrawLine(e.X, e.Y, e.X + 1, e.Y + 1));
        }

        public override void OnMouseMove(DrawArea drawArea, MouseEventArgsF e)
        {
            drawArea.Cursor = Resources.ApplicationCursors.DrawLine;

            if ( (e.Button & MouseButtons.Left) == MouseButtons.Left )
            {
                PointF point = new PointF(e.X, e.Y);
                drawArea.GraphicsList[0].MoveHandleTo(point, 2);
                drawArea.Refresh();
            }
        }
    }
}
