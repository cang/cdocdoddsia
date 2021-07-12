using System;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security;
using System.Xml;

using SIA.Common.Mask;

using SiGlaz.Common;
using SIA.UI.MaskEditor.DocToolkit;
using SIA.UI.MaskEditor.DrawTools;
using SIA.UI.MaskEditor.Dialogs;

using SIA.SystemLayer;

using SIA.UI.Components;
using SIA.UI.Components.Helpers;
using SiGlaz.Common.ImageAlignment;


namespace SIA.UI.MaskEditor.DrawTools
{
	internal enum VisualInteractiveMode
	{
		DrawObject,
		ZoomIn,
		ZoomOut,
		Pan
	};

	/// <summary>
	/// Summary description for DrawArea.
	/// </summary>
	public class DrawArea : SIA.UI.Components.CommonImageViewer
	{
		#region member attributes
        private MetrologySystem _metrologySystem = null;
        public MetrologySystem MetrologySystem
        {
            get { return _metrologySystem; }
            set
            {
                _metrologySystem = value;
                if (_metrologySystem != null)
                    _metrologySystem.RebuildTransformer();
                this.Invalidate(true);
            }
        }
        //private void Handler_TransformChanged(object sender, System.EventArgs e)
        //{
        //    if (_metrologySystem != null)
        //        this.Transformer.Transform.Multiply(_metrologySystem.InvTransformer, MatrixOrder.Append);
        //}

		// (instances of DrawObject-derived classes)
		private GraphicsList _graphicsList;    // list of draw objects
		private DrawToolType _activeTool;      // active drawing tool
		// group selection rectangle
		private RectangleF _netRectangle;
		private bool _drawNetRectangle = false;

        private MetaVertex _selectedVertex = null;
        public MetaVertex SelectedVertex
        {
            get
            {
                return _selectedVertex;
            }
            set
            {
                _selectedVertex = value;
            }
        }

		
		private Tool[] tools;                 // array of tools
		private Cursor[] _cursors;			  // array of cursors
		
		// Information about _maskEditor form
		private IMaskEditor _maskEditor;
		private DocManager docManager;

		// context menu handler
		private ContextMenu _contextMenu = null;
		private bool _contextMenuEnabled = true;
		
		// hot key handler
		private bool _hotKeyEnabled = true;

		// visual interactive mode settings
		private VisualInteractiveMode _interactiveMode = VisualInteractiveMode.DrawObject;

		// history member attributes
		private ArrayList _stateCollection = new ArrayList();
		private int _curStateIndex = -1;

		// event members
		public event EventHandler ActiveToolChanged = null;		
		public new event EventHandler InteractiveModeChanged = null;
		//public event EventHandler UserActionCommited = null;
		public event EventHandler RefreshUIObjects = null;

		#endregion

		#region constructor and destructor

		private void InitializeComponents()
		{
			// initialize default cursors
			int iCount = (int)DrawToolType.NumberOfDrawTools;
			this._cursors = new Cursor[iCount];
			for(int i=0; i<iCount; i++)
				this._cursors[i] = System.Windows.Forms.Cursors.Default;

            //TransformChanged += new EventHandler(Handler_TransformChanged);
		}

		public DrawArea()
		{
			this.InitializeComponents();
		}
		
		public DrawArea(IMaskEditor maskEditor)
		{
			if (maskEditor == null)
				throw new System.ArgumentNullException("invalid parameter");
            this.InitializeComponents();	
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose (disposing);
		}

		#endregion

		#region Properties

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IMaskEditor MaskEditor
		{
			get
			{
				return _maskEditor;
			}
			set
			{
				_maskEditor = value;
				OnMaskEditorChanged();
			}
		}

		protected virtual void OnMaskEditorChanged()
		{
            if (this.GraphicsList != null)
                GraphicsList.MaskEditor = this._maskEditor;
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DocManager DocManager
		{
			get
			{
				return docManager;
			}
			set
			{
				docManager = value;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public RectangleF NetRectangle
		{
			get
			{
				return _netRectangle;
			}
			set
			{
				_netRectangle = value;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool DrawNetRectangle
		{
			get
			{
				return _drawNetRectangle;
			}
			set
			{
				_drawNetRectangle = value;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DrawToolType ActiveTool
		{
			get
			{
				return _activeTool;
			}
			set
			{
				DrawToolType oldTool = _activeTool;
				if (IsAllowToolChange(oldTool, value))
				{
					_activeTool = value;
					OnActiveToolChanged(oldTool, value);				
				}
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public GraphicsList GraphicsList
		{
			get
			{
				return _graphicsList;
			}
			set
			{
				_graphicsList = value;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Cursor[] Cursors
		{
			get
			{
				return _cursors;
			}
			set
			{
				_cursors = value;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public float ZoomScale
		{
			get {return this.ScaleFactor;}
			set {this.ScaleFactor = value;}
		}

		public new ContextMenu ContextMenu
		{
			get {return _contextMenuEnabled ? this._contextMenu : null;}
			set 
			{
				this._contextMenu = value;
				OnContextMenuChanged();
			}
		}

		protected virtual void OnContextMenuChanged()
		{
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool ContextMenuEnabled
		{
			get {return _contextMenuEnabled;}
			set
			{
				_contextMenuEnabled = value;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool HotKeyEnabled
		{
			get {return _hotKeyEnabled;}
			set
			{
				_hotKeyEnabled = value;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		internal new VisualInteractiveMode InteractiveMode
		{
			get {return _interactiveMode;}
			set
			{
				this._interactiveMode = value;
				OnInteractiveModeChanged();
			}
		}

		protected virtual void OnInteractiveModeChanged()
		{
			switch (this._interactiveMode)
			{
				case VisualInteractiveMode.ZoomIn:
					this.Cursor = Resources.ApplicationCursors.ZoomIn;
					base.InteractiveMode = RasterViewerInteractiveMode.ZoomTo;
					break;
				case VisualInteractiveMode.ZoomOut:
					this.Cursor = Resources.ApplicationCursors.ZoomOut;
					base.InteractiveMode = RasterViewerInteractiveMode.ZoomOut;
					break;
				case VisualInteractiveMode.Pan:
					this.Cursor = Resources.ApplicationCursors.Pan;
					base.InteractiveMode = RasterViewerInteractiveMode.Pan;
					break;
				case VisualInteractiveMode.DrawObject:
				default:
					base.InteractiveMode = RasterViewerInteractiveMode.Select;
					break;
			}

			if (this.InteractiveModeChanged != null)
				this.InteractiveModeChanged(this, EventArgs.Empty);
		}


		public bool CanUndo
		{
			get 
			{
				return this.ActiveTool == DrawToolType.Pointer && _stateCollection != null && _stateCollection.Count >= 1 && _curStateIndex > 0;
			}
		}

		public bool CanRedo
		{
			get
			{
				return this.ActiveTool == DrawToolType.Pointer && ((ToolPointer)this.tools[(int)this.ActiveTool]).ActiveMode != ToolPointer.SelectionMode.Size &&
					_stateCollection != null && _stateCollection.Count >=1 && _curStateIndex >= 0 && _curStateIndex < _stateCollection.Count-1;
			}
		}

		public bool CanCopyToClipboard
		{
			get
			{
				return this.ActiveTool == DrawToolType.Pointer && ((ToolPointer)this.tools[(int)this.ActiveTool]).ActiveMode != ToolPointer.SelectionMode.Size &&
                    GraphicsList.SelectionCount > 0;
			}
		}

		public bool CanCutToClipboard
		{
			get
			{
				return this.ActiveTool == DrawToolType.Pointer && ((ToolPointer)this.tools[(int)this.ActiveTool]).ActiveMode != ToolPointer.SelectionMode.Size &&
                    GraphicsList.SelectionCount > 0;
			}
		}

		public bool CanPasteFromClipboard
		{
			get
			{
				bool result = false;

				if (this.ActiveTool == DrawToolType.Pointer && ((ToolPointer)this.tools[(int)this.ActiveTool]).ActiveMode != ToolPointer.SelectionMode.Size)
				{
					try
					{
						IDataObject obj = Clipboard.GetDataObject();
						result = obj.GetDataPresent(typeof(GraphicsList));
					}
					catch(System.Exception exp)
					{
						Trace.WriteLine(exp);
					}
					finally
					{
					}
				}
				return result;
			}
		}

		#endregion

		#region override routines

		protected override void OnPaint(PaintEventArgs e)
		{
            GraphicsState gState = e.Graphics.Save();
            try
            {
                base.OnPaint (e);

			    if (this.IsImageAvailable)
			    {
                    DrawNetSelection(e.Graphics);
				    DrawGraphicsObjects(e.Graphics);
                }
            }
            catch
            {
            }
            finally
            {
                e.Graphics.Restore(gState);
            }


            // draw metrology coordinate system
            try
            {
                using (Matrix transform = this.Transform)
                {
                    _metrologySystem.CurrentCoordinateSystem.Draw(
                        e.Graphics, new RectangleF(0, 0, _image.Width, _image.Height),
                        transform);
                }
            }
            catch
            {
            }
            finally
            {
            }
		}
		
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown (e);

			if (this.InteractiveMode == VisualInteractiveMode.DrawObject)
			{
				PointF point = new PointF(e.X, e.Y);
				PointF pt = PointToLogical(point);
				MouseEventArgsF arg = new MouseEventArgsF(e.Button, e.Clicks, pt.X, pt.Y, e.Delta);
				
				if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
				{
					try
					{
						this.BeginUserAction();

						this.HotKeyEnabled = false;
						this.ContextMenuEnabled = false;

						tools[(int)_activeTool].OnMouseDown(this, arg);			
					}
					catch(System.Exception exp)
					{
						this.AbortUserAction();
						Trace.WriteLine(exp);
					}
					finally
					{	
					}
				}
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove (e);

            PointF point = new PointF(e.X, e.Y);
            PointF pt = PointToLogical(point);
            MouseEventArgsF arg = new MouseEventArgsF(e.Button, e.Clicks, pt.X, pt.Y, e.Delta);

			if (this.InteractiveMode == VisualInteractiveMode.DrawObject)
			{
				bool noButtonPressed = 0 == (int)(arg.Button & (MouseButtons.Left | MouseButtons.Right | MouseButtons.Middle));
				if ( noButtonPressed )
				//if (arg.Button == MouseButtons.None )
				{
					tools[(int)_activeTool].OnMouseMove(this, arg);
				}
				else if ((arg.Button & MouseButtons.Left) == MouseButtons.Left)
				{
					tools[(int)_activeTool].OnMouseMove(this, arg);
				}
			}

            RaiseRefreshUIObjects(arg);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp (e);

			if (this.InteractiveMode == VisualInteractiveMode.DrawObject)
			{
				PointF point = new PointF(e.X, e.Y);
				PointF pt = PointToLogical(point);
				MouseEventArgsF arg = new MouseEventArgsF(e.Button, e.Clicks, pt.X, pt.Y, e.Delta);

				bool skipContextMenu = _activeTool == DrawToolType.Polygon;

				tools[(int)_activeTool].OnMouseUp(this, arg);

				this.HotKeyEnabled = true;
				
				if (this.ContextMenuEnabled == false)
					this.ContextMenuEnabled = true;

                if (!skipContextMenu && (e.Button & MouseButtons.Right) == MouseButtons.Right)
                    OnContextMenu(e);
			}			
			else if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
			{
				// reset to draw object mode
				this.InteractiveMode = VisualInteractiveMode.DrawObject;
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown (e);
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp (e);

			if (this.HotKeyEnabled)
			{
				if (this.ActiveTool == DrawToolType.Polygon)
				{
					if (tools[(int)this.ActiveTool] != null)
						((ToolPolygon)tools[(int)this.ActiveTool]).Closed = true;
				}
				if (e.KeyData == Keys.Delete)
					this.DeleteSelection();
			}
		}
		
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress (e);
		}


		protected override void OnImageChanged()
		{
			base.OnImageChanged ();
		}

		#endregion

		#region public operation

		#region Edit features handlers

        public void EditDescription()
        {
            try
            {
                for (int i = 0; i < GraphicsList.Count; i++)
                {
                    DrawObject obj = GraphicsList[i];
                    if (obj.Selected)
                    {
                        FormEditDescription dlg = new FormEditDescription();
                        dlg.txtDescription.Text = obj.Description;
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            obj.Description = dlg.txtDescription.Text;
                        }
                    }
                }
            }
            catch (System.Exception exp)
            {
                Trace.WriteLine(exp);
            }
            finally
            {

            }
        }

        public void SetAutoVertex()
        {
            try
            {
                SelectedVertex.IsAutoVertex = !SelectedVertex.IsAutoVertex;
                this.CommitUserAction();
                Refresh();
            }
            catch
            {
            }
        }

		public void SelectAll()
		{
            GraphicsList.SelectAll();
			this.CommitUserAction();
			Refresh(); 
		}

		public void UnSelectAll()
		{
            GraphicsList.UnselectAll();
			this.CommitUserAction();
			Refresh();
		}

		public void DeleteSelection()
		{
			if (GraphicsList.SelectionCount > 0)
			{
				GraphicsList.DeleteSelection();

				this.CommitUserAction();

				Refresh();
			}
		}

		public void BringSelectionToFront()
		{
			if (GraphicsList.SelectionCount > 0)
			{
				GraphicsList.MoveSelectionToFront();
				
				this.CommitUserAction();

				Refresh();
			}
		}

		public void SendSelectionToBack()
		{
			if (GraphicsList.SelectionCount > 0)
			{
				GraphicsList.MoveSelectionToBack();
				this.CommitUserAction();
				Refresh();
			}
		}

		public void CutSelectionToClipboard()
		{
			try
			{
				GraphicsList objects = new GraphicsList();
				for (int i=0; i<GraphicsList.Count; i++)
				{
					DrawObject obj = GraphicsList[i];
					if (obj.Selected) objects.Add(obj.Copy());
				}

				// clear selection
				this.DeleteSelection();
				
				Clipboard.SetDataObject(objects, true);
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{
				if (this.Parent != null)
					this.Invalidate();
			}
		}

		public void CopySelectionToClipboard()
		{
			try
			{
				GraphicsList objects = new GraphicsList();
				for (int i=0; i<GraphicsList.Count; i++)
				{
					DrawObject obj = GraphicsList[i];
					if (obj.Selected) objects.Add(obj.Copy());
				}

				Clipboard.SetDataObject(objects, true);
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{

			}
		}

		public void PasteFromClipboard()
		{
			try
			{
				IDataObject dataObject = Clipboard.GetDataObject();
				if (true == dataObject.GetDataPresent(typeof(GraphicsList)))
				{
					GraphicsList objects = (GraphicsList)dataObject.GetData(typeof(GraphicsList));
					for (int i=0; i<objects.Count; i++)
					{
						this.GraphicsList.Add(objects[i]);
					}
				}
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{
				if (this.Parent != null)
					this.Invalidate();
			}
		}
		#endregion

		public void ZoomFitOnScreen()
		{			
			this.ZoomToFit();
		}

		public bool IsValidZoomScale(float scale)
		{
			return this.IsValidScaleFactor(scale);
		}

		public PointF PointToLogical(PointF pt)
		{
			Transformer transform = new Transformer(this.Transform);
			PointF retpt = transform.PointToLogical(pt);
            if (_metrologySystem != null)
                retpt = _metrologySystem.ToRealCoordinate(retpt);
            return retpt;
		}

		public PointF PointToPhysical(PointF pt)
		{
            PointF retpt;
            if (_metrologySystem != null)
                retpt = _metrologySystem.ToPixel(pt);
            else
                retpt = pt;
			Transformer transform = new Transformer(this.Transform);
			retpt = transform.PointToPhysical(retpt);
            return retpt;
		}

		
		public void InitializeGraphicsList()
		{
			if (this.Image != null && this.Image.Mask != null)
			{
				IMask mask = this.Image.Mask;
				if (mask.GraphicsList != null)
				{
					GraphicsList objects = (GraphicsList)mask.GraphicsList;
					for (int i=0; i<objects.Count; i++)
						this.GraphicsList.Add(objects[i].Copy());
				}
			}
		}

//		public void RaiseSelectedChanged()
//		{
//
//		}

		
		#region history helpers

		public void Undo()
		{
			if (this.CanUndo)
			{
				int index = this._curStateIndex - 1;
				WorkspaceState state = (WorkspaceState)this._stateCollection[index];
				this.RestoreState(state);
				this._curStateIndex = index;
			}
		}

		public void Redo()
		{
			if (this.CanRedo)
			{
				int index = this._curStateIndex + 1;
				WorkspaceState state = (WorkspaceState)this._stateCollection[index];
				this.RestoreState(state);
				this._curStateIndex = index;
			}
		}

		public void BeginUserAction()
		{
			
		}

		public void CommitUserAction()
		{
			WorkspaceState state = new WorkspaceState(this);

			if (_curStateIndex < this._stateCollection.Count-1)
			{
				int index = _curStateIndex + 1;
				this._stateCollection.RemoveRange(index, this._stateCollection.Count-index);
			}

			this._curStateIndex = this._stateCollection.Add(state);
            this.RaiseRefreshUIObjects(EventArgs.Empty);
		}

		public void AbortUserAction()
		{	
		}

		public void RaiseRefreshUIObjects(EventArgs e)
		{
			if (this.RefreshUIObjects != null)
				this.RefreshUIObjects(this, e);
		}

		#endregion
		
		#endregion

		#region event handlers
	
		#endregion

		#region Document Event Handlers
		private void OnChangeDocument(object sender, System.EventArgs e)
		{
			this.Invalidate();
		}

		private void OnClearDocument(object sender, System.EventArgs e)
		{
			ResetTools();

			// reset history helper
			this._stateCollection.Clear();
			WorkspaceState state = new WorkspaceState(this);
			this._curStateIndex = _stateCollection.Add(state);
			
			// reset drawing mode
			this._activeTool = DrawToolType.Pointer;

			// set enable context menu
			this.ContextMenuEnabled = true;

			if (this.GraphicsList != null )
			{
				this.GraphicsList.Clear(); 
				this.Refresh();
			}	
		}

		private void OnSaveDocument(object sender, SerializationEventArgs e)
		{
			try
			{
				GraphicsList saveObjects = this.GraphicsList.Clone();
				// convert from screen to image coordinate
				//float imgLeft = 0; float imgTop  = 0;
				//float scaleFactor = this.ScaleFactor;
				//
				//Matrix matTransform = new Matrix();
				//matTransform.Reset();
				//matTransform.Translate(-imgLeft, -imgTop, MatrixOrder.Append);
				//matTransform.Scale(scaleFactor, scaleFactor, MatrixOrder.Append);
				//saveObjects.Transform(matTransform);
				//matTransform.Dispose();

				e.Formatter.Serialize(e.SerializationStream, saveObjects);				
			}
			catch ( ArgumentNullException ex )
			{
				HandleSaveException(ex, e);
			}
			catch ( SerializationException ex )
			{
				HandleSaveException(ex, e);
			}
			catch ( SecurityException ex )
			{
				HandleSaveException(ex, e);
			}
			catch ( XmlException ex )
			{
				HandleSaveException(ex, e);
			}
			catch (Exception ex)
			{
				HandleGenericException(ex, e);
			}			
		}

		private void OnLoadDocument(object sender, SerializationEventArgs e)
		{
			try
			{
				// fix bug of upgrade version
				Assembly assembly = Assembly.GetExecutingAssembly();
				e.Formatter.Binder = new VersionBinder();
				
				GraphicsList loadObjects = (GraphicsList)e.Formatter.Deserialize(e.SerializationStream);
				for (int i=0; i<loadObjects.Count; i++)
				{
					DrawObject obj = loadObjects[i];
					if (obj is SiGlaz.Common.DrawPolygon)
					{
						SIA.UI.MaskEditor.DrawTools.DrawPolygon drawPolygon = new SIA.UI.MaskEditor.DrawTools.DrawPolygon((SiGlaz.Common.DrawPolygon)obj);
						drawPolygon.Container = this.GraphicsList;
						this.GraphicsList.Add(drawPolygon);
					}
					else if (obj is SiGlaz.Common.DrawLine)
					{
						SIA.UI.MaskEditor.DrawTools.DrawLine drawLine = new SIA.UI.MaskEditor.DrawTools.DrawLine((SiGlaz.Common.DrawLine)obj);
						drawLine.Container = this.GraphicsList;
						this.GraphicsList.Add(drawLine);
					}
					else if (obj is SiGlaz.Common.DrawEllipse)
					{
						SIA.UI.MaskEditor.DrawTools.DrawEllipse drawEllipse = new SIA.UI.MaskEditor.DrawTools.DrawEllipse((SiGlaz.Common.DrawEllipse)obj);
						drawEllipse.Container = this.GraphicsList;
						this.GraphicsList.Add(drawEllipse);
					}
					else if (obj is SiGlaz.Common.DrawRectangle)
					{
						SIA.UI.MaskEditor.DrawTools.DrawRectangle drawRectangle = new SIA.UI.MaskEditor.DrawTools.DrawRectangle((SiGlaz.Common.DrawRectangle)obj);
						drawRectangle.Container = this.GraphicsList;
						this.GraphicsList.Add(drawRectangle);
					}
					else if (obj is SiGlaz.Common.DrawOnionRing)
					{
						SIA.UI.MaskEditor.DrawTools.DrawOnionRing drawOnionRing = new SIA.UI.MaskEditor.DrawTools.DrawOnionRing((SiGlaz.Common.DrawOnionRing)obj);
						drawOnionRing.Container = this.GraphicsList;
						this.GraphicsList.Add(drawOnionRing);
					}
				}
			}
			catch ( ArgumentNullException ex )
			{
				HandleLoadException(ex, e);
			}
			catch ( SerializationException ex )
			{
				HandleLoadException(ex, e);
			}
			catch ( SecurityException ex )
			{
				HandleLoadException(ex, e);
			}
			catch ( XmlException ex)
			{
				HandleLoadException(ex, e);
			}
			catch (Exception ex)
			{
				HandleGenericException(ex, e);
			}
		}
		#endregion

		#region internal helpers

		public void Initialize(IMaskEditor editor, DocManager docManager)
		{
			// initialize workspace properties
			this.FrameShadowColor = Color.DarkGray;
			this.CenterMode = RasterViewerCenterMode.Both;

			// Keep reference to _maskEditor form
			this._maskEditor = editor;
			// reset base context menu
			base.ContextMenu = null;
			this.DocManager = docManager;

			// set default tool
			_activeTool = DrawToolType.Pointer;

			// create list of graphic objects
            if (GraphicsList == null)
			    GraphicsList = new GraphicsList();
			GraphicsList.MaskEditor = editor;
            GraphicsList.MetroSys = this.MetrologySystem;
			
			// create array of drawing tools
			tools = new Tool[(int)DrawToolType.NumberOfDrawTools];
			tools[(int)DrawToolType.Pointer]	= new ToolPointer();
			tools[(int)DrawToolType.Rectangle]	= new ToolRectangle(this.MaskEditor);
			tools[(int)DrawToolType.Ellipse]	= new ToolEllipse(this.MaskEditor);
			tools[(int)DrawToolType.Line]		= new ToolLine(this.MaskEditor);
			tools[(int)DrawToolType.Polygon]	= new ToolPolygon(this.MaskEditor);
			tools[(int)DrawToolType.OnionRing]  = new ToolOnionRing(this.MaskEditor);

			((ToolObject)tools[(int)DrawToolType.Rectangle]).Cursor		= this._cursors[(int)DrawToolType.Rectangle];
			((ToolObject)tools[(int)DrawToolType.Ellipse]).Cursor		= this._cursors[(int)DrawToolType.Ellipse];
			((ToolObject)tools[(int)DrawToolType.Line]).Cursor			= this._cursors[(int)DrawToolType.Line];
			((ToolObject)tools[(int)DrawToolType.Polygon]).Cursor		= this._cursors[(int)DrawToolType.Polygon];
			((ToolObject)tools[(int)DrawToolType.OnionRing]).Cursor		= this._cursors[(int)DrawToolType.OnionRing];

			this.docManager.LoadEvent += new LoadEventHandler(this.OnLoadDocument);
			this.docManager.SaveEvent += new SaveEventHandler(this.OnSaveDocument);
			this.docManager.ClearEvent += new EventHandler(this.OnClearDocument);
			this.docManager.DocChangedEvent += new EventHandler(this.OnChangeDocument);
		}

		public void SetDirty()
		{
			DocManager.Dirty = true;
		}


		public void DrawGraphicsObjects(Graphics graph)
		{
			if ( GraphicsList != null )
			{
				graph.Transform = this.Transform;
				GraphicsList.Draw(graph);
			}
		}

		public void DrawNetSelection(Graphics g)
		{
			if ( ! DrawNetRectangle )
				return;
			
			ControlPaint.DrawFocusRectangle(g, Rectangle.Round(NetRectangle), Color.Black, Color.Transparent);
		}


		private void OnContextMenu(MouseEventArgs e)
		{
			#region comments
			//			// Change current selection if necessary
			//			Point point = new Point(e.X, e.Y);
			//
			//			int n = GraphicsList.Count;
			//			DrawObject o = null;
			//
			//			for ( int i = 0; i < n; i++ )
			//			{
			//				if ( GraphicsList[i].HitTest(point) == 0 )
			//				{
			//					o = GraphicsList[i];
			//					break;
			//				}
			//			}
			//
			//			if ( o != null )
			//			{
			//				if ( ! o.Selected )
			//					GraphicsList.UnselectAll();
			//
			//				// Select clicked object
			//				o.Selected = true;
			//			}
			//			else
			//			{
			//				GraphicsList.UnselectAll();
			//			}	
			#endregion

			if (this.ContextMenuEnabled)
				this.ContextMenu.Show(this, new Point(e.X, e.Y));
            
			Refresh();
		}

		private void OnActiveToolChanged(DrawToolType oldType, DrawToolType newType)
		{
			if (this.InteractiveMode != VisualInteractiveMode.DrawObject)
				this.InteractiveMode = VisualInteractiveMode.DrawObject;
			
			if (this.ActiveToolChanged != null)
				this.ActiveToolChanged(this, new System.EventArgs());

			if (this.tools != null && this.tools[(int)this.ActiveTool] != null)
			{
				if (this.ActiveTool != DrawToolType.Pointer)
					this.Cursor = ((ToolObject)this.tools[(int)this.ActiveTool]).Cursor;
				else
					this.Cursor = System.Windows.Forms.Cursors.Default;
			}
			else
				this.Cursor = System.Windows.Forms.Cursors.Default;
		}

		private bool IsAllowToolChange(DrawToolType oldType, DrawToolType newType)
		{
			if (oldType == DrawToolType.Polygon && ((ToolPolygon)tools[(int)oldType]).Closed == false)
				return false;
			return true;
		}

		private void ResetTools()
		{
			foreach(Tool tool in this.tools)
				tool.Reset();
		}


		private WorkspaceState SaveState()
		{
			return new WorkspaceState(this);
		}

		private void RestoreState(WorkspaceState state)
		{
			try
			{
				this.BeginUpdate();
				this.GraphicsList = state.Objects.Clone();
				for (int i=0; i<state.Objects.Count; i++)
					this.GraphicsList[i].Selected = state.Objects[i].Selected;

				//this._activeTool = state.ActiveTool;
				//this._contextMenuEnabled = state.ContextMenuEnabled;
				//this._hotKeyEnabled = state.HotkeyEnabled;
				//this._interactiveMode = state.InteractiveMode;
				//this._netRectangle = state.NetRectangle;
				//this._drawNetRectangle = state.DrawNetRectangle;
				// this.ScaleFactor = state.ScaleFactor;
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{
				this.EndUpdate();
			}
		}

		#endregion

		#region Error Handlers
		/// <summary>
		/// Handle exception from docManager_LoadEvent function
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="fileName"></param>
		private void HandleLoadException(Exception ex, SerializationEventArgs e)
		{
			MessageBox.Show(this, 
				//"Cannot open the file. Please check the file format or the access permission,...\r\n",
				"Cannot load the file because either it is not in a supported file type or it has been corrupted.  \r\n" ,
				DocManager.ProductName, 
				MessageBoxButtons.OK, MessageBoxIcon.Error);

			e.Error = true;
		}

		/// <summary>
		/// Handle exception from docManager_SaveEvent function
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="fileName"></param>
		private void HandleSaveException(Exception ex, SerializationEventArgs e)
		{
			MessageBox.Show(this, 
				//"Cannot save the file. Please check the file format or the access permission,...\r\n",
				"Cannot load the file because either it is not in a supported file type or it has been corrupted. \r\n" ,
				DocManager.ProductName, 
				MessageBoxButtons.OK, MessageBoxIcon.Error);

			e.Error = true;
		}

		/// <summary>
		/// Handle generic exception 
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="e"></param>
		private void HandleGenericException(Exception ex, EventArgs e)
		{
			if (e is SerializationEventArgs)
				((SerializationEventArgs)e).Error = true;

			MessageBox.Show(this,
				"Generic error: " + ex.Message, 
				DocManager.ProductName);			
		}
		#endregion
	}

	internal class WorkspaceState : System.IDisposable
	{
		#region member attributes
		
		private GraphicsList _objects = null;
		private DrawToolType _activeTool = DrawToolType.Pointer;
		private VisualInteractiveMode _interactiveMode = VisualInteractiveMode.DrawObject;
		private RectangleF _netRectangle = Rectangle.Empty;
		private bool _drawNetRectangle = false;
		private bool _contextMenuEnabled = true;
		private bool _hotkeyEnabled = true;
		private float _scaleFactor = 1.0F;

		#endregion

		#region public properties
		
		public GraphicsList Objects
		{
			get {return _objects;}
			set {_objects=value;}
		}

		public DrawToolType ActiveTool
		{
			get {return _activeTool;}
			set {_activeTool=value;}
		}

		public VisualInteractiveMode InteractiveMode
		{
			get {return _interactiveMode;}
			set {_interactiveMode=value;}
		}

		public RectangleF NetRectangle
		{
			get {return _netRectangle;}
			set {_netRectangle=value;}
		}

		public bool DrawNetRectangle
		{
			get {return _drawNetRectangle;}
			set {_drawNetRectangle=value;}
		}

		public bool ContextMenuEnabled
		{
			get {return _contextMenuEnabled;}
			set {_contextMenuEnabled=value;}
		}

		public bool HotkeyEnabled
		{
			get {return _hotkeyEnabled;}
			set {_hotkeyEnabled=value;}
		}

		public float ScaleFactor
		{
			get {return _scaleFactor;}
			set {_scaleFactor = value;}
		}


		#endregion

		#region constructor and destructor

		internal WorkspaceState(DrawArea workspace)
		{
			_objects = workspace.GraphicsList.Clone();
			for (int i=0; i<_objects.Count; i++)
				_objects[i].Selected = workspace.GraphicsList[i].Selected;

			_activeTool = workspace.ActiveTool;
			_interactiveMode = workspace.InteractiveMode;
			_netRectangle = workspace.NetRectangle;
			_drawNetRectangle = workspace.DrawNetRectangle;
			_contextMenuEnabled = workspace.ContextMenuEnabled;
			_hotkeyEnabled = workspace.HotKeyEnabled;
			_scaleFactor = workspace.ScaleFactor;			
		}

		#region IDisposable Members

		public void Dispose()
		{
			_objects = null;			
		}

		#endregion

		#endregion
	}

    public class MouseEventArgsF : MouseEventArgs
    {
        public float Xf;
        public float Yf;

        public MouseEventArgsF(MouseButtons button, int clicks, float x, float y, int delta)
            : base(button, clicks, (int)x, (int)y, delta)
        {
            Xf = x;
            Yf = y;
        }
    }
}
