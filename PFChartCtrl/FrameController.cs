using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using MarketDataSeries;

namespace PFChartCtrl
{
    public class FrameController
    {
        internal PFChartCtrl Parent { get; set; }

        internal System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");

        protected IPositions Positions { get { return Parent.Positions; } }
        protected IPendingOrders PendingOrders { get { return Parent.PendingOrders; } }

        protected int _Width;
        protected int _Height;
        internal int Width { get { return _Width; } set { SetSize(value, _Height); } }
        internal int Height { get { return _Height; } set { SetSize(_Width, value); } }

        //internal ITimeSeries OpenTime { get { return _mainChart.OpenTime; } set { _mainChart.OpenTime = value; } }
        //internal IDataSeries Open { get { return _mainChart.Open; } set { _mainChart.Open = value; } }
        //internal IDataSeries High { get { return _mainChart.High; } set { _mainChart.High = value; } }
        //internal IDataSeries Low { get { return _mainChart.Low; } set { _mainChart.Low = value; } }
        //internal IDataSeries Close { get { return _mainChart.Close; } set { _mainChart.Close = value; } }

        private bool _ShowLeftNumber;
        internal bool ShowLeftNumber { get { return _ShowLeftNumber; } set { _ShowLeftNumber = value; } }

        internal double TickSize { get { return _mainChart.TickSize; } set { _mainChart.TickSize = value; } }
        internal double ViewTickSize { get { return _mainChart.ViewTickSize; } set { _mainChart.ViewTickSize = value; } }

        internal ChartColors Colors;
        internal ChartScaling _scaling;
        internal ChartScaling Scaling
        {
            get
            {
                if (_mainChart != null && _mainChart.scaling != null)
                    return _mainChart.scaling;
                return _scaling;
            }
            set
            {
                if (_mainChart == null)
                    _scaling = value;
                else
                    _mainChart.scaling = value;
            }
        }

        public void SetDataSource(PFDataSeries data)
        {
            _mainChart._dataSource = data;
        }

        private MainFrame _mainChart;
        public FrameView Frame
        {
            get
            {
                return _mainChart;
            }
        }

        internal FrameController() : base()
        {
            _ShowLeftNumber = false;

            Colors = new ChartColors();

            _viewFont = new Font(new FontFamily("Arial"), 8, FontStyle.Regular);

            _scaling = new ChartScaling(this);

            _mainChart = new MainFrame(Scaling, Colors, this);

            _mainChart.TickSize = 0.001;
            _mainChart.ViewTickSize = 0.01;
            _mainChart.NumberFormat = "-,000.0000";

            _mainChart.Colors = this.Colors;
            _mainChart.scaling = Scaling;

        }

        internal int GetElementWidth() { return Scaling.Width + Scaling.Margin; }

        internal MainFrame GetMainChart() { return _mainChart; }

        /// <summary>
        /// ウィンドウ枠に表示できるバー数の計算
        /// </summary>
        internal void UpdateViewMaxBars()
        {
            int vw = _mainChart.GraphRect.Width;
            ViewMaxBarCount = (int)Math.Truncate((decimal)vw / (Scaling.Margin + Scaling.Width));
        }

        internal int ViewMaxBarCount;
        internal void OnResize()
        {
            _mainChart.OnResize(_viewFont.Height);

            UpdateViewMaxBars();

            //System.Diagnostics.Debug.WriteLine(ViewMaxBarCount.ToString() + "  " + (Scaling.Margin + Scaling.width).ToString());
        }

        internal void SetSize(int width, int height)
        {
            // MainChartのみの場合
            _Width = width;
            _Height = height;

            _mainChart.SetSize(width, height, 0);
        }


        private Font _viewFont;
        internal Font ViewFont { get { return _viewFont; } set { _viewFont = value; } }

        internal string Format { get { return _mainChart.NumberFormat; } set { _mainChart.NumberFormat = value; } }

        internal SizeF NumberStrSize { get; set; }
        internal float MeasureStrAreatWidth()
        {
            using (StringFormat sf = new StringFormat(StringFormat.GenericTypographic))
            {
                Parent.myDoubleBuffer.Graphics.DrawString(_mainChart.NumberFormat, _viewFont, Brushes.Black, 50, 100, sf);
                NumberStrSize = Parent.myDoubleBuffer.Graphics.MeasureString(_mainChart.NumberFormat, _viewFont, this.Width, sf);
            }
            return NumberStrSize.Width;
        }

        internal void DrawChart()
        {
            Parent.Refresh();
        }

        internal int IndicatorStrSize()
        {
            return _mainChart.NumberFormat.Length + 1;
        }

        List<int> TimeLines = new List<int>();

        private void DrawRoundRectangle(Graphics g, float x, float y, float w, float h, float r, Brush brush)
        {
            float a = (float)(4 * (1.41421356 - 1) / 3 * r);

            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddBezier(x, y + r, x, y + r - a, x + r - a, y, x + r, y); /* 左上 */
            path.AddBezier(x + w - r, y, x + w - r + a, y, x + w, y + r - a, x + w, y + r); /* 右上 */
            path.AddBezier(x + w, y + h - r, x + w, y + h - r + a, x + w - r + a, y + h, x + w - r, y + h); /* 右下 */
            path.AddBezier(x + r, y + h, x + r - a, y + h, x, y + h - r + a, x, y + h - r); /* 左下 */
            path.CloseFigure();

            g.FillPath(brush, path);
        }

        internal void Paint(Graphics graphics, int startIdx)
        {
            // フレーム内の背景を一括設定
            graphics.Clear(Colors.Background.Color);

            //if (OpenTime == null)
            //{
            //    _mainChart.Paint(graphics);
            //}

            int iMx = startIdx + ViewMaxBarCount;
            int vfh = _viewFont.Height;
            int trueWidth = Scaling.Width + Scaling.Margin;

            // Main
            {
                var el = _mainChart;
                TimeLines.Clear();
                ConvertValueToView cvt = el.ValueToViewYPosition;
                ValueMaxMinRange xn = el.GetMaxMinRange(startIdx, iMx);

                el.DrawMeasure(graphics, xn, vfh);

                // 各フレームでの描画を移譲実行
                int graphIdx = 0;
                int startPoint = el.GraphRect.Left + Scaling.Margin + 1;
                for (int i = startIdx; i < iMx; i++)
                {
                    _mainChart.Paint(graphics, i, startPoint, graphIdx++, ref TimeLines);

                    foreach (var rdr in el)
                        rdr.Paint(graphics, i, graphIdx, startPoint, cvt, Colors, Scaling, el.GraphRect, ref TimeLines, true);

                    startPoint += Scaling.Margin + Scaling.Width;// + 1;

                    // 右端限界点での描画終了
                    if (startPoint + Scaling.Width >= _mainChart.GraphRect.Right - 1)
                        break;

                }

                el.RedrawLeftStr(graphics, xn, vfh, _Height);

                // draw ASK,BID
                if (_mainChart._dataSource != null)
                {
                    if (_mainChart._dataSource.DataSource != null)
                    {
                        int pricetagWidth = this.Width - _mainChart.GraphRect.Width - 4;
                        int tagLeft = _mainChart.GraphRect.Right + 1;
                        var pr = 3.0f;

                        if (!double.IsNaN(_mainChart._dataSource.DataSource.Ask))
                        {
                            int ask = cvt(_mainChart._dataSource.DataSource.Ask);
                            if (ask > 0)
                                graphics.DrawLine(Colors.AskLine.pen, _mainChart.GraphRect.Left, ask, _mainChart.GraphRect.Right, ask);

                            // price tag
                            int askt = ask - 8;
                            int askh = 16;
                            //graphics.FillRectangle(Colors.AskLine.brush, tagLeft, askt, pricetagWidth, askh);
                            DrawRoundRectangle(graphics, tagLeft, askt, pricetagWidth, askh, pr, Colors.AskLine.brush);

                            graphics.DrawString(_mainChart._dataSource.DataSource.Ask.ToString(_mainChart.NumberFormat), ViewFont, Colors.AskLine.xorbrush, tagLeft + 2, askt + 1);
                        }

                        if (!double.IsNaN(_mainChart._dataSource.DataSource.Bid))
                        {
                            int bid = cvt(_mainChart._dataSource.DataSource.Bid);
                            if (bid > 0)
                                graphics.DrawLine(Colors.BidLine.pen, _mainChart.GraphRect.Left, bid, _mainChart.GraphRect.Right, bid);

                            // price tag
                            int bidt = bid - 8;
                            int bidh = 16;
                            //graphics.FillRectangle(Colors.BidLine.brush, tagLeft, bidt, pricetagWidth, bidh);
                            DrawRoundRectangle(graphics, tagLeft, bidt, pricetagWidth, bidh, pr, Colors.BidLine.brush);

                            graphics.DrawString(_mainChart._dataSource.DataSource.Bid.ToString(_mainChart.NumberFormat), ViewFont, Colors.BidLine.xorbrush, tagLeft + 2, bidt + 1);
                        }
                    }
                }

                // draw Positions
                if (Positions != null)
                    foreach (var p in Positions)
                    {
                        if (p.TradeType == TradeType.Buy)
                        {
                            if (p.StopLoss.HasValue)
                            {
                                int sl = cvt(p.StopLoss.Value);
                                
                                using (StringFormat sf = new StringFormat(StringFormat.GenericTypographic))
                                    graphics.DrawString("#" + p.Id.ToString() + " Stoploss:" + p.StopLoss.Value.ToString(), _viewFont, Brushes.Black, _mainChart.GraphRect.Left + 3, sl, sf);
                                
                                graphics.DrawLine(Colors.BuyPositionStoploss.pen, _mainChart.GraphRect.Left, sl, _mainChart.GraphRect.Right, sl);
                            }

                            if (p.TakeProfit.HasValue)
                            {
                                int tp = cvt(p.TakeProfit.Value);

                                using (StringFormat sf = new StringFormat(StringFormat.GenericTypographic))
                                    graphics.DrawString("#" + p.Id.ToString() + " TakeProfit:" + p.TakeProfit.Value.ToString(), _viewFont, Brushes.Black, _mainChart.GraphRect.Left + 3, tp, sf);

                                graphics.DrawLine(Colors.BuyPositionTakeprofit.pen, _mainChart.GraphRect.Left, tp, _mainChart.GraphRect.Right, tp);
                            }

                            int pos = cvt(p.EntryPrice);

                            using (StringFormat sf = new StringFormat(StringFormat.GenericTypographic))
                                graphics.DrawString("#" + p.Id.ToString() + " Buy:" + p.EntryPrice.ToString(), _viewFont, Brushes.Black, _mainChart.GraphRect.Left + 3, pos, sf);

                            graphics.DrawLine(Colors.BuyPosition.pen, _mainChart.GraphRect.Left, pos, _mainChart.GraphRect.Right, pos);
                        }

                        if (p.TradeType == TradeType.Sell)
                        {
                            if (p.StopLoss.HasValue)
                            {
                                int sl = cvt(p.StopLoss.Value);
                                
                                using (StringFormat sf = new StringFormat(StringFormat.GenericTypographic))
                                    graphics.DrawString("#" + p.Id.ToString() + " Stoploss:" + p.StopLoss.Value.ToString(), _viewFont, Brushes.Black, _mainChart.GraphRect.Left + 3, sl, sf);

                                graphics.DrawLine(Colors.SellPositionStoploss.pen, _mainChart.GraphRect.Left, sl, _mainChart.GraphRect.Right, sl);
                            }

                            if (p.TakeProfit.HasValue)
                            {
                                int tp = cvt(p.TakeProfit.Value);

                                using (StringFormat sf = new StringFormat(StringFormat.GenericTypographic))
                                    graphics.DrawString("#" + p.Id.ToString() + " TakeProfit:" + p.TakeProfit.Value.ToString(), _viewFont, Brushes.Black, _mainChart.GraphRect.Left + 3, tp, sf);

                                graphics.DrawLine(Colors.SellPositionTakeprofit.pen, _mainChart.GraphRect.Left, tp, _mainChart.GraphRect.Right, tp);
                            }

                            int pos = cvt(p.EntryPrice);

                            using (StringFormat sf = new StringFormat(StringFormat.GenericTypographic))
                                graphics.DrawString("#" + p.Id.ToString() + " Sell:" + p.EntryPrice.ToString(), _viewFont, Brushes.Black, _mainChart.GraphRect.Left + 3, pos, sf);

                            graphics.DrawLine(Colors.SellPosition.pen, _mainChart.GraphRect.Left, pos, _mainChart.GraphRect.Right, pos);
                        }
                    }

                // draw PendingOrders
                if (PendingOrders != null)
                    foreach (var p in PendingOrders)
                    {
                        if (p.TradeType == TradeType.Buy)
                        {
                            if (p.StopLoss.HasValue)
                            {
                                int sl = cvt(p.StopLoss.Value);
                                graphics.DrawLine(Colors.BuyOrderStoploss.pen, _mainChart.GraphRect.Left, sl, _mainChart.GraphRect.Right, sl);
                            }

                            if (p.TakeProfit.HasValue)
                            {
                                int tp = cvt(p.TakeProfit.Value);
                                graphics.DrawLine(Colors.BuyOrderTakeprofit.pen, _mainChart.GraphRect.Left, tp, _mainChart.GraphRect.Right, tp);
                            }

                            int pos = cvt(p.TargetPrice);
                            graphics.DrawLine(Colors.BuyOrder.pen, _mainChart.GraphRect.Left, pos, _mainChart.GraphRect.Right, pos);
                        }

                        if (p.TradeType == TradeType.Sell)
                        {
                            if (p.StopLoss.HasValue)
                            {
                                int sl = cvt(p.StopLoss.Value);
                                graphics.DrawLine(Colors.SellOrderStoploss.pen, _mainChart.GraphRect.Left, sl, _mainChart.GraphRect.Right, sl);
                            }

                            if (p.TakeProfit.HasValue)
                            {
                                int tp = cvt(p.TakeProfit.Value);
                                graphics.DrawLine(Colors.SellOrderTakeprofit.pen, _mainChart.GraphRect.Left, tp, _mainChart.GraphRect.Right, tp);
                            }

                            int pos = cvt(p.TargetPrice);
                            graphics.DrawLine(Colors.SellOrder.pen, _mainChart.GraphRect.Left, pos, _mainChart.GraphRect.Right, pos);
                        }
                    }

                el.DrawFrameBorder(graphics);               

            }

            // Paint Completed event
            if (Parent.PaintCompleted != null)
            {
                var args = new PaintCompletedEventArgs();
                //args.Rect = new Rectangle(0, 0, this.Width, this.Height);

                args.Rect = new Rectangle(_mainChart.GraphRect.Left + 1, _mainChart.GraphRect.Top + 1, _mainChart.GraphRect.Width - 1, _mainChart.GraphRect.Height - 1);
                args.graphics = graphics;
                Parent.PaintCompleted.Invoke(this, args);
            }

        }

        public delegate void PaintCompletedEventHandler(object sender, PaintCompletedEventArgs e);
        public class PaintCompletedEventArgs : System.EventArgs
        {
            public Rectangle Rect { get; set; }
            public Graphics graphics { get; set; }
        }
    }
}