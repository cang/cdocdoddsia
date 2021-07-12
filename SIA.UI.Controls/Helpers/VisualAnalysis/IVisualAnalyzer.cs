using System;
using System.Drawing;
using System.Windows.Forms;

namespace SIA.UI.Controls.Helpers.VisualAnalysis
{
	/// <summary>
	/// Summary description for IROIAnalyzer.
	/// </summary>
	public interface IVisualAnalyzer : IDisposable
	{
		ImageWorkspace Workspace {get;}
		
		bool Active {get; }
		bool Visible {get; set;}
        Cursor Cursor { get;}

        event EventHandler CursorChanged;

        void Activate();
        void Deactivate();

        void Render(Graphics graph, Rectangle rcClip);
		void Settings();

        void LoadPersistenceData();
        void SavePersistenceData();

        void MouseDown(MouseEventArgs e);
        void MouseMove(MouseEventArgs e);
        void MouseUp(MouseEventArgs e);

        void MouseClick(MouseEventArgs args);
        
        void KeyPress(KeyPressEventArgs e);
        void KeyDown(KeyEventArgs e);
        void KeyUp(KeyEventArgs e);
        
        void LostFocus(EventArgs e);

        void Paint(PaintEventArgs e);
    }
}
