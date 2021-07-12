using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SiGlaz.Algorithms.Core;
using SiGlaz.Common.ImageAlignment;
using SiGlaz.UI.CustomControls.ListViewEx;
using System.IO;

namespace SIA.UI.Controls.Dialogs
{
    public partial class DlgCalibration : Form
    {
        #region Member fields
        public event EventHandler SelectedLineChanged = null;
        public event EventHandler OnDeleteLine = null;
        public event EventHandler OnDeleteAll = null;
        public event EventHandler OnDrawLines = null;

        private ImageWorkspace _workspace = null;        
        public event EventHandler CalibrationCompleted;
        string _sUnitName = string.Empty;
        #endregion Member fields

        public float Pixel
        {            
            set 
            {
                double val = Math.Min(value, (double)txtPixel.Maximum);
                val = Math.Max(val, (double)txtPixel.Minimum);
                txtPixel.Value = (decimal)val; 
            }
            get { return (float)txtPixel.Value; }
        }

        public float Unit
        {
            set {
                double val = Math.Min(value, (double)txtUnit.Maximum);
                val = Math.Max(val, (double)txtUnit.Minimum);
                txtUnit.Value = (decimal)val; 
            }
            get { return (float)txtUnit.Value; }
        }

        private List<ScaleLine> _scaleLines = new List<ScaleLine>(3);
        public List<ScaleLine> ScaleLines
        {
            get { return _scaleLines; }         
        }
   
        public DlgCalibration(ImageWorkspace workspace, DlgMetrologyCoordinateSystem controller)
        {
            InitializeComponent();

            // initialize top level window
            //this.TopLevel = true;

            _workspace = workspace;

            if (_workspace != null)
                this.Owner = _workspace.FindForm();

            _sUnitName = controller.MetrologySystem.CurrentUnit.ShortName;
            label2.Text = string.Format("({0})", _sUnitName);
            lstView.Columns[4].Text = string.Format("Distance ({0})", _sUnitName);

            controller.CalibrationRequestCompleted += new EventHandler(controller_CalibrationRequestCompleted);
        }
              
        void controller_CalibrationRequestCompleted(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;            

            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {           
            this.Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (CalibrationCompleted != null)
            {
                this.CalibrationCompleted(this, EventArgs.Empty);
            }

            // visible this dialogue
            e.Cancel = true;
            this.Visible = false;
            base.OnClosing(e);
        }

      

        
        public void Update(ScaleLine line, int lineIndx)
        {
            if (lineIndx < 0)
                return;

            if (_scaleLines.Count < lineIndx + 1)//new line
            {
                _scaleLines.Add(line);
                Add2ListView(_scaleLines.Count, line);
            } 
            else //update line
            {
                _scaleLines[lineIndx] = line;
                UpdateListView(lineIndx, line);                
            }

            CalculateScale();
            SelectedLineIndex = lineIndx;
        }

        private void Add2ListView(int stt, ScaleLine line)
        {
            ListViewItem item = new ListViewItem();

            item.Text = stt.ToString();
            item.SubItems.Add(String.Format("(X={0:0.###} ; Y={1:0.###})", line.Point1.X, line.Point1.Y));
            item.SubItems.Add(String.Format("(X={0:0.###} ; Y={1:0.###})", line.Point2.X, line.Point2.Y));
            item.SubItems.Add(String.Format("{0:0.###}", line.PixelDistance));
            item.SubItems.Add(String.Format("{0:0.###}", line.UnitDistance));

            item.Tag = line;
            lstView.Items.Add(item);
        }

        private void UpdateListView(int row, ScaleLine line)
        {
            lstView.Items[row].SubItems[1].Text = String.Format("(X={0:0.###} ; Y={1:0.###})", line.Point1.X, line.Point1.Y);
            lstView.Items[row].SubItems[2].Text = String.Format("(X={0:0.###} ; Y={1:0.###})", line.Point2.X, line.Point2.Y);
            lstView.Items[row].SubItems[3].Text = String.Format("{0:0.###}", line.PixelDistance);
            lstView.Items[row].SubItems[4].Text = String.Format("{0:0.###}", line.UnitDistance);
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            _scaleLines.Clear(); 
            this.ClearAll();

            if (this.OnDeleteAll != null)
                this.OnDeleteAll(this, EventArgs.Empty);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int selectedIndx = SelectedLineIndex;
            if (selectedIndx < 0)
                return;

            _scaleLines.RemoveAt(selectedIndx);
            lstView.Items.RemoveAt(selectedIndx);

            if (_scaleLines.Count == 0)
            {
                this.ClearAll();
                SelectedLineIndex = -1;
            }
            else
            {
                CalculateScale();
                if (selectedIndx > 0)
                    SelectedLineIndex = selectedIndx - 1;
                else
                    SelectedLineIndex = 0;
            }
           
            if (this.OnDeleteLine != null)
                this.OnDeleteLine(selectedIndx, EventArgs.Empty);            
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int row = SelectedLineIndex;
            if (row < 0)
                return;

            float value = GetUnitDistance(row);
            if (value > 0)
            {
                lstView.Items[row].SubItems[4].Text = string.Format("{0:0.###}", value);
                _scaleLines[row].UnitDistance = value;
                CalculateScale();
            }
        }

        private float GetUnitDistance(int row)
        {
            DlgCalibrationValueInput dlgValueInput = new DlgCalibrationValueInput(
                _sUnitName, Convert.ToSingle(lstView.Items[row].SubItems[3].Text), Convert.ToSingle(lstView.Items[row].SubItems[4].Text));
            if (dlgValueInput.ShowDialog() == DialogResult.OK)
                return dlgValueInput.Value;

            return -1;//no update
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_scaleLines.Count == 0)
            {
                this.Pixel = 1;
                this.Unit = 1;
            }
            else
                CalculateScale();
        }

        private void CalculateScale()
        {
            float sumpPixelDistances = 0;
            float sumUnitDistances = 0;
            for (int i = _scaleLines.Count - 1; i >= 0; i--)
            {
                ScaleLine line = _scaleLines[i];
                sumpPixelDistances += line.PixelDistance;
                sumUnitDistances += line.UnitDistance;
            }

            float k = sumUnitDistances / sumpPixelDistances;

            this.Pixel = (float)Math.Round(sumpPixelDistances / _scaleLines.Count);
            this.Unit = this.Pixel * k;
        }    
   
        public int SelectedLineIndex
        {
            get
            {
                if (lstView.SelectedIndices == null || lstView.SelectedIndices.Count == 0)
                    return -1;

                return lstView.SelectedIndices[0];
            }

            set
            {
                if (lstView.SelectedIndices != null)
                    lstView.SelectedIndices.Clear();

                if (value >= 0 && value < lstView.Items.Count)
                {
                    lstView.SelectedIndices.Add(value);
                    lstView.EnsureVisible(value);
                    lstView.Update();
                    lstView.Focus();
                }
            }
        }

        private void lstView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstView.SelectedItems.Count == 0)
                return;

            if (this.SelectedLineChanged != null)
                this.SelectedLineChanged(lstView.SelectedItems[0].Index, EventArgs.Empty);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Filter = "Cabliration file (*.crf)|*.crf";
                dlg.Title = "Select a Cabliration File path";
                dlg.ShowDialog();
                if (dlg.FileName!=string.Empty)
                {
                    using (FileStream fs = new FileStream(dlg.FileName, FileMode.Create))
                    {
                        using (BinaryWriter bin = new BinaryWriter(fs))
                        {
                            MetrologyRuler ruler = new MetrologyRuler(_scaleLines, Pixel, Unit);
                            ruler.Serialize(bin);
                        }
                    }
                }
            }           
        }

       
        private void btnLoad_Click(object sender, EventArgs e)
        {         
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Cabliration file (crf)|*.crf";
                dlg.Title = "Open a Cabliration File";
                dlg.ShowDialog();
                if (dlg.FileName != string.Empty)
                {
                    using (FileStream fs = new FileStream(dlg.FileName, FileMode.Open))
                    {
                        using (BinaryReader bin = new BinaryReader(fs))
                        {
                            MetrologyRuler ruler = new MetrologyRuler();
                                ruler.Deserialize(bin);
                            Pixel = ruler.PixelRatio;
                            Unit = ruler.UnitRatio;
                            Init(ruler.ScaleLines);

                            if (this.OnDeleteAll != null)
                                this.OnDeleteAll(this, EventArgs.Empty);

                            if (this.OnDrawLines != null)
                                this.OnDrawLines(_scaleLines, EventArgs.Empty);

                            
                        }
                    }
                }
            }
        }

        private void Init(List<ScaleLine> scaleLines)
        {
            if (scaleLines == null || scaleLines.Count == 0)
                return;

            _scaleLines = scaleLines;
            

            this.ClearAll();

            for (int i = 0; i < _scaleLines.Count; i++)
                Add2ListView(i + 1, _scaleLines[i]);

            SelectedLineIndex = 0;

            CalculateScale();
        }

        private void ClearAll()
        {
            lstView.Items.Clear();
            txtPixel.Value = 1;
            txtUnit.Value = 1;
        }
    }

   
}

