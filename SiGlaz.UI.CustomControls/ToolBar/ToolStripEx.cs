using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SiGlaz.UI.CustomControls
{
    public class ToolStripEx : ToolStrip
    {
        //public ToolStripEx() : base()
        //{
        //}

        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    base.OnPaint(e);
        //}

        //protected override void OnPaintBackground(PaintEventArgs e)
        //{
        //    //base.OnPaintBackground(e);

        //    Graphics g = e.Graphics;
        //    GraphicsState gstate = g.Save();
        //    try
        //    {
        //        using (Region region = this.Renderer.GetTransparentRegion(this))
        //        {
        //            if (region != null)
        //            {
        //                this.EraseCorners(e, region);
        //                g.ExcludeClip(region);
        //            }
        //        }
        //        this.Renderer.DrawToolStripBackground(
        //            new ToolStripRenderEventArgs(g, this));
        //    }
        //    finally
        //    {
        //        if (gstate != null)
        //        {
        //            g.Restore(gstate);
        //        }
        //    }

        //}

        //protected override void OnPaintGrip(PaintEventArgs e)
        //{
        //    base.OnPaintGrip(e);
        //}
    }
}
