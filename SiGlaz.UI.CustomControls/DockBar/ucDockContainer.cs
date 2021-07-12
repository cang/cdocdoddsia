using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using System.Drawing.Drawing2D;

namespace SiGlaz.UI.CustomControls.DockBar
{
    public partial class ucDockContainer : ucBase, IMessageFilter
    {
        #region Members
        private int _tabSize = 26;
        private bool _hasInitData = false;
        #endregion

        #region Constructor
        public ucDockContainer()
        {
            InitializeComponent();
            
            this.Dock = DockStyle.Right;
            _ucDockbar.OnDockbarResize += new DockbarResizeHandler(_ucDockbar_OnDockbarResize);
            _ucDockbar.OnCloseDockbar += new CloseDockbarClickHandler(_ucDockbar_OnCloseDockbar);
            _ucDockbar.OnDockbarAutoHide += new DockbarAutoHideClickHandler(_ucDockbar_OnDockbarAutoHide);
            _ucDockbar.OnShowDockbar += new ShowDockbarHandler(_ucDockbar_OnShowDockbar);
            _ucDockbar.OnDockbarExpansion += new DockbarExpansionHandler(_ucDockbar_OnDockbarExpansion);
            _ucDockbar.OnDockbarCollapse += new DockbarCollapseHandler(_ucDockbar_OnDockbarCollapse);
            
            this.Disposed += new EventHandler(ucDockContainer_Disposed);
        }

        ~ucDockContainer()
        {
            Application.RemoveMessageFilter(this);
        }
        #endregion

        #region Properties
        public override DockStyle Dock
        {
            get { return base.Dock; }
            set
            {
                if (value == DockStyle.None || value == DockStyle.Fill)
                    base.Dock = DockStyle.Left;
                else
                    base.Dock = value;

                _pTab.Dock = base.Dock;
                
                _ucTabStrip.DockbarStyle = base.Dock;

                if (Dock == DockStyle.Left || Dock == DockStyle.Right)
                {
                    if (base.Dock == DockStyle.Left)
                        _pSpace2.Dock = DockStyle.Right;
                    else
                        _pSpace2.Dock = DockStyle.Left;

                    _pSpace1.Dock = DockStyle.Top;
                    _pSpace1.Height = 5;
                    _pTab.Width = _tabSize;
                    _pSpace2.Width = 3;
                    this.Width = 300;

                    _ucTabStrip.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
                    _ucTabStrip.Dock = DockStyle.Top;
                    _dockbarButton.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                    _dockbarButton.TextDirection = ToolStripTextDirection.Vertical90;
                    _dockbarButton.TextImageRelation = TextImageRelation.ImageAboveText;
                }
                else
                {
                    if (base.Dock == DockStyle.Top)
                        _pSpace2.Dock = DockStyle.Bottom;
                    else
                        _pSpace2.Dock = DockStyle.Top;

                    _pSpace1.Dock = DockStyle.Left;
                    _pSpace1.Width = 5;
                    _pTab.Height = _tabSize;
                    _pSpace2.Height = 3;
                    this.Height = 300;

                    _ucTabStrip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
                    _ucTabStrip.Dock = DockStyle.Left;
                    _dockbarButton.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                    _dockbarButton.TextDirection = ToolStripTextDirection.Horizontal;
                    _dockbarButton.TextImageRelation = TextImageRelation.ImageBeforeText;
                }

                _ucDockbar.DockbarStyle = base.Dock;
            }
        }

        public string Caption
        {
            get { return _ucDockbar.Caption; }
            set 
            { 
                _ucDockbar.Caption = value;
                _dockbarButton.Text = value;
            }
        }

        public int CurrentWidth
        {
            get { return _ucDockbar.CurrentWidth; }
            set { _ucDockbar.CurrentWidth = value; }
        }

        public int CurrentHeight
        {
            get { return _ucDockbar.CurrentHeight; }
            set { _ucDockbar.CurrentHeight = value; }
        }

        public bool IsAutoHide
        {
            get { return _ucDockbar.IsAutoHide; }
            set { _ucDockbar.IsAutoHide = value; }
        }

        public Color CaptionBackColor
        {
            get { return _ucDockbar.CaptionBackColor; }
            set
            {
                _ucDockbar.CaptionBackColor = value;
            }
        }

        public Color CaptionForeColor
        {
            get { return _ucDockbar.CaptionForeColor; }
            set { _ucDockbar.CaptionForeColor = value; }
        }

        public Panel PanelContainer
        {
            get { return _ucDockbar.PanelContainer; }
        }

        public Image ImageDockbarButton
        {
            get { return _dockbarButton.Image; }
            set  { _dockbarButton.Image = value; }
        }
        #endregion

        #region UI Command
        public void ShowDockbar()
        {
            _ucDockbar.ShowDockbar();
        }

        public Image RotateImage(Image img, float rotationAngle)
        {
            //create an empty Bitmap image
            Bitmap bmp = new Bitmap(img.Width, img.Height);

            //turn the Bitmap into a Graphics object
            Graphics gfx = Graphics.FromImage(bmp);

            //now we set the rotation point to the center of our image
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            //now rotate the image
            gfx.RotateTransform(rotationAngle);

            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            //set the InterpolationMode to HighQualityBicubic so to ensure a high
            //quality image once it is transformed to the specified size
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //now draw our new image onto the graphics object
            gfx.DrawImage(img, new Point(0, 0));

            //dispose of our Graphics object
            gfx.Dispose();

            //return the image
            return bmp;
        }

        public void InitData()
        {
            Application.AddMessageFilter(this);
            if (this.Width < _tabSize + 30) this.Width = _tabSize + 30;
            if (this.Height < _tabSize + 30) this.Height = _tabSize + 30;

            if (_ucDockbar.IsAutoHide)
            {
                _pTab.Visible = true;
                this.Width = _tabSize;
                this.Height = _tabSize;

            }
            else
            {
                _pTab.Visible = false;
                this.Width = _tabSize + _ucDockbar.CurrentWidth;
                this.Height = _tabSize + _ucDockbar.CurrentHeight;
            }

            if (Dock == DockStyle.Left || Dock == DockStyle.Right)
                _dockbarButton.Image = RotateImage(_dockbarButton.Image, 90);
        }

        public void AddControl(Control ctr, DockStyle dockStyle)
        {
            if (!_hasInitData)
            {
                InitData();
                _hasInitData = true;
            }

            _ucDockbar.PanelContainer.Controls.Add(ctr);
            ctr.Dock = dockStyle;
        }

        public void RemoveControl(Control control)
        {
            try
            {
                _ucDockbar.PanelContainer.Controls.Remove(control);
            }
            catch
            {
            }
        }
        #endregion

        #region Window Event Handler
        private void _ucDockbar_OnDockbarResize(int delta, bool isWidth)
        {
            if (isWidth)
            {
                if (_pTab.Visible)
                    this.Width = delta + _tabSize;
                else
                    this.Width = delta;
            }
            else
            {
                if (_pTab.Visible)
                    this.Height = delta + _tabSize;
                else
                    this.Height = delta;
            }
        }

        private void ucDockContainer_Load(object sender, EventArgs e)
        {
            
        }

        private void _ucDockbar_OnCloseDockbar()
        {
            _pTab.Visible = false;
            this.Width = 0;
            this.Height = 0;
            base.RaiseCloseDockbarClickEvent();
        }

        private void _ucDockbar_OnDockbarAutoHide(bool isAutoHide)
        {
            if (isAutoHide)
            {
                _pTab.Visible = true;
            }
            else
            {
                _pTab.Visible = false;

                if (Dock == DockStyle.Left || Dock == DockStyle.Right)
                    this.Width -= _tabSize;
                else
                    this.Height -= _tabSize;
            }
        }

        private void _dockbarButton_MouseHover(object sender, EventArgs e)
        {
            if (Dock == DockStyle.Left || Dock == DockStyle.Right)
            {
                if (this.Width == _tabSize)
                    _ucDockbar.Expansion();
            }
            else
            {
                if (this.Height == _tabSize)
                    _ucDockbar.Expansion();
            }
        }

        private void _ucDockbar_OnShowDockbar(bool isAutoHide)
        {
            if (isAutoHide)
            {
                _pTab.Visible = true;
                this.Width = _tabSize;
                this.Height = _tabSize;
            }
            else
            {
                _pTab.Visible = false;
                this.Width =  _ucDockbar.CurrentWidth;
                this.Height = _ucDockbar.CurrentHeight;
            }
        }

        private void _ucDockbar_OnDockbarExpansion(int delta)
        {
            if (Dock == DockStyle.Left || Dock == DockStyle.Right)
                this.Width = delta + _tabSize;
            else
                this.Height = delta + _tabSize;

            //_ucTabStrip.Refresh();
            _ucTabStrip.Invalidate();
        }

        private void _ucDockbar_OnDockbarCollapse(int delta)
        {
            if (Dock == DockStyle.Left || Dock == DockStyle.Right)
            {
                this.Width -= delta;
                if (this.Width <= _tabSize)
                    this.Width = _tabSize;
            }
            else
            {
                this.Height -= delta;
                if (this.Height <= _tabSize)
                    this.Height = _tabSize;
            }
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == Win32.WM_LBUTTONDOWN || m.Msg == Win32.WM_RBUTTONDOWN)
            {
                Point p = this.PointToClient(Control.MousePosition);

                if (p.X < 0 || p.Y < 0 || p.X > this.Width || p.Y > this.Height)
                {
                    _ucDockbar.CaptionBackColor = SystemColors.GradientInactiveCaption;

                    if (_ucDockbar.IsAutoHide && this.Width > _tabSize)
                        _ucDockbar.Collapse();
                }
                else
                    _ucDockbar.CaptionBackColor = SystemColors.GradientActiveCaption;
            }

            return false;
        }

        private void ucDockContainer_Disposed(object sender, EventArgs e)
        {
            Application.RemoveMessageFilter(this);
        }
        #endregion

        
    }
}
