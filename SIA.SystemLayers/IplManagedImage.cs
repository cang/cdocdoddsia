using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using SIA.Common.IPLFacade;
using System.Drawing;
using System.Collections;

namespace SIA.SystemLayer
{
    /// <summary>
    /// The IplManagedImage class implements IIplImage class. This class used managed memory and 
    /// wrapped it as an IIplImage interface for using with IPL library.
    /// </summary>
    public class IplManagedImage 
        : IIplImage
    {
        private string _filePath = string.Empty;
        private IplPixelFormat _pixelFormat = IplPixelFormat.Format16bppGrayScale;
        private int _width = 0, _height = 0, _stride = 0;
        private GCHandle _handle;
        private Array _buffer = null;

        public IplManagedImage(int width, int height, IplPixelFormat pixelFormat)
        {
            this.InitClass(width, height, pixelFormat);
        }

        ~IplManagedImage()
        {
            this.Dispose(false);
        }

        private void InitClass(int width, int height, IplPixelFormat pixelFormat)
        {
            _pixelFormat = pixelFormat;
            int nChannels = GetPixelFormatChannels(pixelFormat);
            int pixsize = GetPixelFormatBPP(pixelFormat) / 8;
            _width = width; _height = height;
            _stride = _width * pixsize;
            int length = width * height;
            Type elementType = GetPixelFormatElementType(_pixelFormat);
            _buffer = Array.CreateInstance(elementType, length);

            _handle = GCHandle.Alloc(_buffer, GCHandleType.Pinned);
            IntPtr bufptr = _handle.AddrOfPinnedObject();
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_handle.IsAllocated)
                _handle.Free();

            if (this._buffer != null)
                this._buffer = null;
        }

        #endregion

        #region IIplImage Members

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        public IntPtr Buffer
        {
            get { return _handle.AddrOfPinnedObject(); }
        }

        public IplPixelFormat PixelFormat
        {
            get { return this._pixelFormat; }
        }

        public int BitsPerPixel
        {
            get { return GetPixelFormatBPP(_pixelFormat); }
        }

        public int Channels
        {
            get { return GetPixelFormatChannels(_pixelFormat); }
        }

        public int Length
        {
            get { return _buffer != null ? _buffer.Length : 0; }
        }

        public string FilePath
        {
            get { return _filePath; }
        }

		public IImagePropertyCollection Properties
		{
			get
			{
				throw new NotImplementedException("ATTENTION: MetaData is not available in this class!!"); 
			}
		}
        #endregion


        public static Type GetPixelFormatElementType(IplPixelFormat format)
        {
            switch (format)
            {
                case IplPixelFormat.Format16bppGrayScale:
                    return typeof(ushort);
                default:
                    return typeof(byte);
            }
        }

        public static int GetPixelFormatBPP(IplPixelFormat format)
        {
            switch (format)
            {
                case IplPixelFormat.Format1bppIndexed:
                    return 1;
                case IplPixelFormat.Format4bppIndexed:
                    return 4;
                case IplPixelFormat.Format8bppIndexed:
                case IplPixelFormat.Format8bppGrayScale:
                    return 8;
                case IplPixelFormat.Format16bppArgb1555:
                case IplPixelFormat.Format16bppRgb555:
                case IplPixelFormat.Format16bppRgb565:
                case IplPixelFormat.Format16bppGrayScale:
                    return 16;
                case IplPixelFormat.Format24bppRgb:
                    return 24;
                case IplPixelFormat.Format32bppArgb:
                case IplPixelFormat.Format32bppPArgb:
                case IplPixelFormat.Format32bppRgb:
                    return 32;
                case IplPixelFormat.Format48bppRgb:
                    return 48;
                case IplPixelFormat.Format64bppArgb:
                case IplPixelFormat.Format64bppPArgb:
                    return 64;
                default:
                    throw new NotSupportedException("PixelFormat " + format + " is not supported.");
            }
        }

        public static int GetPixelFormatChannels(IplPixelFormat format)
        {
            switch (format)
            {
                case IplPixelFormat.Format1bppIndexed:
                case IplPixelFormat.Format4bppIndexed:
                case IplPixelFormat.Format8bppIndexed:
                case IplPixelFormat.Format8bppGrayScale:
                case IplPixelFormat.Format16bppGrayScale:
                    return 1;
                case IplPixelFormat.Format16bppRgb555:
                case IplPixelFormat.Format16bppRgb565:
                case IplPixelFormat.Format24bppRgb:
                case IplPixelFormat.Format32bppRgb:
                case IplPixelFormat.Format48bppRgb:
                    return 3;
                case IplPixelFormat.Format16bppArgb1555:
                case IplPixelFormat.Format32bppArgb:
                case IplPixelFormat.Format64bppArgb:
                    return 4;
                case IplPixelFormat.Format32bppPArgb:
                case IplPixelFormat.Format64bppPArgb:
                    return 5;
                default:
                    throw new NotSupportedException("PixelFormat " + format + " is not supported.");
            }
        }
    }
}
