using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SIA.UI.Controls.Dialogs
{
    public partial class DlgCalibrationValueInput : Form
    {
       
        public DlgCalibrationValueInput()
        {
            InitializeComponent();
        }

        public DlgCalibrationValueInput(string unit, float pixelLength)
        {
            InitializeComponent();

            lbUnit.Text = string.Format("({0})", unit);
            nudLogicalLength.Value = (decimal)pixelLength;
        }

        public DlgCalibrationValueInput(string unit, double pixelLength, float unitLength)
        {
            InitializeComponent();

            lbUnit.Text = string.Format("({0})", unit);
            nudLogicalLength.Value = (decimal)pixelLength;
            nudRealLength.Value = (decimal)unitLength;
        }


        public float Value
        {
            get { return (float)nudRealLength.Value; }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }
    }
}
