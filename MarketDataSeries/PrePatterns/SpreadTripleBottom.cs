using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarketDataSeries.PFPatterns;

namespace MarketDataSeries.PrePatterns
{
    public class SpreadTripleBottom : _PFPatternTypes
    {
        /// <summary>
        /// 幅の広いトリプルボトム型
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, -1:売りシグナル</returns>
        public override bool Verify(int index, PFDataSeries series)
        {
            return false;
        }
    }
}
