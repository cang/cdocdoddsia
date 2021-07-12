using System;
using System.Collections.Generic;
using System.Text;
using SIA.UI.Controls.Automation;
using SIA.SystemFrameworks.UI;
using System.Collections;
using SiGlaz.ObjectAnalysis.Common;
using SiGlaz.ObjectAnalysis.Engine;
using SiGlaz.Common.ImageAlignment;

namespace SIA.UI.Controls.Commands.Analysis
{
    public class CombineObjectsCommand : AutoCommand
    {
        private ArrayList _resultObjects = null;
        public CombineObjectsCommand(IProgressCallback callback) 
            : base(callback)
		{
		}

        public override object[] GetOutput()
        {
            return new object[] { _resultObjects };
        }

		protected override void ValidateArguments(params object[] args)
		{
            if (args == null)
                throw new ArgumentNullException("arguments");
            if (args.Length < 3)
                throw new ArgumentException("Not enough arguments", "arguments");
            if (args[0] is ArrayList == false)
                throw new ArgumentException("Argument type does not match. Arguments[0] must be ArrayList", "arguments");
            if (args[1] is List<MDCCParamItem> == false)
                throw new ArgumentException("Argument type does not match. Arguments[1] must be List<MDCCParamItem>", "arguments");
            if (args[2] is MetrologySystem == false)
                throw new ArgumentException("Argument type does not match. Arguments[1] must be MetrologySystem", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
            _resultObjects = CombineObjects(args);
		}

		public override void AutomationRun(params object[] args)
		{
            _resultObjects = CombineObjects(args);
		}

        protected ArrayList CombineObjects(params object[] args)
        {
            ArrayList objList = args[0] as ArrayList;
            List<MDCCParamItem> conditions = args[1] as List<MDCCParamItem>;
            MetrologySystem ms = args[2] as MetrologySystem;

            if (objList == null || objList.Count <= 1 ||
                conditions == null || conditions.Count <= 0 || ms == null)
                return objList;

            ArrayList detectedObjs = new ArrayList();
            detectedObjs.AddRange(objList);
            foreach (MDCCParamItem rule in conditions)
            {
                ArrayList superObjects =
                    CombinationProcessor.DoCombine(detectedObjs, rule, ms);

                if (superObjects.Count == detectedObjs.Count)
                    continue;

                detectedObjs = superObjects;
                if (detectedObjs.Count == 1)
                    break;
            }

            return detectedObjs;
        }
    }
}
