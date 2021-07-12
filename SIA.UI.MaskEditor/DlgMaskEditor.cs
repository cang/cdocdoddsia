#define SIA_PRODUCT

using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Security;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;

using SIA.SystemLayer;
using SiGlaz.Common;

using SIA.UI.Components;
using SIA.UI.Components.Renders;

using SIA.UI.MaskEditor;
using SIA.UI.MaskEditor.DocToolkit;
using SIA.UI.MaskEditor.DrawTools;
using SIA.UI.MaskEditor.UserControls;
using SIA.UI.MaskEditor.UserControls.ColorPicker;

using SIA.Common.IPLFacade;
using SiGlaz.Common.ImageAlignment;

namespace SIA.UI.MaskEditor
{
	public delegate void ApplyMask(object sender, MaskEditorApplyMaskEventArgs args);
	/// <summary>
	/// Summary description for MaskEditor.
	/// </summary>
    public class MaskEditor : System.Windows.Forms.Form, SiGlaz.Common.IMaskEditor
	{
		#region Windows Form member attributes

		private kColorPicker colorPicker;
		private System.Windows.Forms.ImageList mainImageList;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Panel docContainer;
		private System.Windows.Forms.Panel panelColorPicker;
		private System.Windows.Forms.NumericUpDown nudAlpha;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ToolBar mainToolBar;
		private System.Windows.Forms.ToolBarButton tbNew;
		private System.Windows.Forms.ToolBarButton tbOpen;
		private System.Windows.Forms.ToolBarButton tbSave;
		private System.Windows.Forms.ToolBarButton tbSeparator1;
		private System.Windows.Forms.ToolBarButton tbPointer;
		private System.Windows.Forms.ToolBarButton tbRectangle;
		private System.Windows.Forms.ToolBarButton tbEllipse;
		private System.Windows.Forms.ToolBarButton tbPolygon;
		private System.Windows.Forms.ToolBarButton tbOnionRing;
		private System.Windows.Forms.ToolBarButton tbSeparator2;
		private System.Windows.Forms.ToolBarButton tbZoomIn;
		private System.Windows.Forms.ToolBarButton tbZoomOut;
		private System.Windows.Forms.ToolBarButton tbPan;
		private System.Windows.Forms.ToolBarButton tbSeparator3;
		private System.Windows.Forms.ToolBarButton tbFitOnScreen;
		private System.Windows.Forms.ToolBarButton tbActualSize;
		private System.Windows.Forms.ToolBarButton tbSeparator4;
		private System.Windows.Forms.ToolBarButton tbUndo;
		private System.Windows.Forms.ToolBarButton tbRedo;
		private System.Windows.Forms.ToolBarButton tbCopy;
		private System.Windows.Forms.ToolBarButton tbCut;
		private System.Windows.Forms.ToolBarButton tbPaste;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem14;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem menuItem25;
		private System.Windows.Forms.MenuItem menuItem28;
		private System.Windows.Forms.MenuItem menuItem37;
		private System.Windows.Forms.MenuItem mnuView;
		private System.Windows.Forms.MenuItem mnuViewPan;
		private System.Windows.Forms.MenuItem mnuZoomIN;
		private System.Windows.Forms.MenuItem mnuZoomOut;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem mnuFitOnScreen;
		private System.Windows.Forms.MenuItem mnuActualSize;
		private System.Windows.Forms.MenuItem mnuFile;
		private System.Windows.Forms.MenuItem mnuFileNew;
		private System.Windows.Forms.MenuItem mnuEdit;
		private System.Windows.Forms.MenuItem mnuEditUndo;
		private System.Windows.Forms.MenuItem mnuEditRedo;
		private System.Windows.Forms.MenuItem mnuEditSelectAll;
		private System.Windows.Forms.MenuItem mnuEditDelete;
		private System.Windows.Forms.MenuItem mnuEditCut;
		private System.Windows.Forms.MenuItem mnuEditCopy;
		private System.Windows.Forms.MenuItem mnuEditPaste;
		private System.Windows.Forms.MenuItem mnuDraw;
		private System.Windows.Forms.MenuItem mnuDrawSelect;
		private System.Windows.Forms.MenuItem mnuDrawRectangle;
		private System.Windows.Forms.MenuItem mnuDrawEllipse;
		private System.Windows.Forms.MenuItem mnuDrawPolygon;
		private System.Windows.Forms.MenuItem mnuDrawOnionRing;
		private System.Windows.Forms.MenuItem mnuFileOpen;
		private System.Windows.Forms.MenuItem mnuFileSave;
		private System.Windows.Forms.MenuItem mnuFileSaveAs;
		private System.Windows.Forms.MenuItem mnuFileClose;
		private System.Windows.Forms.MenuItem mnuFileApply;
		private System.Windows.Forms.MenuItem mnuEditBringToFront;
		private System.Windows.Forms.MenuItem mnuEditSendToBack;
		private System.Windows.Forms.MenuItem menuItem34;
		private System.Windows.Forms.MenuItem menuItem29;
		private System.Windows.Forms.ContextMenu contextMenu;
		private System.Windows.Forms.MenuItem ctxSelectAll;
		private System.Windows.Forms.MenuItem ctxDelete;
		private System.Windows.Forms.MenuItem ctxCut;
		private System.Windows.Forms.MenuItem ctxCopy;
		private System.Windows.Forms.MenuItem ctxPaste;
		private System.Windows.Forms.MenuItem ctxBringToFront;
		private System.Windows.Forms.MenuItem ctxSendToBack;
		#endregion

		#region internal member attribute
		private IRasterImageRender _render = null;
		private CommonImage _image = null;
		private string fileName = "";
		private DrawArea _workspace;
		private DocManager _docManager = null;
        private MenuItem menuItem1;
        private MenuItem ctxDescription;
        private MenuItem menuItem2;
        private MenuItem mnuEditDescription;
        private Label lbCurrentCoord;
        private MenuItem ctxAutoVertex;
		private bool bAppliedMask = false;
        
        public MetrologySystem MetrologySystem
        {
            get 
            {
                if (_workspace == null)
                    return null;

                return _workspace.MetrologySystem;
            }
            set
            {
                if (_workspace != null)
                {
                    _workspace.MetrologySystem = value;
                }
            }
        }
		#endregion

		#region public event handlers
		public event ApplyMask ApplyMask = null;
		public event EventHandler MaskChanged = null;
		public event EventHandler AppliedMaskChanged = null;
		#endregion

		#region constructor and destructor

		public MaskEditor(CommonImage image, IRasterImageRender render)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// initialize internal image
			if (image == null)
				throw new ArgumentNullException("invalid image parameter");
			this._image = image;	

			// initialize internal image viewer
			if (render == null)
				throw new ArgumentNullException("invalid render parameter");
			this._render = render;
		}

		protected MaskEditor(SIA.SystemLayer.CommonImage image)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			if (image == null)
				throw new ArgumentNullException("invalid parameter");
			this._image = image;			
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion		
		
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MaskEditor));
            this.mainImageList = new System.Windows.Forms.ImageList(this.components);
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.mnuFile = new System.Windows.Forms.MenuItem();
            this.mnuFileNew = new System.Windows.Forms.MenuItem();
            this.mnuFileOpen = new System.Windows.Forms.MenuItem();
            this.mnuFileSave = new System.Windows.Forms.MenuItem();
            this.mnuFileSaveAs = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.mnuFileApply = new System.Windows.Forms.MenuItem();
            this.menuItem14 = new System.Windows.Forms.MenuItem();
            this.mnuFileClose = new System.Windows.Forms.MenuItem();
            this.mnuEdit = new System.Windows.Forms.MenuItem();
            this.mnuEditUndo = new System.Windows.Forms.MenuItem();
            this.mnuEditRedo = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.mnuEditSelectAll = new System.Windows.Forms.MenuItem();
            this.mnuEditDelete = new System.Windows.Forms.MenuItem();
            this.menuItem25 = new System.Windows.Forms.MenuItem();
            this.mnuEditCut = new System.Windows.Forms.MenuItem();
            this.mnuEditCopy = new System.Windows.Forms.MenuItem();
            this.mnuEditPaste = new System.Windows.Forms.MenuItem();
            this.menuItem28 = new System.Windows.Forms.MenuItem();
            this.mnuEditBringToFront = new System.Windows.Forms.MenuItem();
            this.mnuEditSendToBack = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.mnuEditDescription = new System.Windows.Forms.MenuItem();
            this.mnuView = new System.Windows.Forms.MenuItem();
            this.mnuViewPan = new System.Windows.Forms.MenuItem();
            this.mnuZoomIN = new System.Windows.Forms.MenuItem();
            this.mnuZoomOut = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.mnuFitOnScreen = new System.Windows.Forms.MenuItem();
            this.mnuActualSize = new System.Windows.Forms.MenuItem();
            this.mnuDraw = new System.Windows.Forms.MenuItem();
            this.mnuDrawSelect = new System.Windows.Forms.MenuItem();
            this.menuItem37 = new System.Windows.Forms.MenuItem();
            this.mnuDrawRectangle = new System.Windows.Forms.MenuItem();
            this.mnuDrawEllipse = new System.Windows.Forms.MenuItem();
            this.mnuDrawPolygon = new System.Windows.Forms.MenuItem();
            this.mnuDrawOnionRing = new System.Windows.Forms.MenuItem();
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.ctxSelectAll = new System.Windows.Forms.MenuItem();
            this.ctxDelete = new System.Windows.Forms.MenuItem();
            this.menuItem34 = new System.Windows.Forms.MenuItem();
            this.ctxCut = new System.Windows.Forms.MenuItem();
            this.ctxCopy = new System.Windows.Forms.MenuItem();
            this.ctxPaste = new System.Windows.Forms.MenuItem();
            this.menuItem29 = new System.Windows.Forms.MenuItem();
            this.ctxBringToFront = new System.Windows.Forms.MenuItem();
            this.ctxSendToBack = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.ctxDescription = new System.Windows.Forms.MenuItem();
            this.ctxAutoVertex = new System.Windows.Forms.MenuItem();
            this.docContainer = new System.Windows.Forms.Panel();
            this._workspace = new SIA.UI.MaskEditor.DrawTools.DrawArea();
            this.mainToolBar = new System.Windows.Forms.ToolBar();
            this.tbNew = new System.Windows.Forms.ToolBarButton();
            this.tbOpen = new System.Windows.Forms.ToolBarButton();
            this.tbSave = new System.Windows.Forms.ToolBarButton();
            this.tbSeparator1 = new System.Windows.Forms.ToolBarButton();
            this.tbUndo = new System.Windows.Forms.ToolBarButton();
            this.tbRedo = new System.Windows.Forms.ToolBarButton();
            this.tbSeparator3 = new System.Windows.Forms.ToolBarButton();
            this.tbCopy = new System.Windows.Forms.ToolBarButton();
            this.tbCut = new System.Windows.Forms.ToolBarButton();
            this.tbPaste = new System.Windows.Forms.ToolBarButton();
            this.tbSeparator2 = new System.Windows.Forms.ToolBarButton();
            this.tbPointer = new System.Windows.Forms.ToolBarButton();
            this.tbPan = new System.Windows.Forms.ToolBarButton();
            this.tbZoomIn = new System.Windows.Forms.ToolBarButton();
            this.tbZoomOut = new System.Windows.Forms.ToolBarButton();
            this.tbFitOnScreen = new System.Windows.Forms.ToolBarButton();
            this.tbActualSize = new System.Windows.Forms.ToolBarButton();
            this.tbSeparator4 = new System.Windows.Forms.ToolBarButton();
            this.tbRectangle = new System.Windows.Forms.ToolBarButton();
            this.tbEllipse = new System.Windows.Forms.ToolBarButton();
            this.tbPolygon = new System.Windows.Forms.ToolBarButton();
            this.tbOnionRing = new System.Windows.Forms.ToolBarButton();
            this.panelColorPicker = new System.Windows.Forms.Panel();
            this.lbCurrentCoord = new System.Windows.Forms.Label();
            this.nudAlpha = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.colorPicker = new SIA.UI.MaskEditor.UserControls.ColorPicker.kColorPicker();
            this.docContainer.SuspendLayout();
            this.panelColorPicker.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAlpha)).BeginInit();
            this.SuspendLayout();
            // 
            // mainImageList
            // 
            this.mainImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("mainImageList.ImageStream")));
            this.mainImageList.TransparentColor = System.Drawing.Color.White;
            this.mainImageList.Images.SetKeyName(0, "");
            this.mainImageList.Images.SetKeyName(1, "");
            this.mainImageList.Images.SetKeyName(2, "");
            this.mainImageList.Images.SetKeyName(3, "");
            this.mainImageList.Images.SetKeyName(4, "");
            this.mainImageList.Images.SetKeyName(5, "");
            this.mainImageList.Images.SetKeyName(6, "");
            this.mainImageList.Images.SetKeyName(7, "");
            this.mainImageList.Images.SetKeyName(8, "");
            this.mainImageList.Images.SetKeyName(9, "");
            this.mainImageList.Images.SetKeyName(10, "");
            this.mainImageList.Images.SetKeyName(11, "");
            this.mainImageList.Images.SetKeyName(12, "");
            this.mainImageList.Images.SetKeyName(13, "");
            this.mainImageList.Images.SetKeyName(14, "");
            this.mainImageList.Images.SetKeyName(15, "");
            this.mainImageList.Images.SetKeyName(16, "");
            this.mainImageList.Images.SetKeyName(17, "");
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFile,
            this.mnuEdit,
            this.mnuView,
            this.mnuDraw});
            // 
            // mnuFile
            // 
            this.mnuFile.Index = 0;
            this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFileNew,
            this.mnuFileOpen,
            this.mnuFileSave,
            this.mnuFileSaveAs,
            this.menuItem5,
            this.mnuFileApply,
            this.menuItem14,
            this.mnuFileClose});
            this.mnuFile.Text = "&File";
            // 
            // mnuFileNew
            // 
            this.mnuFileNew.Index = 0;
            this.mnuFileNew.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
            this.mnuFileNew.Text = "&New Mask";
            // 
            // mnuFileOpen
            // 
            this.mnuFileOpen.Index = 1;
            this.mnuFileOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.mnuFileOpen.Text = "&Open...";
            // 
            // mnuFileSave
            // 
            this.mnuFileSave.Index = 2;
            this.mnuFileSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.mnuFileSave.Text = "&Save";
            // 
            // mnuFileSaveAs
            // 
            this.mnuFileSaveAs.Index = 3;
            this.mnuFileSaveAs.Text = "Save &As...";
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 4;
            this.menuItem5.Text = "-";
            // 
            // mnuFileApply
            // 
            this.mnuFileApply.Index = 5;
            this.mnuFileApply.Text = "Apply...";
            // 
            // menuItem14
            // 
            this.menuItem14.Index = 6;
            this.menuItem14.Text = "-";
            // 
            // mnuFileClose
            // 
            this.mnuFileClose.Index = 7;
            this.mnuFileClose.Text = "&Close";
            // 
            // mnuEdit
            // 
            this.mnuEdit.Index = 1;
            this.mnuEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuEditUndo,
            this.mnuEditRedo,
            this.menuItem10,
            this.mnuEditSelectAll,
            this.mnuEditDelete,
            this.menuItem25,
            this.mnuEditCut,
            this.mnuEditCopy,
            this.mnuEditPaste,
            this.menuItem28,
            this.mnuEditBringToFront,
            this.mnuEditSendToBack,
            this.menuItem2,
            this.mnuEditDescription});
            this.mnuEdit.Text = "&Edit";
            // 
            // mnuEditUndo
            // 
            this.mnuEditUndo.Index = 0;
            this.mnuEditUndo.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
            this.mnuEditUndo.Text = "&Undo";
            // 
            // mnuEditRedo
            // 
            this.mnuEditRedo.Index = 1;
            this.mnuEditRedo.Shortcut = System.Windows.Forms.Shortcut.CtrlY;
            this.mnuEditRedo.Text = "&Redo";
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 2;
            this.menuItem10.Text = "-";
            // 
            // mnuEditSelectAll
            // 
            this.mnuEditSelectAll.Index = 3;
            this.mnuEditSelectAll.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
            this.mnuEditSelectAll.Text = "Select All";
            // 
            // mnuEditDelete
            // 
            this.mnuEditDelete.Index = 4;
            this.mnuEditDelete.Text = "Delete";
            // 
            // menuItem25
            // 
            this.menuItem25.Index = 5;
            this.menuItem25.Text = "-";
            // 
            // mnuEditCut
            // 
            this.mnuEditCut.Index = 6;
            this.mnuEditCut.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
            this.mnuEditCut.Text = "&Cut";
            // 
            // mnuEditCopy
            // 
            this.mnuEditCopy.Index = 7;
            this.mnuEditCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.mnuEditCopy.Text = "C&opy";
            // 
            // mnuEditPaste
            // 
            this.mnuEditPaste.Index = 8;
            this.mnuEditPaste.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
            this.mnuEditPaste.Text = "&Paste";
            // 
            // menuItem28
            // 
            this.menuItem28.Index = 9;
            this.menuItem28.Text = "-";
            // 
            // mnuEditBringToFront
            // 
            this.mnuEditBringToFront.Index = 10;
            this.mnuEditBringToFront.Text = "Bring to Front";
            // 
            // mnuEditSendToBack
            // 
            this.mnuEditSendToBack.Index = 11;
            this.mnuEditSendToBack.Text = "Send to Back";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 12;
            this.menuItem2.Text = "-";
            // 
            // mnuEditDescription
            // 
            this.mnuEditDescription.Index = 13;
            this.mnuEditDescription.Text = "Description...";
            // 
            // mnuView
            // 
            this.mnuView.Index = 2;
            this.mnuView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuViewPan,
            this.mnuZoomIN,
            this.mnuZoomOut,
            this.menuItem6,
            this.mnuFitOnScreen,
            this.mnuActualSize});
            this.mnuView.Text = "&View";
            // 
            // mnuViewPan
            // 
            this.mnuViewPan.Index = 0;
            this.mnuViewPan.Text = "&Pan";
            // 
            // mnuZoomIN
            // 
            this.mnuZoomIN.Index = 1;
            this.mnuZoomIN.Text = "&Zoom In";
            // 
            // mnuZoomOut
            // 
            this.mnuZoomOut.Index = 2;
            this.mnuZoomOut.Text = "Zoom &Out";
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 3;
            this.menuItem6.Text = "-";
            // 
            // mnuFitOnScreen
            // 
            this.mnuFitOnScreen.Index = 4;
            this.mnuFitOnScreen.Text = "&Fit On Screen";
            // 
            // mnuActualSize
            // 
            this.mnuActualSize.Index = 5;
            this.mnuActualSize.Text = "&Actual Size";
            // 
            // mnuDraw
            // 
            this.mnuDraw.Index = 3;
            this.mnuDraw.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuDrawSelect,
            this.menuItem37,
            this.mnuDrawRectangle,
            this.mnuDrawEllipse,
            this.mnuDrawPolygon,
            this.mnuDrawOnionRing});
            this.mnuDraw.Text = "&Draw";
            // 
            // mnuDrawSelect
            // 
            this.mnuDrawSelect.Index = 0;
            this.mnuDrawSelect.Text = "&Select";
            // 
            // menuItem37
            // 
            this.menuItem37.Index = 1;
            this.menuItem37.Text = "-";
            // 
            // mnuDrawRectangle
            // 
            this.mnuDrawRectangle.Index = 2;
            this.mnuDrawRectangle.Text = "&Rectangle";
            this.mnuDrawRectangle.Visible = false;
            // 
            // mnuDrawEllipse
            // 
            this.mnuDrawEllipse.Index = 3;
            this.mnuDrawEllipse.Text = "&Ellipse";
            this.mnuDrawEllipse.Visible = false;
            // 
            // mnuDrawPolygon
            // 
            this.mnuDrawPolygon.Index = 4;
            this.mnuDrawPolygon.Text = "&Polygon";
            // 
            // mnuDrawOnionRing
            // 
            this.mnuDrawOnionRing.Index = 5;
            this.mnuDrawOnionRing.Text = "&Onion Ring";
            this.mnuDrawOnionRing.Visible = false;
            // 
            // contextMenu
            // 
            this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.ctxSelectAll,
            this.ctxDelete,
            this.menuItem34,
            this.ctxCut,
            this.ctxCopy,
            this.ctxPaste,
            this.menuItem29,
            this.ctxBringToFront,
            this.ctxSendToBack,
            this.menuItem1,
            this.ctxDescription,
            this.ctxAutoVertex});
            this.contextMenu.Popup += new System.EventHandler(this.MenuItem_PopUp);
            // 
            // ctxSelectAll
            // 
            this.ctxSelectAll.Index = 0;
            this.ctxSelectAll.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
            this.ctxSelectAll.Text = "Select All";
            // 
            // ctxDelete
            // 
            this.ctxDelete.Index = 1;
            this.ctxDelete.Shortcut = System.Windows.Forms.Shortcut.Del;
            this.ctxDelete.Text = "Delete";
            // 
            // menuItem34
            // 
            this.menuItem34.Index = 2;
            this.menuItem34.Text = "-";
            // 
            // ctxCut
            // 
            this.ctxCut.Index = 3;
            this.ctxCut.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
            this.ctxCut.Text = "&Cut";
            // 
            // ctxCopy
            // 
            this.ctxCopy.Index = 4;
            this.ctxCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.ctxCopy.Text = "C&opy";
            // 
            // ctxPaste
            // 
            this.ctxPaste.Index = 5;
            this.ctxPaste.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
            this.ctxPaste.Text = "&Paste";
            // 
            // menuItem29
            // 
            this.menuItem29.Index = 6;
            this.menuItem29.Text = "-";
            // 
            // ctxBringToFront
            // 
            this.ctxBringToFront.Index = 7;
            this.ctxBringToFront.Text = "Bring to &Front";
            // 
            // ctxSendToBack
            // 
            this.ctxSendToBack.Index = 8;
            this.ctxSendToBack.Text = "Send to &Back";
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 9;
            this.menuItem1.Text = "-";
            // 
            // ctxDescription
            // 
            this.ctxDescription.Index = 10;
            this.ctxDescription.Text = "Description...";
            // 
            // ctxAutoVertex
            // 
            this.ctxAutoVertex.Index = 11;
            this.ctxAutoVertex.Text = "Auto-vertex";
            this.ctxAutoVertex.Visible = false;
            // 
            // docContainer
            // 
            this.docContainer.BackColor = System.Drawing.SystemColors.Control;
            this.docContainer.Controls.Add(this._workspace);
            this.docContainer.Controls.Add(this.mainToolBar);
            this.docContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.docContainer.Location = new System.Drawing.Point(0, 0);
            this.docContainer.Name = "docContainer";
            this.docContainer.Size = new System.Drawing.Size(704, 377);
            this.docContainer.TabIndex = 1;
            // 
            // _workspace
            // 
            this._workspace.AutoDisposeImages = false;
            this._workspace.AutoResetScaleFactor = false;
            this._workspace.AutoResetScrollPosition = false;
            this._workspace.AutoScroll = true;
            this._workspace.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("_workspace.BackgroundImage")));
            this._workspace.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._workspace.CenterMode = SIA.UI.Components.RasterViewerCenterMode.None;
            this._workspace.Cursor = System.Windows.Forms.Cursors.Default;
            this._workspace.Dock = System.Windows.Forms.DockStyle.Fill;
            this._workspace.DoubleBuffer = true;
            this._workspace.FrameColor = System.Drawing.Color.Black;
            this._workspace.FrameIsPartOfView = true;
            this._workspace.FrameShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._workspace.FrameShadowIsPartOfView = true;
            this._workspace.Image = null;
            this._workspace.Location = new System.Drawing.Point(0, 28);
            this._workspace.MaxPixelScaleFactor = 0.1F;
            this._workspace.MetrologySystem = null;
            this._workspace.MinPixelScaleFactor = 0.025F;
            this._workspace.Name = "_workspace";
            this._workspace.RotateAngle = 0F;
            this._workspace.ScaleFactor = 1F;
            this._workspace.SelectedVertex = null;
            this._workspace.Size = new System.Drawing.Size(704, 349);
            this._workspace.SizeMode = SIA.UI.Components.RasterViewerSizeMode.Normal;
            this._workspace.TabIndex = 3;
            this._workspace.TabStop = false;
            // 
            // mainToolBar
            // 
            this.mainToolBar.AllowDrop = true;
            this.mainToolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.mainToolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.tbNew,
            this.tbOpen,
            this.tbSave,
            this.tbSeparator1,
            this.tbUndo,
            this.tbRedo,
            this.tbSeparator3,
            this.tbCopy,
            this.tbCut,
            this.tbPaste,
            this.tbSeparator2,
            this.tbPointer,
            this.tbPan,
            this.tbZoomIn,
            this.tbZoomOut,
            this.tbFitOnScreen,
            this.tbActualSize,
            this.tbSeparator4,
            this.tbRectangle,
            this.tbEllipse,
            this.tbPolygon,
            this.tbOnionRing});
            this.mainToolBar.ButtonSize = new System.Drawing.Size(16, 18);
            this.mainToolBar.DropDownArrows = true;
            this.mainToolBar.ImageList = this.mainImageList;
            this.mainToolBar.Location = new System.Drawing.Point(0, 0);
            this.mainToolBar.Name = "mainToolBar";
            this.mainToolBar.ShowToolTips = true;
            this.mainToolBar.Size = new System.Drawing.Size(704, 28);
            this.mainToolBar.TabIndex = 2;
            // 
            // tbNew
            // 
            this.tbNew.ImageIndex = 0;
            this.tbNew.Name = "tbNew";
            this.tbNew.ToolTipText = "New";
            // 
            // tbOpen
            // 
            this.tbOpen.ImageIndex = 1;
            this.tbOpen.Name = "tbOpen";
            this.tbOpen.ToolTipText = "Open";
            // 
            // tbSave
            // 
            this.tbSave.ImageIndex = 2;
            this.tbSave.Name = "tbSave";
            this.tbSave.ToolTipText = "Save";
            // 
            // tbSeparator1
            // 
            this.tbSeparator1.Name = "tbSeparator1";
            this.tbSeparator1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // tbUndo
            // 
            this.tbUndo.ImageIndex = 13;
            this.tbUndo.Name = "tbUndo";
            this.tbUndo.ToolTipText = "Undo";
            // 
            // tbRedo
            // 
            this.tbRedo.ImageIndex = 14;
            this.tbRedo.Name = "tbRedo";
            this.tbRedo.ToolTipText = "Redo";
            // 
            // tbSeparator3
            // 
            this.tbSeparator3.Name = "tbSeparator3";
            this.tbSeparator3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // tbCopy
            // 
            this.tbCopy.ImageIndex = 16;
            this.tbCopy.Name = "tbCopy";
            this.tbCopy.ToolTipText = "Copy selection to clipboard";
            // 
            // tbCut
            // 
            this.tbCut.ImageIndex = 15;
            this.tbCut.Name = "tbCut";
            this.tbCut.ToolTipText = "Cut select to clipboard";
            // 
            // tbPaste
            // 
            this.tbPaste.ImageIndex = 17;
            this.tbPaste.Name = "tbPaste";
            this.tbPaste.ToolTipText = "Paste from clipboard";
            // 
            // tbSeparator2
            // 
            this.tbSeparator2.Name = "tbSeparator2";
            this.tbSeparator2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // tbPointer
            // 
            this.tbPointer.ImageIndex = 3;
            this.tbPointer.Name = "tbPointer";
            this.tbPointer.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.tbPointer.ToolTipText = "Select";
            // 
            // tbPan
            // 
            this.tbPan.ImageIndex = 10;
            this.tbPan.Name = "tbPan";
            this.tbPan.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.tbPan.ToolTipText = "Pan";
            // 
            // tbZoomIn
            // 
            this.tbZoomIn.ImageIndex = 8;
            this.tbZoomIn.Name = "tbZoomIn";
            this.tbZoomIn.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.tbZoomIn.ToolTipText = "Zoom In";
            // 
            // tbZoomOut
            // 
            this.tbZoomOut.ImageIndex = 9;
            this.tbZoomOut.Name = "tbZoomOut";
            this.tbZoomOut.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.tbZoomOut.ToolTipText = "Zoom Out";
            // 
            // tbFitOnScreen
            // 
            this.tbFitOnScreen.ImageIndex = 11;
            this.tbFitOnScreen.Name = "tbFitOnScreen";
            this.tbFitOnScreen.ToolTipText = "Fit On Screen";
            // 
            // tbActualSize
            // 
            this.tbActualSize.ImageIndex = 12;
            this.tbActualSize.Name = "tbActualSize";
            this.tbActualSize.ToolTipText = "Actual Size";
            // 
            // tbSeparator4
            // 
            this.tbSeparator4.Name = "tbSeparator4";
            this.tbSeparator4.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // tbRectangle
            // 
            this.tbRectangle.ImageIndex = 4;
            this.tbRectangle.Name = "tbRectangle";
            this.tbRectangle.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.tbRectangle.ToolTipText = "Rectangle";
            this.tbRectangle.Visible = false;
            // 
            // tbEllipse
            // 
            this.tbEllipse.ImageIndex = 5;
            this.tbEllipse.Name = "tbEllipse";
            this.tbEllipse.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.tbEllipse.ToolTipText = "Ellipse";
            this.tbEllipse.Visible = false;
            // 
            // tbPolygon
            // 
            this.tbPolygon.ImageIndex = 6;
            this.tbPolygon.Name = "tbPolygon";
            this.tbPolygon.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.tbPolygon.ToolTipText = "Polygon";
            // 
            // tbOnionRing
            // 
            this.tbOnionRing.ImageIndex = 7;
            this.tbOnionRing.Name = "tbOnionRing";
            this.tbOnionRing.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.tbOnionRing.ToolTipText = "Onion Ring";
            this.tbOnionRing.Visible = false;
            // 
            // panelColorPicker
            // 
            this.panelColorPicker.Controls.Add(this.lbCurrentCoord);
            this.panelColorPicker.Controls.Add(this.nudAlpha);
            this.panelColorPicker.Controls.Add(this.label1);
            this.panelColorPicker.Controls.Add(this.colorPicker);
            this.panelColorPicker.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelColorPicker.Location = new System.Drawing.Point(0, 377);
            this.panelColorPicker.Name = "panelColorPicker";
            this.panelColorPicker.Size = new System.Drawing.Size(704, 40);
            this.panelColorPicker.TabIndex = 1;
            // 
            // lbCurrentCoord
            // 
            this.lbCurrentCoord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbCurrentCoord.Location = new System.Drawing.Point(541, 4);
            this.lbCurrentCoord.Name = "lbCurrentCoord";
            this.lbCurrentCoord.Size = new System.Drawing.Size(151, 20);
            this.lbCurrentCoord.TabIndex = 5;
            this.lbCurrentCoord.Text = "(x, y)";
            this.lbCurrentCoord.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudAlpha
            // 
            this.nudAlpha.Location = new System.Drawing.Point(232, 4);
            this.nudAlpha.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudAlpha.Name = "nudAlpha";
            this.nudAlpha.Size = new System.Drawing.Size(48, 20);
            this.nudAlpha.TabIndex = 4;
            this.nudAlpha.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(190, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Alpha:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // colorPicker
            // 
            this.colorPicker.BrushColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.colorPicker.Location = new System.Drawing.Point(1, 1);
            this.colorPicker.Name = "colorPicker";
            this.colorPicker.PenColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.colorPicker.Size = new System.Drawing.Size(182, 38);
            this.colorPicker.TabIndex = 0;
            // 
            // MaskEditor
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(704, 417);
            this.Controls.Add(this.docContainer);
            this.Controls.Add(this.panelColorPicker);
            this.Menu = this.mainMenu;
            this.Name = "MaskEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Mask Editor";
            this.docContainer.ResumeLayout(false);
            this.docContainer.PerformLayout();
            this.panelColorPicker.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudAlpha)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		#region public properties

		public string FileName
		{
			get { return fileName; }	
			set { fileName = value; }
		}

		public bool AppliedMask
		{
			get { return bAppliedMask; }
			set { bAppliedMask = value; }
		}			
		
		public Matrix Transform
		{
			get {return _workspace.Transform;}
		}

		public GraphicsList GraphicsList
		{
			get {return _workspace.GraphicsList;}
            set { _workspace.GraphicsList = value; }
		}

        public GraphicsList InitialRegions = null;

		#endregion

		#region override routines

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			// initialize windows form properties
			this.Icon = ResourceHelper.ApplicationIcon;
			this.FormBorderStyle = FormBorderStyle.Sizable;

#if SIA_PRODUCT
            Initialize_RegionEditor();
#endif

			// initialize UI Objects
			InitializeUIObjects();
			// initialize Helper Objects
			InitializeHelperObjects();
		}

#if SIA_PRODUCT
        private void Initialize_RegionEditor()
        {
            this.Text = "Region Editor";

            this.mnuFileNew.Text = "&New Region";

            //_docManager.ForceNewDocument();
        }
#endif

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing (e);

            if (!this._docManager.CloseDocument())
                e.Cancel = true;
            else if (_docManager.ApplyOnClose == DialogResult.Yes)
                ApplyCommonImage();
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged (e);
			// refresh draw area
			this._workspace.Invalidate();
		}

		#endregion

		#region public routines
		private void ApplyCommonImage()
		{
            if (this.ApplyMask != null)
            {
                this.ApplyMask(this, new MaskEditorApplyMaskEventArgs(this.GraphicsList));
                _docManager.Dirty = false;
            }
		}

		public static bool IsMaskFile(string FilePath)
		{
			return (Path.GetExtension(FilePath).ToUpper() == ".MSK");
		}

		public static bool ApplyMaskToImage(string FilePath, SIA.SystemLayer.CommonImage commonImage)
		{
			if (commonImage == null)
				throw new ArgumentNullException("Invalid Common Image Parameter");
			
			if (!IsMaskFile(FilePath) || File.Exists(FilePath) == false) 
				throw new ArgumentException("Invalid file path");
			
			bool result = false;
			
			// Read the data
			try 
			{
				using( Stream stream = new FileStream(
						   FilePath, FileMode.Open, FileAccess.Read) ) 
				{
					// Deserialize object from text format					
					IFormatter formatter = new SoapFormatter(); 
					SerializationEventArgs args = new SerializationEventArgs(
						formatter, stream, FilePath);

					GraphicsList loadObjects = (GraphicsList)formatter.Deserialize(stream);
					
					Bitmap bmpResult = new Bitmap(commonImage.Width, commonImage.Height, PixelFormat.Format32bppArgb);
					Graphics graph = Graphics.FromImage(bmpResult);
					graph.Clear(Color.FromArgb(0xFF, Color.Black));
					
					int iCount = loadObjects.Count;
					for (int i=0; i<iCount; i++)
					{
						DrawObject obj = (DrawObject)loadObjects[i];
						if (obj != null) obj.Draw(graph);
					}
			
					graph.Dispose();

					commonImage.MaskImage = bmpResult;
					result = true;
				}
			}
			catch(UnauthorizedAccessException ex)
			{
				throw new UnauthorizedAccessException(ex.Message);
			}

			catch(Exception exp)
			{
				throw new System.Exception(exp.Message);
			}
			return result;
		}

        public void UpdateCurrentCoordFromWorkspace(float x, float y)
        {
            lbCurrentCoord.Text = "(" + x.ToString() + ", " + y.ToString() + ")";
        }
		
		#endregion

		#region internal helpers

		private void InitializeUIObjects()
		{
			// initialize menu items
			foreach (MenuItem item in this.mainMenu.MenuItems)
				InitializeMenuItem(item);

			foreach (MenuItem item in this.contextMenu.MenuItems)
				InitializeMenuItem(item);

			this.contextMenu.Popup +=new EventHandler(MenuItem_PopUp);

			// initialize tool bars
			this.mainToolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.MainToolBar_Click);
			
			// initialize settings
			this.colorPicker.PenColor = DrawObject.LastUsedColor;
			this.colorPicker.BrushColor = DrawObject.LastUsedBrushColor;
			this.nudAlpha.Value = (Decimal)DrawObject.LastUsedAlpha;

			this.colorPicker.PenColorChanged += new EventHandler(ColorPicker_PenColorChanged);
			this.colorPicker.BrushColorChanged += new EventHandler(ColorPicker_BrushColorChanged);
			this.nudAlpha.ValueChanged += new EventHandler(Alpha_ValueChanged);
		}

		private void InitializeMenuItem(MenuItem menuItem)
		{
			if (menuItem == null) return;
			menuItem.Click  += new System.EventHandler(MenuItem_Click);
			menuItem.Select += new System.EventHandler(MenuItem_Select);
			if (menuItem.IsParent == true)
			{
				menuItem.Popup += new System.EventHandler(MenuItem_PopUp);
				foreach(MenuItem subItem in menuItem.MenuItems)
					InitializeMenuItem(subItem);
			}
		}

		public void InitializeHelperObjects()
		{
			// DocManager
			DocManagerData data = new DocManagerData();
			data.Owner = this;
			data.UpdateTitle = true;
#if SIA_PRODUCT
            data.FileDialogFilter = "Region files (*.rgn)|*.rgn|All Files (*.*)|*.*";
#else
			data.FileDialogFilter = "Mask files (*.msk)|*.msk|All Files (*.*)|*.*";
#endif
			data.NewDocName = "Untitled";
			data.RegistryPath = "";

			_docManager = new DocManager(data);
            _docManager.StillDirtyAfterSave = true;
            _docManager.AlternativeSavePromp = "The regions have changed.\nDo you want to apply these changes to the reference file?";
			_docManager.RegisterFileType("rgn", "rgnFile", "SiGlaz Region File");

            _docManager.NewDocument();
			
			// initialize context menu
			_workspace.MaskEditor = this;
			_workspace.ContextMenu = this.contextMenu;
			_workspace.Cursors = SIA.UI.MaskEditor.DrawTools.Resources.ApplicationCursors.Cursors;
            if (InitialRegions != null)
                GraphicsList = InitialRegions;
			_workspace.Initialize(this, this._docManager);
			//_workspace.UserActionCommited += new EventHandler(Workspace_UserActionCommited);
			_workspace.RefreshUIObjects += new EventHandler(Workspace_RefreshUIObjects);
			
			// set rectangle mode
			_workspace.ActiveTool = DrawToolType.Pointer;

			// apply background image
			_workspace.Image = this._image;
			_workspace.RasterImageRender.ViewRange = this._render.ViewRange;
			_workspace.Location = Point.Empty;
			_workspace.Dock = DockStyle.None;
			
			// initialize graphics list from mask object contained in _image object
			_workspace.InitializeGraphicsList();

			// center draw area
			int width = this.docContainer.ClientRectangle.Width;
			int height = this.docContainer.ClientRectangle.Height;
			int left = (width - _workspace.Width) / 2;
			int top  = (height - _workspace.Height) / 2;
			this._workspace.Location = new Point(left, top);			
			_workspace.Dock = DockStyle.Fill;
			
			this.MaximizeBox = true;
			this.MinimizeBox = true;
			this.FormBorderStyle = FormBorderStyle.Sizable;

			_workspace.ZoomFitOnScreen();
			
			// Regis event handler of draw area
			_workspace.ActiveToolChanged += new EventHandler(this.DrawArea_ActiveToolChanged);
			_workspace.InteractiveModeChanged += new EventHandler(DrawArea_InteractiveModeChanged);
			UpdateUIObjects();
		}
		#endregion

		#region event handlers

		private void MenuItem_Select(object sender, System.EventArgs e)
		{
		}

		private void MenuItem_PopUp(object sender, System.EventArgs e)
		{
			UpdateUIObjects();
		}

		private void MenuItem_Click(object sender, System.EventArgs e)
		{
			string text = ((MenuItem)sender).Text;
			string command = text.Replace("&", "");
			
			ExecuteFunction(command);

			UpdateUIObjects();
		}

		private void MainToolBar_Click(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			ToolBarButton tbtn = e.Button as ToolBarButton;
			if (tbtn == tbNew)
				CommandFileNew();
			else if (tbtn == tbOpen)
				CommandFileOpen();
			else if (tbtn == tbSave)
				CommandFileSave();
			else if (tbtn == tbPointer)
				CommandDrawMode(DrawToolType.Pointer);
			else if (tbtn == tbRectangle)
				CommandDrawMode(DrawToolType.Rectangle);
			else if (tbtn == tbEllipse)
				CommandDrawMode(DrawToolType.Ellipse);
//			else if (tbtn == tbLine)
//				CommandDrawMode(DrawToolType.Line);		
			else if (tbtn == tbPolygon)
				CommandDrawMode(DrawToolType.Polygon);		
			else if (tbtn == tbOnionRing)
				CommandDrawMode(DrawToolType.OnionRing);		
			else if (tbtn == tbZoomIn)
				CommandViewZoomIn();
			else if (tbtn == tbZoomOut)
				CommandViewZoomOut();
			else if (tbtn == tbPan)
				CommandViewPan();
			else if (tbtn == tbFitOnScreen)
				CommandViewFitOnScreen();
			else if (tbtn == tbActualSize)
				CommandViewActualSize();
			else if (tbtn == tbUndo)
				CommandEditUndo();
			else if (tbtn == tbRedo)
				CommandEditRedo();
			else if (tbtn == tbCopy)
				CommandEditCopy();
			else if (tbtn == tbCut)
				CommandEditCut();
			else if (tbtn == tbPaste)
				CommandEditPaste();

			UpdateUIObjects();
		}

		private void DrawArea_ActiveToolChanged(object sender, System.EventArgs e)
		{
            UpdateUIObjects();
		}

		private void DrawArea_InteractiveModeChanged(object sender, EventArgs e)
		{
			UpdateUIObjects();
		}

		private void UpdateUIObjects()
		{
			UpdateMenuItems();
			UpdateToolbars();
		}

		private MenuItem SearchMenuItems(MenuItem item, bool SearchChilds, string text)
		{
			MenuItem mnuResult = null;
			if (item.Text == text) 
				mnuResult = item;
			else if (SearchChilds)
			{
				foreach(MenuItem mnuItem in item.MenuItems)
					if ((mnuResult = SearchMenuItems(mnuItem, SearchChilds, text)) != null)
						break;
			}
			return mnuResult;
		}

		private void UpdateMenuItems()
		{	
			// update main menu items
			mnuFileApply.Enabled = (this._workspace.GraphicsList.Count > 0);
			
			mnuEditSelectAll.Enabled = (this._workspace.GraphicsList.Count > 0);
			mnuEditDelete.Enabled = (this._workspace.GraphicsList.SelectionCount > 0);
			mnuEditBringToFront.Enabled = (this._workspace.GraphicsList.SelectionCount > 0);
			mnuEditSendToBack.Enabled = (this._workspace.GraphicsList.SelectionCount > 0);

			mnuDrawSelect.Checked    = (this._workspace.ActiveTool == DrawToolType.Pointer);
			mnuDrawRectangle.Checked = (this._workspace.ActiveTool == DrawToolType.Rectangle);
			mnuDrawEllipse.Checked   = (this._workspace.ActiveTool == DrawToolType.Ellipse);
			mnuDrawPolygon.Checked   = (this._workspace.ActiveTool == DrawToolType.Polygon);
			mnuDrawOnionRing.Checked = (this._workspace.ActiveTool == DrawToolType.OnionRing);

			mnuEditRedo.Enabled = this._workspace.CanRedo;
			mnuEditUndo.Enabled = this._workspace.CanUndo;
			mnuEditCut.Enabled = this._workspace.CanCutToClipboard;
			mnuEditCopy.Enabled = this._workspace.CanCopyToClipboard;
			mnuEditPaste.Enabled = this._workspace.CanPasteFromClipboard;

            mnuEditDescription.Enabled = (this._workspace.GraphicsList.SelectionCount == 1);

			// update context menu items
			ctxSelectAll.Enabled = (this._workspace.GraphicsList.Count > 0);
			ctxDelete.Enabled = (this._workspace.GraphicsList.SelectionCount > 0);
			ctxBringToFront.Enabled = (this._workspace.GraphicsList.SelectionCount > 0);
			ctxSendToBack.Enabled = (this._workspace.GraphicsList.SelectionCount > 0);

			ctxCopy.Enabled = this._workspace.CanCopyToClipboard;
			ctxCut.Enabled = this._workspace.CanCutToClipboard;
			ctxPaste.Enabled = this._workspace.CanPasteFromClipboard;

            ctxDescription.Enabled = (this._workspace.GraphicsList.SelectionCount == 1);

            if (_workspace.SelectedVertex != null)
            {
                ctxAutoVertex.Visible = true;
                ctxAutoVertex.Checked = _workspace.SelectedVertex.IsAutoVertex;
            }
            else
                ctxAutoVertex.Visible = false;
		}

		private void UpdateToolbars()
		{
			tbPointer.Pushed = _workspace.InteractiveMode == VisualInteractiveMode.DrawObject && (_workspace.ActiveTool == DrawToolType.Pointer);
			tbRectangle.Pushed = _workspace.InteractiveMode == VisualInteractiveMode.DrawObject && (_workspace.ActiveTool == DrawToolType.Rectangle);
			tbEllipse.Pushed = _workspace.InteractiveMode == VisualInteractiveMode.DrawObject && (_workspace.ActiveTool == DrawToolType.Ellipse);
			tbPolygon.Pushed = _workspace.InteractiveMode == VisualInteractiveMode.DrawObject && (_workspace.ActiveTool == DrawToolType.Polygon);
			tbOnionRing.Pushed = _workspace.InteractiveMode == VisualInteractiveMode.DrawObject && (_workspace.ActiveTool == DrawToolType.OnionRing);

			//tbZoomIn.Enabled = _workspace.IsValidZoomScale(this._workspace.ZoomScale + 0.1f);
			//tbZoomOut.Enabled = _workspace.IsValidZoomScale(this._workspace.ZoomScale - 0.1f);

			tbUndo.Enabled = _workspace.CanUndo;
			tbRedo.Enabled = _workspace.CanRedo;
			tbCut.Enabled = _workspace.CanCutToClipboard;
			tbCopy.Enabled = _workspace.CanCopyToClipboard;
			tbPaste.Enabled = _workspace.CanPasteFromClipboard;

			tbZoomIn.Pushed = _workspace.InteractiveMode == VisualInteractiveMode.ZoomIn;
			tbZoomOut.Pushed = _workspace.InteractiveMode == VisualInteractiveMode.ZoomOut;
			tbPan.Pushed = _workspace.InteractiveMode == VisualInteractiveMode.Pan;
		}

		private void ExecuteFunction(string command)
		{
			switch (command)
			{
#if SIA_PRODUCT
                case "New Region":
#else
				case "New Mask":
#endif
					CommandFileNew();
					break;
				case "Open...":
					CommandFileOpen();
					break;
				case "Save":
					CommandFileSave();
					break;
				case "Save As...":
					CommandFileSaveAs();
					break;
				case "Apply...":
					CommandFileApply();
					break;
				case "Close":
					CommandFileClose();
					break;

				case "Select":
					CommandDrawMode(DrawToolType.Pointer);
					break;
				case "Rectangle":
					CommandDrawMode(DrawToolType.Rectangle);
					break;
				case "Ellipse":
					CommandDrawMode(DrawToolType.Ellipse);
					break;
				case "Line":
					CommandDrawMode(DrawToolType.Line);
					break;
				case "Polygon":
					CommandDrawMode(DrawToolType.Polygon);
					break;
				case "Onion Ring":
					CommandDrawMode(DrawToolType.OnionRing);
					break;

				case "Undo":
					CommandEditUndo();
					break;
				case "Redo":
					CommandEditRedo();
					break;
				case "Select All":
					CommandEditSelectAll();
					break;
				case "Delete":
					CommandEditDelete();
					break;
				case "Bring to Front":
					CommandEditMoveToFront();
					break;
				case "Send to Back":
					CommandEditMoveToBack();
					break;
				case "Cut":
					CommandEditCut();
					break;
				case "Copy":
					CommandEditCopy();
					break;
				case "Paste":
					CommandEditPaste();
					break;
		
				case "Pan":
					CommandViewPan();
					break;
				case "Zoom In":
					CommandViewZoomIn();
					break;
				case "Zoom Out":
					CommandViewZoomOut();
					break;
				case "Fit On Screen":
					CommandViewFitOnScreen();
					break;
				case "Actual Size":
					CommandViewActualSize();
					break;

                case "Description...":
                    CommandEditDescription();
                    break;
                case "Auto-vertex":
                    CommandAutoVertex();
                    break;
				default:
					break;
			}
		}

		private void CommandFileNew()
		{
			this._docManager.NewDocument();
		}

		private void CommandFileOpen()
		{
#if SIA_PRODUCT
            //if (_docManager.CloseDocument() == false)
            //    return;

            _docManager.ForceNewDocument();
#endif
			this._docManager.OpenDocument("");
		}

		private void CommandFileSave()
		{
			this._docManager.SaveDocument(DocToolkit.DocManager.SaveType.Save);

			if ( this.AppliedMask )
				this.AppliedMaskChanged (this, new System.EventArgs());
		}

		private void CommandFileSaveAs()
		{
			this._docManager.SaveDocument(DocToolkit.DocManager.SaveType.SaveAs);
		}

		private void CommandFileApply()
		{
			this.ApplyCommonImage();
		}

		private void CommandFileClose()
		{
			this.Close();
		}

		private void CommandEditUndo()
		{
			this._workspace.Undo();
		}

		private void CommandEditRedo()
		{
			this._workspace.Redo();
		}

		private void CommandEditSelectAll()
		{
			this._workspace.SelectAll();
		}

		private void CommandEditDelete()
		{
			this._workspace.DeleteSelection();
		}

		private void CommandEditMoveToFront()
		{
			this._workspace.BringSelectionToFront();
		}

		private void CommandEditMoveToBack()
		{
			this._workspace.SendSelectionToBack();
		}

        private void CommandEditDescription()
        {
            this._workspace.EditDescription();
        }

        private void CommandAutoVertex()
        {
            this._workspace.SetAutoVertex();
        }

		private void CommandEditCopy()
		{
			this._workspace.CopySelectionToClipboard();
		}

		private void CommandEditCut()
		{
			this._workspace.CutSelectionToClipboard();
		}

		private void CommandEditPaste()
		{
			this._workspace.PasteFromClipboard();
		}

		private void CommandDrawMode(DrawToolType mode)
		{
			this._workspace.ActiveTool = mode;			
		}

		private void CommandViewZoomIn()
		{
			this._workspace.InteractiveMode = VisualInteractiveMode.ZoomIn;
			this._workspace.Invalidate(true);
		}

		private void CommandViewZoomOut()
		{
			this._workspace.InteractiveMode = VisualInteractiveMode.ZoomOut;
			this._workspace.Invalidate(true);
		}

		private void CommandViewPan()
		{
			this._workspace.InteractiveMode = VisualInteractiveMode.Pan;
			this._workspace.Invalidate(true);
		}

		private void CommandViewFitOnScreen()
		{
			this._workspace.ZoomFitOnScreen();
			this._workspace.Invalidate(true);
		}

		private void CommandViewActualSize()
		{
			this._workspace.ScaleFactor = 1.0F;
			this._workspace.Invalidate(true);
		}

		private void ColorPicker_PenColorChanged(object sender, EventArgs e)
		{
			DrawObject.LastUsedColor = colorPicker.PenColor;
		}

		private void ColorPicker_BrushColorChanged(object sender, EventArgs e)
		{
			DrawObject.LastUsedBrushColor = colorPicker.BrushColor;
		}

		private void Alpha_ValueChanged(object sender, System.EventArgs e)
		{
			DrawObject.LastUsedAlpha = (int)nudAlpha.Value;
		}

		private void Workspace_UserActionCommited(object sender, EventArgs e)
		{
			this.UpdateMenuItems();
			this.UpdateToolbars();
		}

		private void Workspace_RefreshUIObjects(object sender, EventArgs e)
		{
            if (e.GetType() == typeof(MouseEventArgsF))
            {
                UpdateCurrentCoordFromWorkspace((e as MouseEventArgsF).Xf, (e as MouseEventArgsF).Yf);
            }
            else
            {
                this.UpdateMenuItems();
                this.UpdateToolbars();
            }
		}

		#endregion
		
		#region Error Handlers
		/// <summary>
		/// Handle exception from docManager_LoadEvent function
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="fileName"></param>
		private void HandleLoadException(Exception ex, string fileName)
		{
			MessageBox.Show(this, 
				"Load File operation failed. File name: " + fileName + "\n" +
				"Reason: " + ex.Message, 
				DocManager.ProductName, 
				MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		/// <summary>
		/// Handle exception from docManager_SaveEvent function
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="fileName"></param>
		private void HandleSaveException(Exception ex, string fileName)
		{
			MessageBox.Show(this, 
				"Save File operation failed. File name: " + fileName + "\n" +
				"Reason: " + ex.Message, 
				DocManager.ProductName, 
				MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		/// <summary>
		/// Handle generic exception 
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="e"></param>
		private void HandleGenericException(Exception ex)
		{
			MessageBox.Show(this,
				"Generic error: " + ex.Message, 
				DocManager.ProductName);			
		}
		#endregion

		
	}
}
