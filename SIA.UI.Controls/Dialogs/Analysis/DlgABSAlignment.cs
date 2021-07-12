using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIA.UI.Controls.Automation.Commands;
using System.IO;

namespace SIA.UI.Controls.Dialogs
{
    public partial class DlgABSAlignment : Form
    {
        private AlignABSCommandSettings _settings = new AlignABSCommandSettings();
        public AlignABSCommandSettings Settings
        {
            get
            {
                Update(false);
                return _settings;
            }
        }


        public DlgABSAlignment(AlignABSCommandSettings settings)
        {
            InitializeComponent();

            _settings = settings;
            Update(true);
        }

        private void Update(bool toControl)
        {
            if (toControl)
            {
                rdDefault.Checked = _settings.UseDefault;
                rdOther.Checked = !_settings.UseDefault;
                txtFilePath.Text = _settings.FilePath;
            }
            else
            {
                _settings.UseDefault = rdDefault.Checked;
                _settings.FilePath = txtFilePath.Text;
            }
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Specify file name";
                dlg.Filter = "Aligment settings (*.settings)|*.settings";
                dlg.RestoreDirectory = true;
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        SiGlaz.Common.ImageAlignment.Settings settings =
                            SiGlaz.Common.ImageAlignment.Settings.Deserialize(dlg.FileName);
                    }
                    catch
                    {
                        MessageBox.Show(this, 
                            "Specified file is invalid!", 
                            "Aligment settings validation", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }

                    txtFilePath.Text = dlg.FileName;
                }
            }
        }

        private bool IsValid()
        {
            if (!rdDefault.Checked)
            {
                if (txtFilePath.Text == "" ||
                    !File.Exists(txtFilePath.Text))
                {
                    MessageBox.Show(
                        this, "File path is invalid!", 
                        "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
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

        private void ReferenceSettingsChanged(object sender, EventArgs e)
        {
            txtFilePath.Enabled = !rdDefault.Checked;
            btnBrowser.Enabled = !rdDefault.Checked;
        }
    }
}
