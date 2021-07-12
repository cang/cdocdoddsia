using System;
using System.Collections.Generic;
using System.Text;
using SiGlaz.ObjectAnalysis.Common;
using SAC = SiGlaz.Algorithms.Core;
using SiGlaz.Common.Object;

namespace SiGlaz.ObjectAnalysis.Engine
{
    public class MultidimensionalComparer : IConnectivityComparer, IDisposable
    {
        private MDCCParam _MDCCparam;
        private static double _degree_mul = 180.0 / Math.PI;
        private static double _radian_mul = Math.PI / 180.0;
        private EllipseCalculator Calculator = new EllipseCalculator();

        public MultidimensionalComparer(SAC.IHumanCondition human_condition)
        {
            Init(human_condition);
        }

        public bool DefaultValue = false;
        public double GetValue(
            EllipticalDensityShapeObject p1, EllipticalDensityShapeObject p2, MDCCParam.LHS_KEYS key)
        {
            switch (key)
            {
                #region ellipse properties
                case MDCCParam.LHS_KEYS.X: //difference in X Coordinate
                    return Math.Abs(p1.X - p2.X);

                case MDCCParam.LHS_KEYS.Y: // difference in Y Coordinate
                    return Math.Abs(p1.Y - p2.Y);
                
                //				case MDCCParam.LHS_KEYS.D_XY:
                //					return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);

                case MDCCParam.LHS_KEYS.D_E_XY: // center-to-center distance
                    return (float)Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));

                case MDCCParam.LHS_KEYS.EllipseDistance: // point-to-point distance
                    return Calculator.Distance2Ellipse(p1, p2);

                case MDCCParam.LHS_KEYS.Eccentricity: // difference in eccentricity
                    return Math.Abs(p1.Eccentricity - p2.Eccentricity);

                case MDCCParam.LHS_KEYS.Elongation: // difference in elongation
                    return Math.Abs(p1.Elongation - p2.Elongation);

                case MDCCParam.LHS_KEYS.Major: // difference in major length
                    return Math.Abs(p1.Length - p2.Length);

                case MDCCParam.LHS_KEYS.Minor: // difference in minor length
                    return Math.Abs(p1.MinorLength - p2.MinorLength);

                case MDCCParam.LHS_KEYS.Orientation: // difference in orientation
                    return (float)Math.Abs(Math.IEEERemainder(p1.Orientation - p2.Orientation, 180.0));

                //case MDCCParam.LHS_KEYS.Radius: // difference in radius from center
                //    return (float)Math.Abs(
                //        Math.Sqrt(p1.X * p1.X + p1.Y * p1.Y) - Math.Sqrt(p2.X * p2.X + p2.Y * p2.Y));

                //case MDCCParam.LHS_KEYS.Angle: // difference in angle to center
                //    double a1 = Math.Atan2(p1.Y, p1.X) * _degree_mul;
                //    double a2 = Math.Atan2(p2.Y, p2.X) * _degree_mul;
                //    return (float)Math.Abs(Math.IEEERemainder(a1 - a2, 360.0));

                //case MDCCParam.LHS_KEYS.MaxAngleDeviationToBaseLine: // max angular deviation from the center-to-center line
                //    {
                //        double angle_base_line = Math.Atan2(p2.Y - p1.Y, p2.X - p1.X) * _degree_mul;
                //        return (float)Math.Max(Math.Abs(Math.IEEERemainder(p1.Orientation - angle_base_line, 180.0)),
                //            Math.Abs(Math.IEEERemainder(p2.Orientation - angle_base_line, 180.0)));
                //    }
                #endregion ellipse properties

                #region process the hidden keys
                //process the hidden keys
                    /*
                case MDCCParam.LHS_KEYS.DieRelativeDistance:
                    return Math.Abs(p1.DieRelDistance - p2.DieRelDistance);
                     * */

                case MDCCParam.LHS_KEYS.MinMajor: // 
                    return (float)Math.Min(p1.Length, p2.Length);

                case MDCCParam.LHS_KEYS.MaxHorizontalTolerance: // 
                    return (float)Math.Max(
                        Math.Abs(Math.IEEERemainder(p1.Orientation, 180.0)),
                        Math.Abs(Math.IEEERemainder(p2.Orientation, 180.0)));

                case MDCCParam.LHS_KEYS.MinHorizontalTolerance: // 
                    return (float)Math.Min(
                        Math.Abs(Math.IEEERemainder(p1.Orientation, 180.0)),
                        Math.Abs(Math.IEEERemainder(p2.Orientation, 180.0)));
                #endregion process the hidden keys

                #region process for detected object properties
                //case MDCCParam.LHS_KEYS.RectBoundWidth: //difference in rectangle boundary width
                //    return Math.Abs(
                //        p1.DetectedObject.RectBound.Width - 
                //        p2.DetectedObject.RectBound.Width);
                //case MDCCParam.LHS_KEYS.RectBoundHeight: //difference in rectangle boundary height
                //    return Math.Abs(
                //        p1.DetectedObject.RectBound.Height -
                //        p2.DetectedObject.RectBound.Height);
                case MDCCParam.LHS_KEYS.Area: //difference in area
                    return Math.Abs(
                        p1.DetectedObject.Area - p2.DetectedObject.Area);
                case MDCCParam.LHS_KEYS.Perimeter: //difference in perimeter
                    return Math.Abs(
                        p1.DetectedObject.Perimeter - p2.DetectedObject.Perimeter);
                case MDCCParam.LHS_KEYS.PixelCount: //difference in pixel count
                    return Math.Abs(
                        p1.DetectedObject.NumPixels - p2.DetectedObject.NumPixels);
                case MDCCParam.LHS_KEYS.AverageIntensity: //difference in average intensity
                    return Math.Abs(
                        p1.AverageIntensity - p2.AverageIntensity);
                //case MDCCParam.LHS_KEYS.MinIntensity: //difference in min intensity
                //    return Math.Abs(
                //        p1.MinIntensity - p2.MinIntensity);
                //case MDCCParam.LHS_KEYS.MaxIntensity: //difference in max intensity
                //    return Math.Abs(
                //        p1.MaxIntensity - p2.MaxIntensity);
                case MDCCParam.LHS_KEYS.IntegratedIntensity: //difference in integrated intensity
                    return Math.Abs(
                        p1.DetectedObject.TotalIntensity - p2.DetectedObject.TotalIntensity);
                case MDCCParam.LHS_KEYS.ObjectType: //difference in X Coordinate
                    return Math.Abs(
                        p1.DetectedObject.ObjectTypeId - p2.DetectedObject.ObjectTypeId);
                #endregion process for detected object properties

                ///////////////////////////////////////////////////////////////////////////////////////////////////
                default:
                    throw new System.ArgumentException("Invalid LHS key");
            }
        }

        public double GetValue(EllipticalDensityShapeObject p1, EllipticalDensityShapeObject p2, EllipticalDensityShapeObject p3, MDCCParam.LHS_KEYS key)
        {
            return Math.Max(GetValue(p1, p2, key),
                    Math.Max(GetValue(p2, p3, key), GetValue(p3, p1, key)));
        }
        #region IConnectivityComparer Members

        public bool IsConnected(object x, object y)
        {
            if (x == null || y == null)
                return false;

            if (_MDCCparam == null || _MDCCparam.Conditions == null ||
                _MDCCparam.Conditions.Count == 0)
                return DefaultValue;

            double _value = 0f;
            if (_MDCCparam.IsAndCondition) //AND condition
            {
                for (int i = 0; i < _MDCCparam.Conditions.Count; i++)
                {
                    MDCCParam.CONDITION _cond = (MDCCParam.CONDITION)_MDCCparam.Conditions[i];
                    _value = GetValue(x as EllipticalDensityShapeObject, y as EllipticalDensityShapeObject, _cond.LHS);
                    if (!QueryOperator.Compare(_value, _cond.Operator, _cond.RHS))
                        return false;
                }
                return true;
            }
            else //OR condition
            {
                for (int i = 0; i < _MDCCparam.Conditions.Count; i++)
                {
                    MDCCParam.CONDITION _cond = (MDCCParam.CONDITION)_MDCCparam.Conditions[i];
                    _value = GetValue(x as EllipticalDensityShapeObject, y as EllipticalDensityShapeObject, _cond.LHS);
                    if (QueryOperator.Compare(_value, _cond.Operator, _cond.RHS))
                        return true;
                }
                return false;
            }
        }
        public void Init(SAC.IHumanCondition Conditions)
        {
            if (Conditions.GetType() != typeof(MDCCParam))
                throw new System.ArgumentException("The condition is not an instance of Multidimensional Connected Component Condition");
            _MDCCparam = (MDCCParam)Conditions;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        #region IConnectivityComparer Members

        public bool IsConnected(object x, object y, object z)
        {
            if (x == null || y == null || z == null)
                return false;

            if (_MDCCparam == null || _MDCCparam.Conditions == null ||
                _MDCCparam.Conditions.Count == 0)
                return DefaultValue;

            double _value = 0f;
            if (_MDCCparam.IsAndCondition) //AND condition
            {
                for (int i = 0; i < _MDCCparam.Conditions.Count; i++)
                {
                    MDCCParam.CONDITION _cond = (MDCCParam.CONDITION)_MDCCparam.Conditions[i];
                    _value = GetValue(
                        x as EllipticalDensityShapeObject, 
                        y as EllipticalDensityShapeObject, 
                        z as EllipticalDensityShapeObject, _cond.LHS);
                    if (!QueryOperator.Compare(_value, _cond.Operator, _cond.RHS))
                        return false;
                }
                return true;
            }
            else //OR condition
            {
                for (int i = 0; i < _MDCCparam.Conditions.Count; i++)
                {
                    MDCCParam.CONDITION _cond = (MDCCParam.CONDITION)_MDCCparam.Conditions[i];
                    _value = GetValue(
                        x as EllipticalDensityShapeObject, 
                        y as EllipticalDensityShapeObject,
                        z as EllipticalDensityShapeObject, _cond.LHS);
                    if (QueryOperator.Compare(_value, _cond.Operator, _cond.RHS))
                        return true;
                }
                return false;
            }
        }

        public bool IsSuppressible(object x, object y)
        {
            if (x == null || y == null)
                throw new System.ArgumentException("Invalid operands");

            if (_MDCCparam == null || _MDCCparam.Conditions == null ||
                _MDCCparam.Conditions.Count == 0)
                return false;

            double _value = 0f;
            if (_MDCCparam.IsAndCondition) //AND condition
            {
                for (int i = 0; i < _MDCCparam.Conditions.Count; i++)
                {
                    MDCCParam.CONDITION _cond = 
                        (MDCCParam.CONDITION)_MDCCparam.Conditions[i];

                    if (MDCCParam.CheckConditionIsExtension(_cond.LHS))
                        continue;

                    _value = GetValue(
                        x as EllipticalDensityShapeObject, 
                        y as EllipticalDensityShapeObject, _cond.LHS);

                    if (!QueryOperator.Compare(_value, _cond.Operator, _cond.RHS))
                        return true;
                }
                return false;
            }
            else //OR condition
            {
                return false;
            }
        }

        #endregion
    }
}
