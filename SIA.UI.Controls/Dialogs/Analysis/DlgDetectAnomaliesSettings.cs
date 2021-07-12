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
    public partial class DlgDetectAnomaliesSettings : Form
    {
        private DetectAnomaliesSettings _settings = new DetectAnomaliesSettings();
        public DetectAnomaliesSettings Settings
        {
            get { return _settings; }
        }

        public DlgDetectAnomaliesSettings(DetectAnomaliesSettings settings)
        {
            InitializeComponent();

            _settings = settings;
            this.Update(true);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.Update(false);

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
            return true;
        }

        private void Update(bool toControl)
        {
            if (toControl)
            {
                rdAllRegions.Checked = _settings.ApplyRegions;
                rdImage.Checked = !_settings.ApplyRegions;
            }
            else
            {
                _settings.ApplyRegions = rdAllRegions.Checked;
            }
        }
    }
}
