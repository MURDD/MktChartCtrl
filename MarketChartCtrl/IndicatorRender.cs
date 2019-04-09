using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using MarketSeries;

namespace MarketChartCtrl
{
    public abstract class IndicatorRender
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public IDataSeries data { get; set; }

        public IndicatorRender(string seriesName, IDataSeries dataSeries) { Name = seriesName; data = dataSeries; }

        public virtual void Paint(Graphics graphics, int index, int graphIdx, int drawPositionX, ConvertValueToView func1, ChartColors Colors, ChartScaling scaling, Rectangle graphRect, ref List<int> TimeLines, bool main)
        {
            if (main)
                return;

            if (index <= 0)
                return;

            if (TimeLines.Count <= graphIdx)
                return;

            if (TimeLines[graphIdx] <= 0)
                return;

            int ct = drawPositionX + scaling.Center;
            graphics.DrawLine(Colors.Grid.pen, ct, graphRect.Top, ct, graphRect.Bottom + 2);

        }
    }
}
