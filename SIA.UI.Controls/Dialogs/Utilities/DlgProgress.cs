using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;

using SIA.UI.Controls;

namespace SIA.UI.Controls.Dialogs
{
    /// <summary>
    /// Summary description for DlgProgress. 
    /// </summary>
    public class DlgProgress : System.Windows.Forms.Form, IProgressCallback
    {
        #region Windows Form members

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ProgressBar _progressBar;
        private System.Windows.Forms.Label _lblDescription;
        private System.Windows.Forms.PictureBox _pictureBox;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        #endregion

        #region member fields

        private const int defaultTimeout = 100;
        private object _syncObject = new object();

        private String _titleRoot = "";
        private bool _requiresClose = true;

        private bool _bAutoTick = false;
        private int _tick = 0;

        private ManualResetEvent _initEvent = new System.Threading.ManualResetEvent(false);
        private ManualResetEvent _stopEvent = new ManualResetEvent(true);
        private bool _isAbort = false;
        private System.Threading.Timer _timer = null;

        private bool _canAbort = false;
        private object _syncAbortEventHandler = new object();
        private event EventHandler _abortEventHandler = null;

        private bool _isInteractiveBusy = false;
        private Point[] _interactivePoints = null;
        #endregion

        #region public delegate

        public delegate void SetTextInvoker(String text);
        public delegate void IncrementInvoker(int val);
        public delegate void StepToInvoker(int val);
        public delegate void RangeInvoker(int minimum, int maximum);
        public delegate void SetAutoTickInvoker(int millisecond);
        public delegate void SetAbortInvoker(bool enable);
        public delegate void AbortInvoker();

        #endregion

        #region public properties

        #endregion

        #region constructor and destructor

        public DlgProgress()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public DlgProgress(bool skipInitEvent)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            if (skipInitEvent)
                _initEvent.Set();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgProgress));
            this.btnCancel = new System.Windows.Forms.Button();
            this._progressBar = new System.Windows.Forms.ProgressBar();
            this._lblDescription = new System.Windows.Forms.Label();
            this._pictureBox = new System.Windows.Forms.PictureBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(153, 60);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // _progressBar
            // 
            this._progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this._progressBar.Location = new System.Drawing.Point(44, 8);
            this._progressBar.Name = "_progressBar";
            this._progressBar.Size = new System.Drawing.Size(332, 16);
            this._progressBar.TabIndex = 7;
            // 
            // _lblDescription
            // 
            this._lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this._lblDescription.Location = new System.Drawing.Point(44, 28);
            this._lblDescription.Name = "_lblDescription";
            this._lblDescription.Size = new System.Drawing.Size(332, 28);
            this._lblDescription.TabIndex = 6;
            this._lblDescription.Text = "Processing...";
            this._lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _pictureBox
            // 
            this._pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("_pictureBox.Image")));
            this._pictureBox.Location = new System.Drawing.Point(4, 14);
            this._pictureBox.Name = "_pictureBox";
            this._pictureBox.Size = new System.Drawing.Size(32, 32);
            this._pictureBox.TabIndex = 9;
            this._pictureBox.TabStop = false;
            // 
            // DlgProgress
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(380, 88);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this._pictureBox);
            this.Controls.Add(this._progressBar);
            this.Controls.Add(this._lblDescription);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgProgress";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Progress Window";
            this.ResumeLayout(false);

        }
        #endregion

        #region override routines

        protected override void OnLoad(EventArgs e)
        {
            if (this.Owner == null)
            {
                this.StartPosition = FormStartPosition.CenterScreen;
                this.TopMost = true;
            }

            base.OnLoad(e);

            // re-layout dialog for abort and nonabortable
            this.btnCancel.Visible = this._canAbort;
            this.btnCancel.Enabled = this._canAbort;

            int size_grid = 4;
            if (!_canAbort)
                this.Height = ((_lblDescription.Bottom / size_grid) + 1) * size_grid;

            // signal initialize event
            this._initEvent.Set();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this._requiresClose = false;
            this.AbortWork();
            base.OnClosing(e);
        }

        public new void Close()
        {
            base.Close();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Point pt = new Point(e.X, e.Y);
                pt = PointToScreen(pt);

                _interactivePoints = new Point[] { pt, pt };
                _isInteractiveBusy = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_isInteractiveBusy && (e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Point pt = new Point(e.X, e.Y);
                pt = PointToScreen(pt);

                int dx = pt.X - _interactivePoints[1].X;
                int dy = pt.Y - _interactivePoints[1].Y;

                Point loc = this.Location;
                loc.X += dx; loc.Y += dy;
                this.Location = loc;

                _interactivePoints[1] = pt;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (_isInteractiveBusy && (e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Point pt = new Point(e.X, e.Y);
                pt = PointToScreen(pt);

                int dx = pt.X - _interactivePoints[1].X;
                int dy = pt.Y - _interactivePoints[1].Y;

                Point loc = this.Location;
                loc.X += dx; loc.Y += dy;
                this.Location = loc;

                _interactivePoints = null;
                _isInteractiveBusy = false;
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            _isInteractiveBusy = false;
        }

        #endregion

        #region Implementation of IProgressCallback
        /// <summary>
        /// Call this method from the worker thread to initialize
        /// the progress meter.
        /// </summary>
        /// <param name="minimum">The minimum value in the progress range (e.g. 0)</param>
        /// <param name="maximum">The maximum value in the progress range (e.g. 100)</param>
        public void Begin(int minimum, int maximum)
        {
            if (!this.IsDisposed)
            {
                if (!this.IsAborting)
                {
                    if (Monitor.TryEnter(_syncObject, defaultTimeout) == true)
                    {
                        try
                        {
                            _initEvent.WaitOne(defaultTimeout, false);
                            if (this.InvokeRequired)
                                Invoke(new RangeInvoker(DoBegin), new object[] { minimum, maximum });
                            else
                                this.DoBegin(minimum, maximum);
                        }
                        finally
                        {
                            Monitor.Exit(_syncObject);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Call this method from the worker thread to initialize
        /// the progress callback, without setting the range
        /// </summary>
        public void Begin()
        {
            if (!this.IsDisposed)
            {
                if (!this.IsAborting)
                {
                    if (Monitor.TryEnter(_syncObject, defaultTimeout) == true)
                    {
                        try
                        {
                            _initEvent.WaitOne(defaultTimeout, false);
                            if (this.InvokeRequired)
                                Invoke(new MethodInvoker(DoBegin));
                            else
                                this.DoBegin();
                        }
                        finally
                        {
                            Monitor.Exit(_syncObject);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Call this method from the worker thread to initialize
        /// the progress callback which is automatically updated after specified milliseconds
        /// </summary>
        /// <param name="millisecond"></param>
        public void Begin(int millisecond)
        {
            if (!this.IsDisposed)
            {
                if (!this.IsAborting)
                {
                    if (Monitor.TryEnter(_syncObject, defaultTimeout) == true)
                    {
                        try
                        {
                            _initEvent.WaitOne(defaultTimeout, false);

                            if (this.InvokeRequired)
                                Invoke(new SetAutoTickInvoker(DoBegin), new object[] { millisecond });
                            else
                                this.DoBegin(millisecond);
                        }
                        finally
                        {
                            Monitor.Exit(_syncObject);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Call this method from worker thread to initialize
        /// the progress callback can be aborted.
        /// </summary>
        /// <param name="allowAbort"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        public void Begin(bool allowAbort, int minimum, int maximum)
        {
            this._canAbort = allowAbort;
            this.Begin(minimum, maximum);
        }

        /// <summary>
        /// Call this method from the worker thread to reset the range in the progress callback
        /// </summary>
        /// <param name="minimum">The minimum value in the progress range (e.g. 0)</param>
        /// <param name="maximum">The maximum value in the progress range (e.g. 100)</param>
        /// <remarks>You must have called one of the Begin() methods prior to this call.</remarks>
        public void SetRange(int minimum, int maximum)
        {
            if (!this.IsDisposed)
            {
                if (!this.IsAborting)
                {
                    if (Monitor.TryEnter(_syncObject, defaultTimeout) == true)
                    {
                        try
                        {
                            _initEvent.WaitOne(defaultTimeout, false);

                            if (this.InvokeRequired)
                                Invoke(new RangeInvoker(DoSetRange), new object[] { minimum, maximum });
                            else
                                this.DoSetRange(minimum, maximum);
                        }
                        finally
                        {
                            Monitor.Exit(_syncObject);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Call this method form the worker thread to reset autotick millisecond value in the progress callback
        /// </summary>
        /// <param name="milliseconds">
        /// > 0 : enable auto tick
        /// <= 0 : disable auto tick
        /// </param>
        public void SetAutoTick(int milliseconds)
        {
#if ENABLE_AUTOTICK
			_initEvent.WaitOne(defaultTimeout, false);

            if (this.InvokeRequired)
				Invoke( new SetAutoTickInvoker(DoSetAutoTick), new object[] {milliseconds});
			else
				this.DoSetAutoTick(milliseconds);
#endif
        }

        /// <summary>
        /// Call this method from the worker thread to update the progress text.
        /// </summary>
        /// <param name="text">The progress text to display</param>
        public void SetText(String text)
        {
            if (!this.IsDisposed)
            {
                if (!this.IsAborting)
                {
                    if (Monitor.TryEnter(_syncObject, defaultTimeout) == true)
                    {
                        try
                        {
                            if (this.InvokeRequired)
                                Invoke(new SetTextInvoker(DoSetText), new object[] { text });
                            else
                                this.DoSetText(text);
                        }
                        finally
                        {
                            Monitor.Exit(_syncObject);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Call this method from the worker thread to increase the progress counter by a specified value.
        /// </summary>
        /// <param name="val">The amount by which to increment the progress indicator</param>
        public void Increment(int val)
        {
            if (!this.IsDisposed)
            {
                if (!this.IsAborting)
                {
                    if (Monitor.TryEnter(_syncObject, defaultTimeout) == true)
                    {
                        try
                        {
                            if (this.InvokeRequired)
                                Invoke(new IncrementInvoker(DoIncrement), new object[] { val });
                            else
                                this.DoIncrement(val);
                        }
                        finally
                        {
                            Monitor.Exit(_syncObject);
                            Monitor.PulseAll(this);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Call this method from the worker thread to step the progress meter to a particular value.
        /// </summary>
        /// <param name="val"></param>
        public void StepTo(int val)
        {
            if (!this.IsDisposed)
            {
                if (!this.IsAborting)
                {
                    if (Monitor.TryEnter(_syncObject, defaultTimeout) == true)
                    {
                        try
                        {
#if INVOKEREQUIRED_CHECKED
                            if (this.InvokeRequired)
                                Invoke(new StepToInvoker(DoStepTo), new object[] { val });
                            else
#endif
                            this.DoStepTo(val);
                        }
                        finally
                        {
                            Monitor.Exit(_syncObject);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Call this method from worker thread to initialize
        /// the progress callback with capable of aborting the current operation
        /// </summary>
        /// <param name="enable"></param>
        public void SetAbort(bool enable)
        {
            if (!this.IsDisposed)
            {
                if (!this.IsAborting)
                {
                    if (Monitor.TryEnter(_syncObject, defaultTimeout) == true)
                    {
                        try
                        {
                            if (this.InvokeRequired)
                                Invoke(new SetAbortInvoker(DoSetAbort), new object[] { enable });
                            else
                                this.DoSetAbort(enable);
                        }
                        finally
                        {
                            Monitor.Exit(_syncObject);
                        }
                    }
                }
            }
        }

        public bool CanAbort
        {
            get
            {
                bool result = false;
                if (Monitor.TryEnter(_syncObject, defaultTimeout))
                {
                    try
                    {
                        result = _canAbort;
                    }
                    finally
                    {
                        Monitor.Exit(_syncObject);
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// If this property is true, then you should abort work
        /// </summary>
        public bool IsAborting
        {
            get
            {
                return _isAbort;
            }
        }

        public event EventHandler Abort
        {
            add
            {
                lock (_syncAbortEventHandler)
                {
                    _abortEventHandler += value;
                }
            }
            remove
            {
                lock (_syncAbortEventHandler)
                {
                    _abortEventHandler -= value;
                }
            }
        }

        /// <summary>
        /// Call this method from the worker thread to finalize the progress meter
        /// </summary>
        public void End()
        {
            if (_requiresClose)
            {
                if (!this.IsDisposed)
                {
                    if (Monitor.TryEnter(_syncObject, Timeout.Infinite) == true)
                    {
                        try
                        {
                            if (this.InvokeRequired)
                                Invoke(new MethodInvoker(DoEnd));
                            else
                                this.DoEnd();
                        }
                        finally
                        {
                            Monitor.Exit(_syncObject);
                        }
                    }
                    else
                    {
                        Trace.WriteLine("DlgProgress::End(): Failed when monitor enter this");
                    }
                }
            }
        }

        public void SetUserData(object obj)
        {
        }
        #endregion

        #region Implementation members invoked on the owner thread
        private void DoSetText(String text)
        {
            _lblDescription.Text = text;
        }

        private void DoIncrement(int val)
        {
            if (_bAutoTick == true)
            {
                if (_progressBar.Value >= _progressBar.Maximum)
                    _progressBar.Value = _progressBar.Minimum;
            }

            _progressBar.Increment(val);
            UpdateStatusText();
        }

        private void DoStepTo(int val)
        {
            _progressBar.Value = val;
            UpdateStatusText();
        }

        private void DoBegin(int minimum, int maximum)
        {
            DoBegin();
            DoSetRange(minimum, maximum);
        }

        private void DoBegin()
        {
            btnCancel.Enabled = true;
            ControlBox = true;
        }

        private void DoBegin(int milliseconds)
        {
            _bAutoTick = true;
            _tick = milliseconds;
            DoBegin();
            DoStartTimer();
        }

        private void DoSetRange(int minimum, int maximum)
        {
            _progressBar.Minimum = minimum;
            _progressBar.Maximum = maximum;
            _progressBar.Value = minimum;
            _titleRoot = Text;
        }

        private void DoSetAutoTick(int milliseconds)
        {
            DoStopTimer();

            if (milliseconds > 0)
            {
                _bAutoTick = true;
                _tick = milliseconds;
                DoStartTimer();
            }
            else
            {
                _bAutoTick = false;
                _tick = 0;
            }
        }

        private void DoEnd()
        {
            if (_bAutoTick)
            {
                DoStopTimer();
                _bAutoTick = false;
                _tick = 0;
            }
            base.Close();
        }

        private void DoSetAbort(bool enable)
        {
            this._canAbort = enable;
        }

        private void DoAbort()
        {
            if (this._abortEventHandler != null)
                this._abortEventHandler(this, EventArgs.Empty);
            this.DoEnd();
        }

        private void DoStartTimer()
        {
            if (_timer != null)
                DoStopTimer();
            _timer = new System.Threading.Timer(new System.Threading.TimerCallback(Time_Tick), null, 0, _tick);
        }

        private void DoStopTimer()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                System.Threading.Thread.Sleep(1000);
                _timer = null;
            }
        }

        private void Time_Tick(object state)
        {
            CommandProgress.Increment(1);
        }
        #endregion

        #region internal helpers
        /// <summary>
        /// Utility function that formats and updates the title bar text
        /// </summary>
        private void UpdateStatusText()
        {
            Text = _titleRoot + String.Format(" - {0}% complete", (_progressBar.Value * 100) / (_progressBar.Maximum - _progressBar.Minimum));
        }

        /// <summary>
        /// Utility function to terminate the thread
        /// </summary>
        private void AbortWork()
        {
            _isAbort = true;
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            // lock user interface
            this.LockUI();
            // update text
            this.SetText("Aborting...");
            // signal abort flags
            this.AbortWork();
        }

        private void LockUI()
        {
            btnCancel.Enabled = false;
        }

        private void UnlockUI()
        {
            btnCancel.Enabled = true;
        }

        #endregion


    }
}
