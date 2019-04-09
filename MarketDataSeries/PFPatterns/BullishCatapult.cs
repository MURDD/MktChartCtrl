using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketDataSeries.PFPatterns
{
    public class BullishCatapult : _PFPatternTypes
    {
        /// <summary>
        /// 強気のカタパルト型
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, 1:買いシグナル</returns>
        public override bool Verify(int index, PFDataSeries series)
        {
            if (index < 6) return false;
            if (series == null) return false;
            if (series.Count < 7) return false;

            PFDataSet barNow = series[index];
            PFDataSet barPrev1 = series[index - 1];
            PFDataSet barPrev2 = series[index - 2];
            PFDataSet barPrev3 = series[index - 3];
            PFDataSet barPrev4 = series[index - 4];
            PFDataSet barPrev5 = series[index - 5];
            PFDataSet barPrev6 = series[index - 6];

            bool[] cond = new bool[7];

            cond[0] = barPrev6.High == barPrev4.High;
            cond[1] = barPrev6.High > barPrev5.High;
            cond[2] = barPrev4.High > barPrev3.High;
            cond[3] = barPrev4.High < barPrev2.High;
            cond[4] = barPrev2.High > barPrev1.High;
            cond[5] = barPrev4.High < barPrev1.High;
            cond[6] = barPrev2.High < barNow.High;

            if (cond.Where(t => t).Count() == cond.Length)
                return true;

            return false;
        }
    }
}
