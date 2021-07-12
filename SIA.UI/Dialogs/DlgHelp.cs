using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SIA.UI
{
	/// <summary>
	/// Summary description for HelpForm.
	/// </summary>
	public class HelpForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListView ListHelp;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ColumnHeader clmTitle;
		private System.Windows.Forms.ColumnHeader clmLocation;
		private System.Windows.Forms.Button btnDisplay;
		private string ShowType = "Horz. Cut Graph";
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public HelpForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
																													 "Horz. Cut Graph",
																													 "Real-time  Defect Extractor - User Guide"}, -1);
			System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
																													 "Vert. Cut Graph",
																													 "Real-time  Defect Extractor - User Guide"}, -1);
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(HelpForm));
			this.ListHelp = new System.Windows.Forms.ListView();
			this.label1 = new System.Windows.Forms.Label();
			this.clmTitle = new System.Windows.Forms.ColumnHeader();
			this.clmLocation = new System.Windows.Forms.ColumnHeader();
			this.btnDisplay = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// ListHelp
			// 
			this.ListHelp.Activation = System.Windows.Forms.ItemActivation.TwoClick;
			this.ListHelp.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					   this.clmTitle,
																					   this.clmLocation});
			this.ListHelp.FullRowSelect = true;
			this.ListHelp.HideSelection = false;
			listViewItem1.StateImageIndex = 0;
			this.ListHelp.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
																					 listViewItem1,
																					 listViewItem2});
			this.ListHelp.Location = new System.Drawing.Point(8, 32);
			this.ListHelp.MultiSelect = false;
			this.ListHelp.Name = "ListHelp";
			this.ListHelp.Size = new System.Drawing.Size(400, 96);
			this.ListHelp.TabIndex = 0;
			this.ListHelp.View = System.Windows.Forms.View.Details;
			this.ListHelp.DoubleClick += new System.EventHandler(this.ListHelp_DoubleClick);
			this.ListHelp.SelectedIndexChanged += new System.EventHandler(this.ListHelp_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 11);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(162, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Click a topic, then click Display.";
			// 
			// clmTitle
			// 
			this.clmTitle.Text = "Title";
			this.clmTitle.Width = 151;
			// 
			// clmLocation
			// 
			this.clmLocation.Text = "Location";
			this.clmLocation.Width = 239;
			// 
			// btnDisplay
			// 
			this.btnDisplay.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnDisplay.Location = new System.Drawing.Point(240, 134);
			this.btnDisplay.Name = "btnDisplay";
			this.btnDisplay.TabIndex = 2;
			this.btnDisplay.Text = "&Display";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(328, 134);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "&Cancel";
			// 
			// HelpForm
			// 
			this.AcceptButton = this.btnDisplay;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(416, 163);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnDisplay);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.ListHelp);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "HelpForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Topics Found";
			this.Load += new System.EventHandler(this.HelpForm_Load);
			this.ResumeLayout(false);

		}
		#endregion
			
		public  string SelectedItem
		{
			get
			{
				return ShowType;
			}
		}


		private void HelpForm_Load(object sender, System.EventArgs e)
		{
			
		}

		private void ListHelp_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (ListHelp.SelectedItems.Count>0)
			{
				ShowType = ListHelp.SelectedItems[0].Text;
			}
			else
			{
				ListHelp.Select();
			}
			
		}

		private void ListHelp_DoubleClick(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Dispose(true);
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Dispose(false);
		}
	}
}
