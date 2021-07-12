using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Threading;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using SIA.Common;
using SIA.Common.Analysis;
using SIA.Common.Mathematics;
using SIA.SystemLayer;

using SIA.UI.Controls;
using SIA.UI.Controls.Common;
//CONG using SIA.UI.Controls.Components;
using SIA.UI.Controls.UserControls;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Helpers;
using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Helpers.VisualAnalysis;
using SIA.UI.Controls.Helpers.VisualAnalysis.DataProfilers;

namespace SIA.UI.Controls.Dialogs
{
    /// <summary>
    /// Summary description for DlgLineProfile2.
    /// </summary>
    public class DlgLineProfile2 : FloatingFormBase
    {
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnTrendline;
        private System.Windows.Forms.Label lblOrder;
        private System.Windows.Forms.NumericUpDown nudOrder;
        private System.Windows.Forms.Panel pBox;
        private System.Windows.Forms.RadioButton raStdDev;
        private System.Windows.Forms.RadioButton raMaximum;
        private System.Windows.Forms.RadioButton raMinimum;
        private System.Windows.Forms.RadioButton raMean;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnSetting;
        private System.Windows.Forms.TextBox txtCurrent;
        private System.Windows.Forms.TextBox txtLimit;
        public System.Windows.Forms.ComboBox cboType;
        private System.Windows.Forms.CheckBox chkTrendLine;
        private SIA.UI.Controls.UserControls.DataPlot dataPlot;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;


        public event EventHandler PlotTypeChanged = null;
        public event EventHandler SelectedValueChanged = null;

        private DataProfileHelper _dataProfileHelper = null;
        private PointF _begin = PointF.Empty, _end = PointF.Empty;
        private RectangleF _rectangle = RectangleF.Empty;
        private int _ignoreUpdateCounter = 0;
        private TrendLineFormat _trendlineFormat = null;

        public PlotType PlotType
        {
            get { return (PlotType)this.cboType.SelectedIndex; }
            set
            {
                this.cboType.SelectedIndex = (int)value;
                this.OnPlotTypeChanged();
            }
        }

        protected virtual void OnPlotTypeChanged()
        {
            this.UpdateControls();
        }

        public BoxProfileOptions BoxProfileOptions
        {
            get
            {
                if (raMaximum.Checked)
                    return BoxProfileOptions.Maximum;
                else if (raMinimum.Checked)
                    return BoxProfileOptions.Minimum;
                else if (raMean.Checked)
                    return BoxProfileOptions.Mean;
                else if (raStdDev.Checked)
                    return BoxProfileOptions.StdDev;
                else
                    return BoxProfileOptions.Maximum;
            }
        }

        public DataPlot DataPlot
        {
            get { return dataPlot; }
        }

        public bool TrendLine
        {
            get { return chkTrendLine.Checked; }
        }

        public TrendLineFormat TrendLineFormat
        {
            get { return _trendlineFormat; }
        }


        public DlgLineProfile2(DataProfileHelper dataProfileHelper)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            this._dataProfileHelper = dataProfileHelper;

            // initialize trend line format
            ImageWorkspace workspace = this._dataProfileHelper.Workspace;
            if (workspace["TRENDLINE_FORMAT"] != null)
            {
                _trendlineFormat = workspace["TRENDLINE_FORMAT"] as TrendLineFormat;
            }
            else
            {
                _trendlineFormat = new TrendLineFormat();
                workspace["TRENDLINE_FORMAT"] = _trendlineFormat;
            }

            this.Text = "Data Profile";

            // update controls
            this.UpdateControls();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgLineProfile2));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnTrendline = new System.Windows.Forms.Button();
            this.lblOrder = new System.Windows.Forms.Label();
            this.nudOrder = new System.Windows.Forms.NumericUpDown();
            this.chkTrendLine = new System.Windows.Forms.CheckBox();
            this.pBox = new System.Windows.Forms.Panel();
            this.raStdDev = new System.Windows.Forms.RadioButton();
            this.raMaximum = new System.Windows.Forms.RadioButton();
            this.raMinimum = new System.Windows.Forms.RadioButton();
            this.raMean = new System.Windows.Forms.RadioButton();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnSetting = new System.Windows.Forms.Button();
            this.txtCurrent = new System.Windows.Forms.TextBox();
            this.txtLimit = new System.Windows.Forms.TextBox();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.dataPlot = new SIA.UI.Controls.UserControls.DataPlot();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOrder)).BeginInit();
            this.pBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnTrendline);
            this.groupBox1.Controls.Add(this.lblOrder);
            this.groupBox1.Controls.Add(this.nudOrder);
            this.groupBox1.Controls.Add(this.chkTrendLine);
            this.groupBox1.Controls.Add(this.pBox);
            this.groupBox1.Controls.Add(this.btnExport);
            this.groupBox1.Controls.Add(this.btnSetting);
            this.groupBox1.Controls.Add(this.txtCurrent);
            this.groupBox1.Controls.Add(this.txtLimit);
            this.groupBox1.Controls.Add(this.cboType);
            this.groupBox1.Location = new System.Drawing.Point(4, 220);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(540, 104);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // btnTrendline
            // 
            this.btnTrendline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTrendline.Location = new System.Drawing.Point(345, 63);
            this.btnTrendline.Name = "btnTrendline";
            this.btnTrendline.Size = new System.Drawing.Size(60, 20);
            this.btnTrendline.TabIndex = 8;
            this.btnTrendline.Text = "Trendline";
            this.btnTrendline.Visible = false;
            this.btnTrendline.Click += new System.EventHandler(this.btnTrendline_Click);
            // 
            // lblOrder
            // 
            this.lblOrder.Location = new System.Drawing.Point(136, 72);
            this.lblOrder.Name = "lblOrder";
            this.lblOrder.Size = new System.Drawing.Size(48, 20);
            this.lblOrder.TabIndex = 4;
            this.lblOrder.Text = "Order:";
            this.lblOrder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblOrder.Visible = false;
            // 
            // nudOrder
            // 
            this.nudOrder.Location = new System.Drawing.Point(188, 72);
            this.nudOrder.Maximum = new System.Decimal(new int[] {
																	 10,
																	 0,
																	 0,
																	 0});
            this.nudOrder.Minimum = new System.Decimal(new int[] {
																	 2,
																	 0,
																	 0,
																	 0});
            this.nudOrder.Name = "nudOrder";
            this.nudOrder.Size = new System.Drawing.Size(40, 20);
            this.nudOrder.TabIndex = 5;
            this.nudOrder.Value = new System.Decimal(new int[] {
																   6,
																   0,
																   0,
																   0});
            this.nudOrder.Visible = false;
            // 
            // chkTrendLine
            // 
            this.chkTrendLine.Location = new System.Drawing.Point(8, 72);
            this.chkTrendLine.Name = "chkTrendLine";
            this.chkTrendLine.Size = new System.Drawing.Size(96, 20);
            this.chkTrendLine.TabIndex = 3;
            this.chkTrendLine.Tag = "DEFAULT";
            this.chkTrendLine.Text = "Trendline";
            this.chkTrendLine.Visible = false;
            this.chkTrendLine.CheckedChanged += new System.EventHandler(this.chkTrendLine_CheckedChanged);
            // 
            // pBox
            // 
            this.pBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pBox.Controls.Add(this.raStdDev);
            this.pBox.Controls.Add(this.raMaximum);
            this.pBox.Controls.Add(this.raMinimum);
            this.pBox.Controls.Add(this.raMean);
            this.pBox.Location = new System.Drawing.Point(405, 12);
            this.pBox.Name = "pBox";
            this.pBox.Size = new System.Drawing.Size(132, 76);
            this.pBox.TabIndex = 9;
            // 
            // raStdDev
            // 
            this.raStdDev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.raStdDev.Location = new System.Drawing.Point(8, 52);
            this.raStdDev.Name = "raStdDev";
            this.raStdDev.Size = new System.Drawing.Size(124, 16);
            this.raStdDev.TabIndex = 3;
            this.raStdDev.Tag = "DEFAULT";
            this.raStdDev.Text = "Std. Dev.";
            this.raStdDev.CheckedChanged += new System.EventHandler(this.BoxProfileOptions_CheckedChanged);
            // 
            // raMaximum
            // 
            this.raMaximum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.raMaximum.Location = new System.Drawing.Point(8, 36);
            this.raMaximum.Name = "raMaximum";
            this.raMaximum.Size = new System.Drawing.Size(124, 16);
            this.raMaximum.TabIndex = 2;
            this.raMaximum.Tag = "DEFAULT";
            this.raMaximum.Text = "Maximum";
            this.raMaximum.CheckedChanged += new System.EventHandler(this.BoxProfileOptions_CheckedChanged);
            // 
            // raMinimum
            // 
            this.raMinimum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.raMinimum.Location = new System.Drawing.Point(8, 20);
            this.raMinimum.Name = "raMinimum";
            this.raMinimum.Size = new System.Drawing.Size(124, 16);
            this.raMinimum.TabIndex = 1;
            this.raMinimum.Tag = "DEFAULT";
            this.raMinimum.Text = "Minimum";
            this.raMinimum.CheckedChanged += new System.EventHandler(this.BoxProfileOptions_CheckedChanged);
            // 
            // raMean
            // 
            this.raMean.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.raMean.Checked = true;
            this.raMean.Location = new System.Drawing.Point(8, 4);
            this.raMean.Name = "raMean";
            this.raMean.Size = new System.Drawing.Size(124, 16);
            this.raMean.TabIndex = 0;
            this.raMean.TabStop = true;
            this.raMean.Tag = "DEFAULT";
            this.raMean.Text = "Mean";
            this.raMean.CheckedChanged += new System.EventHandler(this.BoxProfileOptions_CheckedChanged);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(345, 39);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(60, 20);
            this.btnExport.TabIndex = 7;
            this.btnExport.Text = "&Export";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnSetting
            // 
            this.btnSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetting.Location = new System.Drawing.Point(345, 15);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(60, 20);
            this.btnSetting.TabIndex = 6;
            this.btnSetting.Text = "&Settings";
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // txtCurrent
            // 
            this.txtCurrent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCurrent.Location = new System.Drawing.Point(8, 44);
            this.txtCurrent.Name = "txtCurrent";
            this.txtCurrent.ReadOnly = true;
            this.txtCurrent.Size = new System.Drawing.Size(333, 20);
            this.txtCurrent.TabIndex = 2;
            this.txtCurrent.Text = "";
            // 
            // txtLimit
            // 
            this.txtLimit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLimit.Location = new System.Drawing.Point(128, 16);
            this.txtLimit.Name = "txtLimit";
            this.txtLimit.ReadOnly = true;
            this.txtLimit.Size = new System.Drawing.Size(213, 20);
            this.txtLimit.TabIndex = 1;
            this.txtLimit.Text = "";
            // 
            // cboType
            // 
            this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboType.Items.AddRange(new object[] {
														 "Line Profile",
														 "Horizontal Line",
														 "Vertical Line",
														 "Horizontal Box",
														 "Vertical Box",
														 "3D Area"});
            this.cboType.Location = new System.Drawing.Point(8, 16);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(120, 21);
            this.cboType.TabIndex = 0;
            this.cboType.Tag = "DEFAULT";
            this.cboType.SelectedValueChanged += new System.EventHandler(this.cboType_SelectedValueChanged);
            // 
            // dataPlot
            // 
            this.dataPlot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.dataPlot.Location = new System.Drawing.Point(4, 4);
            this.dataPlot.Name = "dataPlot";
            this.dataPlot.PlotType = SIA.UI.Controls.UserControls.PlotType.Line;
            this.dataPlot.Size = new System.Drawing.Size(540, 212);
            this.dataPlot.TabIndex = 0;
            this.dataPlot.TrendLineFormat = null;
            this.dataPlot.SelectedValueChanged += new System.EventHandler(this.DataPlot_SelectedValueChanged);
            // 
            // DlgLineProfile2
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(548, 326);
            this.Controls.Add(this.dataPlot);
            this.Controls.Add(this.groupBox1);
            this.FadeOutEnabled = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DlgLineProfile2";
            this.Text = "Line Profile";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudOrder)).EndInit();
            this.pBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        public void BeginUpdate()
        {
            this._ignoreUpdateCounter++;
        }

        public void EndUpdate()
        {
            this._ignoreUpdateCounter--;
        }

        public void UpdateLinePlot()
        {
            this.dataPlot.UpdateLinePlot();
        }


        public void UpdateLinePlot(LineProfileSettings settings, PointF begin, PointF end, float[] data, Point[] categories)
        {
            this._begin = begin;
            this._end = end;

            this.txtLimit.Text = String.Format("({0},{1}) - ({2},{3})", begin.X, begin.Y, end.X, end.Y);

            string title = "";
            switch (this.PlotType)
            {
                case PlotType.Line:
                    title = "Line Profile";
                    break;
                case PlotType.HorizontalLine:
                    title = "Horizontal Line";
                    break;
                case PlotType.VerticalLine:
                    title = "Vertical Line";
                    break;
            }

            if (settings != null)
            {
                this.dataPlot.AutoScaleYAxis = settings.AutoScale;
                this.dataPlot.YAxisMinValue = (int)settings.Mininum;
                this.dataPlot.YAxisMaxValue = (int)settings.Maximum;
            }

            this.dataPlot.UpdateLinePlot(title, data, categories);
        }

        public void UpdateBoxPlot(BoxProfileSettings settings, RectangleF rect, float[] data, float[] categories)
        {
            this._rectangle = rect;
            this.txtLimit.Text = String.Format("Rectangle({0},{1},{2},{3})", rect.Left, rect.Top, rect.Width, rect.Height);

            string title = "";
            float xbegin = 0;
            switch (this.PlotType)
            {

                case PlotType.HorizontalBox:
                    title = "Horizontal Box";
                    xbegin = rect.Left;
                    break;
                case PlotType.VerticalBox:
                    title = "Vertical Box";
                    xbegin = rect.Top;
                    break;
            }

            // filtering data by options
            if (settings != null)
            {
                this.dataPlot.AutoScaleYAxis = settings.AutoScale;
                this.dataPlot.YAxisMinValue = (int)settings.Mininum;
                this.dataPlot.YAxisMaxValue = (int)settings.Maximum;
            }

            for (int i = categories.Length - 1; i >= 0; i--)
                categories[i] += xbegin;

            this.dataPlot.UpdateLinePlot(title, data, categories);
        }

        public void UpdateAreaPlot(RenderMode mode, float resX, float resY, int left, int top, int right, int bottom, float[] data)
        {
            Rectangle rect = new Rectangle(left, top, right - left + 1, bottom - top + 1);
            this._rectangle = rect;
            this.txtLimit.Text = String.Format("Rectangle({0},{1},{2},{3})", rect.Left, rect.Top, rect.Width, rect.Height);

            string title = "Area Plot";

            this.dataPlot.UpdateAreaPlot(title, mode, resX, resY, left, top, right, bottom, data);
        }


        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;

            this.Owner.Invalidate(true);

            base.OnClosing(e);
        }


        private void cboType_SelectedValueChanged(object sender, System.EventArgs e)
        {
            if (_ignoreUpdateCounter == 0)
                this.PlotTypeChanged(this, EventArgs.Empty);


            // update plot controls
            this.UpdateControls();
        }

        private void DataPlot_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                // update current selected information
                string str = "";
                PointF pt = PointF.Empty;

                switch (this.PlotType)
                {
                    case PlotType.Line:
                    case PlotType.HorizontalLine:
                    case PlotType.VerticalLine:
                        str = string.Format("{0}@{1}", dataPlot.SelectedOrdinaryValue, dataPlot.SelectedAbscissaData);
                        break;
                    case PlotType.HorizontalBox:
                        float xVal = Convert.ToSingle(dataPlot.SelectedAbscissaData.ToString());
                        str = string.Format("{0}@X={1}", dataPlot.SelectedOrdinaryValue, xVal);
                        break;
                    case PlotType.VerticalBox:
                        float yVal = Convert.ToSingle(dataPlot.SelectedAbscissaData.ToString());
                        str = string.Format("{0}@Y={1}", dataPlot.SelectedOrdinaryValue, yVal);
                        break;
                }

                this.txtCurrent.Text = str;

                if (this.SelectedValueChanged != null)
                    this.SelectedValueChanged(this, EventArgs.Empty);
            }
            catch (System.Exception exp)
            {
                Trace.WriteLine(exp);
            }

        }

        private void BoxProfileOptions_CheckedChanged(object sender, System.EventArgs e)
        {
            if (_dataProfileHelper != null)
                _dataProfileHelper.Update();
        }

        private void btnSetting_Click(object sender, System.EventArgs e)
        {
            if (_dataProfileHelper != null)
                _dataProfileHelper.DisplaySettingsWindow();
        }

        private void btnExport_Click(object sender, System.EventArgs e)
        {
            if (_dataProfileHelper != null)
                _dataProfileHelper.Export();
        }

        private void chkTrendLine_CheckedChanged(object sender, System.EventArgs e)
        {
            // toggle trend line 
            btnTrendline.Enabled = chkTrendLine.Checked;
            // toggle data plot trend line feature
            this.dataPlot.TrendLineFormat = chkTrendLine.Checked ? this._trendlineFormat : null;
            // update data profiler
            this._dataProfileHelper.Update();
        }

        private void btnTrendline_Click(object sender, System.EventArgs e)
        {
            using (DlgTrendline2 dlg = new DlgTrendline2(_trendlineFormat))
            {
                if (DialogResult.OK == dlg.ShowDialog(this))
                {
                    chkTrendLine.Checked = true;

                    // update trend line format
                    this._trendlineFormat = dlg.TrendLineFormat;
                    this.dataPlot.TrendLineFormat = dlg.TrendLineFormat;
                    // update data profiler
                    this._dataProfileHelper.Update();
                }
            }
        }

        private void UpdateControls()
        {
            PlotType plotType = this.PlotType;
            this.dataPlot.PlotType = plotType;

            raMaximum.Enabled = plotType == PlotType.HorizontalBox || plotType == PlotType.VerticalBox;
            raMinimum.Enabled = plotType == PlotType.HorizontalBox || plotType == PlotType.VerticalBox;
            raMean.Enabled = plotType == PlotType.HorizontalBox || plotType == PlotType.VerticalBox;
            raStdDev.Enabled = plotType == PlotType.HorizontalBox || plotType == PlotType.VerticalBox;
            chkTrendLine.Enabled = plotType == PlotType.Line || plotType == PlotType.HorizontalLine || plotType == PlotType.VerticalLine ||
                plotType == PlotType.HorizontalBox || plotType == PlotType.VerticalBox;
        }
    }
}
