using System;
using System.Collections.Generic;
using System.Text;
using SIA.SystemFrameworks.UI;

namespace SIA.Plugins.Common
{
    /// <summary>
    /// Represents type of progress
    /// </summary>
    public enum ProgressType
    {
        None = 0,
        Normal = 1,
        AutoTick = 2
    };

    /// <summary>
    /// The IProgressHandler provides functions for creation, destruction and aborting the progress
    /// </summary>
    public interface IProgressHandler
    {
        /// <summary>
        /// Starts a progress with the specified description
        /// </summary>
        /// <param name="description">The short description of the progress to start</param>
        void BeginProcess(String description);

        /// <summary>
        /// Starts a progress with the specified description with or without updating the data
        /// </summary>
        /// <param name="description">The short description of the progress to start</param>
        /// <param name="updateData">Boolean value indicates whether data is updated</param>
        /// <param name="redoable">Boolean value indicates whether this progress is undoable</param>
        void BeginProcess(String description, bool updateData, bool redoable);

        /// <summary>
        /// Start a progress with the specified description and type and return the progress callback
        /// </summary>
        /// <param name="description">The short description of the progress to start</param>
        /// <param name="type"></param>
        /// <returns></returns>
        IProgressCallback BeginProcess(String description, ProgressType type);
        /// <summary>
        /// Starts a progress with the specified description with or without updating the data
        /// </summary>
        /// <param name="description">The short description of the progress to start</param>
        /// <param name="updateData">Boolean value indicates whether data is updated</param>
        /// <param name="type">Type of the progress</param>
        /// <param name="ModifiedData">Boolean value indicates whether data is updated</param>
        IProgressCallback BeginProcess(String description, bool updateData, ProgressType type);
        
        /// <summary>
        /// Aborts the current progress
        /// </summary>
        void AbortProcess();

        /// <summary>
        /// Aborts the current progress by the specified progress callback
        /// </summary>
        /// <param name="callback">The progress callback to invoke</param>
        void AbortProcess(IProgressCallback callback);
        
        /// <summary>
        /// End the current progress
        /// </summary>
        void EndProcess();

        /// <summary>
        /// Ends the current progress by the specified progress callback
        /// </summary>
        /// <param name="callback"></param>
        void EndProcess(IProgressCallback callback);

        /// <summary>
        /// Handles common exception throwed while progress is executing
        /// </summary>
        /// <param name="innerException">The internal exception is throwed</param>
        /// <param name="callback">The callback progress is in used</param>
        void HandleGenericException(Exception innerException, ref IProgressCallback callback);

        /// <summary>
        /// Handles common exception throwed while progress is executing
        /// </summary>
        /// <param name="message">The message is throwed</param>
        /// <param name="innerException">The internal exception is throwed</param>
        /// <param name="callback">The callback progress is in used</param>
        void HandleGenericException(string message, Exception innerException, ref IProgressCallback callback);
    }

}
