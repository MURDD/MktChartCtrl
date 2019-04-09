using System;
using System.Collections.Generic;
using System.Text;

namespace MarketDataSeries
{
    public class MarketData
    {
        // 銘柄
        public string Symbol { get; set; } // = "";

        public double Ask { get; set; }
        public double Bid { get; set; }

        // 元データの時間サイズ
        public TimeFrame TimeFrame { get; set; }

        public ISeries<DateTime> OpenTime;
        public ISeries<double> Open;
        public ISeries<double> High;
        public ISeries<double> Low;
        public ISeries<double> Close;
        public ISeries<double> Volume;

        public MarketData(string symbol, ISeries<DateTime> time, ISeries<double> open, ISeries<double> high, ISeries<double> low, ISeries<double> closs, ISeries<double> vol)
        {
            Ask = double.NaN;
            Bid = double.NaN;

            Symbol = symbol;
            OpenTime = time;
            Open = open;
            High = high;
            Low = low;
            Close = closs;
            Volume = vol;
        }

        public void Add(DateTime tm, double open, double hi, double lo, double close, double vol)
        {
            OpenTime.Add(tm);
            Open.Add(open);
            High.Add(hi);
            Low.Add(lo);
            Close.Add(close);
            Volume.Add(vol);
        }

        public void clear()
        {
            OpenTime.Clear();
            Open.Clear();
            High.Clear();
            Low.Clear();
            Close.Clear();
            Volume.Clear();
        }
    }
}
