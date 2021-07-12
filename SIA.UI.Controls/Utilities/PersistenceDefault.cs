/*
 * update 2004-07-27 by cang 
 * 
 * */

using System;
using System.Windows.Forms;
using System.Xml;
using System.Data;
using System.IO;

/*
 * HOW TO USE
 * ----------
 	1. On load form call
  			PersistenceDefault obj=new PersistenceDefault(this);
			obj.Load();
	2. On OK button call
			PersistenceDefault obj=new PersistenceDefault(this);
			obj.Save();
	3. Set All control which you want set default with TAG=DEFAULT			

 * */


namespace SIA.UI.Controls.Utilities
{
	/// <summary>
	/// Summary description for PersistenceDefault.
	/// </summary>
	public class PersistenceDefault
	{
		public	bool	_persis;
		private	Form	_formpersis;
		private string	_filename;

		public PersistenceDefault(Form formpersis)
		{
			_persis=true;
			if(formpersis==null)	
			{
				_persis=false;
				return;
			}
			try
			{
				_formpersis=formpersis;
				_filename=Application.StartupPath + "\\DefaultData\\" + _formpersis.Name + ".xml";
				if( !Directory.Exists(Application.StartupPath + "\\DefaultData") )
					Directory.CreateDirectory(Application.StartupPath + "\\DefaultData");
			}
			catch
			{
				_persis=false;
			}
		}


		public void	Load()
		{
			if(!_persis) return;

			if(!File.Exists(_filename))
				return;

			try
			{
				DataSet ds=new DataSet(_formpersis.Name);
				ds.ReadXml(_filename);
				ReadControlDefaul(_formpersis,ds);
			}
			catch
			{
			}
		}


		public void Save()
		{
			if(!_persis) return;

			if( File.Exists(_filename) )
			{
				File.SetAttributes(_filename, 
					File.GetAttributes(_filename) & ~System.IO.FileAttributes.ReadOnly);
				File.Delete(_filename);
			}			

			try
			{
				DataSet ds=new DataSet(_formpersis.Name);
				ds.Tables.Add(new DataTable());
				ds.Tables[0].Columns.Add(new DataColumn("NAME"));
				ds.Tables[0].Columns.Add(new DataColumn("VALUE"));
				ds.Tables[0].Columns.Add(new DataColumn("ENABLE",typeof(bool)));
				ds.Tables[0].Columns.Add(new DataColumn("VISIBLE",typeof(bool)));

				WriteControlDefaul(_formpersis,ds);

				ds.WriteXml(_filename,XmlWriteMode.IgnoreSchema);
			}
			catch
			{

			}
		}


		public void ReadControlDefaul(Control  parent,DataSet ds)
		{
			foreach(Control ctrl in parent.Controls)
			{
				if(ctrl.Tag!=null && ctrl.Tag.ToString()=="DEFAULT")				
				{
					//Get Value 
					DataRow []row=ds.Tables[0].Select("NAME='" + ctrl.Name + "'");
					if(row!=null && row.Length ==1)
					{
						string svalue=row[0]["VALUE"].ToString();

						//Check type of control
						if( ctrl.GetType()== typeof(CheckBox) )
						{
							((CheckBox)ctrl).Checked = bool.Parse(svalue);
						}
						else if( ctrl.GetType()== typeof(RadioButton) )
						{
							((RadioButton)ctrl).Checked = bool.Parse(svalue);
						}
						else if(ctrl.GetType()== typeof(NumericUpDown ) )
						{
							((NumericUpDown )ctrl).Value = Decimal.Parse(svalue);
						}
						else if(ctrl.GetType()== typeof(TextBox) )
						{
							((TextBox)ctrl).Text = svalue;
						}
						else if(ctrl.GetType()== typeof(ComboBox) )
						{
							((ComboBox)ctrl).SelectedIndex= int.Parse(svalue);
						}
						ctrl.Enabled =bool.Parse(row[0]["ENABLE"].ToString());
						ctrl.Visible=bool.Parse(row[0]["VISIBLE"].ToString());
					}
				}
				if( ctrl.Controls.Count >0)
					ReadControlDefaul(ctrl,ds);
			}
		}


		public void	WriteControlDefaul(Control parent,DataSet ds)
		{
			foreach(Control ctrl in parent.Controls)
			{
				if(ctrl.Tag!=null && ctrl.Tag.ToString()=="DEFAULT")				
				{
					//Get Value 
					DataRow row=ds.Tables[0].NewRow();
					row["NAME"]=ctrl.Name;

					string svalue="";

					//Check type of control
					if( ctrl.GetType()== typeof(CheckBox) )
					{
						svalue=((CheckBox)ctrl).Checked.ToString();
					}
					else if( ctrl.GetType()== typeof(RadioButton) )
					{
						svalue=((RadioButton)ctrl).Checked.ToString();
					}
					else if(ctrl.GetType()== typeof(NumericUpDown ) )
					{
						svalue=((NumericUpDown )ctrl).Value.ToString();
					}
					else if(ctrl.GetType()== typeof(TextBox) )
					{
						svalue=((TextBox)ctrl).Text;
					}
					else if(ctrl.GetType()== typeof(ComboBox) )
					{
						svalue = ((ComboBox)ctrl).SelectedIndex.ToString();
					}

					row["VALUE"]=svalue;
					row["ENABLE"]=ctrl.Enabled;
					row["VISIBLE"]=ctrl.Visible;

					ds.Tables[0].Rows.Add(row);
				}
				if( ctrl.Controls.Count >0)
					WriteControlDefaul(ctrl,ds);
			}

		}
		////////////////////////////////////////////////////////
		
		public void	LoadAll()
		{
			if(!_persis) return;

			if(!File.Exists(_filename))
				return;

			try
			{
				DataSet ds=new DataSet(_formpersis.Name);
				ds.ReadXml(_filename);
				ReadControl(_formpersis,ds);
			}
			catch
			{
			}
		}


		public void SaveALL()
		{
			if(!_persis) return;

			if( File.Exists(_filename) )
			{
				File.SetAttributes(_filename, 
					File.GetAttributes(_filename) & ~System.IO.FileAttributes.ReadOnly);
				File.Delete(_filename);
			}			

			try
			{
				DataSet ds=new DataSet(_formpersis.Name);
				ds.Tables.Add(new DataTable());
				ds.Tables[0].Columns.Add(new DataColumn("NAME"));
				ds.Tables[0].Columns.Add(new DataColumn("VALUE"));
				ds.Tables[0].Columns.Add(new DataColumn("ENABLE",typeof(bool)));
				ds.Tables[0].Columns.Add(new DataColumn("VISIBLE",typeof(bool)));

				WriteControl(_formpersis,ds);

				ds.WriteXml(_filename,XmlWriteMode.IgnoreSchema);
			}
			catch
			{

			}
		}


		private bool IsCorrectType(Control ctrl)
		{
			//Check type of control
			if( ctrl.GetType()== typeof(CheckBox) )	return true;
			
			if( ctrl.GetType()== typeof(RadioButton) ) return true;		
			
			if(ctrl.GetType()== typeof(NumericUpDown ) ) return true;
			
			if(ctrl.GetType()== typeof(TextBox) ) return true;

			if(ctrl.GetType()== typeof(ComboBox) ) return true;
			
			return false;
		}

		public void ReadControl(Control  parent,DataSet ds)
		{
			foreach(Control ctrl in parent.Controls)
			{
				if( IsCorrectType(ctrl) )				
				{
					//Get Value 
					DataRow []row=ds.Tables[0].Select("NAME='" + ctrl.Name + "'");
					if(row!=null && row.Length ==1)
					{
						string svalue=row[0]["VALUE"].ToString();

						//Check type of control
						if( ctrl.GetType()== typeof(CheckBox) )
						{
							((CheckBox)ctrl).Checked = bool.Parse(svalue);
						}
						else if( ctrl.GetType()== typeof(RadioButton) )
						{
							((RadioButton)ctrl).Checked = bool.Parse(svalue);
						}
						else if(ctrl.GetType()== typeof(NumericUpDown ) )
						{
							((NumericUpDown )ctrl).Value = Decimal.Parse(svalue);
						}
						else if(ctrl.GetType()== typeof(TextBox) )
						{
							((TextBox)ctrl).Text = svalue;
						}
						else if(ctrl.GetType()== typeof(ComboBox) )
						{
							((ComboBox)ctrl).SelectedIndex= int.Parse(svalue);
						}
						ctrl.Enabled =bool.Parse(row[0]["ENABLE"].ToString());
						ctrl.Visible=bool.Parse(row[0]["VISIBLE"].ToString());
					}
				}
				if( ctrl.Controls.Count >0)
					ReadControl(ctrl,ds);
			}
		}


		public void	WriteControl(Control parent,DataSet ds)
		{
			foreach(Control ctrl in parent.Controls)
			{
				if( IsCorrectType(ctrl) )				
				{
					//Get Value 
					DataRow row=ds.Tables[0].NewRow();
					row["NAME"]=ctrl.Name;

					string svalue="";

					//Check type of control
					if( ctrl.GetType()== typeof(CheckBox) )
					{
						svalue=((CheckBox)ctrl).Checked.ToString();
					}
					else if( ctrl.GetType()== typeof(RadioButton) )
					{
						svalue=((RadioButton)ctrl).Checked.ToString();
					}
					else if(ctrl.GetType()== typeof(NumericUpDown ) )
					{
						svalue=((NumericUpDown )ctrl).Value.ToString();
					}
					else if(ctrl.GetType()== typeof(TextBox) )
					{
						svalue=((TextBox)ctrl).Text;
					}
					else if(ctrl.GetType()== typeof(ComboBox) )
					{
						svalue = ((ComboBox)ctrl).SelectedIndex.ToString();
					}

					row["VALUE"]=svalue;
					row["ENABLE"]=ctrl.Enabled;
					row["VISIBLE"]=ctrl.Visible;

					ds.Tables[0].Rows.Add(row);
				}
				if( ctrl.Controls.Count >0)
					WriteControl(ctrl,ds);
			}

		}

		
	}
}
