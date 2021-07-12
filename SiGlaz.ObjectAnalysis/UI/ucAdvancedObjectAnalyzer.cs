using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SiGlaz.ObjectAnalysis.UI
{
    public partial class ucAdvancedObjectAnalyzer : UserControl
    {
        #region Member fields
        private ucObjectAnalyzerStep[] _steps = null;
        private RadioButton[] _rdSteps = null;
        #endregion Member fields

        #region Contructors
        public ucAdvancedObjectAnalyzer()
        {
            InitializeComponent();

            _rdSteps = new RadioButton[] {
                rdPreprocessing,
                rdDetectPrimitiveObjects,
                rdFilterPrimitiveObjects,
                rdCombineObjects,
                rdFilterObjects,
                rdClassifyObjects,
                rdOutputOptions
            };

            foreach (RadioButton rdStep in _rdSteps)
            {
                if (rdStep.FlatStyle != FlatStyle.Flat)
                    rdStep.FlatStyle = FlatStyle.Flat;

                rdStep.CheckedChanged += new EventHandler(rdStep_CheckedChanged);
            }
        }
       
        //public ucAdvancedObjectAnalyzer(ucObjectAnalyzerStep[] steps)
        //    : this()
        //{
        //    SetObjectAnalyzerSteps(steps);
        //}

        public void SetObjectAnalyzerSteps(ucObjectAnalyzerStep[] steps)
        {
            _steps = new ucObjectAnalyzerStep[_rdSteps.Length];
            foreach (ucObjectAnalyzerStep step in steps)
            {
                if (step.StepId >= _steps.Length)
                {
                    // index out of bound
                    continue;
                }

                if (step.Dock != DockStyle.Fill)
                    step.Dock = DockStyle.Fill;

                _steps[step.StepId] = step;
            }
        }
        #endregion Contructors

        #region Properties
        protected int this[ucObjectAnalyzerStep step]
        {
            get
            {
                // don't need to hash here => cardinal is too small

                int nSteps = _steps.Length;
                for (int stepId = 0; stepId < nSteps; stepId++)
                {
                    if (_steps[stepId] == step)
                        return stepId;
                }

                return -1;
            }
        }

        protected int this[RadioButton rdStep]
        {
            get
            {
                // don't need to hash here => cardinal is too small

                int nSteps = _rdSteps.Length;
                for (int stepId = 0; stepId < nSteps; stepId++)
                {
                    if (_rdSteps[stepId] == rdStep)
                        return stepId;
                }

                return -1;
            }
        }
        #endregion Properties

        #region Step changed
        private void rdStep_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdStep = sender as RadioButton;
            OnStepVisibleChanged(rdStep);
        }

        protected virtual void OnStepVisibleChanged(RadioButton rdStep)
        {
            if (!rdStep.Checked)
            {
                if (rdStep.BackColor != SystemColors.Control)
                    rdStep.BackColor = SystemColors.Control;

                return;
            }

            // clear current view
            pnBody.Controls.Clear();

            // update current view
            int stepId = this[rdStep];
            ucObjectAnalyzerStep step = _steps[stepId];
            if (step != null)
            {
                rdStep.BackColor = SystemColors.Highlight;
                pnBody.Controls.Add(step);
            }
        }
        #endregion Step changed

        #region Serializations
        private void btnLoad_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }
        #endregion Serializations

        #region Navigations
        private int _currentStepId = 0;
        public virtual void SetActiveStep(int stepId)
        {
            int nSteps = _rdSteps.Length;
            for (int i = 0; i < nSteps; i++)
            {
                if (i == stepId)
                {
                    if (!_rdSteps[i].Checked)
                        _rdSteps[i].Checked = true;

                    _currentStepId = stepId;

                    continue;
                }

                if (_rdSteps[i].Checked)
                    _rdSteps[i].Checked = false;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {

        }

        private void btnNext_Click(object sender, EventArgs e)
        {

        }
        #endregion Navigations
    }
}
