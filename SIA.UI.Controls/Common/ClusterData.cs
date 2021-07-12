using System;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SIA.UI.Controls.Common
{
	/// <summary>
	/// Summary description for ClusterData.
	/// </summary>
	public class ClusterData
	{
		private Point _location = Point.Empty;
		private Color _color = Color.White;

		public Point Location
		{
			get {return _location;}
			set {_location = value;}
		}

		public Color Color
		{
			get {return _color;}
			set {_color = value;}
		}

		public ClusterData(int x, int y, Color color)
		{
			_location = new Point(x, y);
			_color = color;
		}
	}

	public class ClusterDataCollection : System.Collections.CollectionBase
	{
		public int Add(ClusterData data)
		{
			int index = base.List.Add(data);			
			return index;
		}

		public ClusterData this[int index]
		{
			get {return (ClusterData)base.List[index];}
		}

	}
}
