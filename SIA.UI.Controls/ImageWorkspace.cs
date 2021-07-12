using System;
using System.Data;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Diagnostics;

using SIA.Common;
using SIA.Common.Analysis;
using SIA.Common.KlarfExport;
using SIA.Common.Mathematics;
using SIA.Common.Utility;
using SIA.Common.Imaging;
using SIA.Common.Mask;
using SIA.Common.PatternRecognition;
using SIA.Common.Imaging.Filters;

using SIA.SystemFrameworks;

using SIA.SystemFrameworks.UI;

using SIA.SystemLayer;
using SIA.SystemLayer.Mask;

using SIA.IPEngine;
using SIA.IPEngine.KlarfExport;

//using SiGlaz.RDE.Ex.UI;
using SIA.UI.Components;
using SIA.UI.Components.Common;
using SIA.UI.Components.Helpers;
using SIA.UI.Components.Renders;

using SIA.UI.MaskEditor;

using SIA.UI.Controls.Common;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Utilities; 
using SIA.UI.Controls.Helpers; 
using SIA.UI.Controls.Helpers.VisualAnalysis;
using SIA.UI.Controls.Commands;

using SIA.Plugins.Common;

using SIA.SystemLayer.ObjectExtraction;
using SIA.SystemLayer.PatternRecogition;

using SIA.UI.Controls.Automation;
using SIA.UI.Controls.Automation.Dialogs;

using SIA.UI.Controls.Automation.Commands;

using SIA.Common.GoldenImageApproach;

using TYPE = System.UInt16;
using SIA.Common.IPLFacade;

namespace SIA.UI.Controls 
{ 
	/// <summary>
	/// Encapsulates an working image, provide the interface for RDE Application
	/// </summary>
	/// 
	public class ImageWorkspace 
        : AnalysisWorkspace, IDocWorkspace
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// drawing helpers
		/// </summary>
		private DrawHelper _drawingHelper = null;

		/// <summary>
		/// Drawing helper
		/// </summary>
		[Browsable(false)]
		public DrawHelper DrawHelper
		{
			get 
			{
				if (_drawingHelper == null)
					_drawingHelper = new DrawHelper(this.ImageViewer);	
				return _drawingHelper;
			}
		}

		#region Properties

        /// <summary>
        /// Gets the document view
        /// </summary>
		public ImageViewer ImageViewer
        {
            get { return base.DocumentView as ImageViewer; }
        }

        /// <summary>
        /// Gets the location of the document
        /// </summary>
		public String FilePath
		{
			get {return this.Image == null ? "" : this.Image.FilePath;}
			set
			{
				if (this.Image != null)
					this.Image.FilePath = value;
			}
		}

		/// <summary>
		/// Get the image center
		/// </summary>
		public PointF ImageCenterF
		{
			get 
			{
				return new PointF(((float)Image.Width)/2, ((float)Image.Height)/2);
			}
		}

        /// <summary>
        /// Gets boolean value indicates whether the document is empty
        /// </summary>
		public bool Empty
		{
			get
			{
				return (this.Image==null);
			}
		}

        /// <summary>
        /// Gets boolean value indicates whether the document is modified
        /// </summary>
		public bool Modified
		{
			get 
			{
				if (this.Image != null)
					return this.Image.Modified;
				return false;
			}
			set 
			{
				if (this.Image != null)
					this.Image.Modified = value;
			}
		}

        public float DeviationAngle
        {
            get { return 270 - CustomConfiguration.PhysicalAngle; }
        }

		public float RotateAngle
		{
			get
			{
				return CustomConfiguration.PhysicalAngle;
			}
		}
		
      
        #endregion

		#region Constructor and destructor
	
		public ImageWorkspace(AppWorkspace appWorkspace)
            : base(appWorkspace)
		{
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// ImageView
			// 
			this.AutoScroll = true;
			this.BackColor = System.Drawing.SystemColors.ControlDark;
			this.Controls.Add(this.ImageViewer);
			this.Name = "ImageView";
			this.Size = new System.Drawing.Size(500, 300);
			this.ResumeLayout(false);

		}
		#endregion

		public void InitializeDocumentWorkspace()
		{
			try
			{
				// initialize screen stretch window
				this.CreateScreenStretchWindow();

                InitWorkspace();
			}
			catch (Exception exp)
			{
				Trace.WriteLine(exp);
				throw;
			}
		}

		public void UninitializeDocumentWorkspace()
		{
			// destroy screen stretch window
			this.DestroyScreenStretchWindow();
		}

        protected override void OnImageChanged()
        {
            base.OnImageChanged();                   
        }

		public override void CreateWorkspace(String filename)
		{
            base.CreateWorkspace(filename);
		}

        public override void CreateWorkspace(FileStream fs)
        {
            base.CreateWorkspace(fs);
        }

        public override void CreateWorkspace(MemoryStream fs)
        {
            base.CreateWorkspace(fs);
        }

        public override void CreateWorkspace(CommonImage image)
        {
            base.CreateWorkspace(image);

            // initialize extra workspace stuffs
            this.InitializeDocumentWorkspace();
        }
        
        public override void DestroyWorkspace()
		{
            // uninitialized extra stuffs
			this.UninitializeDocumentWorkspace();			

			// reset detected objects information
			this.SelectedObjects = null;
			this.DetectedObjects = null;
        
            base.DestroyWorkspace();
        }

        protected override DocumentView OnCreateDocumentView()
        {
            ImageViewer imageViewer = new ImageViewer(this);
            this.SuspendLayout();
            // 
            // _imageViewer
            // 
            imageViewer.AutoDisposeImages = false;
            imageViewer.AutoFitGrayScale = false;
            imageViewer.AutoResetScaleFactor = false;
            imageViewer.AutoResetScrollPosition = false;
            imageViewer.AutoScroll = true;
            //imageViewer.BackColor = System.Drawing.SystemColors.ControlDark;
            imageViewer.BackColor = Color.FromKnownColor(KnownColor.LightGray);
            imageViewer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            imageViewer.CenterMode = SIA.UI.Components.RasterViewerCenterMode.Both;
            imageViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            imageViewer.DoubleBuffer = true;
            imageViewer.FrameColor = System.Drawing.Color.Black;
            imageViewer.FrameIsPartOfView = true;
            imageViewer.FrameShadowColor = System.Drawing.Color.DimGray;
            imageViewer.FrameShadowIsPartOfView = false;
            imageViewer.Image = null;
            imageViewer.Location = new System.Drawing.Point(0, 0);
            imageViewer.Mask = null;
            imageViewer.MaxPixelScaleFactor = 0.1F;
            imageViewer.MinPixelScaleFactor = 0.025F;
            imageViewer.Name = "imageViewer";
            imageViewer.RotateAngle = 0F;
            imageViewer.ScaleFactor = 1F;
            imageViewer.Size = new System.Drawing.Size(547, 394);
            imageViewer.SizeMode = SIA.UI.Components.RasterViewerSizeMode.Normal;
            imageViewer.TabIndex = 1;
            imageViewer.TabStop = false;
            // 
            // DocumentWorkspace
            // 
            this.Controls.Add(imageViewer);
            this.Name = "ImageWorkspace";
            this.Size = new System.Drawing.Size(547, 394);
            this.ResumeLayout(false);

            return imageViewer;
        }

        protected override void OnInitializeDocumentView()
        {
            base.OnInitializeDocumentView();

            bool autoFit = true;
            int minValue = 0;
            int maxValue = 255;

            try
            {
                // restore image viewer view range
                autoFit = (bool)CustomConfiguration.GetValues("ImageViewer.RasterImageRender.AutoFitGrayScale", true);
                minValue = (int)(double)CustomConfiguration.GetValues("ImageViewer.RasterImageRender.ViewRange.Min", -1);
                maxValue = (int)(double)CustomConfiguration.GetValues("ImageViewer.RasterImageRender.ViewRange.Max", -1);
            }
            catch
            {
                autoFit = true;
                minValue = 0;
                maxValue = 255;
            }

            int minGrayValue = (int)this.Image.MinGreyValue;
            int maxGrayValue = (int)this.Image.MaxGreyValue;

            // update auto fit grayscale
            this.ImageViewer.RasterImageRender.AutoFitGrayScale = autoFit;

            // update view range
            if (minValue >= 0 && maxValue >= 0 && minValue < maxValue &&
                minGrayValue <= minValue && minValue <= maxGrayValue &&
                minGrayValue <= maxValue && maxValue <= maxGrayValue)
            {
                this.ImageViewer.RasterImageRender.ViewRange = new DataRange(minValue, maxValue);
            }
        }

        protected override void OnUninitializeDocumentView()
        {
            // save image viewer view range
            if (this.ImageViewer.RasterImageRender != null)
            {
                IRasterImageRender render = this.ImageViewer.RasterImageRender;
                CustomConfiguration.SetValues("ImageViewer.RasterImageRender.AutoFitGrayScale", render.AutoFitGrayScale);
                CustomConfiguration.SetValues("ImageViewer.RasterImageRender.ViewRange.Min", render.ViewRange.Minimum);
                CustomConfiguration.SetValues("ImageViewer.RasterImageRender.ViewRange.Max", render.ViewRange.Maximum);
            }

            base.OnUninitializeDocumentView();
        }
        
		public static void loadTrebyshevtLookup()
		{
            try
            {
                SIA.SystemLayer.CommonImage.loadTrebyshevtLookup();
            }
            catch (Exception exp)
            {
                Trace.WriteLine(exp);
            }
		}		

        #region Screen Strecth

        private DlgScreenStretchEx _wndScreenStretch = null;

        public bool ScreenStretch
        {
            get
            {
                return _wndScreenStretch != null && _wndScreenStretch.Visible;
            }
            set
            {
                if (_wndScreenStretch != null)
                    _wndScreenStretch.Visible = value;
            }
        }

        public DlgScreenStretchEx ScreenStretchWindow
        {
            get { return _wndScreenStretch; }
        }

        protected virtual void CreateScreenStretchWindow()
        {
            this._wndScreenStretch = new DlgScreenStretchEx(this);
            this._wndScreenStretch.TopMost = false;
            this._wndScreenStretch.Owner = ParentForm;
            this._wndScreenStretch.TopLevel = true;

            // refresh screen stretch window for loaded data
            this._wndScreenStretch.UpdateData(false);
        }

        protected virtual void DestroyScreenStretchWindow()
        {
            if (this._wndScreenStretch != null)
            {
                if (this._wndScreenStretch.Visible)
                    this._wndScreenStretch.Close();
                this._wndScreenStretch.Dispose();
            }
            this._wndScreenStretch = null;
        }

        #endregion		

        #region Object Detection Helpers

		private ArrayList _detectedObjects = null;
		public event EventHandler DetectedObjectsChanged = null;

		private ArrayList _selectedObjects = null;
		public event EventHandler SelectedObjectsChanged = null;

        /// <summary>
        /// Gets or sets the detected objects
        /// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Category("Object Detection")]
		public ArrayList DetectedObjects
		{
			get {return _detectedObjects;}
			set 
			{
				if (_detectedObjects != value)
				{
					this._detectedObjects = value;
					OnDetectedObjectsChanged();
				}
			}
		}

		protected virtual void OnDetectedObjectsChanged()
		{
			if (this.DetectedObjectsChanged != null)
				this.DetectedObjectsChanged(this, EventArgs.Empty);
		}

        /// <summary>
        /// Gets or sets the selected objects
        /// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Category("Object Detection")]
		public ArrayList SelectedObjects
		{
			get {return _selectedObjects;}
			set
			{
				_selectedObjects = value;
				OnSelectedObjectsChanged();
			}
		}

		protected virtual void OnSelectedObjectsChanged()
		{
			if (this.SelectedObjectsChanged != null)
				this.SelectedObjectsChanged(this, EventArgs.Empty);
		}
		
		#endregion        
    }
}


