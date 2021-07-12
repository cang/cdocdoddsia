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
        public ArrayList DoCommandDetectObjects(CommonImage image, ObjectDetectionSettings settings)
        {
            object[] result = this.DoCommand(typeof(DetectObjectCommand), image, settings);
            return result != null && result.Length >= 1 && result[0] is ArrayList ? (ArrayList)result[0] : null;
        }

        public object[] DoCommandDetectCoordinateSystem(CommonImage image, MetrologySystemReference refFile)
        {
            object[] result = this.DoCommand(typeof(DetectCoordinateSystemCommand), image, refFile);
            return result;
        }

        public object[] DoCommandAlignPoleTip(CommonImage image, SiGlaz.Common.ImageAlignment.Settings settings)
        {
            object[] result = this.DoCommand(typeof(AlignPoleTipCommand), image, settings);
            return result;
        }

        public ArrayList DoCommandDetectAnomalies(
            CommonImage image, object refFilePath,
            eGoldenImageMethod method, double darkThreshold, double brightThreshold,
            SIA.Algorithms.Preprocessing.Alignment.AlignmentResult alignmentResult,
            SiGlaz.Common.GraphicsList regions)
        {
            object[] result =
                this.DoCommand(typeof(DetectAnomaliesCommand),
                image, refFilePath, method, darkThreshold, brightThreshold,
                alignmentResult, regions);

            if (result == null || result[0] == null)
                return new ArrayList();

            return result[0] as ArrayList;
        }
    }
}
