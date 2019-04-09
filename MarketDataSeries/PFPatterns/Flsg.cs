using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketDataSeries.PFPatterns
{
    class Flsg : _PFPatternTypes
    {
        public override bool Verify(int index, PFDataSeries series)
        {
            if (index < 5) return false;
            if (series == null) return false;
            if (series.Count < 6) return false;

            var r = series.BoxPipSize * series.PipSize;
            var u = r * 2;

            PFDataSet barNow = series[index];
            PFDataSet barPrev1 = series[index - 1];
            PFDataSet barPrev2 = series[index - 2];
            PFDataSet barPrev3 = series[index - 3];
            PFDataSet barPrev4 = series[index - 4];
            PFDataSet barPrev5 = series[index - 5];

            bool[] cond = new bool[7];

            cond[0] = (barPrev5.High > barPrev3.High && barPrev5.High - u >= barPrev3.High);
            cond[1] = (barPrev3.High > barPrev1.High && barPrev3.High - u >= barPrev1.High);
            cond[2] = (barPrev4.High > barPrev2.High && barPrev4.High - u >= barPrev2.High);
            cond[3] = (barPrev5.Low > barPrev3.Low && barPrev5.Low - u >= barPrev3.Low);
            cond[4] = (barPrev3.Low > barPrev1.Low && barPrev3.Low - u >= barPrev1.Low);
            cond[5] = (barPrev4.Low > barPrev2.Low && barPrev4.Low - u >= barPrev2.Low);
            cond[6] = barPrev1.High < barNow.High;

            if (cond.Where(t => t).Count() == cond.Length)
                return true;

            return false;
        }
    }
}
