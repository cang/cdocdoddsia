using System;

namespace SIA.Automation.Launcher
{
	/// <summary>
	/// Summary description for Constants.
	/// </summary>
	public enum ErrorCodes : int
	{
		OK = 0,
		ProcessWasCancelled = 1,
		ScriptWasNotSpecified = 2,
		InvalidArguments = 3,
		FailedToCreateLogFile = 4,
		GenericException = 5,
		FailedToDeserializeScript = 6,
		FailedToExecuteScript = 7,			    
		UndeterminedExitCode = 8,
		UnhandledException = 9,
		StatusQueueIsNotEmpty = 10,

		MaxErrorCode,

		OutOfKlarfFileSizeLimit = -4,
		OutOfNumberOfDefectsLimit = -5
	}
}
