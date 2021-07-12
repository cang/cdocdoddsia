using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Diagnostics;

using SIA.Common;
using SIA.Common.Analysis;
using SIA.Common.KlarfExport;
using SIA.Common.KlarfExport.BinningLibrary;
using SIA.Common.Mathematics;
using SIA.Common.Utility;

using ControlDemo;

using SIA.UI.Controls.Common;
using SIA.UI.Controls.UserControls;
using SIA.UI.Controls.Utilities;

namespace SIA.UI.Controls.Dialogs
{
	public delegate void DotChange(Point pt);
	public delegate void LineChange(Point pt1,Point pt2);

	/// <summary>
	/// Name : DlgLineProfile
	/// Description : User interface for Line Profile
	/// Thread Support : None
	/// Persistence Data : None
	/// Context-Sensitive Help : True
	/// </summary>
	public class DlgLineProfile : SIA.UI.Controls.Common.ContextSensitiveHelpForm
	{
		#region Windows Form member attributes
		private SIA.UI.Controls.UserControls.LineChart _lineChart;
		private System.Windows.Forms.Panel pContainer;
		private System.Windows.Forms.Panel pFooter;
		private System.Windows.Forms.Panel pClient;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox txtLimit;
		private System.Windows.Forms.TextBox txtCurrent;
		private System.Windows.Forms.Button btnSetting;
		private System.Windows.Forms.Button btnExport;
		private System.Windows.Forms.RadioButton raMean;
		private System.Windows.Forms.RadioButton raMinimum;
		private System.Windows.Forms.RadioButton raMaximum;
		private System.Windows.Forms.RadioButton raStdDev;
		public System.Windows.Forms.ComboBox cboType;
		private System.Windows.Forms.Panel pBox;

		private ControlDemo.OpenglScene _3DPlot;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.NumericUpDown nudOrder;
		private System.Windows.Forms.Label lblOrder;
		private System.Windows.Forms.Button btnTrendline;
		
		private System.Windows.Forms.Button btnContour;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion
		
		#region member attributes

		public SIA.SystemLayer.CommonImage Image;
		private SIA.SystemLayer.CommonImage data;

		private DlgTrendline mTrendlineDlg=null;
		private eTrendlineType trendlinetype;
		private float	zoomscale;
		public	Point	p1=new Point(0,0),p2=new Point(0,0);
		private PointF	pf1=new PointF(0,0),pf2=new PointF(0,0);
		private double	minlimity,maxlimity;
		private bool	bautoscale=true;
		public	PointF	CenterImageViewer = PointF.Empty;
		private float	_rotateAngle=0;
		
		public event EventHandler RotateAngleChanged = null;
		public event DotChange		dotChange;
		public event LineChange	lineChange;


		#endregion

		#region public properties

		public float RotateAngle
		{
			get {return _rotateAngle;}
			set 
			{
				_rotateAngle=value;
				OnRotationAngleChanged();
			}
		}

		protected virtual void OnRotationAngleChanged()
		{
			if (this.RotateAngleChanged != null)
				this.RotateAngleChanged(this, new System.EventArgs());
			UpdateControls();
		}

		public  eChartProfileType ShowType
		{
			get
			{
				if ( (eLineProfileType)cboType.SelectedItem==eLineProfileType.Line ||
					(eLineProfileType)cboType.SelectedItem==eLineProfileType.VerticalLine ||
					(eLineProfileType)cboType.SelectedItem==eLineProfileType.HorizontalLine)
					return eChartProfileType.LineProfile;

				if ( (eLineProfileType)cboType.SelectedItem==eLineProfileType.HorizontalBox ||
					(eLineProfileType)cboType.SelectedItem==eLineProfileType.VerticalBox)
					return eChartProfileType.BoxProfile;

				if( (eLineProfileType)cboType.SelectedItem==eLineProfileType.AreaPlot)
					return eChartProfileType.AreaPlotProfile;

				return eChartProfileType.None;
			}
			set
			{
				if(value==eChartProfileType.LineProfile)
				{
					if((eLineProfileType)cboType.SelectedItem!=eLineProfileType.Line &&
						(eLineProfileType)cboType.SelectedItem!=eLineProfileType.VerticalLine &&
						(eLineProfileType)cboType.SelectedItem!=eLineProfileType.HorizontalLine)
						cboType.SelectedItem=eLineProfileType.Line;
				}
				else if(value==eChartProfileType.BoxProfile)
				{
					if((eLineProfileType)cboType.SelectedItem!=eLineProfileType.VerticalBox &&
						(eLineProfileType)cboType.SelectedItem!=eLineProfileType.HorizontalBox)
						cboType.SelectedItem=eLineProfileType.HorizontalBox;
				}
				else if(value==eChartProfileType.AreaPlotProfile)
				{
					if( (eLineProfileType)cboType.SelectedItem!=eLineProfileType.AreaPlot)
						cboType.SelectedItem=eLineProfileType.AreaPlot;
				}
			}
		}

		public int Order
		{
			get 
			{
				return (int)nudOrder.Value;
			}
			set
			{
				nudOrder.Value = value;
			}
		}

		#endregion

		#region constructor and destructor
		
		public DlgLineProfile()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this._lineChart = new SIA.UI.Controls.UserControls.LineChart();
			this._3DPlot = new ControlDemo.OpenglScene();

			this.pClient.Controls.Add(this._lineChart);
			this.pClient.Controls.Add(this._3DPlot);

			// 
			// _lineChart
			// 
			this._lineChart.CaptionX = "Pixel Location";
			this._lineChart.CaptionY = "Pixel Value";
			this._lineChart.ClientBackColor = System.Drawing.Color.LightGray;
			this._lineChart.Coeffs = null;
			this._lineChart.Dock = System.Windows.Forms.DockStyle.Fill;
			this._lineChart.Location = new System.Drawing.Point(0, 0);
			this._lineChart.MaxX = 1000;
			this._lineChart.MaxY = 2020;
			this._lineChart.MinX = 0;
			this._lineChart.MinY = 20;
			this._lineChart.Name = "_lineChart";
			this._lineChart.Order = 6;
			this._lineChart.SessionX = 10;
			this._lineChart.SessionY = 5;
			this._lineChart.TrendLineType = eTrendlineType.Linear;
			this._lineChart.Size = new System.Drawing.Size(516, 254);
			this._lineChart.SubSessionX = 5;
			this._lineChart.SubSessionY = 5;
			this._lineChart.TabIndex = 0;
			this._lineChart.TRENDLINE_DLG = null;
			this._lineChart.Values = null;
			this._lineChart.reset += new System.EventHandler(this.lineChart_Reset);
			this._lineChart.mousechange += new XValueChange(this.lineChart_MouseChange);
			// 
			// _3DPlot
			// 
			this._3DPlot.Dock = System.Windows.Forms.DockStyle.Fill;
			this._3DPlot.Location = new System.Drawing.Point(0, 0);
			this._3DPlot.Name = "_3DPlot";
			this._3DPlot.Size = new System.Drawing.Size(516, 254);
			this._3DPlot.TabIndex = 0;

            this.Text = "Data Profile";
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgLineProfile));
			this.pContainer = new System.Windows.Forms.Panel();
			this.pClient = new System.Windows.Forms.Panel();
			this.pFooter = new System.Windows.Forms.Panel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnContour = new System.Windows.Forms.Button();
			this.btnTrendline = new System.Windows.Forms.Button();
			this.lblOrder = new System.Windows.Forms.Label();
			this.nudOrder = new System.Windows.Forms.NumericUpDown();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
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
			this.pContainer.SuspendLayout();
			this.pFooter.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudOrder)).BeginInit();
			this.pBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// pContainer
			// 
			this.pContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pContainer.Controls.Add(this.pClient);
			this.pContainer.Controls.Add(this.pFooter);
			this.pContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pContainer.Location = new System.Drawing.Point(0, 0);
			this.pContainer.Name = "pContainer";
			this.pContainer.Size = new System.Drawing.Size(460, 274);
			this.pContainer.TabIndex = 0;
			// 
			// pClient
			// 
			this.pClient.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pClient.Location = new System.Drawing.Point(0, 0);
			this.pClient.Name = "pClient";
			this.pClient.Size = new System.Drawing.Size(456, 174);
			this.pClient.TabIndex = 2;
			// 
			// pFooter
			// 
			this.pFooter.Controls.Add(this.groupBox1);
			this.pFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pFooter.Location = new System.Drawing.Point(0, 174);
			this.pFooter.Name = "pFooter";
			this.pFooter.Size = new System.Drawing.Size(456, 96);
			this.pFooter.TabIndex = 1;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.btnContour);
			this.groupBox1.Controls.Add(this.btnTrendline);
			this.groupBox1.Controls.Add(this.lblOrder);
			this.groupBox1.Controls.Add(this.nudOrder);
			this.groupBox1.Controls.Add(this.checkBox1);
			this.groupBox1.Controls.Add(this.pBox);
			this.groupBox1.Controls.Add(this.btnExport);
			this.groupBox1.Controls.Add(this.btnSetting);
			this.groupBox1.Controls.Add(this.txtCurrent);
			this.groupBox1.Controls.Add(this.txtLimit);
			this.groupBox1.Controls.Add(this.cboType);
			this.groupBox1.Location = new System.Drawing.Point(5, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(447, 96);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// btnContour
			// 
			this.btnContour.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnContour.Location = new System.Drawing.Point(252, 76);
			this.btnContour.Name = "btnContour";
			this.btnContour.Size = new System.Drawing.Size(60, 20);
			this.btnContour.TabIndex = 15;
			this.btnContour.Text = "&Contour";
			this.btnContour.Visible = false;
			this.btnContour.Click += new System.EventHandler(this.btnContour_Click);
			// 
			// btnTrendline
			// 
			this.btnTrendline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTrendline.Location = new System.Drawing.Point(252, 55);
			this.btnTrendline.Name = "btnTrendline";
			this.btnTrendline.Size = new System.Drawing.Size(60, 20);
			this.btnTrendline.TabIndex = 14;
			this.btnTrendline.Text = "Trendline";
			this.btnTrendline.Visible = false;
			this.btnTrendline.Click += new System.EventHandler(this.btnTrendline_Click);
			// 
			// lblOrder
			// 
			this.lblOrder.Location = new System.Drawing.Point(136, 72);
			this.lblOrder.Name = "lblOrder";
			this.lblOrder.Size = new System.Drawing.Size(48, 16);
			this.lblOrder.TabIndex = 12;
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
			this.nudOrder.TabIndex = 11;
			this.nudOrder.Value = new System.Decimal(new int[] {
																   6,
																   0,
																   0,
																   0});
			this.nudOrder.Visible = false;
			this.nudOrder.ValueChanged += new System.EventHandler(this.nudOrder_ValueChanged);
			// 
			// checkBox1
			// 
			this.checkBox1.Location = new System.Drawing.Point(8, 72);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(96, 16);
			this.checkBox1.TabIndex = 10;
			this.checkBox1.Tag = "DEFAULT";
			this.checkBox1.Text = "Trendline";
			this.checkBox1.Visible = false;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.chkTrendLine_CheckedChanged);
			// 
			// pBox
			// 
			this.pBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pBox.Controls.Add(this.raStdDev);
			this.pBox.Controls.Add(this.raMaximum);
			this.pBox.Controls.Add(this.raMinimum);
			this.pBox.Controls.Add(this.raMean);
			this.pBox.Location = new System.Drawing.Point(312, 12);
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
			this.raStdDev.TabIndex = 8;
			this.raStdDev.Tag = "DEFAULT";
			this.raStdDev.Text = "Std. Dev.";
			this.raStdDev.CheckedChanged += new System.EventHandler(this.raStdDev_CheckedChanged);
			// 
			// raMaximum
			// 
			this.raMaximum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.raMaximum.Location = new System.Drawing.Point(8, 36);
			this.raMaximum.Name = "raMaximum";
			this.raMaximum.Size = new System.Drawing.Size(124, 16);
			this.raMaximum.TabIndex = 7;
			this.raMaximum.Tag = "DEFAULT";
			this.raMaximum.Text = "Maximum";
			this.raMaximum.CheckedChanged += new System.EventHandler(this.raMaximum_CheckedChanged);
			// 
			// raMinimum
			// 
			this.raMinimum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.raMinimum.Location = new System.Drawing.Point(8, 20);
			this.raMinimum.Name = "raMinimum";
			this.raMinimum.Size = new System.Drawing.Size(124, 16);
			this.raMinimum.TabIndex = 6;
			this.raMinimum.Tag = "DEFAULT";
			this.raMinimum.Text = "Minimum";
			this.raMinimum.CheckedChanged += new System.EventHandler(this.raMinimum_CheckedChanged);
			// 
			// raMean
			// 
			this.raMean.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.raMean.Checked = true;
			this.raMean.Location = new System.Drawing.Point(8, 4);
			this.raMean.Name = "raMean";
			this.raMean.Size = new System.Drawing.Size(124, 16);
			this.raMean.TabIndex = 5;
			this.raMean.TabStop = true;
			this.raMean.Tag = "DEFAULT";
			this.raMean.Text = "Mean";
			this.raMean.CheckedChanged += new System.EventHandler(this.raMean_CheckedChanged);
			// 
			// btnExport
			// 
			this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExport.Location = new System.Drawing.Point(252, 35);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(60, 20);
			this.btnExport.TabIndex = 4;
			this.btnExport.Text = "&Export";
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			// 
			// btnSetting
			// 
			this.btnSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSetting.Location = new System.Drawing.Point(252, 15);
			this.btnSetting.Name = "btnSetting";
			this.btnSetting.Size = new System.Drawing.Size(60, 20);
			this.btnSetting.TabIndex = 3;
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
			this.txtCurrent.Size = new System.Drawing.Size(240, 20);
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
			this.txtLimit.Size = new System.Drawing.Size(120, 20);
			this.txtLimit.TabIndex = 1;
			this.txtLimit.Text = "";
			// 
			// cboType
			// 
			this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboType.Location = new System.Drawing.Point(8, 16);
			this.cboType.Name = "cboType";
			this.cboType.Size = new System.Drawing.Size(120, 21);
			this.cboType.TabIndex = 0;
			this.cboType.Tag = "DEFAULT";
			this.cboType.SelectedIndexChanged += new System.EventHandler(this.cboType_SelectedIndexChanged);
			// 
			// DlgLineProfile
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(460, 274);
			this.Controls.Add(this.pContainer);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(400, 300);
			this.Name = "DlgLineProfile";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Line Profile";
			this.TopMost = true;
			this.Resize += new System.EventHandler(this.LineProfile_Resize);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.LineProfile_Closing);
			this.Load += new System.EventHandler(this.LineProfile_Load);
			this.Move += new System.EventHandler(this.LineProfile_Move);
			this.Closed += new System.EventHandler(this.LineProfile_Closed);
			this.Activated += new System.EventHandler(this.LineProfile_Activated);
			this.Enter += new System.EventHandler(this.LineProfile_Enter);
			this.pContainer.ResumeLayout(false);
			this.pFooter.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudOrder)).EndInit();
			this.pBox.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region override routines

		public override string OnGetHelpIndexKey()
		{
			string result = string.Empty;

			switch (this.ShowType)
			{
				case eChartProfileType.LineProfile:
					result = "SIA.UI.Controls.Dialogs.DlgLineProfile.LineProfile";
					break;
				case eChartProfileType.BoxProfile:
					result = "SIA.UI.Controls.Dialogs.DlgLineProfile.BoxProfile";
					break;
				case eChartProfileType.AreaPlotProfile:
					result = "SIA.UI.Controls.Dialogs.DlgLineProfile.AreaPlot";
					break;
				default:
					result = null;
					break;
			}

			return result;
		}

		
		#endregion

		#region virtual routines


		#endregion

		#region public routines

		public void Zoom(float zoomscale,ref Point opt1,ref Point opt2)
		{
			if( zoomscale<=0 ) return;
			this.zoomscale=zoomscale;
			opt1.X=(int)(pf1.X*zoomscale/100 + .5);
			opt1.Y=(int)(pf1.Y*zoomscale/100 + .5);
			opt2.X=(int)(pf2.X*zoomscale/100 + .5);
			opt2.Y=(int)(pf2.Y*zoomscale/100 + .5);
		}

		public void Reset()
		{
			p1=p2=new Point(0,0);
			ShowLine();
			ShowHorizontalBox();
			ShowVerticalBox();
			ShowAreaPlot();
		}

		public void SetData(SIA.SystemLayer.CommonImage data,Point pt1,Point pt2,float zoomscale)
		{
			if( data==null) return;
			if( zoomscale<=0 ) return;

			this.zoomscale=zoomscale;
			this.data=data;

			//Map coordinate
			p1.X=(int)(pt1.X*100/zoomscale + .5);
			p1.Y=(int)(pt1.Y*100/zoomscale + .5);
			p2.X=(int)(pt2.X*100/zoomscale + .5);
			p2.Y=(int)(pt2.Y*100/zoomscale + .5);

			pf1.X=(float)(pt1.X*100/zoomscale);
			pf1.Y=(float)(pt1.Y*100/zoomscale);
			pf2.X=(float)(pt2.X*100/zoomscale);
			pf2.Y=(float)(pt2.Y*100/zoomscale);

			ShowLine();
			ShowHorizontalBox();
			ShowVerticalBox();
			ShowAreaPlot();
			UpdateControls();
			ShowThresholdDependOnCheckBox();
			
			
		}

		public void ShowThresholdDependOnCheckBox()
		{
			bool show = true;
			if(!checkBox1.Checked || !checkBox1.Visible)
				show = false;
			_lineChart.ThresholdVisible = show;
		}

		public void UpdateControls()
		{
			if(_lineChart == null) return;
			if(_lineChart.HasTrendline()&& (eLineProfileType)cboType.SelectedItem!=eLineProfileType.AreaPlot && _lineChart.Values != null)
			{			
	
				checkBox1.Enabled = true;
				btnTrendline.Enabled = checkBox1.Checked;
			}
			else
			{
				btnTrendline.Enabled = false;
				checkBox1.Enabled = false;			
			}
			btnContour.Enabled=RotateAngle==0;
		}
		

		#endregion

		#region event handlers

		private void LineProfile_Load(object sender, System.EventArgs e)
		{
			this.Icon = new Icon(this.GetType(),"Icon.icon.ico");
			this.Left=Convert.ToInt32(CustomConfiguration.GetValues("LINEPROFILEDIALOG_LEFT",this.Left));
			this.Top=Convert.ToInt32(CustomConfiguration.GetValues("LINEPROFILEDIALOG_TOP",this.Top ));
			this.Width=Convert.ToInt32(CustomConfiguration.GetValues("LINEPROFILEDIALOG_WIDTH",this.Width ));
			this.Height=Convert.ToInt32(CustomConfiguration.GetValues("LINEPROFILEDIALOG_HIGHT",this.Height) );

			cboType.Items.Add(eLineProfileType.Line);
			cboType.Items.Add(eLineProfileType.HorizontalLine);
			cboType.Items.Add(eLineProfileType.VerticalLine);
			cboType.Items.Add(eLineProfileType.HorizontalBox);
			cboType.Items.Add(eLineProfileType.VerticalBox);
			cboType.Items.Add(eLineProfileType.AreaPlot);
			cboType.SelectedIndex=0;

			trendlinetype = eTrendlineType.Linear;

			PersistenceDefault obj=new PersistenceDefault(this);
			obj.Load();	
	
			nudOrder.Visible = false;
			lblOrder.Visible = false;


			this.checkBox1.Visible = true;
			this.btnTrendline.Visible = true;


			EnableBtnTrendLine();			
			
			UpdateControls();
			

		}

		private void btnContour_Click(object sender, System.EventArgs e)
		{
			if (ShowType != eChartProfileType.BoxProfile && ShowType!=eChartProfileType.AreaPlotProfile)
				return;
		}

		private void LineProfile_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
		}

		private void LineProfile_Activated(object sender, System.EventArgs e)
		{	
		}

		private void LineProfile_Enter(object sender, System.EventArgs e)
		{	
		}

		private void LineProfile_Closed(object sender, System.EventArgs e)
		{
			PersistenceDefault obj=new PersistenceDefault(this);
			obj.Save();
			CustomConfiguration.SetValues("LINEPROFILEDIALOG_LEFT",this.Left);
			CustomConfiguration.SetValues("LINEPROFILEDIALOG_TOP",this.Top );
			CustomConfiguration.SetValues("LINEPROFILEDIALOG_WIDTH",this.Width );
			CustomConfiguration.SetValues("LINEPROFILEDIALOG_HIGHT",this.Height );
		}

		private void raMinimum_CheckedChanged(object sender, System.EventArgs e)
		{
			ShowHorizontalBox();		
			ShowVerticalBox();
		}

		private void raMean_CheckedChanged(object sender, System.EventArgs e)
		{
			ShowHorizontalBox();		
			ShowVerticalBox();
		}

		private void raMaximum_CheckedChanged(object sender, System.EventArgs e)
		{
			ShowHorizontalBox();		
			ShowVerticalBox();
		}

		private void raStdDev_CheckedChanged(object sender, System.EventArgs e)
		{
			ShowHorizontalBox();		
			ShowVerticalBox();
		}

		private void cboType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			pBox.Enabled = 
				(((eLineProfileType)cboType.SelectedItem)==eLineProfileType.HorizontalBox||
				((eLineProfileType)cboType.SelectedItem)==eLineProfileType.VerticalBox);

			foreach(Control ctrl in pBox.Controls)
				ctrl.Enabled = pBox.Enabled;

			_lineChart.ThresholdVisible = checkBox1.Checked && ShowType == eChartProfileType.LineProfile;
			nudOrder.Enabled=_lineChart.ThresholdVisible;

			if (((eLineProfileType)cboType.SelectedItem)==eLineProfileType.AreaPlot)
			{
				this._lineChart.Visible = false;
				this._3DPlot.Visible = true;
			}
			else
			{
				this._lineChart.Visible = true;
				this._3DPlot.Visible = false;
			}

			if(ShowType==eChartProfileType.LineProfile)
			{
				Text="Line Profile";
				EnableTrendlineFunction(true);
			}
			else if(ShowType==eChartProfileType.BoxProfile)
			{
				Text="Box Profile";
				EnableTrendlineFunction(false);
			}
			else if(ShowType==eChartProfileType.AreaPlotProfile)
			{
				Text="3D Area Plot";
				EnableTrendlineFunction(false);
			}

			btnContour.Enabled=ShowType==eChartProfileType.BoxProfile || ShowType==eChartProfileType.AreaPlotProfile;
			btnExport.Enabled = (ShowType != eChartProfileType.AreaPlotProfile);
		}

		private void lineChart_Reset(object sender, EventArgs e)
		{
			txtCurrent.Text="";
			if( 
				p1.X > data.Width ||
				p1.Y> data.Height||
				p2.X> data.Width||
				p2.Y> data.Height||
				p1.X < 0 ||
				p1.Y < 0||
				p2.X < 0||
				p2.Y < 0
				) 
				return;

			if(
				(eLineProfileType)cboType.SelectedItem==eLineProfileType.Line ||
				(eLineProfileType)cboType.SelectedItem==eLineProfileType.HorizontalLine ||
				(eLineProfileType)cboType.SelectedItem==eLineProfileType.VerticalLine
				)
			{
				if(dotChange!=null)
					dotChange(Point.Empty);
			}
			else if((eLineProfileType)cboType.SelectedItem==eLineProfileType.HorizontalBox) 
			{
				if(lineChange!=null)
					lineChange(Point.Empty,Point.Empty);
			}
			else if((eLineProfileType)cboType.SelectedItem==eLineProfileType.VerticalBox) 
			{
				if(lineChange!=null)
					lineChange(Point.Empty,Point.Empty);
			}
		}

		private void lineChart_MouseChange(double x, double y)
		{
			double XValue = 0;
			if( 
				p1.X > data.Width ||
				p1.Y> data.Height||
				p2.X> data.Width||
				p2.Y> data.Height||
				p1.X < 0 ||
				p1.Y < 0||
				p2.X < 0||
				p2.Y < 0
				) 
				return;

			if(
				(eLineProfileType)cboType.SelectedItem==eLineProfileType.Line ||
				(eLineProfileType)cboType.SelectedItem==eLineProfileType.HorizontalLine ||
				(eLineProfileType)cboType.SelectedItem==eLineProfileType.VerticalLine
				)
			{
				int xx=0;
				int yy=0;
				double zz=0;


				int A=p2.Y-p1.Y;
				int B=p1.X-p2.X;
				int C=p1.Y*(p2.X-p1.X) - p1.X*(p2.Y-p1.Y);
				if( Math.Abs(p2.X-p1.X) >= Math.Abs(p2.Y-p1.Y) )
				{
					int delta=Math.Abs(p2.X-p1.X);
					int plus= p2.X-p1.X>0 ? 1:-1;
					xx=(int)(p1.X +x*plus);
					yy=CustomConfiguration.yABC(A,B,C,xx);
					zz=GetPixel(xx,yy);
				}
				else
				{
					int delta=Math.Abs(p2.Y-p1.Y);
					int plus= p2.Y-p1.Y>0 ? 1:-1;
					yy=(int)(p1.Y+x*plus);
					xx=CustomConfiguration.xABC(A,B,C,yy);
					zz=GetPixel(xx,yy);
				}
				txtCurrent.Text=zz.ToString("0.##") + "  @(X=" + xx.ToString() + ",Y=" + yy.ToString() + ")";
				XValue = (double)xx;
				if(dotChange!=null)
					dotChange(new Point((int)(xx*zoomscale/100 + .5),(int)(yy*zoomscale/100 + .5)));
			}
			else if((eLineProfileType)cboType.SelectedItem==eLineProfileType.HorizontalBox) 
			{
				PointF ptfcenter=CenterImageViewer;
				ptfcenter.X=(float)(ptfcenter.X*100/zoomscale);
				ptfcenter.Y=(float)(ptfcenter.Y*100/zoomscale);

				//Rotate for View
				Point pt1=CustomConfiguration.PointRotate(p1,ptfcenter,RotateAngle);
				Point pt2=CustomConfiguration.PointRotate(p2,ptfcenter,RotateAngle);

				int delta=Math.Abs(pt2.X-pt1.X);
				int plus= pt2.X-pt1.X>0 ? 1:-1;

				pt1.X=pt2.X=(int)(pt1.X + x*plus);

				double zz=LineValue(pt1,pt2);
				Point o1=CustomConfiguration.PointRotate(pt1,ptfcenter,-RotateAngle);
				Point o2=CustomConfiguration.PointRotate(pt2,ptfcenter,-RotateAngle);
				txtCurrent.Text=zz.ToString("0.##") + "  @(X1=" + o1.X.ToString() + ",Y1=" + o1.Y.ToString() 
					+ "- X2=" + o2.X.ToString() + ",Y2=" + o2.Y.ToString() + ")";


				if(lineChange!=null)
					lineChange(new Point((int)(pt1.X*zoomscale/100 + .5),(int)(pt1.Y*zoomscale/100 + .5)),
						new Point((int)(pt2.X*zoomscale/100 + .5),(int)(pt2.Y*zoomscale/100 + .5)));
			}
			else if((eLineProfileType)cboType.SelectedItem==eLineProfileType.VerticalBox) 
			{
				PointF ptfcenter=CenterImageViewer;
				ptfcenter.X=(float)(ptfcenter.X*100/zoomscale);
				ptfcenter.Y=(float)(ptfcenter.Y*100/zoomscale);

				//Rotate for View
				Point pt1=CustomConfiguration.PointRotate(p1,ptfcenter,RotateAngle);
				Point pt2=CustomConfiguration.PointRotate(p2,ptfcenter,RotateAngle);

				int delta=Math.Abs(pt2.Y-pt1.Y);
				int plus= pt2.Y-pt1.Y>0 ? 1:-1;

				pt1.Y=pt2.Y=(int)(pt1.Y + x*plus);

				double zz=LineValue(pt1,pt2);
				Point o1=CustomConfiguration.PointRotate(pt1,ptfcenter,-RotateAngle);
				Point o2=CustomConfiguration.PointRotate(pt2,ptfcenter,-RotateAngle);
				txtCurrent.Text=zz.ToString("0.##") + "  @(X1=" + o1.X.ToString() + ",Y1=" + o1.Y.ToString() 
					+ "- X2=" + o2.X.ToString() + ",Y2=" + o2.Y.ToString() + ")";

				if(lineChange!=null)
					lineChange(new Point((int)(pt1.X*zoomscale/100 + .5),(int)(pt1.Y*zoomscale/100 + .5)),
						new Point((int)(pt2.X*zoomscale/100 + .5),(int)(pt2.Y*zoomscale/100 + .5)));
			}
			
			//addTrendlineValueTextToIntensityText(XValue);

		}
		
		private void btnExport_Click(object sender, System.EventArgs e)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.RestoreDirectory = true;
			dlg.Filter = "Comma Separated Values(*.csv)|*.csv";
			dlg.DefaultExt = "*.csv"; 
			if( dlg.ShowDialog()==DialogResult.OK)
			{
				Refresh();
				Cursor.Current=Cursors.WaitCursor;
				Export(dlg.FileName);
			}
		}

		private void btnSetting_Click(object sender, System.EventArgs e)
		{
			
			if ((eLineProfileType)cboType.SelectedItem!=eLineProfileType.AreaPlot)
			{
				using (DlgLineProfileSettings dlg = new DlgLineProfileSettings())
				{
					dlg.nuMin.Value=(Decimal)minlimity;
					dlg.nuMax.Value=(Decimal)maxlimity;
					dlg.chkAuto.Checked=bautoscale;
					dlg.chkLorarithmic.Checked=_lineChart.Logarithmic;
					
					if (dlg.ShowDialog(this) == DialogResult.OK)
					{
						minlimity=(double)dlg.nuMin.Value;
						maxlimity=(double)dlg.nuMax.Value;
						bautoscale=dlg.chkAuto.Checked;
						_lineChart.Logarithmic=dlg.chkLorarithmic.Checked;

						ShowLine();
						ShowHorizontalBox();
						ShowVerticalBox();
						ShowAreaPlot();
					}
				}
				
			}
			else
			{
				using (Dialogs.DlgAreaPlotSettings dlg = new Dialogs.DlgAreaPlotSettings())
				{
					#region Area Plot Settings
					try
					{
						bool bSolid = this._3DPlot.GetRenderMode() == 1?true:false;
						dlg.radioSolid.Checked = bSolid;
						dlg.radioWireFrame.Checked = !bSolid;
						dlg.numericUpDownRelX.Value = (Decimal)this._3DPlot.GetXResolution();
						dlg.numericUpDownRelY.Value = (Decimal)this._3DPlot.GetYResolution();				
				
						bool bAutoScale = _3DPlot.GetIntensityScaleMode()== 0? true:false;
						dlg.chAutoScale.Checked = bAutoScale;
						if (bAutoScale)
						{					
							dlg.ndMin.Value = (Decimal)_3DPlot.GetMinIntensity();
							dlg.ndMax.Value = (Decimal)_3DPlot.GetMaxIntensity();
						}
						else 
						{
							dlg.ndMin.Value = (Decimal)_3DPlot.GetMinCustomIntensity();
							dlg.ndMax.Value = (Decimal)_3DPlot.GetMaxCustomIntensity();
						}

						if (dlg.ShowDialog(this) == DialogResult.OK)
						{
							bool bUpdateData = false, bRender = false;

							int nAutocheck = dlg.chAutoScale.Checked ? 0:1; 
							float min = (float)dlg.ndMin.Value, max = (float)dlg.ndMax.Value;
					
							if ( nAutocheck != _3DPlot.GetIntensityScaleMode()
								|| min != _3DPlot.GetMinCustomIntensity()
								|| max != _3DPlot.GetMaxCustomIntensity() ) 
								bUpdateData = true;					
					
							if (dlg.numericUpDownRelX.Value != this._3DPlot.GetXResolution()
								|| dlg.numericUpDownRelY.Value != this._3DPlot.GetYResolution() )
							{						
								bUpdateData = true;					
								bRender = true;
							}

							if (dlg.radioSolid.Checked != bSolid)
								bRender = true;

							_3DPlot.SetIntensityScaleMode(dlg.chAutoScale.Checked ? 0:1 );
							_3DPlot.SetMinCustomIntensity( (float)dlg.ndMin.Value );
							_3DPlot.SetMaxCustomIntensity( (float)dlg.ndMax.Value );

							this._3DPlot.SetRenderMode(dlg.radioSolid.Checked?1:0);
							this._3DPlot.SetResolution(System.Convert.ToInt32(dlg.numericUpDownRelX.Value), 
								System.Convert.ToInt32(dlg.numericUpDownRelY.Value));

							if (bUpdateData)
							{
								ShowAreaPlot();
							}
							else if (bRender)
								this._3DPlot.Render();
						}
					}
					catch(System.Exception exp)
					{
						Trace.WriteLine(exp);
					}
					finally
					{
					}
					#endregion
				}
			}
		}

		private void LineProfile_Move(object sender, System.EventArgs e)
		{
			ResetDotMask();
		}

		private void LineProfile_Resize(object sender, System.EventArgs e)
		{
			ResetDotMask();
		}

		private void chkTrendLine_CheckedChanged(object sender, System.EventArgs e)
		{
			_lineChart.ThresholdVisible = checkBox1.Checked && ShowType == eChartProfileType.LineProfile;
			nudOrder.Enabled=_lineChart.ThresholdVisible;
			EnableBtnTrendLine();
		}
		
		private void nudOrder_ValueChanged(object sender, System.EventArgs e)
		{
			_lineChart.Order = (int)nudOrder.Value;
		}
	
		private void btnTrendline_Click(object sender, System.EventArgs e)
		{		
			
			
			if(mTrendlineDlg == null)
				mTrendlineDlg = new DlgTrendline();
			
			DlgTrendline dlg = mTrendlineDlg;
			mTrendlineDlg.LineChart = _lineChart;
			mTrendlineDlg.DlgLineProfile = this;
			_lineChart.TRENDLINE_DLG = mTrendlineDlg;
			dlg.TrenlineType = trendlinetype;
			
			int dataValid = _lineChart.IsDataValid;
			if ( dataValid == -1 )
			{
				dlg.DisableControl(true);

			}
			else 
			{
				if ( dataValid == 0 )dlg.DisableControl(false);
				dlg.nudPeriod.Maximum = _lineChart.DataValues;
			}

			dlg.ShowDialog();
			this.Refresh();
			trendlinetype = dlg.TrenlineType;
			_lineChart.TrendLineOrder = dlg.Order;
			_lineChart.TrendLinePeriod = dlg.Period;
			
			_lineChart.setTrendlineDlg(dlg);
			_lineChart.TrendLineType = trendlinetype;

		}

		#endregion

		#region internal routines

		#region Line Profile

		private bool checkvalid()
		{
			if( p1.X==p2.X && p1.Y==p2.Y ) 
			{
				_lineChart.Values=null;
				return false;
			}

			return true;
		}


		private void ShowLine()
		{
			if(!checkvalid()) return;

			if(
				(eLineProfileType)cboType.SelectedItem!=eLineProfileType.Line &&
				(eLineProfileType)cboType.SelectedItem!=eLineProfileType.HorizontalLine &&
				(eLineProfileType)cboType.SelectedItem!=eLineProfileType.VerticalLine
				) return;


			int A=p2.Y-p1.Y;
			int B=p1.X-p2.X;
			int C=p1.Y*(p2.X-p1.X) - p1.X*(p2.Y-p1.Y);
			if( Math.Abs(p2.X-p1.X) >= Math.Abs(p2.Y-p1.Y) )
			{
				_lineChart.CaptionX="Pixel Location Along X";
				int delta=Math.Abs(p2.X-p1.X);
				int plus= p2.X-p1.X>0 ? 1:-1;
				double []ydata=new double[delta+1];
				double ymin=double.MaxValue,ymax=double.MinValue;
				for(int i=0;i<=delta;i++)
				{
					int x=p1.X +i*plus;
					ydata[i]=GetPixel( x ,CustomConfiguration.yABC(A,B,C,x));
					if( ymin > ydata[i])	ymin = ydata[i];
					if( ymax < ydata[i])	ymax = ydata[i];
				}
				_lineChart.MinX=0;
				_lineChart.MaxX=delta;
				RoundCoordinateX(delta);

				RoundCoordinateY(ymin,ymax);
				_lineChart.MinY=minlimity;
				_lineChart.MaxY=maxlimity;

				_lineChart.Values=ydata;
			}
			else
			{
				_lineChart.CaptionX="Pixel Location Along Y";
				int delta=Math.Abs(p2.Y-p1.Y);
				int plus= p2.Y-p1.Y>0 ? 1:-1;
				double []ydata=new double[delta+1];
				double ymin=double.MaxValue,ymax=double.MinValue;
				for(int i=0;i<=delta;i++)
				{
					int x=p1.Y+i*plus;
					ydata[i]=GetPixel(CustomConfiguration.xABC(A,B,C,x),x);
					if( ymin > ydata[i])	ymin = ydata[i];
					if( ymax < ydata[i])	ymax = ydata[i];
				}

				_lineChart.MinX=0;
				_lineChart.MaxX=delta;
				RoundCoordinateX(delta);

				RoundCoordinateY(ymin,ymax);
				_lineChart.MinY=minlimity;
				_lineChart.MaxY=maxlimity;

				_lineChart.Values=ydata;
			}
			SetCaptionY();
			txtLimit.Text=p1.ToString() + "-" + p2.ToString();
		}

		private void ShowInterpolatedLine()
		{
			if (!checkvalid()) return;

			if ((eLineProfileType)cboType.SelectedItem!=eLineProfileType.HorizontalLine &&
				(eLineProfileType)cboType.SelectedItem!=eLineProfileType.VerticalLine)
				return;
			
			int A = p2.Y-p1.Y;
			int B = p1.X-p2.X;
			int C = p1.Y*(p2.X-p1.X) - p1.X*(p2.Y-p1.Y);
			if (Math.Abs(p2.X-p1.X) >= Math.Abs(p2.Y-p1.Y) )
			{
				_lineChart.CaptionX="Pixel Location Along X";
				int delta=Math.Abs(p2.X-p1.X);
				int plus= p2.X-p1.X>0 ? 1:-1;
				double []ydata=new double[delta+1];
				double ymin=double.MaxValue,ymax=double.MinValue;
				for(int i=0;i<=delta;i++)
				{
					int x=p1.X +i*plus;
					ydata[i]=GetPixel( x ,CustomConfiguration.yABC(A,B,C,x));
					if( ymin > ydata[i])	ymin = ydata[i];
					if( ymax < ydata[i])	ymax = ydata[i];
				}
				_lineChart.MinX=0;
				_lineChart.MaxX=delta;
				RoundCoordinateX(delta);

				RoundCoordinateY(ymin,ymax);
				_lineChart.MinY=minlimity;
				_lineChart.MaxY=maxlimity;

				_lineChart.Values=ydata;
			}
			else
			{
				_lineChart.CaptionX="Pixel Location Along Y";
				int delta=Math.Abs(p2.Y-p1.Y);
				int plus= p2.Y-p1.Y>0 ? 1:-1;
				double []ydata=new double[delta+1];
				double ymin=double.MaxValue,ymax=double.MinValue;
				for(int i=0;i<=delta;i++)
				{
					int x=p1.Y+i*plus;
					ydata[i]=GetPixel(CustomConfiguration.xABC(A,B,C,x),x);
					if( ymin > ydata[i])	ymin = ydata[i];
					if( ymax < ydata[i])	ymax = ydata[i];
				}

				_lineChart.MinX=0;
				_lineChart.MaxX=delta;
				RoundCoordinateX(delta);

				RoundCoordinateY(ymin,ymax);
				_lineChart.MinY=minlimity;
				_lineChart.MaxY=maxlimity;

				_lineChart.Values=ydata;
			}
			SetCaptionY();
			txtLimit.Text=p1.ToString() + "-" + p2.ToString();
		}

		#endregion
		
		#region Box Profile

		private void ShowHorizontalBox()
		{
			if(!checkvalid()) return;
			if((eLineProfileType)cboType.SelectedItem!=eLineProfileType.HorizontalBox) return;
			if(p1.Y==p2.Y ) 
			{
				_lineChart.Values=null;
				return;
			}

			_lineChart.CaptionX="Pixel Location Along X";

			PointF ptfcenter=CenterImageViewer;
			ptfcenter.X=(float)(ptfcenter.X*100/zoomscale);
			ptfcenter.Y=(float)(ptfcenter.Y*100/zoomscale);

			//Rotate for View
			Point pt1=CustomConfiguration.PointRotate(p1,ptfcenter,RotateAngle);
			Point pt2=CustomConfiguration.PointRotate(p2,ptfcenter,RotateAngle);

			int delta=Math.Abs(pt2.X-pt1.X);
			int plus= pt2.X-pt1.X>0 ? 1:-1;
			Point []ap1=new Point [delta+1];
			Point []ap2=new Point [delta+1];
			for(int i=0;i<=delta;i++)
			{
				ap1[i].X=ap2[i].X=pt1.X +i*plus;
				ap1[i].Y=pt1.Y;
				ap2[i].Y=pt2.Y;
			}

			//Rotate for Image
			ap1=CustomConfiguration.PointRotate(ap1,ptfcenter,-RotateAngle);
			ap2=CustomConfiguration.PointRotate(ap2,ptfcenter,-RotateAngle);

			double []ydata=new double[delta+1];
			double ymin=double.MaxValue,ymax=double.MinValue;
			for(int i=0;i<=delta;i++)
			{
				ydata[i]= LineValue(ap1[i],ap2[i]);
				if( ymin > ydata[i])	ymin = ydata[i];
				if( ymax < ydata[i])	ymax = ydata[i];
			}

			_lineChart.MinX=0;
			_lineChart.MaxX=delta;
			RoundCoordinateX(delta);

			RoundCoordinateY(ymin,ymax);
			_lineChart.MinY=minlimity;
			_lineChart.MaxY=maxlimity;

			_lineChart.Values=ydata;

			SetCaptionY();
			txtLimit.Text=p1.ToString() + "-" + p2.ToString();
		}

		private void ShowVerticalBox()
		{
			if(!checkvalid()) return;
			if((eLineProfileType)cboType.SelectedItem!=eLineProfileType.VerticalBox) return;
			if(p1.X==p2.X ) 
			{
				_lineChart.Values=null;
				return;
			}

			_lineChart.CaptionX="Pixel Location Along Y";

			PointF ptfcenter=CenterImageViewer;
			ptfcenter.X=(float)(ptfcenter.X*100/zoomscale);
			ptfcenter.Y=(float)(ptfcenter.Y*100/zoomscale);

			//Rotate for View
			Point pt1=CustomConfiguration.PointRotate(p1,ptfcenter,RotateAngle);
			Point pt2=CustomConfiguration.PointRotate(p2,ptfcenter,RotateAngle);

			int delta=Math.Abs(pt2.Y-pt1.Y);
			int plus= pt2.Y-pt1.Y>0 ? 1:-1;
			Point []ap1=new Point [delta+1];
			Point []ap2=new Point [delta+1];
			for(int i=0;i<=delta;i++)
			{
				ap1[i].Y=ap2[i].Y=pt1.Y +i*plus;
				ap1[i].X=pt1.X;
				ap2[i].X=pt2.X;
			}

			//Rotate for Image
			ap1=CustomConfiguration.PointRotate(ap1,ptfcenter,-RotateAngle);
			ap2=CustomConfiguration.PointRotate(ap2,ptfcenter,-RotateAngle);

			double []ydata=new double[delta+1];
			double ymin=double.MaxValue,ymax=double.MinValue;
			for(int i=0;i<=delta;i++)
			{
				ydata[i]= LineValue(ap1[i],ap2[i]);
				if( ymin > ydata[i])	ymin = ydata[i];
				if( ymax < ydata[i])	ymax = ydata[i];
			}

			_lineChart.MinX=0;
			_lineChart.MaxX=delta;
			RoundCoordinateX(delta);

			RoundCoordinateY(ymin,ymax);
			_lineChart.MinY=minlimity;
			_lineChart.MaxY=maxlimity;

			_lineChart.Values=ydata;

			SetCaptionY();
			txtLimit.Text=p1.ToString() + "-" + p2.ToString();
		}


		#endregion

		#region Area Plot

		private void ShowAreaPlot()
		{
			if ((eLineProfileType)cboType.SelectedItem!=eLineProfileType.AreaPlot) 
				return;

			// compute line data

			PointF ptfcenter=CenterImageViewer;
			ptfcenter.X=(float)(ptfcenter.X*100/zoomscale);
			ptfcenter.Y=(float)(ptfcenter.Y*100/zoomscale);

			//Rotate for View
			Point pt1=CustomConfiguration.PointRotate(p1,ptfcenter,RotateAngle);
			Point pt2=CustomConfiguration.PointRotate(p2,ptfcenter,RotateAngle);

			int minX = pt1.X, maxX = pt2.X;
			int minY = pt1.Y, maxY = pt2.Y;

			if (System.Math.Abs(minX - maxX) < 2 
				|| System.Math.Abs(minY - maxY) < 2)
				return;

			if (minX > maxX)
			{
				minX = pt2.X;
				maxX = pt1.X;
			}
			if (minY > maxY)
			{
				minY = pt2.Y;
				maxY = pt1.Y;
			}

			this._3DPlot.InitGridData(minX, minY, maxX, maxY);
			for (int x=minX; x<=maxX; x++)
				for (int y=minY; y<=maxY; y++)
				{
					//Rotate for Image
					Point pdata=CustomConfiguration.PointRotate(new Point(x,y),ptfcenter,-RotateAngle);
					this._3DPlot.SetGridData(x, y, (float)GetPixel(pdata.X,pdata.Y)); 
				}
			this._3DPlot.UpdateGridData(); 
			this._3DPlot.EndInitGridData();
			this._3DPlot.Refresh();
			this._3DPlot.Render();
		}

		#endregion

		private void SetCaptionY()
		{
			if( !pBox.Enabled)
				_lineChart.CaptionY="Pixel Value";
			else
			{
				if(raMean.Checked) _lineChart.CaptionY="Average Pixel Value";
				if(raMinimum.Checked) _lineChart.CaptionY="Minimum Pixel Value";
				if(raMaximum.Checked) _lineChart.CaptionY="Maximum Pixel Value";
				if(raStdDev.Checked) _lineChart.CaptionY="Standard Deviation";
			}
		}

		/*
		Standart deviation is the square root of the mean of the square of the deviation:
		average = A
		sample = x
		deviation = x-A
		square of deviation = (x-A)^2
		mean of the square of the deviation = Sum((x-A)^2) / N
		N = number of samples.
		standart deviation = Sqrt(Sum((x-A)^2) / N)
		*/
		private double yValues(int x,int ymin,int ymax)
		{
			if(ymin>ymax )
			{	
				//swap
				ymin=ymin+ymax;
				ymax=ymin-ymax;
				ymin=ymin-ymax;
			}
			double	min=Double.MaxValue;
			double	max=Double.MinValue;
			double	sum=0;
			for(int i=ymin;i<=ymax;i++)
			{
				if( min > GetPixel(x,i) ) 
					min = GetPixel(x,i);

				if( max < GetPixel(x,i) ) 
					max = GetPixel(x,i);

				sum+=GetPixel(x,i);
			}
			double ave=sum/(ymax-ymin+1);

			if(raMinimum.Checked)
				return min;
			else if(raMaximum.Checked)
				return max;
			else if(raMean.Checked)
				return ave;
			else 
			{
				double dev=0;
				double xx;
				for(int i=ymin;i<=ymax;i++)
				{
					xx=GetPixel(x,i)- ave;
					dev+=xx*xx;
				}
				return  Math.Sqrt(dev/(ymax-ymin+1));
			}
		}
		private double xValues(int y,int xmin,int xmax)
		{
			if(xmin>xmax )
			{	
				//swap
				xmin=xmin+xmax;
				xmax=xmin-xmax;
				xmin=xmin-xmax;
			}
			double	min=Double.MaxValue;
			double	max=Double.MinValue;
			double	sum=0;
			for(int i=xmin;i<=xmax;i++)
			{
				if( min > GetPixel(i,y) ) 
					min = GetPixel(i,y);

				if( max < GetPixel(i,y) ) 
					max = GetPixel(i,y);

				sum+=GetPixel(i,y);
			}
			double ave=sum/(xmax-xmin+1);
			if(raMinimum.Checked)
				return min;
			else if(raMaximum.Checked)
				return max;
			else if(raMean.Checked)
				return ave;
			else 
			{
				double dev=0;
				double xx;
				for(int i=xmin;i<=xmax;i++)
				{
					xx=GetPixel(i,y)- ave;
					dev+=xx*xx;
				}
				return  Math.Sqrt(dev/(xmax-xmin+1));
			}
		}

		private double LineValue(Point pp1,Point pp2)
		{
			//prepare data
			int A=pp2.Y-pp1.Y;
			int B=pp1.X-pp2.X;
			int C=pp1.Y*(pp2.X-pp1.X) - pp1.X*(pp2.Y-pp1.Y);
			int delta,plus;
			double []ydata;
			if( Math.Abs(pp2.X-pp1.X) >= Math.Abs(pp2.Y-pp1.Y) )
			{
				delta=Math.Abs(pp2.X-pp1.X);
				plus= pp2.X-pp1.X>0 ? 1:-1;
				ydata=new double[delta+1];
				for(int i=0;i<=delta;i++)
				{
					int x=pp1.X +i*plus;
					ydata[i]=GetPixel(x,CustomConfiguration.yABC(A,B,C,x));
				}
			}
			else
			{
				delta=Math.Abs(pp2.Y-pp1.Y);
				plus= pp2.Y-pp1.Y>0 ? 1:-1;

				//prepare data
				ydata=new double[delta+1];
				for(int i=0;i<=delta;i++)
				{
					int x=pp1.Y+i*plus;
					ydata[i]=GetPixel(CustomConfiguration.xABC(A,B,C,x),x);
				}
			}

			//calc			
			double	min=Double.MaxValue;
			double	max=Double.MinValue;
			double	sum=0;
			for(int i=0;i<=delta;i++)
			{
				if( min >ydata[i] ) min = ydata[i];
				if( max < ydata[i] ) max = ydata[i];
				sum+=ydata[i];
			}
			double ave=sum/(delta+1);
			if(raMinimum.Checked)
				return min;
			else if(raMaximum.Checked)
				return max;
			else if(raMean.Checked)
				return ave;
			else 
			{
				double dev=0;
				double xx;
				for(int i=0;i<=delta;i++)
				{
					xx=ydata[i]- ave;
					dev+=xx*xx;
				}
				return  Math.Sqrt(dev/(delta+1));
			}
		}

		private void RoundCoordinateX(int max)
		{
			if( max/100 >0)
			{
				_lineChart.MaxX=(max/100 + 1)*100;
				_lineChart.SessionX=(int)_lineChart.MaxX/100;
				if(_lineChart.SessionX>10) _lineChart.SessionX=10;
			}
			else if( max/10>0)
			{
				_lineChart.MaxX= (max/10 + 1)*10;
				_lineChart.SessionX=(int)_lineChart.MaxX/10;
			}
			else
			{
				_lineChart.SessionX=(max<2?2:max);
			}
		}

		private void RoundCoordinateY(double min,double max)
		{
			_lineChart.SessionY=5;
			if( !bautoscale) return;

			if(min<=100 && max<=100 && min>=0 && max>=0)// for 255 image
			{
				minlimity= 0;
				maxlimity= ((int)max/10 + 1)*10;
				_lineChart.SessionY= (int)(maxlimity/10);
			}
			else if(min<=200 && max<=200 && min>=0 && max>=0)// for 255 image
			{
				minlimity= 0;
				maxlimity= 200;
			}
			else if(min<=300 && max<=300 && min>=0 && max>=0)// for 255 image
			{
				minlimity= 0;
				maxlimity= 300;
			}
			else//for 65000....
			{
				minlimity= ((int)(min/100) * 100);
				maxlimity= ((int)(1 + (max/100))*100);
				if( (maxlimity-minlimity)/100 < 5 )
					_lineChart.SessionY=(int) ((maxlimity-minlimity)/100);
				else
				{
					while(true)
					{
						double delta=maxlimity-minlimity;
						for(int i=6;i>=3;i--)
						{
							if( delta/i/100== (int)(delta/i/100) )
							{
								_lineChart.SessionY=i;
								return;
							}
						}
						maxlimity+=100;
					} 
				}
			}
		}

		public void EnableTrendlineFunction(bool bTrendline)
		{
			if(bTrendline)
			{
				checkBox1.Enabled = true;
				//btnTrendline.Enabled = true;
				EnableBtnTrendLine();

			}
			else
			{
				checkBox1.Checked = false;
				checkBox1.Enabled = false;
				btnTrendline.Enabled = false;
			}
		}
		public void addTrendlineValueTextToIntensityText(double x)
		{
			//txtCurrent
			string trenlineValue = _lineChart.getTrendLineValueString(x);
			if(trenlineValue != "")
			{
				trenlineValue = "-" + trenlineValue;
				txtCurrent.Text = txtCurrent.Text + trenlineValue ;
			}
 
		}

		private double  GetPixel(int x,int y)
		{
			if(data==null || data.RasterImage==null ) return 0;
			if(x<0 || x>= data.RasterImage._width ) return 0;
			if(y<0 || y>= data.RasterImage._height ) return 0;
			return data.GetPixel(x,y);		
		}

		private void Export(string fp)
		{
			//Export to CSV
			if (_lineChart.Values==null)	return;

			try
			{
				StreamWriter sw=File.CreateText(fp);
				StringBuilder sb=new StringBuilder();
				
				for(int i=0;i<_lineChart.Values.Length;i++)
				{
					sb.Append(i.ToString() + "," + _lineChart.Values[i].ToString() );
					if(i!=_lineChart.Values.Length-1)
						sb.Append(Environment.NewLine);
				}
				sw.Write(sb.ToString());

				sw.Close();
			}
			catch(Exception ex)
			{
				MessageBoxEx.Error(ex.Message);
			}
		}

		private void ResetDotMask()
		{
			if( cboType.SelectedItem==null) return;
			if(
				(eLineProfileType)cboType.SelectedItem==eLineProfileType.Line ||
				(eLineProfileType)cboType.SelectedItem==eLineProfileType.HorizontalLine ||
				(eLineProfileType)cboType.SelectedItem==eLineProfileType.VerticalLine
				)
			{
				if(dotChange!=null)
					dotChange(Point.Empty);
			}
			else if((eLineProfileType)cboType.SelectedItem==eLineProfileType.HorizontalBox|| 
				(eLineProfileType)cboType.SelectedItem==eLineProfileType.VerticalBox)  
			{
				if( lineChange!=null)
					lineChange(Point.Empty,Point.Empty);
			}
		}

		private void EnableBtnTrendLine()
		{
			if (checkBox1.Checked)
			{
				btnTrendline.Enabled = true;
			}
			else
			{
				btnTrendline.Enabled = false;
			}

		}
		public TrendLineFormat LoadTrendLineInfo()
		{
			if (_lineChart!=null)
			{
				return _lineChart.LoadTrendLineInfo();
			}
			return null;
		}

		public void SaveTrendLineInfo()
		{
			if (_lineChart!=null)
			{
				_lineChart.SaveTrendLineInfo();
			}
		}

		public void UpdateTrendLineInfo()
		{
			if (_lineChart!=null)
			{
				_lineChart.UpdateTrendLineInfo();
			}
		}

		
		#endregion
	}
}
