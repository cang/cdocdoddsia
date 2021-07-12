using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SIA.Workbench.Dialogs
{
    public partial class DlgSetTheMonitorFrequency : Form
    {
        private int _monitorFrequency = 300;

        public DlgSetTheMonitorFrequency()
        {
            InitializeComponent();
        }

        public DlgSetTheMonitorFrequency(int monitorFrequency)
        {
            InitializeComponent();
        }

        public int MonitorFrequency
        {
            get 
            {
                _monitorFrequency = (int)nudMonitorFrequency.Value;
                return _monitorFrequency; 
            }
            set
            {
                _monitorFrequency = value;
                nudMonitorFrequency.Value = (decimal)_monitorFrequency;
            }
        }
    }
}