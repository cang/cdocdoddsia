using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections;
using System.IO;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using SIA.Common;
using SIA.SystemLayer;

using SIA.UI.Controls;
using SIA.UI.Controls.Common;
using SIA.UI.Controls.UserControls;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Helpers;

using SIA.UI.Components;
using SIA.UI.Components.Helpers;

namespace SIA.UI.Controls.Helpers.VisualAnalysis
{
	public enum VisualAnalyzerInteractiveMode
	{
		Normal,
		Selection,
		Resize,
		Move,
		ResizeAndMove

	};

	public delegate void BeginInteractiveMode(object sender, RasterViewerInteractiveEventArgs e);
	public delegate void EndInteractiveMode(object sender, RasterViewerInteractiveEventArgs e);
	public delegate void CancelInteractiveMode(object sender, RasterViewerInteractiveEventArgs e);
	

	public class BaseVisualAnalyzer : IVisualAnalyzer, IDisposable
	{
		#region constants

		private const string GRID_COLOR = "GRID_COLOR";
		private const string AXIS_COLOR = "AXIS_COLOR";

		#endregion

		#region Fields

		protected ImageWorkspace _workspace = null;

        protected bool _visible = false;
        protected bool _active = false;
        protected bool _interactiveOnlySelectMode = false;

        protected Cursor _cursor = Cursors.Default;

        protected bool _isInteractiveModeBusy = false;
        protected VisualAnalyzerInteractiveMode _interactiveMode = VisualAnalyzerInteractiveMode.Normal;
		
		protected PointF _interactivePoint = PointF.Empty;
		protected PointF[] _interactiveLine = new PointF[2];
		
		public event EventHandler VisibledChanged = null;
        
        public event EventHandler CursorChanging = null;
        public event EventHandler CursorChanged = null;

		public event BeginInteractiveMode BeginInteractiveMode = null;
		public event EndInteractiveMode EndInteractiveMode = null;
		public event CancelInteractiveMode CancelInteractiveMode = null;

		public event PaintEventHandler PreViewPaint = null;
		public event PaintEventHandler PostViewPaint = null;

		#endregion

		#region constructor and destructor
		
		public BaseVisualAnalyzer(ImageWorkspace workspace)
		{
			if (workspace == null)
				throw new System.ArgumentNullException("Invalid parameter");
			_workspace = workspace;

			// load persistence data
			LoadPersistenceData();

			// initialize components
			InitializeComponents();
		}

		~BaseVisualAnalyzer()
		{
            this.Dispose(false);
		}

        #region IDisposable

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            // save persistence data
            SavePersistenceData();

            // release workspace reference
            this._workspace = null;
        }

		#endregion

		#region IVisualAnalyzer Members

		public ImageWorkspace Workspace
		{
			get {return _workspace;}
		}

        public virtual bool Visible
		{
			get {return _visible;}
			set 
			{
				_visible = value;
				OnVisibleChanged();
			}
		}						
		
		protected virtual void OnVisibleChanged()
		{
			if (this.VisibledChanged != null)
				this.VisibledChanged(this, EventArgs.Empty);
		}

		public bool Active
		{
			get 
            {
                //if (_interactiveOnlySelectMode)
                //    return (Workspace.IsSelect || _active);

                return _active;
            }
		}

        public bool IsInteractiveOnlySelectMode
        {
            get { return _interactiveOnlySelectMode; }
        }
        
		public VisualAnalyzerInteractiveMode Mode
		{
			get {return _interactiveMode;}
			set 
			{
				_interactiveMode = value;
				OnInteractiveModeChanged();
			}
		}

		protected virtual void OnInteractiveModeChanged()
		{
			this.Workspace.Invalidate(true);
		}

		public bool IsInteractiveModeBusy
		{
			get {return _isInteractiveModeBusy;}
			set 
			{
				_isInteractiveModeBusy = value;
				OnInteractiveModeBusyChanged();
			}
		}

		protected virtual void OnInteractiveModeBusyChanged()
		{
		}

        public Cursor Cursor
        {
            get
            {
                return this._cursor;
            }
            set
            {
                OnCursorChanging();
                this._cursor = value;
                OnCursorChanged();
            }
        }

        protected virtual void OnCursorChanging()
        {
            if (this.CursorChanging != null)
                this.CursorChanging(this, EventArgs.Empty);
        }

        protected virtual void OnCursorChanged()
        {
            if (this.CursorChanged != null)
                this.CursorChanged(this, EventArgs.Empty);
        }

        public virtual void Activate()
        {
            this._active = true;
            this.Visible = true;
            this.Cursor = Cursors.Default;
        }

        public virtual void Deactivate()
        {
            this._active = false;
            this.Visible = false;
        }

        public virtual void Settings()
        {
        }


        private MouseEventArgsEx GetMouseEventArgs(MouseEventArgs e)
        {
            try
            {
                if (this.Workspace.Image == null)
                    return null;

                if (this.Visible == true && this.Active == true)
                {
                    ImageViewer viewer = this.Workspace.ImageViewer;                    
                    MouseEventArgsEx args = null;
                    using (Transformer transformer = viewer.Transformer)
                    {
                        PointF ptMouse = transformer.PointToLogical(new PointF(e.X, e.Y));
                        args = new MouseEventArgsEx(e.Button, e.Clicks, ptMouse, e.Delta);
                    }

                    return args;
                }
            }
            catch
            {
                throw;
            }

            return null;
        }

        public void MouseDown(MouseEventArgs e)
        {
            MouseEventArgsEx args = this.GetMouseEventArgs(e);
            if (args != null)
            {
                OnMouseDown(args);
            }
        }

        public void MouseMove(MouseEventArgs e)
        {
            MouseEventArgsEx args = this.GetMouseEventArgs(e);
            if (args != null)
            {
                OnMouseMove(args);
            }
        }

        public void MouseUp(MouseEventArgs e)
        {
            MouseEventArgsEx args = this.GetMouseEventArgs(e);
            if (args != null)
            {
                OnMouseUp(args);
            }
        }

        public void MouseClick(MouseEventArgs e)
        {
            MouseEventArgsEx args = this.GetMouseEventArgs(e);
            if (args != null)
            {
                OnMouseClick(args);
            }
        }

        public void KeyPress(KeyPressEventArgs e)
        {
            if (this.Active == true)
                OnKeyPressed(e);
        }

        public void KeyDown(KeyEventArgs e)
        {
            if (this.Active == true)
                OnKeyDown(e);
        }

        public void KeyUp(KeyEventArgs e)
        {
            if (this.Active == true)
                OnKeyUp(e);
        }

        public void LostFocus(EventArgs e)
        {
            if (this.Visible == true && this.Workspace.Image != null)
                OnLostFocus(e);
        }

        public void Paint(PaintEventArgs e)
        {
            if (this.Workspace != null && this.Workspace.Image != null)
            {
                if (this.Visible == true)
                {
                    try
                    {
                        OnPreViewPaint(e);
                    }
                    catch (System.Exception exp)
                    {
                        Trace.WriteLine(exp);
                    }

                    try
                    {
                        Render(e.Graphics, e.ClipRectangle);
                    }
                    catch (System.Exception exp)
                    {
                        Trace.WriteLine(exp);
                    }

                    try
                    {
                        OnPostViewPaint(e);
                    }
                    catch (System.Exception exp)
                    {
                        Trace.WriteLine(exp);
                    }
                }
            }
        }

		#endregion

		#region virtual routines
        public virtual void ResetInteractiveMode()
        {
        }

		protected virtual void InitializeComponents()
		{
		}		

		protected virtual void OnInitializeDefaultValues()
		{
		}

		
		protected virtual void OnLoadPersistenceData(IDictionary settings)
		{
		}

		protected virtual void OnSavePersistenceData(IDictionary settings)
		{
		}

		protected virtual void OnMouseDown(MouseEventArgs e)
		{
			try
			{
				ImageViewer viewer = this.Workspace.ImageViewer;
				CommonImage image = this.Workspace.Image;

				// check if Mouse left button is pressed
				if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
				{
					PointF pt = new PointF(e.X, e.Y);
					if (e is MouseEventArgsEx)
						pt = ((MouseEventArgsEx)e).PointF;
				
					switch (this.Mode)
					{
						case VisualAnalyzerInteractiveMode.Selection:
						{
							RasterViewerPointEventArgs args = new RasterViewerPointEventArgs(RasterViewerInteractiveStatus.Begin, pt);
							OnInteractivePoint(args);
						}
							break;
						case VisualAnalyzerInteractiveMode.Normal:
						case VisualAnalyzerInteractiveMode.Move:
						case VisualAnalyzerInteractiveMode.Resize:
						case VisualAnalyzerInteractiveMode.ResizeAndMove:
						{
							RasterViewerLineEventArgs args = new RasterViewerLineEventArgs(RasterViewerInteractiveStatus.Begin, 
								pt, PointF.Empty);
							OnInteractiveLine(args);
						}
							break;
					}					
				}
			}
			catch (System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
		}

		protected virtual void OnMouseMove(MouseEventArgs e)
		{
			try
			{
				ImageViewer viewer = this.Workspace.ImageViewer;
				CommonImage image = this.Workspace.Image;

				PointF pt = new PointF(e.X, e.Y);
				if (e is MouseEventArgsEx)
					pt = ((MouseEventArgsEx)e).PointF;

				if (this.IsInteractiveModeBusy )
				{
					if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
					{
						switch (this.Mode)
						{
							case VisualAnalyzerInteractiveMode.Selection:
							{
								RasterViewerPointEventArgs args = new RasterViewerPointEventArgs(RasterViewerInteractiveStatus.Working, pt);
								OnInteractivePoint(args);			
							}
								break;
							case VisualAnalyzerInteractiveMode.Normal:
							case VisualAnalyzerInteractiveMode.Move:
							case VisualAnalyzerInteractiveMode.Resize:
							case VisualAnalyzerInteractiveMode.ResizeAndMove:

							{
								RasterViewerLineEventArgs args = new RasterViewerLineEventArgs(RasterViewerInteractiveStatus.Working, 
									_interactiveLine[0], pt);
								OnInteractiveLine(args);
							}
								break;
						}
					}	
				}
			}
			catch (System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
		}

		protected virtual void OnMouseUp(MouseEventArgs e)
		{
			try
			{
				if (this.IsInteractiveModeBusy)
				{
					if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
					{
						PointF pt = new PointF(e.X, e.Y);
						if (e is MouseEventArgsEx)
							pt = ((MouseEventArgsEx)e).PointF;

						switch (this.Mode)
						{
							case VisualAnalyzerInteractiveMode.Normal:
							case VisualAnalyzerInteractiveMode.Move:
							case VisualAnalyzerInteractiveMode.Resize:
							case VisualAnalyzerInteractiveMode.ResizeAndMove:
							{
								RasterViewerLineEventArgs args = null;
								args = new RasterViewerLineEventArgs(RasterViewerInteractiveStatus.End, 
									this._interactiveLine[0], pt);
								OnInteractiveLine(args);
								break;
							}
							case VisualAnalyzerInteractiveMode.Selection:
							{
								RasterViewerPointEventArgs args = new RasterViewerPointEventArgs(RasterViewerInteractiveStatus.End, pt);
								OnInteractivePoint(args);
								break;
							}
							default:
								break;
						}
					}
					else
					{
					
					}
				}
		
			}
			catch (System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
		}

        protected virtual void OnMouseClick(MouseEventArgs e)
        {
            
        }

		
		protected virtual void OnKeyPressed(KeyPressEventArgs e)
		{			
		}

		protected virtual void OnKeyDown(KeyEventArgs e)
		{
		}

		protected virtual void OnKeyUp(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				this.OnCancelInteractiveMode();
		}

		protected virtual void OnLostFocus(EventArgs e)
		{
			if (this.Workspace.Image != null && this.Visible == true && this.Active == true)
				this.OnCancelInteractiveMode();
		}


		public virtual void Render(Graphics graph, Rectangle rcClip)
		{
		}

		public virtual void OnPreViewPaint(PaintEventArgs e)
		{
			if (this.PreViewPaint != null)
				this.PreViewPaint(this, e);
		}

		public virtual void OnPostViewPaint(PaintEventArgs e)
		{
			if (this.PostViewPaint != null)
				this.PostViewPaint(this, e);
		}


		protected virtual void OnBeginInteractiveMode(RasterViewerInteractiveEventArgs e)
		{
			ImageViewer viewer = this.Workspace.ImageViewer;
			if (this.IsInteractiveModeBusy == false)
			{
				this.IsInteractiveModeBusy = true;
				viewer.Capture = true;

				if (this.BeginInteractiveMode != null)
					this.BeginInteractiveMode(this, e);
			}			
		}

		protected virtual void OnEndInteractiveMode(RasterViewerInteractiveEventArgs e)
		{
			ImageViewer viewer = this.Workspace.ImageViewer;
			if (this.IsInteractiveModeBusy == true)
			{
				this.IsInteractiveModeBusy = false;
				viewer.Capture = false;
				this.Mode = VisualAnalyzerInteractiveMode.Normal;

				if (this.EndInteractiveMode != null)
					this.EndInteractiveMode(this, e);
			}			
		}

		protected virtual void OnCancelInteractiveMode()
		{
			ImageViewer viewer = this.Workspace.ImageViewer;
			if (this.IsInteractiveModeBusy == true)
			{
				switch (this.Mode) 
				{						
					case VisualAnalyzerInteractiveMode.Selection:
					{
						RasterViewerPointEventArgs args = new RasterViewerPointEventArgs(RasterViewerInteractiveStatus.End, Point.Empty);
						args.Cancel = true;
						this.OnInteractivePoint(args);
					}
						break;
					case VisualAnalyzerInteractiveMode.Normal:
					case VisualAnalyzerInteractiveMode.Move:
					case VisualAnalyzerInteractiveMode.Resize:
					{
						RasterViewerLineEventArgs args = new RasterViewerLineEventArgs(RasterViewerInteractiveStatus.End, this._interactivePoint, PointF.Empty);
						args.Cancel = true;
						this.OnInteractiveLine(args);
					}
						break;
					default:
						break;
				}

				this.IsInteractiveModeBusy = false;
				viewer.Capture = false;
				this._interactiveMode = VisualAnalyzerInteractiveMode.Normal;

				if (this.CancelInteractiveMode != null)
					this.CancelInteractiveMode(this, new RasterViewerInteractiveEventArgs(RasterViewerInteractiveStatus.End));
			}
		}		

		protected virtual void OnInteractivePoint(RasterViewerPointEventArgs e)
		{

		}

		protected virtual void OnInteractiveLine(RasterViewerLineEventArgs e)
		{
			if (e.Cancel)
			{
				this._interactiveLine = new PointF[2];
				return;
			}

			switch (e.Status)
			{
				case RasterViewerInteractiveStatus.Begin:
					this._interactiveLine[0] = e.BeginF;
					this._interactiveLine[1] = e.BeginF;
					break;
				case RasterViewerInteractiveStatus.End:
					this._interactiveLine[0] = e.BeginF;
					this._interactiveLine[1] = e.EndF;
					break;
				case RasterViewerInteractiveStatus.Working:
					this._interactiveLine[0] = e.BeginF;
					this._interactiveLine[1] = e.EndF;
					break;
			}
		}

		#endregion

        #region Public Methods

        public void LoadPersistenceData(string filename)
		{
			FileStream fs = null;
			try
			{
				fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
				BinaryFormatter formatter = new BinaryFormatter();
				IDictionary settings = (IDictionary)formatter.Deserialize(fs);
				if (settings != null)
					OnLoadPersistenceData(settings);
				else
					throw new System.Exception("Failed to load file:" + filename);
			}
			catch(System.Exception exp)
			{
				throw exp;
			}
			finally
			{
				if (fs != null)
					fs.Close();
			}
		}

		public void SavePersistenceData(string filename)
		{
			FileStream fs = null;
			try
			{
				fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
				IDictionary settings = new Hashtable();
				OnSavePersistenceData(settings);
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(fs, settings);
			}
			catch(System.Exception exp)
			{
				throw exp;
			}
			finally
			{
				if (fs != null)
					fs.Close();
			}
		}


		#endregion

		#region Data Serialization

        public void LoadPersistenceData()
        {
            string filename = this.GetType().FullName.ToString() + ".settings";
            IDictionary settings = (IDictionary)UserSettings.RestoreObject(filename);
            if (settings != null)
            {
                try
                {
                    OnLoadPersistenceData(settings);
                }
                catch
                {
                    OnInitializeDefaultValues();
                }
            }
            else // initialize default values
                OnInitializeDefaultValues();
        }

        public void SavePersistenceData()
        {
            string filename = this.GetType().FullName.ToString() + ".settings";
            IDictionary settings = new Hashtable();
            OnSavePersistenceData(settings);
            UserSettings.StoreObject(filename, settings);
        }

        #endregion
		
	}
}
