using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SIA.UI.Controls.Dialogs
{
    public partial class DlgGoldenImageCreation : Form
    {
        public DlgGoldenImageCreation()
        {
            InitializeComponent();
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Title = "Specify output file name";
                dlg.RestoreDirectory = true;
                dlg.Filter = "Bitmap (*.bmp)|*.bmp";

                if (dlg.ShowDialog(this.FindForm()) == DialogResult.OK)
                {
                    txtOutputPath.Text = dlg.FileName;
                }
            }
        }

        public string OutputFilePath
        {
            get { return txtOutputPath.Text; }
            set { txtOutputPath.Text = value; }
        }

        public string[] SelectedFiles
        {
            get { return ucFileList1.SelectedFiles; }

            set
            {
                ucFileList1.FileNames = value;
            }
        }

        private bool IsValid()
        {
            if (this.OutputFilePath == "")
            {
                MessageBox.Show(this, 
                    "Output file path is empty!", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string[] selectedFiles = this.SelectedFiles;
            if (selectedFiles == null ||
                selectedFiles.Length < 2)
            {
                MessageBox.Show(this,
                    "Please select at least two file(s) to process!",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
    }
}
