using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketDataSeries
{
    public interface IPendingOrders : IEnumerable<IPendingOrder>
    {
        //
        // 概要:
        //     Find a pending order by index
        //
        // パラメーター:
        //   index:
        //     The position of the order in the collection
        IPendingOrder this[int index] { get; }

        //
        // 概要:
        //     Total number of pending orders
        int Count { get; }
    }
}