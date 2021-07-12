using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;

namespace SiGlaz.UI.CustomControls.DockBar
{
    public partial class ucDockbar : ucBase
    {
        #region Members
        private Point _startPoint = new Point();
        private int _minWidth = 30;
        private int _minHeight = 30;
        private int _currentWidth = 300;
        private int _currentHeight = 200;
        private ToolTip _toolTip = new ToolTip();
        private bool _isAutoHide = true;
        private DockStyle _dockbarStyle = DockStyle.Left;
        private Pen _pen = new Pen(Color.Gray, 2);
        private Point _mousePosScreen = new Point();
        #endregion

        #region Constructor
        public ucDockbar()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
        }
        #endregion

        #region Override Methods
        public DockStyle DockbarStyle
        {
            get { return _dockbarStyle; }
            set
            {
                _dockbarStyle = value;

                if (_dockbarStyle == DockStyle.Left)
                {
                    this._splitter.Dock = DockStyle.Right;
                    this._pTitle.Dock = DockStyle.Top;
                }
                else if (_dockbarStyle == DockStyle.Right)
                {
                    this._splitter.Dock = DockStyle.Left;
                    this._pTitle.Dock = DockStyle.Top;
                }
                else if (_dockbarStyle == DockStyle.Top)
                {
                    this._splitter.Dock = DockStyle.Bottom;
                    this._pTitle.Dock = DockStyle.Bottom;
                }
                else
                {
                    this._splitter.Dock = DockStyle.Top;
                    this._pTitle.Dock = DockStyle.Top;
                }
            }
        }
        #endregion

        #region Properties
        public int CurrentWidth
        {
            get { return _currentWidth; }
            set { _currentWidth = value; }
        }

        public int CurrentHeight
        {
            get { return _currentHeight; }
            set { _currentHeight = value; }
        }

        public ucPanel PanelContainer
        {
            get { return _pContainer; }
            set { _pContainer = value; }
        }

        public string Caption
        {
            get { return _lbCaption.Text; }
            set { _lbCaption.Text = value; }
        }

        public bool IsAutoHide
        {
            get { return _isAutoHide; }
            set { _isAutoHide = value; }
        }

        public Color CaptionBackColor
        {
            get { return _pTitle.BackColor; }
            set
            {
                _pTitle.BackColor = value;
                _lbCaption.BackColor = value;
                picAutoHide.BackColor = value;
                picClose.BackColor = value;
            }
        }

        public Color CaptionForeColor
        {
            get { return _lbCaption.ForeColor; }
            set { _lbCaption.ForeColor = value; }
        }
        #endregion

        #region UI Command
        public void ShowDockbar()
        {
            if (!_isAutoHide)
                this.Visible = true;

            base.RaiseShowDockbarEvent(_isAutoHide);
        }

        public void HideDockbar()
        {
            this.Visible = false;
        }

        public void Collapse()
        {
            int size = 0;
            if (_dockbarStyle == DockStyle.Left || _dockbarStyle == DockStyle.Right)
                size = this.Width;
            else
                size = this.Height;

            //for (int i = size; i >= 0; i -= 5)
            //{
            //    base.RaiseDockbarCollapseEvent(5);
            //}

            base.RaiseDockbarCollapseEvent(size);

            //this.Visible = false;
        }

        public void Expansion()
        {
            this.Visible = true;

            if (_dockbarStyle == DockStyle.Left || _dockbarStyle == DockStyle.Right)
            {
                base.RaiseDockbarExpansionEvent(_currentWidth);
                //for (int i = _minWidth; i <= _currentWidth; i += 5)
                //{
                //    base.RaiseDockbarExpansionEvent(i);
                //    //this.Refresh();
                //    this.Invalidate();
                //}
            }
            else
            {
                base.RaiseDockbarExpansionEvent(_currentHeight);
                //for (int i = _minHeight; i <= _currentHeight; i += 5)
                //{
                //    base.RaiseDockbarExpansionEvent(i);
                //    //this.Refresh();
                //    this.Invalidate();
                //}
            }
        }

        private void DrawLine()
        {
            IntPtr activeWindowHandle = Win32.GetForegroundWindow();
            if (activeWindowHandle != null && activeWindowHandle.ToInt32() != 0)
            {
                IntPtr hdc = Win32.GetDCEx(activeWindowHandle, IntPtr.Zero, 1027);
                if (hdc != null && hdc.ToInt32() != 0)
                {
                    Control activeWindow = Control.FromHandle(activeWindowHandle);
                    Point mousePos = activeWindow.PointToClient(Control.MousePosition);
                    mousePos.X += 5;

                    _mousePosScreen = Control.MousePosition;

                    Point p1 = new Point(0, 0);
                    Point p2 = new Point(this.Width, this.Height);

                    p1 = this.PointToScreen(p1);
                    p2 = this.PointToScreen(p2);

                    p1 = activeWindow.PointToClient(p1);
                    p2 = activeWindow.PointToClient(p2);

                    int deltaX = activeWindow.Size.Width - activeWindow.ClientSize.Width;
                    int deltaY = activeWindow.Size.Height - activeWindow.ClientSize.Height;

                    int x = 0;
                    int y = 0;

                    activeWindow.Refresh();

                    using (Graphics g = Graphics.FromHdc(hdc))
                    {
                        Point minPoint = Point.Empty;
                        Point minPointScreen = Point.Empty;

                        switch (_dockbarStyle)
                        {
                            case DockStyle.Left:
                                if (mousePos.X > activeWindow.ClientSize.Width - _minWidth)
                                {
                                    mousePos.X = activeWindow.ClientSize.Width - _minWidth;
                                    _mousePosScreen = activeWindow.PointToScreen(mousePos);
                                }

                                minPoint.X = _minWidth;
                                minPointScreen = this.PointToScreen(minPoint);
                                minPoint = activeWindow.PointToClient(minPointScreen);

                                if (mousePos.X < minPoint.X)
                                {
                                    mousePos.X = minPoint.X;
                                    _mousePosScreen.X = minPointScreen.X;
                                }

                                x = mousePos.X;
                                y = p1.Y + deltaY - activeWindow.Margin.Top;

                                g.DrawLine(_pen, x, y, x, y + this.Height - 1);
                                break;

                            case DockStyle.Right:
                                if (mousePos.X < _minWidth)
                                {
                                    mousePos.X = _minWidth - 5;
                                    _mousePosScreen = activeWindow.PointToScreen(mousePos);
                                }

                                minPoint.X = this.Width - (_minWidth - 5);
                                minPointScreen = this.PointToScreen(minPoint);
                                minPoint = activeWindow.PointToClient(minPointScreen);

                                if (mousePos.X > minPoint.X)
                                {
                                    mousePos.X = minPoint.X;
                                    _mousePosScreen.X = minPointScreen.X;
                                }

                                x = mousePos.X;
                                y = p1.Y + deltaY - activeWindow.Margin.Top;

                                g.DrawLine(_pen, x, y, x, y + this.Height - 1);
                                break;

                            case DockStyle.Bottom:
                                if (mousePos.Y < _minHeight + deltaY - activeWindow.Margin.Bottom)
                                {
                                    mousePos.Y = _minHeight + deltaY - activeWindow.Margin.Bottom;
                                    _mousePosScreen = activeWindow.PointToScreen(mousePos);
                                }

                                minPoint.Y = this.Height - (_minHeight + _pTitle.Height + _splitter.Height);
                                minPointScreen = this.PointToScreen(minPoint);
                                minPoint = activeWindow.PointToClient(minPointScreen);

                                if (mousePos.Y > minPoint.Y)
                                {
                                    mousePos.Y = minPoint.Y;
                                    _mousePosScreen.Y = minPointScreen.Y;
                                }

                                x = p1.X + deltaX - activeWindow.Margin.Left;
                                y = mousePos.Y + deltaY - activeWindow.Margin.Top;

                                g.DrawLine(_pen, x, y, x + this.Width - 1, y);
                                break;

                            case DockStyle.Top:
                                if (mousePos.Y > activeWindow.ClientSize.Height - _minHeight)
                                {
                                    mousePos.Y = activeWindow.ClientSize.Height - _minHeight;
                                    _mousePosScreen = activeWindow.PointToScreen(mousePos);
                                }

                                minPoint.Y = _minHeight + _pTitle.Height + _splitter.Height;
                                minPointScreen = this.PointToScreen(minPoint);
                                minPoint = activeWindow.PointToClient(minPointScreen);

                                if (mousePos.Y < minPoint.Y)
                                {
                                    mousePos.Y = minPoint.Y;
                                    _mousePosScreen.Y = minPointScreen.Y;
                                }

                                x = p1.X + deltaX - activeWindow.Margin.Left;
                                y = mousePos.Y + deltaY - activeWindow.Margin.Top;

                                g.DrawLine(_pen, x, y, x + this.Width - 1, y);
                                break;
                        }
                    }
                }
            }
        }
        #endregion

        #region Window Event Handlers
        private void _splitter_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _startPoint.X = e.X;
                _startPoint.Y = e.Y;

                DrawLine();
            }
        }

        private void _splitter_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            DrawLine();
        }

        private void _splitter_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            Point p = _splitter.PointToClient(_mousePosScreen);

            int delta = 0;
            switch (_dockbarStyle)
            {
                case DockStyle.Left:
                    delta = p.X - _startPoint.X;
                    if (this.Width + delta >= _minWidth)
                        base.RaiseDockbarResizeEvent(this.Width + delta, true);
                    else
                        base.RaiseDockbarResizeEvent(_minWidth, true);
                    break;

                case DockStyle.Right:
                    delta = _startPoint.X - p.X;
                    if (this.Width + delta >= _minWidth)
                        base.RaiseDockbarResizeEvent(this.Width + delta, true);
                    else
                        base.RaiseDockbarResizeEvent(_minWidth , true);
                    break;

                case DockStyle.Top:
                    delta = p.Y - _startPoint.Y;
                    if (this.Height + delta >= _minHeight + _pTitle.Height + _splitter.Height)
                        base.RaiseDockbarResizeEvent(this.Height + delta, false);
                    else
                        base.RaiseDockbarResizeEvent(_minHeight + _pTitle.Height + _splitter.Height, false);
                    break;

                case DockStyle.Bottom:
                    delta = _startPoint.Y - p.Y;
                    if (this.Height + delta >= _minHeight + _pTitle.Height + _splitter.Height)
                        base.RaiseDockbarResizeEvent(this.Height + delta, false);
                    else
                        base.RaiseDockbarResizeEvent(_minHeight + _pTitle.Height + _splitter.Height, false);
                    break;
            }

            IntPtr activeWindowHandle = Win32.GetForegroundWindow();
            if (activeWindowHandle != null && activeWindowHandle.ToInt32() != 0)
            {
                Control activeWindow = Control.FromHandle(activeWindowHandle);
                if (activeWindow != null)
                    activeWindow.Invalidate(true);
            }

            _currentWidth = this.Width;
            _currentHeight = this.Height;
        }

        private void picAutoHide_MouseHover(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Hand;

            if (_isAutoHide)
                picAutoHide.Image = ImageResx.auto_hide_over_1;
            else
                picAutoHide.Image = ImageResx.auto_hide_over;
        }

        private void picAutoHide_MouseLeave(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Default;
            if (_isAutoHide)
                picAutoHide.Image = ImageResx.auto_hide1;
            else
                picAutoHide.Image = ImageResx.auto_hide;

            _toolTip.RemoveAll();
            _toolTip.Hide(picAutoHide);
        }

        private void picClose_MouseHover(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Hand;
            picClose.Image = ImageResx.close_over;
        }

        private void picClose_MouseLeave(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Default;
            picClose.Image = ImageResx.close;
            _toolTip.RemoveAll();
            _toolTip.Hide(picClose);
        }

        private void picClose_MouseClick(object sender, MouseEventArgs e)
        {
            HideDockbar();
            base.RaiseCloseDockbarClickEvent();
        }

        private void picAutoHide_MouseMove(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.Hand;
            _toolTip.Show("Auto Hide", picAutoHide, e.X - 40, e.Y + 23, 5000);
        }

        private void picClose_MouseMove(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.Hand;
            _toolTip.Show("Close", picClose, e.X - 40, e.Y + 23);
        }

        private void picAutoHide_MouseClick(object sender, MouseEventArgs e)
        {
            if (_isAutoHide)
            {
                _isAutoHide = false;
                picAutoHide.Image = ImageResx.auto_hide_over;
            }
            else
            {
                _isAutoHide = true;
                picAutoHide.Image = ImageResx.auto_hide_over_1;
                Collapse();
            }

            base.RaiseDockbarAutoHideEvent(_isAutoHide);
        }

        private void ucDockbar_Load(object sender, EventArgs e)
        {
            if (!_isAutoHide)
                picAutoHide.Image = ImageResx.auto_hide;
        }
        #endregion
    }
}
