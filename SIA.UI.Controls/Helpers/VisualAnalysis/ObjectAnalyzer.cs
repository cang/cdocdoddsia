#define AdvancedObjectAnalyzer__

#define AdvancedObjectFilter


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
using SIA.UI.Controls.UserControls;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Helpers;
using SIA.UI.Controls.Utilities;

using SIA.UI.Components;
using SIA.UI.Components.Helpers;
using System.Collections.Generic;
using SiGlaz.Common.Object;
using SIA.UI.Controls.Dialogs.Analysis;

namespace SIA.UI.Controls.Helpers.VisualAnalysis
{
	/// <summary>
	/// Summary description for ObjectAnalyzer.
	/// </summary>
	public class ObjectAnalyzer : BaseVisualAnalyzer
	{
		#region Fields

        private DlgObjectsList _dlgObjectList = null;
        private DlgAdvancedObjectCombination _dlgCombination = null;

#if AdvancedObjectFilter
        private DlgAdvancedObjectFilter _dlgFilter = null;
#endif

#if AdvancedObjectAnalyzer
        private DlgAdvancedObjectAnalyzer2 _dlgAnalyzer = null;
#endif
        

		private bool _drawAllObjects = true;
		private bool _hightlightSelectedObjects = true;
		private Color _clrHightlightObjects = Color.Red;
		private Hashtable _drawingObjects = null;
		
		private TimeSpan _dtRenderTimeOut = new TimeSpan(0, 0, 10);
		/// <summary>
		/// worker thread use for detect object under mouse position
		/// </summary>
        private object _syncObject = new object();
        private Thread _workerThread = null;
        //private DetectedObject _objUnderMouse = null;

        private Image _arrow = null;
        private Image _normal_arrow = null;
		#endregion

		#region Properties

        public DlgObjectsList DetectedObjectsWindow
        {
            get { return _dlgObjectList; }
        }

        public DlgAdvancedObjectCombination CombineObjectAnalyzer
        {
            get { return _dlgCombination; }
        }

#if AdvancedObjectFilter
        public DlgAdvancedObjectFilter FilterObjectAnalyzer
        {
            get { return _dlgFilter; }
        }
#endif

#if AdvancedObjectAnalyzer
        public DlgAdvancedObjectAnalyzer2 AdvancedObjectAnalyzer
        {
            get { return _dlgAnalyzer; }
        }
#endif

		public bool DrawAllObjects
		{
			get{ return _drawAllObjects;}
			set
			{
				_drawAllObjects = value;
				OnDrawAllObjectsChanged();
			}
		}

		protected virtual void OnDrawAllObjectsChanged()
		{
			if (this.Workspace != null)
				this.Workspace.Invalidate();
		}

		public bool HighlightSelectedObjects
		{
			get {return _hightlightSelectedObjects;}
			set
			{
				_hightlightSelectedObjects = value;
				OnHighlightSelectedObjects();
			}
		}

		protected virtual void OnHighlightSelectedObjects()
		{
			if (this.Workspace != null)
				this.Workspace.Invalidate();
		}

        private bool _drawArrow = false;
        public bool DrawArrow
        {
            get { return _drawArrow; }
            set
            {
                _drawArrow = value;
                OnDrawAllObjectsChanged();
            }
        }

        protected Image HighlightArrow
        {
            get
            {
                if (_arrow == null)
                {
                    string resName = "SIA.UI.Controls.Resources.arrow.png";
                    Type type = this.GetType();
                    using (Stream stream = type.Assembly.GetManifestResourceStream(resName))
                    {
                        if (stream == null)
                            return null;
                        _arrow = Image.FromStream(stream, true);
                    }
                }
                return _arrow;
            }
        }

        protected Image NormalArrow
        {
            get
            {
                if (_normal_arrow == null)
                {
                    string resName = "SIA.UI.Controls.Resources.normal_arrow.png";
                    Type type = this.GetType();
                    using (Stream stream = type.Assembly.GetManifestResourceStream(resName))
                    {
                        if (stream == null)
                            return null;
                        _normal_arrow = Image.FromStream(stream, true);
                    }
                }
                return _normal_arrow;
            }
        }
		#endregion
		
		#region constructor and destructor
		
        public ObjectAnalyzer(ImageWorkspace workspace) 
            : base(workspace)
		{
            _interactiveOnlySelectMode = true;

            // construct new object list window
            _dlgObjectList = new DlgObjectsList(workspace, new ArrayList());
            _dlgCombination = 
                new DlgAdvancedObjectCombination(workspace, new ArrayList());

#if AdvancedObjectFilter
            _dlgFilter = new DlgAdvancedObjectFilter(workspace, new ArrayList());
#endif

#if AdvancedObjectAnalyzer
            _dlgAnalyzer =
                new DlgAdvancedObjectAnalyzer2(workspace, new ArrayList());
#endif
            
			// initialize event handlers
			workspace.DetectedObjectsChanged += new EventHandler(Workspace_DetectedObjectsChanged);

            // refresh drawing objects
			this.RefreshDrawingObjects();
		}

		protected override void Dispose(bool disposing)
		{
            if (this.Workspace != null)
                this.Workspace.DetectedObjectsChanged -= new EventHandler(Workspace_DetectedObjectsChanged);

            if (_dlgObjectList != null)
                _dlgObjectList.Dispose();
            _dlgObjectList = null;

            if (_dlgCombination != null)
                _dlgCombination.Dispose();
            _dlgCombination = null;

#if AdvancedObjectFilter
            if (_dlgFilter != null)
                _dlgFilter.Dispose();
            _dlgFilter = null;
#endif

#if AdvancedObjectAnalyzer
            if (_dlgAnalyzer != null)
                _dlgAnalyzer.Dispose();
            _dlgAnalyzer = null;
#endif

            if (_arrow != null)
            {
                _arrow.Dispose();
                _arrow = null;
            }

            if (_normal_arrow != null)
            {
                _normal_arrow.Dispose();
                _normal_arrow = null;
            }

            base.Dispose(disposing);
		}

		#endregion

		#region event handlers
		
		private void Workspace_DetectedObjectsChanged(object sender, EventArgs e)
		{
            // reset drawing object flags
			this._drawAllObjects = true;

            // refresh objects cached
			this.RefreshDrawingObjects();
		}

        void ImageViewer_MouseClick(object sender, MouseEventArgs e)
        {
            if (Workspace == null ||
                Workspace.DetectedObjects == null ||
                Workspace.DetectedObjects.Count == 0)
                return;

            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                PointF pt = new PointF(e.X, e.Y);
                if (e is MouseEventArgsEx == false)
                {
                    pt = this.Workspace.ImageViewer.Transformer.PointToLogical(pt);
                }
                this.FindObjectUnderMouse(pt);
            }
        }
		#endregion 
		
		#region override routines

		public override void Render(Graphics graph, Rectangle rcClip)
		{
			base.Render (graph, rcClip);

            ImageWorkspace workspace = this.Workspace;
            ArrayList detectedObjects = workspace.DetectedObjects;
            
            if (detectedObjects!= null && this.Visible)
			{
                RasterImageViewer imageViewer = workspace.ImageViewer;

                using (Transformer transformer = imageViewer.Transformer)
                {
                    GraphicsState state = graph.Save();

                    try
                    {
                        RectangleF rectVisible = transformer.RectangleToLogical(imageViewer.PhysicalViewRectangle);

                        RectangleF rcPixel = transformer.RectangleToPhysical(new Rectangle(0, 0, 1, 1));
                        float half_pixel_width = (float)Math.Max(0, rcPixel.Width * 0.5F);
                        float half_pixel_height = (float)Math.Max(0, rcPixel.Height * 0.5F);
                        float pen_width = transformer.LengthToPhysical(1);


                        if (this.DrawAllObjects)
                        {
                            this.OnDrawDetectedObjects(graph, transformer.Transform, rectVisible,
                                half_pixel_width, half_pixel_height, pen_width, detectedObjects);
                        }

                        ArrayList selObjects = workspace.SelectedObjects;
                        if (this.HighlightSelectedObjects && selObjects != null && selObjects.Count > 0)
                        {
                            this.OnDrawSelectedObjects(graph, transformer.Transform, rectVisible,
                                half_pixel_width, half_pixel_height, pen_width, selObjects);
                        }

                    }
                    finally
                    {
                        if (state != null)
                            graph.Restore(state);
                    }
                }
			}
		}

        private void OnDrawDetectedObjects(Graphics graph, Matrix transform, 
            RectangleF rectVisible, float half_pixel_width, float half_pixel_height, float pen_width, ArrayList objects)
        {
            DateTime dtRenderStart = DateTime.Now;
            int count = objects.Count;

            Color[] colors = SiGlaz.Common.ABSDefinitions.DefectVisualizer.Colors;
                //new Color[] 
                //{ Color.Pink, Color.Red, Color.Green, Color.Blue, Color.Yellow };
            int nSupportedColors = colors.Length;
            Brush[] brushes = new Brush[nSupportedColors];
            Pen[] pens = new Pen[nSupportedColors];
            for (int i = 0; i < nSupportedColors; i++)
            {
                brushes[i] = new SolidBrush(Color.FromArgb(80, colors[i]));
                pens[i] = new Pen(colors[i], pen_width);
            }

            Image arrow = this.NormalArrow;
            float arrowWidth = (arrow != null ? arrow.Width : 0);
            float arrowHeight = (arrow != null ? arrow.Height : 0);
            RectangleF srcRect = (arrow == null ? RectangleF.Empty : new RectangleF(0, 0, arrowWidth, arrowHeight));            

            try
            {
                for (int i = 0; i < count; i++)
                {
                    DetectedObject obj = objects[i] as DetectedObject;
                    if (obj == null)
                        continue;

                    // check if bounding rectangle is visible
                    if (obj.RectBound.IntersectsWith(rectVisible) == false)
                        continue;

                    if (this._drawingObjects == null || this._drawingObjects[obj] == null)
                        continue;

                    try
                    {
                        GraphicsPath srcPath = (GraphicsPath)this._drawingObjects[obj];

                        if (srcPath != null)
                        {
                            using (GraphicsPath path = (GraphicsPath)srcPath.Clone())
                            {
                                path.Transform(transform);

                                using (Matrix matrix = new Matrix())
                                {
                                    matrix.Translate(half_pixel_width, half_pixel_height, MatrixOrder.Append);
                                    path.Transform(matrix);
                                }

                                int iColor = obj.ObjectTypeId + 1;
                                if (iColor < 0 || iColor >= nSupportedColors)
                                {
                                    Color clr = (Color)obj.UserData;
                                    using (Brush brush = new SolidBrush(clr))
                                        graph.FillPath(brush, path);
                                    using (Pen pen = new Pen(Color.FromArgb(0x80, clr), pen_width))
                                        graph.DrawPath(pen, path);
                                }
                                else
                                {
                                    graph.FillPath(brushes[iColor], path);
                                    graph.DrawPath(pens[iColor], path);
                                }
                            }

                            //if (obj.NumPixels < 10 && _drawArrow && arrow != null)
                            //{
                            //    PointF[] pts = new PointF[] { new PointF(
                            //    obj.RectBound.Left + 0.5f, obj.RectBound.Bottom + 0.5f) };
                            //    transform.TransformPoints(pts);
                            //    RectangleF dstRect = new RectangleF(pts[0].X - arrowWidth, pts[0].Y, arrowWidth, arrowHeight);
                            //    graph.DrawImage(arrow, dstRect, srcRect, GraphicsUnit.Pixel);
                            //}
                        }
                    }
                    catch (System.Exception exp)
                    {
                        Trace.WriteLine(exp);
                    }

                    TimeSpan duration = DateTime.Now - dtRenderStart;
                    if (duration > _dtRenderTimeOut)
                    {
                        if (Monitor.TryEnter(this, 1))
                        {
                            this._drawAllObjects = false;
                            MessageBoxEx.Warning("There are to many objects. Only selected object will be displayed. Please select object from Object List Window");
                            Monitor.Exit(this);
                        }
                        break;
                    }
                }
            }
            catch
            {
            }
            finally
            {
                arrow = null;

                if (nSupportedColors > 0)
                {
                    for (int i = 0; i < nSupportedColors; i++)
                    {
                        if (brushes[i] != null)
                            brushes[i].Dispose();
                        brushes[i] = null;
                        if (pens[i] != null)
                            pens[i].Dispose();
                        pens[i] = null;
                    }
                    brushes = null;
                    pens = null;
                    colors = null;
                }
            }
        }
        
        private void OnDrawSelectedObjects(Graphics graph, Matrix transform,
            RectangleF rectVisible, float half_pixel_width, float half_pixel_height, float pen_width, ArrayList objects)
        {
            DateTime dtRenderStart = DateTime.Now;

            Image arrow = this.NormalArrow; // this.HighlightArrow;
            float arrowWidth = (arrow != null ? arrow.Width : 0);
            float arrowHeight = (arrow != null ? arrow.Height : 0);
            RectangleF srcRect = (arrow == null ? RectangleF.Empty : new RectangleF(0, 0, arrowWidth, arrowHeight));

            //ImageAnalyzer imageAnalyzer = _workspace.GetAnalyzer("ImageAnalyzer") as ImageAnalyzer;

            for (int i = 0; i < objects.Count; i++)
            {
                DetectedObject obj = objects[i] as DetectedObject;
                if (obj == null)
                    continue;

                if (obj.RectBound.IntersectsWith(rectVisible) == true)
                {
                    try
                    {
                        GraphicsPath srcPath = (GraphicsPath)this._drawingObjects[obj];
                        
                        if (srcPath != null)
                        {
                            using (GraphicsPath path = (GraphicsPath)srcPath.Clone())
                            {
                                path.Transform(transform);
                                using (Matrix matrix = new Matrix())
                                {
                                    matrix.Translate(half_pixel_width, half_pixel_height, MatrixOrder.Append);
                                    path.Transform(matrix);
                                }

                                //Color clr = Color.FromArgb(128, _clrHightlightObjects);
                                Color clr = _clrHightlightObjects;
                                //using (Brush brush = new SolidBrush(clr))
                                //    graph.FillPath(brush, path);
                                using (Pen pen = new Pen(clr, pen_width))
                                    graph.DrawPath(pen, path);
                            }

                            EllipticalDensityShapeObject wrapper =
                                    EllipticalDensityShapeObject.FromDetectedObject(obj, null);
                            {
                                if (obj.NumPixels >= 1)
                                {
                                    DrawEllipse(graph, transform,
                                            (float)wrapper.CenterX, (float)wrapper.CenterY,
                                            (float)wrapper.MajorLength, (float)wrapper.MinorLength,
                                            (float)wrapper.Orientation);
                                    
                                }

                                if (/*obj.NumPixels < 10 && */_drawArrow && arrow != null)
                                {
                                    PointF[] pts = new PointF[] { 
                                    new PointF(
                                        (float)wrapper.CenterX + 0.5f, 
                                        (float)wrapper.CenterY +  0.5f) };
                                    transform.TransformPoints(pts);
                                    RectangleF dstRect = new RectangleF(
                                            pts[0].X - arrowWidth + 3, pts[0].Y, 
                                            arrowWidth, arrowHeight);
                                    graph.DrawImage(arrow, dstRect, srcRect, GraphicsUnit.Pixel);
                                }

                                // dispose
                                wrapper.Dispose();
                                wrapper = null;
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

                TimeSpan duration = DateTime.Now - dtRenderStart;
                if (duration > _dtRenderTimeOut)
                {
                    if (Monitor.TryEnter(this, 1))
                    {
                        this._hightlightSelectedObjects = false;
                        MessageBoxEx.Warning("There are to many selected objects. Please select only several objects from Object List Window");
                        Monitor.Exit(this);
                    }
                    break;
                }
            }
        }

        public static void DrawEllipse(
            Graphics graph, Matrix transform,
            float xCentroid, float yCentroid,
            float majorLength, float minorLength, float orientation)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(
                    new RectangleF(
                        xCentroid - majorLength * 0.5f,
                        yCentroid - minorLength * 0.5f,
                        majorLength, minorLength));
                using (Matrix rotate = new Matrix())
                {
                    rotate.RotateAt(orientation, new PointF(xCentroid, yCentroid));
                    rotate.Translate(0.5f, 0.5f, MatrixOrder.Append);
                    path.Transform(rotate);
                }

                path.Transform(transform);

                using (Pen pen = new Pen(Color.Blue, 2.0f))
                {
                    pen.DashStyle = DashStyle.Dash;
                    graph.DrawPath(pen, path);
                }
            }            
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (Workspace == null ||
                Workspace.DetectedObjects == null ||
                Workspace.DetectedObjects.Count == 0)
                return;

            MouseEventArgsEx args = e as MouseEventArgsEx;

            if (args != null && (e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                PointF pt = args.PointF;                
                this.FindObjectUnderMouse(pt);
            }
        }

		#endregion
		
		#region internal helpers
		
		public static GraphicsPath ObjectToGraphicsPath(DetectedObject obj)
		{
			if (obj == null)
				throw new System.ArgumentNullException("obj");

            if (obj is DetectedObjectEx)
            {
                return ObjectToGraphicsPath(obj as DetectedObjectEx);
            }

			GraphicsPath path = new GraphicsPath();

			try
			{
				PolygonExData polyData = obj.PolygonBoundary;
				
				int start = 0, end = start;
				for(int i=0; i<polyData.nContours; i++)
				{
					end = polyData.pIndexEndPointContours[i];
					PointF[] points = new PointF[end-start+1];
					for(int j=0; start<=end; start++, j++)					
						points[j] = new PointF(polyData.pX[start], polyData.pY[start]);
					if(polyData.pHole[i])
						Array.Reverse(points);					
					path.AddPolygon(points);
				}

				if (polyData.ExtPoints != null && polyData.ExtPoints.nContours > 0)
				{
					start = 0; end = start;
					for(int i=0; i<polyData.ExtPoints.nContours; i++)
					{
						end = polyData.ExtPoints.pIndexEndPointContours[i];
						PointF[] points = new PointF[end-start+1];
						for (int j=0; start<=end; start++, j++)
							points[j] = new PointF(polyData.ExtPoints.pX[start], polyData.ExtPoints.pY[start]);
						
						if (points.Length == 1 || (points.Length==2 && points[0] == points[1]))
							path.AddRectangle(new RectangleF(points[0].X-0.5F, points[0].Y-0.5F, 1, 1));
						else if (points.Length == 2)
							path.AddLine(points[0], points[1]);			
						else
							path.AddPolygon(points);
					}
				}
			}
			catch (System.IndexOutOfRangeException exp)
			{
				Trace.WriteLine(exp);
			}
			catch (System.Exception exp)
			{
				Trace.WriteLine(exp);
			}

			return path;
		}

        private static GraphicsPath ObjectToGraphicsPath(DetectedObjectEx obj)
        {
            if (obj == null)
                throw new System.ArgumentNullException("obj");

            GraphicsPath path = new GraphicsPath();

            try
            {
                foreach (DetectedObject primitiveObj in obj.PrimitiveObjects)
                {
                    try
                    {
                        path.AddPath(ObjectToGraphicsPath(primitiveObj), false);
                    }
                    catch
                    {
                        // nothing
                    }
                }
            }
            catch (System.IndexOutOfRangeException exp)
            {
                Trace.WriteLine(exp);
            }
            catch (System.Exception exp)
            {
                Trace.WriteLine(exp);
            }

            return path;
        }

		private void RefreshDrawingObjects()
		{
			// clean up old stuffs
			if (this._drawingObjects != null)
			{
				IEnumerator enumerator = _drawingObjects.GetEnumerator();
				while (enumerator.MoveNext())
				{
					GraphicsPath path = (GraphicsPath)((DictionaryEntry)enumerator.Current).Value;
					if (path != null)
						path.Dispose();
				}
				_drawingObjects = null;
			}

			// initialize new ones
			if (this.Workspace.DetectedObjects != null && this.Workspace.DetectedObjects.Count > 0)
			{
				// create empty hash table
				_drawingObjects = new Hashtable();
				// retrieve detected objects
				ArrayList objects = this.Workspace.DetectedObjects;

				Random rand = new Random(0xFF);
				int count = objects.Count;
				for (int i=0; i<count; i++)
				{
					DetectedObject obj = (DetectedObject)objects[i];
					if (obj == null)
						continue;
					if (obj is DetectedObjectEx == false && obj.PolygonBoundary == null)
						continue;
					GraphicsPath path = ObjectAnalyzer.ObjectToGraphicsPath(obj);
					obj.UserData = Color.FromArgb(0x80, rand.Next(0xFF), rand.Next(0xFF), rand.Next(0xFF));

					this._drawingObjects.Add(obj, path);
				}
			}
		}

		private void FindObjectUnderMouse(PointF pt)
		{
			//if (Monitor.TryEnter(_syncObject, 1))
			{
				try
				{
                    //if (this._workerThread != null)
                    //{
                    //    this._workerThread.Abort();
                    //    this._workerThread.Join();
                    //}

                    ////this._objUnderMouse = null;
                    //ParameterizedThreadStart threadStart = new ParameterizedThreadStart(this.FindObjectUnderMouseThread);
                    //this._workerThread = new Thread(threadStart);
                    //this._workerThread.Name = "FindObjectUnderMouseThread";
                    //this._workerThread.IsBackground = false;
                    //this._workerThread.Start(pt);

                    FindObjectUnderMouseThread(pt);
				}
				finally
				{
					//Monitor.Exit(_syncObject);
				}				
			}
		}

        private void FindObjectUnderMouseThread(object state)
		{
            PointF pt = (PointF)state;
			
            ImageViewer imageViewer = this.Workspace.ImageViewer;
			ArrayList objects = this.Workspace.DetectedObjects;

			if (objects == null)
				return ;

			try
			{

				// start find objects under mouse position
                using (Transformer transformer = imageViewer.Transformer)
                {
                    RectangleF rectVisible = transformer.RectangleToLogical(imageViewer.PhysicalViewRectangle);

                    RectangleF rcPixel = transformer.RectangleToPhysical(new Rectangle(0, 0, 1, 1));
                    float half_pixel_width = (float)Math.Max(0, rcPixel.Width * 0.5F);
                    float half_pixel_height = (float)Math.Max(0, rcPixel.Height * 0.5F);
                    float pen_width = transformer.LengthToPhysical(1);

                    int count = objects.Count;
                    bool bFound = false;
                    Pen pen = new Pen(Color.White, 1.0F);

                    for (int i = 0; i < count; i++)
                    {
                        DetectedObject obj = (DetectedObject)objects[i];
                        if (obj == null)
                            continue;

                        // check if bounding rectangle is visible
                        if (obj.RectBound.IntersectsWith(rectVisible) == false)
                            continue;

                        // check if bounding rectangle contains point
                        if (obj.RectBound.Contains(pt) == false)
                            continue;

                        if (obj.NumPixels == 1)
                        {
                            this.OnSelectObject(obj);

                            bFound = true;

                            break;
                        }

                        if (this._drawingObjects == null || this._drawingObjects[obj] == null)
                            continue;

                        try
                        {
                            GraphicsPath srcPath = (GraphicsPath)this._drawingObjects[obj];

                            if (srcPath != null)
                            {
                                using (GraphicsPath path = (GraphicsPath)srcPath.Clone())
                                {
                                    if (path.IsVisible(pt) || path.IsOutlineVisible(pt, pen))
                                    {
                                        this.OnSelectObject(obj);

                                        bFound = true;
                                        
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

                    if (!bFound)
                        OnClearSelection();

                    // refresh drawing
                    this.Workspace.Invalidate(true);

                    pen.Dispose();
                }
			}
            //catch(ThreadAbortException exp)
            //{                
            //    Trace.WriteLine(exp);
            //}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
		}

        private void OnClearSelection()
        {
            ImageWorkspace workspace = this.Workspace;
            bool bNotSet = workspace.SelectedObjects != null;
            ArrayList selObjects = workspace.SelectedObjects;

            if (selObjects != null)
                selObjects.Clear();
        }

        protected virtual void OnSelectObject(DetectedObject obj)
        {
            ImageWorkspace workspace = this.Workspace;
            bool bNotSet = workspace.SelectedObjects == null;
            ArrayList selObjects = workspace.SelectedObjects;
            
            if (selObjects == null)
                selObjects = new ArrayList();

            // if Shift key was not pressed then clear last selection
            if ((Control.ModifierKeys & Keys.Shift) != Keys.Shift)
                selObjects.Clear();

            // add new object
            selObjects.Add(obj);

            if (bNotSet)
                workspace.SelectedObjects = selObjects;

            if (_dlgObjectList != null)
            {
                _dlgObjectList.UpdateSelection();

                if (workspace.SelectedObjects != null && 
                    workspace.SelectedObjects.Count > 0)
                {
                    if (_dlgObjectList.Visible == false)
                    {
                        _dlgObjectList.Visible = true;
                    }
                }
            }
        }

		#endregion
	}
}
