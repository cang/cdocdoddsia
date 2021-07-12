using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;

namespace SIA.UI.Components.Printing
{
	public class PrintControllerWithDlgPrintStatus 
        : PrintController
    {
		#region constants

		internal const string PrintControllerWithStatusDialog_DialogTitlePrint = "Printing";
		internal const string PrintControllerWithStatusDialog_NowPrinting = "Page {0} of {1}";
		internal const string PrintControllerWithStatusDialog_Canceling = "Canceling Print...";
		internal const string PrintControllerWithStatusDialog_Cancel = "Cancel";

		#endregion

		#region constructor and destructor

		public PrintControllerWithDlgPrintStatus(PrintController underlyingController) : this(underlyingController, PrintControllerWithStatusDialog_DialogTitlePrint)
        {
        }

        public PrintControllerWithDlgPrintStatus(PrintController underlyingController, string _title)
        {
            this._controller = underlyingController;
            this._title = _title;
        }

		#endregion

		#region methods

		public override Graphics OnStartPage(PrintDocument document, PrintPageEventArgs e)
		{
			base.OnStartPage(document, e);

			if (this._backgroundThread != null)
			{
				this._backgroundThread.UpdateLabel();
			}

			// save this argument for cancelling
			this._printPageEventArgs = e;

			// starts print page
			Graphics graph = this._controller.OnStartPage(document, e);

			if ((this._backgroundThread != null) && this._backgroundThread.canceled)
			{
				e.Cancel = true;
			}

			return graph;
		}

        public override void OnEndPage(PrintDocument document, PrintPageEventArgs e)
        {
            this._controller.OnEndPage(document, e);

            if ((this._backgroundThread != null) && this._backgroundThread.canceled)
            {
                e.Cancel = true;
            }

            this._pageNumber++;
            base.OnEndPage(document, e);
        }

     
        public override void OnStartPrint(PrintDocument document, PrintEventArgs e)
        {
            base.OnStartPrint(document, e);

            this._document = document;
            this._pageNumber = 1;

            if (SystemInformation.UserInteractive)
            {
                this._backgroundThread = new BackgroundThread(this);
            }

            try
            {
                this._controller.OnStartPrint(document, e);
            }
            catch (Exception exp)
            {
                if (this._backgroundThread != null)
                {
                    this._backgroundThread.Stop();
                }
                throw exp;
            }
            finally
            {
                if ((this._backgroundThread != null) && this._backgroundThread.canceled)
                {
                    e.Cancel = true;
                }
            }
        }

		public override void OnEndPrint(PrintDocument document, PrintEventArgs e)
		{
			this._controller.OnEndPrint(document, e);
			if ((this._backgroundThread != null) && this._backgroundThread.canceled)
			{
				e.Cancel = true;
			}

			if (this._backgroundThread != null)
			{
				this._backgroundThread.Stop();
			}

			base.OnEndPrint(document, e);
		}

       
		#endregion

		#region fields

		private BackgroundThread _backgroundThread;
        internal string _title;
        internal PrintDocument _document;
        internal int _pageNumber;
        internal PrintController _controller;
		internal PrintPageEventArgs _printPageEventArgs;

		#endregion
    }
}

