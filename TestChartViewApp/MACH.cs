﻿using System;
using System.Collections.Generic;
using System.Text;
using MarketChartCtrl;
using MarketChartCtrl.IndicatorRenderItem;
using MarketDataSeries;

namespace TestChartViewApp
{
    class MACH
    {
        public int period = 7;

        public ISeries<double> HiMA = new DataSeries();
        public ISeries<double> LoMA = new DataSeries();

        private ISeries<double> high = new DataSeries();
        private ISeries<double> low = new DataSeries();
        private ISeries<double> close = new DataSeries();

        public IndicatorRender irHM;
        public IndicatorRender irLM;

        public IndicatorRender irHO;
        public IndicatorRender irLO;

        public IndicatorRender irHI;
        public IndicatorRender irLI;

        public IndicatorRender[] Indicators;

        public MACH(ISeries<double> _high, ISeries<double> _low, ISeries<double> _close)
        {
            high = _high;
            low = _low;
            close = _close;

            irHM = new IndicatorRenderLine("ma7", HiMA);
            ((IndicatorRenderLine)irHM).Color = System.Drawing.Color.Red;

            irLM = new IndicatorRenderLine("ma 7", LoMA);
            ((IndicatorRenderLine)irLM).Color = System.Drawing.Color.Blue;

            Indicators = new IndicatorRender[] { irHM, irLM, irHO, irLO, irHI, irLI };
        }

        public void fill()
        {
            for (int i = 0; i < high.Count; i++)
            {
                calc(i);
            }
        }

        public void calc()
        {
            calc(high.Count - 1);
        }

        public void calc(int index)
        {
            double sum1 = 0;
            double sum2 = 0;

            int st = index - period + 1;

            // atr is 1 plus
            if (st <= 0)
                return;

            // MA
            for (int i = st; i <= index; i++)
            {
                sum1 += high[i];
                sum2 += low[i];
            }
            double s1 = sum1 / period;
            double s2 = sum2 / period;

            // ATR
            //double y = 0.0;
            //for (int i = st; i <= index; i++)
            //{
            //    double a = Math.Abs(high[i] - low[i]);
            //    double b = Math.Abs(high[i] - close[i - 1]);
            //    double c = Math.Abs(low[i] - close[i - 1]);
            //    double x = Math.Max(a, Math.Max(b, c));
            //    y += x;
            //}
            //double atr = y / period;

            HiMA[index] = s1;
            LoMA[index] = s2;
        }
    }
}
