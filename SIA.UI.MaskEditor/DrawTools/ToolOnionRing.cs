using System;
using System.Windows.Forms;
using System.Drawing;

using SiGlaz.Common;

namespace SIA.UI.MaskEditor.DrawTools
{
	/// <summary>
	/// Summary description for ToolOnionRing.
	/// </summary>
	public class ToolOnionRing : DrawTools.ToolRectangle
	{
		private DrawOnionRing _onionRing = null;
		private PointF _pointLast = PointF.Empty;

		public ToolOnionRing(IMaskEditor editor) : base(editor)
		{
			Cursor = Cursors.Arrow;
		}

		public override void Reset()
		{
			base.Reset ();
			_onionRing = null;
			_pointLast = PointF.Empty;
		}


		public override void OnMouseDown(DrawArea drawArea, MouseEventArgsF e)
		{
			drawArea.Cursor = Resources.ApplicationCursors.DrawOnionRing;

			_onionRing = new DrawOnionRing(e.X, e.Y, 5, 5);
			AddNewObject(drawArea, _onionRing);							
			_pointLast = new PointF(e.X, e.Y);
		}

		public int CheckPossitionOfPointer(PointF pRoot,PointF pCurrent)
		{
			if (pCurrent.X < pRoot.X )
			{
				if (pCurrent.Y < pRoot.Y)
					return 1;
				else
					return 4;
			}		
			else
			{
				if (pCurrent.Y < pRoot.Y)
					return 2;
				else
					return 3;
			}
		}

		public override void OnMouseMove(DrawArea drawArea, MouseEventArgsF e)
		{
			drawArea.Cursor = Cursor;
			
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				float dx = e.X - _pointLast.X;
				float dy = e.Y - _pointLast.Y;				
				
				// kiem tra nen move handle nao tuy the vi tri tuong doi cua con tro voi rectangle cua Object
				// move handle duoc chon							

				
				drawArea.GraphicsList[0].MoveHandleTo(new PointF(e.X, e.Y), 3);
				//drawArea.GraphicsList[0].MoveHandleTo(dx,dy, 3);
				drawArea.Refresh();				
				_pointLast = new PointF(e.X, e.Y);

				
			}
		}

	}
}
