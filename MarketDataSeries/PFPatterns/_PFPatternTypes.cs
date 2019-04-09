using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketDataSeries.PFPatterns
{
    public abstract class _PFPatternTypes
    {
        public bool Visible { get; set; }

        public string PatternName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        public virtual bool Verify(int index, PFDataSeries series)
        {
            return false;
        }
    }
}
