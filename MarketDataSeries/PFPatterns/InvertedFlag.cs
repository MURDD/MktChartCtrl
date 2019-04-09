using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketDataSeries.PFPatterns
{
    class InvertedFlag : _PFPatternTypes
    {
        public override bool Verify(int index, PFDataSeries series)
        {
            if (index < 5) return false;
            if (series == null) return false;
            if (series.Count < 6) return false;

            var r = series.BoxPipSize * series.PipSize;

            PFDataSet barNow = series[index];
            PFDataSet barPrev1 = series[index - 1];
            PFDataSet barPrev2 = series[index - 2];
            PFDataSet barPrev3 = series[index - 3];
            PFDataSet barPrev4 = series[index - 4];
            PFDataSet barPrev5 = series[index - 5];

            bool[] cond = new bool[7];

            cond[0] = barPrev5.Low - (2 * r) == barPrev3.Low;
            cond[1] = barPrev3.Low - (2 * r) == barPrev1.Low;
            cond[2] = barPrev4.Low - (2 * r) == barPrev2.Low;
            cond[3] = barPrev5.High < barPrev3.Low;
            cond[4] = barPrev3.High < barPrev1.High;
            cond[5] = barPrev4.High < barPrev2.High;
            cond[6] = barPrev1.Low > barNow.Low;

            if (cond.Where(t => t).Count() == cond.Length)
                return true;

            return false;
        }
    }
}
