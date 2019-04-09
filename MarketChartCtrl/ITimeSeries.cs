using System;
using System.Collections.Generic;
using System.Text;

namespace MarketChartCtrl
{
    public abstract class ITimeSeries
    {
        public abstract int Count { get; }
        public abstract DateTime LastValue { get; }
        public abstract DateTime Last();
		public abstract DateTime this[int index] { get; set; }
    }
}
