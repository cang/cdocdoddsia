using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SIA.UI.Controls.UserControls
{
    public partial class ucObjectFilterItem : UserControl
    {
        #region Properties
        public CheckBox ChkState
        {
            get { return chkState; }
            set { chkState = value; }
        }

        public CheckBox ChkMin
        {
            get { return chkMin; }
            set { chkMin = value; }
        }

        public CheckBox ChkMax
        {
            get { return chkMax; }
            set { chkMax = value; }
        }

        public NumericUpDown NudMin
        {
            get { return nudMin; }
            set { nudMin = value; }
        }

        public NumericUpDown NudMax
        {
            get { return nudMax; }
            set { nudMax = value; }
        }

        public decimal MinThresholdValue
        {
            get { return nudMin.Value; }
            set
            {
                decimal val = (decimal)Math.Min(Math.Max(value, nudMin.Minimum), nudMin.Maximum);
                nudMin.Value = val;
            }
        }

        public decimal MaxThresholdValue
        {
            get { return nudMax.Value; }
            set
            {
                decimal val = (decimal)Math.Min(Math.Max(value, nudMax.Minimum), nudMax.Maximum);
                nudMax.Value = val;
            }
        }
        #endregion Properties

        #region Constructors and destructors
        public ucObjectFilterItem()
        {
            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            chkState.CheckedChanged += new EventHandler(chkState_CheckedChanged);
            chkMin.CheckedChanged += new EventHandler(chkMin_CheckedChanged);
            chkMax.CheckedChanged += new EventHandler(chkMax_CheckedChanged);

            nudMin.ValueChanged += new EventHandler(nudMin_ValueChanged);
            nudMax.ValueChanged += new EventHandler(nudMax_ValueChanged);

            // default
            SetDefaults();
        }

        private void SetDefaults()
        {
            chkState.Checked = false;

            chkMin.ForeColor = SystemColors.ControlText;
            chkMin.Checked = false;
            nudMin.Enabled = chkMin.Checked;

            chkMax.ForeColor = SystemColors.ControlText;
            chkMax.Checked = false;
            nudMax.Enabled = chkMax.Checked;

            pnFilterContents.Visible = chkState.Checked;
            this.Height = (pnFilterContents.Visible ? 48 : 22);

            nudMin.Value = 0;
            nudMin.Minimum = 0;
            nudMin.Maximum = int.MaxValue;

            nudMax.Value = 0;
            nudMax.Minimum = 0;
            nudMax.Maximum = int.MaxValue;            
        }

        public void SetDefaults(
            string text,
            decimal nudMinValue, decimal nudMinMinimum, decimal nudMinMaximum, 
            decimal nudMinStep, int nudMinDecimalPlaces,
            decimal nudMaxValue, decimal nudMaxMinimum, decimal nudMaxMaximum, 
            decimal nudMaxStep, int nudMaxDecimalPlaces)
        {
            chkState.Text = text;

            chkState.Checked = false;

            chkMin.ForeColor = SystemColors.ControlText;
            chkMin.Checked = false;
            nudMin.Enabled = chkMin.Checked;

            chkMax.ForeColor = SystemColors.ControlText;
            chkMax.Checked = false;
            nudMax.Enabled = chkMax.Checked;

            pnFilterContents.Visible = chkState.Checked;
            this.Height = (pnFilterContents.Visible ? 48 : 22);

            nudMin.Value = nudMinValue;
            nudMin.Minimum = nudMinMinimum;
            nudMin.Maximum = nudMinMaximum;
            nudMin.DecimalPlaces = nudMinDecimalPlaces;
            nudMin.Increment = nudMinStep;            

            nudMax.Value = nudMaxValue;
            nudMax.Minimum = nudMaxMinimum;
            nudMax.Maximum = nudMaxMaximum;
            nudMax.DecimalPlaces = nudMaxDecimalPlaces;
            nudMax.Increment = nudMaxStep;
        }
        #endregion Constructors and destructors

        #region Events
        void nudMax_ValueChanged(object sender, EventArgs e)
        {
            
        }

        void nudMin_ValueChanged(object sender, EventArgs e)
        {
            
        }

        void chkMax_CheckedChanged(object sender, EventArgs e)
        {
            nudMax.Enabled = chkMax.Checked;
        }

        void chkMin_CheckedChanged(object sender, EventArgs e)
        {
            nudMin.Enabled = chkMin.Checked;
        }

        void chkState_CheckedChanged(object sender, EventArgs e)
        {
            pnFilterContents.Visible = chkState.Checked;

            this.Height = (pnFilterContents.Visible ? 48 : 22);
        }
        #endregion Events        

        #region Methods
        public bool IsValid()
        {
            bool valid = true;

            try
            {
                if (chkState.Checked && chkMin.Checked && chkMax.Checked)
                {
                    decimal min = MinThresholdValue;
                    decimal max = MaxThresholdValue;

                    if (min > max)
                    {
                        MessageBox.Show(
                            this, 
                            "Maximum value should be greater than or equal minimum value.", 
                            string.Format("Error: Filter by '{0}' parameters", chkState.Text), 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                        valid = false;
                    }
                }
            }
            catch
            {
                valid = false;
            }

            return valid;
        }
        #endregion Methods
    }
}
