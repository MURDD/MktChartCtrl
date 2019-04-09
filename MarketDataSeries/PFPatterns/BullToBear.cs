using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketDataSeries.PFPatterns
{
    public class BullToBear : _PFPatternTypes
    {
        public override bool Verify(int index, PFDataSeries series)
        {
            if (index < 4) return false;
            if (series == null) return false;
            if (series.Count < 5) return false;

            PFDataSet barNow = series[index];
            PFDataSet barPrev1 = series[index - 1];
            PFDataSet barPrev2 = series[index - 2];
            PFDataSet barPrev3 = series[index - 3];
            PFDataSet barPrev4 = series[index - 4];

            bool[] cond = new bool[6];

            cond[0] = barPrev4.Low < barPrev2.Low;
            cond[1] = barPrev3.Low < barPrev1.Low;
            cond[2] = barPrev4.Low < barPrev1.Low;
            cond[3] = barPrev3.High < barPrev1.High;
            cond[4] = barPrev2.High < barPrev1.High;
            cond[5] = barPrev1.Low > barNow.Low;

            if (cond.Where(t => t).Count() == cond.Length)
                return true;

            return false;
        }
    }
}

