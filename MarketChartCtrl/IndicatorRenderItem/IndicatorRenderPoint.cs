using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using MarketDataSeries;

namespace MarketChartCtrl.IndicatorRenderItem
{
    public class IndicatorRenderPoint : IndicatorRender
    {
        public IndicatorRenderPoint(string seriesName, ISeries<double> dataSeries) : base(seriesName, dataSeries) { }

        public Brush brush = new SolidBrush(Color.Green);

        private int _size = 2;
        public int Size { get { return _size; } set { _size = value; } }

        private Color _color;
        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                brush.Dispose();
                _color = value;
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
                if (data[index - 1] <= 0d || double.IsNaN(data[index - 1]))
                    return;

                // Y buffer
                int pointY = func1(data[index]);

                // 範囲外の対応なし
                if (pointY > graphRect.Bottom)
                    return;

                if (pointY < graphRect.Top)
                    return;

                int cminus = _size;
                if (cminus > scaling.Size)
                    cminus = scaling.Size;

                int leftP = (drawPositionX + scaling.Size) - cminus;
                int topP = func1(data[index]) - cminus;
                int cp = cminus * 2 + 1;

                // ひとつ前時点からの線引き
                graphics.FillEllipse(brush, leftP, topP, cp, cp);
            }
            catch (Exception e)
            {
                //System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }
    }
}
