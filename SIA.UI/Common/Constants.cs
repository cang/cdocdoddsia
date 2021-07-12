using System;

namespace SIA.UI.Common
{
	public class Commands
	{
		#region Common commands

		#region File commands
		
		public const String OpenImage = "Open";
		public const String SaveImage = "Save";
		public const String SaveImageAs = "Save As...";
		
		public const String CloseImage = "Close";

		public const String Print = "Print";

		public const String RecentFiles = "Recent Files";

		public const String ExitApplication = "Exit";
				
		#endregion

		#region Edit commands
		
		public const String Undo = "Undo";
		public const String Redo = "Redo";
		
		public const String CopyImageToClipbard = "Copy";
		public const String PasteImageFromClipboard = "Paste";

		public const String Preferences = "Preferences";
		public const String Customize = "Customize...";
		public const String Plugins = "Plug-ins ...";

		#endregion

		#region View commands

		public const String ShowLineProfile = "Line Profile";
		public const String ShowBoxProfile = "Box Profile";
		public const String Show3DAreaPlot = "3D Area Plot";
		public const String ScreenStretch = "Screen Stretch";
		public const String IntensityHistogram = "Intensity Histogram";
		public const String FITHeader = "FIT Header";
		public const String HorzizontalCutGraph = "Horz. Cut Graph";
		public const String VerticalCutGraph = "Vert. Cut Graph";
		public const String PseudoColor = "Pseudo Color";

		public const String CustomZoom = "Custom Zoom...";
		
		public const String MainToolbar = "Toolbar";
		public const String Toolbox = "Toolbox";
		public const String HistoryBar = "History Bar";
		public const String StatusBar = "Status Bar";
		
		#endregion

		#region Process commands

		public const String Calculations = "Calculations";		
		public const String GlobalBackgroundCorrection = "Global Background Correction";
		public const String ExtractGlobalBackground = "Extract Global Background";
		//public const String EstimateBackground = "Estimate Background";
		public const String RemoveBackground = "Remove Background";
		
		public const String Threshold = "Threshold";
		public const String StretchColor = "Stretch Color";
		public const String DistortionCorrection = "Distortion Correction";
		public const String ToggleEdgeExclusionZone = "Edge Exclusion Zone";
		
		public const String Invert = "Invert";
		public const String LightenUp = "Lighten Up";
		public const String HistogramEqualization = "Histogram Equalization";
		
		public const String FlipHorizontal = "Flip Horizontal";
		public const String FlipVertical = "Flip Vertical";
		
		public const String CustomRotate = "Custom Rotate...";
		public const String Rotate90CW = "Rotate 90° CW";
		public const String Rotate90CCW = "Rotate 90° CCW";
		public const String Rotate180 = "Rotate 180°";
		
		public const String IntensityCorrection = "Intensity Correction";
		public const String LensDistortion = "Lens Distortion";
		public const String LensCorrection = "Lens Correction";
		public const String CameraCorrection = "Camera Correction";

		public const String Overlay = "Overlay";
		public const String Alignment = "Alignment";	
		
		public const String Resize = "Resize";
		
		#endregion

		#region Filter commands
		
		public const String KernelFilters = "Kernel Filters";
		public const String RankFilters = "Rank Filters";

		public const String FourierTransform = "Fourier Transform";
		public const String FilterVariance = "Variance";		

		public const String Smooth = "Smooth";
		public const String Sharpening = "Sharpening";
		public const String Laplacian = "Laplacian";
		public const String Gaussian = "Gaussian";
		public const String Emboss135 = "Emboss 135";
		public const String Emboss90 = "Emboss 90";
		public const String EdgeDetection = "Edge Detection";

		public const String FilterMedian = "Median";
		public const String FilterRange = "Rank";
		
		#endregion

		#region Analysis commands

		public const String DetectObjects = "Detect Objects";	
		public const String ToggleObjectsListWindow = "Show Object List";
		
		public const String OverlayKLARFontoFITS = "Overlay KLARF onto image";

		public const String ToggleAnnularAnalyzer = "Annular Analyzer";

		public const String DetectLegendRegion = "Detect Legend Region";

		public const String CoMatrixClasifyObjects = "Classify Objects";

		public const String PatternWaferAnalysis = "Pattern Wafer Analysis";
		
		public const string WaferBoundaryAnalysis = "Wafer Boundary Analysis";

		#endregion

		#region Tools commands

		public const String DrawWaferBoundary = "Draw Wafer Boundary";
		public const String ToggleWaferBoundary = "Show Wafer Boundary";
		
		public const String SelectArcOfInterest = "Select Arc of Interest";
		public const String SelectSeedPoint = "Select Seed Point";
		public const String LoadArcFromFile = "Load arc from file";
		public const String SaveArcToFile = "Save arc to file";
		public const String DetectWaferBoundUsingArcs = "Detect wafer boundary using arcs";
		public const String RefineEdgePoints = "Refine Edge Points";

		public const String SelectWaferMark = "Select Wafer Mark";
		
		public const String LoadWaferSettings = "Load Wafer Boundary";
		public const String SaveWaferSettings = "Save Wafer Boundary";
		
		public const String DetectWaferBoundary = "Detect Wafer Boundary";
		public const string DetectDie = "Detect Die";
		
		public const String ClearWaferBoundary = "Clear Wafer Boundary";
		public const String MarkOrientation = "Mark Orientation";
		
		public const String ToggleDieLayout = "Show Die Layout";
		public const String DieLayoutSettings = "Die Layout Settings";
		
		public const String KLARFSettings = "Export KLARF Settings";
		public const String ExportKLARF = "Export KLARF"; 
		
		public const String MaskEditor = "Mask Editor";
		public const String LoadMaskFile = "Load Mask";
		public const String ToggleMask = "Show Mask";		

		public const String PhysicalCoordinateSetting = "Physical Coordinate Settings";

		public const string GetNotchTemplate = "Get Notch Template";

		#endregion

		#region Help Commands
		
		public const String Help = "Help";
		public const String About = "About";

		#endregion

		#endregion

		#region Menu commands
		#endregion

		#region Toolbar commands

		public const String ToggleSelectMode = "Select";
		public const String TogglePanMode = "Pan";
		public const String ToggleZoomInMode = "Zoom in";
		public const String ToggleZoomOutMode= "Zoom out";
		public const String ZoomFitOnScreen= "Fit on screen";
		public const String ZoomActualPixel= "Actual size";

		public const String ToggleShowAllObjects = "Show All Objects";
		public const String ToggleHighlightSelection = "Highlight Selection";
		
		#endregion
		
	}		
}
