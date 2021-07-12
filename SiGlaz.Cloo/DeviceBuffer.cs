using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using Cloo;
using Cloo.Bindings;
using System.Runtime.InteropServices;

namespace SiGlaz.Cloo
{
    public class DeviceBuffer<T> : ComputeBuffer<T> where T : struct
    {

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="ComputeBuffer{T}"/>.
        /// </summary>
        /// <param name="context"> A <see cref="ComputeContext"/> used to create the <see cref="ComputeBuffer{T}"/>. </param>
        /// <param name="flags"> A bit-field that is used to specify allocation and usage information about the <see cref="ComputeBuffer{T}"/>. </param>
        /// <param name="count"> The number of elements of the <see cref="ComputeBuffer{T}"/>. </param>
        public DeviceBuffer(ComputeContext context, ComputeMemoryFlags flags, long count)
            : base(context, flags, count, IntPtr.Zero)
        { }


        /// <summary>
        /// Creates a new <see cref="ComputeBuffer{T}"/>.
        /// </summary>
        /// <param name="context"> A <see cref="ComputeContext"/> used to create the <see cref="ComputeBuffer{T}"/>. </param>
        /// <param name="flags"> A bit-field that is used to specify allocation and usage information about the <see cref="ComputeBuffer{T}"/>. </param>
        /// <param name="count"> The number of elements of the <see cref="ComputeBuffer{T}"/>. </param>
        /// <param name="dataPtr"> A pointer to the data for the <see cref="ComputeBuffer{T}"/>. </param>
        public DeviceBuffer(ComputeContext context, ComputeMemoryFlags flags, long count, IntPtr dataPtr)
            : base(context, flags,count,dataPtr)
        {
        }

        /// <summary>
        /// Creates a new <see cref="ComputeBuffer{T}"/>.
        /// </summary>
        /// <param name="context"> A <see cref="ComputeContext"/> used to create the <see cref="ComputeBuffer{T}"/>. </param>
        /// <param name="flags"> A bit-field that is used to specify allocation and usage information about the <see cref="ComputeBuffer{T}"/>. </param>
        /// <param name="data"> The data for the <see cref="ComputeBuffer{T}"/>. </param>
        public DeviceBuffer(ComputeContext context, ComputeMemoryFlags flags, T[] data)
            : base(context, flags,data)
        {
        }


        //default context

        /// <summary>
        /// Creates a new <see cref="ComputeBuffer{T}"/>.
        /// </summary>
        /// <param name="flags"> A bit-field that is used to specify allocation and usage information about the <see cref="ComputeBuffer{T}"/>. </param>
        /// <param name="count"> The number of elements of the <see cref="ComputeBuffer{T}"/>. </param>
        public DeviceBuffer(ComputeMemoryFlags flags, long count)
            : this( DeviceProgram.Context , flags, count, IntPtr.Zero)
        { 
        }

        /// <summary>
        /// Creates a new <see cref="ComputeBuffer{T}"/>.
        /// </summary>
        /// <param name="flags"> A bit-field that is used to specify allocation and usage information about the <see cref="ComputeBuffer{T}"/>. </param>
        /// <param name="count"> The number of elements of the <see cref="ComputeBuffer{T}"/>. </param>
        /// <param name="dataPtr"> A pointer to the data for the <see cref="ComputeBuffer{T}"/>. </param>
        public DeviceBuffer(ComputeMemoryFlags flags, long count, IntPtr dataPtr)
            : this(DeviceProgram.Context , flags, count, dataPtr)
        {
        }

        /// <summary>
        /// Creates a new <see cref="ComputeBuffer{T}"/>.
        /// </summary>
        /// <param name="flags"> A bit-field that is used to specify allocation and usage information about the <see cref="ComputeBuffer{T}"/>. </param>
        /// <param name="data"> The data for the <see cref="ComputeBuffer{T}"/>. </param>
        public DeviceBuffer(ComputeMemoryFlags flags, T[] data)
            : this(DeviceProgram.Context,flags, data)
        {
        }

        #endregion

        #region Public static methods

        public static DeviceBuffer<T> CreateHostBufferReadOnly(long count, IntPtr dataPtr)
        {
            return new DeviceBuffer<T>(ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.UseHostPointer
                , count,dataPtr);
        }

        public static DeviceBuffer<T> CreateHostBufferReadWrite(long count, IntPtr dataPtr)
        {
            return new DeviceBuffer<T>(ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.UseHostPointer
                , count, dataPtr);
        }

        public static DeviceBuffer<T> CreateHostBufferReadOnly(T[] data) 
        {
            return new DeviceBuffer<T>(ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.UseHostPointer, data);
        }

        public static DeviceBuffer<T> CreateHostBufferReadWrite(T[] data)
        {
            return new DeviceBuffer<T>(ComputeMemoryFlags.ReadWrite| ComputeMemoryFlags.UseHostPointer, data);
        }

        public static DeviceBuffer<T> CreateBufferReadOnly(long count)
        {
            return new DeviceBuffer<T>(ComputeMemoryFlags.ReadOnly, count);
        }

        //need Write to device 
        public static DeviceBuffer<T> CreateBufferReadOnly(T[] data)
        {
            DeviceBuffer<T> ret = new DeviceBuffer<T>(ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, data);
            return ret;
        }

        public static DeviceBuffer<T> CreateBufferReadOnly(long count, IntPtr dataPtr)
        {
            DeviceBuffer<T> ret = new DeviceBuffer<T>(ComputeMemoryFlags.ReadOnly,count);
            ret.Write(0, count, dataPtr, null);
            return ret;
        }

        public static DeviceBuffer<T> CreateBufferReadWrite(long count)
        {
            return new DeviceBuffer<T>(ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.AllocateHostPointer, count);
        }

        //need Write to device 
        public static DeviceBuffer<T> CreateBufferReadWrite(T[] data)
        {
            DeviceBuffer<T> ret = new DeviceBuffer<T>(ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, data);
            return ret;
        }

        public static DeviceBuffer<T> CreateBufferReadWrite(long count, IntPtr dataPtr)
        {
            DeviceBuffer<T> ret = new DeviceBuffer<T>(ComputeMemoryFlags.ReadWrite, count);
            ret.Write(0, count, dataPtr, null);
            return ret;
        }

        public static DeviceBuffer<T> CreateBufferWriteOnly(long count)
        {
            return new DeviceBuffer<T>(ComputeMemoryFlags.WriteOnly, count);
        }

        //need Write to device 
        public static DeviceBuffer<T> CreateBufferWriteOnly(T[] data)
        {
            DeviceBuffer<T> ret = new DeviceBuffer<T>(ComputeMemoryFlags.WriteOnly | ComputeMemoryFlags.CopyHostPointer, data);
            return ret;
        }

        public static DeviceBuffer<T> CreateBufferWriteOnly(long count, IntPtr dataPtr)
        {
            DeviceBuffer<T> ret = new DeviceBuffer<T>(ComputeMemoryFlags.WriteOnly, count);
            ret.Write(0, count, dataPtr, null);
            return ret;
        }

        #endregion

        #region public methods

        public T[] Read(ComputeCommandQueue CQ,long offset, long count, ICollection<ComputeEventBase> events) 
        {
            T[] localArray = new T[count];
            GCHandle handle = GCHandle.Alloc(localArray, GCHandleType.Pinned);
            try
            {
                CQ.Read<T>(this, true, offset, count, handle.AddrOfPinnedObject(), events);
            }
            finally
            {
                handle.Free();
            }
            return localArray;
        }

        public T[] Read(ComputeCommandQueue CQ,ICollection<ComputeEventBase> events)
        {
            return Read(CQ,0,this.Count,events);
        }

        public T[] Read(long offset, long count, ICollection<ComputeEventBase> events) 
        {
            return Read(DeviceProgram.DefaultCQ, offset, count, events);
        }

        public T[] Read(ICollection<ComputeEventBase> events)
        {
            return Read(DeviceProgram.DefaultCQ, 0, this.Count, events);
        }

        public void Read(long offset, long count, IntPtr dataPtr,ICollection<ComputeEventBase> events)
        {
            DeviceProgram.DefaultCQ.Read<T>(this, true, offset, count, dataPtr, events);
        }

        public void Read(IntPtr dataPtr, ICollection<ComputeEventBase> events)
        {
            DeviceProgram.DefaultCQ.Read<T>(this, true, 0, this.Count, dataPtr, events);
        }

        public void Write(long offset, long count, IntPtr dataPtr,ICollection<ComputeEventBase> events)
        {
            DeviceProgram.DefaultCQ.Write<T>(this, true, offset, count, dataPtr, events);
        }

        public void Write(T[] data, ICollection<ComputeEventBase> events)
        {
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                DeviceProgram.DefaultCQ.Write<T>(this, true, 0, data.Length, handle.AddrOfPinnedObject(), null);
            }
            finally
            {
                handle.Free();
            }
        }

        #endregion public methods


    }

}
