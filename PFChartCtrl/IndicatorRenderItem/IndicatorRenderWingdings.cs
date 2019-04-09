using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using MarketDataSeries;

namespace PFChartCtrl.IndicatorRenderItem
{
    public class IndicatorRenderWingdings : IndicatorRender
    {
        public IndicatorRenderWingdings(string seriesName, ISeries<double> dataSeries) : base(seriesName, dataSeries)
        {
            ch = (char)32;

            brush = new SolidBrush(Color.Black);

            font = new Font("Wingdings", 8);

            fontTateZurashi = (int)(font.Height / 2);
        }

        private char ch;
        private int fontTateZurashi;
        private Font font;
        private SolidBrush brush;

        public float Size
        {
            get
            {
                return font.Size;
            }
            set
            {
                font = new Font("Wingdings", value);
                fontTateZurashi = (int)(font.Height / 2);
            }
        }

        public Color Color
        {
            get
            {
                return brush.Color;
            }
            set
            {
                brush.Color = value;
            }
        }

        public override void Paint(Graphics graphics, int index, int graphIdx, int drawPositionX, ConvertValueToView func1, ChartColors Colors, ChartScaling scaling, Rectangle graphRect, ref List<int> TimeLines, bool main)
        {
            base.Paint(graphics, index, graphIdx, drawPositionX, func1, Colors, scaling, graphRect, ref TimeLines, main);

            try
            {
                if (ch == 230 || ch == 228)  // allow
                    graphics.DrawString(ch.ToString(), font, brush, new Point(drawPositionX - (2 * scaling.Width), func1(data[index]) - fontTateZurashi));
                else
                    graphics.DrawString(ch.ToString(), font, brush, new Point(drawPositionX - scaling.Width, func1(data[index]) - fontTateZurashi));
            }
            catch (Exception e)
            {
                //System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }
    }
}
