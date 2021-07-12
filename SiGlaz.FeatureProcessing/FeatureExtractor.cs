using System;
using System.Collections.Generic;
using System.Text;
using SIA.SystemLayer;
using SiGlaz.FeatureProcessing.Helpers;
using SiGlaz.FeatureProcessing.Textural;
using SiGlaz.Common.ABSInspectionSettings;

namespace SiGlaz.FeatureProcessing
{
    public class FeatureExtractor
    {
        #region ABS Contaminations
        public static FeatureSpace 
            ExtractFeatures_For_ABSContaminationsDetection(CommonImage image)
        {
            FeatureSpace featureSpace = null;

            int[] xList = null;
            int[] yList = null;
            int[] contaminationMask = null;
            double[][] features = 
                ExtractFeatures(image, ref xList, ref yList, ref contaminationMask);

            if (features != null)
            {
                int featureCount = features[0].Length;
                int n = xList.Length;
                FeatureVectorCollection fvs = new FeatureVectorCollection(n);
                for (int i = 0; i < n; i++)
                {
                    IFeatureVector fv = 
                        new FeatureVector(features[i], featureCount);

                    if (contaminationMask[i] < 0)
                        fv.Features = null;

                    fvs.Add(fv);
                }

                featureSpace = new FeatureSpace();
                featureSpace.XInterestedPoints = xList;
                featureSpace.YInterestedPoints = yList;
                featureSpace.ContaminationMask = contaminationMask;
                featureSpace.Raws = features;
                featureSpace.Features = fvs;
            }

            return featureSpace;
        }

        public static FeatureSpace
            ExtractFeatures_For_PoleTipContaminationsDetection(CommonImage image)
        {
            FeatureSpace featureSpace = null;

            int[] xList = null;
            int[] yList = null;
            int[] contaminationMask = null;
            double[][] features =
                ExtractFeaturesPoleTip(image, ref xList, ref yList, ref contaminationMask);

            if (features != null)
            {
                int featureCount = features[0].Length;
                int n = xList.Length;
                FeatureVectorCollection fvs = new FeatureVectorCollection(n);
                for (int i = 0; i < n; i++)
                {
                    IFeatureVector fv =
                        new FeatureVector(features[i], featureCount);

                    if (contaminationMask[i] < 0)
                        fv.Features = null;

                    fvs.Add(fv);
                }

                featureSpace = new FeatureSpace();
                featureSpace.XInterestedPoints = xList;
                featureSpace.YInterestedPoints = yList;
                featureSpace.ContaminationMask = contaminationMask;
                featureSpace.Raws = features;
                featureSpace.Features = fvs;
            }

            return featureSpace;
        }

        private static unsafe double[][] ExtractFeatures(
            CommonImage image, ref int[] x, ref int[] y, ref int[] contaminationMask)
        {
            int w = image.Width;
            int h = image.Height;
            int l = image.Length;
            byte[] data = new byte[l];
            ushort* src = image.RasterImage._aData;
            for (int i = 0; i < l; i++)
            {
                data[i] = (byte)src[i];
            }

            int kernelSize = ContaminationTexturalInfoHelper.TexturalExtractorSettings.KernelSize;
            int[] xInterestedPoints = ContaminationTexturalInfoHelper.XInterestedPoints;
            int[] yInterestedPoints = ContaminationTexturalInfoHelper.YInterestedPoints;
            int[] builtContaminationMask = ContaminationTexturalInfoHelper.ContaminationsMask;

            int n = xInterestedPoints.Length;
            x = new int[n]; Array.Copy(xInterestedPoints, x, n);
            y = new int[n]; Array.Copy(yInterestedPoints, y, n);
            contaminationMask = new int[n]; Array.Copy(builtContaminationMask, contaminationMask, n);

            for (int i = 0; i < n; i++)
            {
                if (xInterestedPoints[i] >= w || yInterestedPoints[i] >= h)
                {
                    contaminationMask[i] = -1;
                }
            }            
            n = x.Length;
            double[][] features = new double[n][];
            int nFeatures = 28;
            for (int i = 0; i < n; i++)
            {
                features[i] = new double[nFeatures];
            }

            int greyLevel =
                ContaminationTexturalInfoHelper.TexturalExtractorSettings.GreyLevel;
            TextureProcessor.ExtractFeaturesParallely(
                data, image.Width, kernelSize, greyLevel, x, y, contaminationMask, features);

            return features;
        }

        private static unsafe double[][] ExtractFeaturesPoleTip(
            CommonImage image, ref int[] x, ref int[] y, ref int[] contaminationMask)
        {
            int w = image.Width;
            int h = image.Height;
            int l = image.Length;
            byte[] data = new byte[l];
            ushort* src = image.RasterImage._aData;
            for (int i = 0; i < l; i++)
            {
                data[i] = (byte)src[i];
            }

            int kernelSize = ContaminationTexturalInfoHelperPoleTip.TexturalExtractorSettings.KernelSize;
            int[] xInterestedPoints = ContaminationTexturalInfoHelperPoleTip.XInterestedPoints;
            int[] yInterestedPoints = ContaminationTexturalInfoHelperPoleTip.YInterestedPoints;
            int[] builtContaminationMask = ContaminationTexturalInfoHelperPoleTip.ContaminationsMask;

            int n = xInterestedPoints.Length;
            x = new int[n]; Array.Copy(xInterestedPoints, x, n);
            y = new int[n]; Array.Copy(yInterestedPoints, y, n);
            contaminationMask = new int[n]; Array.Copy(builtContaminationMask, contaminationMask, n);

            for (int i = 0; i < n; i++)
            {
                if (xInterestedPoints[i] >= w || yInterestedPoints[i] >= h)
                {
                    contaminationMask[i] = -1;
                }
            }
            n = x.Length;
            double[][] features = new double[n][];
            int nFeatures = 28;
            for (int i = 0; i < n; i++)
            {
                features[i] = new double[nFeatures];
            }

            int greyLevel =
                ContaminationTexturalInfoHelperPoleTip.TexturalExtractorSettings.GreyLevel;
            TextureProcessor.ExtractFeaturesParallely(
                data, image.Width, kernelSize, greyLevel, x, y, contaminationMask, features);

            return features;
        }
        #endregion ABS Contaminations

        #region ABS Line Patterns
        public static FeatureSpace 
            ExtractFeatures_For_ABSLinePatternDetection(
            ushort[] data, int width, int height, 
            int[] x, int[] y, TexturalExtractorSettings settings)
        {
            FeatureSpace featureSpace = null;

            double[][] features = null;

            unsafe
            {
                fixed (ushort* src = data)
                {
                    features = ExtractFeatures(
                        src, width, height,
                        x, y, settings.KernelSize, settings.GreyLevel);
                }
            }

            if (features != null)
            {
                int featureCount = features[0].Length;
                int n = x.Length;
                FeatureVectorCollection fvs = new FeatureVectorCollection(n);
                for (int i = 0; i < n; i++)
                {
                    IFeatureVector fv =
                        new FeatureVector(features[i], featureCount);
                    fvs.Add(fv);
                }

                featureSpace = new FeatureSpace();
                featureSpace.XInterestedPoints = x;
                featureSpace.YInterestedPoints = y;
                featureSpace.Raws = features;
                featureSpace.Features = fvs;
            }

            return featureSpace;
        }

        private static unsafe double[][] ExtractFeatures(
            ushort* src, int w, int h, 
            int[] x, int[] y, int kernelSize, int greyLevel)
        {
            int l = w * h;
            byte[] data = new byte[l];
            for (int i = 0; i < l; i++)
            {
                data[i] = (byte)src[i];
            }

            int n = x.Length;
            double[][] features = new double[n][];
            int nFeatures = 28;
            for (int i = 0; i < n; i++)
            {
                features[i] = new double[nFeatures];
            }
            
            TextureProcessor.ExtractFeaturesParallely(
                data, w, kernelSize, greyLevel, x, y, features);

            return features;
        }
        #endregion ABS Line Patterns
    }
}
