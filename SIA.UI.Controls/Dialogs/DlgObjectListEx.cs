using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SiGlaz.UI.CustomControls;
using System.Collections;
using SIA.Common.Analysis;
using SIA.UI.Controls.Helpers.VisualAnalysis;
using System.Drawing.Imaging;
using SiGlaz.Common.ABSDefinitions;
using SIA.SystemLayer;
using System.Drawing.Drawing2D;
using SIA.UI.Controls.Commands;

namespace SIA.UI.Controls.Dialogs
{
    public partial class DlgObjectListEx : FloatingFormBase
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
                _objects = value;
                if (_listView != null)
                {
                    _listView.Objects = _objects;
                }

                this.Invalidate(true);
            }
        }

        public ObjectAnalyzer ObjectAnalyzer
        {
            get
            {
                if (this._workspace == null)
                    return null;
                return this._workspace.GetAnalyzer("ObjectAnalyzer") as ObjectAnalyzer;
            }
        }
        #endregion Properties

        #region Contructors and destructors
        public DlgObjectListEx(ImageWorkspace workSpace, List<DetectedObject> objects)
            : base()
        {            
            InitializeComponent();

            // initialize top level window
            this.TopLevel = true;
            //this.TopMost = true;

            FadeOutEnabled = true;

            _workspace = workSpace;
            _objects = objects;
           
            // invisible this menu item due to system not supported.
            mnViewTiles.Visible = false;

            if (_listView != null)
            {
                InitializeListView();

                (_listView as ObjectListView).Initialize(_workspace, _objects);

                _listView.SelectedIndexChanged += new EventHandler(_listView_SelectedIndexChanged);
            }

            if (_workspace != null)
            {
                _workspace.DetectedObjectsChanged += 
                    new EventHandler(_workspace_DetectedObjectsChanged);
                this.Owner = _workspace.FindForm();
            }            

            tbViewMode.Text = "Details";
            tbViewMode.DropDownItemClicked += new ToolStripItemClickedEventHandler(tbViewMode_DropDownItemClicked);

            //this.SuspendLayout();
            //this.pnListView.Controls.Add(_listView);
            //this.ResumeLayout(false);
        }

        void _listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_workspace == null)
                return;

            if (_workspace.SelectedObjects != null)
                _workspace.SelectedObjects.Clear();
            else
                _workspace.SelectedObjects = new ArrayList();

            if (_listView.CahcedItems == null || _listView.CahcedItems.Count == 0)
                return;

            //ListView.SelectedListViewItemCollection items = _listView.SelectedItems;
            //if (items == null || items.Count == 0)
            //{
            //    return;
            //}

            ArrayList selectedObjects = new ArrayList();
            foreach (ListViewItem item in _listView.CahcedItems)
            {
                if (!item.Selected)
                    continue;

                selectedObjects.Add(item.Tag);
            }

            _workspace.SelectedObjects.AddRange(selectedObjects);
            _workspace.Invalidate(true);
        }
        #endregion Contructors and destructors

        #region Overrides
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            // cancel the closing operation just hide this window out
            e.Cancel = true;
            this.Visible = false;
        }
        #endregion Overrides

        #region Events
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

        void tbViewMode_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            tbViewMode.Text = e.ClickedItem.Text;
            tbViewMode.Image = e.ClickedItem.Image;

            string viewName = e.ClickedItem.Text;
            if (viewName == mnViewLargeIcons.Text)
                _listView.ViewMode = View.LargeIcon;
            else if (viewName == mnViewSmallIcons.Text)
                _listView.ViewMode = View.SmallIcon;
            else if (viewName == mnViewList.Text)
                _listView.ViewMode = View.List;
            else if (viewName == mnViewDetails.Text)
                _listView.ViewMode = View.Details;
            else if (viewName == mnViewTiles.Text)
                _listView.ViewMode = View.Tile;
        }
        #endregion Events

        #region Methods
        private void InitializeListView()
        {
            _smallImageList = new ImageList(this.components);
            _smallImageList.ImageSize =
                new Size(
                ObjectListView.DefaultSmallIconSize,
                ObjectListView.DefaultSmallIconSize);
            Image smallIcon = ObjectListView.DefaultSmallIcon;
            _smallImageList.Images.Add(smallIcon);


            _largeImageList = new ImageList(this.components);
            _largeImageList.ImageSize =
                new Size(
                ObjectListView.DefaultLargeIconSize,
                ObjectListView.DefaultLargeIconSize);
            Image largeIcon = ObjectListView.DefaultLargeIcon;
            _largeImageList.Images.Add(largeIcon);

            // update listview image lists
            _listView.SmallImageList = _smallImageList;
            _listView.LargeImageList = _largeImageList;


            _listView.TileSize =
                new Size(
                120, ObjectListView.DefaultLargeIconSize + 8);

            AddColumnHeader("Thumbnail", 160);
            AddColumnHeader("Type", 100);
            AddColumnHeader("Left", 100);
            AddColumnHeader("Top", 100);
            AddColumnHeader("Width", 100);
            AddColumnHeader("Height", 100);
            AddColumnHeader("Pixel Count", 160);

            List<int> sortingFields = new List<int>();
            sortingFields.Add(-1);
            sortingFields.Add(0);
            sortingFields.Add(1);
            sortingFields.Add(2);
            sortingFields.Add(3);
            sortingFields.Add(4);
            sortingFields.Add(5);

            _listView.SortingFields = sortingFields.ToArray();
        }

        private void AddColumnHeader(string text, int width)
        {
            ColumnHeader colHeader = new ColumnHeader();
            colHeader.Text = text;
            colHeader.Width = width;

            _listView.Columns.Add(colHeader);
        }
        #endregion Methods

        #region Helpers
        #endregion Helpers
    }    

    internal class DetectedObjectComparer : Comparer<DetectedObject>
    {
        internal enum eSortingField
        {
            None = -1,
            ObjectTypeId = 0,
            Left = 1,
            Top = 2,
            Width = 3,
            Height = 4,
            PixelCount = 5,
            Area = 6,
            Perimeter = 7,
            TotalIntensity = 8
        }

        private eSortingField _sortingField = eSortingField.None;
        private SortOrder _sortOrder = SortOrder.None;

        public DetectedObjectComparer()
        {
        }

        public DetectedObjectComparer(eSortingField sortingField)
        {
            _sortingField = sortingField;
        }

        public DetectedObjectComparer(
            eSortingField sortingField, SortOrder sortOrder)
        {
            _sortingField = sortingField;
            _sortOrder = sortOrder;
        }

        public override int Compare(DetectedObject x, DetectedObject y)
        {
            switch (_sortingField)
            {
                case eSortingField.None:
                    return 0;
                case eSortingField.ObjectTypeId:
                    return x.ObjectTypeId.CompareTo(y.ObjectTypeId);
                case eSortingField.Left:
                    return x.RectBound.Left.CompareTo(y.RectBound.Left);
                case eSortingField.Top:
                    return x.RectBound.Top.CompareTo(y.RectBound.Top);
                case eSortingField.Width:
                    return x.RectBound.Width.CompareTo(y.RectBound.Width);
                case eSortingField.Height:
                    return x.RectBound.Height.CompareTo(y.RectBound.Height);
                case eSortingField.PixelCount:
                    return x.NumPixels.CompareTo(y.NumPixels);
                case eSortingField.Area:
                    return x.Area.CompareTo(y.Area);
                case eSortingField.Perimeter:
                    return x.Perimeter.CompareTo(y.Perimeter);
                case eSortingField.TotalIntensity:
                    return x.TotalIntensity.CompareTo(y.TotalIntensity);
                default:
                    return 0;
            }
        }
    }

    internal class ObjectListView : VirtualListView
    {
        #region Default definitions
        public static int DefaultSmallIconSize = 32;
        public static int DefaultLargeIconSize = 64;

        private static Image _defaultSmallIcon = null;
        internal static Image DefaultSmallIcon
        {
            get
            {
                if (_defaultSmallIcon == null)
                {
                    _defaultSmallIcon =
                        CreateInvalidImage(
                        DefaultSmallIconSize, DefaultSmallIconSize, 
                        PixelFormat.Format32bppArgb);
                }
                return _defaultSmallIcon;
            }

            set
            {
                if (_defaultSmallIcon != value)
                {
                    if (_defaultSmallIcon != null)
                        _defaultSmallIcon.Dispose();
                    _defaultSmallIcon = value;
                }
            }
        }

        private static Image _defaultLargeIcon = null;
        internal static Image DefaultLargeIcon
        {
            get
            {
                if (_defaultLargeIcon == null)
                {
                    _defaultLargeIcon =
                        CreateInvalidImage(
                        DefaultLargeIconSize, DefaultLargeIconSize,
                        PixelFormat.Format32bppArgb);
                }
                return _defaultLargeIcon;
            }

            set
            {
                if (_defaultLargeIcon != value)
                {
                    if (_defaultLargeIcon != null)
                        _defaultLargeIcon.Dispose();
                    _defaultLargeIcon = value;
                }
            }
        }

        private static Image CreateInvalidImage(
            int width, int height, PixelFormat pxFormat)
        {
            Image image =new Bitmap(
                width, height, PixelFormat.Format32bppArgb);

            using (Graphics grph = Graphics.FromImage(image))
            {
                grph.Clear(Color.White);

                using (Pen pen = new Pen(Color.Red, 2.0f))
                {
                    grph.DrawRectangle(pen, 1, 1, width - 2, height - 2);

                    grph.DrawLine(pen, 0, 0, width - 1, height - 1);

                    grph.DrawLine(pen, width - 1, 0, 0, height - 1);
                }
            }

            return image;
        }
        #endregion Default definitions

        #region Member fields
        private int[] _sortingFields = null;
        private DetectedObjectComparer.eSortingField
            _sortingField = DetectedObjectComparer.eSortingField.None;
        private int[] _indices = null;
        private SortOrder _sortOrder = SortOrder.None;

        public int[] SortingFields
        {
            get { return _sortingFields; }
            set { _sortingFields = value; }
        }

        private View _viewType = View.Details;

        public View ViewMode
        {
            get { return _viewType; }
            set
            {
                if (_viewType != value)
                {
                    _viewType = value;

                    this.View = _viewType;
                    this.ClearCachedItems();
                }

                if (_itemController != null)
                {
                    (_itemController as ObjectItemController).ViewMode = value;
                }
            }
        }
        #endregion Member fields

        #region Constructors
        public ObjectListView() : this(null, null)
        {
        }

        public ObjectListView(
            ImageWorkspace workSpace, List<DetectedObject> objects)
            : base()
        {            
            if (workSpace == null && objects == null)
                return;

            Initialize(workSpace, objects);
        }
        #endregion Constructors

        public void Initialize(
            ImageWorkspace workSpace, List<DetectedObject> objects)
        {
            _itemController = 
                new ObjectItemController(this, workSpace, objects);

            int count = (objects != null ? objects.Count : 0);
            VirtualListSize = count;
        }

        public List<DetectedObject> Objects
        {
            get
            {
                if (_itemController != null)
                    return (_itemController as ObjectItemController).Objects;
                return null;
            }
            set
            {
                int count = (value != null ? value.Count : 0);

                if (_itemController != null)
                {
                    (_itemController as ObjectItemController).Objects = value;
                    _indices = null;
                    if (count > 0)
                    {
                        _indices = new int[count];
                        for (int i = 0; i < count; i++)
                        {
                            _indices[i] = i;
                        }
                    }

                    this.ClearCachedItems();
                }

                _sortingField = DetectedObjectComparer.eSortingField.None;
                _sortOrder = SortOrder.None;

                if (this.VirtualListSize != count)
                    this.VirtualListSize = count;
            }
        }

        private void Sort(
            DetectedObjectComparer.eSortingField sortingField)
        {
            if (_itemController != null)
            {
                DetectedObjectComparer comparison =
                    new DetectedObjectComparer(_sortingField, SortOrder.Ascending);

                DetectedObject[] objects = this.Objects.ToArray();
                Array.Sort(objects, _indices, comparison);

                this.Objects.Clear();
                this.Objects.AddRange(objects);
            }
        }

        private void Reorder()
        {
            if (_itemController != null)
            {
                this.Objects.Reverse();

                int length = _indices.Length;
                int middle = length >> 1;

                int tmp = 0;
                for (int i = length - 1, j = 0; i >= middle; i--, j++)
                {
                    tmp = _indices[i];
                    _indices[i] = _indices[j];
                    _indices[j] = tmp;
                }
            }
        }

        protected override void OnColumnClick(ColumnClickEventArgs e)
        {
            base.OnColumnClick(e);

            DetectedObjectComparer.eSortingField sortingField =
                (DetectedObjectComparer.eSortingField)_sortingFields[e.Column];

            if (sortingField == 
                DetectedObjectComparer.eSortingField.None)
                return;

            
            if (sortingField == _sortingField) // reorder
            {
                if (_sortOrder == SortOrder.None)
                    return;

                if (_sortOrder == SortOrder.Ascending)
                    _sortOrder = SortOrder.Descending;
                else if (_sortOrder == SortOrder.Descending)
                    _sortOrder = SortOrder.Ascending;

                // call reverse here
                this.Reorder();
            }
            else // need to force sorting
            {
                _sortingField = sortingField;
                _sortOrder = SortOrder.Ascending;

                this.Sort(_sortingField);
            }

            this.ClearCachedItems();
            this.Invalidate(true);
        }

        protected override void OnColumnReordered(ColumnReorderedEventArgs e)
        {
            base.OnColumnReordered(e);


        }

        #region internal class
        class ObjectItemController : IDataItemController
        {
            #region Member fields
            private ImageWorkspace _workSpace = null;
            private List<DetectedObject> _objects = null;
            private ListView _owner = null;
            private View _viewType = View.Details;

            public View ViewMode
            {
                get { return _viewType; }
                set
                {
                    if (_viewType != value)
                    {
                        _viewType = value;
                    }
                }
            }
            #endregion Member fields

            public List<DetectedObject> Objects
            {
                get { return _objects; }
                set { 
                    if (_objects != null)
                        _objects.Clear();
                    _objects = value;
                }
            }

            #region Constructors and destructors
            public ObjectItemController(
                ListView owner,
                ImageWorkspace workSpace, List<DetectedObject> objects)
            {
                _owner = owner;

                if (workSpace == null && objects == null)
                    return;

                Initialize(workSpace, objects);
            }

            public void Initialize(
                ImageWorkspace workSpace, List<DetectedObject> objects)
            {
                _workSpace = workSpace;
                int count = (objects != null ? objects.Count : 0);
                _objects = new List<DetectedObject>(count);
                if (objects != null)
                    _objects.AddRange(objects);
            }
            #endregion Constructors and destructors

            #region IDataItemController Members

            public ListViewItem Create(int idx)
            {
                DetectedObject obj = _objects[idx];

                _owner.SmallImageList.Images.Add(CreateObjectImage(
                    _workSpace.Image, obj, 
                    _owner.SmallImageList.ImageSize.Width, 
                    _owner.SmallImageList.ImageSize.Height));

                _owner.LargeImageList.Images.Add(CreateObjectImage(
                    _workSpace.Image, obj,
                    _owner.LargeImageList.ImageSize.Width,
                    _owner.LargeImageList.ImageSize.Height));

                string defectType = 
                    ((eDefectType)obj.ObjectTypeId).ToString();
                int imageIdx = _owner.SmallImageList.Images.Count - 1;
                string text = "";
                switch (_viewType)
                {                    
                    case View.SmallIcon:
                        text = string.Format("({0} x {1})",
                            obj.RectBound.Width + 1, obj.RectBound.Height + 1);
                        break;
                    case View.LargeIcon:
                        text = string.Format("Loc: ({0}, {1})\nSize: ({2} x {3})",
                            obj.RectBound.Left, obj.RectBound.Top,
                            obj.RectBound.Width + 1, obj.RectBound.Height + 1);

                        //text = string.Format(
                        //    "Type: {0}\nLocation: (x={1}, y={2})\nSize: ({3} x {4}",
                        //    defectType, obj.RectBound.Left, obj.RectBound.Top,
                        //    obj.RectBound.Width + 1, obj.RectBound.Height + 1);
                        //text = string.Format(
                        //    "{0} - (x={1}, y={2}, w={3}, h={4})",
                        //    defectType, obj.RectBound.Left, obj.RectBound.Top,
                        //    obj.RectBound.Width+1, obj.RectBound.Height+1);
                        break;
                    case View.List:
                        //text = string.Format(
                        //    "Type: {0}\nLocation: (x={1}, y={2})\nSize: ({3} x {4}",
                        //    defectType, obj.RectBound.Left, obj.RectBound.Top,
                        //    obj.RectBound.Width + 1, obj.RectBound.Height + 1);
                        text = string.Format("Loc: ({0}, {1}) - Size: ({2} x {3})",
                            obj.RectBound.Left, obj.RectBound.Top,
                            obj.RectBound.Width + 1, obj.RectBound.Height + 1);
                        break;
                    case View.Details:
                        //text = string.Format("{0}", defectType);
                        //text = string.Format("({0}, {1}) - ({2} x {3})",
                        //    obj.RectBound.Left, obj.RectBound.Top,
                        //    obj.RectBound.Width + 1, obj.RectBound.Height + 1);                        
                        break;
                }

                ListViewItem item =
                    new ListViewItem(text, imageIdx);

                item.SubItems.Add(defectType);
                item.SubItems.Add(obj.RectBound.Left.ToString());
                item.SubItems.Add(obj.RectBound.Top.ToString());
                item.SubItems.Add((obj.RectBound.Width+1).ToString());
                item.SubItems.Add((obj.RectBound.Height+1).ToString());
                item.SubItems.Add(obj.NumPixels.ToString());
                item.Tag = obj;
                return item;
            }

            public int Search(SearchForVirtualItemEventArgs e)
            {
                throw new NotImplementedException();
            }

            public void ClearImageLists()
            {
                if (_owner == null)
                    return;

                ClearImageList(_owner.SmallImageList);
                ClearImageList(_owner.LargeImageList);
            }

            private void ClearImageList(ImageList imageList)
            {
                if (imageList == null || 
                    imageList.Images == null || 
                    imageList.Images.Count == 1) // only default image
                    return;

                ImageList.ImageCollection images = imageList.Images;
                for (int i = images.Count - 1; i >= 1; i--)
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

            public void Dispose()
            {
                _workSpace = null;

                if (_objects != null)
                {
                    _objects.Clear();
                    _objects = null;

                    GC.Collect();
                }
            }

            #endregion

            #region Helpers
            private static Color[] colors = 
                new Color[] { Color.Pink, Color.Red, Color.Green, Color.Blue, Color.Yellow };
            private static int nSupportedColors = 5;

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

                                int iColor = obj.ObjectTypeId + 1;
                                if (iColor < 0 || iColor >= nSupportedColors)
                                {
                                    using (Pen pen = new Pen(Color.Red, 1.0f))
                                    {
                                        grph.DrawPath(pen, path);
                                    }
                                }
                                else
                                {
                                    using (Pen pen = new Pen(colors[iColor], 1.0f))
                                    {
                                        grph.DrawPath(pen, path);
                                    }
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
        #endregion internal class
    }
}
