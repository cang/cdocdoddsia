using System;
using System.Windows.Forms;
using System.Drawing;

using SiGlaz.Common;

namespace SIA.UI.MaskEditor.DrawTools
{
	/// <summary>
	/// Base class for all tools which create new graphic object
	/// </summary>
	public abstract class ToolObject : DrawTools.Tool
	{
		private IMaskEditor _maskEditor = null;
        private Cursor cursor;


		public IMaskEditor MaskEditor
		{
			get {return _maskEditor;}
		}

        /// <summary>
        /// Tool cursor.
        /// </summary>
        public Cursor Cursor
        {
            get
            {
                return cursor;
            }
            set
            {
                cursor = value;
            }
        }

		public ToolObject(IMaskEditor editor)
		{
			_maskEditor = editor;
		}


        /// <summary>
        /// Left mouse is released.
        /// New object is created and resized.
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseUp(DrawArea drawArea, MouseEventArgsF e)
        {
			if (drawArea.GraphicsList.Count>0 && drawArea.GraphicsList[0]!=null)
				drawArea.GraphicsList[0].Normalize();
            drawArea.ActiveTool = DrawToolType.Pointer;
    
			drawArea.Capture = false;
            drawArea.Refresh();

			// commit user action
			drawArea.CommitUserAction();
        }

        /// <summary>
        /// Add new object to draw area.
        /// Function is called when user left-clicks draw area,
        /// and one of ToolObject-derived tools is active.
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="o"></param>
        protected void AddNewObject(DrawArea drawArea, DrawObject o)
        {
            drawArea.GraphicsList.UnselectAll();
			o.Container = drawArea.GraphicsList;
            o.Selected = true;
            drawArea.GraphicsList.Add(o);

            drawArea.Capture = true;
            drawArea.Refresh();

            drawArea.SetDirty();
        }
	}
}
