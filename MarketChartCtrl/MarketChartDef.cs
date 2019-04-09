using System;
using System.Collections.Generic;
using System.Text;

namespace MarketChartCtrl
{
    public enum BarStyle
    {
        Candle,
        Bar
    }

    public delegate int ConvertValueToView(double pri);
}
