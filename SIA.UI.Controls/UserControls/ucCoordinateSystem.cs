using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SiGlaz.Common.ImageAlignment;

namespace SIA.UI.Controls.UserControls
{
    public partial class ucCoordinateSystem : UserControl
    {
        #region Member fields
        protected CoordinateSystem _coordinateSystem = null;

        public event EventHandler CoordinateSystemChanged;
        #endregion Member fields

        #region Contructors and destructors
        public ucCoordinateSystem()
        {
            InitializeComponent();
        }

        public ucCoordinateSystem(CoordinateSystem coordinateSystem)
            : this()
        {
            this.CoordinateSystem = coordinateSystem;
        }
        #endregion Contructors and destructors

        #region Properties
        [Browsable(false)]
        public CoordinateSystem CoordinateSystem
        {
            get { return _coordinateSystem; }
            set
            {
                if (_coordinateSystem != value)
                {
                    if (_coordinateSystem != null)
                    {
                        _coordinateSystem.DataChanged -= new EventHandler(_coordinateSystem_DataChanged);
                    }

                    _coordinateSystem = value;

                    OnCoordinateSystemChanged();
                }
            }
        }

        protected virtual void OnCoordinateSystemChanged()
        {
            if (_coordinateSystem != null)
            {
                _coordinateSystem.DataChanged += new EventHandler(_coordinateSystem_DataChanged);
            }

            this.Update(true);
        }

        void _coordinateSystem_DataChanged(object sender, EventArgs e)
        {
            this.Update(true);
        } 

        protected virtual eCoordinateSystemOrientation Orientation
        {
            get { return (eCoordinateSystemOrientation)cmbOrientation.SelectedIndex; }
            set
            {
                if (cmbOrientation.SelectedIndex != (int)value)
                {
                    cmbOrientation.SelectedIndex = (int)value;
                    OnOrientationChanged();
                }
            }
        }

        protected virtual void OnOrientationChanged()
        {
            
        }

        protected virtual float OriginX
        {
            get { return (float)nudX.Value; }
            set
            {
                if (nudX.Value != (decimal)value)
                {
                    nudX.Value = (decimal)value;
                    OnOriginXChanged();
                }
            }
        }

        protected virtual void OnOriginXChanged()
        {            
        }

        protected virtual float OriginY
        {
            get { return (float)nudY.Value; }
            set
            {
                if (nudY.Value != (decimal)value)
                {
                    nudY.Value = (decimal)value;
                    OnOriginYChanged();
                }
            }
        }

        protected virtual void OnOriginYChanged()
        {
        }

        public virtual PointF OriginLocation
        {
            get { return new PointF(OriginX, OriginY); }
            set
            {
                if (value.X != OriginX || value.Y != OriginY)
                {
                    nudX.Value = (decimal)value.X;
                    nudY.Value = (decimal)value.Y;
                    OnOriginLocationChanged();
                }
            }
        }

        protected virtual void OnOriginLocationChanged()
        {
            
        }

        protected virtual float RotationAngle
        {
            get { return (float)nudRotationAngle.Value; }
            set
            {
                if (nudRotationAngle.Value != (decimal)value)
                {
                    nudRotationAngle.Value = (decimal)value;
                    OnRotationAngleChanged();
                }
            }
        }

        protected virtual void OnRotationAngleChanged()
        {
            
        }
        #endregion Properties

        #region Events
        private void btnApply_Click(object sender, EventArgs e)
        {
            InfoChanged(null, EventArgs.Empty);
        }

        private void InfoChanged(object sender, EventArgs e)
        {
            this.Update(false);

            if (CoordinateSystemChanged != null)
                this.CoordinateSystemChanged(this, EventArgs.Empty);
        }

        private void RegisterEvents()
        {
            nudX.ValueChanged += new EventHandler(InfoChanged);
            nudY.ValueChanged += new EventHandler(InfoChanged);
            nudRotationAngle.ValueChanged += new EventHandler(InfoChanged);            
        }

        private void UnregisterEvents()
        {
            nudX.ValueChanged -= new EventHandler(InfoChanged);
            nudY.ValueChanged -= new EventHandler(InfoChanged);
            nudRotationAngle.ValueChanged -= new EventHandler(InfoChanged);
        }
        #endregion Events

        #region Methods
        public virtual void Update(bool toControl)
        {
            if (_coordinateSystem == null)
                return;

            if (toControl)
            {
                try
                {
                    UnregisterEvents();

                    this.SuspendLayout();

                    this.Orientation = _coordinateSystem.Orientation;
                    this.OriginLocation = _coordinateSystem.GetOriginPointF();
                    this.RotationAngle = _coordinateSystem.DrawingAngle;

                    this.ResumeLayout(false);

                    RegisterEvents();
                }
                catch
                {
                }
            }
            else
            {
                _coordinateSystem.Orientation = this.Orientation;
                _coordinateSystem.DrawingOriginX = this.OriginX;
                _coordinateSystem.DrawingOriginY = this.OriginY;
                _coordinateSystem.DrawingAngle = this.RotationAngle;

                _coordinateSystem.Update();
            }
        }
        #endregion Methods        
    }
}
