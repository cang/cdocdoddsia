using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIA.Common.KlarfExport;
using SIA.UI.Controls.Automation.Commands;

namespace SIA.UI.Controls.Dialogs
{
    public partial class DlgObjectFilterEx : Form
    {
        private ObjectFilterSettings _settings = null;
        public ObjectFilterSettings Settings
        {
            get { return _settings; }
            set
            {
                _settings = value;
                UpdateSettings(true);
            }
        }

        public DlgObjectFilterEx(ObjectFilterSettings settings)
        {
            InitializeComponent();

            this.Settings = settings;
        }

        private void optFilter_CheckedChanged(object sender, EventArgs e)
        {
            btnFilter1.Enabled = chkFilter1.Checked;
            btnFilter2.Enabled = chkFilter2.Checked;
            btnFilter3.Enabled = chkFilter3.Checked;
            btnFilter4.Enabled = chkFilter4.Checked;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void btnFilter1_Click(object sender, EventArgs e)
        {
            ConfigureFilter(ref _settings.DarkObjectFilterSettings);
        }

        private void btnFilter2_Click(object sender, EventArgs e)
        {
            ConfigureFilter(ref _settings.BrightObjectFilterSettings);
        }

        private void btnFilter3_Click(object sender, EventArgs e)
        {
            ConfigureFilter(ref _settings.DarkObjectAcrossBoundaryFilterSettings);
        }

        private void btnFilter4_Click(object sender, EventArgs e)
        {
            ConfigureFilter(ref _settings.BrightObjectAcrossBoundaryFilterSettings);
        }

        private void ConfigureFilter(ref ObjectFilterArguments filter)
        {
            ObjectFilterArguments objFilter = (ObjectFilterArguments)filter.Clone();

            using (DlgObjectFilter dlg = new DlgObjectFilter(objFilter))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    filter = objFilter;
                }
            }
        }

        private bool IsValid()
        {
            this.UpdateSettings(false);

            if (!chkFilter1.Checked &&
                !chkFilter2.Checked &&
                !chkFilter4.Checked &&
                !chkFilter3.Checked)
            {
                MessageBox.Show(this,
                        "Please configure at least one filter to process!",
                        "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            return true;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                if (!IsValid())
                {
                    e.Cancel = true;
                    return;
                }
            }

            base.OnClosing(e);
        }

        private void UpdateSettings(bool toControl)
        {
            if (_settings == null)
                _settings = new ObjectFilterSettings();

            if (toControl)
            {
                chkFilter1.Checked = _settings.ApplyFilterForDarkObject;
                chkFilter2.Checked = _settings.ApplyFilterForBrightObject;
                chkFilter3.Checked = _settings.ApplyFilterForDarkObjectAcrossBoundary;
                chkFilter4.Checked = _settings.ApplyFilterForBrightObjectAcrossBoundary;
            }
            else
            {
                _settings.ApplyFilterForDarkObject = chkFilter1.Checked;
                _settings.ApplyFilterForBrightObject = chkFilter2.Checked;
                _settings.ApplyFilterForDarkObjectAcrossBoundary = chkFilter3.Checked;
                _settings.ApplyFilterForBrightObjectAcrossBoundary = chkFilter4.Checked;
            }
        }
    }
}
