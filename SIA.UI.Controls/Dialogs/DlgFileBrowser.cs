using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SIA.UI.Controls.Dialogs
{
    public partial class DlgFileBrowser : Form
    {
        public DlgFileBrowser()
        {
            InitializeComponent();

            ucFileBrowser1.Caption = "Specify golden image file:";
        }

        public string Caption
        {
            get { return ucFileBrowser1.Caption; }
            set { ucFileBrowser1.Caption = value; }
        }

        protected virtual bool IsValid()
        {
            if (ucFileBrowser1.FilePath == "")
            {
                MessageBox.Show(this,
                    "File path is empty!",
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

        public string FilePath
        {
            get { return ucFileBrowser1.FilePath; }
            set { ucFileBrowser1.FilePath = value; }
        }
    }
}
