using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarketDataSeries;
using MarketDataSeries.PFPatterns;
using PFChartCtrl.IndicatorRenderItem;

namespace ForexPAF
{
    public class CsvLoader
    {
        public enum CSVFormat
        {
            NONE,
            Standard,
            MT4,
            MT5
        }

        public static ICsvLoader GetInstance(string filename)
        {
            if (CsvMT4.CanLoading(filename))
                return new CsvMT4();

            if (CsvMT5.CanLoading(filename))
                return new CsvMT5();

            if (CsvStandard.CanLoading(filename))
                return new CsvStandard();

            return null;
        }

        public static CSVFormat TypeCheck(string filename)
        {
            if (CsvMT4.CanLoading(filename))
                return CSVFormat.MT4;

            if (CsvMT5.CanLoading(filename))
                return CSVFormat.MT5;

            if (CsvStandard.CanLoading(filename))
                return CSVFormat.Standard;

            return CSVFormat.NONE;
        }

    }
}
