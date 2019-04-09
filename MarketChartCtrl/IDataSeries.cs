using System;
using System.Collections.Generic;
using System.Text;

namespace MarketChartCtrl
{
    public abstract class IDataSeries
    {
        public abstract int Count { get; }
        public abstract double LastValue { get; }
        public abstract double Last();
		public abstract double this[int index] { get; set; }
    }
}
