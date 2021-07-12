using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SIA.UI.Controls.Automation.Commands;

namespace SIA.UI.Controls.ObjectAnalysis
{
    public partial class ucABSPrimitiveObjectDetection : UserControl
    {
        private ObjectClassificationSettings _settings = null;
        
        [Browsable(false)]
        public ObjectClassificationSettings Settings
        {
            get { return _settings; }
            set
            {
                _settings = value;
                UpdateSettings(true);
            }
        }

        public ucABSPrimitiveObjectDetection()
        {
            InitializeComponent();
        }

        private bool IsValid()
        {
            UpdateSettings(false);

            if (!checkBox1.Checked &&
                !checkBox2.Checked &&
                !checkBox3.Checked &&
                !checkBox4.Checked)
            {
                MessageBox.Show(this,
                        "Please specify at least one object type to process!",
                        "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            return true;
        }

        private void UpdateSettings(bool toControl)
        {
            if (_settings == null)
                _settings = new ObjectClassificationSettings();

            if (toControl)
            {
                checkBox1.Checked = _settings.ClassifyDarkObject;
                checkBox2.Checked = _settings.ClassifyBrightObject;
                checkBox3.Checked = _settings.ClassifyDarkObjectAcrossBoundary;
                checkBox4.Checked = _settings.ClassifyBrightObjectAcrossBoundary;
            }
            else
            {
                _settings.ClassifyDarkObject = checkBox1.Checked;
                _settings.ClassifyBrightObject = checkBox2.Checked;
                _settings.ClassifyDarkObjectAcrossBoundary = checkBox3.Checked;
                _settings.ClassifyBrightObjectAcrossBoundary = checkBox4.Checked;
            }
        }
    }
}
