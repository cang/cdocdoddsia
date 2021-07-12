using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Collections;

using SIA.UI.Components.Printing;

namespace SIA.UI.Components.Dialogs
{
	/// <summary>
	/// Summary description for PrintPreviewForm.
	/// </summary>
	public class DlgPrintPreview : System.Windows.Forms.Form
	{
		#region events

		public event EventHandler PrintPreviewMode_Started = null;
		public event EventHandler PrintPreviewMode_Ended = null;

		#endregion

		#region fields

		private bool _isPreviewMode = false;
		private ArrayList _saveControls = null;
		private ucPrintPreview _printPreviewCtrl = null;
		private PrintDocument _printDocument = null;

		#endregion

		#region properties

		public bool IsPreviewMode
		{
			get {return _isPreviewMode;}
		}

		#endregion

		#region constructor and destructor

		public DlgPrintPreview()
		{
		}


		#endregion

		#region public methods

		public void StartPrintPreview(PrintDocument document)
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
				this.InitializePrintPreviewControl(document);

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
			this.SuspendLayout();

			// init save buffer
			_saveControls = new ArrayList();	
			
			// save child controls
			while (this.Controls.Count > 0)
			{
				_saveControls.Add(this.Controls[0]);
				this.Controls.RemoveAt(0);
			}

			// save main menu bar
			if (this.Menu != null)
			{
				_saveControls.Add(this.Menu);
				this.Menu = null;
			}

			this.ResumeLayout(false);
		}

		private void RestoreLayout()
		{
			this.SuspendLayout();
			
			if (_saveControls != null)
			{
				foreach(Object ctrl in _saveControls)
				{
					if (ctrl is Control) // restore child controls					
						this.Controls.Add((Control)ctrl);
					else if (ctrl is MainMenu) // restore main menu
						this.Menu = (MainMenu)ctrl;
				}

				_saveControls.Clear();
			}

			this.ResumeLayout(true);
		}

		public void InitializePrintPreviewControl(PrintDocument document)
		{
			this.SuspendLayout();
			
			this._printPreviewCtrl = new ucPrintPreview();
			this._printPreviewCtrl.Document = document;
			this._printPreviewCtrl.CloseButtonClick += new EventHandler(PrintPreviewCtrl_CloseButtonClick);
			this._printPreviewCtrl.Dock = DockStyle.Fill;

			this._printDocument = document;

			this.Controls.Add(this._printPreviewCtrl);
			this.ResumeLayout();
		}

		public void UninitializePrintPreviewControl()
		{
			this.SuspendLayout();
			this.Controls.Remove(this._printPreviewCtrl);
			
			this._printPreviewCtrl.Document = null;
			this._printPreviewCtrl.Dispose();
			this._printPreviewCtrl = null;
			this._printDocument = null;

			this.ResumeLayout(true);
		}

		private void InitializeComponent()
		{
			// 
			// DlgPrintPreview
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(332, 290);
			this.Name = "DlgPrintPreview";

		}


		private void PrintPreviewCtrl_CloseButtonClick(object sender, EventArgs e)
		{
			this.EndPrintPreview();
		}

		#endregion
	}
}
