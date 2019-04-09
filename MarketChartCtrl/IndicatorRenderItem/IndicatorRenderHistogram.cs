using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using MarketDataSeries;

namespace MarketChartCtrl.IndicatorRenderItem
{
    public class IndicatorRenderHistogram : IndicatorRender
    {
        public Pen pen = new Pen(System.Drawing.Color.Green, 1);
        public SolidBrush brush;

        public IndicatorRenderHistogram(string seriesName, ISeries<double> dataSeries)
            : base(seriesName, dataSeries)
        {
        }

        public IndicatorRenderHistogram(string seriesName, ISeries<double> dataSeries, Color color)
            : base(seriesName, dataSeries)
        {
            this.Color = color;
        }

        public Color Color 
        {
            get 
            {
                return brush.Color; 
            } 
            set 
            {
                pen.Color = value;
                brush = new SolidBrush(value);
            } 
        }

        public override void Paint(FrameControler controler, Graphics graphics, int index, int graphIdx, int drawPositionX, ConvertValueToView func1, ChartColors Colors, ChartScaling scaling, Rectangle graphRect, ref List<int> TimeLines, bool main)
        {

            base.Paint(controler, graphics, index, graphIdx, drawPositionX, func1, Colors, scaling, graphRect, ref TimeLines, main);

            // 開始点は線なし
            if (index <= 0)
                return;

            // データが終端を超えたら処理しない
            if (index >= data.Count)
                return;

            if (double.IsNaN(data[index]) || double.IsNaN(data[index - 1]))
                return;

            try
            {
                //System.Diagnostics.Debug.WriteLine("[" + series[index].ToString()+"]");
                // 無効データは描画しない
                if (data[index] <= 0d || double.IsNaN(data[index]))
                    return;

                // Y buffer
                int startY = 0;
                int hight = 0;

                if (data[index] > 0)
                {
                    startY = func1(data[index]);
                    int y2 = func1(0d);
                    hight = y2 - startY;
                }
                else
                {
                    startY = func1(0d);
                    int y2 = func1(data[index]);
                    hight = startY - y2;
                }

                if (startY + hight >= graphRect.Bottom)
                    hight = graphRect.Bottom - startY;

                // 範囲外の補正
                if (startY > graphRect.Bottom)
                    startY = graphRect.Bottom;

                if (startY < graphRect.Top)
                    startY = graphRect.Top;

                graphics.FillRectangle(Colors.BearFill.brush, drawPositionX + 1, startY, scaling.Width - 1, hight);
                graphics.DrawRectangle(Colors.BearOutline.pen, drawPositionX + 1, startY, scaling.Width - 1, hight);
            }
            catch (Exception e)
            {
                //System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }
    }
}
