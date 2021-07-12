using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SIA.UI.Components
{
#if false
	/// <summary>
	/// Encapsulates the range between minium and maximum values
	/// </summary>
	[Serializable, StructLayout(LayoutKind.Sequential), ComVisible(true)]
	public struct DataRange
	{
		#region Fields

		private int min;
		private int max;

		public static readonly DataRange Empty;

		#endregion

		static DataRange()
		{
			Empty = new DataRange();
		}

		public DataRange(int min, int max)
		{
			this.min = min;
			this.max = max;
		}

		#region override methods

		public override bool Equals(object obj)
		{
			if (obj is DataRange)
			{
				DataRange DataRange = (DataRange) obj;
				if (DataRange.min == this.min)
				{
					return (DataRange.max == this.max);
				}
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (this.min ^ this.max);
		}

		public override string ToString()
		{
			return ("{min=" + this.min.ToString() + ", max=" + this.max.ToString() + "}");
		}

		#endregion

		#region operators

		public static DataRange operator +(DataRange rng1, DataRange rgn2)
		{
			return new DataRange(rng1.min + rgn2.min, rng1.max + rgn2.max);
		}

		public static bool operator ==(DataRange rng1, DataRange rgn2)
		{
			if (rng1.min == rgn2.min)
			{
				return (rng1.max == rgn2.max);
			}
			return false;
		}

		public static bool operator !=(DataRange rng1, DataRange rgn2)
		{
			return !(rng1 == rgn2);
		}

		public static DataRange operator -(DataRange rng1, DataRange rgn2)
		{
			return new DataRange(rng1.min - rgn2.min, rng1.max - rgn2.max);
		}

		#endregion

		#region Properties

        /// <summary>
        /// Gets or sets the maximum value
        /// </summary>
		public int Max
		{
			get
			{
				return this.max;
			}
			set
			{
				this.max = value;
			}
		}

        /// <summary>
        /// Gets or sets the minimum values
        /// </summary>
		public int Min
		{
			get
			{
				return this.min;
			}
			set
			{
				this.min = value;
			}
		}
 
        /// <summary>
        /// Flags indicates whether the range is empty
        /// </summary>
		[Browsable(false)]
		public bool IsEmpty
		{
			get
			{
				if (this.min == 0)
				{
					return (this.max == 0);
				}
				return false;
			}
		}

		#endregion

	}

#endif 

}
