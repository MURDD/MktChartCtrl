using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MarketDataSeries
{
    public class PFDataSet : List<PFDataBox>
    {
        //public List<PFDataBox> Bar = new List<PFDataBox>();

        //public int Direction  => Bar.Count > 0 ? Bar.Last().Direction : 0;
        //public int High  => Bar.Count > 0 ? Bar.Max(t => t.High) : 0;
        //public int Low  => Bar.Count > 0 ? Bar.Min(t => t.Low) : 0;


        /*
         0: Mid
         1: End
         2: High
         3: Low
         */
        public double Value(int priceType)
        {
            switch (priceType)
            {
                case 0: return Mid; break;
                case 1: return End; break;
                case 2: return High; break;
                case 3: return Low; break;
            }
            return 0;
        }

        // 上げ下げの方向
        public int Direction 
        {
            get
            {
                return this.Count > 0 ? this[0].Direction : 0;
            }
        }

        // 高値
        public double High 
        {
            get
            {
                if (Count > 0)
                {
                    if (this[0].Direction > 0)
                    {
                        return this[Count - 1].High;
                    }
                    else
                        if (this[0].Direction < 0)
                    {
                        return this[0].High;
                    }
                }
                return double.MinValue;
            }
        }

        // 安値
        public double Low
        {
            get
            {
                if (Count > 0)
                {
                    if (this[0].Direction > 0)
                    {
                        return this[0].Low;
                    }
                    else
                        if (this[0].Direction < 0)
                    {
                        return this[Count - 1].Low;
                    }
                }
                return double.MinValue;
            }
        }

        // 中値
        public double Mid
        {
            get
            {
                if (Count > 0)
                {
                    if (this[0].Direction > 0)
                    {
                        return (this[Count - 1].High + this[0].Low) / 2;
                    }
                    else
                        if (this[0].Direction < 0)
                        {
                            return (this[Count - 1].Low + this[0].High) / 2;
                        }
                }
                return double.MinValue;
            }
        }

        // 終値
        public double End
        {
            get
            {
                if (Count > 0)
                {
                    if (this[0].Direction > 0)
                    {
                        return this.Last().High;
                    }
                    else
                        if (this[0].Direction < 0)
                        {
                            return this.Last().Low;
                        }
                }
                return double.MinValue;
            }
        }

    }
}
