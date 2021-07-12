using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SIA.UI.Controls.Automation.Utilities
{
	/// <summary>
	/// Summary description for FileNameGenerator.
	/// </summary>
	public class FileNameGenerator
	{
		public static string GetDisplayName(ArrayList alFiterField ,bool hasDefine,string defineFileName,bool prefix)
		{
			return GetDisplayName( alFiterField , hasDefine, defineFileName,prefix,true);
		}

		public static string GetDisplayName(ArrayList alFiterField ,bool hasDefine,string defineFileName,bool prefix,bool HaveUniqueCode)
		{

			string fileName = "";				

			if (hasDefine && prefix )
			{
				fileName = fileName + defineFileName ;
			}

			for ( int i=0;i<alFiterField.Count;i++)
			{
				if ( fileName != "" )
					fileName = fileName + "_<" +  alFiterField[i].ToString()+">";
				else  fileName = fileName + "<" + alFiterField[i].ToString() + ">";
			}

			if ( hasDefine && !prefix )
			{
				fileName = fileName + "_" + defineFileName ;
			}

			if(HaveUniqueCode)
			{
				if (fileName != "")
					fileName = fileName + "_<unique code>.000";
				else  fileName = "<unique code>.000" ;
			}
			return fileName;
		}

		
		public class OutputFolderFormat
		{
			public bool isByDate            = false;
			public bool isDatePrefix        = true;
			public int changeType           = 1; // 1 by date, 2 by week, 3 by month      
			public DateTime changeTime      = DateTime.Now;
			public string chageDate         = string.Empty;

			public bool isByHeader          = false;
			public ArrayList alHeaderData   = null;

			public OutputFolderFormat()
			{
			}

			public OutputFolderFormat Copy()
			{
				return (OutputFolderFormat) this.MemberwiseClone();
			}
		}

		public static String GetFileNameOutPut(string _source, string KlarfFormat)
		{
			string fileName = "";

			string[] format_split = KlarfFormat.Split("_".ToCharArray());
			
			for (int i = 0; i < format_split.Length; i++)
			{
				if (format_split[i] == "<Source File Name>")
				{
					if (fileName != "") fileName += "_";
					fileName += _source;					
				}
				if (format_split[i] == "<Current Date>")
				{
					if (fileName != "") fileName += "_";
					DateTime currentDate = DateTime.Today;
					fileName += currentDate.Year.ToString() + "_" + currentDate.Month.ToString() + "_" + currentDate.Day.ToString();					
				}
				if (format_split[i] == "<Current Time>")
				{
					if (fileName != "") fileName += "_";
					String time = DateTime.Now.ToShortTimeString();
					time = time.Replace(":", "_");
					time = time.Replace(" ", "_");
					fileName += time;
				}				
			}

			return fileName;
		}
	}
}
