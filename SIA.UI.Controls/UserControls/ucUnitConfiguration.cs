using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SiGlaz.Common.ImageAlignment;

namespace SIA.UI.Controls.UserControls
{
    public partial class ucUnitConfiguration : UserControl
    {
        #region Member fields
        private MetrologyUnitBase _unit = null;

        public event EventHandler CalibrationClicked;
        #endregion Member fields

        #region Constructors and destructors
        public ucUnitConfiguration()
        {
            InitializeComponent();

            _unit = MicronUnit.CreateInstance(1, 1);

            this.Update(true);
        }
        #endregion Constructors and destructors

        #region Properties
        public MetrologyUnitBase CurrentUnit
        {
            get { return _unit; }
            set
            {
                _unit = value;
                this.Update(true);
            }
        }

        public float PixelVal
        {
            get { return (float)nudPixelVal.Value; }
            set
            {
                if ((float)nudPixelVal.Value != value)
                {
                    nudPixelVal.Value = (decimal)value;
                    OnPixelValChanged();
                }
            }
        }

        private void OnPixelValChanged()
        {
            
        }

        public float UnitVal
        {
            get { return (float)nudUnitVal.Value; }
            set
            {
                if ((float)nudUnitVal.Value != value)
                {
                    nudUnitVal.Value = (decimal)value;
                    OnUniValChanged();
                }
            }
        }

        private void OnUniValChanged()
        {
            
        }

        protected void UpdateScale(float pixelVal, float unitVal)
        {
            nudPixelVal.Value = (decimal)pixelVal;
            nudUnitVal.Value = (decimal)unitVal;
        }
        #endregion Properties

        #region Helpers
        
        public void Update(bool toControl)
        {
            if (toControl)
            {
                this.cmbUnit.Text = _unit.FullName;
                this.lbUnit.Text = String.Format("({0})", _unit.ShortName);
                this.UpdateScale(_unit.PixelVal, _unit.UnitVal);
            }
            else
            {
                _unit.UpdateScale(this.PixelVal, this.UnitVal);
            }
        }
        #endregion Helpers

        private void cmbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnCalibrate_Click(object sender, EventArgs e)
        {
            if (CalibrationClicked != null)
                this.CalibrationClicked(this, EventArgs.Empty);
        }
    }
}
