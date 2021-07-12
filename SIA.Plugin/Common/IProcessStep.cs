using System;
using System.IO;
using System.Reflection;
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace SIA.Plugins.Common
{
    /// <summary>
    /// The IProcessStep represents the process step in the RDE Monitor script.
    /// </summary>
    public interface IProcessStep
	{
        /// <summary>
        /// Gets the unique identifier string value
        /// </summary>
        string ID {get;}

        /// <summary>
        /// Gets name of the step. Normally the full class name is used for this field
        /// </summary>
		string Name {get;}

        /// <summary>
        /// Gets the display name of the step. The display name is used for displaying the process step
        /// in RDE-Monitor interface
        /// </summary>
		string DisplayName {get;}

        /// <summary>
        /// Gets the short description of the step.
        /// </summary>
		string Description {get;}

        /// <summary>
        /// Index of the step within the script
        /// </summary>
        int Index { get; set;}

        /// <summary>
        /// Gets the input validation keys of the step. The input keys are used by the step to make sure
        /// that all the required information of this step is provided properly.
        /// </summary>
        string[] InputKeys { get;}

        /// <summary>
        /// Gets the output keys of the step. The output keys are produced when the step is executed 
        /// and added into RDE-Monitor workspace.
        /// </summary>
		string[] OutputKeys {get;}

        /// <summary>
        /// Gets a boolean value indicates whether the step is removable
        /// </summary>
		bool Removable {get;}

        /// <summary>
        /// Gets a boolean value indicates whether the step is enabled
        /// </summary>
		bool Enabled {get;}

        /// <summary>
        /// Gets a boolean value indicates whether the step has settings
        /// </summary>
		bool HasSettings {get;}

        /// <summary>
        /// Gets the settings of the step. This value can be null of the process step does not contain settings.
        /// </summary>
		IRasterCommandSettings Settings {get; set;}

		/// <summary>
		/// Loads the settings from the specified file
		/// </summary>
		/// <param name="fileName">The location of the file</param>
		/// <returns>The loaded settings.</returns>
		IRasterCommandSettings LoadAutoCommandSettings(string fileName);
		
        /// <summary>
        /// Validates internal data of the step
        /// </summary>
        void Validate();
		
        /// <summary>
        /// Validates the using of step inside the RDE-Monitor script
        /// </summary>
        /// <param name="workspace"></param>
        void Validate(IAutomationWorkspace workspace);

        /// <summary>
        /// Displays the settings window. 
        /// </summary>
        /// <param name="owner">The owner of the settings window</param>
        /// <returns>True if settings is updated, otherwise false</returns>
        bool ShowSettingsDialog(IWin32Window owner);
	}

    /// <summary>
    /// The IProcessStep2 provides functionality for categorizing the process step
    /// </summary>
    public interface IProcessStep2
        : IProcessStep
    {
        string Category { get;}
    }

    /// <summary>
    /// The IProcessStep manager provides declaration for the object which manages all the process steps
    /// </summary>
	public interface IProcessStepManager
	{

	}

    /// <summary>
    /// The IProcessStepFactory class provides declaration of the factory class used for process step creation.
    /// </summary>
	public interface IProcessStepFactory
	{
        /// <summary>
        /// Gets the list of exposed process steps 
        /// </summary>
        /// <returns></returns>
		ArrayList CreateSteps();

        /// <summary>
        /// Gets the process step specified by the ID
        /// </summary>
        /// <param name="id">The id of the process step</param>
        /// <returns>The result process step if succeeded, otherwise null</returns>
		IProcessStep CreateStep(string id);
	}
}
