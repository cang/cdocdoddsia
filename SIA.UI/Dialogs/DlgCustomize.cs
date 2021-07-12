using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace SIA.UI
{
	/// <summary>
	/// Summary description for DlgCustomize.
	/// </summary>
	public class DlgCustomize : SIA.UI.Controls.Dialogs.DialogBase
	{
		#region Windows Form member attributes

		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Button button_Cancel;
		private System.Windows.Forms.Button button_OK;
		private System.Windows.Forms.Label lblMenu;
		private System.Windows.Forms.TextBox txtShortcut;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Button button2;
		private System.ComponentModel.IContainer components;

		#endregion

		#region constructor and destructor
		#endregion

		public DlgCustomize()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		public DlgCustomize(MainMenu mnu)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			cmdMenu = mnu;
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DlgCustomize));
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.button_Cancel = new System.Windows.Forms.Button();
			this.button_OK = new System.Windows.Forms.Button();
			this.lblMenu = new System.Windows.Forms.Label();
			this.txtShortcut = new System.Windows.Forms.TextBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.button2 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// treeView1
			// 
			this.treeView1.ImageIndex = -1;
			this.treeView1.Location = new System.Drawing.Point(4, 52);
			this.treeView1.Name = "treeView1";
			this.treeView1.SelectedImageIndex = -1;
			this.treeView1.Size = new System.Drawing.Size(408, 228);
			this.treeView1.TabIndex = 3;
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 284);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(256, 20);
			this.label1.TabIndex = 4;
			this.label1.Text = "Shortcut(s) for selected command:";
			// 
			// groupBox1
			// 
			this.groupBox1.Location = new System.Drawing.Point(64, 7);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(368, 8);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(4, 4);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.TabIndex = 0;
			this.label2.Text = "Commands";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(260, 304);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(72, 24);
			this.button1.TabIndex = 6;
			this.button1.Text = "&Assign";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox3.Location = new System.Drawing.Point(-11, 332);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(440, 4);
			this.groupBox3.TabIndex = 8;
			this.groupBox3.TabStop = false;
			// 
			// button_Cancel
			// 
			this.button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button_Cancel.Location = new System.Drawing.Point(214, 340);
			this.button_Cancel.Name = "button_Cancel";
			this.button_Cancel.TabIndex = 10;
			this.button_Cancel.Text = "Cancel";
			// 
			// button_OK
			// 
			this.button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button_OK.Location = new System.Drawing.Point(130, 340);
			this.button_OK.Name = "button_OK";
			this.button_OK.TabIndex = 9;
			this.button_OK.Text = "OK";
			this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
			// 
			// lblMenu
			// 
			this.lblMenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblMenu.Location = new System.Drawing.Point(4, 24);
			this.lblMenu.Name = "lblMenu";
			this.lblMenu.Size = new System.Drawing.Size(412, 23);
			this.lblMenu.TabIndex = 2;
			this.lblMenu.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtShortcut
			// 
			this.txtShortcut.Location = new System.Drawing.Point(8, 308);
			this.txtShortcut.Name = "txtShortcut";
			this.txtShortcut.Size = new System.Drawing.Size(248, 20);
			this.txtShortcut.TabIndex = 5;
			this.txtShortcut.Text = "";
			this.toolTip1.SetToolTip(this.txtShortcut, "Press shortcut key(s) and press Assign");
			this.txtShortcut.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtShortcut_KeyDown);
			this.txtShortcut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtShortcut_KeyPress);
			this.txtShortcut.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtShortcut_KeyUp);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(340, 304);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(72, 24);
			this.button2.TabIndex = 7;
			this.button2.Text = "&None";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// DlgCustomize
			// 
			this.AcceptButton = this.button_OK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.button_Cancel;
			this.ClientSize = new System.Drawing.Size(418, 368);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.txtShortcut);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lblMenu);
			this.Controls.Add(this.button_Cancel);
			this.Controls.Add(this.button_OK);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.treeView1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DlgCustomize";
			this.Text = "Customize";
			this.Load += new System.EventHandler(this.CustomizeFunction_Load);
			this.ResumeLayout(false);

		}
		#endregion

		public MainMenu	cmdMenu;
		public	class MyTract
		{
			public MenuItem		item;
			public Shortcut		shortcut;
		}

		private void CustomizeFunction_Load(object sender, System.EventArgs e)
		{
			if(cmdMenu!=null)
			{
				foreach(MenuItem mi in cmdMenu.MenuItems)
				{
					if( mi.Text=="-") continue;
					if( !mi.Visible) continue;

					TreeNode tn=treeView1.Nodes.Add(mi.Text);

					MyTract t=new MyTract();
					t.item=mi;
					t.shortcut=mi.Shortcut;
					tn.Tag=t;

					ConstructTree(ref tn,mi);
				}
			}
		}

		private void ConstructTree(ref TreeNode node,Menu mn)
		{
			if(node==null)		return;
			if( mn==null)		return;
			if(mn.MenuItems.Count<=0) return;
			if( mn.GetType() !=typeof(NiceMenu) ) return;

			foreach( MenuItem mi in ((NiceMenu)mn).MenuItems)
			{
				if( mi.Text=="-") continue;
				if( !mi.Visible) continue;
				if( mi.GetType()!=typeof(NiceMenu)) continue;

				//Check Recent files
				if( mi.Text == "Recent Files" ) continue;

				TreeNode treeitem=node.Nodes.Add(mi.Text);
				MyTract t=new MyTract();
				t.item=mi;
				t.shortcut=mi.Shortcut;
				treeitem.Tag=t;

				ConstructTree(ref treeitem,mi);
			}
		}

		private void treeView1_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if(treeView1.SelectedNode==null) return;
			lblMenu.Text=treeView1.SelectedNode.FullPath;

			if(treeView1.SelectedNode.Tag==null) return;
			txtShortcut.Text=  ((MyTract)treeView1.SelectedNode.Tag).shortcut.ToString();

			if( treeView1.SelectedNode.Nodes.Count >0) 
			{
				txtShortcut.Enabled=false;
				button1.Enabled=false;
				button2.Enabled=false;
			}
			else
			{
				txtShortcut.Enabled=true;
				button1.Enabled=true;
				button2.Enabled=true;
			}

		}

		private void mi_Click(object sender, EventArgs e)
		{

		}

		private void txtShortcut_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			e.Handled=true;
		}


		private bool Ctrl;
		private bool Alt;
		private bool Shift;

		private void txtShortcut_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			e.Handled=true;
			if( e.Shift) 
			{
				Shift=e.Shift;
			}
			if( e.Control )
			{
				Ctrl=e.Control;
			}
			if( e.Alt )
			{
				Alt=e.Alt;
			}

			string alphanumber="";
			if( 
				(e.KeyCode >= Keys.A  && e.KeyCode <=Keys.Z ) 
				||
				(e.KeyCode >= Keys.D0  && e.KeyCode <=Keys.D9  ) 
				)
			{
				alphanumber=(Convert.ToChar(e.KeyValue)).ToString();
			}
			if( alphanumber=="")
			{
				alphanumber=e.KeyData.ToString();
				int pos=alphanumber.IndexOf(",");
				if( pos>0)
					alphanumber=alphanumber.Substring(0,pos);
			}

			if(alphanumber!="Space" && alphanumber.Length>3)
				alphanumber=alphanumber.Substring(0,3);

			if(alphanumber=="Space")
				alphanumber="Bksp";

			if( Ctrl && Shift )
			{
				txtShortcut.Text="CtrlShift" + alphanumber;	
				Ctrl=Shift=Alt=false;
				return;
			}
			else if( Ctrl )
			{
				txtShortcut.Text="Ctrl" + alphanumber;	
				Ctrl=Shift=Alt=false;
			}
			else if( Shift )
			{
				txtShortcut.Text="Shift" + alphanumber;	
				Ctrl=Shift=Alt=false;
				return;
			}
			else if( Alt )
			{
				txtShortcut.Text="Alt" + alphanumber;	
				Ctrl=Shift=Alt=false;
				return;
			}
			else if(	!Ctrl && !Alt && !Shift)
				txtShortcut.Text=alphanumber;
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			if( treeView1.SelectedNode==null)	
			{
				MessageBox.Show("Please select function.","SiGlaz Image Analyzer");
				return;
			}

			Shortcut sc;
			try
			{
				sc= (Shortcut) (Enum.Parse( typeof(Shortcut), txtShortcut.Text.Trim()));
			}
			catch
			{
				MessageBox.Show("This shortcut does not exist.","SiGlaz Image Analyzer");
				sc=Shortcut.None;
			}

			((MyTract)(treeView1.SelectedNode.Tag)).shortcut=sc;
			txtShortcut.Text=sc.ToString();

			MessageBox.Show("Assign successfully. Shortcut of " + lblMenu.Text + " is " + txtShortcut.Text,"SiGlaz Image Analyzer");
		}

		private void txtShortcut_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			e.Handled=true;
		}

		private void button_OK_Click(object sender, System.EventArgs e)
		{
			AssignAllMenu();
			Save2Disk(Application.StartupPath + "\\shortcutmenu.dat");
		}

		private void Save2Disk(string fp)
		{
			//Save to Disk
			FileStream fs=new FileStream(fp,FileMode.Create,FileAccess.Write,FileShare.ReadWrite);
			BinaryWriter bw=new BinaryWriter(fs);

			foreach( TreeNode node in treeView1.Nodes)
				SaveNode(node,bw);
			bw.Close();
			fs.Close();
		}

		private void SaveNode(TreeNode node,BinaryWriter bw)
		{
			if( node==null) return;
			if( node.Tag==null) return;
			bw.Write( ((MyTract)node.Tag).shortcut.ToString()   );
			foreach(TreeNode sn in node.Nodes)
				SaveNode(sn,bw);
		}

		private void AssignAllMenu()
		{
            foreach( TreeNode node in treeView1.Nodes)
				AssignNode(node);
		}

		private void AssignNode(TreeNode node)
		{
			if( node==null) return;
			if( node.Tag==null) return;
			if( ((MyTract)node.Tag).item.Shortcut!=((MyTract)node.Tag).shortcut)
			{
				((MyTract)node.Tag).item.Shortcut=((MyTract)node.Tag).shortcut;
				if( ((MyTract)node.Tag).item.Parent!=cmdMenu)
				{
					//cause Meature Menu Item
					((MyTract)node.Tag).item.Visible=false;
					((MyTract)node.Tag).item.Visible =true;
				}
			}
			foreach(TreeNode sn in node.Nodes)
				AssignNode(sn);
		}
		

		public static void	LoadShotcutMenu(MainMenu cmdMenu,string fn)
		{
			if( fn=="") fn=Application.StartupPath + "\\shortcutmenu.dat";

			if( !File.Exists(fn) ) return;

			FileStream fs = new FileStream(fn,FileMode.Open,FileAccess.Read,FileShare.ReadWrite);
			BinaryReader br = new BinaryReader(fs);

			foreach(MenuItem mi in cmdMenu.MenuItems)
				Load4Menu(mi,br);

			br.Close();
			fs.Close();
		}

		private static void Load4Menu(MenuItem item,BinaryReader br)
		{
			if( item==null) return;
			if( item.Text=="-") return;
			if( !item.Visible) return;
			if( item.GetType()!=typeof(NiceMenu)) return;

			//Check Recent files
			if( item.Text == "Recent Files" ) return;

			try
			{
				item.Shortcut= (Shortcut)Enum.Parse(typeof(Shortcut),br.ReadString());
				foreach(MenuItem mi in item.MenuItems)
					Load4Menu(mi,br);
			}
			catch
			{
				return;
			}
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			if( treeView1.SelectedNode==null)	
			{
				MessageBox.Show("Please select function.","SiGlaz Image Analyzer");
				return;
			}
			((MyTract)(treeView1.SelectedNode.Tag)).shortcut=Shortcut.None;
			txtShortcut.Text=Shortcut.None.ToString();

			MessageBox.Show("Assign successfully. Shortcut of " + lblMenu.Text + " is " + txtShortcut.Text,"SiGlaz Image Analyzer");
		}

	}
}
