using System;
using System.Data;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

using SIA.Common;
using SIA.Common.KlarfExport;
using SIA.Common.Imaging;
using SIA.Common.Imaging.Filters;
using SIA.Common.PatternRecognition;
using SIA.Common.GoldenImageApproach;

using SIA.IPEngine;

using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;

using SIA.UI.Components;
using SIA.UI.Components.Common;
using SIA.UI.Components.Renders;

using SIA.UI.Controls;
using SIA.UI.Controls.Dialogs;
using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Helpers;
using SIA.UI.Controls.Helpers.VisualAnalysis;
using SIA.UI.Controls.UserControls;
using SIA.UI.Controls.Automation.Commands;

using SIA.SystemLayer;
using SiGlaz.Common;
using SIA.SystemLayer.ImageProcessing;
using SiGlaz.Common.ImageAlignment;
using SIA.Algorithms.ReferenceFile;

namespace SIA.UI.Controls.Commands
{
    /// <summary>
    /// Summary description for CommandExecutor.
    /// </summary>
    public partial class CommandExecutor : IDisposable
    {
        #region member fields

        private IWin32Window _workSpace = null;
        private DlgProgress _callback = null;

        #endregion

        #region properties

        public IWin32Window Owner
        {
            get { return _workSpace; }
        }

        public IProgressCallback StatusCallback
        {
            get { return _callback; }
        }


        #endregion

        #region constructor and destructor

        public CommandExecutor(IWin32Window owner)
        {
            this._workSpace = owner;
        }


        ~CommandExecutor()
        {
            this.Dispose(false);
        }

        #endregion

        #region raster command helpers

        public virtual RasterCommand OnBeginCommand(Type type, IProgressCallback callback)
        {
            RasterCommand command = null;

            try
            {
                // initialize command and its arguments
                command = (RasterCommand)Activator.CreateInstance(type, new object[] { callback });
                if (callback != null)
                    callback.SetAbort(command.CanAbort);

                // regist for command message handlers
                command.ThreadException += new RasterCommandExceptionHandler(RasterCommand_ThreadException);
                command.ThreadBegin += new EventHandler(RasterCommand_ThreadBegin);
                command.ThreadEnd += new EventHandler(RasterCommand_ThreadEnd);
            }
            finally
            {
            }

            return command;
        }

        public virtual void OnEndCommand(ref RasterCommand command)
        {
            if (command != null)
            {
                // unregist command message handlers
                command.ThreadException -= new RasterCommandExceptionHandler(RasterCommand_ThreadException);
                command.ThreadBegin -= new EventHandler(RasterCommand_ThreadBegin);
                command.ThreadEnd -= new EventHandler(RasterCommand_ThreadEnd);

                command.Dispose();
            }
            command = null;
        }

        protected virtual void CheckCommandResult(RasterCommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            switch (command.Result)
            {
                case RasterCommandResult.Aborted:
                    throw new CommandAbortedException(String.Format("Command {0} was aborted", command.GetType().Name));

                case RasterCommandResult.Failed:
                    {
                        if (command.Exception != null)
                        {
                            if (command.Exception is OutOfMemoryException ||
                                command.Exception is CommandAbortedException)
                                throw command.Exception;
                            else
                                throw new System.ApplicationException("An error occurred when execute command", command.Exception);
                        }
                        else if (RasterCommandResult.Failed == command.Result)
                            throw new ApplicationException("Unknown exception");

                        break;
                    }
                default:
                    break;
            }
        }

        private void RasterCommand_ThreadEnd(object sender, EventArgs e)
        {
            this.StopStatusCallback();
        }

        private void RasterCommand_ThreadBegin(object sender, EventArgs e)
        {

        }

        private void RasterCommand_ThreadException(object sender, RasterCommandExceptionArgs exp)
        {
            // Notes: Trace log has been dump OnThreadException
            //Trace.WriteLine("Thread: " + Thread.CurrentThread.Name);
            //Trace.WriteLine("Exeption: " + exp.ToString());
        }

        #endregion

        #region status callback helpers

        protected virtual void InitializeStatusCallback()
        {
            // initialize progress callback
            if (_callback != null)
                this.UninitializeStatusCallback();
            _callback = new DlgProgress();
        }

        protected virtual void UninitializeStatusCallback()
        {
            if (_callback != null)
                _callback.Dispose();
            _callback = null;
        }

        protected virtual void StartStatusCallback()
        {
            if (this._callback == null)
                throw new System.InvalidOperationException();

            // initialize system callback            
            //CommandProgress.SetCallbackHandler(_callback);            

            lock (SIA.SystemFrameworks.UI.CommandProgress.Instance)
                SIA.SystemFrameworks.UI.CommandProgress.Instance.Callback = _callback;

            // display progress bar
            _callback.ShowDialog(this.Owner);
        }

        protected virtual void StopStatusCallback()
        {
            if (this._callback == null)
                throw new System.InvalidOperationException();

            // release system callback
            //CommandProgress.SetCallbackHandler(null);

            lock (SIA.SystemFrameworks.UI.CommandProgress.Instance)
                SIA.SystemFrameworks.UI.CommandProgress.Instance.Callback = null;

            if (this._callback.IsHandleCreated)
            {
                if (_callback.Created)
                    _callback.End();
            }
        }


        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }

            // release references
            this._workSpace = null;
            this._callback = null;
        }

        #endregion

        #region command handler

        public object[] DoCommand(Type type, params object[] args)
        {
            RasterCommand command = null;
            object[] result = null;

            try
            {
                // initialize progress callback
                this.InitializeStatusCallback();

                // initialize command and its arguments
                command = this.OnBeginCommand(type, this.StatusCallback);

                // execute command
                command.Run(args);

                // display progress bar and wait for processing finish
                this.StartStatusCallback();

                // check processing result
                this.CheckCommandResult(command);

                // retrieve output data
                result = command.GetOutput();
            }
            catch
            {
                throw;
            }
            finally
            {
                // uninitialize status callback
                this.UninitializeStatusCallback();

                if (command != null)
                {
                    // uninitialize raster command
                    this.OnEndCommand(ref command);
                }

                // refresh workspace
                if (_workSpace != null && _workSpace is ImageWorkspace)
                {
                    ImageViewer viewer = ((ImageWorkspace)_workSpace).ImageViewer;
                    if (viewer != null && viewer.RasterImageRender != null)
                        viewer.RasterImageRender.IsDirty = true;
                    ((Control)_workSpace).Invalidate(true);
                }
            }

            return result;
        }        

        public void DoCommandUndo(HistoryHelper helper)
        {
            this.DoCommand(typeof(HistoryUndoCommand), helper);
        }

        public void DoCommandRedo(HistoryHelper helper)
        {
            this.DoCommand(typeof(HistoryRedoCommand), helper);
        }

        public void DoCommandRestoreByStepIndex(HistoryHelper helper, int stepIndex)
        {
            this.DoCommand(typeof(HistoryRestoreByStepCommand), helper, stepIndex);
        }                                
        #endregion
    }
}
