using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using SIA.UI.Controls.Automation.Commands;
using SIA.Common.KlarfExport;
using SIA.Common.Analysis;
using SiGlaz.Common.ABSDefinitions;

namespace SIA.UI.Controls.Commands
{
    public class SimpleFilterCommand
    {
        public static void ApplyFilter(
            ArrayList detectedObjects, ObjectFilterSettings settings)
        {
            if (settings.ApplyFilterForDarkObject)
            {
                ApplyFilter(
                    detectedObjects, 
                    (int)eDefectType.DarkObject, 
                    settings.DarkObjectFilterSettings);
            }

            if (settings.ApplyFilterForBrightObject)
            {
                ApplyFilter(
                    detectedObjects,
                    (int)eDefectType.BrightObject,
                    settings.BrightObjectFilterSettings);
            }

            if (settings.ApplyFilterForDarkObjectAcrossBoundary)
            {
                ApplyFilter(
                    detectedObjects,
                    (int)eDefectType.DarkObjectAcrossBoundary,
                    settings.DarkObjectAcrossBoundaryFilterSettings);
            }

            if (settings.ApplyFilterForDarkObject)
            {
                ApplyFilter(
                    detectedObjects,
                    (int)eDefectType.BrightObjectAcrossBoundary,
                    settings.BrightObjectAcrossBoundaryFilterSettings);
            }
        }

        public static void ApplyFilter(
            ArrayList detectedObjects, int objectId, ObjectFilterArguments settings)
        {
            if (detectedObjects == null || detectedObjects.Count == 0)
                return;

            for (int i = detectedObjects.Count - 1; i >= 0; i--)
            {
                DetectedObject obj = detectedObjects[i] as DetectedObject;
                if (obj.ObjectTypeId != objectId)
                    continue;

                if (settings.FilterByArea)
                {
                    if (!IsValid(obj.Area, settings.Area))
                    {
                        detectedObjects.RemoveAt(i);
                        continue;
                    }
                }

                if (settings.FilterByIntegratedIntensity)
                {
                    if (!IsValid(obj.TotalIntensity / obj.NumPixels, settings.IntegratedIntensity))
                    {
                        detectedObjects.RemoveAt(i);
                        continue;
                    }
                }

                if (settings.FilterByNumberOfPixels)
                {
                    if (!IsValid(obj.NumPixels, settings.NumberOfPixelFilter))
                    {
                        detectedObjects.RemoveAt(i);
                        continue;
                    }
                }

                if (settings.FilterByPerimeter)
                {
                    if (!IsValid(obj.Perimeter, settings.Perimeter))
                    {
                        detectedObjects.RemoveAt(i);
                        continue;
                    }
                }
            }
        }

        private static bool IsValid(double val, RangeFilterArgument range)
        {
            if (range.UseUpperValue && val > range.UpperValue)
                return false;

            if (range.UseLowerValue && val < range.LowerValue)
                return false;

            return true;
        }

        public static void RemoveObjects(ArrayList detectedObjects, int objectId)
        {
            if (detectedObjects == null || detectedObjects.Count == 0)
                return;

            for (int i = detectedObjects.Count - 1; i >= 0; i--)
            {
                DetectedObject obj = detectedObjects[i] as DetectedObject;
                if (obj.ObjectTypeId != objectId)
                    continue;

                detectedObjects.RemoveAt(i);
            }
        }

        public static void FilterTooSmallObject(ArrayList detectedObjects, double threshold)
        {
            if (detectedObjects == null || detectedObjects.Count == 0)
                return;

            for (int i = detectedObjects.Count - 1; i >= 0; i--)
            {
                DetectedObject obj = detectedObjects[i] as DetectedObject;
                if (obj.NumPixels < threshold)
                    detectedObjects.RemoveAt(i);
            }
        }

        public static void FilterTooSmallObject(
            ArrayList detectedObjects, int objectTypeId, double threshold)
        {
            if (detectedObjects == null || detectedObjects.Count == 0)
                return;

            for (int i = detectedObjects.Count - 1; i >= 0; i--)
            {
                DetectedObject obj = detectedObjects[i] as DetectedObject;

                if (obj.ObjectTypeId != objectTypeId)
                    continue;

                if (obj.NumPixels < threshold)
                    detectedObjects.RemoveAt(i);
            }
        }
    }
}
