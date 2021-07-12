using System;
using System.Collections.Generic;
using System.Text;
using SiGlaz.Common.ImageAlignment;

namespace SIA.Algorithms.Preprocessing.Alignment
{
    public class AlignerFactory
    {
        public static AlignerBase CreateInstance(AlignmentSettings settings)
        {
            AlignerBase aligner = null;

            try
            {
                if (settings is ABSAlignmentSettings)
                    aligner = new ABSAligner(settings);
                else if (settings is PoleTipAlignmentSettings)
                    aligner = new PoleTipAligner(settings);
                else if (settings is DepoAlignmentSettings)
                    aligner = new DepoAligner(settings);
                else
                    throw new ArgumentException(
                        string.Format("Not supported settings: {0}", settings.GetType()));
            }
            catch (System.Exception exp)
            {
                aligner = null;
                throw exp;
            }

            return aligner;
        }
    }
}
