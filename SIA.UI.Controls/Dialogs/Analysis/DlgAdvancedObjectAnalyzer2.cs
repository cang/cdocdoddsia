using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using SiGlaz.ObjectAnalysis.UI;
using SIA.UI.Controls.ObjectAnalysis;

namespace SIA.UI.Controls.Dialogs
{
    public partial class DlgAdvancedObjectAnalyzer2 : Form //: FloatingFormBase
    {
        #region Member fields
        private ImageWorkspace _workspace = null;
        private ArrayList _objects = null;        
        #endregion Member fields

        #region Constructors and destructors
        public DlgAdvancedObjectAnalyzer2(
            ImageWorkspace workSpace, ArrayList objects)
            //: base()
        {
            InitializeComponent();

            //spRulesWizardCtrl.IsLimitedCombinationCompareOperator = true;

            // initialize top level window
            this.TopLevel = true;
            //this.TopMost = true;

            //FadeOutEnabled = true;

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

            //UpdateEdittingWorkspace(false);

            //this.ClearStorageWorkingObjects();

            Initialize();
        }

        void _workspace_ImageDataChanged(object sender, EventArgs e)
        {
            //this.ClearStorageWorkingObjects();
        }
        #endregion Constructors and destructors

        #region Initialization
        private ucObjectAnalyzerStep[] _steps = null;
        private void Initialize()
        {
            _steps = new ucObjectAnalyzerStep[] {
                new ucABSPreprocessingStep(),
                new ucABSPrimitiveObjectDetectionStep(),
                new ucPrimitiveObjectFilterStep(),
                new ucObjectCombinationStep(),
                new ucObjectFilterStep(),
                new ucObjectClassificationStep(),
                new ucObjectExportingStep()
            };

            ucAdvancedObjectAnalyzer1.SetObjectAnalyzerSteps(_steps);

            ucAdvancedObjectAnalyzer1.SetActiveStep(3);
        }
        #endregion Initialization

        #region Overrides
        protected override void OnClosing(CancelEventArgs e)
        {
            // cancel the closing operation just hide this window out
            e.Cancel = true;
            this.Visible = false;

            _workspace.Focus();

            base.OnClosing(e);           
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
            else if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
                this.Close();
            }

            base.OnSizeChanged(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        #endregion Overrides
    }
}
