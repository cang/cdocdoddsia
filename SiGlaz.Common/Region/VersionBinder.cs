using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

using SiGlaz.Common;

namespace SiGlaz.Common
{
	/// <summary>
	/// Summary description for VersionBinder.
	/// </summary>
	public class VersionBinder : SerializationBinder
	{
		public override Type BindToType(string assemblyName, string typeName)
		{
			Type type = typeof(GraphicsList);
			if (type.FullName == typeName)
				return type;
			return null;
		}
	}
}
	