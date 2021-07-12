using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;

using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;

namespace SiGlaz.Common
{
	/// <summary>
	/// Base class for all draw objects
	/// </summary>
	public abstract class DrawObject
	{
        #region Members
		private GraphicsList _container = null;
	
        // Object properties
        private string _description = "";
        private bool _selected;
        private Color color = Color.Green;
        private int penWidth = 0;
		private Color _brushColor;
		private Color _brushTracker = Color.DarkGreen;
		private Color _penTracker = Color.Green;
		private int _alpha = 0x88;
        private bool _initialized;
		
        // Last used property values (may be kept in the Registry)
        private static Color lastUsedColor = Color.Blue;
        private static int lastUsedPenWidth = 1;		
		private static Color _lastUsedBrushColor = Color.White;
		private static int	 _lastUsedAlpha = 0x80;
		
        // Entry names for serialization
        private const string entryDescription = "Description";
        private const string entryColor = "Color";
        private const string entryPenWidth = "PenWidth";
		private const string entryBrushColor = "BrushColor";
		private const string entryAlpha = "Alpha";

        #endregion

        #region Properties

		public GraphicsList Container
		{
			get {return _container;}			
			set 
			{
				_container = value;
				OnContainerChanged();
			}
		}

		protected virtual void OnContainerChanged()
		{
			
		}

        /// <summary>
        /// Description
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        /// <summary>
        /// Initialized
        /// </summary>
        public bool Initialized
        {
            get
            {
                return _initialized;
            }
            set
            {
                _initialized = value;
            }
        }

		/// <summary>
        /// Selection flag
        /// </summary>
        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
				OnSelectedChanged();
            }
        }

		protected virtual void OnSelectedChanged()
		{
		}

        /// <summary>
        /// Color
        /// </summary>
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        /// <summary>
        /// Pen width
        /// </summary>
        public int PenWidth
        {
            get
            {
                return penWidth;
            }
            set
            {
                penWidth = value;
            }
        }

		public Color BrushColor
		{
			get
			{
				return _brushColor;
			}
			set
			{
				_brushColor = value;
			}
		}

		public int Alpha
		{
			get
			{
				return this._alpha;
			}
			set
			{
				this._alpha = value;
			}
		}

		public Color TrackerBrushColor
		{
			get
			{
				return this._brushTracker;
			}
			set
			{
				this._brushTracker = value;
			}
		}

		public Color TrackerPenColor
		{
			get
			{
				return this._penTracker;
			}
			set
			{
				this._penTracker = value;
			}
		}

        /// <summary>
        /// Number of handles
        /// </summary>
        public virtual int HandleCount
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Last used color
        /// </summary>
        public static Color LastUsedColor
        {
            get
            {
                return lastUsedColor;
            }
            set
            {
                lastUsedColor = value;
            }
        }

        /// <summary>
        /// Last used pen width
        /// </summary>
        public static int LastUsedPenWidth
        {
            get
            {
                return lastUsedPenWidth;
            }
            set
            {
                lastUsedPenWidth = value;
            }
        }

		/// <summary>
		/// Last used Brush Color
		/// </summary>
		public static Color LastUsedBrushColor
		{
			get
			{
				return _lastUsedBrushColor;
			}
			set
			{
				_lastUsedBrushColor = value;
			}
		}

		public static int LastUsedAlpha
		{
			get
			{
				return _lastUsedAlpha;
			}
			set
			{
				_lastUsedAlpha = value;
			}
		}

		public virtual RectangleF BoundingRectangle
		{
			get
			{
				return RectangleF.Empty;
			}
		}

        #endregion

		#region constructor and destructor

		public DrawObject()
		{
            Initialized = false;
		}

		public DrawObject(DrawObject obj)
		{
            this._description = obj.Description;
			this._container = obj.Container;
			this._selected = obj.Selected;
			this.color = obj.Color;
			this.penWidth = obj.PenWidth;
			this._brushColor = obj.BrushColor;
			this._brushTracker = obj._brushTracker;
			this._penTracker = obj._penTracker;
			this._alpha = obj._alpha;
            this.Initialized = obj.Initialized;
		}
		
		#endregion

        #region Virtual Functions

        public virtual string GetDrawingType()
        {
            return "Object";
        }

        /// <summary>
        /// Draw object
        /// </summary>
        /// <param name="g"></param>
        public virtual void Draw(Graphics g)
        {
        }

        public virtual GraphicsPath CreateGraphicsPath()
        {
            return new GraphicsPath();
        }

        public virtual void AddToGraphicsPath(GraphicsPath path)
        {
        }

		/// <summary>
		/// Render object into mask 
		/// </summary>
		/// <param name="mask"></param>
		public virtual void Draw(SIA.Common.Mask.IMask mask)
		{

		}

        /// <summary>
        /// Get handle point by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public virtual PointF GetHandle(int handleNumber)
        {
            return new PointF(0, 0);
        }

        /// <summary>
        /// Get handle rectangle by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public virtual RectangleF GetHandleRectangle(int handleNumber)
        {
			PointF point = GetHandle(handleNumber);
            //if (Container.MetroSys != null)
            //    point = Container.MetroSys.ToPixel(point);

            return new RectangleF(point.X - 3, point.Y - 3, 7, 7);
        }

        /// <summary>
        /// Draw tracker for _selected object
        /// </summary>
        /// <param name="g"></param>
        public virtual void DrawTracker(Graphics g)
        {
            if (!Selected)
                return;

			SolidBrush brush = new SolidBrush(this._brushTracker);
			Pen pen = new Pen(this._penTracker, 5);

            CompositingMode bkCompositingMode = g.CompositingMode;
            g.CompositingMode = CompositingMode.SourceOver;

            #region Draw bounding rectangle
            RectangleF rcBound = GetBounds();
			rcBound = DrawRectangle.GetNormalizedRectangle(rcBound);

            if (rcBound.IsEmpty == false)
            {
                GraphicsPath path = new GraphicsPath();
                path.AddRectangle(rcBound);
                if (Container.MetroSys != null)
                    path.Transform(Container.MetroSys.InvTransformer);
                g.DrawPath(pen, path);
                //g.DrawRectangle(pen, rcBound.Left, rcBound.Top, rcBound.Width, rcBound.Height);
            }
            #endregion
			
			for ( int i=1; i <= HandleCount; i++ )
			{
				RectangleF rectHandle = GetHandleRectangle(i);
                g.FillRectangle(brush, rectHandle.X, rectHandle.Y, rectHandle.Width, rectHandle.Height);
				g.DrawRectangle(pen, rectHandle.X, rectHandle.Y, rectHandle.Width, rectHandle.Height);
			}

            g.CompositingMode = bkCompositingMode;

			brush.Dispose();
			pen.Dispose();
		}

        /// <summary>
        /// Hit test.
        /// Return value: -1 - no hit
        ///                0 - hit anywhere
        ///                > 1 - handle number
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual int HitTest(PointF point)
        {
            return -1;
        }


        /// <summary>
        /// Test whether point is inside of the object
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual bool PointInObject(PointF point)
        {
            return false;
        }
        

        /// <summary>
        /// Get cursor for the handle
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public virtual Cursor GetHandleCursor(int handleNumber)
        {
            return Cursors.Default;
        }

        /// <summary>
        /// Test whether object intersects with rectangle
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public virtual bool IntersectsWith(RectangleF rectangle)
        {
            return false;
        }

        /// <summary>
        /// Move object
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        public virtual void Move(float deltaX, float deltaY)
        {
        }

        /// <summary>
        /// Move handle to the point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="handleNumber"></param>
        public virtual void MoveHandleTo(PointF point, int handleNumber)
        {
        }

		public virtual void MoveHandleTo(float dx, float dy, int handleNumber)
		{
		}

        /// <summary>
        /// Dump (for debugging)
        /// </summary>
        public virtual void Dump()
        {
            Trace.WriteLine("");
            Trace.WriteLine(this.GetType().Name);
            Trace.WriteLine("Selected = " + _selected.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Normalize object.
        /// Call this function in the end of object resizing.
        /// </summary>
        public virtual void Normalize()
        {
        }

		/// <summary>
		/// Get bounding rectangle
		/// </summary>
		/// <returns>bounding rectangle</returns>
		public virtual RectangleF GetBounds()
		{
			return RectangleF.Empty;
		}

		/// <summary>
		/// Set last point
		/// </summary>
		/// <param name="pt">point value</param>
		public virtual void SetLastPoint(PointF pt)
		{
		}

		
		/// <summary>
		/// Transform object by a specified matrix
		/// </summary>
		/// <param name="transform">transform matrix</param>
		public virtual void Transform(Matrix transform)
		{
		}

		public virtual DrawObject Copy()
		{
			return null;
		}
		
        /// <summary>
        /// Save object to serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public virtual void SaveToStream(SerializationInfo info, int orderNumber)
        {
            info.AddValue(
                String.Format(CultureInfo.InvariantCulture,
                    "{0}{1}",
                    entryDescription, orderNumber),
                Description);
            info.AddValue(
                String.Format(CultureInfo.InvariantCulture,
                    "{0}{1}",
                    entryColor, orderNumber),
                Color.ToArgb());

            info.AddValue(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryPenWidth, orderNumber),
                PenWidth);

			info.AddValue(
				String.Format(CultureInfo.InvariantCulture,
				"{0}{1}",
				entryBrushColor, orderNumber),
				BrushColor.ToArgb());

			info.AddValue(
				String.Format(CultureInfo.InvariantCulture,
				"{0}{1}",
				entryAlpha, orderNumber),
				Alpha);
        }

        /// <summary>
        /// Load object from serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="orderNumber"></param>
        public virtual void LoadFromStream(SerializationInfo info, int orderNumber)
        {
            this.Description = info.GetString(
                String.Format(CultureInfo.InvariantCulture,
                    "{0}{1}",
                    entryDescription, orderNumber));

            int n = info.GetInt32(
                String.Format(CultureInfo.InvariantCulture,
                    "{0}{1}",
                    entryColor, orderNumber));

            this.Color = Color.FromArgb(n);

            PenWidth = info.GetInt32(
                String.Format(CultureInfo.InvariantCulture,
                "{0}{1}",
                entryPenWidth, orderNumber));

			n = info.GetInt32(
				String.Format(CultureInfo.InvariantCulture,
					"{0}{1}",
					entryBrushColor, orderNumber));
			this.BrushColor = Color.FromArgb(n);

			this.Alpha =  info.GetInt32(String.Format(CultureInfo.InvariantCulture,
				"{0}{1}",
				entryAlpha, orderNumber));

            Initialized = true;
        }

        public virtual void Serialize(BinaryWriter writer)
        {
            writer.Write(Description);
            writer.Write(Color.ToArgb());
            writer.Write(PenWidth);
            writer.Write(BrushColor.ToArgb());
            writer.Write(Alpha);
        }
        public virtual void Deserialize(BinaryReader reader)
        {
            this.Description = reader.ReadString();
            this.Color = Color.FromArgb(reader.ReadInt32());
            PenWidth = reader.ReadInt32();
            this.BrushColor = Color.FromArgb(reader.ReadInt32());
            this.Alpha = reader.ReadInt32();

            Initialized = true;
        }

        #endregion

		#region helper routines
        
		protected void Initialize()
        {
            if (!_initialized)
            {
                _alpha = LastUsedAlpha;
                penWidth = LastUsedPenWidth;
                color = Color.FromArgb(_alpha, LastUsedColor);
                BrushColor = Color.FromArgb(_alpha, LastUsedBrushColor);
            }
        }


        #endregion
		
    }
}
