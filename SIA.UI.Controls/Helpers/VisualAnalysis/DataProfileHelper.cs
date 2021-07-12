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
using SIA.SystemLayer;

using SIA.UI.Controls;
using SIA.UI.Controls.Common;
//CONG using SIA.UI.Controls.Components;
using SIA.UI.Controls.UserControls;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Helpers;
using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Helpers.VisualAnalysis.DataProfilers;

using SIA.UI.Components;
using SIA.UI.Components.Helpers;

namespace SIA.UI.Controls.Helpers.VisualAnalysis
{
	/// <summary>
	/// Summary description for DataProfileHelper.
	/// </summary>
	public class DataProfileHelper : BaseVisualAnalyzer
	{
		public enum HitTestStatus
		{
			Begin,
			End,
			Edge,
			Inside,
			Outside,

			// for box profile
			LeftEdge,
			TopEdge,
			RightEdge,
			BottomEdge
		}

		public class HitTestInfo
		{
			public HitTestStatus Status = HitTestStatus.Outside;
			public PointF Point = PointF.Empty;

			public HitTestInfo(HitTestStatus status, PointF pt)
			{
				this.Status = status;
				this.Point = pt;
			}
		}

		private DlgLineProfile2 _dlgDataProfile = null;
		private PlotType _plotType = PlotType.Line;
		private IDataProfiler _dataProfiler = null;

		public DlgLineProfile2 DlgLineProfile
		{
			get {return _dlgDataProfile;}
		}

        public IDataProfiler DataProfiler
        {
            get { return _dataProfiler; }
        }

		public PlotType PlotType
		{
			get {return _plotType;}
			set
			{
				if (_plotType != value)
				{
					_plotType = value;
					OnPlotTypeChanged();
				}
			}
		}

		protected virtual void OnPlotTypeChanged()
		{
			_dlgDataProfile.BeginUpdate();
			_dlgDataProfile.PlotType = this._plotType;
			this.UpdateDataProfiler();
			_dlgDataProfile.EndUpdate();
		}

		public DataProfileHelper(ImageWorkspace owner) 
			: base(owner)
		{
            _interactiveOnlySelectMode = true;

			_dlgDataProfile = new DlgLineProfile2(this);
			_dlgDataProfile.BeginUpdate();
			_dlgDataProfile.TopLevel = true;
			_dlgDataProfile.Owner = owner.FindForm();
			_dlgDataProfile.Closing += new System.ComponentModel.CancelEventHandler(DlgDataProfile_Closing);
			_dlgDataProfile.PlotTypeChanged += new EventHandler(DlgDataProfile_PlotTypeChanged);
			_dlgDataProfile.SelectedValueChanged += new EventHandler(DlgDataProfile_SelectedValueChanged);
			
			// update plot type
			_dlgDataProfile.PlotType = this._plotType;
			_dlgDataProfile.EndUpdate();

            // register for image changed event
            owner.DataChanged += new EventHandler(ImageWorkspace_DataChanged);

			// initialize data profiler
			this.UpdateDataProfiler();
		}

        protected override void Dispose(bool disposing)
		{
			base.Dispose (disposing);

            // unregister for image changed event
            if (this.Workspace != null)
                this.Workspace.DataChanged -= new EventHandler(ImageWorkspace_DataChanged);


			if (this._dataProfiler != null)
				this._dataProfiler.Dispose();
			this._dataProfiler = null;

			if (this._dlgDataProfile != null)
			{
				_dlgDataProfile.Closing -= new System.ComponentModel.CancelEventHandler(DlgDataProfile_Closing);
				_dlgDataProfile.PlotTypeChanged -= new EventHandler(DlgDataProfile_PlotTypeChanged);

				this._dlgDataProfile.Close();
				this._dlgDataProfile.Dispose();
			}
			this._dlgDataProfile = null;
		}

        public override void Activate()
        {
            base.Activate();

            if (this.Workspace.DocumentView != null)
                this.Workspace.DocumentView.InteractiveMode = RasterViewerInteractiveMode.Select;
        }
        
        protected override void OnVisibleChanged()
		{
			base.OnVisibleChanged ();

			// show data profile dialog
			this._dlgDataProfile.Visible = this.Visible;
		}


		public override void Render(Graphics graph, Rectangle rcClip)
		{
			base.Render (graph, rcClip);		

			this._dataProfiler.Render(graph, rcClip);
		}

		public void DisplaySettingsWindow()
		{
			if (this._dataProfiler != null)
				this._dataProfiler.DisplaySettingsWindow();
		}

		public void Export()
		{
			if (this._dataProfiler != null)
				this._dataProfiler.Export();
		}

		public void Update()
		{
			if (this._dataProfiler != null)
				this._dataProfiler.Update();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown (e);

			this._dataProfiler.MouseDown(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			this._dataProfiler.MouseMove(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp (e);

			this._dataProfiler.MouseUp(e);
		}

		protected override void OnInteractiveLine(RasterViewerLineEventArgs e)
		{
			base.OnInteractiveLine(e);
			
			this._dataProfiler.InteractiveLine(e);         
		}

		private void UpdateDataProfiler()
		{
            CommonImage image = this.Workspace.Image;
            int width = image.Width, height = image.Height;
            IDataProfiler dataProfiler = null;
            PointF begin = PointF.Empty, end = PointF.Empty;

            if (this._dataProfiler != null)
            {
                begin = _dataProfiler.Begin;
                end = _dataProfiler.End;
            }

            float middleX = width * 0.5f;
            float middleY = height * 0.5f;

            switch (this._plotType)
            {
                case PlotType.Line:
                    dataProfiler = new LineProfile(this);
                    dataProfiler.Begin = begin;
                    dataProfiler.End = end;
                    break;
                case PlotType.HorizontalLine:
                    dataProfiler = new HorizontalLineProfile(this);
                    dataProfiler.Begin = new PointF(0, middleY);
                    dataProfiler.End = new PointF(width - 1, middleY);
                    break;
                case PlotType.VerticalLine:
                    dataProfiler = new VerticalLineProfile(this);
                    dataProfiler.Begin = new PointF(middleX, 0);
                    dataProfiler.End = new PointF(middleX, height - 1);
                    break;
                case PlotType.HorizontalBox:
                    dataProfiler = new HorizontalBoxProfile(this);
                    dataProfiler.Begin = new PointF(middleX - middleX * 0.5f, middleY - middleY * 0.25f);
                    dataProfiler.End = new PointF(middleX + middleX * 0.5f, middleY + middleY * 0.25f);
                    break;
                case PlotType.VerticalBox:
                    dataProfiler = new VerticalBoxProfile(this);
                    dataProfiler.Begin = new PointF(middleX - middleX * 0.25f, middleY - middleY * 0.5f);
                    dataProfiler.End = new PointF(middleX + middleX * 0.25f, middleY + middleY * 0.5f); ;
                    break;
                case PlotType.AreaPlot:
                    dataProfiler = new AreaPlot(this);
                    dataProfiler.Begin = begin;
                    dataProfiler.End = end;
                    break;
            }

          
			
			this._dataProfiler = dataProfiler;
            if (_dataProfiler != null)
                _dataProfiler.Update();
       
			this.Workspace.Invalidate(true);
		}

		private void DlgDataProfile_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
            this.Visible = false;
            this.Workspace.SelectMode();
		}

		private void DlgDataProfile_PlotTypeChanged(object sender, EventArgs e)
		{
			this._plotType = _dlgDataProfile.PlotType;

			this.UpdateDataProfiler();

		}

		private void DlgDataProfile_SelectedValueChanged(object sender, EventArgs e)
		{
			object absVal = this._dlgDataProfile.DataPlot.SelectedAbscissaData;
			object ordVal = this._dlgDataProfile.DataPlot.SelectedOrdinaryValue;
			this._dataProfiler.UpdateSelectedValue(absVal, ordVal);
		}

        private void ImageWorkspace_DataChanged(object sender, EventArgs e)
        {
            if (this.Workspace != null && !this.Workspace.IsDisposed && this._dataProfiler != null &&
                this.Workspace.Image != null)
                this._dataProfiler.Update();
        }

		
	}
}
