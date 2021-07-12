using System;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SIA.UI.Components
{
	/// <summary>
	/// Summary description for PictureBoxEx.
	/// </summary>
	public class PictureBoxEx : UserControl
	{
		#region Constants
		const int _scrollControlSize = 25;
		#endregion Constants

		#region Member fields
		private Bitmap _image = null;

		private System.Windows.Forms.VScrollBar vScrollBar = null;
		private System.Windows.Forms.HScrollBar hScrollBar = null;
		private System.Windows.Forms.PictureBox pictureBox = null;
		#endregion Member fields

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;		

		#region Constructors and Destructors
		public PictureBoxEx()
		{
			InitializeComponent();
		}

		public PictureBoxEx(Bitmap image)
		{
			InitializeComponent();

			this.Image = image;
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
		#endregion Constructors and Destructors

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// PictureBoxEx
			// 
			this.AutoScroll = true;
			this.Name = "PictureBoxEx";
			this.Size = new System.Drawing.Size(641, 439);
			this.ResumeLayout(false);

		}
		#endregion

		#region Properties
		public bool UseVScroll
		{
			get
			{
				return (this.vScrollBar != null);
			}
		}

		public bool UseHScroll
		{
			get
			{
				return (this.hScrollBar != null);
			}
		}

		public Bitmap Image
		{
			get
			{
				return _image;
			}

			set
			{
				if (_image != null)
				{
					_image.Dispose();
					_image = null;
				}
				_image = value;
				this.OnImageChanged();
			}
		}
		#endregion Properties

		#region Override methods
		#endregion Override methods

		#region Initialize Class
		private void InitializePictureBox()
		{
			if (this.pictureBox != null)
				return;

			try
			{
				this.SuspendLayout();

				this.pictureBox = new System.Windows.Forms.PictureBox();
				this.pictureBox.Location = new System.Drawing.Point(0, 0);
				this.pictureBox.Name = "pictureBox";
				this.pictureBox.Size = new System.Drawing.Size(0, 0);
				this.pictureBox.TabStop = false;
				this.Controls.Add(this.pictureBox);
				this.pictureBox.TabIndex = this.Controls.Count - 1;
			}
			catch (System.Exception exp)
			{
				throw exp;
			}
			finally
			{
				this.ResumeLayout(false);
			}
		}

		private void UninitializePictureBox()
		{
			try
			{
				this.SuspendLayout();

				if (this.pictureBox != null)
				{
					if (this.pictureBox.Image != null)
					{
						this.pictureBox.Image.Dispose();
						this.pictureBox.Image = null;
					}

					this.Controls.Remove(this.pictureBox);

					this.pictureBox.Dispose();
					this.pictureBox = null;
				}
			}
			catch (System.Exception exp)
			{
				throw exp;
			}
			finally
			{
				this.ResumeLayout(false);
			}
		}

		#region VScroll
		private void InitializeVScroll()
		{
			if (this.vScrollBar != null)
				return;

			try
			{
				this.SuspendLayout();

				this.vScrollBar = new System.Windows.Forms.VScrollBar();

				this.vScrollBar.Name = "vScrollBar";

				this.configVScroll();

				this.Controls.Add(this.vScrollBar);
				this.vScrollBar.TabIndex = this.Controls.Count - 1;
			}
			catch (System.Exception exp)
			{
				throw exp;
			}
			finally
			{
				this.ResumeLayout(false);
			}
		}

		private void configVScroll()
		{
			if (this.vScrollBar == null)
				return;

			int width = this.Width;
			int height = this.Height;

			int vScrollHeight = height - _scrollControlSize;
			if (vScrollHeight < 0)
				vScrollHeight = 0;
			this.vScrollBar.Size = new System.Drawing.Size(_scrollControlSize, vScrollHeight);

			int xPosition = width - _scrollControlSize;
			if (xPosition < 0)
				xPosition = 0;
			this.vScrollBar.Location = new System.Drawing.Point(xPosition, 0);
		}

		private void UninitializeVScroll()
		{
			if (this.vScrollBar == null)
				return;
			try
			{
				this.SuspendLayout();

				this.Controls.Remove(this.vScrollBar);

				this.vScrollBar.Dispose();
				this.vScrollBar = null;
			}
			catch (System.Exception exp)
			{
				throw exp;
			}
			finally
			{
				this.ResumeLayout(false);
			}
		}
		#endregion VScroll

		#region HScroll
		private void InitializeHScroll()
		{
			if (this.hScrollBar != null)
				return;
			try
			{
				this.SuspendLayout();

				this.hScrollBar = new System.Windows.Forms.HScrollBar();

				this.hScrollBar.Name = "hScrollBar";

				this.configHScroll();

				this.Controls.Add(this.hScrollBar);
				this.hScrollBar.TabIndex = this.Controls.Count - 1;
			}
			catch (System.Exception exp)
			{
				throw exp;
			}
			finally
			{
				this.ResumeLayout(false);
			}
		}

		private void configHScroll()
		{
			if (this.hScrollBar == null)
				return;

			int width = this.Width;
			int height = this.Height;

			int hScrollWidth = width - _scrollControlSize;
			if (hScrollWidth < 0)
				hScrollWidth = 0;
			this.hScrollBar.Size = new System.Drawing.Size(hScrollWidth, _scrollControlSize);

			int yPosition = height - _scrollControlSize;
			if (yPosition < 0)
				yPosition = 0;
			this.hScrollBar.Location = new System.Drawing.Point(0, yPosition);
		}

		private void UninitializeHScroll()
		{
			if (this.hScrollBar == null)
				return;
			try
			{
				this.SuspendLayout();

				this.Controls.Remove(this.hScrollBar);

				this.hScrollBar.Dispose();
				this.hScrollBar = null;
			}
			catch (System.Exception exp)
			{
				throw exp;
			}
			finally
			{
				this.ResumeLayout(false);
			}
		}
		#endregion HScroll

		#endregion Initialize Class

		#region Methods
		#endregion Methods

		#region Internal event handlers

		#endregion Internal event handlers

		#region Event handlers
		private void OnImageChanged()
		{
			if (this._image == null)
			{
				this.UninitializePictureBox();
				this.UninitializeVScroll();
				this.UninitializeHScroll();
				return;
			}

			try
			{
				this.InitializePictureBox();

				if (this.pictureBox.Image != null)
				{
					this.pictureBox.Image.Dispose();
					this.pictureBox.Image = null;
				}

				this.pictureBox.Image = this.GetImage();
				if (_image != null)
				{
					this.pictureBox.Size = _image.Size;
				}
			}
			catch (System.Exception exp)
			{
				throw exp;
			}
			finally
			{
			}
		}
		#endregion Event handlers

		#region Internal helpers
		private Image GetImage()
		{
			return (Image)_image;
			//return null;
		}
		#endregion Internal helpers
	}
}
