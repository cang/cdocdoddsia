using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;

using SIA.Common;

namespace SIA.UI.Controls.Commands
{
	/// <summary>
	/// RDE performance counter
	/// </summary>
	public class RasterCommandPerformanceCounter : IDisposable
	{
		#region member attribute

		private string _name = string.Empty;
		private string _categoryName = string.Empty;
		private string _counterName = string.Empty;
		private string _instanceName = AppDomain.CurrentDomain.FriendlyName;
		private bool _readOnly = true;

		private double _startValue = 0.0F;
		private double _finishValue = 0.0F;

		private double _minValue = double.MaxValue;
		private double _maxValue = double.MinValue;
		
		private double _total = 0;
		private double _count = 0;
		private double _average = 0;

		private double _lastValue = 0;
		private double _curValue = 0;

		private PerformanceCounter _perfCounter = null;

		private Timer _timer = null;
		private int _period = 500; // miliseconds
		#endregion

		#region member attributes

		public virtual string Name
		{
			get {return _name;}
			set {_name = value;}
		}

		public virtual string CategoryName 
		{
			get {return _categoryName;}
			set {_categoryName = value;}
		}

		public string CounterName
		{
			get {return _counterName;}
			set {_counterName = value;}
		}

		public virtual string InstanceName 
		{
			get {return _instanceName;}
			set {_instanceName = value;}
		}

		public virtual bool ReadOnly
		{
			get {return _readOnly;}
			set {_readOnly = value;}
		}

		public double MinValue
		{
			get {return _minValue;}
		}

		public double MaxValue
		{
			get {return _maxValue;}
		}

		public double LastValue
		{
			get {return _lastValue;}
		}

		public double CurrentValue
		{
			get {return _curValue;}
		}

		public double Average
		{
			get {return _average;}
		}

		public double StartValue
		{
			get {return _average;}
		}

		public double FinishValue
		{
			get {return _curValue;}
		}

		#endregion
		
		#region constructor and destructor

		public RasterCommandPerformanceCounter()
		{
		}

		public RasterCommandPerformanceCounter(string name, string categoryName, string counterName, string instanceName, bool readOnly)
		{
			this._name = name;
			this._categoryName = categoryName;
			this._counterName = counterName;
			this._instanceName = instanceName;
			this._readOnly = readOnly;
		}

		~RasterCommandPerformanceCounter()
		{
			this.Dispose(false);
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			this.Dispose(true);
		}

		public virtual void Dispose(bool disposing)
		{
			if (_perfCounter != null)
				_perfCounter.Dispose();
			_perfCounter = null;
		}

		#endregion

		#region methods
		public virtual void Start()
		{
			if (_perfCounter == null)
			{
				// try to create counter
				this._perfCounter = this.CreatePerformanceCounter(this.CategoryName, this.CounterName, this.InstanceName);
				// if failed than query for existed counters
				if (this._perfCounter == null)
					this._perfCounter = this.QueryPerformanceCounter(this.CategoryName, this.CounterName, this.InstanceName);
			}

			if (_timer != null)
				this.StopTimer();

			if (_perfCounter != null)
				this.StartTimer();	
		}

		public virtual void Finish()
		{
			this.StopTimer();
			// save finish value
			this._finishValue = _curValue;
		}

		public virtual String GetLog()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("Name: " + this.Name);
			sb.Append("\t");
			sb.Append("Counter Name: " + this.CounterName);
			sb.Append("\t");
			sb.Append("Minimum: " + this.MinValue.ToString("N"));
			sb.Append("\t");
			sb.Append("Maximum: " + this.MaxValue.ToString("N"));
			sb.Append("\t");
			sb.Append("Average: " + this.Average.ToString("N"));
			sb.Append("\t");
			sb.Append("Start Value: " + this.StartValue.ToString("N"));
			sb.Append("\t");
			sb.Append("Finish Value: " + this.FinishValue.ToString("N"));

			return sb.ToString();
		}

		#endregion

		#region internal helpers

		private void StartTimer()
		{
			_timer = new Timer(new TimerCallback(Timer_Tick), null, 0, _period);			
		}

		private void StopTimer()
		{
			if (_timer != null)
			{
				using (AutoResetEvent waitHandle = new AutoResetEvent(false))
				{
					_timer.Dispose(waitHandle);
					waitHandle.WaitOne();
				}
			}
		}

		private void Timer_Tick(object state)
		{
			this.OnTimerTick();			
		}
		
		protected virtual void OnTimerTick()
		{
			if (_perfCounter != null)
			{
				if (Monitor.TryEnter(this, 0))
				{
					try
					{
						if (_count == 0.0F)
							this._startValue = this._curValue;

						this._lastValue = this._curValue;
						this._curValue = this._perfCounter.NextValue();
						
						this._minValue = Math.Min(_minValue, _curValue);
						this._maxValue = Math.Max(_maxValue, _curValue);

						this._count += 1.0F;
						this._total += _curValue;
						this._average = _total/_count;
					}
					catch
					{
					}
					finally
					{
						Monitor.Exit(this);
					}
				}
			}
		}

		
		protected PerformanceCounter CreatePerformanceCounter(string categoryName, string counterName, string instanceName)
		{
			PerformanceCounter counter = null;
			
			try
			{
				counter = new PerformanceCounter();
				
				counter.BeginInit();
				counter.CategoryName = categoryName;
				counter.CounterName = counterName;
				counter.InstanceName = instanceName;
				counter.ReadOnly = this.ReadOnly;
				counter.EndInit();

				// try to retrieve value
				counter.NextValue();
			}
			catch
			{
				if (counter != null)
					counter.Dispose();
				counter = null;

				//Trace.WriteLine(String.Format("Cannot create counter Name: {0}; Category: {1}; Counter Name: {2}; Instance Name: {3}; Reason: {4}", 
				//	this._name, categoryName, counterName, instanceName, exp.Message));
			}
			finally
			{

			}

			return counter;
		}

		protected PerformanceCounter QueryPerformanceCounter(string categoryName, string counterName, string instanceName)
		{
			PerformanceCounterCategory[] categories = PerformanceCounterCategory.GetCategories();
			foreach(PerformanceCounterCategory category in categories)
			{
				if (category.CategoryName == categoryName)
				{
					String[] instances = category.GetInstanceNames();
					foreach(String instance in instances)
					{
						if (instanceName.IndexOf(instance) >= 0)
						{
							PerformanceCounter[] counters = category.GetCounters(instance);
					
							foreach(PerformanceCounter counter in counters)
							{
								if (counter.CounterName == counterName)
									return counter;
							}
						}
					}
				}
			}

			return null;
		}

		#endregion
	}

	public class RasterCommandDurationCounter : RasterCommandPerformanceCounter
	{
		#region fields

		private DateTime _startTime = DateTime.Now;
		private DateTime _finishTime = DateTime.Now;

		#endregion

		#region constructor and destructor

		public RasterCommandDurationCounter() : base()
		{
			this.Name = "RasterCommandDurationCounter";
			this.CategoryName = "";
			this.CounterName = "";
			this.InstanceName = "";			
		}

		#endregion

		#region methods

		public override void Start()
		{
			// save start time
			_startTime = DateTime.Now;
		}

		public override void Finish()
		{
			// save finish time
			_finishTime = DateTime.Now;
			base.Finish ();
		}

		public override String GetLog()
		{
			TimeSpan duration = _finishTime - _startTime;
			StringBuilder sb = new StringBuilder();
			sb.Append("Name: " + this.Name);
			sb.Append("\t");
			sb.Append("Start Time: " + this._startTime.ToString("yyyy-MM-dd HH:mm:ss:fffffff"));
			sb.Append("\t");
			sb.Append("Finish Time: " + this._finishTime.ToString("yyyy-MM-dd HH:mm:ss:fffffff"));
			sb.Append("\t");
			sb.Append(String.Format("Total: " + duration.TotalMilliseconds + " (miliseconds)"));
			
			return sb.ToString();
		}

		#endregion

	}

	
	public class RasterCommandPerformanceCounterManager
	{
		#region constants

		const string KEY_PERFORMANCE_LOG = "EnableRasterCommandPerformanceLog";
		const string KEY_CUSTOM_PERFORMANCE_COUNTER = "CustomRasterCommandPerformanceCounters";
				
		public readonly int _majorVersion = 1;
		public readonly int _minorVersion = 0;

		#endregion

		#region fields
		
		private string _logString = null;
		private RasterCommandPerformanceCounter[] _counters = null;
		
		#endregion

		#region properties

		public bool PerformanceLogEnabled
		{
			get
			{
				object value = ConfigurationManager.AppSettings[KEY_PERFORMANCE_LOG];
				if (value != null)
				{
					try 
					{
						return Boolean.Parse(value.ToString());
					}
					catch 
					{
					}

					return false;
				}
				else
					return false;
			}
		}

		public string CustomRasterCommandPerformanceCounters
		{
			get
			{
				object value = ConfigurationManager.AppSettings[KEY_CUSTOM_PERFORMANCE_COUNTER];
				if (value != null)
				{
					try 
					{
						return value.ToString();
					}
					catch 
					{
					}

					return string.Empty;
				}
				else
					return string.Empty;
			}
		}

		public String LogString
		{
			get {return _logString;}
		}


		#endregion

		#region constructor and destructor

		public RasterCommandPerformanceCounterManager()
		{
			if (this.PerformanceLogEnabled)
				this.InitializeCounters();
		}

		~RasterCommandPerformanceCounterManager()
		{
			this.Dispose(false);
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		public virtual void Dispose(bool disposing)
		{
			this.StopCounters();
		}

		#endregion

		#region methods

		public void StartCounters()
		{
			if (this.PerformanceLogEnabled)
			{
				if (this._counters != null && this._counters.Length > 0)
				{
					foreach(RasterCommandPerformanceCounter counter in this._counters)
						counter.Start();
				}
			}
		}

		public void StopCounters()
		{
			if (this.PerformanceLogEnabled)
			{
				// save log string and uninitialize performance counters
				if (this._counters != null && this._counters.Length > 0)
				{					
					StringBuilder sb = new StringBuilder();
					sb.Append("\tStart Trace Performance Information.\r\n");
					sb.Append(String.Format("\tVersion: {0}.{1}\r\n", _majorVersion, _minorVersion));

					for (int i=0; i<_counters.Length; i++)
					{
						RasterCommandPerformanceCounter counter = (RasterCommandPerformanceCounter)_counters[i];
						if (counter != null)
						{
							// stop counter
							counter.Finish();

							// log command performance information
							string log = counter.GetLog();
							sb.Append("\t" + log);
							sb.Append("\r\n");
						}
						_counters[i] = null;
					}

					sb.Append("\tFinish Trace Performance Information.\r\n");

					this._logString = sb.ToString();

					// clean up counter references
					this._counters = null;
				}
			}
		}

		public void LogPerformanceData()
		{
			if (this.PerformanceLogEnabled)
			{
				string buffer = this._logString;
				if (buffer != null && buffer.Length > 0)
					Console.WriteLine(buffer);
			}
		}


		#endregion

		#region internal helpers

		private void InitializeCounters()
		{
			ArrayList result = new ArrayList();

			try
			{
				RasterCommandPerformanceCounter[] counters = this.LoadFromInternal();
				if (counters != null)
					result.AddRange(counters);
			}
			catch (System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
			
			try
			{
				RasterCommandPerformanceCounter[] counters = this.LoadFromFile();
				if (counters != null)
					result.AddRange(counters);
			}
			catch (System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
			
			this._counters = (RasterCommandPerformanceCounter[])result.ToArray(typeof(RasterCommandPerformanceCounter));
		}

		
		private RasterCommandPerformanceCounter[] LoadFromInternal()
		{
			string instanceName = AppDomain.CurrentDomain.FriendlyName;
			return new RasterCommandPerformanceCounter[] {
				new RasterCommandDurationCounter(),
				new RasterCommandPerformanceCounter("Number of Bytes in All Heaps", ".NET CLR Memory", "# Bytes in all Heaps", instanceName, true),
				new RasterCommandPerformanceCounter("Number of Bytes in All Heaps", ".NET CLR Memory", "# GC Handles", instanceName, true),
			};
		}

		private RasterCommandPerformanceCounter[] LoadFromFile()
		{
			RasterCommandPerformanceCounter[] result = null;

			try
			{
				string filePath = this.CustomRasterCommandPerformanceCounters;
				if (File.Exists(filePath))
				{
					using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
					{
						XmlSerializerEx serializer = new XmlSerializerEx(typeof(RasterCommandPerformanceCounter[]));
						result = (RasterCommandPerformanceCounter[])serializer.Deserialize(reader);
					}
				}
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{
			}

			return result;
		}

		private void SaveToFile(string fileName, RasterCommandPerformanceCounter[] counters)
		{
			try
			{
				using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.UTF8))
				{
					XmlSerializerEx serializer = new XmlSerializerEx(typeof(RasterCommandPerformanceCounter[]));
					serializer.Serialize(writer, counters);
				}
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{
			}
		}


		#endregion
	}
}
