using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SIA.IPEngine;
using SiGlaz.Common.ImageAlignment;
using SIA.Algorithms.Preprocessing.Alignment;
using SIA.Algorithms.Preprocessing.Interpolation;
using SIA.Algorithms.Classification;
using SiGlaz.Common;
using SIA.SystemLayer;
using SIA.Algorithms.Preprocessing.Filtering;
using SIA.Algorithms.Preprocessing.Matching;
using SiGlaz.Common.Pattern;

namespace UnitTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class PreprocessingTest
    {
        public PreprocessingTest()
        {
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

        #region TestAlign
        [TestMethod]
        public void TestAlignment()
        {
            string settingFilename = @"d:\Projects\Xyratex\Data\Alignment\Settings\Alignment.settings";
            string inputFolder = @"d:\Projects\Xyratex\Data\input";
            string outputFolder = @"d:\Projects\Xyratex\Data\output";

            string[] files = Directory.GetFiles(inputFolder);

            Console.WriteLine("Setting: {0}\nInput: {1}\nOutput: {2}",
                settingFilename, inputFolder, outputFolder);

            Settings settings = Settings.Deserialize(settingFilename);
            Aligner aligner = new Aligner(settings);

            foreach (string input in files)
            {
                TestContext.WriteLine(Path.GetFileName(input));

                GreyDataImage image = GreyDataImage.FromFile(input);

                AlignmentResult alignment = aligner.Align_ABS(image);

                System.Drawing.Drawing2D.Matrix inverseTransform = null;

                GreyDataImage result = ImageInterpolator.AffineTransform(InterpolationMethod.Bilinear, image, alignment.SourceCoordinates,
                    settings.NewWidth, settings.NewHeight, ref inverseTransform);

                string output = Path.Combine(outputFolder, Path.GetFileName(input));
                result.SaveToBitmap(output);
            }
        }

        [TestMethod]
        public void TestAlignment_Depo()
        {
            string settingFilename = @"d:\Projects\Xyratex\Data\Alignment\Settings\Depo\Alignment_Depo.settings";
            string inputFolder = @"d:\Projects\Xyratex\Data\Alignment\Settings\Depo\input\";
            string outputFolder = @"d:\Projects\Xyratex\Data\Alignment\Settings\Depo\output\";

            string[] files = Directory.GetFiles(inputFolder);

            Console.WriteLine("Setting: {0}\nInput: {1}\nOutput: {2}",
                settingFilename, inputFolder, outputFolder);

            DepoAlignmentSettings settings = DepoAlignmentSettings.Deserialize(settingFilename);
            

            foreach (string input in files)
            {
                Console.WriteLine(Path.GetFileName(input));

                GreyDataImage image = GreyDataImage.FromFile(input);

                DepoAligner aligner = new DepoAligner(settings);
                aligner.InitImage(image);
                
                //AlignmentResult alignment = aligner.Align(image);
                
                AlignmentResult alignment = null;
                System.Drawing.Drawing2D.Matrix inverseTransform = null;

                //aligner.PerformDraftAlignment(ref alignment, ref inverseTransform);

                GreyDataImage imgDraft = aligner.DraftAlign(ref alignment, ref inverseTransform);
                string output = Path.Combine(outputFolder, Path.GetFileName(input));
                imgDraft.SaveToBitmap(output);

                // Detect Plus
                aligner.DetectPatterns(imgDraft, new Rectangle(0, 0, imgDraft._width, imgDraft._height), 0);

                // Detect Pads
                int roiWidth = imgDraft._width / settings.NumPadCol;
                int roiHeight= imgDraft._height / settings.NumPadRow;
                int len = settings.NumPadCol * settings.NumPadRow;
                for (int i = 0; i < settings.NumPadCol; i++)
                {
                    int rowIdx = i / settings.NumPadCol;
                    int colIdx = i % settings.NumPadCol;

                    Console.WriteLine();
                    Rectangle roi = new Rectangle(colIdx * roiWidth, rowIdx * roiHeight, roiWidth, roiHeight);
                    if (i>0)
                    {
                        roi = new Rectangle(colIdx * roiWidth - 20, rowIdx * roiHeight, roiWidth, roiHeight);
                    }
                    
                    Console.WriteLine(roi.ToString());
                    aligner.DetectPatterns(imgDraft, roi, 1);
                }

                imgDraft = Filter.ThresholdBinary(imgDraft, 150);
                output = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(input) + "_thres.bmp");
                imgDraft.SaveToBitmap(output);

                //break;
            }
        }

        [TestMethod]
        public void TestAlignment_PoleTip()
        {
            string settingFilename = @"d:\Projects\Xyratex\Data\Alignment\Settings\Alignment_PoleTip.settings";
            string inputFolder = @"d:\Projects\Xyratex\Data\Alignment\Settings\input";
            string outputFolder = @"d:\Projects\Xyratex\Data\Alignment\Settings\output";

            string[] files = Directory.GetFiles(inputFolder);

            Console.WriteLine("Setting: {0}\nInput: {1}\nOutput: {2}",
                settingFilename, inputFolder, outputFolder);

            Settings settings = Settings.Deserialize(settingFilename);
            Aligner aligner = new Aligner(settings);

            foreach (string input in files)
            {
                TestContext.WriteLine(Path.GetFileName(input));

                GreyDataImage image = GreyDataImage.FromFile(input);

                AlignmentResult alignment = aligner.Align_PoleTip(image);

                System.Drawing.Drawing2D.Matrix inverseTransform = null;

                GreyDataImage result = ImageInterpolator.AffineTransform(InterpolationMethod.Bilinear, image, alignment.SourceCoordinates,
                    settings.NewWidth, settings.NewHeight, ref inverseTransform);

                string output = Path.Combine(outputFolder, Path.GetFileName(input));
                result.SaveToBitmap(output);
            }
        }

        #endregion

        #region Setting
        [TestMethod]
        public void CreateAlignmentSettings()
        {
            string filename = @"d:\Projects\Xyratex\Data\Alignment\Settings\Alignment.settings";
            Console.WriteLine("Create default settings to: {0}", filename);

            Settings settings = new Settings();
            settings.IntensityThreshold = 50;
            settings.MinCoorelationCoefficient = 0.5;
            settings.MinAffectedKeypoints = 8;
            settings.KeyColumns = new int[] {160, 180, 200, 220, 240, 260, 280, 300, 320, 340, 360, 380, 400, 420, 440, 460, 480, 500, 
                520, 540, 560, 580, 600, 620, 640, 660, 680, 700, 720, 740, 760, 780, 800, 820, 840, 860, 880, 900, 920, 940, 960, 980, 
                1000, 1020, 1040, 1060, 1080, 1100, 1120, 1140, 1160, 1180, 1200, 1220, 1240, 1260, 1280, 1300, 1320, 1340, 1360, 1380, 
                1400, 1420, 1440, 1460, 1480, 1500, 1520, 1540, 1560, 1580, 1600, 1620, 1640, 1660, 1680, 1700, 1720, 1740, 1760, 1780, 
                1800, 1820, 1840, 1860, 1880, 1900, 1920, 1940, 1960, 1980, 2000, 2020, 2040, 2060, 2080, 2100, 2120, 2140, 2160, 2180, 2200};

            settings.KeyRowsPoleTip = new int[] { };

            settings.NewWidth = 2237;
            settings.NewHeight = 1380;
            settings.SampleWidth = 41;
            settings.SampleHeight = 41;
            settings.SampleExpandWidth = 10;
            settings.SampleExpandHeight = 10;

            settings.Serialize(filename);
        }

        [TestMethod]
        public void CreateAlignmentSettings_PoleTip()
        {
            string filename = @"d:\Projects\Xyratex\Data\Alignment\Settings\Alignment_PoleTip.settings";
            Console.WriteLine("Create default settings to: {0}", filename);

            Settings settings = new Settings();
            settings.IntensityThreshold = 20;
            settings.MinCoorelationCoefficient = 0.5;
            settings.MinAffectedKeypoints = 4;
            settings.KeyColumns = new int[] { };

            settings.KeyRowsPoleTip = new int[] {60, 80, 100, 120, 140, 160, 180, 200, 220, 240, 260, 280, 300, 320, 340, 360, 380, 400, 420, 440, 460, 480, 500, 
                520, 540, 560, 580, 600, 620, 640, 660, 680, 700, 720, 740, 760, 780, 800, 820, 840, 860, 880, 900, 920, 940, 960, 980, 
                1000, 1020, 1040, 1060, 1080, 1100, 1120, 1140, 1160, 1180, 1200, 1220, 1240, 1260, 1280, 1300, 1320, 1340, 1360, 1380, 
                1400, 1420, 1440, 1460, 1480, 1500, 1520, 1540, 1560, 1580, 1600, 1620, 1640, 1660, 1680, 1700, 1720, 1740, 1760, 1780, 
                1800, 1820, 1840, 1860, 1880, 1900};

            settings.NewWidth = 2456;
            settings.NewHeight = 2058;
            settings.SampleWidth = 151;
            settings.SampleHeight = 151;
            settings.SampleExpandWidth = 40;
            settings.SampleExpandHeight = 40;

            settings.Serialize(filename);
        }

        [TestMethod]
        public void CreateAlignmentSettings_Depo()
        {
            string filename = @"d:\Projects\Xyratex\Data\Alignment\Settings\Depo\Alignment_Depo.settings";
            Console.WriteLine("Create default settings to: {0}", filename);

            DepoAlignmentSettings setting = new DepoAlignmentSettings();
            setting.Serialize(filename);
        }

        [TestMethod]
        public void CreateLinePatternOutput()
        {
            string imageInputFolder = @"d:\Projects\Xyratex\Data\Test\ImageOriginal";
            string imageTemplatePath = @"d:\Projects\Xyratex\Data\Test\Template.bmp";
            string libFile = @"d:\Projects\Xyratex\Data\Test\LineThickWidthTopLeft.txt";
            string outputFolder = @"d:\Projects\Xyratex\Data\Test\output\";

            LinePatternLibrary lib = new LinePatternLibrary();
            lib.LoadFromText(libFile);
            ABSPatternClassifier classifier = new ABSPatternClassifier();
            List<LineClassifyingInput> lines = null;
            lines = classifier.CreateInput(lib);

            string[] filesInput = Directory.GetFiles(imageInputFolder);

            foreach (string imgFile in filesInput)
            {
                GreyDataImage image = GreyDataImage.FromFile(imgFile);
                unsafe
                {
                    classifier.GetLineClassifyingInput(image._aData, image._width, image._height, lines);
                }
                if (lines == null || lines.Count == 0)
                {
                    TestContext.WriteLine("There is no pattern");
                    return;
                }
                string outSubFolder = outputFolder + Path.GetFileName(imgFile).Replace(".bmp", @"\");
                Directory.CreateDirectory(outSubFolder);
                LineClassifer.WriteInputToBitmap(lines, outSubFolder);
            }
        }

        [TestMethod]
        public void ConvertTextLinePatternLibraryToBinary()
        {
            string textLibFolder = @"d:\Projects\Xyratex\Data\Test\TextLibrary";
            string binLibFolder = @"d:\Projects\Xyratex\Data\Test\BinLibrary";

            string[] files = Directory.GetFiles(textLibFolder, "*.txt");
            foreach (string file in files)
            {
                TestContext.WriteLine(Path.GetFileName(file));

                LinePatternLibrary lib = new LinePatternLibrary();
                lib.LoadFromText(file);

                lib.Serialize(Path.Combine(binLibFolder, Path.GetFileNameWithoutExtension(file) + ".settings"));
            }
        }

        [TestMethod]
        public void UpdateAlignmentPatterns()
        {
            string filename = @"d:\Projects\Xyratex\Data\Alignment\Settings\Alignment.settings";
            string keyPointList = @"d:\Projects\Xyratex\Data\Alignment\Settings\keypoints.csv";
            string templateImage = @"d:\Projects\Xyratex\Data\Alignment\Settings\TemplateImage.bmp";
            string outputTemplate = @"d:\Projects\Xyratex\Data\Alignment\Settings\output\";

            Console.WriteLine("Update keypoints: {0}", filename);

            List<int> xlist = new List<int>();
            List<int> ylist = new List<int>();
            using (StreamReader stream = new StreamReader(keyPointList))
            {                
                string line = null;
                while ((line = stream.ReadLine()) != null)
                {
                    string[] lines = line.Split(',');
                    if (lines.Length != 2)
                        break;
                    xlist.Add(Int32.Parse(lines[0]));
                    ylist.Add(Int32.Parse(lines[1]));
                }
            }

            Settings settings = Settings.Deserialize(filename);

            settings.SampleExpandHeight = 20;
            settings.SampleExpandWidth = 20;

            GreyDataImage image = GreyDataImage.FromFile(templateImage);
            int len = xlist.Count;
            settings.SampleCount = len;
            settings.SampleXCoordinates = xlist.ToArray();
            settings.SampleYCoordinates = ylist.ToArray();
            settings.SampleData = new ushort[len][];
            settings.SampleWeightSet = new double[len][];
            settings.SampleMeans = new double[len];
            settings.SampleStds = new double[len];
            int width = settings.SampleWidth;
            int height = settings.SampleHeight;
            int halfSampleWidth = width / 2;
            int halfSampleHeight = height / 2;
            
            double mean = 0, std = 0;

            unsafe
            {
                for (int i = 0; i < len; i++)
                {
                    ushort[] sampleData = new ushort[width * height];
                    TestContext.WriteLine("[{0},{1}]", xlist[i], ylist[i]);

                    SIA.Algorithms.Preprocessing.Misc.GetData(image, xlist[i] - halfSampleWidth, ylist[i] - halfSampleHeight, width, height, sampleData,
                        ref mean, ref std);
                    settings.SampleData[i] = sampleData;
                    settings.SampleWeightSet[i] = new double[width * height];
                    settings.SampleMeans[i] = mean;
                    settings.SampleStds[i] = std;

                    fixed (ushort* pdata = sampleData)
                    {
                        GreyDataImage result = new GreyDataImage(settings.SampleWidth, settings.SampleHeight, pdata);

                        result.SaveToBitmap(Path.Combine(outputTemplate, i.ToString("00") + ".bmp"));
                    }
                }

            }
            settings.Serialize(filename);
        }

        [TestMethod]
        public void UpdateAlignmentPatterns_Depo()
        {
            UpdateAlignmentHaarPatterns_Depo();
            UpdateAlignmentCorrelationPatterns_Depo();
        }

        [TestMethod]
        public void UpdateAlignmentHaarPatterns_Depo()
        {
            string filename = @"d:\Projects\Xyratex\Data\Alignment\Settings\Depo\Alignment_Depo.settings";
            string keyPointList = @"d:\Projects\Xyratex\Data\Alignment\Settings\Depo\haarPtns_Depo.csv";
            string templateImage = @"d:\Projects\Xyratex\Data\Alignment\Settings\Depo\TemplateImage_Depo.bmp";
            string outputTemplate = @"d:\Projects\Xyratex\Data\Alignment\Settings\Depo\output\";

            DepoAlignmentSettings settings = DepoAlignmentSettings.Deserialize(filename);

            settings.HaarPatterns = new List<HaarPattern>();

            int left = 0, top = 0;
            using (StreamReader stream = new StreamReader(keyPointList))
            {
                string line = null;
                while ((line = stream.ReadLine()) != null)
                {
                    string[] lines = line.Split(',');
                    int OFFSETREAD = 6;
                    if (lines.Length < OFFSETREAD)
                        break;
                    HaarPattern ptn = new HaarPattern();
                    left = Int32.Parse(lines[0]);
                    top = Int32.Parse(lines[1]);
                    ptn.Width = Int32.Parse(lines[2]);
                    ptn.Height = Int32.Parse(lines[3]);
                    ptn.ShiftToCenterX = Int32.Parse(lines[4]);
                    ptn.ShiftToCenterY = Int32.Parse(lines[5]);

                    ptn.PositiveRectPoints = new int[lines.Length - OFFSETREAD];
                    for (int i = OFFSETREAD; i < lines.Length; i++)
                    {
                        ptn.PositiveRectPoints[i - OFFSETREAD] = Int32.Parse(lines[i]);
                    }
                    settings.HaarPatterns.Add(ptn);
                }
            }

            GreyDataImage image = GreyDataImage.FromFile(templateImage);
            for (int i = 0; i < settings.HaarPatterns.Count; i++)
            {
                int width = settings.HaarPatterns[i].Width;
                int height = settings.HaarPatterns[i].Height;
                double mean = 0.0, std = 0.0;
                unsafe
                {
                    settings.HaarPatterns[i].Data = new ushort[width * height];
                    SIA.Algorithms.Preprocessing.Misc.GetData(image, left, top, width, height, 
                        settings.HaarPatterns[i].Data, ref mean, ref std);
                    fixed (ushort* pdata = settings.HaarPatterns[i].Data)
                    {
                        GreyDataImage result = new GreyDataImage(width, height, pdata);
                        result.SaveToBitmap(Path.Combine(outputTemplate, "haar.bmp"));
                    }
                    settings.HaarPatterns[i].BuildEmbededParameters(width);
                    settings.HaarPatterns[i].EigenVal = settings.HaarPatterns[i].CalcEigenValue();
                }
            }
            
            settings.Serialize(filename);
        }

        [TestMethod]
        public void UpdateAlignmentCorrelationPatterns_Depo()
        {
            string filename = @"d:\Projects\Xyratex\Data\Alignment\Settings\Depo\Alignment_Depo.settings";
            string keyPointList = @"d:\Projects\Xyratex\Data\Alignment\Settings\Depo\corrPtns_Depo.csv";
            string templateImage = @"d:\Projects\Xyratex\Data\Alignment\Settings\Depo\TemplateImage_Depo.bmp";
            string outputTemplate = @"d:\Projects\Xyratex\Data\Alignment\Settings\Depo\output\";

            DepoAlignmentSettings settings = DepoAlignmentSettings.Deserialize(filename);

            settings.CorrPatterns = new List<CorrelationPattern>();

            using (StreamReader stream = new StreamReader(keyPointList))
            {
                string line = null;
                while ((line = stream.ReadLine()) != null)
                {
                    string[] lines = line.Split(',');
                    if (lines.Length < 4)
                        break;
                    CorrelationPattern ptn = new CorrelationPattern();
                    ptn.Left = Int32.Parse(lines[0]);
                    ptn.Top = Int32.Parse(lines[1]);
                    ptn.Width = Int32.Parse(lines[2]);
                    ptn.Height = Int32.Parse(lines[3]);
                    ptn.ExpandWidth = Int32.Parse(lines[4]);
                    ptn.ExpandHeight = Int32.Parse(lines[5]);
                    
                    settings.CorrPatterns.Add(ptn);
                }
            }

            GreyDataImage image = GreyDataImage.FromFile(templateImage);
            int len = settings.CorrPatterns.Count;
            unsafe
            {
                for (int i = 0; i < len; i++)
                {
                    int width = settings.CorrPatterns[i].Width;
                    int height = settings.CorrPatterns[i].Height;
                    int halfSampleWidth = width / 2;
                    int halfSampleHeight = height / 2;

                    settings.CorrPatterns[i].Data = new ushort[width * height];

                    SIA.Algorithms.Preprocessing.Misc.GetData(image, settings.CorrPatterns[i].Left - halfSampleWidth, settings.CorrPatterns[i].Top - halfSampleHeight, width, height, settings.CorrPatterns[i].Data,
                        ref settings.CorrPatterns[i].Mean, ref settings.CorrPatterns[i].Std);
                    settings.CorrPatterns[i].PrepareDefaultWeightSet();
                    settings.CorrPatterns[i].ComputeMeanStd();

                    fixed (ushort* pdata = settings.CorrPatterns[i].Data)
                    {
                        GreyDataImage result = new GreyDataImage(width, height, pdata);
                        try
                        {
                            GreyDataImage imgWS = GreyDataImage.FromFile(Path.Combine(outputTemplate, i.ToString("00") + "_.bmp"));
                            settings.CorrPatterns[i].CreateWeightSet(imgWS._aData, width, height, 100, 5);
                        }
                        catch (System.Exception ex)
                        {
                            settings.CorrPatterns[i].PrepareDefaultWeightSet();
                        }
                        result.SaveToBitmap(Path.Combine(outputTemplate, i.ToString("00") + ".bmp"));
                    }
                }

            }
            settings.Serialize(filename);
        }

        [TestMethod]
        public void UpdateAlignmentPatterns_PoleTip()
        {
            string filename = @"d:\Projects\Xyratex\Data\Alignment\Settings\Alignment_PoleTip.settings";
            string keyPointList = @"d:\Projects\Xyratex\Data\Alignment\Settings\keypoints_PoleTip.csv";
            string templateImage = @"d:\Projects\Xyratex\Data\Alignment\Settings\TemplateImage_PoleTip.bmp";
            string outputTemplate = @"d:\Projects\Xyratex\Data\Alignment\Settings\output\";

            Console.WriteLine("Update keypoints: {0}", filename);

            List<int> xlist = new List<int>();
            List<int> ylist = new List<int>();
            using (StreamReader stream = new StreamReader(keyPointList))
            {
                string line = null;
                while ((line = stream.ReadLine()) != null)
                {
                    string[] lines = line.Split(',');
                    if (lines.Length != 2)
                        break;
                    xlist.Add(Int32.Parse(lines[0]));
                    ylist.Add(Int32.Parse(lines[1]));
                }
            }

            Settings settings = Settings.Deserialize(filename);
            GreyDataImage image = GreyDataImage.FromFile(templateImage);
            int len = xlist.Count;
            settings.SampleCount = len;
            settings.SampleXCoordinates = xlist.ToArray();
            settings.SampleYCoordinates = ylist.ToArray();
            settings.SampleData = new ushort[len][];
            settings.SampleWeightSet = new double[len][];
            settings.SampleMeans = new double[len];
            settings.SampleStds = new double[len];
            int width = settings.SampleWidth;
            int height = settings.SampleHeight;
            int halfSampleWidth = width / 2;
            int halfSampleHeight = height / 2;

            double mean = 0, std = 0;

            List<double[]> WeightSetList = new List<double[]>();

            unsafe
            {
                for (int i = 0; i < len; i++)
                {
                    ushort[] sampleData = new ushort[width * height];
                    TestContext.WriteLine("[{0},{1}]", xlist[i], ylist[i]);

                    SIA.Algorithms.Preprocessing.Misc.GetData(image, xlist[i] - halfSampleWidth, ylist[i] - halfSampleHeight, width, height, sampleData,
                        ref mean, ref std);
                    settings.SampleData[i] = sampleData;
                    settings.SampleMeans[i] = mean;
                    settings.SampleStds[i] = std;

                    fixed (ushort* pdata = sampleData)
                    {
                        GreyDataImage result = new GreyDataImage(settings.SampleWidth, settings.SampleHeight, pdata);
                        GreyDataImage imgWS = GreyDataImage.FromFile(Path.Combine(outputTemplate, i.ToString("00") + "_.bmp"));
                        settings.SampleWeightSet[i] = CreateWeightSet(imgWS, 100);
                        result.SaveToBitmap(Path.Combine(outputTemplate, i.ToString("00") + ".bmp"));
                    }
                }

            }
            settings.Serialize(filename);
        }

        [TestMethod]
        public void GetAnchorGoldenImage()
        {
            string filename = @"d:\Projects\Xyratex\Data\Alignment\Settings\TemplateImage_PoleTip.bmp";
            string settingFilename = @"d:\Projects\Xyratex\Data\Alignment\Settings\Alignment_PoleTip.settings";
            Settings settings = Settings.Deserialize(settingFilename);
            Aligner aligner = new Aligner(settings);

            settings.IntensityThreshold = 200;

            double anchorX = 0.0;
            double anchorY = 0.0;

            GreyDataImage image = GreyDataImage.FromFile(filename);
            aligner.ScanWhiteRegion_PoleTip(image, 600, 1400, 2105, 2436, ref anchorX, ref anchorY);

            settings.TopScanWhite = 600;
            settings.BottomScanWhite = 1300;
            settings.LeftScanWhite = 1700;
            settings.RightScanWhite = 2300;

            settings.SampleExpandHeight = 10;
            settings.SampleExpandWidth = 10;

            settings.AnchorX = anchorX;
            settings.AnchorY = anchorY;

            settings.Serialize(settingFilename);

        }

        private double[] CreateWeightSet(GreyDataImage image, ushort thres)
        {
            int width = image._width;
            int height = image._height;

            double[] ws = new double[width * height];
            unsafe
            {
                ushort* greyBuffer = image._aData;
                ushort* pData = greyBuffer;
                int wsIdx = 0;

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++, pData++, wsIdx++)
                    {
                        if (*pData < thres)
                            ws[wsIdx] = 5;
                        else
                            ws[wsIdx] = 1;
                    }
                }
            }
            return ws;
        }
        #endregion

        [TestMethod]
        public void TestCorrelationMatcher()
        {
            string filename = @"d:\Projects\Xyratex\Data\Alignment\Settings\TemplateImage.bmp";
            string settingFilename = @"d:\Projects\Xyratex\Data\Alignment\Settings\Alignment.settings";
            Settings settings = Settings.Deserialize(settingFilename);
            Aligner aligner = new Aligner(settings);
            GreyDataImage image = GreyDataImage.FromFile(filename);

            int roiWidth = settings.SampleWidth + 2 * settings.SampleExpandWidth;
            int roiHeight = settings.SampleHeight + 2 * settings.SampleExpandHeight;
            int halfRoiWidth = roiWidth / 2;
            int halfRoiHeight = roiHeight / 2;

            ushort[] roiData = new ushort[roiWidth * roiHeight];

            List<double> corrCoeffList = new List<double>();

            for (int isample = 0; isample < settings.SampleCount; isample++)
            {
                System.Drawing.Drawing2D.Matrix getDraft = new System.Drawing.Drawing2D.Matrix();
                getDraft.Translate(settings.SampleXCoordinates[isample] - halfRoiWidth,
                    settings.SampleYCoordinates[isample] - halfRoiHeight, System.Drawing.Drawing2D.MatrixOrder.Prepend);

                unsafe
                {
                    fixed (ushort* proiData = roiData)
                    {

                        SIA.Algorithms.Preprocessing.Interpolation.ImageInterpolator.AffineTransform(InterpolationMethod.Bilinear,
                            image._aData, image._width, image._height, getDraft, proiData, roiWidth, roiHeight);
                    }
                }

                int shiftX = 0, shiftY = 0;
                double corrCoeff = 0;
                CorrelationMatcher corrMatcher = new CorrelationMatcher();
                corrMatcher.Match(
                    roiData, roiWidth, roiHeight,
                    settings.SampleData[isample], settings.SampleWidth, settings.SampleHeight,
                    null,
                    settings.MinCoorelationCoefficient,
                    out corrCoeff, out shiftX, out shiftY);
                corrCoeffList.Add(corrCoeff);
            }
        }

        [TestMethod]
        public void TestSpiral()
        {
            int[][] result = null;
            List<Point> offsetList = null;
            List<Int32> radiusList = null;
            CreateSpiralMatrix(4, ref result, ref offsetList, ref radiusList);
        }

        public bool CreateSpiralMatrix(int n, ref int[][] result, ref List<Point> offsetList, ref List<Int32> radiusList)
        {
            result = new int[n][];
            for (int i = n - 1; i >= 0; i--)
                result[i] = new int[n];
            offsetList = new List<Point>();
            radiusList = new List<Int32>();

            int pos = 0;
            int count = n;
            int value = -n;
            int sum = -1;

            int m = n;
            int radius = n / 2 + 1;
            int radius_ele = 4 * m - 4; // m^2 - (m-2)^2;
            int count_ele = 1;

            do
            {
                value = -1 * value / n;
                for (int i = 0; i < count; i++)
                {
                    sum += value;
                    AddSpiralEle(ref result, ref offsetList, ref radiusList, sum / n, sum % n, pos++, ref count_ele, ref radius_ele, ref radius, ref m);
                }
                value *= n;
                count--;
                for (int i = 0; i < count; i++)
                {
                    sum += value;
                    AddSpiralEle(ref result, ref offsetList, ref radiusList, sum / n, sum % n, pos++, ref count_ele, ref radius_ele, ref radius, ref m);
                }
            } while (count > 0);

            return true;
        }

        public void AddSpiralEle(ref int[][] result, ref List<Point> offsetList, ref List<Int32> radiusList, int i, int j, int value, ref int count_ele, ref int radius_ele, ref int radius, ref int m)
        {
            result[i][j] = value;
            offsetList.Add(new Point(i, j));
            radiusList.Add(radius);
            count_ele++;
            if (count_ele > radius_ele)
            {
                m -= 2;
                radius_ele = 4 * m - 4; // m^2 - (m-2)^2;
                count_ele = 1;
                radius--;
            }
        }
        
        #region IR
        [TestMethod]
        public void UpdateAlignmentPatternsIR()
        {
            string filename = @"d:\Projects\Xyratex\Data\Alignment\Settings\IR\Alignment.settings";
            string keyPointList = @"d:\Projects\Xyratex\Data\Alignment\Settings\IR\keypoints1.csv";
            string templateImage = @"d:\Projects\Xyratex\Data\Alignment\Settings\IR\TemplateImage1.bmp";
            string outputTemplate = @"d:\Projects\Xyratex\Data\Alignment\Settings\IR\output\";

            Console.WriteLine("Update keypoints: {0}", filename);

            List<int> xlist = new List<int>();
            List<int> ylist = new List<int>();
            using (StreamReader stream = new StreamReader(keyPointList))
            {
                string line = null;
                while ((line = stream.ReadLine()) != null)
                {
                    string[] lines = line.Split(',');
                    if (lines.Length != 2)
                        break;
                    xlist.Add(Int32.Parse(lines[0]));
                    ylist.Add(Int32.Parse(lines[1]));
                }
            }

            Settings settings = Settings.Deserialize(filename);

            settings.SampleExpandWidth = 150;
            settings.SampleExpandHeight = 100;
            settings.SampleHeight = 40;
            settings.SampleWidth = 40;

            GreyDataImage image = GreyDataImage.FromFile(templateImage);
            int len = xlist.Count;
            settings.SampleCount = len;
            settings.SampleXCoordinates = xlist.ToArray();
            settings.SampleYCoordinates = ylist.ToArray();
            settings.SampleData = new ushort[len][];
            settings.SampleMeans = new double[len];
            settings.SampleStds = new double[len];
            int width = settings.SampleWidth;
            int height = settings.SampleHeight;
            int halfSampleWidth = width / 2;
            int halfSampleHeight = height / 2;

            double mean = 0, std = 0;

            unsafe
            {
                for (int i = 0; i < len; i++)
                {
                    ushort[] sampleData = new ushort[width * height];
                    TestContext.WriteLine("[{0},{1}]", xlist[i], ylist[i]);

                    SIA.Algorithms.Preprocessing.Misc.GetData(image, xlist[i] - halfSampleWidth, ylist[i] - halfSampleHeight, width, height, sampleData,
                        ref mean, ref std);
                    settings.SampleData[i] = sampleData;
                    settings.SampleMeans[i] = mean;
                    settings.SampleStds[i] = std;

                    fixed (ushort* pdata = sampleData)
                    {
                        GreyDataImage result = new GreyDataImage(settings.SampleWidth, settings.SampleHeight, pdata);

                        result.SaveToBitmap(Path.Combine(outputTemplate, i.ToString("00") + ".bmp"));
                    }
                }

            }
            settings.Serialize(filename);
        }

        [TestMethod]
        public void UpdateAlignmentPatternsIR3()
        {
            string filename = @"d:\Projects\Xyratex\Data\Alignment\Settings\IR\Alignment.settings";
            string keyPointList = @"d:\Projects\Xyratex\Data\Alignment\Settings\IR\keypoints3.csv";
            string templateImage = @"d:\Projects\Xyratex\Data\Alignment\Settings\IR\TemplateImage3.bmp";
            string outputTemplate = @"d:\Projects\Xyratex\Data\Alignment\Settings\IR\output\";

            Console.WriteLine("Update keypoints: {0}", filename);

            List<int> xlist = new List<int>();
            List<int> ylist = new List<int>();
            using (StreamReader stream = new StreamReader(keyPointList))
            {
                string line = null;
                while ((line = stream.ReadLine()) != null)
                {
                    string[] lines = line.Split(',');
                    if (lines.Length != 2)
                        break;
                    xlist.Add(Int32.Parse(lines[0]));
                    ylist.Add(Int32.Parse(lines[1]));
                }
            }

            Settings settings = Settings.Deserialize(filename);

            settings.SampleExpandHeight = 3;
            settings.SampleExpandWidth = 3;
            settings.SampleHeight = 10;
            settings.SampleWidth = 10;

            GreyDataImage image = GreyDataImage.FromFile(templateImage);
            int len = xlist.Count;
            settings.SampleCount = len;
            settings.SampleXCoordinates = xlist.ToArray();
            settings.SampleYCoordinates = ylist.ToArray();
            settings.SampleData = new ushort[len][];
            settings.SampleMeans = new double[len];
            settings.SampleStds = new double[len];
            int width = settings.SampleWidth;
            int height = settings.SampleHeight;
            int halfSampleWidth = width / 2;
            int halfSampleHeight = height / 2;

            double mean = 0, std = 0;

            unsafe
            {
                for (int i = 0; i < len; i++)
                {
                    ushort[] sampleData = new ushort[width * height];
                    TestContext.WriteLine("[{0},{1}]", xlist[i], ylist[i]);

                    SIA.Algorithms.Preprocessing.Misc.GetData(image, xlist[i] - halfSampleWidth, ylist[i] - halfSampleHeight, width, height, sampleData,
                        ref mean, ref std);
                    settings.SampleData[i] = sampleData;
                    settings.SampleMeans[i] = mean;
                    settings.SampleStds[i] = std;

                    fixed (ushort* pdata = sampleData)
                    {
                        GreyDataImage result = new GreyDataImage(settings.SampleWidth, settings.SampleHeight, pdata);

                        result.SaveToBitmap(Path.Combine(outputTemplate, i.ToString("00") + ".bmp"));
                    }
                }

            }
            settings.Serialize(filename);
        }

        [TestMethod]
        public void UpdateAlignmentPatternsIR4()
        {
            string filename = @"d:\Projects\Xyratex\Data\Alignment\Settings\IR\Alignment.settings";
            string keyPointList = @"d:\Projects\Xyratex\Data\Alignment\Settings\IR\keypoints4.csv";
            string templateImage = @"d:\Projects\Xyratex\Data\Alignment\Settings\IR\TemplateImage4.bmp";
            string outputTemplate = @"d:\Projects\Xyratex\Data\Alignment\Settings\IR\output\";

            Console.WriteLine("Update keypoints: {0}", filename);

            List<int> xlist = new List<int>();
            List<int> ylist = new List<int>();
            using (StreamReader stream = new StreamReader(keyPointList))
            {
                string line = null;
                while ((line = stream.ReadLine()) != null)
                {
                    string[] lines = line.Split(',');
                    if (lines.Length != 2)
                        break;
                    xlist.Add(Int32.Parse(lines[0]));
                    ylist.Add(Int32.Parse(lines[1]));
                }
            }

            Settings settings = Settings.Deserialize(filename);

            settings.SampleExpandHeight = 3;
            settings.SampleExpandWidth = 3;
            settings.SampleHeight = 10;
            settings.SampleWidth = 10;

            GreyDataImage image = GreyDataImage.FromFile(templateImage);
            int len = xlist.Count;
            settings.SampleCount = len;
            settings.SampleXCoordinates = xlist.ToArray();
            settings.SampleYCoordinates = ylist.ToArray();
            settings.SampleData = new ushort[len][];
            settings.SampleMeans = new double[len];
            settings.SampleStds = new double[len];
            int width = settings.SampleWidth;
            int height = settings.SampleHeight;
            int halfSampleWidth = width / 2;
            int halfSampleHeight = height / 2;

            double mean = 0, std = 0;

            unsafe
            {
                for (int i = 0; i < len; i++)
                {
                    ushort[] sampleData = new ushort[width * height];
                    TestContext.WriteLine("[{0},{1}]", xlist[i], ylist[i]);

                    SIA.Algorithms.Preprocessing.Misc.GetData(image, xlist[i] - halfSampleWidth, ylist[i] - halfSampleHeight, width, height, sampleData,
                        ref mean, ref std);
                    settings.SampleData[i] = sampleData;
                    settings.SampleMeans[i] = mean;
                    settings.SampleStds[i] = std;

                    fixed (ushort* pdata = sampleData)
                    {
                        GreyDataImage result = new GreyDataImage(settings.SampleWidth, settings.SampleHeight, pdata);

                        result.SaveToBitmap(Path.Combine(outputTemplate, i.ToString("00") + ".bmp"));
                    }
                }

            }
            settings.Serialize(filename);
        }

        [TestMethod]
        public void UpdateAlignmentPatternsIR5()
        {
            string filename = @"d:\Projects\Xyratex\Data\Alignment\Settings\IR\Alignment.settings";
            string keyPointList = @"d:\Projects\Xyratex\Data\Alignment\Settings\IR\keypoints5.csv";
            string templateImage = @"d:\Projects\Xyratex\Data\Alignment\Settings\IR\TemplateImage5.bmp";
            string outputTemplate = @"d:\Projects\Xyratex\Data\Alignment\Settings\IR\output\";

            Console.WriteLine("Update keypoints: {0}", filename);

            List<int> xlist = new List<int>();
            List<int> ylist = new List<int>();
            using (StreamReader stream = new StreamReader(keyPointList))
            {
                string line = null;
                while ((line = stream.ReadLine()) != null)
                {
                    string[] lines = line.Split(',');
                    if (lines.Length != 2)
                        break;
                    xlist.Add(Int32.Parse(lines[0]));
                    ylist.Add(Int32.Parse(lines[1]));
                }
            }

            Settings settings = Settings.Deserialize(filename);

            settings.SampleExpandHeight = 3;
            settings.SampleExpandWidth = 3;
            settings.SampleHeight = 10;
            settings.SampleWidth = 10;

            GreyDataImage image = GreyDataImage.FromFile(templateImage);
            int len = xlist.Count;
            settings.SampleCount = len;
            settings.SampleXCoordinates = xlist.ToArray();
            settings.SampleYCoordinates = ylist.ToArray();
            settings.SampleData = new ushort[len][];
            settings.SampleMeans = new double[len];
            settings.SampleStds = new double[len];
            int width = settings.SampleWidth;
            int height = settings.SampleHeight;
            int halfSampleWidth = width / 2;
            int halfSampleHeight = height / 2;

            double mean = 0, std = 0;

            unsafe
            {
                for (int i = 0; i < len; i++)
                {
                    ushort[] sampleData = new ushort[width * height];
                    TestContext.WriteLine("[{0},{1}]", xlist[i], ylist[i]);

                    SIA.Algorithms.Preprocessing.Misc.GetData(image, xlist[i] - halfSampleWidth, ylist[i] - halfSampleHeight, width, height, sampleData,
                        ref mean, ref std);
                    settings.SampleData[i] = sampleData;
                    settings.SampleMeans[i] = mean;
                    settings.SampleStds[i] = std;

                    fixed (ushort* pdata = sampleData)
                    {
                        GreyDataImage result = new GreyDataImage(settings.SampleWidth, settings.SampleHeight, pdata);

                        result.SaveToBitmap(Path.Combine(outputTemplate, i.ToString("00") + ".bmp"));
                    }
                }

            }
            settings.Serialize(filename);
        }

        [TestMethod]
        public void TestAlignmentIR()
        {//used for case 1 3 4 5
            string settingFilename = @"d:\Projects\Xyratex\Data\Alignment\Settings\IR\Alignment.settings";
            string inputFolder = @"d:\Projects\Xyratex\Data\Alignment\Settings\IR\input\";
            string outputFolder = @"d:\Projects\Xyratex\Data\Alignment\Settings\IR\output\";

            string[] files = Directory.GetFiles(inputFolder);

            Console.WriteLine("Setting: {0}\nInput: {1}\nOutput: {2}",
                settingFilename, inputFolder, outputFolder);

            Settings settings = Settings.Deserialize(settingFilename);
            settings.MinAffectedKeypoints = 1;

            Aligner aligner = new Aligner(settings);
                foreach (string input in files)
                {
                    TestContext.WriteLine(Path.GetFileName(input));

                    GreyDataImage image = GreyDataImage.FromFile(input);
                    GreyDataImage imageTest = Filter.ThresholdBinary(image, 100, true, Filter.GetFullRectImage(image));

                    System.Drawing.Drawing2D.Matrix matAlign = aligner.AlignUsingMultipeKeypoints_PoleTip(imageTest, new System.Drawing.Drawing2D.Matrix());
                    //System.Drawing.Drawing2D.Matrix matAlign = aligner.AlignUsingMultipeKeypoints(image, new System.Drawing.Drawing2D.Matrix());
                    System.Drawing.Drawing2D.Matrix matTransform = new System.Drawing.Drawing2D.Matrix();
                    matTransform.Translate(matAlign.OffsetX, matAlign.OffsetY);

                    PointF[] resPoints = new PointF[] {
                    new PointF(0, 0), new PointF(image._width, 0), new PointF(0, image._height)};
                    matTransform.TransformPoints(resPoints);

                    GreyDataImage result = ImageInterpolator.AffineTransform(InterpolationMethod.Bilinear, image, resPoints,
                        image._width, image._height, ref matTransform);

                    string output = Path.Combine(outputFolder, Path.GetFileName(input));
                    result.SaveToBitmap(output);
                }
        }

        [TestMethod]
        public void TestAlignmentIR2()
        {
            string settingFilename = @"d:\Projects\Xyratex\Data\Alignment\Settings\IR\Alignment.settings";
            string inputFolder = @"d:\Projects\Xyratex\Data\Alignment\Settings\IR\input\";
            string outputFolder = @"d:\Projects\Xyratex\Data\Alignment\Settings\IR\output\";

            string[] files = Directory.GetFiles(inputFolder);

            Console.WriteLine("Setting: {0}\nInput: {1}\nOutput: {2}",
                settingFilename, inputFolder, outputFolder);

            Settings settings = Settings.Deserialize(settingFilename);
            settings.MinAffectedKeypoints = 1;
            settings.SampleExpandWidth = 150;
            settings.SampleExpandHeight = 100;
            /**/
            settings.SampleExpandWidth = 20;
            /**/
            settings.SampleExpandHeight = 5;
            Aligner aligner = new Aligner(settings);
            int threshold = 80;
            double angle = 0;
            double shift = 0;
            int avgCoord = 0;
            foreach (string input in files)
            {
                TestContext.WriteLine(Path.GetFileName(input));

                GreyDataImage image = GreyDataImage.FromFile(input);

                GreyDataImage template = Filter.ThresholdBinary(image, 100, true, Filter.GetFullRectImage(image));
                template.SaveToBitmap(Path.Combine(outputFolder, Path.GetFileName(input)));

                GreyDataImage imgTest = Filter.Median(image, 7, new Rectangle(new Point(0, 0), new Size(image._width, image._height)));
                imgTest.SaveToBitmap(@"D:\test0.bmp");

                //threshold = 100;
                //imgTest = Filter.Convolution(imgTest, Filter.Kernel_SobelX3);
                //avgCoord = aligner.ScanIRVerticalLine(imgTest, 0, imgTest._height, 1, threshold, ref angle, ref shift);

                threshold = 80;
                imgTest = Filter.Convolution(imgTest, Filter.Kernel_SobelY3);
                avgCoord = aligner.ScanIRHorizontalLine(imgTest, 100, 550, 1, threshold, ref angle, ref shift);
                imgTest.SaveToBitmap(@"D:\test1.bmp");
                
                imgTest = Filter.ThresholdBinary(imgTest, threshold);
                imgTest.SaveToBitmap(@"D:\test2.bmp");

                Console.WriteLine("{0} Coor: {1}", Path.GetFileName(input), avgCoord);
            }
        }

        [TestMethod]
        public void TestAlignmentIR3()
        {
            string settingFilename = @"d:\Projects\Xyratex\Data\Alignment\Settings\IR\Alignment.settings";
            string inputFolder = @"d:\Projects\Xyratex\Data\Alignment\Settings\IR\input\";
            string outputFolder = @"d:\Projects\Xyratex\Data\Alignment\Settings\IR\output\";

            string[] files = Directory.GetFiles(inputFolder);

            Console.WriteLine("Setting: {0}\nInput: {1}\nOutput: {2}",
                settingFilename, inputFolder, outputFolder);

            Settings settings = Settings.Deserialize(settingFilename);
            settings.MinAffectedKeypoints = 1;
            settings.SampleExpandWidth = 150;
            settings.SampleExpandHeight = 100;
            /**/
            settings.SampleExpandWidth = 20;
            /**/
            settings.SampleExpandHeight = 5;
            Aligner aligner = new Aligner(settings);
            int threshold = 100;
            foreach (string input in files)
            {
                TestContext.WriteLine(Path.GetFileName(input));

                GreyDataImage image = GreyDataImage.FromFile(input);
                GreyDataImage imgTest = Filter.ThresholdBinary(image, threshold, true, Filter.GetFullRectImage(image));
                imgTest.SaveToBitmap(@"D:\test0.bmp");

                Filter.MorphologyModes mode = Filter.MorphologyModes.Thinning;
                for (int loop = 0; loop < 10; loop++)
                {
                    imgTest = Filter.MorphologicalOperator(imgTest, Filter.Kernel_Morphology_Erosion, mode);
                    //imgTest = Filter.MorphologicalOperator(imgTest, Filter.Kernel_Morphology_Dilation, mode);

                    //imgTest = Filter.MorphologicalOperator(imgTest, Filter.Kernel_Morphology0, mode);
                    //imgTest = Filter.MorphologicalOperator(imgTest, Filter.Kernel_Morphology1, mode);
                    //imgTest = Filter.MorphologicalOperator(imgTest, Filter.Kernel_Morphology2, mode);
                    //imgTest = Filter.MorphologicalOperator(imgTest, Filter.Kernel_Morphology3, mode);
                    //imgTest = Filter.MorphologicalOperator(imgTest, Filter.Kernel_Morphology4, mode);
                    //imgTest = Filter.MorphologicalOperator(imgTest, Filter.Kernel_Morphology5, mode);
                    //imgTest = Filter.MorphologicalOperator(imgTest, Filter.Kernel_Morphology6, mode);
                    //imgTest = Filter.MorphologicalOperator(imgTest, Filter.Kernel_Morphology7, mode);
                }

                string output = Path.Combine(outputFolder, Path.GetFileName(input));
                imgTest.SaveToBitmap(output);
            }
        }

        [TestMethod]
        public void TestSubtraction()
        {
            string inputFolder = @"d:\Projects\Xyratex\Data\Alignment\Settings\IR\output\";
            string outputFolder = @"d:\Projects\Xyratex\Data\Alignment\Settings\IR\output\";
            string[] files = Directory.GetFiles(inputFolder);

            if (files.Length > 1)
            {
                GreyDataImage image0 = GreyDataImage.FromFile(files[0]);
                //image0 = Filter.Median(image0, 9, Filter.GetFullRectImage(image0));
                GreyDataImage image1 = GreyDataImage.FromFile(files[1]);
                //image1 = Filter.Median(image1, 9, Filter.GetFullRectImage(image1));
                
                GreyDataImage imageSub = Filter.Subtraction(image1, image0, true, Filter.GetFullRectImage(image0));

                string output = Path.Combine(outputFolder, @"zubtract.bmp");
                imageSub.SaveToBitmap(output);
            }
        }

        #endregion
    }
}

