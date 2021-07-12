using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Data;
using System.Diagnostics;
using System.Text;

using SIA.SystemFrameworks.ComputerVision;

namespace SIA.SystemLayer.OpenCv
{
	/// <summary>
	/// Wrapper class for OpenCV cxcore module
	/// </summary>
	public unsafe class cxcore
	{		
#if DEBUG
		public const string dllName = "cxcored.dll";
#else
		public const string dllName = "cxcore.dll";
#endif

		public const int CV_CN_MAX    = 64;
		public const int CV_CN_SHIFT  = 3;
		public const int CV_DEPTH_MAX = (1 << CV_CN_SHIFT);

		public static int CV_MAKETYPE(int depth, int cn) 
		{
			return ((depth) + (((cn)-1) << CV_CN_SHIFT));
		}

		static cxcore()
		{
			cxcore.cvRedirectError(new CvErrorCallback(CustomCvErrorCallback), IntPtr.Zero, IntPtr.Zero);
		}

		private static int CustomCvErrorCallback(int status, string func_name, string err_msg, string file_name, 
			int line, string userdata)
		{
			throw new Exception("An error has occurred: " + err_msg); 
		}

		[DllImport(dllName, CharSet=CharSet.Ansi)]
		public static extern void* cvAlloc(long size);

		[DllImport(dllName, CharSet=CharSet.Ansi)]
		public static extern void cvFree(void** ptr);


		[DllImport(dllName, CharSet=CharSet.Ansi, EntryPoint="scvMat")]
		public static extern CvMat cvMat(int rows, int cols, int type, void* data);

		[DllImport(dllName, CharSet=CharSet.Ansi)]
		public static extern CvMat* cvCreateMat( int rows, int cols, int type );

		[DllImport(dllName, CharSet=CharSet.Ansi)]
		public static extern void cvReleaseMat(CvMat** mat);

        [DllImport(dllName, CharSet=CharSet.Ansi)]
		public static extern CvMat* cvCloneMat(CvMat* mat);

		[DllImport(dllName, CharSet=CharSet.Ansi)]
		public static extern CvMat* cvInitMatHeader( CvMat* mat, int rows, int cols, int type, void* data, int step);

		public static CvMat* cvInitMatHeader( CvMat* mat, int rows, int cols, int type, void* data)
		{
			int CV_AUTOSTEP  = 0x7fffffff;
			return cvInitMatHeader(mat, rows, cols, type, null, CV_AUTOSTEP);
		}

		[DllImport(dllName, CharSet=CharSet.Ansi)]
		public static extern CvMemStorage* cvCreateMemStorage(int block_size);

		[DllImport(dllName, CharSet=CharSet.Ansi)]
		public static extern void cvReleaseMemStorage(CvMemStorage** storage);

		[DllImport(dllName, CharSet=CharSet.Ansi)]
		public static extern CvSeq* cvCreateSeq(int seq_flags, int header_size, int element_size, CvMemStorage* storage);

		[DllImport(dllName, CharSet=CharSet.Ansi)]
		public static extern void* cvSeqPush(CvSeq* seq, void* element);

		#region System functions

		public delegate int CvErrorCallback(
			int status,
			[MarshalAs(UnmanagedType.LPStr)]string func_name,
			[MarshalAs(UnmanagedType.LPStr)]string err_msg,
			[MarshalAs(UnmanagedType.LPStr)]string file_name,
			int line,
			[MarshalAs(UnmanagedType.LPStr)]string userdata);

		/* Get current OpenCV error status */
		[DllImport(dllName, CharSet=CharSet.Ansi)]
		public static extern int cvGetErrStatus();

		/* Sets error status silently */
		[DllImport(dllName, CharSet=CharSet.Ansi)]
		public static extern void cvSetErrStatus(int status);

		/* Retrieves current error processing mode */
		[DllImport(dllName, CharSet=CharSet.Ansi)]
		public static extern CvErrorMode cvGetErrMode();

		/* Sets error processing mode, returns previously used mode */
		[DllImport(dllName, CharSet=CharSet.Ansi)]
		public static extern CvErrorMode cvSetErrMode(CvErrorMode mode );

		/* Retrieves textual description of the error given its code */
		[DllImport(dllName, CharSet=CharSet.Ansi)]
		public static extern String cvErrorStr(int status);

		[DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr cvRedirectError(CvErrorCallback errorHandler, IntPtr userdata, IntPtr prev_userdata);

		public static IntPtr cvRedirectError(CvErrorCallback errorHandler, IntPtr userdata)
		{
			return cvRedirectError(errorHandler, userdata, IntPtr.Zero);
		}

		public static IntPtr cvRedirectError(CvErrorCallback errorHandler)
		{
			return cvRedirectError(errorHandler, IntPtr.Zero, IntPtr.Zero);
		}



		#endregion
	}
}
