#define USE_RASTER_COMMAND

using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using SIA.Common;
using SIA.SystemLayer;

using SIA.UI.Controls;
using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Commands;

using SIA.UI.Components;
using SIA.UI.Components.Helpers;

using SIA.UI.Controls.Automation.Commands;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Name : DlgCalculation
	/// Description : User interface for Calculation operation
	/// Thread Support : True
	/// Persistence Data : True
	/// </summary>
	public class DlgCalculation 
        : DialogPreviewBase
	{
		#region Windows Form member
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.GroupBox grpPreview;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.GroupBox grpParameters;
		private System.Windows.Forms.GroupBox grpOperand;
		public System.Windows.Forms.NumericUpDown nudConstant;
		private System.Windows.Forms.Button btnBrowse;
		public System.Windows.Forms.Label lblOperandConstantValue;
		private System.Windows.Forms.RadioButton radioButtonImage;
		private System.Windows.Forms.RadioButton radioButtonConstant;
		private System.Windows.Forms.GroupBox grpOperators;
		private System.Windows.Forms.RadioButton radioDiv;
		private System.Windows.Forms.RadioButton radioMul;
		private System.Windows.Forms.RadioButton radioSub;
		private System.Windows.Forms.RadioButton radioAdd;
		private System.Windows.Forms.ToolTip _toolTip;
		private SIA.UI.Components.ImagePreview _imagePreview;
		private System.Windows.Forms.Button btnPreview;
		private System.Windows.Forms.TextBox txtFileName;
		private System.ComponentModel.IContainer components;
        public System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnSaveSettings;
		private System.Windows.Forms.Button btnLoadSettings;
		
		#endregion

		#region Fields

        private CalculationCommandSettings _settings = new CalculationCommandSettings();
        private CommonImage _cachedImage = null;
        
        #endregion

		#region Properties

        public CalculationCommandSettings Settings
        {
            get {return _settings;}
        }

		#endregion

		#region constructor and destructor
		
		public DlgCalculation(SIA.UI.Controls.ImageWorkspace owner) : base(owner, true)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            // update ui elements
            this.UpdateData(false);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if (disposing )
			{
				if (components != null)
					components.Dispose();
			}

            this.CleanCachedImage();

			base.Dispose( disposing );
		}

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgCalculation));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.grpPreview = new System.Windows.Forms.GroupBox();
            this._imagePreview = new SIA.UI.Components.ImagePreview();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.grpParameters = new System.Windows.Forms.GroupBox();
            this.grpOperand = new System.Windows.Forms.GroupBox();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.nudConstant = new System.Windows.Forms.NumericUpDown();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lblOperandConstantValue = new System.Windows.Forms.Label();
            this.radioButtonImage = new System.Windows.Forms.RadioButton();
            this.radioButtonConstant = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.grpOperators = new System.Windows.Forms.GroupBox();
            this.radioDiv = new System.Windows.Forms.RadioButton();
            this.radioMul = new System.Windows.Forms.RadioButton();
            this.radioSub = new System.Windows.Forms.RadioButton();
            this.radioAdd = new System.Windows.Forms.RadioButton();
            this._toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnLoadSettings = new System.Windows.Forms.Button();
            this.grpPreview.SuspendLayout();
            this.grpParameters.SuspendLayout();
            this.grpOperand.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudConstant)).BeginInit();
            this.grpOperators.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(288, 36);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(84, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(288, 8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(84, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            // 
            // grpPreview
            // 
            this.grpPreview.Controls.Add(this._imagePreview);
            this.grpPreview.Controls.Add(this.btnReset);
            this.grpPreview.Controls.Add(this.btnPreview);
            this.grpPreview.Location = new System.Drawing.Point(4, 4);
            this.grpPreview.Name = "grpPreview";
            this.grpPreview.Size = new System.Drawing.Size(276, 304);
            this.grpPreview.TabIndex = 4;
            this.grpPreview.TabStop = false;
            this.grpPreview.Text = "Preview";
            // 
            // _imagePreview
            // 
            this._imagePreview.ImageViewer = null;
            this._imagePreview.Location = new System.Drawing.Point(6, 16);
            this._imagePreview.Name = "_imagePreview";
            this._imagePreview.PreviewRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
            this._imagePreview.Size = new System.Drawing.Size(264, 256);
            this._imagePreview.TabIndex = 0;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(8, 276);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(64, 23);
            this.btnReset.TabIndex = 1;
            this.btnReset.Text = "Reset";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point(80, 276);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(64, 23);
            this.btnPreview.TabIndex = 1;
            this.btnPreview.Text = "Preview";
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // grpParameters
            // 
            this.grpParameters.Controls.Add(this.grpOperand);
            this.grpParameters.Controls.Add(this.grpOperators);
            this.grpParameters.Location = new System.Drawing.Point(4, 308);
            this.grpParameters.Name = "grpParameters";
            this.grpParameters.Size = new System.Drawing.Size(276, 188);
            this.grpParameters.TabIndex = 5;
            this.grpParameters.TabStop = false;
            this.grpParameters.Text = "Parameters";
            // 
            // grpOperand
            // 
            this.grpOperand.Controls.Add(this.txtFileName);
            this.grpOperand.Controls.Add(this.nudConstant);
            this.grpOperand.Controls.Add(this.btnBrowse);
            this.grpOperand.Controls.Add(this.lblOperandConstantValue);
            this.grpOperand.Controls.Add(this.radioButtonImage);
            this.grpOperand.Controls.Add(this.radioButtonConstant);
            this.grpOperand.Controls.Add(this.label1);
            this.grpOperand.Location = new System.Drawing.Point(10, 88);
            this.grpOperand.Name = "grpOperand";
            this.grpOperand.Size = new System.Drawing.Size(258, 91);
            this.grpOperand.TabIndex = 1;
            this.grpOperand.TabStop = false;
            this.grpOperand.Text = "Operand";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(12, 64);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.ReadOnly = true;
            this.txtFileName.Size = new System.Drawing.Size(204, 20);
            this.txtFileName.TabIndex = 6;
            // 
            // nudConstant
            // 
            this.nudConstant.DecimalPlaces = 4;
            this.nudConstant.Location = new System.Drawing.Point(140, 14);
            this.nudConstant.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudConstant.Name = "nudConstant";
            this.nudConstant.Size = new System.Drawing.Size(56, 20);
            this.nudConstant.TabIndex = 2;
            this.nudConstant.Tag = "DEFAULT";
            this.nudConstant.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(224, 64);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(24, 20);
            this.btnBrowse.TabIndex = 5;
            this.btnBrowse.Text = "...";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // lblOperandConstantValue
            // 
            this.lblOperandConstantValue.Location = new System.Drawing.Point(104, 16);
            this.lblOperandConstantValue.Name = "lblOperandConstantValue";
            this.lblOperandConstantValue.Size = new System.Drawing.Size(36, 20);
            this.lblOperandConstantValue.TabIndex = 1;
            this.lblOperandConstantValue.Text = "Value:";
            this.lblOperandConstantValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // radioButtonImage
            // 
            this.radioButtonImage.Location = new System.Drawing.Point(17, 40);
            this.radioButtonImage.Name = "radioButtonImage";
            this.radioButtonImage.Size = new System.Drawing.Size(120, 20);
            this.radioButtonImage.TabIndex = 3;
            this.radioButtonImage.Tag = "DEFAULT";
            this.radioButtonImage.Text = "Reference Image:";
            this.radioButtonImage.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // radioButtonConstant
            // 
            this.radioButtonConstant.Checked = true;
            this.radioButtonConstant.Location = new System.Drawing.Point(17, 16);
            this.radioButtonConstant.Name = "radioButtonConstant";
            this.radioButtonConstant.Size = new System.Drawing.Size(84, 20);
            this.radioButtonConstant.TabIndex = 0;
            this.radioButtonConstant.TabStop = true;
            this.radioButtonConstant.Tag = "DEFAULT";
            this.radioButtonConstant.Text = "Constant";
            this.radioButtonConstant.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(200, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "(Intensity)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpOperators
            // 
            this.grpOperators.Controls.Add(this.radioDiv);
            this.grpOperators.Controls.Add(this.radioMul);
            this.grpOperators.Controls.Add(this.radioSub);
            this.grpOperators.Controls.Add(this.radioAdd);
            this.grpOperators.Location = new System.Drawing.Point(10, 12);
            this.grpOperators.Name = "grpOperators";
            this.grpOperators.Size = new System.Drawing.Size(258, 72);
            this.grpOperators.TabIndex = 0;
            this.grpOperators.TabStop = false;
            this.grpOperators.Text = "Operator";
            // 
            // radioDiv
            // 
            this.radioDiv.Location = new System.Drawing.Point(137, 44);
            this.radioDiv.Name = "radioDiv";
            this.radioDiv.Size = new System.Drawing.Size(100, 20);
            this.radioDiv.TabIndex = 3;
            this.radioDiv.Tag = "DEFAULT";
            this.radioDiv.Text = "Division";
            this.radioDiv.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // radioMul
            // 
            this.radioMul.Location = new System.Drawing.Point(137, 20);
            this.radioMul.Name = "radioMul";
            this.radioMul.Size = new System.Drawing.Size(100, 20);
            this.radioMul.TabIndex = 1;
            this.radioMul.Tag = "DEFAULT";
            this.radioMul.Text = "Multiplication";
            this.radioMul.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // radioSub
            // 
            this.radioSub.Location = new System.Drawing.Point(17, 44);
            this.radioSub.Name = "radioSub";
            this.radioSub.Size = new System.Drawing.Size(100, 20);
            this.radioSub.TabIndex = 2;
            this.radioSub.Tag = "DEFAULT";
            this.radioSub.Text = "Subtraction";
            this.radioSub.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // radioAdd
            // 
            this.radioAdd.Checked = true;
            this.radioAdd.Location = new System.Drawing.Point(17, 20);
            this.radioAdd.Name = "radioAdd";
            this.radioAdd.Size = new System.Drawing.Size(100, 20);
            this.radioAdd.TabIndex = 0;
            this.radioAdd.TabStop = true;
            this.radioAdd.Tag = "DEFAULT";
            this.radioAdd.Text = "Addition";
            this.radioAdd.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveSettings.Location = new System.Drawing.Point(288, 92);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(84, 23);
            this.btnSaveSettings.TabIndex = 13;
            this.btnSaveSettings.Text = "Save Settings";
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // btnLoadSettings
            // 
            this.btnLoadSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadSettings.Location = new System.Drawing.Point(288, 64);
            this.btnLoadSettings.Name = "btnLoadSettings";
            this.btnLoadSettings.Size = new System.Drawing.Size(84, 23);
            this.btnLoadSettings.TabIndex = 12;
            this.btnLoadSettings.Text = "Load Settings";
            this.btnLoadSettings.Click += new System.EventHandler(this.btnLoadSettings_Click);
            // 
            // DlgCalculation
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(378, 500);
            this.Controls.Add(this.btnSaveSettings);
            this.Controls.Add(this.btnLoadSettings);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.grpPreview);
            this.Controls.Add(this.grpParameters);
            this.Controls.Add(this.btnCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DlgCalculation";
            this.ShowInTaskbar = false;
            this.Text = "Calculation";
            this.grpPreview.ResumeLayout(false);
            this.grpParameters.ResumeLayout(false);
            this.grpOperand.ResumeLayout(false);
            this.grpOperand.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudConstant)).EndInit();
            this.grpOperators.ResumeLayout(false);
            this.ResumeLayout(false);

		}

       
		#endregion

		#region Event handlers

		private void RadioButton_CheckedChanged(object sender, System.EventArgs e)
		{
			UpdateData(true);
		}

		private void btnReset_Click(object sender, System.EventArgs e)
		{
			this.ResetPreview();
		}

		private void btnPreview_Click(object sender, System.EventArgs e)
		{
			this.ApplyToPreview();
		}

		private void btnBrowse_Click(object sender, System.EventArgs e)
		{
			using (OpenFileDialog dlg = CommonDialogs.OpenImageFileDialog("Select an image", CommonDialogs.ImageFileFilter.AllSupportedImageFormat))
			{
				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					this.txtFileName.Text = dlg.FileName;

					RefreshCachedImage();
				}
			}
		}

		#endregion

		#region override routines

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);	
		
			// set manual preview mode
			this.AutoPreview = false;

			// update data
			UpdateData(true);
		}
        
		protected override object OnGetDefaultValue(Control ctrl)
		{
			if (ctrl == radioAdd)
				return true;
			else if (ctrl == radioSub)
				return false;
			else if (ctrl == radioMul)
				return false;
			else if (ctrl == radioDiv)
				return false;
			else if (ctrl == radioButtonConstant)
				return true;
			else if (ctrl == radioButtonImage)
				return false;
			else if (ctrl == txtFileName)
				return string.Empty;
			else if (ctrl == nudConstant)
				return (Decimal)0.0F;

			return null;
		}

		public override ImagePreview GetPreviewer()
		{
			return _imagePreview;
		}

		public override void ApplyToCommonImage(CommonImage image)
		{
            try
			{
                this.UpdateData(true);

                using (CalculationCommand command = new CalculationCommand(null))
                    command.AutomationRun(image, _settings);
			}
			catch(System.OutOfMemoryException exp)
			{
				System.Diagnostics.Trace.WriteLine(exp);
				MessageBoxEx.Error("Not enough memory. Please close unused application for reducing memory usage");
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
                MessageBoxEx.Error("Failed to process image: " + exp.Message);
			}
		}

		protected override void LockUserInputObjects()
		{
			grpOperand.Enabled = false;
			grpOperators.Enabled = false;
			btnReset.Enabled = false;
			btnPreview.Enabled = false;
		}

		protected override void UnlockUserInputObjects()
		{
			btnPreview.Enabled = true;
			btnReset.Enabled = true;
			grpOperators.Enabled = true;
			grpOperand.Enabled = true;
		}

		#endregion

		#region internal helpers
		
		private String OperatorToString(OperatorType type)
		{
			string strOperator = string.Empty;

			switch (type)
			{
				case OperatorType.Addition:
					strOperator = "ADD";
					break;
				case OperatorType.Subtraction:
					strOperator = "SUB";
					break;
				case OperatorType.Multiplication:
					strOperator = "MUL";
					break;
				case OperatorType.Division:
					strOperator = "DIV";
					break;
				default:
					break;
			}

			return strOperator;
		}

		public void UpdateData(bool bSaveAndValidate)
		{
			if (bSaveAndValidate)
			{
                this.Invalidate(true);

                if (radioButtonConstant.Checked)
                    _settings.Type = eCalculationType.Monadic;
                else
                    _settings.Type = eCalculationType.Dyadic;

                string type = "ADD";
                if (radioAdd.Checked)
                    type = "ADD";
                else if (radioSub.Checked)
                    type = "SUB";
                else if (radioMul.Checked)
                    type = "MUL";
                else if (radioDiv.Checked)
                    type = "DIV";

				btnBrowse.Enabled = radioButtonImage.Checked;
				nudConstant.Enabled = radioButtonConstant.Checked;

                _settings.MonadicType = type;
                _settings.DyadicType = type;
                _settings.FileName = txtFileName.Text;
                _settings.Value = Convert.ToSingle(nudConstant.Value);
			}
			else
			{
                eCalculationType calcType = _settings.Type;

                nudConstant.Value = (decimal)_settings.Value;
                txtFileName.Text = _settings.FileName;
                
                this.SuspendLayout();
                string type = _settings.MonadicType;
                radioAdd.Checked = type == "ADD";// this._operatorType == OperatorType.Addition;
                radioSub.Checked = type == "SUB";//this._operatorType == OperatorType.Subtraction;
                radioMul.Checked = type == "MUL";//this._operatorType == OperatorType.Multiplication;
                radioDiv.Checked = type == "DIV";//this._operatorType == OperatorType.Division;
                this.ResumeLayout(false);

                radioButtonConstant.Checked = calcType == eCalculationType.Monadic;
                radioButtonImage.Checked = calcType == eCalculationType.Dyadic;                

                //nudConstant.Update();
			}
		}

		public void CleanCachedImage()
		{
			if (_cachedImage != null)
				_cachedImage.Dispose();
			_cachedImage = null;
		}

		public void RefreshCachedImage()
		{
			if (_cachedImage != null)
				_cachedImage.Dispose();

            string fileName = txtFileName.Text;

			try
			{
                _cachedImage = SIA.SystemLayer.CommonImage.FromFile(fileName);
			}
			catch(System.Exception exp)
			{
				MessageBoxEx.Error("Failed to load file \"" + fileName + "\".");
				System.Diagnostics.Trace.WriteLine(exp);
				txtFileName.Text = string.Empty;
			}
			finally
			{
			}
		}
		
		#endregion

        private void btnLoadSettings_Click(object sender, EventArgs e)
        {
            try
            {
                CalculationCommandSettings settings = base.LoadCommandSettings(_settings.GetType()) as CalculationCommandSettings;
                if (settings != null)
                {
                    _settings = settings;
                    this.UpdateData(false);
                }
            }
            catch (System.Exception exp)
            {
                MessageBoxEx.Error("Failed to load settings: " + exp.Message);
            }            
        }

        private void btnSaveSettings_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.UpdateData(true);
                base.SaveCommandSettings(this._settings);
            }
            catch (System.Exception exp)
            {
                MessageBoxEx.Error("Failed to save settings: " + exp.Message);
            }            
        }

	}

    public enum OperatorType : int
    {
        Addition,
        Subtraction,
        Multiplication,
        Division
    }

    public enum OperandType : int
    {
        Constant,
        ReferenceImage
    }
}
