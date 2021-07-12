using System;
using System.Collections.Generic;
using System.Text;

using SIA.UI.Controls.Automation;
using System.Drawing;
using System.Windows.Forms;
using SiGlaz.UI.CustomControls;
using System.Collections;
using SIA.Workbench.UserControls;

namespace SIA.Workbench
{
    internal class SIAWExplorerBarManager
    {
        #region Member fields
        private ExplorerBarContainer _explorerBarContainer = null;
        private SplitterEx _splitter = null;
        private IExplorerBarTheme _theme = null;

        private List<ExplorerItemsGroup> _groups = new List<ExplorerItemsGroup>(5);

        public event ExplorerBarEventHandler ItemClicked;

        private Hashtable _lookupTable = null;
        #endregion Member fields

        #region Properties
        public ExplorerBarContainer ExplorerBarContainer
        {
            get
            {
                return _explorerBarContainer;
            }
        }

        public ExplorerBar ExplorerBar
        {
            get
            {
                if (_explorerBarContainer == null)
                    return null;

                return _explorerBarContainer.ExplorerBar;
            }
        }
        #endregion Properties

        #region Constructors
        public SIAWExplorerBarManager(
            ExplorerBarContainer explorerBarContainer, SplitterEx splitter)
        {
            //_appWorkspace = owner;
            _explorerBarContainer = explorerBarContainer;
            _splitter = splitter;

            _splitter.ControlToHide = _explorerBarContainer;

            Initialize();
        }
        #endregion Constructors

        #region Initializations
        private void Initialize()
        {
            InitializeExplorerBarProperties();

            InitializeCategories();
            
            // register for event handlers
            _explorerBarContainer.AutoHideButtonClicked += 
                new EventHandler(AutoHideButtonClicked);

            this._splitter.SplitterMoved +=
                new System.Windows.Forms.SplitterEventHandler(this.SplitterMoved);
        }

        private void InitializeExplorerBarProperties()
        {
            _theme = new ExplorerBarLunaBlueTheme();
            this.ExplorerBar.Theme = _theme;
            this.ExplorerBar.AllowMultiExpanding = false;            
        }

        private void InitializeCategories()
        {
            _groups.Clear();

            // initialize categories
            string allStepCategory = "All Process Steps";
            ExplorerItemsGroup group = this.GetCategory(allStepCategory);
            if (group == null)
                group = this.AddCategory(allStepCategory);
            //group.Image = Resource.CustomProcessNodeIcon.Clone() as Image;
            string[] allCategories = ProcessStepCategories.AllCategories;
            foreach (string category in allCategories)
            {
                group = this.GetCategory(category);
                if (group == null)
                    this.AddCategory(category);
            }

            ProcessStepInfo[] stepInfos = ProcessStepManager.GetRegistedProcessSteps();
            if (stepInfos != null)
            {
                Array.Sort(stepInfos, new ProcessStepInfoComparer());
            }
            foreach (ProcessStepInfo stepInfo in stepInfos)
            {
                group = this.GetCategory(allStepCategory);
                if (group == null)
                    group = this.AddCategory(allStepCategory);
                
                Image image = null;
                if (stepInfo.IsBuiltIn)
                    image = Resource.GetProcessStepIcon(stepInfo.Type.Name);
                else
                    image = Resource.GetProcessStepIcon(stepInfo.ID);

                if (image == null)
                    image = Resource.CustomProcessNodeIcon;                

                // create new explorer bar item and add it into the band
                ExplorerItem item = new ExplorerItem(image.Clone() as Image, stepInfo.DisplayName);
                item.Tag = stepInfo;
                group.Add(item);

                string category = stepInfo.Category;
                if (category != null && 
                    category != string.Empty && 
                    category != allStepCategory)
                {
                    group = this.GetCategory(category);
                    if (group == null)
                        group = this.AddCategory(category);
                    if (group.Items.Count == 0)
                        group.Image = image.Clone() as Image;
                    
                    // create new explorer bar item and add it into the band
                    item = new ExplorerItem(image.Clone() as Image, stepInfo.DisplayName);
                    item.Tag = stepInfo;
                    group.Add(item);
                }

                if (image != null)
                {
                    image.Dispose();
                    image = null;
                }
            }

            foreach (ExplorerItemsGroup itemGroup in _groups)
                this.ExplorerBar.AddGroup(itemGroup);

            if (_groups.Count > 1)
                _groups[1].Status = eExplorerItemGroupStatus.Expanded;
        }

        internal class ProcessStepInfoComparer : IComparer
        {
            #region IComparer Members

            public int Compare(object x, object y)
            {
                ProcessStepInfo si1 = x as ProcessStepInfo;
                ProcessStepInfo si2 = y as ProcessStepInfo;

                return String.Compare(si1.DisplayName, si2.DisplayName);
            }

            #endregion

        }
        #endregion Initializations

        #region Events
        private void SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
        {
            if (_explorerBarContainer.Width < 120)
            {
                _explorerBarContainer.Width = 120;
                _splitter.ToggleSplitter();
            }
        }

        private void AutoHideButtonClicked(object sender, EventArgs e)
        {
            _splitter.ToggleSplitter();
        }
        #endregion Events

        #region Helpers
        private ExplorerItemsGroup AddCategory(string text)
        {
            ExplorerItemsGroup group = new ExplorerItemsGroup(null, text);

            // add the created band to the control
            this._groups.Add(group);

            return group;
        }

        private ExplorerItemsGroup GetCategory(string text)
        {
            foreach (ExplorerItemsGroup group in this._groups)
                if (group.Text == text)
                    return group;
            return null;
        }
        #endregion Helper
    }
}
