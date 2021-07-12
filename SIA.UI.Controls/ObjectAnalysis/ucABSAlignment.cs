using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SIA.UI.Controls.ObjectAnalysis
{
    public partial class ucABSAlignment : UserControl
    {
        private string _methodName = "Auto";
        public string MethodName
        {
            get 
            {
                _methodName = "Auto";

                if (rdAuto.Checked)
                    _methodName = "Auto";
                else if (rdAlignABSImage.Checked)
                    _methodName = "ABS";
                else if (rdAlignPoleTipImage.Checked)
                    _methodName = "Pole";

                return _methodName;
            }
            set
            {
                if (_methodName != value)
                {
                    _methodName = value;
                    switch (_methodName)
                    {
                        case "Auto":
                            rdAuto.Checked = true;
                            break;
                        case "ABS":
                            rdAlignABSImage.Checked = true;
                            break;
                        case "Pole":
                            rdAlignPoleTipImage.Checked = true;
                            break;
                        default:
                            throw new System.Exception("Not support!");
                    }
                }
            }
        }

        public ucABSAlignment()
        {
            InitializeComponent();

            chkStatus.Checked = true;
        }

        private void chkStatus_CheckedChanged(object sender, EventArgs e)
        {
            groupAlignments.Enabled = chkStatus.Checked;
        }

        private void MethodAlignmentChanged(object sender, EventArgs e)
        {

        }
    }
}
