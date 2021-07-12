using System;
using System.Collections.Generic;
using System.Text;
using SIA.UI.Controls.Commands;
using SiGlaz.ObjectAnalysis.Common;

namespace SIA.UI.Controls.Automation.CommandSettings
{
    [Serializable]
    public class CombineObjectsCommandSettings : AutoCommandSettings
	{
		#region member fields
        public string FilePath = "";
        public MDCCParamLibrary Library = null;
        public bool[] SelectedConditionMap = null;

        public List<MDCCParamItem> SelectedConditions
        {
            get
            {
                if (Library == null || Library.Items.Count == 0 || SelectedConditionMap == null)
                    return new List<MDCCParamItem>();

                List<MDCCParamItem> selectedConditions = 
                    new List<MDCCParamItem>(Library.Items.Count);

                for (int i = 0; i < SelectedConditionMap.Length; i++)
                {
                    if (SelectedConditionMap[i])
                    {
                        selectedConditions.Add(Library.Items[i]);
                    }
                }

                return selectedConditions;
            }
        }
		#endregion member fields

		#region Constructors ans Deconstructors
		public CombineObjectsCommandSettings()
		{			
		}
        #endregion Constructors ans Deconstructors

		#region Properties
		#endregion Properties

		#region Methods
		
		public override void Copy(RasterCommandSettings settings)
		{
            if (settings is CombineObjectsCommandSettings == false)
                throw new ArgumentException("Settings is not an instance of CombineObjectsCommandSettings", "settings");

			base.Copy(settings);

            CombineObjectsCommandSettings cmdSettings = (CombineObjectsCommandSettings)settings;
            this.FilePath = cmdSettings.FilePath;
            if (cmdSettings.Library == null)
            {
                this.Library = null;
            }
            else
            {
                this.Library = cmdSettings.Library.Clone() as MDCCParamLibrary;
            }

            if (cmdSettings.SelectedConditionMap == null)
            {
                this.SelectedConditionMap = null;
            }
            else
            {
                this.SelectedConditionMap = new bool[cmdSettings.SelectedConditionMap.Length];
                Array.Copy(cmdSettings.SelectedConditionMap, this.SelectedConditionMap, cmdSettings.SelectedConditionMap.Length);
            }
		}

		public override void Validate()
		{			
		}		
	
		#endregion Methods

		#region Serializable && Deserialize
		#endregion Serializable && Deserialize		
	}
}
