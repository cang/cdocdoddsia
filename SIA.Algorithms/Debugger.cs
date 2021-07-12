
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SIA.IPEngine;
using System.IO;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace SIA.Algorithms
{
#if DEBUG
    public class SIADebugger
    {
        public static void SaveDetectedLineAndImage(
            GreyDataImage image, int[] xPoints, int[] yPoints,
            double slope, double c,
            string fileName)
        {
            try
            {
                
                using (Bitmap bmp = image.CreateBitmap())
                {
                    using (Graphics grph = Graphics.FromImage(bmp))
                    {
                        using (Pen pen = new Pen(Color.Red, 1.0f))
                        {
                            int n = xPoints.Length;
                            for (int i = 0; i < n - 1; i++)
                            {
                                grph.DrawLine(pen,
                                    xPoints[i], image.Height / 2 - yPoints[i],
                                    xPoints[i + 1], image.Height / 2 - yPoints[i + 1]);
                            }

                            if (slope != -1 && c != -1)
                            {
                                int y = 0;
                                int x = (int)((y - c) / slope);

                                grph.DrawRectangle(pen,
                                    x - 15, image.Height / 2 - y - 15, 31, 31);

                                int y1 = image.Height / 2;
                                int x1 = (int)((y1 - c) / slope);

                                int y2 = -image.Height / 2;
                                int x2 = (int)((y2 - c) / slope);

                                grph.DrawLine(
                                    pen,
                                    x1, image.Height / 2 - y1,
                                    x2, image.Height / 2 - y2);


                                //float angle = Math.Atan(-slope);
                                //angle = -180 * angle / Math.PI;


                            }
                        }
                    }

                    bmp.Save(fileName);
                }
            }
            catch
            {
            }
        }

        unsafe public static void SaveImages(
            ushort[] sample, int w1, int h1,
            ushort[] pattern, int w2, int h2,
            int[] pivotIndicesOnPattern, int[] bestMatchOnSamples,
            string fileName)
        {
            int margin = 10;

            int w = w1 + w2 + margin * 3;
            int h = Math.Max(h1, h2) + margin * 2;

            using (Bitmap bmp = new Bitmap(w, h))
            {
                using (Graphics grph = Graphics.FromImage(bmp))
                {
                    using (Bitmap b1 = CreateImage(sample, w1, h1, bestMatchOnSamples, w2))
                    {
                        grph.DrawImage(b1, margin, margin);
                    }
                    using (Pen pen = new Pen(Color.Blue, 2.0f))
                    {
                        grph.DrawRectangle(pen, margin - 3, margin - 3, w1 + 6, h1 + 6);
                    }

                    using (Bitmap b2 = CreateImage(pattern, w2, h2, pivotIndicesOnPattern, 1.0f))
                    {
                        grph.DrawImage(b2, w1 + margin * 2, margin);
                        
                    }
                    using (Pen pen = new Pen(Color.Red, 2.0f))
                    {
                        grph.DrawRectangle(pen, w1 + margin * 2 - 3, margin - 3, w2 + 6, h2 + 6);
                    }
                }

                bmp.Save(fileName);
            }
        }

        unsafe public static Bitmap CreateImage(ushort[] data, int w, int h)
        {
            Bitmap bmp = null;

            using (GreyDataImage image = new GreyDataImage(w, h))
            {
                Utilities.Copy(data, image._aData);
                bmp = image.CreateBitmap();
            }

            return bmp;
        }

        unsafe public static void SaveImage(
            ushort[] data, int w, int h, string fileName)
        {
            using (GreyDataImage image = new GreyDataImage(w, h))
            {
                Utilities.Copy(data, image._aData);
                image.SaveImage(fileName, SIA.Common.eImageFormat.Bmp);
            }
        }

        unsafe public static Bitmap CreateImage(
            ushort[] data, int w, int h, int[] highlightPoints, float highlightSize)
        {
            Bitmap bmp = null;

            using (GreyDataImage image = new GreyDataImage(w, h))
            {
                Utilities.Copy(data, image._aData);
                bmp = image.CreateBitmap();
            }

            if (bmp != null && highlightPoints != null)
            {
                using (Graphics grph = Graphics.FromImage(bmp))
                {
                    using (Pen pen = new Pen(Color.Red, 1.0f))
                    {
                        foreach (int i in highlightPoints)
                        {
                            float x = i % w + 0.5f;
                            float y = i / w + 0.5f;
                            float l = x - highlightSize * 0.5f;
                            float t = y - highlightSize * 0.5f;

                            grph.DrawRectangle(pen, l, t, highlightSize, highlightSize);
                        }
                    }
                }
            }

            return bmp;
        }

        unsafe public static void DrawPoints(
            Bitmap bmp, int[] xPoints, int[] yPoints)
        {

        }

        public static void AutoSacleImages(
            string folderPath, string outFolderPath, int outWidth, int outHeight)
        {
            // 750 x 536
            if (!Directory.Exists(folderPath))
                return;

            if (!Directory.Exists(outFolderPath))
            {
                PathHelper.CreateMissingFolderAuto(outFolderPath);   
            }

            string[] files = Directory.GetFiles(folderPath);
            if (files != null && files.Length > 0)
            {                
                //using (Image image = new Bitmap(outWidth, outHeight))
                {
                    {
                        Rectangle dstRect = new Rectangle(0, 0, outWidth, outHeight);
                        foreach (string file in files)
                        {
                            using (Image image = new Bitmap(outWidth, outHeight))
                            using (Graphics grph = Graphics.FromImage(image))
                            {
                                grph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                                grph.Clear(Color.White);

                                try
                                {
                                    using (Image img = Image.FromFile(file))
                                    {
                                        Rectangle srcRect =
                                            new Rectangle(0, 0, img.Width, img.Height);

                                        grph.DrawImage(img, dstRect, srcRect, GraphicsUnit.Pixel);
                                    }


                                    DirectoryInfo difo = Directory.GetParent(file);
                                    try
                                    {
                                        using (Brush brush = new SolidBrush(Color.White))
                                        using (Font font = new Font("Arial", 28.0f))
                                        {
                                            string s =
                                                string.Format("{0}: {1}",
                                                difo.Name, Path.GetFileName(file));
                                            
                                            Console.WriteLine(s);


                                            grph.DrawString(
                                                s,
                                                font, brush, 10, 10);
                                        }
                                    }
                                    catch
                                    {
                                    }

                                    string outfile = Path.Combine(outFolderPath,
                                        string.Format("{1}\\{0}.jpg", Path.GetFileNameWithoutExtension(file), difo.Name));

                                    PathHelper.CreateMissingFolderAuto(outfile);
                                    image.Save(outfile, ImageFormat.Jpeg);
                                }
                                catch (System.Exception exp)
                                {
                                    Console.WriteLine(exp.Message);
                                    Console.WriteLine(exp.StackTrace);
                                }
                            }
                        }
                    }
                }
            }

            string[] subfolders = Directory.GetDirectories(folderPath);
            if (subfolders != null && subfolders.Length > 0)
            {
                int i = 1;
                foreach (string subfolder in subfolders)
                {                    

                    string outSubFolder = 
                        Path.Combine(outFolderPath, i.ToString());
                    i++;
                    
                    AutoSacleImages(subfolder, outFolderPath, outWidth, outHeight);
                }
            }
        }
    }

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
#endif


//#if DEBUG_METETIME
    public class DebugMeteTime
    {
        Stopwatch sw;
        StringBuilder sbTrace;
        long total = 0;

        public DebugMeteTime()
        {
            total = 0;
            sbTrace = new StringBuilder();
            sw = new Stopwatch();
            Start();
        }

        public void ResetInformation() 
        { 
            sw.Reset(); 
            total = 0;
            sbTrace = new StringBuilder();
        }

        public void Stop()
        {
            if (sw.IsRunning)
                sw.Stop();
        }

        public void Start()
        {
            sw.Start();
        }

        public void Reset()
        {
            sw.Reset();
        }

        public void AddLine(string msg)
        {
            Stop();

            //total += sw.ElapsedTicks;
            //sbTrace.AppendLine(msg + ": " + sw.ElapsedTicks);

            total += sw.ElapsedMilliseconds;
            sbTrace.AppendLine(msg + ": " + sw.ElapsedMilliseconds);

            Reset();
            Start();
        }

        public void Write2Debug(bool stop)
        {
            sbTrace.AppendLine("Totals :" + total);
            //Debug.WriteLine(sbTrace.ToString());
           
            Trace.WriteLine(sbTrace.ToString());
            Trace.WriteLine("");

            ResetInformation();
            if (stop)
                Stop();
        }


    }
//#endif

}
