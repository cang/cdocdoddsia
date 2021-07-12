using System;
using System.IO;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SiGlaz.Common
{
	public abstract class ParameterBase
	{
		protected const string nullstr = "NULL";
		public int CurrentVersion 
		{
			get
			{
				return 1;
			}
		}

		protected virtual void OnDeserialized()
		{

		}

		protected virtual void OnSerializing()
		{

		}

		protected virtual void BinDeserialize(BinaryReader bin)
		{
			int currentVerision = bin.ReadInt32();
		}

		public void Serialize(string filename)
		{
			using (FileStream fs = new FileStream(filename, FileMode.Create))
			using (BinaryWriter bin = new BinaryWriter(fs))
				this.Serialize(bin);
		}

		public virtual void Serialize(BinaryWriter bin)
		{
			this.BaseSerialize(bin);
		}

		public static void Serialize(BinaryWriter bin, ParameterBase obj)
		{
			if (obj == null)
			{
				bin.Write(nullstr);
				return;
			}

			obj.BaseSerialize(bin);
		}

		protected virtual void BaseSerialize(BinaryWriter bin)
		{
			OnSerializing();

			Type type = this.GetType();
			bin.Write(type.AssemblyQualifiedName);
			bin.Write(this.CurrentVersion);
		}

		protected static ParameterBase BaseDeserialize(string filename)
		{
			using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
			using (BinaryReader bin = new BinaryReader(fs))
				return BaseDeserialize(bin);
		}

		protected static ParameterBase BaseDeserialize(BinaryReader bin, params object[] objs)
		{
			string typeName = bin.ReadString();
			if (typeName == null || typeName == string.Empty || typeName == nullstr)
				return null;

			Type type = Type.GetType(typeName);
			ParameterBase obj = Activator.CreateInstance(type, objs) as ParameterBase;

			obj.BinDeserialize(bin);
			obj.OnDeserialized();

			return obj;
		}

		public void LoadContent(BinaryReader bin)
		{
			if (bin == null)
				return;

			string typeName = bin.ReadString();

			this.BinDeserialize(bin);

			OnDeserialized();
		}

		public void LoadContent(byte[] bytes)
		{
			if (bytes == null || bytes.Length == 0)
				return;

			using (MemoryStream ms = new MemoryStream(bytes))
			using (BinaryReader bin = new BinaryReader(ms))
				LoadContent(bin);
		}


		public static ParameterBase ParameterBaseDeserialize(BinaryReader bin, params object[] objs)
		{
			return BaseDeserialize(bin, objs);
		}

		public static void Serialize(string filename, ParameterBase obj)
		{
			using (FileStream fs = new FileStream(filename, FileMode.Create))
			using (BinaryWriter bin = new BinaryWriter(fs))
			{
				Serialize(bin, obj);
			}
		}

		public byte[] Bytes
		{
			get
			{
				using (MemoryStream ms = new MemoryStream())
				using (BinaryWriter bin = new BinaryWriter(ms))
				{
					this.Serialize(bin);
					ms.Flush();
					return ms.ToArray();
				}
			}
		}

		public static bool IsIdentical(ParameterBase obj1, ParameterBase obj2)
		{
			if (obj1 == null && obj2 == null)
				return true;
			if (obj1 != null && obj2 != null)
			{
				if (obj1.GetType().FullName != 
					obj2.GetType().FullName)
					return false;

				byte[] bytes1 = obj1.Bytes;
				byte[] bytes2 = obj2.Bytes;
				if (bytes1.Length != bytes2.Length)
					return false;
				for (int i = bytes1.Length - 1; i >= 0; i--)
					if (bytes1[i] != bytes2[i])
						return false;
				return true;
			}
			else
				return false;
		}

	}

	/// <summary>
	/// Summary description for BinarySerializationCommon.
	/// </summary>
	public class BinarySerializationCommon
	{
		public BinarySerializationCommon()
		{
		}

		public static StringCollection ReadStringCollection(BinaryReader bin)
		{
			int len = bin.ReadInt32();
			StringCollection result = new StringCollection();
			for (int i = 0; i < len; i++)
				result.Add(bin.ReadString());

			return result;
		}

        //public static PointFCollection ReadPointFCollection(BinaryReader bin)
        //{
        //    int len = bin.ReadInt32();
        //    if (len == 0)
        //        return null;
        //    PointFCollection result = new PointFCollection(len);
        //    for (int i = 0; i < len; i++)
        //    {
        //        result.Add(new PointF(bin.ReadSingle(), bin.ReadSingle()));
        //    }

        //    return result;
        //}
        //public static PointCollection ReadPointCollection(BinaryReader bin)
        //{
        //    int len = bin.ReadInt32();
        //    if (len == 0)
        //        return null;
        //    PointCollection result = new PointCollection(len);
        //    for (int i = 0; i < len; i++)
        //    {
        //        result.Add(new Point(bin.ReadInt32(), bin.ReadInt32()));
        //    }

        //    return result;
        //}

		public static Point[] ReadPointArray(BinaryReader bin)
		{
			int len = bin.ReadInt32();
			if (len == 0)
				return null;

			Point[] result = new Point[len];
			for (int i = 0; i < len; i++)
			{
				result[i] = new Point(bin.ReadInt32(), bin.ReadInt32());
			}

			return result;
		}

		public static PointF[] ReadPointFArray(BinaryReader bin)
		{
			int len = bin.ReadInt32();
			if (len == 0)
				return null;

			PointF[] result = new PointF[len];
			for (int i = 0; i < len; i++)
			{
				result[i] = new PointF(bin.ReadSingle(), bin.ReadSingle());
			}

			return result;
		}

		public static Rectangle ReadRectangle(BinaryReader bin)
		{
			return new Rectangle(bin.ReadInt32(), bin.ReadInt32(), bin.ReadInt32(), bin.ReadInt32());
		}

		public static RectangleF ReadRectangleF(BinaryReader bin)
		{
			return new RectangleF(bin.ReadSingle(), bin.ReadSingle(), bin.ReadSingle(), bin.ReadSingle());
		}


		public static bool[] ReadBoolArray(BinaryReader bin)
		{
			int len = bin.ReadInt32();
			if (len == 0)
				return null;
			bool[] result = new bool[len];
			for (int i = 0; i < len; i++)
				result[i] = bin.ReadBoolean();

			return result;
		}
		public static byte[] ReadBytes(BinaryReader bin)
		{
			int len = bin.ReadInt32();
			if (len == 0)
				return null;

			return bin.ReadBytes(len);
		}

		public static float[] ReadFloatArray(BinaryReader bin)
		{
			int len = bin.ReadInt32();
			if (len == 0)
				return null;
			float[] result = new float[len];
			for (int i = 0; i < len; i++)
				result[i] = bin.ReadSingle();

			return result;
		}
		public static double[] ReadDoubleArray(BinaryReader bin)
		{
			int len = bin.ReadInt32();
			if (len == 0)
				return null;
			double[] result = new double[len];
			for (int i = 0; i < len; i++)
				result[i] = bin.ReadDouble();

			return result;
		}
		public static int[] ReadIntArray(BinaryReader bin)
		{
			int len = bin.ReadInt32();
			if (len == 0)
				return null;
			int[] result = new int[len];
			for (int i = 0; i < len; i++)
				result[i] = bin.ReadInt32();

			return result;
		}
        public static ushort[] ReadUshortArray(BinaryReader bin)
        {
            int len = bin.ReadInt32();
            if (len == 0)
                return null;
            ushort[] result = new ushort[len];
            for (int i = 0; i < len; i++)
                result[i] = bin.ReadUInt16();

            return result;
        }


		public static void Write(BinaryWriter bin, StringCollection strings)
		{
			if (strings == null || strings.Count == 0)
			{
				bin.Write(0);
				return;
			}

			int len = strings.Count;
			bin.Write(len);
			for (int i = 0; i < len; i++)
				bin.Write(strings[i]);
		}

        //public static void Write(BinaryWriter bin, PointFCollection objs)
        //{
        //    if (objs == null || objs.Count == 0)
        //    {
        //        bin.Write(0);
        //        return;
        //    }

        //    int len = objs.Count;
        //    bin.Write(len);
        //    for (int i = 0; i < len; i++)
        //    {
        //        bin.Write(objs[i].X);
        //        bin.Write(objs[i].Y);
        //    }
        //}
        //public static void Write(BinaryWriter bin, PointCollection objs)
        //{
        //    if (objs == null || objs.Count == 0)
        //    {
        //        bin.Write(0);
        //        return;
        //    }

        //    int len = objs.Count;
        //    bin.Write(len);
        //    for (int i = 0; i < len; i++)
        //    {
        //        bin.Write(objs[i].X);
        //        bin.Write(objs[i].Y);
        //    }
        //}

		public static void Write(BinaryWriter bin, Point[] objs)
		{
			if (objs == null || objs.Length == 0)
			{
				bin.Write(0);
				return;
			}

			int len = objs.Length;
			bin.Write(len);
			for (int i = 0; i < len; i++)
			{
				bin.Write(objs[i].X);
				bin.Write(objs[i].Y);
			}
		}

        public static void Write(BinaryWriter bin, PointF[] objs)
        {
            if (objs == null || objs.Length == 0)
            {
                bin.Write(0);
                return;
            }

            int len = objs.Length;
            bin.Write(len);
            for (int i = 0; i < len; i++)
            {
                bin.Write(objs[i].X);
                bin.Write(objs[i].Y);
            }
        }

		public static void Write(BinaryWriter bin, bool[] values)
		{
			if (values == null || values.Length == 0)
			{
				bin.Write(0);
				return;
			}

			int len = values.Length;
			bin.Write(len);
			for (int i = 0; i < len; i++)
				bin.Write(values[i]);
		}
		public static void Write(BinaryWriter bin, float[] values)
		{
			if (values == null || values.Length == 0)
			{
				bin.Write(0);
				return;
			}

			int len = values.Length;
			bin.Write(len);
			for (int i = 0; i < len; i++)
				bin.Write(values[i]);
		}
		public static void Write(BinaryWriter bin, double[] values)
		{
			if (values == null || values.Length == 0)
			{
				bin.Write(0);
				return;
			}

			int len = values.Length;
			bin.Write(len);
			for (int i = 0; i < len; i++)
				bin.Write(values[i]);
		}
		public static void Write(BinaryWriter bin, int[] values)
		{
			if (values == null || values.Length == 0)
			{
				bin.Write(0);
				return;
			}

			int len = values.Length;
			bin.Write(len);
			for (int i = 0; i < len; i++)
				bin.Write(values[i]);
		}
		public static void Write(BinaryWriter bin, byte[] values)
		{
			if (values == null || values.Length == 0)
			{
				bin.Write(0);
				return;
			}

			int len = values.Length;
			bin.Write(len);
			for (int i = 0; i < len; i++)
				bin.Write(values[i]);
		}

        public static void Write(BinaryWriter bin, ushort[] values)
        {
            if (values == null || values.Length == 0)
            {
                bin.Write(0);
                return;
            }

            int len = values.Length;
            bin.Write(len);
            for (int i = 0; i < len; i++)
                bin.Write(values[i]);
        }

		public static void Write(BinaryWriter bin, Rectangle rect)
		{
			bin.Write(rect.X);
			bin.Write(rect.Y);
			bin.Write(rect.Width);
			bin.Write(rect.Height);
		}
		public static void Write(BinaryWriter bin, RectangleF rect)
		{
			bin.Write(rect.X);
			bin.Write(rect.Y);
			bin.Write(rect.Width);
			bin.Write(rect.Height);
		}

	}
}
