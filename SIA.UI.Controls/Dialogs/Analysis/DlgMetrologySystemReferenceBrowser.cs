using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIA.UI.Controls.Automation.Commands;
using System.IO;
using SiGlaz.Common.ImageAlignment;

namespace SIA.UI.Controls.Dialogs
{
    public partial class DlgMetrologySystemReferenceBrowser : Form
    {
        private MetrologySystemReferenceSettings _settings = new MetrologySystemReferenceSettings();
        public MetrologySystemReferenceSettings Settings
        {
            get
            {
                Update(false);
                return _settings;
            }
        }


        public DlgMetrologySystemReferenceBrowser(MetrologySystemReferenceSettings settings)
        {
            InitializeComponent();

            _settings = settings;
            Update(true);
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Specify file name";
                dlg.Filter = MetrologySystemReference.FileFilter;
                dlg.RestoreDirectory = true;
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        MetrologySystemReference settings = 
                            MetrologySystemReference.Deserialize(dlg.FileName);
                        settings = null;
                    }
                    catch
                    {
                        MessageBox.Show(this,
                            "Specified file is invalid!",
                            "Settings validation",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }

                    txtFilePath.Text = dlg.FileName;
                }
            }
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
            if (txtFilePath.Text == "" || !File.Exists(txtFilePath.Text))
            {
                MessageBox.Show(
                    this, "File path is invalid!",
                    "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void Update(bool toControl)
        {
            if (toControl)
            {
                txtFilePath.Text = _settings.FilePath;
            }
            else
            {
                _settings.FilePath = txtFilePath.Text;
            }
        }
    }
}
