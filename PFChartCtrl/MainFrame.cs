using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Drawing;
using MarketDataSeries;

namespace PFChartCtrl
{
    class MainFrame : FrameView
    {
        //public ITimeSeries OpenTime;
        //public IDataSeries Open;
        //public IDataSeries High;
        //public IDataSeries Low;
        //public IDataSeries Close;
        public PFDataSeries _dataSource;

        //****************************************************************
        private List<double> MatrixX = new List<double>();
        private List<double> MatrixY = new List<double>();
        // 最小マトリックス間
        // これを割らない間隔で縦横の線を描くこと
        const int MatrixXMin = 50;
        const int MatrixYMin = 25;

        internal bool ShowGridLine = false;
        internal bool ShowLine = false;

        public bool BarStyle { get; set; } //= true;

        internal MainFrame(ChartScaling scaling, ChartColors colors, FrameController parent) : base(true, scaling, colors, parent) 
        {
            BarStyle = true;
        }

        internal override ValueMaxMinRange GetMaxMinRange(int start, int count)
        {
            ValueMaxMinRange value = new ValueMaxMinRange { valueMax = double.NaN, valueMin = double.NaN };

            if (_dataSource != null && start >= 0)
            {
                double t = double.MinValue;
                double b = double.MaxValue;

                for (int i = start; i < count; i++)
                {
                    if (_dataSource.Count == i)
                        break;

                    // hi, lo
                    if (_dataSource.Count > i)
                    {
                        t = Math.Max(_dataSource[i].High, t);
                        b = Math.Min(_dataSource[i].Low, b);
                    }
                }

                //value.valueMax = t;
                //value.valueMin = b;

                // Box pips高値安値余白を入れる
                var h = _dataSource.BoxPipSize * ViewTickSize;
                value.valueMax = t + h;
                value.valueMin = b - h;

                //****************************************************************
                // BOXサイズの比率調整
                if (null != _dataSource)
                {
                    //****************************************************************
                    // 目盛りのちょい線位置計算
                    int tpLine = _top + 1;
                    int bmLine = _top + _height - 1;

                    //****************************************************************
                    // 物理デバイスでの描画域の高さ (上下2 pixel分を引く)
                    int localPxHeight = bmLine - tpLine - (MarginTopBottom * 2);

                    //****************************************************************
                    // 論理描画データの上下域の高さ
                    double vtHeight = (value.valueMax - value.valueMin) / TickSize;

                    //****************************************************************
                    // 物理デバイス単位への変換率を計算 (重要な計算)
                    // live scale = px / virtual
                    // liveUnir => TickSizeの高さを格納
                    double localLiveUnit = localPxHeight / vtHeight;

                    // box縦の高さを計算
                    var y = _dataSource.BoxPipSize * 10 * localLiveUnit;

                    // 高さから横の幅を引いて余分を計算
                    var sa = y - scaling.Width;

                    //****************************************************************
                    // BOX縦サイズが正方形に近くなるように補正する計算ロジック
                    if (sa > 0d)
                    {
                        // boxのpipサイズにpip単位をかけてboxのpipsとしての高さを求める
                        var boxHeightEqWidth = _dataSource.BoxPipSize * ViewTickSize;

                        //縦にはいるbox数の計算
                        var boxCount = (int)Math.Round((value.valueMax - value.valueMin) / boxHeightEqWidth, 0, MidpointRounding.ToEven);

                        // 縦の個数をかけて余分合計高さを求める
                        var g = Math.Round(boxCount * sa * 100, 0, MidpointRounding.ToEven) / 100;

                        double d = 0;

                        double box_rate = (double)scaling.Width / (double)_dataSource.BoxPipSize * 2d;

                        switch (scaling.Width)
                        {
                            case 3: d = g / 10000 / box_rate; break;
                            case 5: d = g / 10000 / box_rate; break;
                            case 7: d = g / 10000 / box_rate; break;
                            case 9: d = g / 10000 / box_rate; break;
                            case 11: d = g / 10000 / box_rate; break;
                            case 13: d = g / 10000 / box_rate; break;
                            case 15: d = g / 10000 / box_rate; break;
                        }

                        // クロス円などの小数点が少ない場合の対応コード
                        int p1 = TickSize.ToString().IndexOf(".");
                        var p2 = TickSize.ToString().Length - p1;

                        if (p2 == 2)
                            d *= 10000;
                        else if (p2 == 3)
                            d *= 1000;
                        else if (p2 == 4)
                            d *= 100;


                        value.valueMax += d;
                        value.valueMin -= d;
                    }
                }

                value.Height = t - b;
            }

            return value;
        }

        // boxサイズ計算用
        internal double CalcBox()
        {
            if (null == _dataSource)
                return 0d;

            // box縦の高さを計算
            var y = _dataSource.BoxPipSize * 10 * liveUnit;

            // 高さから横の幅を引いて余分を計算
            return y - scaling.Width;
        }

        internal override void Paint(System.Drawing.Graphics graphics)
        {
            graphics.DrawRectangle(_colors.Foreground.pen, graphRect);
        }

        internal void DrawFrameBorder(System.Drawing.Graphics graphics)
        {
            // 枠描画
            // 描画する四角形の左上隅の x 座標。
            // 描画する四角形の左上隅の y 座標。
            // 描画する四角形の幅。
            // 描画する四角形の高さ。
            graphics.DrawRectangle(_colors.Foreground.pen, graphRect);
        }

        internal void RedrawLeftStr(System.Drawing.Graphics graphics, ValueMaxMinRange range, int fontHeight, int height)
        {
            //****************************************************************
            // 目盛りの上限下限計算 (10単位に補正)
            int tmpVal = (int)Math.Truncate(range.valueMax / ViewTickSize);
            tmpVal = tmpVal + (10 - (tmpVal % 10));
            double topValue = tmpVal * ViewTickSize;

            tmpVal = (int)Math.Truncate(range.valueMin / ViewTickSize);
            tmpVal = tmpVal - (tmpVal % 10);
            btmValue = tmpVal * ViewTickSize;

            //****************************************************************
            // 論理描画データの上下域の高さ
            double vtHeight = (topValue - btmValue) / TickSize;

            //****************************************************************
            // 物理デバイス単位への変換率を計算 (重要な計算)
            // live scale = px / virtual
            // liveUnir => TickSizeの高さを格納
            liveUnit = pxHeight / vtHeight;

            // 枠描画
            // 描画する四角形の左上隅の x 座標。
            // 描画する四角形の左上隅の y 座標。
            // 描画する四角形の幅。
            // 描画する四角形の高さ。
            graphics.DrawRectangle(_colors.Foreground.pen, graphRect);

            //****************************************************************
            // Y軸の方眼計算
            int spanY = 10;  // 10 pips単位で拡大計算
            for (int i = 1; i < 50; i++)
            {
                int d = ValueToViewYPosition(topValue - (spanY * ViewTickSize)) - topText;
                if (d > MatrixYMin)
                    break;
                spanY += 10;
            }

            var spanLineY = spanY;
            if (_dataSource != null)
                spanLineY = _dataSource.BoxPipSize;

            double r = spanY * ViewTickSize;  // 価格間隔単位作成
            double u = spanLineY * ViewTickSize;  // 価格間隔単位作成
            MatrixY.Clear();
            int lastY = 0;
            int counter = 0;


            graphics.FillRectangle(Colors.Background.brush, 0, 0, graphRect.Left, Height);

            // 価格文字
            if (Parent.ShowLeftNumber)
                if (topValue > 0 && btmValue > 0)
                    for (double i = topValue; i >= btmValue; i -= r)
                    {
                        counter++;
                        //if (counter <= 1)
                        //    continue;

                        if (i == btmValue)
                            break;

                        lastY = ValueToViewYPosition(i);
                        if (graphRect.Bottom - 4 <= lastY)
                            break;

                        graphics.DrawString(i.ToString(NumberFormat), Parent.ViewFont, Colors.Foreground.brush, 4, lastY - fntHalf);
                    }
        }

        internal override void DrawMeasure(System.Drawing.Graphics graphics, ValueMaxMinRange range, int fontHeight)
        {
            //****************************************************************
            // 目盛りの上限下限計算 (10単位に補正)
            int tmpVal = (int)Math.Truncate(range.valueMax / ViewTickSize);
            tmpVal = tmpVal + (10 - (tmpVal % 10));
            double topValue = tmpVal * ViewTickSize;

            tmpVal = (int)Math.Truncate(range.valueMin / ViewTickSize);
            tmpVal = tmpVal - (tmpVal % 10);
            btmValue = tmpVal * ViewTickSize;

            //****************************************************************
            // 論理描画データの上下域の高さ
            double vtHeight = (topValue - btmValue) / TickSize;

            //****************************************************************
            // 物理デバイス単位への変換率を計算 (重要な計算)
            // live scale = px / virtual
            // liveUnir => TickSizeの高さを格納
            liveUnit = pxHeight / vtHeight;

            // 枠描画
            // 描画する四角形の左上隅の x 座標。
            // 描画する四角形の左上隅の y 座標。
            // 描画する四角形の幅。
            // 描画する四角形の高さ。
            graphics.DrawRectangle(_colors.Foreground.pen, graphRect);

            //****************************************************************
            // 目盛りのちょい線出力
            //graphics.DrawLine(Colors.Foreground.pen, graphRect.Right, graphRect.Top, graphRect.Right + 3, graphRect.Top);
            //graphics.DrawLine(Colors.Foreground.pen, graphRect.Right, graphRect.Bottom, graphRect.Right + 3, graphRect.Bottom);

            //****************************************************************
            // 右の線
            //graphics.DrawLine(colors.Foreground.pen, x, tpLine, x, bmLine);

            //****************************************************************
            // 下の線
            //graphics.DrawLine(colors.Foreground.pen, 0, y, x + 4, y);

            //****************************************************************
            // 目盛りの価格表示 - 画像領域に文字列を書き込む (文字表示が被るから外す)
            //graphics.DrawString(topValue.ToString(NumberFormat), Parent.ViewFont, Colors.Foreground.brush, graphRect.Right + 4, topText);
            //graphics.DrawString(btmValue.ToString(NumberFormat), Parent.ViewFont, Colors.Foreground.brush, graphRect.Right + 4, btmText);

            //if (Parent.ShowLeftNumber)
            //{
            //    graphics.DrawString(topValue.ToString(NumberFormat), Parent.ViewFont, Colors.Foreground.brush, 4, topText);
            //    graphics.DrawString(btmValue.ToString(NumberFormat), Parent.ViewFont, Colors.Foreground.brush, 4, btmText);
            //}

            //****************************************************************
            // Y軸の方眼計算
            int spanY = 10;  // 10 pips単位で拡大計算
            for (int i = 1; i < 50; i++)
            {
                int d = ValueToViewYPosition(topValue - (spanY * ViewTickSize)) - topText;
                if (d > MatrixYMin)
                    break;
                spanY += 10;
            }

            var spanLineY = spanY;
            if (_dataSource != null)
                spanLineY = _dataSource.BoxPipSize;

            double r = spanY * ViewTickSize;  // 価格間隔単位作成
            double u = spanLineY * ViewTickSize;  // 価格間隔単位作成
            MatrixY.Clear();
            int lastY = 0;
            int counter = 0;

            // 方眼横線
            if (ShowGridLine)
                for (double i = topValue; i >= btmValue; i -= u)
                {
                    counter++;
                    if (counter <= 1)
                        continue;

                    if (i == btmValue)
                        break;

                    lastY = ValueToViewYPosition(i);
                    if (graphRect.Bottom - 4 <= lastY)
                        break;

                    graphics.DrawLine(Colors.Grid.pen, graphRect.Left, lastY, graphRect.Right, lastY);
                    //if (i % r == 0)
                    //    graphics.DrawString(i.ToString(NumberFormat), Parent.ViewFont, Colors.Foreground.brush, graphRect.Right + 4, lastY - fntHalf);

                }

            // 価格文字
            if (topValue > 0 && btmValue > 0)
                for (double i = topValue; i >= btmValue; i -= r)
                {
                    counter++;
                    if (counter <= 1)
                        continue;

                    if (i == btmValue)
                        break;

                    lastY = ValueToViewYPosition(i);
                    if (graphRect.Bottom - 4 <= lastY)
                        break;

                    graphics.DrawString(i.ToString(NumberFormat), Parent.ViewFont, Colors.Foreground.brush, graphRect.Right + 4, lastY - fntHalf);

                    if (Parent.ShowLeftNumber)
                        graphics.DrawString(i.ToString(NumberFormat), Parent.ViewFont, Colors.Foreground.brush, 4, lastY - fntHalf);

                    if (ShowLine && !ShowGridLine)
                    {
                        lastY = ValueToViewYPosition(i);

                        graphics.DrawLine(Colors.Grid.pen, graphRect.Left, lastY, graphRect.Right, lastY);
                        //if (i % r == 0)
                        //    graphics.DrawString(i.ToString(NumberFormat), Parent.ViewFont, Colors.Foreground.brush, graphRect.Right + 4, lastY - fntHalf);
                    }
                }

        }

        bool TimeStrDay;
        int NextWriteTimeStr;
        int lastYYYY = 0;
        internal override void Paint(System.Drawing.Graphics graphics, int idx, int startPoint, int graphIdx, ref List<int> TimeLines)
        {
            int ct = startPoint + scaling.Center;
            int endPoint = startPoint + scaling.Width;

            // 表示範囲チェック
            if (graphRect.Right - 1 < endPoint)
                return;

            if (_dataSource == null)
                return;

            if (_dataSource.Count > idx)
            {
                DateTime tmpDate = _dataSource[idx].First().OpenTime;
                switch (graphIdx)
                {
                    case 0:
                        {
                            // 時間文字の表示に使用する時間間隔の計算
                            TimeSpan ts = new TimeSpan();
                            if (idx + 1 < _dataSource.Count)
                                ts = _dataSource[idx].First().OpenTime - _dataSource[idx + 1].First().OpenTime;
                            else
                            if (idx - 1 >= 0)
                                ts = _dataSource[idx - 1].First().OpenTime - _dataSource[idx].First().OpenTime;

                            TimeStrDay = ts.Hours >= 24;

                            string st = tmpDate.Day.ToString() + " " + tmpDate.ToString("MMM", Parent.culture) + " " + tmpDate.ToString("yyyy", Parent.culture);
                            lastYYYY = tmpDate.Year;
                            SizeF numberStrSize = Parent.Parent.myDoubleBuffer.Graphics.MeasureString(st, Parent.ViewFont);
                            graphics.DrawString(st, Parent.ViewFont, Colors.Foreground.brush, graphRect.Left, graphRect.Bottom + 1);
                            NextWriteTimeStr = (int)numberStrSize.Width + graphRect.Left + 20;
                            TimeLines.Add(0);
                            break;
                        }
                    default:
                        {
                            if (ShowGridLine)
                                graphics.DrawLine(Colors.Grid.pen, startPoint, 
                                                                   graphRect.Top,
                                                                   startPoint, 
                                                                   graphRect.Bottom - 1);

                            if (NextWriteTimeStr < startPoint)
                            {
                                if (ShowLine && !ShowGridLine)
                                    graphics.DrawLine(Colors.Grid.pen, startPoint, graphRect.Top, startPoint, graphRect.Bottom + 2);

                                string st = tmpDate.Day.ToString() + " " + tmpDate.ToString("MMM", Parent.culture) + " " + tmpDate.ToString("HH:mm", Parent.culture);
                                SizeF numberStrSize = Parent.Parent.myDoubleBuffer.Graphics.MeasureString(st, Parent.ViewFont);
                                if (numberStrSize.Width + ct <= graphRect.Right)
                                {
                                    if (TimeStrDay || lastYYYY != tmpDate.Year)
                                    {
                                        lastYYYY = tmpDate.Year;
                                        st = tmpDate.Day.ToString() + " " + tmpDate.ToString("MMM", Parent.culture) + " " + tmpDate.ToString("yyyy", Parent.culture);
                                    }

                                    graphics.DrawString(st, Parent.ViewFont, Colors.Foreground.brush, ct, graphRect.Bottom + 1);
                                }
                                NextWriteTimeStr = (int)numberStrSize.Width + startPoint + 20;
                                TimeLines.Add(1);
                            }
                            else
                                TimeLines.Add(0);

                            break;
                        }
                }
            }

            if (_dataSource.Count <= idx)
                return;

            //int xtop = Math.Min(op, cl);
            //if (xtop < 0)
            //    return;
            int x1 = 0;
            int sigY = 0;
            if (BarStyle)
            {
                int top = ValueToViewYPosition(_dataSource[idx].High);
                int bottom = ValueToViewYPosition(_dataSource[idx].Low);

                int xheight = bottom - top;

                x1 = startPoint + 1;
                int x2 = scaling.Width - 1;

                // Bar Style Paint
                if (_dataSource[idx].Direction >= 0)
                {
                    // バーの描画

                    graphics.FillRectangle(Colors.BullFill.brush, x1, top, x2, xheight);
                    graphics.DrawRectangle(Colors.BullOutline.pen, x1, top, x2, xheight);

                    sigY = bottom; // - 10;
                }
                else
                {
                    // バーの描画

                    graphics.FillRectangle(Colors.BearFill.brush, x1, top, x2, xheight);
                    graphics.DrawRectangle(Colors.BearOutline.pen, x1, top, x2, xheight);

                    sigY = bottom; // + 10;
                }
            }
            else
            {
                // Block Style Paint
                if (_dataSource[idx].Direction >= 0)
                {
                    // BOXの描画
                    foreach (var b in _dataSource[idx])
                    {
                        int top = ValueToViewYPosition(b.High);
                        int bottom = ValueToViewYPosition(b.Low);

                        int xheight = bottom - top;

                        x1 = startPoint + 1;
                        int x2 = x1 + scaling.Width - 1;

                        graphics.DrawLine(Colors.BullOutline.pen, x1, top, x2, bottom);
                        graphics.DrawLine(Colors.BullOutline.pen, x2, top, x1, bottom);
                        if (b.Patterns != null && b.Patterns.Count == 0)
                        {
                        }
                        else
                        {
                            if (b.Patterns != null)
                            {
                                var nl = b.Patterns.Where(t => t.Visible).Select(t => t.PatternName);
                                var x = string.Join(",",nl);
                                graphics.DrawString(x, Parent.ViewFont, Colors.Foreground.brush, x1, top);
                            }
                        }

                    }
                }
                else
                {
                    // BOXの描画
                    foreach (var b in _dataSource[idx])
                    {
                        int top = ValueToViewYPosition(b.High);
                        int bottom = ValueToViewYPosition(b.Low);

                        int xheight = bottom - top;

                        x1 = startPoint + 1;
                        int x2 = scaling.Width - 1;

                        graphics.DrawEllipse(Colors.BearOutline.pen, x1, top, x2, xheight);
                        if (b.Patterns != null && b.Patterns.Count == 0)
                        {
                        }
                        else
                        {
                            if (b.Patterns != null)
                            {
                                var nl = b.Patterns.Where(t => t.Visible).Select(t => t.PatternName);
                                var x = string.Join(",", nl);
                                graphics.DrawString(x, Parent.ViewFont, Colors.Foreground.brush, x1, top);
                            }
                        }
                    }
                }
            }
        }
    }
}
