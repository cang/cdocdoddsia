using System;
using System.Collections.Generic;
using System.Text;

namespace SiGlaz.ObjectAnalysis.UI
{
    public class ucObjectCombinationStep : ucObjectAnalyzerStep
    {
        public const int DefaultStepId = 3;

        #region Member fields
        #endregion Member fields

        #region Contructors and destructors
        public ucObjectCombinationStep()
            : base(DefaultStepId)
        {
            InitializeComponent();

            chkStatus.Text = "Apply combining object(s)";

            SupportSearialization = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }
        #endregion Contructors and destructors

        #region Component Designer generated code

        #region Window Form fields
        private System.Windows.Forms.Panel pnLeft;
        private System.Windows.Forms.CheckedListBox ruleList;
        private System.Windows.Forms.ToolStrip toolbarLib;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolbarNewLibrary;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolbarOpen;
        private System.Windows.Forms.ToolStripButton toolbarSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolbarNewCondition;
        private System.Windows.Forms.ToolStripButton toolbarEditCondition;
        private System.Windows.Forms.ToolStripButton toolbarDelCondition;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtSigName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRuleName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnSavetoLib;
        private System.Windows.Forms.Button btnPreviewRule;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoad;
        private SPRulesWizardCtrl spRulesWizardCtrl;
        #endregion Window Form fields

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucObjectCombinationStep));
            this.pnLeft = new System.Windows.Forms.Panel();
            this.ruleList = new System.Windows.Forms.CheckedListBox();
            this.toolbarLib = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolbarNewLibrary = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolbarOpen = new System.Windows.Forms.ToolStripButton();
            this.toolbarSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolbarNewCondition = new System.Windows.Forms.ToolStripButton();
            this.toolbarEditCondition = new System.Windows.Forms.ToolStripButton();
            this.toolbarDelCondition = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtSigName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRuleName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnSavetoLib = new System.Windows.Forms.Button();
            this.btnPreviewRule = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.spRulesWizardCtrl = new SiGlaz.ObjectAnalysis.UI.SPRulesWizardCtrl();
            this.pnHeader.SuspendLayout();
            this.pnBody.SuspendLayout();
            this.pnLeft.SuspendLayout();
            this.toolbarLib.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnHeader
            // 
            this.pnHeader.Size = new System.Drawing.Size(827, 32);
            // 
            // pnFooter
            // 
            this.pnFooter.Location = new System.Drawing.Point(0, 533);
            this.pnFooter.Size = new System.Drawing.Size(827, 32);
            // 
            // pnBody
            // 
            this.pnBody.Controls.Add(this.spRulesWizardCtrl);
            this.pnBody.Controls.Add(this.panel2);
            this.pnBody.Controls.Add(this.panel1);
            this.pnBody.Controls.Add(this.pnLeft);
            this.pnBody.Size = new System.Drawing.Size(827, 501);
            // 
            // btnSaveStepSettings
            // 
            this.btnSaveStepSettings.Location = new System.Drawing.Point(4153, 5);
            // 
            // btnLoadStepSettings
            // 
            this.btnLoadStepSettings.Location = new System.Drawing.Point(4072, 5);
            // 
            // pnLeft
            // 
            this.pnLeft.Controls.Add(this.ruleList);
            this.pnLeft.Controls.Add(this.toolbarLib);
            this.pnLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnLeft.Location = new System.Drawing.Point(0, 0);
            this.pnLeft.Name = "pnLeft";
            this.pnLeft.Size = new System.Drawing.Size(200, 501);
            this.pnLeft.TabIndex = 3;
            // 
            // ruleList
            // 
            this.ruleList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ruleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ruleList.FormattingEnabled = true;
            this.ruleList.Location = new System.Drawing.Point(0, 25);
            this.ruleList.Name = "ruleList";
            this.ruleList.Size = new System.Drawing.Size(200, 467);
            this.ruleList.TabIndex = 2;
            // 
            // toolbarLib
            // 
            this.toolbarLib.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolbarLib.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator2,
            this.toolbarNewLibrary,
            this.toolStripSeparator1,
            this.toolbarOpen,
            this.toolbarSave,
            this.toolStripSeparator3,
            this.toolbarNewCondition,
            this.toolbarEditCondition,
            this.toolbarDelCondition});
            this.toolbarLib.Location = new System.Drawing.Point(0, 0);
            this.toolbarLib.Name = "toolbarLib";
            this.toolbarLib.Size = new System.Drawing.Size(200, 25);
            this.toolbarLib.TabIndex = 1;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolbarNewLibrary
            // 
            this.toolbarNewLibrary.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarNewLibrary.Image = global::SiGlaz.ObjectAnalysis.Properties.Resources.zone_lib_new;
            this.toolbarNewLibrary.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarNewLibrary.Name = "toolbarNewLibrary";
            this.toolbarNewLibrary.Size = new System.Drawing.Size(23, 22);
            this.toolbarNewLibrary.Text = "New Library";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolbarOpen
            // 
            this.toolbarOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarOpen.Image = global::SiGlaz.ObjectAnalysis.Properties.Resources.open;
            this.toolbarOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarOpen.Name = "toolbarOpen";
            this.toolbarOpen.Size = new System.Drawing.Size(23, 22);
            this.toolbarOpen.Text = "Open";
            this.toolbarOpen.ToolTipText = "Open Library";
            // 
            // toolbarSave
            // 
            this.toolbarSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarSave.Image = global::SiGlaz.ObjectAnalysis.Properties.Resources.zone_lib_save;
            this.toolbarSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarSave.Name = "toolbarSave";
            this.toolbarSave.Size = new System.Drawing.Size(23, 22);
            this.toolbarSave.Text = "Save";
            this.toolbarSave.ToolTipText = "Save Library";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolbarNewCondition
            // 
            this.toolbarNewCondition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarNewCondition.Image = global::SiGlaz.ObjectAnalysis.Properties.Resources.new_object;
            this.toolbarNewCondition.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarNewCondition.Name = "toolbarNewCondition";
            this.toolbarNewCondition.Size = new System.Drawing.Size(23, 22);
            this.toolbarNewCondition.Text = "New Condition";
            // 
            // toolbarEditCondition
            // 
            this.toolbarEditCondition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarEditCondition.Image = global::SiGlaz.ObjectAnalysis.Properties.Resources.edit_available_object;
            this.toolbarEditCondition.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarEditCondition.Name = "toolbarEditCondition";
            this.toolbarEditCondition.Size = new System.Drawing.Size(23, 22);
            this.toolbarEditCondition.Text = "Edit Condition";
            // 
            // toolbarDelCondition
            // 
            this.toolbarDelCondition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarDelCondition.Image = global::SiGlaz.ObjectAnalysis.Properties.Resources.delete_obj;
            this.toolbarDelCondition.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarDelCondition.Name = "toolbarDelCondition";
            this.toolbarDelCondition.Size = new System.Drawing.Size(23, 22);
            this.toolbarDelCondition.Text = "Delete Condition";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txtSigName);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtRuleName);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(200, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(627, 28);
            this.panel1.TabIndex = 4;
            // 
            // txtSigName
            // 
            this.txtSigName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSigName.Location = new System.Drawing.Point(325, 3);
            this.txtSigName.Name = "txtSigName";
            this.txtSigName.Size = new System.Drawing.Size(341, 20);
            this.txtSigName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(186, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Classify combined object is:";
            // 
            // txtRuleName
            // 
            this.txtRuleName.Location = new System.Drawing.Point(45, 3);
            this.txtRuleName.Name = "txtRuleName";
            this.txtRuleName.Size = new System.Drawing.Size(135, 20);
            this.txtRuleName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(4, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel2.Controls.Add(this.btnBack);
            this.panel2.Controls.Add(this.btnSavetoLib);
            this.panel2.Controls.Add(this.btnPreviewRule);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.btnLoad);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(200, 468);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(627, 33);
            this.panel2.TabIndex = 5;
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.Enabled = false;
            this.btnBack.Location = new System.Drawing.Point(401, 5);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(56, 23);
            this.btnBack.TabIndex = 5;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Visible = false;
            // 
            // btnSavetoLib
            // 
            this.btnSavetoLib.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSavetoLib.Location = new System.Drawing.Point(525, 5);
            this.btnSavetoLib.Name = "btnSavetoLib";
            this.btnSavetoLib.Size = new System.Drawing.Size(94, 23);
            this.btnSavetoLib.TabIndex = 3;
            this.btnSavetoLib.Text = "Save to Library";
            this.btnSavetoLib.UseVisualStyleBackColor = true;
            // 
            // btnPreviewRule
            // 
            this.btnPreviewRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPreviewRule.Location = new System.Drawing.Point(463, 5);
            this.btnPreviewRule.Name = "btnPreviewRule";
            this.btnPreviewRule.Size = new System.Drawing.Size(56, 23);
            this.btnPreviewRule.TabIndex = 2;
            this.btnPreviewRule.Text = "Preview";
            this.btnPreviewRule.UseVisualStyleBackColor = true;
            this.btnPreviewRule.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(67, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(56, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(5, 5);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(56, 23);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            // 
            // spRulesWizardCtrl
            // 
            this.spRulesWizardCtrl.Condition = ((System.Collections.ArrayList)(resources.GetObject("spRulesWizardCtrl.Condition")));
            this.spRulesWizardCtrl.DescriptionDetail = "           primitive objects are included in the same object if between them:";
            this.spRulesWizardCtrl.DescriptionHead = "Which condition(s) do you want to check?";
            this.spRulesWizardCtrl.DescriptionLink = "Criterion";
            this.spRulesWizardCtrl.DescriptionStep1 = "Step 1: Select condition(s)";
            this.spRulesWizardCtrl.DescriptionStep2 = "Step 2: Edit the rule description (click hyperlink)";
            this.spRulesWizardCtrl.DFSLevel = 2;
            this.spRulesWizardCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spRulesWizardCtrl.HeaderSize = 208;
            this.spRulesWizardCtrl.Location = new System.Drawing.Point(200, 28);
            this.spRulesWizardCtrl.Name = "spRulesWizardCtrl";
            this.spRulesWizardCtrl.OptionMode = SiGlaz.ObjectAnalysis.Common.COMPARE_OPERATOR.NONE;
            this.spRulesWizardCtrl.Size = new System.Drawing.Size(627, 440);
            this.spRulesWizardCtrl.TabIndex = 6;
            // 
            // ucObjectCombinationStep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "ucObjectCombinationStep";
            this.Size = new System.Drawing.Size(827, 565);
            this.pnHeader.ResumeLayout(false);
            this.pnHeader.PerformLayout();
            this.pnBody.ResumeLayout(false);
            this.pnLeft.ResumeLayout(false);
            this.pnLeft.PerformLayout();
            this.toolbarLib.ResumeLayout(false);
            this.toolbarLib.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion Component Designer generated code

        #region Methods
        public virtual bool CanNext()
        {
            return true;
        }

        public virtual bool CanBack()
        {
            return true;
        }
        #endregion Methods
    }
}
