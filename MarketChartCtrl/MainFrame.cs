using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using MarketDataSeries;

namespace MarketChartCtrl
{
    class MainFrame : FrameView
    {
        //****************************************************************
        private List<double> MatrixX = new List<double>();
        private List<double> MatrixY = new List<double>();
        // 最小マトリックス間
        // これを割らない間隔で縦横の線を描くこと
        const int MatrixXMin = 50;
        const int MatrixYMin = 25;


        internal MainFrame(ChartScaling scaling, ChartColors colors, FrameControler parent) : base(true, scaling, colors, parent) { }

        internal MarketData DataSource
        {
            get
            {
                return Parent.DataSource;
            }
        }

        internal override ValueMaxMinRange GetMaxMinRange(int start, int count)
        {
            ValueMaxMinRange value = new ValueMaxMinRange { valueMax = double.NaN, valueMin = double.NaN };

            if (DataSource != null && start >= 0)
                if (DataSource.OpenTime != null)
                {
                    var o = DataSource.OpenTime;
                    var h = DataSource.High;
                    var l = DataSource.Low;

                    double t = double.MinValue;
                    double b = double.MaxValue;

                    for (int i = start; i < count; i++)
                    {
                        if (o.Count == i)
                            break;

                        // hi, lo
                        t = Math.Max(h[i], t);
                        b = Math.Min(l[i], b);

                        // indicator
                        if (Parent.IndicatorDrawOuter)
                            foreach (var v in _item)
                            {
                                if (!double.IsNaN(v.data[i]) && v.data[i] != 0.0)
                                {
                                    t = Math.Max(v.data[i], t);
                                    b = Math.Min(v.data[i], b);
                                }
                            }
                    }

                    value.valueMax = t;
                    value.valueMin = b;
                    value.Height = t - b;
                }

            return value;
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

            // 物理デバイス単位への変換率を計算 (重要な計算)
            // live scale = px / virtual
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

            graphics.FillRectangle(Colors.Background.brush, 0, 0, graphRect.Left, Height);

            double u = spanY * ViewTickSize;  // 価格間隔単位作成
            MatrixY.Clear();
            int lastY = 0;
            int counter = 0;
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

                if (i > 0)
                {
                    graphics.DrawString(i.ToString(NumberFormat), Parent.ViewFont, Colors.Foreground.brush, graphRect.Right + 4, lastY - fntHalf);

                    if (Parent.ShowLeftNumber)
                        graphics.DrawString(i.ToString(NumberFormat), Parent.ViewFont, Colors.Foreground.brush, 4, lastY - fntHalf);
                }
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

            // 物理デバイス単位への変換率を計算 (重要な計算)
            // live scale = px / virtual
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

            double u = spanY * ViewTickSize;  // 価格間隔単位作成
            MatrixY.Clear();
            int lastY = 0;
            int counter = 0;
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

                // Grid横ライン
                if (Parent.ShowGridLine)
                    graphics.DrawLine(Colors.Grid.pen, graphRect.Left, lastY, graphRect.Right - 1, lastY);

                if (i > 0)
                {
                    graphics.DrawString(i.ToString(NumberFormat), Parent.ViewFont, Colors.Foreground.brush, graphRect.Right + 4, lastY - fntHalf);

                    if (Parent.ShowLeftNumber)
                        graphics.DrawString(i.ToString(NumberFormat), Parent.ViewFont, Colors.Foreground.brush, 4, lastY - fntHalf);
                }
            }

        }

        bool TimeStrDay;
        int NextWriteTimeStr;
        int lastYYYY = 0;
        internal override void Paint(System.Drawing.Graphics graphics, int idx, int startPoint, int graphIdx, ref List<int> TimeLines)
        {
            int ct = startPoint + scaling.Center;

            // 表示範囲チェック
            if (startPoint + scaling.Width >= graphRect.Right - 1)
                return;

            if (DataSource == null)
                return;

            if (DataSource.OpenTime == null)
                return;

            if (DataSource.OpenTime.Count > idx)
            {
                DateTime tmpDate = DataSource.OpenTime[idx];
                switch (graphIdx)
                {
                    case 0:
                        {
                            // 時間文字の表示に使用する時間間隔の計算
                            TimeSpan ts = new TimeSpan();
                            if (idx + 1 < DataSource.OpenTime.Count)
                                ts = DataSource.OpenTime[idx] - DataSource.OpenTime[idx + 1];
                            else
                            if (idx - 1 >= 0)
                                ts = DataSource.OpenTime[idx - 1] - DataSource.OpenTime[idx];

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

                            if (NextWriteTimeStr < startPoint)
                            {
                                // Grid縦ライン
                                if (Parent.ShowGridLine)
                                    graphics.DrawLine(Colors.Grid.pen, ct, graphRect.Top + 1, ct, graphRect.Bottom - 1);

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

            int op = ValueToViewYPosition(DataSource.Open[idx]);
            int cl = ValueToViewYPosition(DataSource.Close[idx]);
            int top = ValueToViewYPosition(DataSource.High[idx]);
            int bottom = ValueToViewYPosition(DataSource.Low[idx]);

            int xtop = Math.Min(op, cl);
            if (xtop < 0)
                return;

            int xheight = op == cl ? 1 : op > cl ? op - cl : cl - op;

            switch (Parent.BarStyle)
            { 
                case BarStyle.Candle:
                {
                    if (op >= cl)
                    {
                        // ひげの描画
                        graphics.DrawLine(Colors.BullOutline.pen, ct, top, ct, bottom);

                        // バーの描画

                        graphics.FillRectangle(Colors.BullFill.brush, startPoint + 1, xtop, scaling.Width - 1, xheight);
                        graphics.DrawRectangle(Colors.BullOutline.pen, startPoint + 1, xtop, scaling.Width - 1, xheight);
                    }
                    else
                    {

                        // ひげの描画
                        graphics.DrawLine(Colors.BearOutline.pen, ct, top, ct, bottom);

                        // バーの描画

                        graphics.FillRectangle(Colors.BearFill.brush, startPoint + 1, xtop, scaling.Width - 1, xheight);
                        graphics.DrawRectangle(Colors.BearOutline.pen, startPoint + 1, xtop, scaling.Width - 1, xheight);

                    }
                    break;
                }
                case BarStyle.Bar:
                {
                    if (op >= cl)
                    {
                        // ひげの描画
                        graphics.DrawLine(Colors.BullOutline.pen, ct, top, ct, bottom);

                        // バーの描画

                        graphics.DrawLine(Colors.BullOutline.pen, startPoint, op, ct, op);
                        graphics.DrawLine(Colors.BullOutline.pen, ct, cl, ct + scaling.Center, cl);

                    }
                    else
                    {

                        // ひげの描画
                        graphics.DrawLine(Colors.BearOutline.pen, ct, top, ct, bottom);

                        // バーの描画

                        graphics.DrawLine(Colors.BearOutline.pen, startPoint, op, ct, op);
                        graphics.DrawLine(Colors.BearOutline.pen, ct, cl, ct + scaling.Center, cl);

                    }
                    break;
                }
            }
        }
    }
}
