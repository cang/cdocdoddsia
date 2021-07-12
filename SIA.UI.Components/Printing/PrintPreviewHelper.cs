using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Collections;

namespace SIA.UI.Components.Printing
{
	/// <summary>
	/// Provides functionality for print prewview
	/// </summary>
	public class PrintPreviewHelper : IDisposable
	{
		#region events

		public event EventHandler PrintPreviewMode_Started = null;
		public event EventHandler PrintPreviewMode_Ended = null;

		public event EventHandler PrintButton_Click = null;

		#endregion

		#region fields

		private bool _isPreviewMode = false;
		private ArrayList _saveControls = null;		
		private Form _ownerForm = null;
		private ucPrintPreview _printPreviewCtrl = null;
		private PrintDocument _printDocument = null;

		#endregion

		#region properties

		public Form Owner
		{
			get {return _ownerForm;}
		}

		public bool IsPreviewMode
		{
			get {return _isPreviewMode;}
		}

		public PrintPreviewControlEx PrintPreviewControl
		{
			get {return _printPreviewCtrl != null ? _printPreviewCtrl.PrintPreviewControl : null;}
		}

		#endregion

		#region constructor and destructor

		public PrintPreviewHelper(Form owner)
		{
			if (owner == null)
				throw new ArgumentNullException("owner");

			this._ownerForm = owner;
		}

		~PrintPreviewHelper()
		{
			this.Dispose(false);
		}

		#endregion

		#region public methods

		public void StartPrintPreview(PrintDocument document, bool autoCalculatePageInfo)
		{
			if (this._isPreviewMode)
				return ;

			if (document == null)
				throw new ArgumentNullException("PrintDocument");

			try
			{
				// enable print preview mode flags
				this._isPreviewMode = true;

				// save and reset current layout
				this.SaveAndResetLayout();

				// initialize print preview user control
				this.InitializePrintPreviewControl(document, autoCalculatePageInfo);

				// raise event print preview mode start
				if (this.PrintPreviewMode_Started != null)
					this.PrintPreviewMode_Started(this, EventArgs.Empty);
			}
			catch
			{
				// uninitialize print preview user control
				this.UninitializePrintPreviewControl();
			
				// restore saved layout
				this.RestoreLayout();

				// restore print preview mode
				this._isPreviewMode = false;

				// re-throw exception
				throw;
			}
			finally
			{

			}

		}

		public void EndPrintPreview()
		{
			if (!this._isPreviewMode)
				return ;

			try
			{
				// uninitialize print preview user control
				this.UninitializePrintPreviewControl();
			
				// restore saved layout
				this.RestoreLayout();

				// raise event print preview mode end
				if (this.PrintPreviewMode_Ended != null)
					this.PrintPreviewMode_Ended(this, EventArgs.Empty);
			}
			catch
			{
				throw;
			}
			finally
			{
				// reset preview mode flags
				this._isPreviewMode = false;
			}
		}


		#endregion

		#region internal helpers

		private void SaveAndResetLayout()
		{
			if (this._ownerForm == null)
				throw new ArgumentNullException("_ownerForm");

			this.Owner.SuspendLayout();

			// init save buffer
			_saveControls = new ArrayList();	
			
			// save child controls
			while (this.Owner.Controls.Count > 0)
			{
				_saveControls.Add(this.Owner.Controls[0]);
				this.Owner.Controls.RemoveAt(0);
			}

			// save main menu bar
			if (this.Owner.Menu != null)
			{
				_saveControls.Add(this.Owner.Menu);
				this.Owner.Menu = null;
			}

			this.Owner.ResumeLayout(false);
		}

		private void RestoreLayout()
		{
			if (this._ownerForm == null)
				throw new ArgumentNullException("_ownerForm");

			this.Owner.SuspendLayout();
			
			if (_saveControls != null)
			{
				foreach(Object ctrl in _saveControls)
				{
					if (ctrl is Control) // restore child controls					
						this.Owner.Controls.Add((Control)ctrl);
					else if (ctrl is MainMenu) // restore main menu
						this.Owner.Menu = (MainMenu)ctrl;
				}

				_saveControls.Clear();
			}

			this.Owner.ResumeLayout(true);
		}

		public void InitializePrintPreviewControl(PrintDocument document, bool autoCalculatePageInfo)
		{
			if (this._ownerForm == null)
				throw new ArgumentNullException("_ownerForm");

			this.Owner.SuspendLayout();
			
			this._printPreviewCtrl = new ucPrintPreview();
			this._printPreviewCtrl.Document = document;
			this._printPreviewCtrl.Dock = DockStyle.Fill;

			this._printPreviewCtrl.CloseButtonClick += new EventHandler(PrintPreviewCtrl_CloseButtonClick);
			this._printPreviewCtrl.Print += new EventHandler(PrintPreviewCtrl_Print);
			
			// enable/disable auto calculate page info
			this._printPreviewCtrl.PrintPreviewControl.AutoCalculatePageInfo = autoCalculatePageInfo;

			this._printDocument = document;

			this.Owner.Controls.Add(this._printPreviewCtrl);
			this.Owner.ResumeLayout();
		}

		public void UninitializePrintPreviewControl()
		{
			if (this._ownerForm == null)
				throw new ArgumentNullException("_ownerForm");

			this.Owner.SuspendLayout();
			this.Owner.Controls.Remove(this._printPreviewCtrl);
			
			this._printPreviewCtrl.Document = null;
			this._printPreviewCtrl.Dispose();
			this._printPreviewCtrl = null;
			this._printDocument = null;

			this.Owner.ResumeLayout(true);
		}


		private void PrintPreviewCtrl_CloseButtonClick(object sender, EventArgs e)
		{
			this.EndPrintPreview();
		}

		private void PrintPreviewCtrl_Print(object sender, EventArgs e)
		{
			if (this.PrintButton_Click != null)
				this.PrintButton_Click(sender, e);
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			this.Dispose(true);
		}

		public void Dispose(bool dispose)
		{
			if (_printPreviewCtrl != null)
				_printPreviewCtrl.Dispose();
			
			this._ownerForm = null;
			this._printDocument = null;
			if (this._saveControls != null)
				this._saveControls.Clear();
			this._saveControls = null;

			GC.SuppressFinalize(this);
		}

		#endregion		
	}
}
