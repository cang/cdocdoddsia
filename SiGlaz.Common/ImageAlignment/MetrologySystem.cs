using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace SiGlaz.Common.ImageAlignment
{
    public enum eMetrologySystemInteractiveMode
    {
        None = 0,
        MoveOrigin = 1,
        DrawNewMarker = 2,
        Calibrate = 3
    }

    public class MetrologySystem : ParameterBase
    {
        public const string Description = "Metrology System";
        public const string FileExt = "msf";
        public const string Filter = "Metrology System Format (*.msf)|*.msf";

        public int Version
        {
            get { return 1; }
        }

        protected MetrologyUnitBase _currentUnit = null;
        protected CoordinateSystem _coordinateSystem = new CoordinateSystem();
        protected List<MarkerPoint> _makers = new List<MarkerPoint>();
        protected Matrix _transformer = new Matrix();
        public Matrix Transformer
        {
            get { return _transformer; }
        }
        protected Matrix _invTransformer = new Matrix();
        public Matrix InvTransformer
        {
            get { return _invTransformer; }
        }

        public event EventHandler DataChanged;
        public event EventHandler NewMarkerAdded;

        protected PointF _pt = PointF.Empty;
        protected PointF[] _onePoints = new PointF[] { PointF.Empty };
        protected PointF[] _twoPoints = new PointF[] { PointF.Empty, PointF.Empty };
        protected PointF[] _threePoints = new PointF[] { PointF.Empty, PointF.Empty, PointF.Empty };

        public MetrologySystem()
        {
            _currentUnit = MicronUnit.CreateInstance(1, 1);
        }

        public MetrologyUnitBase CurrentUnit
        {
            get { return _currentUnit; }
        }

        public CoordinateSystem CurrentCoordinateSystem
        {
            get { return _coordinateSystem; }
        }

        public List<MarkerPoint> Markers
        {
            get { return _makers; }
        }

        #region Transformer
        public void ResetTransformer()
        {
            _transformer.Reset();
            _invTransformer.Reset();
        }

        public void RebuildTransformer()
        {
            _transformer.Reset();

            // translate to new origin
            _transformer.Translate(
                -CurrentCoordinateSystem.DrawingOriginX, 
                -CurrentCoordinateSystem.DrawingOriginY, MatrixOrder.Append);

            // rotate at origin
            _transformer.Rotate(CurrentCoordinateSystem.DrawingAngle, MatrixOrder.Append);

            // coordinate orientation
            float scaleX = CurrentUnit.UnitVal / CurrentUnit.PixelVal;
            float scaleY = scaleX;
            switch (CurrentCoordinateSystem.Orientation)
            {
                case eCoordinateSystemOrientation.Northeast:
                    _transformer.Scale(scaleX, -scaleY, MatrixOrder.Append);
                    break;
                case eCoordinateSystemOrientation.Northwest:
                    _transformer.Scale(-scaleX, -scaleY, MatrixOrder.Append);
                    break;
                case eCoordinateSystemOrientation.Southwest:
                    _transformer.Scale(-scaleX, scaleY, MatrixOrder.Append);
                    break;
                case eCoordinateSystemOrientation.Southeast:
                    _transformer.Scale(scaleX, scaleY, MatrixOrder.Append);
                    break;
                default:
                    break;
            }

            if (_invTransformer != null)
            {
                _invTransformer.Dispose();
                _invTransformer = null;
            }

            _invTransformer = _transformer.Clone();
            _invTransformer.Invert();
        }

        public void ToRealCoordinate(ref float x, ref float y)
        {
            _onePoints[0].X = x;
            _onePoints[0].Y = y;

            _transformer.TransformPoints(_onePoints);

            // update real-coordinate
            x = _onePoints[0].X;
            y = _onePoints[0].Y;
        }

        public PointF ToRealCoordinate(PointF pt)
        {
            _onePoints[0] = pt;
            _transformer.TransformPoints(_onePoints);

            return new PointF(_onePoints[0].X, _onePoints[0].Y);
        }

        public void ToRealCoordinate(ref PointF[] pts)
        {
            _transformer.TransformPoints(pts);
        }

        public void ToPixel(ref float x, ref float y)
        {
            _onePoints[0].X = x;
            _onePoints[0].Y = y;

            _invTransformer.TransformPoints(_onePoints);

            // update real coordinate
            x = _onePoints[0].X;
            y = _onePoints[0].Y;
        }

        public PointF ToPixel(PointF pt)
        {
            _onePoints[0] = pt;
            _invTransformer.TransformPoints(_onePoints);

            return new PointF(_onePoints[0].X, _onePoints[0].Y);
        }

        public void ToPixel(ref PointF[] pts)
        {
            _invTransformer.TransformPoints(pts);
        }
        #endregion Transformer

        #region Methods
        public virtual void RaiseDataChangedEvent()
        {
            if (DataChanged != null)
                this.DataChanged(this, EventArgs.Empty);
        }

        public virtual MarkerPoint FindMarker(float x, float y)
        {
            foreach (MarkerPoint marker in _makers)
            {
                if (!(marker is MarkerRegion))
                    continue;

                if ((marker as MarkerRegion).Contains(x, y))
                    return marker;
            }

            return null;
        }        
        #endregion Methods

        #region Markers
        public virtual void AddMarker(MarkerPoint marker)
        {
            Markers.Add(marker);
            if (NewMarkerAdded != null)
                this.NewMarkerAdded(this, EventArgs.Empty);
        }

        public virtual void RemoveMarker(MarkerPoint marker)
        {
            Markers.Remove(marker);
        }

        public virtual void ClearMarkers()
        {
            Markers.Clear();
        }
        #endregion Markers

        #region Copy data
        public void CopyFrom(MetrologySystem ms)
        {
            _currentUnit.CopyFrom(ms.CurrentUnit);
            _coordinateSystem.CopyFrom(ms.CurrentCoordinateSystem);

            _makers.Clear();
            _makers.AddRange(ms.Markers);
        }
        #endregion Copy data

        #region Serialize and deserialize
        protected override void BaseSerialize(System.IO.BinaryWriter bin)
        {
            base.BaseSerialize(bin); bin.Write(CurrentVersion);

            _currentUnit.Serialize(bin);
            _coordinateSystem.Serialize(bin);

            int n = _makers.Count;
            bin.Write(n);
            for (int i = 0; i < n; i++)
            {
                _makers[i].Serialize(bin);
            }
        }

        protected override void BinDeserialize(System.IO.BinaryReader bin)
        {
            base.BinDeserialize(bin); int version = bin.ReadInt32();

            _currentUnit = MetrologyUnitBase.Deserialize(bin);
            _coordinateSystem = CoordinateSystem.Deserialize(bin);

            int n = bin.ReadInt32();
            for (int i = 0; i < n; i++)
            {
                _makers.Add(MarkerPoint.Deserialize(bin));
            }
        }

        public static MetrologySystem Deserialize(System.IO.BinaryReader bin)
        {
            return ParameterBase.BaseDeserialize(bin) as MetrologySystem;
        }

        public static MetrologySystem Deserialize(string filename)
        {
            return ParameterBase.BaseDeserialize(filename) as MetrologySystem;
        }
        #endregion Serialize and deserialize

        #region Extension
        public void CalcError(MetrologySystem ms, 
            ref double originError, ref double angleError, 
            ref double markerError, ref double[] markerErrors)
        {
            CurrentCoordinateSystem.CalcError(
                ms.CurrentCoordinateSystem, ref originError, ref angleError);

            int n = (int)Math.Min(Markers.Count, ms.Markers.Count);
            markerErrors = new double[n];
            markerError = 0;
            for (int i = 0; i < n; i++)
            {
                Markers[i].CalcError(ms.Markers[i], ref markerErrors[i]);

                markerError += markerErrors[i];
            }

            if (n > 0)
                markerError = markerError / n;
        }
        #endregion Extension
    }
}
