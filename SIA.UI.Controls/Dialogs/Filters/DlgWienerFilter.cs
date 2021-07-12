using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SIA.UI.Controls.Dialogs
{
    public partial class DlgWienerFilter : Form
    {
        public DlgWienerFilter(int kernelSize, bool isAuto, double noiseLevel)
        {
            InitializeComponent();

            nudKernelSize.Value = (decimal)kernelSize;

            nudNoiseLevel.Value = (decimal)noiseLevel;
            rdAuto.Checked = isAuto;
            rdManual.Checked = !isAuto;
        }

        private void rdAuto_CheckedChanged(object sender, EventArgs e)
        {
            bool isManual = rdManual.Checked;
            if (isManual && !nudNoiseLevel.Enabled)
                nudNoiseLevel.Enabled = isManual;
        }

        private void rdManual_CheckedChanged(object sender, EventArgs e)
        {
            bool isManual = rdManual.Checked;
            if (isManual && !nudNoiseLevel.Enabled)
                nudNoiseLevel.Enabled = isManual;
        }

        private void nudNoiseLevel_ValueChanged(object sender, EventArgs e)
        {

        }

        private void nudKernelSize_ValueChanged(object sender, EventArgs e)
        {
            int val = (int)(sender as NumericUpDown).Value;
            int validVal = ((val >> 1) << 1) + 1;
            if (val != validVal)
                (sender as NumericUpDown).Value = (decimal)validVal;
        }

        public int KernelSize
        {
            get 
            {
                int kernelSize = (int)nudKernelSize.Value;
                kernelSize = ((kernelSize >> 1) << 1) + 1;

                return kernelSize; 
            }
        }

        public bool IsAuto
        {
            get { return rdAuto.Checked; }
        }

        public double NoiseLevel
        {
            get { return (double)nudNoiseLevel.Value; }
        }

        public static void SaveSettings(string filePath, 
            int kernelSize, bool isAuto, double noiseLevel)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    using (BinaryWriter writer = new BinaryWriter(fs))
                    {
                        writer.Write(kernelSize);
                        writer.Write(isAuto);
                        writer.Write(noiseLevel);
                    }
                }
            }
            catch
            {
            }
        }

        public static void LoadSettings(string filePath,
            out int kernelSize, out bool isAuto, out double noiseLevel)
        {
            kernelSize = 9;
            isAuto = true;
            noiseLevel = 0.01;

            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        kernelSize = reader.ReadInt32();
                        isAuto = reader.ReadBoolean();
                        noiseLevel = reader.ReadDouble();
                    }
                }
            }
            catch
            {
            }
        }
    }
}
