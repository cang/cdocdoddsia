using System;
using System.Collections.Generic;
using System.Text;
using SIA.SystemLayer.ImageProcessing;
using SIA.UI.Controls.Automation;
using SIA.SystemFrameworks.UI;
using SIA.IPEngine;
using SIA.Algorithms.ReferenceFile;

namespace SIA.UI.Controls.Commands
{
    public class CreateGoldenImageCommand : RasterCommand
	{
        private GreyDataImage _goldenImage = null;
        public CreateGoldenImageCommand(IProgressCallback callback) 
            : base(callback)
		{
		}

        public override object[] GetOutput()
        {
            return new object[] { _goldenImage };
        }

		protected override void ValidateArguments(params object[] args)
		{
			if (args == null)
				throw new ArgumentNullException("arguments");
			if (args.Length < 2)
				throw new ArgumentException("Not enough arguments", "arguments");
			if (args[0] is string[] == false)
				throw new ArgumentException("Argument type does not match. Arguments[0] must be string[]", "arguments");
			if (args[1] is eGoldenImageMethod == false)
                throw new ArgumentException("Argument type does not match. Arguments[1] must be eGoldenImageMethod", "arguments");			
		}

		protected override void OnRun(params object[] args)
		{
            CreateGoldenImage(args);
		}

        private void CreateGoldenImage(params object[] args)
        {
            this.SetStatusText("Creating Golden Image...");

            string[] sampleFiles = (string[])args[0];
            eGoldenImageMethod method = (eGoldenImageMethod)args[1];
            GoldenImageCreator processor = new GoldenImageCreator();

#if DEBUG
            //method = eGoldenImageMethod.DiffToAverage;
            //method = eGoldenImageMethod.Min;
            //method = eGoldenImageMethod.Max;
            method = eGoldenImageMethod.Average;
            //method = eGoldenImageMethod.Median;
            
#else
            throw new System.Exception("testing...");
#endif

            _goldenImage = processor.CreateGoldenImage(sampleFiles, method);
            processor = null;
        }
	}
}
