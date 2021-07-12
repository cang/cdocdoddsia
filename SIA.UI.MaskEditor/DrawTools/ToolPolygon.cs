using System;
using System.Windows.Forms;
using System.Drawing;

using SiGlaz.Common;

namespace SIA.UI.MaskEditor.DrawTools
{
	/// <summary>
	/// Polygon tool
	/// </summary>
	public class ToolPolygon : DrawTools.ToolObject
	{
		private DrawPolygon newPolygon;
		private int lastX;
		private int lastY;

		public ToolPolygon(IMaskEditor editor) : base(editor)
		{
            Cursor = Cursors.Arrow;
        }

		public override void Reset()
		{
			base.Reset ();
			newPolygon = null;
			lastX = 0;
			lastY = 0;
		}
        
        /// <summary>
        /// Left mouse button is pressed
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(DrawArea drawArea, MouseEventArgsF e)
        {
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				if (newPolygon == null)
				{
					// Create new polygon, add it to the list
					// and keep reference to it
					newPolygon = new SIA.UI.MaskEditor.DrawTools.DrawPolygon();
					newPolygon.AddPoint(new PointF(e.X, e.Y));
					AddNewObject(drawArea, newPolygon);

					// disable context menu for closed polygon
					drawArea.ContextMenuEnabled = false;
					// disable hot key for delete
					drawArea.HotKeyEnabled = false;
				}
				else
				{
					// add new point of created polygon
					newPolygon.AddPoint(new PointF(e.X, e.Y));
				}
				lastX = e.X;
				lastY = e.Y;
				drawArea.Invalidate();
			}
        }

        /// <summary>
        /// Mouse move - resize new polygon
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseMove(DrawArea drawArea, MouseEventArgsF e)
        {
            drawArea.Cursor = Cursor;

            if ( (e.Button & MouseButtons.Left) != MouseButtons.Left )
                return;

            if ( newPolygon == null )
                return;                 // precaution

            PointF point = new PointF(e.X, e.Y);
            // move last point
            newPolygon.MoveHandleTo(point, newPolygon.HandleCount);
            drawArea.Invalidate();
        }

        public override void OnMouseUp(DrawArea drawArea, MouseEventArgsF e)
        {
			if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
			{	
                if (newPolygon != null)
                {
				    // remove selected object
				    if (newPolygon.Count <= 2)
					    drawArea.GraphicsList.Remove(newPolygon);
    				
				    // clear all stuff
				    newPolygon = null;
				    base.OnMouseUp (drawArea, e);

				    // enable context menu of draw area
				    drawArea.ContextMenuEnabled = true;
				    // enable hot key for delete
				    drawArea.HotKeyEnabled = true;

				    drawArea.Invalidate();
                }
                else
                {
                    
                }
			}
        }

		public bool Closed
		{
			get
			{
				return (this.newPolygon == null);
			}
			set
			{
				this.newPolygon = null;
			}
		}
	}
}
