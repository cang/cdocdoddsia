using System;
using System.Windows.Forms;
using System.Drawing;

using SiGlaz.Common;

namespace SIA.UI.MaskEditor.DrawTools
{
	/// <summary>
	/// Rectangle tool
	/// </summary>
	public class ToolRectangle : DrawTools.ToolObject
	{
		public ToolRectangle(IMaskEditor editor) : base(editor)
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
            AddNewObject(drawArea, new SIA.UI.MaskEditor.DrawTools.DrawRectangle(e.X, e.Y, 1, 1));			
        }

        public override void OnMouseMove(DrawArea drawArea, MouseEventArgsF e)
        {
			drawArea.Cursor = Resources.ApplicationCursors.DrawRectangle;
				
            if ( (e.Button & MouseButtons.Left) == MouseButtons.Left )
            {
				PointF point = new PointF(e.X, e.Y);
				drawArea.GraphicsList[0].MoveHandleTo(point, 5);
				drawArea.Refresh();				
            }
        }
	}
}
