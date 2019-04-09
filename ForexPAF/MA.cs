using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using PFChartCtrl;
using PFChartCtrl.IndicatorRenderItem;
using MarketDataSeries;

namespace ForexPAF
{
    class MA : IndicatorRenderLine
    {
        /*
         * 0: Mid
         * 1: End
         * 2: High
         * 3: Low
         */
        public int PriceType { get; set; }

        /*
         * 0: SMA
         * 1: EMA
         * 2: SMMA
         * 3: LWMA
         */
        public int Method { get; set; }
        private int _period = 7;
        public int Period 
        {
            get
            {
                return _period;
            }
            set
            {
                if (value < 2)
                    return;

                _period = value;
                ema_p = 2.0d / ((double)_period + 1.0d);
            }
        }
        private double ema_p = 0;

        private MarketDataSeries.PFDataSeries pfd;

        public static string GetName(int method, int priceType, int period)
        {
            string mt = "";
            switch (method)
            {
                case 0: mt = "SMA"; break;
                case 1: mt = "EMA"; break;
                case 2: mt = "SMMA"; break;
                case 3: mt = "LWMA"; break;
            }

            string pt = "";
            switch (priceType)
            {
                case 0: pt = "Mid"; break;
                case 1: pt = "End"; break;
                case 2: pt = "High"; break;
                case 3: pt = "Low"; break;
            }

            string maName = mt + "_" + pt + "_" + period.ToString();
            return maName;
        }

        public MA(MarketDataSeries.PFDataSeries _pfd, string name, Color color)
            : base(name, new MarketDataSeries.DataSeries())
        {
            PriceType = 0;
            Method = 0;
            Period = 7;
            Color = color;
            pfd = _pfd;
        }

        public void Fill(MarketDataSeries.PFDataSeries ds)
        {
            pfd = ds;
            Fill();
        }

        public void Fill()
        {
            if (pfd == null)
                return;

            data.Clear();

            for (int i = 0; i < pfd.Count; i++)
            {
                Calc(i);
            }
        }

        public void LastCalc()
        {
            Calc(pfd.Count - 1);
        }

        public void Calc(int index)
        {
            switch (Method)
            {
                case 0: data[index] = SMA(index, pfd, Period); break;
                case 1: data[index] = EMA(index, pfd, Period, data.Count > 1 ? data[index - 1] : 0); break;
                case 2: data[index] = SMMA(index, pfd, Period); break;
                case 3: data[index] = LWMA(index, pfd, Period); break;
            }
        }

        private double SMA(int index, MarketDataSeries.PFDataSeries source, int period)
        {
            if (index - period + 1 < 0)
                return 0;

            double sum = 0.0;

            int k = 1;

            for (int i = index - period + 1; i <= index; i++)
            {
                sum += source[i].Value(PriceType);
            }
            return sum / period;
        }

        private double EMA(int index, MarketDataSeries.PFDataSeries source, int period, double prev)
        {
            if (index - period + 1 < 0)
                return 0;

            if (prev > 0)
            {
                // EMA = (CLOSE (i) * P) + (EMA (i -1) * (1 - P))
                return source[index].Value(PriceType) * ema_p + prev * (1 - ema_p);
            }

            // SMA start
            double sum = 0.0;

            int k = 1;

            for (int i = index - period + 1; i <= index; i++)
            {
                sum += source[i].Value(PriceType);
            }
            return sum / period;
        }

        private double SMMA(int index, MarketDataSeries.PFDataSeries source, int period)
        {
            if (index < 2 || index <= period)
                return 0;

            if (source.Count + 2 < period)
                return 0;

            double sum1 = 0;
            for (int i = 0; i < period; i++)
            {
                sum1 += source[index - (i + 2)].Value(PriceType);
            }
            double smma1 = sum1 / period;
            double smma2 = (smma1 * (period - 1) + source[index].Value(PriceType)) / period;
            double prev = smma2 * period;
            return (prev - smma2 + source[index].Value(PriceType)) / period;
        }

        private double LWMA(int index, MarketDataSeries.PFDataSeries source, int period)
        {
            if (index - period + 1 < 0)
                return 0;

            double sum = 0.0;
            int weightSum = 0;
            int k = 1;

            for (int i = index - period + 1; i <= index; i++)
            {
                sum += source[i].Value(PriceType) * k;
                weightSum += k;
                k++;
            }

            return sum / weightSum;

        }

    }
}
