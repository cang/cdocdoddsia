using System;
using System.Collections;
using System.Data;

namespace SIA.Plugins.Common
{
	/// <summary>
	/// The IScript interface provides basic declaration of the RDE-Monitor script.
    /// The IScript interface provides functionality for managing the script includes process steps.
	/// </summary>
	public interface IScript
	{
        /// <summary>
        /// Gets name of the script
        /// </summary>
		string Name {get;}

        /// <summary>
        /// Gets short description of the script
        /// </summary>
		string Description {get;}

        /// <summary>
        /// Gets version of the script
        /// </summary>
		int Version {get;}
		
        /// <summary>
        /// Gets number of the process steps contained by the script
        /// </summary>
		int NumProcessSteps {get;}

        /// <summary>
        /// Gets list of the process steps contained by the script
        /// </summary>
        /// <returns></returns>
		IProcessStep[] GetProcessSteps();
	}

    /// <summary>
    /// The IExecutionScript provide the functionality for a script in process.
    /// </summary>
	public interface IExecutionScript
	{
        /// <summary>
        /// Gets the zero-based index value of the current process step.
        /// </summary>
		int CurrentStepIndex {get; set;}
	}
}
