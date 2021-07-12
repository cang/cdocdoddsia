#define SUPPORED_REAL_COORDINATE__

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SIA.UI.Controls.Dialogs;
using SiGlaz.Common.ImageAlignment;

namespace SIA.UI.Controls.UserControls
{
    public partial class ucMarkerList : UserControl
    {
        #region Member fields
        public event EventHandler SelectedMarkerChanged;
        public event EventHandler DeletedMarker;
        public event EventHandler DeletedAllMarkers;
        public event EventHandler MarkerHintClicked;
        public event EventHandler NavigatedToMarker;

        private MetrologySystem _metrologySystem = null;
        public MetrologySystem MetrologySystem
        {
            get { return _metrologySystem; }
            set
            {
                if (_metrologySystem != value)
                {
                    if (_metrologySystem != null)
                    {
                        _metrologySystem.NewMarkerAdded -= new EventHandler(_metrologySystem_NewMarkerAdded);
                    }

                    _metrologySystem = value;

                    OnMetrologySystemChanged();
                }
            }
        }

        private void OnMetrologySystemChanged()
        {
            _metrologySystem.NewMarkerAdded += new EventHandler(_metrologySystem_NewMarkerAdded);

#if SUPPORED_REAL_COORDINATE
            
#else
            markerListView.Columns.Remove(colXCentroidMicron);
            markerListView.Columns.Remove(colYCentroidMicron);
            markerListView.Update();

            btnEditItem.Visible = false;
#endif

            this.Update(true);
        }

        void _metrologySystem_NewMarkerAdded(object sender, EventArgs e)
        {
            int n = _metrologySystem.Markers.Count;
            AddNewMarkerToList(_metrologySystem.Markers[n - 1], n - 1);
        }

        public int SelectedMarkerIndex
        {
            get
            {
                if (markerListView.SelectedIndices == null ||
                    markerListView.SelectedIndices.Count == 0)
                    return -1;

                return markerListView.SelectedIndices[0];
            }

            set
            {
                if (markerListView.SelectedIndices != null)
                    markerListView.SelectedIndices.Clear();

                if (value >= 0 && value < markerListView.Items.Count)
                {
                    markerListView.SelectedIndices.Add(value); 
                    markerListView.EnsureVisible(value);
                    markerListView.Update();
                    markerListView.Focus();
                }
            }
        }

        public ListViewItem SelectedItem
        {
            get
            {
                if (markerListView.SelectedItems == null ||
                    markerListView.SelectedItems.Count == 0)
                    return null;

                return markerListView.SelectedItems[0];
            }
        }
        #endregion Member fields

        #region Constructors and destructors
        public ucMarkerList()
        {
            InitializeComponent();
        }
        #endregion Constructors and destructors

        #region Override routines
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            markerListView.Items.Clear();
        }
        #endregion Override routines

        #region Events
        private void btnEditItem_Click(object sender, EventArgs e)
        {
            int index = this.SelectedMarkerIndex;
            if (index < 0)
                return;

            MarkerPoint marker = this.SelectedItem.Tag as MarkerPoint;

            DlgCoordinate dlg = new DlgCoordinate();
            dlg.XCoordinate = (float)marker.PhysicalX;
            dlg.YCoordinate = (float)marker.PhysicalY;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                marker.PhysicalX = dlg.XCoordinate;
                marker.PhysicalY = dlg.YCoordinate;
            }
        }

        private void btnDeleteItem_Click(object sender, EventArgs e)
        {
            int index = this.SelectedMarkerIndex;
            if (index < 0) return;

            if (this.DeletedMarker != null)
            {
                // raise to owner process
                // delete current marker
                // rebuild list
                this.DeletedMarker(this, EventArgs.Empty);
            }
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            if (markerListView.Items.Count == 0)
                return;

            if (this.DeletedAllMarkers != null)
            {
                // raise to owner process
                // delete all markers
                // rebuild empty list
                this.DeletedAllMarkers(this, EventArgs.Empty);
            }
        }

        private void btnHintMarkers_Click(object sender, EventArgs e)
        {
            if (this.MarkerHintClicked != null)
            {
                // raise to owner process
                // delete all markers
                // detect markers using default-built-in settings                
                // rebuild list
                this.MarkerHintClicked(this, EventArgs.Empty);
            }
        }

        private void markerListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.SelectedMarkerChanged != null)
                this.SelectedMarkerChanged(this, EventArgs.Empty);
        }

        private void markerListView_DoubleClick(object sender, EventArgs e)
        {
            int index = this.SelectedMarkerIndex;
            if (index < 0) return;

            if (this.NavigatedToMarker != null)
            {
                this.NavigatedToMarker(this, EventArgs.Empty);
            }
        }
        #endregion Events        

        #region Methods
        public void UpdateMarkerLocation(MarkerPoint marker)
        {
            int index = this.SelectedMarkerIndex;
            if (index < 0) return;

            ListViewItem item = this.SelectedItem;
            if (item == null) return;

            int drawingXIndex = 1;
            int drawingYIndex = 2;
            item.SubItems[drawingXIndex].Text = marker.DrawingX.ToString();
            item.SubItems[drawingYIndex].Text = marker.DrawingY.ToString();
        }

        public virtual void Update(bool toControl)
        {
            if (toControl)
            {
                markerListView.Items.Clear();
                if (_metrologySystem != null)
                {
                    int index = 0;
                    foreach (MarkerPoint marker in _metrologySystem.Markers)
                    {
                        AddNewMarkerToList(marker, index++);
                    }
                }
            }
            else
            {
            }
        }

        protected virtual void AddNewMarkerToList(MarkerPoint marker, int index)
        {
            string id = String.Format("{0}", index + 1);
            ListViewItem item = new ListViewItem(
                new string[] { 
                    id, 
#if SUPPORED_REAL_COORDINATE
                    "", "",
#else                     
#endif
                    marker.DrawingX.ToString(), marker.DrawingY.ToString()});
            item.Text = id;
            item.Tag = marker;
            markerListView.Items.Add(item);
        }
        #endregion Methods        
    }
}
