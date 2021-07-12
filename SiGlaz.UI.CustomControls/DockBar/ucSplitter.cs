using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;

namespace SiGlaz.UI.CustomControls.DockBar
{
    public partial class ucSplitter : Splitter
    {
        #region Members
        private Pen _pen = new Pen(Color.Gray);   
        #endregion

        #region Constructor
        public ucSplitter()
        {
            InitializeComponent();
        }
        #endregion

        #region Override Methods
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            switch (Dock)
            {
                case DockStyle.Left:
                    e.Graphics.DrawLine(_pen, 0, 0, 0, this.Height - 1);
                    break;

                case DockStyle.Right:
                    e.Graphics.DrawLine(_pen, this.Width - 1, 0, this.Width - 1, this.Height - 1);
                    break;

                case DockStyle.Top:
                    e.Graphics.DrawLine(_pen, 0, 0, this.Width - 1, 0);
                    break;

                case DockStyle.Bottom:
                    e.Graphics.DrawLine(_pen, 0, this.Height - 1, this.Width - 1, this.Height - 1);
                    break;                

                default:
                    e.Graphics.DrawRectangle(_pen, 0, 0, this.Width - 1, this.Height - 1);
                    break;

            }
            
        }
        #endregion
    }
}
