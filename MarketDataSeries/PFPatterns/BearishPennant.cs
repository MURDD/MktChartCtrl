using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketDataSeries.PFPatterns
{
    public class BearishPennant : _PFPatternTypes
    {
        /// <summary>
        /// 売りペナント
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, -1:売りシグナル</returns>
        public override bool Verify(int index, PFDataSeries series)
        {
            if (index < 5) return false;
            if (series == null) return false;
            if (series.Count < 6) return false;

            PFDataSet barNow = series[index];
            PFDataSet barPrev1 = series[index - 1];
            PFDataSet barPrev2 = series[index - 2];
            PFDataSet barPrev3 = series[index - 3];
            PFDataSet barPrev4 = series[index - 4];
            PFDataSet barPrev5 = series[index - 5];

            bool[] cond = new bool[5];

            var blck = series.BoxPipSize * series.PipSize;

            cond[0] = barPrev5.High == barPrev3.High;
            cond[1] = barPrev3.High == barPrev1.High;
            cond[2] = barPrev5.Low < barPrev3.Low;
            cond[3] = barPrev3.Low < barPrev1.Low;
            cond[4] = barPrev1.Low > barNow.Low;

            if (cond.Where(t => t).Count() == cond.Length)
                return true;

            return false;
        }
    }
}
