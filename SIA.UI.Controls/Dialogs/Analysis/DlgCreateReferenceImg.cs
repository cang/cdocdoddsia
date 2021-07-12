using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace SIA.UI.Controls.Dialogs
{
    public partial class DlgCreateReferenceImg : Form
    {
        public DlgCreateReferenceImg()
        {
            InitializeComponent();
        }

        #region Commands
        private void ExportImage2File(Image img, string filename)
        {
            if (img == null) return;
            string efn = System.IO.Path.GetExtension(filename);
            if (efn == null) return;

            switch (efn.ToUpper())
            {
                case ".BMP":
                    img.Save(filename, System.Drawing.Imaging.ImageFormat.Bmp);
                    break;
                case ".JPG":
                    img.Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;
                case ".GIF":
                    img.Save(filename, System.Drawing.Imaging.ImageFormat.Gif);
                    break;
                case ".PNG":
                    img.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
                    break;
                case ".TIF":
                    img.Save(filename, System.Drawing.Imaging.ImageFormat.Tiff);
                    break;
                default:
                    img.Save(filename, System.Drawing.Imaging.ImageFormat.Gif);//default is gif
                    break;
            }
        }

        private bool Validate(ref string error)
        {
            bool result = true;

          if (dataGridViewData.Rows.Count <5)
            {
                result = false;
                error = "Please input at least 5 Metrology System Reference File.";
            }
            else if (txtOutputPath.Text.Trim() == string.Empty)
            {
                result = false;
                error = "Please input output file path.";
            }

            return result;
        }


        private bool LoadMetrologySystemReferenceFile(string fileName)
        {
            try
            {
                SiGlaz.Common.ImageAlignment.MetrologySystemReference.Deserialize(fileName);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region UI Commands

        private void btnAddFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = true;
            dlg.Filter = "Reference File (*.msr)|*.msr";
           

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ArrayList invalidFiles = new ArrayList();
                foreach (string fileName in dlg.FileNames)
                {
                    if (!LoadMetrologySystemReferenceFile(fileName))
                    {
                        invalidFiles.Add(fileName);
                        continue;
                    }

                    DataGridViewRow row = new DataGridViewRow();
                    int index = dataGridViewData.Rows.Add(row);
                    dataGridViewData.Rows[index].Cells[0].Value = fileName;
                }

                if (invalidFiles.Count > 0)
                {
                    string text = string.Empty;
                    foreach (string fileName in invalidFiles)
                        text += (fileName + ", ");

                    text = text.TrimEnd(", ".ToCharArray());

                    text = string.Format("File '{0}' format is invalid.", text);

                    MessageBox.Show(text, this.Text);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewData.SelectedRows == null)
            {
                MessageBox.Show("Please select at least 1 file to delete", this.Text);
                return;
            }

            foreach (DataGridViewRow row in dataGridViewData.SelectedRows)
                dataGridViewData.Rows.Remove(row);
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            dataGridViewData.Rows.Clear();
        }

        private void btnOutputPath_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "bitmaps (*.bmp)|*.bmp";
           
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtOutputPath.Text = dlg.FileName;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            if (!Validate(ref error))
            {
                MessageBox.Show(error, this.Text);
                DialogResult = DialogResult.None;
            }
        }

        #endregion

        #region Properties
        public string[] FileList
        {
            get
            {
                string[] fileList = new string[dataGridViewData.RowCount];
                int index = 0;
                foreach (DataGridViewRow row in dataGridViewData.Rows)
                {
                    fileList[index++] = (string)row.Cells[0].Value;
                }
                return fileList;
            }
        }

        public string OutputFilePath
        {
            get
            {
                return txtOutputPath.Text;
            }
        }
        #endregion

    }
}
