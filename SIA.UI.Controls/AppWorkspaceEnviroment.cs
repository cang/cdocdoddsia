using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.IO;

namespace SIA.UI.Controls
{
    /// <summary>
    /// Represents the application workspace settings
    /// </summary>
    public class AppWorkspaceEnviroment 
        : IDisposable
    {
        const string CACHED_FOLDER = @"Cached\";
        const string HISTORY_FOLDER = @"Histories\";

        private AppWorkspace _appWorkspace = null;

        public AppWorkspaceEnviroment(AppWorkspace appWorkspace)
        {
            this._appWorkspace = appWorkspace;

            // initialize settings
            this.OnInitialize();
        }

        ~AppWorkspaceEnviroment()
        {
            this.Dispose();
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            this.OnUninitialize();
        }

        protected virtual void OnInitialize()
        {
            string str = AppSettings.CachedFolder;
            if (Directory.Exists(str) == false)
                Directory.CreateDirectory(str);

            str = AppSettings.HistoryFolder;
            if (Directory.Exists(str) == false)
                Directory.CreateDirectory(str);

            str = AppSettings.MySettingsFolder;
            if (Directory.Exists(str) == false)
                Directory.CreateDirectory(str);
        }

        protected virtual void OnUninitialize()
        {
            // clean up temporary files
            if (Directory.Exists(AppSettings.CachedFolder))
                Directory.Delete(AppSettings.CachedFolder, true);
            if (Directory.Exists(AppSettings.HistoryFolder))
                Directory.Delete(AppSettings.HistoryFolder, true);
        }

        
    }
}
