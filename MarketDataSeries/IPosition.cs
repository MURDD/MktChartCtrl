using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketDataSeries
{
    public interface IPosition
    {
        //
        // 概要:
        //     Symbol code of the position.
        string SymbolCode { get; }
        //
        // 概要:
        //     Label can be used to represent the order.
        string Label { get; }
        //
        // 概要:
        //     Represents the winning or loosing pips of the position.
        double Pips { get; }
        //
        // 概要:
        //     Entry time of trade associated with a position. The Timezone used is set in the
        //     Robot attribute
        DateTime EntryTime { get; }
        //
        // 概要:
        //     Commission Amount of the request to trade one way(Buy/Sell) associated with this
        //     position.
        double Commissions { get; }
        //
        // 概要:
        //     Swap is the overnight interest rate if any, accrued on the position.
        double Swap { get; }
        //
        // 概要:
        //     The Net profit of the position.
        double NetProfit { get; }
        //
        // 概要:
        //     Comment can be used as a note for the order
        string Comment { get; }
        //
        // 概要:
        //     The take profit level of the position.
        double? TakeProfit { get; }
        //
        // 概要:
        //     Entry price of the position.
        double EntryPrice { get; }
        //
        // 概要:
        //     Gross profit accrued by the order associated with a position.
        double GrossProfit { get; }

        [Obsolete("Use GrossProfit instead")]
        decimal Profit { get; }
        //
        // 概要:
        //     The position's unique identifier.
        int Id { get; }
        //
        // 概要:
        //     The amount traded by the position.
        long Volume { get; }
        //
        // 概要:
        //     Trade type (Buy/Sell) of the position.
        TradeType TradeType { get; }
        //
        // 概要:
        //     The stop loss level (price) of the position.
        double? StopLoss { get; }
        //
        // 概要:
        //     Quantity (lots) traded by the position
        double Quantity { get; }
    }
}
