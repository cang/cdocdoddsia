using System;
using System.Collections;
using System.IO;
using System.Reflection;

using SIA.Common;
using SIA.Common.GoldenImageApproach;
using SIA.Common.KlarfExport;

using SIA.SystemFrameworks;
using SIA.SystemLayer;
using SIA.UI.Controls.Dialogs;

using SIA.UI.Controls.Automation;
using SIA.UI.Controls.Automation.Commands;

namespace SIA.UI.Controls.Resources.Settings
{
	/// <summary>
	/// Summary description for DefaultSettings.
	/// </summary>
	public class DefaultSettings
	{
		private const string KLARF_EXPORT_SETTINGS = "SIA.UI.Controls.Resources.Settings.KLARFExport.settings";
		private const string OBJECT_DETECTION_SETTINGS = "SIA.UI.Controls.Resources.Settings.ObjDetectionSettings.settings";
		private const string KLARF_EXPORT_SETTINGS_N = "SIA.UI.Controls.Resources.Settings.KlarfExportSettings.settings";
		private const string OBJECT_FILTER_SETTINGS = "SIA.UI.Controls.Resources.Settings.ObjectFilterSettings.ofs";
		private const string LEGEND_REGION_SETTINGS = "SIA.UI.Controls.Resources.Settings.LegendRegionSettings.settings";
		private const string DETECT_WB_SETTINGS = "SIA.UI.Controls.Resources.Settings.DetectWaferBoundDefaultSettings.settings";		
		
		public static KLARFExportSettings KlarfExportSettings
		{
			get
			{
				Stream stream = DefaultSettings.GetResourceStream(KLARF_EXPORT_SETTINGS);
				if (stream == null)
					throw new System.Exception("Resource Load Exception occurred");
				KLARFExportSettings settings = new KLARFExportSettings();
				settings.Deserialize(stream);
				return settings;
			}
		}

		public static ObjectDetectionSettings ObjDetectionSettings
		{
			get
			{
				Stream stream = DefaultSettings.GetResourceStream(OBJECT_DETECTION_SETTINGS);
				if (stream == null)
					throw new System.Exception("Resource Load Exception occurred");
				ObjectDetectionSettings settings = new ObjectDetectionSettings();
				settings.Deserialize(stream);
				return settings;
			}
		}

		public static ObjectFilterArguments ObjectFilterSettings
		{
			get
			{
				Stream stream = DefaultSettings.GetResourceStream(OBJECT_DETECTION_SETTINGS);
				if (stream == null)
					throw new System.Exception("Resource Load Exception occurred");
				ObjectFilterArguments settings = new ObjectFilterArguments();
				settings.Deserialize(stream);
				return settings;
			}
		}

		public static KlarfExportSettings KlarfExportSettings_N
		{
			get
			{
				Stream stream = DefaultSettings.GetResourceStream(KLARF_EXPORT_SETTINGS_N);
				if (stream == null)
					throw new System.Exception("Resource Load Exception occurred");
				KlarfExportSettings settings = new KlarfExportSettings();
				settings.Deserialize(stream);
				return settings;
			}
		}

		private static Stream GetResourceStream(string name)
		{			
			Type type = typeof(DefaultSettings);
			Assembly assembly = type.Assembly;
			return assembly.GetManifestResourceStream(name);
		}
	}
}
