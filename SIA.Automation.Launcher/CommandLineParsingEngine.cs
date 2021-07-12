using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace SIA.Automation.Launcher
{
	/// <summary>
	/// Provides methods for parsing a command line argument set, into a collection of name/value pairs using a variety of switches and combinations
	/// </summary>
	public class CommandLineParsingEngine : IEnumerable
	{
		#region Member fields
		private StringDictionary _arguments;
		#endregion

		#region Constructor and destructor

		/// <summary>
		/// Initializes a new instance of the CommandLineParsingEngine class
		/// </summary>
		public CommandLineParsingEngine()
		{			
		}

		/// <summary>
		/// Initializes a new instance of the CommandLineParsingEngine class
		/// </summary>
		/// <param name="args">A command line argument set to parse</param>
		public CommandLineParsingEngine(string[] args)
		{
			this.Parse(args);
		}

		#endregion

		#region Properties
	
		/// <summary>
		/// Gets the value associated with the specified parameter name.
		/// </summary>
		/// <param name="paramName">The parameter's name whose value to get</param>
		/// <value>The value associated with the specified parameter name. 
		/// If the specified parameter was not found, attempting to get it return a empty <b>String</b>.</value>
		public string this [string paramName]
		{
			get
			{
				return this.ToString(paramName);
			}
		}			
	
		#endregion
		
		#region Implementation of IEnumerable
		
		/// <summary>
		/// Returns a <see cref="IEnumerator">IEnumerator</see> that can iterate through the command line parameters.
		/// </summary>
		/// <returns>A <see cref="IEnumerator">IEnumerator</see> for the command line arguments</returns>
		public System.Collections.IEnumerator GetEnumerator()
		{
			return _arguments.GetEnumerator();
		}
		
		#endregion

		#region Methods
		/// <summary>
		/// Parses the command line argument set into a collection of name/value pairs
		/// </summary>
		/// <param name="args">The command line argument set</param>
		public void Parse(string[] args)
		{
			_arguments = new StringDictionary();

			Regex Spliter = new Regex(@"^-{1,2}|^/|=|:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			Regex Remover = new Regex(@"^['""]?(.*?)['""]?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

			string param = null;
			string[] paramElements;

			// Valid parameters forms:
			// {-,/,--}param{ ,=,:}((",')value(",'))
			// Examples: -param1 value1 --param2 /param3:"Test-:-work" /param4=happy -param5 '--=nice=--'
			foreach(string arg in args)
			{
				// Look for new parameters (-,/ or --) and a possible enclosed value (=,:)
				paramElements = Spliter.Split(arg, 3);

				switch(paramElements.Length)
				{
					// Found a value (for the last parameter found (space separator))
				case 1:

					if (param!=null)
					{
						if(!_arguments.ContainsKey(param))
						{
							paramElements[0] = Remover.Replace(paramElements[0],"$1");
							_arguments.Add(param,paramElements[0]);
						}
						param = null;
					}
					// else Error: no parameter waiting for a value (skipped)
					break;
					// Found just a parameter

				case 2:
					// The last parameter is still waiting. With no value, set it to true.
					if (param!=null)
					{
						if (!_arguments.ContainsKey(param)) _arguments.Add(param,"true");
					}
					param = paramElements[1];
					break;
					// param with enclosed value

				case 3:
					// The last parameter is still waiting. With no value, set it to true.
					if (param!=null)
					{
						if (!_arguments.ContainsKey(param)) _arguments.Add(param,"true");
					}
					param = paramElements[1];
					// Remove possible enclosing characters (",')
					if (!_arguments.ContainsKey(param))
					{
						paramElements[2] = Remover.Replace(paramElements[2],"$1");
						_arguments.Add(param,paramElements[2]);
					}
					param = null;
					break;
				}
			}
			// In case a parameter is still waiting
			if (param!=null)
			{
				if (!_arguments.ContainsKey(param)) _arguments.Add(param,"true");
			}
		}

		
		/// <summary>
		/// Determine whether an element is in the command line argument set.
		/// </summary>
		/// <param name="paramName">The parameter's name</param>
		/// <returns><b>true</b> if parameter is found in the command line argument set; otherwise, <b>false</b></returns>
		public bool Exists(string paramName)
		{
			return (_arguments[paramName] == null ? false : true);			
		}


		/// <summary>
		/// Converts the value associated with the specified parameter name to its equivalent <see cref="String">String</see> representation.
		/// </summary>
		/// <param name="paramName">The parameter's name whose value to convert.</param>
		/// <returns>The <see cref="Single">Single</see> equivalent of the value.</returns>
		public string ToString(string paramName)
		{
			if (this.Exists(paramName))
                return _arguments[paramName].ToString();
			return string.Empty;
		}

		/// <summary>
		/// Converts the value associated with the specified parameter name to its equivalent <see cref="Boolean">Boolean</see> representation.
		/// </summary>
		/// <param name="paramName">The parameter's name whose value to convert.</param>
		/// <returns>The <see cref="bool">Boolean</see> equivalent of the value.</returns>
		public bool ToBoolean(string paramName)
		{
			if (this.Exists(paramName))
                return Convert.ToBoolean(_arguments[paramName]);
			return false;
		}

		/// <summary>
		/// Converts the value associated with the specified parameter name to its equivalent <see cref="Int16">Int16</see> representation.
		/// </summary>
		/// <param name="paramName">The parameter's name whose value to convert.</param>
		/// <returns>The <see cref="Int16">Int16</see> equivalent of the value.</returns>
		public Int16 ToInt16(string paramName)
		{
			if (this.Exists(paramName))
				return Convert.ToInt16(_arguments[paramName]);
			return 0;
		}

		/// <summary>
		/// Converts the value associated with the specified parameter name to its equivalent <see cref="Int32">Int32</see> representation.
		/// </summary>
		/// <param name="paramName">The parameter's name whose value to convert.</param>
		/// <returns>The <see cref="Int32">Int32</see> equivalent of the value.</returns>
		public Int32 ToInt32(string paramName)
		{
			if (this.Exists(paramName))
				return Convert.ToInt32(_arguments[paramName]);
			return 0;
		}

		/// <summary>
		/// Converts the value associated with the specified parameter name to its equivalent <see cref="Int64">Int64</see> representation.
		/// </summary>
		/// <param name="paramName">The parameter's name whose value to convert.</param>
		/// <returns>The <see cref="Int64">Int64</see> equivalent of the value.</returns>
		public Int64 ToInt64(string paramName)
		{
			if (this.Exists(paramName))
				return Convert.ToInt64(_arguments[paramName]);
			return 0;
		}

		/// <summary>
		/// Converts the value associated with the specified parameter name to its equivalent <see cref="Single">Single</see> representation.
		/// </summary>
		/// <param name="paramName">The parameter's name whose value to convert.</param>
		/// <returns>The <see cref="Single">Single</see> equivalent of the value.</returns>
		public Single ToSingle(string paramName)
		{
			if (this.Exists(paramName))
				return Convert.ToSingle(_arguments[paramName]);
			return 0;
		}

		#endregion
	}
}
