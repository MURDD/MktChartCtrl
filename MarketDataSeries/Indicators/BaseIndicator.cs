using System;
using System.Collections.Generic;
using System.Text;

namespace MarketDataSeries.Indicators
{
    public abstract class BaseIndicator
    {
        public abstract void fill();
        public abstract double calc();

    }
}
