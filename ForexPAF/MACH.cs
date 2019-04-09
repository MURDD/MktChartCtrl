using System;
using System.Collections.Generic;
using System.Text;
using PFChartCtrl;
using PFChartCtrl.IndicatorRenderItem;

namespace ForexPAF
{
    class MACH
    {
        public int period = 7;
        public double pin = 0.25;
        public double pout = 0.5;

        public MarketDataSeries.DataSeries HiMA = new MarketDataSeries.DataSeries();
        public MarketDataSeries.DataSeries LoMA = new MarketDataSeries.DataSeries();

        public MarketDataSeries.DataSeries HiExt = new MarketDataSeries.DataSeries();
        public MarketDataSeries.DataSeries LoExt = new MarketDataSeries.DataSeries();

        public MarketDataSeries.DataSeries HiInn = new MarketDataSeries.DataSeries();
        public MarketDataSeries.DataSeries LoInn = new MarketDataSeries.DataSeries();

        private MarketDataSeries.PFDataSeries pfd;

        public IndicatorRender irHM;
        public IndicatorRender irLM;

        public IndicatorRender irHO;
        public IndicatorRender irLO;

        public IndicatorRender irHI;
        public IndicatorRender irLI;

        public IndicatorRender[] Indicators;

        public MACH(MarketDataSeries.PFDataSeries _pfd)
        {
            pfd = _pfd;

            irHM = new IndicatorRenderLine("ma7", HiMA);
            ((IndicatorRenderLine)irHM).Color = System.Drawing.Color.Red;

            irHO = new IndicatorRenderLine("ma 7", HiExt);
            ((IndicatorRenderLine)irHO).LineStyle = PFChartCtrl.IndicatorRenderItem.LineStyle.DotsRare;
            ((IndicatorRenderLine)irHO).Color = System.Drawing.Color.Red;

            irHI = new IndicatorRenderLine("ma 7", HiInn);
            ((IndicatorRenderLine)irHI).LineStyle = PFChartCtrl.IndicatorRenderItem.LineStyle.DotsVeryRare;
            ((IndicatorRenderLine)irHI).Color = System.Drawing.Color.Red;


            irLM = new IndicatorRenderLine("ma 7", LoMA);
            ((IndicatorRenderLine)irLM).Color = System.Drawing.Color.Blue;

            irLO = new IndicatorRenderLine("ma 7", LoExt);
            ((IndicatorRenderLine)irLO).LineStyle = PFChartCtrl.IndicatorRenderItem.LineStyle.DotsRare;
            ((IndicatorRenderLine)irLO).Color = System.Drawing.Color.Blue;

            irLI = new IndicatorRenderLine("ma 7", LoInn);
            ((IndicatorRenderLine)irLI).LineStyle = PFChartCtrl.IndicatorRenderItem.LineStyle.DotsVeryRare;
            ((IndicatorRenderLine)irLI).Color = System.Drawing.Color.Blue;

            Indicators = new IndicatorRender[] { irHM, irLM, irHO, irLO, irHI, irLI };
        }

        public void fill()
        {
            if (pfd == null)
                return;

            for (int i = 0; i < pfd.Count; i++)
            {
                calc(i);
            }
        }

        public void calc()
        {
            calc(pfd.Count - 1);
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
                sum1 += pfd[i].High;
                sum2 += pfd[i].Low;
            }
            double s1 = sum1 / period;
            double s2 = sum2 / period;

            // ATR
            double y = 0.0;
            for (int i = st; i <= index; i++)
            {
                double a = Math.Abs(pfd[i].High - pfd[i].Low);
                double b = Math.Abs(pfd[i].High - pfd[i - 1].Mid);
                double c = Math.Abs(pfd[i].Low - pfd[i - 1].Mid);
                double x = Math.Max(a, Math.Max(b, c));
                y += x;
            }
            double atr = y / period;

            HiMA[index] = s1;
            LoMA[index] = s2;

            double atr_o = atr * pout;
            HiExt[index] = s1 + atr_o;
            LoExt[index] = s2 - atr_o;

            double atr_i = atr * pin;
            HiInn[index] = s1 - atr_i;
            LoInn[index] = s2 + atr_i;
        }
    }
}
