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
        public GreyDataImage DoCommandCreateReferenceImage(
            string[] sampleFiles, eGoldenImageMethod method)
        {
            object[] result = this.DoCommand(typeof(CreateReferenceImageCommand), sampleFiles, method);
            return result[0] as GreyDataImage;
        }

        public GreyDataImage DoCommandSubtractGoldenImage(
            CommonImage image, string goldenImageFile,
            eGoldenImageMethod method)
        {
            object[] result = this.DoCommand(typeof(SubtractGoldenImageCommand), image, goldenImageFile, method);

            if (result[0] == null)
                return null;

            return result[0] as GreyDataImage;
        }

        public Image DoCommandOverlayDefectOnImage(CommonImage image, ArrayList defectList)
        {
            object[] result = this.DoCommand(typeof(OverlayDefectOnABSCommand), image, defectList);

            return (result == null || result[0] == null) ? null : result[0] as Image;
        }
    }
}
