using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MarketChartCtrl
{
    public class ChartColors
    {
        internal ChartColors()
        {
            Background = new ChartBrush();
            Foreground = new ChartPenBrush();
            Grid = new ChartPen();
            PeriodSeparators = new ChartPen();
            BullOutline = new ChartPen();
            BearOutline = new ChartPen();
            BullFill = new ChartBrush();
            BearFill = new ChartBrush();
            Volume = new ChartBrush();
            WinningDeal = new ChartPen();
            LosingDeal = new ChartPen();
            AskLine = new ChartPenBrush();
            BidLine = new ChartPenBrush();

            BuyPosition = new ChartPen();
            SellPosition = new ChartPen();
            BuyPositionStoploss = new ChartPen();
            SellPositionStoploss = new ChartPen();
            BuyPositionTakeprofit = new ChartPen();
            SellPositionTakeprofit = new ChartPen();

            BuyOrder = new ChartPen();
            SellOrder = new ChartPen();
            BuyOrderStoploss = new ChartPen();
            SellOrderStoploss = new ChartPen();
            BuyOrderTakeprofit = new ChartPen();
            SellOrderTakeprofit = new ChartPen();

            Background.SetColor(Color.White);
            Foreground.SetColor(Color.Black);
            Grid.SetColor(Color.Gray);
            PeriodSeparators.SetColor(Color.Gray);

            BullOutline.SetColor(Color.Crimson);
            BullFill.SetColor(Color.Red);

            BearOutline.SetColor(Color.Gray);
            BearFill.SetColor(Color.DeepSkyBlue);

            Volume.SetColor(32, 178, 170);
            WinningDeal.SetColor(0, 0, 255);
            LosingDeal.SetColor(255, 0, 0);
            AskLine.SetColor(255, 0, 0);
            BidLine.SetColor(0, 0, 255);

            BuyPosition.SetColor(0, 0, 255);
            SellPosition.SetColor(255, 0, 0);
            BuyPositionStoploss.SetColor(Color.Blue);
            SellPositionStoploss.SetColor(Color.Red);
            BuyPositionTakeprofit.SetColor(Color.Blue);
            SellPositionTakeprofit.SetColor(Color.Red);

            BuyPositionStoploss.pen.DashStyle = DashStyle.Dot;
            SellPositionStoploss.pen.DashStyle = DashStyle.Dot;
            BuyPositionTakeprofit.pen.DashStyle = DashStyle.Dash;
            SellPositionTakeprofit.pen.DashStyle = DashStyle.Dash;

            BuyOrder.SetColor(Color.Blue);
            SellOrder.SetColor(Color.Red);
            BuyOrderStoploss.SetColor(Color.Blue);
            SellOrderStoploss.SetColor(Color.Red);
            BuyOrderTakeprofit.SetColor(Color.Blue);
            SellOrderTakeprofit.SetColor(Color.Red);

            BuyOrderStoploss.pen.DashStyle = DashStyle.Dot;
            SellOrderStoploss.pen.DashStyle = DashStyle.Dot;
            BuyOrderTakeprofit.pen.DashStyle = DashStyle.Dash;
            SellOrderTakeprofit.pen.DashStyle = DashStyle.Dash;
        }

        public ChartBrush Background;
        public ChartPenBrush Foreground;
        public ChartPen Grid;
        public ChartPen PeriodSeparators;

        public ChartPen BullOutline;
        public ChartPen BearOutline;

        public ChartBrush BullFill;
        public ChartBrush BearFill;

        public ChartBrush Volume;

        public ChartPen WinningDeal;
        public ChartPen LosingDeal;
        public ChartPenBrush AskLine;
        public ChartPenBrush BidLine;

        public ChartPen BuyPosition;
        public ChartPen SellPosition;
        public ChartPen BuyPositionStoploss;
        public ChartPen SellPositionStoploss;
        public ChartPen BuyPositionTakeprofit;
        public ChartPen SellPositionTakeprofit;

        public ChartPen BuyOrder;
        public ChartPen SellOrder;
        public ChartPen BuyOrderStoploss;
        public ChartPen SellOrderStoploss;
        public ChartPen BuyOrderTakeprofit;
        public ChartPen SellOrderTakeprofit;
    }
}
