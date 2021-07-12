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
        public void DoCommandFilterSmooth(CommonImage image)
        {
            this.DoCommand(typeof(FilterSmoothCommand), image);
        }

        public void DoCommandFilterSharpening(CommonImage image)
        {
            this.DoCommand(typeof(FilterSharpeningCommand), image);
        }

        public void DoCommandFilterLaplacian(CommonImage image)
        {
            this.DoCommand(typeof(FilterLaplacianCommand), image);
        }

        public void DoCommandFilterGaussian(CommonImage image)
        {
            this.DoCommand(typeof(FilterGaussianCommand), image);
        }

        public void DoCommandFilterEdgeDetection(CommonImage image)
        {
            this.DoCommand(typeof(FilterEdgeDetectionCommand), image);
        }

        public void DoCommandFilterEmboss135(CommonImage image)
        {
            this.DoCommand(typeof(FilterEmboss135Command), image);
        }

        public void DoCommandFilterEmboss90(CommonImage image)
        {
            this.DoCommand(typeof(FilterEmboss90Command), image);
        }

        public void DoCommandFilterFFT(CommonImage image, FFTFilterType type, float cutoff, float weight)
        {
            this.DoCommand(typeof(FilterFFTCommand), image, type, cutoff, weight);
        }

        public void DoCommandFilterVariance(CommonImage image, float radius, float weight)
        {
            this.DoCommand(typeof(FilterVarianceCommand), image, radius, weight);
        }

        public void DoCommandFilterRank(CommonImage image, int typeFilter, int kernel, int pass)
        {
            this.DoCommand(typeof(FilterRankCommand), image, typeFilter, kernel, pass);
        }

        public void DoCommandFilterWiener(
            CommonImage image, int kernelWidth, int kernelHeight, bool isAuto, double noiseLevel)
        {
            this.DoCommand(typeof(FilterWienerCommand), image, kernelWidth, kernelHeight, isAuto, noiseLevel);
        }
    }
}
