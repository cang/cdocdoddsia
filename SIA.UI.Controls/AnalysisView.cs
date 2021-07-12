using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using SIA.UI.Controls.Helpers.VisualAnalysis;

namespace SIA.UI.Controls
{
    /// <summary>
    /// The analysis view provides functions for handling analyzers which have 
    /// responsibility for displaying and managing user interaction.
    /// </summary>
    public class AnalysisView 
        : DocumentView
    {
        /// <summary>
        /// Gets the associated analysis workspace
        /// </summary>
        public AnalysisWorkspace AnalysisWorkspace
        {
            get { return this.DocumentWorkspace as AnalysisWorkspace; }
        }

        public AnalysisView()
            : base()
        {
        }

        public AnalysisView(DocumentWorkspace docWorkspace)
            : base(docWorkspace)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (this.AnalysisWorkspace != null)
            {
                List<IVisualAnalyzer> analyzers = this.AnalysisWorkspace.Analyzers;
                if (analyzers != null && analyzers.Count > 0)
                {
                    foreach (IVisualAnalyzer analyzer in analyzers)
                    {
                        if (!analyzer.Visible)
                            continue;

                        analyzer.Paint(e);
                    }
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs args)
        {
            base.OnMouseDown(args);

            if (this.AnalysisWorkspace != null)
            {
                List<IVisualAnalyzer> analyzers = this.AnalysisWorkspace.Analyzers;
                if (analyzers != null)
                {
                    foreach (IVisualAnalyzer activeAnalyzer in analyzers)
                    {
                        if (activeAnalyzer.Active)
                            activeAnalyzer.MouseDown(args);
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs args)
        {
            base.OnMouseMove(args);

            if (this.AnalysisWorkspace != null)
            {
                List<IVisualAnalyzer> analyzers = this.AnalysisWorkspace.Analyzers;
                if (analyzers != null)
                {
                    foreach (IVisualAnalyzer activeAnalyzer in analyzers)
                    {
                        if (activeAnalyzer.Active)
                            activeAnalyzer.MouseMove(args);
                    }
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs args)
        {
            base.OnMouseUp(args);

            if (this.AnalysisWorkspace != null)
            {
                List<IVisualAnalyzer> analyzers = this.AnalysisWorkspace.Analyzers;
                if (analyzers != null)
                {
                    foreach (IVisualAnalyzer activeAnalyzer in analyzers)
                    {
                        if (activeAnalyzer.Active)
                            activeAnalyzer.MouseUp(args);
                    }
                }
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (this.AnalysisWorkspace != null)
            {
                List<IVisualAnalyzer> analyzers = this.AnalysisWorkspace.Analyzers;
                if (analyzers != null)
                {
                    foreach (IVisualAnalyzer activeAnalyzer in analyzers)
                    {
                        if (activeAnalyzer.Active)
                            activeAnalyzer.MouseClick(e);
                    }
                }
            }
        }
    }
}
