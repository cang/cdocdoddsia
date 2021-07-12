using System;
using System.Collections;
using Microsoft.Win32;

namespace SIA.UI.Controls
{
    /// <summary>
    /// Summary description for RecentFileManager.
    /// </summary>
    public enum UI_TYPE
    {
        SIA = 0,
        SIAW = 1
    }
    public class RecentFileManager
    {
        private string RecentRegis = "";
        public ArrayList AR;
        public RecentFileManager(UI_TYPE ut)
        {
            switch (ut)
            {
                case UI_TYPE.SIA:
                    RecentRegis = @"Software\SIA\SiGlaz Image Analyzer\RECENT";
                    break;
                case UI_TYPE.SIAW:
                    RecentRegis = @"Software\SIA\SiGlaz Image Analyzer Automation\RECENT";
                    break;

            }
            AR = new ArrayList();
        }
        public void Serialize()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(RecentRegis);
                for (int i = 0; i < AR.Count; i++)
                {
                    key.SetValue(string.Format("File{0}", i + 1), AR[i]);
                }
            }
            catch
            {
            }
        }

        public void Derialize()
        {
            try
            {
                AR.Clear();
                RegistryKey key = Registry.CurrentUser.CreateSubKey(RecentRegis);
                string[] _firsts = key.GetValueNames();
                for (int i = 0; i < _firsts.Length; i++)
                {
                    AR.Add((string)key.GetValue(_firsts[i]));
                }
            }
            catch
            {
            }
        }
        private Boolean IsExist(string rfn)
        {
            foreach (object obj in this.AR)
            {
                string s_obj = (string)obj;
                if (s_obj.Equals(rfn))
                {
                    this.AR.Remove(obj);
                    return true;
                }
            }
            return false;
        }
        private void CheckMaximum()
        {
            if (this.AR.Count > 9)
            {
                this.AR.RemoveAt(9);
            }
        }
        public void Add(string rfn)
        {
            IsExist(rfn);
            this.AR.Insert(0, rfn);
            CheckMaximum();
        }
        public void Remove(string rfn)
        {
            IsExist(rfn);
            this.AR.Remove(rfn);
        }
    }
}
