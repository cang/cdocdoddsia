using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

using System.Diagnostics;

using SIA.Common;
using SIA.Common.Mask;
using SIA.Common.KlarfExport;

using SIA.SystemFrameworks;

using SIA.SystemLayer;
using SiGlaz.RDE.Ex.Mask;

using SIA.UI.Components.Helpers;

namespace SIA.UI.Controls.Helpers.VisualAnalysis
{
	/// <summary>
	/// Summary description for MaskViewer.
	/// </summary>
	public class MaskViewer : BaseVisualAnalyzer
	{
		#region member attribute

		#region visualization attributes

		private SIA.Common.Mask.IMask _mask = null;
		private Matrix _transform = new Matrix();
		private float _rotateAngle = 0.0F;
		private float _scaleFactor = 1.0F;

		#endregion

		#endregion

		#region constructor and destructor
		
		public MaskViewer(SIA.UI.Controls.ImageWorkspace owner) : base(owner)
		{
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose (disposing);

			if (_transform != null)
			{
				_transform.Dispose();
				_transform = null;
			}
		}


		#endregion

		#region public properties
		
        public bool IsMaskAvailable
		{
			get {return this.Workspace != null && this.Workspace.Image != null && this.Workspace.Image.Mask != null;}
		}

		#region Visualization Helpers

		public float RotateAngle
		{
			get {return _rotateAngle;}
			set
			{
				_rotateAngle = value;
				OnRotateAngleChanged();
			}
		}

		protected virtual void OnRotateAngleChanged()
		{
			CalculateTransform();
		}

		public float ScaleFactor
		{
			get {return _scaleFactor;}
			set
			{
				_scaleFactor = value;
				OnScaleFactorChanged();
			}
		}

		protected virtual void OnScaleFactorChanged()
		{
			CalculateTransform();
		}

		#endregion
		
		#endregion

		#region override routines
		
		protected override void OnVisibleChanged()
		{
			base.OnVisibleChanged ();

			if (this.Visible == false)
				this._mask = null;
			else
			{
				if (this.Workspace != null && this.Workspace.Image != null && this.Workspace.Image.Mask != null)
				{
					IMask mask = null;
					try
					{
						mask = this.Workspace.Image.Mask;
						InternalRefreshData(mask);
					}
					catch(System.Exception exp)
					{
						mask = null;
						Trace.WriteLine(exp);
					}
					finally
					{
						this._mask = mask;
					}
				}
			}

			if (this.Workspace != null && this.Workspace.ImageViewer != null)
				this.Workspace.ImageViewer.Invalidate(true);
		}


		#endregion

		#region operation routines
		
		public override void Render(Graphics graph, Rectangle rcClip)
		{
			if (this.IsMaskAvailable)
			{
				GraphicsState state = graph.Save();
				try
				{
					IMask mask = this._mask;
					if (mask.GraphicsList != null && mask.GraphicsList is GraphicsList)
					{
						Matrix transform = this.Workspace.ImageViewer.Transform;
						GraphicsList objects = (GraphicsList)((GraphicsList)mask.GraphicsList).Clone();
						objects.Transform(transform);
						
						for (int i=0; i<objects.Count; i++)
						{
							objects[i].Draw(graph);
						}
					}
				}
				catch(System.Exception exp)
				{
					Trace.WriteLine(exp);
				}
				finally
				{
					graph.Flush();
					graph.Restore(state);
				}
			}
		}

		#endregion
	
		#region internal helpers

		private void InternalResetData()
		{
			_rotateAngle = 0.0F;
			_scaleFactor = 1.0F;
		}

		private void InternalRefreshData(SIA.Common.Mask.IMask mask)
		{
			// reset internal data
			InternalResetData();
			
			// re-calculate transform
			this.CalculateTransform();
		}
		
		private void CalculateTransform()
		{
			if (this.Workspace == null)
				return ;

			try
			{
				this.Workspace.ImageViewer.BeginUpdate();

				if (_transform != null)
					_transform.Dispose();
				_transform = new Matrix();

				SizeF imageSize = this.Workspace.ImageViewer.ImageSize;
				PointF ptCenter = new PointF(imageSize.Width/2, imageSize.Height/2);				
				float offsetX = ptCenter.X / _scaleFactor;
				float offsetY = ptCenter.Y / _scaleFactor;
				_transform.Translate(offsetX, offsetY, MatrixOrder.Append);

				if (_rotateAngle != 0)
					_transform.RotateAt(-_rotateAngle, ptCenter, MatrixOrder.Append);
			
				_transform.Scale(_scaleFactor, _scaleFactor, MatrixOrder.Append);			
			
				this.Workspace.Invalidate(true);
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{
				this.Workspace.ImageViewer.EndUpdate();
			}
		}

		#endregion
	}
}
