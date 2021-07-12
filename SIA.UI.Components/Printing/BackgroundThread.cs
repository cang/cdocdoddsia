using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;

using SIA.UI.Components.Dialogs;

namespace SIA.UI.Components.Printing
{
	internal class BackgroundThread
	{
		internal BackgroundThread(PrintControllerWithDlgPrintStatus parent)
		{
			this.canceled = false;
			this.alreadyStopped = false;
			this.parent = parent;
			this.thread = new Thread(new ThreadStart(this.Run));
			this.thread.SetApartmentState(ApartmentState.STA);
			this.thread.Start();
		}

		private void Run()
		{
			BackgroundThread thread;
			try
			{
				lock ((thread = this))
				{
					if (this.alreadyStopped)
					{
						return;
					}

					this.dialog = new DlgPrintStatus(this, this.parent._title);
					this.ThreadUnsafeUpdateLabel();
					this.dialog.Visible = true;
				}

				if (!this.alreadyStopped)
				{
					Application.Run(this.dialog);										
				}
			}
			finally
			{
				lock ((thread = this))
				{
					if (this.dialog != null)
					{
						this.dialog.Dispose();
						this.dialog = null;
					}
				}
			}
		}

		
		internal void Stop()
		{
			lock (this)
			{
				if ((this.dialog != null) && this.dialog.IsHandleCreated)
				{
					this.dialog.BeginInvoke(new MethodInvoker(this.dialog.Close));
				}
				else
				{
					this.alreadyStopped = true;
				}
			}
		}

		internal void Cancel()
		{
			lock (this)
			{
				if (this.parent != null && this.parent._printPageEventArgs != null)
				{
					this.parent._printPageEventArgs.Cancel = true;
				}
			}
		}

		private void ThreadUnsafeUpdateLabel()
		{
			this.dialog.lblStatus.Text = String.Format(PrintControllerWithDlgPrintStatus.PrintControllerWithStatusDialog_NowPrinting, this.parent._pageNumber, this.parent._document.DocumentName);
		}

		internal void UpdateLabel()
		{
			if ((this.dialog != null) && this.dialog.IsHandleCreated)
			{
				this.dialog.BeginInvoke(new MethodInvoker(this.ThreadUnsafeUpdateLabel));
			}
		}


		private bool alreadyStopped;
		internal bool canceled;
		private DlgPrintStatus dialog;
		private PrintControllerWithDlgPrintStatus parent;
		private Thread thread;
	}
}
