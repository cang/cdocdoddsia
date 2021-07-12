using System;

namespace SIA.UI.Controls.Common
{
	public enum  eGraphType { Horizontal, Vertical };
	public enum  eZoomType { ZoomActualPixel, ZoomInRectangle, ZoomOut, ZoomCustom, ZoomFitOnScreen};
	public enum  eStateHistory
	{
		Empty,
		First,
		Last,
		Middle
	}

	[Flags]
	public enum eAppState : uint
	{
		Normal			= 1,
		ZoomIn			= 1<<1,
		ZoomOut			= 1<<2,
		Pan				= 1<<3,
		DrawWaferBound	= 1<<4,
		SelectNotch		= 1<<5,
		//AnnularAnalysis = 1<<6,
		WaferAlignment	= 1<<7,
		//EdgeExclusionZone = 1<<8,
		VisualAnalysis = 1<<9,
		//GetNotchTemplate = 1 << 10
	}

	public enum VisualAnalysisFlags : uint
	{
		None = 0,
		Point = 1<<1,
		Line = 1<<2,
		Rectangle = 1<<3,
		Ellipse = 1<<4,
		Ring = 1<<5,
		Arc = 1<<6,
		EdgeExclusionZone = 1<<7,
		TrainingWaferBound = 1<<8,
		SelectSeedPoint = 1<<9,
		GetNotchTemplate = 1<<10,
		DataProfile = 1<<11,
		PatternAnalyzer = 1<<12
	}

	public enum eMousePostion
	{
		Normal,
		EdgeCircle,
		Move,
		Resize,
		Cancel,
		NotchRectangle

	}

	public enum eLineProfileType
	{
		Line,
		HorizontalLine,
		VerticalLine,
		HorizontalBox,
		VerticalBox,
		AreaPlot
	}

	public enum eUpdateGUI
	{
		Update_Function = 0,
		Update_Status	= 1
	}

	[Flags]
	public enum eChartProfileType : byte
	{
		None=0x00,
		LineProfile=0x01,
		BoxProfile=0x02,
		AreaPlotProfile=0x04
	}
}
