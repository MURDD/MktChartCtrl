using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketDataSeries.PFPatterns
{
    public class BullSignal : _PFPatternTypes
    {
        /// <summary>
        /// 強気信号型
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, 1:買いシグナル</returns>
        public override bool Verify(int index, PFDataSeries series)
        {
            if (index < 3) return false;
            if (series == null) return false;
            if (series.Count < 4) return false;

            PFDataSet barNow = series[index];
            PFDataSet barPrev1 = series[index - 1];
            PFDataSet barPrev2 = series[index - 2];
            PFDataSet barPrev3 = series[index - 3];

            bool[] cond = new bool[5];

            cond[0] = barPrev3.Low < barPrev1.Low;
            cond[1] = barPrev2.Low < barNow.Low;
            cond[2] = barPrev3.Low < barNow.Low;
            cond[3] = barPrev2.High < barNow.High;
            cond[4] = barPrev1.High < barNow.High;

            if (cond.Where(t => t).Count() == cond.Length)
                return true;

            return false;
        }
    }
}
