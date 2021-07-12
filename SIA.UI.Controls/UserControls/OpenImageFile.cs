 using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

using SIA.Common.Native;
using SIA.SystemFrameworks.UI;
using SIA.SystemLayer;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Summary description for OpenImageFile.
	/// </summary>
	public class OpenImageFile : CustomizedOpenFileDialog, IProgressCallback
	{
		#region Windows Form Members
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label lblColors;
		private System.Windows.Forms.PictureBox pbxPreview;
		private System.Windows.Forms.Label lblSize;
		private System.Windows.Forms.Label lblSizeValue;
		private System.Windows.Forms.Label lblColorsValue; 
		private System.Windows.Forms.ProgressBar progressBar;

		#endregion

		#region Member Fields

		private object _syncObject = new object();
		private Thread _workerThread = null;
		private System.Windows.Forms.CheckBox chkPreview;
		private string _filePath = string.Empty;

		#endregion

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.pbxPreview = new System.Windows.Forms.PictureBox();
			this.lblSize = new System.Windows.Forms.Label();
			this.lblSizeValue = new System.Windows.Forms.Label();
			this.lblColors = new System.Windows.Forms.Label();
			this.lblColorsValue = new System.Windows.Forms.Label();
			this.chkPreview = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.progressBar);
			this.groupBox1.Controls.Add(this.pbxPreview);
			this.groupBox1.Location = new System.Drawing.Point(0, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(220, 224);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Preview";
			// 
			// progressBar
			// 
			this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar.Location = new System.Drawing.Point(4, 200);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(212, 20);
			this.progressBar.TabIndex = 5;
			// 
			// pbxPreview
			// 
			this.pbxPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pbxPreview.Location = new System.Drawing.Point(3, 16);
			this.pbxPreview.Name = "pbxPreview";
			this.pbxPreview.Size = new System.Drawing.Size(214, 180);
			this.pbxPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pbxPreview.TabIndex = 4;
			this.pbxPreview.TabStop = false;
			this.pbxPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.pbxPreview_Paint);
			// 
			// lblSize
			// 
			this.lblSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblSize.Location = new System.Drawing.Point(4, 236);
			this.lblSize.Name = "lblSize";
			this.lblSize.Size = new System.Drawing.Size(32, 20);
			this.lblSize.TabIndex = 5;
			this.lblSize.Text = "Size:";
			this.lblSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblSizeValue
			// 
			this.lblSizeValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblSizeValue.Location = new System.Drawing.Point(44, 236);
			this.lblSizeValue.Name = "lblSizeValue";
			this.lblSizeValue.Size = new System.Drawing.Size(176, 20);
			this.lblSizeValue.TabIndex = 8;
			this.lblSizeValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblColors
			// 
			this.lblColors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblColors.Location = new System.Drawing.Point(2, 308);
			this.lblColors.Name = "lblColors";
			this.lblColors.Size = new System.Drawing.Size(42, 13);
			this.lblColors.TabIndex = 3;
			this.lblColors.Text = "Colors:";
			// 
			// lblColorsValue
			// 
			this.lblColorsValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblColorsValue.Location = new System.Drawing.Point(50, 308);
			this.lblColorsValue.Name = "lblColorsValue";
			this.lblColorsValue.Size = new System.Drawing.Size(178, 13);
			this.lblColorsValue.TabIndex = 6;
			// 
			// chkPreview
			// 
			this.chkPreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.chkPreview.Location = new System.Drawing.Point(4, 260);
			this.chkPreview.Name = "chkPreview";
			this.chkPreview.Size = new System.Drawing.Size(216, 24);
			this.chkPreview.TabIndex = 9;
			this.chkPreview.Text = "Preview";
			this.chkPreview.CheckedChanged += new System.EventHandler(this.chkPreview_CheckedChanged);
			// 
			// OpenImageFile
			// 
			this.Controls.Add(this.chkPreview);
			this.Controls.Add(this.lblColorsValue);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.lblColors);
			this.Controls.Add(this.lblSize);
			this.Controls.Add(this.lblSizeValue);
			this.Name = "OpenImageFile";
			this.Size = new System.Drawing.Size(228, 296);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		#region Properties

		public string FileName
		{
			get {return this.OpenDialog.FileName;}
			set {this.OpenDialog.FileName = value;}
		}

		public int FilterIndex
		{
			get {return this.OpenDialog.FilterIndex;}
			set {this.OpenDialog.FilterIndex = value;}
		}


		public bool Preview
		{
			get { return chkPreview.Checked; }
			set { chkPreview.Checked = value; }
		}

		
		#endregion
		
		#region Constructor and destructor

		public OpenImageFile(OpenFileDialog dlg) 
			: base(dlg)
		{
			this.InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#endregion

		#region Implementation of IProgressCallback
		/// <summary>
		/// Call this method from the worker thread to initialize
		/// the progress meter.
		/// </summary>
		/// <param name="minimum">The minimum value in the progress range (e.g. 0)</param>
		/// <param name="maximum">The maximum value in the progress range (e.g. 100)</param>
		public void Begin( int minimum, int maximum )
		{
			this.SetRange(minimum, maximum);
			this.progressBar.Value = 0;
		}

		/// <summary>
		/// Call this method from the worker thread to initialize
		/// the progress callback, without setting the range
		/// </summary>
		public void Begin()
		{
			this.Begin(0, 100);
		}

		/// <summary>
		/// Call this method from the worker thread to initialize
		/// the progress callback which is automatically updated after specified milliseconds
		/// </summary>
		/// <param name="millisecond"></param>
		public void Begin(int millisecond)
		{
			this.Begin(0, 100);
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
			this.Begin(minimum, maximum);
		}

		/// <summary>
		/// Call this method from the worker thread to reset the range in the progress callback
		/// </summary>
		/// <param name="minimum">The minimum value in the progress range (e.g. 0)</param>
		/// <param name="maximum">The maximum value in the progress range (e.g. 100)</param>
		/// <remarks>You must have called one of the Begin() methods prior to this call.</remarks>
		public void SetRange( int minimum, int maximum )
		{
			this.progressBar.Minimum = minimum;
			this.progressBar.Maximum = maximum;
		}

		/// <summary>
		/// Call this method form the worker thread to reset autotick millisecond value in the progress callback
		/// </summary>
		/// <param name="milliseconds">
		/// > 0 : enable auto tick
		/// <= 0 : disable auto tick
		/// </param>
		public void SetAutoTick( int milliseconds )
		{
		}

		/// <summary>
		/// Call this method from the worker thread to update the progress text.
		/// </summary>
		/// <param name="text">The progress text to display</param>
		public void SetText( String text )
		{
			
		}

		/// <summary>
		/// Call this method from the worker thread to increase the progress counter by a specified value.
		/// </summary>
		/// <param name="val">The amount by which to increment the progress indicator</param>
		public void Increment( int val )
		{
			while (progressBar.Value < progressBar.Maximum)
				progressBar.Value++;
		}

		/// <summary>
		/// Call this method from the worker thread to step the progress meter to a particular value.
		/// </summary>
		/// <param name="val"></param>
		public void StepTo( int val )
		{
			progressBar.Value = val;
		}

		/// <summary>
		/// Call this method from worker thread to initialize
		/// the progress callback with capable of aborting the current operation
		/// </summary>
		/// <param name="enable"></param>
		public void SetAbort(bool enable)
		{
			
		}

		public bool CanAbort
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// If this property is true, then you should abort work
		/// </summary>
		public bool IsAborting
		{
			get
			{
				return false;
			}
		}

		public event EventHandler Abort
		{
			add 
			{	
			}
			remove
			{	
			}
		}

		/// <summary>
		/// Call this method from the worker thread to finalize the progress meter
		/// </summary>
		public void End()
		{
			
		}

		public void SetUserData(object obj)
		{
		}

		#endregion

		#region Event Handlers

		private void chkPreview_CheckedChanged(object sender, System.EventArgs e)
		{
			if (_workerThread != null)
			{
				Thread thread = _workerThread;
				thread.Abort();
				thread.Join();
			}

			lock (_syncObject)
				_workerThread = null;

			pbxPreview.Visible = chkPreview.Checked;

			this.OnFileNameChanged(this._filePath);
		}

		#endregion

		#region Override routines

		public override void OnFileNameChanged(string fileName)
		{
			base.OnFileNameChanged (fileName);

			// update selected file
			this._filePath = fileName;

			// start worker thread if preview is enabled
			if (chkPreview.Checked)
			{
				if (_workerThread != null)
				{
					Thread thread = _workerThread;
					thread.Abort();
					thread.Join();
				}

				lock (_syncObject)
				{
					_workerThread = new Thread(new ThreadStart(WorkerThread));
					_workerThread.Start();
				}
			}
		}

		public override void OnFolderNameChanged(string folderName)
		{
			base.OnFolderNameChanged (folderName);
		}

		public override void OnClosingDialog()
		{
			base.OnClosingDialog ();

			if (_workerThread != null)
			{
				Thread thread = _workerThread;
				thread.Abort();
				thread.Join();
			}

			lock (_syncObject)
				_workerThread = null;

			pbxPreview.Visible = chkPreview.Checked;
		}

		#endregion

		#region Internal Helpers

		private void WorkerThread()
		{
			try 
			{
				// skip when control is disposed
				if (this.IsDisposed)
					return;

				// hide old image
				pbxPreview.Visible = false;
				if (pbxPreview.Image != null)
				{
					Image oldThumbnail = pbxPreview.Image;
					pbxPreview.Image = null;
					oldThumbnail.Dispose();
				}
				pbxPreview.Image = null;

				string filePath = this._filePath; 
				if (filePath == string.Empty || File.Exists(filePath) == false)
					return;
				
				// show progress bar
				//this.progressBar.Visible = true;

				// initialize progress handler
				CommandProgress.Instance.Callback = this; 

				using (CommonImage image = CommonImage.FromFile(filePath))
				{
					int width = image.Width;
					int height = image.Height;
					int dst_width = pbxPreview.Width;
					int dst_height = pbxPreview.Height;

					float scaleDx = dst_width*1.0F/width;
					float scaleDy = dst_height*1.0F/height;
					float scale = Math.Min(scaleDx, scaleDy);
					dst_width = (int)Math.Floor(width*scale);
					dst_height = (int)Math.Floor(height*scale);

					using (Bitmap bitmap = image.CreateBitmap())
					{
						Image thumbnail = null;
						
						try
						{
							thumbnail = bitmap.GetThumbnailImage(dst_width, dst_height, null, IntPtr.Zero);
							this.pbxPreview.Image = thumbnail;
						}
						catch (ThreadAbortException)
						{
							if (pbxPreview.Image != thumbnail)
							{
								if (thumbnail != null)
									thumbnail.Dispose();
								thumbnail = null;
							}
						}
					}

					this.lblSizeValue.Text = string.Format("{0}x{1}", width, height);
				}
			}
			catch (ThreadAbortException)
			{
			}
			catch (Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{
				// release system callback
                //CommandProgress.SetCallbackHandler(null);
                CommandProgress.Instance.Callback = null;

				// hide progress bar
				//this.progressBar.Visible = false;
				// show preview picture
				pbxPreview.Visible = true;

				lock (_syncObject)
					_workerThread = null;
			}

		}

		#endregion

		private void pbxPreview_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}
	}
}
