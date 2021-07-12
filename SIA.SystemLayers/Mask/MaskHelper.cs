//#define TRACE_IMAGE

using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.IO;

using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

using SIA.Common;
using SIA.Common.KlarfExport;
using SIA.Common.Mask;

using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;

using SIA.SystemLayer;
using SiGlaz.RDE.Ex.Mask;

using SIA.IPEngine;
using SIA.IPEngine.KlarfExport;

namespace SIA.SystemLayer.Mask
{	
	/// <summary>
	/// The MaskHelper class provides functions for creation, serialization and drawing the mask data.
    /// The mask is used for object detection and created by the Mask Editor.
    /// The MaskHelper class use the quarternion technique for rendering vectorized objects into huge raster image.
	/// </summary>
	public class MaskHelper : System.IDisposable
	{
		#region member attributes
		
		private SIA.SystemLayer.CommonImage _owner;
		private bool _bAutoDisposeUserData = true;
		private QuadTreeNode _rootNode = null;
		private ArrayList _leafNodes = null;

		#endregion

		#region constructor and destructor

		public MaskHelper(CommonImage owner)
		{
			_owner = owner;
			Size size = MakeQuadAlignment(new Size(_owner.Width, _owner.Height));
			_rootNode = new QuadTreeNode(new Rectangle(Point.Empty, size), null);
		}

		public MaskHelper(Rectangle rect)
		{
			_owner = null;
			Size size = MakeQuadAlignment(rect.Size);
			_rootNode = new QuadTreeNode(new Rectangle(Point.Empty, size), null);
		}

		
		#region IDisposable Members
		public void Dispose()
		{
			DestroyQuadTree();
		}

		#endregion

		#endregion

		#region public properties

		public bool AutoDisposeUserData
		{
			get {return _bAutoDisposeUserData;}
			set
			{
				_bAutoDisposeUserData = value;
				OnAutoDisposeUserDataChanged();
			}
		}

		protected virtual void OnAutoDisposeUserDataChanged()
		{
		}

		public CommonImage Owner
		{
			get {return _owner;}
		}

		internal QuadTreeNode[] LeafNodes
		{
			get
			{
				return (QuadTreeNode[])_leafNodes.ToArray(typeof(QuadTreeNode));
			}
		}

		#endregion

		#region public operations

		public IMask CreateMask(string filename)
		{
			GraphicsList objects = this.LoadMask(filename);
			return CreateMask(objects);
		}

		public IMask CreateMask(GraphicsList objects)
		{
			const int PREFER_SIZE = 1024;//512;
			return CreateMask(objects, PREFER_SIZE);
		}

		public IMask CreateMask(GraphicsList objects, int cellSize)
		{
			if (objects == null)
				throw new System.ArgumentNullException();
			if (objects.Count == 0)
				throw new System.ArgumentException("Invalid parameter");

			IMask result = null;

			try
			{
				Size image_size = new Size(_owner.Width, _owner.Height);
				Size mask_size = MakeQuadAlignment(image_size);
				// calculate tree's depth 
				int depth = 1;
				int max_size = Math.Max(mask_size.Width, mask_size.Height);
				int iter = max_size + max_size%cellSize;
				while (iter > cellSize)
				{
					iter = iter / 2;
					depth++;
				}

				// release old quad-tree
				this.DestroyQuadTree();

				// create new quad-tree
				this._rootNode = new QuadTreeNode(new Rectangle(Point.Empty, new Size(max_size, max_size)), null);
				this.CreateQuadTree(depth);

				// create mask data
				SplitMask mask = new SplitMask(max_size, max_size);
				this.CreateMaskData(objects, mask);
#if TRACE_IMAGE
				mask.ExportImage(@"C:\\mask_image_result.jpg");
#endif
				// initialize mask vector objects
				mask.GraphicsList = objects;

				// successful
				result = mask;
			}
			catch (System.Exception exp)
			{
				this.DestroyQuadTree();

				if (result != null)
				{
					result.Dispose();
					result = null;
				}

				throw exp;
			}
			finally
			{
				
			}

			return result;
		}

        #endregion
		
		#region internal helpers

		private void CreateQuadTree(int depthLevel)
		{
			_rootNode.Generate(depthLevel-1);
			// retrieve leaf nodes
			_leafNodes = _rootNode.GetLeafNodes();
			_leafNodes.TrimToSize();
		}

		private void DestroyQuadTree()
		{
			if (_rootNode != null)
			{	
				_rootNode.Dispose(_bAutoDisposeUserData);
				_rootNode = null;				
			}

			_leafNodes = null;
		}

		private void CreateMaskData(GraphicsList objects, SplitMask mask)
		{
			if (this._rootNode == null)
				throw new System.Exception("Invalid operation");

			if (this._leafNodes == null)
				this._leafNodes = this._rootNode.GetLeafNodes();
#if TRACE_IMAGE
			int index = 0;
#endif
			// update callback status
			int current = 0;
			int total = _leafNodes.Count;
			CommandProgress.SetText("Creating mask...");
			CommandProgress.SetRange(0, 100);
			CommandProgress.StepTo(0);

			try
			{
				foreach (QuadTreeNode node in this._leafNodes)
				{
					Bitmap image = null;
					try
					{
						image = (Bitmap)CreateSubMask(node, objects);
						mask.Add(node.Rectangle, image);
#if TRACE_IMAGE
					image.Save(@"C:\" + (index++).ToString() + ".jpg", ImageFormat.Jpeg);
#endif
					}
					catch(System.Exception exp)
					{
						throw exp;
					}
					finally
					{
						if (image != null)
						{
							image.Dispose();
							image = null;
						}
					}

					// update callback status
					CommandProgress.StepTo((int)(current++*100.0F/total));
					// check for aborting
					if (CommandProgress.IsAborting)
						throw new CommandAbortedException();
				}
			}
			catch
			{
				throw;
			}
			finally
			{
			}
		}

		private Image CreateSubMask(QuadTreeNode node, GraphicsList objects)
		{
			if (node.IsLeaf == false)
				throw new System.ArgumentException("invalid parameter");
			
			Rectangle rect = node.Rectangle;
			int width = rect.Width;
			int height = rect.Height;
			int offsetX = -rect.X;
			int offsetY = -rect.Y;

			Bitmap bitmap = null;
			try
			{
				bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
				using (Graphics graph = Graphics.FromImage(bitmap))
				{
					graph.TranslateTransform(offsetX, offsetY, MatrixOrder.Append);
					for (int i=0; i<objects.Count; i++)
						objects[i].Draw(graph);
				}
			}
			catch(System.Exception exp)
			{
				if (bitmap != null)
				{
					bitmap.Dispose();
					bitmap = null;
				}

				throw exp;
			}
			finally
			{
			}

			return bitmap;
		}
		
		private Size MakeQuadAlignment(Size size)
		{
			int width = size.Width + size.Width%4;
			int height = size.Height + size.Height%4;
			return new Size(width, height);
		}

		private GraphicsList LoadMask(Stream stream)
		{
			GraphicsList objects = null;
			try
			{
				BinaryFormatter formatter = new BinaryFormatter();
				objects = OnLoadMask(stream, formatter);
			}
			catch(System.Exception exp)
			{
				throw new System.Exception(string.Format("Invalid mask: {0} - {1}", exp.Message, exp.StackTrace), exp);
			}
			finally
			{
			}
			return objects;
		}

		public GraphicsList LoadMask(string filepath)
		{
			FileStream stream = null;
			try
			{
				stream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
				return LoadMask(stream);
			}
			catch(System.Exception exp)
			{
				throw new System.Exception("Invalid mask file \"" + filepath + "\".", exp);
			}
			finally
			{
				if (stream != null)
					stream.Close();
			}
		}
		public GraphicsList LoadMask(byte[] maskData)
		{
			if (maskData == null)
				return null;
			try
			{
				using (MemoryStream stream = new MemoryStream(maskData, 0, maskData.Length))
					return LoadMask(stream);
			}
			catch(System.Exception exp)
			{
				throw new System.Exception(string.Format("Invalid mask data: {0} - {1}", exp.Message, exp.StackTrace), exp);
			}
		}

		private void SaveMask(GraphicsList objects, Stream stream)
		{
			try
			{
				BinaryFormatter formatter = new BinaryFormatter();
				OnSaveMask(objects, stream, formatter);
			}
			catch(System.Exception exp)
			{
				throw exp;
			}
		}

		private void SaveMask(GraphicsList objects, string filepath)
		{
			try
			{
				using (FileStream stream = new FileStream(filepath, FileMode.Create, FileAccess.Write))
					SaveMask(objects, stream);
			}
			catch
			{
				throw;
			}
		}

		public byte[] SerializeMaskToBin(GraphicsList objects)
		{
			try
			{
				using (MemoryStream stream = new MemoryStream())
				{
					SaveMask(objects, stream);
					return stream.ToArray();
				}
			}
			catch(System.Exception exp)
			{
				throw exp;
			}
		}

		public byte[] SerializeMaskToBin(string filename)
		{
			try
			{
				GraphicsList graphList = LoadMask(filename);
				return SerializeMaskToBin(graphList);
			}
			catch
			{
				throw;
			}
		}

		private GraphicsList OnLoadMask(Stream stream, BinaryFormatter formatter)
		{
			GraphicsList loadObjects = null;
			try
			{
				formatter.Binder = new VersionBinder();
				loadObjects = (GraphicsList)formatter.Deserialize(stream);
			}
			catch (System.Exception exp)
			{
				throw exp;
			}

			return loadObjects;
		}

		private void OnSaveMask(GraphicsList objects, Stream stream, BinaryFormatter formatter)
		{
			if (objects == null || stream == null || formatter == null)
				throw new System.ArgumentException("Invalid Parameters");

			try
			{
				formatter.Serialize(stream, objects);
			}
			catch (System.Exception exp)
			{
				throw exp;
			}
		}

		#endregion
		
	}
}
