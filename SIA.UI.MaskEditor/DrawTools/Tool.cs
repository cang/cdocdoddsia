using System;
using System.Windows.Forms;
using System.Drawing;

using SiGlaz.Common;


namespace SIA.UI.MaskEditor.DrawTools
{
	/// <summary>
	/// Base class for all drawing tools
	/// </summary>
	public abstract class Tool
	{
		/// <summary>
		/// Reset tools properties
		/// </summary>
		public virtual void Reset()
		{
		}

        /// <summary>
        /// Left nous button is pressed
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public virtual void OnMouseDown(DrawArea drawArea, MouseEventArgsF e)
        {
        }


        /// <summary>
        /// Mouse is moved, left mouse button is pressed or none button is pressed
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public virtual void OnMouseMove(DrawArea drawArea, MouseEventArgsF e)
        {
        }


        /// <summary>
        /// Left mouse button is released
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public virtual void OnMouseUp(DrawArea drawArea, MouseEventArgsF e)
        {
        }
    }
}
