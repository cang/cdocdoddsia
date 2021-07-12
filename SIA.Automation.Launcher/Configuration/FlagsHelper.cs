using System;

namespace SIA.Automation.Launcher.Configuration
{
	/// <summary>
	/// Summary description for FlagsHelper.
	/// </summary>
	public class FlagsHelper
	{
		/// <summary>
		/// Checks a bitmask to see if a particular flag is set on or off
		/// </summary>
		/// <param name="value">The bitmask to check</param>
		/// <param name="flag">The flag whose state is in question</param>
		/// <returns></returns>
		public static bool IsFlagSet(int value, int flag) 
		{
			return (bool)((value & flag) == flag);
		}

		/// <summary>
		/// Enables a mask
		/// </summary>
		/// <param name="value"></param>
		/// <param name="flag"></param>
		public static void Enable(int value, int flag)
		{
			value |= flag;
		}

		/// <summary>
		/// Disables a mask
		/// </summary>
		/// <param name="value"></param>
		/// <param name="flag"></param>
		public static void Disable(int value, int flag)
		{
			if (!FlagsHelper.IsFlagSet(value, flag))
				value ^= flag;
		}
	}
}
