using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Data;
using System.Diagnostics;


namespace SIA.SystemLayer.OpenCv
{
	[StructLayout(LayoutKind.Sequential)]
	public struct CvPoint2D32f
	{
		public float X;
		public float Y;

		public CvPoint2D32f(float x, float y)
		{
			this.X = x;
			this.Y = y;
		}

		public static implicit operator PointF(CvPoint2D32f pt)
		{
			return new PointF(pt.X, pt.Y);
		}

		public static implicit operator CvPoint2D32f(PointF pt)
		{
			return new CvPoint2D32f(pt.X, pt.Y);
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CvSize2D32f
	{
		public float Width;
		public float Height;

		public CvSize2D32f(float width, float height)
		{
			this.Width = width;
			this.Height = height;
		}

		public static implicit operator SizeF(CvSize2D32f size)
		{
			return new SizeF(size.Width, size.Height);
		}

		public static implicit operator CvSize2D32f(SizeF size)
		{
			return new CvSize2D32f(size.Width, size.Height);
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CvBox2D
	{
		public CvPoint2D32f Center;  /* center of the box */
		public CvSize2D32f  Size;    /* box width and length */
		public float Angle;          /* angle between the horizontal axis
                             and the first side (i.e. length) in radians */
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct CvMat
	{
		public int type;
		public int step;

		/* for internal use only */
		private int* refcount;
		private int hdr_refcount;

		public byte* data;
		public int rows;
		public int cols;
	};

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct CvSeqBlock
	{
		public CvSeqBlock*  prev; /* previous sequence block */
		public CvSeqBlock*  next; /* next sequence block */
		public int    start_index;       /* index of the first element in the block +
                                 sequence->first->start_index */
		public int    count;             /* number of elements in the block */
		public char*  data;              /* pointer to the first element of the block */
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct CvSeq
	{
		public int      flags;         /* micsellaneous flags */          
		public int      header_size;   /* size of sequence header */      
		public CvSeq*	h_prev; /* previous sequence */        
		public CvSeq*	h_next; /* next sequence */            
		public CvSeq*	v_prev; /* 2nd previous sequence */    
		public CvSeq*	v_next;  /* 2nd next sequence */

		int       total;          /* total number of elements */            
		int       elem_size;      /* size of sequence element in bytes */   
		char*     block_max;      /* maximal bound of the last block */     
		char*     ptr;            /* current write pointer */               
		int       delta_elems;    /* how many elements allocated when the seq grows */  
		
		CvMemStorage* storage;    /* where the seq is stored */             
		CvSeqBlock* free_blocks;  /* free blocks list */                    
		CvSeqBlock* first; /* pointer to the first sequence block */
	}

	[Flags]
	public enum CvMatType : int
	{
		CV_8U   = 0,
		CV_8S   = 1,
		CV_16U  = 2,
		CV_16S  = 3,
		CV_32S  = 4,
		CV_32F  = 5,
		CV_64F  = 6,
		CV_USRTYPE1 = 7,

		CV_8UC1 = CV_8U + ((1 - 1) << 3),
		CV_8UC2 = CV_8U + ((2 - 1) << 3),
		CV_8UC3 = CV_8U + ((3 - 1) << 3),
		CV_8UC4 = CV_8U + ((4 - 1) << 3),

		CV_8SC1 = CV_8S + ((1 - 1) << 3),
		CV_8SC2 = CV_8S + ((2 - 1) << 3),
		CV_8SC3 = CV_8S + ((3 - 1) << 3),
		CV_8SC4 = CV_8S + ((4 - 1) << 3),

		CV_16UC1 = CV_16U + ((1 - 1) << 3),
		CV_16UC2 = CV_16U + ((2 - 1) << 3),
		CV_16UC3 = CV_16U + ((3 - 1) << 3),
		CV_16UC4 = CV_16U + ((4 - 1) << 3),

		CV_16SC1 = CV_16S + ((1 - 1) << 3),
		CV_16SC2 = CV_16S + ((2 - 1) << 3),
		CV_16SC3 = CV_16S + ((3 - 1) << 3),
		CV_16SC4 = CV_16S + ((4 - 1) << 3),

		CV_32SC1 = CV_32S + ((1 - 1) << 3),
		CV_32SC2 = CV_32S + ((2 - 1) << 3),
		CV_32SC3 = CV_32S + ((3 - 1) << 3),
		CV_32SC4 = CV_32S + ((4 - 1) << 3),

		CV_32FC1 = CV_32F + ((1 - 1) << 3),
		CV_32FC2 = CV_32F + ((2 - 1) << 3),
		CV_32FC3 = CV_32F + ((3 - 1) << 3),
		CV_32FC4 = CV_32F + ((4 - 1) << 3),

		CV_64FC1 = CV_64F + ((1 - 1) << 3),
		CV_64FC2 = CV_64F + ((2 - 1) << 3),
		CV_64FC3 = CV_64F + ((3 - 1) << 3),
		CV_64FC4 = CV_64F + ((4 - 1) << 3),
	}

	[Flags]
	public enum CvSeqElemType : int
	{
		CV_SEQ_KIND_BITS = 5,
		CV_SEQ_ELTYPE_GENERIC = 0,
		CV_SEQ_ELTYPE_POINT = CvMatType.CV_32SC2, /* (x,y) */
		CV_SEQ_ELTYPE_CODE = CvMatType.CV_8UC1, /* freeman code: 0..7 */
		CV_SEQ_ELTYPE_PTR            = CvMatType.CV_USRTYPE1,
		CV_SEQ_ELTYPE_PPOINT         = CV_SEQ_ELTYPE_PTR,/* &(x,y) */
		CV_SEQ_ELTYPE_INDEX          = CvMatType.CV_32SC1,/* #(x,y) */
		CV_SEQ_ELTYPE_GRAPH_EDGE     = 0, /* &next_o, &next_d, &vtx_o, &vtx_d */
		CV_SEQ_ELTYPE_GRAPH_VERTEX   = 0, /* first_edge, &(x,y) */
		CV_SEQ_ELTYPE_TRIAN_ATR      = 0, /* vertex of the binary tree   */
		CV_SEQ_ELTYPE_CONNECTED_COMP = 0, /* connected component  */
		CV_SEQ_ELTYPE_POINT3D        = CvMatType.CV_32FC3, /* (x,y,z)  */
	}

	[Flags]
	public enum CvSeqType : int
	{
		CV_SEQ_KIND_BITS = 5,
		/* types of sequences */
		CV_SEQ_KIND_GENERIC		= (0 << CV_SEQ_KIND_BITS),
		CV_SEQ_KIND_CURVE		= (1 << CV_SEQ_KIND_BITS),
		CV_SEQ_KIND_BIN_TREE	= (2 << CV_SEQ_KIND_BITS),
		/* types of sparse sequences (sets) */
		CV_SEQ_KIND_GRAPH		= (3 << CV_SEQ_KIND_BITS),
		CV_SEQ_KIND_SUBDIV2D	= (4 << CV_SEQ_KIND_BITS)
	}

	public enum CvErrorMode : int
	{
		CV_ErrModeLeaf     = 0,   /* Print error and exit program */
		CV_ErrModeParent   = 1,   /* Print error and continue */
		CV_ErrModeSilent   = 2,   /* Don't print and continue */
	}

    /// <summary>
    /// Growing memory storage
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CvMemStorage
    {
        /// <summary>
        /// internal signature
        /// </summary>
        public int signature;

        /// <summary>
        /// first allocated block
        /// </summary>
        public IntPtr bottom;

        /// <summary>
        /// current memory block - top of the stack
        /// </summary>
        public IntPtr top;

        /// <summary>
        /// borrows new blocks from
        /// </summary>
        public IntPtr parent;

        /// <summary>
        /// block size
        /// </summary>
        public int block_size;

        /// <summary>
        /// free space in the current block
        /// </summary>
        public int free_space;

        /// <summary>
        /// this pointer
        /// </summary>
        public IntPtr ptr;
    }

}
