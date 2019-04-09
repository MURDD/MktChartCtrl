using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarketDataSeries.PFPatterns;

namespace MarketDataSeries.PrePatterns
{
    public class BroadeningTop : _PFPatternTypes
    {
        /// <summary>
        /// 上抜け末広がり型
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, 1:買いシグナル</returns>
        public override bool Verify(int index, PFDataSeries series)
        {
            return false;
        }
    }
}
