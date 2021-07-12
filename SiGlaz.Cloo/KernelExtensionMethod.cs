
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Cloo;

///*
// * I Want reuse Cloo.ComputeKernel to do more method
// * 
// * 
// * Cang
//*/

//namespace SiGlaz.Cloo
//{
//    public static class KernelExtensionMethod
//    {
//        public static void SetArguments(this ComputeKernel kernel, ComputeMemory [] Variables)
//        {
//            for (int i = 0; i < Variables.Length; i++)
//            {
//                kernel.SetMemoryArgument(i, Variables[i]);
//            }
//        }

//        public static ComputeCommandQueue CQ
//        {
//            get
//            {
//                return DeviceProgram.Program.DefaultCQ;
//            }
//        }

//         /// <summary>Execute this kernel</summary>
//        /// <param name="CQ">Command queue to use</param>
//        /// <param name="Arguments">Arguments of the kernel function</param>
//        /// <param name="GlobalWorkSize">Array of maximum index arrays. Total work-items = product(max[i],i+0..n-1), n=max.Length</param>
//        /// <param name="LocalWorkSize">Local work sizes</param>
//        /// <param name="events">Event of this command</param>
//        public static void Execute(this ComputeKernel kernel,ComputeCommandQueue CQ, ComputeMemory[] Arguments, int[] GlobalWorkSize, int[] LocalWorkSize, ICollection<ComputeEventBase> events)
//        {
//            kernel.SetArguments(Arguments);
//            if (LocalWorkSize != null && GlobalWorkSize.Length != LocalWorkSize.Length) throw new Exception("Global and local work size must have same dimension");


//            long[] globWSize = new long[GlobalWorkSize.Length];
//            for (int i = 0; i < globWSize.Length; i++) globWSize[i] = GlobalWorkSize[i];
//            long[] locWSize = null;

//            if (LocalWorkSize != null)
//            {
//                locWSize = new long[LocalWorkSize.Length];
//                for (int i = 0; i < locWSize.Length; i++) locWSize[i] = LocalWorkSize[i];
//            }
//            CQ.Execute(kernel, null, globWSize, locWSize, events);
//        }

//          /// <summary>Execute this kernel</summary>
//        /// <param name="GlobalWorkSize">Array of maximum index arrays. Total work-items = product(max[i],i+0..n-1), n=max.Length</param>
//        /// <param name="Arguments">Arguments of the kernel function</param>
//        public static void Execute(this ComputeKernel kernel, ComputeMemory[] Arguments, int[] GlobalWorkSize)
//        {
//            kernel.Execute(CQ, Arguments, GlobalWorkSize, null, null);
//        }

//        /// <summary>Execute this kernel using work_dim = 1</summary>
//        /// <param name="GlobalWorkSize">Global work size in one-dimension. global_work_size = new int[1] {GlobalWorkSize}</param>
//        /// <param name="Arguments">Arguments of the kernel function</param>
//        public static void Execute(this ComputeKernel kernel, ComputeMemory[] Arguments, int GlobalWorkSize)
//        {
//            kernel.Execute(CQ, Arguments, new int[] { GlobalWorkSize }, null, null);
//        }

//        /// <summary>Execute this kernel</summary>
//        /// <param name="GlobalWorkSize">Array of maximum index arrays. Total work-items = product(max[i],i+0..n-1), n=max.Length</param>
//        /// <param name="LocalWorkSize">Local work sizes</param>
//        /// <param name="Arguments">Arguments of the kernel function</param>
//        public static void Execute(this ComputeKernel kernel, ComputeMemory[] Arguments, int[] GlobalWorkSize, int[] LocalWorkSize)
//        {
//            kernel.Execute(CQ, Arguments, GlobalWorkSize, LocalWorkSize, null);
//        }

//        /// <summary>Execute this kernel</summary>
//        /// <param name="GlobalWorkSize">Array of maximum index arrays. Total work-items = product(max[i],i+0..n-1), n=max.Length</param>
//        /// <param name="LocalWorkSize">Local work sizes</param>
//        /// <param name="Arguments">Arguments of the kernel function</param>
//        /// <param name="events">Events list</param>
//        public static void Execute(this ComputeKernel kernel, ComputeMemory[] Arguments, int[] GlobalWorkSize, int[] LocalWorkSize, ICollection<ComputeEventBase> events)
//        {
//            kernel.Execute(CQ, Arguments, GlobalWorkSize, LocalWorkSize, events);
//        }

//    }
//}
