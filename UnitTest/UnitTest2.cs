using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SiGlaz.Common.ImageAlignment;
using SIA.IPEngine;
using SIA.Algorithms.Preprocessing.Alignment;
using System.Diagnostics;

namespace UnitTest
{
    /// <summary>
    /// Summary description for UnitTest2
    /// </summary>
    [TestClass]
    public class UnitTest2
    {
        public UnitTest2()
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
        public void TestCorrelationMatcherPerformance()
        {
            string filename = @"d:\test.bmp";
            string settingFilename = @"d:\p.msr";

            MetrologySystemReference refFile = MetrologySystemReference.Deserialize(settingFilename);

            GreyDataImage greyImage = GreyDataImage.FromFile(filename);

            AlignerBase aligner = new ABSAligner(refFile.AlignmentSettings);

            Stopwatch ts = new System.Diagnostics.Stopwatch();
            ts.Start();

            AlignmentResult alignResult = aligner.Align(greyImage);

            ts.Stop();
            Debug.WriteLine("TestCorrelationMatcherPerformance :" + ts.Elapsed);

            /*
                        float newWidth = refFile.AlignmentSettings.NewWidth;
                        float newHeight = refFile.AlignmentSettings.NewHeight;
                        float newAngle = alignResult.GetRotateAngle(newWidth, newHeight);
                        float newLeft = alignResult.GetLeft(newWidth, newHeight);
                        float newTop = alignResult.GetTop(newWidth, newHeight);

                        // update alignment result
                        _alignmentResult = alignResult;

                        // update metrology coordinate system
                        _metrologySystem = new MetrologySystem();
                        _metrologySystem.CurrentUnit.CopyFrom(refFile.MetrologySystem.CurrentUnit);
                        CoordinateSystem cs = _metrologySystem.CurrentCoordinateSystem;

                        PointF[] pts = new PointF[] { PointF.Empty };

                        pts[0] = refFile.TransformToLTDeviceCoordinate(
                            refFile.MetrologySystem.CurrentCoordinateSystem.GetOriginPointF());

                        // transform to abs-left-top coordinate
                        pts[0] =
                            MetrologySystemReference.TransformToImageCoordinate(pts[0], newLeft, newTop, newAngle);

                        // update coordinate system
                        cs.Orientation = refFile.MetrologySystem.CurrentCoordinateSystem.Orientation;
                        cs.DrawingOriginX = pts[0].X;
                        cs.DrawingOriginY = pts[0].Y;
                        cs.DrawingAngle =
                            newAngle -
                            (refFile.DeviceOrientation - refFile.MetrologySystem.CurrentCoordinateSystem.DrawingAngle);

                        // rebuild metrology system parameters
                        _metrologySystem.RebuildTransformer();
             */
        }
    }
}
