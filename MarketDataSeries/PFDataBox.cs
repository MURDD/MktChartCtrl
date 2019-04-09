using System;
using System.Collections.Generic;
using System.Text;

namespace MarketDataSeries
{
    public class PFDataBox
    {
        public DateTime OpenTime { get; set; } //= DateTime.MinValue;
        public double High { get; set; } //= 0;
        public double Low { get; set; } //= 0;

        // 上げ下げの方向 (-1, 0, 1)
        public int Direction { get; set; } //= 0;

        public List<PFPatterns._PFPatternTypes> Patterns { get; set; }

        public PFDataBox()
        {
            OpenTime = DateTime.MinValue;
            High = 0;
            Low = 0;
            Direction = 0;
            Patterns = null;
        }
    }
}
