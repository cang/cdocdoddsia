using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;

namespace SiGlaz.UI.CustomControls.DockBar
{
    public partial class ucPanel : Panel
    {
        #region Members
        private Pen _pen = new Pen(Color.Gray);
        #endregion

        #region Constructor
        public ucPanel()
        {
            InitializeComponent();
        }
        #endregion

        #region Override Methods
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.DrawRectangle(_pen, 0, 0, this.Width - 1, this.Height - 1);
        }
        #endregion
    }
}
