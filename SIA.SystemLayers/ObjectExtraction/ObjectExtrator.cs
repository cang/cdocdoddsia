//#define _LENS_CORRECTION_ENABLED
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Collections;
using System.Diagnostics;

using SIA.Common;
using SIA.Common.Analysis;
using SIA.Common.Imaging;
using SIA.Common.Mask;
using SIA.Common.Utility;
using SIA.Common.KlarfExport;
using SiGlaz.RDE.Ex.Mask;
using SIA.SystemFrameworks;
using SIA.SystemFrameworks.UI;
using SIA.SystemLayer;
using SIA.SystemLayer.ImageProcessing;
using SIA.SystemLayer.Mask;
using SIA.IPEngine;
using SIA.SystemLayer.ObjectExtraction.Thresholders;
using SIA.SystemLayer.ObjectExtraction.Utilities;
using SIA.Common.IPLFacade;

namespace SIA.SystemLayer.ObjectExtraction
{
    /// <summary>
    /// Provides functionality to transform the gray data image into binary format
    /// </summary>
	internal interface IThresholder : IDisposable
	{
		String Name {get;}
		String Description {get;}
		
		bool CanExtract(ObjectDetectionSettings settings);
		BinaryImage Threshold(CommonImage image, ObjectDetectionSettings settings);
	};

    /// <summary>
    /// Provides functionality to filter the detected objects
    /// </summary>
	internal interface IObjectFilter
	{
		String Name {get;}
		String Description {get;}

		bool CanFilter(ObjectDetectionSettings settings);
		void Filter(ArrayList defects, ObjectDetectionSettings settings);
	}

	/// <summary>
	/// The ObjectExtractor class provides functions for detecting objects within a specified image.
	/// </summary>
	public class ObjectExtractor
	{		
		#region constructor and destructor
		
		public ObjectExtractor()
		{
		}

		#endregion

		#region defect extractor handlers
		private static ArrayList _registedThresholder = null;

		private static ArrayList Thresholders
		{
			get
			{
				if (_registedThresholder == null)
					InitializeThresholders();
				return _registedThresholder;
			}
		}

		private static void InitializeThresholders()
		{
			_registedThresholder = new ArrayList();
			RegisterThresholder(new StaticThresholder());
			RegisterThresholder(new DynamicThresholder());
			RegisterThresholder(new AutomaticThresholder());
			_registedThresholder.TrimToSize();
		}

		private static void RegisterThresholder(IThresholder thresholder)
		{
			lock (_registedThresholder)
				_registedThresholder.Add(thresholder);
		}

		private static IThresholder FindThresholder(ObjectDetectionSettings settings)
		{
			foreach(IThresholder extractor in ObjectExtractor.Thresholders)
				if (extractor.CanExtract(settings))
					return extractor;
			return null;
		}

		#endregion

        /// <summary>
        /// Detects objects for the specified image
        /// </summary>
        /// <param name="image">The image to detect</param>
        /// <param name="settings">The settings information</param>
        /// <returns></returns>
		public static ArrayList ExtractObjects(CommonImage image, ObjectDetectionSettings settings)
		{
			ArrayList detectedObjects = null;
			DetectedObjectCollection objects = null;

			IThresholder thresholder = null;
			BinaryImage binary_image = null;
			
			try
			{
                // initialize result list of detected objects
				detectedObjects = new ArrayList();

				// update callback status
                if (CommandProgress.IsAborting)
					throw new CommandAbortedException();

				// generate mask
				GenerateMask(image, settings);

				// update callback status
                if (CommandProgress.IsAborting)
					throw new CommandAbortedException();

				// [ 20070627 Cong] :
				// convert from DynamicCutOffPercentageThreshold into StaticThreshold
				if (settings.ThresholdType == ThresholdType.DynamicCutOfPercentageThreshold)
				{
					DynamicCutOffPercentageThresholdParameters t = (DynamicCutOffPercentageThresholdParameters)settings.ThresholdParameters;
					kHistogram histogram = null;
					bool bApplyOnAllImage = true;
					try
					{
						if (t.ApplyOnAllImage)
						{
							histogram = image.Histogram;
							bApplyOnAllImage = true;
						}
						else
						{
                            
						}

						// find static threshold parameters
						float fromPercentage = t.FromPercentage;
						float toPercentage = t.ToPercentage;
						double []hist = histogram.Histogram;
						double total = 0;
						int nBins = hist.Length;
						for (int i=0; i<nBins; i++)
						{
							total += hist[i];
						}
						int rankMin = (int)(total*fromPercentage*0.01);
						int rankMax = (int)(total*toPercentage*0.01);
						float minThreshold = 0;
						float maxThreshold = 0;

						image.FindRangeThreshold(ref minThreshold, ref maxThreshold, hist, rankMin, rankMax);

						// converting threshold type
						settings.ThresholdType = ThresholdType.StaticThreshold;
						StaticThresholdParameters staticThresholdParameters = new StaticThresholdParameters((int)minThreshold, (int)maxThreshold);
						settings.ThresholdParameters = staticThresholdParameters;						
					}
					catch (System.Exception exp)
					{
						Trace.WriteLine(exp);
						throw exp;
					}
					finally
					{
						if (histogram != null)
						{
							histogram.Dispose();
							histogram = null;
						}
					}

					// search for appreciated extractor
					thresholder = FindThresholder(settings);
					if (thresholder == null)
						throw new System.Exception("Cannot find appreciated thresholder. Please re-check settings");

					// update callback status
					if (CommandProgress.IsAborting  /*  || true )  Abort testing: */ )
						throw new CommandAbortedException();

					// threshold image using specified method
					if (bApplyOnAllImage)
					{
						binary_image = ((StaticThresholder)thresholder).Threshold(image, settings);
					}
					else
					{						
					}
				}
				else
				{
					// search for appreciated extractor
					thresholder = FindThresholder(settings);
					if (thresholder == null)
						throw new System.Exception("Cannot find appreciated thresholder. Please re-check settings");

					// update callback status
                    if (CommandProgress.IsAborting)
						throw new CommandAbortedException();

					// threshold image using specified method
					binary_image = thresholder.Threshold(image, settings);
				}

#if DUMP_BINARY_IMAGE
				try
				{
					string path = AppDomain.CurrentDomain.BaseDirectory + "binary_image.bmp";
					binary_image.Save(path);
				}
				catch
				{
					Trace.WriteLine("Failed to dump binary image");
				}
#endif
				// update callback status
				if (CommandProgress.IsAborting)
					throw new CommandAbortedException();

				// extract object from thresholded image
				IntPtr binary_buffer = binary_image.Buffer;

                int width = image.RasterImage.Width;
				int height = image.RasterImage.Height;

                // update progress callback
                CommandProgress.SetText("Detecting objects ...");

                // detect objects				
				objects = ObjectDetector.FindObjects(width, height, binary_buffer);

				// update callback status
                if (CommandProgress.IsAborting)
                    throw new CommandAbortedException();
                
                // update progress callback
                CommandProgress.SetText("Retreiving objects external data information...");
                CommandProgress.SetRange(0, 100);

				//[Nov 20 2006: calculate object external data such as: numPixels and totalIntensity
				if (objects.Count > 0)
				{
					//improving only:
					int maxNumContours = 0, maxNumPoints = 0;
					foreach (DetectedObject obj in objects)
					{
						if (obj.PolygonBoundary != null)
						{
							if(obj.PolygonBoundary.nContours > maxNumContours)
								maxNumContours = obj.PolygonBoundary.nContours;
							if(obj.PolygonBoundary.nPoints > maxNumPoints)
								maxNumPoints = obj.PolygonBoundary.nPoints;
						}
					}
					
					try
					{
						PolygonEx poly = new PolygonEx(maxNumContours, maxNumPoints, false);
						PolygonExData polyData = null;

						float numPixels = 0, totalIntensity = 0;
                        int num_objects = objects.Count;

						for (int i=0; i<num_objects; i++)
						{
                            DetectedObject obj = objects[i] as DetectedObject;
							if (obj == null)
								continue;

							if (obj.PolygonBoundary == null)
								continue;

							// update callback status
							if (CommandProgress.IsAborting)
								throw new CommandAbortedException();

							try
							{																						
								polyData = obj.PolygonBoundary;
								if(polyData != null)
								{
									poly.UpdatePolygonEx(polyData);
									
									poly.Intialize();
									
									numPixels = 0; totalIntensity = 0;
									PointF gravity = PointF.Empty;
									//poly.GetObjectExtData(image.RasterImage, ref numPixels, ref totalIntensity);
									poly.GetObjectExtData(image.RasterImage, polyData.ExtPoints, ref numPixels, ref totalIntensity, ref gravity);
									obj.NumPixels = numPixels; obj.TotalIntensity = totalIntensity;
									obj.Gravity = gravity;

									// correct area and perimeter of objects by total intensity
									if (obj.Area == 0)
										obj.Area = numPixels;
									if (obj.Perimeter == 0)
										obj.Perimeter = numPixels;
								}
							}
							catch (System.Exception exp)
							{
								Trace.WriteLine(exp);
							}							
						}

                        CommandProgress.StepTo(100);

						if (poly != null)
							poly.Dispose();
					}
					catch (System.OutOfMemoryException exp)
					{
						Trace.WriteLine(exp);
					}
					
				}

				if (settings.FilterParametersInUse)
				{
					ObjectFilterArguments filter = settings.ObjectFilterArguments;
					
                    // check for filter meaning					
					if (filter.FilterByArea || filter.FilterByIntegratedIntensity || 
                        filter.FilterByNumberOfPixels || filter.FilterByPerimeter)
					{
						RangeFilterArgument aFilter = filter.Area;
						RangeFilterArgument pFilter = filter.Perimeter;
						RangeFilterArgument nFilter = filter.NumberOfPixelFilter;
						RangeFilterArgument iFilter = filter.IntegratedIntensity;

						bool byArea = filter.FilterByArea;
						bool byPerimeter = filter.FilterByPerimeter;
						bool byIntegratedIntensity = filter.FilterByIntegratedIntensity;
						bool byNumberOfPixels = filter.FilterByNumberOfPixels;

                        CommandProgress.SetText("Filtering objects...");
                        CommandProgress.SetRange(0, 100);
                        CommandProgress.StepTo(0);
				
						int num_objects = objects.Count;
                        DetectedObjectCollection filteredObjects = new DetectedObjectCollection(num_objects);
						DetectedObject detObject = null;

						for (int i=0; i < num_objects; i++)
                        {
							detObject = objects[i] as DetectedObject;
                            if (detObject == null)
                                continue;

							if (byArea)
							{
								if (aFilter.UseLowerValue && detObject.Area < aFilter.LowerValue)
                                    continue;							
								if (aFilter.UseUpperValue && detObject.Area > aFilter.UpperValue)
                                    continue;
							}

							if (byPerimeter)
							{
								if (pFilter.UseLowerValue && detObject.Perimeter < pFilter.LowerValue)
                                    continue;
				
								if (pFilter.UseUpperValue && detObject.Perimeter > pFilter.UpperValue)
                                    continue;
							}

							if (byIntegratedIntensity)
							{
								if (iFilter.UseLowerValue && detObject.TotalIntensity < iFilter.LowerValue)
                                    continue;					
								if (iFilter.UseUpperValue && detObject.TotalIntensity > iFilter.UpperValue)
                                    continue;
							}

							if (byNumberOfPixels)
							{
								if (nFilter.UseLowerValue && detObject.NumPixels < nFilter.LowerValue)
                                    continue;

                                if (nFilter.UseUpperValue && detObject.NumPixels > nFilter.UpperValue)
                                    continue;
							}

                            filteredObjects.Add(detObject);
						}
                        
                        // clean up the original detected objects list
                        objects.Clear();
                        objects = filteredObjects;

                        CommandProgress.StepTo(100);
					}
				}

                detectedObjects.AddRange(objects);

                CommandProgress.StepTo(100);
			}
			catch (System.Exception exp)
			{
				if (detectedObjects != null)
					detectedObjects.Clear();
				detectedObjects = null;

				if (objects != null)
					objects.Clear();
				objects = null;

				throw exp;
			}
			finally
			{
				if (binary_image != null)
				{
					binary_image.Dispose();
					binary_image = null;
				}

				if (thresholder != null)
				{
					thresholder.Dispose();
					thresholder = null;
				}

				// release mask temporary object
				if (settings != null && settings.MaskParametersInUse == true && settings.MaskParameters != null)
				{
					if (settings.MaskParameters.Mask != null)
					{
						settings.MaskParameters.Mask.Dispose();
						settings.MaskParameters.Mask = null;
					}
				}
			}

			return detectedObjects;
		}

        /// <summary>
        /// Detects objects for the specified image
        /// </summary>
        /// <param name="image">The image to detect</param>
        /// <param name="settings">The settings information</param>
        /// <returns></returns>
        public static ArrayList ExtractObjectsAsAmountOfWafers(CommonImage image, ObjectDetectionSettings settings)
        {
            ArrayList detectedObjects = null;
            DetectedObjectCollection objects = null;

            IThresholder thresholder = null;
            BinaryImage binary_image = null;

            try
            {
                // initialize result list of detected objects
                detectedObjects = new ArrayList();

                // update callback status
                if (CommandProgress.IsAborting)
                    throw new CommandAbortedException();

                // generate mask
                GenerateMask(image, settings);

                // update callback status
                if (CommandProgress.IsAborting)
                    throw new CommandAbortedException();

                // [ 20070627 Cong] :
                // convert from DynamicCutOffPercentageThreshold into StaticThreshold
                if (settings.ThresholdType == ThresholdType.DynamicCutOfPercentageThreshold)
                {
                    DynamicCutOffPercentageThresholdParameters t = (DynamicCutOffPercentageThresholdParameters)settings.ThresholdParameters;
                    kHistogram histogram = null;
                    bool bApplyOnAllImage = true;
                    try
                    {
                        if (t.ApplyOnAllImage)
                        {
                            histogram = image.Histogram;
                            bApplyOnAllImage = true;
                        }
                        else
                        {                            
                        }

                        // find static threshold parameters
                        float fromPercentage = t.FromPercentage;
                        float toPercentage = t.ToPercentage;
                        double[] hist = histogram.Histogram;
                        double total = 0;
                        int nBins = hist.Length;
                        for (int i = 0; i < nBins; i++)
                        {
                            total += hist[i];
                        }
                        int rankMin = (int)(total * fromPercentage * 0.01);
                        int rankMax = (int)(total * toPercentage * 0.01);
                        float minThreshold = 0;
                        float maxThreshold = 0;

                        image.FindRangeThreshold(ref minThreshold, ref maxThreshold, hist, rankMin, rankMax);

                        // converting threshold type
                        settings.ThresholdType = ThresholdType.StaticThreshold;
                        StaticThresholdParameters staticThresholdParameters = new StaticThresholdParameters((int)minThreshold, (int)maxThreshold);
                        settings.ThresholdParameters = staticThresholdParameters;
                    }
                    catch (System.Exception exp)
                    {
                        Trace.WriteLine(exp);
                        throw exp;
                    }
                    finally
                    {
                        if (histogram != null)
                        {
                            histogram.Dispose();
                            histogram = null;
                        }
                    }

                    // search for appreciated extractor
                    thresholder = FindThresholder(settings);
                    if (thresholder == null)
                        throw new System.Exception("Cannot find appreciated thresholder. Please re-check settings");

                    // update callback status
                    if (CommandProgress.IsAborting  /*  || true )  Abort testing: */ )
                        throw new CommandAbortedException();

                    // threshold image using specified method
                    if (bApplyOnAllImage)
                    {
                        binary_image = ((StaticThresholder)thresholder).Threshold(image, settings);
                    }
                    else
                    {
                       
                    }
                }
                else
                {
                    // search for appreciated extractor
                    thresholder = FindThresholder(settings);
                    if (thresholder == null)
                        throw new System.Exception("Cannot find appreciated thresholder. Please re-check settings");

                    // update callback status
                    if (CommandProgress.IsAborting)
                        throw new CommandAbortedException();

                    // threshold image using specified method
                    if (thresholder is StaticThresholder)
                        binary_image = (thresholder as StaticThresholder).ThresholdSupportedROI(image, settings, false);
                    else
                        binary_image = thresholder.Threshold(image, settings);
                }

#if DUMP_BINARY_IMAGE
				try
				{
					string path = AppDomain.CurrentDomain.BaseDirectory + "binary_image.bmp";
					binary_image.Save(path);
				}
				catch
				{
					Trace.WriteLine("Failed to dump binary image");
				}
#endif
                // update callback status
                if (CommandProgress.IsAborting)
                    throw new CommandAbortedException();

                // extract object from thresholded image
                IntPtr binary_buffer = binary_image.Buffer;

                int width = image.RasterImage.Width;
                int height = image.RasterImage.Height;

                // update progress callback
                CommandProgress.SetText("Detecting objects ...");

                // detect objects				
                objects = ObjectDetector.FindObjects(width, height, binary_buffer);

                // update callback status
                if (CommandProgress.IsAborting)
                    throw new CommandAbortedException();

                // update progress callback
                CommandProgress.SetText("Retreiving objects external data information...");
                CommandProgress.SetRange(0, 100);

                //[Nov 20 2006: calculate object external data such as: numPixels and totalIntensity
                if (objects.Count > 0)
                {
                    //improving only:
                    int maxNumContours = 0, maxNumPoints = 0;
                    foreach (DetectedObject obj in objects)
                    {
                        if (obj.PolygonBoundary != null)
                        {
                            if (obj.PolygonBoundary.nContours > maxNumContours)
                                maxNumContours = obj.PolygonBoundary.nContours;
                            if (obj.PolygonBoundary.nPoints > maxNumPoints)
                                maxNumPoints = obj.PolygonBoundary.nPoints;
                        }
                    }

                    try
                    {
                        PolygonEx poly = new PolygonEx(maxNumContours, maxNumPoints, false);
                        PolygonExData polyData = null;

                        float numPixels = 0, totalIntensity = 0;
                        int num_objects = objects.Count;

                        for (int i = 0; i < num_objects; i++)
                        {
                            DetectedObject obj = objects[i] as DetectedObject;
                            if (obj == null)
                                continue;

                            if (obj.PolygonBoundary == null)
                                continue;

                            // update callback status
                            if (CommandProgress.IsAborting)
                                throw new CommandAbortedException();

                            try
                            {
                                polyData = obj.PolygonBoundary;
                                if (polyData != null)
                                {
                                    poly.UpdatePolygonEx(polyData);

                                    poly.Intialize();

                                    numPixels = 0; totalIntensity = 0;
                                    PointF gravity = PointF.Empty;
                                    //poly.GetObjectExtData(image.RasterImage, ref numPixels, ref totalIntensity);
                                    poly.GetObjectExtData(image.RasterImage, polyData.ExtPoints, ref numPixels, ref totalIntensity, ref gravity);
                                    obj.NumPixels = numPixels; obj.TotalIntensity = totalIntensity;
                                    obj.Gravity = gravity;

                                    // correct area and perimeter of objects by total intensity
                                    if (obj.Area == 0)
                                        obj.Area = numPixels;
                                    if (obj.Perimeter == 0)
                                        obj.Perimeter = numPixels;
                                }
                            }
                            catch (System.Exception exp)
                            {
                                Trace.WriteLine(exp);
                            }
                        }

                        CommandProgress.StepTo(100);

                        if (poly != null)
                            poly.Dispose();
                    }
                    catch (System.OutOfMemoryException exp)
                    {
                        Trace.WriteLine(exp);
                    }

                }

                if (settings.FilterParametersInUse)
                {
                    ObjectFilterArguments filter = settings.ObjectFilterArguments;

                    // check for filter meaning					
                    if (filter.FilterByArea || filter.FilterByIntegratedIntensity ||
                        filter.FilterByNumberOfPixels || filter.FilterByPerimeter)
                    {
                        RangeFilterArgument aFilter = filter.Area;
                        RangeFilterArgument pFilter = filter.Perimeter;
                        RangeFilterArgument nFilter = filter.NumberOfPixelFilter;
                        RangeFilterArgument iFilter = filter.IntegratedIntensity;

                        bool byArea = filter.FilterByArea;
                        bool byPerimeter = filter.FilterByPerimeter;
                        bool byIntegratedIntensity = filter.FilterByIntegratedIntensity;
                        bool byNumberOfPixels = filter.FilterByNumberOfPixels;

                        CommandProgress.SetText("Filtering objects...");
                        CommandProgress.SetRange(0, 100);
                        CommandProgress.StepTo(0);

                        int num_objects = objects.Count;
                        DetectedObjectCollection filteredObjects = new DetectedObjectCollection(num_objects);
                        DetectedObject detObject = null;

                        for (int i = 0; i < num_objects; i++)
                        {
                            detObject = objects[i] as DetectedObject;
                            if (detObject == null)
                                continue;

                            if (byArea)
                            {
                                if (aFilter.UseLowerValue && detObject.Area < aFilter.LowerValue)
                                    continue;
                                if (aFilter.UseUpperValue && detObject.Area > aFilter.UpperValue)
                                    continue;
                            }

                            if (byPerimeter)
                            {
                                if (pFilter.UseLowerValue && detObject.Perimeter < pFilter.LowerValue)
                                    continue;

                                if (pFilter.UseUpperValue && detObject.Perimeter > pFilter.UpperValue)
                                    continue;
                            }

                            if (byIntegratedIntensity)
                            {
                                if (iFilter.UseLowerValue && detObject.TotalIntensity < iFilter.LowerValue)
                                    continue;
                                if (iFilter.UseUpperValue && detObject.TotalIntensity > iFilter.UpperValue)
                                    continue;
                            }

                            if (byNumberOfPixels)
                            {
                                if (nFilter.UseLowerValue && detObject.NumPixels < nFilter.LowerValue)
                                    continue;

                                if (nFilter.UseUpperValue && detObject.NumPixels > nFilter.UpperValue)
                                    continue;
                            }

                            filteredObjects.Add(detObject);
                        }

                        // clean up the original detected objects list
                        objects.Clear();
                        objects = filteredObjects;

                        CommandProgress.StepTo(100);
                    }
                }

                detectedObjects.AddRange(objects);

                CommandProgress.StepTo(100);
            }
            catch (System.Exception exp)
            {
                if (detectedObjects != null)
                    detectedObjects.Clear();
                detectedObjects = null;

                if (objects != null)
                    objects.Clear();
                objects = null;

                throw exp;
            }
            finally
            {
                if (binary_image != null)
                {
                    binary_image.Dispose();
                    binary_image = null;
                }

                if (thresholder != null)
                {
                    thresholder.Dispose();
                    thresholder = null;
                }

                // release mask temporary object
                if (settings != null && settings.MaskParametersInUse == true && settings.MaskParameters != null)
                {
                    if (settings.MaskParameters.Mask != null)
                    {
                        settings.MaskParameters.Mask.Dispose();
                        settings.MaskParameters.Mask = null;
                    }
                }
            }

            return detectedObjects;
        }

        /// <summary>
        /// Creates mask for detecting the objects
        /// </summary>
        /// <param name="image">The input image</param>
        /// <param name="settings">The settings contains the mask values</param>
		private static void GenerateMask(CommonImage image, ObjectDetectionSettings settings)
		{
			if (image == null || settings == null)
				throw new System.ArgumentNullException("Invalid Parameter");

			// generate mask from mask data
			if (settings.MaskParametersInUse)
			{
				MaskParameters parameters = settings.MaskParameters;
				if (settings.UseMaskData)
				{
					#region Use External Script Mask Data
					GraphicsList objects = null;				
					try
					{
						using (MaskHelper helper = new MaskHelper(image))
						{
							objects = helper.LoadMask(settings.MaskData);
							parameters.Mask = helper.CreateMask(objects);
						}
					}
					catch (System.Exception exp)
					{
						throw new System.Exception(string.Format("Generate Internal Script Mask Data: {0} - {1}", exp.Message, exp.StackTrace), exp);
					}
					#endregion
				}
				else
				{
					#region Internal Script Mask Data
					if (parameters.UseInternalMask)
					{
						MaskHelper helper = null;
						GraphicsList objects = (GraphicsList)image.Mask.GraphicsList;
					
						try
						{
							helper = new MaskHelper(image);
							parameters.Mask = helper.CreateMask(objects);
						}
						catch(System.Exception exp)
						{
							throw exp;
						}
						finally
						{
							if (helper != null)
								helper.Dispose();
						}
					}
					else
					{
						MaskHelper helper = null;
						GraphicsList objects = null;
					
						try
						{
							helper = new MaskHelper(image);
							objects = helper.LoadMask(parameters.ExtMaskFilePath);
						
							parameters.Mask = helper.CreateMask(objects);
						}
						catch(System.Exception exp)
						{
							throw exp;
						}
						finally
						{
							if (helper != null)
								helper.Dispose();
						}
					}
					#endregion
				}
			}
		}

        //[Obsolete]
        //public static int ExportObjectsToKlarf(String filename, CommonImage image, ArrayList objects, KLARFExportSettings settings, IKlarfFile parser)
        //{
        //    return ObjectExtractor.ExportObjectsToKlarf(filename, image, objects, settings, parser, 0, 0);
        //}

        ///// <summary>
        ///// Exports the detected objects to Klarf file
        ///// </summary>
        ///// <param name="filename">The name of the output Klarf file.</param>
        ///// <param name="image">The image contains the information about the wafermark</param>
        ///// <param name="objects">The objects which is exported</param>
        ///// <param name="settings">The settings contains information about the exporting operation</param>
        ///// <param name="parser">The template klarf file</param>
        ///// <returns>Number of exported defects</returns>
        //[Obsolete]
        //public static int ExportObjectsToKlarf(String filename, CommonImage image, ArrayList objects, KLARFExportSettings settings, IKlarfFile parser, int maxKlarfFileSize, int maxNumberOfDefects)
        //{
        //    if (image == null || objects == null || settings == null || parser == null)
        //        throw new System.ArgumentNullException();
        //    if (filename == null || filename == string.Empty)
        //        throw new System.Exception("Invalid File Name parameters");

        //    // initialize klarf parser
        //    InitKlarfFile(parser, settings);

        //    int numDefects = 0;
        //    ArrayList defects = null;

        //    try
        //    {
        //        // convert object to point or area defects
        //        defects = ConvertObjectToDefects(objects, image, settings);

        //        // process camera correction (lens distortion0
        //        if (settings.LensCorrectionParametersInUse)
        //        {
        //            CameraCorrection camCorrector = new CameraCorrection(settings.LensCorrectionParameters);
        //            camCorrector.Correct(defects);
        //        }

        //        // convert cluster from image coordinate to wafer coordinate
        //        float left = image.CenterF.X - (float)image.Radius;
        //        float top = image.CenterF.Y - (float)image.Radius;
			
        //        foreach(ClusterEx defect in defects)
        //            defect.Rectangle = new RectangleF(defect.Center.X-left, defect.Center.Y-top, defect.Rectangle.Width, defect.Rectangle.Height);

        //        // recalculate grid
        //        parser.RecalGRID();
			
        //        // parse data from fits
        //        parser.FromClusters(defects, (float)image.Radius);
			
        //        // process orientation mark
        //        float radAngle = 0;
        //        if(image.WaferMark.Type == WaferMarkType.Flat)
        //            radAngle = (float)(settings.DeviationAngle*Math.PI/180f - (float)((FlatMark)image.WaferMark).PrimaryFlatAngle);
        //        else
        //            radAngle = (float)(settings.DeviationAngle*Math.PI/180f - (float)((NotchMark)image.WaferMark).Angle);			

        //        // calculate angle to rotate
        //        float angle = radAngle;
        //        Orientation orientation = settings.Orientation;
        //        if(orientation == Orientation.Up)
        //        {
        //            angle = (float)Math.PI /2 - radAngle;
        //        }
        //        else if(orientation == Orientation.Down)
        //        {
        //            angle = -(float)Math.PI /2 - radAngle;
        //        }
        //        else if(orientation == Orientation.Left)
        //        {
        //            angle = (float)Math.PI - radAngle;
        //        }
        //        else if(orientation == Orientation.Right)
        //        {
        //            angle =  - radAngle;
        //        }
				
        //        // apply defect rotation				
        //        parser.RotateDefectData((float)angle);

        //        // remove wrong defects
        //        parser.RemoveWrongDefect();
        //        parser.ReUpdateWrongRelativeCoordinate();
        //        parser.UpdateData();

        //        // calculate sample test plan
        //        parser.RecalSampleTestPlan();

        //        // output result to file
        //        parser.Save(filename, maxKlarfFileSize, maxNumberOfDefects);
        //    }
        //    catch 
        //    {
        //        throw; 
        //    }
        //    finally
        //    {
        //        if (defects != null)
        //        {
        //            numDefects = defects.Count;
        //            defects.Clear();
        //        }
        //        defects = null;
        //    }

        //    return numDefects;
        //}

        //[Obsolete]
        //public static ArrayList ConvertObjectToDefects(ArrayList objects, CommonImage image, KLARFExportSettings settings)
        //{
        //    if (objects == null)
        //        throw new System.ArgumentNullException();
				
        //    #region Preparing			
        //    float resolution = 0;
        //    DefectType defectType = settings.DefectType;
        //    if (defectType == DefectType.AreaDefect)
        //    {
        //        AreaDefectTypeParameters parameters = (AreaDefectTypeParameters)settings.DefectTypeParameters;
        //        resolution = (float)parameters.Resolution;
        //        if (resolution <= 0)
        //            throw new System.ArgumentOutOfRangeException("resolution");				
        //    }
			
        //    // improving only			
        //    float radius = (float)image.Radius;
        //    float centerX = image.CenterF.X;
        //    float centerY = image.CenterF.Y;
        //    float left = centerX - radius;
        //    float right = centerX + radius;
        //    float top = centerY - radius;
        //    float bottom = centerY + radius;
        //    RectangleF rect;

        //    float leftInside = left;
        //    float topInside = top;
        //    float rightInside = right;
        //    float bottomInside = bottom;
        //    bool bInside = false;
        //    bool bFlat = false;
        //    float radAngle = 0;
        //    if(image.WaferMark.Type == WaferMarkType.Flat)
        //    {
        //        bFlat = true;				
        //        radAngle = (float)(settings.DeviationAngle*Math.PI/180f - (float)((FlatMark)image.WaferMark).PrimaryFlatAngle);
        //    }
        //    else
        //    {
        //        radAngle = (float)(settings.DeviationAngle*Math.PI/180f - (float)((NotchMark)image.WaferMark).Angle);						
        //    }

        //    float dx, dy, c;
        //    dx = dy = c = 0;
        //    PointF point1 = new PointF(0, 0);
        //    PointF point2 = new PointF(0, 0);
        //    if (bFlat)
        //    {							
        //        double primaryLength = ((FlatMark)image.WaferMark).PrimaryFlatLength;
        //        double lengtToRatio = SIA.SystemLayer.FlatMark.LengthToRatio(image, primaryLength)*radius;
        //        float flatLength = (float)Math.Sqrt((double)(radius)*radius - lengtToRatio*lengtToRatio)*2f;
								
        //        ObjectExtractor.GetLine(ref point1, ref point2, ref dx, ref dy, ref c, radius, flatLength, radAngle);

        //        // update rect to check inside
        //        float dd = (float)(Math.Sqrt(2)*lengtToRatio*0.5f);
        //        leftInside = centerX - dd;
        //        topInside = centerY - dd;
        //        rightInside = centerX + dd;
        //        bottomInside = centerY + dd;
        //    }
        //    else
        //    {
        //        float dd = (float)(Math.Sqrt(2)*radius*0.5f);
        //        leftInside = centerX - dd;
        //        topInside = centerY - dd;
        //        rightInside = centerX + dd;
        //        bottomInside = centerY + dd;
        //    }
        //    #endregion Preparing

        //    ArrayList defects = new ArrayList();
        //    try
        //    {
        //        //Cong [Nov 22 2006]:
        //        if (objects.Count > 0)
        //        {
        //            //improving only:
        //            int maxNumContours = 0, maxNumPoints = 0;
        //            foreach(DetectedObject obj in objects)
        //            {
        //                if(obj.PolygonBoundary != null)
        //                {
        //                    if(obj.PolygonBoundary.nContours > maxNumContours)
        //                        maxNumContours = obj.PolygonBoundary.nContours;
        //                    if(obj.PolygonBoundary.nPoints > maxNumPoints)
        //                        maxNumPoints = obj.PolygonBoundary.nPoints;
        //                }
        //            }

        //            PolygonEx poly = null;
        //            PolygonExData polyData = null;
	
        //            try
        //            {						
        //                poly = new PolygonEx(maxNumContours, maxNumPoints, true);					
        //                poly.CenterX = centerX;
        //                poly.CenterY = centerY;
        //                poly.Radius = radius;
        //                poly.MarkAngle = radAngle;
        //                poly.Angle = 0; // don't care in this version
        //                poly.HasFlat = bFlat;
        //                if(bFlat)
        //                {
        //                    poly.DX = dx;
        //                    poly.DY = dy;
        //                    poly.C = c;
	
        //                    poly.Point1 = point1;
        //                    poly.Point2 = point2;
        //                }
																
        //                int iObj = 1;
        //                foreach(DetectedObject obj in objects)
        //                {
        //                    if(obj == null)
        //                        continue;

        //                    if(obj.PolygonBoundary == null)
        //                        continue;														

        //                    // check in wafer boundary ex
        //                    rect = obj.RectBound;
        //                    if(rect.Right < left || rect.Left > right)
        //                        continue;
        //                    if(rect.Bottom < top || rect.Top > bottom)
        //                        continue;
        //                    bInside = false;
        //                    if(rect.Left >= leftInside && rect.Right <= rightInside && rect.Top >= topInside && rect.Bottom  <= bottomInside)
        //                        bInside = true;

        //                    try
        //                    {																						
        //                        polyData = obj.PolygonBoundary;
        //                        if (polyData != null)
        //                        {
        //                            poly.UpdatePolygonEx(polyData);
										
        //                            poly.Inside = bInside;									

        //                            poly.Intialize();
									
        //                            if (defectType == DefectType.AreaDefect)
        //                                poly.GetAreaDefects(defects, polyData.ExtPoints, resolution);
        //                            else if(defectType == DefectType.PointDefect)
        //                                poly.GetPointDefects(defects, polyData.ExtPoints);
        //                        }

        //                        int percent = (int)((float)iObj*100.0F/objects.Count);	
						
        //                        CommandProgress.StepTo(percent);							
        //                        if (iObj % 100 == 0)
        //                            CommandProgress.SetText(String.Format("Retrieving objects information ({0}/{1})", iObj, objects.Count));							
								
        //                        // update callback status
        //                        if (CommandProgress.IsAborting  /*  || true )  Abort testing: */ )
        //                            throw new CommandAbortedException();
								
        //                        iObj++;
        //                    }
        //                    catch (System.Exception exp)
        //                    {
        //                        Trace.WriteLine(exp);
        //                    }							
        //                }
						
        //                CommandProgress.SetText(String.Format("Retrieving objects information ({0}/{1})", objects.Count, objects.Count));		
        //            }
        //            catch (System.OutOfMemoryException exp)
        //            {
        //                Trace.WriteLine(exp);
        //            }		
        //            finally
        //            {
        //                if (poly != null)
        //                    poly.Dispose();
        //            }
        //        }
        //    }
        //    catch (System.Exception exp)
        //    {
        //        if (defects != null)
        //            defects.Clear();
        //        defects = null;

        //        Trace.WriteLine(exp);
        //    }						
        //    finally
        //    {
        //    }

        //    return defects;
        //}

        //[Obsolete]
        //public static ArrayList ConvertObjectToPointDefects(ArrayList objects, CommonImage image, KLARFExportSettings settings)
        //{
        //    if (objects == null)
        //        throw new System.ArgumentNullException("objects");

        //    ArrayList defects = new ArrayList();
        //    try
        //    {		
        //        PointF center_tmp;
        //        int iObj = 1;
        //        foreach(DetectedObject obj in objects)
        //        {
        //            if(obj == null)
        //                continue;

        //            if(obj.PolygonBoundary == null)
        //                continue;					

        //            ClusterEx cluster = new ClusterEx();
					
        //            center_tmp = obj.PolygonBoundary.ExtractDefects();
					
        //            cluster.Rectangle = new RectangleF(center_tmp.X-0.5f, center_tmp.Y-0.5f, 1.0f, 1.0f);

        //            //cluster.Rectangle = new RectangleF(center_tmp.X-0.5f*obj.RectBound.Width, center_tmp.Y-0.5f*obj.RectBound.Height, obj.RectBound.Width, obj.RectBound.Height);
					
        //            defects.Add(cluster);

        //            int percent = (int)((float)iObj*100.0F/objects.Count);	
						
        //            CommandProgress.StepTo(percent);							
        //            if (iObj % 100 == 0)
        //                CommandProgress.SetText(String.Format("Retreiving objects information ({0}/{1})", iObj, objects.Count));							
								
        //            iObj++;
        //        }
        //        CommandProgress.SetText(String.Format("Retreiving objects information ({0}/{1})", objects.Count, objects.Count));
        //    }
        //    catch (System.Exception exp)
        //    {
        //        Trace.WriteLine(exp);
        //    }

        //    return defects;
        //}

        //[Obsolete]
        //public static ArrayList ConvertObjectToAreaDefects(ArrayList objects, CommonImage image, KLARFExportSettings settings)		
        //{
        //    if (objects == null || image == null || settings == null)
        //        throw new System.ArgumentNullException();
			
        //    AreaDefectTypeParameters parameters = (AreaDefectTypeParameters)settings.DefectTypeParameters;
        //    int resolution = parameters.Resolution;
        //    if (resolution <= 0)
        //        throw new System.ArgumentOutOfRangeException("resolution");
		
        //    int width = image.Width;
        //    int height = image.Height;

        //    return ConvertObjectToAreaDefects(objects, image, settings, resolution);
        //}

        //[Obsolete]
        //public static ArrayList ConvertObjectToAreaDefects(ArrayList objects, CommonImage image, KLARFExportSettings settings, float resolution)
        //{
        //    if (objects == null)
        //        throw new System.ArgumentNullException();
				
        //    #region Preparing
			
        //    // improving only			
        //    float radius = (float)image.Radius;
        //    float centerX = image.CenterF.X;
        //    float centerY = image.CenterF.Y;
        //    float left = centerX - radius;
        //    float right = centerX + radius;
        //    float top = centerY - radius;
        //    float bottom = centerY + radius;
        //    RectangleF rect;

        //    float leftInside = left;
        //    float topInside = top;
        //    float rightInside = right;
        //    float bottomInside = bottom;
        //    bool bInside = false;
        //    bool bFlat = false;
        //    float radAngle = 0;

        //    if(image.WaferMark.Type == WaferMarkType.Flat)
        //    {
        //        bFlat = true;				
        //        radAngle = (float)(settings.DeviationAngle*Math.PI/180f - (float)((FlatMark)image.WaferMark).PrimaryFlatAngle);
        //    }
        //    else
        //        radAngle = (float)(settings.DeviationAngle*Math.PI/180f - (float)((NotchMark)image.WaferMark).Angle);			

        //    // calculate angle to rotate
        //    float angle = radAngle;
        //    Orientation orientation = settings.Orientation;
        //    if(orientation == Orientation.Up)
        //    {
        //        angle = (float)Math.PI /2 - radAngle;
        //    }
        //    else if(orientation == Orientation.Down)
        //    {
        //        angle = -(float)Math.PI /2 - radAngle;
        //    }
        //    else if(orientation == Orientation.Left)
        //    {
        //        angle = (float)Math.PI - radAngle;
        //    }
        //    else if(orientation == Orientation.Right)
        //    {
        //        angle =  - radAngle;
        //    }

        //    float dx, dy, c;
        //    dx = dy = c = 0;
        //    PointF point1 = new PointF(0, 0);
        //    PointF point2 = new PointF(0, 0);
        //    if (bFlat)
        //    {							
        //        double primaryLength = ((FlatMark)image.WaferMark).PrimaryFlatLength;
        //        double lengtToRatio = SIA.SystemLayer.FlatMark.LengthToRatio(image, primaryLength)*radius;
        //        float flatLength = (float)Math.Sqrt((double)(radius)*radius - lengtToRatio*lengtToRatio)*2f;
								
        //        GetLine(ref point1, ref point2, ref dx, ref dy, ref c, radius, flatLength, radAngle+angle);

        //        // update rect to check inside
        //        float dd = (float)(Math.Sqrt(2)*lengtToRatio*0.5f);
        //        leftInside = centerX - dd;
        //        topInside = centerY - dd;
        //        rightInside = centerX + dd;
        //        bottomInside = centerY + dd;
        //    }
        //    else
        //    {
        //        float dd = (float)(Math.Sqrt(2)*radius*0.5f);
        //        leftInside = centerX - dd;
        //        topInside = centerY - dd;
        //        rightInside = centerX + dd;
        //        bottomInside = centerY + dd;
        //    }
        //    #endregion Preparing

        //    ArrayList defects = new ArrayList();
        //    try
        //    {
        //        //Cong [Nov 22 2006]:
        //        if (objects.Count > 0)
        //        {
        //            //improving only:
        //            int maxNumContours = 0, maxNumPoints = 0;
        //            foreach(DetectedObject obj in objects)
        //            {
        //                if(obj.PolygonBoundary != null)
        //                {
        //                    if(obj.PolygonBoundary.nContours > maxNumContours)
        //                        maxNumContours = obj.PolygonBoundary.nContours;
        //                    if(obj.PolygonBoundary.nPoints > maxNumPoints)
        //                        maxNumPoints = obj.PolygonBoundary.nPoints;
        //                }
        //            }

        //            PolygonEx poly = null;
        //            PolygonExData polyData = null;
	
        //            try
        //            {						
        //                poly = new PolygonEx(maxNumContours, maxNumPoints, true);					
        //                poly.CenterX = centerX;
        //                poly.CenterY = centerY;
        //                poly.Radius = radius;
        //                poly.MarkAngle = radAngle;
        //                poly.Angle = angle;						
        //                poly.HasFlat = bFlat;
        //                if(bFlat)
        //                {
        //                    poly.DX = dx;
        //                    poly.DY = dy;
        //                    poly.C = c;
	
        //                    poly.Point1 = point1;
        //                    poly.Point2 = point2;
        //                }
																
        //                int iObj = 1;
        //                foreach(DetectedObject obj in objects)
        //                {
        //                    if(obj == null)
        //                        continue;

        //                    if(obj.PolygonBoundary == null)
        //                        continue;
							
        //                    // check in wafer boundary ex
        //                    rect = obj.RectBound;
        //                    if(rect.Right < left || rect.Left > right)
        //                        continue;
        //                    if(rect.Bottom < top || rect.Top > bottom)
        //                        continue;
        //                    bInside = false;
        //                    if(rect.Left >= leftInside && rect.Right <= rightInside && rect.Top >= topInside && rect.Bottom  <= bottomInside)
        //                        bInside = true;

        //                    try
        //                    {																						
        //                        polyData = obj.PolygonBoundary;
        //                        if (polyData != null)
        //                        {
        //                            poly.UpdatePolygonEx(polyData);
										
        //                            poly.Inside = bInside;									

        //                            poly.Intialize();
																			
        //                            poly.GetAreaDefects(defects, polyData.ExtPoints, resolution);
        //                        }

        //                        int percent = (int)((float)iObj*100.0F/objects.Count);	
						
        //                        SIA.SystemFrameworks.UI.CommandProgress.StepTo(percent);		
        //                        if (iObj % 100 == 0)
        //                            SIA.SystemFrameworks.UI.CommandProgress.SetText(String.Format("Retreiving objects information ({0}/{1})", iObj, objects.Count));							
								
        //                        iObj++;
        //                    }
        //                    catch (System.Exception exp)
        //                    {
        //                        Trace.WriteLine(exp);
        //                    }							
        //                }	
        //                SIA.SystemFrameworks.UI.CommandProgress.SetText(String.Format("Retreiving objects information ({0}/{0})", objects.Count));						
        //            }
        //            catch (System.OutOfMemoryException exp)
        //            {
        //                Trace.WriteLine(exp);
        //            }		
        //            finally
        //            {
        //                if (poly != null)
        //                    poly.Dispose();
        //            }
        //        }
        //    }
        //    catch (System.Exception exp)
        //    {
        //        if (defects != null)
        //            defects.Clear();
        //        defects = null;

        //        Trace.WriteLine(exp);
        //    }						
        //    finally
        //    {
        //    }

        //    return defects;
        //}
		
		
		public static void GetLine(ref PointF point1, ref PointF point2, ref float dx, ref float dy, ref float c, float radius, float primaryLength, float flatAngle)
		{			
			float halfLenght = primaryLength*0.5f;
			float x1, y1, x2, y2, x3, y3;
			y3 = halfLenght;	
			x3 = (float)Math.Sqrt(radius*radius - y3*y3);	
			double dTheta = flatAngle;
			x1 = (float)(x3*Math.Cos(dTheta) - y3*Math.Sin(dTheta));
			y1 = (float)(y3*Math.Cos(dTheta) + x3*Math.Sin(dTheta));
			y3 = -halfLenght;
			x2 = (float)(x3*Math.Cos(dTheta) - y3*Math.Sin(dTheta));
			y2 = (float)(y3*Math.Cos(dTheta) + x3*Math.Sin(dTheta));		
			
			dx = x2-x1;
			dy = y2-y1;
			c = y2*x1-y1*x2;

			point1.X = x1;
			point1.Y = y1;
			point2.X = x2;
			point2.Y = y2;
		}
	}
}
