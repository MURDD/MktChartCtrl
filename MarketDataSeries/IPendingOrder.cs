using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketDataSeries
{
    public interface IPendingOrder
    {
        //
        // 概要:
        //     Symbol code of the order
        string SymbolCode { get; }
        //
        // 概要:
        //     Specifies whether this order is to buy or sell.
        TradeType TradeType { get; }
        //
        // 概要:
        //     Volume of this order.
        long Volume { get; }
        //
        // 概要:
        //     Unique order Id.
        int Id { get; }
        //
        // 概要:
        //     Specifies whether this order is Stop or Limit.
        PendingOrderType OrderType { get; }
        //
        // 概要:
        //     The order target price.
        double TargetPrice { get; }
        //
        // 概要:
        //     The order Expiration time The Timezone used is set in the Robot attribute
        DateTime? ExpirationTime { get; }
        //
        // 概要:
        //     The order stop loss in price
        double? StopLoss { get; }
        //
        // 概要:
        //     The order stop loss in pips
        double? StopLossPips { get; }
        //
        // 概要:
        //     The order take profit in price
        double? TakeProfit { get; }
        //
        // 概要:
        //     The order take profit in pips
        double? TakeProfitPips { get; }
        //
        // 概要:
        //     User assigned identifier for the order.
        string Label { get; }
        //
        // 概要:
        //     User assigned Order Comment
        string Comment { get; }
        //
        // 概要:
        //     Quantity (lots) of this order
        double Quantity { get; }
    }
}
