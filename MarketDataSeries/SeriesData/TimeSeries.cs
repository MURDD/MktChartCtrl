using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MarketDataSeries
{
    public class TimeSeries : ISeries<DateTime>, IEnumerable<DateTime>
    {
        private List<DateTime> _item = new List<DateTime>();
        public Dictionary<DateTime, int> dic = new Dictionary<DateTime, int>();

        public TimeSeries()
        {
        }

        public DateTime[] ToArray()
        {
            return _item.ToArray();
        }

        public void Clear()
        {
            _item.Clear();
        }

        public void Add(DateTime value)
        {
            _item.Add(value);
        }

        public int Count
        {
            get { return _item.Count; }
        }

        public DateTime Last()
        {
            return _item[_item.Count - 1];
        }

        public DateTime LastValue
        {
            get { return _item[Count - 1]; }
        }

        public DateTime this[int index]
        {
            get
            {
                if (index < 0)
                    return DateTime.MinValue;
                if (_item.Count <= index)
                    return DateTime.MinValue;
                return _item[index];
            }
            set
            {
                while (_item.Count - 1 < index)
                {
                    _item.Add(DateTime.MinValue);
                }
                _item[index] = value;
            }
        }
        public DateTime Last(int index)
        {
            int i = _item.Count - 1 - index;
            if (i >= 0)
                return _item[index];
            return DateTime.MinValue;
        }

        public IEnumerator<DateTime> GetEnumerator()
        {
            return ((IEnumerable<DateTime>)_item).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<DateTime>)_item).GetEnumerator();
        }
    }
}
