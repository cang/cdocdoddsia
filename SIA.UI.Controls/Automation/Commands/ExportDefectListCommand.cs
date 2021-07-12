using System;
using System.Drawing;
using System.Data;
using System.IO;
using System.Threading;
using System.Diagnostics;

using SIA.Common;
using SIA.Common.Imaging;

using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;

using SIA.IPEngine;
using SIA.SystemLayer;

using SIA.UI.Controls.Utilities;

using SIA.UI.Controls.Commands;
using System.Collections;
using System.Text;
using SIA.Common.Analysis;
using SiGlaz.Common.ABSDefinitions;
using SiGlaz.Common.Object;
using SiGlaz.Common;
using SiGlaz.Common.ImageAlignment;
using SIA.UI.Controls.Commands.Analysis;

namespace SIA.UI.Controls.Automation.Commands
{
    public class ExportDefectListCommand : AutoCommand
	{
        public ExportDefectListCommand(IProgressCallback callback)
            : base(callback)
		{
		}

		protected override void ValidateArguments(params object[] args)
		{
			if (args == null)
				throw new ArgumentNullException("arguments");
			if (args.Length < 3)
				throw new ArgumentException("Not enough arguments", "arguments");
			if (args[0] is string == false)
				throw new ArgumentException("Argument type does not match. Arguments[0] must be string", "arguments");
			if (args[1] is string == false)
                throw new ArgumentException("Argument type does not match. Arguments[1] must be string", "arguments");
            if (args[2] != null && args[2] is ArrayList == false)
                throw new ArgumentException("Argument type does not match. Arguments[1] must be ArrayList", "arguments");
		}

		protected override void OnRun(params object[] args)
		{            
		}

		public override void AutomationRun(params object[] args)
		{
            ExportResult(args);
		}

        private void ExportResult(params object[] args)
        {
            string filePath = (string)args[0];
            string imageProcessingFile = (string)args[1];
            ArrayList results = null;
            if (args[2] != null) results = args[2] as ArrayList;

            MetrologySystem ms = null;
            if (args.Length > 3)
                ms = args[3] as MetrologySystem;

            string customizedName = "";
            if (args.Length > 4)
                customizedName = (string)args[4];


            ExportResult(filePath, imageProcessingFile, results, ms, customizedName);
        }
		
        private void ExportResult(
            string outputFilePath, string image_processing_file, 
            ArrayList results, MetrologySystem ms, string customizeName)
        {
            PathHelper.CreateMissingFolderAuto(outputFilePath);

            string extension = Path.GetExtension(outputFilePath);
            if (extension != null && extension.Trim() != "" && 
                string.Compare(extension.Trim(), ".csv", true) == 0)
            {
                DefectExporter.SaveAsCSV(
                results, ms, outputFilePath, image_processing_file, customizeName);

                return;
            }
            

            DefectExporter.SaveAsText(
                results, ms, outputFilePath, image_processing_file, customizeName);
        }
	}
}
