using System;
using System.Data;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Diagnostics;

using SIA.Common;

using SIA.UI.Controls.Common;
using SIA.SystemFrameworks;

using SIA.UI.Components;
using SIA.UI.Components.Common;
using SIA.UI.Components.Helpers;
using SIA.UI.Components.Renders;

namespace SIA.UI.Controls
{
	/// <summary>
	/// The ImageViewer represents the RDE image viewer which provides built-in functions for 
    /// displaying and user interactive of RDE
	/// </summary>
	public class ImageViewer 
        : AnalysisView
	{
		#region members

		/// <summary>
		/// current mask
		/// </summary>
		private System.Drawing.Image _mask = null;

		/// <summary>
		/// Recent used cursor
		/// </summary>
		private System.Windows.Forms.Cursor _oldCursor = null;

		/// <summary>
		/// Rotation Angle
		/// </summary>
		private float _rotateAngle = 0.0f;

		/// <summary>
		/// Collection of cluster
		/// </summary>
        [Obsolete]
		private ClusterDataCollection _clusterCollection = null;
		
		#endregion

		#region public properties

        /// <summary>
        /// Gets the current cursor
        /// </summary>
		public override Cursor Cursor
		{
			get 
			{ 
				return base.Cursor;
			}
			set
			{
				_oldCursor = base.Cursor;
				base.Cursor = value;
			}
		}

        /// <summary>
        /// Gets the rotation angle 
        /// </summary>
		public float RotateAngle
		{
			get {return _rotateAngle;}
			set 
			{
				if (_rotateAngle != value)
				{
					_rotateAngle = value;
					OnRotateAngleChanged();
				}
			}
		}

        /// <summary>
        /// Gets the mask that is currently used
        /// </summary>
		public System.Drawing.Image Mask
		{
			get 
			{
				return _mask;
			}
			set
			{
				if (_mask != null) _mask.Dispose();
				_mask = value;
				OnMaskImageChanged();
			}
		}

        [Obsolete]
		private ClusterDataCollection Clusters
		{
			get 
			{
				return _clusterCollection;
			}
			set
			{
				_clusterCollection = value;
				OnClusterDataChanged();
			}
		}
		

		protected virtual void OnRotateAngleChanged()
		{
		}

        protected virtual void OnMaskImageChanged()
		{
		}

		protected virtual void OnClusterDataChanged()
		{
		}

		
		#endregion

		#region constructor and destructor

        public ImageViewer()
            : base()
        {
        }

		public ImageViewer(DocumentWorkspace docWorkspace) 
            : base(docWorkspace)
		{
			base.AutoDisposeImages = false;
			base.AutoResetScaleFactor = false;
			base.AutoResetScrollPosition = false;
			
			this.AutoFitGrayScale = false;
			this.RasterImageRender.AutoFitGrayScale = false;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose (disposing);
		}

		#endregion

		#region public operations
		
        
		public void RestoreLastCursor()
		{
			if (_oldCursor != null)
				this.Cursor = _oldCursor;
			_oldCursor = null;
		}


		#endregion

		#region override routines

		
		// override default function for accepting the arrow keys
		protected override bool IsInputKey(Keys keyData)
		{
			bool result = false;

			switch (keyData)
			{
				case Keys.Up:
				case Keys.Down:
				case Keys.Left:
				case Keys.Right:
					result = true;
					break;
				default:
					result = base.IsInputKey (keyData);
					break;
			}
			return result;
		}
		
		#endregion

        [Obsolete]
		protected virtual void DrawClusterData(Graphics graph, Rectangle rectClip)
		{
			if (this._clusterCollection == null)
				return;

			foreach (ClusterData cluster in this._clusterCollection)
			{
				if (this.LogicalViewRectangle.Contains(cluster.Location) == true)
					graph.FillRectangle(Brushes.Red, cluster.Location.X, cluster.Location.Y, 1, 1);
			}
		}

		protected override void OnImageChanged(EventArgs e)
        {
            base.OnImageChanged(e);

            // release mask if image is set to null
            if (this.Image == null)
            {
                if (_mask != null)
                    _mask.Dispose();
                _mask = null;
            }
        }

		
	}
}
