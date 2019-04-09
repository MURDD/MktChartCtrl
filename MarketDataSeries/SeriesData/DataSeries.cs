using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MarketDataSeries
{
    public class DataSeries : ISeries<double>, IEnumerable<double>
    {
        public List<double> _item = new List<double>();
        public int Count 
        {
            get 
            {
                return _item.Count; 
            }
        }

        public DataSeries()
        {

        }

        public double[] ToArray()
        {
            return _item.ToArray();
        }

        public double LastValue { get { return _item[_item.Count - 1]; } }
        public double Last()
        {
            return _item[_item.Count - 1];
        }

        public void SetValue(int index, double value)
        {
            if (index < 0)
                return;
            _item[index] = value;
        }

        public void Add(double value)
        {
            _item.Add(value);
        }

        public void Clear()
        {
            _item.Clear();
        }

        public IEnumerator<double> GetEnumerator()
        {
            return ((IEnumerable<double>)_item).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<double>)_item).GetEnumerator();
        }

        public double this[int index]
        {
            get
            {
                if (0 > index)
                    return double.NaN;

                while (_item.Count - 1 < index)
                {
                    _item.Add(double.NaN);
                }
                return _item[index];
            }
            set
            {
                if (index < 0)
                    return;

                while (_item.Count - 1 < index)
                {
                    _item.Add(double.NaN);
                }
                _item[index] = value;
            }
        }
    }

    //class TimeSeriesData
    //{
    //    private List<DateTime> _item = new List<DateTime>();
    //    public Dictionary<DateTime, int> dic = new Dictionary<DateTime, int>();

    //    public int Count { get { return _item.Count; } }
    //    public DateTime LastValue { get { return _item[_item.Count - 1]; } }
    //    public DateTime Last(int index)
    //    {
    //        return _item[_item.Count - 1];
    //    }
    //    public void SetValue(int index, DateTime value)
    //    {
    //        if (index < 0)
    //            return;
    //        _item[index] = value;
    //    }
    //    public void Add(DateTime value)
    //    {
    //        _item.Add(value);
    //    }

    //    public void Clear()
    //    {
    //        _item.Clear();
    //    }

    //    public DateTime this[int index]
    //    {
    //        get
    //        {
    //            return _item[index];
    //        }
    //        set
    //        {
    //            _item[index] = value;
    //        }
    //    }


    //    int timeframe;
    //    int baseTimeframe;
    //    int TimeDifferenceHour;
    //    public void initialize(int tf, int diff, int baseTime)
    //    {
    //        baseTimeframe = baseTime;
    //        timeframe = tf;
    //        TimeDifferenceHour = diff;
    //    }

    //}
}
