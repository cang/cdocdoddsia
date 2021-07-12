using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using SIA.SystemLayer;
using SIA.Plugins.Common;
using SIA.UI.Controls.UserControls;
using SIA.UI.Controls.Dialogs;
using SIA.SystemFrameworks.UI;
using SIA.UI.Controls.Helpers;
using SIA.UI.Controls.Commands;
using SIA.UI.Components.Common;
using SIA.Common;
using System.Windows.Forms;
using System.IO;

namespace SIA.UI.Controls
{
    /// <summary>
    /// Provides basic implementation of the document workspace
    /// </summary>
    public class DocumentWorkspace 
        : UserControl, IHistoryWorkspace, IDocWorkspace
    {
        private AppWorkspace _appWorkspace = null;
        private CommonImage _image = null;

        private Dictionary<string, object> _storageBag = null;
        private DocumentView _documentView;
        private HistoryHelper _historyHelper = null;

        public event EventHandler WorkspaceCreated = null;
        public event EventHandler WorkspaceDestroyed = null;
        public event EventHandler DataChanged = null;
        public event EventHandler StorageBagChanged = null;
	
        /// <summary>
        /// Gets the application workspace
        /// </summary>
        public AppWorkspace AppWorkspace
        {
            get { return _appWorkspace; }
        }

        /// <summary>
        /// Gets the document view
        /// </summary>
        public DocumentView DocumentView
        {
            get { return _documentView; }
        }

        /// <summary>
        /// Gets the image (document)
        /// </summary>
        public CommonImage Image
        {
            get { return _image; }
            set
            {
                _image = value;
                OnImageChanged();
            }
        }

        protected virtual void OnImageChanged()
        {
            this.OnUpdateDocumentViewer();

            this.RaiseDataChangedEvent();
        }

        public double MINGRAYSCALE
        {
            get 
            {
                if (this._image != null)
                    return this._image.MinGreyValue;
                return 0;
            }
        }

        public double MAXGRAYSCALE
        {
            get 
            {
                if (this._image != null)
                    return this._image.MaxGreyValue;
                return ushort.MaxValue;
            }
        }

        public double MinCurrentView
        {
            get 
            {
                if (this._image != null)
                    return this._image.MinCurrentView;
                return 0;
            }
            set
            {
                if (this._image != null)
                {
                    this._image.MinCurrentView = value;
                    OnMinCurrentViewChanged();
                }
            }
        }

        protected virtual void OnMinCurrentViewChanged()
        {

        }

        public double MaxCurrentView
        {
            get 
            {
                if (this._image != null)
                    return this._image.MaxCurrentView;
                return ushort.MaxValue;
            }
            set
            {
                if (this._image != null)
                {
                    this._image.MaxCurrentView = value;
                    OnMaxCurrentViewChanged();
                }
            }
        }

        protected virtual void OnMaxCurrentViewChanged()
        {
        }

        public Dictionary<string, object> StorageBag
        {
            get { return this._storageBag; }
        }

        public object this[string key]
        {
            get
            {
                if (this._storageBag == null)
                    return null;
                if (this._storageBag.ContainsKey(key))
                    return this._storageBag[key];
                return null;
            }
            set
            {
                this._storageBag[key] = value;
                OnStorageBagChanged();
            }
        }

        protected virtual void OnStorageBagChanged()
        {
            if (this.StorageBagChanged != null)
                this.StorageBagChanged(this, EventArgs.Empty);
        }

        public HistoryHelper HistoryHelper
        {
            get { return _historyHelper; }
        }

        #region Constructor and destructor

        public DocumentWorkspace()
        {
            this._documentView = this.OnCreateDocumentView();
        }

        public DocumentWorkspace(AppWorkspace appWorkspace)
        {
            this._appWorkspace = appWorkspace;
            this._documentView = this.OnCreateDocumentView();
        }

        protected virtual DocumentView OnCreateDocumentView()
        {
            this._documentView = new DocumentView(this);
            this.SuspendLayout();
            // 
            // _imageViewer
            // 
            this._documentView.AutoDisposeImages = false;
            this._documentView.AutoFitGrayScale = false;
            this._documentView.AutoResetScaleFactor = false;
            this._documentView.AutoResetScrollPosition = false;
            this._documentView.AutoScroll = true;
            this._documentView.BackColor = System.Drawing.SystemColors.ControlDark;
            this._documentView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._documentView.CenterMode = SIA.UI.Components.RasterViewerCenterMode.Both;
            this._documentView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._documentView.DoubleBuffer = true;
            this._documentView.FrameColor = System.Drawing.Color.Black;
            this._documentView.FrameIsPartOfView = true;
            this._documentView.FrameShadowColor = System.Drawing.Color.DimGray;
            this._documentView.FrameShadowIsPartOfView = false;
            this._documentView.Image = null;
            this._documentView.Location = new System.Drawing.Point(0, 0);
            this._documentView.MaxPixelScaleFactor = 0.1F;
            this._documentView.MinPixelScaleFactor = 0.025F;
            this._documentView.Name = "imageViewer";
            this._documentView.ScaleFactor = 1F;
            this._documentView.Size = new System.Drawing.Size(547, 394);
            this._documentView.SizeMode = SIA.UI.Components.RasterViewerSizeMode.Normal;
            this._documentView.TabIndex = 1;
            this._documentView.TabStop = false;
            // 
            // DocumentWorkspace
            // 
            this.Controls.Add(this._documentView);
            this.Name = "DocumentWorkspace";
            this.Size = new System.Drawing.Size(547, 394);
            this.ResumeLayout(false);

            return _documentView;
        }

        #endregion

        #region Methods

        public virtual void CreateWorkspace(string fileName)
        {
            CommonImage image = null;
            IProgressCallback callback = null;

            try
            {
                OnPreCreateWorkspace();

                // initialize progress callback
                callback = BeginProcess("Loading image...", false, ProgressType.Normal);
                callback.Begin(0, 100);

                // load image from file
                using (CommandExecutor cmdExec = new CommandExecutor(this))
                    image = cmdExec.DoCommandLoadImage(fileName);

                // create workspace from loaded image
                this.CreateWorkspace(image);

                OnPostCreateWorkspace();
            }
            catch (Exception)
            {
                if (callback != null)
                {
                    this.AbortProcess(callback);
                    callback = null;
                }

                this.DestroyWorkspace();

                throw;
            }
            finally
            {
                // notify end of process load image
                if (callback != null)
                {
                    EndProcess(callback);
                    callback = null;
                }
            }

        }

        public virtual void CreateWorkspace(FileStream fs)
        {
            CommonImage image = null;
            IProgressCallback callback = null;

            try
            {
                OnPreCreateWorkspace();

                // initialize progress callback
                callback = BeginProcess("Loading image...", false, ProgressType.Normal);
                callback.Begin(0, 100);

                // load image from file
                using (CommandExecutor cmdExec = new CommandExecutor(this))
                    image = cmdExec.DoCommandLoadImage(fs);                

                // create workspace from loaded image
                this.CreateWorkspace(image);

                OnPostCreateWorkspace();
            }
            catch (Exception)
            {
                if (callback != null)
                {
                    this.AbortProcess(callback);
                    callback = null;
                }

                this.DestroyWorkspace();

                throw;
            }
            finally
            {
                // notify end of process load image
                if (callback != null)
                {
                    EndProcess(callback);
                    callback = null;
                }
            }

        }
        public virtual void CreateWorkspace(MemoryStream fs)
        {
            CommonImage image = null;
            IProgressCallback callback = null;

            try
            {
                OnPreCreateWorkspace();

                // initialize progress callback
                callback = BeginProcess("Loading image...", false, ProgressType.Normal);
                callback.Begin(0, 100);

                // load image from file
                using (CommandExecutor cmdExec = new CommandExecutor(this))
                    image = cmdExec.DoCommandLoadImage(fs);

                // create workspace from loaded image
                this.CreateWorkspace(image);

                OnPostCreateWorkspace();
            }
            catch (Exception)
            {
                if (callback != null)
                {
                    this.AbortProcess(callback);
                    callback = null;
                }

                this.DestroyWorkspace();

                throw;
            }
            finally
            {
                // notify end of process load image
                if (callback != null)
                {
                    EndProcess(callback);
                    callback = null;
                }
            }

        }

        public virtual void CreateWorkspace(CommonImage image)
        {
            // ininitialize image data
            this._image = image;

            // initialize document view
            this.OnInitializeDocumentView();
        }

        public virtual void SaveWorkspace(string fileName, eImageFormat format)
        {
            // check if data is already load and in valid state
            if (this._image == null)
                throw new InvalidOperationException("Image was not initialized");

            IProgressCallback callback = null;

            try
            {
                // notify begin process save image
                callback = BeginProcess("Save image", false, ProgressType.Normal);
                callback.Begin(0, 100);

                // executes save image command
                using (CommandExecutor cmdExec = new CommandExecutor(this))
                    cmdExec.DoCommandSaveImage(this._image, fileName, format);

                // update modified flags
                this._image.Modified = false;
            }
            catch (Exception)
            {
                if (callback != null)
                {
                    this.AbortProcess(callback);
                    callback = null;
                }

                throw;
            }
            finally
            {
                // notify end of process load image
                if (callback != null)
                {
                    EndProcess(callback);
                    callback = null;
                }
            }
        }

        public virtual void DestroyWorkspace()
        {
            OnPreDestroyWorkspace();

            // uninitialize document view
            this.OnUninitializeDocumentView();

            // reset image
            this._image = null;

            OnPostDestroyWorkspace();
        }

        public void RestoreFromHistory(int stepIndex)
        {
            using (CommandExecutor cmdExec = new CommandExecutor(this))
                cmdExec.DoCommandRestoreByStepIndex(this._historyHelper, stepIndex);
        }

        #endregion

        #region Virtual Methods

        protected virtual void OnPreCreateWorkspace()
        {
            // initialize history helper
            this._historyHelper = new HistoryHelper(this);

            // initialize storage bag
            this.OnInitializeStorageBag();
        }

        protected virtual void OnPostCreateWorkspace()
        {
            this.RaiseWorkspaceCreatedEvent();
        }
                
        protected void OnPreDestroyWorkspace()
        {
            
        }

        protected void OnPostDestroyWorkspace()
        {
            // uninitialize storage bag
            this.OnUninitializeStorageBag();

            // uninitialize history helper
            if (this._historyHelper != null)
                this._historyHelper.Dispose();
            this._historyHelper = null;

            // raise event
            this.RaiseWorkspaceDestroyedEvent();
        }

        protected virtual void OnInitializeStorageBag()
        {
            this._storageBag = new Dictionary<string, object>();
        }

        protected virtual void OnUninitializeStorageBag()
        {
            if (this._storageBag != null)
                this._storageBag.Clear();
            this._storageBag = null;
        }

        protected virtual void OnInitializeDocumentView()
        {
            this._documentView.Image = this._image;
            //this._documentView.Mask = null;

            // initialize pseudo color
            this._documentView.PseudoColor = PseudoColors.GrayScale;

            // initialize zooming factor
            if (Math.Max(_image.Width, _image.Height) < Math.Min(this.Width, this.Height))
                this._documentView.ZoomActualSize();
            else
                this._documentView.ZoomToFit();
        }

        protected virtual void OnUninitializeDocumentView()
        {
            this._documentView.Image = null;
            //this._documentView.Mask = null;
        }

        #endregion

        #region Progress handler

        private bool _updateData = true;
        private bool _redoable = true;
        private string _description = string.Empty;
        
        public virtual void BeginProcess(String description)
        {
            this.BeginProcess(description, true, true);
        }

        public virtual void BeginProcess(String description, bool updateData)
        {
            this.BeginProcess(description, updateData, true);
        }

        public virtual void BeginProcess(String description, bool updateData, bool redoable)
        {
            this._description = description;
            this._updateData = updateData;
            this._redoable = redoable;

            this.Invalidate(true);
        }

        public virtual void AbortProcess()
        {
            if (this._image == null)
                return;

            _description = _image.Description;
            _updateData = _image.Modified;

            // reset redoable
            _redoable = true;
            
            // update image viewer
            this._documentView.Image = this._image;
            this._documentView.Invalidate(true);
        }

        public virtual void EndProcess()
        {
            if (_image != null)
            {
                _image.Description = _description;
                _image.Modified = _updateData;

                if (this._historyHelper != null && _redoable)
                    this._historyHelper.AddToHistory(this._image);
            }

            // reset redoable
            _redoable = true;

            // raise event data changed
            this.OnDataChanged();

            // update image viewer
            this.OnUpdateDocumentViewer();
        }

        public virtual IProgressCallback BeginProcess(String description, ProgressType type)
        {
            return BeginProcess(description, true, type);
        }

        public virtual IProgressCallback BeginProcess(String description, bool updateData, ProgressType type)
        {
            if (type == ProgressType.None)
            {
                this.BeginProcess(description, updateData);
                return null;
            }
            else
            {
                this.BeginProcess(description, updateData);

                IProgressCallback callback = this.ShowProgressBar(true);

                if (callback != null)
                {
                    if (type == ProgressType.Normal)
                    {
                        callback.SetRange(0, 100);
                        callback.SetText(description);
                    }
                    else if (type == ProgressType.AutoTick)
                    {
                        callback.SetRange(0, 100);
                        callback.SetText(description);
                        callback.SetAutoTick(100);
                    }
                }

                return callback;
            }
        }

        public virtual void AbortProcess(IProgressCallback callback)
        {
            if (callback != null)
                this.ShowProgressBar(false);

            this.AbortProcess();
        }

        public virtual void EndProcess(IProgressCallback callback)
        {
            if (callback != null)
                this.ShowProgressBar(false);
            this.EndProcess();
        }

        public virtual void HandleGenericException(Exception innerException, ref IProgressCallback callback)
        {
            this.HandleGenericException(null, innerException, ref callback);
        }

        public virtual void HandleGenericException(string message, Exception innerException, ref IProgressCallback callback)
        {
            if (innerException is OutOfMemoryException)
            {
                MessageBoxEx.Error("Not enough memory");
            }
            else if (innerException is CommandAbortedException)
            {
                // do nothing here
            }
            else
            {
                if (message == null || message.Length <= 0)
                    MessageBoxEx.Error(innerException.Message);
                else
                    MessageBoxEx.Error(message);
            }

            if (callback != null)
            {
                this.AbortProcess(callback);
                callback = null;
            }
            else
            {
                this.AbortProcess();
            }
        }

        #endregion

        #region Progress Bar Helpers

        private DlgProgress _dlgProgress = null;

        private IProgressCallback ShowProgressBar(bool bShow)
        {
            if (bShow == true)
            {
                // initialize thread progress bar
                _dlgProgress = new DlgProgress(true);
                if (this.ParentForm != null)
                    _dlgProgress.Owner = this.ParentForm;
            }
            else if (_dlgProgress != null)
            {
                if (_dlgProgress.Created)
                    _dlgProgress.End();
                if (_dlgProgress != null)
                    _dlgProgress.Dispose();
                _dlgProgress = null;
            }
            return _dlgProgress;
        }

        private void ProgressBarWorkerThread(System.Object state)
        {
            DlgProgress dlg = null;

            try
            {
                dlg = (DlgProgress)state;
                if (dlg != null)
                {
                    dlg.TopMost = false;
                    dlg.ShowDialog();
                }
            }
            catch (System.Exception exp)
            {
                Trace.WriteLine(exp);
            }
        }

        #endregion

        protected virtual void OnDataChanged()
        {
            this.RaiseDataChangedEvent();
        }
                
        protected virtual void OnUpdateDocumentViewer()
        {
            this._documentView.Image = this._image;
            this._documentView.Invalidate(true);
        }

        private void RaiseWorkspaceCreatedEvent()
        {
            if (this.WorkspaceCreated != null)
                this.WorkspaceCreated(this, EventArgs.Empty);
        }

        private void RaiseWorkspaceDestroyedEvent()
        {
            if (this.WorkspaceDestroyed != null)
                this.WorkspaceDestroyed(this, EventArgs.Empty);
        }

        private void RaiseDataChangedEvent()
        {
            if (this.DataChanged != null)
                this.DataChanged(this, EventArgs.Empty);
        }
    }
}
