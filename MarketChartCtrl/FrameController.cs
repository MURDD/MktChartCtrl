using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using MarketDataSeries;
using MarketChartCtrl.IndicatorRenderItem;

namespace MarketChartCtrl
{
    public class FrameControler : FrameSeparator
    {
        private MainFrame _mainChart;
        internal MarketChartCtrl Parent { get; set; }

        internal System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");

        internal MarketData DataSource
        {
            get
            {
                return Parent.DataSource;
            }
        }

        internal bool ShowGridLine = false;

        private bool _ShowLeftNumber;
        internal bool ShowLeftNumber { get { return _ShowLeftNumber; } set { _ShowLeftNumber = value; } }

        internal double TickSize { get { return _mainChart.TickSize; } set { _mainChart.TickSize = value; } }
        internal double ViewTickSize { get { return _mainChart.ViewTickSize; } set { _mainChart.ViewTickSize = value; } }

        protected IPositions Positions { get { return Parent.Positions; } }
        protected IPendingOrders PendingOrders { get { return Parent.PendingOrders; } }

        private BarStyle _BarStyle;
        internal BarStyle BarStyle { get { return _BarStyle; } set { _BarStyle = value; } }

        internal ChartColors Colors;
        internal ChartScaling Scaling;

        public bool IndicatorDrawOuter { get; set; } //= false;
        public int Count { get { return _item.Count; } }

        public FrameView this[int index]
        {
            get
            {
                if (index >= _item.Count)
                    return null;

                return _item[index];
            }
        }

        public override IEnumerator<FrameView> GetEnumerator()
        {
            foreach (var elm in _item) yield return elm;
        }

        internal FrameControler() : base()
        {
            _ShowLeftNumber = false;
            _BarStyle = BarStyle.Candle;

            Colors = new ChartColors();

            _viewFont = new Font(new FontFamily("Arial"), 8, FontStyle.Regular);

            Scaling = new ChartScaling(this);

            _mainChart = new MainFrame(Scaling, Colors, this);

            _mainChart.TickSize = 0.001;
            _mainChart.ViewTickSize = 0.01;
            _mainChart.NumberFormat = "-,000.0000";

            _mainChart.Colors = this.Colors;
            _mainChart.scaling = Scaling;

            _item.Add(_mainChart);
        }

        internal int GetElementWidth() { return Scaling.Width + Scaling.Margin; }

        internal MainFrame GetMainChart() { return _mainChart; }

        internal void UpdateViewMaxBars()
        {
            int vw = _mainChart.GraphRect.Width;
            ViewMaxBarCount = (int)Math.Truncate((decimal)vw / (Scaling.Margin + Scaling.Width));
        }

        internal int ViewMaxBarCount;
        internal override void OnResize()
        {
            foreach (var el in _item)
                el.OnResize(_viewFont.Height);

            UpdateViewMaxBars();

            //System.Diagnostics.Debug.WriteLine(ViewMaxBarCount.ToString() + "  " + (Scaling.Margin + Scaling.width).ToString());
        }

        public int MainFrameIndex
        {
            get
            {
                int i = -1;

                foreach (var el in _item)
                {
                    i++;
                    if (el.isMainFrame)
                        return i;
                }
                return -1;
            }
        }

        public override IndicatorFrame Add(int index = -1, IndicatorRender rdr = null)
        {
            if (index > _item.Count)
                return null;

            if (index < 0)
                index = _item.Count;

            // 全体の高さの 1/7 サイズで追加
            IndicatorFrame sc = new IndicatorFrame(Scaling, Colors, this);
            int szNewHeight = (int)System.Math.Truncate((double)_Height / 7.0);
            sc.PreSetSize(_Width, szNewHeight);
            sc.Colors = this.Colors;
            sc.scaling = Scaling;

            if (index < _item.Count)
                _item.Insert(index, (FrameView)sc);
            else
                _item.Add((FrameView)sc);

            _mainChart.PreSetSize(_mainChart.Width, _mainChart.Height - szNewHeight);

            // メインの高さが小さいとき
            if (_mainChart.Height < szNewHeight)
            {
                // 高さの均等化
                szNewHeight = (int)System.Math.Truncate((double)_Height / _item.Count);
                int amari = _Height - _item.Count * szNewHeight;

                int isum = 0;
                foreach (var el in _item)
                {
                    if (el.isMainFrame)
                    {
                        el.SetSize(_Width, szNewHeight + amari, isum);
                        isum += szNewHeight + amari;
                    }
                    else
                    {
                        el.SetSize(_Width, szNewHeight, isum);
                        isum += szNewHeight;
                    }
                }
            }
            else
            {
                FrameRegulation();
            }
            if (rdr != null)
                sc.Add(rdr);

            OnResize();
            Parent.Refresh();

            return sc;
        }

        public override void Clear()
        {
            for (int i = _item.Count - 1; i >= 0; i--)
            {
                if (!_item[i].isMainFrame)
                {
                    _item.Remove(_item[i]); // 要素の削除
                }
            }
            // 全体の高さ調整
            FrameRegulation();
            OnResize();
        }

        public override bool Remove(FrameView item)
        {
            if (item.isMainFrame)
                return false;

            var v =_item.Remove(item);

            // 全体の高さ調整
            FrameRegulation();
            OnResize();

            Parent.Refresh();

            return v;
        }

        public override void RemoveAt(int idx)
        {
            if (idx < 0)
                return;

            if (idx >= _item.Count)
                return;

            if (_item[idx].isMainFrame)
                return;

            int i = 0;
            int main = -1;
            foreach(var el in _item)      
            {
                if (el.isMainFrame)
                {
                    main = i;
                }
                ++i;
            }

            // 削除時のリサイズロジックをここに記述する (メインフレームをサイズアップする)
            int delHeight = _item[idx].Height;
            int mainHeight = _mainChart.Height;
            int mainTop = _mainChart.Top;
            if (main < idx)
                _mainChart.Height = delHeight + mainHeight;
            else
                if (main > idx)
                    _mainChart.SetTopHeight(mainTop - delHeight, delHeight + mainHeight);

            _item.RemoveAt(idx);

            // 全体の高さ調整
            FrameRegulation();
            OnResize();

            Parent.Refresh();

            return;
        }

        private Font _viewFont;
        internal Font ViewFont { get { return _viewFont;  } set { _viewFont = value; } }

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

        List<int> TimeLines = new List<int>();

        internal void Paint(Graphics graphics, int startIdx)
        {
            // 縦線の表示および時刻文字列の表示フラグを格納する領域のクリア
            TimeLines.Clear();

            // フレーム内の背景を一括設定
            graphics.Clear(Colors.Background.Color);

            if (DataSource != null)
                if (DataSource.OpenTime != null)
                {
                    foreach (var el in _item)
                        el.Paint(graphics);
                }

            int iMx = startIdx + ViewMaxBarCount;
            int vfh = _viewFont.Height;
            int trueWidth = Scaling.Width + Scaling.Margin;

            // Main
            {
                var el = _mainChart;
                ConvertValueToView cvt = el.ValueToViewYPosition;
                ValueMaxMinRange xn = el.GetMaxMinRange(startIdx, iMx);

                el.DrawMeasure(graphics, xn, vfh);

                if (Parent.DataSource != null)
                {
                    if (!double.IsNaN(Parent.DataSource.Ask))
                    {
                        int ask = cvt(Parent.DataSource.Ask);

                        if (ask > 0)
                            graphics.DrawLine(Colors.AskLine.pen, _mainChart.GraphRect.Left, ask, _mainChart.GraphRect.Right, ask);
                    }

                    if (!double.IsNaN(Parent.DataSource.Bid))
                    {
                        int bid = cvt(Parent.DataSource.Bid);
                        if (bid > 0)
                            graphics.DrawLine(Colors.BidLine.pen, _mainChart.GraphRect.Left, bid, _mainChart.GraphRect.Right, bid);
                    }
                }
                el.RedrawLeftStr(graphics, xn, vfh, _Height);

                // 各フレームでの描画を移譲実行
                int graphIdx = 0;
                int startPoint = el.GraphRect.Left + Scaling.Margin + 1;
                for (int i = startIdx; i < iMx; i++)
                {
                    if (el.isMainFrame)
                    {
                        el.Paint(graphics, i, startPoint, graphIdx, ref TimeLines);
                    }
                    foreach (var rdr in el)
                    {
                        rdr.Paint(this, graphics, i, graphIdx, startPoint, cvt, Colors, Scaling, el.GraphRect, ref TimeLines, true);
                    }

                    graphIdx++;
                    startPoint += Scaling.Margin + Scaling.Width;// + 1;

                    // 右端限界点での描画終了
                    if (startPoint + Scaling.Width >= _mainChart.GraphRect.Right - 1)
                        break;

                }

                // draw ASK,BID
                if (_mainChart.DataSource != null)
                {
                    int pricetagWidth = this.Width - _mainChart.GraphRect.Width - 4;
                    int tagLeft = _mainChart.GraphRect.Right + 1;
                    var pr = 3.0f;

                    if (!double.IsNaN(_mainChart.DataSource.Ask))
                    {
                        int ask = cvt(_mainChart.DataSource.Ask);
                        if (ask > 0)
                            graphics.DrawLine(Colors.AskLine.pen, _mainChart.GraphRect.Left, ask, _mainChart.GraphRect.Right, ask);

                        // price tag
                        int askt = ask - 8;
                        int askh = 16;
                        //graphics.FillRectangle(Colors.AskLine.brush, tagLeft, askt, pricetagWidth, askh);
                        DrawRoundRectangle(graphics, tagLeft, askt, pricetagWidth, askh, pr, Colors.AskLine.brush);

                        graphics.DrawString(_mainChart.DataSource.Ask.ToString(_mainChart.NumberFormat), ViewFont, Colors.AskLine.xorbrush, tagLeft + 2, askt + 1);
                    }

                    if (!double.IsNaN(_mainChart.DataSource.Bid))
                    {
                        int bid = cvt(_mainChart.DataSource.Bid);
                        if (bid > 0)
                            graphics.DrawLine(Colors.BidLine.pen, _mainChart.GraphRect.Left, bid, _mainChart.GraphRect.Right, bid);

                        // price tag
                        int bidt = bid - 8;
                        int bidh = 16;
                        //graphics.FillRectangle(Colors.BidLine.brush, tagLeft, bidt, pricetagWidth, bidh);
                        DrawRoundRectangle(graphics, tagLeft, bidt, pricetagWidth, bidh, pr, Colors.BidLine.brush);

                        graphics.DrawString(_mainChart.DataSource.Bid.ToString(_mainChart.NumberFormat), ViewFont, Colors.BidLine.xorbrush, tagLeft + 2, bidt + 1);
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
                //el.DrawFrameBorder(graphics);
                el.DrawMeasure(graphics, xn, vfh);
            }

            foreach (var el in _item)
            {
                if (el.isMainFrame)
                {
                    continue;
                }

                ConvertValueToView cvt = el.ValueToViewYPosition;
                ValueMaxMinRange xn = el.GetMaxMinRange(startIdx, iMx);

                // 各フレームでの描画を移譲実行
                int graphIdx = 0;
                int startPoint = _mainChart.GraphRect.Left + Scaling.Margin + 1;
                for (int i = startIdx; i < iMx; i++)
                {
                    if (xn.valueMax != 0.0 && xn.valueMin != 0.0)
                        if (TimeLines.Count > 0)
                            foreach (var rdr in el)
                            {
                                //rdr.Paint(this, graphics, i, graphIdx++, startPoint, cvt, Colors, Scaling, el.GraphRect, ref TimeLines, false);
                                rdr.Paint(this, graphics, i, graphIdx, startPoint, cvt, Colors, Scaling, el.GraphRect, ref TimeLines, false);
                            }
                    graphIdx++;
                    startPoint += Scaling.Margin + Scaling.Width;// + 1;

                    // 右端限界点での描画終了
                    if (startPoint + Scaling.Width >= _mainChart.GraphRect.Right - 1)
                        break;

                }
                el.DrawMeasure(graphics, xn, vfh);
            }
        }
    }
}
