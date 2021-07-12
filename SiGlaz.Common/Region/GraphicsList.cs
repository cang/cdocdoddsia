using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Globalization;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Remoting;
using System.Windows.Forms;

using SiGlaz.Common.ImageAlignment;

namespace SiGlaz.Common
{
	/// <summary>
	/// List of graphic objects
	/// </summary>
    [Serializable]
    public class GraphicsList : ISerializable
    {
		#region member attributes
		// mask editor reference
		private IMaskEditor _maskEditor = null;

        private MetrologySystem _metrosys = null;

        public MetrologySystem MetroSys
        {
            get { return _metrosys; }
            set { _metrosys = value; }
        }

		// graphics object collection
		private ArrayList _graphicsList;
		#endregion

		#region internal constants

		// entry for serialization
		private const string entryFileID = "FileID";
		private const string entryFileVersion_Major = "Major";
		private const string entryFileVersion_Minor = "Minor";
		private const string entryCount = "Count";
		private const string entryType = "Type";

		private const string valueFileID = "SiGlaz System Region File";
		private const string valueFileVersion_Major = "3";
		private const string valueFileVersion_Minor = "0";

		#endregion
		
		#region Event Handlers

		public event EventHandler ItemsChanged = null;
		public event EventHandler SelectItemsChanged = null;
		
		#endregion

		#region public properties

		public IMaskEditor MaskEditor
		{
			get {return _maskEditor;}
			set
			{
				_maskEditor = value;
				OnMaskEditorChanged();
			}
		}

		protected virtual void OnMaskEditorChanged()
		{
		}


		public int Count
		{
			get
			{
				return _graphicsList.Count;
			}
		}

		public DrawObject this [int index]
		{
			get
			{
				if ( index < 0  ||  index >= _graphicsList.Count )
					return null;

				return ((DrawObject)_graphicsList[index]);
			}
		}

		public int SelectionCount
		{
			get
			{
				int n = 0;

				foreach (DrawObject o in _graphicsList)
				{
					if ( o.Selected )
						n++;
				}

				return n;
			}
		}

		public ArrayList Items
		{
			get
			{
				return this._graphicsList;
			}
		}

		
		#endregion

		#region constructor and destructor
		public GraphicsList()
		{
			_graphicsList = new ArrayList();
		}

		protected GraphicsList(SerializationInfo info, StreamingContext context)
		{
			string fileID = (string)info.GetValue(entryFileID, typeof(string));
			if (fileID != valueFileID)
				throw new SerializationException("Invalid Region File Format");
			string fileVersion_Major = (string)info.GetValue(entryFileVersion_Major, typeof(string));
			string fileVersion_Minor = (string)info.GetValue(entryFileVersion_Minor, typeof(string));
			if (fileVersion_Major != valueFileVersion_Major || 
				fileVersion_Minor != valueFileVersion_Minor)
				throw new SerializationException(String.Format("Version {0}.{1} is not supported", fileVersion_Major, fileVersion_Minor));
				
			
			_graphicsList = new ArrayList();
			int n = info.GetInt32(entryCount);
			string typeName;
			object drawObject = null;

			for ( int i = 0; i < n; i++ )
			{
				typeName = info.GetString(
					String.Format(CultureInfo.InvariantCulture,
					"{0}{1}",
					entryType, i));

				// BUG Fixed !!!
				//Debug.Assert(false, "Debug serialize GraphicsList");
                //drawObject = Assembly.GetExecutingAssembly().CreateInstance(typeName);
                //drawObject = Activator.CreateInstance(assemblyFile, typeName);

				try
				{
					drawObject = Assembly.GetExecutingAssembly().CreateInstance(typeName);
				}
				catch {}

				if (drawObject == null)
				{
					try
					{

						Assembly assembly = Assembly.GetExecutingAssembly();
						string assemblyFile = Path.GetDirectoryName(assembly.Location) + @"\SIA.UI.MaskEditor.dll";
						ObjectHandle objHandle = Activator.CreateInstanceFrom(assemblyFile, typeName);
						if (objHandle == null)
							throw new Exception("Cannot instaniate the drawObject: " + typeName);
                
						drawObject = objHandle.Unwrap();
					}
					catch {}
				}

				((DrawObject)drawObject).LoadFromStream(info, i);
				((DrawObject)drawObject).Container = this;
				_graphicsList.Add(drawObject);
			}

		}

		[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter=true)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(entryFileID, valueFileID);
            info.AddValue(entryFileVersion_Major, valueFileVersion_Major);
			info.AddValue(entryFileVersion_Minor, valueFileVersion_Minor);
			info.AddValue(entryCount, _graphicsList.Count);

			int i = 0;

			foreach ( DrawObject o in _graphicsList )
			{
				info.AddValue(
					String.Format(CultureInfo.InvariantCulture,
					"{0}{1}",
					entryType, i),
					o.GetType().FullName);

				o.SaveToStream(info, i);

				i++;
			}
		}

		#endregion

		#region public routines
		
		public void Draw(Graphics g)
		{
			int iUpperBound = _graphicsList.Count-1;
			for (int i=iUpperBound; i>=0; i--)
			{
				DrawObject obj = (DrawObject)_graphicsList[i];
				if (obj != null)
				{
					if (obj.Selected == true)
						obj.DrawTracker(g);

                    obj.Draw(g);
				}
			}
		}

        public GraphicsPath CreateGraphicsPath()
        {
            GraphicsPath result = new GraphicsPath();

            foreach (DrawObject obj in _graphicsList)
            {
                using (GraphicsPath path = obj.CreateGraphicsPath())
                {
                    if (path != null)
                        result.AddPath(path, false);
                }
            }

            return result;
        }

		public virtual GraphicsList Clone()
		{
            GraphicsList result = new GraphicsList();
            result.MaskEditor = this.MaskEditor;
            result.MetroSys = this.MetroSys;
			int iCount = _graphicsList.Count;
			for (int i=0; i<iCount; i++)
			{
				DrawObject obj = (DrawObject)_graphicsList[i];
				result.Add(obj.Copy());
			}
			return result;
		}
        
		public DrawObject GetSelectedObject(int index)
		{
			int n = -1;

			foreach (DrawObject o in _graphicsList)
			{
				if ( o.Selected )
				{
					n++;

					if ( n == index )
						return o;
				}
			}

			return null;
		}

        public bool PointInRegions(PointF pt)
        {
            foreach (DrawObject o in _graphicsList)
            {
                if (o.PointInObject(pt))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Get the graphics path of graphic list.
        /// Please dispose after calling.
        /// </summary>
        /// <returns> A GraphicsPath. </returns>
        public GraphicsPath GetGraphicsPath()
        {
            GraphicsPath path = new GraphicsPath();
            foreach (DrawObject o in _graphicsList)
            {
                o.AddToGraphicsPath(path);
            }
            if (MetroSys != null)
                path.Transform(MetroSys.InvTransformer);

            return path;
        }

        public unsafe void CheckInRegions(bool* buffer, int w, int h)
        {
            using (GraphicsPath path = GetGraphicsPath())
            {
                using (Region region = new Region(path)) //Region better then GraphicsPath :))
                {
                    int ibuff = 0;
                    for (int y = 0; y < h; y++)
                        for (int x = 0; x < w; x++)
                        {
                            if (buffer[ibuff])
                            {
                                if (!region.IsVisible(x, y))
                                    buffer[ibuff] = false;
                            }
                            ibuff++;
                        }
                }
            }

            //GraphicsPath path = new GraphicsPath();
            //foreach (DrawObject o in _graphicsList)
            //{
            //    o.AddToGraphicsPath(path);
            //}

            //if (MetroSys != null)
            //    path.Transform(MetroSys.InvTransformer);

            //int nThreads = System.Environment.ProcessorCount;
            //GraphicsPath[] paths = new GraphicsPath[nThreads];
            //paths[0] = path;
            //for (int iThread = 1; iThread < nThreads; iThread++)
            //{
            //    paths[iThread] = path.Clone() as GraphicsPath;
            //}

            ////for (int y = 0; y < h; y++)
            //SIA.SystemFrameworks.Parallel.For(0, nThreads, delegate(int iThread)
            //{
            //    int yStart = iThread * (h / nThreads);
            //    int yEnd = yStart + (h / nThreads) - 1;
            //    if (iThread == nThreads - 1)
            //        yEnd = h - 1;

            //    for (int y = yStart; yStart <= yEnd; yStart++)
            //    {
            //        int ibuff = y * w;
            //        for (int x = 0; x < w; x++)
            //        {
            //            if (buffer[ibuff])
            //            {
            //                if (!path.IsVisible(x, y))
            //                    buffer[ibuff] = false;
            //            }
            //            ibuff++;
            //        }
            //    }
            //}
            //);

            //for (int iThread = 0; iThread < nThreads; iThread++)
            //{
            //    if (paths[iThread] != null)
            //    {
            //        paths[iThread].Dispose();
            //        paths[iThread] = null;
            //    }
            //}
            //paths = null;

            //path.Dispose();
        }

        #endregion

        #region Items Management Hanlder

        public virtual void Add(DrawObject obj)
        {
			if (obj == null)
				throw new System.ArgumentNullException("Invalid parameter");
			
			// create relationship
			obj.Container = this;

            // insert to the top of z-order
            _graphicsList.Insert(0, obj);

			// fire event if any
			OnItemChanged();
        }

		public virtual void Remove(DrawObject obj)
		{
			// remove select object
			if (obj != null)
				_graphicsList.Remove(obj);

			// fire event if any
			OnItemChanged();
		}

		public virtual bool Clear()
		{
			bool result = (_graphicsList.Count > 0);
			_graphicsList.Clear();
			
			// fire event if any
			OnItemChanged();

			return result;
		}


		#endregion

		#region Selection Handlers

        public void SelectInRectangle(RectangleF rectangle)
        {
            UnselectAll();

            foreach (DrawObject o in _graphicsList)
            {
                if ( o.IntersectsWith(rectangle) )
                    o.Selected = true;
            }
        }

        public void UnselectAll()
        {
            foreach (DrawObject o in _graphicsList)
            {
                o.Selected = false;
            }
        }

        public void SelectAll()
        {
            foreach (DrawObject o in _graphicsList)
            {
                o.Selected = true;
            }
        }

		public bool DeleteSelection()
        {
            bool result = false; 

            int n = _graphicsList.Count;

            for ( int i = n-1; i >= 0; i-- )
            {
                if ( ((DrawObject)_graphicsList[i]).Selected )
                {
                    this.Remove((DrawObject)_graphicsList[i]);
                    result = true;
                }
            }

			if (result)
				OnItemChanged();

            return result;
        }


        public bool MoveSelectionToFront()
        {
            int n;
            int i;
            ArrayList tempList;

            tempList = new ArrayList();
            n = _graphicsList.Count;

            // Read source list in reverse order, add every selected item
            // to temporary list and remove it from source list
            for ( i = n - 1; i >= 0; i-- )
            {
                if ( ((DrawObject)_graphicsList[i]).Selected )
                {
                    tempList.Add(_graphicsList[i]);
                    _graphicsList.RemoveAt(i);
                }
            }

            // Read temporary list in direct order and insert every item
            // to the beginning of the source list
            n = tempList.Count;

            for ( i = 0; i < n; i++ )
            {
                _graphicsList.Insert(0, tempList[i]);
            }

            return ( n > 0 );
        }

        public bool MoveSelectionToBack()
        {
            int n;
            int i;
            ArrayList tempList;

            tempList = new ArrayList();
            n = _graphicsList.Count;

            // Read source list in reverse order, add every selected item
            // to temporary list and remove it from source list
            for ( i = n - 1; i >= 0; i-- )
            {
                if ( ((DrawObject)_graphicsList[i]).Selected )
                {
                    tempList.Add(_graphicsList[i]);
                    _graphicsList.RemoveAt(i);
                }
            }

            // Read temporary list in reverse order and add every item
            // to the end of the source list
            n = tempList.Count;

            for ( i = n - 1; i >= 0; i-- )
            {
                _graphicsList.Add(tempList[i]);
            }

            return ( n > 0 );
        }


		#endregion

		#region Graphics Object Properties Handlers
        
        private GraphicsProperties GetProperties()
        {
            GraphicsProperties properties = new GraphicsProperties();

            int n = SelectionCount;

            if ( n < 1 )
                return properties;

            DrawObject o = GetSelectedObject(0);

            int firstColor = o.Color.ToArgb();
            int firstPenWidth = o.PenWidth;

            bool allColorsAreEqual = true;
            bool allWidthAreEqual = true;

            for ( int i = 1; i < n; i++ )
            {
                if ( GetSelectedObject(i).Color.ToArgb() != firstColor )
                    allColorsAreEqual = false;

                if ( GetSelectedObject(i).PenWidth != firstPenWidth )
                    allWidthAreEqual = false;
            }

            if ( allColorsAreEqual )
            {
                properties.ColorDefined = true;
                properties.Color = Color.FromArgb(firstColor);
            }

            if ( allWidthAreEqual )
            {
                properties.PenWidthDefined = true;
                properties.PenWidth = firstPenWidth;
            }

            return properties;
        }

        private void ApplyProperties(GraphicsProperties properties)
        {
            foreach ( DrawObject o in _graphicsList )
            {
                if ( o.Selected )
                {
                    if ( properties.ColorDefined )
                    {
                        o.Color = properties.Color;
                        DrawObject.LastUsedColor = properties.Color;
                    }

                    if ( properties.PenWidthDefined )
                    {
                        o.PenWidth = properties.PenWidth;
                        DrawObject.LastUsedPenWidth = properties.PenWidth;
                    }
                }
            }
        }

        public bool ShowPropertiesDialog(IWin32Window parent)
        {
            if ( SelectionCount < 1 )
                return false;
            return true;
        }

		
		#endregion

		#region Transform Handlers

		public void Transform(Matrix transform)
		{
			foreach (DrawObject obj in this._graphicsList)
				obj.Transform(transform);
		}

		public void TranslateSelection(int dx, int dy)
		{
			ArrayList selectedObjects = new ArrayList();
			foreach (DrawObject obj in _graphicsList)
				if (obj.Selected) selectedObjects.Add(obj);
			
			if (selectedObjects.Count > 0)
			{
				Matrix matTransform = new Matrix();
				matTransform.Translate(dx, dy);
				
				foreach(DrawObject o in selectedObjects)
					o.Transform(matTransform);
				matTransform.Dispose();
			}
		}
		
		
		#endregion

		#region Event Handlers

		private void OnItemChanged()
		{
			if (this.ItemsChanged != null)
				this.ItemsChanged(this, new System.EventArgs());
		}

		private void OnSelectItemsChanged()
		{
			if (this.SelectItemsChanged!= null)
				this.SelectItemsChanged(this, new System.EventArgs());
		}
		
		#endregion

        #region Serialize and deserialize
        public void Serialize(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                Serialize(fs);
            }
        }

        public void Serialize(FileStream fs)
        {
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                Serialize(writer);
            }
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(valueFileID);
            writer.Write(valueFileVersion_Major);
            writer.Write(valueFileVersion_Minor);
            writer.Write(_graphicsList.Count);

            foreach (DrawObject o in _graphicsList)
            {
                writer.Write(o.GetType().FullName);
                o.Serialize(writer);
            }
        }

        public static GraphicsList Deserialize(string fileName)
        {
            GraphicsList gl = null;

            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                gl = Deserialize(fs);
            }

            return gl;
        }

        public static GraphicsList Deserialize(FileStream fs)
        {
            GraphicsList gl = null;

            using (BinaryReader reader = new BinaryReader(fs))
            {
                gl = Deserialize(reader);
            }

            return gl;
        }

        public static GraphicsList Deserialize(BinaryReader reader)
        {
            GraphicsList graphicslist = new GraphicsList();
            string fileID = reader.ReadString();
            if (fileID != valueFileID)
                throw new SerializationException("Invalid Region File Format");
            string fileVersion_Major = reader.ReadString();
            string fileVersion_Minor = reader.ReadString();
            if (fileVersion_Major != valueFileVersion_Major ||
                fileVersion_Minor != valueFileVersion_Minor)
                throw new SerializationException(String.Format("Version {0}.{1} is not supported", fileVersion_Major, fileVersion_Minor));

            graphicslist._graphicsList = new ArrayList();
            int n = reader.ReadInt32();
            string typeName;
            object drawObject = null;

            for (int i = 0; i < n; i++)
            {
                typeName = reader.ReadString();

                try
                {
                    drawObject = Assembly.GetExecutingAssembly().CreateInstance(typeName);
                }
                catch { }

                if (drawObject == null)
                {
                    try
                    {
                        Assembly assembly = Assembly.GetExecutingAssembly();
                        string assemblyFile = Path.GetDirectoryName(assembly.Location) + @"\SIA.UI.MaskEditor.dll";
                        ObjectHandle objHandle = Activator.CreateInstanceFrom(assemblyFile, typeName);
                        if (objHandle == null)
                            throw new Exception("Cannot instaniate the drawObject: " + typeName);

                        drawObject = objHandle.Unwrap();
                    }
                    catch { }
                }

                ((DrawObject)drawObject).Deserialize(reader);
                ((DrawObject)drawObject).Container = graphicslist;
                graphicslist._graphicsList.Add(drawObject);
            }
            return graphicslist;
        }

        public byte[] ToBytes()
        {
            byte[] bytes = null;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (BinaryWriter writer = new BinaryWriter(ms))
                    {
                        Serialize(writer);
                    }

                    bytes = ms.ToArray();
                }
            }
            catch (System.Exception exp)
            {
                bytes = null;

                throw exp;
            }
            finally
            {
            }

            return bytes;
        }

        public static GraphicsList FromBytes(byte[] bytes)
        {
            GraphicsList gl = null;

            try
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    using (BinaryReader reader = new BinaryReader(ms))
                    {
                        gl = GraphicsList.Deserialize(reader);
                    }
                }
            }
            catch (System.Exception exp)
            {
                gl = null;
                throw exp;
            }
            finally
            {
            }

            return gl;
        }

        #endregion Serialize and deserialize
    }
}
