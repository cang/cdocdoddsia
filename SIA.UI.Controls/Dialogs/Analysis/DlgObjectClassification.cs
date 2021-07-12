using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIA.UI.Controls.Automation.Commands;

namespace SIA.UI.Controls.Dialogs
{
    public partial class DlgObjectClassification : Form
    {
        private ObjectClassificationSettings _settings = null;
        public ObjectClassificationSettings Settings
        {
            get { return _settings; }
            set
            {
                _settings = value;
                UpdateSettings(true);
            }
        }

        public DlgObjectClassification(
            ObjectClassificationSettings settings)
        {
            InitializeComponent();

            this.Settings = settings;
        }

        private void chkApplyFilter_CheckedChanged(object sender, EventArgs e)
        {
            txtFilePath.Enabled = chkApplyFilter.Checked;
            btnBrowse.Enabled = chkApplyFilter.Checked;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {

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

        private bool IsValid()
        {
            UpdateSettings(false);

            if (!checkBox1.Checked &&
                !checkBox2.Checked &&
                !checkBox3.Checked &&
                !checkBox4.Checked)
            {
                MessageBox.Show(this,
                        "Please specify at least one object type to process!",
                        "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            if (chkApplyFilter.Checked)
            {
                if (txtFilePath.Text == "")
                {
                    MessageBox.Show(this, 
                        "Please specify filter setting file!", 
                        "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return false;
                }
            }

            return true;
        }

        private void UpdateSettings(bool toControl)
        {
            if (_settings == null)
                _settings = new ObjectClassificationSettings();

            if (toControl)
            {
                checkBox1.Checked = _settings.ClassifyDarkObject;
                checkBox2.Checked = _settings.ClassifyBrightObject;
                checkBox3.Checked = _settings.ClassifyDarkObjectAcrossBoundary;
                checkBox4.Checked = _settings.ClassifyBrightObjectAcrossBoundary;
            }
            else
            {
                _settings.ClassifyDarkObject = checkBox1.Checked;
                _settings.ClassifyBrightObject = checkBox2.Checked;
                _settings.ClassifyDarkObjectAcrossBoundary = checkBox3.Checked;
                _settings.ClassifyBrightObjectAcrossBoundary = checkBox4.Checked;
            }
        }
    }
}
