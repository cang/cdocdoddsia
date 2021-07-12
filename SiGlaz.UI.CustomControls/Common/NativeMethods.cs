using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace SiGlaz.UI.CustomControls.Common
{
	public class NativeMethods
	{
		#region Errors

		[DllImport("kernel32.dll", EntryPoint = "GetLastError")]
		public static extern int Error_GetLast();
		
		#endregion
		
		#region Modules

		[DllImport("kernel32.dll", EntryPoint = "LoadLibraryExW")]
		public static extern IntPtr Module_Load(
			[In] [MarshalAs(UnmanagedType.LPWStr)] string filename,
			[In] IntPtr reserved,
			[In] int flags);

		[DllImport("kernel32.dll", EntryPoint = "GetModuleHandle")]
		public static extern IntPtr Module_HandleGet(
			[In] [MarshalAs(UnmanagedType.LPWStr)] string filename);

		[DllImport("kernel32.dll", EntryPoint = "FreeLibrary")]
		public static extern bool Module_Unload(
			[In] IntPtr hModule);

		#endregion

		#region Resources

		#region Constants

		public enum ResourceType : int
		{
			Cursor        =  1,
			Bitmap        =  2,
			Icon          =  3,
			Menu          =  4,
			Dialog        =  5,
			String        =  6,
			FontDir       =  7,
			Font          =  8,
			Accelerator   =  9,
			RCData        = 10,
			MessageTable  = 11,
			CursorGroup   = 12,
			IconGroup     = 14,
			Version       = 16,
			DialogInclude = 17,
			PlugPlay      = 19,
			Vxd           = 20,
			AniCursor     = 21,
			AniIcon       = 22,
			Html          = 23,
			Manifest      = 24,
			File          = 2110
		}

		public enum ImageType : int
		{
			Bitmap = 0,
			Icon = 1,
			Cursor = 2
		}

		[Flags]
		public enum ImageLoadFlags : int
		{
			DefaultColor     = 0x0000,
			Monochrome       = 0x0001,
			Color            = 0x0002,
			CopyReturnOrg    = 0x0004,
			CopyDeleteOrg    = 0x0008,
			LoadFromFile     = 0x0010,
			LoadTransparent  = 0x0020,
			DefaultSize      = 0x0040,
			VgaColor         = 0x0080,
			LoadMap3DColors  = 0x1000,
			CreateDibSection = 0x2000,
			CopyFromResource = 0x4000,
			Shared           = 0x8000
		}

		#endregion

		#region Methods

		[DllImport("kernel32.dll", EntryPoint = "FindResourceEx")]
		public static extern IntPtr Resource_Find(
			[In] IntPtr hModule,
			[In][MarshalAs(UnmanagedType.U4)] ResourceType resourceType,
			[In][MarshalAs(UnmanagedType.LPTStr)] string resourceName,
			[In] short resourceLanguage);

		[DllImport("kernel32.dll", EntryPoint = "FindResourceEx")]
		public static extern IntPtr Resource_Find(
			[In] IntPtr hModule,
			[In][MarshalAs(UnmanagedType.U4)] ResourceType resourceType,
			[In] int resourceID,
			[In] short resourceLanguage);

		[DllImport("kernel32.dll", EntryPoint = "LoadResource")]
		public static extern IntPtr Resource_Load(
			[In] IntPtr hModule,
			[In] IntPtr hResourceInfo);

		[DllImport("kernel32.dll", EntryPoint = "LockResource")]
		public static extern IntPtr Resource_Lock(
			[In] IntPtr hResource);

		[DllImport("kernel32.dll", EntryPoint = "SizeofResource")]
		public static extern int Resource_Size(
			[In] IntPtr hModule,
			[In] IntPtr hResource);
		
		[DllImport("user32.dll", EntryPoint = "LoadString")]
		public static extern int String_Load(
			[In] IntPtr hModule,
			[In] int id,
			[Out] StringBuilder buffer,
			[In] int bufferSize);

		[DllImport("user32.dll", EntryPoint = "LoadImage")]
		public static extern IntPtr Image_Load(
			[In] IntPtr hModule,
			[In] int imageID,
			[In][MarshalAs(UnmanagedType.U4)] ImageType imageType,
			[In] int imageWidth,
			[In] int imageHeight,
			[In][MarshalAs(UnmanagedType.U4)] ImageLoadFlags flags);

		[DllImport("user32.dll", EntryPoint = "LoadImage")]
		public static extern IntPtr Image_Load(
			[In] IntPtr hModule,
			[In][MarshalAs(UnmanagedType.LPTStr)] string imageName,
			[In][MarshalAs(UnmanagedType.U4)] ImageType imageType,
			[In] int imageWidth,
			[In] int imageHeight,
			[In][MarshalAs(UnmanagedType.U4)] ImageLoadFlags flags);

		[DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
		public static extern bool Bitmap_Delete(
			[In] IntPtr hBitmap);

		[DllImport("user32.dll", EntryPoint = "DestroyIcon")]
		public static extern bool Icon_Delete(
			[In] IntPtr hIcon);

		[DllImport("user32.dll", EntryPoint = "DestroyCursor")]
		public static extern bool Cursor_Delete(
			[In] IntPtr hCursor);

		#endregion

		#endregion

		#region Windows

		#region Constants

		#region Window Messages

		#region General

		public const int WM_NULL = 0x0000;
		public const int WM_CREATE = 0x0001;
		public const int WM_DESTROY = 0x0002;
		public const int WM_MOVE = 0x0003;
		public const int WM_SIZE = 0x0005;
		public const int WM_ACTIVATE = 0x0006;
		public const int WM_SETFOCUS = 0x0007;
		public const int WM_KILLFOCUS = 0x0008;
		public const int WM_ENABLE = 0x000A;
		public const int WM_SETREDRAW = 0x000B;
		public const int WM_SETTEXT = 0x000C;
		public const int WM_GETTEXT = 0x000D;
		public const int WM_GETTEXTLENGTH = 0x000E;
		public const int WM_PAINT = 0x000F;
		public const int WM_CLOSE = 0x0010;
		public const int WM_QUERYENDSESSION = 0x0011;
		public const int WM_QUERYOPEN = 0x0013;
		public const int WM_ENDSESSION = 0x0016;
		public const int WM_QUIT = 0x0012;
		public const int WM_ERASEBKGND = 0x0014;
		public const int WM_SYSCOLORCHANGE = 0x0015;
		public const int WM_SHOWWINDOW = 0x0018;
		public const int WM_WININICHANGE = 0x001A;
		public const int WM_SETTINGCHANGE = WM_WININICHANGE;
		public const int WM_DEVMODECHANGE = 0x001B;
		public const int WM_ACTIVATEAPP = 0x001C;
		public const int WM_FONTCHANGE = 0x001D;
		public const int WM_TIMECHANGE = 0x001E;
		public const int WM_CANCELMODE = 0x001F;
		public const int WM_SETCURSOR = 0x0020;
		public const int WM_MOUSEACTIVATE = 0x0021;
		public const int WM_CHILDACTIVATE = 0x0022;
		public const int WM_QUEUESYNC = 0x0023;
		public const int WM_GETMINMAXINFO = 0x0024;
		public const int WM_PAINTICON = 0x0026;
		public const int WM_ICONERASEBKGND = 0x0027;
		public const int WM_NEXTDLGCTL = 0x0028;
		public const int WM_SPOOLERSTATUS = 0x002A;
		public const int WM_DRAWITEM = 0x002B;
		public const int WM_MEASUREITEM = 0x002C;
		public const int WM_DELETEITEM = 0x002D;
		public const int WM_VKEYTOITEM = 0x002E;
		public const int WM_CHARTOITEM = 0x002F;
		public const int WM_SETFONT = 0x0030;
		public const int WM_GETFONT = 0x0031;
		public const int WM_SETHOTKEY = 0x0032;
		public const int WM_GETHOTKEY = 0x0033;
		public const int WM_QUERYDRAGICON = 0x0037;
		public const int WM_COMPAREITEM = 0x0039;
		public const int WM_GETOBJECT = 0x003D;
		public const int WM_COMPACTING = 0x0041;
		public const int WM_COMMNOTIFY = 0x0044;
		public const int WM_WINDOWPOSCHANGING = 0x0046;
		public const int WM_WINDOWPOSCHANGED = 0x0047;
		public const int WM_POWER = 0x0048;
		public const int WM_COPYDATA = 0x004A;
		public const int WM_CANCELJOURNAL = 0x004B;
		public const int WM_NOTIFY = 0x004E;
		public const int WM_INPUTLANGCHANGEREQUEST = 0x0050;
		public const int WM_INPUTLANGCHANGE = 0x0051;
		public const int WM_TCARD = 0x0052;
		public const int WM_HELP = 0x0053;
		public const int WM_USERCHANGED = 0x0054;
		public const int WM_NOTIFYFORMAT = 0x0055;
		public const int WM_CONTEXTMENU = 0x007B;
		public const int WM_STYLECHANGING = 0x007C;
		public const int WM_STYLECHANGED = 0x007D;
		public const int WM_DISPLAYCHANGE = 0x007E;
		public const int WM_GETICON = 0x007F;
		public const int WM_SETICON = 0x0080;
		public const int WM_NCCREATE = 0x0081;
		public const int WM_NCDESTROY = 0x0082;
		public const int WM_NCCALCSIZE = 0x0083;
		public const int WM_NCHITTEST = 0x0084;
		public const int WM_NCPAINT = 0x0085;
		public const int WM_NCACTIVATE = 0x0086;
		public const int WM_GETDLGCODE = 0x0087;
		public const int WM_SYNCPAINT = 0x0088;
		public const int WM_NCMOUSEMOVE = 0x00A0;
		public const int WM_NCLBUTTONDOWN = 0x00A1;
		public const int WM_NCLBUTTONUP = 0x00A2;
		public const int WM_NCLBUTTONDBLCLK = 0x00A3;
		public const int WM_NCRBUTTONDOWN = 0x00A4;
		public const int WM_NCRBUTTONUP = 0x00A5;
		public const int WM_NCRBUTTONDBLCLK = 0x00A6;
		public const int WM_NCMBUTTONDOWN = 0x00A7;
		public const int WM_NCMBUTTONUP = 0x00A8;
		public const int WM_NCMBUTTONDBLCLK = 0x00A9;
		public const int WM_NCXBUTTONDOWN = 0x00AB;
		public const int WM_NCXBUTTONUP = 0x00AC;
		public const int WM_NCXBUTTONDBLCLK = 0x00AD;
		public const int WM_INPUT = 0x00FF;
		public const int WM_KEYFIRST = 0x0100;
		public const int WM_KEYDOWN = 0x0100;
		public const int WM_KEYUP = 0x0101;
		public const int WM_CHAR = 0x0102;
		public const int WM_DEADCHAR = 0x0103;
		public const int WM_SYSKEYDOWN = 0x0104;
		public const int WM_SYSKEYUP = 0x0105;
		public const int WM_SYSCHAR = 0x0106;
		public const int WM_SYSDEADCHAR = 0x0107;
		public const int WM_UNICHAR = 0x0109;
		public const int WM_KEYLAST = 0x0109;
		public const int WM_IME_STARTCOMPOSITION = 0x010D;
		public const int WM_IME_ENDCOMPOSITION = 0x010E;
		public const int WM_IME_COMPOSITION = 0x010F;
		public const int WM_IME_KEYLAST = 0x010F;
		public const int WM_INITDIALOG = 0x0110;
		public const int WM_COMMAND = 0x0111;
		public const int WM_SYSCOMMAND = 0x0112;
		public const int WM_TIMER = 0x0113;
		public const int WM_HSCROLL = 0x0114;
		public const int WM_VSCROLL = 0x0115;
		public const int WM_INITMENU = 0x0116;
		public const int WM_INITMENUPOPUP = 0x0117;
		public const int WM_MENUSELECT = 0x011F;
		public const int WM_MENUCHAR = 0x0120;
		public const int WM_ENTERIDLE = 0x0121;
		public const int WM_MENURBUTTONUP = 0x0122;
		public const int WM_MENUDRAG = 0x0123;
		public const int WM_MENUGETOBJECT = 0x0124;
		public const int WM_UNINITMENUPOPUP = 0x0125;
		public const int WM_MENUCOMMAND = 0x0126;
		public const int WM_CHANGEUISTATE = 0x0127;
		public const int WM_UPDATEUISTATE = 0x0128;
		public const int WM_QUERYUISTATE = 0x0129;
		public const int WM_CTLCOLORMSGBOX = 0x0132;
		public const int WM_CTLCOLOREDIT = 0x0133;
		public const int WM_CTLCOLORLISTBOX = 0x0134;
		public const int WM_CTLCOLORBTN = 0x0135;
		public const int WM_CTLCOLORDLG = 0x0136;
		public const int WM_CTLCOLORSCROLLBAR = 0x0137;
		public const int WM_CTLCOLORSTATIC = 0x0138;
		public const int MN_GETHMENU = 0x01E1;
		public const int WM_MOUSEFIRST = 0x0200;
		public const int WM_MOUSEMOVE = 0x0200;
		public const int WM_LBUTTONDOWN = 0x0201;
		public const int WM_LBUTTONUP = 0x0202;
		public const int WM_LBUTTONDBLCLK = 0x0203;
		public const int WM_RBUTTONDOWN = 0x0204;
		public const int WM_RBUTTONUP = 0x0205;
		public const int WM_RBUTTONDBLCLK = 0x0206;
		public const int WM_MBUTTONDOWN = 0x0207;
		public const int WM_MBUTTONUP = 0x0208;
		public const int WM_MBUTTONDBLCLK = 0x0209;
		public const int WM_MOUSEWHEEL = 0x020A;
		public const int WM_XBUTTONDOWN = 0x020B;
		public const int WM_XBUTTONUP = 0x020C;
		public const int WM_XBUTTONDBLCLK = 0x020D;
		public const int WM_MOUSELAST = 0x020D;
		public const int WM_PARENTNOTIFY = 0x0210;
		public const int WM_ENTERMENULOOP = 0x0211;
		public const int WM_EXITMENULOOP = 0x0212;
		public const int WM_NEXTMENU = 0x0213;
		public const int WM_SIZING = 0x0214;
		public const int WM_CAPTURECHANGED = 0x0215;
		public const int WM_MOVING = 0x0216;
		public const int WM_POWERBROADCAST = 0x0218;
		public const int WM_DEVICECHANGE = 0x0219;
		public const int WM_MDICREATE = 0x0220;
		public const int WM_MDIDESTROY = 0x0221;
		public const int WM_MDIACTIVATE = 0x0222;
		public const int WM_MDIRESTORE = 0x0223;
		public const int WM_MDINEXT = 0x0224;
		public const int WM_MDIMAXIMIZE = 0x0225;
		public const int WM_MDITILE = 0x0226;
		public const int WM_MDICASCADE = 0x0227;
		public const int WM_MDIICONARRANGE = 0x0228;
		public const int WM_MDIGETACTIVE = 0x0229;
		public const int WM_MDISETMENU = 0x0230;
		public const int WM_ENTERSIZEMOVE = 0x0231;
		public const int WM_EXITSIZEMOVE = 0x0232;
		public const int WM_DROPFILES = 0x0233;
		public const int WM_MDIREFRESHMENU = 0x0234;
		public const int WM_IME_SETCONTEXT = 0x0281;
		public const int WM_IME_NOTIFY = 0x0282;
		public const int WM_IME_CONTROL = 0x0283;
		public const int WM_IME_COMPOSITIONFULL = 0x0284;
		public const int WM_IME_SELECT = 0x0285;
		public const int WM_IME_CHAR = 0x0286;
		public const int WM_IME_REQUEST = 0x0288;
		public const int WM_IME_KEYDOWN = 0x0290;
		public const int WM_IME_KEYUP = 0x0291;
		public const int WM_MOUSEHOVER = 0x02A1;
		public const int WM_MOUSELEAVE = 0x02A3;
		public const int WM_NCMOUSEHOVER = 0x02A0;
		public const int WM_NCMOUSELEAVE = 0x02A2;
		public const int WM_WTSSESSION_CHANGE = 0x02B1;
		public const int WM_TABLET_FIRST = 0x02c0;
		public const int WM_TABLET_LAST = 0x02df;
		public const int WM_CUT = 0x0300;
		public const int WM_COPY = 0x0301;
		public const int WM_PASTE = 0x0302;
		public const int WM_CLEAR = 0x0303;
		public const int WM_UNDO = 0x0304;
		public const int WM_RENDERFORMAT = 0x0305;
		public const int WM_RENDERALLFORMATS = 0x0306;
		public const int WM_DESTROYCLIPBOARD = 0x0307;
		public const int WM_DRAWCLIPBOARD = 0x0308;
		public const int WM_PAINTCLIPBOARD = 0x0309;
		public const int WM_VSCROLLCLIPBOARD = 0x030A;
		public const int WM_SIZECLIPBOARD = 0x030B;
		public const int WM_ASKCBFORMATNAME = 0x030C;
		public const int WM_CHANGECBCHAIN = 0x030D;
		public const int WM_HSCROLLCLIPBOARD = 0x030E;
		public const int WM_QUERYNEWPALETTE = 0x030F;
		public const int WM_PALETTEISCHANGING = 0x0310;
		public const int WM_PALETTECHANGED = 0x0311;
		public const int WM_HOTKEY = 0x0312;
		public const int WM_PRINT = 0x0317;
		public const int WM_PRINTCLIENT = 0x0318;
		public const int WM_APPCOMMAND = 0x0319;
		public const int WM_THEMECHANGED = 0x031A;
		public const int WM_HANDHELDFIRST = 0x0358;
		public const int WM_HANDHELDLAST = 0x035F;
		public const int WM_AFXFIRST = 0x0360;
		public const int WM_AFXLAST = 0x037F;
		public const int WM_PENWINFIRST = 0x0380;
		public const int WM_PENWINLAST = 0x038F;
		public const int WM_USER = 0x0400;
		public const int WM_APP = 0x8000;

		#endregion

		#region Edit/RichEdit

		public const int EM_GETSEL                = 0x00B0;
		public const int EM_SETSEL                = 0x00B1;
		public const int EM_GETRECT               = 0x00B2;
		public const int EM_SETRECT               = 0x00B3;
		public const int EM_SETRECTNP             = 0x00B4;
		public const int EM_SCROLL                = 0x00B5;
		public const int EM_LINESCROLL            = 0x00B6;
		public const int EM_SCROLLCARET           = 0x00B7;
		public const int EM_GETMODIFY             = 0x00B8;
		public const int EM_SETMODIFY             = 0x00B9;
		public const int EM_GETLINECOUNT          = 0x00BA;
		public const int EM_LINEINDEX             = 0x00BB;
		public const int EM_SETHANDLE             = 0x00BC;
		public const int EM_GETHANDLE             = 0x00BD;
		public const int EM_GETTHUMB              = 0x00BE;
		public const int EM_LINELENGTH            = 0x00C1;
		public const int EM_REPLACESEL            = 0x00C2;
		public const int EM_GETLINE               = 0x00C4;
		public const int EM_LIMITTEXT             = 0x00C5;
		public const int EM_CANUNDO               = 0x00C6;
		public const int EM_UNDO                  = 0x00C7;
		public const int EM_FMTLINES              = 0x00C8;
		public const int EM_LINEFROMCHAR          = 0x00C9;
		public const int EM_SETTABSTOPS           = 0x00CB;
		public const int EM_SETPASSWORDCHAR       = 0x00CC;
		public const int EM_EMPTYUNDOBUFFER       = 0x00CD;
		public const int EM_GETFIRSTVISIBLELINE   = 0x00CE;
		public const int EM_SETREADONLY           = 0x00CF;
		public const int EM_SETWORDBREAKPROC      = 0x00D0;
		public const int EM_GETWORDBREAKPROC      = 0x00D1;
		public const int EM_GETPASSWORDCHAR       = 0x00D2;
		public const int EM_SETMARGINS            = 0x00D3;
		public const int EM_GETMARGINS            = 0x00D4;
		public const int EM_SETLIMITTEXT          = EM_LIMITTEXT;
		public const int EM_GETLIMITTEXT          = 0x00D5;
		public const int EM_POSFROMCHAR           = 0x00D6;
		public const int EM_CHARFROMPOS           = 0x00D7;
		public const int EM_SETIMESTATUS          = 0x00D8;
		public const int EM_GETIMESTATUS          = 0x00D9;

		//public const int EM_GETLIMITTEXT          = WM_USER +  37;
		//public const int EM_POSFROMCHAR           = WM_USER +  38;
		//public const int EM_CHARFROMPOS           = WM_USER +  39;
		//public const int EM_SCROLLCARET           = WM_USER +  49;
		public const int EM_CANPASTE              = WM_USER +  50;
		public const int EM_DISPLAYBAND           = WM_USER +  51;
		public const int EM_EXGETSEL              = WM_USER +  52;
		public const int EM_EXLIMITTEXT           = WM_USER +  53;
		public const int EM_EXLINEFROMCHAR        = WM_USER +  54;
		public const int EM_EXSETSEL              = WM_USER +  55;
		public const int EM_FINDTEXT              = WM_USER +  56;
		public const int EM_FORMATRANGE           = WM_USER +  57;
		public const int EM_GETCHARFORMAT         = WM_USER +  58;
		public const int EM_GETEVENTMASK          = WM_USER +  59;
		public const int EM_GETOLEINTERFACE       = WM_USER +  60;
		public const int EM_GETPARAFORMAT         = WM_USER +  61;
		public const int EM_GETSELTEXT            = WM_USER +  62;
		public const int EM_HIDESELECTION         = WM_USER +  63;
		public const int EM_PASTESPECIAL          = WM_USER +  64;
		public const int EM_REQUESTRESIZE         = WM_USER +  65;
		public const int EM_SELECTIONTYPE         = WM_USER +  66;
		public const int EM_SETBKGNDCOLOR         = WM_USER +  67;
		public const int EM_SETCHARFORMAT         = WM_USER +  68;
		public const int EM_SETEVENTMASK          = WM_USER +  69;
		public const int EM_SETOLECALLBACK        = WM_USER +  70;
		public const int EM_SETPARAFORMAT         = WM_USER +  71;
		public const int EM_SETTARGETDEVICE       = WM_USER +  72;
		public const int EM_STREAMIN              = WM_USER +  73;
		public const int EM_STREAMOUT             = WM_USER +  74;
		public const int EM_GETTEXTRANGE          = WM_USER +  75;
		public const int EM_FINDWORDBREAK         = WM_USER +  76;
		public const int EM_SETOPTIONS            = WM_USER +  77;
		public const int EM_GETOPTIONS            = WM_USER +  78;
		public const int EM_FINDTEXTEX            = WM_USER +  79;
		public const int EM_GETWORDBREAKPROCEX    = WM_USER +  80;
		public const int EM_SETWORDBREAKPROCEX    = WM_USER +  81;
		public const int EM_SETUNDOLIMIT          = WM_USER +  82;
		public const int EM_REDO                  = WM_USER +  84;
		public const int EM_CANREDO               = WM_USER +  85;
		public const int EM_GETUNDONAME           = WM_USER +  86;
		public const int EM_GETREDONAME           = WM_USER +  87;
		public const int EM_STOPGROUPTYPING       = WM_USER +  88;
		public const int EM_SETTEXTMODE           = WM_USER +  89;
		public const int EM_GETTEXTMODE           = WM_USER +  90;
		public const int EM_AUTOURLDETECT         = WM_USER +  91;
		public const int EM_GETAUTOURLDETECT      = WM_USER +  92;
		public const int EM_SETPALETTE            = WM_USER +  93;
		public const int EM_GETTEXTEX             = WM_USER +  94;
		public const int EM_GETTEXTLENGTHEX       = WM_USER +  95;
		public const int EM_SHOWSCROLLBAR         = WM_USER +  96;
		public const int EM_SETTEXTEX             = WM_USER +  97;
		public const int EM_SETPUNCTUATION        = WM_USER + 100;
		public const int EM_GETPUNCTUATION        = WM_USER + 101;
		public const int EM_SETWORDWRAPMODE       = WM_USER + 102;
		public const int EM_GETWORDWRAPMODE       = WM_USER + 103;
		public const int EM_SETIMECOLOR           = WM_USER + 104;
		public const int EM_GETIMECOLOR           = WM_USER + 105;
		public const int EM_SETIMEOPTIONS         = WM_USER + 106;
		public const int EM_GETIMEOPTIONS         = WM_USER + 107;
		public const int EM_CONVPOSITION          = WM_USER + 108;
		public const int EM_SETLANGOPTIONS        = WM_USER + 120;
		public const int EM_GETLANGOPTIONS        = WM_USER + 121;
		public const int EM_GETIMECOMPMODE        = WM_USER + 122;
		public const int EM_FINDTEXTW             = WM_USER + 123;
		public const int EM_FINDTEXTEXW           = WM_USER + 124;
		public const int EM_RECONVERSION          = WM_USER + 125;
		public const int EM_SETIMEMODEBIAS        = WM_USER + 126;
		public const int EM_GETIMEMODEBIAS        = WM_USER + 127;
		public const int EM_SETBIDIOPTIONS        = WM_USER + 200;
		public const int EM_GETBIDIOPTIONS        = WM_USER + 201;
		public const int EM_SETTYPOGRAPHYOPTIONS  = WM_USER + 202;
		public const int EM_GETTYPOGRAPHYOPTIONS  = WM_USER + 203;
		public const int EM_SETEDITSTYLE          = WM_USER + 204;
		public const int EM_GETEDITSTYLE          = WM_USER + 205;
		public const int EM_OUTLINE               = WM_USER + 220;
		public const int EM_GETSCROLLPOS          = WM_USER + 221;
		public const int EM_SETSCROLLPOS          = WM_USER + 222;
		public const int EM_SETFONTSIZE           = WM_USER + 223;
		public const int EM_GETZOOM               = WM_USER + 224;
		public const int EM_SETZOOM               = WM_USER + 225;
		public const int EM_GETVIEWKIND           = WM_USER + 226;
		public const int EM_SETVIEWKIND           = WM_USER + 227;
		public const int EM_GETPAGE               = WM_USER + 228;
		public const int EM_SETPAGE               = WM_USER + 229;
		public const int EM_GETHYPHENATEINFO      = WM_USER + 230;
		public const int EM_SETHYPHENATEINFO      = WM_USER + 231;
		public const int EM_GETPAGEROTATE         = WM_USER + 235;
		public const int EM_SETPAGEROTATE         = WM_USER + 236;
		public const int EM_GETCTFMODEBIAS        = WM_USER + 237;
		public const int EM_SETCTFMODEBIAS        = WM_USER + 238;
		public const int EM_GETCTFOPENSTATUS      = WM_USER + 240;
		public const int EM_SETCTFOPENSTATUS      = WM_USER + 241;
		public const int EM_GETIMECOMPTEXT        = WM_USER + 242;
		public const int EM_ISIME                 = WM_USER + 243;
		public const int EM_GETIMEPROPERTY        = WM_USER + 244;

		#endregion

		#region ListView

		public const int LVM_FIRST = 0x1000;
		public const int LVN_FIRST = -100;

		public const int LVM_GETBKCOLOR                  = LVM_FIRST + 0;
		public const int LVM_SETBKCOLOR                  = LVM_FIRST + 1;
		public const int LVM_GETIMAGELIST                = LVM_FIRST + 2;
		public const int LVM_SETIMAGELIST                = LVM_FIRST + 3;
		public const int LVM_GETITEMCOUNT                = LVM_FIRST + 4;
		public const int LVM_GETITEMA                    = LVM_FIRST + 5;
		public const int LVM_SETITEMA                    = LVM_FIRST + 6;
		public const int LVM_INSERTITEMA                 = LVM_FIRST + 7;
		public const int LVM_DELETEITEM                  = LVM_FIRST + 8;
		public const int LVM_DELETEALLITEMS              = LVM_FIRST + 9;
		public const int LVM_GETCALLBACKMASK             = LVM_FIRST + 10;
		public const int LVM_SETCALLBACKMASK             = LVM_FIRST + 11;
		public const int LVM_GETNEXTITEM                 = LVM_FIRST + 12;
		public const int LVM_FINDITEMA                   = LVM_FIRST + 13;
		public const int LVM_GETITEMRECT                 = LVM_FIRST + 14;
		public const int LVM_SETITEMPOSITION             = LVM_FIRST + 15;
		public const int LVM_GETITEMPOSITION             = LVM_FIRST + 16;
		public const int LVM_GETSTRINGWIDTHA             = LVM_FIRST + 17;
		public const int LVM_HITTEST                     = LVM_FIRST + 18;
		public const int LVM_ENSUREVISIBLE               = LVM_FIRST + 19;
		public const int LVM_SCROLL                      = LVM_FIRST + 20;
		public const int LVM_REDRAWITEMS                 = LVM_FIRST + 21;
		public const int LVM_ARRANGE                     = LVM_FIRST + 22;
		public const int LVM_EDITLABELA                  = LVM_FIRST + 23;
		public const int LVM_GETEDITCONTROL              = LVM_FIRST + 24;
		public const int LVM_GETCOLUMNA                  = LVM_FIRST + 25;
		public const int LVM_SETCOLUMNA                  = LVM_FIRST + 26;
		public const int LVM_INSERTCOLUMNA               = LVM_FIRST + 27;
		public const int LVM_DELETECOLUMN                = LVM_FIRST + 28;
		public const int LVM_GETCOLUMNWIDTH              = LVM_FIRST + 29;
		public const int LVM_SETCOLUMNWIDTH              = LVM_FIRST + 30;
		public const int LVM_GETHEADER                   = LVM_FIRST + 31;
		public const int LVM_CREATEDRAGIMAGE             = LVM_FIRST + 33;
		public const int LVM_GETVIEWRECT                 = LVM_FIRST + 34;
		public const int LVM_GETTEXTCOLOR                = LVM_FIRST + 35;
		public const int LVM_SETTEXTCOLOR                = LVM_FIRST + 36;
		public const int LVM_GETTEXTBKCOLOR              = LVM_FIRST + 37;
		public const int LVM_SETTEXTBKCOLOR              = LVM_FIRST + 38;
		public const int LVM_GETTOPINDEX                 = LVM_FIRST + 39;
		public const int LVM_GETCOUNTPERPAGE             = LVM_FIRST + 40;
		public const int LVM_GETORIGIN                   = LVM_FIRST + 41;
		public const int LVM_UPDATE                      = LVM_FIRST + 42;
		public const int LVM_SETITEMSTATE                = LVM_FIRST + 43;
		public const int LVM_GETITEMSTATE                = LVM_FIRST + 44;
		public const int LVM_GETITEMTEXTA                = LVM_FIRST + 45;
		public const int LVM_SETITEMTEXTA                = LVM_FIRST + 46;
		public const int LVM_SETITEMCOUNT                = LVM_FIRST + 47;
		public const int LVM_SORTITEMS                   = LVM_FIRST + 48;
		public const int LVM_SETITEMPOSITION32           = LVM_FIRST + 49;
		public const int LVM_GETSELECTEDCOUNT            = LVM_FIRST + 50;
		public const int LVM_GETITEMSPACING              = LVM_FIRST + 51;
		public const int LVM_GETISEARCHSTRINGA           = LVM_FIRST + 52;
		public const int LVM_SETICONSPACING              = LVM_FIRST + 53;
		public const int LVM_SETEXTENDEDLISTVIEWSTYLE    = LVM_FIRST + 54;
		public const int LVM_GETEXTENDEDLISTVIEWSTYLE    = LVM_FIRST + 55;
		public const int LVM_GETSUBITEMRECT              = LVM_FIRST + 56;
		public const int LVM_SUBITEMHITTEST              = LVM_FIRST + 57;
		public const int LVM_SETCOLUMNORDERARRAY         = LVM_FIRST + 58;
		public const int LVM_GETCOLUMNORDERARRAY         = LVM_FIRST + 59;
		public const int LVM_SETHOTITEM                  = LVM_FIRST + 60;
		public const int LVM_GETHOTITEM                  = LVM_FIRST + 61;
		public const int LVM_SETHOTCURSOR                = LVM_FIRST + 62;
		public const int LVM_GETHOTCURSOR                = LVM_FIRST + 63;
		public const int LVM_APPROXIMATEVIEWRECT         = LVM_FIRST + 64;
		public const int LVM_SETWORKAREAS                = LVM_FIRST + 65;
		public const int LVM_SETBKIMAGEA                 = LVM_FIRST + 68;
		public const int LVM_GETWORKAREAS                = LVM_FIRST + 70;
		public const int LVM_GETNUMBEROFWORKAREAS        = LVM_FIRST + 73;
		public const int LVM_GETSELECTIONMARK            = LVM_FIRST + 66;
		public const int LVM_SETSELECTIONMARK            = LVM_FIRST + 67;
		public const int LVM_GETBKIMAGEA                 = LVM_FIRST + 69;
		public const int LVM_SETHOVERTIME                = LVM_FIRST + 71;
		public const int LVM_GETHOVERTIME                = LVM_FIRST + 72;
		public const int LVM_SETTOOLTIPS                 = LVM_FIRST + 74;
		public const int LVM_GETITEMW                    = LVM_FIRST + 75;
		public const int LVM_SETITEMW                    = LVM_FIRST + 76;
		public const int LVM_INSERTITEMW                 = LVM_FIRST + 77;
		public const int LVM_GETTOOLTIPS                 = LVM_FIRST + 78;
		public const int LVM_SORTITEMSEX                 = LVM_FIRST + 81;
		public const int LVM_FINDITEMW                   = LVM_FIRST + 83;
		public const int LVM_GETSTRINGWIDTHW             = LVM_FIRST + 87;
		public const int LVM_GETCOLUMNW                  = LVM_FIRST + 95;
		public const int LVM_SETCOLUMNW                  = LVM_FIRST + 96;
		public const int LVM_INSERTCOLUMNW               = LVM_FIRST + 97;
		public const int LVM_GETITEMTEXTW                = LVM_FIRST + 115;
		public const int LVM_SETITEMTEXTW                = LVM_FIRST + 116;
		public const int LVM_GETISEARCHSTRINGW           = LVM_FIRST + 117;
		public const int LVM_EDITLABELW                  = LVM_FIRST + 118;
		public const int LVM_SETBKIMAGEW                 = LVM_FIRST + 138;
		public const int LVM_GETBKIMAGEW                 = LVM_FIRST + 139;
		public const int LVM_SETSELECTEDCOLUMN           = LVM_FIRST + 140;
		public const int LVM_SETTILEWIDTH                = LVM_FIRST + 141;
		public const int LVM_SETVIEW                     = LVM_FIRST + 142;
		public const int LVM_GETVIEW                     = LVM_FIRST + 143;
		public const int LVM_INSERTGROUP                 = LVM_FIRST + 145;
		public const int LVM_SETGROUPINFO                = LVM_FIRST + 147;
		public const int LVM_GETGROUPINFO                = LVM_FIRST + 149;
		public const int LVM_REMOVEGROUP                 = LVM_FIRST + 150;
		public const int LVM_MOVEGROUP                   = LVM_FIRST + 151;
		public const int LVM_MOVEITEMTOGROUP             = LVM_FIRST + 154;
		public const int LVM_SETGROUPMETRICS             = LVM_FIRST + 155;
		public const int LVM_GETGROUPMETRICS             = LVM_FIRST + 156;
		public const int LVM_ENABLEGROUPVIEW             = LVM_FIRST + 157;
		public const int LVM_SORTGROUPS                  = LVM_FIRST + 158;
		public const int LVM_INSERTGROUPSORTED           = LVM_FIRST + 159;
		public const int LVM_REMOVEALLGROUPS             = LVM_FIRST + 160;
		public const int LVM_HASGROUP                    = LVM_FIRST + 161;
		public const int LVM_SETTILEVIEWINFO             = LVM_FIRST + 162;
		public const int LVM_GETTILEVIEWINFO             = LVM_FIRST + 163;
		public const int LVM_SETTILEINFO                 = LVM_FIRST + 164;
		public const int LVM_GETTILEINFO                 = LVM_FIRST + 165;
		public const int LVM_SETINSERTMARK               = LVM_FIRST + 166;
		public const int LVM_GETINSERTMARK               = LVM_FIRST + 167;
		public const int LVM_INSERTMARKHITTEST           = LVM_FIRST + 168;
		public const int LVM_GETINSERTMARKRECT           = LVM_FIRST + 169;
		public const int LVM_SETINSERTMARKCOLOR          = LVM_FIRST + 170;
		public const int LVM_GETINSERTMARKCOLOR          = LVM_FIRST + 171;
		public const int LVM_SETINFOTIP                  = LVM_FIRST + 173;
		public const int LVM_GETSELECTEDCOLUMN           = LVM_FIRST + 174;
		public const int LVM_ISGROUPVIEWENABLED          = LVM_FIRST + 175;
		public const int LVM_GETOUTLINECOLOR             = LVM_FIRST + 176;
		public const int LVM_SETOUTLINECOLOR             = LVM_FIRST + 177;
		public const int LVM_CANCELEDITLABEL             = LVM_FIRST + 179;
		public const int LVM_MAPINDEXTOID                = LVM_FIRST + 180;
		public const int LVM_MAPIDTOINDEX                = LVM_FIRST + 181;

		public const int LVN_ITEMCHANGING                = LVN_FIRST - 0;
		public const int LVN_ITEMCHANGED                 = LVN_FIRST - 1;
		public const int LVN_INSERTITEM                  = LVN_FIRST - 2;
		public const int LVN_DELETEITEM                  = LVN_FIRST - 3;
		public const int LVN_DELETEALLITEMS              = LVN_FIRST - 4;
		public const int LVN_BEGINLABELEDITA             = LVN_FIRST - 5;
		public const int LVN_ENDLABELEDITA               = LVN_FIRST - 6;
		public const int LVN_COLUMNCLICK                 = LVN_FIRST - 8;
		public const int LVN_BEGINDRAG                   = LVN_FIRST - 9;
		public const int LVN_BEGINRDRAG                  = LVN_FIRST - 11;
		public const int LVN_ODCACHEHINT                 = LVN_FIRST - 13;
		public const int LVN_ITEMACTIVATE                = LVN_FIRST - 14;
		public const int LVN_ODSTATECHANGED              = LVN_FIRST - 15;
		public const int LVN_HOTTRACK                    = LVN_FIRST - 21;
		public const int LVN_GETDISPINFOA                = LVN_FIRST - 50;
		public const int LVN_SETDISPINFOA                = LVN_FIRST - 51;
		public const int LVN_ODFINDITEMA                 = LVN_FIRST - 52;
		public const int LVN_KEYDOWN                     = LVN_FIRST - 55;
		public const int LVN_MARQUEEBEGIN                = LVN_FIRST - 56;
		public const int LVN_GETINFOTIPA                 = LVN_FIRST - 57;
		public const int LVN_GETINFOTIPW                 = LVN_FIRST - 58;
		public const int LVN_BEGINLABELEDITW             = LVN_FIRST - 75;
		public const int LVN_ENDLABELEDITW               = LVN_FIRST - 76;
		public const int LVN_GETDISPINFOW                = LVN_FIRST - 77;
		public const int LVN_SETDISPINFOW                = LVN_FIRST - 78;
		public const int LVN_ODFINDITEMW                 = LVN_FIRST - 79;
		public const int LVN_BEGINSCROLL                 = LVN_FIRST - 80;
		public const int LVN_ENDSCROLL                   = LVN_FIRST - 81;

		#endregion

		#region ToolTip

		public const int TTM_ACTIVATE        = WM_USER +  1;
		public const int TTM_SETDELAYTIME    = WM_USER +  3;
		public const int TTM_ADDTOOLA        = WM_USER +  4;
		public const int TTM_DELTOOLA        = WM_USER +  5;
		public const int TTM_NEWTOOLRECTA    = WM_USER +  6;
		public const int TTM_RELAYEVENT      = WM_USER +  7;
		public const int TTM_GETTOOLINFOA    = WM_USER +  8;
		public const int TTM_SETTOOLINFOA    = WM_USER +  9;
		public const int TTM_HITTESTA        = WM_USER + 10;
		public const int TTM_GETTEXTA        = WM_USER + 11;
		public const int TTM_UPDATETIPTEXTA  = WM_USER + 12;
		public const int TTM_GETTOOLCOUNT    = WM_USER + 13;
		public const int TTM_ENUMTOOLSA      = WM_USER + 14;
		public const int TTM_GETCURRENTTOOLA = WM_USER + 15;
		public const int TTM_WINDOWFROMPOINT = WM_USER + 16;
		public const int TTM_TRACKACTIVATE   = WM_USER + 17;
		public const int TTM_TRACKPOSITION   = WM_USER + 18;
		public const int TTM_SETTIPBKCOLOR   = WM_USER + 19;
		public const int TTM_SETTIPTEXTCOLOR = WM_USER + 20;
		public const int TTM_GETDELAYTIME    = WM_USER + 21;
		public const int TTM_GETTIPBKCOLOR   = WM_USER + 22;
		public const int TTM_GETTIPTEXTCOLOR = WM_USER + 23;
		public const int TTM_SETMAXTIPWIDTH  = WM_USER + 24;
		public const int TTM_GETMAXTIPWIDTH  = WM_USER + 25;
		public const int TTM_SETMARGIN       = WM_USER + 26;
		public const int TTM_GETMARGIN       = WM_USER + 27;
		public const int TTM_POP             = WM_USER + 28;
		public const int TTM_UPDATE          = WM_USER + 29;
		public const int TTM_GETBUBBLESIZE   = WM_USER + 30;
		public const int TTM_ADJUSTRECT      = WM_USER + 31;
		public const int TTM_SETTITLEA       = WM_USER + 32;
		public const int TTM_SETTITLEW       = WM_USER + 33;
		public const int TTM_POPUP           = WM_USER + 34;
		public const int TTM_GETTITLE        = WM_USER + 35;
		public const int TTM_ADDTOOLW        = WM_USER + 50;
		public const int TTM_DELTOOLW        = WM_USER + 51;
		public const int TTM_NEWTOOLRECTW    = WM_USER + 52;
		public const int TTM_GETTOOLINFOW    = WM_USER + 53;
		public const int TTM_SETTOOLINFOW    = WM_USER + 54;
		public const int TTM_HITTESTW        = WM_USER + 55;
		public const int TTM_GETTEXTW        = WM_USER + 56;
		public const int TTM_UPDATETIPTEXTW  = WM_USER + 57;
		public const int TTM_ENUMTOOLSW      = WM_USER + 58;
		public const int TTM_GETCURRENTTOOLW = WM_USER + 59;

		#endregion

		#endregion

		#region Window Message Parameters

		#region General

		#region WM_ACTIVATE

		public const int WA_INACTIVE    = 0;
		public const int WA_ACTIVE      = 1;
		public const int WA_CLICKACTIVE = 2;

		#endregion

		#region WM_POWER

		public const int PWR_OK             = 1;
		public const int PWR_FAIL           = -1;
		public const int PWR_SUSPENDREQUEST = 1;
		public const int PWR_SUSPENDRESUME  = 2;
		public const int PWR_CRITICALRESUME = 3;

		#endregion

		#region WM_NC*

		public enum HitTestCode : int
		{
			Error       = -2,
			Transparent = -1,
			NoWhere     =  0,
			Client      =  1,
			Caption     =  2,
			SysMenu     =  3,
			GrowBox     =  4,
			Size        =  GrowBox,
			Menu        =  5,
			HScroll     =  6,
			VScroll     =  7,
			MinButton   =  8,
			MaxButton   =  9,
			Left        = 10,
			Right       = 11,
			Top         = 12,
			TopLeft     = 13,
			TopRight    = 14,
			Bottom      = 15,
			BottomLeft  = 16,
			BottomRight = 17,
			Border      = 18,
			Reduce      = MinButton,
			Zoom        = MaxButton,
			SizeFirst   = Left,
			SizeLast    = BottomRight
		}

		#endregion

		#region WM_NOTIFYFORMAT

		public const int NFR_ANSI    = 1;
		public const int NFR_UNICODE = 2;
		public const int NF_QUERY    = 3;
		public const int NF_REQUERY  = 4;

		#endregion

		#region WM_SYSCOMMAND

		public const int SC_SIZE         = 0xF000;
		public const int SC_MOVE         = 0xF010;
		public const int SC_MINIMIZE     = 0xF020;
		public const int SC_MAXIMIZE     = 0xF030;
		public const int SC_NEXTWINDOW   = 0xF040;
		public const int SC_PREVWINDOW   = 0xF050;
		public const int SC_CLOSE        = 0xF060;
		public const int SC_VSCROLL      = 0xF070;
		public const int SC_HSCROLL      = 0xF080;
		public const int SC_MOUSEMENU    = 0xF090;
		public const int SC_KEYMENU      = 0xF100;
		public const int SC_ARRANGE      = 0xF110;
		public const int SC_RESTORE      = 0xF120;
		public const int SC_TASKLIST     = 0xF130;
		public const int SC_SCREENSAVE   = 0xF140;
		public const int SC_HOTKEY       = 0xF150;
		public const int SC_DEFAULT      = 0xF160;
		public const int SC_MONITORPOWER = 0xF170;
		public const int SC_CONTEXTHELP  = 0xF180;
		public const int SC_SEPARATOR    = 0xF00F;

		#endregion

		#region WM_*UISTATE

		public const int UIS_SET        = 1;
		public const int UIS_CLEAR      = 2;
		public const int UIS_INITIALIZE = 3;
		public const int UISF_HIDEFOCUS = 1;
		public const int UISF_HIDEACCEL = 2;
		public const int UISF_ACTIVE    = 4;

		#endregion

		#region WM_POWERBROADCAST

		public const int PBT_APMQUERYSUSPEND       = 0x0000;
		public const int PBT_APMQUERYSTANDBY       = 0x0001;
		public const int PBT_APMQUERYSUSPENDFAILED = 0x0002;
		public const int PBT_APMQUERYSTANDBYFAILED = 0x0003;
		public const int PBT_APMSUSPEND            = 0x0004;
		public const int PBT_APMSTANDBY            = 0x0005;
		public const int PBT_APMRESUMECRITICAL     = 0x0006;
		public const int PBT_APMRESUMESUSPEND      = 0x0007;
		public const int PBT_APMRESUMESTANDBY      = 0x0008;
		public const int PBTF_APMRESUMEFROMFAILURE = 0x0001;
		public const int PBT_APMBATTERYLOW         = 0x0009;
		public const int PBT_APMPOWERSTATUSCHANGE  = 0x000A;
		public const int PBT_APMOEMEVENT           = 0x000B;
		public const int PBT_APMRESUMEAUTOMATIC    = 0x0012;

		#endregion

		#region Window_Animate

		public const int AW_HOR_POSITIVE = 0x00000001;
		public const int AW_HOR_NEGATIVE = 0x00000002;
		public const int AW_VER_POSITIVE = 0x00000004;
		public const int AW_VER_NEGATIVE = 0x00000008;
		public const int AW_CENTER       = 0x00000010;
		public const int AW_HIDE         = 0x00010000;
		public const int AW_ACTIVATE     = 0x00020000;
		public const int AW_SLIDE        = 0x00040000;
		public const int AW_BLEND        = 0x00080000;

		#endregion

		#region Window_Long*

		public const int GWL_WNDPROC    =  -4;
		public const int GWL_HINSTANCE  =  -6;
		public const int GWL_HWNDPARENT =  -8;
		public const int GWL_STYLE      = -16;
		public const int GWL_EXSTYLE    = -20;
		public const int GWL_USERDATA   = -21;
		public const int GWL_ID         = -12;

		#endregion

		#region Window_Show

		public const int SW_HIDE            =  0;
		public const int SW_SHOWNORMAL      =  1;
		public const int SW_NORMAL          =  1;
		public const int SW_SHOWMINIMIZED   =  2;
		public const int SW_SHOWMAXIMIZED   =  3;
		public const int SW_MAXIMIZE        =  3;
		public const int SW_SHOWNOACTIVATE  =  4;
		public const int SW_SHOW            =  5;
		public const int SW_MINIMIZE        =  6;
		public const int SW_SHOWMINNOACTIVE =  7;
		public const int SW_SHOWNA          =  8;
		public const int SW_RESTORE         =  9;
		public const int SW_SHOWDEFAULT     = 10;
		public const int SW_FORCEMINIMIZE   = 11;
		public const int SW_MAX             = 11;

		#endregion

		#region Window_Position*

		[Flags]
			public enum WindowPositionFlags : int
		{
			NoSize         = 0x0001,
			NoMove         = 0x0002,
			NoZOrder       = 0x0004,
			NoReDraw       = 0x0008,
			NoActivate     = 0x0010,
			FrameChanged   = 0x0020,
			ShowWindow     = 0x0040,
			HideWindow     = 0x0080,
			NoCopyBits     = 0x0100,
			NoOwnerZOrder  = 0x0200,
			NoSendChanging = 0x0400,
			DrawFrame      = FrameChanged,
			NoReposition   = NoOwnerZOrder,
			DeferErase     = 0x2000,
			AsyncWindowPos = 0x4000
		}

		#endregion

		#endregion

		#region Common Controls

		#region Notifications

		public const int NM_OUTOFMEMORY     =  -1;
		public const int NM_CLICK           =  -2;
		public const int NM_DBLCLK          =  -3;
		public const int NM_RETURN          =  -4;
		public const int NM_RCLICK          =  -5;
		public const int NM_RDBLCLK         =  -6;
		public const int NM_SETFOCUS        =  -7;
		public const int NM_KILLFOCUS       =  -8;
		public const int NM_CUSTOMDRAW      = -12;
		public const int NM_HOVER           = -13;
		public const int NM_NCHITTEST       = -14;
		public const int NM_KEYDOWN         = -15;
		public const int NM_RELEASEDCAPTURE = -16;
		public const int NM_SETCURSOR       = -17;
		public const int NM_CHAR            = -18;
		public const int NM_TOOLTIPSCREATED = -19;
		public const int NM_LDOWN           = -20;
		public const int NM_RDOWN           = -21;
		public const int NM_THEMECHANGED    = -22;

		[Flags]
			public enum CustomDrawStage : int
		{
			PrePaint         = 0x00000001,
			PostPaint        = 0x00000002,
			PreErase         = 0x00000003,
			PostErase        = 0x00000004,
			Item             = 0x00010000,
			ItemPrePaint     = Item | PrePaint,
			ItemPostPaint    = Item | PostPaint,
			ItemPreErase     = Item | PreErase,
			ItemPostErase    = Item | PostErase,
			SubItem          = 0x00020000,
			SubItemPrePaint  = SubItem | PrePaint,
			SubItemPostPaint = SubItem | PostPaint,
			SubItemPreErase  = SubItem | PreErase,
			SubItemPostErase = SubItem | PostErase,
		}

		[Flags]
			public enum CustomDrawItemState : int
		{
			Selected         = 0x0001,
			Grayed           = 0x0002,
			Disabled         = 0x0004,
			Checked          = 0x0008,
			Focus            = 0x0010,
			Default          = 0x0020,
			Hot              = 0x0040,
			Marked           = 0x0080,
			Indeterminate    = 0x0100,
			ShowKeyboardCues = 0x0200
		}

		[Flags]
			public enum CustomDrawReturn : int
		{
			DoDefault         = 0x00000000,
			NewFont           = 0x00000002,
			SkipDefault       = 0x00000004,
			NotifyPostPaint   = 0x00000010,
			NotifyItemDraw    = 0x00000020,
			NotifySubItemDraw = 0x00000020,
			NotifyPostErase   = 0x00000040
		}

		#endregion

		#endregion

		#region ToolTip

		[Flags]
			public enum ToolTipFlags : int
		{
			IDIsHWnd      = 0x0001,
			CenterTip     = 0x0002,
			RTLReading    = 0x0004,
			Subclass      = 0x0010,
			Track         = 0x0020,
			Absolute      = 0x0080,
			Transparent   = 0x0100,
			ParseLinks    = 0x1000,
			SetItem       = 0x8000
		}

		[Flags]
			public enum ToolTipDelayType : int
		{
			Automatic = 0,
			ReShow    = 1,
			AutoPop   = 2,
			Initial   = 3
		}

		[Flags]
			public enum ToolTipTitleIcon : int
		{
			None    = 0,
			Info    = 1,
			Warning = 2,
			Error   = 3
		}

		#endregion

		#endregion

		#region Window Styles

		[Flags]
			public enum WindowStyle : int
		{
			Overlapped       = 0x00000000,
			PopUp            = unchecked((int)0x80000000),
			Child            = 0x40000000,
			Minimize         = 0x20000000,
			Visible          = 0x10000000,
			Disabled         = 0x08000000,
			ClipSiblings     = 0x04000000,
			ClipChildren     = 0x02000000,
			Maximize         = 0x01000000,
			Caption          = 0x00C00000,
			Border           = 0x00800000,
			DialogFrame      = 0x00400000,
			VScroll          = 0x00200000,
			HScroll          = 0x00100000,
			SysMenu          = 0x00080000,
			ThickFrame       = 0x00040000,
			Group            = 0x00020000,
			TabStop          = 0x00010000,
			MinimizeBox      = 0x00020000,
			MaximizeBox      = 0x00010000,
			Tiled            = Overlapped,
			Iconic           = Minimize,
			SizeBox          = ThickFrame,
			TiledWindow      = OverlappedWindow,
			OverlappedWindow = Overlapped | Caption | SysMenu | ThickFrame | MinimizeBox | MaximizeBox,
			PopUpWindow      = PopUp | Border | SysMenu,
			ChildWindow      = Child
		}

		[Flags]
			public enum WindowStyleExtended : int
		{
			DialogModalFrame = 0x00000001,
			NoParentNotify   = 0x00000004,
			Topmost          = 0x00000008,
			AcceptFiles      = 0x00000010,
			Transparent      = 0x00000020,
			MdiChild         = 0x00000040,
			ToolWindow       = 0x00000080,
			WindowEdge       = 0x00000100,
			ClientEdge       = 0x00000200,
			ContextHelp      = 0x00000400,
			Right            = 0x00001000,
			Left             = 0x00000000,
			RtlReading       = 0x00002000,
			LtrReading       = 0x00000000,
			LeftScrollbar    = 0x00004000,
			RightScrollbar   = 0x00000000,
			ControlParent    = 0x00010000,
			StaticEdge       = 0x00020000,
			AppWindow        = 0x00040000,
			OverlappedWindow = WindowEdge | ClientEdge,
			PaletteWindow    = WindowEdge | ToolWindow | Topmost,
			Layered          = 0x00080000,
			NoInheritLayout  = 0x00100000,
			LayoutRtl        = 0x00400000,
			Composited       = 0x02000000,
			NoActivate       = 0x08000000,
		}

		[Flags]
			public enum WindowClassStyle : int
		{
			VReDraw         = 0x00000001,
			HReDraw         = 0x00000002,
			DoubleClicks    = 0x00000008,
			OwnDC           = 0x00000020,
			ClassDC         = 0x00000040,
			ParentDC        = 0x00000080,
			NoClose         = 0x00000200,
			SaveBits        = 0x00000800,
			ByteAlignClient = 0x00001000,
			ByteAlignWindow = 0x00002000,
			GlobalClass     = 0x00004000,
			Ime             = 0x00010000,
			DropShadow      = 0x00020000
		}

		[Flags]
			public enum CommonControlStyle
		{
			Top           = 0x00000001,
			NoMoveY       = 0x00000002,
			Bottom        = 0x00000003,
			NoResize      = 0x00000004,
			NoParentAlign = 0x00000008,
			Adjustable    = 0x00000020,
			NoDivider     = 0x00000040,
			Vert          = 0x00000080,
			Left          = Vert | Top,
			Right         = Vert | Bottom,
			NoMoveX       = Vert | NoMoveY
		}

		[Flags]
			public enum ToolTipStyle : int
		{
			AlwaysTip = 0x01,
			NoPrefix  = 0x02,
			NoAnimate = 0x10,
			NoFade    = 0x20,
			Balloon   = 0x40,
			Close     = 0x80
		}

		#endregion

		#region ScrollBar

		[Flags]
			public enum ScrollBarBar : int
		{
			Horizontal = 0,
			Vertical   = 1,
			Control    = 2,
			Both       = 3
		}

		[Flags]
			public enum ScrollBarArrow : int
		{
			Enable       = 0,
			DisableLeft  = 1,
			DisableUp    = 1,
			DisableRight = 2,
			DisableDown  = 2,
			DisableBoth  = 3
		}

		public enum ScrollBarObject : int
		{
			HScroll = unchecked((int)0xFFFFFFFA),
			VScroll = unchecked((int)0xFFFFFFFB),
			Client  = unchecked((int)0xFFFFFFFC)
		}

		[Flags]
			public enum ScrollBarInfoFlag
		{
			Range           =  1,
			Page            =  2,
			Position        =  4,
			DisableNoScroll =  8,
			TrackPosition   = 16,
			All = (Range | Page | Position | DisableNoScroll | TrackPosition)
		}

		public enum ScrollBarNotificationCode : int
		{
			LineUp        = 0,
			LineLeft      = 0,
			LineDown      = 1,
			LineRight     = 1,
			PageUp        = 2,
			PageLeft      = 2,
			PageDown      = 3,
			PageRight     = 3,
			ThumbPosition = 4,
			ThumbTrack    = 5,
			Top           = 6,
			Left          = 6,
			Bottom        = 7,
			Right         = 7,
			End           = 8
		}

		#endregion

		#endregion

		#region Types

		#region General

		[StructLayout(LayoutKind.Sequential)]
			public struct WindowPosition
		{
			public IntPtr HWnd;
			public IntPtr HWndAfter;
			public int Left;
			public int Top;
			public int Width;
			public int Height;
			[MarshalAs(UnmanagedType.U4)]
			public WindowPositionFlags Flags;
		}

		[StructLayout(LayoutKind.Sequential)]
			public struct NMHeader
		{
			public IntPtr HWnd;
			public int ID;
			public int Code;
		}

		#endregion

		#region ScrollBar

		[StructLayout(LayoutKind.Sequential)]
			public struct ScrollBarInfo
		{
			public int Size;
			public Rectangle ScrollBar;
			public int ThumbSize;
			public int ThumbTop;
			public int ThumbBottom;
			public int Reserved;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
			public int[] State;
		}

		[StructLayout(LayoutKind.Sequential)]
			public struct ScrollInfo
		{
			public int Size;
			[MarshalAs(UnmanagedType.U4)]
			public ScrollBarInfoFlag Mask;
			public int Min;
			public int Max;
			public int Page;
			public int Position;
			public int TrackPosition;
		}

		[Flags]
			public enum ScrollWindowFlags : int
		{
			ScrollChildren =  1,
			Invalidate     =  2,
			Erase          =  4,
			Smooth         = 16
		}

		#endregion

		#region Common Controls

		[StructLayout(LayoutKind.Sequential)]
			public struct NotificationMouse
		{
			public NMHeader Header;
			public IntPtr Reserved1;
			public IntPtr Reserved2;
			public Point Point;
			public int HitInfo;
		}

		[StructLayout(LayoutKind.Sequential)]
			public struct NotificationKey
		{
			public NMHeader Header;
			public int VKey;
			public int Flags;
		}

		[StructLayout(LayoutKind.Sequential)]
			public struct NotificationChar
		{
			public NMHeader Header;
			public int Symbol;
			public int Reserved1;
			public int Reserved2;
		}

		[StructLayout(LayoutKind.Sequential)]
			public struct NotificationTooltipsCreated
		{
			public NMHeader Header;
			public IntPtr HWndTooltips;
		}

		[StructLayout(LayoutKind.Sequential)]
			public struct NotificationCustomDrawInfo
		{
			public NMHeader Header;
			[MarshalAs(UnmanagedType.U4)]
			public CustomDrawStage DrawStage;
			public IntPtr HDC;
			public Rectangle Rect;
			public int Item;
			[MarshalAs(UnmanagedType.U4)]
			public CustomDrawItemState ItemState;
			public int LParam;
		}

		#endregion

		#region ToolTip

		[StructLayout(LayoutKind.Sequential)]
			public struct ToolTipToolInfo
		{
			public int Size;
			[MarshalAs(UnmanagedType.U4)]
			public ToolTipFlags Flags;
			public IntPtr HWnd;
			public IntPtr ID;
			public Rectangle Rect;
			public IntPtr HInstance;
			public IntPtr Text;
			public IntPtr LParam;

			public ToolTipToolInfo(IntPtr lParam)
			{
				this.Size = Marshal.SizeOf(typeof(ToolTipToolInfo));
				this.Flags = 0;
				this.HWnd = IntPtr.Zero;
				this.ID = IntPtr.Zero;
				this.Rect = Rectangle.Empty;
				this.HInstance = Module_HandleGet(null);
				this.Text = IntPtr.Zero;
				this.LParam = lParam;
			}

			public ToolTipToolInfo(ToolTipFlags flags, IntPtr id, IntPtr text, IntPtr lParam)
			{
				this.Size = Marshal.SizeOf(typeof(ToolTipToolInfo));
				this.Flags = flags;
				this.HWnd = IntPtr.Zero;
				this.ID = id;
				this.Rect = Rectangle.Empty;
				this.HInstance = Module_HandleGet(null);
				this.Text = text;
				this.LParam = lParam;
			}

			public ToolTipToolInfo(ToolTipFlags flags, IntPtr hWnd, IntPtr id, Rectangle rect, IntPtr text, IntPtr lParam)
			{
				this.Size = Marshal.SizeOf(typeof(ToolTipToolInfo));
				this.Flags = flags;
				this.HWnd = hWnd;
				this.ID = id;
				this.Rect = rect;
				this.HInstance = Module_HandleGet(null);
				this.Text = text;
				this.LParam = lParam;
			}
		}

		#endregion

		#endregion

		#region Methods

		#region General

		#region Create/Destroy

		[DllImport("user32.dll", EntryPoint = "CreateWindowExW")]
		public static extern IntPtr Window_Create(
			[In][MarshalAs(UnmanagedType.U4)] WindowStyleExtended styleExtended,
			[In][MarshalAs(UnmanagedType.LPWStr)] string className,
			[In][MarshalAs(UnmanagedType.LPWStr)] string windowName,
			[In][MarshalAs(UnmanagedType.U4)] WindowStyle style,
			[In] int left,
			[In] int top,
			[In] int width,
			[In] int height,
			[In] IntPtr hWndParent,
			[In] IntPtr hMenu,
			[In] IntPtr hInstance,
			[In] IntPtr lpParam);

		[DllImport("user32.dll", EntryPoint = "DestroyWindow")]
		public static extern bool Window_Destroy(IntPtr hWnd);

		#endregion

		#region Window_Long*

		[DllImport("user32.dll", EntryPoint = "GetWindowLong")]
		public static extern int Window_LongGet(IntPtr hWnd, int index);

		[DllImport("user32.dll", EntryPoint = "SetWindowLong")]
		public static extern int Window_LongSet(IntPtr hWnd, int index, int value);

		public static bool Window_GetStyle(IntPtr hWnd, int styleMask)
		{
			return (Window_LongGet(hWnd, GWL_STYLE) & styleMask) != 0;
		}

		public static void Window_SetStyle(IntPtr hWnd, int styleMask, bool value)
		{
			int windowLong = Window_LongGet(hWnd, GWL_STYLE);

			if (value)
			{
				windowLong |= styleMask;
			}
			else
			{
				windowLong &= ~styleMask;
			}

			Window_LongSet(hWnd, GWL_STYLE, windowLong);
		}

		#endregion

		#region Window_Position*

		[DllImport("user32.dll", EntryPoint = "SetWindowPos")]
		public static extern bool Window_PositionSet(
			[In] IntPtr hWnd,
			[In] IntPtr hWndInsertAfter,
			[In] int left,
			[In] int top,
			[In] int width,
			[In] int height,
			[In][MarshalAs(UnmanagedType.U4)] WindowPositionFlags Flags);

		#endregion

		#region Window_MessagePost

		[DllImport("user32.dll", EntryPoint = "PostMessage")]
		public static extern int Window_MessagePost(
			[In] IntPtr hWnd,
			[In] int message,
			[In] IntPtr wParam,
			[In] IntPtr lParam);

		[DllImport("user32.dll", EntryPoint = "PostMessage")]
		public static extern int Window_MessagePost(
			[In] IntPtr hWnd,
			[In] int message,
			[In] int wParam,
			[In] IntPtr lParam);

		[DllImport("user32.dll", EntryPoint = "PostMessage")]
		public static extern int Window_MessagePost(
			[In] IntPtr hWnd,
			[In] int message,
			[In] IntPtr wParam,
			[In] int lParam);

		[DllImport("user32.dll", EntryPoint = "PostMessage")]
		public static extern int Window_MessagePost(
			[In] IntPtr hWnd,
			[In] int message,
			[In] int wParam,
			[In] int lParam);

		#endregion

		#region Window_MessageSend

		#region General

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		public static extern int Window_MessageSend(
			[In] IntPtr hWnd,
			[In] int message,
			[In] IntPtr wParam,
			[In] IntPtr lParam);

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		public static extern int Window_MessageSend(
			[In] IntPtr hWnd,
			[In] int message,
			[In] IntPtr wParam,
			[In] int lParam);

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		public static extern int Window_MessageSend(
			[In] IntPtr hWnd,
			[In] int message,
			[In] int wParam,
			[In] IntPtr lParam);

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		public static extern int Window_MessageSend(
			[In] IntPtr hWnd,
			[In] int message,
			[In] int wParam,
			[In] int lParam);

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		public static extern int Window_MessageSend(
			[In] IntPtr hWnd,
			[In] int message,
			[Out] out IntPtr wParam,
			[In] IntPtr lParam);

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		public static extern int Window_MessageSend(
			[In] IntPtr hWnd,
			[In] int message,
			[In] IntPtr wParam,
			[Out] out IntPtr lParam);

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		public static extern int Window_MessageSend(
			[In] IntPtr hWnd,
			[In] int message,
			[Out] out IntPtr wParam,
			[Out] out IntPtr lParam);

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		public static extern int Window_MessageSend(
			[In] IntPtr hWnd,
			[In] int message,
			[Out] out IntPtr wParam,
			[In] int lParam);

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		public static extern int Window_MessageSend(
			[In] IntPtr hWnd,
			[In] int message,
			[In] IntPtr wParam,
			[Out] out int lParam);

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		public static extern int Window_MessageSend(
			[In] IntPtr hWnd,
			[In] int message,
			[Out] out IntPtr wParam,
			[Out] out int lParam);

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		public static extern int Window_MessageSend(
			[In] IntPtr hWnd,
			[In] int message,
			[Out] out int wParam,
			[In] IntPtr lParam);

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		public static extern int Window_MessageSend(
			[In] IntPtr hWnd,
			[In] int message,
			[In] int wParam,
			[Out] out IntPtr lParam);

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		public static extern int Window_MessageSend(
			[In] IntPtr hWnd,
			[In] int message,
			[Out] out int wParam,
			[Out] out IntPtr lParam);

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		public static extern int Window_MessageSend(
			[In] IntPtr hWnd,
			[In] int message,
			[Out] out int wParam,
			[In] int lParam);

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		public static extern int Window_MessageSend(
			[In] IntPtr hWnd,
			[In] int message,
			[In] int wParam,
			[Out] out int lParam);

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		public static extern int Window_MessageSend(
			[In] IntPtr hWnd,
			[In] int message,
			[Out] out int wParam,
			[Out] out int lParam);

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		public static extern int Window_MessageSendA(
			[In] IntPtr hWnd,
			[In] int message,
			[In] IntPtr wParam,
			[In][MarshalAs(UnmanagedType.LPStr)] string lParam);

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		public static extern int Window_MessageSendA(
			[In] IntPtr hWnd,
			[In] int message,
			[In] int wParam,
			[In][MarshalAs(UnmanagedType.LPStr)] string lParam);

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		public static extern int Window_MessageSendA(
			[In] IntPtr hWnd,
			[In] int message,
			[In] IntPtr wParam,
			[Out][MarshalAs(UnmanagedType.LPStr)] StringBuilder lParam);

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		public static extern int Window_MessageSendA(
			[In] IntPtr hWnd,
			[In] int message,
			[In] int wParam,
			[Out][MarshalAs(UnmanagedType.LPStr)] StringBuilder lParam);

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		public static extern int Window_MessageSendW(
			[In] IntPtr hWnd,
			[In] int message,
			[In] IntPtr wParam,
			[In][MarshalAs(UnmanagedType.LPWStr)] string lParam);

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		public static extern int Window_MessageSendW(
			[In] IntPtr hWnd,
			[In] int message,
			[In] int wParam,
			[In][MarshalAs(UnmanagedType.LPWStr)] string lParam);

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		public static extern int Window_MessageSendW(
			[In] IntPtr hWnd,
			[In] int message,
			[In] IntPtr wParam,
			[Out][MarshalAs(UnmanagedType.LPWStr)] StringBuilder lParam);

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		public static extern int Window_MessageSendW(
			[In] IntPtr hWnd,
			[In] int message,
			[In] int wParam,
			[Out][MarshalAs(UnmanagedType.LPWStr)] StringBuilder lParam);

		#endregion

		#region ToolTip

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		public static extern int Window_MessageSend(
			[In] IntPtr hWnd,
			[In] int message,
			[In] int wParam,
			[In][Out] ref ToolTipToolInfo lParam);

		#endregion

		#endregion

		#endregion

		#region ScrollBar

		[DllImport("user32.dll", EntryPoint = "EnableScrollBar")]
		public static extern bool ScrollBar_Enable(
			[In] IntPtr hWnd,
			[In][MarshalAs(UnmanagedType.U4)] ScrollBarBar bar,
			[In][MarshalAs(UnmanagedType.U4)] ScrollBarArrow arrow);

		[DllImport("user32.dll", EntryPoint = "ShowScrollBar")]
		public static extern bool ScrollBar_Show(
			[In] IntPtr hWnd,
			[In][MarshalAs(UnmanagedType.U4)] ScrollBarBar bar,
			[In] bool show);

		[DllImport("user32.dll", EntryPoint = "GetScrollBarInfo")]
		public static extern bool ScrollBar_InfoGet(
			[In] IntPtr hWnd,
			[In][MarshalAs(UnmanagedType.U4)] ScrollBarObject objectID,
			[In][Out] ref ScrollBarInfo scrollBarInfo);

		[DllImport("user32.dll", EntryPoint = "GetScrollInfo")]
		public static extern bool Scroll_InfoGet(
			[In] IntPtr hWnd,
			[In][MarshalAs(UnmanagedType.U4)] ScrollBarBar bar,
			[In][Out] ref ScrollInfo scrollInfo);

		[DllImport("user32.dll", EntryPoint = "SetScrollInfo")]
		public static extern bool Scroll_InfoSet(
			[In] IntPtr hWnd,
			[In][MarshalAs(UnmanagedType.U4)] ScrollBarBar bar,
			[In] ref ScrollInfo scrollInfo,
			[In] bool redraw);

		[DllImport("user32.dll", EntryPoint = "ScrollWindowEx")]
		public static extern int Scroll_Window(
			[In] IntPtr hWnd,
			[In] int dx,
			[In] int dy,
			[In] ref Rectangle rectScroll,
			[In] ref Rectangle rectClip,
			[In] IntPtr hRegionUpdate,
			[In] ref Rectangle rectUpdate,
			[In] int flags);

		[DllImport("user32.dll", EntryPoint = "ScrollWindowEx")]
		public static extern int Scroll_Window(
			[In] IntPtr hWnd,
			[In] int dx,
			[In] int dy,
			[In] IntPtr rectScrollNull,
			[In] ref Rectangle rectClip,
			[In] IntPtr hRegionUpdate,
			[In] ref Rectangle rectUpdate,
			[In] int flags);

		[DllImport("user32.dll", EntryPoint = "ScrollWindowEx")]
		public static extern int Scroll_Window(
			[In] IntPtr hWnd,
			[In] int dx,
			[In] int dy,
			[In] ref Rectangle rectScroll,
			[In] IntPtr rectClipNull,
			[In] IntPtr hRegionUpdate,
			[In] ref Rectangle rectUpdate,
			[In] int flags);

		[DllImport("user32.dll", EntryPoint = "ScrollWindowEx")]
		public static extern int Scroll_Window(
			[In] IntPtr hWnd,
			[In] int dx,
			[In] int dy,
			[In] ref Rectangle rectScroll,
			[In] ref Rectangle rectClip,
			[In] IntPtr hRegionUpdate,
			[In] IntPtr rectUpdateNull,
			[In] int flags);

		[DllImport("user32.dll", EntryPoint = "ScrollWindowEx")]
		public static extern int Scroll_Window(
			[In] IntPtr hWnd,
			[In] int dx,
			[In] int dy,
			[In] IntPtr rectScrollNull,
			[In] IntPtr rectClipNull,
			[In] IntPtr hRegionUpdate,
			[In] ref Rectangle rectUpdate,
			[In] int flags);

		#endregion

		#endregion

		#endregion

		#region Device Contexts
		
		#region Constants
		
		public enum DeviceCapIndex : int
		{
			DriverVersion   =   0,
			Technology      =   2,
			HorzSize        =   4,
			VertSize        =   6,
			HorzRes         =   8,
			VertRes         =  10,
			BitsPixel       =  12,
			Planes          =  14,
			NumBrushes      =  16,
			NumPens         =  18,
			NumMarkers      =  20,
			NumFonts        =  22,
			NumColors       =  24,
			PDeviceSize     =  26,
			CurveCaps       =  28,
			LineCaps        =  30,
			PolygonalCaps   =  32,
			TextCaps        =  34,
			ClipCaps        =  36,
			RasterCaps      =  38,
			AspectX         =  40,
			AspectY         =  42,
			AspectXY        =  44,
			LogPixelsX      =  88,
			LogPixelsY      =  90,
			SizePalette     = 104,
			NumReserved     = 106,
			ColorRes        = 108,
			PhysicalWidth   = 110,
			PhysicalHeight  = 111,
			PhysicalOffsetX = 112,
			PhysicalOffsetY = 113,
			ScalingFactorX  = 114,
			ScalingFactorY  = 115,
			VRefresh        = 116,
			DesktopVertRes  = 117,
			DesktopHorzRes  = 118,
			BltAlignment    = 119,
			ShadeBlendCaps  = 120,
			ColorMgmtCaps   = 121
		}

		#endregion

		#region Types

		public struct PaintStruct
		{
			public IntPtr HDC;
			public bool Erase;
			public Rectangle RectPaint;
			public bool Reserved1;
			public bool Reserved2;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			public byte[] Reserved3;
		}

		#endregion

		#region Methods

		[DllImport("gdi32.dll", EntryPoint = "CreateDC")]
		public static extern IntPtr DC_CreateA(
			[In][MarshalAs(UnmanagedType.LPStr)] string driver,
			[In][MarshalAs(UnmanagedType.LPStr)] string device,
			[In][MarshalAs(UnmanagedType.LPStr)] string reserved,
			[In] IntPtr initData);

		[DllImport("gdi32.dll", EntryPoint = "CreateDC")]
		public static extern IntPtr DC_CreateW(
			[In][MarshalAs(UnmanagedType.LPWStr)] string driver,
			[In][MarshalAs(UnmanagedType.LPWStr)] string device,
			[In][MarshalAs(UnmanagedType.LPWStr)] string reserved,
			[In] IntPtr initData);

		[DllImport("gdi32.dll", EntryPoint = "CreateDC")]
		public static extern bool DC_Delete(
			[In] IntPtr hDC);

		[DllImport("user32.dll", EntryPoint = "BeginPaint")]
		public static extern IntPtr Paint_Begin(
			[In] IntPtr hWnd,
			[In][Out] ref PaintStruct paintStruct);

		[DllImport("user32.dll", EntryPoint = "EndPaint")]
		public static extern bool Paint_End(
			[In] IntPtr hWnd,
			[In][Out] ref PaintStruct paintStruct);

		[DllImport("gdi32.dll", EntryPoint = "GetDeviceCaps")]
		public static extern int DC_GetCaps(
			[In] IntPtr hDC,
			[In][MarshalAs(UnmanagedType.U4)] DeviceCapIndex index);

		#endregion

		#endregion

		#region Drawing

		#region Constants

		public enum FrameControlType : int
		{
			Caption   = 1,
			Menu      = 2,
			Scroll    = 3,
			Button    = 4,
			PopUpMenu = 5
		}

		[Flags]
			public enum FrameControlState : int
		{
			CaptionClose        = 0x0000,
			CaptionMin          = 0x0001,
			CaptionMax          = 0x0002,
			CaptionRestore      = 0x0003,
			CaptionHelp         = 0x0004,
			MenuArrow           = 0x0000,
			MenuCheck           = 0x0001,
			MenuBullet          = 0x0002,
			MenuArrowRight      = 0x0004,
			ScrollUp            = 0x0000,
			ScrollDown          = 0x0001,
			ScrollLeft          = 0x0002,
			ScrollRight         = 0x0003,
			ScrollComboBox      = 0x0005,
			ScrollSizeGrip      = 0x0008,
			ScrollSizeGripRight = 0x0010,
			ButtonCheck         = 0x0000,
			ButtonRadioImage    = 0x0001,
			ButtonRadioMask     = 0x0002,
			ButtonRadio         = 0x0004,
			Button3State        = 0x0008,
			ButtonPush          = 0x0010,
			Inactive            = 0x0100,
			Pushed              = 0x0200,
			Checked             = 0x0400,
			Transparent         = 0x0800,
			Hot                 = 0x1000,
			AdjustRect          = 0x2000,
			Flat                = 0x4000,
			Mono                = 0x8000
		}

		#endregion

		#region Methods

		[DllImport("user32.dll", EntryPoint = "DrawFrameControl")]
		public static extern bool FrameControl_Draw(
			[In] IntPtr hDC,
			[In][Out] ref Rectangle rect,
			[In][MarshalAs(UnmanagedType.U4)] FrameControlType type,
			[In][MarshalAs(UnmanagedType.U4)] FrameControlState state);

		#endregion

		#endregion

		#region Fonts

		#region Types

		[StructLayout(LayoutKind.Sequential, Pack=4)]
			public struct LogFontA
		{
			public int Height;
			public int Width;
			public int Escapement;
			public int Orientation;
			public int Weight;
			public byte Italic;
			public byte Underline;
			public byte StrikeOut;
			public byte CharSet;
			public byte OutPrecision;
			public byte ClipPrecision;
			public byte Quality;
			public byte PitchAndFamily;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
			public char[] FaceName;
		}

		[StructLayout(LayoutKind.Sequential, Pack=4)]
			public struct LogFontW
		{
			public int Height;
			public int Width;
			public int Escapement;
			public int Orientation;
			public int Weight;
			public byte Italic;
			public byte Underline;
			public byte StrikeOut;
			public byte CharSet;
			public byte OutPrecision;
			public byte ClipPrecision;
			public byte Quality;
			public byte PitchAndFamily;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
			public ushort[] FaceName;
		}

		#endregion

		#region Methods

		[DllImport("gdi32.dll", EntryPoint = "CreateFontA")]
		public static extern IntPtr Font_CreateA(
			[In] int height,
			[In] int width,
			[In] int escapement,
			[In] int orientation,
			[In] int weight,
			[In] int italic,
			[In] int underline,
			[In] int strikeOut,
			[In] int charSet,
			[In] int outputPrecision,
			[In] int clipPrecision,
			[In] int quality,
			[In] int pitchAndFamily,
			[In][MarshalAs(UnmanagedType.LPStr)] string lpszFace);

		[DllImport("gdi32.dll", EntryPoint = "CreateFontW")]
		public static extern IntPtr Font_CreateW(
			[In] int height,
			[In] int width,
			[In] int escapement,
			[In] int orientation,
			[In] int weight,
			[In] int italic,
			[In] int underline,
			[In] int strikeOut,
			[In] int charSet,
			[In] int outputPrecision,
			[In] int clipPrecision,
			[In] int quality,
			[In] int pitchAndFamily,
			[In][MarshalAs(UnmanagedType.LPStr)] string lpszFace);

		[DllImport("gdi32.dll", EntryPoint = "CreateFontIndirectA")]
		public static extern IntPtr Font_CreateA(
			[In] ref LogFontA logFont);

		[DllImport("gdi32.dll", EntryPoint = "CreateFontIndirectW")]
		public static extern IntPtr Font_CreateW(
			[In] ref LogFontW logFont);

		#endregion

		#endregion

		#region Keyboard

		#region Constants

		public enum VirtualKey : int
		{
			VK_LBUTTON               = 0x01,
			VK_RBUTTON               = 0x02,
			VK_CANCEL                = 0x03,
			VK_MBUTTON               = 0x04,
			VK_XBUTTON1              = 0x05,
			VK_XBUTTON2              = 0x06,
			VK_BACK                  = 0x08,
			VK_TAB                   = 0x09,
			VK_CLEAR                 = 0x0C,
			VK_RETURN                = 0x0D,
			VK_SHIFT                 = 0x10,
			VK_CONTROL               = 0x11,
			VK_MENU                  = 0x12,
			VK_PAUSE                 = 0x13,
			VK_CAPITAL               = 0x14,
			VK_KANA                  = 0x15,
			VK_HANGEUL               = 0x15,
			VK_HANGUL                = 0x15,
			VK_JUNJA                 = 0x17,
			VK_FINAL                 = 0x18,
			VK_HANJA                 = 0x19,
			VK_KANJI                 = 0x19,
			VK_ESCAPE                = 0x1B,
			VK_CONVERT               = 0x1C,
			VK_NONCONVERT            = 0x1D,
			VK_ACCEPT                = 0x1E,
			VK_MODECHANGE            = 0x1F,
			VK_SPACE                 = 0x20,
			VK_PRIOR                 = 0x21,
			VK_NEXT                  = 0x22,
			VK_END                   = 0x23,
			VK_HOME                  = 0x24,
			VK_LEFT                  = 0x25,
			VK_UP                    = 0x26,
			VK_RIGHT                 = 0x27,
			VK_DOWN                  = 0x28,
			VK_SELECT                = 0x29,
			VK_PRINT                 = 0x2A,
			VK_EXECUTE               = 0x2B,
			VK_SNAPSHOT              = 0x2C,
			VK_INSERT                = 0x2D,
			VK_DELETE                = 0x2E,
			VK_HELP                  = 0x2F,
			VK_0                     = 0x30,
			VK_1                     = 0x31,
			VK_2                     = 0x32,
			VK_3                     = 0x33,
			VK_4                     = 0x34,
			VK_5                     = 0x35,
			VK_6                     = 0x36,
			VK_7                     = 0x37,
			VK_8                     = 0x38,
			VK_9                     = 0x39,
			VK_A                     = 0x41,
			VK_B                     = 0x42,
			VK_C                     = 0x43,
			VK_D                     = 0x44,
			VK_E                     = 0x45,
			VK_F                     = 0x46,
			VK_G                     = 0x47,
			VK_H                     = 0x48,
			VK_I                     = 0x49,
			VK_J                     = 0x4A,
			VK_K                     = 0x4B,
			VK_L                     = 0x4C,
			VK_M                     = 0x4D,
			VK_N                     = 0x4E,
			VK_O                     = 0x4F,
			VK_P                     = 0x50,
			VK_Q                     = 0x51,
			VK_R                     = 0x52,
			VK_S                     = 0x53,
			VK_T                     = 0x54,
			VK_U                     = 0x55,
			VK_V                     = 0x56,
			VK_W                     = 0x57,
			VK_X                     = 0x58,
			VK_Y                     = 0x59,
			VK_Z                     = 0x5A,
			VK_LWIN                  = 0x5B,
			VK_RWIN                  = 0x5C,
			VK_APPS                  = 0x5D,
			VK_SLEEP                 = 0x5F,
			VK_NUMPAD0               = 0x60,
			VK_NUMPAD1               = 0x61,
			VK_NUMPAD2               = 0x62,
			VK_NUMPAD3               = 0x63,
			VK_NUMPAD4               = 0x64,
			VK_NUMPAD5               = 0x65,
			VK_NUMPAD6               = 0x66,
			VK_NUMPAD7               = 0x67,
			VK_NUMPAD8               = 0x68,
			VK_NUMPAD9               = 0x69,
			VK_MULTIPLY              = 0x6A,
			VK_ADD                   = 0x6B,
			VK_SEPARATOR             = 0x6C,
			VK_SUBTRACT              = 0x6D,
			VK_DECIMAL               = 0x6E,
			VK_DIVIDE                = 0x6F,
			VK_F1                    = 0x70,
			VK_F2                    = 0x71,
			VK_F3                    = 0x72,
			VK_F4                    = 0x73,
			VK_F5                    = 0x74,
			VK_F6                    = 0x75,
			VK_F7                    = 0x76,
			VK_F8                    = 0x77,
			VK_F9                    = 0x78,
			VK_F10                   = 0x79,
			VK_F11                   = 0x7A,
			VK_F12                   = 0x7B,
			VK_F13                   = 0x7C,
			VK_F14                   = 0x7D,
			VK_F15                   = 0x7E,
			VK_F16                   = 0x7F,
			VK_F17                   = 0x80,
			VK_F18                   = 0x81,
			VK_F19                   = 0x82,
			VK_F20                   = 0x83,
			VK_F21                   = 0x84,
			VK_F22                   = 0x85,
			VK_F23                   = 0x86,
			VK_F24                   = 0x87,
			VK_NUMLOCK               = 0x90,
			VK_SCROLL                = 0x91,
			VK_OEM_NEC_EQUAL         = 0x92,
			VK_OEM_FJ_JISHO          = 0x92,
			VK_OEM_FJ_MASSHOU        = 0x93,
			VK_OEM_FJ_TOUROKU        = 0x94,
			VK_OEM_FJ_LOYA           = 0x95,
			VK_OEM_FJ_ROYA           = 0x96,
			VK_LSHIFT                = 0xA0,
			VK_RSHIFT                = 0xA1,
			VK_LCONTROL              = 0xA2,
			VK_RCONTROL              = 0xA3,
			VK_LMENU                 = 0xA4,
			VK_RMENU                 = 0xA5,
			VK_BROWSER_BACK          = 0xA6,
			VK_BROWSER_FORWARD       = 0xA7,
			VK_BROWSER_REFRESH       = 0xA8,
			VK_BROWSER_STOP          = 0xA9,
			VK_BROWSER_SEARCH        = 0xAA,
			VK_BROWSER_FAVORITES     = 0xAB,
			VK_BROWSER_HOME          = 0xAC,
			VK_VOLUME_MUTE           = 0xAD,
			VK_VOLUME_DOWN           = 0xAE,
			VK_VOLUME_UP             = 0xAF,
			VK_MEDIA_NEXT_TRACK      = 0xB0,
			VK_MEDIA_PREV_TRACK      = 0xB1,
			VK_MEDIA_STOP            = 0xB2,
			VK_MEDIA_PLAY_PAUSE      = 0xB3,
			VK_LAUNCH_MAIL           = 0xB4,
			VK_LAUNCH_MEDIA_SELECT   = 0xB5,
			VK_LAUNCH_APP1           = 0xB6,
			VK_LAUNCH_APP2           = 0xB7,
			VK_OEM_1                 = 0xBA,
			VK_OEM_PLUS              = 0xBB,
			VK_OEM_COMMA             = 0xBC,
			VK_OEM_MINUS             = 0xBD,
			VK_OEM_PERIOD            = 0xBE,
			VK_OEM_2                 = 0xBF,
			VK_OEM_3                 = 0xC0,
			VK_OEM_4                 = 0xDB,
			VK_OEM_5                 = 0xDC,
			VK_OEM_6                 = 0xDD,
			VK_OEM_7                 = 0xDE,
			VK_OEM_8                 = 0xDF,
			VK_OEM_AX                = 0xE1,
			VK_OEM_102               = 0xE2,
			VK_ICO_HELP              = 0xE3,
			VK_ICO_00                = 0xE4,
			VK_PROCESSKEY            = 0xE5,
			VK_ICO_CLEAR             = 0xE6,
			VK_PACKET                = 0xE7,
			VK_OEM_RESET             = 0xE9,
			VK_OEM_JUMP              = 0xEA,
			VK_OEM_PA1               = 0xEB,
			VK_OEM_PA2               = 0xEC,
			VK_OEM_PA3               = 0xED,
			VK_OEM_WSCTRL            = 0xEE,
			VK_OEM_CUSEL             = 0xEF,
			VK_OEM_ATTN              = 0xF0,
			VK_OEM_FINISH            = 0xF1,
			VK_OEM_COPY              = 0xF2,
			VK_OEM_AUTO              = 0xF3,
			VK_OEM_ENLW              = 0xF4,
			VK_OEM_BACKTAB           = 0xF5,
			VK_ATTN                  = 0xF6,
			VK_CRSEL                 = 0xF7,
			VK_EXSEL                 = 0xF8,
			VK_EREOF                 = 0xF9,
			VK_PLAY                  = 0xFA,
			VK_ZOOM                  = 0xFB,
			VK_NONAME                = 0xFC,
			VK_PA1                   = 0xFD,
			VK_OEM_CLEAR             = 0xFE
		}

		#endregion

		#region Methods

		[DllImport("user32.dll", EntryPoint = "GetKeyState")]
		public static extern short Key_StateGet(
			[In][MarshalAs(UnmanagedType.U4)] VirtualKey virtualKey);

		#endregion

		#endregion

		#region Clipboard

		#region Methods

		[DllImport("user32.dll", EntryPoint = "RegisterClipboardFormatA")]
		public static extern short Clipboard_FormatRegisterA(
			[In][MarshalAs(UnmanagedType.LPStr)] string format);

		[DllImport("user32.dll", EntryPoint = "RegisterClipboardFormatW")]
		public static extern short Clipboard_FormatRegisterW(
			[In][MarshalAs(UnmanagedType.LPWStr)] string format);

		#endregion

		#endregion

		#region Caret

		#region Methods

		[DllImport("user32.dll", EntryPoint = "CreateCaret")]
		public static extern bool Caret_Create(
			[In] IntPtr hWnd,
			[In] IntPtr hBitmap,
			[In] int width,
			[In] int height);

		[DllImport("user32.dll", EntryPoint = "DestroyCaret")]
		public static extern bool Caret_Destroy();

		[DllImport("user32.dll", EntryPoint = "GetCaretPos")]
		public static extern bool Caret_PositionGet(
			[Out] out Point position);

		[DllImport("user32.dll", EntryPoint = "SetCaretPos")]
		public static extern bool Caret_PositionSet(
			[In] int x,
			[In] int y);

		[DllImport("user32.dll", EntryPoint = "ShowCaret")]
		public static extern bool Caret_Show(
			[In] IntPtr hWnd);

		[DllImport("user32.dll", EntryPoint = "HideCaret")]
		public static extern bool Caret_Hide(
			[In] IntPtr hWnd);

		#endregion

		#endregion
	}
}
