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
        public CommonImage DoCommandLoadImage(string filename)
        {
            object[] result = this.DoCommand(typeof(LoadImageCommand), filename);
            return result != null && result.Length >= 1 && result[0] is CommonImage ? (CommonImage)result[0] : null;
        }

        public CommonImage DoCommandLoadImage(FileStream fs)
        {
            object[] result = this.DoCommand(typeof(LoadImageCommand), fs);
            return result != null && result.Length >= 1 && result[0] is CommonImage ? (CommonImage)result[0] : null;
        }

        public CommonImage DoCommandLoadImage(MemoryStream fs)
        {
            object[] result = this.DoCommand(typeof(LoadImageCommand), fs);
            return result != null && result.Length >= 1 && result[0] is CommonImage ? (CommonImage)result[0] : null;
        }

        public void DoCommandSaveImage(CommonImage image, string filename, eImageFormat format)
        {
            this.DoCommand(typeof(SaveImageCommand), image, filename, format);
        }
    }
}
