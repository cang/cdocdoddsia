using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using SAC = SiGlaz.Algorithms.Core;
using SiGlaz.Common;

namespace SiGlaz.ObjectAnalysis.UI
{
	/// <summary>
	/// Summary description for SPRulesWizardCtrl.
	/// </summary>
	public class SPRulesWizardCtrl : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Panel pTop;
		private System.Windows.Forms.Panel pClient;
		private System.Windows.Forms.Panel pClientTop;
		private System.Windows.Forms.Panel pTopTop;
		private System.Windows.Forms.Panel pClientClient;
		private System.Windows.Forms.Panel pTopClient;
		private System.Windows.Forms.Panel pData;
		private System.Windows.Forms.CheckedListBox checkedListBox1;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.LinkLabel llCheckAll;
		private System.Windows.Forms.LinkLabel llUnCheckAll;
		private System.Windows.Forms.LinkLabel llDFSLevel;
		private System.Windows.Forms.ToolTip tipMain;
		private System.ComponentModel.IContainer components;

		public event	EventHandler	OnDataChange;

		public SPRulesWizardCtrl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			_dfsLevelMenu = new ContextMenu(new MenuItem[]{
											  new MenuItem("Two", new System.EventHandler(this.dfsLevelMenuClick)),
											  new MenuItem("Three", new System.EventHandler(this.dfsLevelMenuClick))});
			foreach (MenuItem item in _dfsLevelMenu.MenuItems)
				item.RadioCheck = true;

			// TODO: Add any initialization after the InitializeComponent call
			InitData();

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if (alConditionCtrls != null)
			{
				foreach (ArrayList cctrls in alConditionCtrls)
				{
					if (cctrls != null)
						cctrls.Clear();					
				}
				alConditionCtrls.Clear();
				alConditionCtrls = null;
			}
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.pTop = new System.Windows.Forms.Panel();
            this.pTopClient = new System.Windows.Forms.Panel();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.pTopTop = new System.Windows.Forms.Panel();
            this.llUnCheckAll = new System.Windows.Forms.LinkLabel();
            this.llCheckAll = new System.Windows.Forms.LinkLabel();
            this.pClient = new System.Windows.Forms.Panel();
            this.pClientClient = new System.Windows.Forms.Panel();
            this.pData = new System.Windows.Forms.Panel();
            this.llDFSLevel = new System.Windows.Forms.LinkLabel();
            this.pClientTop = new System.Windows.Forms.Panel();
            this.tipMain = new System.Windows.Forms.ToolTip(this.components);
            this.pTop.SuspendLayout();
            this.pTopClient.SuspendLayout();
            this.pTopTop.SuspendLayout();
            this.pClient.SuspendLayout();
            this.pClientClient.SuspendLayout();
            this.pData.SuspendLayout();
            this.pClientTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(207, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Which condition(s) do you want to check?";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Step 1: Select condition(s)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(230, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Step 2: Edit the rule description (click hyperlink)";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(8, 8);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(346, 13);
            this.lblDescription.TabIndex = 4;
            this.lblDescription.Text = "           primitive objects are included in the same object if between them:";
            // 
            // pTop
            // 
            this.pTop.Controls.Add(this.pTopClient);
            this.pTop.Controls.Add(this.pTopTop);
            this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTop.Location = new System.Drawing.Point(0, 0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(400, 208);
            this.pTop.TabIndex = 5;
            // 
            // pTopClient
            // 
            this.pTopClient.Controls.Add(this.checkedListBox1);
            this.pTopClient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pTopClient.Location = new System.Drawing.Point(0, 40);
            this.pTopClient.Name = "pTopClient";
            this.pTopClient.Size = new System.Drawing.Size(400, 168);
            this.pTopClient.TabIndex = 3;
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.checkedListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox1.Location = new System.Drawing.Point(0, 0);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(400, 167);
            this.checkedListBox1.TabIndex = 0;
            this.checkedListBox1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox1_ItemCheck);
            // 
            // pTopTop
            // 
            this.pTopTop.Controls.Add(this.llUnCheckAll);
            this.pTopTop.Controls.Add(this.llCheckAll);
            this.pTopTop.Controls.Add(this.label1);
            this.pTopTop.Controls.Add(this.label2);
            this.pTopTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTopTop.Location = new System.Drawing.Point(0, 0);
            this.pTopTop.Name = "pTopTop";
            this.pTopTop.Size = new System.Drawing.Size(400, 40);
            this.pTopTop.TabIndex = 2;
            // 
            // llUnCheckAll
            // 
            this.llUnCheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llUnCheckAll.AutoSize = true;
            this.llUnCheckAll.Location = new System.Drawing.Point(328, 24);
            this.llUnCheckAll.Name = "llUnCheckAll";
            this.llUnCheckAll.Size = new System.Drawing.Size(66, 13);
            this.llUnCheckAll.TabIndex = 3;
            this.llUnCheckAll.TabStop = true;
            this.llUnCheckAll.Text = "UnCheck All";
            this.tipMain.SetToolTip(this.llUnCheckAll, "Click to deselect all conditions");
            this.llUnCheckAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llUnCheckAll_LinkClicked);
            // 
            // llCheckAll
            // 
            this.llCheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llCheckAll.AutoSize = true;
            this.llCheckAll.Location = new System.Drawing.Point(264, 24);
            this.llCheckAll.Name = "llCheckAll";
            this.llCheckAll.Size = new System.Drawing.Size(52, 13);
            this.llCheckAll.TabIndex = 2;
            this.llCheckAll.TabStop = true;
            this.llCheckAll.Text = "Check All";
            this.tipMain.SetToolTip(this.llCheckAll, "Click to select all conditions");
            this.llCheckAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llCheckAll_LinkClicked);
            // 
            // pClient
            // 
            this.pClient.Controls.Add(this.pClientClient);
            this.pClient.Controls.Add(this.pClientTop);
            this.pClient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pClient.Location = new System.Drawing.Point(0, 208);
            this.pClient.Name = "pClient";
            this.pClient.Size = new System.Drawing.Size(400, 120);
            this.pClient.TabIndex = 6;
            // 
            // pClientClient
            // 
            this.pClientClient.AutoScroll = true;
            this.pClientClient.BackColor = System.Drawing.Color.White;
            this.pClientClient.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pClientClient.Controls.Add(this.pData);
            this.pClientClient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pClientClient.Location = new System.Drawing.Point(0, 16);
            this.pClientClient.Name = "pClientClient";
            this.pClientClient.Size = new System.Drawing.Size(400, 104);
            this.pClientClient.TabIndex = 6;
            // 
            // pData
            // 
            this.pData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pData.BackColor = System.Drawing.Color.White;
            this.pData.Controls.Add(this.llDFSLevel);
            this.pData.Controls.Add(this.lblDescription);
            this.pData.Location = new System.Drawing.Point(0, 0);
            this.pData.Name = "pData";
            this.pData.Size = new System.Drawing.Size(392, 72);
            this.pData.TabIndex = 0;
            // 
            // llDFSLevel
            // 
            this.llDFSLevel.AutoSize = true;
            this.llDFSLevel.Location = new System.Drawing.Point(0, 8);
            this.llDFSLevel.Name = "llDFSLevel";
            this.llDFSLevel.Size = new System.Drawing.Size(28, 13);
            this.llDFSLevel.TabIndex = 2;
            this.llDFSLevel.TabStop = true;
            this.llDFSLevel.Text = "Two";
            this.tipMain.SetToolTip(this.llDFSLevel, "Click to select the level of combining primitive object");
            this.llDFSLevel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llDFSLevel_LinkClicked);
            // 
            // pClientTop
            // 
            this.pClientTop.Controls.Add(this.label3);
            this.pClientTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pClientTop.Location = new System.Drawing.Point(0, 0);
            this.pClientTop.Name = "pClientTop";
            this.pClientTop.Size = new System.Drawing.Size(400, 16);
            this.pClientTop.TabIndex = 5;
            // 
            // SPRulesWizardCtrl
            // 
            this.Controls.Add(this.pClient);
            this.Controls.Add(this.pTop);
            this.Name = "SPRulesWizardCtrl";
            this.Size = new System.Drawing.Size(400, 328);
            this.pTop.ResumeLayout(false);
            this.pTopClient.ResumeLayout(false);
            this.pTopTop.ResumeLayout(false);
            this.pTopTop.PerformLayout();
            this.pClient.ResumeLayout(false);
            this.pClientClient.ResumeLayout(false);
            this.pData.ResumeLayout(false);
            this.pData.PerformLayout();
            this.pClientTop.ResumeLayout(false);
            this.pClientTop.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private	ArrayList	alCondition=new ArrayList();//MDCCParam.CONDITION
		private ArrayList alConditionCtrls = new ArrayList();
		private ContextMenu _dfsLevelMenu = null;
		string slink="Criterion";
		private ArrayList GetConditionCtrls(MDCCParam.CONDITION con)
		{
			if (alCondition == null || con == null || alConditionCtrls == null) return null;
			int indx = alCondition.IndexOf(con);
			if (indx < 0 || indx >= alConditionCtrls.Count) return null;
			return (ArrayList)alConditionCtrls[indx];
		}
		private int ArrangeConditionCtrl(ArrayList ConditionCtrls, ref int X0, ref int Y0)
		{
			if (ConditionCtrls == null || ConditionCtrls.Count == 0) return -1;
			int result = 0;
			X0 = (ConditionCtrls[0] as Control).Location.X;
			Y0 = (ConditionCtrls[0] as Control).Location.Y;
			foreach (Control ctrl in ConditionCtrls)
			{
				if (ctrl == null) continue;
				ctrl.Left = result + X0;
				ctrl.Top = Y0;
				result += ctrl.Width;
			}
			return result + 5;
		}
		private int ArrangeConditionCtrl(ArrayList ConditionCtrls)
		{
			int X0 = 0, Y0 = 0;
			return ArrangeConditionCtrl(ConditionCtrls,ref X0, ref Y0);
		}
		private void checkedListBox1_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			if(skipcheck) return;

			#region Add To Condition Des
			if(e.NewValue==CheckState.Checked)
			{
				int ikey= e.Index;
				ArrayList ConditionCtrls = new ArrayList(5);
				MDCCParam.CONDITION con=new MDCCParam.CONDITION((MDCCParam.LHS_KEYS)ikey,COMPARE_OPERATOR.NONE,0f);

				string shuman="the " +  MDCCParam._humankeylist[ikey,0];
				if(cprOptionMode!=COMPARE_OPERATOR.NONE)//new
					shuman+=QueryOperator.HumanString(cprOptionMode);

				string sand=alCondition.Count>0?"- and ":"- ";

				Point pos=lblDescription.Location;
				pos.X+=5;
				pos.Y+= lblDescription.Size.Height;
				pos.Y+= lblDescription.Size.Height*alCondition.Count;

				Label lbl1=new Label();
				lbl1.AutoSize=true;
				lbl1.Location=pos;
				lbl1.Text=sand + shuman;
				lbl1.Tag=con;
				pData.Controls.Add(lbl1);
				ConditionCtrls.Add(lbl1);

				LinkLabel link1=new LinkLabel();
				link1.AutoSize=true;
				//link1.Location=new Point(lbl1.Left + lbl1.Width + 5,pos.Y);
				link1.Text=slink;
				link1.Tag=con;
				link1.LinkClicked+=new LinkLabelLinkClickedEventHandler(link1_LinkClicked);
				pData.Controls.Add(link1);
				link1.BringToFront();
				ConditionCtrls.Add(link1);

				Label lblUnit = new Label();
				lblUnit.AutoSize=true;
				//lblUnit.Location=new Point(link1.Left + link1.Width + 5, pos.Y);
				lblUnit.Text=MDCCParam._humankeylist[ikey,1];
				lblUnit.Tag=con;
					pData.Controls.Add(lblUnit);
				ConditionCtrls.Add(lblUnit);


				pData.Height= pos.Y + 30;
				//pData.Width=link1.Left + link1.Width + 10;

				int w = ArrangeConditionCtrl(ConditionCtrls);
				if(pData.Width<w) pData.Width= w;			
				alCondition.Add(con);
				alConditionCtrls.Add(ConditionCtrls);

			}
				#endregion Add To Condition Des

			#region Remove To Condition Des
			else if(e.NewValue==CheckState.Unchecked)
			{
				int ikey= e.Index;
				MDCCParam.CONDITION con=SearchCondition(ikey);
				if( con.LHS>=MDCCParam.LHS_KEYS.NONE) return;
				int index=alCondition.IndexOf(con);
				if(index<0) return;
				
				LinkLabel link1=SearchLinkLabel(ikey);
				link1.LinkClicked-=new LinkLabelLinkClickedEventHandler(link1_LinkClicked);

				//				Label lbl1=SearchLabel(ikey);
				//				pData.Controls.Remove(lbl1);lbl1=null;
				//				pData.Controls.Remove(link1);link1=null;
				//Phong 20061125
				ArrayList ConditionCtrls =(ArrayList)((alConditionCtrls == null || alConditionCtrls.Count <= index)?null:alConditionCtrls[index]);
				if (ConditionCtrls != null)
				{
					foreach (Control  ctrl in ConditionCtrls)
						pData.Controls.Remove(ctrl);
					ConditionCtrls.Clear();
				}

				for(int i=index+1;i<alCondition.Count;i++)
				{
					foreach(Control ctrl in pData.Controls)
					{
						if( ctrl.Tag==null) continue;
						if( ((MDCCParam.CONDITION)ctrl.Tag).LHS==((MDCCParam.CONDITION)alCondition[i]).LHS)
						{
							ctrl.Location=new Point(ctrl.Location.X,ctrl.Location.Y-lblDescription.Height);
						}
					}
				}

				alCondition.Remove(con);
				alConditionCtrls.Remove(ConditionCtrls);

				pData.Height-=lblDescription.Height;
			}

			#endregion Remove To Condition Des

			if( OnDataChange!=null)
				OnDataChange(null,EventArgs.Empty);
		}

		public MDCCParam.CONDITION SearchCondition(int ikey)
		{
			foreach(MDCCParam.CONDITION con in alCondition)
			{
				if( (int)con.LHS==ikey)
					return con;
			}
			return new MDCCParam.CONDITION(MDCCParam.LHS_KEYS.NONE,COMPARE_OPERATOR.NONE,0f);
		}

		public Label SearchLabel(int ikey)
		{
			foreach(Control ctrl in pData.Controls)
			{
				if( ctrl.Tag==null) continue;
				if( ctrl.GetType()!= typeof(Label)) continue;
				MDCCParam.CONDITION con=(MDCCParam.CONDITION)ctrl.Tag;
				if( (int)con.LHS==ikey)
					return (Label)ctrl;
			}
			return null;
		}

		public LinkLabel SearchLinkLabel(int ikey)
		{
			foreach(Control ctrl in pData.Controls)
			{
				if( ctrl.Tag==null) continue;
				if( ctrl.GetType()!= typeof(LinkLabel)) continue;
				MDCCParam.CONDITION con=(MDCCParam.CONDITION)ctrl.Tag;
				if( (int)con.LHS==ikey)
					return (LinkLabel)ctrl;
			}
			return null;
		}

		public void InitData()
		{
			//MDCCParam.CONDITION
			//MDCCParam._humankeylist
			//MDCCParam.LHS_KEYS;
			checkedListBox1.Items.Clear();			
			int iLenght=MDCCParam._humankeylist.GetLength(0);//12;
			for(int i=0;i<iLenght;i++)
			{
				string shuman=MDCCParam._humankeylist[i,0];
				checkedListBox1.Items.Add(shuman);
			}
		}

		private void link1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			LinkLabel linkObj=sender as LinkLabel;
			MDCCParam.CONDITION con=(MDCCParam.CONDITION)linkObj.Tag;

			if( cprOptionMode==COMPARE_OPERATOR.NONE 
				|| (con.Operator!=COMPARE_OPERATOR.NONE && con.Operator!=cprOptionMode)
				)
			{
				SPCriterionDlg dlg=new SPCriterionDlg(con);
				dlg.Text= MDCCParam._humankeylist[(int)con.LHS,0];
				if( dlg.ShowDialog(this)==DialogResult.OK)
				{
					linkObj.Text =  QueryOperator.HumanString(con.Operator) +  " " + con.RHS.ToString();
					ArrayList ConditionCtrls = GetConditionCtrls(con);
					if (ConditionCtrls == null || ConditionCtrls.Count == 0) return;
					int w = ArrangeConditionCtrl(ConditionCtrls);
					if(pData.Width<w) pData.Width= w;
					if( OnDataChange!=null)
						OnDataChange(null,EventArgs.Empty);
				}
			}
			else
			{
				SPValueDlg dlg=new SPValueDlg(con,cprOptionMode);
				if( dlg.ShowDialog(this)==DialogResult.OK)
				{
					linkObj.Text = con.RHS.ToString();
					con.Operator=cprOptionMode;
					ArrayList ConditionCtrls = GetConditionCtrls(con);
					if (ConditionCtrls == null || ConditionCtrls.Count == 0) return;
					int w = ArrangeConditionCtrl(ConditionCtrls);
					if(pData.Width<w) pData.Width= w;			
					if( OnDataChange!=null)
						OnDataChange(null,EventArgs.Empty);
				}
			}
		}

		#region Properties

		public string DescriptionStep1
		{
			get
			{
				return label2.Text;
			}
			set
			{
				label2.Text=value;
			}
		}

		public string DescriptionStep2
		{
			get
			{
				return label3.Text;
			}
			set
			{
				label3.Text=value;
			}
		}


		public string DescriptionHead
		{
			get
			{
				return label1.Text;
			}
			set
			{
				label1.Text=value;
			}
		}

		public string DescriptionDetail
		{
			get
			{
				return lblDescription.Text;
			}
			set
			{
				lblDescription.Text=value;
			}
		}

		public int HeaderSize
		{
			get
			{
				return pTop.Height;
			}
			set
			{
				pTop.Height=value;
			}
		}

		public string DescriptionLink
		{
			get
			{
				return slink;
			}
			set
			{
				slink=value;
			}
		}

		#endregion Properties


		public void ClearCondition()
		{
			for(int i=0;i<checkedListBox1.Items.Count;i++)
			{
				if( checkedListBox1.GetItemChecked(i) )
					checkedListBox1.SetItemChecked(i,false);
			}
			if(alCondition!=null)
				alCondition.Clear();
		}


		bool skipcheck=false;

		private void llCheckAll_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			for(int i=0;i<checkedListBox1.Items.Count;i++)
			{
				if( !checkedListBox1.GetItemChecked(i) )
					checkedListBox1.SetItemChecked(i,true);
			}
		}

		private void llUnCheckAll_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			ClearCondition();
		}

		#region DFS Level routines
		private int _dfsLevel = 2;
		public int DFSLevel
		{
			get
			{
				return _dfsLevel;
			}
			set
			{
				_dfsLevel = value;
				if (llDFSLevel != null)
				{
					switch (value)
					{
						case 2:
							llDFSLevel.Text = "Two";
							break;
						case 3:
							llDFSLevel.Text = "Three";
							break;
					}

				}
			}
		}
		private void dfsLevelMenuClick(object sender, System.EventArgs args)
		{
			if ((sender as MenuItem).Text == "Two")
				DFSLevel = 2;
			else if ((sender as MenuItem).Text == "Three")
				DFSLevel = 3;
		}
		private void llDFSLevel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			foreach (MenuItem item in _dfsLevelMenu.MenuItems)
				item.Checked = false;
			switch (DFSLevel)
			{
				case 2:
					_dfsLevelMenu.MenuItems[0].Checked = true;
					break;
				case 3:
					_dfsLevelMenu.MenuItems[1].Checked = true;
					break;
			}
			_dfsLevelMenu.Show((sender as Control), Point.Empty);
		}
		#endregion

		public	ArrayList Condition
		{
			get
			{
				return alCondition;
			}
			set
			{
				if(alCondition!=null)
					alCondition=(ArrayList)alCondition.Clone();
				ClearCondition();
				if(value!=null)
				{
					foreach(MDCCParam.CONDITION con in value)
					{
						int ikey= (int)con.LHS;

						skipcheck=true;
						if (ikey >= checkedListBox1.Items.Count)
							continue;
						checkedListBox1.SetItemChecked(ikey,true);
						skipcheck=false;

						string shuman= "the " + MDCCParam._humankeylist[ikey,0];
						if(cprOptionMode!=COMPARE_OPERATOR.NONE && con.Operator==cprOptionMode )//new
							shuman+=QueryOperator.HumanString(cprOptionMode);

						string sand=alCondition.Count>0?"- and ":"- ";

						ArrayList ConditionCtrls = new ArrayList(5);

						Point pos=lblDescription.Location;
						pos.X+=5;
						pos.Y+= lblDescription.Size.Height;
						pos.Y+= lblDescription.Size.Height*alCondition.Count;

						Label lbl1=new Label();
						lbl1.AutoSize=true;
						lbl1.Location=pos;
						lbl1.Text=sand + shuman;
						lbl1.Tag=con;
						pData.Controls.Add(lbl1);
						ConditionCtrls.Add(lbl1);

						LinkLabel link1=new LinkLabel();
						link1.AutoSize=true;
						//link1.Location=new Point(pos.X+ lbl1.Width-10,pos.Y);

						if( con.Operator==COMPARE_OPERATOR.NONE)
							link1.Text= slink;
						else
						{
							if(cprOptionMode!=COMPARE_OPERATOR.NONE && con.Operator==cprOptionMode )//New
								link1.Text =  con.RHS.ToString();
							else
								link1.Text =  QueryOperator.HumanString(con.Operator) +  " " + con.RHS.ToString();
						}

						link1.Tag=con;
						link1.LinkClicked+=new LinkLabelLinkClickedEventHandler(link1_LinkClicked);
						pData.Controls.Add(link1);
						link1.BringToFront();
						ConditionCtrls.Add(link1);

						Label lblUnit = new Label();
						lblUnit.AutoSize=true;
						//lblUnit.Location=new Point(link1.Left + link1.Width + 5, pos.Y);
						lblUnit.Text=MDCCParam._humankeylist[ikey,1];
						lblUnit.Tag=con;
						pData.Controls.Add(lblUnit);
						ConditionCtrls.Add(lblUnit);

						pData.Height= pos.Y + 30;

						int w = ArrangeConditionCtrl(ConditionCtrls)+20;
						if(pData.Width<w) pData.Width= w;	
						alCondition.Add(con);
						alConditionCtrls.Add(ConditionCtrls);
					}
				}
				if( OnDataChange!=null)
					OnDataChange(null,EventArgs.Empty);
			}
		}

		public  bool ValidCondition
		{
			get
			{
				foreach(MDCCParam.CONDITION con in alCondition)
				{
					if( con.Operator==COMPARE_OPERATOR.NONE)
						return false;
				}
				return true;
			}
		}

		public string ValidMessage
		{
			get
			{
				return "A value in the \'rule description\' box has not been set.\nTo set a value, click hyperlink words in the rule description area.";
			}
		}

		public string sValidateMessage=string.Empty;
		public bool IsValidate
		{
			get
			{
				if(alCondition==null ||  alCondition.Count <=0) 
				{
					sValidateMessage="Combine Primitive Objects : Condition cannot be empty. Please set at least one condition.";
					return false;
				}
				else if(!ValidCondition)
				{
					sValidateMessage="Combine Primitive Objects : A value in the \'rule description\' box has not been set.\nTo set a value, click hyperlink words in the rule description area.";
					return false;
				}
				else
				{
					sValidateMessage=string.Empty;
					return true;
				}
			}
		}


		COMPARE_OPERATOR cprOptionMode=COMPARE_OPERATOR.NONE;

		/// <summary>
		/// COMPARE_OPERATOR.NONE : use many condition
		/// Others : use one condition
		/// </summary>
		public COMPARE_OPERATOR	OptionMode
		{
			get
			{
				return cprOptionMode;
			}
			set
			{
				cprOptionMode=value;
				if( cprOptionMode!=COMPARE_OPERATOR.NONE)
					slink="Value";
			}
		}

	}
}
