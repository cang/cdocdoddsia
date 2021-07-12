using System;
using System.Collections.Generic;
using System.Text;
using Cloo;
using Cloo.Bindings;
using System.Diagnostics;


namespace SiGlaz.Cloo
{


    public static class DeviceProgram
    {
        /// <summary>List of available platforms</summary>
        public static List<ComputePlatform> CLPlatforms;

        /// <summary>List of available devices</summary>
        public static List<ComputeDevice> CLDevices;

        public static bool GPUSupported = false;

        /// <summary>Initialization error</summary>
        public static string ErrorDescription = "";

        /// <summary>OpenCL context using all devices</summary>
        public static ComputeContext Context;

        /// <summary>Synchronous command queues that are executed in call order</summary>
        public static List<ComputeCommandQueue> CommQueues;

        /// <summary>Asynchronous command queues</summary>
        public static List<ComputeCommandQueue> AsyncCommQueues;

        /// <summary>Default synchronous command queue set as the first GPU, for ease of use.</summary>
        public static int DefaultCQIndex;
        public static ComputeCommandQueue DefaultCQ
        {
            get
            {
                return CommQueues[DefaultCQIndex];
            }
        }
        

        /// <summary>Compiled program</summary>
        public static ComputeProgram Prog;


        public static void InitCL()
        {
            if (GPUSupported) return;
            InitCL(ComputeDeviceTypes.Gpu,null,null);
            if( !GPUSupported )
                InitCL(ComputeDeviceTypes.All, null, null);
        }

        /// <summary>Initializes OpenCL and reads devices. Uses previously created context and command queue if supplied. In that case DevicesToUse is ignored.</summary>
        public static void InitCL(ComputeDeviceTypes DevicesToUse, ComputeContext PrevCtx, ComputeCommandQueue PrevCQ)
        {
            if (!GPUSupported)
            {
                try
                {
                    if (ComputePlatform.Platforms.Count > 0)
                        GPUSupported = true;
                    else
                        GPUSupported = false;

                    //Program.Event = new List<ComputeEventBase>();

                    CLPlatforms = new List<ComputePlatform>();
                    foreach (ComputePlatform pp in ComputePlatform.Platforms) CLPlatforms.Add(pp);

                    ComputeContextPropertyList Properties = new ComputeContextPropertyList(ComputePlatform.Platforms[0]);

                    if (PrevCtx == null)
                    {
                        Context = new ComputeContext(DevicesToUse, Properties, null, IntPtr.Zero);
                    }
                    else Context = PrevCtx;

                    CLDevices = new List<ComputeDevice>();
                    for (int i = 0; i < Context.Devices.Count; i++)
                    {
                        CLDevices.Add(Context.Devices[i]);

                    }

                    CommQueues = new List<ComputeCommandQueue>();
                    AsyncCommQueues = new List<ComputeCommandQueue>();
                    DefaultCQIndex = -1;

                    if (PrevCQ == null)
                    {
                        for (int i = 0; i < CLDevices.Count; i++)
                        {
                            //Comandos para os devices
                            ComputeCommandQueue CQ = new ComputeCommandQueue(Context, CLDevices[i], ComputeCommandQueueFlags.None);

                            ComputeCommandQueue AsyncCQ = new ComputeCommandQueue(Context, CLDevices[i], ComputeCommandQueueFlags.OutOfOrderExecution);



                            //Comando para a primeira GPU
                            if ((CLDevices[i].Type == ComputeDeviceTypes.Gpu || CLDevices[i].Type == ComputeDeviceTypes.Accelerator) && DefaultCQIndex < 0)
                                DefaultCQIndex = i;

                            CommQueues.Add(CQ);
                            AsyncCommQueues.Add(AsyncCQ);

                        }
                        //Só tem CPU
                        if (DefaultCQIndex < 0 && CommQueues.Count > 0) DefaultCQIndex = 0;
                    }
                    else
                    {
                        CommQueues.Add(PrevCQ);
                        DefaultCQIndex = 0;
                    }
                }
                catch (Exception ex)
                {
                    ErrorDescription = ex.ToString();
                    GPUSupported = false;
                }

            }
        }

        /// <summary>Ends all commands being executed</summary>
        public static void Finish()
        {
            for (int i = 0; i < CommQueues.Count; i++)
            {
                CommQueues[i].Finish();
                AsyncCommQueues[i].Finish();
            }
        }

        #region Compilation

        public static long t = 0;

        /// <summary>Compiles program contained in a single string.</summary>
        /// <param name="SourceCode">Source code to compile</param>
        public static void Compile(string SourceCode)
        {
            Stopwatch sw = new Stopwatch(); sw.Start();
            
            List<string> Logs;
            Compile(new string[] { SourceCode }, out Logs);

            sw.Stop(); t += sw.ElapsedMilliseconds;
        }

        /// <summary>Compiles program contained in a single string. Returns build logs for each device.</summary>
        /// <param name="SourceCode">Source code to compile</param>
        /// <param name="BuildLogs">Build logs for each device</param>
        public static void Compile(string SourceCode, out List<string> BuildLogs)
        {
            Compile(new string[] { SourceCode }, out BuildLogs);
        }

        /// <summary>Compiles the program.</summary>
        /// <param name="SourceCode">Source code to compile</param>
        public static void Compile(string[] SourceCode)
        {
            List<string> Logs;
            Compile(SourceCode, out Logs);
        }

        /// <summary>Compiles the program. Returns the build logs for each device.</summary>
        /// <param name="SourceCode">Source code array to compile</param>
        /// <param name="BuildLogs">Build logs for each device</param>
        public static void Compile(string[] SourceCode, out List<string> BuildLogs)
        {
            //CLProgram Prog = OpenCLDriver.clCreateProgramWithSource(ContextoGPUs, 1, new string[] { sProgramSource }, null, ref Err);
            Prog = new ComputeProgram(Context, SourceCode);


            //Verifica se compilou em algum device
            bool funcionou = false;

            for (int i = 0; i < CLDevices.Count; i++)
            {
                try
                {
                    Prog.Build(new List<ComputeDevice>() { CLDevices[i] }, "", null, IntPtr.Zero);
                    funcionou = true;
                }
                catch
                {
                    throw;
                }
            }

            //Build Information
            BuildLogs = new List<string>();
            for (int i = 0; i < CLDevices.Count; i++)
            {
                string LogInfo = "";
                try
                {
                    LogInfo = Prog.GetBuildLog(CLDevices[i]);
                }
                catch
                {
                    LogInfo = "Error retrieving build info";
                }
                //if (!CLCalc.CLDevices[i].CLDeviceAvailable) LogInfo = "Possible compilation failure for device " + i.ToString() + "\n" + LogInfo;
                BuildLogs.Add(LogInfo);
            }

            //Nao compilou em nenhum, joga exception
            if (!funcionou)
            {
                throw new Exception("Could not compile program");
            }
        }

        #endregion

        #region Kernel

        public static ComputeKernel CreateKernel(string kernelName)
        {
            Stopwatch sw = new Stopwatch(); sw.Start();
            ComputeKernel ret = Prog.CreateKernel(kernelName);
            sw.Stop(); t += sw.ElapsedMilliseconds;
            return ret;
        }

        public static void SetArguments(/*this*/ ComputeKernel kernel, ComputeMemory [] Variables)
        {
            for (int i = 0; i < Variables.Length; i++)
            {
                kernel.SetMemoryArgument(i, Variables[i]);
            }
        }

        /// <summary>Execute this kernel</summary>
        /// <param name="CQ">Command queue to use</param>
        /// <param name="Arguments">Arguments of the kernel function</param>
        /// <param name="GlobalWorkSize">Array of maximum index arrays. Total work-items = product(max[i],i+0..n-1), n=max.Length</param>
        /// <param name="LocalWorkSize">Local work sizes</param>
        /// <param name="events">Event of this command</param>
        public static void ExecuteKernel(/*this */ComputeKernel kernel, ComputeCommandQueue CQ, ComputeMemory[] Arguments, int[] GlobalWorkSize, int[] LocalWorkSize, ICollection<ComputeEventBase> events)
        {
            SetArguments(kernel,Arguments);
            if (LocalWorkSize != null && GlobalWorkSize.Length != LocalWorkSize.Length) throw new Exception("Global and local work size must have same dimension");


            long[] globWSize = new long[GlobalWorkSize.Length];
            for (int i = 0; i < globWSize.Length; i++) globWSize[i] = GlobalWorkSize[i];
            long[] locWSize = null;

            if (LocalWorkSize != null)
            {
                locWSize = new long[LocalWorkSize.Length];
                for (int i = 0; i < locWSize.Length; i++) locWSize[i] = LocalWorkSize[i];
            }
            CQ.Execute(kernel, null, globWSize, locWSize, events);
        }

        /// <summary>Execute this kernel</summary>
        /// <param name="GlobalWorkSize">Array of maximum index arrays. Total work-items = product(max[i],i+0..n-1), n=max.Length</param>
        /// <param name="Arguments">Arguments of the kernel function</param>
        public static void ExecuteKernel(/*this */ComputeKernel kernel, ComputeMemory[] Arguments, int[] GlobalWorkSize)
        {
            ExecuteKernel(kernel,DefaultCQ, Arguments, GlobalWorkSize, null, null);
        }

        /// <summary>Execute this kernel using work_dim = 1</summary>
        /// <param name="GlobalWorkSize">Global work size in one-dimension. global_work_size = new int[1] {GlobalWorkSize}</param>
        /// <param name="Arguments">Arguments of the kernel function</param>
        public static void ExecuteKernel(/*this */ComputeKernel kernel, ComputeMemory[] Arguments, int GlobalWorkSize)
        {
            ExecuteKernel(kernel,DefaultCQ, Arguments, new int[] { GlobalWorkSize }, null, null);
        }

        /// <summary>Execute this kernel</summary>
        /// <param name="GlobalWorkSize">Array of maximum index arrays. Total work-items = product(max[i],i+0..n-1), n=max.Length</param>
        /// <param name="LocalWorkSize">Local work sizes</param>
        /// <param name="Arguments">Arguments of the kernel function</param>
        public static void ExecuteKernel(/*this */ComputeKernel kernel, ComputeMemory[] Arguments, int[] GlobalWorkSize, int[] LocalWorkSize)
        {
            ExecuteKernel(kernel,DefaultCQ, Arguments, GlobalWorkSize, LocalWorkSize, null);
        }

        /// <summary>Execute this kernel</summary>
        /// <param name="GlobalWorkSize">Array of maximum index arrays. Total work-items = product(max[i],i+0..n-1), n=max.Length</param>
        /// <param name="LocalWorkSize">Local work sizes</param>
        /// <param name="Arguments">Arguments of the kernel function</param>
        /// <param name="events">Events list</param>
        public static void ExecuteKernel(/*this */ComputeKernel kernel, ComputeMemory[] Arguments, int[] GlobalWorkSize, int[] LocalWorkSize, ICollection<ComputeEventBase> events)
        {
            ExecuteKernel(kernel,DefaultCQ, Arguments, GlobalWorkSize, LocalWorkSize, events);
        }



        #endregion Kernel

        #region Memory

        //public static DeviceBuffer<T> CreateHostBufferReadOnly<T>(T[] data) where T : struct
        //{
        //    return new DeviceBuffer<T>(ComputeMemoryFlags.ReadOnly, data);
        //}

        //public static DeviceBuffer<T> CreateHostBufferReadWrite<T>(T[] data) where T : struct
        //{
        //    return new DeviceBuffer<T>(ComputeMemoryFlags.ReadWrite, data);
        //}


        #endregion Memory

      

    }

}
