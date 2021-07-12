using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Diagnostics;

using SIA.Common;
using SIA.Common.Analysis;
using SIA.Common.KlarfExport;
using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;
using SIA.IPEngine;

namespace SIA.SystemLayer.ImageProcessing
{
	/// <summary>
	/// The CameraCorrection class provides functions for removing distortion of an image or a list of points
    /// </summary>
    public class CameraCorrection
	{
        /// <summary>
        /// The distortion or lens settings used for removing distortion
        /// </summary>
		LensCorrectionParameters _lensParameters = null;

		public CameraCorrection(LensCorrectionParameters args)
		{
			_lensParameters = args;
		}

        /// <summary>
        /// Undistort the specified point provided by x and y
        /// </summary>
        /// <param name="x">The x location of the input point</param>
        /// <param name="y">The y location of the input point</param>
        /// <param name="xUnDist">The result undistort x location</param>
        /// <param name="yUnDist">The result undistort y location</param>
		public void Correct(float x, float y, ref float xUnDist, ref float yUnDist)
		{
			if (_lensParameters == null)
				throw new System.ExecutionEngineException("Lens settings are not defined");
			if (Math.Abs(_lensParameters.FocalLength) <= float.Epsilon)
				throw new System.ExecutionEngineException("Lens Focal Length is too small");

			x = (x - _lensParameters.PrincipalPoint.X) / _lensParameters.FocalLength;
			y = (y - _lensParameters.PrincipalPoint.Y) / _lensParameters.FocalLength;

			double k1 = _lensParameters.DistortionCoeffs[0];
			double k2 = _lensParameters.DistortionCoeffs[1];
			double k3 = _lensParameters.DistortionCoeffs[4];
			double p1 = _lensParameters.DistortionCoeffs[2];
			double p2 = _lensParameters.DistortionCoeffs[3];

			double xTemp = x, yTemp = y;
			double deltaX = 0, deltaY = 0;
			double r2 = 0;
			double kRadial = 0;

			for (int kk = 0; kk < 20; kk++)
			{
				r2 = xTemp * xTemp + yTemp * yTemp;
				kRadial = 1 + k1 * r2 + k2 * r2 * r2 + k3 * r2 * r2 * r2;
				deltaX = 2 * p1 * xTemp * yTemp + p2 * (r2 + 2 * xTemp * xTemp);
				deltaY = p1 * (r2 + 2 * yTemp * yTemp) + 2 * p2 * xTemp * yTemp;
				xTemp = (x - deltaX) / kRadial;
				yTemp = (y - deltaY) / kRadial;
			}
			
			xUnDist = (float)(xTemp * _lensParameters.FocalLength +_lensParameters.PrincipalPoint.X);
			yUnDist = (float)(yTemp * _lensParameters.FocalLength +_lensParameters.PrincipalPoint.Y);

		}

        /// <summary>
        /// Undistort the collection of cluster objects
        /// </summary>
        /// <param name="clusters">The list of cluster objects</param>
		public void Correct(ArrayList clusters)
		{
			if (_lensParameters == null)
				throw new System.ExecutionEngineException("Lens settings are not defined");
			if (Math.Abs(_lensParameters.FocalLength) <= float.Epsilon)
				throw new System.ExecutionEngineException("Lens Focal Length is too small");

			double k1 = _lensParameters.DistortionCoeffs[0];
			double k2 = _lensParameters.DistortionCoeffs[1];
			double k3 = _lensParameters.DistortionCoeffs[4];
			double p1 = _lensParameters.DistortionCoeffs[2];
			double p2 = _lensParameters.DistortionCoeffs[3];

			float x = 0, y = 0;
			float xUnDist = 0, yUnDist = 0;
			float width = 0, height = 0;

			double xTemp = 0, yTemp = 0;
			double deltaX = 0, deltaY = 0;
			double r2 = 0;
			double kRadial = 0;

			int oldStep = 0, newStep = 0;
			CommandProgress.SetText("Camera correction...");
			CommandProgress.StepTo(0);
			
			int num_clusters = clusters.Count;
			int index = 0;		
			foreach (ClusterEx cluster in clusters)
			{

				x = (cluster.Center.X - _lensParameters.PrincipalPoint.X) / _lensParameters.FocalLength;
				y = (cluster.Center.Y - _lensParameters.PrincipalPoint.Y) / _lensParameters.FocalLength;

				xTemp = x;
				yTemp = y;

				for (int kk = 0; kk < 20; kk++)
				{
					r2 = xTemp * xTemp + yTemp * yTemp;
					kRadial = 1 + k1 * r2 + k2 * r2 * r2 + k3 * r2 * r2 * r2;
					deltaX = 2 * p1 * xTemp * yTemp + p2 * (r2 + 2 * xTemp * xTemp);
					deltaY = p1 * (r2 + 2 * yTemp * yTemp) + 2 * p2 * xTemp * yTemp;
					xTemp = (x - deltaX) / kRadial;
					yTemp = (y - deltaY) / kRadial;
				}
			
				xUnDist = (float)(xTemp * _lensParameters.FocalLength +_lensParameters.PrincipalPoint.X);
				yUnDist = (float)(yTemp * _lensParameters.FocalLength +_lensParameters.PrincipalPoint.Y);
				
				width = cluster.Rectangle.Width;
				height = cluster.Rectangle.Height;
				cluster.Rectangle = new RectangleF(xUnDist - width*0.5F, yUnDist - height*0.5F, width, height);

				newStep = (int)(++index*100.0F/num_clusters);
				if (newStep != oldStep)
				{
					oldStep = newStep;
					CommandProgress.StepTo(newStep);
				}

			}
		}

        /// <summary>
        /// Undistort the list of points
        /// </summary>
        /// <param name="pts">The input list of points to undistort</param>
        /// <returns>The undistorted list of points</returns>
		public PointF[] Correct(PointF[] pts)
		{
			if (_lensParameters == null)
				throw new System.ExecutionEngineException("Lens settings are not defined");
			if (Math.Abs(_lensParameters.FocalLength) <= float.Epsilon)
				throw new System.ExecutionEngineException("Lens Focal Length is too small");

			PointF[] result = new PointF[pts.Length];

			for (int i=0; i<result.Length; i++)
			{
				float x = pts[i].X, y = pts[i].Y;
				x = (x - _lensParameters.PrincipalPoint.X) / _lensParameters.FocalLength;
				y = (y - _lensParameters.PrincipalPoint.Y) / _lensParameters.FocalLength;

				double k1 = _lensParameters.DistortionCoeffs[0];
				double k2 = _lensParameters.DistortionCoeffs[1];
				double k3 = _lensParameters.DistortionCoeffs[4];
				double p1 = _lensParameters.DistortionCoeffs[2];
				double p2 = _lensParameters.DistortionCoeffs[3];

				double xTemp = x, yTemp = y;
				double deltaX = 0, deltaY = 0;
				double r2 = 0;
				double kRadial = 0;

				for (int kk = 0; kk < 20; kk++)
				{
					r2 = xTemp * xTemp + yTemp * yTemp;
					kRadial = 1 + k1 * r2 + k2 * r2 * r2 + k3 * r2 * r2 * r2;
					deltaX = 2 * p1 * xTemp * yTemp + p2 * (r2 + 2 * xTemp * xTemp);
					deltaY = p1 * (r2 + 2 * yTemp * yTemp) + 2 * p2 * xTemp * yTemp;
					xTemp = (x - deltaX) / kRadial;
					yTemp = (y - deltaY) / kRadial;
				}
			
				float xUnDist = (float)(xTemp * _lensParameters.FocalLength +_lensParameters.PrincipalPoint.X);
				float yUnDist = (float)(yTemp * _lensParameters.FocalLength +_lensParameters.PrincipalPoint.Y);

				result[i] = new PointF(xUnDist, yUnDist);
			}

			return result;
		}

	}
}
