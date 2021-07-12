using System;
using System.Drawing;

using TYPE = System.UInt16;

namespace SIA.UI.Controls.Utilities
{
	public delegate void UpdateGUIEventHandler(object sender, UpdateGUIEventArgs e);

	/// <summary>
	/// Provides data for the <c>UpdateGUI</c> event.
	/// </summary>
	//Initializes a new instance of the <c>UpdateGUIEventArg</c> class.
	public class UpdateGUIEventArgs : System.EventArgs
	{
		private UInt32	 _updateType;
		private PointF	 _mouseLoc;
		private PointF	_waferLoc;
		private TYPE		_intensity;

		public uint UpdateType
		{
			get {return _updateType;}
			set {_updateType = value;}
		}

		public PointF PixelLocation
		{
			get {return _mouseLoc;}
			set {_mouseLoc = value;}
		}

		public PointF WaferLocation
		{
			get {return _waferLoc;}
			set {_waferLoc = value;}
		}

		public TYPE Intensity
		{
			get {return _intensity;}
			set {_intensity = value;}
		}
		
		public UpdateGUIEventArgs(UInt32 type)
		{
			_updateType = type;
		}

		public UpdateGUIEventArgs(UInt32 type, PointF mouseLoc, PointF waferLoc, TYPE val)
		{
			_updateType = type;
			_mouseLoc = mouseLoc;
			_waferLoc = waferLoc;
			_intensity = val;
		}
	}

}
