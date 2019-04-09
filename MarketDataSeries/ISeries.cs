using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MarketDataSeries
{
    public interface ISeries<T>
    {
        int Count { get; }
        T LastValue { get; }
        T Last();
		T this[int index] { get; set; }

        void Add(T open);
        void Clear();
        T[] ToArray();
    }
}
