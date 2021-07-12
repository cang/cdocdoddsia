using System;

namespace SIA.UI.Components
{
    /// <summary>
    /// Encapsulates border settings 
    /// </summary>
    public class ViewerBorderPadding
    {
        private float _bottom;
        private float _left;
        private bool _proportional;
        private float _right;
        private float _top;

        public event System.EventHandler Changed;

        private bool Proportional
        {
            get
            {
                return _proportional;
            }
            set
            {
                _proportional = value;
                if (Changed != null)
                    Changed(this, null);
            }
        }

        public virtual float All
        {
            get
            {
                return Left;
            }
            set
            {
                if (value < 0.0F)
                    throw new System.ArgumentOutOfRangeException("All cannot be less than 0");
                float padding = value;
               
                _bottom = value;
                _right = value;
                _top = value;
                _left = value;

                if (Changed != null)
                    Changed(this, null);
            }
        }

        public virtual float Bottom
        {
            get
            {
                return _bottom;
            }
            set
            {
                if (value < 0.0F)
                    throw new System.ArgumentOutOfRangeException("Bottom cannot be less than 0");
                _bottom = value;
                if (Changed != null)
                    Changed(this, null);
            }
        }

        public virtual float Left
        {
            get
            {
                return _left;
            }
            set
            {
                if (value < 0.0F)
                    throw new System.ArgumentOutOfRangeException("Left cannot be less than 0");
                _left = value;
                if (Changed != null)
                    Changed(this, null);
            }
        }

        public virtual float Right
        {
            get
            {
                return _right;
            }
            set
            {
                if (value < 0.0F)
                    throw new System.ArgumentOutOfRangeException("Right cannot be less than 0");
                _right = value;
                if (Changed != null)
                    Changed(this, null);
            }
        }

        public virtual float Top
        {
            get
            {
                return _top;
            }
            set
            {
                if (value < 0.0F)
                    throw new System.ArgumentOutOfRangeException("Top cannot be less than 0");
                _top = value;
                if (Changed != null)
                    Changed(this, null);
            }
        }

        public ViewerBorderPadding()
        {
            _proportional = false;
            _left = 0.0F;
            _top = 0.0F;
            _right = 0.0F;
            _bottom = 0.0F;
            Changed = null;
        }

        public virtual SIA.UI.Components.ViewerBorderPadding Clone()
        {
            SIA.UI.Components.ViewerBorderPadding viewerBorderPadding = new SIA.UI.Components.ViewerBorderPadding();
            viewerBorderPadding.Left = Left;
            viewerBorderPadding.Top = Top;
            viewerBorderPadding.Right = Right;
            viewerBorderPadding.Bottom = Bottom;
            return viewerBorderPadding;
        }

    } // class ViewerBorderPadding

}

