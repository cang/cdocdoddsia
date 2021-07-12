#define PARALLEL

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using SiGlaz.ObjectAnalysis.Common;
using SIA.Common.Analysis;
using SiGlaz.Common.Object;
using SIA.SystemFrameworks;
using SiGlaz.Common.ImageAlignment;

namespace SiGlaz.ObjectAnalysis.Engine
{
    public class FilterProcessor
    {
        public static ArrayList DoFilter(BaseQuery query, ArrayList objList, MetrologySystem ms)
        {
            if (objList == null || objList.Count == 0)
                return new ArrayList();

            int nPrimitiveObjects = objList.Count;
            EllipticalDensityShapeObject[] basePoints =
                new EllipticalDensityShapeObject[nPrimitiveObjects];
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
                    EllipticalDensityShapeObject.FromDetectedObject(objList[i] as DetectedObject);
            };
#endif

            try
            {
                for (int i = objList.Count - 1; i >= 0; i--)
                {
                    if (!query.CheckQuery(basePoints[i]))
                    {
                        //(objList[i] as EllipticalDensityShapeObject).Dispose();                        
                        objList.RemoveAt(i);
                    }
                }

                return objList;
            }
            catch (Exception ex)
            {
                return objList; // failed to combine
            }
            finally
            {
                basePoints = null;
                GC.Collect();
            }
        }

        public static ArrayList DoFilter(
            BaseQuery query, string sQuery, 
            QUERY_TYPE queryType, ArrayList objList, MetrologySystem ms)
        {
            if (!query.ParseQuery(sQuery, queryType))
                return objList;

            return DoFilter(query, objList, ms);
        }

        
        public static ArrayList DoFilter(string sQuery, 
            QUERY_TYPE queryType, ArrayList objList, MetrologySystem ms)
        {
            ObjectQuery objQuery = new ObjectQuery();
            if (!objQuery.ParseQuery(sQuery, queryType))
                return objList;

            return DoFilter(objQuery, objList, ms);
        }
    }
}
