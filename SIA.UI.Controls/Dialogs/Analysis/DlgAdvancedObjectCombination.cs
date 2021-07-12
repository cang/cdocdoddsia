using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIA.Common.Analysis;
using System.Collections;
using SiGlaz.ObjectAnalysis.Common;
using SiGlaz.ObjectAnalysis.Engine;

namespace SIA.UI.Controls.Dialogs
{
    public partial class DlgAdvancedObjectCombination : FloatingFormBase
    {
        #region Member fields
        private ImageWorkspace _workspace = null;
        private ArrayList _objects = null;
        private MDCCParamItem 
            _edittingItem = new MDCCParamItem();
        private MDCCParamLibrary _edittingLibrary = new MDCCParamLibrary();
        #endregion Member fields

        #region Constructors and destructors
        public DlgAdvancedObjectCombination(
            ImageWorkspace workSpace, ArrayList objects)
            : base()
        {
            InitializeComponent();

            spRulesWizardCtrl.IsLimitedCombinationCompareOperator = true;

            // initialize top level window
            this.TopLevel = true;
            //this.TopMost = true;

            FadeOutEnabled = true;

            _workspace = workSpace;
            _objects = objects;

            if (_workspace != null)
            {
                this.Owner = _workspace.FindForm();
                _workspace.DataChanged += new EventHandler(_workspace_ImageDataChanged);
            }

#if DEBUG__
            btnOK.Enabled = false;
            btnCancel.Enabled = false;
#endif

            UpdateEdittingWorkspace(false);

            this.ClearStorageWorkingObjects();
        }

        void _workspace_ImageDataChanged(object sender, EventArgs e)
        {
            this.ClearStorageWorkingObjects();
        }
        #endregion Constructors and destructors

        #region Overrides
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            // cancel the closing operation just hide this window out
            e.Cancel = true;
            this.Visible = false;

            _workspace.Focus();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }

            base.OnSizeChanged(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        #endregion Overrides

        #region empty editting workspace
        private void UpdateEdittingWorkspace(bool visibleEditting)
        {
            pnFooter.Visible = visibleEditting;
            pnHeader.Visible = visibleEditting;
            spRulesWizardCtrl.Visible = visibleEditting;

            pnEditting.BackColor = 
                (visibleEditting ? SystemColors.Control : Color.White);
        }
        #endregion empty editting workspace

        #region Current Rule
        private bool _isNewCondition = false;
        private bool _isRuleModified = false;
        private void DoCommandNewCondition()
        {
            // update current editting
            this.Update(false);

            // check rule is modified
            if (IsRuleModified())
            {
                if (!ConfirmToUpdateNewCondition())
                    return;

                if (!ConfirmToUpdateExistedCondition())
                    return;
            }

            if (_edittingIndex >= 0)
            {
                ruleList.Items[_edittingIndex] = _edittingItem.RuleName;
            }

            // hence: everything is ok
            _isNewCondition = true;
            _isRuleModified = false;
            _edittingIndex = -1;
            _edittingItem = new MDCCParamItem();
            this.Update(true);

            UpdateEdittingWorkspace(true);
        }
        
        private void btnLoad_Click(object sender, EventArgs e)
        {
            // update editting item
            //this.Update(false);
            if (IsRuleModified())
            {
              // nothing  
            }

            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Choose file";
                dlg.RestoreDirectory = true;
                dlg.Filter = "SIA Combine Condition (*.sscr)|*.sscr";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    MDCCParamItem item = 
                        MDCCParamItem.Deserialize(dlg.FileName);

                    if (item != null)
                    {                        
                        _edittingItem.CopyFrom(item);

                        // update to control
                        this.Update(true);

                        // set rule is modified
                        _isRuleModified = true;
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Update(false);
            string msg = ValidateRule(_edittingItem);
            if (msg != "")
            {
                ShowMsgError(msg);
                return;
            }

            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Title = "Specify file name";
                dlg.RestoreDirectory = true;
                dlg.Filter = "SIA Combine Condition (*.sscr)|*.sscr";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    _edittingItem.Serialize(dlg.FileName);
                }
            }
        }

        private void btnPreviewRule_Click(object sender, EventArgs e)
        {
            this.Update(false);
            string msg = ValidateRule(_edittingItem);
            if (msg != "")
            {
                ShowMsgError(msg);
                return;
            }

            // preview
            PerformPreview(_edittingItem);
        }

        private void btnSavetoLib_Click(object sender, EventArgs e)
        {
            this.Update(false);
            string msg = ValidateRule(_edittingItem);
            if (msg != "")
            {
                ShowMsgError(msg);
                return;
            }

            if (_isNewCondition && IsExisted(_edittingItem) != null)
            {
                ShowMsgError("Rule name already exists!");
                return;
            }

            if (!_isNewCondition && 
                IsExisted(_edittingItem, _edittingIndex) != null)
            {
                ShowMsgError("Rule name already exists!");
                return;
            }

            if (_isNewCondition)
            {
                AddNewConditionToLibrary();
            }
            else
            {
                UpdateExistedRuleInfo();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // perform delete

        }

        private string ValidateRule(MDCCParamItem item)
        {
            string msg = "";
            if (item.RuleName == "")
            {
                msg = "Rule name is not set!";
                return msg;
            }

            if (item.SignatureName == "")
            {
                msg = "Classifed object name is not set!";
                return msg;
            }

            if (item.Condition.Conditions.Count == 0)
            {
                msg = "Does not define any condition!";
                return msg;
            }

            return msg;
        }

        private void Update(bool toControl)
        {
            if (toControl)
            {
                txtRuleName.Text = _edittingItem.RuleName;
                txtSigName.Text = _edittingItem.SignatureName;
                spRulesWizardCtrl.Condition = _edittingItem.Condition.Conditions;
                spRulesWizardCtrl.DFSLevel = _edittingItem.Condition.DFSLevel;                
            }
            else
            {
                CheckRuleModification();

                _edittingItem.RuleName = txtRuleName.Text;
                _edittingItem.SignatureName = txtSigName.Text;
                _edittingItem.Condition.Conditions = spRulesWizardCtrl.Condition;
                _edittingItem.Condition.DFSLevel = spRulesWizardCtrl.DFSLevel;                
            }
        }

        private bool IsRuleModified()
        {
            return _isRuleModified;
        }

        private void CheckRuleModification()
        {
            if (txtRuleName.Text != _edittingItem.RuleName)
            {
                _isRuleModified = true;
                return;
            }

            if (txtSigName.Text != _edittingItem.SignatureName)
            {
                _isRuleModified = true;
                return;
            }

            if (!_edittingItem.Condition.IsIdentifyWidth(
                spRulesWizardCtrl.Condition, spRulesWizardCtrl.DFSLevel))
            {
                _isRuleModified = true;
                return;
            }

            _isRuleModified = false;
        }

        private void DoCommandDeleteCondition()
        {
            int index = ruleList.SelectedIndex;
            if (index < 0)
                return;
           
            if (index == _edittingIndex)
            {
                _edittingLibrary.Items.RemoveAt(_edittingIndex);
                ruleList.Items.RemoveAt(_edittingIndex);

                _edittingIndex = -1;
                _edittingItem = new MDCCParamItem();
                _isRuleModified = false;

                UpdateEdittingWorkspace(false);
            }
            else
            {
                ruleList.Items.RemoveAt(index);
                _edittingLibrary.Items.RemoveAt(index);

                if (index < _edittingIndex)
                    _edittingIndex -= 1;
            }

            _isRuleModified = true;
        }

        private void DoCommandEditCondition()
        {
            int index = ruleList.SelectedIndex;
            if (index < 0)
                return;

            if (index == _edittingIndex)
                return;

            this.Update(false);
            if (IsRuleModified())
            {
                if (!ConfirmToUpdateNewCondition())
                    return;

                if (!ConfirmToUpdateExistedCondition())
                    return;
            }
           
            UpdateSelectedEdittingItem(index);
        }
        #endregion Current Rule

        #region Lib
        private bool _isNewLibrary = true;
        private int _edittingIndex = -1;
        private bool _isLibModified = false;
        private void toolbarLibItem_Click(
            object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == null)
                return;

            if (e.ClickedItem == toolbarNewLibrary)
            {
                DoCommandNewLibrary();
            }
            else if (e.ClickedItem == toolbarOpen)
            {
                DoCommandOpenLibrary();
            }
            else if (e.ClickedItem == toolbarSave)
            {
                DoCommandSaveLibrary();
            }
            else if (e.ClickedItem == toolbarNewCondition)
            {
                DoCommandNewCondition();
            }
            else if (e.ClickedItem == toolbarEditCondition)
            {
                DoCommandEditCondition();
            }
            else if (e.ClickedItem == toolbarDelCondition)
            {
                DoCommandDeleteCondition();
            }
        }

        private bool DoCommandNewLibrary()
        {
            // update current editting
            this.Update(false);

            // check rule is modified
            if (IsRuleModified())
            {
                if (!ConfirmToUpdateNewCondition())
                    return false;

                if (!ConfirmToUpdateExistedCondition())
                    return false;
            }

            // check library is modified
            if (IsLibModified())
            {
                if (!ConfirmToSaveModifiedLibrary())
                    return false;
            }

            // hence: everything is ok to new library
            _isNewLibrary = true;
            _isLibModified = false;
            _edittingLibrary = new MDCCParamLibrary();

            _isNewCondition = false;
            _isRuleModified = false;
            _edittingIndex = -1;
            _edittingItem = new MDCCParamItem();

            this.UpdateLibrary(_edittingLibrary);

            this.Update(true);

            UpdateEdittingWorkspace(false);

            // roll-back
            this.ClearStorageWorkingObjects();

            return true;
        }

        private void DoCommandOpenLibrary()
        {
            // check editting library here
            if ((!IsRuleModified() && !IsLibModified()) || DoCommandNewLibrary())
            {
                MDCCParamLibrary lib = LoadLibrary();
                if (lib != null)
                {
                    _edittingLibrary = lib;
                    _edittingIndex = -1;
                    _isLibModified = false;
                    _isRuleModified = false;
                    _edittingItem = new MDCCParamItem();
                    UpdateLibrary(_edittingLibrary);
                    this.Update(true);

                    UpdateEdittingWorkspace(false);

                    _isNewLibrary = false;

                    // roll-back
                    this.ClearStorageWorkingObjects();
                }

                return;
            }            
        }

        private void DoCommandSaveLibrary()
        {
            // check editting library here
            
            // update current editting
            this.Update(false);

            // check rule is modified
            if (IsRuleModified())
            {
                if (!ConfirmToUpdateNewCondition())
                    return;

                if (!ConfirmToUpdateExistedCondition())
                    return;
            }

            // save library here
            bool[] states = _edittingLibrary.States;
            _edittingLibrary.States = GetStatesFromUI();
            if (!SaveLibrary(_edittingLibrary))
            {
                // roll-back
                _edittingLibrary.States = states;                
            }
            else
            {
                _isLibModified = false;
            }
        }
        
        private void UpdateLibrary(MDCCParamLibrary lib)
        {
            ruleList.Items.Clear();
            foreach (MDCCParamItem item in lib.Items)
            {
                AddConditionToRuleList(item);
            }
        }

        private void AddConditionToRuleList(MDCCParamItem item)
        {
            int index = ruleList.Items.Add(item.RuleName);
            ruleList.SetItemChecked(index, item.ApplyCombination);
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            this.Update(false);
            string msg = ValidateRule(_edittingItem);
            if (msg != "")
            {
                ShowMsgError(msg);

                return;
            }

            PerformPreview(_edittingItem);


            //MDCCParam mdccParam = new MDCCParam();
            //mdccParam.Conditions = spRulesWizardCtrl.Condition;
            //mdccParam.DFSLevel = spRulesWizardCtrl.DFSLevel;

            //this.Cursor = Cursors.WaitCursor;
            //ArrayList superObjects = CombinationProcessor.DoCombine(
            //    _workspace.DetectedObjects, mdccParam);
            //this.Cursor = Cursors.Default;

            //if (superObjects == null)
            //    superObjects = new ArrayList();

            //_workspace.DetectedObjects = superObjects;
            //_workspace.Invalidate(true);

            //if (chkMinimize.Checked)
            //{
            //    this.Close();
            //    _workspace.Focus();
            //}
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //if (IsLibModified())
            //{
            //    DialogResult confirm =
            //        MessageBox.Show(this, 
            //        string.Format("{0}\n{1}\n{2}\n{3}",                    
            //        "The library has been modified!",
            //        "Do you want to continue apply combining?",
            //        "  - Choose YES to continue",
            //        "  - Choose NO to cancel"), "Warning", 
            //        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            //    if (confirm == DialogResult.No)
            //        return;
            //}

            List<MDCCParamItem> selectedConditions = this.GetSelectedConditions();
            if (selectedConditions.Count == 0)
            {
                ShowMsgError("Please select condition(s) from library to combine!");
                return;
            }

            PerformCombine(selectedConditions);

            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool IsLibModified()
        {
            if (_isNewLibrary && 
                _edittingLibrary.Items.Count == 0)
                return false;

            if (_isLibModified)
                return true;

            //int n = (int)Math.Min(
            //    ruleList.Items.Count, 
            //    _edittingLibrary.Items.Count);

            //for (int i = 0; i < n; i++)
            //{
            //    CheckState state = ruleList.GetItemCheckState(i);
            //    if (state == CheckState.Indeterminate)
            //        continue;

            //    bool isChecked = state == CheckState.Checked;

            //    if (isChecked != _edittingLibrary.Items[i].ApplyCombination)
            //        return true;
            //}

            return _isLibModified;
        }

        private void UpdateCheckState(MDCCParamLibrary lib, bool[] states)
        {
            lib.States = states;
        }

        private bool[] GetStatesFromUI()
        {
            if (ruleList.Items == null || ruleList.Items.Count == 0)
                return null;

            bool[] states = new bool[ruleList.Items.Count];

            int n = ruleList.Items.Count;
            for (int i = 0; i < n; i++)
            {
                CheckState state = ruleList.GetItemCheckState(i);
                if (state == CheckState.Indeterminate)
                    continue;

                states[i] = state == CheckState.Checked;
            }

            return states;
        }

        private List<MDCCParamItem> GetSelectedConditions()
        {
            if (_edittingLibrary.Items.Count == 0)
                return new List<MDCCParamItem>();

            List<MDCCParamItem> selectedConditions = 
                new List<MDCCParamItem>(_edittingLibrary.Items.Count);
            for (int i = 0; i < _edittingLibrary.Items.Count; i++)
            {
                CheckState state = ruleList.GetItemCheckState(i);
                if (state == CheckState.Checked)
                {
                    selectedConditions.Add(_edittingLibrary.Items[i]);
                }
            }

            return selectedConditions;
        }
        #endregion Lib

        #region Helpers
        private void ShowMsgError(string msg)
        {
            MessageBox.Show(this, msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private DialogResult Confirm(string msg)
        {
            return MessageBox.Show(this, msg, "Confirm", 
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }

        private MDCCParamItem IsExisted(MDCCParamItem item)
        {
            return _edittingLibrary.IsExisted(item);
        }

        private MDCCParamItem IsExisted(
            MDCCParamItem item, int exceptIndex)
        {
            return _edittingLibrary.IsExisted(item, exceptIndex);
        }
        #endregion Helpers
        
        private void ruleList_ItemCheck(object sender, ItemCheckEventArgs e)
        {            
            //CheckState checkState = e.CurrentValue;
            //if (checkState == CheckState.Indeterminate)
            //    return;

            //int index = e.Index;
            //if (index < 0)
            //    return;

            //checkState = 
            //    (checkState == CheckState.Checked ? CheckState.Unchecked : CheckState.Checked);

            //_edittingLibrary.Items[index].ApplyCombination = 
            //    (checkState == CheckState.Checked ? true : false);

            //_isLibModified = true;
        }
        
        private void ruleList_DoubleClick(object sender, EventArgs e)
        {
            //int index = ruleList.SelectedIndex;
            //if (index < 0)
            //    return;

            //UpdateSelectedEdittingItem(index);

            //btnDelete.Enabled = (_edittingIndex >= 0);
        }

        private void UpdateSelectedEdittingItem(int index)
        {
            if (_edittingIndex == index)
                return;

            if (_edittingIndex >= 0)
            {
                ruleList.Items[_edittingIndex] =
                    _edittingLibrary.Items[_edittingIndex].RuleName;
            }

            ruleList.Items[index] =
                string.Format("{0} - editting",
                _edittingLibrary.Items[index].RuleName);

            _edittingItem = new MDCCParamItem();
            _edittingItem.CopyFrom(_edittingLibrary.Items[index]);
            this.Update(true);
            _isRuleModified = false;

            UpdateEdittingWorkspace(true);

            _edittingIndex = index;

            if (_edittingIndex >= 0)
                _isNewCondition = false;
        }

        #region logic
        private bool ConfirmToUpdateNewCondition()
        {
            if (!_isNewCondition)
                return true;

            DialogResult confirm = Confirm("Do you want to save new rule to library?");
            if (confirm == DialogResult.Cancel)
                return false;
            if (confirm == DialogResult.No)
            {
                // continue
            }
            else if (confirm == DialogResult.Yes)
            {
                if (!AddNewConditionToLibrary())
                    return false;
            }

            return true;
        }

        private bool ConfirmToUpdateExistedCondition()
        {
            if (_isNewCondition)
                return true;

            DialogResult confirm = Confirm("The current rule has been modified.\nDo you want to update it?");
            if (confirm == DialogResult.Cancel)
                return false;

            if (confirm == DialogResult.No)
            {
                // continue
            }
            else if (confirm == DialogResult.Yes)
            {
                if (!UpdateExistedRuleInfo())
                    return false;
            }

            return true;
        }

        private bool ConfirmToSaveModifiedLibrary()
        {
            //if (!_isLibModified)
            //    return true;

            DialogResult confirm = Confirm("The library has been modified.\nDo you want to save it?");
            if (confirm == DialogResult.Cancel)
                return false;

            if (confirm == DialogResult.No)
            {
                // continue
            }
            else if (confirm == DialogResult.Yes)
            {
                bool[] states = _edittingLibrary.States;
                _edittingLibrary.States = GetStatesFromUI();
                if (!SaveLibrary(_edittingLibrary))
                {
                    // roll-back
                    _edittingLibrary.States = states;

                    return false;
                }
            }

            return true;
        }

        private bool AddNewConditionToLibrary()
        {
            this.Update(false);
            string msg = ValidateRule(_edittingItem);
            if (msg != "")
            {
                ShowMsgError(msg);
                return false;
            }

            if (IsExisted(_edittingItem) != null)
            {
                ShowMsgError("Rule name already exists!");

                return false;
            }

            // set is not new condition
            _isNewCondition = false;

            // set is not condition modified
            _isRuleModified = false;

            // add to library
            _edittingLibrary.Items.Add(_edittingItem);

            // set is library modified
            _isLibModified = true;

            // add to control
            _edittingIndex = ruleList.Items.Add(
                string.Format("{0} - editting", _edittingItem.RuleName));

            // set current index
            //_edittingIndex = ruleList.Items.Count - 1;

            // switch to edit existed rule mode
            _edittingItem = new MDCCParamItem();
            _edittingItem.CopyFrom(
                _edittingLibrary.Items[_edittingIndex]);

            return true;
        }

        private bool UpdateExistedRuleInfo()
        {
            this.Update(false);
            string msg = ValidateRule(_edittingItem);
            if (msg != "")
            {
                ShowMsgError(msg);
                return false;
            }

            if (!IsRuleModified())
                return true; // no need do any thing more

            // check existed name except current editting item
            if (IsExisted(_edittingItem, _edittingIndex) != null)
            {
                // msg here
                ShowMsgError("Rule name already exists!");

                return false;
            }

            // set is not condition modified
            _isRuleModified = false;

            // update existed rule in library
            _edittingLibrary.Items[_edittingIndex].CopyFrom(_edittingItem);

            // set is library modified
            _isLibModified = true;

            // update current item in control
            ruleList.Items[_edittingIndex] =
                string.Format("{0} - editting", _edittingItem.RuleName);

            return true;
        }

        private bool SaveLibrary(MDCCParamLibrary library)
        {
            // everything is ok to save library, 
            // except file path validation...
            bool succeed = false;
            try
            {
                using (SaveFileDialog dlg = new SaveFileDialog())
                {
                    dlg.Title = "Specify file name";
                    dlg.RestoreDirectory = true;
                    dlg.Filter = "SIA Combine Condition Library (*.sscl)|*.sscl";
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        _edittingLibrary.Serialize(dlg.FileName);

                        succeed = true;
                    }
                }
            }
            catch
            {
                succeed = false;
            }

            return succeed;
        }

        private MDCCParamLibrary LoadLibrary()
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
        #endregion logic

        #region Perform Preview
        // ArrayList of ArrayList
        private ArrayList _storageObjects = new ArrayList();

        private bool PerformPreview(MDCCParamItem rule)
        {
            ArrayList workingObjects = StoreWorkingingObjects();
            _storageObjects.Add(workingObjects);

            bool bStatus = false;

            int count = 
                _workspace.DetectedObjects.Count;

            this.Cursor = Cursors.WaitCursor;
            ArrayList superObjects = 
                CombinationProcessor.DoCombine(
                _workspace.DetectedObjects, rule, _workspace.GetCurrentMetrologySystem());
            this.Cursor = Cursors.Default;

            if (superObjects == null)
                superObjects = new ArrayList();

            _workspace.DetectedObjects = superObjects;
            _workspace.Invalidate(true);

            if (chkMinimize.Checked)
            {
                this.Close();
                _workspace.Focus();
            }

            bStatus = count == _workspace.DetectedObjects.Count;

            if (bStatus)
            {
                _storageObjects.RemoveAt(_storageObjects.Count - 1);
            }

            btnBack.Enabled = _storageObjects.Count > 0;

            return bStatus;
        }

        private bool PerformCombine(List<MDCCParamItem> rules)
        {
            _storageObjects.Clear();
            btnBack.Enabled = _storageObjects.Count > 0;
            
            if (_workspace.DetectedObjects.Count == 0)
                return true;

            ArrayList detectedObjs = new ArrayList();
            detectedObjs.AddRange(_workspace.DetectedObjects);

            this.Cursor = Cursors.WaitCursor;
            foreach (MDCCParamItem rule in rules)
            {
                ArrayList superObjects =
                    CombinationProcessor.DoCombine(detectedObjs, rule, _workspace.GetCurrentMetrologySystem());

                if (superObjects.Count == detectedObjs.Count)
                    continue;

                detectedObjs = superObjects;
                if (detectedObjs.Count == 1)
                    break;
            }
            this.Cursor = Cursors.Default;

            _workspace.DetectedObjects = detectedObjs;
            _workspace.Invalidate(true);

            return true;
        }

        private ArrayList StoreWorkingingObjects()
        {
            if (_workspace.DetectedObjects == null ||
                _workspace.DetectedObjects.Count == 0)
                return new ArrayList();

            ArrayList objs = new ArrayList(_workspace.DetectedObjects.Count);
            objs.AddRange(_workspace.DetectedObjects);

            return objs;
        }

        public void ClearStorageWorkingObjects()
        {
            _storageObjects.Clear();
            btnBack.Enabled = _storageObjects.Count > 0;
        }
        #endregion Perform Preview

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (_storageObjects != null)
            
            btnBack.Enabled = (_storageObjects.Count - 1) > 0;

            if (_storageObjects.Count == 0)
                return;

            _workspace.DetectedObjects = _storageObjects[_storageObjects.Count - 1] as ArrayList;
            _workspace.Invalidate(true);

            _storageObjects.RemoveAt(_storageObjects.Count - 1);
        }
    }
}
