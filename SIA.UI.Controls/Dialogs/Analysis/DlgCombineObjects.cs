using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIA.UI.Controls.Automation.CommandSettings;
using SiGlaz.ObjectAnalysis.Common;

namespace SIA.UI.Controls.Dialogs.Analysis
{
    public partial class DlgCombineObjects : Form
    {
        private CombineObjectsCommandSettings _settings = new CombineObjectsCommandSettings();
        public CombineObjectsCommandSettings Settings
        {
            get { return _settings; }
        }

        public DlgCombineObjects(CombineObjectsCommandSettings settings)
        {
            InitializeComponent();

            _settings = settings;
            if (_settings == null)
                _settings = new CombineObjectsCommandSettings();

            this.Update(true);
        }

        private void btnLoadLib_Click(object sender, EventArgs e)
        {
            string fileName = "";
            MDCCParamLibrary lib = LoadLibrary(ref fileName);
            if (fileName != "")
            {
                if (lib == null)
                {
                    ShowErr(string.Format("Failed to load from file: {0}", fileName));
                }
                else
                {
                    _settings.Library = lib;
                    _settings.FilePath = fileName;
                    _settings.SelectedConditionMap = new bool[_settings.Library.Items.Count];

                    this.Update(true);
                }
            }
        }

        private MDCCParamLibrary LoadLibrary(ref string fileName)
        {
            MDCCParamLibrary lib = null;
            try
            {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.Title = "Choose file";
                    dlg.RestoreDirectory = true;
                    dlg.Filter = "SIA Combine Condition Library (*.sscl)|*.sscl";
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        fileName = dlg.FileName;
                        lib = MDCCParamLibrary.Deserialize(dlg.FileName);
                    }
                }
            }
            catch
            {
                lib = null;
            }

            return lib;
        }

        private void Update(bool toControl)
        {
            if (_settings == null)
                return;

            if (toControl)
            {
                UpdateLibrary(_settings.Library);
                if (_settings.Library != null)
                {
                    for (int i = 0; i < _settings.Library.Items.Count; i++)
                    {
                        chkList.SetItemCheckState(i, (_settings.SelectedConditionMap[i] ? CheckState.Checked : CheckState.Unchecked));
                    }
                }
            }
            else
            {
                if (_settings.Library != null)
                {
                    _settings.SelectedConditionMap = new bool[_settings.Library.Items.Count];
                    for (int i = 0; i < _settings.Library.Items.Count; i++)
                    {
                        CheckState state = chkList.GetItemCheckState(i);
                        if (state == CheckState.Checked)
                        {
                            _settings.SelectedConditionMap[i] = true;
                        }
                    }
                }
            }
        }

        private void UpdateLibrary(MDCCParamLibrary lib)
        {
            chkList.Items.Clear();

            if (lib != null)
            {
                foreach (MDCCParamItem item in lib.Items)
                {
                    AddConditionToRuleList(item);
                }
            }
        }

        private void AddConditionToRuleList(MDCCParamItem item)
        {
            int index = chkList.Items.Add(item.RuleName);
            chkList.SetItemChecked(index, item.ApplyCombination);
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
            if (_settings == null || _settings.Library == null)
            {
                ShowErr("Please specify at least 1 condition.");
                return false;
            }

            if (_settings.SelectedConditions.Count <= 0)
            {
                ShowErr("Please specify at least 1 condition.");
                return false;
            }

            return true;
        }

        private void ShowErr(string msg)
        {
            MessageBox.Show(this, msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
