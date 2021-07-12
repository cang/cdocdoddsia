using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace SIA.UI.Controls.Automation
{
    /// <summary>
    /// The utility class provides function for file name mask pattern
    /// </summary>
	public class StringParser
	{
		#region Constants
		
		public const string patName = "N";
		public const string patExt = "E";		
		public const string patCounter = "C";

		public const string patYear = "Y";
		public const string patMonth = "M";
		public const string patDay = "D";
		public const string patHour = "h";
		public const string patMinute = "m";
		public const string patSecond = "s";

		public const string patDate = "Date";
		public const string patDateTime = "DateTime";

		#endregion

		#region Static Members

		public static string yearFormat = "####";
		public static string monthFormat = "0#";
		public static string dayFormat = "0#";

		public static string hourFormat = "0#";
		public static string minuteFormat = "0#";
		public static string secondFormat = "0#";
        public static string milliSecondFormat = "0#";

		private static ArrayList _parsers = null;

		#endregion

		#region Constructor and destructor

		static StringParser()
		{
			_parsers = new ArrayList();
			_parsers.Add(new FileNameParser());
			_parsers.Add(new FileExtensionParser());
			_parsers.Add(new FileCounterParser());

			// NOTES: always make date time parser is the last in parser list
			_parsers.Add(new CurrentDateTimeParser());
		}

		#endregion

		#region Methods

		public static String GetString(String mask, String value, int counter)
		{
			return StringParser.Parse(mask, null);
		}

		public static bool CheckValid(string str)
		{
			return true;
		}

		public static string Parse(string str)
		{
			return Parse(str, null);
		}

        private const string patternMask = @"(?<temp>[^\[]*\[(?<mask>[^\]]*)][^\[^\]]*)*";
        private static Regex regMask = null;

		public static string Parse(string str, WorkingSpace workingSpace)
		{
            string result = str.Clone() as string;

            if (regMask == null)
                regMask = new Regex(patternMask);
            if (regMask == null)
                return str;

            Match match = regMask.Match(str);
            if (match.Success)
            {
                CaptureCollection maskes = match.Groups["mask"].Captures;
                if (maskes != null && maskes.Count > 0)
                {
                    foreach (Capture mask_capture in maskes)
                    {
                        if (mask_capture == null) continue;

                        string mask = mask_capture.Value;
                        
                        if (mask == string.Empty) continue;

                        // search for appreciated parser
                        foreach (IPatternParser parser in _parsers)
                        {
                            if (parser.CanParse(mask))
                            {
                                // parse text
                                string text = parser.Parse(mask, workingSpace);

                                string substr = string.Format("[{0}]", mask);

                                result = result.Replace(substr, text);
                            }
                        }
                    }
                }
            }

            return result;
		}


		#endregion

		#region Internal Helpers

		#endregion
	}

	internal interface IPatternParser
	{
		bool CanParse(string mask);
		string Parse(string mask, WorkingSpace workingSpace);
	};

	internal class FileNameParser	: IPatternParser
	{
		public FileNameParser()
		{
		}

		public bool CanParse(string mask)
		{
			string str = mask.Trim();
			if (String.Compare(str, "N", true) == 0)
				return true;
			return false;
		}

		public virtual string Parse(string mask, WorkingSpace workingSpace)
		{
			string filePath = "[FilePath]";
			if (workingSpace != null)
				filePath = workingSpace.ProcessingFileName;
			return Path.GetFileNameWithoutExtension(filePath);
		}
	};

	internal class FileExtensionParser	: IPatternParser
	{
		public FileExtensionParser() 			
		{
		}

		public bool CanParse(string mask)
		{
			string str = mask.Trim();
			if (String.Compare(str, "E", true) == 0)
				return true;
			return false;
		}

		public virtual string Parse(string mask, WorkingSpace workingSpace)
		{
			string filePath = "[FilePath]";
			if (workingSpace != null)
				filePath = workingSpace.ProcessingFileName;
			return Path.GetExtension(filePath);
		}
	};

	internal class FileCounterParser : IPatternParser
	{
		public FileCounterParser() 			
		{
		}

		public bool CanParse(string mask)
		{
			string str = mask.Trim();
			if (String.Compare(str, "C", true) == 0)
				return true;
			return false;
		}

		public virtual string Parse(string mask, WorkingSpace workingSpace)
		{
			if (workingSpace != null)
				return workingSpace.Counter.ToString();
			return "[Counter]";
		}
	};

	internal class FileDirectoryParser : IPatternParser
	{
		public FileDirectoryParser() 
		{
		}

		public bool CanParse(string mask)
		{
			string str = mask.Trim();
			if (String.Compare(str, "Dir", true) == 0)
				return true;
			return false;
		}

		public virtual string Parse(string mask, WorkingSpace workingSpace)
		{
			string filePath = "[Directory]";
			if (workingSpace != null)
				filePath = workingSpace.ProcessingFileName;
			return Path.GetDirectoryName(filePath);
		}
	};

	internal class CurrentDateTimeParser : IPatternParser
	{
		public CurrentDateTimeParser() 
		{
		}

		public bool CanParse(string mask)
		{
			try
			{
				DateTime.Now.ToString(mask);
				return true;
			}
			catch 
			{
				return false;
			}
		}

		public virtual string Parse(string mask, WorkingSpace workingSpace)
		{
			DateTime now = DateTime.Now;
			return now.ToString(mask);
		}
	};
}

