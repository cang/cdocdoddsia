using System;
using System.Data;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

using SIA.Common;
using SIA.Common.KlarfExport;
using SIA.Common.Imaging;
using SIA.Common.Imaging.Filters;
using SIA.Common.PatternRecognition;
using SIA.Common.GoldenImageApproach;

using SIA.IPEngine;

using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;

using SIA.UI.Components;
using SIA.UI.Components.Common;
using SIA.UI.Components.Renders;

using SIA.UI.Controls;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Helpers;
using SIA.UI.Controls.Helpers.VisualAnalysis;
using SIA.UI.Controls.UserControls;
using SIA.UI.Controls.Automation.Commands;

using SIA.SystemLayer;
using SiGlaz.Common;
using SIA.SystemLayer.ImageProcessing;
using SiGlaz.Common.ImageAlignment;
using SIA.Algorithms.ReferenceFile;

namespace SIA.UI.Controls.Commands
{
    public partial class CommandExecutor
    {
        public kHistogram DoCommandComputeIntensityHistogram(CommonImage image)
        {
            object[] result = this.DoCommand(typeof(ComputeIntensityHistogramCommand), image);
            return result != null && result.Length >= 1 && result[0] is kHistogram ? (kHistogram)result[0] : null;
        }

        public void DoCommandCameraCorrection(CommonImage image, float focalLength, PointF ptPrincipal, float[] distCoeffs, bool interpolation)
        {
            this.DoCommand(typeof(CameraCorrectionCommand), image, focalLength, ptPrincipal, distCoeffs, interpolation);
        }

        public void DoCommandResizeImage(CommonImage image, ResizeImageCommandSettings settings)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            if (settings == null)
                throw new ArgumentNullException("settings");
            this.DoCommand(typeof(ResizeImageCommand), image, settings);
        }

        public void DoCommandRotateImage(CommonImage image, float degree)
        {
            this.DoCommand(typeof(RotateImageCommand), image, degree);
        }

        public void DoCommandFlipImageHorizontal(CommonImage image)
        {
            this.DoCommand(typeof(FlipImageCommand), image, (int)FlipType.Horizontal);
        }

        public void DoCommandFlipImageVertical(CommonImage image)
        {
            this.DoCommand(typeof(FlipImageCommand), image, (int)FlipType.Vertical);
        }

        public void DoCommandMonadic(CommonImage image, string type, float value)
        {
            this.DoCommand(typeof(MonadicCommand), image, type, value);
        }

        public void DoCommandDyadic(CommonImage image, string type, string filename)
        {
            this.DoCommand(typeof(DyadicCommand), image, type, filename);
        }

        public void DoCommandGbcErosion(CommonImage image, int num_pass)
        {
            this.DoCommand(typeof(GbcErosionCommand), image, num_pass);
        }

        public void DoCommandGbcFFT(CommonImage image, int threshold, float cutoff)
        {
            this.DoCommand(typeof(GbcFFTCommand), image, threshold, cutoff);
        }

        public void DoCommandGbcRefImages(CommonImage image, string[] FilePaths)
        {
            this.DoCommand(typeof(GbcRefImageCommand), image, FilePaths);
        }

        public void DoCommandGbcUnsharp(CommonImage image, UnsharpParam arg)
        {
            this.DoCommand(typeof(GbcUnsharpCommand), image, arg);
        }

        public void DoCommandExtBckgndFFT(CommonImage image, int threshold, float cutoff)
        {
            this.DoCommand(typeof(ExtBckgndFFTCommand), image, threshold, cutoff);
        }

        public void DoCommandExtBckgndErosion(CommonImage image, int num_pass)
        {
            this.DoCommand(typeof(ExtBckgndErosionCommand), image, num_pass);
        }

        public void DoCommandThreshold(CommonImage image, ThresholdCommandSettings settings)
        {
            this.DoCommand(typeof(ThresholdCommand), image, settings);
        }

        public void DoCommandStretchColor(CommonImage image, int minimum, int maximum)
        {
            this.DoCommand(typeof(StretchColorCommand), image, minimum, maximum);
        }

        public void DoCommandInvert(CommonImage image)
        {
            this.DoCommand(typeof(InvertCommand), image);
        }

        public void DoCommandLightenUp(CommonImage image)
        {
            this.DoCommand(typeof(LightenUpCommand), image);
        }

        public void DoCommandHistEqualize(CommonImage image)
        {
            this.DoCommand(typeof(HistEqualizeCommand), image);
        }

        public void DoCommandConvolution(CommonImage image, eMaskType MaskType, eMatrixType MatrixType, int num_pass, float threshold)
        {
            //this.DoCommand(typeof(ConvolutionCommand), image, MaskType, MatrixType, num_pass, threshold);
            ConvolutionCommandSettings cmdSettings =
                new ConvolutionCommandSettings(MaskType, MatrixType, num_pass, threshold);
            this.DoCommand(typeof(ConvolutionCommand), image, cmdSettings);
        }

        public void DoCommandCustomConvolution(CommonImage image, int[,] matrix, int num_pass)
        {
            this.DoCommand(typeof(CustConvolutionCommand), image, matrix, num_pass);
        }

        public void DoCommandMorphology(CommonImage image, eMorphType MaskType, eMatrixType MatrixType, int num_pass)
        {
            this.DoCommand(typeof(MorphologyCommand), image, MaskType, MatrixType, num_pass);
        }
    }
}
