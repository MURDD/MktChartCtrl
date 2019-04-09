namespace MarketDataSeries
{
    public enum PendingOrderType
    {
        //
        // 概要:
        //     A limit order is an order to buy or sell at a specific price or better.
        Limit = 0,
        //
        // 概要:
        //     A stop order is an order to buy or sell once the price of the symbol reaches
        //     a specified price.
        Stop = 1
    }
}