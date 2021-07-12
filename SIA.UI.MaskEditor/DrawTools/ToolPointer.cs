using System;
using System.Windows.Forms;
using System.Drawing;

using SiGlaz.Common;

namespace SIA.UI.MaskEditor.DrawTools
{
	/// <summary>
	/// Pointer tool
	/// </summary>
	public class ToolPointer : DrawTools.Tool
	{
        public enum SelectionMode
        {
            None,
            NetSelection,   // group selection is active
            Move,           // object(s) are moves
            Size            // object is resized
        }

        private SelectionMode selectMode = SelectionMode.None;

        // Object which is currently resized:
        private DrawObject resizedObject;
        private int resizedObjectHandle;

        // Keep state about last and current point (used to move and resize objects)
        private PointF lastPoint = new PointF(0,0);
        private PointF startPoint = new PointF(0, 0);

		public ToolPointer()
		{
		}

		public SelectionMode ActiveMode
		{
			get {return selectMode;}
		}

        /// <summary>
        /// Left mouse button is pressed
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(DrawArea drawArea, MouseEventArgsF e)
        {
            selectMode = SelectionMode.None;
            PointF point = new PointF(e.X, e.Y);

            // Test for resizing (only if control is selected, cursor is on the handle)
            int n = drawArea.GraphicsList.SelectionCount;

            for ( int i = 0; i < n; i++ )
            {
                DrawObject o = drawArea.GraphicsList.GetSelectedObject(i);
                int handleNumber = o.HitTest(point);

                if ( handleNumber > 0 )
                {
                    selectMode = SelectionMode.Size;

                    // keep resized object in class members
                    resizedObject = o;
                    resizedObjectHandle = handleNumber;

                    // Since we want to resize only one object, unselect all other objects
                    drawArea.GraphicsList.UnselectAll();
					if (o.Selected == false)
					{
						o.Selected = true;
					}

					drawArea.CommitUserAction();

                    break;
                }
            }

            // Test for move (cursor is on the object)
            if ( selectMode == SelectionMode.None )
            {
                int n1 = drawArea.GraphicsList.Count;
                DrawObject o = null;

                for ( int i = 0; i < n1; i++ )
                {
                    if ( drawArea.GraphicsList[i].HitTest(point) == 0 )
                    {
                        o = drawArea.GraphicsList[i];
                        break;
                    }
                }

                if ( o != null )
                {
                    selectMode = SelectionMode.Move;

                    // Unselect all if Ctrl is not pressed and clicked object is not selected yet
                    if ( ( Control.ModifierKeys & Keys.Control ) == 0  && !o.Selected )
                        drawArea.GraphicsList.UnselectAll();

                    // Select clicked object
					o.Selected = true;
				    drawArea.Cursor = Cursors.SizeAll;
                }
            }

            // Net selection
            if ( selectMode == SelectionMode.None )
            {
                // click on background
                if ( ( Control.ModifierKeys & Keys.Control ) == 0 )
                    drawArea.GraphicsList.UnselectAll();

                selectMode = SelectionMode.NetSelection;
                drawArea.DrawNetRectangle = true;
            }

            lastPoint.X = e.X;
            lastPoint.Y = e.Y;
            startPoint.X = e.X;
            startPoint.Y = e.Y;

            drawArea.Capture = true;

            drawArea.NetRectangle = DrawRectangle.GetNormalizedRectangle(startPoint, lastPoint);
            drawArea.Refresh();

			drawArea.RaiseRefreshUIObjects(EventArgs.Empty);
        }


        /// <summary>
        /// Mouse is moved.
        /// None button is pressed, ot left button is pressed.
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseMove(DrawArea drawArea, MouseEventArgsF e)
        {
            PointF point = new PointF(e.X, e.Y);
            //point = PointToLogical(point);

            // set cursor when mouse button is not pressed
			bool noButtonPressed = 0 == (int)(e.Button & (MouseButtons.Left | MouseButtons.Right | MouseButtons.Middle));
            if ( noButtonPressed )
            {
                Cursor cursor = null;

                for ( int i = 0; i < drawArea.GraphicsList.Count; i++ )
                {
                    int n = drawArea.GraphicsList[i].HitTest(point);

                    if (n > 0)
                    {
                        cursor = drawArea.GraphicsList[i].GetHandleCursor(n);
                        break;
                    }					
                }

                if (cursor == null)
                    cursor = Cursors.Default;

                drawArea.Cursor = cursor;

                return;
            }

            if ((e.Button & MouseButtons.Left) != MouseButtons.Left)
                return;

			// Find difference between previous and current position
            float dx = e.X - lastPoint.X;
            float dy = e.Y - lastPoint.Y;

			// resize mode
            if ( selectMode == SelectionMode.Size )
            {
                if ( resizedObject != null )
                {	
					resizedObject.SetLastPoint(lastPoint);
					resizedObject.MoveHandleTo(point, resizedObjectHandle);
					drawArea.SetDirty();
					drawArea.Refresh();
                }
            }

            // move
            if ( selectMode == SelectionMode.Move )
            {
                int n = drawArea.GraphicsList.SelectionCount;

                for ( int i = 0; i < n; i++ )
                {
                    drawArea.GraphicsList.GetSelectedObject(i).Move(dx, dy);
                }

                drawArea.Cursor = Cursors.SizeAll;
                drawArea.SetDirty();
                drawArea.Refresh();
            }

			lastPoint.X = e.X;
			lastPoint.Y = e.Y;

            if ( selectMode == SelectionMode.NetSelection )
            {
                drawArea.NetRectangle = DrawRectangle.GetNormalizedRectangle(startPoint, lastPoint);
                drawArea.Refresh();
                return;
            }

        }
		
		/// <summary>
        /// Right mouse button is released
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseUp(DrawArea drawArea, MouseEventArgsF e)
        {
            if ( selectMode == SelectionMode.NetSelection )
            {
                // Group selection
                drawArea.GraphicsList.SelectInRectangle(drawArea.NetRectangle);

                selectMode = SelectionMode.None; 
                drawArea.DrawNetRectangle = false;
            }
			
			if ( resizedObject != null )
            {
                // after resizing
                resizedObject.Normalize();
                resizedObject = null;
            }

            drawArea.SelectedVertex = null;
            PointF point = new PointF(e.X, e.Y);
            int handle = -1;
            for (int i = 0; i < drawArea.GraphicsList.Count; i++)
            {
                handle = drawArea.GraphicsList[i].HitTest(point);
                if (handle > 0)
                {
                    try
                    {
                        drawArea.SelectedVertex = (drawArea.GraphicsList.GetSelectedObject(0) as SiGlaz.Common.DrawPolygon).pointArray[handle - 1];
                    }
                    catch
                    { }
                    break;
                }
            }

			if (selectMode != SelectionMode.None)
				drawArea.CommitUserAction();
			
			drawArea.Cursor = Cursors.Default;                
			drawArea.Capture = false;
            drawArea.Refresh();
			drawArea.RaiseRefreshUIObjects(EventArgs.Empty);
        }
	}
}
