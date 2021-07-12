using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;

namespace SIA.UI.Components.Printing
{
    /// <summary>
    /// Controls how a document is printed with displaying
    /// </summary>
	public class PrintControllerWithCallbackStatus 
        : PrintController	
	{
		#region constants

		internal const string PrintControllerWithCallbackStatus_DialogTitlePrint = "Printing";
		internal const string PrintControllerWithCallbackStatus_NowPrinting = "Page {0} of {1}";
		internal const string PrintControllerWithCallbackStatus_Canceling = "Canceling Print...";
		internal const string PrintControllerWithCallbackStatus_Cancel = "Cancel";

		#endregion

		#region fields

		internal string _title;
		internal PrintDocument _document;
		internal int _pageNumber;
		internal PrintController _controller;
		internal PrintPageEventArgs _printPageEventArgs;

		#endregion

		#region constructor and destructor

		public PrintControllerWithCallbackStatus(PrintController underlyingController) 
            : this(underlyingController, PrintControllerWithCallbackStatus.PrintControllerWithCallbackStatus_DialogTitlePrint)
		{
		}

		public PrintControllerWithCallbackStatus(PrintController underlyingController, string dialogTitle)
		{
			this._controller = underlyingController;
			this._title = dialogTitle;
		}

		#endregion

		#region override routines

		public override Graphics OnStartPage(PrintDocument document, PrintPageEventArgs e)
		{
			base.OnStartPage(document, e);

			// save this argument for cancelling
			this._printPageEventArgs = e;

			// starts print page
			Graphics graph = this._controller.OnStartPage(document, e);

			return graph;
		}

		public override void OnEndPage(PrintDocument document, PrintPageEventArgs e)
		{
			this._controller.OnEndPage(document, e);
			this._pageNumber++;
			base.OnEndPage(document, e);
		}

     
		public override void OnStartPrint(PrintDocument document, PrintEventArgs e)
		{
			base.OnStartPrint(document, e);

			this._document = document;
            this._pageNumber = 1;
            this._controller.OnStartPrint(document, e);
		}

		public override void OnEndPrint(PrintDocument document, PrintEventArgs e)
		{
			this._controller.OnEndPrint(document, e);			
			base.OnEndPrint(document, e);
		}

		#endregion

	}
}
