using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketDataSeries.PFPatterns
{
    public class DoubleTop : _PFPatternTypes
    {
        /// <summary>
        /// 価格変動中のダブルトップ型
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, 1:買いシグナル</returns>
        public override bool Verify(int index, PFDataSeries series)
        {
            if (index < 2) return false;
            if (series == null) return false;
            if (series.Count < 3) return false;

            PFDataSet barNow = series[index];
            PFDataSet barPrev1 = series[index - 1];
            PFDataSet barPrev2 = series[index - 2];

            bool[] cond = new bool[3];

            cond[0] = barPrev2.High >= barPrev1.High;
            cond[1] = barPrev2.Low < barPrev1.Low;
            cond[2] = barPrev2.High < barNow.High;

            if (cond.Where(t => t).Count() == cond.Length)
                return true;

            return false;
        }

    }
}
