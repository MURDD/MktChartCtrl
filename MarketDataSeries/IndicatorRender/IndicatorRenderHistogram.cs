using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using MarketChartCtrl;

namespace MarketSeries
{
    public class IndicatorRenderHistogram : IndicatorRender
    {
        public IndicatorRenderHistogram(string seriesName, IDataSeries dataSeries) : base(seriesName, dataSeries) { }

        public override void Paint(Graphics graphics, int index, int graphIdx, int drawPositionX, ConvertValueToView func1, ChartColors Colors, ChartScaling scaling, Rectangle graphRect, ref List<int> TimeLines, bool main)
        {
            base.Paint(graphics, index, graphIdx, drawPositionX, func1, Colors, scaling, graphRect, ref TimeLines, main);

        }
    }
}
