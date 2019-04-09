using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarketDataSeries;

namespace ForexPAF
{
    public interface ICsvLoader
    {
        Tuple<bool, int, int> NFormat(string filename, int iMaxLength = 7);
        MarketDataSeries.MarketData Load(string filename);
    }
}
