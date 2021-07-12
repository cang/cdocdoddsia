using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIA.UI.Controls.Automation.Commands;

namespace SIA.UI.Controls.Automation.Dialogs
{
    public partial class DlgExportDefectListSettings : Form
    {
        private ExportDefectListSettings _settings = null;
        public ExportDefectListSettings Settings
        {
            get { return _settings; }
        }

        public DlgExportDefectListSettings(ExportDefectListSettings settings)
        {
            InitializeComponent();

            _settings = settings;
            if (_settings == null)
                _settings = new ExportDefectListSettings();

            Initialize();

            this.Update(true);
        }

        private void Initialize()
        {
            // initialize mask string editor
            //this.ucFileNameFormat.FileTypes = FileTypes.TextFileTypes;
            //this.ucFileNameFormat.SupportOnlyTextFile = true;
            //this.ucFileNameFormat.FileTypes = FileTypes.ReportTextCsvFileTypes;

            this.ucFileNameFormat.FileTypes = FileTypes.CsvFileTypes;
            this.ucFileNameFormat.SupportOnlyTextFile = false;
            //this.ucFileNameFormat.SupportTextCsvFile = true;
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

        private bool ValidateData()
        {
            if (ucFileNameFormat.FileNameFormat.Trim() == "")
            {
                MessageBoxEx.Error("Format of output file name was not specified. Please try again.");
                return false;
            }

            return true;
        }

        private void Update(bool toControl)
        {
            if (_settings == null)
                _settings = new ExportDefectListSettings();

            if (toControl)
            {
                ucFileNameFormat.FileNameFormat = _settings.FileNameFormat;
                rdKeepName.Checked = _settings.KeepName;
                rdCustomizeName.Checked = !_settings.KeepName;

                txtCustomizeName.Text = _settings.CustomizedName;
                txtCustomizeName.Enabled = !_settings.KeepName;
            }
            else
            {
                _settings.FileNameFormat = ucFileNameFormat.FileNameFormat;
                _settings.KeepName = rdKeepName.Checked;
                _settings.CustomizedName = txtCustomizeName.Text.Trim();
            }
        }

        private void classifyAnomalyNameChanged(object sender, EventArgs e)
        {
            txtCustomizeName.Enabled = rdCustomizeName.Checked;
        }

        private bool IsValid()
        {
            this.Update(false);

            if (_settings.FileNameFormat.Trim() == "")
            {
                ShowErr("Please specify output file path.");

                return false;
            }

            if (!_settings.KeepName && _settings.CustomizedName == "")
            {
                ShowErr("Please specify customized anomaly name.");

                return false;
            }

            return true;
        }

        private void ShowErr(string msg)
        {
            MessageBox.Show(this, msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}