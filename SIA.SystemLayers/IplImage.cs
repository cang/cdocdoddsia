using System;
using System.Collections.Generic;
using System.Text;
using SIA.Common.IPLFacade;

#if USING_IPL

namespace SIA.SystemLayer
{
    /// <summary>
    /// Wrapper class of the CommonImage to make it compatible with the IPL library
    /// </summary>
    public class IplCommonImage : IIplImage
    {
        /// <summary>
        /// Value indicates whether the internal image is disposed.
        /// </summary>
        private bool _autoDisposeImage = false;

        /// <summary>
        /// Internal common image holding the image data
        /// </summary>
        private CommonImage _image = null;

        public IplCommonImage(CommonImage image)
        {
            _image = image;
            _autoDisposeImage = false;
        }

        public IplCommonImage(string fileName)
        {
            _image = CommonImage.FromFile(fileName);
            _autoDisposeImage = true;
        }

        ~IplCommonImage()
        {
            this.Dispose(false);
        }

        #region IIplImage Members

        /// <summary>
        /// Gets the pixel format for this image
        /// </summary>
        public IplPixelFormat PixelFormat
        {
            get
            {
                return IplPixelFormat.Format16bppGrayScale;
            }
        }

        /// <summary>
        /// Gets the number of bits per pixel for this image
        /// </summary>
        public int BitsPerPixel
        {
            get
            {
                return 16;
            }
        }

        /// <summary>
        /// Gets the number of channels for this image.
        /// </summary>
        public int Channels
        {
            get { return 1; }
        }

        /// <summary>
        /// Gets the unsafe pointer to the image data buffer
        /// </summary>
        public IntPtr Buffer
        {
            get
            {
                return _image.RasterImage.Buffer;
            }
        }

        /// <summary>
        /// Gets the width for this image
        /// </summary>
        public int Width
        {
            get { return _image.Width; }
        }

        /// <summary>
        /// Gets the height for this image
        /// </summary>
        public int Height
        {
            get { return _image.Height; }
        }

        /// <summary>
        /// Gets the number of pixels of the image buffer
        /// </summary>
        public int Length
        {
            get { return _image.Length; }
        }

        /// <summary>
        /// Gets the file location for this image
        /// </summary>
        public string FilePath
        {
            get { return _image.FilePath; }
        }

		public IImagePropertyCollection Properties
		{
			get 
			{
				return _image.Properties;
			}
		}
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (_autoDisposeImage && _image != null)
                _image.Dispose();
            _image = null;
        }
    }
}

#endif