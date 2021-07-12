using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIA.Common.Analysis;
using SIA.SystemLayer;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using SIA.UI.Controls.Commands;
using SiGlaz.Common.ABSDefinitions;
using SiGlaz.UI.CustomControls.ListViewEx;

namespace SIA.UI.Controls.Dialogs
{
    public partial class DlgObjectListEx2 : FloatingFormBase
    {
        #region Member fields
        private ImageWorkspace _workspace = null;
        private List<DetectedObject> _objects = null;
        #endregion Member fields

        #region Properties
        public List<DetectedObject> Objects
        {
            get
            {
                return _objects;
            }
            set
            {
                if (_objects != null)
                    _objects.Clear();

                _objects = value;
                if (objectList != null)
                {
                    objectList.Items.Clear();
                    ClearImageList(smallImageList);
                    ClearImageList(largeImageList);

                    if (_objects != null)
                    {                        
                        FillDataSource();
                    }
                }

                this.Invalidate(true);
            }
        }
        #endregion Properties

        #region Contructors and destructors
        public DlgObjectListEx2(ImageWorkspace workSpace, List<DetectedObject> objects)
            : base()
        {            
            InitializeComponent();

            // initialize top level window
            this.TopLevel = true;

            _workspace = workSpace;
            _objects = objects;

            if (_workspace != null)
            {
                this.Owner = _workspace.FindForm();
                _workspace.DetectedObjectsChanged += new EventHandler(_workspace_DetectedObjectsChanged);
            }

            objectList.Columns.Remove(idColumn);

            TypedObjectListView<DetectedObject> tlist =
                new TypedObjectListView<DetectedObject>(this.objectList);
            tlist.GenerateAspectGetters();
            typeColumn.AspectGetter = delegate(object row)
            {
                DetectedObject obj = row as DetectedObject;

                return ((eDefectType)obj.ObjectTypeId);
            };

            xColumn.AspectGetter = delegate(object row)
            {
                DetectedObject obj = row as DetectedObject;

                return obj.RectBound.Left;
            };
            yColumn.AspectGetter = delegate(object row)
            {
                DetectedObject obj = row as DetectedObject;

                return obj.RectBound.Top;
            };
            widthColumn.AspectGetter = delegate(object row)
            {
                DetectedObject obj = row as DetectedObject;

                return (obj.RectBound.Width + 1);
            };
            heightColumn.AspectGetter = delegate(object row)
            {
                DetectedObject obj = row as DetectedObject;

                return (obj.RectBound.Height + 1);
            };

            thumbnailColumn.ImageGetter = delegate(object row)
            {
                DetectedObject obj = row as DetectedObject;
                return _objects.IndexOf(obj);
            };


            //typeColumn.MakeGroupies(
            //    new object[] { 
            //        eDefectType.Contamination.ToString(), 
            //        eDefectType.Scratch.ToString(), 
            //        eDefectType.OverPattern.ToString() },
            //    new string[] { 
            //        eDefectType.Contamination.ToString(), 
            //        eDefectType.Scratch.ToString(), 
            //        eDefectType.OverPattern.ToString(),
            //        eDefectType.Unknown.ToString()}
            //        );
            //idColumn.AspectGetter = delegate(object row)
            //{
            //    DetectedObject obj = row as DetectedObject;
            //    return _objects.IndexOf(obj) + 1;
            //};
            
        }

        void _workspace_DetectedObjectsChanged(object sender, EventArgs e)
        {
            List<DetectedObject> objects = new List<DetectedObject>();
            if (_workspace.DetectedObjects != null &&
                _workspace.DetectedObjects.Count > 0)
            {
                objects.AddRange(
                    (DetectedObject[])_workspace.DetectedObjects.ToArray(typeof(DetectedObject)));
            }

            this.Objects = objects;
        }
        #endregion Contructors and destructors

        #region Overrides
        protected override void OnClosing(CancelEventArgs e)
        {            
            // cancel the closing operation just hide this window out
            e.Cancel = true;
            this.Visible = false;

            base.OnClosing(e);
        }
        #endregion Overrides

        #region Events
        #endregion Events

        #region Methods
        private void ClearImageList(ImageList imageList)
        {
            if (imageList == null ||
                imageList.Images == null ||
                imageList.Images.Count == 0)
                return;

            ImageList.ImageCollection images = imageList.Images;
            for (int i = images.Count - 1; i >= 0; i--)
            {
                Image image = images[i];
                images.RemoveAt(i);

                if (image != null)
                {
                    image.Dispose();
                    image = null;
                }
            }
        }

        private void FillDataSource()
        {
            int objectCount = _objects.Count;

            


            //DetectedObject obj;
            //obj.ObjectTypeId
            //obj.RectBound
            //obj.NumPixels
            //ListViewItem[] items = new ListViewItem[objectCount];

            CommonImage srcImage = _workspace.Image;
            int smallSize = smallImageList.ImageSize.Width;
            int largeSize = largeImageList.ImageSize.Width;
            for (int i = 0; i < objectCount; i++)
            {
                DetectedObject obj = _objects[i];

                smallImageList.Images.Add(
                    CreateObjectImage(
                    srcImage, obj, smallSize, smallSize));

                largeImageList.Images.Add(
                    CreateObjectImage(
                    srcImage, obj, largeSize, largeSize));

                //string defectType =
                //    ((eDefectType)obj.ObjectTypeId).ToString();

                //ListViewItem item =
                //    new ListViewItem(defectType, i);

                //item.SubItems.Add(obj.RectBound.Left.ToString());
                //item.SubItems.Add(obj.RectBound.Top.ToString());
                //item.SubItems.Add(obj.RectBound.Width.ToString());
                //item.SubItems.Add(obj.RectBound.Height.ToString());
                //item.SubItems.Add(obj.NumPixels.ToString());

                //items[i] = item;
            }

            //objectList.Items.AddRange(items);

            objectList.SetObjects(_objects);
        }
        #endregion Methods

        #region Helpers
        private Image CreateObjectImage(
                CommonImage srcImage, DetectedObject obj, int width, int height)
        {
            Bitmap bmp =
                new Bitmap(width, height, PixelFormat.Format32bppArgb);

            RectangleF rectBound = obj.RectBound;

            int centerX = (int)(rectBound.Left + rectBound.Width * 0.5f);
            int centerY = (int)(rectBound.Top + rectBound.Height * 0.5f);

            int w = (int)rectBound.Width;
            if (w < width) w = width;
            int h = (int)rectBound.Height;
            if (h < height) h = height;

            int l = (centerX - w / 2);
            if (l < 0) l = 0;
            int t = (centerY - h / 2);
            if (t < 0) t = 0;

            Rectangle croppedRect = new Rectangle(l, t, w, h);

            using (CommonImage objImage = srcImage.CropImage(croppedRect))
            {
                using (Bitmap temp = objImage.CreateBitmap())
                {
                    using (Graphics grph = Graphics.FromImage(temp))
                    {
                        using (GraphicsPath path =
                            OverlayDefectOnABSCommand.ObjectToGraphicsPath(obj))
                        {
                            Matrix m = new Matrix();
                            m.Translate(-l, -t);
                            path.Transform(m);

                            using (Pen pen = new Pen(Color.Red, 1.0f))
                            {
                                grph.DrawPath(pen, path);
                            }
                        }
                    }

                    l = width / 2 - w / 2;
                    t = height / 2 - h / 2;

                    using (Graphics grph = Graphics.FromImage(bmp))
                    {
                        grph.Clear(Color.White);
                        grph.DrawImage(temp, l, t);
                    }
                }
            }

            return bmp;
        }
        #endregion Helpers
    }
}
