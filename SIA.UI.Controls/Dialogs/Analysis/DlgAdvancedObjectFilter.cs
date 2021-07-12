using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using SiGlaz.ObjectAnalysis.Common;
using SiGlaz.ObjectAnalysis.Engine;
using SIA.UI.Controls.Automation.Commands;

namespace SIA.UI.Controls.Dialogs.Analysis
{
    public partial class DlgAdvancedObjectFilter : FloatingFormBase
    {
        ImageWorkspace _workspace = null;
        ArrayList _objList = new ArrayList();
        private MDCCParamFilter _edittingFilter = new MDCCParamFilter();

        public DlgAdvancedObjectFilter(ImageWorkspace worksapce, ArrayList objList)
            : base()
        {
            InitializeComponent();

            // initialize top level window
            this.TopLevel = true;
            //this.TopMost = true;

            FadeOutEnabled = true;

            _workspace = worksapce;
            _objList = objList;

            if (_workspace != null)
            {
                this.Owner = _workspace.FindForm();
                //_workspace.DataChanged += new EventHandler(_workspace_ImageDataChanged);
            }

//#if DEBUG__
//            btnOK.Enabled = false;
//            btnCancel.Enabled = false;
//#endif

            //UpdateEdittingWorkspace(false);

            //this.ClearStorageWorkingObjects();
        }

        public DlgAdvancedObjectFilter(AdObjectFilterSettings settings)
        {
            InitializeComponent();

            //Set for toolbox
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ShowIcon = false;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;

            _edittingFilter = settings.ParamFilter;

            UpdateData(true);
            
        }

        public void UpdateData(bool update)
        {
            if (update)//update to control
            {
                _ucObjectFilter.unverControl.UseOrAnd = (_edittingFilter.TypeQuery != QUERY_TYPE.AND);
                _ucObjectFilter.unverControl.ConditionExpression = _edittingFilter.StrQuery;                
                //_ucObjectFilter.unverControl.ConditionExpression = SiGlaz.ObjectAnalysis.Engine.BaseQuery.ConvertToOldTypeQuery(_edittingFilter.StrQuery);                
            }
            else
            {
                _edittingFilter.TypeQuery = _ucObjectFilter.unverControl.UseOrAnd ? QUERY_TYPE.OR : QUERY_TYPE.AND;
                _edittingFilter.StrQuery = _ucObjectFilter.unverControl.ConditionExpression;                
                //_edittingFilter.StrQuery = SiGlaz.ObjectAnalysis.Engine.BaseQuery.ConvertFromOldTypeQuery(_ucObjectFilter.unverControl.ConditionExpression);                
            }
        }

        public MDCCParamFilter ParamFilter
        {
            get
            {
                return _edittingFilter;
            }
        }

        private bool PerformFilter(MDCCParamFilter paramFilter)
        {
            if (_workspace.DetectedObjects.Count == 0)
                return true;

            ArrayList detectedObjs = new ArrayList();
            detectedObjs.AddRange(_workspace.DetectedObjects);

            this.Cursor = Cursors.WaitCursor;
            ArrayList superObjects =
                        FilterProcessor.DoFilter(paramFilter.StrQuery, paramFilter.TypeQuery, detectedObjs, _workspace.GetCurrentMetrologySystem());
            detectedObjs = superObjects;

            this.Cursor = Cursors.Default;

            _workspace.DetectedObjects = detectedObjs;
            _workspace.Invalidate(true);

            return true;
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            if (!_ucObjectFilter.IsValidate)
            {
                MessageBox.Show(this, _ucObjectFilter.sValidateMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            UpdateData(false);

            if (_workspace != null)
            {                
                PerformFilter(_edittingFilter);
            }
            
            DialogResult = DialogResult.OK;
            this.Close();
        }

        #region Overrides
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            // cancel the closing operation just hide this window out
            if (_workspace != null)
            {
                e.Cancel = true;
                this.Visible = false;

                _workspace.Focus();
            }           
            
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

        private void button_Load_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Choose file";
                dlg.RestoreDirectory = true;
                dlg.Filter = "SIA Filter Condition (*.ssfr)|*.ssfr";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    MDCCParamFilter item =
                        MDCCParamFilter.Deserialize(dlg.FileName);

                    if (item != null)
                    {
                        _edittingFilter.CopyFrom(item);

                        // update to control
                        UpdateData(true);                        
                    }
                    else
                        MessageBox.Show(this, "Invalid SIA Filter Condition file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            if (!_ucObjectFilter.IsValidate)
            {
                MessageBox.Show(this, _ucObjectFilter.sValidateMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Title = "Specify file name";
                dlg.RestoreDirectory = true;
                dlg.Filter = "SIA Filter Condition (*.ssfr)|*.ssfr";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    UpdateData(false);
                    _edittingFilter.Serialize(dlg.FileName);
                }
            }
        }
    }
}
