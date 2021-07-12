using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Collections;
using System.Diagnostics;

using SIA.UI.Components;
using SIA.SystemLayer;
using SIA.UI.Components.Renders;
using SIA.UI.Components.Common;
using SIA.Common;
using SIA.UI.Components.Helpers;
using System.Drawing.Drawing2D;

namespace SIA.UI.Controls
{
    /// <summary>
    /// The DocumentView class provides basic functionality for the view of the document (the image)
    /// </summary>
    public class DocumentView 
        : RasterImageViewer
    {
        private const int eventTypePaint = 0;

        private DocumentWorkspace _docWorkspace = null;
        private CommonImage _image = null;
        private IRasterImageRender _render = null;
        private PseudoColor _pseudoColor = null;
        private bool _autoFitGrayScale = false;


        private EventMapTable _eventMapTable = new EventMapTable();

        public new event PaintEventHandler Paint
        {
            add
            {
                lock (_eventMapTable)
                    _eventMapTable.AddHandler(eventTypePaint, value);
            }
            remove
            {
                lock (_eventMapTable)
                    _eventMapTable.RemoveHandler(eventTypePaint, value);
            }
        }

        /// <summary>
        /// Gets the document workspace
        /// </summary>
        public DocumentWorkspace DocumentWorkspace
        {
            get { return _docWorkspace; }
        }

        /// <summary>
        /// Gets or sets the document (the image)
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new CommonImage Image
        {
            get { return _image; }
            set
            {
                _image = value;
                OnImageChanged();
            }
        }

        protected virtual void OnImageChanged()
        {
            base.Image = _image != null ? _image.RasterImage : null;

            // reset RasterImageRender
            if (this._image == null && this._render != null)
            {
                this._render.ImageViewer = null;
                this._render = null;
            }
            else
            {
                if (this._render != null)
                    this._render.IsDirty = true;
            }
        }

        /// <summary>
        /// Gets the active render for the image
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override IRasterImageRender RasterImageRender
        {
            get
            {
                if (this.IsImageAvailable == false)
                    return RasterImageRenderFactory.DefaultRender;
                else if (_render == null || _render == RasterImageRenderFactory.DefaultRender)
                {
                    _render = RasterImageRenderFactory.CreateRender(this);
                    _render.ImageViewer = this;
                }
                return _render;
            }
        }

        /// <summary>
        /// Gets or sets the active pseudo color using for rendering the image
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PseudoColor PseudoColor
        {
            get
            {
                if (_pseudoColor == null)
                    _pseudoColor = PseudoColors.GrayScale;
                return _pseudoColor;
            }
            set
            {
                if (value == null)
                    throw new System.ArgumentNullException("Invalid Pseudo Color parameter");
                _pseudoColor = (PseudoColor)value.Clone();
                OnPseudoColorChanged();
            }
        }

        protected virtual void OnPseudoColorChanged()
        {
            // valid pseudo color
            if (_pseudoColor != null)
            {
                if (_pseudoColor.Colors.Length < 2 || _pseudoColor.Positions.Length < 2 ||
                    _pseudoColor.Colors.Length != _pseudoColor.Positions.Length)
                    throw new System.ArgumentException("Invalid Pseudo Color parameter");

                int num_stops = _pseudoColor.Colors.Length;
                Color[] colors = _pseudoColor.Colors;
                float[] positions = _pseudoColor.Positions;

                if (positions[0] != 0.0F || positions[num_stops - 1] != 1.0F)
                {
                    ArrayList pos_array = new ArrayList(positions);
                    ArrayList color_array = new ArrayList(colors);

                    if (positions[0] != 0.0F)
                    {
                        pos_array.Insert(0, 0.0F);
                        color_array.Insert(0, colors[0]);
                    }

                    if (positions[num_stops - 1] != 1.0F)
                    {
                        pos_array.Add(1.0F);
                        color_array.Add(colors[num_stops - 1]);
                    }

                    _pseudoColor.Colors = (Color[])color_array.ToArray(typeof(Color));
                    _pseudoColor.Positions = (float[])pos_array.ToArray(typeof(float));
                }

                // force the render to refresh buffer
                if (this._render != null)
                    this._render.IsDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets boolean value for auto fitting the intensity range of the image
        /// </summary>
        public bool AutoFitGrayScale
        {
            get { return _autoFitGrayScale; }
            set
            {
                _autoFitGrayScale = value;
                OnAutoFitGrayScaleChanged();
            }
        }

        protected virtual void OnAutoFitGrayScaleChanged()
        {
            this.RasterImageRender.AutoFitGrayScale = _autoFitGrayScale;
        }
       
        public DocumentView()
        {            
        }
		
        public DocumentView(DocumentWorkspace docWorkspace)
        {
            this._docWorkspace = docWorkspace;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // raise paint event
            this.RaisePaintEvents(e);
        }

        protected override void OnPaint(PaintEventArgs e, RectangleF src, RectangleF srcClip, RectangleF dest, RectangleF destClip)
        {
            if (IsImageAvailable)
            {
                IRasterImage image = base.Image;
                if (image != null && image.Length > 0)
                {
                    IRasterImageRender render = this.RasterImageRender;
                    render.UpdateColorMapTable(PseudoColor.Colors, PseudoColor.Positions);
                    render.Paint(e.Graphics, src, srcClip, dest, destClip);
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            using (Transformer transformer = this.Transformer)
            {
                PointF pt = transformer.PointToLogical(new PointF(e.X, e.Y));
                MouseEventArgsEx args = new MouseEventArgsEx(e, pt);
                this.OnMouseDown(args);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            using (Transformer transformer = this.Transformer)
            {
                PointF pt = transformer.PointToLogical(new PointF(e.X, e.Y));
                MouseEventArgsEx args = new MouseEventArgsEx(e, pt);
                this.OnMouseMove(args);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            using (Transformer transformer = this.Transformer)
            {
                PointF pt = transformer.PointToLogical(new PointF(e.X, e.Y));
                MouseEventArgsEx args = new MouseEventArgsEx(e, pt);
                this.OnMouseUp(args);
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            using (Transformer transformer = this.Transformer)
            {
                PointF pt = transformer.PointToLogical(new PointF(e.X, e.Y));
                MouseEventArgsEx args = new MouseEventArgsEx(e, pt);
                this.OnMouseClick(args);
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) != Keys.Control)
            {
                base.OnMouseWheel(e);

                this.CalculateTransform();
                this.Invalidate();
            }

            using (Transformer transformer = this.Transformer)
            {
                PointF pt = transformer.PointToLogical(new PointF(e.X, e.Y));
                MouseEventArgsEx args = new MouseEventArgsEx(e, pt);
                this.OnMouseWheel(args);
            }
        }

        protected virtual void OnMouseDown(MouseEventArgsEx e)
        {
            this.Focus();
        }

        protected virtual void OnMouseMove(MouseEventArgsEx e)
        {

        }

        protected virtual void OnMouseUp(MouseEventArgsEx e)
        {

        }

        protected virtual void OnMouseClick(MouseEventArgsEx e)
        {

        }

        protected virtual void OnMouseWheel(MouseEventArgsEx e)
        {            
        }

        private void RaisePaintEvents(PaintEventArgs e)
        {
            lock (_eventMapTable)
            {
                try
                {
                    List<Delegate> handlers = this._eventMapTable[eventTypePaint];
                    if (handlers != null)
                    {
                        //foreach (Delegate handler in handlers)
                        for (int i = 0; i < handlers.Count; i++)
                        {
                            Delegate handler = handlers[i] as Delegate;
                            if (handler != null)
                                handler.Method.Invoke(handler.Target, new object[] { this, e });
                        }
                    }
                }
                catch (System.Exception exp)
                {
                    Trace.WriteLine(exp);
                    throw;
                }
            }
        }
    }

    public class EventMapTable
    {
        private Dictionary<int, List<Delegate>> _mapTable = new Dictionary<int, List<Delegate>>();

        public void AddHandler(int eventType, Delegate value)
        {
            List<Delegate> handlers;
            if (_mapTable.ContainsKey(eventType))
            {
                handlers = _mapTable[eventType] as List<Delegate>;
            }
            else
            {
                handlers = new List<Delegate>();
                _mapTable[eventType] = handlers;
            }

            handlers.Add(value);
        }

        public void RemoveHandler(int eventType, Delegate value)
        {
            List<Delegate> handlers = null;
            if (_mapTable.ContainsKey(eventType))
                handlers = _mapTable[eventType] as List<Delegate>;
            
            if (handlers != null)
                handlers.Remove(value);
        }

        public List<Delegate> this[int eventType]
        {
            get 
            {
                if (_mapTable.ContainsKey(eventType) == false)
                    return null;
                return _mapTable[eventType] as List<Delegate>; 
            }
        }
    }
}
