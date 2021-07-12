using System;
using System.Data;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;

using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

using SIA.Common;
using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;

using SIA.SystemLayer;
using SIA.Plugins.Common;

namespace SIA.UI.Controls.Commands
{
	[Serializable]
	public abstract class RasterCommandSettings : IDisposable, ICloneable, ISerializable, IRasterCommandSettings
	{
		#region Member Fields
		
		private int _version = 1;

		#endregion

		#region Properties
		
		/// <summary>
		/// Gets the version of the <b>RasterCommandSettings</b> class;
		/// </summary>
		/// <remarks>
		/// The Version property is internal used for version management 
		/// and do not intent to use it directly.
		/// </remarks>		
		public int Version
		{
			get {return _version;}
			set {_version = value;}
		}

		#endregion

		#region Constructor and Destructor

		public RasterCommandSettings()
		{
		}

		public RasterCommandSettings(SerializationInfo info, StreamingContext context)
		{
		}

		~RasterCommandSettings()
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

		protected virtual void Dispose(bool disposing)
		{
			
		}

		#endregion

		#region Methods

		/// <summary>
		/// The <b>Copy</b> method copied data from the <see cref="RasterCommandSettings">RasterCommandSettings</see> object.
		/// </summary>
		/// <param name="settings">The source where data is copied.</param>
		public virtual void Copy(RasterCommandSettings settings)
		{
			this._version = settings._version;
		}

		public virtual void Serialize(System.IO.Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException("stream");
			using (StreamWriter writer = new StreamWriter(stream))
			{
				XmlSerializerEx serializer = new XmlSerializerEx(this.GetType());
				serializer.Serialize(writer, this);
			}
		}

		public virtual void Deserialize(System.IO.Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException("stream");
			using (StreamReader reader = new StreamReader(stream))
			{
				XmlSerializerEx serializer = new XmlSerializerEx(this.GetType());
				RasterCommandSettings settings = (RasterCommandSettings)serializer.Deserialize(reader);
				this.Copy(settings);
			}
		}

		#endregion

		#region Abstract methods

		public abstract void Validate();
		
		#endregion

		#region ICloneable Members

		/// <summary>
		/// Creates a shallow copy of the <see cref="RasterCommandSettings">RasterCommandSettings</see>.
		/// </summary>
		/// <returns>A shallow copy of the <see cref="RasterCommandSettings">RasterCommandSettings</see>.</returns>
		public object Clone()
		{
			Type type = this.GetType();
			Assembly assembly = type.Assembly;
			// initializes a new instance with the same type of this instance
			System.Runtime.Remoting.ObjectHandle objHandle = Activator.CreateInstance(assembly.FullName, type.FullName);
			// copy settings from this instance to the newly created one.
			RasterCommandSettings settings = (RasterCommandSettings)objHandle.Unwrap();
			settings.Copy(this);
			// return the cloned object
			return settings;
		}

		#endregion

		#region ISerializable Members

		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			
		}

		#endregion
	}
}
