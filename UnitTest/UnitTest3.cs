using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIA.IPEngine;
using SIA.Algorithms.ObjectDetection;
using System.Drawing;

namespace UnitTest
{
    using Polygon = List<Point>;
    using PolygonF = List<PointF>;
    using PolygonList = List<List<Point>>;
    using PolygonFList = List<List<PointF>>;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;

    /// <summary>
    /// Summary description for UnitTest3
    /// </summary>
    [TestClass]
    public class UnitTest3
    {
        public UnitTest3()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        unsafe public void TestLabelAlgorithm()
        {
            string greyFile = @"D:\temp\contour_tracing_1.bmp";
            string outFile = @"D:\temp\out_contour_tracing_1.bmp";


            greyFile = @"D:\temp\contour_tracing_2.bmp";
            outFile = @"D:\temp\out_contour_tracing_2.bmp";

            greyFile = @"D:\temp\contour_tracing_3.bmp";
            outFile = @"D:\temp\out_contour_tracing_3.bmp";

            greyFile = @"D:\temp\contour_tracing_4.bmp";
            outFile = @"D:\temp\out_contour_tracing_4.bmp";

            greyFile = @"D:\temp\contour_tracing_5.bmp";
            outFile = @"D:\temp\out_contour_tracing_5.bmp";
            

            GreyDataImage greyImage = GreyDataImage.FromFile(greyFile);
            int w = greyImage.Width;
            int h = greyImage.Height;
            bool[] binImage = new bool[w * h];
            int[] labelImage = new int[w * h];
            ushort* greyData = greyImage._aData;
            int index = 0;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++, index++)
                {
                    if (greyData[index] < 90)
                    {
                        binImage[index] = true;
                    }
                }
            }
            //greyImage.Dispose(); greyImage = null;

            DateTime start = DateTime.Now;

            List<Blob> blobs = new List<Blob>();
            fixed (bool* pBinImage = binImage)
            fixed (int* pLabelImage = labelImage)
            {
                ContourTracing.PerformLabel(pBinImage, w, h, pLabelImage, blobs);
            }
            DateTime ended = DateTime.Now;
            Console.WriteLine("====DURATiON: {0} ms", (ended - start).TotalMilliseconds);

            using (Bitmap bmp = greyImage.CreateBitmap())
            {
                using (Graphics grph = Graphics.FromImage(bmp))
                using (Pen pen = new Pen(Color.FromArgb(255, Color.Red), 1.0f))
                using (GraphicsPath path = new GraphicsPath())
                {
                    if (blobs != null)
                    {
                        foreach (Blob blob in blobs)
                        {
                            PolygonFList pfl = blob.ToPolygonFs();
                            GraphicsPath blobPath = new GraphicsPath();
                            blobPath.AddPolygon(pfl[0].ToArray());

                            if (pfl.Count > 1)
                            {
                                for (int i = 1; i < pfl.Count; i++)
                                {
                                    blobPath.AddPolygon(pfl[i].ToArray());
                                }
                            }

                            path.AddPath(blobPath, false);
                        }
                    }

                    grph.DrawPath(pen, path);
                }

                bmp.Save(outFile, ImageFormat.Bmp);
            }

            Console.WriteLine(string.Format("==== FOUNT: {0}", blobs.Count));
        }
    }
}
