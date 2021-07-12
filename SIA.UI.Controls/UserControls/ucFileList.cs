using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SIA.UI.Controls.UserControls
{
    public partial class ucFileList : UserControl
    {
        public string[] FileNames
        {
            get
            {
                if (checkedListBox.Items.Count == 0)
                    return null;

                int n = checkedListBox.Items.Count;
                string[] files = new string[n];
                for (int i = 0; i < n; i++)
                {
                    files[i] = (string)checkedListBox.Items[i];
                }
                return files;
            }
            set
            {
                checkedListBox.Items.Clear();
                if (value == null)
                    return;
                foreach (string file in value)
                {
                    this.AddItem(file);
                }
            }
        }

        public string[] SelectedFiles
        {
            get
            {
                if (checkedListBox.CheckedItems == null || 
                    checkedListBox.CheckedItems.Count == 0)
                    return null;

                int n = checkedListBox.CheckedItems.Count;
                List<string> selectedFiles = new List<string>(n);
                foreach (string selectedFile in checkedListBox.CheckedItems)
                {
                    selectedFiles.Add(selectedFile);
                }

                return selectedFiles.ToArray();
            }
        }

        public ucFileList()
        {
            InitializeComponent();
        }

        private void checkedListBox_SelectedIndexChanged(
            object sender, EventArgs e)
        {
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.Title = "Specify file name";
                    dlg.RestoreDirectory = true;
                    dlg.Filter = "Text file (*.txt)|*.txt";

                    if (dlg.ShowDialog(this.FindForm()) == DialogResult.OK)
                    {
                        checkedListBox.Items.Clear();

                        using (StreamReader reader = new StreamReader(dlg.FileName))
                        {
                            while (!reader.EndOfStream)
                            {
                                string fileName = reader.ReadLine();
                                this.AddItem(fileName);
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (checkedListBox.Items.Count == 0)
            {
                return;
            }

            try
            {
                using (SaveFileDialog dlg = new SaveFileDialog())
                {
                    dlg.Title = "Specify file name";
                    dlg.RestoreDirectory = true;
                    dlg.Filter = "Text file (*.txt)|*.txt";

                    if (dlg.ShowDialog(this.FindForm()) == DialogResult.OK)
                    {
                        using (StreamWriter writer = new StreamWriter(dlg.FileName))
                        {
                            foreach (string item in checkedListBox.Items)
                            {
                                writer.WriteLine(item);
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Select file(s)";
                dlg.RestoreDirectory = true;
                dlg.Filter = "Common image file (*.bmp, *.jpg, *.jpeg, *.png)|*.bmp;*.jpg;*.jpeg;*.png";
                dlg.Multiselect = true;
                if (dlg.ShowDialog(this.FindForm()) == DialogResult.OK)
                {
                    foreach (string fileName in dlg.FileNames)
                    {
                        AddItem(fileName);
                    }
                }
            }
        }

        private void AddItem(string fileName)
        {            
            foreach (string item in checkedListBox.Items)
            {
                if (String.Compare(fileName, item, true) == 0)
                    return;
            }

            checkedListBox.Items.Add(fileName);
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (checkedListBox.SelectedIndex < 0)
                return;

            checkedListBox.Items.RemoveAt(checkedListBox.SelectedIndex);
        }

        private void btnDelAll_Click(object sender, EventArgs e)
        {
            checkedListBox.Items.Clear();
        }
    }
}
