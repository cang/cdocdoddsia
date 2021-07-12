using System;
using System.Reflection;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

using SIA.SystemLayer;
using SIA.UI.Controls;
using SIA.UI.Controls.Automation;
using SIA.UI.Controls.Commands;
using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Common;
using SIA.UI.Components;

namespace SIA.UI.Controls.Dialogs
{
    /// <summary>
    /// Summary description for DialogBase.
    /// </summary>
    public class DialogBase
        : DlgContextSensitiveHelp
    {
        #region member attributes

        private IDictionary _settings = null;

        #endregion

        #region public properties

        protected IDictionary DialogSettings
        {
            get
            {
                if (_settings == null)
                {
                    string filename = this.GetHashCode().ToString() + ".settings";
                    _settings = (Hashtable)UserSettings.RestoreObject(filename);
                    if (_settings == null)
                        _settings = new Hashtable();
                }

                return _settings;
            }
        }

        #endregion

        #region constructor and destructor

        public DialogBase()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // 
            // DialogBase
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 266);
            // initialize default dialog style
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.TopMost = false;
            this.Name = "DialogBase";

        }

        #endregion

        #region override routines

        protected override void OnLoad(EventArgs e)
        {
            // execute base routine
            base.OnLoad(e);

            // update child control style when Visual Style enabled
            this.EnableVisualStyles(this);

            try
            {
                // initialize dialog icons
                this.Icon = ResourceHelper.ApplicationIcon;

                // initialize persistence helper
                this.LoadPersistenceData();

                // load persistence data
                this.OnLoadPersistenceData(this.DialogSettings);
            }
            catch (System.Exception exp)
            {
                Trace.WriteLine(exp);
            }
            finally
            {
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                try
                {
                    // save persistence data
                    this.OnSavePersistenceData(this.DialogSettings);
                }
                catch (System.Exception exp)
                {
                    Trace.WriteLine(exp);
                }
                finally
                {
                    // uninitialized persistence helper
                    this.SavePersistenceData();
                }
            }


            base.OnClosed(e);
        }

        protected virtual void EnableVisualStyles(Control ctrl)
        {
            if (ctrl == null)
                return;

            Type type = ctrl.GetType();
            if (type.IsSubclassOf(typeof(ButtonBase)) && (ctrl as ButtonBase).ImageList == null)
                ((ButtonBase)ctrl).FlatStyle = FlatStyle.System;

            if (ctrl.Controls.Count > 0)
            {
                foreach (Control childControl in ctrl.Controls)
                    EnableVisualStyles(childControl);
            }
        }

        #endregion

        #region Methods

        #endregion

        #region Virtual Methods

        #region Persistence Data

        public object this[string key]
        {
            get
            {
                if (_settings.Contains(key))
                    return _settings[key];
                return null;
            }
            set
            {
                if (_settings.Contains(key))
                    _settings[key] = value;
                _settings.Add(key, value);
            }
        }

        protected virtual void OnLoadPersistenceData(IDictionary storage)
        {
            ArrayList controls = GetPersistedControls(this);
            foreach (Control ctrl in controls)
            {
                try
                {
                    ValueRestorer.Restore(ctrl, storage, OnGetDefaultValue(ctrl));
                }
                catch (UnknownRestorerException exp)
                {
                    this.OnLoadUnknownPersistenceData(exp.Control, exp.Storage, exp.DefaultValue);
                }
                catch (System.Exception exp)
                {
                    Trace.WriteLine(String.Format("Failed to restore last saved value for the control {0} on form {1}: {2}", ctrl.Name, this.Name, exp.Message));
                }
            }
        }

        protected virtual void OnSavePersistenceData(IDictionary storage)
        {
            ArrayList controls = GetPersistedControls(this);
            foreach (Control ctrl in controls)
            {
                try
                {
                    ValueRestorer.Persist(ctrl, storage);
                }
                catch (UnknownRestorerException exp)
                {
                    this.OnSaveUnknownPersistenceData(exp.Control, exp.Storage);
                }
                catch (System.Exception exp)
                {
                    Trace.WriteLine(String.Format("Failed to save value for the control {0} on form {1}: {2}", ctrl.Name, this.Name, exp.Message));
                }
            }
        }

        protected virtual object OnGetDefaultValue(System.Windows.Forms.Control ctrl)
        {
            return null;
        }

        protected virtual void OnLoadUnknownPersistenceData(Control ctrl, IDictionary storage, object defaultValue)
        {
        }

        protected virtual void OnSaveUnknownPersistenceData(Control ctrl, IDictionary storage)
        {
        }

        #region internal persitence helpers

        private ArrayList GetPersistedControls(Control container)
        {
            ArrayList controls = new ArrayList();
            foreach (Control ctrl in container.Controls)
            {
                if (ctrl.Controls.Count > 0)
                {
                    ArrayList childs = GetPersistedControls(ctrl);
                    if (childs.Count > 0)
                        controls.AddRange(childs);
                }
                if (IsPersisted(ctrl))
                    controls.Add(ctrl);
            }
            return controls;
        }

        private bool IsPersisted(System.Windows.Forms.Control ctrl)
        {
            return OnGetDefaultValue(ctrl) != null;
        }

        protected virtual void LoadPersistenceData()
        {
            if (_settings == null)
            {
                string filename = this.GetType().FullName + ".params";
                _settings = (Hashtable)UserSettings.RestoreObject(filename);
                if (_settings == null)
                    _settings = new Hashtable();
            }
        }

        protected virtual void SavePersistenceData()
        {
            string filename = this.GetType().FullName + ".params";
            UserSettings.StoreObject(filename, _settings);
        }

        public void FlushPersistenceData()
        {
            this.OnSavePersistenceData(this._settings);
            this.SavePersistenceData();
        }

        #endregion

        #endregion

        #region RasterCommandSetings Serialization Helpers

        public virtual bool SaveCommandSettings(RasterCommandSettings settings)
        {
            using (SaveFileDialog dlg = FileTypes.Xml.SaveFileDialog("Save settings as..."))
            {
                dlg.OverwritePrompt = true;
                dlg.FileName = "Settings.xml";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // serialize command settings
                    RasterCommandSettingsSerializer.Serialize(dlg.FileName, settings);

                    return true;
                }

                return false;
            }
        }

        public virtual RasterCommandSettings LoadCommandSettings(Type type)
        {
            using (OpenFileDialog dlg = FileTypes.Xml.OpenFileDialog("Load settings from..."))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // deserialize command settings
                    return RasterCommandSettingsSerializer.Deserialize(dlg.FileName, type);
                }

                return null;
            }
        }

        #endregion

        #endregion
    }



    internal delegate void RestoreValueHandler(System.Windows.Forms.Control ctrl, IDictionary storage, object defaultValue);
    internal delegate void PersistValueHandler(System.Windows.Forms.Control ctrl, IDictionary storage);

    internal class ValueRestorer
    {
        /// <summary>
        /// internal Restore Object helper
        /// </summary>
        private class RestoreObject
        {
            private System.Type _restoreType;

            public event RestoreValueHandler RestoreValue;
            public event PersistValueHandler PersistValue;

            public Type RestoreType
            {
                get { return _restoreType; }
                set { _restoreType = value; }
            }

            public RestoreObject(System.Type type, RestoreValueHandler RestoreHandler, PersistValueHandler PersistHandler)
            {
                this._restoreType = type;
                this.RestoreValue += RestoreHandler;
                this.PersistValue += PersistHandler;
            }

            public void InvokeRestoreHandler(Control ctrl, IDictionary storage, object defaultValue)
            {
                if (this.RestoreValue != null)
                    this.RestoreValue(ctrl, storage, defaultValue);
            }

            public void InvokePersistHandler(Control ctrl, IDictionary storage)
            {
                if (this.PersistValue != null)
                    this.PersistValue(ctrl, storage);
            }
        };

        private static ArrayList _supportTypes = null;

        public static ArrayList SupportTypes
        {
            get
            {
                if (_supportTypes == null)
                {
                    _supportTypes = new ArrayList();
                    InitializeBuiltInTypes();
                }
                return _supportTypes;
            }
        }

        private static void InitializeBuiltInTypes()
        {
            RegisterType(typeof(Label), new RestoreValueHandler(ValueRestorer.RestoreLabelValue), new PersistValueHandler(ValueRestorer.PersistLabelValue));
            RegisterType(typeof(TextBox), new RestoreValueHandler(ValueRestorer.RestoreTextBoxValue), new PersistValueHandler(ValueRestorer.PersistTextBoxValue));
            RegisterType(typeof(NumericUpDown), new RestoreValueHandler(ValueRestorer.RestoreNumericUpDownValue), new PersistValueHandler(ValueRestorer.PersistNumericUpDownValue));
            RegisterType(typeof(CheckBox), new RestoreValueHandler(ValueRestorer.RestoreCheckBoxValue), new PersistValueHandler(ValueRestorer.PersistCheckBoxValue));
            RegisterType(typeof(ComboBox), new RestoreValueHandler(ValueRestorer.RestoreComboBoxValue), new PersistValueHandler(ValueRestorer.PersistComboBoxValue));
            RegisterType(typeof(ListBox), new RestoreValueHandler(ValueRestorer.RestoreListBoxValue), new PersistValueHandler(ValueRestorer.PersistListBoxValue));
            RegisterType(typeof(RadioButton), new RestoreValueHandler(ValueRestorer.RestoreRadioButtonValue), new PersistValueHandler(ValueRestorer.PersistRadioButtonValue));
        }

        #region public routines
        public static void RegisterType(System.Type type, RestoreValueHandler restorer, PersistValueHandler persister)
        {
            ValueRestorer.SupportTypes.Add(new RestoreObject(type, restorer, persister));
        }

        public static void Restore(System.Windows.Forms.Control ctrl, IDictionary storage, object defaultValue)
        {
            ArrayList supportTypes = ValueRestorer.SupportTypes;
            RestoreObject restorer = null;
            foreach (RestoreObject obj in supportTypes)
            {
                if (obj.RestoreType.FullName == ctrl.GetType().FullName)
                    restorer = obj;
            }

            if (restorer != null)
                restorer.InvokeRestoreHandler(ctrl, storage, defaultValue);
            else
                throw new UnknownRestorerException(ctrl, storage, defaultValue);
        }

        public static void Persist(System.Windows.Forms.Control ctrl, IDictionary storage)
        {
            ArrayList supportTypes = ValueRestorer.SupportTypes;
            RestoreObject restorer = null;
            foreach (RestoreObject obj in supportTypes)
            {
                if (obj.RestoreType.FullName == ctrl.GetType().FullName)
                    restorer = obj;
            }

            if (restorer != null)
                restorer.InvokePersistHandler(ctrl, storage);
            else
                throw new UnknownRestorerException(ctrl, storage);
        }
        #endregion

        #region Built-in Helpers
        private static void RestoreLabelValue(Control ctrl, IDictionary storage, object defaultValue)
        {
        }

        private static void RestoreTextBoxValue(Control txt, IDictionary storage, object defaultValue)
        {
            TextBox textBox = (TextBox)txt;
            if (storage[textBox.Name] == null)
                textBox.Text = defaultValue.ToString();
            else
                textBox.Text = (String)storage[textBox.Name];
        }

        private static void RestoreNumericUpDownValue(Control ctrl, IDictionary storage, object defaultValue)
        {
            NumericUpDown nud = (NumericUpDown)ctrl;
            string maxKey = string.Format("{0}.Maximum", nud.Name);
            string minKey = string.Format("{0}.Minimum", nud.Name);
            Decimal maxValue = nud.Maximum, minValue = nud.Minimum;

            if (storage[nud.Name] == null)
            {
                Decimal value = Convert.ToDecimal(defaultValue);
                nud.Minimum = Math.Min(minValue, value);
                nud.Maximum = Math.Max(maxValue, value);
                nud.Value = value;
            }
            else
            {
                Decimal value = Convert.ToDecimal(storage[nud.Name]);
                if (storage[maxKey] != null)
                    maxValue = Convert.ToDecimal(storage[maxKey]);
                if (storage[minKey] != null)
                    minValue = Convert.ToDecimal(storage[minKey]);
                nud.Minimum = Math.Min(minValue, value);
                nud.Maximum = Math.Max(maxValue, value);
                nud.Value = value;
            }
        }

        private static void RestoreComboBoxValue(Control cbx, IDictionary storage, object defaultValue)
        {
            ComboBox comboBox = (ComboBox)cbx;
            if (storage[comboBox.Name] == null)
            {
                int selIndex = (int)defaultValue;
                if (selIndex >= 0 && selIndex < comboBox.Items.Count)
                    comboBox.SelectedIndex = selIndex;
            }
            else
            {
                int selIndex = (int)storage[comboBox.Name];
                if (selIndex >= 0 && selIndex < comboBox.Items.Count)
                    comboBox.SelectedIndex = selIndex;
            }
        }

        private static void RestoreListBoxValue(Control ctrl, IDictionary storage, object defaultValue)
        {
        }

        private static void RestoreCheckBoxValue(Control ctrl, IDictionary storage, object defaultValue)
        {
            CheckBox chk = (CheckBox)ctrl;
            if (storage[chk.Name] == null)
                chk.CheckState = (CheckState)defaultValue;
            else
                chk.CheckState = (CheckState)storage[chk.Name];
        }

        private static void RestoreRadioButtonValue(Control ctrl, IDictionary storage, object defaultValue)
        {
            RadioButton radioButton = (RadioButton)ctrl;
            if (storage[radioButton.Name] == null)
            {
                if (defaultValue is System.Boolean)
                    radioButton.Checked = (System.Boolean)defaultValue;
                else
                    radioButton.Checked = Convert.ToBoolean(defaultValue.ToString().ToUpper() == Boolean.TrueString.ToUpper());
            }
            else
                radioButton.Checked = (Boolean)storage[radioButton.Name];
        }


        private static void PersistLabelValue(Control ctrl, IDictionary storage)
        {
        }

        private static void PersistTextBoxValue(Control ctrl, IDictionary storage)
        {
            TextBox txt = (TextBox)ctrl;
            storage[txt.Name] = txt.Text;
        }

        private static void PersistNumericUpDownValue(Control ctrl, IDictionary storage)
        {
            NumericUpDown nud = (NumericUpDown)ctrl;
            string key = string.Empty;
            key = string.Format("{0}.Maximum", nud.Name);
            storage[key] = nud.Maximum;
            key = string.Format("{0}.Minimum", nud.Name);
            storage[key] = nud.Minimum;
            key = string.Format("{0}", nud.Name);
            storage[nud.Name] = nud.Value;
        }

        private static void PersistComboBoxValue(Control ctrl, IDictionary storage)
        {
            ComboBox cbx = (ComboBox)ctrl;
            storage[cbx.Name] = cbx.SelectedIndex;
        }

        private static void PersistListBoxValue(Control lbx, IDictionary storage)
        {
        }

        private static void PersistCheckBoxValue(Control ctrl, IDictionary storage)
        {
            CheckBox chk = (CheckBox)ctrl;
            storage[chk.Name] = chk.CheckState;
        }

        private static void PersistRadioButtonValue(Control ctrl, IDictionary storage)
        {
            RadioButton radioButton = (RadioButton)ctrl;
            storage[radioButton.Name] = radioButton.Checked;
        }

        #endregion
    }

    public class UnknownRestorerException : System.ApplicationException
    {
        private Control _control;
        private IDictionary _storage;
        private object _defaultValue;

        public Control Control
        {
            get { return _control; }
        }

        public IDictionary Storage
        {
            get { return _storage; }
        }

        public object DefaultValue
        {
            get { return _defaultValue; }
        }

        public UnknownRestorerException(Control ctrl, IDictionary storage, object defaultValue)
        {
            _control = ctrl;
            _storage = storage;
            _defaultValue = defaultValue;
        }

        public UnknownRestorerException(Control ctrl, IDictionary storage)
        {
            _control = ctrl;
            _storage = storage;
            _defaultValue = null;
        }
    }
}
