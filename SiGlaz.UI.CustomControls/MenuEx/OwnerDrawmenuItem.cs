using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
namespace SiGlaz.UI.CustomControls
{
    /// <summary>
    /// Summary description for OwnerDrawmenuItem.
    /// </summary>
    public class OwnerDrawmenuItem : MenuItem
    {
        #region Fields

        #region static Fields
        private static Color window;
        private static Color backcolor;
        private static Color barcolor;
        private static Color selectioncolor;
        private static Color framecolor;
        private static Color mainMenuBackColor;
        private static LinearGradientBrush mainMenuBrush = null;

        private static int iconSize = SystemInformation.SmallIconSize.Width + 5;
        private static int itemHeight;
        private static bool doColorUpdate = false;
        private static int bitmapSize = 16;
        private static int stripeWidth = iconSize + 5;

        #endregion

        private string shortcutText = "";
        private Image image = null;

        #endregion

        #region Constructor and destructor

        public OwnerDrawmenuItem()
            : base()
        {
            this.OwnerDraw = true;
            UpdateColors();
        }

        public OwnerDrawmenuItem(string text)
            : base(text)
        {
            this.OwnerDraw = true;
            UpdateColors();
        }

        protected override void Dispose(bool disposing)
        {
            if (this.Image != null)
                this.Image.Dispose();
            this.Image = null;

            base.Dispose(disposing);
        }

        #endregion

        #region Properties

        public Image Image
        {
            get { return image; }
            set { image = value; }
        }

        public static Color SelectionColor
        {
            get { return selectioncolor; }
            set { selectioncolor = value; }
        }

        public static Color BackColor
        {
            get { return backcolor; }
            set { backcolor = value; }
        }

        public static Color BarColor
        {
            get { return barcolor; }
            set { barcolor = value; }
        }

        public static Color FrameColor
        {
            get { return framecolor; }
            set { framecolor = value; }
        }

        public static Color MainMenuBackColor
        {
            get { return mainMenuBackColor; }
            set { mainMenuBackColor = value; }
        }

        public static LinearGradientBrush MainMenuBrush
        {
            get
            {
                if (mainMenuBrush == null)
                {
                    int width = Screen.PrimaryScreen.Bounds.Width;
                    int height = 24;
                    Rectangle rect = new Rectangle(0, 0, width, height);
                    Color[] c =
                        GradientColor.GradientColorCollection[GradientColor.MainMenuBandIdx];
                    Color start = c[0];
                    //Color end = c[1];
                    Color end = c[0];
                    mainMenuBrush =
                        new LinearGradientBrush(
                            rect, start, end, LinearGradientMode.Vertical);
                }
                return mainMenuBrush;
            }

            set
            {
                if (mainMenuBrush != null)
                {
                    mainMenuBrush.Dispose();
                    mainMenuBrush = null;
                }

                mainMenuBrush = value;
            }
        }

        private static LinearGradientBrush highlightMainMenuBrush = null;
        public static LinearGradientBrush HighlightMainMenuBrush
        {
            get
            {
                if (highlightMainMenuBrush == null)
                {
                    int width = 300;
                    int height = 24;
                    Rectangle rect = new Rectangle(0, 0, width, height);
                    Color[] c =
                        GradientColor.GradientColorCollection[GradientColor.HighlightBandIdx];
                    Color start = c[0];
                    Color end = c[1];
                    highlightMainMenuBrush =
                        new LinearGradientBrush(
                            rect, start, end, LinearGradientMode.Vertical);
                }
                return highlightMainMenuBrush;
            }

            set
            {
                if (highlightMainMenuBrush != null)
                {
                    highlightMainMenuBrush.Dispose();
                    highlightMainMenuBrush = null;
                }

                highlightMainMenuBrush = value;
            }
        }

        private static LinearGradientBrush pushedMainMenuBrush = null;
        public static LinearGradientBrush PushedMainMenuBrush
        {
            get
            {
                if (pushedMainMenuBrush == null)
                {
                    int width = Screen.PrimaryScreen.Bounds.Width;
                    int height = 24;
                    Rectangle rect = new Rectangle(0, 0, width, height);
                    Color[] c =
                        GradientColor.GradientColorCollection[GradientColor.PushedBandIdx];
                    Color start = c[0];
                    Color end = c[1];
                    pushedMainMenuBrush =
                        new LinearGradientBrush(
                            rect, start, end, LinearGradientMode.Vertical);
                }
                return pushedMainMenuBrush;
            }

            set
            {
                if (pushedMainMenuBrush != null)
                {
                    pushedMainMenuBrush.Dispose();
                    pushedMainMenuBrush = null;
                }

                pushedMainMenuBrush = value;
            }
        }
        #endregion

        #region Static Methods

        private static void UpdateMenuColors()
        {
            doColorUpdate = true;
        }

        #endregion

        #region Override Methods
        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            base.OnMeasureItem(e);

            if (this.Shortcut != Shortcut.None)
            {
                string text = "";
                int key = (int)Shortcut;
                int ch = key & 0xFF;
                if (((int)Keys.Control & key) > 0) text += "Ctrl+";
                if (((int)Keys.Shift & key) > 0) text += "Shift+";
                if (((int)Keys.Alt & key) > 0) text += "Alt+";

                if (ch >= (int)Shortcut.F1 && ch <= (int)Shortcut.F12)
                    text += "F" + (ch - (int)Shortcut.F1 + 1);
                else
                {
                    if (Shortcut == Shortcut.Del)
                        text += "Del";
                    else
                        text += (char)ch;
                }
                shortcutText = text;
            }

            if (Text == "-")
            {
                e.ItemHeight = 8;
                e.ItemWidth = 4;
            }
            else
            {

                bool topLevel = Parent == Parent.GetMainMenu();
                string tempShortcutText = shortcutText;
                if (topLevel)
                    tempShortcutText = "";

                int textwidth = (int)(e.Graphics.MeasureString(Text + tempShortcutText, SystemInformation.MenuFont).Width);
                int extraHeight = 4;
                e.ItemHeight = SystemInformation.MenuHeight + extraHeight;
                if (topLevel)
                    e.ItemWidth = textwidth - 5;
                else
                    e.ItemWidth = Math.Max(160, textwidth + 50);

                itemHeight = e.ItemHeight;
            }
        }

        private bool IsTopItem()
        {
            return this.Parent == this.Parent.GetMainMenu();
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            try
            {
                if (doColorUpdate)
                    DoUpdateMenuColors();

                base.OnDrawItem(e);

                Graphics graph = e.Graphics;
                Rectangle bounds = e.Bounds;
                bool selected = (e.State & DrawItemState.Selected) > 0;
                bool toplevel = IsTopItem();
                bool hasicon = Image != null;
                bool enabled = Enabled;

                DrawBackground(graph, bounds, e.State, toplevel, hasicon, enabled);
                if (hasicon)
                    DrawIcon(graph, Image, bounds, selected, Enabled, Checked);
                else
                {
                    if (Checked)
                        DrawCheckmark(graph, bounds, selected);
                    if (RadioCheck)
                        DrawRadioCheckmark(graph, bounds, selected);
                }

                if (Text == "-")
                    DrawSeparator(graph, bounds);
                else
                    DrawMenuText(graph, bounds, Text, shortcutText, Enabled, toplevel, e.State);

                if (toplevel)
                {
                    MainMenu mainMenu = this.Parent.GetMainMenu();
                    int count = mainMenu.MenuItems.Count;
                    if (mainMenu.MenuItems[count - 1] == this)
                    {
                        int width = Screen.PrimaryScreen.Bounds.Width + 12;
                        int height = bounds.Height;
                        if (MainMenuBrush != null)
                        {
                            int x = bounds.Right - 1;
                            e.Graphics.FillRectangle(MainMenuBrush,
                                x, bounds.Top, width - x, height);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                Trace.WriteLine(exp);
            }
        }

        #endregion

        #region Internal Helpers
        private void DrawRadioCheckmark(Graphics graph, Rectangle bounds, bool selected)
        {
            int checkTop = bounds.Top + (itemHeight - bitmapSize) / 2;
            int checkLeft = bounds.Left + (stripeWidth - bitmapSize) / 2;
            ControlPaint.DrawMenuGlyph(graph, new Rectangle(checkLeft, checkTop, bitmapSize, bitmapSize), MenuGlyph.Bullet);
            graph.DrawRectangle(new Pen(framecolor), checkLeft - 1, checkTop - 1, bitmapSize + 1, bitmapSize + 1);
        }

        private void DrawCheckmark(Graphics graph, Rectangle bounds, bool selected)
        {
            int checkTop = bounds.Top + (itemHeight - bitmapSize) / 2;
            int checkLeft = bounds.Left + (stripeWidth - bitmapSize) / 2;
            ControlPaint.DrawMenuGlyph(graph, new Rectangle(checkLeft, checkTop, bitmapSize, bitmapSize), MenuGlyph.Checkmark);
            graph.DrawRectangle(new Pen(framecolor), checkLeft - 1, checkTop - 1, bitmapSize + 1, bitmapSize + 1);
        }

        private void DrawIcon(Graphics graph, Image image, Rectangle bounds, bool selected, bool enabled, bool ischecked)
        {
            int iconTop = bounds.Top + (itemHeight - bitmapSize) / 2;
            int iconLeft = bounds.Left + (stripeWidth - bitmapSize) / 2;
            if (enabled)
            {
                if (selected)
                {
                    ControlPaint.DrawImageDisabled(graph, image, iconLeft + 1, iconTop, Color.Black);
                    graph.DrawImage(image, iconLeft, iconTop - 1);
                }
                else
                {
                    graph.DrawImage(image, iconLeft + 1, iconTop);
                }
            }
            else
            {
                ControlPaint.DrawImageDisabled(graph, image, iconLeft + 1, iconTop, SystemColors.HighlightText);
            }
        }

        private void DrawSeparator(Graphics graph, Rectangle bounds)
        {
            int y = bounds.Y + bounds.Height / 2;
            graph.DrawLine(new Pen(SystemColors.ControlDark), bounds.X + iconSize + 7, y, bounds.X + bounds.Width - 2, y);
        }

        private LinearGradientBrush GetBrush(
            Rectangle bounds, int colorIdx, LinearGradientMode mode)
        {
            Color[] c = GradientColor.GradientColorCollection[colorIdx];
            return new LinearGradientBrush(bounds, c[0], c[1], mode);
        }

        private void DrawBackground(
            Graphics graph,
            Rectangle bounds, DrawItemState state,
            bool toplevel, bool hasicon, bool enabled)
        {
            bool selected = (state & DrawItemState.Selected) > 0;

            if (selected || ((state & DrawItemState.HotLight) > 0))
            {
                //if (toplevel && selected) 
                if (toplevel)
                {
                    bounds.Inflate(-1, 0);

                    //graph.FillRectangle(
                    //    new SolidBrush(barcolor), bounds);
                    if (HighlightMainMenuBrush != null)
                    {
                        graph.FillRectangle(HighlightMainMenuBrush, bounds);
                    }

                    ControlPaint.DrawBorder3D(
                        graph, bounds.Left, bounds.Top,
                        bounds.Width, bounds.Height,
                        Border3DStyle.Flat,
                        Border3DSide.Top | Border3DSide.Left | Border3DSide.Right);
                }
                else
                {
                    if (enabled)
                    {
                        graph.FillRectangle(
                            new SolidBrush(selectioncolor), bounds);

                        graph.DrawRectangle(
                            new Pen(framecolor),
                            bounds.X, bounds.Y,
                            bounds.Width - 1, bounds.Height - 1);
                    }
                    else
                    {
                        graph.FillRectangle(
                            new SolidBrush(barcolor), bounds);

                        bounds.X += stripeWidth;
                        bounds.Width -= stripeWidth;

                        graph.FillRectangle(
                            new SolidBrush(backcolor), bounds);
                    }
                }
            }
            else
            {
                if (!toplevel)
                {
                    graph.FillRectangle(
                        new SolidBrush(barcolor), bounds);

                    bounds.X += stripeWidth;
                    bounds.Width -= stripeWidth;

                    graph.FillRectangle(
                        new SolidBrush(backcolor), bounds);
                }
                else
                {
                    //graph.FillRectangle(
                    //    SystemBrushes.Control, bounds);

                    if (MainMenuBrush != null)
                    {
                        graph.FillRectangle(MainMenuBrush, bounds);
                    }
                }
            }
        }

        private void DrawMenuText(
            Graphics graph, Rectangle bounds,
            string text, string shortcut,
            bool enabled, bool toplevel, DrawItemState state)
        {
            StringFormat stringformat = new StringFormat();
            stringformat.HotkeyPrefix =
                ((state & DrawItemState.NoAccelerator) > 0) ? HotkeyPrefix.Hide : HotkeyPrefix.Show;

            if (toplevel)
            {
                int index = text.IndexOf("&");
                if (index != -1)
                    text = text.Remove(index, 1);
            }

            graph.SmoothingMode = SmoothingMode.HighQuality;

            //Font font = SystemInformation.MenuFont;
            using (Font font = new Font("Tahoma", 9, FontStyle.Regular))
            {
                int textwidth = (int)(graph.MeasureString(text, font).Width);
                int x = toplevel ? bounds.Left + (bounds.Width - textwidth) / 2 : bounds.Left + iconSize + 10;
                int topGap = 4;
                if (toplevel) topGap = 2;
                int y = bounds.Top + topGap;
                Brush brush = null;

                if (!enabled)
                    brush = new SolidBrush(SystemColors.GrayText);
                else if (toplevel)
                    brush = new SolidBrush(Color.Black);
                else
                    brush = new SolidBrush(SystemColors.MenuText);

                graph.DrawString(text, font, brush, x, y, stringformat);

                if (!toplevel)
                {
                    stringformat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                    graph.DrawString(
                        shortcut, font, brush,
                        bounds.Width - 10, bounds.Top + topGap, stringformat);
                }

                if (brush != null)
                    brush.Dispose();
                brush = null;
            }
        }

        public static void UpdateColors()
        {
            window = SystemColors.Window;
            backcolor = SystemColors.ControlLightLight;
            barcolor = SystemColors.Control;
            selectioncolor = SystemColors.Highlight;
            framecolor = SystemColors.Highlight;

            int wa = (int)window.A;
            int wr = (int)window.R;
            int wg = (int)window.G;
            int wb = (int)window.B;

            int mna = (int)backcolor.A;
            int mnr = (int)backcolor.R;
            int mng = (int)backcolor.G;
            int mnb = (int)backcolor.B;

            int sta = (int)barcolor.A;
            int str = (int)barcolor.R;
            int stg = (int)barcolor.G;
            int stb = (int)barcolor.B;

            int sla = (int)selectioncolor.A;
            int slr = (int)selectioncolor.R;
            int slg = (int)selectioncolor.G;
            int slb = (int)selectioncolor.B;

            backcolor = Color.FromArgb(wr - (((wr - mnr) * 2) / 5), wg - (((wg - mng) * 2) / 5), wb - (((wb - mnb) * 2) / 5));
            barcolor = Color.FromArgb(wr - (((wr - str) * 4) / 5), wg - (((wg - stg) * 4) / 5), wb - (((wb - stb) * 4) / 5));
            selectioncolor = Color.FromArgb(wr - (((wr - slr) * 2) / 5), wg - (((wg - slg) * 2) / 5), wb - (((wb - slb) * 2) / 5));

            barcolor =
                GradientColor.GradientColorCollection[GradientColor.MainMenuBandIdx][0];
            selectioncolor =
                GradientColor.GradientColorCollection[GradientColor.HighlightBandIdx][0];
        }

        private void DoUpdateMenuColors()
        {
            UpdateColors();
            doColorUpdate = false;
        }

        #endregion
    }
}
