using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;

using SIA.UI.Controls;
using SIA.UI.Controls.UserControls;
using SIA.Plugins.Common;
using SIA.Common.Native;

using SIA.UI.Components;
using SIA.SystemLayer;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Summary description for DialogPreviewBase.
	/// </summary>
	public abstract class DialogPreviewBase : DialogBase
	{
		#region internal constants
		internal const int DEFAULT_TIMEOUT = 100;
		#endregion

		#region member attributes
		private bool _bAutoPreview = true;
		private bool _threadEnabled = false;
		private ImagePreview _previewer = null;
		private ImageWorkspace _workspace = null;

		//private ucNavigator _navigator = null;
		#endregion

		#region constructor and destructor
		private DialogPreviewBase() : base()
		{
			this.InitClass(null, false);
		}

		public DialogPreviewBase(IDocWorkspace workspace) : base()
		{
			if (workspace == null)
				throw new System.ArgumentNullException("Invalid parameter");
			
			this.InitClass(workspace, false);
		}
		
		protected DialogPreviewBase(IDocWorkspace workspace, bool useThread) : base()
		{
			if (workspace == null)
				throw new System.ArgumentNullException("Invalid parameter");
			
			this.InitClass(workspace, useThread);
		}

		protected override void Dispose(bool disposing)
		{
			_previewer = null;

			base.Dispose (disposing);
		}

		private void InitClass(IDocWorkspace workspace, bool useThread)
		{
			_workspace = workspace as ImageWorkspace;
			_threadEnabled = useThread;
		}

		
		private void UninitClass()
		{
			_previewer = null;
		}
		#endregion

		#region public properties
		public bool ThreadEnabled
		{
			get {return _threadEnabled;}
		}


		public ImageWorkspace ImageView
		{
			get {return _workspace;}
		}

		public ImagePreview Previewer
		{
			get
			{
				if (this._previewer == null)
				{
					_previewer = this.GetPreviewer();
					if (_previewer == null)
						throw new System.Exception("Preview is not set to a reference");
					_previewer.EndInteractiveMode += new EventHandler(ImagePreview_EndInteractiveMode);
				}
				return _previewer;
			}
		}

		public bool AutoPreview
		{
			get {return _bAutoPreview;}
			set
			{
				_bAutoPreview = value;
				OnAutoPreviewChanged();
			}
		}

		protected virtual void OnAutoPreviewChanged()
		{

		}

		public Rectangle RectanglePreview
		{
			get
			{
				return Previewer.PreviewRectangle;
			}
		}
		#endregion

		#region operation routines

		protected virtual void ApplyToPreview()
		{
			if (this.ThreadEnabled)
			{
				System.Threading.ThreadPool.QueueUserWorkItem(new WaitCallback(InternalApplyToCommonImage));
			}
			else
			{
				SIA.SystemLayer.CommonImage image = null;
					
				try
				{
					image = this.Previewer.LockPreviewRectangle();
					if (image != null)
						ApplyToCommonImage(image);
				}
				catch(System.Exception exp)
				{
					System.Diagnostics.Trace.WriteLine(exp);
				}
				finally
				{
					this.Previewer.UnlockPreviewRectangle();
				}
			}
		}

		protected virtual void ResetPreview()
		{
			if (this.BeginUpdate() == true)
			{
				try
				{
					this.Previewer.Reset();
				}
				catch(System.Exception exp)
				{
					System.Diagnostics.Trace.WriteLine(exp);
				}
				finally
				{
					this.EndUpdate();
				}
			}
		}

		#region Thread helper

		private void InternalApplyToCommonImage(object state)
		{
			if (BeginUpdate() == true)
			{				
				SIA.SystemLayer.CommonImage image = null;
				try
				{
					image = this.Previewer.LockPreviewRectangle();
					if (image != null)
						ApplyToCommonImage(image);
				}
				catch(System.Exception exp)
				{
					System.Diagnostics.Trace.WriteLine(exp);
				}
				finally
				{
					this.Previewer.UnlockPreviewRectangle();
					EndUpdate();
				}
			}
		}

		private bool BeginUpdate()
		{
			bool bResult = Monitor.TryEnter(this, DEFAULT_TIMEOUT);
			if (bResult)
				this.LockUserInputObjects();
			return bResult;
		}

		private void EndUpdate()
		{
			this.UnlockUserInputObjects();
			Monitor.Exit(this);

			this.Previewer.Invalidate(true);
			Application.DoEvents();
		}

		#endregion

		#endregion

		#region virtual routines

        public abstract void ApplyToCommonImage(CommonImage image);

        public abstract ImagePreview GetPreviewer();

        protected abstract void LockUserInputObjects();

        protected abstract void UnlockUserInputObjects();

		#endregion

		#region override routines
		protected override void OnLoad(EventArgs e)
		{
			// initialize image preview control
			if (this.DesignMode == false)
				this.Previewer.ImageViewer = _workspace.ImageViewer;

			base.OnLoad (e);			
		}
		#endregion

		#region event handlers
		private void ImagePreview_EndInteractiveMode(object sender, EventArgs e)
		{
			if (this._bAutoPreview)
			{
				this.ApplyToPreview();
				this.Previewer.Invalidate(true);
			}
		}

		private void OnPreviewImgeChanged(object sender, System.EventArgs e)
		{
			
		}

		
		#endregion		

		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DialogPreviewBase));
			// 
			// DialogPreviewBase
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(294, 275);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DialogPreviewBase";
			this.Resize += new System.EventHandler(this.OnResize);

		}

		private void OnResize(object sender, System.EventArgs e)
		{
		
		}
	}
}
