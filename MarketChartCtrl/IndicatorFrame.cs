using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace MarketChartCtrl
{
    public class IndicatorFrame : FrameView
    {
        internal IndicatorFrame(ChartScaling scaling, ChartColors colors, FrameControler parent) : base(false, scaling, colors, parent)
        {
            TickSize = 1;
            ViewTickSize = 1.0;
            ZeroLine = false;
        }

        public bool ZeroLine { get; set; }

        //****************************************************************
        // draw Rect in nothing data
        internal override void Paint(System.Drawing.Graphics graphics)
        {
            graphics.DrawRectangle(_colors.Foreground.pen, graphRect);
        }

        //****************************************************************
        // Min Max
        internal override ValueMaxMinRange GetMaxMinRange(int start, int count)
        {
            ValueMaxMinRange value = new ValueMaxMinRange { valueMax = double.NaN, valueMin = double.NaN };

            int x = 0;
            foreach (var v in _item) x = Math.Max(x, v.data.Count);

            if (x == 0)
                return value;

            double t = double.MinValue;
            double b = double.MaxValue;

            foreach (var v in _item)
                for (int i = start; i < count; i++)
                {
                    if (x == i)
                        break;

                    if (!double.IsNaN(v.data[i]))
                    {
                        t = Math.Max(v.data[i], t);
                        b = Math.Min(v.data[i], b);
                    }
                }

            value.valueMax = t;
            value.valueMin = b;
            value.Height = t - b;
            return value;
        }

        //****************************************************************
        // draw measure
        internal override void DrawMeasure(System.Drawing.Graphics graphics, ValueMaxMinRange range, int fontHeight)
        {
            // 上下の描画レンジ計算
            double topValue = range.valueMax;
            btmValue = range.valueMin;

            // zero 位置の判定
            if (ZeroLine)
            {
                //double g = Math.Floor(btmValue / TickSize) * TickSize;
                //if (g <= ViewTickSize)
                    btmValue = 0d;
            }

            //****************************************************************
            // 論理描画データの上下域の高さ
            double vtHeight = (topValue - btmValue) / TickSize;

            // 物理デバイス単位への変換率を計算 (重要な計算)
            // live scale = px / virtual
            liveUnit = pxHeight / vtHeight;

            //****************************************************************
            // 枠描画
            graphics.DrawRectangle(_colors.Foreground.pen, graphRect);

            //if (topValue != 0 && btmValue != 0)
            {
                int strSize = Parent.IndicatorStrSize();

                int c = topValue.ToString().IndexOf('.');
                int d = btmValue.ToString().IndexOf('.');
                int e = Math.Max(c, d);
                if (e <= strSize && e > 0.0)
                    NumberFormat = new String('0', e) + "." + new string('0', strSize - e - 1);
                else
                    NumberFormat = new String('0', 1);

                string topValueStr = topValue.ToString(NumberFormat);
                string btmValueStr = btmValue.ToString(NumberFormat);

                graphics.DrawString(topValueStr, Parent.ViewFont, Colors.Foreground.brush, graphRect.Right + 4, topText);
                graphics.DrawString(btmValueStr, Parent.ViewFont, Colors.Foreground.brush, graphRect.Right + 4, btmText);

                if (Parent.ShowLeftNumber)
                {
                    graphics.DrawString(topValueStr, Parent.ViewFont, Colors.Foreground.brush, 4, topText);
                    graphics.DrawString(btmValueStr, Parent.ViewFont, Colors.Foreground.brush, 4, btmText);
                }
            }
        }

        //****************************************************************
        // paint
        internal override void Paint(System.Drawing.Graphics graphics, int startPoint, int idx, int graphIdx, ref List<int> TimeLines)
        {
        }
    }
}
