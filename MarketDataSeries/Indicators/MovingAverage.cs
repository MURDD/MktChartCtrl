using System;
using System.Collections.Generic;
using System.Text;

namespace MarketDataSeries.Indicators
{
    public enum MaType
    {
        Simple,
        Exponential,
        Weighted,
    }

    public class MovingAverage : BaseIndicator
    {
        MaType _maType;
        int _period;
        ISeries<double> _series;

        public ISeries<double> Result;

        //private Func<double, int> f;

        public MovingAverage(MaType maType, int Period, ISeries<double> series)
        {
            _maType = maType;
            _period = Period;
            _series = series;
            Result = new DataSeries();
        }

        public override void fill()
        {
            for (int i = 0; i < _series.Count; i++)
            {
                calc(i);
            }
        }

        public override double calc()
        {
            return calc(_series.Count - 1);
        }

        public double calc(int index)
        {
            double sum = 0;

            int st = index - _period;

            if (st < 0)
            {
                return double.NaN;
            }

            // MA
            for (int i = st; i < index; i++)
            {
                sum += _series[i];
            }

            Result[index] = sum / _period;
            return Result[index];
        }
    }
}
