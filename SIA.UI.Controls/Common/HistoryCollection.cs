using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SIA.UI.Controls.Helpers;
using SIA.SystemLayer;
using System.IO;
using System.Diagnostics;

namespace SIA.UI.Controls.Common
{
    /// <summary>
    /// The History Collection class
    /// </summary>
    public class HistoryCollection 
        : CollectionBase
    {
        private static int _stepCounter = 1;

        public HistoryCollection()
        {
        }

        ~HistoryCollection()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            this.Clear();
        }

        public int Add(CommonImage image)
        {
            History item = null;

            string fileName;

            try
            {
                // retrieve history folder from application settings
                string folder = AppSettings.HistoryFolder;

                // creates if the folder is not exists
                if (Directory.Exists(folder) == false)
                    Directory.CreateDirectory(folder);

                // acquires history items's filename
                fileName = string.Format(@"{0}\{1}.hist", folder, _stepCounter++);

                // create new history item
                item = new History(fileName, image);

                // insert into base's list
                return base.List.Add(item);
            }
            catch (System.Exception exp)
            {
                if (item != null)
                    item.Dispose();
                item = null;

                Trace.WriteLine(exp);
            }
            finally
            {
            }

            return -1;
        }

        protected override void OnClear()
        {
            while (base.InnerList.Count > 0)
            {
                History item = base.InnerList[0] as History;
                if (item == null)
                    continue;

                // delete the temporary file if any
                if (File.Exists(item.FileName))
                    File.Delete(item.FileName);

                // clean up item's resoure
                item.Dispose();

                // remove item out of the list
                base.InnerList.RemoveAt(0);
            }

            base.OnClear();
        }

        protected override void OnRemove(int index, object value)
        {
            History item = (History)value;
            if (item != null)
            {
                // delete the temporary file if any
                if (File.Exists(item.FileName))
                    File.Delete(item.FileName);

                // clean up item's resoure
                item.Dispose();
            }

            base.OnRemove(index, value);
        }


        public History this[int index]
        {
            get
            {
                History history = null;
                try
                {
                    history = (History)base.List[index];
                }
                catch (System.Exception exp)
                {
                    Trace.WriteLine(exp);
                }

                return history;
            }
        }
    }
}
