using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace SIA.SystemLayer.Mask
{
	internal enum QuadTreeDirection : int
	{
		TopLeft = 0,
		TopRight = 1,
		BottomRight = 2,
		BottomLeft = 3,
		NumDirection
	}

	internal class QuadTreeNode : System.IDisposable
	{
		private int _level = 0;
		private Rectangle _rect = Rectangle.Empty;
		private QuadTreeNode _parent = null;
		private QuadTreeNode[] _childs = null;
		private System.Object _userData = null;

		public int Level
		{
			get {return _level;}
		}

		public Rectangle Rectangle
		{
			get {return _rect;}
		}

		public QuadTreeNode Parent
		{
			get {return _parent;}
		}

		public QuadTreeNode[] Childs
		{
			get {return _childs;}
		}

		public System.Object UserData
		{
			get {return _userData;}
			set
			{
				_userData = value;
				OnUserDataChanged();
			}
		}

		protected virtual void OnUserDataChanged()
		{
		}

		public bool IsRoot
		{
			get {return _level == 0 && _parent == null;}
		}

		public bool IsLeaf
		{
			get {return _childs == null;}
		}

		public QuadTreeNode()
		{			
		}

		public QuadTreeNode(Rectangle rect, QuadTreeNode parent)
		{
			_parent = parent;
			_rect = rect;
		}

		public void Dispose()
		{
			this._userData = null;
			
			if (this._childs != null)
			{
				foreach(QuadTreeNode child in this._childs)
				{
					if (child != null)
						child.Dispose();
				}
			}

			this._parent = null;
			this._childs = null;
		}

		public void Dispose(bool disposeUserData)
		{
			if (this._userData != null)
			{
				if (this._userData is IDisposable)
				{
					try
					{
						((IDisposable)this._userData).Dispose();
					}
					catch(System.Exception exp)
					{
						Trace.WriteLine(exp);
					}
				}
				this._userData = null;
			}
			
			if (this._childs != null)
			{
				foreach(QuadTreeNode child in this._childs)
				{
					if (child != null)
						child.Dispose();
				}
			}

			this._parent = null;
			this._childs = null;
		}

		public ArrayList GetLeafNodes()
		{
			return this.GetLeafNodes(this);
		}

		private ArrayList GetLeafNodes(QuadTreeNode node)
		{
			ArrayList result = new ArrayList();
			ArrayList parents = new ArrayList();
			ArrayList child_index = new ArrayList();
			parents.Add(this);
			child_index.Add(0);

			while (parents.Count > 0)
			{
				int i=0;
				QuadTreeNode parent = (QuadTreeNode)parents[parents.Count-1];
				if (parent.Childs == null)
				{
					result.Add(parent);
					parents.Remove(parent);
				}
				else  
				{
					QuadTreeNode[] childs = parent._childs;
					for (i=(int)(child_index[child_index.Count-1]); i<childs.Length; i++)
					{
						QuadTreeNode child = childs[i];
						if (child.IsLeaf)
							result.Add(child);
						else
						{
							child_index[child_index.Count-1] = (int)i+1;
							parents.Add(child);
							child_index.Add(0);
							break;
						}
					}

					if (i == childs.Length)
					{
						parents.Remove(parent);
						child_index.RemoveAt(child_index.Count-1);
					}
				}
			}

			return result;
		}

		public void Generate(int depthLevel)
		{
			if (depthLevel > 0)
			{
				this._childs = new QuadTreeNode[(int)QuadTreeDirection.NumDirection];

				Point loc = _rect.Location;
				Size size = _rect.Size;
				int child_width = size.Width / 2;
				int child_height = size.Height / 2;

				if (child_width > 0 && child_height > 0)
				{
					this._childs[(int)QuadTreeDirection.TopLeft]	= new QuadTreeNode(new Rectangle(loc.X, loc.Y, child_width, child_height), this);
					this._childs[(int)QuadTreeDirection.TopRight]	= new QuadTreeNode(new Rectangle(loc.X + child_width, loc.Y, size.Width - child_width, child_height), this);
					this._childs[(int)QuadTreeDirection.BottomLeft] = new QuadTreeNode(new Rectangle(loc.X, loc.Y + child_height, child_width, size.Height - child_height), this);
					this._childs[(int)QuadTreeDirection.BottomRight]= new QuadTreeNode(new Rectangle(loc.X + child_width, loc.Y + child_height, size.Width - child_width, size.Height - child_height), this);
					
					foreach(QuadTreeNode child in this._childs)
					{
						child._level = this._level + 1;
						child.Generate(depthLevel-1);
					}	
				}				
			}
		}
	}
	

}
