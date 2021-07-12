using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SIA.UI.Controls.Dialogs
{
    public partial class DlgCoordinate : Form
    {
        public DlgCoordinate()
        {
            InitializeComponent();
        }

        public float XCoordinate
        {
            get
            {
                return (float)nudX.Value;
            }

            set
            {
                float v = (float)Math.Min(value, (float)nudX.Maximum);
                v = (float)Math.Max(v, (float)nudX.Minimum);

                nudX.Value = (decimal)v;
            }
        }

        public float YCoordinate
        {
            get
            {
                return (float)nudY.Value;
            }

            set
            {
                float v = (float)Math.Min(value, (float)nudY.Maximum);
                v = (float)Math.Max(v, (float)nudY.Minimum);

                nudY.Value = (decimal)v;
            }
        }
    }
}
