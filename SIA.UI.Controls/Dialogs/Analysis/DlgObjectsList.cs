#define WAFER_LIST__

using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Data;

using SIA.Common.Analysis;
using SIA.Common.KlarfExport;

using SIA.UI.Components;
using SIA.UI.Components.Helpers;
//CONG using SIA.UI.Controls.Components;
using SIA.UI.Controls.Utilities;
using SIA.UI.Controls.Helpers;

using SIA.UI.Controls.Automation;
using SIA.UI.Controls.Automation.Commands;
using SIA.UI.Controls.Commands;

using SIA.Plugins.Common;
using SIA.UI.Controls.Helpers.VisualAnalysis;
using System.Threading;
using SIA.UI.Controls.UserControls;
using SiGlaz.Common.ABSDefinitions;
using SiGlaz.ObjectAnalysis.Common;
using System.Drawing.Imaging;
using SIA.SystemLayer;
using System.Drawing.Drawing2D;
using SiGlaz.Common.Object;
using SiGlaz.UI.CustomControls.ListViewEx;
using SiGlaz.Common.ImageAlignment;
using SIA.UI.Controls.Commands.Analysis;

namespace SIA.UI.Controls.Dialogs
{
	/// <summary>
	/// Summary description for DlgObjectsList.
	/// </summary>
	public class DlgObjectsList : FloatingFormBase
	{
		#region Member Fields

		private ImageWorkspace _workspace = null;
		//private DataTable _dataSource;
		private ArrayList _objectList;
        private List<DetectedObjectView> _objListView = new List<DetectedObjectView>();
		
		//private HistogramExporter _histExporter = null;
		private int _ignoreUpdateUICounter = 0;
        private int _ignoreSelectionChanged = 0;

        
		#endregion

        #region Properties

        public ObjectAnalyzer ObjectAnalyzer
        {
            get
            {
                if (this._workspace == null)
                    return null;
                return this._workspace.GetAnalyzer("ObjectAnalyzer") as ObjectAnalyzer;
            }
        }

        #endregion

        #region Windows Form member attributes

        private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem mnuFile;
		private System.Windows.Forms.MenuItem mnuFileSaveData;
		private System.Windows.Forms.MenuItem mnuFileClose;
		private System.Windows.Forms.MenuItem mnuEdit;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem mnuEditCopy;
		private System.Windows.Forms.MenuItem mnuEditCut;
		private System.Windows.Forms.MenuItem mnuEditDeleteSelected;
		private System.Windows.Forms.MenuItem mnuEditSelectAll;
		private System.Windows.Forms.MenuItem mnuTools;
		private System.Windows.Forms.MenuItem mnuToolsFilterObjects;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem mnuView;
		private System.Windows.Forms.MenuItem mnuViewAllObjects;
        private System.Windows.Forms.MenuItem mnuViewHighlightSelection;
		protected System.Windows.Forms.Label txtNumObjects;
		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuItem mnuFileSaveChart;
        private TreeView treeViewObjectProperties;
        private Panel panelStatus;
        private MenuItem mnShowHidePreview;
        private ImageList imageListObj;
        private Panel panelPrevew;
        private Panel panelThumbnail;
        DetectedObjectListViewer objectListView = null;
        private IContainer components;

		#endregion		

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgObjectsList));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.mnuFile = new System.Windows.Forms.MenuItem();
            this.mnuFileSaveData = new System.Windows.Forms.MenuItem();
            this.mnuFileSaveChart = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.mnuFileClose = new System.Windows.Forms.MenuItem();
            this.mnuEdit = new System.Windows.Forms.MenuItem();
            this.mnuEditCut = new System.Windows.Forms.MenuItem();
            this.mnuEditCopy = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.mnuEditSelectAll = new System.Windows.Forms.MenuItem();
            this.mnuEditDeleteSelected = new System.Windows.Forms.MenuItem();
            this.mnuView = new System.Windows.Forms.MenuItem();
            this.mnuViewAllObjects = new System.Windows.Forms.MenuItem();
            this.mnuViewHighlightSelection = new System.Windows.Forms.MenuItem();
            this.mnShowHidePreview = new System.Windows.Forms.MenuItem();
            this.mnuTools = new System.Windows.Forms.MenuItem();
            this.mnuToolsFilterObjects = new System.Windows.Forms.MenuItem();
            this.panelPrevew = new System.Windows.Forms.Panel();
            this.treeViewObjectProperties = new System.Windows.Forms.TreeView();
            this.panelThumbnail = new System.Windows.Forms.Panel();
            this.panelStatus = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNumObjects = new System.Windows.Forms.Label();
            this.objectListView = new SIA.UI.Controls.Dialogs.DetectedObjectListViewer();
            this.imageListObj = new System.Windows.Forms.ImageList(this.components);
            this.panelPrevew.SuspendLayout();
            this.panelStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFile,
            this.mnuEdit,
            this.mnuView,
            this.mnuTools});
            // 
            // mnuFile
            // 
            this.mnuFile.Index = 0;
            this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFileSaveData,
            this.mnuFileSaveChart,
            this.menuItem2,
            this.mnuFileClose});
            this.mnuFile.Text = "&File";
            this.mnuFile.Popup += new System.EventHandler(this.MenuItem_Popup);
            // 
            // mnuFileSaveData
            // 
            this.mnuFileSaveData.Index = 0;
            this.mnuFileSaveData.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftS;
            this.mnuFileSaveData.Text = "&Save Data as ...";
            this.mnuFileSaveData.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // mnuFileSaveChart
            // 
            this.mnuFileSaveChart.Index = 1;
            this.mnuFileSaveChart.Text = "Save &Chart as...";
            this.mnuFileSaveChart.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 2;
            this.menuItem2.Text = "-";
            // 
            // mnuFileClose
            // 
            this.mnuFileClose.Index = 3;
            this.mnuFileClose.Text = "&Close";
            this.mnuFileClose.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // mnuEdit
            // 
            this.mnuEdit.Index = 1;
            this.mnuEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuEditCut,
            this.mnuEditCopy,
            this.menuItem4,
            this.mnuEditSelectAll,
            this.mnuEditDeleteSelected});
            this.mnuEdit.Text = "&Edit";
            this.mnuEdit.Popup += new System.EventHandler(this.MenuItem_Popup);
            // 
            // mnuEditCut
            // 
            this.mnuEditCut.Index = 0;
            this.mnuEditCut.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
            this.mnuEditCut.Text = "Cu&t";
            this.mnuEditCut.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // mnuEditCopy
            // 
            this.mnuEditCopy.Index = 1;
            this.mnuEditCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.mnuEditCopy.Text = "&Copy";
            this.mnuEditCopy.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "-";
            // 
            // mnuEditSelectAll
            // 
            this.mnuEditSelectAll.Index = 3;
            this.mnuEditSelectAll.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
            this.mnuEditSelectAll.Text = "&Select All";
            this.mnuEditSelectAll.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // mnuEditDeleteSelected
            // 
            this.mnuEditDeleteSelected.Index = 4;
            this.mnuEditDeleteSelected.Text = "&Delete";
            this.mnuEditDeleteSelected.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // mnuView
            // 
            this.mnuView.Index = 2;
            this.mnuView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuViewAllObjects,
            this.mnuViewHighlightSelection,
            this.mnShowHidePreview});
            this.mnuView.Text = "&View";
            this.mnuView.Popup += new System.EventHandler(this.MenuItem_Popup);
            // 
            // mnuViewAllObjects
            // 
            this.mnuViewAllObjects.Index = 0;
            this.mnuViewAllObjects.Text = "&Show All Objects";
            this.mnuViewAllObjects.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // mnuViewHighlightSelection
            // 
            this.mnuViewHighlightSelection.Index = 1;
            this.mnuViewHighlightSelection.Text = "&Highlight Selection";
            this.mnuViewHighlightSelection.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // mnShowHidePreview
            // 
            this.mnShowHidePreview.Checked = true;
            this.mnShowHidePreview.Index = 2;
            this.mnShowHidePreview.Text = "Show/Hide the Object Properties Panel";
            this.mnShowHidePreview.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // mnuTools
            // 
            this.mnuTools.Index = 3;
            this.mnuTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuToolsFilterObjects});
            this.mnuTools.Text = "&Tools";
            this.mnuTools.Popup += new System.EventHandler(this.MenuItem_Popup);
            // 
            // mnuToolsFilterObjects
            // 
            this.mnuToolsFilterObjects.Index = 0;
            this.mnuToolsFilterObjects.Text = "&Filter Object";
            this.mnuToolsFilterObjects.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // panelPrevew
            // 
            this.panelPrevew.Controls.Add(this.treeViewObjectProperties);
            this.panelPrevew.Controls.Add(this.panelThumbnail);
            this.panelPrevew.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelPrevew.Location = new System.Drawing.Point(449, 0);
            this.panelPrevew.Name = "panelPrevew";
            this.panelPrevew.Size = new System.Drawing.Size(211, 478);
            this.panelPrevew.TabIndex = 8;
            // 
            // treeViewObjectProperties
            // 
            this.treeViewObjectProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewObjectProperties.Location = new System.Drawing.Point(0, 100);
            this.treeViewObjectProperties.Name = "treeViewObjectProperties";
            this.treeViewObjectProperties.Size = new System.Drawing.Size(211, 378);
            this.treeViewObjectProperties.TabIndex = 6;
            // 
            // panelThumbnail
            // 
            this.panelThumbnail.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelThumbnail.Location = new System.Drawing.Point(0, 0);
            this.panelThumbnail.Name = "panelThumbnail";
            this.panelThumbnail.Size = new System.Drawing.Size(211, 100);
            this.panelThumbnail.TabIndex = 0;
            // 
            // panelStatus
            // 
            this.panelStatus.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panelStatus.Controls.Add(this.label1);
            this.panelStatus.Controls.Add(this.txtNumObjects);
            this.panelStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelStatus.Location = new System.Drawing.Point(0, 478);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(660, 26);
            this.panelStatus.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Total Objects :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtNumObjects
            // 
            this.txtNumObjects.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtNumObjects.Location = new System.Drawing.Point(87, 5);
            this.txtNumObjects.Name = "txtNumObjects";
            this.txtNumObjects.Size = new System.Drawing.Size(87, 16);
            this.txtNumObjects.TabIndex = 4;
            this.txtNumObjects.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // objectListView
            // 
            this.objectListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectListView.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.objectListView.Location = new System.Drawing.Point(0, 0);
            this.objectListView.Name = "objectListView";
            this.objectListView.Size = new System.Drawing.Size(449, 478);
            this.objectListView.TabIndex = 0;
            // 
            // imageListObj
            // 
            this.imageListObj.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListObj.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListObj.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // DlgObjectsList
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(660, 504);
            this.Controls.Add(this.objectListView);
            this.Controls.Add(this.panelPrevew);
            this.Controls.Add(this.panelStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.MinimumSize = new System.Drawing.Size(550, 400);
            this.Name = "DlgObjectsList";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Objects List";
            this.panelPrevew.ResumeLayout(false);
            this.panelStatus.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region constructor and destructor
		
		internal DlgObjectsList()
		{
			InitializeComponent();
			_objectList = new ArrayList();
		}

		public DlgObjectsList(ImageWorkspace workSpace, ArrayList objects) 
		{
			if (workSpace == null)
				throw new ArgumentNullException("workspace");
			if (objects == null)
#if WAFER_LIST
                throw new ArgumentNullException("wafers");
#else
				throw new ArgumentNullException("objects");
#endif
			
			// initialize components
			this.InitializeComponent();

            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

			// initialize class
			this.InitClass(workSpace, objects);
		}

		private void InitClass(ImageWorkspace workspace, ArrayList objects)
		{
			// initialize top level window
			this.TopLevel = true;
			
			
			// initialize workspace
			if (workspace != null)
			{
				this.Owner = workspace.FindForm();
				this._workspace = workspace;
				this._workspace.DetectedObjectsChanged += new EventHandler(Workspace_DetectedObjectsChanged);
			}

			// initialize object list
			if (objects == null)
				objects = new ArrayList();
			this._objectList = objects;

            objectListView.Viewer.SelectionChanged += new EventHandler(Viewer_SelectionChanged);
            objectListView.Viewer.DoubleClick += new EventHandler(Viewer_DoubleClick);

			InitializeObjectPropertiesPreview();
		}                

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if (disposing )
			{
				if (components != null)
					components.Dispose();
				
                //if (_dataSource != null)
                //{
                //    if (_dataSource.Rows != null)
                //        _dataSource.Rows.Clear();

                //    if (_dataSource != null)
                //    {
                //        _dataSource.Clear();					
                //        _dataSource.Dispose();
                //        _dataSource = null;
                //    }
                //}

				if (_objectList != null)
					_objectList = null;

				// unregisters for event DetectedObjectChanged
				if (this._workspace != null)
					_workspace.DetectedObjectsChanged -= new EventHandler(Workspace_DetectedObjectsChanged);
				this._workspace = null;						
			}

			base.Dispose( disposing );
		}
		
		#endregion

		#region Event handlers

		private void MenuItem_Popup(object sender, System.EventArgs e)
		{
			this.UpdateMainMenu();
		}

		private void MenuItem_Click(object sender, System.EventArgs e)
		{
            if (sender == mnuFileSaveData)
                DoCommandFileSaveData();
            else if (sender == mnuFileSaveChart)
                DoCommandFileSaveChart();
            else if (sender == mnuFileClose)
                DoCommandFileClose();
            //else if (sender == mnuFileExportKlarf)
            //	DoCommandFileExportKlarf();
            else if (sender == mnuEditSelectAll)
                DoCommandEditSelectAll();
            else if (sender == mnuEditDeleteSelected)
                DoCommandEditDelete();
            else if (sender == mnuEditCut)
                DoCommandEditCut();
            else if (sender == mnuEditCopy)
                DoCommandEditCopy();
            else if (sender == mnuViewAllObjects)
                DoCommandViewShowAllObjects();
            else if (sender == mnuViewHighlightSelection)
                DoCommandViewHighlightSelection();
            else if (sender == mnuToolsFilterObjects)
                DoCommandToolsFilterObjects();
            else if (sender == mnShowHidePreview)
                DoShowHidePrivewPanel();
		}

		private void Workspace_DetectedObjectsChanged(object sender, EventArgs e)
		{
			this._objectList = this._workspace.DetectedObjects;
			
			// initialize detect objects
			this.DataBind();

			// refresh view
			Invalidate();
		}
									
		private void btnLoadSettings_Click(object sender, System.EventArgs e)
		{
			try
			{
                //using (OpenFileDialog dlg = CommonDialogs.OpenXmlFileDialog("Load settings from file..."))
                //{
                //    if (dlg.ShowDialog() == DialogResult.OK)
                //    {
                //        // deserialize settings
                //        object obj = RasterCommandSettingsSerializer.Deserialize(dlg.FileName, typeof(ExportObjectHistogramCommandSettings));					
                //        // update command settings
                //        ExportObjectHistogramCommandSettings settings = obj as ExportObjectHistogramCommandSettings;
                //        this.UpdateData(false, settings);
                //    }
                //}
			}
			catch (System.Exception exp)
			{
				Trace.WriteLine(exp);
				MessageBoxEx.Error("Failed to load settings: " + exp.Message);
			}
		}

		private void btnSaveSettings_Click(object sender, System.EventArgs e)
		{
			try
			{
                //using (SaveFileDialog dlg = CommonDialogs.SaveXmlFileDialog("Save settings as..."))
                //{
                //    dlg.FileName = "Untitled";

                //    if (dlg.ShowDialog() == DialogResult.OK)
                //    {
                //        if (File.Exists(dlg.FileName.ToString()))
                //        {
                //            System.IO.FileAttributes fileAttribs =System.IO.File.GetAttributes(dlg.FileName.ToString()); 
                //            if((fileAttribs & System.IO.FileAttributes.ReadOnly) == System.IO.FileAttributes.ReadOnly)
                //            {					
                //                MessageBoxEx.Error("The file is read only and can not be overridden.");
                //                return ;
                //            } 
                //        }
						
                //        // update command settings
                //        ExportObjectHistogramCommandSettings settings = new ExportObjectHistogramCommandSettings();
                //        this.UpdateData(true, settings);

                //        // serialize settings
                //        RasterCommandSettingsSerializer.Serialize(dlg.FileName, settings);					
                //    }
                //}
			}
			catch (System.Exception exp)
			{
				Trace.WriteLine(exp);
				MessageBoxEx.Error("Failed to save settings: " + exp.Message);
			}
		}
		
		#endregion

		#region public methods

		public void ShowAllObjects()
		{
			this.DoCommandViewShowAllObjects();
		}

		public void HighlightSelection()
		{
			this.DoCommandViewHighlightSelection();
		}

		#endregion

		#region override routines
		
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			// initialize Data Grid Text Box Column Format			
			this.DataBind();

            // disable histogram hree
            mnuFileSaveChart.Visible = false;
		}
		
#if WAFER_LIST
        private void Initialize_WaferList_Configurations()
        {
            this.Text = "Wafer List";
            
            // menu configurations
            //this.mnuFileSaveChart.Visible = false;
            this.mnuViewAllObjects.Text = "&Show All Wafers";
            this.mnuTools.Visible = false;
            this.tabControl1.TabPages[0].Text = "Wafer List";
            this.label1.Text = "Total Wafers:";
            this.label4.Text = "Number of statistical wafers:";
        }
#endif

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing (e);

			// cancel the closing operation just hide this window out
			e.Cancel = true;
			this.Visible = false;
		}


		#endregion	

		#region overridable routines

		protected virtual void DataBind()
		{
			try
			{
                ImageAnalyzer imageAnalyzer =
                    _workspace.GetAnalyzer("ImageAnalyzer") as ImageAnalyzer;
                MetrologySystem ms = imageAnalyzer.MetrologySystem;
				if (_objectList != null)
				{
                    _objListView.Clear();
                    int n = _objectList.Count;
                    _objListView.Capacity = n;
                    for (int i = 0; i < n; i++)
                    {
                        DetectedObjectView ov = 
                            new DetectedObjectView(i, _objectList[i] as DetectedObject, ms);
                        _objListView.Add(ov);
                    }

					if (objectListView != null)
                        objectListView.UpdateObjectList(_objListView);

                    txtNumObjects.Text = 
                        string.Format(
                        "{0} object{1}", 
                        _objectList.Count, 
                        (_objectList.Count <= 1 ? "" : "s"));
                }
				else
				{					
				}
			}
			catch (System.Exception e)
			{
				Trace.WriteLine(e);
				MessageBoxEx.Error("Failed to initialize the window: " + e.Message);
			}			
		}
		#region command handlers

		protected virtual void DoCommandFileSaveData()
		{
			if (_objectList == null)
			{
                MessageBoxEx.Error("Objects were not detected or not found on the image. Please try again!");
            }
			else if (_objectList.Count <= 0)
            {
                MessageBoxEx.Error("Objects were not found on the image. Please try again!");
            }
			else
            {

                using (SaveFileDialog dlg = 
                    Utilities.CommonDialogs.SaveCsvFileDialog("Export detected objects"))
                {
					dlg.Filter = "Command Separated Values (*.csv)|*.csv";
					dlg.FilterIndex = 0;

					if ( dlg.ShowDialog() == DialogResult.OK)
					{
                        try
                        {
                            this.ExportCVS(dlg.FileName);

                            MessageBoxEx.Info("Objects were exported successfully!");
                        }
                        catch (Exception exp)
                        {
                            Trace.WriteLine(exp);

                            MessageBoxEx.Error("Failed to export objects");
                        }
                        finally
                        {

                        }
					}
				}
			}
		}

		protected virtual void DoCommandFileSaveChart()
		{
//            if (_objectList == null)
//            {
//#if WAFER_LIST
//                MessageBoxEx.Error("Wafers were not detected or not found on the image. Please try again!");
//#else
//                MessageBoxEx.Error("Objects were not detected or not found on the image. Please try again!");
//#endif
//                return;
//            }
//            else if (_objectList.Count <= 0)
//            {
//#if WAFER_LIST
//                MessageBoxEx.Error("Wafers were not found on the image. Please try again!");
//#else
//                MessageBoxEx.Error("Objects were not found on the image. Please try again!");
//#endif
//                return;
//            }
//#if WAFER_LIST
//            using (SaveFileDialog dlg = Utilities.CommonDialogs.SaveImageFileDialog("Export detected wafers", CommonDialogs.ImageFileFilter.CommonImage))
//#else
//            using (SaveFileDialog dlg = Utilities.CommonDialogs.SaveImageFileDialog("Export detected objects", CommonDialogs.ImageFileFilter.CommonImage ))
//#endif
//            {
//                if ( dlg.ShowDialog()==DialogResult.OK)
//                {	
//                    try
//                    {
//                        string fileName = dlg.FileName;
//                        this.histogramViewer.ExportChartImage(fileName);

//                        MessageBoxEx.Info("Histogram chart was saved successfully!");
//                    }
//                    catch (Exception exp)
//                    {
//                        Trace.WriteLine(exp);

//                        MessageBoxEx.Error("Failed to save histogram chart");
//                    }
//                    finally
//                    {

//                    }
//                }
//            }
		}

		protected virtual void DoCommandFileClose()
		{
			this.Visible = false;
		}

		protected virtual void DoCommandEditDelete()
		{
			try
			{
                if (this._workspace.SelectedObjects != null && this._workspace.SelectedObjects.Count > 0)
				{
					ArrayList selectedObjects = this._workspace.SelectedObjects;
					foreach (DetectedObject obj in selectedObjects)
						_objectList.Remove(obj);
					
					this._workspace.SelectedObjects.Clear();

                    this.DataBind();

					this._workspace.Invalidate(true);
				}
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{
			}
		}
		
		protected virtual void DoCommandEditCut()
		{
			try
			{
                ArrayList selectedObjects = this._workspace.SelectedObjects;
                if (selectedObjects != null && selectedObjects.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < selectedObjects.Count; i++)
                    {
                        DetectedObject obj = (DetectedObject)selectedObjects[i];

                        sb.Append(obj.RectBound.Left.ToString() + ", " +
                            obj.RectBound.Top.ToString() + ", " +
                            obj.RectBound.Right.ToString() + ", " +
                            obj.RectBound.Bottom.ToString() + ", " +
                            obj.Area.ToString() + ", " +
                            obj.Perimeter.ToString() + ", " +
                            obj.NumPixels.ToString() + ", " +
                            obj.TotalIntensity.ToString());

                        _objectList.Remove(obj);

                        if (i != selectedObjects.Count - 1)
                            sb.Append(Environment.NewLine);
                    }

                    if (sb.Length > 0)
                        Clipboard.SetDataObject(sb.ToString(), true);

                    this._workspace.SelectedObjects.Clear();

                    this.DataBind();

                    this._workspace.Invalidate(true);
                }
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{

			}
		}

		protected virtual void DoCommandEditCopy()
		{
			try
			{
				ArrayList selectedObjects = this._workspace.SelectedObjects;
				if(selectedObjects != null && selectedObjects.Count > 0)
				{
					StringBuilder sb = new StringBuilder();
					for(int i=0; i<selectedObjects.Count; i++)
					{
						DetectedObject obj = (DetectedObject)selectedObjects[i];
					
						sb.Append(obj.RectBound.Left.ToString() + ", " +
							obj.RectBound.Top.ToString() + ", " +
                            obj.RectBound.Right.ToString() + ", " +
                            obj.RectBound.Bottom.ToString() + ", " +
							obj.Area.ToString() + ", " +
							obj.Perimeter.ToString() + ", " +
							obj.NumPixels.ToString() + ", " +
							obj.TotalIntensity.ToString());


						if(i != selectedObjects.Count-1)
							sb.Append(Environment.NewLine);							
					}
					
					if(sb.Length > 0)
						Clipboard.SetDataObject(sb.ToString(), true);
				}
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{

			}
		}

		protected virtual void DoCommandEditPaste()
		{
		}

		protected virtual void DoCommandViewShowAllObjects()
		{
            //this._workspace.DoCommandToggleShowAllObjects();
            IAppWorkspace appWorkspace = this._workspace.AppWorkspace;
			appWorkspace.DispatchCommand("CmdToggleDetectedObjects");
		}

		protected virtual void DoCommandViewHighlightSelection()
		{
			//this._workspace.DoCommandToggleHighlightSelection();
            IAppWorkspace appWorkspace = this._workspace.AppWorkspace;
            appWorkspace.DispatchCommand("CmdToggleSelectedObjects");
		}

		protected virtual void DoCommandToolsFilterObjects()
		{
            string filePath = ObjectFilterSettings.SIADefaultFilePath;
            ObjectFilterSettings filterSettings = 
                ObjectFilterSettings.Deserialize(filePath);
            if (filterSettings == null)
                filterSettings = new ObjectFilterSettings();

            DlgObjectFilterEx dlg = new DlgObjectFilterEx(filterSettings);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                filterSettings = dlg.Settings;

                SimpleFilterCommand.ApplyFilter(_objectList, filterSettings);

                ArrayList filteredObjects = null;
                
                if (_objectList != null)
                {
                    filteredObjects = new ArrayList(_objectList.Count);
                    filteredObjects.AddRange(_objectList);
                }

                this._workspace.DetectedObjects = filteredObjects;
                this._workspace.Invalidate(true);

                RasterCommandSettingsSerializer.Serialize(filePath, filterSettings);
            }
		}

		#endregion
		
		#endregion

		#region internal helpers

		#region process apply filter to Object List
		
		public virtual void ApplyFilter()
		{				
			ArrayList index = null;
			try
			{				
				index = new ArrayList();								
                //DataView view = gridObject.DataTable.DefaultView;								
                //foreach(DataRowView drv in view)
                //{
                //    int ind = Convert.ToInt32(drv[0].ToString())-1;
                //    index.Add(ind);
                //}
                //index.Sort();
								
                //int iIndx = index.Count-1;
                //for (int i=_objectList.Count-1; i >=0; i--)
                //{
                //    if (iIndx >= 0 && i == (int)index[iIndx]) 
                //    {
                //        iIndx--;
                //        continue;
                //    }
                //    _objectList.RemoveAt(i);
                //}				
			}
			catch (System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{
				index.Clear();
			}
		}

		public virtual bool LoadObjectFilterSettings(ObjectFilterArguments settings)
		{
			bool bResult = false;
			try
			{
				String filename = AppSettings.MySettingsFolder + "\\ObjectFilterSettings.ofs";
				if(File.Exists(filename) == false)
					return false;
				settings.Deserialize(filename);				
				bResult = true;
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}	

			return bResult;
		}

		public virtual void SaveObjectFilterSettings(ObjectFilterArguments settings)
		{			
			try
			{
                String filename = AppSettings.MySettingsFolder + "\\ObjectFilterSettings.ofs";				
				settings.Serialize(filename);
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}	
		}

		
		#endregion

		private bool CanCut()
		{
			if (this._workspace == null)
				return false;

			return (this._workspace.SelectedObjects != null && this._workspace.SelectedObjects.Count > 0);
		}

		private bool CanCopy()
		{
			if (this._workspace == null)
				return false;

			return (this._workspace.SelectedObjects != null && this._workspace.SelectedObjects.Count > 0);
		}

		private bool CanPaste()
		{
			if (this._workspace == null)
				return false;

			bool result = false;

			try
			{
				IDataObject obj = Clipboard.GetDataObject();
				result = obj.GetDataPresent(typeof(DetectedObjectCollection));
			}
			catch(System.Exception exp)
			{
				Trace.WriteLine(exp);
			}
			finally
			{
			}

			return result;
		}

		private bool CanDelete()
		{
			if (this._workspace == null)
				return false;

			return (this._workspace.SelectedObjects != null && this._workspace.SelectedObjects.Count > 0); 
		}


		

        //private void UpdateData(bool saveAndValidate, ExportObjectHistogramCommandSettings settings)
        //{
        //    if (saveAndValidate)
        //    {                
        //    }
        //    else
        //    {
        //        this._ignoreUpdateUICounter++;                

        //        this._ignoreUpdateUICounter--;

        //        // refresh ui elements
        //        this.UpdateUI();
        //    }
        //}

		private void UpdateUI()
		{
			
		}

		private void UpdateMainMenu()
		{
			bool objectAvailable = this._objectList!=null && this._objectList.Count>0;
			bool hasSelection = objectAvailable && _workspace.SelectedObjects != null && _workspace.SelectedObjects.Count > 0;
			this.mnuFileSaveData.Enabled = objectAvailable;
			//this.mnuFileExportKlarf.Enabled = objectAvailable;

			this.mnuEditSelectAll.Enabled = objectAvailable;
			this.mnuEditDeleteSelected.Enabled = hasSelection;
			this.mnuEditCut.Enabled = this.CanCut();
			this.mnuEditCopy.Enabled = this.CanCopy();			
			this.mnuEditDeleteSelected.Enabled = this.CanDelete();

			if (this._workspace != null && this.ObjectAnalyzer != null)
			{
				this.mnuViewAllObjects.Enabled = true;
				this.mnuViewHighlightSelection.Enabled = true;

				this.mnuViewAllObjects.Checked = objectAvailable && this.ObjectAnalyzer.DrawAllObjects;
				this.mnuViewHighlightSelection.Checked = objectAvailable && this.ObjectAnalyzer.HighlightSelectedObjects;
			}
			else
			{
				this.mnuViewAllObjects.Enabled = false;
				this.mnuViewHighlightSelection.Enabled = false;
			}

			this.mnuToolsFilterObjects.Enabled = objectAvailable;
		}

		private void ExportExcel(string fileName)
		{
            //using (Bitmap bitmap = this.histogramViewer.ExportChartBitmap(624, 280))
            //{
            //    using (ExcelExporter exporter = new ExcelExporter())
            //        exporter.Export(fileName, this._objectList, bitmap);
            //}
		}

		private void ExportCVS(string filename)
		{
            DefectExporter.SaveAsCSV(
                _objectList, _workspace.GetCurrentMetrologySystem(), 
                filename, _workspace.FilePath, "");
		}

		private void ExportExcelHtml(string fileName)
		{
            //if (this._workspace == null)
            //    return;

            //ExportObjectHistogramCommandSettings settings = new ExportObjectHistogramCommandSettings();
            //this.UpdateData(true, settings);
            //settings.FileName = fileName;		
            //this._workspace.AppWorkspace.DispatchCommand("CmdAnalysisExportObjectList", settings);
        }

        #endregion internal helpers

        #region Object properties preview
        private void DoShowHidePrivewPanel()
        {
            mnShowHidePreview.Checked = !mnShowHidePreview.Checked;
            panelPrevew.Visible = mnShowHidePreview.Checked;
        }


        TreeNode _root = new TreeNode("Selected Object Information");
        //TreeNode _imageNode = null;
        TreeNode _idNode = new TreeNode("Id: N/A");
        TreeNode _objectTypeCategory = new TreeNode("Classified Object Type");
        TreeNode _objectTypeNode = new TreeNode("Type: N/A");
        TreeNode _objecTypeNameNode = new TreeNode("Name: N/A");

        TreeNode _generalCategory = new TreeNode("General Information");
        TreeNode _rectBoundNodes = new TreeNode("Rectangle Boundary");
        TreeNode _leftNode = new TreeNode("Left: N/A");
        TreeNode _topNode = new TreeNode("Top: N/A");
        TreeNode _widthNode = new TreeNode("Width: N/A");
        TreeNode _heightNode = new TreeNode("Height: N/A");
        TreeNode _pixelCountNode = new TreeNode("Pixel Count: N/A");
        TreeNode _areaNode = new TreeNode("Area: N/A");
        TreeNode _perimeterNode = new TreeNode("Perimeter: N/A");
        TreeNode _compactNode = new TreeNode("Compact (Area / Perimeter): N/A");
        TreeNode _intensityNodes = new TreeNode("Intensity");
        TreeNode _averageIntensityNode = new TreeNode("Average Intensity: N/A");
        TreeNode _integratedIntensityNode = new TreeNode("Integrated Intensity: N/A");

        TreeNode _densityShapeCategory = new TreeNode("Elliptical Density Shape");
        TreeNode _orientationNode = new TreeNode("Orientation (): N/A");
        TreeNode _xCentroidNode = new TreeNode("X-Centroid: N/A");
        TreeNode _yCentroidNode = new TreeNode("Y-Centroid: N/A");
        TreeNode _majorAxisLengthNode = new TreeNode("Major Length: N/A");
        TreeNode _minorAxisLengthNode = new TreeNode("Minor Length: N/A");
        TreeNode _elongationNode = new TreeNode("Elongation: N/A");
        TreeNode _eccentricityNode = new TreeNode("Eccentricity: N/A");
        Image _imageObjPreview = null;
        private void InitializeObjectPropertiesPreview()
        {
            //_imageObjPreview = new Bitmap(
            //    panelThumbnail.Width, panelThumbnail.Height, PixelFormat.Format24bppRgb);
            //ResetImageObj();
            panelThumbnail.Visible = false;
            //panelThumbnail.Paint += new PaintEventHandler(panelThumbnail_Paint);
            //panelThumbnail.Invalidate(true);

            _objectTypeCategory.Nodes.AddRange(
                new TreeNode[] {
                    _objectTypeNode, _objecTypeNameNode
                });

            _rectBoundNodes.Nodes.AddRange(
                new TreeNode[] {
                    //_leftNode, _topNode, 
                    _widthNode, _heightNode
                });
            _intensityNodes.Nodes.AddRange(
                new TreeNode[] {
                    _averageIntensityNode, _integratedIntensityNode
                });
            _generalCategory.Nodes.AddRange(
                new TreeNode[] {
                    _pixelCountNode, 
                    _areaNode, _perimeterNode, /*_compactNode,*/
                    //_rectBoundNodes, 
                    _intensityNodes
                });

            _densityShapeCategory.Nodes.AddRange(
                new TreeNode[] {
                    _orientationNode,
                    _xCentroidNode, _yCentroidNode,
                    _majorAxisLengthNode, _minorAxisLengthNode,
                    _elongationNode, _eccentricityNode
                });

            
            _root.Nodes.AddRange(
                new TreeNode[] {
                    _idNode, /*_imageNode,*/ _objectTypeCategory, _generalCategory, _densityShapeCategory
                });
            treeViewObjectProperties.Nodes.Add(_root);

            _root.ForeColor = Color.Red;
            _objectTypeCategory.ForeColor = Color.Red;
            _generalCategory.ForeColor = Color.Red;
            _densityShapeCategory.ForeColor = Color.Red;
            _rectBoundNodes.ForeColor = Color.Red;
            _intensityNodes.ForeColor = Color.Red;

            treeViewObjectProperties.ExpandAll();
        }

        void panelThumbnail_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (_imageObjPreview == null)
                    return;

                Rectangle src = new Rectangle(0, 0, _imageObjPreview.Width, _imageObjPreview.Height);
                Rectangle dst = new Rectangle(0, 0, panelThumbnail.Width, panelThumbnail.Height);
                e.Graphics.DrawImage(_imageObjPreview, dst, src, GraphicsUnit.Pixel);
                using (Pen pen = new Pen(Color.Red, 2.0f))
                {
                    e.Graphics.DrawRectangle(pen,
                        0, 0, dst.Width - 1f, dst.Height - 1f);
                }
            }
            catch
            {
            }
        }

        private void ResetObjectPropertiesPreview()
        {
            //if (panelThumbnail.Visible == true)
            //    panelThumbnail.Visible = false;

            //ResetImageObj();
            //panelThumbnail.Invalidate(true);

            UpdateNode(_idNode, "Id", "N/A");
            
            UpdateNode(_objectTypeNode, "Type", "N/A");
            UpdateNode(_objecTypeNameNode, "Name", "N/A");

            //UpdateNode(_leftNode, "Left", "N/A");
            //UpdateNode(_topNode, "Top", "N/A");
            //UpdateNode(_widthNode, "Width", "N/A");
            //UpdateNode(_heightNode, "Height", "N/A");

            UpdateNode(_pixelCountNode, "Pixel Count", "N/A");
            UpdateNode(_areaNode, "Area", "N/A");
            UpdateNode(_perimeterNode, "Perimeter", "N/A");
            UpdateNode(_compactNode, "Compact", "N/A");

            UpdateNode(_averageIntensityNode, "Average Intensity", "N/A");
            UpdateNode(_integratedIntensityNode, "Integrated Intensity", "N/A");

            UpdateNode(_orientationNode, "Orientation", "N/A");
            UpdateNode(_xCentroidNode, "X-Centroid", "N/A");
            UpdateNode(_yCentroidNode, "Y-Centroid", "N/A");
            UpdateNode(_majorAxisLengthNode, "Major Length", "N/A");
            UpdateNode(_minorAxisLengthNode, "Minor Length", "N/A");
            UpdateNode(_elongationNode, "Elongation", "N/A");
            UpdateNode(_eccentricityNode, "Eccentricity", "N/A");
        }

        private void ResetImageObj()
        {
            if (_imageObjPreview == null)
            {
                _imageObjPreview = new Bitmap(
                imageListObj.ImageSize.Width,
                imageListObj.ImageSize.Height,
                PixelFormat.Format32bppArgb);
            }

            if (_imageObjPreview != null)
            {
                int width = _imageObjPreview.Width;
                int height = _imageObjPreview.Height;
                using (Graphics grph = Graphics.FromImage(_imageObjPreview))
                {
                    grph.Clear(Color.White);

                    using (Pen pen = new Pen(Color.Red, 2.0f))
                    {
                        grph.DrawRectangle(pen, 1, 1, width - 2, height - 2);

                        grph.DrawLine(pen, 0, 0, width - 1, height - 1);

                        grph.DrawLine(pen, width - 1, 0, 0, height - 1);
                    }
                }
            }
        }

        private void UpdateNode(
            TreeNode node, string description, object value)
        {
            node.Text = description + ": " + value.ToString();
        }

        private void UpdateNode(
            TreeNode node, string description, object value, string unit)
        {
            node.Text = description + ": " + value.ToString() + unit;
        }

        private void UpdateObjectPropertiesPreview(DetectedObject obj)
        {
            //UpdateObjThumbnail(_workspace.Image, obj, _imageObjPreview);
            //if (panelThumbnail.Visible == false)
            //    panelThumbnail.Visible = true;
            //panelThumbnail.Invalidate(true);

            int id = this._workspace.DetectedObjects.IndexOf(obj);
            if (id >= 0) id += 1;
            UpdateNode(_idNode, "Id", id);

            int objTypeId = (int)obj.ObjectTypeId;
            string objType = ((objTypeId >= 0 && objTypeId < (int)eDefectType.SuperObject ? "Primitive" : ((eDefectType)obj.ObjectTypeId).ToString()));            
            UpdateNode(_objectTypeNode, "Type", objType);

            string sigName = "";
            if (obj is DetectedObjectEx)
            {
                sigName = string.Format(
                    "{0} - ({1} primitive objects)", 
                    (obj as DetectedObjectEx).SignatureName,
                    (obj as DetectedObjectEx).PrimitiveObjects.Count);
            }
            else
            {
                sigName = 
                    ((SiGlaz.Common.ABSDefinitions.eDefectType)obj.ObjectTypeId).ToString();
            }
                        
            //string objName = "";
            //if (objType == "")
            //{
            //    objName = ((eDefectType)obj.ObjectTypeId).ToString();
            //}
            //else
            //{
            //    objName = ((eDefectType)obj.ObjectTypeId).ToString();
            //}
            //UpdateNode(_objecTypeNameNode, "Name", objName);
            UpdateNode(_objecTypeNameNode, "Name", sigName);

            ImageAnalyzer imageAnalyzer = 
                _workspace.GetAnalyzer("ImageAnalyzer") as ImageAnalyzer;
            EllipticalDensityShapeObject objWrapper = 
                EllipticalDensityShapeObject.FromDetectedObject(obj, imageAnalyzer.MetrologySystem);

            //UpdateNode(_leftNode, "Left", obj.RectBound.Left, " (pixel)");
            //UpdateNode(_topNode, "Top", obj.RectBound.Top, " (pixel)");
            //UpdateNode(_widthNode, "Width", 
            //    imageAnalyzer.MetrologySystem.CurrentUnit.FromPixel(obj.RectBound.Width), " (micron)");
            //UpdateNode(_heightNode, "Height", 
            //    imageAnalyzer.MetrologySystem.CurrentUnit.FromPixel(obj.RectBound.Height), " (micron)");

            UpdateNode(_pixelCountNode, "Pixel Count", obj.NumPixels);
            UpdateNode(_areaNode, "Area", obj.Area.ToString("0.000"), " (micron^2)");
            UpdateNode(_perimeterNode, "Perimeter", obj.Perimeter.ToString("0.000"), " (micron)");
            UpdateNode(_compactNode, "Compact", (obj.Area / obj.Perimeter).ToString("0.00000"));

            UpdateNode(_averageIntensityNode, "Average Intensity", (obj.TotalIntensity / obj.NumPixels).ToString("0.000"));
            UpdateNode(_integratedIntensityNode, "Integrated Intensity", obj.TotalIntensity);
            
            double orientation = objWrapper.Orientation;            
            if (orientation < 0) orientation += 180;
            else if (orientation > 180) orientation -= 180;
            orientation = 180.0 - orientation;

            UpdateNode(_orientationNode, "Orientation [0..180]",
                string.Format("{0}", orientation.ToString("0.000")));
            UpdateNode(_xCentroidNode, "X-Centroid", objWrapper.CenterX.ToString("0.000"), " (micron)");
            UpdateNode(_yCentroidNode, "Y-Centroid", objWrapper.CenterY.ToString("0.000"), " (micron)");
            UpdateNode(_majorAxisLengthNode, "Major Length", objWrapper.MajorLength.ToString("0.000"), " (micron)");
            UpdateNode(_minorAxisLengthNode, "Minor Length", objWrapper.MinorLength.ToString("0.000"), " (micron)");
            UpdateNode(_elongationNode, "Elongation", objWrapper.Elongation.ToString("0.000"));
            UpdateNode(_eccentricityNode, "Eccentricity", objWrapper.Eccentricity.ToString("0.000"));
        }

        private void UpdateObjThumbnail(
                CommonImage srcImage, DetectedObject obj, Image bmp)
        {
            int width = bmp.Width;
            int height = bmp.Height;

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
                        Color[] colors = SiGlaz.Common.ABSDefinitions.DefectVisualizer.Colors;
                        int nSupportedColors = colors.Length;
                        using (GraphicsPath path =
                            ObjectAnalyzer.ObjectToGraphicsPath(obj))
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
        }
        #endregion Object properties preview

        #region List view
        
        protected virtual void DoCommandEditSelectAll()
        {
            objectListView.Viewer.SelectAll();
        }

        public void UpdateSelection()
        {
            ArrayList detectedObjects = this._workspace.DetectedObjects;
            ArrayList selection = this._workspace.SelectedObjects;
            try
            {
                Interlocked.Increment(ref _ignoreSelectionChanged);                
                List<DetectedObjectView> slv = new List<DetectedObjectView>(10);
                for (int i = 0; i < selection.Count; i++)
                {
                    int index = detectedObjects.IndexOf(selection[i]);
                    slv.Add(_objListView[index]);
                }

                objectListView.Viewer.SelectedObjects = slv;

                if (slv != null && slv.Count > 0)
                    objectListView.Viewer.EnsureModelVisible(slv[0]);

                UpdateObjectPropertiesPreview(selection);
            }
            finally
            {
                Interlocked.Decrement(ref _ignoreSelectionChanged);
            }
        }

        void Viewer_DoubleClick(object sender, EventArgs e)
        {
            Viewer_SelectionChanged(sender, e);

            if (_workspace.SelectedObjects != null && 
                _workspace.SelectedObjects.Count > 0)
            {
                DetectedObject obj = _workspace.SelectedObjects[0] as DetectedObject;

                Rectangle rect = Rectangle.Round(obj.RectBound);

                if (_workspace.ImageViewer.ScaleFactor < 1.0f)
                    _workspace.ImageViewer.ScaleFactor = 1.0f;

                using (Transformer transformer = this._workspace.ImageViewer.Transformer)
                {
                    int x = (int)(rect.Left + rect.Width * 0.5F);
                    int y = (int)(rect.Top + rect.Height * 0.5F);
                    Point pt = Point.Round(transformer.PointToPhysical(new Point(x, y)));
                    this._workspace.ImageViewer.CenterAtPoint(pt);
                }
            }
        }

        void Viewer_SelectionChanged(object sender, EventArgs e)
        {
            if (_ignoreSelectionChanged == 0)
            {
                try
                {
                    Interlocked.Increment(ref _ignoreSelectionChanged);

                    ArrayList selectedObjects = new ArrayList();

                    if (objectListView.Viewer.SelectedObjects != null &&
                        objectListView.Viewer.SelectedObjects.Count > 0)
                    {
                        selectedObjects.Capacity = objectListView.Viewer.SelectedObjects.Count;

                        IList tmp = objectListView.Viewer.SelectedObjects;
                        foreach (DetectedObjectView ov in tmp)
                        {
                            selectedObjects.Add(_objectList[ov.Index]);
                        }
                    }

                    this._workspace.SelectedObjects = selectedObjects;
                    
                    this._workspace.Invalidate(true);

                    UpdateObjectPropertiesPreview(selectedObjects);
                }
                catch (System.Exception exp)
                {
                    Trace.WriteLine(exp);
                }
                finally
                {
                    Interlocked.Decrement(ref _ignoreSelectionChanged);
                }
            }
        }
       
        private void UpdateObjectPropertiesPreview(ArrayList selectedObjects)
        {
            if (selectedObjects == null || selectedObjects.Count != 1)
            {
                ResetObjectPropertiesPreview();
            }
            else
            {
                UpdateObjectPropertiesPreview(selectedObjects[0] as DetectedObject);
            }
        }
        #endregion List view
    }

    internal class DetectedObjectListViewer : Control
    {
        #region Member fields
        private ImageList _smallImageList = null;
        private ImageList _largeImageList = null;
        private ImageList _groupImageList = null;

        private FastObjectListView _listView = null;
        [Browsable(false)]
        public FastObjectListView Viewer
        {
            get { return _listView; }
        }
        #endregion Member fields

        #region Constructors and destructors
        public DetectedObjectListViewer()
        {
            InitializeListView();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_listView != null)
                {
                    DisposeImageLists();

                    _listView.ClearObjects();
                    _listView.Clear();
                    _listView.Dispose();
                    _listView = null;
                }
            }

            base.Dispose(disposing);
        }

        protected virtual void DisposeImageLists()
        {
            _listView.SmallImageList = null;
            Dispose(_smallImageList);

            _listView.LargeImageList = null;
            Dispose(_largeImageList);

            _listView.GroupImageList = null;
            Dispose(_groupImageList);
        }

        protected virtual void Dispose(ImageList imageList)
        {
            if (imageList != null)
            {
                imageList.Dispose();
                imageList = null;
            }
        }
        #endregion Constructors and destructors

        protected virtual void InitializeListView()
        {
            _listView = new FastObjectListView();

            // create column instances
            _index = new IndexColumn();
            _type = new TypeColumn();
            _xCentroid = new XCentroidColumn();
            _yCentroid = new YCentroidColumn();
            _pixelCount = new PixelCountColumn();
            _averageIntensity = new AverageIntensityColumn();

            ((System.ComponentModel.ISupportInitialize)(this._listView)).BeginInit();
            this.SuspendLayout();

            this._listView.AllColumns.Add(this._index);
            this._listView.AllColumns.Add(this._type);
            this._listView.AllColumns.Add(this._xCentroid);
            this._listView.AllColumns.Add(this._yCentroid);
            this._listView.AllColumns.Add(this._pixelCount);
            this._listView.AllColumns.Add(this._averageIntensity);

            this._listView.AlternateRowBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this._listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._index,
            this._type,
            this._xCentroid,
            this._yCentroid,
            this._pixelCount,
            this._averageIntensity});
            this._listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._listView.FullRowSelect = true;
            this._listView.GridLines = true;
            //this._listView.LargeImageList = this._largeImageList;
            this._listView.Location = new System.Drawing.Point(0, 0);
            this._listView.Name = "_listView";
            this._listView.OwnerDraw = true;
            this._listView.Size = new System.Drawing.Size(628, 399);
            //this._listView.SmallImageList = this._smallImageList;
            this._listView.TabIndex = 0;
            this._listView.UseCompatibleStateImageBehavior = false;
            this._listView.View = System.Windows.Forms.View.Details;
            this._listView.VirtualMode = true;

            this.Controls.Add(this._listView);
            _listView.SelectAllOnControlA = true;
            _listView.ShowGroups = true;
            //_listView.ShowItemCountOnGroups = true;
            _listView.ShowItemToolTips = true;
            _listView.UseTranslucentHotItem = true;
            _listView.UseTranslucentSelection = true;
            _listView.BuildGroups(_type, SortOrder.None);
            _listView.ColumnClick += new ColumnClickEventHandler(_listView_ColumnClick);
            ((System.ComponentModel.ISupportInitialize)(this._listView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

            // Use different font under Vista
            if (ObjectListView.IsVistaOrLater)
                this.Font = new Font("Segoe UI", 9);

            
        }

        private int _currentColumn = -1;
        void _listView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                if (e.Column == _currentColumn)
                {
                    return;
                }

                _currentColumn = e.Column;
                bool bRebuild = false;

                if (_currentColumn != 1 && _listView.ShowGroups)
                {
                    _listView.ShowItemCountOnGroups = false;
                    _listView.ShowGroups = false;                    
                    bRebuild = true;
                }
                else if (_currentColumn == 1 && !_listView.ShowGroups)
                {
                    _listView.ShowGroups = true;
                    _listView.ShowItemCountOnGroups = true;
                    bRebuild = true;
                }

                if (bRebuild)
                {
                    _listView.BuildList();
                }
            }
            catch
            {
            }
        }

        protected OLVColumn _index = null;
        protected OLVColumn _type = null;
        protected OLVColumn _xCentroid = null;
        protected OLVColumn _yCentroid = null;
        protected OLVColumn _pixelCount = null;
        protected OLVColumn _averageIntensity = null;


        public void UpdateObjectList(List<DetectedObjectView> data)
        {
            if (data == null || data.Count == 0)
            {
                _listView.SetObjects(new List<DetectedObjectView>());
                return;
            }

            try
            {
                _listView.SetObjects(data);
                if (_currentColumn == 1 && !_listView.ShowGroups)
                {
                    _listView.ShowGroups = true;
                    _listView.ShowItemCountOnGroups = true;
                }
                else
                {
                    _listView.ShowItemCountOnGroups = false;
                    _listView.ShowGroups = false;                    
                }

                _listView.BuildList();
            }
            catch
            {
            }
        }


        #region custom column
        class IndexColumn : OLVColumn
        {
            public IndexColumn() : base()
            {
                this.AspectName = "Index";
                this.Text = "Index";
                this.Hyperlink = true;
                this.UseInitialLetterForGroup = false;
                this.Width = 70;
                this.IsEditable = false;

                this.AspectGetter = delegate(object x) 
                {
                    return (x as DetectedObjectView).Index + 1;
                };

                //this.ImageGetter = delegate(object x)
                //{
                //    return (x as DetectedObjectView).Index;
                //};
            }
        }

        class TypeColumn : OLVColumn
        {
            public TypeColumn()
                : base()
            {
                this.AspectName = "TypeName";
                this.Text = "Type";
                this.UseInitialLetterForGroup = false;
                this.Width = 90;
                this.IsEditable = false;
                this.GroupWithItemCountFormat = "{0} ({1} objects)";
                this.GroupWithItemCountSingularFormat = "{0} (only {1} object)";

                this.AspectGetter = delegate(object x)
                {
                    return (x as DetectedObjectView).TypeName;
                };
            }
        }

        class XCentroidColumn : OLVColumn
        {
            public XCentroidColumn()
                : base()
            {
                this.AspectName = "XCentroid";
                this.Text = "X-Centroid";
                this.UseInitialLetterForGroup = false;
                this.AspectToStringFormat = "{0:0.##}";
                this.Width = 70;
                this.IsEditable = false;

                this.AspectGetter = delegate(object x)
                {
                    return (x as DetectedObjectView).XCentroid;
                };
            }
        }

        class YCentroidColumn : OLVColumn
        {
            public YCentroidColumn()
                : base()
            {
                this.AspectName = "YCentroid";
                this.Text = "Y-Centroid";
                this.UseInitialLetterForGroup = false;
                this.AspectToStringFormat = "{0:0.##}";
                this.Width = 70;
                this.IsEditable = false;

                this.AspectGetter = delegate(object x)
                {
                    return (x as DetectedObjectView).YCentroid;
                };
            }
        }

        class PixelCountColumn : OLVColumn
        {
            public PixelCountColumn()
                : base()
            {
                this.AspectName = "PixelCount";
                this.Text = "Pixel Count";
                this.UseInitialLetterForGroup = false;
                this.Width = 75;
                this.IsEditable = false;

                this.AspectGetter = delegate(object x)
                {
                    return (x as DetectedObjectView).DetectedObject.NumPixels;
                };
            }
        }

        class AverageIntensityColumn : OLVColumn
        {
            public AverageIntensityColumn()
                : base()
            {
                this.AspectName = "AverageIntensity";
                this.Text = "Average Intensity";
                this.UseInitialLetterForGroup = false;
                this.AspectToStringFormat = "{0:0.##}";
                this.Width = 110;
                this.IsEditable = false;

                this.AspectGetter = delegate(object x)
                {
                    return (x as DetectedObjectView).AverageIntensity;
                };
            }
        }
        #endregion custom column
    }

    public class DetectedObjectView
    {
        public int Index = 0;
        public string TypeName = "";
        public double XCentroid = 0;
        public double YCentroid = 0;
        public double AverageIntensity = 0;

        public DetectedObject DetectedObject = null;
        
        public DetectedObjectView(int id, DetectedObject obj, MetrologySystem ms)
        {
            Index = id;
            DetectedObject = obj;

            AverageIntensity = obj.TotalIntensity / obj.NumPixels;

            if (obj is DetectedObjectEx)
                TypeName = "SuperObject";
            else if (obj.ObjectTypeId < 0)
                TypeName = "Unknow";
            else
            {
                eDefectType dft = (eDefectType)obj.ObjectTypeId;
                switch(dft)
                {
                    case eDefectType.DarkObject:
                    case eDefectType.DarkObjectAcrossBoundary:
                        TypeName = "Dark";
                        break;
                    case eDefectType.BrightObject:
                    case eDefectType.BrightObjectAcrossBoundary:
                        TypeName = "Bright";
                        break;
                    case eDefectType.Unknown:
                    default:
                        TypeName = "Unknow";
                        break;
                }
            }

            EllipticalDensityShapeObject objWrapper =
                EllipticalDensityShapeObject.FromDetectedObject(obj, ms);

            XCentroid = objWrapper.CenterX;
            YCentroid = objWrapper.CenterY;
        }
    }    
}
