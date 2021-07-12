using System;
using System.Collections.Generic;
using System.Text;
using SIA.UI.Controls.Automation.Commands;

namespace SIA.UI.Controls.Automation
{
    /// <summary>
    /// The IDataProviderStep provides declaration for LoadImageStep which can listen to 1 or multi folder.
    /// </summary>
    public interface IDataProviderStep
    {
        bool ScanSubFolder {get;}
        bool ClearProcessedFileHistory { get;}
        bool UseFilter { get;}
        InputFileFilter Filter { get;}

        string[] GetFiles();
        string[] GetScanFolders();
        string GetSearchPatterns();
    }
}
