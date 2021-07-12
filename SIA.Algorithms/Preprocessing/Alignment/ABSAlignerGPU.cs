#define PARALLEL__
#define DEBUG_METETIME_

#if GPU_SUPPORTED

/* for optimize
ImageInterpolator.AffineTransform :: 27840
GradientMapMatcherGPU Totals:: 49125
CorrelationMatcher :: 2835
*/

using System;
using System.Collections.Generic;
using System.Text;
using SiGlaz.Common.ImageAlignment;
using SIA.IPEngine;
using SDD = System.Drawing.Drawing2D;
using System.Drawing;
using SIA.Algorithms.Preprocessing.Matching;
using SIA.Algorithms.Preprocessing.Interpolation;
using System.Diagnostics;
using SiGlaz.Cloo;
using SIA.SystemFrameworks;

namespace SIA.Algorithms.Preprocessing.Alignment
{
    public class ABSAlignerGPU : ABSAligner
    {
        DeviceBuffer<ushort> cbImage;
        public ABSAlignerGPU(AlignmentSettings settings)
            : base(settings)
        {
            //DeviceProgram.InitCL();//temporary 
        }

        protected override void GradientMapMatching(
         int isample, ushort[] roiData, double[] weightSet,
         System.Drawing.Drawing2D.Matrix transform,
         int featureCount, int pivotCount,
         ref double confidence, ref int shiftX, ref int shiftY)
        {
            if(!DeviceProgram.GPUSupported)
            {
                base.GradientMapMatching(isample,roiData,weightSet,transform,featureCount, pivotCount,
                    ref confidence, ref shiftX, ref shiftY);
                return;
            }

            //GPU
            int roiWidth = _settings.SampleWidth + 2 * _settings.SampleExpandWidth;
            int roiHeight = _settings.SampleHeight + 2 * _settings.SampleExpandHeight;
            float halfRoiWidth = roiWidth * 0.5f;
            float halfRoiHeight = roiHeight * 0.5f;

#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif

            transform.Translate(
                (float)_settings.SampleXCoordinates[isample] - halfRoiWidth,
                (float)_settings.SampleYCoordinates[isample] - halfRoiHeight,
                SDD.MatrixOrder.Prepend);

            //Proi device data
            DeviceBuffer<ushort> cbProi = 
                        Interpolation.ImageInterpolator.AffineTransform(
                            InterpolationMethod.Bilinear,
                            cbImage,//gpu buffer 
                            new Size(_image._width, _image._height),
                            new Size(roiWidth, roiHeight)
                            ,transform);

#if DEBUG_METETIME
            dm.AddLine("ImageInterpolator.AffineTransform :");
#endif

#if DEBUG && TEST
            SIADebugger.SaveImage(
                roiData, roiWidth, roiHeight,
                string.Format("D:\\temp\\test\\sample_{0}.bmp", isample));
#endif

            GradientMapMatcherGPU ratingProcessor = new GradientMapMatcherGPU(featureCount, pivotCount);
            ratingProcessor.SampleDataDeviceBuffer = cbProi;
            unsafe
            {
                fixed (ushort* patternData = _settings.SampleData[isample])
                {
                    ratingProcessor.Match(
                        null, roiWidth, roiHeight,
                        patternData, _settings.SampleWidth, _settings.SampleHeight, 0,
                        out confidence, out shiftX, out shiftY);
                }
            }


#if DEBUG_METETIME
            dm.AddLine("GradientMapMatcherGPU Totals:");
#endif

            double[] refinedWeightSet = ratingProcessor.WeightSet;
            refinedWeightSet = weightSet;

            CorrelationMatcherGPU matcher =  new CorrelationMatcherGPU();
            matcher.SampleDataDeviceBuffer = cbProi;
            matcher.Match(
                roiData, roiWidth, roiHeight,
                _settings.SampleData[isample], _settings.SampleWidth, _settings.SampleHeight,
                refinedWeightSet, //weightSet, 
                0,
                ratingProcessor.InterestedXList, ratingProcessor.InterestedYList,
                out confidence, out shiftX, out shiftY);

#if DEBUG_METETIME
            dm.AddLine("CorrelationMatcher :");
            dm.Write2Debug(true);
#endif

#if DEBUG && TEST
            //SIADebugger.SaveImage(
            //    roiData, roiWidth, roiHeight,
            //    string.Format("D:\\temp\\test\\sample_{0}.bmp", isample));

            SIADebugger.SaveImage(
                _settings.SampleData[isample], _settings.SampleWidth, _settings.SampleHeight,
                string.Format("D:\\temp\\test\\pattern_{0}.bmp", isample));

            int[] bestMatchs = new int[] { (shiftY + roiHeight/2) * roiWidth + (shiftX + roiWidth/2) };
            SIADebugger.SaveImages(
                roiData, roiWidth, roiHeight,
                _settings.SampleData[isample], _settings.SampleWidth, _settings.SampleHeight,
                ratingProcessor.PivotIndices,
                bestMatchs,
                string.Format("D:\\temp\\test\\gradientmap_sample_pattern_{0}.bmp", isample));
#endif

            // set null here, hope that GC will collect successfully
            roiData = null;
            cbProi.Dispose();
            cbProi = null;
        }

        //override only to avoid PARALLEL
        protected override void MatchKeypointsParallely(
           int idxBegin, int length, SDD.Matrix[] clonedDrafts,
           double deltaX, double deltaY,
           double[] xArray, double[] yArray,
           double[] shiftXArray, double[] shiftYArray,
           bool[] maskArray, double[] confidences)
        {
            if(!DeviceProgram.GPUSupported)
            {
                base.MatchKeypointsParallely(idxBegin, length, clonedDrafts, deltaX, deltaY, xArray
                    , yArray, shiftXArray, shiftYArray, maskArray, confidences);
                return;
            }


#if DEBUG_METETIME
            DebugMeteTime dm = new DebugMeteTime();
#endif
            unsafe
            {
                cbImage = DeviceBuffer<ushort>.CreateHostBufferReadOnly(_image._lenght, (IntPtr)_image._aData);
                //cbImage = DeviceBuffer<ushort>.CreateBufferReadOnly(_image._lenght, (IntPtr)_image._aData);
            }

#if DEBUG_METETIME
            dm.AddLine("MatchKeypointsParallely : CreateBufferReadOnly");
            dm.Write2Debug(true);
#endif


#if !PARALLEL
            for (int isample = idxBegin; isample < idxBegin + length; isample++)
#endif

#if PARALLEL
            Parallel.For(idxBegin, idxBegin + length, delegate(int isample)
#endif
            {
                int shiftX = 0, shiftY = 0;
                double corrCoeff = 0;

                using (SDD.Matrix getDraft = clonedDrafts[isample])
                {
                    // save
                    double seedX = _settings.SampleXCoordinates[isample];
                    double seedY = _settings.SampleYCoordinates[isample];
                    try
                    {
                        _settings.SampleXCoordinates[isample] += deltaX;
                        _settings.SampleYCoordinates[isample] += deltaY;

                        MatchKeyPoint(
                            isample, null, _settings.SampleWeightSet[isample],
                            getDraft,
                            ref corrCoeff, ref shiftX, ref shiftY);

                        shiftX = (int)Math.Round(shiftX + deltaX);
                        shiftY = (int)Math.Round(shiftY + deltaY);
                    }
                    finally
                    {
                        _settings.SampleXCoordinates[isample] = seedX;
                        _settings.SampleYCoordinates[isample] = seedY;
                    }
                }

                // confident
                confidences[isample] = corrCoeff;

                xArray[isample] = _settings.SampleXCoordinates[isample];
                yArray[isample] = _settings.SampleYCoordinates[isample];
                shiftXArray[isample] = shiftX;
                shiftYArray[isample] = shiftY;

                if (corrCoeff < _settings.MinCoorelationCoefficient)
                {
                    maskArray[isample] = false;
                }
                else
                {
                    maskArray[isample] = true;
                }
            }
#if PARALLEL
);
#endif

            if (cbImage != null)
                cbImage.Dispose();

        }


        //override only to debug
//        protected override SDD.Matrix AlignUsingMultipeKeypointsParallely(
//          GreyDataImage image, SDD.Matrix draft)
//        {
//            // Initialize
//            int nSamples = _settings.SampleCount;

//            double[] xArray = new double[nSamples];
//            double[] yArray = new double[nSamples];
//            double[] shiftXArray = new double[nSamples];
//            double[] shiftYArray = new double[nSamples];
//            bool[] maskArray = new bool[nSamples];
//            double[] confidences = new double[nSamples];
//            SDD.Matrix[] clonedDrafts = new SDD.Matrix[nSamples];
//            for (int i = 0; i < nSamples; i++)
//            {
//                clonedDrafts[i] = draft.Clone();
//            }

//            double deltaX = 0;
//            double deltaY = 0;

//            // save
//            int oldExpandingWidth = _settings.SampleExpandWidth;
//            int oldExpandingHeight = _settings.SampleExpandHeight;

//            // Match Keypoints
//            int idxBeginMatch = 0;
//            bool bIsDraftHighConfidence = true;
//            if (_settings.SampleCount > _settings.FineTuneNumSampleMatchFirstly &&
//                _settings.FineTuneNumSampleMatchFirstly > 0)
//            {
//                _settings.SampleExpandWidth = (int)(_settings.SampleExpandWidth * 1.25);
//                _settings.SampleExpandHeight = (int)(_settings.SampleExpandHeight * 1.25);

//#if DEBUG
//                Stopwatch sw = new Stopwatch(); sw.Start();
//#endif

//                MatchKeypointsParallely(
//                    idxBeginMatch, _settings.FineTuneNumSampleMatchFirstly,
//                    clonedDrafts, deltaX, deltaY,
//                    xArray, yArray, shiftXArray, shiftYArray, maskArray, confidences);
//                idxBeginMatch = _settings.FineTuneNumSampleMatchFirstly;

//#if DEBUG
//                sw.Stop(); Debug.WriteLine("MatchKeypointsParallely1 :  " + sw.ElapsedTicks); sw.Reset(); sw.Start();
//#endif
//                for (int isample = 0; isample < _settings.FineTuneNumSampleMatchFirstly; isample++)
//                {
//                    deltaX += shiftXArray[isample];
//                    deltaY += shiftYArray[isample];

//#if TEST
//                    System.Diagnostics.Trace.WriteLine(
//                        string.Format("shiftX: {0}", shiftXArray[isample]));
//                    System.Diagnostics.Trace.WriteLine(
//                        string.Format("shiftY: {0}", shiftYArray[isample]));
//#endif

//                    if (confidences[isample] < _settings.FineTuneHighConfThres)
//                    {
//#if TEST
//                        System.Diagnostics.Trace.WriteLine(        
//                            string.Format("Confidenceeeeeeeee: {0}", confidences[isample]));
//#endif

//                        bIsDraftHighConfidence = false;
//                        break;
//                    }
//                }


//                if (bIsDraftHighConfidence)
//                {
//#if TEST
//                    System.Diagnostics.Trace.WriteLine("We wonnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn");                    
//#endif

//                    deltaX = deltaX / _settings.FineTuneNumSampleMatchFirstly;
//                    deltaY = deltaY / _settings.FineTuneNumSampleMatchFirstly;

//#if TEST
//                    System.Diagnostics.Trace.WriteLine(
//                        string.Format("deltaX: {0}", deltaX));
//                    System.Diagnostics.Trace.WriteLine(
//                        string.Format("deltaY: {0}", deltaY));
//#endif

//                    _settings.SampleExpandWidth = (int)(_settings.SampleExpandWidth * _settings.FineTuneExpandDecreaseFactor);
//                    _settings.SampleExpandHeight = (int)(_settings.SampleExpandHeight * _settings.FineTuneExpandDecreaseFactor);

//                    // I'm very expect that hence the location only around pivot markers
//                    // reduce computing cost by descreasing scanning area :)
//                    if (_settings.SampleExpandWidth > 15)
//                        _settings.SampleExpandWidth = 15;
//                    if (_settings.SampleExpandHeight > 15)
//                        _settings.SampleExpandHeight = 15;

//                    ResetPrivateParamsAfterPruning();
//                }
//                else
//                {
//                    deltaX = 0;
//                    deltaY = 0;
//                }

//                MatchKeypointsParallely(
//                    idxBeginMatch, nSamples - _settings.FineTuneNumSampleMatchFirstly,
//                    clonedDrafts, deltaX, deltaY,
//                    xArray, yArray, shiftXArray, shiftYArray, maskArray, confidences);
//                // restore
//                _settings.SampleExpandWidth = oldExpandingWidth;
//                _settings.SampleExpandHeight = oldExpandingHeight;
//#if DEBUG
//                sw.Stop(); Debug.WriteLine("MatchKeypointsParallely2 :  " + sw.ElapsedTicks);
//#endif
//            }
//            else
//            {
//                MatchKeypointsParallely(
//                    idxBeginMatch, nSamples,
//                    clonedDrafts, deltaX, deltaY,
//                    xArray, yArray, shiftXArray, shiftYArray, maskArray, confidences);
//            }



//            List<float> shiftXList = new List<float>();
//            List<float> shiftYList = new List<float>();

//            List<float> affectedXList = new List<float>();
//            List<float> affectedYList = new List<float>();

//            _matchedXList = xArray;
//            _matchedYList = yArray;
//            _matchedConfiences = confidences;

//            // update affected list here
//            for (int isample = 0; isample < nSamples; isample++)
//            {
//                if (maskArray[isample])
//                {
//                    affectedXList.Add((float)xArray[isample]);
//                    affectedYList.Add((float)yArray[isample]);
//                    shiftXList.Add((float)shiftXArray[isample]);
//                    shiftYList.Add((float)shiftYArray[isample]);
//                }

//                _matchedXList[isample] += shiftXArray[isample];
//                _matchedYList[isample] += shiftYArray[isample];
//            }

//            confidences = null;
//            int minAffectedKeypoints = _settings.MinAffectedKeypoints;
//            minAffectedKeypoints = 4;
//            if (shiftXList.Count < minAffectedKeypoints)
//                return draft.Clone();

//#if TEST
//            System.Diagnostics.Trace.WriteLine(
//                string.Format(
//                "We wonnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn: {0}/{1}", 
//                shiftXList.Count, _settings.SampleCount));
//#endif

//            SDD.Matrix fineTune = EstimateAffineOfKeypoints(affectedXList.ToArray(), affectedYList.ToArray(),
//                shiftXList.ToArray(), shiftYList.ToArray());

//            SDD.Matrix result = fineTune.Clone();
//            result.Multiply(draft, System.Drawing.Drawing2D.MatrixOrder.Append);

//            return result;
//        }

    }
}

#endif