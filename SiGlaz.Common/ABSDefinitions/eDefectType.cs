using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SiGlaz.Common.ABSDefinitions
{
    public enum eDefectType : int
    {
        Unknown = -1,
        DarkObject = 0,
        BrightObject = 1,
        DarkObjectAcrossBoundary = 2,
        BrightObjectAcrossBoundary = 3,
        SuperObject = 4
    }

    public class DefectVisualizer
    {
        public static Color[] Colors = new Color[] 
            { Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.Pink, Color.DarkKhaki};
    }
}
