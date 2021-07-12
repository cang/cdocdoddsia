using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SiGlaz.ObjectAnalysis.UI
{
    public partial class ucObjectAnalyzerStep : UserControl
    {
        private int _stepId = 0;
        public int StepId
        {
            get { return _stepId; }
        }

        public ucObjectAnalyzerStep()
        {
            InitializeComponent();

            pnFooter.Visible = false;
        }

        public ucObjectAnalyzerStep(int stepId) : this()
        {
            _stepId = stepId;
        }

        [Browsable(false)]
        public Panel Header
        {
            get { return pnHeader; }
        }

        [Browsable(false)]
        public Panel Body
        {
            get { return pnBody; }
        }

        [Browsable(false)]
        public Panel Footer
        {
            get { return pnFooter; }
        }

        [Browsable(false)]
        public CheckBox CheckBoxStatus
        {
            get { return chkStatus; }
        }

        [Browsable(false)]
        public virtual bool SupportSearialization
        {
            get
            {
                return btnLoadStepSettings.Visible || btnSaveStepSettings.Visible;
            }

            set
            {
                btnLoadStepSettings.Visible = value;
                btnSaveStepSettings.Visible = value;
            }
        }

        protected virtual void chkStatus_CheckedChanged(object sender, EventArgs e)
        {
            pnBody.Enabled = chkStatus.Checked;

            btnLoadStepSettings.Enabled = chkStatus.Checked;

            btnSaveStepSettings.Enabled = chkStatus.Checked;
        }

        protected virtual void btnLoad_Click(object sender, EventArgs e)
        {
            string fileName = OpenSettingFromFile("", "");
            if (fileName == "")
                return;

            // deserialize here
        }

        protected virtual void btnSave_Click(object sender, EventArgs e)
        {
            string fileName = SaveSettingToFile("", "");
            if (fileName == "")
                return;

            // serialize here
        }

        protected virtual string OpenSettingFromFile(string title, string filter)
        {
            string fileName = "";
            try
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.Title = title;
                    dlg.Filter = filter;
                    dlg.RestoreDirectory = true;
                    if (dlg.ShowDialog(this.FindForm()) == DialogResult.OK )
                    {
                        fileName = dlg.FileName;
                    }
                }
            }
            catch
            {
                fileName = "";
            }

            return fileName;
        }

        protected virtual string SaveSettingToFile(string title, string filter)
        {
            string fileName = "";
            try
            {
                using (SaveFileDialog dlg = new SaveFileDialog())
                {
                    dlg.Title = title;
                    dlg.Filter = filter;
                    dlg.RestoreDirectory = true;
                    if (dlg.ShowDialog(this.FindForm()) == DialogResult.OK)
                    {
                        fileName = dlg.FileName;
                    }
                }
            }
            catch
            {
                fileName = "";
            }

            return fileName;
        }
    }
}
