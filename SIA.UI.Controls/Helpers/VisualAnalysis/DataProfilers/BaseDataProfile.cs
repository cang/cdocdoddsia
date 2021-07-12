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
//CONG using SIA.UI.Controls.Components;
using SIA.UI.Controls.UserControls;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Helpers;
using SIA.UI.Controls.Helpers.VisualAnalysis;
using SIA.UI.Controls.Utilities;

using SIA.UI.Components;


namespace SIA.UI.Controls.Helpers.VisualAnalysis.DataProfilers
{
	/// <summary>
	/// Summary description for BaseDataProfiler.
	/// </summary>
	public abstract class BaseDataProfiler : IDataProfiler
	{
		private DataProfileHelper _container = null;
		protected PointF _begin = PointF.Empty, _end = PointF.Empty, _selected = PointF.Empty;
		protected PointF[] _lastPoints = null;

        public PointF Begin
        {
            get { return _begin; }
            set { _begin = value; }
        }

        public PointF End
        {
            get { return _end; }
            set { _end = value; }
        }

		public Cursor Cursor
		{
			get {return this._container.Cursor;}
			set
			{
                this._container.Cursor = value;
			}
		}

		public BaseDataProfiler(DataProfileHelper container)
		{
			this._container = container;
		}

		~BaseDataProfiler()
		{
			this.Dispose(false);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
		}

		#region IDataProfiler Members

		public DataProfileHelper Container
		{
			get {return _container;}
		}

		public ImageWorkspace Workspace
		{
			get
			{
				if (_container != null)
					return _container.Workspace;
				return null;
			}
		}

		public CommonImage Image
		{
			get
			{
				if (this.Workspace != null)
					return this.Workspace.Image;
				return null;
			}
		}

		public DlgLineProfile2 DlgLineProfile
		{
			get
			{
				if (this._container != null)
					return this._container.DlgLineProfile;
				return null;
			}
		}

		public abstract void Render(Graphics graph, Rectangle rcClip);

		public abstract void MouseDown(MouseEventArgs e);
		public abstract void MouseMove(MouseEventArgs e);
		public abstract void MouseUp(MouseEventArgs e);

		public abstract void InteractiveLine(RasterViewerLineEventArgs e);
		public abstract void UpdateSelectedValue(object abscissaValue, object ordinaryValue);

		public abstract void DisplaySettingsWindow();
		public abstract void Export();
		public abstract void Update();

		#endregion
	}
}
