using System;
using System.Data;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

using SIA.IPEngine;
using SIA.Common;

using TYPE = System.UInt16; 

namespace SIA.SystemLayer
{
    /// <summary>
    /// Specifies the type of morphology operation
    /// </summary>
	public enum eMorphType
	{
		kMORPH_EROSION,
		kMORPH_DILATION,
		kMORPH_CLOSING,
		kMORPH_OPENING,
		kMORPH_FLAT_EROSION,
		kMORPH_FLAT_DILATION,
		kMORPH_FLAT_CLOSING,
		kMORPH_FLAT_OPENING,
		kMORPH_TOP_HAT,
		kMOPRH_BOTTOM_HAT,
		kMORPH_FLATTEN,

		kMORPH_UNKNOWN
	};

    /// <summary>
    /// Specifies the type of the kernel matrix 
    /// </summary>
	public enum eMatrixType
	{
		kMATRIX_2x2,
		kMATRIX_3x3,
		kMATRIX_5x5,
		kMATRIX_7x7,
		kMATRIX_9x9,
		
		kMATRIX_CIRCLE5x5,
		kMATRIX_CIRCLE7x7,
		
		kMATRIX_CROSS3x3,
		
		kMATRIX_RING3x3,
		kMATRIX_RING5x5,
		kMATRIX_RING7x7,

		kMATRIX_SQUARE2x2,
		kMATRIX_SQUARE3x3,

		kMATRIX_UNKNOWN
	};

    /// <summary>
    /// specifies the type of the mask
    /// </summary>
	public enum eMaskType
	{
		kMASK_CIRCLE	= 0,
		kMASK_CROSS		= 1,
		kMASK_RING		= 2,
		kMASK_SQUARE	= 3,
		kMASK_HIGHGAUSS	= 4,
		kMASK_HIGHPASS	= 5,
		kMASK_LOWPASS	= 6,
		kMASK_HORZEDGE	= 7,
		kMASK_VERTEDGE	= 8,
		kMASK_LAPLACE	= 9,
		kMASK_UNSHARP	= 10,
		kMASK_WELL		= 11,
		kMASK_TOPHAT	= 12,
		kMASK_SCULPT	= 13,
		kMASK_HOTPIXEL	= 14,
		kMASK_DEADPIXEL = 15,
		kMASK_COUNT		= 16,
		kMASK_MEDIAN    = 17,
		kMASK_SMOOTH = 18, // [20070618 Cong]
		kMASK_UNKNOWN
	};

//	public enum eMaskType
//	{
//		kMASK_CIRCLE5x5 = 0,
//		kMASK_CIRCLE7x7 = 1,
//
//		kMASK_CROSS3x3  = 2,
//		
//		kMASK_RING3x3 = 3,
//		kMASK_RING5x5 = 4,
//		kMASK_RING7x7 = 5,
//
//		kMASK_SQUARE2x2 = 6,
//		kMASK_SQUARE3x3 = 7,
//		
//		kMASK_HIGHGAUSS3x3 = 8,
//		kMASK_HIGHGAUSS5x5 = 9,
//		kMASK_HIGHGAUSS7x7 = 10,
//
//		kMASK_HIGHPASS3x3 = 11,
//		kMASK_HIGHPASS5x5 = 12,
//		kMASK_HIGHPASS7x7 = 13,
//		
//		kMASK_LOWPASS3x3 = 14,
//		kMASK_LOWPASS5x5 = 15,
//		kMASK_LOWPASS7x7 = 16,
//		
//		kMASK_HORZEDGE3x3 = 17,
//		kMASK_HORZEDGE5x5 = 18,
//		kMASK_HORZEDGE7x7 = 19,
//		
//		kMASK_VERTEDGE3x3 = 20,
//		kMASK_VERTEDGE5x5 = 21,
//		kMASK_VERTEDGE7x7 = 22,
//		
//		kMASK_LAPLACE3x3 = 23,
//		kMASK_LAPLACE5x5 = 24,
//		kMASK_LAPLACE7x7 = 25,
//		
//		kMASK_UNSHARP3x3 = 26,
//		kMASK_UNSHARP5x5 = 27,
//		kMASK_UNSHARP7x7 = 28,
//		
//		kMASK_WELL3x3 = 29,
//		kMASK_WELL5x5 = 30,
//		kMASK_WELL7x7 = 31,
//		
//		kMASK_TOPHAT3x3 = 32,
//		kMASK_TOPHAT5x5 = 33,
//		kMASK_TOPHAT7x7 = 34,
//
//		kMASK_SCULPT3x3 = 35,
//
//
//		kMASK_COUNT = 36
//	};

	/// <summary>
	/// The kKernelFilters class provides functions for convolution and morphological operations
	/// </summary>
	public class kKernelFilters
	{
		protected static System.String[] m_names =
		{
		};

		protected static System.String[] m_descriptions =
		{
		};

		protected static int [][,] m_kernels = 
		{
			/* Morphological Matrices */
			/* Circle Matrices */
			new int[,] 
			{
				{0,	1,	1,	1,	0},
				{1,	1,	1,	1,	1},
				{1,	1,	1,	1,	1},
				{1,	1,	1,	1,	1},
				{0,	1,	1,	1,	0}
			},

			new int[,] 
			{
				{0,	0,	1,	1,	1,	0,	0},
				{0,	1,	1,	1,	1,	1,	0},
				{1,	1,	1,	1,	1,	1,	1},
				{1,	1,	1,	1,	1,	1,	1},
				{1,	1,	1,	1,	1,	1,	1},
				{0,	1,	1,	1,	1,	1,	0},
				{0,	0,	1,	1,	1,	0,	0}
			},
			/* Cross */
			new int[,] 
			{
				{0,1,0},
				{1,1,1},
				{0,1,0}
			},
			/* Ring */
			new int[,] 
			{
				{1,1,1},
				{1,0,1},
				{1,1,1}
			},
			new int[,]
			{
				{0,1,1,1,0},
				{1,1,0,1,1},
				{1,0,0,0,1},
				{1,1,0,1,1},
				{0,1,1,1,0}
			},			
			new int[,]
			{
				{0,0,1,1,1,0,0},
				{0,1,1,1,1,1,0},
				{1,1,0,0,0,1,1},
				{1,1,0,0,0,1,1},
				{1,1,0,0,0,1,1},
				{0,1,1,1,1,1,0},
				{0,0,1,1,1,0,0}
			},
			/* Square */
			new int[,]
			{
				{1,1},
				{1,1}
			},
			new int[,]
			{
				{1,1,1},
				{1,1,1},
				{1,1,1},
			},
			/* High Gauss 5x5, 7x7, 9x9*/
			new int[,]
			{
				{ 0,-1,-1,-1, 0},
				{-1, 0, 2, 0,-1},
				{-1, 2, 5, 2,-1},
				{-1, 0, 2, 0,-1},
				{ 0,-1,-1,-1, 0},
			},
			new int[,]
			{
				{ 0, 0,-1,-1,-1, 0, 0},
				{ 0,-1,-1, 0,-1,-1, 0},
				{-1,-1, 2, 4, 2,-1,-1},
				{-1, 0, 4,10, 4, 0,-1},
				{-1,-1, 2, 4, 2,-1,-1},
				{ 0,-1,-1, 0,-1,-1, 0},
				{ 0, 0,-1,-1,-1, 0, 0}
			},
			new int[,]
			{
				{-1,0,-1,-3,-4,-3,-1,0,-1},
				{0,-2,-7,-8,-7,-8,-7,-2,0},
				{-1,-7,-5,3,7,3,-5,-7,-1},
				{-3,-8,3,19,27,19,3,-8,-3},
				{-4,-7,7,27,42,27,7,-7,-4},
				{-3,-8,3,19,27,19,3,-8,-3},
				{-1,-7,-5,3,7,3,-5,-7,-1},
				{0,-2,-7,-8,-7,-8,-7,-2,0},
				{-1,0,-1,-3,-4,-3,-1,0,-1}
			},
			/* High Pass */
			new int[,]
			{
// normal public kernel
//				{-1,-1,-1},
//				{-1,9,-1},
//				{-1,-1,-1}
//			new int[,]
//			{
//				{0,-1,-1,-1,0},
//				{-1,-1,-1,-1,-1},
//				{-1,-1,21,-1,-1},
//				{-1,-1,-1,-1,-1},
//				{0,-1,-1,-1,0}
//			},
				// MaxIm high pass kernel 
				{-1,-1,-1},
				{-1,16,-1},
				{-1,-1,-1}
			},
			new int[,]
			{
				{-1, -1, -1, -1, -1},
				{-1, -2, -2, -2, -1},
				{-1, -2, 48, -2, -1},
				{-1, -2, -2, -2, -1},
				{-1, -1, -1, -1, -1}
			},
//			new int[,]
//			{
//				{0,0,-1,-1,-1,0,0},
//				{0,-1,-1,-1,-1,-1,0},
//				{-1,-1,-1,-1,-1,-1,-1},
//				{-1,-1,-1,37,-1,-1,-1},
//				{-1,-1,-1,-1,-1,-1,-1},
//				{0,-1,-1,-1,-1,-1,0},
//				{0,0,-1,-1,-1,0,0}
//			},
			new int[,]
			{
				{-1,-1,-1,-1,-1,-1,-1},
				{-1,-2,-2,-2,-2,-2,-1},
				{-1,-2,-4,-4,-4,-4,-1},
				{-1,-2,-4,48,-1,-4,-1},
				{-1,-2,-4,-4,-4,-4,-1},
				{-1,-2,-2,-2,-2,-2,-1},
				{-1,-1,-1,-1,-1,-1,-1}
			},			
			/* Low Pass */
			new int[,]
			{
//				{1,1,1},
//				{1,1,1},
//				{1,1,1}
			/* Maxim method */
				{1,1,1},
				{1,8,1},
				{1,1,1}
			},
			new int[,]
			{
				{1,1, 1,1,1},
				{1,2, 2,2,1},
				{1,2,16,2,1},
				{1,2, 2,2,1},
				{1,1, 1,1,1}
			},
			new int[,]
			{
				{1,1,1,1,1,1,1},
				{1,1,1,1,1,1,1},
				{1,1,1,1,1,1,1},
				{1,1,1,1,1,1,1},
				{1,1,1,1,1,1,1},
				{1,1,1,1,1,1,1},
				{1,1,1,1,1,1,1}
			},
			/* Horizontal Edge */
			new int[,]
			{
				{-1,-2,-1},
				{0,0,0},
				{1,2,1}
			},
			new int[,]
			{
				{-1,-1,-1,-1,-1},
				{-1,-1,-1,-1,-1},
				{0,0,0,0,0},
				{1,1,1,1,1},
				{1,1,1,1,1}
			},
			new int[,]
			{
				{-1,-1,-1,-1,-1,-1,-1},
				{-1,-1,-1,-1,-1,-1,-1},
				{-1,-1,-1,-1,-1,-1,-1},
				{0,0,0,0,0,0,0},
				{1,1,1,1,1,1,1},
				{1,1,1,1,1,1,1},
				{1,1,1,1,1,1,1}
			},
			/* Vertical Edge */
			new int[,]
			{
				{-1,0,1},
				{-2,0,2},
				{-1,0,1}
			},
			new int[,]
			{
				{-1,-1,0,1,1},
				{-1,-1,0,1,1},
				{-1,-1,0,1,1},
				{-1,-1,0,1,1},
				{-1,-1,0,1,1}
			},
			new int[,]
			{
				{-1,-1,-1,0,1,1,1},
				{-1,-1,-1,0,1,1,1},
				{-1,-1,-1,0,1,1,1},
				{-1,-1,-1,0,1,1,1},
				{-1,-1,-1,0,1,1,1},
				{-1,-1,-1,0,1,1,1},
				{-1,-1,-1,0,1,1,1}
			},
			/* Laplace */
			new int[,]
			{
				{-1,-1,-1},
				{-1,8,-1},
				{-1,-1,-1}
			},
			new int[,]
			{
				{0,-1,-1,-1,0},
				{-1,0,1,0,-1},
				{-1,1,8,1,-1},
				{-1,0,1,0,-1},
				{0,-1,-1,-1,0}
			},
			new int[,]
			{
				{-1,-1,-1,-1,-1,-1,-1},
				{-1,-1,-1,-1,-1,-1,-1},
				{-1,-1,-1,-1,-1,-1,-1},
				{-1,-1,-1,48,-1,-1,-1},
				{-1,-1,-1,-1,-1,-1,-1},
				{-1,-1,-1,-1,-1,-1,-1},
				{-1,-1,-1,-1,-1,-1,-1}
			},
			/* Unsharp */
			new int[,]
			{
				{-1,-1,-1},
				{-1,17,-1},
				{-1,-1,-1}
			},
			new int[,]
			{
				{-1,-1,-1,-1,-1},
				{-1,-1,-1,-1,-1},
				{-1,-1,49,-1,-1},
				{-1,-1,-1,-1,-1},
				{-1,-1,-1,-1,-1}
			},
			new int[,]
			{
				{-1,-1,-1,-1,-1,-1,-1},
				{-1,-1,-1,-1,-1,-1,-1},
				{-1,-1,-1,-1,-1,-1,-1},
				{-1,-1,-1,97,-1,-1,-1},
				{-1,-1,-1,-1,-1,-1,-1},
				{-1,-1,-1,-1,-1,-1,-1},
				{-1,-1,-1,-1,-1,-1,-1}
			},
			/* Well */
			new int[,]
			{
				{1,1,1},
				{1,-8,1},
				{1,1,1}
			},
			new int[,]
			{
				{0,1,1,1,0},
				{1,1,-3,1,1},
				{1,-3,-4,-3,1},
				{1,1,-3,1,1},
				{0,1,1,1,0}
			},
			new int[,]
			{
				{0,0,1,1,1,0,0},
				{0,1,1,1,1,1,0},
				{1,1,-3,-3,-3,1,1},
				{1,1,-3,-4,-3,1,1},
				{1,1,-3,-3,-3,1,1},
				{0,1,1,1,1,1,0},
				{0,0,1,1,1,0,0}
			},
			/* Top Hat */
			new int[,]
			{
				{-1,-1,-1},
				{-1,8,-1},
				{-1,-1,-1}
			},
			new int[,]
			{
				{0,-1,-1,-1,0},
				{-1,-1,3,-1,-1},
				{-1,3,4,3,-1},
				{-1,-1,3,-1,-1},
				{0,-1,-1,-1,0},
			},
			new int[,]
			{
				{ 0, 0,-1,-1,-1, 0,0},
				{ 0,-1,-1,-1,-1,-1,0},
				{-1,-1,3,3,3,-1,-1},
				{-1,-1,3,4,3,-1,-1},
				{-1,-1,3,3,3,-1,-1},
				{0,-1,-1,-1,-1,-1,0},
				{0,0,-1,-1,-1,0,0},
			},
			/* Sculpt */
			new int[,]
			{
				{-1, 0,	0},
				{ 0, 0,	0},
				{ 0, 0,	1}
			},
			/* Hot Pixel */
			new int[,]
			{
				{1, 1,	1},
				{1, 0,	1},
				{1, 1,	1}
			},
			/* Dead Pixel */
			new int[,]
			{
				{ 1, 1,	1},
				{ 1, 0,	1},
				{ 1, 1,	1}
			},
			/* Smooth */
			new int[,]
			{
				{ 1, 2,	1},
				{ 2, 4,	2},
				{ 1, 2,	1}
			}
		};

		public static eMatrixType[] QueryMatrixTypeSupported(eMorphType morphType)
		{
			ArrayList result = new ArrayList();
			
			result.Add(eMatrixType.kMATRIX_CIRCLE5x5);
			result.Add(eMatrixType.kMATRIX_CIRCLE7x7);
			result.Add(eMatrixType.kMATRIX_CROSS3x3);
			result.Add(eMatrixType.kMATRIX_RING3x3);
			result.Add(eMatrixType.kMATRIX_RING5x5);
			result.Add(eMatrixType.kMATRIX_RING7x7);
			result.Add(eMatrixType.kMATRIX_SQUARE2x2);
			result.Add(eMatrixType.kMATRIX_SQUARE3x3);

			return (eMatrixType[])result.ToArray(typeof(eMatrixType));
		}

		public static eMatrixType[] QueryMatrixTypeSupported(eMaskType filterType)
		{
			ArrayList result = new ArrayList();
			switch (filterType)
			{
				case eMaskType.kMASK_HIGHGAUSS:		
					result.Add(eMatrixType.kMATRIX_5x5);
					result.Add(eMatrixType.kMATRIX_7x7);
					break;
				case eMaskType.kMASK_HIGHPASS:
				case eMaskType.kMASK_LOWPASS:
					result.Add(eMatrixType.kMATRIX_3x3);
					result.Add(eMatrixType.kMATRIX_5x5);
					break;
				case eMaskType.kMASK_HORZEDGE:
				case eMaskType.kMASK_VERTEDGE:
				case eMaskType.kMASK_LAPLACE:
				case eMaskType.kMASK_UNSHARP:
				case eMaskType.kMASK_WELL	:
				case eMaskType.kMASK_TOPHAT:
				case eMaskType.kMASK_MEDIAN:
					result.Add(eMatrixType.kMATRIX_3x3);
					result.Add(eMatrixType.kMATRIX_5x5);
					result.Add(eMatrixType.kMATRIX_7x7);
					break;
				case eMaskType.kMASK_SCULPT:
					result.Add(eMatrixType.kMATRIX_3x3);
					break;
				default:
					break;
			}

			return (eMatrixType[])result.ToArray(typeof(eMatrixType));
		}

		public static int[,] QueryMorphologyKernelMatrix(eMatrixType matrixType)
		{
			int [,] filter = null;
			int filter_index = -1;
			switch (matrixType)
			{
				case eMatrixType.kMATRIX_CIRCLE5x5:
					filter_index = 0;
					break;
				case eMatrixType.kMATRIX_CIRCLE7x7:
					filter_index = 1;
					break;
				case eMatrixType.kMATRIX_CROSS3x3:
					filter_index = 2;
					break;
				case eMatrixType.kMATRIX_RING3x3:
					filter_index = 3;
					break;
				case eMatrixType.kMATRIX_RING5x5:
					filter_index = 4;
					break;
				case eMatrixType.kMATRIX_RING7x7:
					filter_index = 5;
					break;
				case eMatrixType.kMATRIX_SQUARE2x2:
					filter_index = 6;
					break;
				case eMatrixType.kMATRIX_SQUARE3x3:
					filter_index = 7;
					break;
			}
			if (filter_index>=0 && filter_index<m_kernels.Length)
				filter = m_kernels[filter_index];
			return filter;
		}

		public static int[,] QueryConvolutionKernelMatrix(eMaskType convolveType, eMatrixType matrixType)
		{
			int filter_offset = 0;
			switch (matrixType)
			{
				case eMatrixType.kMATRIX_3x3:
					filter_offset = 0;
					break;
				case eMatrixType.kMATRIX_5x5:
					filter_offset = 1;
					break;
				case eMatrixType.kMATRIX_7x7:
					filter_offset = 2;
					break;
				default:
					return null;
			}

			int [,] filter = null;
			int filter_index = -1;
			switch (convolveType)
			{
				case eMaskType.kMASK_HIGHGAUSS:		
					filter_index = 8 + filter_offset - 1;
					break;
				case eMaskType.kMASK_HIGHPASS:		
					filter_index = 11 + filter_offset;
					break;
				case eMaskType.kMASK_LOWPASS:		
					filter_index = 14 + filter_offset;
					break;
				case eMaskType.kMASK_HORZEDGE:		
					filter_index = 17 + filter_offset;
					break;
				case eMaskType.kMASK_VERTEDGE:		
					filter_index = 20 + filter_offset;
					break;
				case eMaskType.kMASK_LAPLACE:		
					filter_index = 23 + filter_offset;
					break;
				case eMaskType.kMASK_UNSHARP:		
					filter_index = 26 + filter_offset;
					break;
				case eMaskType.kMASK_WELL:		
					filter_index = 29 + filter_offset;
					break;
				case eMaskType.kMASK_TOPHAT:		
					filter_index = 32 + filter_offset;
					break;
				case eMaskType.kMASK_SCULPT:
					filter_index = 35;
					break;
				case eMaskType.kMASK_HOTPIXEL:
					filter_index = 36;
					break;
				case eMaskType.kMASK_DEADPIXEL:
					filter_index = 37;
					break;
				case eMaskType.kMASK_SMOOTH:
					filter_index = 38;
					break;
				case eMaskType.kMASK_MEDIAN:
					filter_index = -1;
					break;
				default:
					break;
			}
			if (filter_index>=0 && filter_index<m_kernels.Length)
				filter = m_kernels[filter_index];
			return filter;
		}

		public static kFilter QueryMorphologyFilter(eMorphType morphType, eMatrixType matrixType)
		{
			kFilter filter = null;			
			int [,] mask = QueryMorphologyKernelMatrix(matrixType);
			
			if (mask != null)
			{
				switch (morphType)
				{
					case eMorphType.kMORPH_EROSION:
						filter = new kErosion(mask);
						break;
					case eMorphType.kMORPH_DILATION:
						filter = new kDilation(mask);
						break;
					case eMorphType.kMORPH_CLOSING:
						filter = new kClosing(mask);
						break;
					case eMorphType.kMORPH_OPENING:
						filter = new kOpening(mask);
						break;
					case eMorphType.kMORPH_FLAT_EROSION:
						filter = new kFlatErosion(mask);
						break;
					case eMorphType.kMORPH_FLAT_DILATION:
						filter = new kFlatDilation(mask);
						break;
					case eMorphType.kMORPH_FLAT_CLOSING:
						filter = new kFlatClosing(mask);
						break;
					case eMorphType.kMORPH_FLAT_OPENING:
						filter = new kFlatOpening(mask);
						break;
					case eMorphType.kMORPH_TOP_HAT:
						filter = new kTopHat(mask);
						break;
					case eMorphType.kMOPRH_BOTTOM_HAT:
						filter = new kBottomHat(mask);
						break;
					case eMorphType.kMORPH_FLATTEN:
						filter = new kFlatten(mask);
						break;
					default:
						throw new System.Exception("Unknown morphology type");
				}			
			}

			return filter;
		}

		public static kFilter QueryConvolutionFilter(eMaskType filterType, eMatrixType matrixType)
		{
			kFilter filter = null;
			int [,] mask = QueryConvolutionKernelMatrix(filterType, matrixType);
			if ((mask != null) || (mask == null && filterType == eMaskType.kMASK_MEDIAN))
			{
				switch (filterType)
				{
					case eMaskType.kMASK_HOTPIXEL:
						filter = new kHotPixel(mask);
						break;
					case eMaskType.kMASK_DEADPIXEL:
						filter = new kDeadPixel(mask);
						break;
					case eMaskType.kMASK_MEDIAN :
						int kernel_width = 3;
						if(matrixType == eMatrixType.kMATRIX_3x3)
							kernel_width = 3;
						else if(matrixType == eMatrixType.kMATRIX_5x5)
							kernel_width = 5;
						else if(matrixType == eMatrixType.kMATRIX_7x7)
							kernel_width = 7;
						filter = new kMedian(kernel_width);
						break;
					default:
						filter = new kConvolution(mask);
						break;
				}
			}
			return filter;
		}

		public static kFilter QueryConvolutionFilter(int[,] Matrix)
		{
			kFilter filter = null;
			filter = new kConvolution(Matrix);
			return filter;
		}
	}
}
