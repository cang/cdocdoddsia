using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using SIA.UI.Controls.Utilities;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Name : DlgCustomFilter
	/// Description : User interface for Physical Coordinate Settings operation
	/// Thread Support : False
	/// Persistence Data : False
	/// </summary>
	public class DlgCustomFilter
        : DialogBase
	{
		#region Windows Form members

		private System.Windows.Forms.TextBox textBoxFileName;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.ErrorProvider errorProvider;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panelMain;
		private System.Windows.Forms.Panel panel5_5;
		private System.Windows.Forms.TextBox txt5_13;
		private System.Windows.Forms.TextBox txt5_12;
		private System.Windows.Forms.TextBox txt5_11;
		private System.Windows.Forms.TextBox txt5_7;
		private System.Windows.Forms.TextBox txt5_8;
		private System.Windows.Forms.TextBox txt5_6;
		private System.Windows.Forms.TextBox txt5_1;
		private System.Windows.Forms.TextBox txt5_3;
		private System.Windows.Forms.TextBox txt5_2;
		private System.Windows.Forms.TextBox txt5_14;
		private System.Windows.Forms.TextBox txt5_19;
		private System.Windows.Forms.TextBox txt5_10;
		private System.Windows.Forms.TextBox txt5_9;
		private System.Windows.Forms.TextBox txt5_4;
		private System.Windows.Forms.TextBox txt5_15;
		private System.Windows.Forms.TextBox txt5_20;
		private System.Windows.Forms.TextBox txt5_24;
		private System.Windows.Forms.TextBox txt5_25;
		private System.Windows.Forms.TextBox txt5_5;
		private System.Windows.Forms.TextBox txt5_21;
		private System.Windows.Forms.TextBox txt5_17;
		private System.Windows.Forms.TextBox txt5_16;
		private System.Windows.Forms.TextBox txt5_18;
		private System.Windows.Forms.TextBox txt5_23;
		private System.Windows.Forms.TextBox txt5_22;

		private System.Windows.Forms.RadioButton radiobtn3_3;
		private System.Windows.Forms.RadioButton radiobnt7_7;
        private System.Windows.Forms.RadioButton radiobnt5_5;
        private IContainer components;

		private Panel currentPanel;
		private System.Windows.Forms.TextBox textBoxItem9;
		private System.Windows.Forms.TextBox textBoxItem8;
		private System.Windows.Forms.TextBox textBoxItem7;
		private System.Windows.Forms.TextBox textBoxItem5;
		private System.Windows.Forms.TextBox textBoxItem6;
		private System.Windows.Forms.TextBox textBoxItem4;
		private System.Windows.Forms.TextBox textBoxItem1;
		private System.Windows.Forms.TextBox textBoxItem3;
		private System.Windows.Forms.TextBox textBoxItem2;
		private System.Windows.Forms.Panel panel3_3;
		private System.Windows.Forms.TextBox txt7_17;
		private System.Windows.Forms.TextBox txt7_16;
		private System.Windows.Forms.TextBox txt7_15;
		private System.Windows.Forms.TextBox txt7_9;
		private System.Windows.Forms.TextBox txt7_10;
		private System.Windows.Forms.TextBox txt7_8;
		private System.Windows.Forms.TextBox txt7_1;
		private System.Windows.Forms.TextBox txt7_3;
		private System.Windows.Forms.TextBox txt7_2;
		private System.Windows.Forms.TextBox txt7_49;
		private System.Windows.Forms.TextBox txt7_27;
		private System.Windows.Forms.TextBox txt7_35;
		private System.Windows.Forms.TextBox txt7_7;
		private System.Windows.Forms.TextBox txt7_41;
		private System.Windows.Forms.TextBox txt7_34;
		private System.Windows.Forms.TextBox txt7_42;
		private System.Windows.Forms.TextBox txt7_13;
		private System.Windows.Forms.TextBox txt7_20;
		private System.Windows.Forms.TextBox txt7_48;
		private System.Windows.Forms.TextBox txt7_14;
		private System.Windows.Forms.TextBox txt7_28;
		private System.Windows.Forms.TextBox txt7_21;
		private System.Windows.Forms.TextBox txt7_6;
		private System.Windows.Forms.TextBox txt7_18;
		private System.Windows.Forms.TextBox txt7_25;
		private System.Windows.Forms.TextBox txt7_12;
		private System.Windows.Forms.TextBox txt7_11;
		private System.Windows.Forms.TextBox txt7_4;
		private System.Windows.Forms.TextBox txt7_19;
		private System.Windows.Forms.TextBox txt7_26;
		private System.Windows.Forms.TextBox txt7_32;
		private System.Windows.Forms.TextBox txt7_33;
		private System.Windows.Forms.TextBox txt7_39;
		private System.Windows.Forms.TextBox txt7_40;
		private System.Windows.Forms.TextBox txt7_46;
		private System.Windows.Forms.TextBox txt7_47;
		private System.Windows.Forms.TextBox txt7_5;
		private System.Windows.Forms.TextBox txt7_29;
		private System.Windows.Forms.TextBox txt7_45;
		private System.Windows.Forms.TextBox txt7_38;
		private System.Windows.Forms.TextBox txt7_44;
		private System.Windows.Forms.TextBox txt7_23;
		private System.Windows.Forms.TextBox txt7_22;
		private System.Windows.Forms.TextBox txt7_37;
		private System.Windows.Forms.TextBox txt7_36;
		private System.Windows.Forms.TextBox txt7_43;
		private System.Windows.Forms.TextBox txt7_24;
		private System.Windows.Forms.TextBox txt7_31;
		private System.Windows.Forms.TextBox txt7_30;
		private System.Windows.Forms.Panel panel7_7;
		#endregion		

		#region member attributes 
		private ArrayList m_Matrix = new ArrayList();		
		private ArrayList arrTextBox = new ArrayList();
		private ArrayList arrTextBox5_5 = new ArrayList();
		private ArrayList arrTextBox7_7 = new ArrayList();
		public int [,] mMatrixArr;
		
		private bool loaded = true;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button buttonLoadFile;
		private System.Windows.Forms.Button btnSave;
		private bool matrixValidation = false;
		#endregion 

		#region constructor and destructor
		public DlgCustomFilter()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.radiobnt5_5.CheckedChanged += new System.EventHandler(this.OnRadioCheckedChange);
			this.radiobnt7_7.CheckedChanged += new System.EventHandler(this.OnRadioCheckedChange);
			this.radiobtn3_3.CheckedChanged += new System.EventHandler(this.OnRadioCheckedChange);

			//add key up events for matrix text box
			
			
			#region contruc matrix 3 x 3
			//Matrix 3 x 3

			arrTextBox.Add( textBoxItem1 );
			arrTextBox.Add( textBoxItem2 );
			arrTextBox.Add( textBoxItem3 );
			arrTextBox.Add( textBoxItem4 );
			arrTextBox.Add( textBoxItem5);
			arrTextBox.Add( textBoxItem6 );
			arrTextBox.Add( textBoxItem7 );
			arrTextBox.Add( textBoxItem8 );
			arrTextBox.Add( textBoxItem9 );


			#endregion			

			#region contruc matrix 5 x 5
			
			
			//Matrix 5 x 5
			arrTextBox5_5.Add( txt5_1 );
			arrTextBox5_5.Add( txt5_2 );
			arrTextBox5_5.Add( txt5_3 );
			arrTextBox5_5.Add( txt5_4 );
			arrTextBox5_5.Add( txt5_5 );
			arrTextBox5_5.Add( txt5_6 );
			arrTextBox5_5.Add( txt5_7 );
			arrTextBox5_5.Add( txt5_8 );
			arrTextBox5_5.Add( txt5_9 );
			arrTextBox5_5.Add( txt5_10 );
			arrTextBox5_5.Add( txt5_11 );
			arrTextBox5_5.Add( txt5_12 );
			arrTextBox5_5.Add( txt5_13 );
			arrTextBox5_5.Add( txt5_14 );
			arrTextBox5_5.Add( txt5_15 );
			arrTextBox5_5.Add( txt5_16 );
			arrTextBox5_5.Add( txt5_17 );
			arrTextBox5_5.Add( txt5_18 );
			arrTextBox5_5.Add( txt5_19 );
			arrTextBox5_5.Add( txt5_20 );
			arrTextBox5_5.Add( txt5_21 );
			arrTextBox5_5.Add( txt5_22 );
			arrTextBox5_5.Add( txt5_23 );
			arrTextBox5_5.Add( txt5_24 );
			arrTextBox5_5.Add( txt5_25 );
			#endregion

			#region construc matrix 7 x 7
			//Matrix 7 x 7 
			arrTextBox7_7.Add( txt7_1 );
			arrTextBox7_7.Add( txt7_2 );
			arrTextBox7_7.Add( txt7_3 );
			arrTextBox7_7.Add( txt7_4 );
			arrTextBox7_7.Add( txt7_5 );
			arrTextBox7_7.Add( txt7_6 );
			arrTextBox7_7.Add( txt7_7 );
			arrTextBox7_7.Add( txt7_8 );
			arrTextBox7_7.Add( txt7_9 );
			arrTextBox7_7.Add( txt7_10 );
			arrTextBox7_7.Add( txt7_11 );
			arrTextBox7_7.Add( txt7_12 );
			arrTextBox7_7.Add( txt7_13 );
			arrTextBox7_7.Add( txt7_14 );
			arrTextBox7_7.Add( txt7_15 );
			arrTextBox7_7.Add( txt7_16 );
			arrTextBox7_7.Add( txt7_17 );
			arrTextBox7_7.Add( txt7_18 );
			arrTextBox7_7.Add( txt7_19 );
			arrTextBox7_7.Add( txt7_20 );
			arrTextBox7_7.Add( txt7_21 );
			arrTextBox7_7.Add( txt7_22 );
			arrTextBox7_7.Add( txt7_23 );
			arrTextBox7_7.Add( txt7_24 );
			arrTextBox7_7.Add( txt7_25 );
			arrTextBox7_7.Add( txt7_26 );
			arrTextBox7_7.Add( txt7_27 );
			arrTextBox7_7.Add( txt7_28 );
			arrTextBox7_7.Add( txt7_29 );
			arrTextBox7_7.Add( txt7_30 );
			arrTextBox7_7.Add( txt7_31 );
			arrTextBox7_7.Add( txt7_32 );
			arrTextBox7_7.Add( txt7_33 );
			arrTextBox7_7.Add( txt7_34 );
			arrTextBox7_7.Add( txt7_35 );
			arrTextBox7_7.Add( txt7_36 );
			arrTextBox7_7.Add( txt7_37 );
			arrTextBox7_7.Add( txt7_38 );
			arrTextBox7_7.Add( txt7_39 );
			arrTextBox7_7.Add( txt7_40 );
			arrTextBox7_7.Add( txt7_41 );
			arrTextBox7_7.Add( txt7_42 );
			arrTextBox7_7.Add( txt7_43 );
			arrTextBox7_7.Add( txt7_44 );
			arrTextBox7_7.Add( txt7_45 );
			arrTextBox7_7.Add( txt7_46 );
			arrTextBox7_7.Add( txt7_47 );
			arrTextBox7_7.Add( txt7_48 );
			arrTextBox7_7.Add( txt7_49 );
			#endregion
			

			//add events for text box
			
			AddEventsTextbox( panel3_3 );
			AddEventsTextbox( panel5_5 );
			AddEventsTextbox( panel7_7 );

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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (radiobtn3_3.Checked)
                OnRadioCheckedChange(radiobtn3_3, EventArgs.Empty);
            else if (radiobnt5_5.Checked)
                OnRadioCheckedChange(radiobnt5_5, EventArgs.Empty);
            else if (radiobnt7_7.Checked)
                OnRadioCheckedChange(radiobnt7_7, EventArgs.Empty);
        }
		#endregion		

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgCustomFilter));
            this.textBoxFileName = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel3_3 = new System.Windows.Forms.Panel();
            this.textBoxItem9 = new System.Windows.Forms.TextBox();
            this.textBoxItem8 = new System.Windows.Forms.TextBox();
            this.textBoxItem7 = new System.Windows.Forms.TextBox();
            this.textBoxItem5 = new System.Windows.Forms.TextBox();
            this.textBoxItem6 = new System.Windows.Forms.TextBox();
            this.textBoxItem4 = new System.Windows.Forms.TextBox();
            this.textBoxItem1 = new System.Windows.Forms.TextBox();
            this.textBoxItem3 = new System.Windows.Forms.TextBox();
            this.textBoxItem2 = new System.Windows.Forms.TextBox();
            this.buttonLoadFile = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radiobnt5_5 = new System.Windows.Forms.RadioButton();
            this.radiobnt7_7 = new System.Windows.Forms.RadioButton();
            this.radiobtn3_3 = new System.Windows.Forms.RadioButton();
            this.panel5_5 = new System.Windows.Forms.Panel();
            this.txt5_13 = new System.Windows.Forms.TextBox();
            this.txt5_12 = new System.Windows.Forms.TextBox();
            this.txt5_11 = new System.Windows.Forms.TextBox();
            this.txt5_7 = new System.Windows.Forms.TextBox();
            this.txt5_8 = new System.Windows.Forms.TextBox();
            this.txt5_6 = new System.Windows.Forms.TextBox();
            this.txt5_1 = new System.Windows.Forms.TextBox();
            this.txt5_3 = new System.Windows.Forms.TextBox();
            this.txt5_2 = new System.Windows.Forms.TextBox();
            this.txt5_14 = new System.Windows.Forms.TextBox();
            this.txt5_19 = new System.Windows.Forms.TextBox();
            this.txt5_10 = new System.Windows.Forms.TextBox();
            this.txt5_9 = new System.Windows.Forms.TextBox();
            this.txt5_4 = new System.Windows.Forms.TextBox();
            this.txt5_15 = new System.Windows.Forms.TextBox();
            this.txt5_20 = new System.Windows.Forms.TextBox();
            this.txt5_24 = new System.Windows.Forms.TextBox();
            this.txt5_25 = new System.Windows.Forms.TextBox();
            this.txt5_5 = new System.Windows.Forms.TextBox();
            this.txt5_21 = new System.Windows.Forms.TextBox();
            this.txt5_17 = new System.Windows.Forms.TextBox();
            this.txt5_16 = new System.Windows.Forms.TextBox();
            this.txt5_18 = new System.Windows.Forms.TextBox();
            this.txt5_23 = new System.Windows.Forms.TextBox();
            this.txt5_22 = new System.Windows.Forms.TextBox();
            this.panel7_7 = new System.Windows.Forms.Panel();
            this.txt7_17 = new System.Windows.Forms.TextBox();
            this.txt7_16 = new System.Windows.Forms.TextBox();
            this.txt7_15 = new System.Windows.Forms.TextBox();
            this.txt7_9 = new System.Windows.Forms.TextBox();
            this.txt7_10 = new System.Windows.Forms.TextBox();
            this.txt7_8 = new System.Windows.Forms.TextBox();
            this.txt7_1 = new System.Windows.Forms.TextBox();
            this.txt7_3 = new System.Windows.Forms.TextBox();
            this.txt7_2 = new System.Windows.Forms.TextBox();
            this.txt7_49 = new System.Windows.Forms.TextBox();
            this.txt7_27 = new System.Windows.Forms.TextBox();
            this.txt7_35 = new System.Windows.Forms.TextBox();
            this.txt7_7 = new System.Windows.Forms.TextBox();
            this.txt7_41 = new System.Windows.Forms.TextBox();
            this.txt7_34 = new System.Windows.Forms.TextBox();
            this.txt7_42 = new System.Windows.Forms.TextBox();
            this.txt7_13 = new System.Windows.Forms.TextBox();
            this.txt7_20 = new System.Windows.Forms.TextBox();
            this.txt7_48 = new System.Windows.Forms.TextBox();
            this.txt7_14 = new System.Windows.Forms.TextBox();
            this.txt7_28 = new System.Windows.Forms.TextBox();
            this.txt7_21 = new System.Windows.Forms.TextBox();
            this.txt7_6 = new System.Windows.Forms.TextBox();
            this.txt7_18 = new System.Windows.Forms.TextBox();
            this.txt7_25 = new System.Windows.Forms.TextBox();
            this.txt7_12 = new System.Windows.Forms.TextBox();
            this.txt7_11 = new System.Windows.Forms.TextBox();
            this.txt7_4 = new System.Windows.Forms.TextBox();
            this.txt7_19 = new System.Windows.Forms.TextBox();
            this.txt7_26 = new System.Windows.Forms.TextBox();
            this.txt7_32 = new System.Windows.Forms.TextBox();
            this.txt7_33 = new System.Windows.Forms.TextBox();
            this.txt7_39 = new System.Windows.Forms.TextBox();
            this.txt7_40 = new System.Windows.Forms.TextBox();
            this.txt7_46 = new System.Windows.Forms.TextBox();
            this.txt7_47 = new System.Windows.Forms.TextBox();
            this.txt7_5 = new System.Windows.Forms.TextBox();
            this.txt7_29 = new System.Windows.Forms.TextBox();
            this.txt7_45 = new System.Windows.Forms.TextBox();
            this.txt7_38 = new System.Windows.Forms.TextBox();
            this.txt7_44 = new System.Windows.Forms.TextBox();
            this.txt7_23 = new System.Windows.Forms.TextBox();
            this.txt7_22 = new System.Windows.Forms.TextBox();
            this.txt7_37 = new System.Windows.Forms.TextBox();
            this.txt7_36 = new System.Windows.Forms.TextBox();
            this.txt7_43 = new System.Windows.Forms.TextBox();
            this.txt7_24 = new System.Windows.Forms.TextBox();
            this.txt7_31 = new System.Windows.Forms.TextBox();
            this.txt7_30 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.panelMain.SuspendLayout();
            this.panel3_3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel5_5.SuspendLayout();
            this.panel7_7.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.Location = new System.Drawing.Point(8, 228);
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.ReadOnly = true;
            this.textBoxFileName.Size = new System.Drawing.Size(344, 20);
            this.textBoxFileName.TabIndex = 5;
            this.textBoxFileName.TabStop = false;
            this.textBoxFileName.Tag = "DEFAULT";
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(264, 36);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 9;
            this.buttonCancel.Text = "Cancel";
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(264, 8);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 8;
            this.buttonOK.Text = "OK";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 212);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "File Name:";
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.btnSave);
            this.panelMain.Controls.Add(this.panel3_3);
            this.panelMain.Controls.Add(this.buttonLoadFile);
            this.panelMain.Controls.Add(this.groupBox2);
            this.panelMain.Controls.Add(this.label2);
            this.panelMain.Controls.Add(this.textBoxFileName);
            this.panelMain.Controls.Add(this.buttonCancel);
            this.panelMain.Controls.Add(this.buttonOK);
            this.panelMain.Controls.Add(this.panel5_5);
            this.panelMain.Controls.Add(this.panel7_7);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(358, 284);
            this.panelMain.TabIndex = 0;
            this.panelMain.Paint += new System.Windows.Forms.PaintEventHandler(this.panelMain_Paint);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(88, 256);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panel3_3
            // 
            this.panel3_3.Controls.Add(this.textBoxItem9);
            this.panel3_3.Controls.Add(this.textBoxItem8);
            this.panel3_3.Controls.Add(this.textBoxItem7);
            this.panel3_3.Controls.Add(this.textBoxItem5);
            this.panel3_3.Controls.Add(this.textBoxItem6);
            this.panel3_3.Controls.Add(this.textBoxItem4);
            this.panel3_3.Controls.Add(this.textBoxItem1);
            this.panel3_3.Controls.Add(this.textBoxItem3);
            this.panel3_3.Controls.Add(this.textBoxItem2);
            this.panel3_3.Location = new System.Drawing.Point(8, 8);
            this.panel3_3.Name = "panel3_3";
            this.panel3_3.Size = new System.Drawing.Size(236, 200);
            this.panel3_3.TabIndex = 0;
            // 
            // textBoxItem9
            // 
            this.textBoxItem9.Location = new System.Drawing.Point(137, 118);
            this.textBoxItem9.Name = "textBoxItem9";
            this.textBoxItem9.Size = new System.Drawing.Size(26, 20);
            this.textBoxItem9.TabIndex = 8;
            this.textBoxItem9.Tag = "DEFAULT";
            // 
            // textBoxItem8
            // 
            this.textBoxItem8.Location = new System.Drawing.Point(105, 118);
            this.textBoxItem8.Name = "textBoxItem8";
            this.textBoxItem8.Size = new System.Drawing.Size(26, 20);
            this.textBoxItem8.TabIndex = 7;
            this.textBoxItem8.Tag = "DEFAULT";
            // 
            // textBoxItem7
            // 
            this.textBoxItem7.Location = new System.Drawing.Point(73, 118);
            this.textBoxItem7.Name = "textBoxItem7";
            this.textBoxItem7.Size = new System.Drawing.Size(26, 20);
            this.textBoxItem7.TabIndex = 6;
            this.textBoxItem7.Tag = "DEFAULT";
            // 
            // textBoxItem5
            // 
            this.textBoxItem5.Location = new System.Drawing.Point(105, 90);
            this.textBoxItem5.Name = "textBoxItem5";
            this.textBoxItem5.Size = new System.Drawing.Size(26, 20);
            this.textBoxItem5.TabIndex = 4;
            this.textBoxItem5.Tag = "DEFAULT";
            // 
            // textBoxItem6
            // 
            this.textBoxItem6.Location = new System.Drawing.Point(137, 90);
            this.textBoxItem6.Name = "textBoxItem6";
            this.textBoxItem6.Size = new System.Drawing.Size(26, 20);
            this.textBoxItem6.TabIndex = 5;
            this.textBoxItem6.Tag = "DEFAULT";
            // 
            // textBoxItem4
            // 
            this.textBoxItem4.Location = new System.Drawing.Point(73, 90);
            this.textBoxItem4.Name = "textBoxItem4";
            this.textBoxItem4.Size = new System.Drawing.Size(26, 20);
            this.textBoxItem4.TabIndex = 3;
            this.textBoxItem4.Tag = "DEFAULT";
            // 
            // textBoxItem1
            // 
            this.textBoxItem1.Location = new System.Drawing.Point(73, 62);
            this.textBoxItem1.Name = "textBoxItem1";
            this.textBoxItem1.Size = new System.Drawing.Size(26, 20);
            this.textBoxItem1.TabIndex = 0;
            this.textBoxItem1.Tag = "DEFAULT";
            // 
            // textBoxItem3
            // 
            this.textBoxItem3.Location = new System.Drawing.Point(137, 62);
            this.textBoxItem3.Name = "textBoxItem3";
            this.textBoxItem3.Size = new System.Drawing.Size(26, 20);
            this.textBoxItem3.TabIndex = 2;
            this.textBoxItem3.Tag = "DEFAULT";
            // 
            // textBoxItem2
            // 
            this.textBoxItem2.Location = new System.Drawing.Point(105, 62);
            this.textBoxItem2.Name = "textBoxItem2";
            this.textBoxItem2.Size = new System.Drawing.Size(26, 20);
            this.textBoxItem2.TabIndex = 1;
            this.textBoxItem2.Tag = "DEFAULT";
            // 
            // buttonLoadFile
            // 
            this.buttonLoadFile.Location = new System.Drawing.Point(8, 256);
            this.buttonLoadFile.Name = "buttonLoadFile";
            this.buttonLoadFile.Size = new System.Drawing.Size(75, 23);
            this.buttonLoadFile.TabIndex = 6;
            this.buttonLoadFile.Text = "Load";
            this.buttonLoadFile.Click += new System.EventHandler(this.buttonLoadFile_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radiobnt5_5);
            this.groupBox2.Controls.Add(this.radiobnt7_7);
            this.groupBox2.Controls.Add(this.radiobtn3_3);
            this.groupBox2.Location = new System.Drawing.Point(252, 128);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(100, 80);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Matrix Size";
            // 
            // radiobnt5_5
            // 
            this.radiobnt5_5.Location = new System.Drawing.Point(8, 34);
            this.radiobnt5_5.Name = "radiobnt5_5";
            this.radiobnt5_5.Size = new System.Drawing.Size(84, 16);
            this.radiobnt5_5.TabIndex = 1;
            this.radiobnt5_5.Tag = "DEFAULT";
            this.radiobnt5_5.Text = "Matrix 5 x 5";
            // 
            // radiobnt7_7
            // 
            this.radiobnt7_7.Location = new System.Drawing.Point(8, 54);
            this.radiobnt7_7.Name = "radiobnt7_7";
            this.radiobnt7_7.Size = new System.Drawing.Size(84, 16);
            this.radiobnt7_7.TabIndex = 2;
            this.radiobnt7_7.Tag = "DEFAULT";
            this.radiobnt7_7.Text = "Matrix 7 x 7";
            // 
            // radiobtn3_3
            // 
            this.radiobtn3_3.Checked = true;
            this.radiobtn3_3.Location = new System.Drawing.Point(8, 14);
            this.radiobtn3_3.Name = "radiobtn3_3";
            this.radiobtn3_3.Size = new System.Drawing.Size(84, 16);
            this.radiobtn3_3.TabIndex = 0;
            this.radiobtn3_3.TabStop = true;
            this.radiobtn3_3.Tag = "DEFAULT";
            this.radiobtn3_3.Text = "Matrix 3 x 3";
            // 
            // panel5_5
            // 
            this.panel5_5.Controls.Add(this.txt5_13);
            this.panel5_5.Controls.Add(this.txt5_12);
            this.panel5_5.Controls.Add(this.txt5_11);
            this.panel5_5.Controls.Add(this.txt5_7);
            this.panel5_5.Controls.Add(this.txt5_8);
            this.panel5_5.Controls.Add(this.txt5_6);
            this.panel5_5.Controls.Add(this.txt5_1);
            this.panel5_5.Controls.Add(this.txt5_3);
            this.panel5_5.Controls.Add(this.txt5_2);
            this.panel5_5.Controls.Add(this.txt5_14);
            this.panel5_5.Controls.Add(this.txt5_19);
            this.panel5_5.Controls.Add(this.txt5_10);
            this.panel5_5.Controls.Add(this.txt5_9);
            this.panel5_5.Controls.Add(this.txt5_4);
            this.panel5_5.Controls.Add(this.txt5_15);
            this.panel5_5.Controls.Add(this.txt5_20);
            this.panel5_5.Controls.Add(this.txt5_24);
            this.panel5_5.Controls.Add(this.txt5_25);
            this.panel5_5.Controls.Add(this.txt5_5);
            this.panel5_5.Controls.Add(this.txt5_21);
            this.panel5_5.Controls.Add(this.txt5_17);
            this.panel5_5.Controls.Add(this.txt5_16);
            this.panel5_5.Controls.Add(this.txt5_18);
            this.panel5_5.Controls.Add(this.txt5_23);
            this.panel5_5.Controls.Add(this.txt5_22);
            this.panel5_5.Location = new System.Drawing.Point(8, 8);
            this.panel5_5.Name = "panel5_5";
            this.panel5_5.Size = new System.Drawing.Size(236, 200);
            this.panel5_5.TabIndex = 1;
            // 
            // txt5_13
            // 
            this.txt5_13.Location = new System.Drawing.Point(105, 90);
            this.txt5_13.Name = "txt5_13";
            this.txt5_13.Size = new System.Drawing.Size(26, 20);
            this.txt5_13.TabIndex = 12;
            this.txt5_13.Tag = "DEFAULT";
            // 
            // txt5_12
            // 
            this.txt5_12.Location = new System.Drawing.Point(73, 90);
            this.txt5_12.Name = "txt5_12";
            this.txt5_12.Size = new System.Drawing.Size(26, 20);
            this.txt5_12.TabIndex = 11;
            this.txt5_12.Tag = "DEFAULT";
            // 
            // txt5_11
            // 
            this.txt5_11.Location = new System.Drawing.Point(41, 90);
            this.txt5_11.Name = "txt5_11";
            this.txt5_11.Size = new System.Drawing.Size(26, 20);
            this.txt5_11.TabIndex = 10;
            this.txt5_11.Tag = "DEFAULT";
            // 
            // txt5_7
            // 
            this.txt5_7.Location = new System.Drawing.Point(73, 62);
            this.txt5_7.Name = "txt5_7";
            this.txt5_7.Size = new System.Drawing.Size(26, 20);
            this.txt5_7.TabIndex = 6;
            this.txt5_7.Tag = "DEFAULT";
            // 
            // txt5_8
            // 
            this.txt5_8.Location = new System.Drawing.Point(105, 62);
            this.txt5_8.Name = "txt5_8";
            this.txt5_8.Size = new System.Drawing.Size(26, 20);
            this.txt5_8.TabIndex = 7;
            this.txt5_8.Tag = "DEFAULT";
            // 
            // txt5_6
            // 
            this.txt5_6.Location = new System.Drawing.Point(41, 62);
            this.txt5_6.Name = "txt5_6";
            this.txt5_6.Size = new System.Drawing.Size(26, 20);
            this.txt5_6.TabIndex = 5;
            this.txt5_6.Tag = "DEFAULT";
            // 
            // txt5_1
            // 
            this.txt5_1.Location = new System.Drawing.Point(41, 34);
            this.txt5_1.Name = "txt5_1";
            this.txt5_1.Size = new System.Drawing.Size(26, 20);
            this.txt5_1.TabIndex = 0;
            this.txt5_1.Tag = "DEFAULT";
            // 
            // txt5_3
            // 
            this.txt5_3.Location = new System.Drawing.Point(105, 34);
            this.txt5_3.Name = "txt5_3";
            this.txt5_3.Size = new System.Drawing.Size(26, 20);
            this.txt5_3.TabIndex = 2;
            this.txt5_3.Tag = "DEFAULT";
            // 
            // txt5_2
            // 
            this.txt5_2.Location = new System.Drawing.Point(73, 34);
            this.txt5_2.Name = "txt5_2";
            this.txt5_2.Size = new System.Drawing.Size(26, 20);
            this.txt5_2.TabIndex = 1;
            this.txt5_2.Tag = "DEFAULT";
            // 
            // txt5_14
            // 
            this.txt5_14.Location = new System.Drawing.Point(137, 90);
            this.txt5_14.Name = "txt5_14";
            this.txt5_14.Size = new System.Drawing.Size(26, 20);
            this.txt5_14.TabIndex = 13;
            this.txt5_14.Tag = "DEFAULT";
            // 
            // txt5_19
            // 
            this.txt5_19.Location = new System.Drawing.Point(137, 118);
            this.txt5_19.Name = "txt5_19";
            this.txt5_19.Size = new System.Drawing.Size(26, 20);
            this.txt5_19.TabIndex = 18;
            this.txt5_19.Tag = "DEFAULT";
            // 
            // txt5_10
            // 
            this.txt5_10.Location = new System.Drawing.Point(169, 62);
            this.txt5_10.Name = "txt5_10";
            this.txt5_10.Size = new System.Drawing.Size(26, 20);
            this.txt5_10.TabIndex = 9;
            this.txt5_10.Tag = "DEFAULT";
            // 
            // txt5_9
            // 
            this.txt5_9.Location = new System.Drawing.Point(137, 62);
            this.txt5_9.Name = "txt5_9";
            this.txt5_9.Size = new System.Drawing.Size(26, 20);
            this.txt5_9.TabIndex = 8;
            this.txt5_9.Tag = "DEFAULT";
            // 
            // txt5_4
            // 
            this.txt5_4.Location = new System.Drawing.Point(137, 34);
            this.txt5_4.Name = "txt5_4";
            this.txt5_4.Size = new System.Drawing.Size(26, 20);
            this.txt5_4.TabIndex = 3;
            this.txt5_4.Tag = "DEFAULT";
            // 
            // txt5_15
            // 
            this.txt5_15.Location = new System.Drawing.Point(169, 90);
            this.txt5_15.Name = "txt5_15";
            this.txt5_15.Size = new System.Drawing.Size(26, 20);
            this.txt5_15.TabIndex = 14;
            this.txt5_15.Tag = "DEFAULT";
            // 
            // txt5_20
            // 
            this.txt5_20.Location = new System.Drawing.Point(169, 118);
            this.txt5_20.Name = "txt5_20";
            this.txt5_20.Size = new System.Drawing.Size(26, 20);
            this.txt5_20.TabIndex = 19;
            this.txt5_20.Tag = "DEFAULT";
            // 
            // txt5_24
            // 
            this.txt5_24.Location = new System.Drawing.Point(137, 146);
            this.txt5_24.Name = "txt5_24";
            this.txt5_24.Size = new System.Drawing.Size(26, 20);
            this.txt5_24.TabIndex = 23;
            this.txt5_24.Tag = "DEFAULT";
            // 
            // txt5_25
            // 
            this.txt5_25.Location = new System.Drawing.Point(169, 146);
            this.txt5_25.Name = "txt5_25";
            this.txt5_25.Size = new System.Drawing.Size(26, 20);
            this.txt5_25.TabIndex = 24;
            this.txt5_25.Tag = "DEFAULT";
            // 
            // txt5_5
            // 
            this.txt5_5.Location = new System.Drawing.Point(169, 34);
            this.txt5_5.Name = "txt5_5";
            this.txt5_5.Size = new System.Drawing.Size(26, 20);
            this.txt5_5.TabIndex = 4;
            this.txt5_5.Tag = "DEFAULT";
            // 
            // txt5_21
            // 
            this.txt5_21.Location = new System.Drawing.Point(41, 146);
            this.txt5_21.Name = "txt5_21";
            this.txt5_21.Size = new System.Drawing.Size(26, 20);
            this.txt5_21.TabIndex = 20;
            this.txt5_21.Tag = "DEFAULT";
            // 
            // txt5_17
            // 
            this.txt5_17.Location = new System.Drawing.Point(73, 118);
            this.txt5_17.Name = "txt5_17";
            this.txt5_17.Size = new System.Drawing.Size(26, 20);
            this.txt5_17.TabIndex = 16;
            this.txt5_17.Tag = "DEFAULT";
            // 
            // txt5_16
            // 
            this.txt5_16.Location = new System.Drawing.Point(41, 118);
            this.txt5_16.Name = "txt5_16";
            this.txt5_16.Size = new System.Drawing.Size(26, 20);
            this.txt5_16.TabIndex = 15;
            this.txt5_16.Tag = "DEFAULT";
            // 
            // txt5_18
            // 
            this.txt5_18.Location = new System.Drawing.Point(105, 118);
            this.txt5_18.Name = "txt5_18";
            this.txt5_18.Size = new System.Drawing.Size(26, 20);
            this.txt5_18.TabIndex = 17;
            this.txt5_18.Tag = "DEFAULT";
            // 
            // txt5_23
            // 
            this.txt5_23.Location = new System.Drawing.Point(105, 146);
            this.txt5_23.Name = "txt5_23";
            this.txt5_23.Size = new System.Drawing.Size(26, 20);
            this.txt5_23.TabIndex = 22;
            this.txt5_23.Tag = "DEFAULT";
            // 
            // txt5_22
            // 
            this.txt5_22.Location = new System.Drawing.Point(73, 146);
            this.txt5_22.Name = "txt5_22";
            this.txt5_22.Size = new System.Drawing.Size(26, 20);
            this.txt5_22.TabIndex = 21;
            this.txt5_22.Tag = "DEFAULT";
            // 
            // panel7_7
            // 
            this.panel7_7.Controls.Add(this.txt7_17);
            this.panel7_7.Controls.Add(this.txt7_16);
            this.panel7_7.Controls.Add(this.txt7_15);
            this.panel7_7.Controls.Add(this.txt7_9);
            this.panel7_7.Controls.Add(this.txt7_10);
            this.panel7_7.Controls.Add(this.txt7_8);
            this.panel7_7.Controls.Add(this.txt7_1);
            this.panel7_7.Controls.Add(this.txt7_3);
            this.panel7_7.Controls.Add(this.txt7_2);
            this.panel7_7.Controls.Add(this.txt7_49);
            this.panel7_7.Controls.Add(this.txt7_27);
            this.panel7_7.Controls.Add(this.txt7_35);
            this.panel7_7.Controls.Add(this.txt7_7);
            this.panel7_7.Controls.Add(this.txt7_41);
            this.panel7_7.Controls.Add(this.txt7_34);
            this.panel7_7.Controls.Add(this.txt7_42);
            this.panel7_7.Controls.Add(this.txt7_13);
            this.panel7_7.Controls.Add(this.txt7_20);
            this.panel7_7.Controls.Add(this.txt7_48);
            this.panel7_7.Controls.Add(this.txt7_14);
            this.panel7_7.Controls.Add(this.txt7_28);
            this.panel7_7.Controls.Add(this.txt7_21);
            this.panel7_7.Controls.Add(this.txt7_6);
            this.panel7_7.Controls.Add(this.txt7_18);
            this.panel7_7.Controls.Add(this.txt7_25);
            this.panel7_7.Controls.Add(this.txt7_12);
            this.panel7_7.Controls.Add(this.txt7_11);
            this.panel7_7.Controls.Add(this.txt7_4);
            this.panel7_7.Controls.Add(this.txt7_19);
            this.panel7_7.Controls.Add(this.txt7_26);
            this.panel7_7.Controls.Add(this.txt7_32);
            this.panel7_7.Controls.Add(this.txt7_33);
            this.panel7_7.Controls.Add(this.txt7_39);
            this.panel7_7.Controls.Add(this.txt7_40);
            this.panel7_7.Controls.Add(this.txt7_46);
            this.panel7_7.Controls.Add(this.txt7_47);
            this.panel7_7.Controls.Add(this.txt7_5);
            this.panel7_7.Controls.Add(this.txt7_29);
            this.panel7_7.Controls.Add(this.txt7_45);
            this.panel7_7.Controls.Add(this.txt7_38);
            this.panel7_7.Controls.Add(this.txt7_44);
            this.panel7_7.Controls.Add(this.txt7_23);
            this.panel7_7.Controls.Add(this.txt7_22);
            this.panel7_7.Controls.Add(this.txt7_37);
            this.panel7_7.Controls.Add(this.txt7_36);
            this.panel7_7.Controls.Add(this.txt7_43);
            this.panel7_7.Controls.Add(this.txt7_24);
            this.panel7_7.Controls.Add(this.txt7_31);
            this.panel7_7.Controls.Add(this.txt7_30);
            this.panel7_7.Location = new System.Drawing.Point(8, 8);
            this.panel7_7.Name = "panel7_7";
            this.panel7_7.Size = new System.Drawing.Size(236, 200);
            this.panel7_7.TabIndex = 2;
            // 
            // txt7_17
            // 
            this.txt7_17.Location = new System.Drawing.Point(73, 62);
            this.txt7_17.Name = "txt7_17";
            this.txt7_17.Size = new System.Drawing.Size(26, 20);
            this.txt7_17.TabIndex = 16;
            this.txt7_17.Tag = "DEFAULT";
            // 
            // txt7_16
            // 
            this.txt7_16.Location = new System.Drawing.Point(41, 62);
            this.txt7_16.Name = "txt7_16";
            this.txt7_16.Size = new System.Drawing.Size(26, 20);
            this.txt7_16.TabIndex = 15;
            this.txt7_16.Tag = "DEFAULT";
            // 
            // txt7_15
            // 
            this.txt7_15.Location = new System.Drawing.Point(9, 62);
            this.txt7_15.Name = "txt7_15";
            this.txt7_15.Size = new System.Drawing.Size(26, 20);
            this.txt7_15.TabIndex = 14;
            this.txt7_15.Tag = "DEFAULT";
            // 
            // txt7_9
            // 
            this.txt7_9.Location = new System.Drawing.Point(41, 34);
            this.txt7_9.Name = "txt7_9";
            this.txt7_9.Size = new System.Drawing.Size(26, 20);
            this.txt7_9.TabIndex = 8;
            this.txt7_9.Tag = "DEFAULT";
            // 
            // txt7_10
            // 
            this.txt7_10.Location = new System.Drawing.Point(73, 34);
            this.txt7_10.Name = "txt7_10";
            this.txt7_10.Size = new System.Drawing.Size(26, 20);
            this.txt7_10.TabIndex = 9;
            this.txt7_10.Tag = "DEFAULT";
            // 
            // txt7_8
            // 
            this.txt7_8.Location = new System.Drawing.Point(9, 34);
            this.txt7_8.Name = "txt7_8";
            this.txt7_8.Size = new System.Drawing.Size(26, 20);
            this.txt7_8.TabIndex = 7;
            this.txt7_8.Tag = "DEFAULT";
            // 
            // txt7_1
            // 
            this.txt7_1.Location = new System.Drawing.Point(9, 6);
            this.txt7_1.Name = "txt7_1";
            this.txt7_1.Size = new System.Drawing.Size(26, 20);
            this.txt7_1.TabIndex = 0;
            this.txt7_1.Tag = "DEFAULT";
            // 
            // txt7_3
            // 
            this.txt7_3.Location = new System.Drawing.Point(73, 6);
            this.txt7_3.Name = "txt7_3";
            this.txt7_3.Size = new System.Drawing.Size(26, 20);
            this.txt7_3.TabIndex = 2;
            this.txt7_3.Tag = "DEFAULT";
            // 
            // txt7_2
            // 
            this.txt7_2.Location = new System.Drawing.Point(41, 6);
            this.txt7_2.Name = "txt7_2";
            this.txt7_2.Size = new System.Drawing.Size(26, 20);
            this.txt7_2.TabIndex = 1;
            this.txt7_2.Tag = "DEFAULT";
            // 
            // txt7_49
            // 
            this.txt7_49.Location = new System.Drawing.Point(201, 174);
            this.txt7_49.Name = "txt7_49";
            this.txt7_49.Size = new System.Drawing.Size(26, 20);
            this.txt7_49.TabIndex = 48;
            this.txt7_49.Tag = "DEFAULT";
            // 
            // txt7_27
            // 
            this.txt7_27.Location = new System.Drawing.Point(169, 90);
            this.txt7_27.Name = "txt7_27";
            this.txt7_27.Size = new System.Drawing.Size(26, 20);
            this.txt7_27.TabIndex = 26;
            this.txt7_27.Tag = "DEFAULT";
            // 
            // txt7_35
            // 
            this.txt7_35.Location = new System.Drawing.Point(201, 118);
            this.txt7_35.Name = "txt7_35";
            this.txt7_35.Size = new System.Drawing.Size(26, 20);
            this.txt7_35.TabIndex = 34;
            this.txt7_35.Tag = "DEFAULT";
            // 
            // txt7_7
            // 
            this.txt7_7.Location = new System.Drawing.Point(201, 6);
            this.txt7_7.Name = "txt7_7";
            this.txt7_7.Size = new System.Drawing.Size(26, 20);
            this.txt7_7.TabIndex = 6;
            this.txt7_7.Tag = "DEFAULT";
            // 
            // txt7_41
            // 
            this.txt7_41.Location = new System.Drawing.Point(169, 146);
            this.txt7_41.Name = "txt7_41";
            this.txt7_41.Size = new System.Drawing.Size(26, 20);
            this.txt7_41.TabIndex = 40;
            this.txt7_41.Tag = "DEFAULT";
            // 
            // txt7_34
            // 
            this.txt7_34.Location = new System.Drawing.Point(169, 118);
            this.txt7_34.Name = "txt7_34";
            this.txt7_34.Size = new System.Drawing.Size(26, 20);
            this.txt7_34.TabIndex = 33;
            this.txt7_34.Tag = "DEFAULT";
            // 
            // txt7_42
            // 
            this.txt7_42.Location = new System.Drawing.Point(201, 146);
            this.txt7_42.Name = "txt7_42";
            this.txt7_42.Size = new System.Drawing.Size(26, 20);
            this.txt7_42.TabIndex = 41;
            this.txt7_42.Tag = "DEFAULT";
            // 
            // txt7_13
            // 
            this.txt7_13.Location = new System.Drawing.Point(169, 34);
            this.txt7_13.Name = "txt7_13";
            this.txt7_13.Size = new System.Drawing.Size(26, 20);
            this.txt7_13.TabIndex = 12;
            this.txt7_13.Tag = "DEFAULT";
            // 
            // txt7_20
            // 
            this.txt7_20.Location = new System.Drawing.Point(169, 62);
            this.txt7_20.Name = "txt7_20";
            this.txt7_20.Size = new System.Drawing.Size(26, 20);
            this.txt7_20.TabIndex = 19;
            this.txt7_20.Tag = "DEFAULT";
            // 
            // txt7_48
            // 
            this.txt7_48.Location = new System.Drawing.Point(169, 174);
            this.txt7_48.Name = "txt7_48";
            this.txt7_48.Size = new System.Drawing.Size(26, 20);
            this.txt7_48.TabIndex = 47;
            this.txt7_48.Tag = "DEFAULT";
            // 
            // txt7_14
            // 
            this.txt7_14.Location = new System.Drawing.Point(201, 34);
            this.txt7_14.Name = "txt7_14";
            this.txt7_14.Size = new System.Drawing.Size(26, 20);
            this.txt7_14.TabIndex = 13;
            this.txt7_14.Tag = "DEFAULT";
            // 
            // txt7_28
            // 
            this.txt7_28.Location = new System.Drawing.Point(201, 90);
            this.txt7_28.Name = "txt7_28";
            this.txt7_28.Size = new System.Drawing.Size(26, 20);
            this.txt7_28.TabIndex = 27;
            this.txt7_28.Tag = "DEFAULT";
            // 
            // txt7_21
            // 
            this.txt7_21.Location = new System.Drawing.Point(201, 62);
            this.txt7_21.Name = "txt7_21";
            this.txt7_21.Size = new System.Drawing.Size(26, 20);
            this.txt7_21.TabIndex = 20;
            this.txt7_21.Tag = "DEFAULT";
            // 
            // txt7_6
            // 
            this.txt7_6.Location = new System.Drawing.Point(169, 6);
            this.txt7_6.Name = "txt7_6";
            this.txt7_6.Size = new System.Drawing.Size(26, 20);
            this.txt7_6.TabIndex = 5;
            this.txt7_6.Tag = "DEFAULT";
            // 
            // txt7_18
            // 
            this.txt7_18.Location = new System.Drawing.Point(105, 62);
            this.txt7_18.Name = "txt7_18";
            this.txt7_18.Size = new System.Drawing.Size(26, 20);
            this.txt7_18.TabIndex = 17;
            this.txt7_18.Tag = "DEFAULT";
            // 
            // txt7_25
            // 
            this.txt7_25.Location = new System.Drawing.Point(105, 90);
            this.txt7_25.Name = "txt7_25";
            this.txt7_25.Size = new System.Drawing.Size(26, 20);
            this.txt7_25.TabIndex = 24;
            this.txt7_25.Tag = "DEFAULT";
            // 
            // txt7_12
            // 
            this.txt7_12.Location = new System.Drawing.Point(137, 34);
            this.txt7_12.Name = "txt7_12";
            this.txt7_12.Size = new System.Drawing.Size(26, 20);
            this.txt7_12.TabIndex = 11;
            this.txt7_12.Tag = "DEFAULT";
            // 
            // txt7_11
            // 
            this.txt7_11.Location = new System.Drawing.Point(105, 34);
            this.txt7_11.Name = "txt7_11";
            this.txt7_11.Size = new System.Drawing.Size(26, 20);
            this.txt7_11.TabIndex = 10;
            this.txt7_11.Tag = "DEFAULT";
            // 
            // txt7_4
            // 
            this.txt7_4.Location = new System.Drawing.Point(105, 6);
            this.txt7_4.Name = "txt7_4";
            this.txt7_4.Size = new System.Drawing.Size(26, 20);
            this.txt7_4.TabIndex = 3;
            this.txt7_4.Tag = "DEFAULT";
            // 
            // txt7_19
            // 
            this.txt7_19.Location = new System.Drawing.Point(137, 62);
            this.txt7_19.Name = "txt7_19";
            this.txt7_19.Size = new System.Drawing.Size(26, 20);
            this.txt7_19.TabIndex = 18;
            this.txt7_19.Tag = "DEFAULT";
            // 
            // txt7_26
            // 
            this.txt7_26.Location = new System.Drawing.Point(137, 90);
            this.txt7_26.Name = "txt7_26";
            this.txt7_26.Size = new System.Drawing.Size(26, 20);
            this.txt7_26.TabIndex = 25;
            this.txt7_26.Tag = "DEFAULT";
            // 
            // txt7_32
            // 
            this.txt7_32.Location = new System.Drawing.Point(105, 118);
            this.txt7_32.Name = "txt7_32";
            this.txt7_32.Size = new System.Drawing.Size(26, 20);
            this.txt7_32.TabIndex = 31;
            this.txt7_32.Tag = "DEFAULT";
            // 
            // txt7_33
            // 
            this.txt7_33.Location = new System.Drawing.Point(137, 118);
            this.txt7_33.Name = "txt7_33";
            this.txt7_33.Size = new System.Drawing.Size(26, 20);
            this.txt7_33.TabIndex = 32;
            this.txt7_33.Tag = "DEFAULT";
            // 
            // txt7_39
            // 
            this.txt7_39.Location = new System.Drawing.Point(105, 146);
            this.txt7_39.Name = "txt7_39";
            this.txt7_39.Size = new System.Drawing.Size(26, 20);
            this.txt7_39.TabIndex = 38;
            this.txt7_39.Tag = "DEFAULT";
            // 
            // txt7_40
            // 
            this.txt7_40.Location = new System.Drawing.Point(137, 146);
            this.txt7_40.Name = "txt7_40";
            this.txt7_40.Size = new System.Drawing.Size(26, 20);
            this.txt7_40.TabIndex = 39;
            this.txt7_40.Tag = "DEFAULT";
            // 
            // txt7_46
            // 
            this.txt7_46.Location = new System.Drawing.Point(105, 174);
            this.txt7_46.Name = "txt7_46";
            this.txt7_46.Size = new System.Drawing.Size(26, 20);
            this.txt7_46.TabIndex = 45;
            this.txt7_46.Tag = "DEFAULT";
            // 
            // txt7_47
            // 
            this.txt7_47.Location = new System.Drawing.Point(137, 174);
            this.txt7_47.Name = "txt7_47";
            this.txt7_47.Size = new System.Drawing.Size(26, 20);
            this.txt7_47.TabIndex = 46;
            this.txt7_47.Tag = "DEFAULT";
            // 
            // txt7_5
            // 
            this.txt7_5.Location = new System.Drawing.Point(137, 6);
            this.txt7_5.Name = "txt7_5";
            this.txt7_5.Size = new System.Drawing.Size(26, 20);
            this.txt7_5.TabIndex = 4;
            this.txt7_5.Tag = "DEFAULT";
            // 
            // txt7_29
            // 
            this.txt7_29.Location = new System.Drawing.Point(9, 118);
            this.txt7_29.Name = "txt7_29";
            this.txt7_29.Size = new System.Drawing.Size(26, 20);
            this.txt7_29.TabIndex = 28;
            this.txt7_29.Tag = "DEFAULT";
            // 
            // txt7_45
            // 
            this.txt7_45.Location = new System.Drawing.Point(73, 174);
            this.txt7_45.Name = "txt7_45";
            this.txt7_45.Size = new System.Drawing.Size(26, 20);
            this.txt7_45.TabIndex = 44;
            this.txt7_45.Tag = "DEFAULT";
            // 
            // txt7_38
            // 
            this.txt7_38.Location = new System.Drawing.Point(73, 146);
            this.txt7_38.Name = "txt7_38";
            this.txt7_38.Size = new System.Drawing.Size(26, 20);
            this.txt7_38.TabIndex = 37;
            this.txt7_38.Tag = "DEFAULT";
            // 
            // txt7_44
            // 
            this.txt7_44.Location = new System.Drawing.Point(41, 174);
            this.txt7_44.Name = "txt7_44";
            this.txt7_44.Size = new System.Drawing.Size(26, 20);
            this.txt7_44.TabIndex = 43;
            this.txt7_44.Tag = "DEFAULT";
            // 
            // txt7_23
            // 
            this.txt7_23.Location = new System.Drawing.Point(41, 90);
            this.txt7_23.Name = "txt7_23";
            this.txt7_23.Size = new System.Drawing.Size(26, 20);
            this.txt7_23.TabIndex = 22;
            this.txt7_23.Tag = "DEFAULT";
            // 
            // txt7_22
            // 
            this.txt7_22.Location = new System.Drawing.Point(9, 90);
            this.txt7_22.Name = "txt7_22";
            this.txt7_22.Size = new System.Drawing.Size(26, 20);
            this.txt7_22.TabIndex = 21;
            this.txt7_22.Tag = "DEFAULT";
            // 
            // txt7_37
            // 
            this.txt7_37.Location = new System.Drawing.Point(41, 146);
            this.txt7_37.Name = "txt7_37";
            this.txt7_37.Size = new System.Drawing.Size(26, 20);
            this.txt7_37.TabIndex = 36;
            this.txt7_37.Tag = "DEFAULT";
            // 
            // txt7_36
            // 
            this.txt7_36.Location = new System.Drawing.Point(9, 146);
            this.txt7_36.Name = "txt7_36";
            this.txt7_36.Size = new System.Drawing.Size(26, 20);
            this.txt7_36.TabIndex = 35;
            this.txt7_36.Tag = "DEFAULT";
            // 
            // txt7_43
            // 
            this.txt7_43.Location = new System.Drawing.Point(9, 174);
            this.txt7_43.Name = "txt7_43";
            this.txt7_43.Size = new System.Drawing.Size(26, 20);
            this.txt7_43.TabIndex = 42;
            this.txt7_43.Tag = "DEFAULT";
            // 
            // txt7_24
            // 
            this.txt7_24.Location = new System.Drawing.Point(73, 90);
            this.txt7_24.Name = "txt7_24";
            this.txt7_24.Size = new System.Drawing.Size(26, 20);
            this.txt7_24.TabIndex = 23;
            this.txt7_24.Tag = "DEFAULT";
            // 
            // txt7_31
            // 
            this.txt7_31.Location = new System.Drawing.Point(73, 118);
            this.txt7_31.Name = "txt7_31";
            this.txt7_31.Size = new System.Drawing.Size(26, 20);
            this.txt7_31.TabIndex = 30;
            this.txt7_31.Tag = "DEFAULT";
            // 
            // txt7_30
            // 
            this.txt7_30.Location = new System.Drawing.Point(41, 118);
            this.txt7_30.Name = "txt7_30";
            this.txt7_30.Size = new System.Drawing.Size(26, 20);
            this.txt7_30.TabIndex = 29;
            this.txt7_30.Tag = "DEFAULT";
            // 
            // DlgCustomFilter
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(358, 284);
            this.Controls.Add(this.panelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgCustomFilter";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Set Filter";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.panel3_3.ResumeLayout(false);
            this.panel3_3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.panel5_5.ResumeLayout(false);
            this.panel5_5.PerformLayout();
            this.panel7_7.ResumeLayout(false);
            this.panel7_7.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion		

		#region public properties
		
		public ArrayList Matrix
		{
			get { return  m_Matrix; }			
		}

		public bool MatrixValidation
		{
			get	{ return matrixValidation;}
			set { matrixValidation = value;}
		}				
		public int[,] MatrixArr
		{
			get{ return mMatrixArr; }
			set{ mMatrixArr = MatrixArr; }
		}

		#endregion

		#region override rountines

        protected override object OnGetDefaultValue(Control ctrl)
        {
            return base.OnGetDefaultValue(ctrl);
        }
		
		#endregion

		#region event handlers
		
		private bool OnTextChange(object sender)
		{
			TextBox curentTextBox = ((TextBox) sender) ;
			try
			{				
				if ( curentTextBox.Text != "" )	
					if ( curentTextBox.Text.Length == 1 && curentTextBox.Text == "-" ) return true;
					else int.Parse( curentTextBox.Text );								
				return true;
			}
			catch
			{	
				curentTextBox.Focus();
				curentTextBox.Select( 0,curentTextBox.Text.Length );
				return false;
			}
		}
		private void OnTextBoxMatrixKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{			
			if ( OnTextChange(sender))
				if( e.KeyCode == Keys.Enter )
				{
					SendKeys.Send ("{Tab}");
				}
		}
		private void OnTextBoxValidated(object sender, System.EventArgs e)
		{
			OnTextChange(sender);
		}
		
		private void ClearTextBox()
		{
			switch ( currentPanel.Name )
			{				
				case "panel3_3" :						
					InitTextBox( arrTextBox );						
					break;
				case "panel5_5" :
					InitTextBox( arrTextBox5_5 );												
					break;
				case "panel7_7" :
					InitTextBox( arrTextBox7_7 );															
					break;
			}
		}

		private void buttonLoadFile_Click(object sender, System.EventArgs e)
		{
			using (OpenFileDialog openFile = CommonDialogs.OpenTextFileDialog("Select Filter File"))
			{
				if ( openFile.ShowDialog() == DialogResult.OK ) 
				{	
					textBoxFileName.Text = "";

					ClearTextBox();

					if (LoadCustomMatrix(openFile.FileName)) 
						textBoxFileName.Text = openFile.FileName;	
				}			
			}
		}
		
		private void buttonOK_Click(object sender, System.EventArgs e)
		{	
			matrixValidation = UpdateData();
			if ( !matrixValidation ) return;
			
			PersistenceDefault obj=new PersistenceDefault(this);
			obj.Save();
			this.DialogResult = DialogResult.OK; 						
		}		
		
		private void OnRadioCheckedChange(object sender, System.EventArgs e)
		{
			if ( !loaded || !((RadioButton)sender).Checked ) return;

			string  currentButtonName = ((RadioButton)sender).Name;
			
			switch ( currentButtonName )
			{
				case "radiobtn3_3" :
					ResizePanel( panel3_3 ,460,250 );
					break;
				case "radiobnt5_5" :
					ResizePanel( panel5_5 ,460,305 );						
					break;
				case "radiobnt7_7" :
					ResizePanel( panel7_7 ,460,366 );	
					break;
			}
		}
		
		private void panelMain_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			using (SaveFileDialog dlg = CommonDialogs.SaveTextFileDialog("Save Filter as..."))
			{
				if( dlg.ShowDialog() == DialogResult.OK )
				{
					if( !UpdateData() ) return;
					try
					{
						if (File.Exists( dlg.FileName ))
						{
							File.SetAttributes(dlg.FileName, FileAttributes.Normal );
						}
					}
					catch(Exception ex)
					{
						MessageBoxEx.Error(ex.Message);
						return;
					}

					try
					{
						SaveCustomMatrix(dlg.FileName);
						this.textBoxFileName.Text = dlg.FileName;
					}
					catch(System.Exception exp)
					{
						Trace.WriteLine(exp);
						MessageBoxEx.Error("Failed to save filter data");
					}
					finally
					{
					}
				}
			}
		}

		#endregion

		#region internal helpers

		private void SetMatrix( ArrayList arr , int count )	
		{			
			mMatrixArr = new int [count,count];
			int index =0;
			for( int i=0; i<count ;i++ )
			{
				for( int j=0; j<count;j++ )
				{					
					((TextBox)arr[index]).Focus();
					mMatrixArr[i,j] = int.Parse(((TextBox)arr[index]).Text);
					index += 1;
				}				
			}
		}

		private bool UpdateData()
		{
			try
			{				
				switch ( currentPanel.Name )
				{
					case "panel3_3" :						
						SetMatrix( arrTextBox , 3 );						
						break;
					case "panel5_5" :						
						SetMatrix( arrTextBox5_5 ,5);						
						break;
					case "panel7_7" :						
						SetMatrix( arrTextBox7_7 ,7);						
						break;
				}	
				return true;
			}
			catch (Exception ex)
			{					
				MessageBoxEx.Error(ex.Message);
				return false;
			}
		}
		
		
		private void AddEventsTextbox( Control m_control )
		{
			foreach ( Control control in m_control.Controls )
			{				
				if (  control.Name.Substring(0,3) == "txt" || control.Name.Substring(0,4) == "text" )
				{
					control.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnTextBoxMatrixKeyUp);
					control.Validated += new System.EventHandler(this.OnTextBoxValidated);
				}
			}			
		}

		private void InitTextBox( ArrayList arrTextBox )
		{
			for ( int i=0; i<arrTextBox.Count;i++) 
				((TextBox)arrTextBox[i]).Text = "0";
		}

		private void ResizePanel( Panel panel ,int width,int height )
		{
			ArrayList arrTextBoxes;
			/* disable other panels */
			if ( panel3_3 == panel) 
			{				
				arrTextBoxes = arrTextBox;
			}

			if ( panel5_5 == panel ) 
			{
				arrTextBoxes = arrTextBox5_5;
			}

			if ( panel7_7 == panel) 
			{
				arrTextBoxes = arrTextBox7_7;
			}

			currentPanel = panel;			
			currentPanel.BringToFront();
			currentPanel.Enabled = true;

			foreach(TextBox item in arrTextBox)
				item.Enabled = true;
		}


		private bool LoadCustomMatrix(string fileName)
		{
			if (File.Exists(fileName) == false)
				return false;

			ArrayList arrMatrix = new ArrayList ();		
			
			try
			{
				using (TextReader reader = new StreamReader(fileName))
				{
					char[] separator = new char[] {' '};
					String strline = String.Empty;
					while ((strline = reader.ReadLine()) != null)
					{
						if (strline == string.Empty)
							continue;

						try
						{
							string[] fragments = strline.Split(separator);
							foreach (string fragment in fragments)
							{
								int value = int.Parse(fragment);
								arrMatrix.Add(value);
							}
						}
						catch(System.Exception exp)
						{
							Trace.WriteLine(exp);
						}
					}
				}

				// initialize text box matrix's elements
				int countMatrix = arrMatrix.Count;

                int sz = (int)Math.Sqrt(Math.Abs(countMatrix));

                string matrix_type_name = "panel3_3";
                RadioButton matrix_type_controller = radiobtn3_3;

                switch (sz)
                {
                    case 3:
                        matrix_type_name = "panel3_3";
                        matrix_type_controller = radiobtn3_3;
                        break;
                    case 5:
                        matrix_type_name = "panel5_5";
                        matrix_type_controller = radiobnt5_5;
                        break;
                    case 7:
                        matrix_type_name = "panel7_7";
                        matrix_type_controller = radiobnt7_7;
                        break;
                }

				try
				{
                    matrix_type_controller.Checked = true;

					//switch ( currentPanel.Name )
                    switch (matrix_type_name)
					{
						case "panel3_3" :							
							for( int i=0; i<countMatrix; i++)
								(( TextBox )arrTextBox[i]).Text = arrMatrix[i].ToString();					
							break;
						case "panel5_5" :
							for( int i=0; i<countMatrix; i++)
								(( TextBox )arrTextBox5_5[i]).Text = arrMatrix[i].ToString();				
							break;
						case "panel7_7" :
							for( int i=0; i<countMatrix; i++)
								(( TextBox )arrTextBox7_7[i]).Text = arrMatrix[i].ToString();				
							break;
					}
				}
				catch (Exception ex)
				{
					System.Diagnostics.Trace.Write(ex);
				}		
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);

				MessageBoxEx.Error("SiGlaz Image Analyzer cannot read this file.\r\nThis is not a valid bitmap file, or its format is not currently supported.");
				return false;
			}
			finally
			{
			}

			return true;
		}

		private bool SaveCustomMatrix(string fileName)
		{
			bool bResult = false;

			try
			{
				if (File.Exists(fileName))
					File.SetAttributes(fileName, FileAttributes.Normal );
			}
			catch (Exception exp)
			{
				MessageBoxEx.Warning(exp.Message);	
				return bResult;
			}

			try
			{
				using (StreamWriter writer =  File.CreateText(fileName))
				{
					double index = 0;
					if (mMatrixArr != null) 
					{
						index = Math.Sqrt((double)mMatrixArr.Length );
						for( int i=0 ;i<index ;i++)
						{
							for( int j=0;j<index;j++ )
							{
								writer.Write(mMatrixArr[i,j].ToString() + " ");
							}
							writer.WriteLine("");
						}
					}
					writer.Close();
				}

				bResult = true;
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
				MessageBoxEx.Error("Failed to save custom filter file.");
			}
			finally
			{
			}

			return bResult;
		}
		#endregion

	}
}
