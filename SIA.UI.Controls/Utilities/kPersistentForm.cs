using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SIA.UI.Controls.Utilities
{
	/// <summary>
	/// Summary description for kPersistentForm.
	/// </summary>
	public class kPersistentForm : System.Windows.Forms.Form
	{
		private String		_filename = String.Empty;
		private ArrayList	_SerializableObjects = null;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public kPersistentForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// initialize peristence storage 
			//
			this._filename = Application.StartupPath + @"\DefaultData\" + this.Name + ".xml";
			if (!Directory.Exists(Application.StartupPath + @"\DefaultData") )
				Directory.CreateDirectory(Application.StartupPath + @"\DefaultData");
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
			// 
			// kPersistentForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Name = "kPersistentForm";
			this.Text = "kPersistentForm";
			this.Load += new System.EventHandler(this.kPersistentForm_Load);
			this.Closed += new System.EventHandler(this.kPersistentForm_Closed);

		}
		#endregion

		private void kPersistentForm_Load(object sender, System.EventArgs e)
		{
			// initialize form's icon
			try
			{
				this.Icon = new Icon(this.GetType(),"Icon.icon.ico");
			}
			catch(System.Exception exp)
			{
				System.Diagnostics.Debug.WriteLine(exp.ToString());
			}

			if (Deserialize()==false)
				InitializeDefaultData();
		}

		private void kPersistentForm_Closed(object sender, System.EventArgs e)
		{
			Deserialize();
		}

		protected virtual bool SetDefaultValue(System.Windows.Forms.Control ctrl)
		{	
			return false;
		}

		protected bool Serialize()
		{	
			bool bResult = false;
			try
			{
				XmlTextWriter writer = new XmlTextWriter(this._filename, System.Text.Encoding.Default);
				writer.Formatting = System.Xml.Formatting.Indented;
				
				writer.WriteStartDocument();

				foreach(Control ctrl in this._SerializableObjects)
				{
					System.String[] peristencedata = GetPersistenceData(ctrl);
					if (peristencedata!=null && peristencedata.Length > 0)
					{
						writer.WriteStartElement(ctrl.Name);
						writer.WriteEndElement();
					}
				}

				writer.WriteEndDocument();
				bResult = true;
			}
			catch(System.Exception exp) 
			{ 
				HandleSystemException(exp);
			}
			return bResult;
		}

		protected bool Deserialize()
		{
			bool bResult = false;
			try
			{
				bResult = true;
			}
			catch(System.Exception exp) 
			{ 
				HandleSystemException(exp);
			}
			return bResult;
		}

		protected void InitializeDefaultData()
		{
			foreach(Control ctrl in this.Controls)
			{
				if (SetDefaultValue(ctrl) == true)
					this._SerializableObjects.Add(ctrl);
			}
			this._SerializableObjects.Add(this);
		}

		private void HandleSystemException(System.Exception exp)
		{
			System.Diagnostics.Debug.WriteLine(exp.ToString());
		}

		private String[] GetPersistenceData(System.Windows.Forms.Control ctrl)
		{
			ArrayList result = new ArrayList();
			
			if (ctrl is System.Windows.Forms.Form)
			{
				result.Add(ctrl.Location.ToString());
				result.Add(ctrl.Size.ToString());
			}
			else if (ctrl is System.Windows.Forms.Label)
			{
			}
			else if (ctrl is System.Windows.Forms.Button)
			{
			}
			else if (ctrl is System.Windows.Forms.CheckBox)
			{
			}
			else
				result = null;

			return result != null ? (String[])result.ToArray(typeof(String)) : null;
		}
	}
}
