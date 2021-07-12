//#define GPU_SUPPORTED

using System;
using System.Data;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

using SIA.Common;
using SIA.Common.Analysis;
using SIA.Common.Mathematics;
using SIA.Common.Utility;
using SIA.Common.KlarfExport;
using SIA.Common.KlarfExport.BinningLibrary;
using SIA.Common.Imaging;
using SIA.Common.Imaging.Filters;
using SIA.SystemFrameworks;
using SIA.SystemFrameworks.ComputerVision;
using SIA.IPEngine;
using SIA.IPEngine.Codecs;
using SIA.IPEngine.KlarfExport;
using SIA.SystemLayer.Mathematics;
using SIA.Common.IPLFacade;

using TYPE = System.UInt16;
using SiGlaz.Cloo;
using Cloo;
using System.Text;

namespace SIA.SystemLayer
{
    /// <summary>
	/// Old Purpose: The common image is something like a global workspace containing all necessary data (in a way kept for compatibility)
	/// Now Several Purposes:
	/// - Wrapper of GreyDataImage containing the real image data and Wafer boundary
	/// - Keeps Wafer Boundary data, too
	/// - Provides a collection of some function for image processing
	/// 
	/// Several CommonImages may share one GreyDataImage
    /// </summary>
    public class CommonImage 
        : System.ICloneable, System.IDisposable
    {
        #region constants
        [Obsolete]
        private const String BORDER_CLEARED = "BORDER_CLEARED";
        #endregion

        #region member attributes
        private bool _bAutoDisposeImage = true;				// set to false, if you attach the instance to another GreyDataImage
        private GreyDataImage _image = null;	// pointer to GreyDataImage

        private bool _bAutoDisposeMask = true;
        private System.Drawing.Image _maskImage = null;			// image mask for some operations (under control of this class)
		private SIA.Common.Mask.IMask _mask = null;	// image mask for some operations (under control of this class)

        //private Hashtable _properties = null;
        private string _filePath = "";		// the only location of the filename of this image
        private string _description;		// used for history (description of performed operation)
        private bool _modified = false;		// dirty flag

        [Obsolete]
        private CircleFinder _circleFinder;	// a processing class

        #endregion

        #region event handler
		[Obsolete]
        public event EventHandler MinCurrentViewChanged;
		[Obsolete]
		public event EventHandler MaxCurrentViewChanged;

        public event EventHandler RadiusChanged;
        public event EventHandler CenterChanged;
        public event EventHandler BorderThicknessChanged;	// refers to data of GreyDataImage

        public event EventHandler AutoDisposeImageChanged;
        public event EventHandler RasterImageChanged;

        public event EventHandler AutoDisposeMaskChanged;
        public event EventHandler MaskChanged;
        public event EventHandler MaskImageChanged;

        public event EventHandler ModifiedChanged;
        public event EventHandler FilePathChanged;
        public event EventHandler DescriptionChanged;

        #endregion

        #region constructor and destructor

        private void InitClass()
        {
            this._bAutoDisposeImage = true;
            this._image = null;

            this._bAutoDisposeMask = true;
            this._maskImage = null;

            this._circleFinder = null;

            //this._properties = new Hashtable();
            //this._properties[BORDER_CLEARED] = false;

            this._filePath = string.Empty;
            this._description = string.Empty;
            this._modified = false;

#if GPU_SUPPORTED
        cbImage = null;
#endif
        }

        public CommonImage()
        {
            this.InitClass();

            // initialize empty raster image
            _image = new GreyDataImage();
        }

        public CommonImage(GreyDataImage image)
        {
            if (image == null)
                throw new System.ArgumentNullException("image is not set to a reference");
            // initialize member attributes
            this.InitClass();
            // clone image from reference
            this._image = (GreyDataImage)image.Copy();
        }

        public CommonImage(GreyDataImage image, bool clone)
        {
            if (image == null)
                throw new System.ArgumentNullException("image is not set to a reference");
            // initialize member attributes
            this.InitClass();

            if (clone) // clone image from reference				
                this._image = image.Copy();
            else
                this._image = image;
        }

        /// <summary>
        /// construct new image from Gray Data Image 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="type">
        /// copy flags: 0 : copy 1 : Not copy 
        /// </param>
        public CommonImage(GreyDataImage image, int type)
        {
            if (image == null)
                throw new System.ArgumentNullException("invalid image parameter");

            // initialize member attributes
            this.InitClass();

            // initialize raster image
            if (type == 0)
                _image = (GreyDataImage)image.Copy();
            else if (type == 1)
                _image = image;
        }

        public CommonImage(CommonImage image)
        {
            if (image == null && image.RasterImage != null)
                throw new System.ArgumentNullException("invalid image parameter");

            // initialize member attributes
            this.InitClass();

            // copy constructor implementation
            this._image = (GreyDataImage)(image._image.Copy());
            this.Mask = (image.Mask != null ? (SIA.Common.Mask.IMask)image.Mask.Clone() : null);
            this.MaskImage = image.MaskImage != null ? (Image)image.MaskImage.Clone() : null;
            this._circleFinder = image._circleFinder != null ? image._circleFinder : null;
            //this._properties = (Hashtable)image._properties.Clone();
            this._filePath = image._filePath;
            this._description = image._description;
            this._modified = false;
        }

        public CommonImage(int width, int height)
        {
            // initialize member attributes
            this.InitClass();
            
            // initialize raster image			
            _image = new GreyDataImage(width, height);
        }

        public unsafe CommonImage(int width, int height, IntPtr buffer)
        {
            // initialize member attributes
            this.InitClass();
            
            // initialize raster image
            _image = new GreyDataImage();
            _image.SetImageData(width, height, (ushort*)buffer.ToPointer(), SetImageDataFlags.Reference);
        }

        public CommonImage(System.Drawing.Bitmap image)
        {
            if (image == null)
                throw new System.ArgumentNullException("invalid image parameter");

            // initialize member attributes
            this.InitClass();

            // initialize raster image			
            _image = new GreyDataImage(image);
        }

        public CommonImage(System.IO.FileStream fs)
        {
            if (fs == null)
                throw new System.ArgumentNullException("invalid image parameter");

            Bitmap image = null;
            try
            {
                image = Image.FromStream(fs) as Bitmap;
            }
            catch
            {
                throw;
            }

            if (image == null)
                throw new System.ArgumentNullException("invalid image parameter");

            // initialize member attributes
            this.InitClass();

            // initialize raster image			
            _image = new GreyDataImage(image);
        }

        public CommonImage(System.IO.MemoryStream fs)
        {
            if (fs == null)
                throw new System.ArgumentNullException("invalid image parameter");

            Bitmap image = null;
            try
            {
                image = Image.FromStream(fs) as Bitmap;
            }
            catch
            {
                throw;
            }

            if (image == null)
                throw new System.ArgumentNullException("invalid image parameter");

            // initialize member attributes
            this.InitClass();

            // initialize raster image			
            _image = new GreyDataImage(image);
        }

        public static CommonImage FromGreyDataImage(GreyDataImage image)
        {
            return new CommonImage(image);
        }

        #endregion

        #region public properties

        #region image attributes

        public Size Size
        {
            get
            {
                if (_image == null)
                    return Size.Empty;
                return new Size(_image.Width, _image.Height);
            }
        }

        public int Width
        {
            get
            {
                if (_image == null) return 0;
                return _image._width;
            }
        }

        public int Height
        {
            get
            {
                if (_image == null) return 0;
                return _image._height;
            }
        }

        public int Length
        {
            get
            {
                if (_image == null) return 0;
                return _image._lenght;
            }
        }

        public double MaxGreyValue
        {
            get { return _image.MAXGRAYVALUE; }
        }

        public double MinGreyValue
        {
            get { return _image.MINGRAYVALUE; }
        }

        public double MinCurrentView
        {
            get { return _image.MinCurrentView; }
            set
            {
                _image.MinCurrentView = value;
                OnMinCurrentViewChanged();
            }
        }

        protected virtual void OnMinCurrentViewChanged()
        {
            if (this.MinCurrentViewChanged != null)
                this.MinCurrentViewChanged(this, EventArgs.Empty);
        }

        public double MaxCurrentView
        {
            get { return _image.MaxCurrentView; }
            set
            {
                _image.MaxCurrentView = value;
                OnMaxCurrentViewChanged();
            }
        }

        protected virtual void OnMaxCurrentViewChanged()
        {
            if (this.MaxCurrentViewChanged != null)
                this.MaxCurrentViewChanged(this, EventArgs.Empty);
        }

        #endregion

        #region Wafer specific attributes

        public double BorderThick
        {
            get { return _image.BorderThick; }
            set
            {
                _image.BorderThick = value;
                OnBorderThicknessChanged();
            }
        }

        protected virtual void OnBorderThicknessChanged()
        {
            if (this.BorderThicknessChanged != null)
                this.BorderThicknessChanged(this, EventArgs.Empty);
        }

        public Point Center
        {
            get
            {
                return new Point(_image._x, _image._y);
            }
            set
            {
                _image._x = ((Point)value).X;
                _image._y = ((Point)value).Y;
                OnCenterChanged();
            }
        }

        public PointF CenterF
        {
            get { return new PointF(_image._x + _image._xdecimal, _image._y + _image._ydecimal); }
            set
            {
                _image._x = (int)(((PointF)value).X);
                _image._y = (int)(((PointF)value).Y);
                _image._xdecimal = (float)((double)(((PointF)value).X) - _image._x);
                _image._ydecimal = (float)((double)(((PointF)value).Y) - _image._y);

                OnCenterChanged();
            }
        }

        protected virtual void OnCenterChanged()
        {
            if (this.CenterChanged != null)
                this.CenterChanged(this, EventArgs.Empty);
        }
        #endregion

        public bool AutoDisposeImage
        {
            get { return _bAutoDisposeImage; }
            set
            {
                _bAutoDisposeImage = value;
                OnAutoDisposeImageChanged();
            }
        }

        protected virtual void OnAutoDisposeImageChanged()
        {
            if (this.AutoDisposeImageChanged != null)
                this.AutoDisposeImageChanged(this, EventArgs.Empty);
        }

        public GreyDataImage RasterImage
        {
            get { return _image; }
            set
            {
                if (_image != null && _bAutoDisposeImage)
                    _image.Dispose();
                _image = value;
                OnRasterImageChanged();
            }
        }

        protected virtual void OnRasterImageChanged()
        {
            if (this.RasterImageChanged != null)
                this.RasterImageChanged(this, EventArgs.Empty);
        }

        public bool AutoDisposeMask
        {
            get { return _bAutoDisposeMask; }
            set
            {
                _bAutoDisposeMask = value;
                OnAutoDisposeMaskChanged();
            }
        }

        protected virtual void OnAutoDisposeMaskChanged()
        {
            if (this.AutoDisposeMaskChanged != null)
                this.AutoDisposeMaskChanged(this, EventArgs.Empty);
        }

        public System.Drawing.Image MaskImage
        {
            get { return _maskImage; }
            set
            {
                if (_maskImage != null)
                    _maskImage.Dispose();
                _maskImage = value;
                OnMaskChanged();
            }
        }

        protected virtual void OnMaskImageChanged()
        {
            if (this.MaskImageChanged != null)
                this.MaskImageChanged(this, EventArgs.Empty);
        }

        public SIA.Common.Mask.IMask Mask
        {
            get { return _mask; }
            set
            {
                if (_bAutoDisposeMask && _mask != null)
                    DisposeOldMask();
                _mask = value;
                OnMaskChanged();
            }
        }

        protected virtual void OnMaskChanged()
        {
            if (this.MaskChanged != null)
                this.MaskChanged(this, EventArgs.Empty);
        }

        public RasterImagePropertyCollection Properties
        {
            get { return _image.Properties; }
        }

        public bool Modified
        {
            get { return _modified; }
            set
            {
                _modified = value;
                OnModifiedChanged();
            }
        }

        protected virtual void OnModifiedChanged()
        {
            if (this.ModifiedChanged != null)
                this.ModifiedChanged(this, EventArgs.Empty);
        }

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                OnFilePathChanged();
            }
        }

        protected virtual void OnFilePathChanged()
        {
            if (this.FilePathChanged != null)
                this.FilePathChanged(this, EventArgs.Empty);
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnDescriptionChanged();
            }
        }

        protected virtual void OnDescriptionChanged()
        {
            if (this.DescriptionChanged != null)
                this.DescriptionChanged(this, EventArgs.Empty);
        }

        #endregion

        #region Helper Rountines

        public bool IsNearCircle(Point p)
        {
            return _image.IsNearCircle(p);
        }

        public bool IsValidPixel(int xPos, int yPos)
        {
            return (xPos >= 0) && (xPos < this.Width) && (yPos >= 0) && (yPos < this.Height);
        }

        public TYPE GetPixel(int x, int y)
        {
            return (TYPE)_image.getPixel(x, y);
        }

        public void SetPixel(int x, int y, TYPE color)
        {
            _image.setPixel(x, y, color);
        }
        
        public static CommonImage FromFile(System.String filename)
        {
            CommonImage result = null;
            GreyDataImage image = null;

            try
            {                
                image = GreyDataImage.FromFile(filename);

                result = new CommonImage();
                result._image.Dispose();
                result._image = image;
                result._filePath = filename;
            }
            catch (System.Exception exp)
            {
                if (image != null)
                {
                    image.Dispose();
                    image = null;
                }

                if (result != null)
                {
                    result.Dispose();
                    result = null;
                }

                throw exp;
            }

            return result;
        }

        public static CommonImage FromFileStream(FileStream fs)
        {
            CommonImage result = null;
            GreyDataImage image = null;

            try
            {
                Image dnImage = Image.FromStream(fs);                

                image = new GreyDataImage(dnImage as Bitmap);

                result = new CommonImage();
                result._image.Dispose();
                result._image = image;
                //result._filePath = filename;
            }
            catch (System.Exception exp)
            {
                if (image != null)
                {
                    image.Dispose();
                    image = null;
                }

                if (result != null)
                {
                    result.Dispose();
                    result = null;
                }

                throw exp;
            }

            return result;
        }

        public static CommonImage FromMemoryStream(MemoryStream fs)
        {
            CommonImage result = null;
            GreyDataImage image = null;

            try
            {
                Image dnImage = Image.FromStream(fs);

                image = new GreyDataImage(dnImage as Bitmap);

                result = new CommonImage();
                result._image.Dispose();
                result._image = image;
                //result._filePath = filename;
            }
            catch (System.Exception exp)
            {
                if (image != null)
                {
                    image.Dispose();
                    image = null;
                }

                if (result != null)
                {
                    result.Dispose();
                    result = null;
                }

                throw exp;
            }

            return result;
        }

        public void SaveImage(String filePath, eImageFormat format)
        {
            _image.SaveImage(filePath, format);
            _modified = false;
            this.FilePath = filePath;
        }

        public System.Drawing.Bitmap CreateBitmap()
        {
            if (_image != null)
                return _image.CreateBitmap();
            return null;
        }

        public System.Drawing.Bitmap CreateBitmap(int Alpha)
        {
            if (_image != null)
                return _image.CreateBitmap(Alpha);
            return null;
        }

        public TYPE getPixel(int x, int y)
        {
            return (TYPE)_image.getPixel(x, y);
        }

        public static float convertGradian(float angle)
        {
            return (float)(angle * Math.PI) / 180;
        }

        public bool IsSameSize(CommonImage image)
        {
            return image._image.IsSameSize(_image);
            //return true;
        }

        public void CopyWaferBound(CommonImage image)
        {
            this._image.CopyWaferBound(image._image);
        }

        public double AngleDifferent(OrientationType orientation)
        {
            if (orientation == OrientationType.NONE) return 0;
            AffineBitmap af = new AffineBitmap(_image);
            return af.AngleDifferent(orientation);
        }

        public double AngleDifferentFromDownOriginalCW(OrientationType orientation)
        {
            if (orientation == OrientationType.NONE) return 0;
            AffineBitmap af = new AffineBitmap(_image);
            return af.AngleDifferentFromDownOriginalCW(orientation);
        }

        public OrientationType getOrientationType()
        {
            return _image.getOrientationType();
        }

        private double roundradian(double stream)
        {
            int tron = (int)(stream / (Math.PI * 2));
            double result = (stream / (Math.PI * 2) - tron) * Math.PI * 2;
            if (result < 0)
                result = result + Math.PI * 2;
            return result;
        }

        public void Offset(int X, int Y)
        {
            if (_image != null)
                _image.Offset(X, Y);
        }


        #region trebyshevt lookup table helpers

        public static void loadTrebyshevtLookup()
        {
            // Cong edited
            try
            {
                Interpolate.LoadTrebyshevtLookup();
            }
            catch
            {
            }
        }
        #endregion

        #endregion

        #region Image Processing

        public void ClearImage()
        {
            _image.SetImage();
        }

        public void Inversion()
        {
            MatrixFilter filter = new MatrixFilter();
            filter.mData = _image;
            filter.invert();
            _image.MinCurrentView = _image.MINGRAYVALUE;
            _image.MaxCurrentView = _image.MAXGRAYVALUE;
        }

        #region Geometry Routines

        public void Scale(float dx, float dy)
        {
            int dst_width = (int)(_image.Width * dx);
            int dst_height = (int)(_image.Height * dy);
            GreyDataImage result = new GreyDataImage(dst_width, dst_height);
            if (result == null)
                throw new System.OutOfMemoryException();
            SIA.SystemFrameworks.ComputerVision.ResizeInterpolationMethod method =
                SIA.SystemFrameworks.ComputerVision.ResizeInterpolationMethod.Cubic;
            SIA.SystemFrameworks.ComputerVision.CVWrapper.Resize(_image, result, method);
            _image.DeleteImageContent();
            _image.SetImageData(dst_width, dst_height, result.Buffer);
            result.Dispose();
        }

        public void Rotate(int degree)
        {
            AffineBitmap af = new AffineBitmap(_image);
            GreyDataImage result = af.Rotate((double)degree);
            if (result != null)
            {
                _image.Dispose();
                _image = result;
            }
        }

        public void Rotate(double angle)
        {
            AffineBitmap af = new AffineBitmap(_image);
            GreyDataImage result = af.Rotate(angle);
            if (result != null)
            {
                _image.Dispose();
                _image = result;
            }
        }

        public void RotateLeft()
        {
            AffineBitmap af = new AffineBitmap(_image);
            GreyDataImage result = af.Rotate90CCW();
            if (result != null)
            {
                _image.Dispose();
                _image = result;
            }
        }

        public void RotateRight()
        {
            AffineBitmap af = new AffineBitmap(_image);

            GreyDataImage result = af.Rotate90();
            if (result != null)
            {
                _image.Dispose();
                _image = result;
            }
        }

        public void Rotate180()
        {
            AffineBitmap af = new AffineBitmap(_image);
            GreyDataImage result = af.Rotate180();
            if (result != null)
            {
                _image.Dispose();
                _image = result;
            }
        }
        
        public void FlipHoz()
        {
            AffineBitmap af = new AffineBitmap(_image);
            GreyDataImage result = af.VerSymmetric();
            _image.Dispose();
            if (result != null) _image = result;
        }

        public void FlipVer()
        {
            AffineBitmap af = new AffineBitmap(_image);
            GreyDataImage result = af.HozSymmetric();
            _image.Dispose();
            if (result != null) _image = result;

        }

        public unsafe void Resize(int newWidth, int newHeight, int samplingType)
        {
            GreyDataImage result = null;

            try
            {
                result = new GreyDataImage(newWidth, newHeight);
                _image.CopyHeaderInformation(result);

                CVWrapper.Resize(this._image, result, (ResizeInterpolationMethod)samplingType);

                // copy data from result image
                this._image.SetImageData(result.Width, result.Height, result.Buffer);
                result.CopyHeaderInformation(_image);

                result._aData = null;
                result._width = 0;
                result._height = 0;
            }
            catch
            {
                if (result != null)
                    result.Dispose();
                result = null;

                throw;
            }
            finally
            {
            }
        }

        public unsafe void Translate(int dx, int dy)
        {
            GreyDataImage dest = null;

            try
            {
                int width = this._image._width;
                int height = this._image._height;
                int newWidth = width;// + Math.Abs(dx);
                int newHeight = height;// + Math.Abs(dy);
                dest = new GreyDataImage(newWidth, newHeight);

                ushort* src_ptr = this._image._aData;
                ushort* dst_ptr = dest._aData;
                int src_y, src_x;

                for (int y = 0; y < newHeight; y++)
                {
                    for (int x = 0; x < newWidth; x++)
                    {
                        try
                        {
                            src_y = y - dy;
                            src_x = x - dx;
                            if (src_y >= 0 && src_y < height && src_x >= 0 && src_x < width)
                                dst_ptr[y * newWidth + x] = src_ptr[src_y * width + src_x];
                        }
                        catch
                        {
                            throw;
                        }
                    }
                }

                this._image.CopyHeaderInformation(dest);
                this._image.SetImageData(width, height, dst_ptr);
                dest.CopyHeaderInformation(this._image);
                dest._aData = null;
                dest._width = dest._height = 0;
            }
            catch
            {
                if (dest != null)
                    dest.Dispose();
                dest = null;

                throw;
            }
            finally
            {
            }
        }

        public void AlignImage(int combineType, string refImage, int dx, int dy, float angle)
        {
            // translate image
            this.Translate(dx, dy);

            //this.SaveImage(@"C:\translated.bmp", eImageFormat.Bmp);

            // apply calculating operation
            string op = "ADD";
            if (combineType == 0) // addiction
                op = "ADD";
            else if (combineType == 1) // subtraction
                op = "SUB";
            else if (combineType == 2) // multiply
                op = "MUL";
            else if (combineType == 3) // division
                op = "DIV";

            this.kDyadicOperation(op, refImage);

            //this.SaveImage(@"C:\subtracted.bmp", eImageFormat.Bmp);
        }

        public void AlignImage(int combineType, CommonImage refImage, int dx, int dy, float angle)
        {
            // translate image
            this.Translate(dx, dy);

            //this.SaveImage(@"C:\translated.bmp", eImageFormat.Bmp);

            // apply calculating operation
            string op = "ADD";
            if (combineType == 0) // addiction
                op = "ADD";
            else if (combineType == 1) // subtraction
                op = "SUB";
            else if (combineType == 2) // multiply
                op = "MUL";
            else if (combineType == 3) // division
                op = "DIV";

            this.kDyadicOperation(op, refImage, false);

            //this.SaveImage(@"C:\subtracted.bmp", eImageFormat.Bmp);
        }

        #endregion

        #region Correction Function

        public void kOverlayImages(ArrayList stream, kOVERLAY_METHOD type, bool bCopyHeader)
        {
            ArrayList imagelist = new ArrayList();
            for (int i = 0; i < stream.Count; i++)
            {
                CommonImage image = stream[i] as CommonImage;
                if (image._image != null)
                    imagelist.Add(image._image);
                else
                    throw new System.ArgumentNullException("Invalid stream image");
            }

            GreyDataImage result = kOverlay.MergeImages(imagelist, type);
            if (bCopyHeader) _image.CopyHeaderInformation(result);
            _image.Dispose();
            _image = result;
        }

        //BLACK and WHITE
        public bool TransBLACKWHITE(int MarkPoint)
        {
            if (_image != null) return _image.TransBLACKWHITE(MarkPoint);
            return false;
        }

        // Stretch Color
        public bool StretchColor(double low, double high)
        {
            bool bResult = false;
            if (_image != null)
            {
                bResult = _image.StretchColor(low, high);
            }
            return false;
        }

        public void RemoveBlurBackGroundOptionParam(int erosionNumber)
        {
            _image.RemoveBlurBackGroundOptionParam(erosionNumber);
        }

        public void IntensityGlobalBackground(TYPE threshold, float cutoff)
        {
#if GPU_SUPPORTED
            if(DeviceProgram.GPUSupported)
            {
                //this code will be removed in feature
                bool bCreateBuff = !this.HasDeviceBuffer;
                if (bCreateBuff)
                    this.CreateDeviceBuffer();

                //////////////////////////////////////
                FFTFilterType filter_type = FFTFilterType.LowPass;
                float weight = 100;

                DeviceBuffer<ushort> original_image = this.CloneDeviceMemory();
                this.FFTFilter(filter_type, cutoff, weight);
                this.kThresholdGPU(0, false, threshold, true);
                kDyadicOperationGPU("SUB", original_image,cbImage,Width,Height,MinGreyValue,MaxGreyValue);
                cbImage.Dispose();
                cbImage = original_image;

                ///////////////////////////////////////
                //this code will be removed in feature
                this.ReadDataFromDeviceBuffer();
                if (bCreateBuff)
                    this.DisposeDeviceBuffer();
            }
            else
            {
                FFTFilterType filter_type = FFTFilterType.LowPass;
                float weight = 100;

                GreyDataImage original_image = (GreyDataImage)_image.Copy();

                _image.FFTFilter((int)filter_type, cutoff, weight);

                _image.kThreshold(0, false, threshold, true);
                original_image.kDyadicOperation("SUB", _image);
                _image.Dispose();
                _image = original_image;
            }

#else
            FFTFilterType filter_type = FFTFilterType.LowPass;
            float weight = 100;

            GreyDataImage original_image = (GreyDataImage)_image.Copy();

            _image.FFTFilter((int)filter_type, cutoff, weight);

            _image.kThreshold(0, false, threshold, true);
            original_image.kDyadicOperation( "SUB", _image);
            _image.Dispose();
            _image = original_image;
#endif
        }

        public void ExtractGlobalBackgroundByFFT(double threshold, float cutoff)
        {
#if GPU_SUPPORTED
            if(DeviceProgram.GPUSupported)
            {
                //this code will be removed in feature
                bool bCreateBuff = !this.HasDeviceBuffer;
                if (bCreateBuff)
                    this.CreateDeviceBuffer();

                FFTFilterType filter_type = FFTFilterType.LowPass;
                float weight = 100;
                this.FFTFilter(filter_type, cutoff, weight);
                this.kThresholdGPU(0, false, threshold, true);

                ///////////////////////////////////////
                //this code will be removed in feature
                this.ReadDataFromDeviceBuffer();
                if (bCreateBuff)
                    this.DisposeDeviceBuffer();
            }
            else
            {
                FFTFilterType filter_type = FFTFilterType.LowPass;
                float weight = 100;
                _image.FFTFilter((int)filter_type, cutoff, weight);
                _image.kThreshold(0, false, threshold, true);
            }
#else
            FFTFilterType filter_type = FFTFilterType.LowPass;
            float weight = 100;
            _image.FFTFilter((int)filter_type, cutoff, weight);
            _image.kThreshold(0, false, threshold, true);
#endif
        }

        public void ExtractGlobalBackgroundByErosion(int numOfErosion)
        {
            _image.ExtractGlobalBackgroundByErosion(numOfErosion);
        }

        public void kMonadicOperation(String Operator, float val, bool bUseMask)
        {
            _image.kMonadicOperation(Operator, val, bUseMask ? (_maskImage as Bitmap) : null);
        }

        public void kDyadicOperation(String Operator, String Operand)
        {
            _image.kDyadicOperation(Operator, Operand);
        }

        public void kDyadicOperation(String Operator, Bitmap Operand, bool bUseMask)
        {
            if (bUseMask)
                _image.kDyadicOperation(Operator, Operand, _maskImage as Bitmap);
            else
                _image.kDyadicOperation(Operator, Operand);
        }

        public void kDyadicOperation(String Operator, CommonImage Operand, bool bUseMask)
        {
            if (bUseMask)
                _image.kDyadicOperation(Operator, Operand._image, _maskImage as Bitmap);
            else
                _image.kDyadicOperation(Operator, Operand._image);
        }


        #endregion

        #region Filter

        /// <summary>
        /// Do command smooth filter.
        /// </summary>
        public void SmoothFilter()
        {
#if MEASURE_PERFORMANCE
			System.DateTime  start = System.DateTime.Now;
#endif
            MatrixFilter filter = new MatrixFilter(1, ImageFilterType.SMOOTH);
            filter.mData = _image;
            filter.filterData();

#if MEASURE_PERFORMANCE
			System.DateTime  endtime = System.DateTime.Now;
			System.Diagnostics.Debug.Write("Smooth filter :"  );
			System.Diagnostics.Debug.Write((endtime - start).ToString());
			System.Diagnostics.Debug.Write("\r\n"  );
#endif
        }

        /// <summary>
        /// Do command filter Gaussian.
        /// </summary>
        public void GaussianSmoothFilter()
        {
            MatrixFilter filter = new MatrixFilter(1, ImageFilterType.GAUSSIAN);
            filter.mData = _image;
            filter.filterData();
        }

        /// <summary>
        /// Do command sharpening filter.
        /// </summary>
        public void ShapreningFilter()
        {
            MatrixFilter filter = new MatrixFilter(1, ImageFilterType.SHARPENING);
            filter.mData = _image;
            filter.filterDataCheckBounds();
        }

        /// <summary>
        /// Do command Laplacian filter.
        /// </summary>
        public void LaplasianFilter()
        {
            MatrixFilter filter = new MatrixFilter(1, ImageFilterType.LAPLACE, _image);
            filter.filterDataCheckBounds();
        }

        public void DilationFilter()
        {
            _image = _image.DilationFilter(1);
        }

        public void DilationFilter(int nHalf)
        {
            _image = _image.DilationFilter(nHalf);
        }

        public void ErosionFilter()
        {
            _image = _image.ErosionFilter(1);
        }

        public void ErosionFilter(int nHalf)
        {
            _image = _image.ErosionFilter(nHalf);
        }

        public void ErosionFilter(int nHalf, CommonImage erosion_image)
        {
            _image = _image.ErosionFilter(nHalf, erosion_image._image);
        }

        public void HotPixelFilter(float _percentage)
        {
            MatrixFilter filter = new MatrixFilter(1, ImageFilterType.HOT_PIXEL);
            filter.mData = _image;
            GreyDataImage oldImage = _image;
            _image = filter.HotPixelFilter(_percentage);
            oldImage.Dispose(true);
            //Phong new-code
            //kFilter filter = new kHotPixel(3,3);
            //TODO: in completed
        }

        public void DeadPixelFilter(float _percentage)
        {
            MatrixFilter filter = new MatrixFilter(1, ImageFilterType.DEAD_PIXEL);
            filter.mData = _image;
            GreyDataImage oldImage = _image;
            _image = filter.DeadPixelFilter(_percentage);
            oldImage.Dispose(true);
        }

        /// <summary>
        /// Do command Emboss 135 filter.
        /// </summary>
        public void FilterEmboss135()
        {
            MatrixFilter filter = new MatrixFilter(1, ImageFilterType.EMBOSS135, _image);
            filter.filterDataCheckBounds();
        }

        /// <summary>
        /// Do filter Emboss 90 degree.
        /// </summary>
        public void FilterEmboss90()
        {
            MatrixFilter filter = new MatrixFilter(1, ImageFilterType.EMBOSS90, _image);
            filter.mData = _image;
            filter.filterDataCheckBounds();
        }

        /// <summary>
        /// Do command edge detection filter.
        /// </summary>
        public void FilterOutline()
        {
            MatrixFilter filter = new MatrixFilter(1);
            filter.mData = _image;
            filter.EgdeDetection();
        }

        public void FilterMedian()
        {
            kFilter filter = new kMedian();
            filter.Apply(_image, 1);
            filter.Dispose();
        }

        public void FilterVariance(float radius)
        {
            kFilter filter = null;
            int pass = 0;

            try
            {
                filter = new kVariance(radius, _image);
                filter.Apply(_image, pass);
            }
            finally
            {
                if (filter != null)
                    filter.Dispose();
                filter = null;
            }
        }

        public void FilterRank()
        {
        }

        public void FilterRank(int typeFilter, int kernel, int pass)
        {
            SIA.IPEngine.RankFilter rankFilter = null;
            try
            {
                rankFilter = new SIA.IPEngine.RankFilter(this.RasterImage, kernel, kernel);
                rankFilter.Apply(typeFilter, pass);
            }
            catch (System.Exception exp)
            {
                Trace.WriteLine(exp);
            }
            finally
            {
                rankFilter = null;
            }
        }

        public void LightenUp()
        {
            _image.kMonadicOperation("ADD", System.Convert.ToUInt16((_image.MAXGRAYVALUE - _image.MINGRAYVALUE) / 17));
        }

        public void MatrixErosion(int nHalf)
        {
            GreyDataImage oldImage = _image;
            _image = _image.ErosionFilter(nHalf);
            oldImage.Dispose(true);
        }

        public void MatrixDilation(int nHalf)
        {
            GreyDataImage oldImage = _image;
            _image = _image.DilationFilter(nHalf);
            oldImage.Dispose(true);
        }

        public void ScrachFilter(int nHalf)
        {
            _image.ScratchFilter(nHalf);
        }

        public void ScrachFilter(int nHalf, TYPE threshold)
        {
            _image.TransBLACKWHITE((int)threshold);
            _image.ScratchFilter(nHalf);
        }

        public void KeepSolid(int nHalf)
        {
            GreyDataImage result = null;
            _image.KeepSolidCluster(nHalf);
            _image.Dispose();
            _image = result;
        }

        public void KeepSolid(int nHalf, TYPE threshold)
        {
            GreyDataImage result = null;
            _image.TransBLACKWHITE((int)threshold);
            result = _image.KeepSolidCluster(nHalf);
            _image.Dispose();
            _image = result;
        }

        public void kHistogramEqualization()
        {
            kHistogram histogram = Histogram;
            histogram.Equalize();
        }

        public void kDistanceTransform(eDM_TYPE type)
        {
            kClustering.kDistanceTransform(_image, type);
        }


        public void kSkeletonize(double threshold)
        {
            kClustering.kSkeletonize(_image, (TYPE)threshold);
        }

        public void kThreshold(double MinThreshold, bool bZeroMin, double MaxThreshold, bool bZeroMax)
        {
            _image.kThreshold(MinThreshold, bZeroMin, MaxThreshold, bZeroMax);
        }

        public void kThreshold(TYPE threshold)
        {
            _image.kThreshold(threshold);
        }

        public void kApplyFilter(eMaskType maskType, eMatrixType matrixType, int pass)
        {

            using (kFilter filter = kKernelFilters.QueryConvolutionFilter(maskType, matrixType))
            {
                GreyDataImage output = (GreyDataImage)_image.Copy();
                filter.Apply(_image, output, pass);
                _image.Dispose();
                filter.Dispose();
                _image = output;
            }
        }

        public void kApplyFilter(eMaskType maskType, eMatrixType matrixType, int pass, float threshold)
        {
            if (maskType == eMaskType.kMASK_HOTPIXEL /*&& matrixType == eMatrixType.kMATRIX_3x3*/)
            {
                HotPixelFilter(threshold);
            }
            else if (maskType == eMaskType.kMASK_DEADPIXEL /*&& matrixType == eMatrixType.kMATRIX_3x3*/)
            {
                DeadPixelFilter(threshold);
            }
            else
            {

                using (kFilter filter = kKernelFilters.QueryConvolutionFilter(maskType, matrixType))
                {
                    //GreyDataImage output = (GreyDataImage)_image.Copy();
                    //filter.Apply(_image, output, pass);
                    //_image.Dispose();
                    //filter.Dispose();
                    //_image = output;

                    //Phong - use _ASM_FILTER_
                    filter.Apply(_image, pass);
                    filter.Dispose();
                }
            }
        }

        public void kApplyFilter(eMorphType morphType, eMatrixType matrixType, int pass)
        {
            using (kFilter filter = kKernelFilters.QueryMorphologyFilter(morphType, matrixType))
            {
                GreyDataImage output = (GreyDataImage)_image.Copy();
                filter.Apply(_image, output, pass);
                _image.Dispose();
                _image = output;
            }
        }

        public void kApplyFilter(int[,] Matrix, int pass)
        {
            using (kFilter filter = kKernelFilters.QueryConvolutionFilter(Matrix))
            {
                //GreyDataImage output = _image.Copy();
                //filter.Apply(_image, output, pass);
                //filter.Apply(_image, null, pass);		
                //_image.Dispose();
                //_image = output;
                //filter.Dispose();

                //Phong - use _ASM_FILTER_
                filter.Apply(_image, pass);
                filter.Dispose();
            }
        }

        #endregion

        #region Distortion

        public void kDistortion_Removal(float focal_length, PointF principal_point, float[] distCoeffs, bool interpolation)
        {
            float[] intrMatrix = new float[9];
            /* focal length */
            //intrMatrix[0] = 3550.0f;
            //intrMatrix[4] = 3550.0f;
            intrMatrix[0] = focal_length;
            intrMatrix[4] = focal_length;
            /* principal point */
            //intrMatrix[2] = _image.Width /2.0f;
            //intrMatrix[5] = _image.Height/2.0f;
            intrMatrix[2] = principal_point.X;
            intrMatrix[5] = principal_point.Y;
            /* distortion coefficients */
            //float[] distCoeffs = new float [] {0.28f, -0.48f, -0.007f, 0.003f};

            GreyDataImage output = (GreyDataImage)_image.Copy();
            kCameraCorrection.kUnDistort(_image, output, intrMatrix, distCoeffs, interpolation);
            _image.Dispose();
            _image = output;
        }

        public ArrayList kDistortion_Removal(String filename)
        {
            kCameraCorrection cameraCorrection = new kCameraCorrection(_image);
            return cameraCorrection.kDistortion_Removal(filename);
        }

        public void kDistortion_Removal(ArrayList source, ArrayList dest)
        {
            kCameraCorrection cameraCorrection = new kCameraCorrection(_image);
            cameraCorrection.kDistortion_Removal(source, dest);
        }

        public static void kDistortion_MakeFilter(
            String source, String dest, String output, int Radius, double threshold)
        {
            ArrayList arSourcePoints = kCameraCorrection.kDistortion_ExtractData(source, threshold);
            ArrayList arDestPoints = kCameraCorrection.kDistortion_ExtractData(dest, threshold);
            if (arSourcePoints.Count != arDestPoints.Count)
                throw new Exception("Source and Dest Point do not match ");
            kCameraCorrection.kDistortion_MatchPoints(arSourcePoints, arDestPoints, Radius);
            kCameraCorrection.kDistortion_MakeFilter(arSourcePoints, arDestPoints, output);
        }

        public static void kDistortion_LoadFilter(String filename, ArrayList source, ArrayList dest)
        {
            if (kCameraCorrection.kDistortion_LoadFilter(filename, source, dest) == false)
                throw new Exception("invalid data format or bad filter");
        }

        public void RemoveRadialDistortion(
            float focalLength, PointF principalPoint, float[] distortionCoeffs, bool interpolation)
        {
            float[] intrMatrix = new float[9];
            /* focal length */
            //			intrMatrix[0] = 3550.0f;
            //			intrMatrix[4] = 3550.0f;
            intrMatrix[0] = focalLength;
            intrMatrix[4] = focalLength;
            /* principal point */
            //			intrMatrix[2] = mData.Width /2.0f;
            //			intrMatrix[5] = mData.Height/2.0f;
            intrMatrix[2] = principalPoint.X;
            intrMatrix[5] = principalPoint.Y;
            /* distortion coefficients */
            //			float[] distCoeffs = new float [] {0.28f, -0.48f, -0.007f, 0.003f};

            this.RemoveRadialDistortion(intrMatrix, distortionCoeffs, interpolation);
        }

        public void RemoveRadialDistortion(
            float[] instrisicParams, float[] distortionCoeffs, bool interpolation)
        {
            GreyDataImage input = this._image;
            GreyDataImage output = null;

            try
            {
                // clone a copy of internal image
                output = input.Copy();
                if (output == null)
                    throw new OutOfMemoryException();

                kCameraCorrection.kUnDistort(
                    input, output, instrisicParams, distortionCoeffs, interpolation);

                // release old image
                if (input != null)
                    input.Dispose();
                input = null;

                // save output data
                this._image = output;
            }
            catch
            {
                if (output != null)
                    output.Dispose();
                output = null;
                throw;
            }
            finally
            {
            }
        }

        public void RemoveBendDistortion(int min_threshold, int max_threshold, bool isflat)
        {
#if false        
            SIA.IPEngine.Distortion distortion = new Distortion(this._image);
            distortion._Min_Threshold = min_threshold;
            distortion._Max_Threshold = max_threshold;
            distortion._bFlat = isflat;

            distortion.Run();

            if (distortion._MarkType == WaferMarkType.Notch)
            {
                NotchMark mark = new NotchMark();
                mark.Angle = _image.Notch;
                this.Notch = mark.Angle;
                this.WaferMark = mark;
            }
            else if (distortion._MarkType == WaferMarkType.Flat)
            {
                FlatMark mark = new FlatMark();
                PointF p1 = distortion._FlatPoint1;
                PointF p2 = distortion._FlatPoint2;
                PointF p = new PointF((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
                float xCenter = _image.XBoundCenter;
                float yCenter = _image.YBoundCenter;
                if (_image.Radius > 0)
                {
                    //double notch_angle =Math.Atan2(-(p.Y - yCenter),p.X - xCenter);
                    mark.PrimaryFlatAngle = distortion.GetNotchAngle();
                    double flat_length = Math.Sqrt((p.X - xCenter) * (p.X - xCenter) + (p.Y - yCenter) * (p.Y - yCenter));
                    mark.PrimaryFlatRatio = flat_length / _image.Radius;
                    mark.PrimaryFlatLength = flat_length;
                    this.WaferMark = mark;
                }
            }
#endif
        }
        #endregion

        #region Mask Helpers
		[Obsolete]
        public void kApplyMask(System.Drawing.Image mask_image, int mask_id)
        {
            ArrayList fragments = kFragmentFactory.Extract(mask_image, mask_id);
            // execute defect extractor using trend line method
            if (fragments != null && fragments.Count > 0)
            {
            }
        }

		[Obsolete]
		public Image MaskAndWaferbound()
        {
            Bitmap new_maskImage = null;

            GreyDataImage gi = null;
            if (Mask != null)
                gi = new GreyDataImage((Bitmap)this.MaskImage);
            else
                gi = _image;
            for (int y = 0; y < _image.Height; y++)
            {
                for (int x = 0; x < _image.Width; x++)
                {
                    if (!_image.WithinWaferBoundNotBorder(x, y))
                    {
                        gi.setPixel(x, y, (TYPE)Constants.MASK_ID + 1);
                    }
                }
            }
            new_maskImage = gi.CreateBitmap();
            gi.Dispose();
            return new_maskImage;

        }

        public Image MaskOfWaferbound()
        {
            Bitmap new_maskImage = null;
            GreyDataImage gi = null;
            try
            {
                gi = new GreyDataImage(_image.Width, _image.Height);
                for (int y = 0; y < _image.Height; y++)
                {
                    for (int x = 0; x < _image.Width; x++)
                    {
                        if (!_image.WithinWaferBoundNotBorder(x, y))
                        {
                            gi.setPixel(x, y, (TYPE)Constants.MASK_ID + 1);
                        }
                        else
                        {
                            gi.setPixel(x, y, (TYPE)Constants.MASK_ID);
                        }
                    }
                }
                new_maskImage = gi.CreateSimpleBitmap();
                gi.Dispose();
                return new_maskImage;
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (gi != null) gi.Dispose();
            }

        }

        public void zeroDataMaskOuter()
        {
            GreyDataImage maskGI = null;
            try
            {

                if (this.Mask != null)
                {
                    maskGI = new GreyDataImage((Bitmap)this.MaskImage);
                    int MaskID = Constants.MASK_ID;
                    for (int y = 0; y < this.Height; y++)
                    {
                        for (int x = 0; x < this.Width; x++)
                        {
                            if (maskGI.getPixel(x, y) != MaskID)
                            {
                                _image.setPixel(x, y, 0);
                            }

                        }
                    }

                }
            }
            catch (System.Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (maskGI != null)
                    maskGI.Dispose();
            }

        }
        #endregion

        #region Global Background Correction

        /// <summary>
        /// Global Background Correction Common Interface
        /// </summary>
        /// <param name="type">type of Correction will be used</param>
        /// <param name="args">arguments of specified type</param>
        /// <returns>specified return value</returns>

        public System.Object GlobalBackgroundCorrection(
            eGlobalBackgroundCorrectionType type, params object[] args)
        {
            System.Object objResult = null;

            if (type == eGlobalBackgroundCorrectionType.FastFourierTransform)
            {
                TYPE Threshold = 0;
                float CutOff = 0;

                if (args.Length != 2)
                    throw new System.ArgumentException("No overload for GlobalBackgroundCorrection FFT Method take " + (args.Length) + " arguments");

                if (!(args[0] is TYPE))
                    throw new System.ArgumentException("Type of Threshold Parameter must be " + typeof(TYPE).ToString());
                else
                    Threshold = (TYPE)args[0];

                if (!(args[1] is float))
                    throw new System.ArgumentException("Type of FFT Cut Off Parameter must be " + typeof(float).ToString());
                else
                    CutOff = (float)args[1];

                objResult = GlobalBackgroundCorrection(Threshold, CutOff);
            }
            else if (type == eGlobalBackgroundCorrectionType.ErosionFilter)
            {
                int nErosionStep = 0;

                if (args.Length != 1)
                    throw new System.ArgumentException("No overload for GlobalBackgroundCorrection Erosion Method take " + (args.Length) + " arguments");

                if (!(args[0] is int))
                    throw new System.ArgumentException("Type of Erosion Parameter must be " + typeof(int).ToString());
                else
                    nErosionStep = (int)args[0];

                objResult = GlobalBackgroundCorrection(nErosionStep);
            }
            else if (type == eGlobalBackgroundCorrectionType.ReferenceImages)
            {
                String[] file_paths = null;

                foreach (System.Object arg in args)
                    if (!(arg is string))
                        throw new System.ArgumentException("Type of Reference Image Parameter must be " + typeof(string).ToString());

                file_paths = (string[])args;

                objResult = GlobalBackgroundCorrection(file_paths);
            }
            else if (type == eGlobalBackgroundCorrectionType.UnsharpFilter)
            {
                IRasterImage image = this._image;
                //SIA.SystemFrameworks.BEDE.BEDEPOCs.EstimateGlobalBackground(image);
                GlobalBackGroundCorrection.EstimateGlobalBackground(image, (UnsharpParam)args[0]);
            }

            return objResult;
        }

        private System.Boolean GlobalBackgroundCorrection(
            TYPE threshold, float FFTCutOff)
        {
            System.Boolean bResult = false;
            try
            {
                this.IntensityGlobalBackground(threshold, FFTCutOff);
                bResult = true;
            }
            catch (System.Exception exp)
            {
                throw exp;
            }
            return bResult;
        }

        private System.Boolean GlobalBackgroundCorrection(int nErosionStep)
        {
            System.Boolean bResult = false;
            try
            {
                this.RemoveBlurBackGroundOptionParam(nErosionStep);
                bResult = true;
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return bResult;
        }

        private System.Int32 GlobalBackgroundCorrection(
            string[] ref_image_paths)
        {
            if (ref_image_paths == null || ref_image_paths.Length <= 0)
                throw new System.ArgumentException("Invalid reference image paths ");

            int num_ref_images = ref_image_paths.Length;
            String[] file_paths = ref_image_paths;
            Int64[] distances = new Int64[num_ref_images];
            Int64 total_pixels = _image.Length;
            int selected_images_index = -1;
            Int64 min_distance = Int64.MaxValue;

            for (int j = 0; j < num_ref_images; j++)
                distances[j] = Int64.MaxValue;

            if (num_ref_images > 1)
            {
                for (int i = 0; i < num_ref_images; i++)
                {
                    String path = file_paths[i];
                    GreyDataImage cloneImage = (GreyDataImage)_image.Copy();
                    GreyDataImage image = null;

                    try
                    {
                        image = GreyDataImage.FromFile(path);
                    }
                    catch (System.Exception exp)
                    {
                        throw new System.Exception(
                            "Failed to load file \"" + path + "\"", exp);
                    }

                    try
                    {
                        if (image._width == cloneImage._width && image._height == cloneImage._height)
                            distances[i] = cloneImage.ComputeTotalIntensityDistance(image);
                        else
                            throw new System.Exception(
                                "Size of the image \"" + path + "\" does not match with the processed image.");
                    }
                    catch (System.Exception exp)
                    {
                        throw exp;
                    }
                    finally
                    {
                        if (image != null)
                        {
                            image.Dispose();
                            image = null;
                        }
                        if (cloneImage != null)
                        {
                            cloneImage.Dispose();
                            cloneImage = null;
                        }
                    }
                }

                // choose best fit background image by selecting minimum distance 
                for (int i = 0; i < num_ref_images; i++)
                {
                    if (distances[i] < min_distance)
                    {
                        min_distance = distances[i];
                        selected_images_index = i;
                    }
                }
            }
            else if (num_ref_images == 1)
            {
                // set default background image
                selected_images_index = 0;
            }


            if (selected_images_index >= 0 && selected_images_index < num_ref_images)
            {
                CommonImage subImage = null;
                try
                {
                    subImage = CommonImage.FromFile(file_paths[selected_images_index]);
                }
                catch (System.Exception exp)
                {
                    throw new System.Exception(
                        "Failed to load file \"" + file_paths[selected_images_index] + "\"", exp);
                }

                this.kDyadicOperation("SUB", subImage, false);
                subImage.Dispose();
            }

            return selected_images_index;
        }


        public Int64 GetTotalIntensity()
        {
            return _image.GetTotalIntensity();
        }

        public Int64 GetTotalIntensityDifferent(CommonImage image)
        {
            if (image == null)
                throw new ArgumentNullException("Invalid common image parameter");
            return _image.GetTotalIntensityDifferent(image._image);
        }

        public int ComputeMatchingPercent(CommonImage image, float delta)
        {
            if (image == null)
                throw new ArgumentNullException("Invalid common image parameter");
            return _image.ComputeMatchingPercent(image._image, delta);
        }
        #endregion

        #endregion

        #region Analysis Routines

        public System.Collections.ArrayList GetDetachPoint(int MarkPoint)
        {
            if (_image != null) return _image.GetDetachPoint(MarkPoint);
            return null;
        }

		// unknown purpose
        public System.Collections.ArrayList GetDetachPoint(int MinMarkPoint, int MaxMarkPoint)
        {
            if (_image != null) return _image.GetDetachPoint(MinMarkPoint, MaxMarkPoint);
            return null;
        }

        #region Get List of Defects

        public ArrayList GetDefect(TYPE threshold)
        {
            return _image.getRealDefect(threshold);
        }

        public ArrayList GetDefect(double minGrayscaleThreshold, double maxGrayscaleThreshold)
        {
            if (maxGrayscaleThreshold > TYPE.MaxValue)
            {
                maxGrayscaleThreshold = TYPE.MaxValue;
            }
            if (minGrayscaleThreshold > TYPE.MaxValue)
            {
                minGrayscaleThreshold = TYPE.MaxValue;
            }
            TYPE minGrayValue = (TYPE)minGrayscaleThreshold;
            TYPE maxGrayValue = (TYPE)maxGrayscaleThreshold;
            return _image.getRealDefect(minGrayValue, maxGrayValue);
        }

        #endregion

        
        #region Zone Analysis

        public void createZone()
        {
            Zone zone = new Zone();
            zone._data = _image;
            zone.createZoneOnImage();
        }

        public void createZone(int startProgress, int endProgress)
        {
            Zone zone = new Zone();
            zone._data = _image;
            zone.createZoneOnImage(startProgress, endProgress);
        }

        public ZoneType ZoneType
        {
            get
            {
                return _image.mZone;
            }
        }

        #endregion

        #region Contour Helpers

        public ArrayList GetClustering(
            int distance, double threshold, int suspendnumdefect, int suspendmaxdistance)
        {
            if (distance < 0 || threshold < 0) return new ArrayList();
            Clustering clr = new Clustering(_image);
            return clr.GetClusterings(
                distance, threshold, suspendnumdefect, suspendmaxdistance);
        }

        public ArrayList GetClustering(
            int distance, double threshold, double maxthreshold, int suspendnumdefect, int suspendmaxdistance)
        {
            if (distance < 0 || threshold < 0) return new ArrayList();
            Clustering clr = new Clustering(_image);
            return clr.GetClusterings(
                distance, threshold, maxthreshold, suspendnumdefect, suspendmaxdistance);
        }

        public ArrayList GetClustering(ClusteringParam cls_param)
        {
            double distance = cls_param.distance;
            if (distance < 0) return new ArrayList();
            SIA.IPEngine.Clustering clr = new Clustering(_image);
            return clr.GetClusterings(cls_param);
        }

        public ArrayList GetDefects(bool[] mtThreshold, bool[] mtPSLThreshold)
        {
            SIA.IPEngine.Clustering clr = new Clustering(_image);
            return clr.GetDefects(mtThreshold, mtPSLThreshold);
        }

        public ArrayList GetRealDefect(double minthreshold, double maxthreshold)
        {
            return _image.getRealDefect((ushort)minthreshold, (ushort)maxthreshold);
        }

        public ArrayList GetContours(
            int distance, double threshold, int suspendnumdefect, int suspendmaxdistance)
        {
            if (distance < 0 || threshold < 0) return new ArrayList();
            SIA.IPEngine.Clustering clr = new Clustering(_image);
            return clr.GetContour(
                distance, threshold, suspendnumdefect, suspendmaxdistance);
        }

        #endregion

        #region Histogram Helpers

        public Bitmap kGetHistogramBitmap(int width, int height)
        {
            Bitmap bmpChart = null;
            Graphics graph = null;

            try
            {
                /* Extract Histogram Data */
                Double iMaxCount = 0, iMinCount = 0;
                Double Mean = 0.0f, StdDev = 0.0f;
                Double Median = 0;
                Double[] arHistogram = null;
                kHistogram histogram = _image.get_Histogram();
                arHistogram = histogram.Histogram;
                iMaxCount = histogram.MaxCount;
                iMinCount = histogram.MinCount;
                Mean = histogram.Mean;
                StdDev = histogram.StdDev;
                Median = histogram.Median;

                /* Export Histogram bitmap */
                int maxWidth = (int)arHistogram.Length;
                int maxHeight = (int)iMaxCount;

                bmpChart = new System.Drawing.Bitmap(
                    width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                graph = Graphics.FromImage(bmpChart);
                System.Drawing.Pen blackPen = new System.Drawing.Pen(Color.Black, 1.0f);
                graph.Clear(Color.Gray);

                Double iCount = 0;
                float fXScaleFactor = (float)width / (float)(maxWidth);
                float fYScaleFactor = (float)height / (float)(maxHeight);

                for (int i = 0; i < maxWidth; i++)
                {
                    iCount = arHistogram[i];
                    graph.DrawLine(
                        blackPen, (float)i, (float)(height - (iCount * fYScaleFactor)), 
                        (float)i, (float)height);
                }

                graph.Dispose();
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.ToString());
            }
            return bmpChart;
        }

        public kHistogram Histogram
        {
            get { return _image.get_Histogram(); }
        }

        protected TYPE kGetPixel(int x, int y)
        {
            if (x < 0 || x >= _image._width) return 0;
            if (y < 0 || y >= _image._height) return 0;
            return GetPixel(x, y);
        }

        public TYPE[] kGetHistogramOfLine(
            Point pt1, Point pt2, ref TYPE iMaxValue, ref TYPE iMinValue)
        {
            //prepare data
            int A = pt2.Y - pt1.Y;
            int B = pt1.X - pt2.X;
            int C = pt1.Y * (pt2.X - pt1.X) - pt1.X * (pt2.Y - pt1.Y);
            int delta, plus;
            TYPE[] ydata;
            iMinValue = TYPE.MaxValue;
            iMaxValue = TYPE.MinValue;

            if (Math.Abs(pt2.X - pt1.X) >= Math.Abs(pt2.Y - pt1.Y))
            {
                delta = Math.Abs(pt2.X - pt1.X);
                plus = pt2.X - pt1.X > 0 ? 1 : -1;
                ydata = new TYPE[delta + 1];
                for (int i = 0; i <= delta; i++)
                {
                    int x = pt1.X + i * plus;
                    ydata[i] = kGetPixel(x, CustomConfiguration.yABC(A, B, C, x));
                    iMinValue = Math.Min(iMinValue, ydata[i]);
                    iMaxValue = Math.Max(iMaxValue, ydata[i]);
                }
            }
            else
            {
                delta = Math.Abs(pt2.Y - pt1.Y);
                plus = pt2.Y - pt1.Y > 0 ? 1 : -1;

                //prepare data
                ydata = new TYPE[delta + 1];
                for (int i = 0; i <= delta; i++)
                {
                    int x = pt1.Y + i * plus;
                    ydata[i] = kGetPixel(CustomConfiguration.xABC(A, B, C, x), x);
                    iMinValue = Math.Min(iMinValue, ydata[i]);
                    iMaxValue = Math.Max(iMaxValue, ydata[i]);
                }
            }
            return ydata;
        }

        public TYPE[] kGetHistogramOfRow(
            int iRow, int iBeginCol, int iEndCol, ref TYPE iMaxValue, ref TYPE iMinValue)
        {
            return _image.getRowData(iRow, iBeginCol, iEndCol, ref iMaxValue, ref iMinValue);
        }

        public TYPE[] kGetHistogramOfCol(
            int iCol, int iBeginRow, int iEndRow, ref TYPE iMaxValue, ref TYPE iMinValue)
        {
            return _image.getColData(iCol, iBeginRow, iEndRow, ref iMaxValue, ref iMinValue);
        }

        public SIA.SystemFrameworks.Histogram 
            ComputeHistogram(Bitmap mask, Rectangle rcMask)
        {
            return SIA.IPEngine.kHistogram.ComputeHistogram(this._image, mask, rcMask);
        }

        public SIA.SystemFrameworks.Histogram 
            ComputeHistogram(SIA.Common.Mask.IMask mask)
        {
            return SIA.IPEngine.kHistogram.ComputeHistogram(this._image, mask);
        }

        #endregion

        #region Clustering

        public ArrayList ReClustering(double Threshold, int MinFragment)
        {
            return kClustering.kReClustering(
                _image, (TYPE)Threshold, MinFragment);
        }

        #endregion


#if GPU_SUPPORTED
        
        /// <summary>
        /// Do command Fourier transform.
        /// </summary>
        /// <param name="filter_type">Fourier filter type.</param>
        /// <param name="cutoff">Cutoff value.</param>
        /// <param name="weight">Weight value.</param>
        public void FFTFilter(FFTFilterType filter_type, float cutoff, float weight)
        {
            if(!DeviceProgram.GPUSupported)
            {
                _image.FFTFilter((int)filter_type, cutoff, weight);
                return;
            }

            //GPU
            //this code will be removed in feature
            bool bCreateBuff = !this.HasDeviceBuffer;
            if (bCreateBuff)
                this.CreateDeviceBuffer();

            //////////////////////////////////////
            if (cbImage == null) return;
            DeviceFFTFloat fftf = new DeviceFFTFloat();
            fftf.FFTApply(cbImage, Width, Height, weight, cutoff, (int)filter_type);
            ///////////////////////////////////////

            //this code will be removed in feature
            this.ReadDataFromDeviceBuffer();
            if (bCreateBuff)
                this.DisposeDeviceBuffer();
        }
#else
        /// <summary>
        /// Do command Fourier transform.
        /// </summary>
        /// <param name="filter_type">Fourier filter type.</param>
        /// <param name="cutoff">Cutoff value.</param>
        /// <param name="weight">Weight value.</param>
        public void FFTFilter(FFTFilterType filter_type, float cutoff, float weight)
        {
            _image.FFTFilter((int)filter_type, cutoff, weight);
        }
#endif


        #endregion

        #region ICloneable Members

        public object Clone()
        {
            return new CommonImage(this);
        }

        #endregion

        #region IDisposable Members

        protected virtual void Dispose(bool manual)
        {
            if (manual)
            {

#if GPU_SUPPORTED
                //if (_bAutoDisposeImage)//this line will be removed
                {
                    DisposeDeviceBuffer();
                }
#endif

                if (_image != null && _bAutoDisposeImage)
                {
                    _image.Dispose();
                    _image = null;
                }

                if (_maskImage != null && _bAutoDisposeMask)
                {
                    _maskImage.Dispose();
                    _maskImage = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        //~CommonImage()
        //{
        //    Dispose(false);
        //}

        #endregion        

        #region history helpers

		// dump data into a file quickly
        public bool QuickLoad(String filename)
        {
            bool bResult = false;
            FileStream stream = null;
            try
            {
                stream = new FileStream(filename, FileMode.Open);
                this.QuickLoad(stream);
                bResult = true;
            }
            catch (System.Exception exp)
            {
                Trace.WriteLine(exp);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                stream = null;
            }

            return bResult;
        }

        public bool QuickSave(String filename)
        {
            bool bResult = false;
            FileStream stream = null;
            try
            {
                stream = new FileStream(filename, FileMode.Create);
                bResult = this.QuickSave(stream);
            }
            catch (System.Exception exp)
            {
                Trace.WriteLine(exp);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                stream = null;
            }

            return bResult;
        }

        public bool QuickLoad(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            bool bResult = false;
            using (BinaryReader reader = new BinaryReader(stream))
            {
                try
                {
                    bResult = _image.QuickLoad(reader);
                    if (bResult)
                    {
                        int length = reader.ReadInt32();
                        if (length > 0) this._filePath = new String(reader.ReadChars(length));
                        length = reader.ReadInt32();
                        if (length > 0) this._description = new String(reader.ReadChars(length));

                        if (_maskImage != null)
                        {
                            _maskImage.Dispose();
                            _maskImage = null;
                        }

                        bool bHasMask = reader.ReadBoolean();
                        if (bHasMask)
                            this._maskImage = Image.FromStream(reader.BaseStream);
                        ///this._properties[BORDER_CLEARED] = reader.ReadBoolean();                        
                    }
                }
                catch (System.Exception exp)
                {
                    Trace.WriteLine(exp);
                }
            }
            return bResult;
        }

        public bool QuickSave(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            bool bResult = false;

            try
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    bResult = _image.QuickSave(writer);
                    if (bResult)
                    {
                        Char[] chars = this._filePath.ToCharArray();
                        writer.Write(chars.Length);
                        if (chars.Length > 0) 
                            writer.Write(chars);

                        chars = this._description.ToCharArray();
                        writer.Write(chars.Length);
                        if (chars.Length > 0) 
                            writer.Write(chars);

                        bool hasMask = _maskImage != null;
                        writer.Write(hasMask);
                        if (_maskImage != null)
                            _maskImage.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                       
                        // initialize new circle finder
                        _circleFinder = new CircleFinder();
                    }
                }
            }
            catch (System.Exception exp)
            {
                Trace.WriteLine(exp);
            }
            return bResult;
        }

        #endregion

        #region Utility routines

        public CommonImage CropImage(int x1, int y1, int x2, int y2)
        {
            GreyDataImage rasterImage = _image.createGreyDataBimap(x1, y1, x2, y2);
            return new CommonImage(rasterImage);
        }

        public CommonImage CropImage(Rectangle rect)
        {
            return CropImage(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        public CommonImage CreateScaledImage(int width, int height)
        {
            float scale = 1.0f;
            if (this.Width > this.Height)
            {
                scale = width * 1.0f / this.Width;
            }
            else
            {
                scale = height * 1.0f / this.Height;
            }

            return new CommonImage(
                _image.CreateScaledImage(scale));
        }


        private void DisposeOldMask()
        {
            if (this._mask != null)
            {
                try
                {
                    this._mask.Dispose();
                }
                catch (System.Exception exp)
                {
                    Trace.WriteLine(exp);
                }
                finally
                {
                    this._mask = null;
                }
            }
        }


        unsafe public void FillSubImage(
            int[,] subImage, int left, int top)
        {
            if (subImage == null || _image == null)
                return;
            int stride = this._image._width;
            int startLineIndex = left + top * stride;
            int subImageIndex = 0;
            fixed (int* ptrSubImage = subImage)
            {
                for (int i = 0; i < subImage.GetLength(0); i++)
                {
                    int index = startLineIndex;
                    for (int j = 0; j < subImage.GetLength(1); j++, index++, subImageIndex++)
                    {
                        ptrSubImage[subImageIndex] = this._image._aData[index];
                    }
                    startLineIndex += stride;
                }
            }
        }

        public TYPE[] GetPixels(Point first, Point second)
        {
            TYPE[] values = null;
            int x1 = first.X;
            int y1 = first.Y;
            int x2 = second.X;
            int y2 = second.Y;
            int dx = x2 - x1;
            int dy = y2 - y1;
            int dxabs = (int)Math.Abs(dx);
            int dyabs = (int)Math.Abs(dy);
            int px = x1;
            int py = y1;
            int sdx = Math.Sign(dx);
            int sdy = Math.Sign(dy);
            int x = 0;
            int y = 0;

            if (dxabs > dyabs)
            {
                //coords = new PointF[dxabs + 1];
                values = new TYPE[dxabs + 1];

                for (int i = 0; i <= dxabs; i++)
                {
                    y += dyabs;

                    if (y >= dxabs)
                    {
                        y -= dxabs;
                        py += sdy;
                    }

                    //coords[i] = new Point(px, py);
                    values[i] = this.GetPixel(px, py);
                    px += sdx;
                }
            }
            else if (dxabs == dyabs) // had to add in this cludge for slopes of 1 ... wasn't drawing half the line
            {
                //coords = new PointF[dxabs + 1];
                values = new TYPE[dxabs + 1];

                for (int i = 0; i <= dxabs; i++)
                {
                    //coords[i] = new Point(px, py);
                    values[i] = this.GetPixel(px, py);
                    px += sdx;
                    py += sdy;
                }
            }
            else
            {
                //coords = new PointF[dyabs + 1];
                values = new TYPE[dyabs + 1];

                for (int i = 0; i <= dyabs; i++)
                {
                    x += dxabs;

                    if (x >= dyabs)
                    {
                        x -= dyabs;
                        px += sdx;
                    }

                    //coords[i] = new PointF(px, py);
                    values[i] = this.GetPixel(px, py);
                    py += sdy;
                }
            }

            return values;
        }

        public static Point[] GetLinePoints(Point first, Point second)
        {
            Point[] coords = null;

            int x1 = first.X;
            int y1 = first.Y;
            int x2 = second.X;
            int y2 = second.Y;
            int dx = x2 - x1;
            int dy = y2 - y1;
            int dxabs = Math.Abs(dx);
            int dyabs = Math.Abs(dy);
            int px = x1;
            int py = y1;
            int sdx = Math.Sign(dx);
            int sdy = Math.Sign(dy);
            int x = 0;
            int y = 0;

            if (dxabs > dyabs)
            {
                coords = new Point[dxabs + 1];

                for (int i = 0; i <= dxabs; i++)
                {
                    y += dyabs;

                    if (y >= dxabs)
                    {
                        y -= dxabs;
                        py += sdy;
                    }

                    coords[i] = new Point(px, py);
                    px += sdx;
                }
            }
            else if (dxabs == dyabs) // had to add in this cludge for slopes of 1 ... wasn't drawing half the line
            {
                coords = new Point[dxabs + 1];

                for (int i = 0; i <= dxabs; i++)
                {
                    coords[i] = new Point(px, py);
                    px += sdx;
                    py += sdy;
                }
            }
            else
            {
                coords = new Point[dyabs + 1];

                for (int i = 0; i <= dyabs; i++)
                {
                    x += dxabs;

                    if (x >= dyabs)
                    {
                        x -= dyabs;
                        px += sdx;
                    }

                    coords[i] = new Point(px, py);
                    py += sdy;
                }
            }

            return coords;
        }
        #endregion

        #region Object Detection        

        #region helpers        
        public void PreviewDynamicCutOffPercentageThreshold_OnAllImage(
            kHistogram histogram, float fromPercentage, float toPercentage)
        {
            if (histogram == null)
                return;

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
            FindRangeThreshold(ref minThreshold, ref maxThreshold, hist, rankMin, rankMax);

            if (this.RasterImage != null)
            {
                using (StaticThreshold thresholder = new StaticThreshold(this.RasterImage))
                {
                    thresholder.Threshold((int)minThreshold, (int)maxThreshold);
                }
            }
        }

        public void PreviewDynamicCutOffPercentageThreshold_WB(
            kHistogram histogram, int offsetX, int offsetY, 
            PointF center, float radius, float fromPercentage, float toPercentage)
        {
            if (histogram == null)
                return;

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
            FindRangeThreshold(ref minThreshold, ref maxThreshold, hist, rankMin, rankMax);

            if (this.RasterImage != null)
            {
                using (StaticThreshold thresholder = new StaticThreshold(this.RasterImage))
                {
                    thresholder.Threshold(offsetX, offsetY, center.X, center.Y, radius, (int)minThreshold, (int)maxThreshold);
                }
            }
        }

        public void ThresholdByPercentile(
            float fromPercentage, float toPercentage, bool zeroFrom, bool zeroTo)
        {
            kHistogram histogram = null;
            try
            {
                histogram = this.Histogram;

                double[] hist = histogram.Histogram;
                double totalIntensity = 0;
                int nBins = hist.Length;
                for (int i = 0; i < nBins; i++)
                {
                    totalIntensity += hist[i];
                }
                int rankMin = (int)(totalIntensity * fromPercentage * 0.01);
                int rankMax = (int)(totalIntensity * toPercentage * 0.01);
                float minThreshold = 0;
                float maxThreshold = 0;
                FindRangeThreshold(ref minThreshold, ref maxThreshold, hist, rankMin, rankMax);

                int minimum = -1;
                if (fromPercentage > 0)
                    minimum = (int)minThreshold;
                int maximum = -1;
                if (toPercentage < 100)
                    maximum = (int)maxThreshold;

                this.kThreshold(minimum, zeroFrom, maximum, zeroTo);
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
        }

        public void FindRangeThreshold(
            ref float minThreshold, ref float maxThreshold, double[] hist, int rankMin, int rankMax)
        {
            bool bFoundMin = false;
            bool bFoundMax = false;

            if (rankMin <= 0)
            {
                minThreshold = 0;
                bFoundMin = true;
            }

            if (rankMax <= 0)
            {
                maxThreshold = 0;
                bFoundMax = true;
            }

            if (!bFoundMin || !bFoundMax)
            {
                double total = 0;
                int nBins = hist.Length;

                for (int i = 0; i < nBins; i++)
                {
                    total += hist[i];
                    if (!bFoundMin && total >= rankMin)
                    {
                        minThreshold = (float)i;
                        bFoundMin = true;
                    }

                    if (!bFoundMax && total >= rankMax)
                    {
                        maxThreshold = (float)i;
                        return;
                    }
                }
                if (!bFoundMin)
                    minThreshold = (float)nBins - 1;
                if (!bFoundMax)
                    maxThreshold = (float)nBins - 1;
            }
        }
        #endregion helpers
        #endregion

        #region Intensity Correction
        public void IntensityCorrection(
            CommonImage referenceImage, eTypeIntensityCorrection zoneType, int _value, eExceptionManipulation em, float cap_threshold)
        {
            IntensityCorrectionUsingScanLine(
                referenceImage, zoneType, _value, em, cap_threshold);
        }
        public void IntensityCorrectionUsingFloatImage(
            CommonImage referenceImage, eTypeIntensityCorrection zoneType, 
            int _value, eExceptionManipulation em, float cap_threshold)
        {
            float zone_value = 0;
            switch (zoneType)
            {
                case eTypeIntensityCorrection.Median:
                    zone_value = referenceImage.RasterImage.mZone.mMedian;
                    break;
                case eTypeIntensityCorrection.Arverage:
                    zone_value = referenceImage.RasterImage.mZone.mAverage;
                    break;
                case eTypeIntensityCorrection.Max:
                    zone_value = referenceImage.RasterImage.mZone.mMax;
                    break;
                case eTypeIntensityCorrection.Value:
                    zone_value = _value;
                    break;
            }

            double old_min_current_view = this.RasterImage.MinCurrentView;
            double old_max_current_view = this.RasterImage.MaxCurrentView;
            using (FloatImage floatImage = new FloatImage(referenceImage.RasterImage))
            {
                floatImage.CopyHeaderFrom(this.RasterImage);
                floatImage.setCircleAndNotch(this.RasterImage);
                floatImage.constant_division(zone_value, em, referenceImage.RasterImage, cap_threshold, 26, 50);
                floatImage.multiplyKeepPixelOuterBoundary(this.RasterImage, 51, 75);

                using (GreyDataImage image = floatImage.convertGreyDataImageWithoutStretch(76, 100))
                {
                    this.RasterImage.CopyDataFrom(image);
                }
            }
        }
        public void IntensityCorrectionUsingScanLine(
            CommonImage referenceImage, eTypeIntensityCorrection zoneType, 
            int _value, eExceptionManipulation em, float cap_threshold)
        {
            float zone_value = 0;
            switch (zoneType)
            {
                case eTypeIntensityCorrection.Median:
                    zone_value = referenceImage.RasterImage.mZone.mMedian;
                    break;
                case eTypeIntensityCorrection.Arverage:
                    zone_value = referenceImage.RasterImage.mZone.mAverage;
                    break;
                case eTypeIntensityCorrection.Max:
                    zone_value = referenceImage.RasterImage.mZone.mMax;
                    break;
                case eTypeIntensityCorrection.Value:
                    zone_value = _value;
                    break;
            }

            double old_min_current_view = this.RasterImage.MinCurrentView;
            double old_max_current_view = this.RasterImage.MaxCurrentView;

            this._image.IntensityCorrection(zone_value, referenceImage._image, em, cap_threshold);
        }
        #endregion Intensity Correction

#region GPU_SUPPORTED

#if GPU_SUPPORTED
        public DeviceBuffer<ushort> cbImage = null;

        public bool HasDeviceBuffer 
        {
            get { return cbImage != null; }
        }

        public void CreateDeviceBuffer()
        {
            if (DeviceProgram.GPUSupported)
            {
                DisposeDeviceBuffer();
                unsafe
                {
                    cbImage = DeviceBuffer<ushort>.CreateHostBufferReadOnly(_image._lenght, (IntPtr)_image._aData);
                    //cbImage = DeviceBuffer<ushort>.CreateBufferReadOnly(_image._lenght, (IntPtr)_image._aData);
                }
            }
        }

        public void DisposeDeviceBuffer()
        {
            if (cbImage != null)
            {
                cbImage.Dispose();
                cbImage = null;
            }
        }

        public void ReadDataFromDeviceBuffer()
        {
            if(cbImage == null) return;
            if( _image ==null) return;
            unsafe
            {
                cbImage.Read((IntPtr)_image._aData, null);
            }

            //mis update image information min,max....
            //_image.RefreshDataLimit(false);
            //_image.SetImageData()
        }

#region Source Copy
        const string srcCopy = @"
#pragma OPENCL EXTENSION cl_khr_fp64 : enable
__kernel void Copy(
__global __read_only ushort* idata
,__global __write_only ushort* odata
,__global __read_only int* psize
)
{
   int x = get_global_id(0);
   int y = get_global_id(1);
   int w = psize[0];//get_global_size(0);
   int i = y*w+x;
   odata[i] = idata[i];
}

__kernel void ResetData(__global ushort* idata
,__constant int *     psize//w,h,offsetx,offsety,value
)
{
   int x0 = get_global_id(0);
   int y0 = get_global_id(1);
   int w = psize[0];
   
   int x = x0 + psize[2];
   int y = y0 + psize[3];

   int val =  psize[4];
   idata[y*w+x] = (ushort)val;
}
";
#endregion

        public static ComputeKernel kernerlCopy;
        public static ComputeKernel kernerlResetData;

        public static void PrepareDataKernel()
        {
            if (kernerlCopy == null)
            {
                DeviceProgram.Compile(srcCopy);
                kernerlCopy = DeviceProgram.CreateKernel("Copy");
                kernerlResetData = DeviceProgram.CreateKernel("ResetData");
            }
        }

        public DeviceBuffer<ushort> CloneDeviceMemory()
        {
            if (cbImage == null) return null;

            //Stopwatch sw = new Stopwatch(); sw.Start();
            //StringBuilder sb = new StringBuilder();
            PrepareDataKernel();
           

            //sw.Stop(); sb.AppendLine("CloneDeviceMemory :" + sw.ElapsedTicks); sw.Reset(); sw.Start();

            DeviceBuffer<ushort> cbRet = DeviceBuffer<ushort>.CreateBufferReadWrite(Length);
            DeviceBuffer<int> cbSize = DeviceBuffer<int>.CreateHostBufferReadOnly(new int[] { Width, Height });

            DeviceProgram.ExecuteKernel(kernerlCopy,
                new ComputeMemory[]{ 
                cbImage
                ,cbRet
                ,cbSize
            }, new int[]{Width,Height}
                );

            DeviceProgram.Finish();

            cbSize.Dispose();

            //sw.Stop(); sb.AppendLine("kernerlCopy :" + sw.ElapsedTicks);
            //Debug.WriteLine(sb.ToString());

            return cbRet;
        }

        public static void ResetDeviceMemory(DeviceBuffer<ushort> cbData,int w,int h,int scanw,int scanh,int offsetx,int offsety,ushort val)
        {
            if (cbData == null) return;

            PrepareDataKernel();

            DeviceBuffer<int> cbSize = DeviceBuffer<int>.CreateHostBufferReadOnly(new int[]{w,h,offsetx,offsety,(int)val});

            DeviceProgram.ExecuteKernel(kernerlResetData,
                new ComputeMemory[]{ 
                cbData
                ,cbSize
            }, new int[] { scanw, scanh}
                );
            DeviceProgram.Finish();

            cbSize.Dispose();
        }

        public static void ResetDeviceKernelBound(DeviceBuffer<ushort> cbData,int w,int h,int ksize,ushort val)
        {
            if (cbData== null) return;
            int kh = ksize>>1;
            ResetDeviceMemory(cbData, w, h, kh, h - (kh << 1), 0, 0, val);
            ResetDeviceMemory(cbData, w, h, kh, h - (kh << 1), w - kh + 1, 0, val);

            ResetDeviceMemory(cbData, w, h, w - (kh << 1), kh, 0, 0, val);
            ResetDeviceMemory(cbData, w, h, w - (kh << 1), kh, 0, h - kh - 1, val);
        }

        public void ResetDeviceKernelBound(int ksize,ushort val)
        {
            ResetDeviceKernelBound(cbImage, Width, Height, ksize, val);
        }

#region Operation Source 
        const string srcMonadicOperation = @"#define CHECKBOUND(value, MIN, MAX) max(min(value,MAX),MIN);
#pragma OPENCL EXTENSION cl_khr_fp64 : enable 

__kernel void MonadicOperationAdd(
__global ushort* idata
,__global __read_only float* fval
,__global __read_only int* psize//w,h,min,max
)
{
    int x = get_global_id(0);
    int y = get_global_id(1);
    int w = psize[0];//get_global_size(0);
    int i = y*w+x;

    double val = idata[i] + fval[0];
    idata[i] = CHECKBOUND(val,(double)psize[2],(double)psize[3]);
}

__kernel void MonadicOperationSub(
__global ushort* idata
,__global __read_only float* fval
,__global __read_only int* psize//w,h,min,max
)
{
    int x = get_global_id(0);
    int y = get_global_id(1);
    int w = psize[0];//get_global_size(0);
    int i = y*w+x;

    double val = idata[i] - fval[0];
    idata[i] = CHECKBOUND(val,(double)psize[2],(double)psize[3]);
}

__kernel void MonadicOperationMul(
__global ushort* idata
,__global __read_only float* fval
,__global __read_only int* psize//w,h,min,max
)
{
    int x = get_global_id(0);
    int y = get_global_id(1);
    int w = psize[0];//get_global_size(0);
    int i = y*w+x;

    double val = idata[i] * fval[0];
    idata[i] = CHECKBOUND(val,(double)psize[2],(double)psize[3]);
}

__kernel void MonadicOperationDiv(
__global ushort* idata
,__global __read_only float* fval
,__global __read_only int* psize//w,h,min,max
)
{
    int x = get_global_id(0);
    int y = get_global_id(1);
    int w = psize[0];//get_global_size(0);
    int i = y*w+x;

    double val = idata[i] / fval[0];
    idata[i] = CHECKBOUND(val,(double)psize[2],(double)psize[3]);
}

__kernel void DyadicOperationAdd(
__global ushort* idata
,__global __read_only ushort* ival
,__global __read_only int* psize//w,h,min,max
)
{
    int x = get_global_id(0);
    int y = get_global_id(1);
    int w = psize[0];//get_global_size(0);
    int i = y*w+x;
    
    double val = idata[i] + ival[i];
    idata[i] = CHECKBOUND(val,(double)psize[2],(double)psize[3]);
}

__kernel void DyadicOperationSub(
__global ushort* idata
,__global __read_only ushort* ival
,__global __read_only int* psize//w,h,min,max
)
{
    int x = get_global_id(0);
    int y = get_global_id(1);
    int w = psize[0];//get_global_size(0);
    int i = y*w+x;
    
    double val = idata[i] - ival[i];
    idata[i] = CHECKBOUND(val,(double)psize[2],(double)psize[3]);
}

__kernel void DyadicOperationMul(
__global ushort* idata
,__global __read_only ushort* ival
,__global __read_only int* psize//w,h,min,max
)
{
    int x = get_global_id(0);
    int y = get_global_id(1);
    int w = psize[0];//get_global_size(0);
    int i = y*w+x;
    
    double val = idata[i] * ival[i];
    idata[i] = CHECKBOUND(val,(double)psize[2],(double)psize[3]);
}

__kernel void DyadicOperationDiv(
__global ushort* idata
,__global __read_only ushort* ival
,__global __read_only int* psize//w,h,min,max
)
{
    int x = get_global_id(0);
    int y = get_global_id(1);
    int w = psize[0];//get_global_size(0);
    int i = y*w+x;
    
    double val = idata[i] / ival[i];
    idata[i] = CHECKBOUND(val,(double)psize[2],(double)psize[3]);
}


";
#endregion

        public static ComputeKernel kernelMonadicOperationAdd;
        public static ComputeKernel kernelMonadicOperationSub;
        public static ComputeKernel kernelMonadicOperationMul;
        public static ComputeKernel kernelMonadicOperationDiv;
        public static ComputeKernel kernelDyadicOperationAdd;
        public static ComputeKernel kernelDyadicOperationSub;
        public static ComputeKernel kernelDyadicOperationMul;
        public static ComputeKernel kernelDyadicOperationDiv;

        public static void PrepareOperationKernel()
        {
            if(kernelMonadicOperationAdd==null)
            {
                DeviceProgram.Compile(srcMonadicOperation);
                kernelMonadicOperationAdd = DeviceProgram.CreateKernel("MonadicOperationAdd");
                kernelMonadicOperationSub = DeviceProgram.CreateKernel("MonadicOperationSub");
                kernelMonadicOperationMul = DeviceProgram.CreateKernel("MonadicOperationMul");
                kernelMonadicOperationDiv = DeviceProgram.CreateKernel("MonadicOperationDiv");

                kernelDyadicOperationAdd = DeviceProgram.CreateKernel("DyadicOperationAdd");
                kernelDyadicOperationSub = DeviceProgram.CreateKernel("DyadicOperationSub");
                kernelDyadicOperationMul = DeviceProgram.CreateKernel("DyadicOperationMul");
                kernelDyadicOperationDiv = DeviceProgram.CreateKernel("DyadicOperationDiv");
            }
        }

        public void kMonadicOperationGPU(string type, float value)
        {
            if (!DeviceProgram.GPUSupported)
            {
                kMonadicOperation(type, value, false);
                return;
            }

            if (cbImage == null) return;
            PrepareOperationKernel();

            float []fval = new float[]{value};
            DeviceBuffer<float> cbVal = DeviceBuffer<float>.CreateBufferReadOnly(fval);

            int[] isize = new int[]{Width,Height,(int)MinGreyValue,(int)MaxGreyValue};
            DeviceBuffer<int> cbSize = DeviceBuffer<int>.CreateBufferReadOnly(isize);

            ComputeKernel pkernel = null;
            type = type.ToUpper();
            switch (type)
            {
                case "ADD":
                    pkernel = kernelMonadicOperationAdd;
                    break;

                case "SUB":
                    pkernel = kernelMonadicOperationSub;
                    break;

                case "MUL":
                    pkernel = kernelMonadicOperationMul;
                    break;

                case "DIV":
                    pkernel = kernelMonadicOperationDiv;
                    break;

                default:
                    throw new Exception("Unknown operator ");
            }

            if (pkernel!=null)
            {
                DeviceProgram.ExecuteKernel(pkernel,
                    new ComputeMemory[]{ 
                cbImage 
                ,cbVal
                ,cbSize
                }, new int[] { Width, Height }
                    );
                DeviceProgram.Finish();
            }

            cbVal.Dispose();
            cbSize.Dispose();
        }

        public static void kDyadicOperationGPU(string type, DeviceBuffer<ushort> cbImage, DeviceBuffer<ushort> refImage, int Width, int Height,double mingray,double maxgray)
        {
            if (cbImage == null) return;
            PrepareOperationKernel();

            int[] isize = new int[] { Width, Height, (int)mingray, (int)maxgray};
            DeviceBuffer<int> cbSize = DeviceBuffer<int>.CreateBufferReadOnly(isize);

            ComputeKernel pkernel = null;
            type = type.ToUpper();
            switch (type)
            {
                case "ADD":
                    pkernel = kernelDyadicOperationAdd;
                    break;

                case "SUB":
                    pkernel = kernelDyadicOperationSub;
                    break;

                case "MUL":
                    pkernel = kernelDyadicOperationMul;
                    break;

                case "DIV":
                    pkernel = kernelDyadicOperationDiv;
                    break;

                default:
                    throw new Exception("Unknown operator ");
            }

            if (pkernel != null)
            {
                DeviceProgram.ExecuteKernel(pkernel,
                    new ComputeMemory[]{ 
                cbImage 
                ,refImage
                ,cbSize
                }, new int[] { Width, Height }
                    );
                DeviceProgram.Finish();
            }

            cbSize.Dispose();
        }

        public void kDyadicOperationGPU(string type,CommonImage refImage)
        {
            if (!DeviceProgram.GPUSupported)
            {
                kDyadicOperation(type, refImage, false);
                return;
            }

            if (cbImage == null) return;

            bool bCreateBuf = !refImage.HasDeviceBuffer;
            if (bCreateBuf)
                refImage.CreateDeviceBuffer();

            kDyadicOperationGPU(type, cbImage, refImage.cbImage,Width,Height,MinGreyValue,MaxGreyValue);

            if (bCreateBuf)
                refImage.DisposeDeviceBuffer();
        }

        #region Threshold Source
        const string srcThresholdIntensity = @"
#pragma OPENCL EXTENSION cl_khr_fp64 : enable 

__kernel void ThresholdIntensity(
__global ushort* idata
,__global __read_only bool* param//zeromin,zeromax
,__global __read_only int* psize//w,h,min,max,mingray,maxgray
)
{
    int x = get_global_id(0);
    int y = get_global_id(1);
    int w = psize[0];//get_global_size(0);
    int i = y*w+x;
    
    ushort val = idata[i];
    int valmin = psize[2];
    int valmax = psize[3];
    int mingray = psize[4];
    //int maxgray = psize[5];

    //copy logic from old version
    ushort newval = val;
    if(valmax!=-1 && newval > valmax)
        newval = param[1]==true?mingray :valmax;

    if(valmin!=-1 && newval < valmin)
        newval = param[0]==true?mingray :valmin;

    if(newval!=val)
        idata[i] = newval;

/*  //this logic correct
    if(valmin!=-1 && val < valmin)
        idata[i] = param[0]==true?mingray :valmin;

    if(valmax!=-1 && val > valmax)
        idata[i] = param[1]==true?mingray :valmax;
*/
}


__kernel void ThresholdHistogram(
__global __read_only ushort* idata
,__global __write_only ushort* odata //the matrix : height x 256
,__global __read_only int* psize//w,h
)
{
    int y = get_global_id(0);
    int w = psize[0] ;//get_global_size(0);

    int index0 = y*w;
    int index1 = y*256;
    for(int x=0;x<w;x++)
    {
    odata[index1 + idata[index0++]]++;
    }
}

__kernel void ThresholdHistogramSum(
__global __read_only ushort* idata//the matrix : height x 256
,__global __write_only double* odata //vector 256 
,__global __read_only int* psize//w,h
)
{
    int i = get_global_id(0);
    int h = psize[1] ;//get_global_size(0);
    
    for(int y=0;y<h;y++)
    {
    odata[i]+= idata[y*256+i];
    }
}

__kernel void EqualizeHistogram(
__global ushort* idata
,__global __read_only double* hist//256 vector
,__global __read_only int* psize//w,h,his size
)
{
    int x = get_global_id(0);
    int y = get_global_id(1);
    int w = psize[0];
    int i = y*w+x;
    
    idata[i] =  hist[idata[i]];
}

";
        #endregion

        public static ComputeKernel kernelThresholdIntensity;
        public static ComputeKernel kernelThresholdHistogram;
        public static ComputeKernel kernelThresholdHistogramSum;
        public static ComputeKernel kernelEqualizeHistogram;
        

        public static void PrepareThresholdKernel()
        {
            if (kernelThresholdIntensity == null)
            {
                DeviceProgram.Compile(srcThresholdIntensity);
                kernelThresholdIntensity = DeviceProgram.CreateKernel("ThresholdIntensity");
                kernelThresholdHistogram = DeviceProgram.CreateKernel("ThresholdHistogram");
                kernelThresholdHistogramSum = DeviceProgram.CreateKernel("ThresholdHistogramSum");
                kernelEqualizeHistogram = DeviceProgram.CreateKernel("EqualizeHistogram");
            }
        }

        public void kThresholdGPU(double MinThreshold, bool bZeroMin, double MaxThreshold, bool bZeroMax)
        {
            if (!DeviceProgram.GPUSupported)
            {
                kThreshold(MinThreshold,bZeroMin,MaxThreshold,bZeroMax);
                return;
            }

            if (cbImage == null) return;

            PrepareThresholdKernel();

            bool[] fval = new bool[] { bZeroMin,bZeroMax};
            DeviceBuffer<bool> cbVal = DeviceBuffer<bool>.CreateBufferReadOnly(fval);

            int[] isize = new int[] { Width, Height, (int)MinThreshold,(int)MaxThreshold,(int)MinGreyValue,(int)MaxGreyValue};
            DeviceBuffer<int> cbSize = DeviceBuffer<int>.CreateBufferReadOnly(isize);


            DeviceProgram.ExecuteKernel(kernelThresholdIntensity,
                    new ComputeMemory[]{ 
                cbImage 
                ,cbVal
                ,cbSize
                }, new int[] { Width, Height }
                    );
                DeviceProgram.Finish();
            

            cbVal.Dispose();
            cbSize.Dispose();
        }


        public void ThresholdByPercentileGPU(
      float fromPercentage, float toPercentage, bool zeroFrom, bool zeroTo)
        {
            if (!DeviceProgram.GPUSupported)
            {
                ThresholdByPercentileGPU(fromPercentage, toPercentage, zeroFrom, zeroTo);
                return;
            }


            try
            {
                if (cbImage == null) return;

                //find histogram by GPU
                PrepareThresholdKernel();


                int[] isize = new int[] { Width, Height};
                DeviceBuffer<int> cbSize = DeviceBuffer<int>.CreateBufferReadOnly(isize);

                DeviceBuffer<ushort> cbTemp = DeviceBuffer<ushort>.CreateBufferReadWrite(Height * 256);
                DeviceBuffer<double> cbHistogram = DeviceBuffer<double>.CreateBufferReadWrite(256);

                DeviceProgram.ExecuteKernel(kernelThresholdHistogram,
                        new ComputeMemory[]{ 
                cbImage 
                ,cbTemp
                ,cbSize
                }, Height );
                DeviceProgram.Finish();


                DeviceProgram.ExecuteKernel(kernelThresholdHistogramSum,
                        new ComputeMemory[]{ 
                cbTemp
                ,cbHistogram
                ,cbSize
                }, 256);
                DeviceProgram.Finish();

                double[] hist = cbHistogram.Read(null);

                cbTemp.Dispose();
                cbHistogram.Dispose();
                cbSize.Dispose();


                double totalIntensity = 0;
                int nBins = hist.Length;
                for (int i = 0; i < nBins; i++)
                {
                    totalIntensity += hist[i];
                }
                int rankMin = (int)(totalIntensity * fromPercentage * 0.01);
                int rankMax = (int)(totalIntensity * toPercentage * 0.01);
                float minThreshold = 0;
                float maxThreshold = 0;
                FindRangeThreshold(ref minThreshold, ref maxThreshold, hist, rankMin, rankMax);

                int minimum = -1;
                if (fromPercentage > 0)
                    minimum = (int)minThreshold;
                int maximum = -1;
                if (toPercentage < 100)
                    maximum = (int)maxThreshold;

                //filter by GPU
                this.kThresholdGPU(minimum, zeroFrom, maximum, zeroTo);
            }
            catch (System.Exception exp)
            {
                Trace.WriteLine(exp);
                throw exp;
            }
            finally
            {
            }
        }
#region StretchColor Source
        const string srcStretchColor = @"#define CHECKBOUND(value, MIN, MAX) max(min(value,MAX),MIN);
#pragma OPENCL EXTENSION cl_khr_fp64 : enable 

__kernel void StretchColor(
__global ushort* idata
,__global __read_only double* param//low,scale
,__global __read_only int* psize//w,h,mingray,maxgray
)
{
    int x = get_global_id(0);
    int y = get_global_id(1);
    int w = psize[0] ;//get_global_size(0);

    int i = y*w+x;
    
    int val = param[1]*(idata[i] - param[0]);
    idata[i] = CHECKBOUND(val,psize[2],psize[3]);
}

";
#endregion

        public static ComputeKernel kernelStretchColor;

        public static void PrepareStretchColorKernel()
        {
            if (kernelStretchColor == null)
            {
                DeviceProgram.Compile(srcStretchColor);
                kernelStretchColor = DeviceProgram.CreateKernel("StretchColor");
            }
        }

        // Stretch Color
        public bool StretchColorGPU(double low, double high)
        {
            if (!DeviceProgram.GPUSupported)
                return StretchColorGPU(low, high);

            if (low < MinGreyValue || high > MaxGreyValue || low > high)
                return false;

            if (cbImage == null) return false;

            //GPU
            PrepareStretchColorKernel();

            double range = high - low, scale = 0.0f;
            scale = range > 0 ? (MaxGreyValue - MinGreyValue) / range : 1;

            double[] param = new double[] {low,scale};
            DeviceBuffer<double> cbParam = DeviceBuffer<double>.CreateHostBufferReadOnly(param);

            int[] isize = new int[] { Width, Height, (int)MinGreyValue, (int)MaxGreyValue};
            DeviceBuffer<int> cbSize = DeviceBuffer<int>.CreateBufferReadOnly(isize);

            DeviceProgram.ExecuteKernel(kernelStretchColor,
                    new ComputeMemory[]{ 
                cbImage 
                ,cbParam
                ,cbSize
                }, new int[] { Width, Height }
                    );
            DeviceProgram.Finish();


            cbParam.Dispose();
            cbSize.Dispose();

            return true;
        }

        #region Inversion Source
        const string srcInversion = @"
#pragma OPENCL EXTENSION cl_khr_fp64 : enable 

__kernel void Inversion(
__global ushort* idata
,__global __read_only int* psize//w,h,mingray,maxgray
)
{
    int x = get_global_id(0);
    int y = get_global_id(1);
    int w = psize[0] ;//get_global_size(0);
    int i = y*w+x;
    
    idata[i] = psize[3] - idata[i];
}

";
        #endregion

        public static ComputeKernel kernelInversion;
        public void InversionGPU()
        {
            if (!DeviceProgram.GPUSupported)
            {
                Inversion();
                return;
            }

            if (cbImage == null) return;

            //GPU
            if(kernelInversion==null)
            {
                DeviceProgram.Compile(srcInversion);
                kernelInversion = DeviceProgram.CreateKernel("Inversion");
            }

            int[] isize = new int[] { Width, Height, (int)MinGreyValue, (int)MaxGreyValue };
            DeviceBuffer<int> cbSize = DeviceBuffer<int>.CreateBufferReadOnly(isize);

            DeviceProgram.ExecuteKernel(kernelInversion,
                    new ComputeMemory[]{ 
                cbImage 
                ,cbSize
                }, new int[] { Width, Height }
                    );
            DeviceProgram.Finish();


            cbSize.Dispose();

        }

        public void kHistogramEqualizationGPU()
        {
            if (!DeviceProgram.GPUSupported)
            {
                kHistogramEqualization();
                return;
            }

            if( cbImage==null) return;

            PrepareThresholdKernel();


            //Get Histograms
            int[] isize = new int[] { Width, Height };
            DeviceBuffer<int> cbSize = DeviceBuffer<int>.CreateBufferReadOnly(isize);

            DeviceBuffer<ushort> cbTemp = DeviceBuffer<ushort>.CreateBufferReadWrite(Height * 256);
            DeviceBuffer<double> cbHistogram = DeviceBuffer<double>.CreateBufferReadWrite(256);

            DeviceProgram.ExecuteKernel(kernelThresholdHistogram,
                    new ComputeMemory[]{ 
                cbImage 
                ,cbTemp
                ,cbSize
                }, Height);
            DeviceProgram.Finish();


            DeviceProgram.ExecuteKernel(kernelThresholdHistogramSum,
                    new ComputeMemory[]{ 
                cbTemp
                ,cbHistogram
                ,cbSize
                }, 256);
            DeviceProgram.Finish();

            double[] hist = cbHistogram.Read(null);
            double[] LUT = hist.Clone() as double[];

            cbTemp.Dispose();

            //Process data
            int m_bins = 256;
            ushort intensity = (ushort)MaxGreyValue;
            ushort MAXGRAYVALUE = (ushort)MaxGreyValue;
            double strength = MaxGreyValue;
            
            /* compute lookup table by accumulating */
            for (int i = 1; i < m_bins; i++)
                LUT[i] = LUT[i - 1] + hist[i];

            /* normalizes lookup table */
            for (int i = 0; i < m_bins; i++)
                LUT[i] = (LUT[i] * intensity) / Length;

            /* Adjust the LUT based on the selected strength. This is an alpha
	            mix of the calculated LUT and a linear LUT with gain 1. */
            for (int i = 0; i < m_bins; i++)
                LUT[i] = (strength * LUT[i]) / MAXGRAYVALUE +
                         ((MAXGRAYVALUE - strength) * i) / MAXGRAYVALUE;

            cbHistogram.Write(LUT, null);

            DeviceProgram.ExecuteKernel(kernelEqualizeHistogram,
                    new ComputeMemory[]{ 
                cbImage 
                ,cbHistogram
                ,cbSize
                }, isize);
            DeviceProgram.Finish();

            cbHistogram.Dispose();
            cbSize.Dispose();
        }

        #region Rotate Source
        const string srcRotate= @"
#pragma OPENCL EXTENSION cl_khr_fp64 : enable 
__kernel void Rotate(
__global __read_only ushort* idata
,__global __write_only ushort* odata
,__global __read_only double* param//sina,cosa,vectorx,vectory
,__global __read_only int* psize//ow,oh,w,h
)
{
    int x = get_global_id(0);
    int y = get_global_id(1);
    int ow = psize[0];
    int oh = psize[1];
    int w = psize[2];
    int h = psize[3];
    
    int i = y*w+x;
    
    double sin_angle = param[0];
    double cos_angle = param[1];
    
    double xx =  (x*cos_angle + y*sin_angle + param[2]);
    double yy =  (y*cos_angle - x*sin_angle + param[3]);
    
    if (xx >= 0 && xx < ow  && yy >= 0 && yy < oh)
    {
        if (xx<0.0) xx = 0.0;
        if (xx>=ow-1.0) xx = ow-1.001;
      
	    if (yy<0.0) yy = 0.0;
	    if (yy>=oh-1.0) yy = oh-1.001;

	    int xbase = (int)xx;
	    int ybase = (int)yy;

	    double xFraction = xx - xbase;
	    double yFraction = yy - ybase;

	    int offset = ybase * ow + xbase;

          double lowerLeft = idata[offset];
	    double lowerRight = idata[offset + 1];
	    double upperRight = idata[offset + ow + 1];
	    double upperLeft = idata[offset + ow];

	    double upperAverage = upperLeft + xFraction * (upperRight - upperLeft);
	    double lowerAverage = lowerLeft + xFraction * (lowerRight - lowerLeft);

	    odata[i] = (ushort)(lowerAverage + yFraction * (upperAverage - lowerAverage));
    }
    else
        odata[i]=0;
}


__kernel void VerSymmetric(
__global __read_only ushort* idata
,__global __write_only ushort* odata
,__global __read_only int* psize//w,h
)
{
    int x = get_global_id(0);
    int y = get_global_id(1);
    int w = psize[0];
    //int h = psize[1];
    
    int i = y*w+x;
    
    odata[i] = idata[i - x + w - x  - 1];
}


__kernel void HozSymmetric(
__global __read_only ushort* idata
,__global __write_only ushort* odata
,__global __read_only int* psize//w,h
)
{
    int x = get_global_id(0);
    int y = get_global_id(1);
    int w = psize[0];
    int h = psize[1];

    odata[y*w+x] = idata[(h-y-1)*w+x];
}


";
        #endregion

        public static ComputeKernel kernelRotate;
        public static ComputeKernel kernelVerSymmetric;
        public static ComputeKernel kernelHozSymmetric;

        public static void PrepareRotateKernel()
        {
            if (kernelRotate == null)
            {
                DeviceProgram.Compile(srcRotate);
                kernelRotate = DeviceProgram.CreateKernel("Rotate");
                kernelVerSymmetric = DeviceProgram.CreateKernel("VerSymmetric");
                kernelHozSymmetric = DeviceProgram.CreateKernel("HozSymmetric");
            }
        }

        unsafe public void RotateGPU(double angle)
        {
            if(!DeviceProgram.GPUSupported)
            {
                Rotate(angle);
                return;
            }

            if(cbImage==null) return;

            int maxsize = Math.Max(Width, Height);

            GreyDataImage result = new GreyDataImage(maxsize, maxsize);
            result.SetImage();
            _image.CopyHeaderInformation(result);

            double a = Math.PI*(double)angle/(double)180;
		    a = -a;
		    double cos_angle = Math.Cos(a);
		    double sin_angle = Math.Sin(a);

            double vx, vy;

                #region getAdvanceVector
                    double[] x = new double[4];
                    double[] y = new double[4];
	                x[0] = 0;
	                y[0] = 0;

	                double xx = Width - 1;
	                double yy = 0;
	                x[1] = xx*cos_angle +  yy *sin_angle;
	                y[1] = yy*cos_angle - xx*sin_angle;

	                xx = Width - 1;
	                yy = Height - 1;
	                x[2] = xx*cos_angle +  yy *sin_angle;
	                y[2] = yy*cos_angle - xx*sin_angle;

	                xx = 0;
	                yy = Height - 1;
	                x[3] = xx*cos_angle +  yy *sin_angle;
	                y[3] = yy*cos_angle - xx*sin_angle;

                    double xmin = x[0];
                    double ymin = y[0];
                    double xmax = x[0];
                    double ymax = y[0];

	                int n = 4;
	                for(int i=0;i < n;i ++)	
	                {
		                if(xmin > x[i] ){
			                xmin = x[i];
		                }
		                if(ymin > y[i])
		                {
			                ymin = y[i];
		                }

		                if(xmax < x[i] ){
			                xmax = x[i];
		                }
		                if(ymax < y[i])
		                {
			                ymax = y[i];
		                }

	                }
	                float xOldCenter = (Width - 1)/2;
	                float yOldCenter = (Height - 1)/2;

	                vx= xOldCenter  -(xmin + xmax)/2;
	                vy = yOldCenter - (ymin + ymax)/2;
                #endregion

            //GPU
            PrepareRotateKernel();

            //create new device memory
            DeviceBuffer<ushort> cbNewBuff = DeviceBuffer<ushort>.CreateBufferWriteOnly(maxsize*maxsize);

            double[] param = new double[] { sin_angle,cos_angle,vx,vy};
            DeviceBuffer<double> cbParam = DeviceBuffer<double>.CreateBufferReadOnly(param);

            int[] isize = new int[] { Width, Height, maxsize,maxsize  };
            DeviceBuffer<int> cbSize = DeviceBuffer<int>.CreateBufferReadOnly(isize);

            DeviceProgram.ExecuteKernel(kernelRotate,
                    new ComputeMemory[]{ 
                cbImage
                ,cbNewBuff
                ,cbParam
                ,cbSize
                }, new int[] {maxsize, maxsize}
                    );
            DeviceProgram.Finish();

            //get data
            cbNewBuff.Read((IntPtr)result._aData, null);

            cbImage.Dispose();
            cbImage = cbNewBuff;

            _image.Dispose();
            _image = result;

            cbParam.Dispose();
            cbSize.Dispose();
        }

        public void FlipHozGPU()
        {
            if (!DeviceProgram.GPUSupported)
            {
                FlipHoz();
                return;
            }

            if (cbImage == null) return;

            //GPU
            PrepareRotateKernel();

            //create new device memory
            DeviceBuffer<ushort> cbNewBuff = DeviceBuffer<ushort>.CreateBufferWriteOnly(Length);

            int[] isize = new int[] { Width, Height};
            DeviceBuffer<int> cbSize = DeviceBuffer<int>.CreateBufferReadOnly(isize);

            DeviceProgram.ExecuteKernel(kernelVerSymmetric,
                    new ComputeMemory[]{ 
                cbImage
                ,cbNewBuff
                ,cbSize
                }, isize
                    );
            DeviceProgram.Finish();

            //get data
            cbImage.Dispose();
            cbSize.Dispose();

            cbImage = cbNewBuff;
        }

        public void FlipVerGPU()
        {
            if (!DeviceProgram.GPUSupported)
            {
                FlipVer();
                return;
            }

            if (cbImage == null) return;

            //GPU
            PrepareRotateKernel();

            //create new device memory
            DeviceBuffer<ushort> cbNewBuff = DeviceBuffer<ushort>.CreateBufferWriteOnly(Length);

            int[] isize = new int[] { Width, Height };
            DeviceBuffer<int> cbSize = DeviceBuffer<int>.CreateBufferReadOnly(isize);

            DeviceProgram.ExecuteKernel(kernelHozSymmetric,
                    new ComputeMemory[]{ 
                cbImage
                ,cbNewBuff
                ,cbSize
                }, isize
                    );
            DeviceProgram.Finish();

            //get data
            cbImage.Dispose();
            cbSize.Dispose();

            cbImage = cbNewBuff;
        }

        //public unsafe void ResizeGPU(int newWidth, int newHeight, int samplingType)
        //{
        //    if (!DeviceProgram.GPUSupported)
        //    {
        //        Resize(newWidth,newHeight,samplingType);
        //        return;
        //    }

        //    GreyDataImage result = null;

        //    try
        //    {
        //        result = new GreyDataImage(newWidth, newHeight);
        //        _image.CopyHeaderInformation(result);

        //        CVWrapper.Resize(this._image, result, (ResizeInterpolationMethod)samplingType);

        //        // copy data from result image
        //        this._image.SetImageData(result.Width, result.Height, result.Buffer);
        //        result.CopyHeaderInformation(_image);

        //        result._aData = null;
        //        result._width = 0;
        //        result._height = 0;
        //    }
        //    catch
        //    {
        //        if (result != null)
        //            result.Dispose();
        //        result = null;

        //        throw;
        //    }
        //    finally
        //    {
        //    }
        //}


        #region Convolution  Source
        const string srcConvolution = @"#define CHECKBOUND(value, MIN, MAX) max(min(value,MAX),MIN);
                                        //#define BLOCK_SIZE 16
#pragma OPENCL EXTENSION cl_khr_fp64 : enable 

__kernel void BasicConvolution(__global __read_only ushort* idata,
                            __constant float *   kernelValues,
                            __constant int *     psize,//w,h,mingrey,maxgrey,ksize
                            __global __write_only ushort* odata)
{

   int wk = psize[4];
   int w = psize[0] ;//get_global_size(0);
   int x = get_global_id(0);
   int y = get_global_id(1);
   
   double sum = 0;
   
   int index = y*w+x;
   int indexk = 0;
   for (int i = 0; i < wk; i++,index+=w)
   {
       for (int j = 0; j < wk; j++)
       {
            sum+= idata[ index + j ] * kernelValues[indexk++];
       }
   }

   x = x + (wk>>1); 
   y = y + (wk>>1);
   odata[y*w+x] =  CHECKBOUND( (int)sum , psize[2] ,psize[3]);
}

/*
__kernel void BasicConvolutionLocal(__global __read_only ushort* idata,
                            __constant float *   kernelValues,
                            __constant int *     psize,//w,h,mingrey,maxgrey,ksize
                            __global __write_only ushort* odata)
{

   int wk = psize[4];
   int w = psize[0] ;

    __local ushort P[BLOCK_SIZE+20][BLOCK_SIZE+20];

   //Identification of this workgroup
   int gx = get_group_id(0);
   int gy = get_group_id(1);

   //Identification of work-item
   int idX = get_local_id(0);
   int idY = get_local_id(1); 

   int x = gx*BLOCK_SIZE + idX; // == get_global_id(0);
   int y = gy*BLOCK_SIZE + idY; // == get_global_id(1);

    if( x >= psize[0] 
        ||  y >= psize[1]) return;

    //Reads pixels
    P[idX][idY] = idata[y*w+x];

   //Needs to read extra elements for the filter in the borders
   if (idX < w)
        P[idX + BLOCK_SIZE][idY] = idata[y*w+x+BLOCK_SIZE];
   if (idY < w)
        P[idX][idY + BLOCK_SIZE] = idata[(y+BLOCK_SIZE)*w+x];
   barrier(CLK_LOCAL_MEM_FENCE);

   double sum = 0;

   int indexk = 0;
   for (int i = 0; i < wk; i++)
   {
       int ty = idY + i;
       for (int j = 0; j < wk; j++)
       {
            int tx = idX + j;
            sum+= P[tx][ty] * kernelValues[indexk++];
       }
   }

   barrier(CLK_LOCAL_MEM_FENCE);

   x = x + (wk>>1); 
   y = y + (wk>>1);

    if( sum < psize[2])
        sum = psize[2];
    else if(sum > psize[3])
        sum = psize[3];

   odata[y*w+x] =  (int)sum;
}
*/

void sort(ushort a[],int n)
{
   ushort tmp;
   for(int i=0;i<n;i++)
   {
      for(int j=i+1;j<n;j++)
      {
         if( a[i] > a[j] )
         {
            tmp = a[i];
            a[i] = a[j];
            a[j] = tmp;
         }
      }
   }
}

ushort median(ushort a[],int n)
{
   sort(a,n);
   return a[n/2];
}

__kernel void HotPixelFilter(__global __read_only ushort* idata
                            ,__constant int *     psize//w,h,mingrey,maxgrey,ksize
                            ,__constant double *     param//
                            ,__global __write_only ushort* odata)
{

   int wk = psize[4];
   int wkh = wk >> 1;
   int w = psize[0] ;//get_global_size(0);
   int x0 = get_global_id(0);
   int y0 = get_global_id(1);
   int x = x0 + wkh;
   int y = y0 + wkh;   
   double percentage = param[0];
   
   ushort center,maxNextPixel=0;
   ushort a[9];
   
   int index0 = y0*w+x0;
   int indexk = 0;
   for (int i = 0; i < wk; i++,index0+=w)
   {
       for (int j = 0; j < wk; j++)
       {
            ushort val = idata[index0+j];
            a[indexk++] = val;
            
            if(i==wkh && j==wkh)
                center = val;
            else if( maxNextPixel < val )
                maxNextPixel = val;
       }
   }
   
   double more = center - maxNextPixel;
   if(more >  percentage*(float)maxNextPixel)
       odata[y*w+x] = median(a,indexk);
   else
       odata[y*w+x] = center;
}

__kernel void DeadPixelFilter(__global __read_only ushort* idata
                            ,__constant int *     psize//w,h,mingrey,maxgrey,ksize
                            ,__constant double *     param//
                            ,__global __write_only ushort* odata)
{

   int wk = psize[4];
   int wkh = wk >> 1;
   int w = psize[0] ;//get_global_size(0);
   int x0 = get_global_id(0);
   int y0 = get_global_id(1);
   int x = x0 + wkh;
   int y = y0 + wkh;   
   double percentage = param[0];
   
   ushort center,minNextPixel=psize[4];
   ushort a[9];
   
   int index0 = y0*w+x0;
   int indexk = 0;
   for (int i = 0; i < wk; i++,index0+=w)
   {
       for (int j = 0; j < wk; j++)
       {
            ushort val = idata[index0+j];
            a[indexk++] = val;
            
            if(i==wkh && j==wkh)
                center = val;
            else if( minNextPixel > val )
                minNextPixel= val;
       }
   }
   
   double less= minNextPixel - center;
   if(less >  percentage*(float)minNextPixel )
       odata[y*w+x] = median(a,indexk);
   else
       odata[y*w+x] = center;
}


__kernel void PseudoMedianFilter(__global __read_only ushort* idata
                            ,__constant int *     psize//w,h,mingrey,maxgrey,ksize
                            ,__global __write_only ushort* odata)
{

   int wk = psize[4];
   int wkh= wk >> 1;
   int w = psize[0] ;//get_global_size(0);
   int x0 = get_global_id(0);
   int y0 = get_global_id(1);
   int x = x0 + wkh;
   int y = y0 + wkh;   
   
   ushort roi[13];// (7/2)*4 + 1

   int roiIndex = 0;
   int subArrayLenght = 3; // constant
   int runLenght = 4*wkh + 1 - subArrayLenght;
   int startIndex = 0;
   int endIndex = 0;
   ushort min1 = 0;
   ushort max1 = 0;
   ushort min2 = 0;
   ushort max2 = 0;
   int i = 0;   
   
   for(i=-wkh; i<=wkh; i++)
      roi[roiIndex++] = idata[(i+y)*w + x];
	

   for(i=-wkh; i<0; i++)
   {
      roi[roiIndex++] = idata[y*w + (x+i)];
	roi[roiIndex++] = idata[y*w + (x-i)];
   }
   
   	// find median item
	// initialize
	min1 = roi[0];
	max2 = roi[0];
	for(i=1; i<subArrayLenght; i++)
	{
		if(min1 > roi[i])
			min1 = roi[i];
		if(max2 < roi[i])
			max2 = roi[i];
	}
	max1 = min1;
	min2 = max2;

	for(i=1; i<=runLenght; i++)
	{
		min1 = roi[i];
		max2 = roi[i];
		startIndex = i + 1;
		endIndex = i + subArrayLenght;
		for(startIndex; startIndex<endIndex; startIndex++)
		{
			if(min1 > roi[startIndex])
				min1 = roi[startIndex];
			if(max2 < roi[startIndex])
				max2 = roi[startIndex];
		}
		if(max1 < min1)
			max1 = min1;
		if(min2 > max2)
			min2 = max2;
	}

   odata[y*w + x] = (max1 + min2)/2;
}

__kernel void GaussianSmoothFilter(__global __read_only ushort* idata
                            ,__constant float *   kernelValues
                            ,__constant int *     psize//w,h,mingrey,maxgrey,ksize
                            ,__global __write_only ushort* odata)
{

   int wk = psize[4];
   int wkh = wk >> 1;
   int w = psize[0] ;//get_global_size(0);
   int x0 = get_global_id(0);
   int y0 = get_global_id(1);
   int x = x0 + wkh;
   int y = y0 + wkh;   
   
   double result=0;
   
   int index0 = y0*w+x0;
   int indexk = 0;
   for (int i = 0; i < wk; i++,index0+=w)
   {
       for (int j = 0; j < wk; j++)
       {
            result+= kernelValues[indexk++] * idata[index0+j];
       }
   }
   
   result = fabs(result);//difference BasicConvolution here
/*   
   if(result < psize[3])
      result = psize[3];
   else if(result > psize[4])
      result = psize[4];
*/      
   odata[y*w + x] = (ushort)result;
}


__kernel void MinFilter(__global __read_only ushort* idata
                            ,__constant int *     psize//w,h,mingrey,maxgrey,ksize
                            ,__global __write_only ushort* odata)
{

   int wk = psize[4];
   int wkh = wk >> 1;
   int w = psize[0] ;//get_global_size(0);
   int x0 = get_global_id(0);
   int y0 = get_global_id(1);
   int x = x0 + wkh;
   int y = y0 + wkh;   
   
   ushort retVal = 65535;//max ushort
   int index0 = y0*w+x0;
   for (int i = 0; i < wk; i++,index0+=w)
   {
       for (int j = 0; j < wk; j++)
       {
            ushort val = idata[index0+j];
            if( retVal > val) 
               retVal  = val;
       }
   }
   
   odata[y*w+x] = retVal;
}


__kernel void MaxFilter(__global __read_only ushort* idata
                            ,__constant int *     psize//w,h,mingrey,maxgrey,ksize
                            ,__global __write_only ushort* odata)
{

   int wk = psize[4];
   int wkh = wk >> 1;
   int w = psize[0] ;//get_global_size(0);
   int x0 = get_global_id(0);
   int y0 = get_global_id(1);
   int x = x0 + wkh;
   int y = y0 + wkh;   
   
   ushort retVal = 0;//min ushort
   int index0 = y0*w+x0;
   for (int i = 0; i < wk; i++,index0+=w)
   {
       for (int j = 0; j < wk; j++)
       {
            ushort val = idata[index0+j];
            if( retVal < val) 
               retVal  = val;
       }
   }
   
   odata[y*w+x] = retVal;
}


__kernel void MeanFilter(__global __read_only ushort* idata
                            ,__constant int *     psize//w,h,mingrey,maxgrey,ksize
                            ,__global __write_only ushort* odata)
{

   int wk = psize[4];
   int wkh = wk >> 1;
   int w = psize[0] ;//get_global_size(0);
   int x0 = get_global_id(0);
   int y0 = get_global_id(1);
   int x = x0 + wkh;
   int y = y0 + wkh;   
   
   float sum =0;
   int index0 = y0*w+x0;
   for (int i = 0; i < wk; i++,index0+=w)
   {
       for (int j = 0; j < wk; j++)
       {
            ushort val = idata[index0+j];
            sum+= val;
       }
   }
   
   odata[y*w+x] = (ushort)( sum/(wk*wk) );
}


__kernel void FastPseudoMedianFilter(__global __read_only ushort* idata
                            ,__constant int *     psize//w,h,mingrey,maxgrey,ksize
                            ,__global __write_only ushort* odata)
{
   int wk = psize[4];
   int wkh= wk >> 1;
   int w = psize[0] ;//get_global_size(0);
   int x0 = get_global_id(0);
   int y0 = get_global_id(1);
   int x = x0 + wkh;
   int y = y0 + wkh;   
   
   ushort roi[13];// (7/2)*4 + 1

   int roiIndex = 0;
   int roiLenght = wkh*4+1;
   int middleIndex = roiLenght/2;
   int middleValue = 0;
   int indexLelf = 0;
   int indexRight = roiLenght-1;
   int i = 0;
   int j = 0;
   ushort temp = 0;
   
   for(i=-wkh; i<=wkh; i++)
      roi[roiIndex++] = idata[(i+y)*w + x];
	

   for(i=-wkh; i<0; i++)
   {
      roi[roiIndex++] = idata[y*w + (x+i)];
	roi[roiIndex++] = idata[y*w + (x-i)];
   }
   
   // find median item
	indexLelf = 0;
	indexRight = roiLenght - 1;
	middleIndex = roiLenght/2;
	middleValue = roi[middleIndex];
	while(indexLelf < indexRight)
	{
		i = indexLelf;
		j = indexRight;				
		do {
			while (roi[i] < middleValue) {
				i++;
			}
			while (roi[j] > middleValue) {
				j--;
			}

			// swap
			temp = roi[i];
			roi[i] = roi[j];
			roi[j] = temp;

			// update index
			i++;
			j--;
		} while (j >= middleIndex && i<=middleIndex);

		// update index
		if(j < middleIndex)
			indexLelf = i;
		if(middleIndex < i)
			indexRight = j;

		middleValue = roi[middleIndex];
	}

   odata[y*w + x] = middleValue;
}



";
        #endregion

        public static ComputeKernel kernelBasicConvolution;
        public static ComputeKernel kernelHotPixelFilter;
        public static ComputeKernel kernelDeadPixelFilter;
        public static ComputeKernel kernelPseudoMedianFilter;
        public static ComputeKernel kernelGaussianSmoothFilter;
        public static ComputeKernel kernelMinFilter;
        public static ComputeKernel kernelMaxFilter;
        public static ComputeKernel kernelMeanFilter;
        public static ComputeKernel kernelFastPseudoMedianFilter;
        

        public static void PrepareConvolutionKernel()
        {
            if (kernelBasicConvolution == null)
            {
                DeviceProgram.Compile(srcConvolution);
                kernelBasicConvolution = DeviceProgram.CreateKernel("BasicConvolution");
                //kernelBasicConvolution = DeviceProgram.CreateKernel("BasicConvolutionLocal");
                kernelHotPixelFilter = DeviceProgram.CreateKernel("HotPixelFilter");
                kernelDeadPixelFilter = DeviceProgram.CreateKernel("DeadPixelFilter");
                kernelPseudoMedianFilter = DeviceProgram.CreateKernel("PseudoMedianFilter");
                kernelGaussianSmoothFilter = DeviceProgram.CreateKernel("GaussianSmoothFilter");
                kernelMinFilter = DeviceProgram.CreateKernel("MinFilter");
                kernelMaxFilter = DeviceProgram.CreateKernel("MaxFilter");
                kernelMeanFilter = DeviceProgram.CreateKernel("MeanFilter");
                kernelFastPseudoMedianFilter = DeviceProgram.CreateKernel("FastPseudoMedianFilter");
            }
        }

        private float[] ConvertKernel(int[,] Matrix)
        {
            //prepare kernel matrix
            int ksize = Matrix.GetLength(0);
            int n = ksize * ksize;
            float[] ret = new float[n];

            for (int y = 0; y < ksize; y++)
                for (int x = 0; x < ksize; x++)
                    ret[y * ksize + x] = Matrix[y, x];

            return ret;
        }

        private float[] NormalizeKernel(int[,] Matrix)
        {
            //prepare kernel matrix
            int ksize = Matrix.GetLength(0);
            int n = ksize * ksize;
            float[] ret = new float[n];

            float sum = 0;
            for (int y = 0; y < ksize; y++)
                for (int x = 0; x < ksize; x++)
                    sum += Matrix[y, x];

            if (sum == 0) sum = 1;

            sum = 1.0f / sum;
            for (int y = 0; y < ksize; y++)
                for (int x = 0; x < ksize; x++)
                    ret[y * ksize + x] = Matrix[y, x] * sum;

            return ret;
        }

        public void kApplyFilterGPU(int[,] Matrix, int pass)
        {
            if (!DeviceProgram.GPUSupported)
            {
                kApplyFilter(Matrix, pass);
                return;
            }

            if (cbImage == null) return;

            PrepareConvolutionKernel();

            int ksize = Matrix.GetLength(0);

            float[] fKernel = NormalizeKernel(Matrix);
            DeviceBuffer<float> cbKernel = DeviceBuffer<float>.CreateHostBufferReadOnly(fKernel);

            int[] isize = new int[] { Width, Height, (int)MinGreyValue, (int)MaxGreyValue ,ksize};
            DeviceBuffer<int> cbSize = DeviceBuffer<int>.CreateBufferReadOnly(isize);

            DeviceBuffer<ushort> cbTemp = DeviceBuffer<ushort>.CreateBufferReadWrite(Length);

            for (int i = 0; i < pass; i++)
            {
                DeviceProgram.ExecuteKernel(kernelBasicConvolution,
                   new ComputeMemory[]{ 
                cbImage
                ,cbKernel
                ,cbSize
                ,cbTemp
                }, new int[] { Width - ((ksize >> 1) << 1), Height - ((ksize >> 1) << 1) });
                DeviceProgram.Finish();

                //for next pass
                DeviceBuffer<ushort> tmp = cbImage;
                cbImage = cbTemp;
                cbTemp = tmp;
            }

            cbKernel.Dispose();
            cbSize.Dispose();
            cbTemp.Dispose();

            #region local test

            //if (!DeviceProgram.GPUSupported)
            //{
            //    kApplyFilter(Matrix, pass);
            //    return;
            //}

            //if (cbImage == null) return;

            //PrepareConvolutionKernel();
            //int BLOCK_SIZE = 16;
            //int ksize = Matrix.GetLength(0);

            //int groupSizeX = (int)Math.Ceiling( 1.0*(Width - (ksize >> 1)) / BLOCK_SIZE);
            //int groupSizeY = (int)Math.Ceiling(1.0 * (Height - (ksize >> 1)) / BLOCK_SIZE);
            //int[] GlobalSize = new int[] { groupSizeX * BLOCK_SIZE, groupSizeY * BLOCK_SIZE };
            //int[] LocalSize = new int[] { BLOCK_SIZE, BLOCK_SIZE };


            //float[] fKernel = NormalizeKernel(Matrix);
            //DeviceBuffer<float> cbKernel = DeviceBuffer<float>.CreateHostBufferReadOnly(fKernel);

            //int[] isize = new int[] { Width, Height,  (int)MinGreyValue, (int)MaxGreyValue ,ksize};
            //DeviceBuffer<int> cbSize = DeviceBuffer<int>.CreateBufferReadOnly(isize);

            //DeviceBuffer<ushort> cbTemp = DeviceBuffer<ushort>.CreateBufferReadWrite(Length);

            //for (int i = 0; i < pass; i++)
            //{
            //    DeviceProgram.ExecuteKernel(kernelBasicConvolution,
            //       new ComputeMemory[]{ 
            //    cbImage
            //    ,cbKernel
            //    ,cbSize
            //    ,cbTemp
            //    }, GlobalSize,LocalSize );
            //    DeviceProgram.Finish();

            //    //for next pass
            //    DeviceBuffer<ushort> tmp = cbImage;
            //    cbImage = cbTemp;
            //    cbTemp = tmp;
            //}

            //cbKernel.Dispose();
            //cbSize.Dispose();
            //cbTemp.Dispose();

            #endregion local test
        }

        public void kApplyFilterGPU(eMaskType maskType, eMatrixType matrixType, int pass, float threshold)
        {
            if(!DeviceProgram.GPUSupported)
            {
                kApplyFilter(maskType, matrixType, pass, threshold);
                return;
            }

            if (cbImage == null) return;

            PrepareConvolutionKernel();

            if (maskType == eMaskType.kMASK_HOTPIXEL)
            {
                int ksize = 3;
                int[] isize = new int[] { Width, Height, (int)MinGreyValue, (int)MaxGreyValue ,ksize};
                DeviceBuffer<int> cbSize = DeviceBuffer<int>.CreateBufferReadOnly(isize);
                DeviceBuffer<double> cbParam = DeviceBuffer<double>.CreateHostBufferReadOnly(new double[]{threshold});
                DeviceBuffer<ushort> cbTemp = DeviceBuffer<ushort>.CreateBufferReadWrite(Length);

                for (int i = 0; i < 1; i++)
                {
                    DeviceProgram.ExecuteKernel(kernelHotPixelFilter,
                       new ComputeMemory[]{ 
                        cbImage
                        ,cbSize
                        ,cbParam
                        ,cbTemp
                        }, new int[] { Width - ((ksize >> 1) << 1), Height - ((ksize >> 1) << 1) });
                            DeviceProgram.Finish();

                            //for next pass
                            DeviceBuffer<ushort> tmp = cbImage;
                            cbImage = cbTemp;
                            cbTemp = tmp;
                        }

                cbParam.Dispose();
                cbSize.Dispose();
                cbTemp.Dispose();

            }
            else if (maskType == eMaskType.kMASK_DEADPIXEL)
            {
                int ksize = 3;
                int[] isize = new int[] { Width, Height, (int)MinGreyValue, (int)MaxGreyValue,ksize};
                DeviceBuffer<int> cbSize = DeviceBuffer<int>.CreateBufferReadOnly(isize);
                DeviceBuffer<double> cbParam = DeviceBuffer<double>.CreateHostBufferReadOnly(new double[] { threshold });
                DeviceBuffer<ushort> cbTemp = DeviceBuffer<ushort>.CreateBufferReadWrite(Length);

                for (int i = 0; i < 1; i++)
                {
                    DeviceProgram.ExecuteKernel(kernelDeadPixelFilter,
                       new ComputeMemory[]{ 
                        cbImage
                        ,cbSize
                        ,cbParam
                        ,cbTemp
                        }, new int[] { Width - ((ksize >> 1) << 1), Height - ((ksize >> 1) << 1) });
                            DeviceProgram.Finish();

                            //for next pass
                            DeviceBuffer<ushort> tmp = cbImage;
                            cbImage = cbTemp;
                            cbTemp = tmp;
                        }

                cbParam.Dispose();
                cbSize.Dispose();
                cbTemp.Dispose();
            }
            else
            {
                kFilter filter = kKernelFilters.QueryConvolutionFilter(maskType, matrixType);
                if (maskType == eMaskType.kMASK_MEDIAN)
                {
                    int ksize = filter.Width;//w=h=size

                    int[] isize = new int[] { Width, Height,(int)MinGreyValue, (int)MaxGreyValue,ksize };
                    DeviceBuffer<int> cbSize = DeviceBuffer<int>.CreateBufferReadOnly(isize);
                    DeviceBuffer<ushort> cbTemp = DeviceBuffer<ushort>.CreateBufferReadWrite(Length);

                    for (int i = 0; i < pass; i++)
                    {
                        DeviceProgram.ExecuteKernel(kernelPseudoMedianFilter,
                           new ComputeMemory[]{ 
                        cbImage
                        ,cbSize
                        ,cbTemp
                        }, new int[] { Width - ((ksize >> 1) << 1), Height - ((ksize >> 1) << 1) });
                                DeviceProgram.Finish();

                                //for next pass
                                DeviceBuffer<ushort> tmp = cbImage;
                                cbImage = cbTemp;
                                cbTemp = tmp;
                    }

                    cbSize.Dispose();
                    cbTemp.Dispose();
                }
                else
                    kApplyFilterGPU(filter.Kernel, pass);

                filter.Dispose();
            }
        }

        public void GaussianSmoothFilterGPU()
        {
            if(!DeviceProgram.GPUSupported)
            {
                GaussianSmoothFilter();
                return;
            }

            if (cbImage == null) return;

            PrepareConvolutionKernel();

            int[,] kfilter = new int[,] {    {0,1,0}
                                            ,{1,4,1}
                                            ,{0,1,0}
            };

            int ksize = 3;
            float[] fKernel = NormalizeKernel(kfilter);
            DeviceBuffer<float> cbKernel = DeviceBuffer<float>.CreateHostBufferReadOnly(fKernel);

            int[] isize = new int[] { Width, Height,(int)MinGreyValue, (int)MaxGreyValue,ksize };
            DeviceBuffer<int> cbSize = DeviceBuffer<int>.CreateBufferReadOnly(isize);

            DeviceBuffer<ushort> cbTemp = DeviceBuffer<ushort>.CreateBufferReadWrite(Length);

            DeviceProgram.ExecuteKernel(kernelGaussianSmoothFilter,
               new ComputeMemory[]{ 
            cbImage
            ,cbKernel
            ,cbSize
            ,cbTemp
            }, new int[] { Width - ((ksize >> 1) << 1), Height - ((ksize >> 1) << 1) });
            DeviceProgram.Finish();

            //for next pass
            cbImage.Dispose();
            cbImage = cbTemp;
        
            cbKernel.Dispose();
            cbSize.Dispose();

        }


        public void FilterRankGPU(int typeFilter, int kernel, int pass)
        {
            if(!DeviceProgram.GPUSupported)
            {
                FilterRank(typeFilter, kernel, pass);
                return;
            }

            if (cbImage == null) return;

            PrepareConvolutionKernel();

            int ksize = kernel;

            int[] isize = new int[] { Width, Height, (int)MinGreyValue, (int)MaxGreyValue,ksize };
            DeviceBuffer<int> cbSize = DeviceBuffer<int>.CreateBufferReadOnly(isize);

            DeviceBuffer<ushort> cbTemp = DeviceBuffer<ushort>.CreateBufferReadWrite(Length);

            ComputeKernel kernelFilter = kernelMinFilter;//min
            if (1 == typeFilter)//max
                kernelFilter = kernelMaxFilter;
            else if (2 == typeFilter)//MEDIAN
                kernelFilter = kernelFastPseudoMedianFilter;
            else//MEAN
                kernelFilter = kernelMeanFilter;

            for (int i = 0; i < pass; i++)
            {
                DeviceProgram.ExecuteKernel(kernelFilter,
                   new ComputeMemory[]{ 
                cbImage
                ,cbSize
                ,cbTemp
                }, new int[] { Width - ((ksize >> 1) << 1), Height - ((ksize >> 1) << 1) });
                DeviceProgram.Finish();

                //for next pass
                DeviceBuffer<ushort> tmp = cbImage;
                cbImage = cbTemp;
                cbTemp = tmp;
            }

            kernelFilter = null;

            cbSize.Dispose();
            cbTemp.Dispose();
        }


        #region Convolution  Source
        const string srcMorphology = @"#define CHECKBOUND(value, MIN, MAX) max(min(value,MAX),MIN);
                                        //#define BLOCK_SIZE 16
#pragma OPENCL EXTENSION cl_khr_fp64 : enable 

__kernel void kErosion(__global __read_only ushort* idata,
                            __constant float *   kernelValues,
                            __constant int *     psize,//w,h,mingrey,maxgrey,ksize
                            __global __write_only ushort* odata)
{
   int wk = psize[4];
   int wkh = wk >> 1;
   int w = psize[0];//get_global_size(0);
   int h = psize[1];//get_global_size(1);
   int x = get_global_id(0);
   int y = get_global_id(1);
   int x0 = x - wkh;
   int y0 = y - wkh;   
   int xx,yy;
   
   ushort retVal = 65535;//max ushort
   int index0 = y0*w+x0;
   int indexk =0;
   for (int i = 0; i < wk; i++,index0+=w)
   {
       for (int j = 0; j < wk; j++)
       {
            xx = x0+j;yy=y0+i;
            if( yy < 0 || yy >= h || xx <0 || xx >=w) continue;

            uint val = idata[index0+j] + kernelValues[indexk++];
            if( retVal > val) 
               retVal  = val;
       }
   }
   odata[y*w+x] = retVal;
}

__kernel void kDilation(__global __read_only ushort* idata,
                            __constant float *   kernelValues,
                            __constant int *     psize,//w,h,mingrey,maxgrey,ksize
                            __global __write_only ushort* odata)
{
   int wk = psize[4];
   int wkh = wk >> 1;
   int w = psize[0];//get_global_size(0);
   int h = psize[1];//get_global_size(1);
   int x = get_global_id(0);
   int y = get_global_id(1);
   int x0 = x - wkh;
   int y0 = y - wkh;   
   int xx,yy;
   
   ushort retVal = 0;//min ushort
   int index0 = y0*w+x0;
   int indexk =0;
   for (int i = 0; i < wk; i++,index0+=w)
   {
       for (int j = 0; j < wk; j++)
       {
            xx = x0+j;yy=y0+i;
            if( yy < 0 || yy >= h || xx <0 || xx >=w) continue;

            uint val = idata[index0+j] + kernelValues[indexk++];
            if( retVal < val) 
               retVal  = val;
       }
   }
   
   odata[y*w+x] = retVal;
}


__kernel void kFlatErosion(__global __read_only ushort* idata,
                            __constant float *   kernelValues,
                            __constant int *     psize,//w,h,mingrey,maxgrey,ksize
                            __global __write_only ushort* odata)
{
   int wk = psize[4];
   int wkh = wk >> 1;
   int w = psize[0];//get_global_size(0);
   int h = psize[1];//get_global_size(1);
   int x = get_global_id(0);
   int y = get_global_id(1);
   int x0 = x - wkh;
   int y0 = y - wkh;   
   int xx,yy;
   
   ushort retVal = 255;//65535;//max ushort
   int index0 = y0*w+x0;
   int indexk =0;
   for (int i = 0; i < wk; i++,index0+=w)
   {
       for (int j = 0; j < wk; j++)
       {
            xx = x0+j;yy=y0+i;
            if( yy < 0 || yy >= h || xx <0 || xx >=w) continue;

            if(kernelValues[indexk++]!=0)
            {
                uint val = idata[index0+j];
                if( retVal > val)
                    retVal  = val;                
            }
       }
   }
   
   odata[y*w+x] = retVal;
}

__kernel void kFlatDilation(__global __read_only ushort* idata,
                            __constant float *   kernelValues,
                            __constant int *     psize,//w,h,mingrey,maxgrey,ksize
                            __global __write_only ushort* odata)
{
   int wk = psize[4];
   int wkh = wk >> 1;
   int w = psize[0];//get_global_size(0);
   int h = psize[1];//get_global_size(1);
   int x = get_global_id(0);
   int y = get_global_id(1);
   int x0 = x - wkh;
   int y0 = y - wkh;   
   int xx,yy;
   
   ushort retVal = 0;
   int index0 = y0*w+x0;
   int indexk =0;
   for (int i = 0; i < wk; i++,index0+=w)
   {
       for (int j = 0; j < wk; j++)
       {
            xx = x0+j;yy=y0+i;
            if( yy < 0 || yy >= h || xx <0 || xx >=w) continue;

            if(kernelValues[indexk++]!=0)
            {
                uint val = idata[index0+j];
                if( retVal < val)
                    retVal  = val;                
            }
       }
   }
   
   odata[y*w+x] = retVal;
}

__kernel void SumValue(__global __read_only ushort* idata,
                            __constant int *     psize,//w,h,mingrey,maxgrey,ksize
                            __global __write_only double* odata)
{
   int w = psize[0] ;//get_global_size(0);
   //int h = psize[1] ;
   int y = get_global_id(0);

   double sum = 0;
   int index = y*w;
   for(int i=0;i<w;i++)
   {
      //sum+= idata[index++]/(w*h);//original code
      sum+= idata[index++];
   }
   odata[y] = sum;
}

__kernel void kFlatten(__global __read_only ushort* idata,
                       __global ushort* odata,
                       __constant int *     psize,//w,h,mingrey,maxgrey,ksize
                       __constant double *avg)
{
   int w = psize[0] ;//get_global_size(0);
   int x = get_global_id(0);
   int y = get_global_id(1);

   int index = y*w+x;
   double ret = idata[index] - odata[index] + avg[0];
   ret = fabs(ret);
   
   if( ret < psize[2])
      ret = psize[2];
   else if( ret > psize[3])
      ret = psize[3];
      
   odata[index] = ret;
}


";
        #endregion

        public static ComputeKernel kernelkErosion;
        public static ComputeKernel kernelkDilation;
        public static ComputeKernel kernelkFlatErosion;
        public static ComputeKernel kernelkFlatDilation;
        public static ComputeKernel kernelSumValue;
        public static ComputeKernel kernelkFlatten;
        
        
        public static void PrepareMorphologyKernel()
        {
            if (kernelkErosion == null)
            {
                DeviceProgram.Compile(srcMorphology);
                kernelkErosion = DeviceProgram.CreateKernel("kErosion");
                kernelkDilation = DeviceProgram.CreateKernel("kDilation");
                kernelkFlatErosion = DeviceProgram.CreateKernel("kFlatErosion");
                kernelkFlatDilation = DeviceProgram.CreateKernel("kFlatDilation");
                kernelSumValue = DeviceProgram.CreateKernel("SumValue");
                kernelkFlatten = DeviceProgram.CreateKernel("kFlatten");
            }
        }

        public void kApplyFilterGPU(eMorphType morphType, eMatrixType matrixType, int pass)
        {
            if(!DeviceProgram.GPUSupported)
            {
                kApplyFilter(morphType, matrixType, pass);
                return;
            }

            if (cbImage == null) return;

            PrepareMorphologyKernel();

            int [,] mask = kKernelFilters.QueryMorphologyKernelMatrix(matrixType);

            int ksize = mask.GetLength(0);
            float[] fKernel = ConvertKernel(mask);//NormalizeKernel(mask);
            DeviceBuffer<float> cbKernel = DeviceBuffer<float>.CreateHostBufferReadOnly(fKernel);

            int[] isize = new int[] { Width, Height,(int)MinGreyValue, (int)MaxGreyValue,ksize };
            DeviceBuffer<int> cbSize = DeviceBuffer<int>.CreateBufferReadOnly(isize);

            DeviceBuffer<ushort> cbTemp = DeviceBuffer<ushort>.CreateBufferReadWrite(Length);

            ComputeKernel kernelFilter = null;

            if (morphType == eMorphType.kMORPH_EROSION
                || morphType == eMorphType.kMORPH_DILATION
                || morphType == eMorphType.kMORPH_FLAT_EROSION
                || morphType == eMorphType.kMORPH_FLAT_DILATION)
            {
                if (morphType == eMorphType.kMORPH_EROSION)
                    kernelFilter = kernelkErosion;
                else if (morphType == eMorphType.kMORPH_DILATION)
                    kernelFilter = kernelkDilation;
                else if (morphType == eMorphType.kMORPH_FLAT_EROSION)
                    kernelFilter = kernelkFlatErosion;
                else if (morphType == eMorphType.kMORPH_FLAT_DILATION)
                    kernelFilter = kernelkFlatDilation;

                for (int i = 0; i < pass; i++)
                {
                    DeviceProgram.ExecuteKernel(kernelFilter,
                    new ComputeMemory[]{ 
                    cbImage
                    ,cbKernel
                    ,cbSize
                    ,cbTemp
                    }, new int[] { Width, Height});
                    DeviceProgram.Finish();

                    //for next pass
                    DeviceBuffer<ushort> tmp = cbImage; cbImage = cbTemp; cbTemp = tmp;
                }
            }
            else
            {
                if (morphType == eMorphType.kMORPH_CLOSING)
                {
                    //dilation -> erosion
                    for (int i = 0; i < pass; i++)
                    {
                        DeviceProgram.ExecuteKernel(kernelkDilation,
                        new ComputeMemory[]{ 
                        cbImage
                        ,cbKernel
                        ,cbSize
                        ,cbTemp
                        }, new int[] { Width, Height });
                        DeviceProgram.Finish();

                        DeviceProgram.ExecuteKernel(kernelkErosion,
                      new ComputeMemory[]{ 
                        cbTemp
                        ,cbKernel
                        ,cbSize
                        ,cbImage
                        }, new int[] { Width, Height });
                        DeviceProgram.Finish();
                    }
                }
                else if (morphType == eMorphType.kMORPH_OPENING)
                {
                    //erosion -> dilation
                    for (int i = 0; i < pass; i++)
                    {
                        DeviceProgram.ExecuteKernel(kernelkErosion,
                        new ComputeMemory[]{ 
                        cbImage
                        ,cbKernel
                        ,cbSize
                        ,cbTemp
                        }, new int[] { Width, Height });
                        DeviceProgram.Finish();

                        DeviceProgram.ExecuteKernel(kernelkDilation,
                      new ComputeMemory[]{ 
                        cbTemp
                        ,cbKernel
                        ,cbSize
                        ,cbImage
                        }, new int[] { Width, Height });
                        DeviceProgram.Finish();
                    }
                }
                else if (morphType == eMorphType.kMORPH_FLAT_CLOSING)
                {
                    //flat dilation -> flat erosion
                    for (int i = 0; i < pass; i++)
                    {
                        DeviceProgram.ExecuteKernel(kernelkFlatDilation,
                        new ComputeMemory[]{ 
                        cbImage
                        ,cbKernel
                        ,cbSize
                        ,cbTemp
                        }, new int[] { Width, Height });
                        DeviceProgram.Finish();

                        DeviceProgram.ExecuteKernel(kernelkFlatErosion,
                      new ComputeMemory[]{ 
                        cbTemp
                        ,cbKernel
                        ,cbSize
                        ,cbImage
                        }, new int[] { Width, Height });
                        DeviceProgram.Finish();
                    }
                }
                else if (morphType == eMorphType.kMORPH_FLAT_OPENING)
                {
                    //flat erosion -> flat dilation
                    for (int i = 0; i < pass; i++)
                    {
                        DeviceProgram.ExecuteKernel(kernelkFlatErosion,
                        new ComputeMemory[]{ 
                        cbImage
                        ,cbKernel
                        ,cbSize
                        ,cbTemp
                        }, new int[] { Width, Height });
                        DeviceProgram.Finish();

                        DeviceProgram.ExecuteKernel(kernelkFlatDilation,
                      new ComputeMemory[]{ 
                        cbTemp
                        ,cbKernel
                        ,cbSize
                        ,cbImage
                        }, new int[] { Width, Height });
                        DeviceProgram.Finish();
                    }
                }
                else if (morphType == eMorphType.kMORPH_TOP_HAT)
                {
                    //opening then source - output
                    //erosion -> dilation
                    DeviceBuffer<ushort> cbBuff = DeviceBuffer<ushort>.CreateBufferReadWrite(Length);
                    for (int i = 0; i < pass; i++)
                    {
                        DeviceProgram.ExecuteKernel(kernelkErosion,
                        new ComputeMemory[]{ 
                        cbImage
                        ,cbKernel
                        ,cbSize
                        ,cbTemp
                        }, new int[] { Width, Height });
                        DeviceProgram.Finish();

                        DeviceProgram.ExecuteKernel(kernelkDilation,
                        new ComputeMemory[]{ 
                        cbTemp
                        ,cbKernel
                        ,cbSize
                        ,cbBuff
                        }, new int[] { Width, Height });
                        DeviceProgram.Finish();

                        DeviceProgram.ExecuteKernel(kernelMonadicOperationSub,
                        new ComputeMemory[]{ 
                        cbImage
                        ,cbSize
                        ,cbBuff
                        }, new int[] {Width,Height});
                        DeviceProgram.Finish();
                    }
                    cbBuff.Dispose();
                }
                else if (morphType == eMorphType.kMOPRH_BOTTOM_HAT)
                {
                    //closing then source - output
                    //dilation -> erosion
                    DeviceBuffer<ushort> cbBuff = DeviceBuffer<ushort>.CreateBufferReadWrite(Length);
                    for (int i = 0; i < pass; i++)
                    {
                        DeviceProgram.ExecuteKernel(kernelkDilation,
                        new ComputeMemory[]{ 
                        cbImage
                        ,cbKernel
                        ,cbSize
                        ,cbTemp
                        }, new int[] { Width, Height });
                        DeviceProgram.Finish();

                        DeviceProgram.ExecuteKernel(kernelkErosion,
                      new ComputeMemory[]{ 
                        cbTemp
                        ,cbKernel
                        ,cbSize 
                        ,cbBuff
                        }, new int[] { Width, Height });
                        DeviceProgram.Finish();

                        DeviceProgram.ExecuteKernel(kernelMonadicOperationSub,
                        new ComputeMemory[]{ 
                        cbImage
                        ,cbSize
                        ,cbBuff
                        }, new int[] { Width, Height });
                        DeviceProgram.Finish();
                    }
                    cbBuff.Dispose();
                    ResetDeviceKernelBound(ksize, 0);
                }
                else if (morphType == eMorphType.kMORPH_FLATTEN)
                {
                    DeviceBuffer<ushort> cbSource = CloneDeviceMemory();
                    DeviceBuffer<double> cbSum = DeviceBuffer<double>.CreateBufferReadWrite(Height);

                    for (int i = 0; i < pass; i++)
                    {
#region this code is good more ?
                      //  //closing : dilation -> erosion
                      //  DeviceProgram.ExecuteKernel(kernelkDilation,
                      // new ComputeMemory[]{ 
                      //  cbImage
                      //  ,cbKernel
                      //  ,cbSize
                      //  ,cbTemp
                      //  }, new int[] { Width, Height });
                      //  DeviceProgram.Finish();

                      //  DeviceProgram.ExecuteKernel(kernelkErosion,
                      //new ComputeMemory[]{ 
                      //  cbTemp
                      //  ,cbKernel
                      //  ,cbSize 
                      //  ,cbImage
                      //  }, new int[] { Width, Height });
                      //  DeviceProgram.Finish();

                      //  //compute average pixel value of closing result
                      //  DeviceProgram.ExecuteKernel(kernelSumValue,
                      //new ComputeMemory[]{ 
                      //  cbImage
                      //  ,cbSize
                      //  ,cbSum
                      //  }, Height);
                      //  DeviceProgram.Finish();

                      //  //sum
                      //  double[] lsum = cbSum.Read(null);
                      //  double sum = 0;
                      //  for (int j = 0; j < Height; j++)
                      //  {
                      //      sum += lsum[j];
                      //  }
                      //  sum = sum / (Width * Height);//comment this to original code

                      //  //kFlatten
                      //  DeviceBuffer<double> cbAvg = DeviceBuffer<double>.CreateHostBufferReadOnly(new double[] { sum });
                      //  DeviceProgram.ExecuteKernel(kernelkFlatten,
                      //  new ComputeMemory[]{ 
                      //  cbSource
                      //  ,cbImage//output
                      //  ,cbSize
                      //  ,cbAvg
                      //  }, new int[] { Width, Height });
                      //  DeviceProgram.Finish();
                      //  cbAvg.Dispose();
#endregion
                        //ORIGINAL CODE
                        //closing = erosion
                        DeviceProgram.ExecuteKernel(kernelkErosion,
                      new ComputeMemory[]{ 
                        cbImage
                        ,cbKernel
                        ,cbSize 
                        ,cbTemp
                        }, new int[] { Width, Height });
                        DeviceProgram.Finish();
                        DeviceBuffer<ushort> tmp = cbImage; cbImage = cbTemp; cbTemp = tmp;


                        //compute average pixel value of closing result
                        DeviceProgram.ExecuteKernel(kernelSumValue,
                      new ComputeMemory[]{ 
                        cbImage
                        ,cbSize
                        ,cbSum
                        }, Height);
                        DeviceProgram.Finish();

                        //sum
                        double []lsum = cbSum.Read(null);
                        double sum = 0;
                        for (int j = 0; j < Height; j++)
                        {
                            sum += lsum[j];
                        }
                        sum =  sum /(Width*Height);//comment this to original code

                        //kFlatten
                        DeviceBuffer<double> cbAvg = DeviceBuffer<double>.CreateHostBufferReadOnly(new double[]{sum});
                        DeviceProgram.ExecuteKernel(kernelkFlatten,
                        new ComputeMemory[]{ 
                        cbSource
                        ,cbImage//output
                        ,cbSize
                        ,cbAvg
                        }, new int[] { Width, Height });
                        DeviceProgram.Finish();
                        cbAvg.Dispose();
                    }

                    cbSum.Dispose();
                    cbSource.Dispose();
                }
            }

            cbKernel.Dispose();
            cbSize.Dispose();
            cbTemp.Dispose();

        }

#endif

#endregion GPU_SUPPORTED

    }
}
