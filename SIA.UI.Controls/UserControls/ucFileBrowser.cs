using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SIA.UI.Controls.UserControls
{
    public partial class ucFileBrowser : UserControl
    {
        public ucFileBrowser()
        {
            InitializeComponent();
        }

        public string BrowserTitle = "Specify file name";
        public string BrowserFilter = "Common image file (*.bmp, *.jpg, *.jpeg, *.png)|*.bmp;*.jpg;*.jpeg;*.png";
        public bool IsModeOpen = true;

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            if (IsModeOpen)
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.Title = BrowserTitle;
                    dlg.RestoreDirectory = true;
                    dlg.Filter = BrowserFilter;
                    if (dlg.ShowDialog(this.FindForm()) == DialogResult.OK)
                    {
                        txtFilePath.Text = dlg.FileName;
                    }
                }
            }
            else
            {
                using (SaveFileDialog dlg = new SaveFileDialog())
                {
                    dlg.Title = BrowserTitle;
                    dlg.RestoreDirectory = true;
                    dlg.Filter = BrowserFilter;
                    if (dlg.ShowDialog(this.FindForm()) == DialogResult.OK)
                    {
                        txtFilePath.Text = dlg.FileName;
                    }
                }
            }
        }

        public string FilePath
        {
            get { return txtFilePath.Text; }
            set { txtFilePath.Text = value; }
        }

        public string Caption
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }
    }
}
