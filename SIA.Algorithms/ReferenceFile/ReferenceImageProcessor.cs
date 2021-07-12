#define PARALLEL__

#define TEST___

#define DEBUG_METETIME_


/* for optimze
new GoldenImageProcesso: 1
ReferenceImageAlignmentHelper.AlignImage: 94896 (1)
GetInterestedPixels: 55944 (2)
Totals :150841

 
(1)
ReferenceFileProcessor.GetImage: 48789
InterpolationMethod.Bilinear: 39815
Totals :88604

(2)
update coordinate system: 133
Copy Image: 5409
Filter Image: 93367
Binarize: 153534
Totals :252443

*/



using System;
using System.Collections.Generic;
using System.Text;
using SIA.IPEngine;
using SiGlaz.Common.ImageAlignment;
using SIA.Algorithms.Preprocessing.Interpolation;
using System.Windows.Forms;
using System.IO;
using SiGlaz.Common;
using SIA.SystemFrameworks;
using SIA.Algorithms.Preprocessing.Alignment;
using System.Drawing;
using SIA.Algorithms.Preprocessing;
using SIA.Algorithms.FeatureProcessing.Filters;
using SIA.SystemLayer;
using SiGlaz.Cloo;

namespace SIA.Algorithms.ReferenceFile
{
    public class ReferenceImageAlignmentHelper
    {
        public static GreyDataImage AlignImage(
            MetrologySystemReference srcRefFile, 
            AlignmentSettings settings, float deviceLeft, float deviceTop, float deviceAngle)
        {
            GreyDataImage image = null;

            try
            {                
                using (GreyDataImage srcImage = ReferenceFileProcessor.GetImage(srcRefFile))
                {
                    using (System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix())
                    {
                        m.Translate(deviceLeft, deviceTop);
                        m.Rotate(-deviceAngle);
                        m.Invert();

                        AlignerBase aligner = AlignerFactory.CreateInstance(settings);
                        AlignmentResult alignment = aligner.Align(srcImage);

                        System.Drawing.Drawing2D.Matrix inverseTransform = null;

                        RectangleF dest = new RectangleF(
                            0, 0, settings.NewWidth, settings.NewHeight);
                        using (inverseTransform = 
                            new System.Drawing.Drawing2D.Matrix(dest, alignment.SourceCoordinates))
                        {
                            inverseTransform.Multiply(m);

                            image = new GreyDataImage(srcImage.Width, srcImage.Height);
                            ImageInterpolator.AffineTransform(
                                InterpolationMethod.Bilinear, srcImage, inverseTransform, image);
                        }
                    }                    
                }
            }
            catch (System.Exception exp)
            {
                if (image != null)
                {
                    image.Dispose();
                    image = null;
                }

                throw exp;
            }
            finally
            {
            }

            return image;
        }

        public static GreyDataImage AlignImage(
            MetrologySystemReference srcRefFile, 
            float dstDeviceLeft, float dstDeviceTop, float dstDeviceAngle)
        {
            GreyDataImage image = null;
            try
            {
#if DEBUG_METETIME
                DebugMeteTime dm = new DebugMeteTime();
#endif
                float srcDeviceLeft = srcRefFile.DeviceLeft;
                float srcDeviceTop = srcRefFile.DeviceTop;
                float srcAngle = srcRefFile.DeviceOrientation;

                using (GreyDataImage srcImage = ReferenceFileProcessor.GetImage(srcRefFile))
                {
#if DEBUG_METETIME
                    dm.AddLine("ReferenceFileProcessor.GetImage");
#endif

                    using (System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix())
                    {
                        m.Translate(dstDeviceLeft, dstDeviceTop);
                        m.Rotate(-dstDeviceAngle);
                        m.Invert();

                        using (System.Drawing.Drawing2D.Matrix inverseTransform = 
                            new System.Drawing.Drawing2D.Matrix())
                        {
                            inverseTransform.Translate(srcDeviceLeft, srcDeviceTop);
                            inverseTransform.Rotate(-srcAngle);

                            inverseTransform.Multiply(m);

                            image = new GreyDataImage(srcImage.Width, srcImage.Height);

#if DEBUG_METETIME
                            dm.AddLine("new GreyDataImage");
#endif

                            ImageInterpolator.AffineTransform(
                                InterpolationMethod.Bilinear
                                , srcImage, inverseTransform, image);

#if DEBUG_METETIME
                            dm.AddLine("InterpolationMethod.Bilinear");
                            dm.Write2Debug(true);
#endif

                        }
                    }
                }
            }
            catch (System.Exception exp)
            {
                if (image != null)
                {
                    image.Dispose();
                    image = null;
                }

                throw exp;
            }
            finally
            {
            }

            return image;
        }

#if GPU_SUPPORTED
        unsafe public static DeviceBuffer<ushort> AlignImageDeviceBuffer(
          MetrologySystemReference srcRefFile,
          float dstDeviceLeft, float dstDeviceTop, float dstDeviceAngle)
        {
            DeviceBuffer<ushort> image = null;

            try
            {
#if DEBUG_METETIME
                DebugMeteTime dm = new DebugMeteTime();
#endif
                float srcDeviceLeft = srcRefFile.DeviceLeft;
                float srcDeviceTop = srcRefFile.DeviceTop;
                float srcAngle = srcRefFile.DeviceOrientation;

                using (GreyDataImage srcImage = ReferenceFileProcessor.GetImage(srcRefFile))
                {
#if DEBUG_METETIME
                    dm.AddLine("ReferenceFileProcessor.GetImage");
#endif

                    using (System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix())
                    {
                        m.Translate(dstDeviceLeft, dstDeviceTop);
                        m.Rotate(-dstDeviceAngle);
                        m.Invert();

                        using (System.Drawing.Drawing2D.Matrix inverseTransform =
                            new System.Drawing.Drawing2D.Matrix())
                        {
                            inverseTransform.Translate(srcDeviceLeft, srcDeviceTop);
                            inverseTransform.Rotate(-srcAngle);

                            inverseTransform.Multiply(m);

                            //create device buffer from ReferenceFileProcessor.GetImage(srcRefFile))
                            DeviceBuffer<ushort> cbsrcImage =  DeviceBuffer<ushort>.CreateBufferReadOnly(srcImage._lenght, (IntPtr)srcImage._aData);

                            image = ImageInterpolator.AffineTransform(
                                InterpolationMethod.Bilinear
                                , cbsrcImage, new Size(srcImage.Width, srcImage.Height)
                                , new Size(srcImage.Width, srcImage.Height)
                                ,inverseTransform);

                            //release device buffer
                            cbsrcImage.Dispose();

#if DEBUG_METETIME
                            dm.AddLine("InterpolationMethod.Bilinear");
                            dm.Write2Debug(true);
#endif

                        }
                    }
                }
            }
            catch (System.Exception exp)
            {
                if (image != null)
                {
                    image.Dispose();
                    image = null;
                }

                throw exp;
            }
            finally
            {
            }

            return image;
        }

#endif
    }


    public class ReferenceImageProcessor
    {
        #region Create
        public static GreyDataImage CreateRefImage(
            List<string> refFiles, eGoldenImageMethod method)
        {
            if (refFiles == null || refFiles.Count < 3)
                throw new System.ArgumentException("Please specify at least 3 files to process!");

            GreyDataImage refImage = null;

            string userAppFolder = Application.UserAppDataPath;
            string tmpFolder = Path.Combine(userAppFolder, "SIA\\RefImages\\");

            try
            {
                PathHelper.CreateMissingFolderAuto(tmpFolder);

                int nRefFiles = refFiles.Count;
                int iKeyFile = 0;
                string keyRefFile = refFiles[iKeyFile];

                MetrologySystemReference dstRefFile =
                    MetrologySystemReference.Deserialize(keyRefFile);

                string[] sampleFiles = new string[refFiles.Count];

#if PARALLEL
                Parallel.For(0, nRefFiles, delegate(int iRefFile)
#else
                for (int iRefFile = 0; iRefFile < nRefFiles; iRefFile++)
#endif
                {
                    string tmpFile = Path.Combine(
                            tmpFolder, string.Format("refFile_{0}.bmp", iRefFile));

                    MetrologySystemReference refFile = 
                        MetrologySystemReference.Deserialize(refFiles[iRefFile]);
                    
                    using (GreyDataImage image = ReferenceImageAlignmentHelper.AlignImage(
                        refFile, dstRefFile.AlignmentSettings, 
                        dstRefFile.DeviceLeft, dstRefFile.DeviceTop, dstRefFile.DeviceOrientation))
                    {
                        sampleFiles[iRefFile] = tmpFile;

                        image.SaveImage(tmpFile, SIA.Common.eImageFormat.Bmp);
                    }
                }
#if PARALLEL
                );
#endif

                GoldenImageCreator creator = new GoldenImageCreator();
                refImage = creator.CreateGoldenImage(sampleFiles, method);
            }
            catch (System.Exception exp)
            {
                if (refImage != null)
                {
                    refImage.Dispose();
                    refImage = null;
                }

                throw exp;
            }
            finally
            {
                try
                {
                    if (Directory.Exists(tmpFolder))
                    {
                        Directory.Delete(tmpFolder, true);
                    }
                }
                catch
                {
                }
            }

            return refImage;
        }        
        #endregion Create

        #region Subtract
        public static BinaryImage CompareImageToRefImage(
            MetrologySystemReference srcRefFile, 
            GreyDataImage sample, double darkThreshold, double brightThreshold)
        {
            BinaryImage binarizedImage = null;
            try
            {
                #region detect coordinate system
                AlignmentSettings settings = srcRefFile.AlignmentSettings;
                AlignerBase aligner = AlignerFactory.CreateInstance(settings);
                AlignmentResult alignmentResult = aligner.Align(sample);
                #endregion detect coordinate system

                binarizedImage = CompareImageToRefImage(
                    srcRefFile, alignmentResult, sample, darkThreshold, brightThreshold);
            }
            catch (System.Exception exp)
            {
                if (binarizedImage != null)
                {
                    binarizedImage.Dispose();
                    binarizedImage = null;
                }

                throw exp;
            }
            finally
            {
            }

            return binarizedImage;
        }

        public static BinaryImage CompareImageToRefImage(
            MetrologySystemReference srcRefFile,
            AlignmentResult alignmentResult,
            GreyDataImage sample, 
            double darkThreshold, double brightThreshold)
        {
            BinaryImage binarizedImage = null;
            GreyDataImage filteredImage = null;
            try
            {
                #region update coordinate system
                AlignmentSettings settings = srcRefFile.AlignmentSettings;
                AlignmentResult alignment = alignmentResult;
                float deviceLeft = alignment.GetLeft(settings.NewWidth, settings.NewHeight);
                float deviceTop = alignment.GetTop(settings.NewWidth, settings.NewHeight);
                float deviceAngle = alignment.GetRotateAngle(settings.NewWidth, settings.NewHeight);
                #endregion update coordinate system



                #region filter image
                filteredImage = sample.Copy();
                int kernelWidth = AnomalyDetectorDefinition.ABSKernelWidthFilter;
                int kernelHeight = AnomalyDetectorDefinition.ABSKernelHeightFilter;
                bool isAuto = AnomalyDetectorDefinition.ABSAutoNoiseFilter;
                double noiseLevel = AnomalyDetectorDefinition.ABSNoiseLevelFilter;
                unsafe
                {
                    WienerProcessor processor = new WienerProcessor(
                        filteredImage._aData,
                        filteredImage.Width, filteredImage.Height,
                        kernelWidth, kernelHeight);
                    processor.Filter(isAuto, noiseLevel);
                    processor = null;
                }
                #endregion filter image                

                #region binarize
                binarizedImage = Binarize(
                    srcRefFile, filteredImage, sample,
                    deviceLeft, deviceTop, deviceAngle,
                    darkThreshold, brightThreshold);
                #endregion binarize
            }
            catch (System.Exception exp)
            {
                if (binarizedImage != null)
                {
                    binarizedImage.Dispose();
                    binarizedImage = null;
                }

                throw exp;
            }
            finally
            {
                if (filteredImage != null)
                {
                    filteredImage.Dispose();
                    filteredImage = null;
                }
            }

            return binarizedImage;
        }

        protected static BinaryImage Binarize(
            MetrologySystemReference srcRefFile, 
            GreyDataImage filteredImage, GreyDataImage originImage,
            float deviceLeft, float deviceTop, float deviceAngle,
            double darkThreshold, double brightThreshold)
        {
            BinaryImage binarizedImage = null;
            try
            {
                GoldenImageProcessor processor = new GoldenImageProcessor();
                processor.SetKernelSize(7, 7); // ABS
                processor.SetKernelSize(9, 9); // ABS

                using (GreyDataImage goldenImage = ReferenceImageAlignmentHelper.AlignImage(
                    srcRefFile, deviceLeft, deviceTop, deviceAngle))
                {
#if DEBUG && TEST
                    goldenImage.SaveImage(@"D:\temp\abs-test-detect-anomalies.bmp", 
                        SIA.Common.eImageFormat.Bmp);
#endif

                    binarizedImage = processor.Binarize(
                        goldenImage, filteredImage, originImage, darkThreshold, brightThreshold);
                }
            }
            catch (System.Exception exp)
            {
                if (binarizedImage != null)
                {
                    binarizedImage.Dispose();
                    binarizedImage = null;
                }

                throw exp;
            }

            return binarizedImage;
        }        
        #endregion Subtract
    }

    public class ReferenceImageProcessorEx
    {
        #region Subtract
        public static int[] CompareImageToRefImage(
            MetrologySystemReference srcRefFile,
            GreyDataImage sample, double darkThreshold, double brightThreshold, Region regions)
        {
            int[] interestedPixels = null;
            try
            {
                #region detect coordinate system
                AlignmentSettings settings = srcRefFile.AlignmentSettings;
                AlignerBase aligner = AlignerFactory.CreateInstance(settings);
                AlignmentResult alignmentResult = aligner.Align(sample);
                #endregion detect coordinate system

                interestedPixels = CompareImageToRefImage(
                    srcRefFile, alignmentResult, sample, darkThreshold, brightThreshold, regions);
            }
            catch (System.Exception exp)
            {
                interestedPixels = null;

                throw exp;
            }
            finally
            {
            }

            return interestedPixels;
        }

        public static int[] CompareImageToRefImage(
            MetrologySystemReference srcRefFile,
            AlignmentResult alignmentResult,
            GreyDataImage sample,
            double darkThreshold, double brightThreshold,
            Region regions)
        {
            int[] interestedPixels = null;
            GreyDataImage filteredImage = null;
            try
            {
#if DEBUG_METETIME
                DebugMeteTime dm = new DebugMeteTime();
#endif

                #region update coordinate system
                AlignmentSettings settings = srcRefFile.AlignmentSettings;
                AlignmentResult alignment = alignmentResult;
                float deviceLeft = alignment.GetLeft(settings.NewWidth, settings.NewHeight);
                float deviceTop = alignment.GetTop(settings.NewWidth, settings.NewHeight);
                float deviceAngle = alignment.GetRotateAngle(settings.NewWidth, settings.NewHeight);
                #endregion update coordinate system

#if DEBUG_METETIME
                dm.AddLine("update coordinate system");
#endif

                #region filter image
                filteredImage = sample.Copy();

#if DEBUG_METETIME
                dm.AddLine("Copy Image");
#endif

                int kernelWidth = AnomalyDetectorDefinition.ABSKernelWidthFilter;
                int kernelHeight = AnomalyDetectorDefinition.ABSKernelHeightFilter;
                bool isAuto = AnomalyDetectorDefinition.ABSAutoNoiseFilter;
                double noiseLevel = AnomalyDetectorDefinition.ABSNoiseLevelFilter;
                unsafe
                {
                    WienerProcessor processor = null;

#if GPU_SUPPORTED
                    processor = new WienerProcessorGPU(
                        filteredImage._aData,
                        filteredImage.Width, filteredImage.Height,
                        kernelWidth, kernelHeight);
#else
                    processor = new WienerProcessor(
                        filteredImage._aData,
                        filteredImage.Width, filteredImage.Height,
                        kernelWidth, kernelHeight);
#endif

                    processor.Filter(isAuto, noiseLevel);
                    processor = null;

                }
                #endregion filter image

#if DEBUG_METETIME
                dm.AddLine("Filter Image");
#endif

                #region binarize
                interestedPixels = GetInterestedPixels(
                    srcRefFile, filteredImage, sample,
                    deviceLeft, deviceTop, deviceAngle,
                    darkThreshold, brightThreshold, regions);
                #endregion binarize

#if DEBUG_METETIME
                dm.AddLine("Binarize");
                dm.Write2Debug(true);
#endif
            }
            catch (System.Exception exp)
            {
                interestedPixels = null;

                throw exp;
            }
            finally
            {
                if (filteredImage != null)
                {
                    filteredImage.Dispose();
                    filteredImage = null;
                }
            }

            return interestedPixels;
        }

        protected static int[] GetInterestedPixels(
            MetrologySystemReference srcRefFile,
            GreyDataImage filteredImage, GreyDataImage originImage,
            float deviceLeft, float deviceTop, float deviceAngle,
            double darkThreshold, double brightThreshold, Region regions)
        {
            int[] interestedPixels = null;
            try
            {

#if DEBUG_METETIME
                DebugMeteTime dm = new DebugMeteTime();
#endif

                GoldenImageProcessor processor = new GoldenImageProcessor();
                processor.SetKernelSize(7, 7); // ABS
                processor.SetKernelSize(9, 9); // ABS

#if DEBUG_METETIME
                dm.AddLine("new GoldenImageProcesso");
#endif

                using (GreyDataImage goldenImage = ReferenceImageAlignmentHelper.AlignImage(
                    srcRefFile, deviceLeft, deviceTop, deviceAngle))
                {
#if DEBUG && TEST
                    goldenImage.SaveImage(@"D:\temp\abs-test-detect-anomalies.bmp", 
                        SIA.Common.eImageFormat.Bmp);
#endif

#if DEBUG_METETIME
                    dm.AddLine("ReferenceImageAlignmentHelper.AlignImage");
#endif

                    processor.Regions = regions;
                    interestedPixels = processor.GetInterestedPixels(
                        goldenImage, filteredImage, originImage, darkThreshold, brightThreshold);

#if DEBUG_METETIME
                    dm.AddLine("GetInterestedPixels");
                    dm.Write2Debug(true);
#endif
                }
            }
            catch (System.Exception exp)
            {
                interestedPixels = null;

                throw exp;
            }

            return interestedPixels;
        }


#if GPU_SUPPORTED

        public static int[] CompareImageToRefImage(
            MetrologySystemReference srcRefFile,
            CommonImage sample, double darkThreshold, double brightThreshold, Region regions)
        {
            if (!DeviceProgram.GPUSupported)
                return CompareImageToRefImage(srcRefFile, sample.RasterImage, darkThreshold, brightThreshold, regions);

            int[] interestedPixels = null;
            try
            {
                #region detect coordinate system
                AlignmentSettings settings = srcRefFile.AlignmentSettings;
                AlignerBase aligner = AlignerFactory.CreateInstance(settings);
                AlignmentResult alignmentResult = aligner.Align(sample.RasterImage);
                #endregion detect coordinate system

                interestedPixels = CompareImageToRefImage(
                    srcRefFile, alignmentResult, sample, darkThreshold, brightThreshold, regions);
            }
            catch (System.Exception exp)
            {
                interestedPixels = null;

                throw exp;
            }
            finally
            {
            }

            return interestedPixels;
        }

        public static int[] CompareImageToRefImage(
                 MetrologySystemReference srcRefFile,
                 AlignmentResult alignmentResult,
                 CommonImage sample,
                 double darkThreshold, double brightThreshold,
                 Region regions)
        {
            if (!DeviceProgram.GPUSupported)
                return CompareImageToRefImage(srcRefFile, alignmentResult, sample.RasterImage, darkThreshold, brightThreshold, regions);

            int[] interestedPixels = null;
            DeviceBuffer<ushort> filteredImage = null;

            try
            {
#if DEBUG_METETIME
                DebugMeteTime dm = new DebugMeteTime();
#endif

                #region update coordinate system
                AlignmentSettings settings = srcRefFile.AlignmentSettings;
                AlignmentResult alignment = alignmentResult;
                float deviceLeft = alignment.GetLeft(settings.NewWidth, settings.NewHeight);
                float deviceTop = alignment.GetTop(settings.NewWidth, settings.NewHeight);
                float deviceAngle = alignment.GetRotateAngle(settings.NewWidth, settings.NewHeight);
                #endregion update coordinate system

#if DEBUG_METETIME
                dm.AddLine("update coordinate system");
#endif

                #region filter image
                filteredImage = sample.CloneDeviceMemory();

#if DEBUG_METETIME
                dm.AddLine("Copy Image");
#endif

                int kernelWidth = AnomalyDetectorDefinition.ABSKernelWidthFilter;
                int kernelHeight = AnomalyDetectorDefinition.ABSKernelHeightFilter;
                bool isAuto = AnomalyDetectorDefinition.ABSAutoNoiseFilter;
                double noiseLevel = AnomalyDetectorDefinition.ABSNoiseLevelFilter;
                unsafe
                {
                    WienerProcessor processor = null;

                    processor = new WienerProcessorGPU(
                        null,
                        filteredImage,//get Device Buffer from cache
                        sample.Width, sample.Height,
                        kernelWidth, kernelHeight);

                    processor.Filter(isAuto, noiseLevel);//filtered data in device buffer
                    processor = null;
                }
                #endregion filter image

#if DEBUG_METETIME
                dm.AddLine("Filter Image");
#endif

                #region binarize
                interestedPixels = GetInterestedPixels(
                    srcRefFile, filteredImage,sample,
                    deviceLeft, deviceTop, deviceAngle,
                    darkThreshold, brightThreshold, regions);
                #endregion binarize

#if DEBUG_METETIME
                dm.AddLine("Binarize");
                dm.Write2Debug(true);
#endif

            }
            catch (System.Exception exp)
            {
                interestedPixels = null;

                throw exp;
            }
            finally
            {
                if (filteredImage != null)
                {
                    //will be changed to create with CommonImage Constructor
                    filteredImage.Dispose();
                    filteredImage = null;
                }
            }

            return interestedPixels;
        }

        protected static int[] GetInterestedPixels(
                   MetrologySystemReference srcRefFile,
                   DeviceBuffer<ushort> filteredImage, CommonImage originImage,
                   float deviceLeft, float deviceTop, float deviceAngle,
                   double darkThreshold, double brightThreshold, Region regions)
        {
            int[] interestedPixels = null;
            try
            {

#if DEBUG_METETIME
                DebugMeteTime dm = new DebugMeteTime();
#endif

                GoldenImageProcessor processor = new GoldenImageProcessor();
                processor.SetKernelSize(7, 7); // ABS
                processor.SetKernelSize(9, 9); // ABS

#if DEBUG_METETIME
                dm.AddLine("new GoldenImageProcesso");
#endif

                using (DeviceBuffer<ushort> goldenImage = ReferenceImageAlignmentHelper.AlignImageDeviceBuffer(
           srcRefFile, deviceLeft, deviceTop, deviceAngle))
                {
#if DEBUG_METETIME
                    dm.AddLine("ReferenceImageAlignmentHelper.AlignImage");
#endif
                    processor.Regions = regions;

                    interestedPixels = processor.GetInterestedPixels(
                        goldenImage,filteredImage, originImage, darkThreshold, brightThreshold);

#if DEBUG_METETIME
                    dm.AddLine("GetInterestedPixels");
                    dm.Write2Debug(true);
#endif
                }
            }
            catch (System.Exception exp)
            {
                interestedPixels = null;
                throw exp;
            }

            return interestedPixels;
        }

#endif

        #endregion Subtract



    }

   
}
