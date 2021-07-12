using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SIA.UI
{
	/// <summary>
	/// Summary description for DlgFlashScreen.
	/// </summary>
	public class DlgFlashScreen : System.Windows.Forms.Form
	{
		private static DlgFlashScreen _instance = null;
		private System.Windows.Forms.Timer timer;
		private System.ComponentModel.IContainer components;

		public static DlgFlashScreen FlashScreen
		{
			get {return _instance;}
		}

		public DlgFlashScreen()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			DlgFlashScreen._instance = this;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
				if (DlgFlashScreen._instance != null)
					DlgFlashScreen._instance = null;
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgFlashScreen));
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // DlgFlashScreen
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackgroundImage = global::SIA.UI.Properties.Resources.SIA;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(500, 300);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgFlashScreen";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FlashWindow_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FlashWindow_MouseDown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FlashWindow_KeyDown);
            this.ResumeLayout(false);

		}
		#endregion

		private void FlashWindow_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(UserClose!=null)
				UserClose(null,EventArgs.Empty);
			Close();

		}

		private void FlashWindow_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(UserClose!=null)
				UserClose(null,EventArgs.Empty);
			Close();
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			if(++secondwait>=limitwait) 
			{
				if(UserClose!=null)
					UserClose(null,EventArgs.Empty);
				Close();
			}
		}
		private void FlashWindow_Load(object sender, System.EventArgs e)
		{
            //try
            //{
            //    limitwait=Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["FLASH_DELAY"]);
            //    timer.Enabled=true;
            //}
            //catch
            {
                limitwait = 2;
            }
		}
	
		public event EventHandler UserClose;
		private int	secondwait=0;
		private int	limitwait=0;


	}
}
