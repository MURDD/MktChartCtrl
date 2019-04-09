using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketDataSeries.PFPatterns
{
    public class TripleTop : _PFPatternTypes
    {
        /// <summary>
        /// トリプルトップ型
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, 1:買いシグナル</returns>
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

            bool[] cond = new bool[4];

            cond[0] = barPrev4.High == barPrev2.High;
            cond[1] = barPrev4.High > barPrev3.High;
            cond[2] = barPrev2.High > barPrev1.High;
            cond[3] = barPrev2.High < barNow.High;

            if (cond.Where(t => t).Count() == cond.Length)
                return true;

            return false;
        }
    }
}
