using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketDataSeries
{
    public interface IPositions : IEnumerable<IPosition>
    {
        //
        // 概要:
        //     Find a position by index
        //
        // パラメーター:
        //   index:
        //     the index in the list
        IPosition this[int index] { get; }

        //
        // 概要:
        //     Total number of open positions
        int Count { get; }

        //
        // 概要:
        //     Find a position by its label
        //
        // パラメーター:
        //   label:
        //     Label to search by
        //
        // 戻り値:
        //     Position if it exists, null otherwise
        IPosition Find(string label);
        //
        // 概要:
        //     Find all positions with this label
        //
        // パラメーター:
        //   label:
        //     Label to search by
        //
        // 戻り値:
        //     Array of Positions
        IPosition[] FindAll(string label);
    }
}