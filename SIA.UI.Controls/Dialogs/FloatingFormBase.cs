using System;
using System.Reflection;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

using SIA.SystemLayer;
using SIA.UI.Controls;
using SIA.UI.Controls.Common;
using SIA.UI.Controls.Utilities;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Summary description for FloatingFormBase.
	/// </summary>
	public class FloatingFormBase : DialogBase
	{
		private System.Windows.Forms.Timer timer1;
		protected System.ComponentModel.IContainer components;
		
		public const double minOpacity = 0.45F;
		public const double maxOpacity = 1F;		
		
		private const double step = 0.05F;
		private bool fadeOutEnabled = true;

		
		public bool FadeOutEnabled
		{
			get {return fadeOutEnabled;}
			set 
			{
				fadeOutEnabled = value;
				OnFadeOutEnabledChanged();
			}
		}

		protected virtual void OnFadeOutEnabledChanged()
		{
			if (this.timer1 != null)
				this.timer1.Enabled = false;
		}

		public FloatingFormBase()
		{
			this.InitializeComponent();
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = 
                new System.Resources.ResourceManager(typeof(FloatingFormBase));
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			// 
			// timer1
			// 
			this.timer1.Interval = 100;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// FloatingFormBase
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(306, 256);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FloatingFormBase";
		}
		

		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated (e);

			if (this.fadeOutEnabled)
			{
				this.Opacity = maxOpacity;
				timer1.Enabled = false;
			}
		}

		protected override void OnDeactivate(EventArgs e)
		{
			base.OnDeactivate (e);

			if (this.fadeOutEnabled)
				timer1.Enabled = true;
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter (e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave (e);
		}


		protected override void OnMouseHover(EventArgs e)
		{
			base.OnMouseHover (e);
		}


		private void timer1_Tick(object sender, System.EventArgs e)
		{
			double opacity = this.Opacity;
			if (opacity > minOpacity)
				this.Opacity -= 0.1f;
			else
				this.timer1.Enabled = false;
		}
	}
}
