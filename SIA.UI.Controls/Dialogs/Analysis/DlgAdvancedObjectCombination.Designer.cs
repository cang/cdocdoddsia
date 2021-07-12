namespace SIA.UI.Controls.Dialogs
{
    partial class DlgAdvancedObjectCombination
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgAdvancedObjectCombination));
            this.pnWorkspace = new System.Windows.Forms.Panel();
            this.pnEditting = new System.Windows.Forms.Panel();
            this.spRulesWizardCtrl = new SiGlaz.ObjectAnalysis.UI.SPRulesWizardCtrl();
            this.pnFooter = new System.Windows.Forms.Panel();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnSavetoLib = new System.Windows.Forms.Button();
            this.btnPreviewRule = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.pnHeader = new System.Windows.Forms.Panel();
            this.txtSigName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRuleName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnLeftSeparator = new System.Windows.Forms.Panel();
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
            this.pnBottom = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.chkMinimize = new System.Windows.Forms.CheckBox();
            this.pnWorkspace.SuspendLayout();
            this.pnEditting.SuspendLayout();
            this.pnFooter.SuspendLayout();
            this.pnHeader.SuspendLayout();
            this.pnLeft.SuspendLayout();
            this.toolbarLib.SuspendLayout();
            this.pnBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnWorkspace
            // 
            this.pnWorkspace.BackColor = System.Drawing.SystemColors.Control;
            this.pnWorkspace.Controls.Add(this.pnEditting);
            this.pnWorkspace.Controls.Add(this.pnLeftSeparator);
            this.pnWorkspace.Controls.Add(this.pnLeft);
            this.pnWorkspace.Controls.Add(this.pnBottom);
            this.pnWorkspace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnWorkspace.Location = new System.Drawing.Point(0, 0);
            this.pnWorkspace.Name = "pnWorkspace";
            this.pnWorkspace.Size = new System.Drawing.Size(723, 446);
            this.pnWorkspace.TabIndex = 0;
            // 
            // pnEditting
            // 
            this.pnEditting.BackColor = System.Drawing.Color.White;
            this.pnEditting.Controls.Add(this.spRulesWizardCtrl);
            this.pnEditting.Controls.Add(this.pnFooter);
            this.pnEditting.Controls.Add(this.pnHeader);
            this.pnEditting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnEditting.Location = new System.Drawing.Point(202, 0);
            this.pnEditting.Name = "pnEditting";
            this.pnEditting.Size = new System.Drawing.Size(521, 398);
            this.pnEditting.TabIndex = 4;
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
            this.spRulesWizardCtrl.Location = new System.Drawing.Point(0, 28);
            this.spRulesWizardCtrl.Name = "spRulesWizardCtrl";
            this.spRulesWizardCtrl.OptionMode = SiGlaz.ObjectAnalysis.Common.COMPARE_OPERATOR.NONE;
            this.spRulesWizardCtrl.Size = new System.Drawing.Size(521, 337);
            this.spRulesWizardCtrl.TabIndex = 0;
            // 
            // pnFooter
            // 
            this.pnFooter.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pnFooter.Controls.Add(this.btnBack);
            this.pnFooter.Controls.Add(this.btnSavetoLib);
            this.pnFooter.Controls.Add(this.btnPreviewRule);
            this.pnFooter.Controls.Add(this.btnSave);
            this.pnFooter.Controls.Add(this.btnLoad);
            this.pnFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnFooter.Location = new System.Drawing.Point(0, 365);
            this.pnFooter.Name = "pnFooter";
            this.pnFooter.Size = new System.Drawing.Size(521, 33);
            this.pnFooter.TabIndex = 4;
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.Enabled = false;
            this.btnBack.Location = new System.Drawing.Point(295, 5);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(56, 23);
            this.btnBack.TabIndex = 5;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnSavetoLib
            // 
            this.btnSavetoLib.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSavetoLib.Location = new System.Drawing.Point(419, 5);
            this.btnSavetoLib.Name = "btnSavetoLib";
            this.btnSavetoLib.Size = new System.Drawing.Size(94, 23);
            this.btnSavetoLib.TabIndex = 3;
            this.btnSavetoLib.Text = "Save to Library";
            this.btnSavetoLib.UseVisualStyleBackColor = true;
            this.btnSavetoLib.Click += new System.EventHandler(this.btnSavetoLib_Click);
            // 
            // btnPreviewRule
            // 
            this.btnPreviewRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPreviewRule.Location = new System.Drawing.Point(357, 5);
            this.btnPreviewRule.Name = "btnPreviewRule";
            this.btnPreviewRule.Size = new System.Drawing.Size(56, 23);
            this.btnPreviewRule.TabIndex = 2;
            this.btnPreviewRule.Text = "Preview";
            this.btnPreviewRule.UseVisualStyleBackColor = true;
            this.btnPreviewRule.Click += new System.EventHandler(this.btnPreviewRule_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(67, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(56, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(5, 5);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(56, 23);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // pnHeader
            // 
            this.pnHeader.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pnHeader.Controls.Add(this.txtSigName);
            this.pnHeader.Controls.Add(this.label2);
            this.pnHeader.Controls.Add(this.txtRuleName);
            this.pnHeader.Controls.Add(this.label1);
            this.pnHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnHeader.Location = new System.Drawing.Point(0, 0);
            this.pnHeader.Name = "pnHeader";
            this.pnHeader.Size = new System.Drawing.Size(521, 28);
            this.pnHeader.TabIndex = 3;
            // 
            // txtSigName
            // 
            this.txtSigName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSigName.Location = new System.Drawing.Point(346, 3);
            this.txtSigName.Name = "txtSigName";
            this.txtSigName.Size = new System.Drawing.Size(170, 20);
            this.txtSigName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(205, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Classify combined object is:";
            // 
            // txtRuleName
            // 
            this.txtRuleName.Location = new System.Drawing.Point(45, 3);
            this.txtRuleName.Name = "txtRuleName";
            this.txtRuleName.Size = new System.Drawing.Size(154, 20);
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
            // pnLeftSeparator
            // 
            this.pnLeftSeparator.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnLeftSeparator.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnLeftSeparator.Location = new System.Drawing.Point(200, 0);
            this.pnLeftSeparator.Name = "pnLeftSeparator";
            this.pnLeftSeparator.Size = new System.Drawing.Size(2, 398);
            this.pnLeftSeparator.TabIndex = 2;
            // 
            // pnLeft
            // 
            this.pnLeft.Controls.Add(this.ruleList);
            this.pnLeft.Controls.Add(this.toolbarLib);
            this.pnLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnLeft.Location = new System.Drawing.Point(0, 0);
            this.pnLeft.Name = "pnLeft";
            this.pnLeft.Size = new System.Drawing.Size(200, 398);
            this.pnLeft.TabIndex = 2;
            // 
            // ruleList
            // 
            this.ruleList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ruleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ruleList.FormattingEnabled = true;
            this.ruleList.Location = new System.Drawing.Point(0, 25);
            this.ruleList.Name = "ruleList";
            this.ruleList.Size = new System.Drawing.Size(200, 362);
            this.ruleList.TabIndex = 2;
            this.ruleList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ruleList_ItemCheck);
            this.ruleList.DoubleClick += new System.EventHandler(this.ruleList_DoubleClick);
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
            this.toolbarLib.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolbarLibItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolbarNewLibrary
            // 
            this.toolbarNewLibrary.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarNewLibrary.Image = global::SIA.UI.Controls.Properties.Resources.zone_lib_new;
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
            this.toolbarOpen.Image = global::SIA.UI.Controls.Properties.Resources.open;
            this.toolbarOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarOpen.Name = "toolbarOpen";
            this.toolbarOpen.Size = new System.Drawing.Size(23, 22);
            this.toolbarOpen.Text = "Open";
            this.toolbarOpen.ToolTipText = "Open Library";
            // 
            // toolbarSave
            // 
            this.toolbarSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarSave.Image = global::SIA.UI.Controls.Properties.Resources.zone_lib_save;
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
            this.toolbarNewCondition.Image = global::SIA.UI.Controls.Properties.Resources.new_object;
            this.toolbarNewCondition.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarNewCondition.Name = "toolbarNewCondition";
            this.toolbarNewCondition.Size = new System.Drawing.Size(23, 22);
            this.toolbarNewCondition.Text = "New Condition";
            // 
            // toolbarEditCondition
            // 
            this.toolbarEditCondition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarEditCondition.Image = global::SIA.UI.Controls.Properties.Resources.edit_available_object;
            this.toolbarEditCondition.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarEditCondition.Name = "toolbarEditCondition";
            this.toolbarEditCondition.Size = new System.Drawing.Size(23, 22);
            this.toolbarEditCondition.Text = "Edit Condition";
            // 
            // toolbarDelCondition
            // 
            this.toolbarDelCondition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarDelCondition.Image = global::SIA.UI.Controls.Properties.Resources.delete_obj;
            this.toolbarDelCondition.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarDelCondition.Name = "toolbarDelCondition";
            this.toolbarDelCondition.Size = new System.Drawing.Size(23, 22);
            this.toolbarDelCondition.Text = "Delete Condition";
            // 
            // pnBottom
            // 
            this.pnBottom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnBottom.Controls.Add(this.btnCancel);
            this.pnBottom.Controls.Add(this.btnOK);
            this.pnBottom.Controls.Add(this.btnPreview);
            this.pnBottom.Controls.Add(this.chkMinimize);
            this.pnBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnBottom.Location = new System.Drawing.Point(0, 398);
            this.pnBottom.Name = "pnBottom";
            this.pnBottom.Size = new System.Drawing.Size(723, 48);
            this.pnBottom.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(634, 14);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(553, 14);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPreview.Location = new System.Drawing.Point(472, 14);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(75, 23);
            this.btnPreview.TabIndex = 1;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Visible = false;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // chkMinimize
            // 
            this.chkMinimize.AutoSize = true;
            this.chkMinimize.Location = new System.Drawing.Point(12, 19);
            this.chkMinimize.Name = "chkMinimize";
            this.chkMinimize.Size = new System.Drawing.Size(174, 17);
            this.chkMinimize.TabIndex = 0;
            this.chkMinimize.Text = "Minimize window when preview";
            this.chkMinimize.UseVisualStyleBackColor = true;
            // 
            // DlgAdvancedObjectCombination
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 446);
            this.Controls.Add(this.pnWorkspace);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.MinimumSize = new System.Drawing.Size(700, 436);
            this.Name = "DlgAdvancedObjectCombination";
            this.ShowInTaskbar = false;
            this.Text = "Combine Objects";
            this.pnWorkspace.ResumeLayout(false);
            this.pnEditting.ResumeLayout(false);
            this.pnFooter.ResumeLayout(false);
            this.pnHeader.ResumeLayout(false);
            this.pnHeader.PerformLayout();
            this.pnLeft.ResumeLayout(false);
            this.pnLeft.PerformLayout();
            this.toolbarLib.ResumeLayout(false);
            this.toolbarLib.PerformLayout();
            this.pnBottom.ResumeLayout(false);
            this.pnBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnWorkspace;
        private System.Windows.Forms.Panel pnBottom;
        private SiGlaz.ObjectAnalysis.UI.SPRulesWizardCtrl spRulesWizardCtrl;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.CheckBox chkMinimize;
        private System.Windows.Forms.Panel pnEditting;
        private System.Windows.Forms.Panel pnFooter;
        private System.Windows.Forms.Panel pnHeader;
        private System.Windows.Forms.Panel pnLeft;
        private System.Windows.Forms.Panel pnLeftSeparator;
        private System.Windows.Forms.ToolStrip toolbarLib;
        private System.Windows.Forms.ToolStripButton toolbarOpen;
        private System.Windows.Forms.ToolStripButton toolbarSave;
        private System.Windows.Forms.CheckedListBox ruleList;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.TextBox txtRuleName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSavetoLib;
        private System.Windows.Forms.Button btnPreviewRule;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtSigName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolbarNewLibrary;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolbarNewCondition;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.ToolStripButton toolbarEditCondition;
        private System.Windows.Forms.ToolStripButton toolbarDelCondition;
    }
}