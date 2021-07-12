using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.InteropServices;

namespace SiGlaz.UI.CustomControls.DockBar
{
    public class Win32
    {
        #region Const
        public const int WM_MOUSEMOVE = 0x200;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_RBUTTONDOWN = 0x204;
        public const int WM_RBUTTONUP = 0x205;
        public const int SRCCOPY = 13369376;
        #endregion

        #region Win32 Methods
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        public  static extern IntPtr GetDCEx(IntPtr hwnd, IntPtr hrgn, uint flags);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public  static extern IntPtr GetWindowText(int hWnd, StringBuilder text, int count);

        [DllImport("GDI32.DLL", CharSet = CharSet.Auto)]
        public static extern bool BitBlt(
            IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, 
            IntPtr hdcSrc, int nXSrc, int nYSrc, Int32 dwRop);
        #endregion
    }
}
