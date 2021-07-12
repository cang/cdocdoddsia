using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SiGlaz.ObjectAnalysis.UI
{
    public partial class ucObjectFilter : UserControl
    {
        public ucObjectFilter()
        {
            InitializeComponent();
        }

        public string sValidateMessage = string.Empty;
        public bool IsValidate
        {
            get
            {
                string sDescription = "Filter Objects";
                if (unverControl.ConditionExpression == "")
                {
                    sValidateMessage = sDescription + " : Filter condition cannot be empty. Please set at least one condition.";
                    return false;
                }
                else
                {
                    sValidateMessage = string.Empty;
                    return true;
                }
            }
        }
    }
}
