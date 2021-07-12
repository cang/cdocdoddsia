using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SIA.SystemFrameworks.UI;
using SIA.SystemLayer;
using System.Collections;
using SIA.Common.Analysis;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace SIA.UI.Controls.Automation.Commands
{
    using duplicatedImplement = SIA.UI.Controls.Commands;

    public class OverlayDefectOnABSCommand : AutoCommand
    {
        private Image _image = null;

        public OverlayDefectOnABSCommand(IProgressCallback callback)
            : base(callback)
		{
		}

        protected override void UninitClass()
        {
            base.UninitClass();

            _image = null;
        }

        public override bool CanAbort
        {
            get { return false; }
        }

        public override object[] GetOutput()
        {
            return new object[] { _image };
        }

		protected override void ValidateArguments(params object[] args)
		{
			if (args == null)
				throw new ArgumentNullException("arguments");
			if (args.Length < 2)
				throw new ArgumentException("Not enough arguments", "arguments");
			if (args[0] is CommonImage == false)
				throw new ArgumentException("Argument type does not match. Arguments[0] must be CommonImage", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
            ArrayList defectList = args[1] as ArrayList;

			this.SetStatusRange(0, 100);
            this.SetStatusText("Overlaying Object on ABS...");

            this.OverlayDefectOnImage(image, defectList);
		}

		public override void AutomationRun(params object[] args)
		{
			CommonImage image = args[0] as CommonImage;
            ArrayList defectList = args[1] as ArrayList;

            this.SetStatusText("Overlaying Object on ABS...");
            this.OverlayDefectOnImage(image, defectList);
		}

        private void OverlayDefectOnImage(CommonImage image, ArrayList defectList)
        {
            _image =
                duplicatedImplement.OverlayDefectOnABSCommand.OverlayDefectOnImage(image, defectList);
        }
    }
}
