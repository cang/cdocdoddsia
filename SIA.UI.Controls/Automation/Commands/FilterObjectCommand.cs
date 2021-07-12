using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using SIA.SystemFrameworks.UI;
using SiGlaz.Common.ImageAlignment;
using SiGlaz.ObjectAnalysis.Common;
using SiGlaz.ObjectAnalysis.Engine;

namespace SIA.UI.Controls.Automation.Commands
{
    public class FilterObjectCommand : AutoCommand
	{
        private ArrayList _resultObjects = null;

        public FilterObjectCommand(IProgressCallback callback)
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
            if (args[1] is MDCCParamFilter == false)
                throw new ArgumentException("Argument type does not match. Arguments[1] must be MDCCParamFilter", "arguments");
            if (args[2] is MetrologySystem == false)
                throw new ArgumentException("Argument type does not match. Arguments[1] must be MetrologySystem", "arguments");
		}

		protected override void OnRun(params object[] args)
		{
            _resultObjects = FilterObjects(args);
		}

		public override void AutomationRun(params object[] args)
		{
            _resultObjects = FilterObjects(args);
		}

        protected ArrayList FilterObjects(params object[] args)
        {
            ArrayList objList = args[0] as ArrayList;
            MDCCParamFilter paramFilter = args[1] as MDCCParamFilter;
            MetrologySystem ms = args[2] as MetrologySystem;

            if (objList == null || objList.Count <= 0 ||
                paramFilter == null || ms == null)
                return objList;

            ArrayList detectedObjs = new ArrayList();
            detectedObjs.AddRange(objList);

            ArrayList superObjects =
                        FilterProcessor.DoFilter(paramFilter.StrQuery, paramFilter.TypeQuery, detectedObjs, ms);
            detectedObjs = superObjects;

            return detectedObjs;
        }
	}
}
