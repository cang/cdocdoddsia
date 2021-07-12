#define PARALLEL

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using SiGlaz.ObjectAnalysis.Common;
using SAC = SiGlaz.Algorithms.Core;
using SIA.Common.Analysis;
using System.Drawing;
using SiGlaz.Common.Object;
using SIA.SystemFrameworks;
using SiGlaz.Common.ImageAlignment;

namespace SiGlaz.ObjectAnalysis.Engine
{
    public class CombinationProcessor
    {
        public static ArrayList DoCombine(
            ArrayList objList, MDCCParamItem conditionItem, MetrologySystem ms)
        {
            if (objList == null || objList.Count == 0)
                return new ArrayList();

            MDCCParam condition = conditionItem.Condition;

            int nPrimitiveObjects = objList.Count;
            SAC.BasePoint[] basePoints = 
                new SiGlaz.Algorithms.Core.BasePoint[nPrimitiveObjects];
#if PARALLEL
            Parallel.For(0, nPrimitiveObjects, delegate(int i)
            {
                basePoints[i] =
                    EllipticalDensityShapeObject.FromDetectedObject(objList[i] as DetectedObject, ms);
            });
#else
            for (int i=0; i<nPrimitiveObjects; i++)
            {
                basePoints[i] =
                    EllipticalDensityShapeObject.FromDetectedObject(objList[i] as DetectedObject, ms);
            };
#endif

            ArrayList groups = null;
            try
            {
                groups = DoCombine(basePoints, condition);

                ArrayList combinedObjs = new ArrayList(nPrimitiveObjects);
                if (groups != null && groups.Count > 0)
                {
                    foreach (ArrayList group in groups)
                    {
                        if (group.Count == 1)
                        {
                            combinedObjs.Add(
                                (group[0] as EllipticalDensityShapeObject).DetectedObject);

                            continue;
                        }

                        List<DetectedObject> objs = new List<DetectedObject>(group.Count);
                        foreach (EllipticalDensityShapeObject obj in group)
                        {
                            if (obj.DetectedObject is DetectedObjectEx)
                            {
                                foreach (DetectedObject primitiveObj in (obj.DetectedObject as DetectedObjectEx).PrimitiveObjects)
                                {
                                    objs.Add(primitiveObj);
                                }
                            }
                            else
                            {
                                objs.Add(obj.DetectedObject);
                            }
                        }

                        DetectedObject newObj = CombineObjects(objs);
                        if (newObj is DetectedObjectEx)
                        {
                            (newObj as DetectedObjectEx).SignatureName = conditionItem.SignatureName;
                        }

                        combinedObjs.Add(newObj);
                    }
                }

                return combinedObjs;
            }
            catch (System.Exception exp)
            {
                System.Diagnostics.Trace.WriteLine(exp.Message);
                System.Diagnostics.Trace.WriteLine(exp.StackTrace);

                return objList; // failed to combine
            }
            finally
            {
                basePoints = null;
                GC.Collect();
            }
        }

        public static ArrayList DoCombine(
            SAC.BasePoint[] basePoints, MDCCParam condition)
        {
            MultidimensionalComparer _comp = new MultidimensionalComparer(condition);
            DFSConnectTracking _tracker = null;
            if (condition.DFSLevel == 3)
            {
                _tracker = new DFSConnectTracking3Points(basePoints, _comp);
            }
            else
                _tracker = new DFSConnectTracking(basePoints, _comp);

            if (_tracker.AssignLabel() != 0)
                throw new System.ExecutionEngineException("Getting connected component failed");
            if (_tracker.Label == null || _tracker.Label.Length != basePoints.Length)
                throw new System.ExecutionEngineException("Invalid Label Matrix of connected component");

            int nSO = 0;
            for (int i = 0; i < _tracker.Label.Length; i++)
                if (nSO < _tracker.Label[i])
                    nSO = _tracker.Label[i];

            ArrayList result = new ArrayList(nSO);
            for (int i = 0; i < nSO; i++)
                result.Add(new ArrayList());

            for (int iobj = 0; iobj < _tracker.Label.Length; iobj++)
            {
                int _label = _tracker.Label[iobj];

                if (_label == 0)
                    continue;

                (result[_label - 1] as ArrayList).Add(basePoints[iobj]);
            }

            for (int iso = result.Count - 1; iso >= 0; iso--)
            {
                if (result[iso] == null || (result[iso] as ArrayList).Count == 0)
                    result.RemoveAt(iso);
                else
                    (result[iso] as ArrayList).TrimToSize();
            }

            return result;
        }

        public static DetectedObject CombineObjects(List<DetectedObject> objList)
        {
            if (objList == null)
                return null;
            if (objList.Count == 1)
                return objList[0];

            DetectedObject super = null;
            try
            {
                super = new DetectedObjectEx(objList);
            }
            catch
            {
                super = null;
            }
            return super;
        }
    }
}
