using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SIA.UI.Controls.Dialogs
{
    public partial class DlgDetectContaminationsSettings : Form
    {
        public DlgDetectContaminationsSettings()
        {
            InitializeComponent();
        }

        public string RegionFile
        {
            get { return txtRegionFile.Text; }
        }

        public string NNModelFile
        {
            get { return txtNNModelFile.Text; }
        }

        private void btnRegionFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Specify Region file";
            dlg.RestoreDirectory = true;
            dlg.Filter = "Region file(*.rgn)|*.rgn";
            if (dlg.ShowDialog(this) == DialogResult.OK)
                txtRegionFile.Text = dlg.FileName;
        }

        private void btnNNModelFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Specify Neural Network model file";
            dlg.RestoreDirectory = true;
            dlg.Filter = "NN Model file(*.nns)|*.nns";
            if (dlg.ShowDialog(this) == DialogResult.OK)
                txtNNModelFile.Text = dlg.FileName;
        }
    }
}
