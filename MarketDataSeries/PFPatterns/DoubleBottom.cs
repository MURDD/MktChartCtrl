using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketDataSeries.PFPatterns
{
    public class DoubleBottom : _PFPatternTypes
    {
        /// <summary>
        /// 価格変動中のダブルボトム型
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, -1:売りシグナル</returns>
        public override bool Verify(int index, PFDataSeries series)
        {
            if (index < 2) return false;
            if (series == null) return false;
            if (series.Count < 3) return false;

            PFDataSet barNow = series[index];
            PFDataSet barPrev1 = series[index - 1];
            PFDataSet barPrev2 = series[index - 2];

            bool[] cond = new bool[3];

            cond[0] = barPrev2.Low <= barPrev1.Low;
            cond[1] = barPrev2.High > barPrev1.High;
            cond[2] = barPrev2.Low > barNow.Low;

            if (cond.Where(t => t).Count() == cond.Length)
                return true;

            return false;
        }
    }
}
