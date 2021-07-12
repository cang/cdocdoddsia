using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;

namespace SiGlaz.UI.CustomControls.DockBar
{
    public partial class ucTabStrip : ToolStrip
    {
        #region Members
        private Pen _pen = new Pen(Color.Gray);
        private DockStyle _dockbarStyle = DockStyle.Left;
        #endregion

        #region Constructor
        public ucTabStrip()
        {
            InitializeComponent();
            base.BackColor = SystemColors.ControlLight;
            base.GripStyle = ToolStripGripStyle.Hidden;
            base.ShowItemToolTips = false;
        }
        #endregion

        #region Properties
        public DockStyle DockbarStyle
        {
            get { return _dockbarStyle; }
            set { _dockbarStyle = value; }
        }
        #endregion

        #region Override Methods
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            switch (_dockbarStyle)
            {
                
                case DockStyle.Left:
                    e.Graphics.DrawLine(_pen, 0, 0, base.Width - 3, 0);
                    e.Graphics.DrawLine(_pen, 0, base.Height - 1, base.Width - 3, base.Height - 1);
                    e.Graphics.DrawLine(_pen, 0, 0, 0, base.Height - 1);
                    e.Graphics.DrawLine(_pen, base.Width - 1, 2, base.Width - 1, base.Height - 3);

                    e.Graphics.DrawLine(_pen, base.Width - 3, 0, base.Width - 1, 2);
                    e.Graphics.DrawLine(_pen, base.Width - 3, base.Height - 1, base.Width - 1, base.Height - 3);

                    break;

                case DockStyle.Right:
                    e.Graphics.DrawLine(_pen, 2, 0, base.Width - 1, 0);
                    e.Graphics.DrawLine(_pen, 2, base.Height - 1, base.Width - 1, base.Height - 1);
                    e.Graphics.DrawLine(_pen, base.Width - 1, 0, base.Width - 1, base.Height - 1);
                    e.Graphics.DrawLine(_pen, 0, 2, 0, base.Height - 3);

                    e.Graphics.DrawLine(_pen, 2, 0, 0, 2);
                    e.Graphics.DrawLine(_pen, 0, base.Height - 3, 2, base.Height - 1);
                    break;

                case DockStyle.Top:
                    e.Graphics.DrawLine(_pen, 0, 0, base.Width - 1, 0);
                    e.Graphics.DrawLine(_pen, 0, 0, 0, base.Height - 3);
                    e.Graphics.DrawLine(_pen, base.Width - 1, 0, base.Width - 1, base.Height - 3);
                    e.Graphics.DrawLine(_pen, 2, base.Height - 1, base.Width - 3, base.Height - 1);

                    e.Graphics.DrawLine(_pen, 0, base.Height - 3, 2, base.Height - 1);
                    e.Graphics.DrawLine(_pen, base.Width - 1, base.Height - 3, base.Width - 3, base.Height - 1);
                    break;

                case DockStyle.Bottom:
                    e.Graphics.DrawLine(_pen, 0, 2, 0, base.Height - 1);
                    e.Graphics.DrawLine(_pen, base.Width - 1, 2, base.Width - 1, base.Height - 1);
                    e.Graphics.DrawLine(_pen, 0, base.Height - 1, base.Width - 1, base.Height - 1);
                    e.Graphics.DrawLine(_pen, 2, 0, base.Width - 3, 0);

                    e.Graphics.DrawLine(_pen, 0, 2, 2, 0);
                    e.Graphics.DrawLine(_pen, base.Width - 3, 0, base.Width - 1, 2);
                    break;
            }
        }
        #endregion
    }
}
