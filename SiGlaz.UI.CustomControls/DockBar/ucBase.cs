using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;

namespace SiGlaz.UI.CustomControls.DockBar
{
    #region Delegate Events
    public delegate void CloseDockbarClickHandler();
    public delegate void DockbarResizeHandler(int delta, bool isWidth);
    public delegate void DockbarAutoHideClickHandler(bool isAutoHide);
    public delegate void ShowDockbarHandler(bool isAutoHide);
    public delegate void DockbarExpansionHandler(int delta);
    public delegate void DockbarCollapseHandler(int delta);
    #endregion

    public partial class ucBase : UserControl
    {
        #region Events
        public event CloseDockbarClickHandler OnCloseDockbar;
        public event DockbarResizeHandler OnDockbarResize;
        public event DockbarAutoHideClickHandler OnDockbarAutoHide;
        public event ShowDockbarHandler OnShowDockbar;
        public event DockbarExpansionHandler OnDockbarExpansion;
        public event DockbarCollapseHandler OnDockbarCollapse;
        #endregion

        #region Constructor
        public ucBase()
        {
            InitializeComponent();
        }
        #endregion

        #region Raise Events
        public void RaiseCloseDockbarClickEvent()
        {
            if (OnCloseDockbar != null)
                OnCloseDockbar();
        }

        public void RaiseDockbarResizeEvent(int delta, bool isWidth)
        {
            if (OnDockbarResize != null)
                OnDockbarResize(delta, isWidth);
        }

        public void RaiseDockbarAutoHideEvent(bool isAutoHide)
        {
            if (OnDockbarAutoHide != null)
                OnDockbarAutoHide(isAutoHide);
        }

        public void RaiseShowDockbarEvent(bool isAutoHide)
        {
            if (OnShowDockbar != null)
                OnShowDockbar(isAutoHide);
        }

        public void RaiseDockbarExpansionEvent(int delta)
        {
            if (OnDockbarExpansion != null)
                OnDockbarExpansion(delta);
        }

        public void RaiseDockbarCollapseEvent(int delta)
        {
            if (OnDockbarCollapse != null)
                OnDockbarCollapse(delta);
        }
        #endregion
    }
}
