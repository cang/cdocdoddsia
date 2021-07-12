using System;
using System.Collections.Generic;
using System.Text;
using SIA.SystemFrameworks.UI;

#if USING_IPL

using SIA.Common.IPLFacade;

namespace SIA.SystemLayer
{
    /// <summary>
    /// Wrapper class for compatible with IPL library and CommandProgress callback interface
    /// </summary>
    public class IplProgressCallback 
        : IIplProgressCallback
    {
        /// <summary>
        /// Singleton instance 
        /// </summary>
        public static readonly IplProgressCallback Instance = null;

        static IplProgressCallback()
        {
            Instance = new IplProgressCallback();
        }

        internal IplProgressCallback()
        {
        }

        #region IIplProgressCallback Members

        /// <summary>
        /// Get a value indicates whether the abort signal is triggerred.
        /// </summary>
        public bool IsAbort
        {
            get
            {
                return CommandProgress.IsAborting;
            }
        }

        /// <summary>
        /// Set the description for the current processing
        /// </summary>
        /// <param name="text"></param>
        void IIplProgressCallback.SetText(string text)
        {
            CommandProgress.SetText(text);
        }

        /// <summary>
        /// Set the range for the current processing
        /// </summary>
        /// <param name="min">Minimum value of the current processing</param>
        /// <param name="max">Maximum value of the current processing</param>
        void IIplProgressCallback.SetRange(int min, int max)
        {
            CommandProgress.SetRange(min, max);
        }

        /// <summary>
        /// Set the current process value to a specified value between the current processing data range
        /// </summary>
        /// <param name="value">Value to step</param>
        void IIplProgressCallback.StepTo(int value)
        {
            CommandProgress.StepTo(value);
        }

        #endregion
    }
}

#endif