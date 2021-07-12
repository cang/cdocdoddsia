using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace SiGlaz.Common
{
    public class PathHelper
    {
        private const string patternPath = @"(?<disk>[^:]+):(?<temp>\\?(?<subfolder>[^\\]+)\\?)*";
        private static Regex regPath = null;
        public static bool CreateMissingFolderAuto(string path)
        {
            if (regPath == null)
                regPath = new Regex(patternPath);

            if (regPath == null)
                throw new System.Exception("Cannot create Regular Expression instance in PathHelper.");

            path = Path.GetDirectoryName(path);

            bool bSucceed = true;

            Match match = regPath.Match(path);
            if (match.Success)
            {
                string disk = string.Empty;
                if (match.Groups["disk"] != null && match.Groups["disk"].Captures != null &&
                    match.Groups["disk"].Captures.Count > 0)
                {
                    Capture disk_capture = match.Groups["disk"].Captures[0];
                    if (disk_capture.Value != null && disk_capture.Value != string.Empty)
                    {
                        disk = string.Format("{0}:\\", disk_capture.Value);
                    }
                }

                List<string> subfolderItems = new List<string>();

                CaptureCollection subFolders = match.Groups["subfolder"].Captures;
                if (subFolders != null && subFolders.Count > 0)
                {
                    foreach (Capture subfolder_capture in subFolders)
                    {
                        if (subfolder_capture == null) continue;

                        subfolderItems.Add(subfolder_capture.Value);
                    }
                }

                string folder = disk;

                if (subfolderItems != null && subfolderItems.Count > 0)
                {
                    int n = subfolderItems.Count;
                    int i = 0;
                    while (i < n)
                    {
                        folder += subfolderItems[i] + "\\";

                        if (!Directory.Exists(folder))
                        {
                            try
                            {
                                Directory.CreateDirectory(folder);
                            }
                            catch (System.Exception exp)
                            {
                                string msg = string.Format(
                                    "Failed to create folder: {0} reason: {1}",
                                    folder, exp.Message);

                                Console.WriteLine(msg);

                                throw exp;
                            }
                        }

                        i++;
                    }
                }
            }

            return bSucceed;
        }
    }
}
