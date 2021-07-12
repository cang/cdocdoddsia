namespace SiGlaz.UI.CustomControls.DockBar
{
    partial class ucDockbar
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._splitter = new ucSplitter();
            this._pTitle = new System.Windows.Forms.Panel();
            this.picAutoHide = new System.Windows.Forms.PictureBox();
            this.picClose = new System.Windows.Forms.PictureBox();
            this._lbCaption = new System.Windows.Forms.Label();
            this._pContainer = new ucPanel();
            this._pTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAutoHide)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).BeginInit();
            this.SuspendLayout();
            // 
            // _splitter
            // 
            this._splitter.BackColor = System.Drawing.SystemColors.Control;
            this._splitter.Dock = System.Windows.Forms.DockStyle.Right;
            this._splitter.Location = new System.Drawing.Point(334, 0);
            this._splitter.Name = "_splitter";
            this._splitter.Size = new System.Drawing.Size(4, 576);
            this._splitter.TabIndex = 0;
            this._splitter.TabStop = false;
            this._splitter.MouseMove += new System.Windows.Forms.MouseEventHandler(this._splitter_MouseMove);
            this._splitter.MouseDown += new System.Windows.Forms.MouseEventHandler(this._splitter_MouseDown);
            this._splitter.MouseUp += new System.Windows.Forms.MouseEventHandler(this._splitter_MouseUp);
            // 
            // _pTitle
            // 
            this._pTitle.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this._pTitle.Controls.Add(this.picAutoHide);
            this._pTitle.Controls.Add(this.picClose);
            this._pTitle.Controls.Add(this._lbCaption);
            this._pTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this._pTitle.Location = new System.Drawing.Point(0, 0);
            this._pTitle.Name = "_pTitle";
            this._pTitle.Size = new System.Drawing.Size(334, 17);
            this._pTitle.TabIndex = 1;
            // 
            // picAutoHide
            // 
            this.picAutoHide.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.picAutoHide.Dock = System.Windows.Forms.DockStyle.Right;
            this.picAutoHide.Image = global::SiGlaz.UI.CustomControls.DockBar.ImageResx.auto_hide1;
            this.picAutoHide.Location = new System.Drawing.Point(300, 0);
            this.picAutoHide.Name = "picAutoHide";
            this.picAutoHide.Size = new System.Drawing.Size(17, 17);
            this.picAutoHide.TabIndex = 3;
            this.picAutoHide.TabStop = false;
            this.picAutoHide.MouseLeave += new System.EventHandler(this.picAutoHide_MouseLeave);
            this.picAutoHide.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picAutoHide_MouseMove);
            this.picAutoHide.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picAutoHide_MouseClick);
            this.picAutoHide.MouseHover += new System.EventHandler(this.picAutoHide_MouseHover);
            // 
            // picClose
            // 
            this.picClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.picClose.Image = global::SiGlaz.UI.CustomControls.DockBar.ImageResx.close;
            this.picClose.Location = new System.Drawing.Point(317, 0);
            this.picClose.Name = "picClose";
            this.picClose.Size = new System.Drawing.Size(17, 17);
            this.picClose.TabIndex = 2;
            this.picClose.TabStop = false;
            this.picClose.MouseLeave += new System.EventHandler(this.picClose_MouseLeave);
            this.picClose.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picClose_MouseMove);
            this.picClose.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picClose_MouseClick);
            this.picClose.MouseHover += new System.EventHandler(this.picClose_MouseHover);
            // 
            // _lbCaption
            // 
            this._lbCaption.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lbCaption.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this._lbCaption.Location = new System.Drawing.Point(0, 0);
            this._lbCaption.Name = "_lbCaption";
            this._lbCaption.Size = new System.Drawing.Size(294, 17);
            this._lbCaption.TabIndex = 2;
            this._lbCaption.Text = "caption 1";
            this._lbCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _pContainer
            // 
            this._pContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pContainer.Location = new System.Drawing.Point(0, 17);
            this._pContainer.Name = "_pContainer";
            this._pContainer.Size = new System.Drawing.Size(334, 559);
            this._pContainer.TabIndex = 2;
            // 
            // ucDockbar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._pContainer);
            this.Controls.Add(this._pTitle);
            this.Controls.Add(this._splitter);
            this.Name = "ucDockbar";
            this.Size = new System.Drawing.Size(338, 576);
            this.Load += new System.EventHandler(this.ucDockbar_Load);
            this._pTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picAutoHide)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ucSplitter _splitter;
        private System.Windows.Forms.Panel _pTitle;
        private System.Windows.Forms.PictureBox picAutoHide;
        private System.Windows.Forms.PictureBox picClose;
        private System.Windows.Forms.Label _lbCaption;
        private ucPanel _pContainer;
    }
}
