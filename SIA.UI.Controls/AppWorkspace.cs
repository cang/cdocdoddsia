using System;
using System.Collections.Generic;
using System.Text;
using SIA.UI.Controls.Dialogs;
using SIA.Plugins.Common;
using System.Windows.Forms;
using SiGlaz.UI.CustomControls;

namespace SIA.UI.Controls
{
    /// <summary>
    /// The AppWorkspace provides basic implementation of the IAppWorkspace interface.
    /// This class serves as an application workspace which contains 1 or more document workspace.
    /// </summary>
    public abstract class AppWorkspace 
        : DialogBase, IAppWorkspace
    {
        private AppWorkspaceEnviroment _enviroment = null;

        /// <summary>
        /// Gets the application enviroment settings
        /// </summary>
        public AppWorkspaceEnviroment Enviroment
        {
            get { return _enviroment; }
        }

        public AppWorkspace()
        {
            this.InitializeComponent();

            // initialize enviroment
            _enviroment = new AppWorkspaceEnviroment(this);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppWorkspace));
            this.SuspendLayout();
            // 
            // AppWorkspace
            // 
            this.ClientSize = new System.Drawing.Size(383, 291);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.Name = "AppWorkspace";
            this.Text = "AppWorkspace";
            this.ResumeLayout(false);

        }

        #region IAppWorkspace Members

        /// <summary>
        /// Gets the document workspace created by the application workspace
        /// </summary>
        public abstract IDocWorkspace DocumentWorkspace { get;}

        /// <summary>
        /// Gets the plugin manager handling all the plugins
        /// </summary>
        public abstract IPluginManager PluginManager { get;}

        /// <summary>
        /// Gets the main menu of the application workspace
        /// </summary>
        public abstract MainMenu MainMenu { get;}

        /// <summary>
        /// Gets the main toolbar of the application workspace
        /// </summary>
        public abstract ToolBarEx MainToolBar { get;}

        #endregion

        #region ICommandDispatcher Members

        /// <summary>
        /// Gets all registered commands 
        /// </summary>
        public abstract ICommandHandler[] Commands { get;}

        /// <summary>
        /// Push a command into the command queue and exit without waiting for the command to be executed
        /// </summary>
        /// <param name="command">The command key</param>
        /// <param name="args">The arguments of the command</param>
        public abstract void PostCommand(string command, params object[] args);

        /// <summary>
        /// Push a command into the command queue and wait until the command finish processing
        /// </summary>
        /// <param name="command">The command key</param>
        /// <param name="args">The arguments of the command</param>
        public abstract void DispatchCommand(string command, params object[] args);

        #endregion

        /// <summary>
        /// Updates the user interface elements such as menu item, toolbar button, etc...
        /// </summary>
        public virtual void UpdateUI()
        {
        }
    }
}
