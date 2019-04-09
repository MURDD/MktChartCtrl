using System;
using System.Collections.Generic;
using System.Text;

namespace MarketDataSeries.Indicators
{
    public class AverageTrueRange : BaseIndicator
    {
        int _period;
        ISeries<double> _close;
        ISeries<double> _high;
        ISeries<double> _low;

        public DataSeries Result;

        public AverageTrueRange(int Period, ISeries<double> close, ISeries<double> high, ISeries<double> low)
        {
            _period = Period;
            _close = close;
            _high = high;
            _low = low;
            Result = new DataSeries();
        }

        public override void fill()
        {
            for (int i = 0; i < _close.Count; i++)
            {
                calc(i);
            }
        }

        public override double calc()
        {
            return calc(_close.Count - 1);
        }

        public double calc(int index)
        {
            int st = index - _period;

            if (st <= 0)
                return double.NaN;

            // ATR
            double y = 0.0;
            for (int i = st; i <= index; i++)
            {
                double a = Math.Abs(_high[i] - _low[i]);
                double b = Math.Abs(_high[i] - _close[i - 1]);
                double c = Math.Abs(_low[i] - _close[i - 1]);
                double x = Math.Max(a, Math.Max(b, c));
                y += x;
            }

            Result[index] = y / _period;
            return Result[index];
        }
    }
}
