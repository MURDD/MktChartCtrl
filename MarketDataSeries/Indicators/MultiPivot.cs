using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketDataSeries.Indicators
{
    public class MultiPivot : BaseIndicator
    {
        #region ENUM
        public enum ENUM_TIMEFRAMES
        {
            PERIOD_CURRENT = 0,
            PERIOD_M1 = 1,
            PERIOD_M2 = 2,
            PERIOD_M3 = 3,
            PERIOD_M4 = 4,
            PERIOD_M5 = 5,
            PERIOD_M6 = 6,
            PERIOD_M10 = 10,
            PERIOD_M12 = 12,
            PERIOD_M15 = 15,
            PERIOD_M20 = 20,
            PERIOD_M30 = 30,
            PERIOD_H1 = 60,
            PERIOD_H2 = 120,
            PERIOD_H3 = 180,
            PERIOD_H4 = 240,
            PERIOD_H6 = 360,
            PERIOD_H8 = 480,
            PERIOD_H12 = 720,
            PERIOD_D1 = 1440,
            PERIOD_W1 = 10080,
            PERIOD_MN1 = 43200
        }

        public enum PIVOTSPAN
        {
            IntraDay,
            Daily,
            Weekly,
            Monthly,
            Month2,
            Month3,
            Month4,
            Month6,
            Year1,
            Year2,
            Year5
        };

        public enum PIVOTTYPE
        {
            Floor,
            Fibonacci,
            Camarilla,
            Woodie,
            DeMark
        };

        public enum TIMESPAN
        {
            Minute2 = 2,
            Minute3 = 3,
            Minute4 = 4,
            Minute5 = 5,
            Minute6 = 6,
            Minute7 = 7,
            Minute8 = 8,
            Minute9 = 9,
            Minute10 = 10,
            Minute15 = 15,
            Minute20 = 20,
            Minute30 = 30,
            Minute45 = 45,
            Hour1 = 60,
            Hour1_5 = 90,
            Hour2 = 120,
            Hour3 = 180,
            Hour4 = 240,
            Hour6 = 360,
            Hour8 = 480,
            Hour12 = 720
        };

        // 自分が見たい時間に対してこれだけ差があるよっていう指定
        public enum HOURDIFF
        {
            Plus_23 = 23,
            Plus_22 = 22,
            Plus_21 = 21,
            Plus_20 = 20,
            Plus_19 = 19,
            Plus_18 = 18,
            Plus_17 = 17,
            Plus_16 = 16,
            Plus_15 = 15,
            Plus_14 = 14,
            Plus_13 = 13,
            Plus_12 = 12,
            Plus_11 = 11,
            Plus_10 = 10,
            Plus_9 = 9,
            Plus_8 = 8,
            Plus_7 = 7,
            Plus_6 = 6,
            Plus_5 = 5,
            Plus_4 = 4,
            Plus_3 = 3,
            Plus_2 = 2,
            Plus_1 = 1,
            Zero = 0,
            Minus_1 = -1,
            Minus_2 = -2,
            Minus_3 = -3,
            Minus_4 = -4,
            Minus_5 = -5,
            Minus_6 = -6,
            Minus_7 = -7,
            Minus_8 = -8,
            Minus_9 = -9,
            Minus_10 = -10,
            Minus_11 = -11,
            Minus_12 = -12,
            Minus_13 = -13,
            Minus_14 = -14,
            Minus_15 = -15,
            Minus_16 = -16,
            Minus_17 = -17,
            Minus_18 = -18,
            Minus_19 = -19,
            Minus_20 = -20,
            Minus_21 = -21,
            Minus_22 = -22,
            Minus_23 = -23,
        };
        public enum PivotMethod
        {
            HLC = 1,      // Avg of High, Low, Close
            HLCC = 2,     // Avg of High, Low, Close, Close
            HLOC = 3,     // Avg of High, Low, Open, Close
            HLOO = 4,     // Avg of High, Low, Open, Open
            HLO = 5,// Avg of High, Low, Open
        };
        #endregion

        //---- input parameters
        public PIVOTTYPE Type { get; set; } //= PIVOTTYPE.Fibonacci;
        public PivotMethod PivotMeth { get; set; } //= PivotMethod.HLC;
        public PIVOTSPAN Mode { get; set; } //= PIVOTSPAN.Weekly;
        public TIMESPAN IntraDayMinuteUnit { get; set; } //= TIMESPAN.Hour8;  /* IntraDayMinuteUnit : 1 - 720 */
        private HOURDIFF _DayOpenTimeHourDiff = HOURDIFF.Zero; /* DayOpenTimeHourDiff : -23 - 23 */
        public HOURDIFF DayOpenTimeHourDiff
        {
            get
            {
                return _DayOpenTimeHourDiff;
            }
            set
            {
                _DayOpenTimeHourDiff = value;
                if ((int)value > 0)
                    DayOpenTime = new TimeSpan((int)value, 0, 0);
                else if ((int)value < 0)
                    DayOpenTime = new TimeSpan(24 + (int)value, 0, 0);
                else
                    DayOpenTime = new TimeSpan(0, 0, 0);
            }
        }
        public bool ShowSR { get; set; } //= true;
        public bool ShowMidPoint { get; set; } //= true;
        public bool ShowFibonacci { get; set; } //= false;
        public bool ShowPreviousPrice { get; set; } //= false;

        public bool ShowATRPivot { get; set; } //= true;
        public double ATRLevel1 { get; set; } //= 0.38;
        public double ATRLevel2 { get; set; } //= 0.5;
        public double ATRLevel3 { get; set; } //= 0.62;
        public double ATRLevel4 { get; set; } //= 1.0;
        public double ATRLevel5 { get; set; } //= 1.38;
        public int ATRPeriod { get; set; } //= 14;

        //----- input data
        ISeries<DateTime> _time;
        ISeries<double>   _open;
        ISeries<double>   _high;
        ISeries<double>   _low;
        ISeries<double>   _close;

        //---- output data
        public DataSeries PPBuffer;
        public DataSeries P2Buffer;
        public DataSeries P3Buffer;
        public DataSeries S1Buffer;
        public DataSeries R1Buffer;
        public DataSeries S2Buffer;
        public DataSeries R2Buffer;
        public DataSeries S3Buffer;
        public DataSeries R3Buffer;
        public DataSeries S4Buffer;
        public DataSeries R4Buffer;
        public DataSeries S5Buffer;
        public DataSeries R5Buffer;

        public DataSeries OPBuffer;

        // Mid Point
        public DataSeries M0;
        public DataSeries M1;
        public DataSeries M2;
        public DataSeries M3;
        public DataSeries M4;
        public DataSeries M5;
        public DataSeries M6;
        public DataSeries M7;
        public DataSeries M8;
        public DataSeries M9;

        // Fibot Levels
        public DataSeries f214Buf;
        public DataSeries f236Buf;
        public DataSeries f382Buf;
        public DataSeries f050Buf;
        public DataSeries f618Buf;
        public DataSeries f764Buf;
        public DataSeries f786Buf;

        // prev OHLC
        public DataSeries prevO;
        public DataSeries prevH;
        public DataSeries prevL;
        public DataSeries prevC;

        // ATR
        public DataSeries atrR5;
        public DataSeries atrR4;
        public DataSeries atrR3;
        public DataSeries atrR2;
        public DataSeries atrR1;
        public DataSeries atrS1;
        public DataSeries atrS2;
        public DataSeries atrS3;
        public DataSeries atrS4;
        public DataSeries atrS5;

        //------------- buffer
        double f214 = 0;
        double f236 = 0;
        double f382 = 0;
        double f050 = 0;
        double f618 = 0;
        double f764 = 0;
        double f786 = 0;

        double pO = 0;
        double pH = 0;
        double pL = 0;
        double pC = 0;

        double atrR5e = 0;
        double atrR4e = 0;
        double atrR3e = 0;
        double atrR2e = 0;
        double atrR1e = 0;
        double atrS1e = 0;
        double atrS2e = 0;
        double atrS3e = 0;
        double atrS4e = 0;
        double atrS5e = 0;

        double P, P2, S1, R1, S2, R2, S3, R3, OP, R4, S4, R5, S5;
        double LastHigh, LastLow, x;

        int _Period;
        TimeSpan unitCloseTime;
        TimeSpan initTime;
        TimeSpan DayOpenTime = new TimeSpan(0,0,0);
        DateTime chkArDateTime = DateTime.MinValue;
        List<double> TR = new List<double>();
        bool init;
        int Adjustment = 0;


        public MultiPivot(int Period, ISeries<DateTime> time, ISeries<double> open, ISeries<double> high, ISeries<double> low, ISeries<double> close)
        {
            _time = time;
            _open = open;
            _high = high;
            _low = low;
            _close = close;
            _Period = Period;


            PPBuffer = new DataSeries();
            P2Buffer = new DataSeries();
            P3Buffer = new DataSeries();
            S1Buffer = new DataSeries();
            R1Buffer = new DataSeries();
            S2Buffer = new DataSeries();
            R2Buffer = new DataSeries();
            S3Buffer = new DataSeries();
            R3Buffer = new DataSeries();
            S4Buffer = new DataSeries();
            R4Buffer = new DataSeries();
            S5Buffer = new DataSeries();
            R5Buffer = new DataSeries();

            OPBuffer = new DataSeries();


            M0 = new DataSeries();
            M1 = new DataSeries();
            M2 = new DataSeries();
            M3 = new DataSeries();
            M4 = new DataSeries();
            M5 = new DataSeries();
            M6 = new DataSeries();
            M7 = new DataSeries();
            M8 = new DataSeries();
            M9 = new DataSeries();


            f214Buf = new DataSeries();
            f236Buf = new DataSeries();
            f382Buf = new DataSeries();
            f050Buf = new DataSeries();
            f618Buf = new DataSeries();
            f764Buf = new DataSeries();
            f786Buf = new DataSeries();


            prevO = new DataSeries();
            prevH = new DataSeries();
            prevL = new DataSeries();
            prevC = new DataSeries();


            atrR5 = new DataSeries();
            atrR4 = new DataSeries();
            atrR3 = new DataSeries();
            atrR2 = new DataSeries();
            atrR1 = new DataSeries();
            atrS1 = new DataSeries();
            atrS2 = new DataSeries();
            atrS3 = new DataSeries();
            atrS4 = new DataSeries();
            atrS5 = new DataSeries();
        }

        public override void fill()
        {
            for (int i = 0; i < _close.Count; i++)
            {
                calc(i);
            }
        }

        public override double calc()
        {
            return calc(_time.Count - 2);
        }

        public double calc(int index)
        {
            //int st = index - _period;

            //if (st <= 0)
            //    return double.NaN;

            if (index == 0)
                return 0;

            int i = index;

            DateTime d1 = _time[i];
            DateTime d2 = _time[i - 1];

            // 分レベルのおかしなデータの判定扱い時補正
            switch (_Period)
            {
                case (int)ENUM_TIMEFRAMES.PERIOD_M5:
                    {
                        d1 = _time[i].AddMinutes(-(_time[i].Minute % 5));
                        d2 = _time[i - 1].AddMinutes(-(_time[i].Minute % 5));
                        break;
                    }
                case (int)ENUM_TIMEFRAMES.PERIOD_M15:
                    {
                        d1 = _time[i].AddMinutes(-(_time[i].Minute % 15));
                        d2 = _time[i - 1].AddMinutes(-(_time[i].Minute % 15));
                        break;
                    }
                case (int)ENUM_TIMEFRAMES.PERIOD_M30:
                    {
                        d1 = _time[i].AddMinutes(-(_time[i].Minute % 30));
                        d2 = _time[i - 1].AddMinutes(-(_time[i].Minute % 30));
                        break;
                    }
                case (int)ENUM_TIMEFRAMES.PERIOD_H1:
                case (int)ENUM_TIMEFRAMES.PERIOD_H2:
                case (int)ENUM_TIMEFRAMES.PERIOD_H3:
                case (int)ENUM_TIMEFRAMES.PERIOD_H4:
                case (int)ENUM_TIMEFRAMES.PERIOD_H6:
                case (int)ENUM_TIMEFRAMES.PERIOD_H8:
                case (int)ENUM_TIMEFRAMES.PERIOD_H12:
                case (int)ENUM_TIMEFRAMES.PERIOD_D1:
                case (int)ENUM_TIMEFRAMES.PERIOD_W1:
                    {
                        d1 = _time[i].AddMinutes(-(_time[i].Minute));
                        d2 = _time[i - 1].AddMinutes(-(_time[i].Minute));
                        break;
                    }
            }

            // (データ異常対応) 翌営業日なのに時間が0:00でない場合の補正
            if (d1.Day != d2.Day && (d1.Hour != 0 || d1.Minute != 0))
            {
                d1 = d1.AddHours(-1 * d1.Hour).AddMinutes(-1 * d1.Minute);
            }

            if (_high[i - 1] > LastHigh) LastHigh = _high[i - 1];
            if (_low[i - 1] < LastLow) LastLow = _low[i - 1];

            // Intra Day Mode
            if (Mode == 0)
            {
                // 適用時間軸の判定
                if (_Period < (int)IntraDayMinuteUnit)
                {
                    DateTime rtime = d1.AddHours((int)DayOpenTimeHourDiff);
                    if ((rtime.Hour * 60 + rtime.Minute) % (int)IntraDayMinuteUnit == 0)
                    {
                        // 単位切り替えの新足時刻の計算
                        // この時刻でPIVOT計算を一度行う
                        unitCloseTime = rtime.AddHours((int)DayOpenTimeHourDiff).TimeOfDay;  // ここを動的に変更する

                        DateTime dtime = d1.AddMinutes(_Period);

                        // 単位切り替えの最終足時刻の計算
                        // (この時刻で上で設定した時刻でPIVOT計算するように計算開始フラグを未計算状態に変更する)
                        initTime = dtime.TimeOfDay;
                    }
                }
            }

            TimeSpan itime = d1.TimeOfDay;

            if (itime.CompareTo(initTime) == 0)
            {
                if ((Mode == PIVOTSPAN.IntraDay)
                 || (Mode == PIVOTSPAN.Daily)
                 || (Mode == PIVOTSPAN.Weekly && WeekFirst(i))
                 || (Mode == PIVOTSPAN.Monthly && MonthFirst(i))
                 || (Mode == PIVOTSPAN.Month2 && Month2Span(i))
                 || (Mode == PIVOTSPAN.Month3 && Month3Span(i))
                 || (Mode == PIVOTSPAN.Month4 && Month4Span(i))
                 || (Mode == PIVOTSPAN.Month6 && Month6Span(i))
                 || (Mode == PIVOTSPAN.Year1 && Year1Span(i))
                 || (Mode == PIVOTSPAN.Year2 && Year2Span(i))
                 || (Mode == PIVOTSPAN.Year5 && Year5Span(i))
                  )
                {
                    init = false;
                }
            }
            if (!init)
            {
                if ((Mode == PIVOTSPAN.IntraDay && itime.CompareTo(unitCloseTime)==0)
                 || (Mode ==PIVOTSPAN.Daily     && itime == DayOpenTime)
                 || (Mode ==PIVOTSPAN.Weekly    && itime == DayOpenTime && WeekFirst(i))
                 || (Mode ==PIVOTSPAN.Monthly   && itime == DayOpenTime && MonthFirst(i))
                 || (Mode ==PIVOTSPAN.Month2    && itime == DayOpenTime && Month2Span(i))
                 || (Mode ==PIVOTSPAN.Month3    && itime == DayOpenTime && Month3Span(i))
                 || (Mode ==PIVOTSPAN.Month4    && itime == DayOpenTime && Month4Span(i))
                 || (Mode ==PIVOTSPAN.Month6    && itime == DayOpenTime && Month6Span(i))
                 || (Mode ==PIVOTSPAN.Year1     && itime == DayOpenTime && Year1Span(i))
                 || (Mode ==PIVOTSPAN.Year2     && itime == DayOpenTime && Year2Span(i))
                 || (Mode ==PIVOTSPAN.Year5     && itime == DayOpenTime && Year5Span(i))
                  )
                {

                    init = true;


                    double LastClose = _close[i - 1 - Adjustment];
                    double range = LastHigh - LastLow;

                    switch (Type)
                    {
                        case PIVOTTYPE.Floor:
                            {
                                if (Mode < PIVOTSPAN.Weekly)
                                {
                                    switch (PivotMeth)
                                    {
                                        case PivotMethod.HLC: P = (LastHigh + LastLow + LastClose) / 3; break;
                                        case PivotMethod.HLCC: P = (LastHigh + LastLow + LastClose + LastClose) / 4; break;
                                        case PivotMethod.HLOC: P = (LastHigh + LastLow + LastClose + OP) / 4; break;
                                        case PivotMethod.HLOO: P = (LastHigh + LastLow + OP + OP) / 4; break;
                                        case PivotMethod.HLO: P = (LastHigh + LastLow + OP) / 3; break;
                                    }
                                }
                                else
                                {
                                    P = (LastHigh + LastLow + LastClose + _open[i]) / 4;
                                }

                                P2 = (LastHigh + LastLow) / 2;

                                R4 = P + 3 * range;
                                R3 = LastHigh + 2 * (P - LastLow);
                                R2 = P + range;
                                R1 = (2 * P) - LastLow;

                                S1 = (2 * P) - LastHigh;
                                S2 = P - range;
                                S3 = LastLow - 2 * (LastHigh - P);
                                S4 = P - 3 * range;

                                R5 = (P - S3) + R4;
                                S5 = S4 - (R3 - P);

                                break;
                            }
                        case PIVOTTYPE.Fibonacci:  // fibpnacci
                            {
                                if (Mode < PIVOTSPAN.Weekly)
                                {
                                    switch (PivotMeth)
                                    {
                                        case PivotMethod.HLC: P = (LastHigh + LastLow + LastClose) / 3; break;
                                        case PivotMethod.HLCC: P = (LastHigh + LastLow + LastClose + LastClose) / 4; break;
                                        case PivotMethod.HLOC: P = (LastHigh + LastLow + LastClose + OP) / 4; break;
                                        case PivotMethod.HLOO: P = (LastHigh + LastLow + OP + OP) / 4; break;
                                        case PivotMethod.HLO: P = (LastHigh + LastLow + OP) / 3; break;
                                    }
                                }
                                else
                                {
                                    P = (LastHigh + LastLow + LastClose + _open[i]) / 4;
                                }
                                P2 = (LastHigh + LastLow) / 2;

                                R5 = P + range * 1.382;
                                R4 = P + range * 1;
                                R3 = P + range * 0.786;
                                R2 = P + range * 0.618;
                                R1 = P + range * 0.382;

                                S1 = P - range * 0.382;
                                S2 = P - range * 0.618;
                                S3 = P - range * 0.786;
                                S4 = P - range * 1;
                                S5 = P - range * 1.382;

                                break;
                            }
                        case PIVOTTYPE.Camarilla:
                            {
                                //P = (LastHigh + LastLow + LastClose) / 3;
                                //P2 = (LastHigh+LastLow)/2;

                                double dfp = range * 1.1;

                                R4 = LastClose + dfp / 2;
                                R3 = LastClose + dfp / 4;
                                R2 = LastClose + dfp / 6;
                                R1 = LastClose + dfp / 12;

                                S1 = LastClose - dfp / 12;
                                S2 = LastClose - dfp / 6;
                                S3 = LastClose - dfp / 4;
                                S4 = LastClose - dfp / 2;

                                if (LastLow > 0)
                                {
                                    R5 = (LastHigh / LastLow) * LastClose;
                                    S5 = LastClose - (R5 - LastClose);
                                }

                                break;
                            }
                        case PIVOTTYPE.Woodie: // woody
                            {
                                P = (LastHigh + LastLow + (2 * LastClose)) / 4;
                                P2 = (LastHigh + LastLow) / 2;

                                R4 = P + 3 * range;
                                R3 = P + 2 * range;
                                R2 = P + range;
                                R1 = P * 2 - LastLow;

                                S1 = P * 2 - LastHigh;
                                S2 = P - range;
                                S3 = P - 2 * range;
                                S4 = P - 3 * range;

                                break;
                            }
                        case PIVOTTYPE.DeMark: // DeMark
                            {
                                double X = LastHigh + LastLow + (2 * LastClose);
                                if (LastClose < OP)
                                    X = LastHigh + (2 * LastLow) + LastClose;
                                else
                                if (LastClose > OP)
                                    X = (2 * LastHigh) + LastLow + LastClose;

                                P = X / 4;
                                P2 = (LastHigh + LastLow) / 2;

                                R4 = P + 3 * range;
                                R3 = P + 2 * range;
                                R2 = P + range;
                                R1 = X / 2 - LastLow;

                                S1 = X / 2 - LastHigh;
                                S2 = P - range;
                                S3 = P - 2 * range;
                                S4 = P - 3 * range;

                                break;
                            }
                    }

                    // fibonacci
                    f214 = (LastLow + ((range / 100) * (100 - 21.4)));
                    f236 = (LastLow + ((range / 100) * (100 - 23.6)));
                    f382 = (LastLow + ((range / 100) * (100 - 38.2)));
                    f050 = (LastLow + ((range / 100) * (100 - 50)));
                    f618 = (LastLow + ((range / 100) * (38.2)));
                    f764 = (LastLow + ((range / 100) * (23.6)));
                    f786 = (LastLow + ((range / 100) * (21.4)));


                    // 処理した日付が古い場合のみ新しい配列を追加してTR計算する
                    // 同一バーの処理はここで抑制する
                    if (chkArDateTime != d1 && pC > 0 && LastLow > 0)
                    {
                        chkArDateTime = d1;

                        double tr1 = range;
                        // ここでのpCはLastCloseに対して一つ前のCloseを持っている
                        double tr2 = Math.Abs(LastHigh - pC);
                        double tr3 = Math.Abs(LastLow - pC);

                        TR.Add(Math.Max(Math.Max(tr1, tr2), tr3));
                    }

                    // atr
                    int asz = TR.Count();
                    if (ATRPeriod < asz)
                    {
                        //double atr = iMAOnArray(TR, 0, ATRPeriod, 0, MODE_SMA, 0);       

                        double sumtr = 0;
                        asz--;
                        for (int jk = 0; jk < ATRPeriod; jk++)
                            sumtr += TR[asz - jk];
                        double atr = sumtr / ATRPeriod;

                        atrR5e = _open[i] + atr * ATRLevel5;
                        atrR4e = _open[i] + atr * ATRLevel4;
                        atrR3e = _open[i] + atr * ATRLevel3;
                        atrR2e = _open[i] + atr * ATRLevel2;
                        atrR1e = _open[i] + atr * ATRLevel1;

                        atrS1e = _open[i] - atr * ATRLevel1;
                        atrS2e = _open[i] - atr * ATRLevel2;
                        atrS3e = _open[i] - atr * ATRLevel3;
                        atrS4e = _open[i] - atr * ATRLevel4;
                        atrS5e = _open[i] - atr * ATRLevel5;
                    }

                    // previous values
                    pO = OP;
                    pH = LastHigh;
                    pL = LastLow;
                    pC = LastClose;

                    // new values
                    LastLow = _open[i];
                    LastHigh = _open[i];
                    OP = _open[i];

                }
            }

            if (ShowPreviousPrice)
            {
                prevO[i] = pO;
                prevH[i] = pH;
                prevL[i] = pL;
                prevC[i] = pC;
            }

            OPBuffer[i] = OP;
            PPBuffer[i] = P;
            P2Buffer[i] = P2;
            if (P2 > P)
                P3Buffer[i] = P - (P2 - P);
            else
                P3Buffer[i] = P + (P - P2);

            if (ShowSR)
            {
                S1Buffer[i] = S1;
                R1Buffer[i] = R1;
                S2Buffer[i] = S2;
                R2Buffer[i] = R2;
                S3Buffer[i] = S3;
                R3Buffer[i] = R3;
                S4Buffer[i] = S4;
                R4Buffer[i] = R4;
                S5Buffer[i] = S5;
                R5Buffer[i] = R5;

            }
            else
            {
                S1Buffer[i] = 0;
                R1Buffer[i] = 0;
                S2Buffer[i] = 0;
                R2Buffer[i] = 0;
                S3Buffer[i] = 0;
                R3Buffer[i] = 0;
                S4Buffer[i] = 0;
                R4Buffer[i] = 0;
                S5Buffer[i] = 0;
                R5Buffer[i] = 0;
            }

            if (ShowMidPoint)
            {
                M0[i] = 0.5 * (S4 + S5);
                M1[i] = 0.5 * (S3 + S4);
                M2[i] = 0.5 * (S2 + S3);
                M3[i] = 0.5 * (S1 + S2);
                M4[i] = 0.5 * (P + S1);
                M5[i] = 0.5 * (P + R1);
                M6[i] = 0.5 * (R1 + R2);
                M7[i] = 0.5 * (R2 + R3);
                M8[i] = 0.5 * (R3 + R4);
                M9[i] = 0.5 * (R4 + R5);
            }
            else
            {
                M0[i] = 0;
                M1[i] = 0;
                M2[i] = 0;
                M3[i] = 0;
                M4[i] = 0;
                M5[i] = 0;
                M6[i] = 0;
                M7[i] = 0;
                M8[i] = 0;
                M9[i] = 0;
            }

            if (ShowFibonacci)
            {
                f214Buf[i] = f214;
                f236Buf[i] = f236;
                f382Buf[i] = f382;
                f050Buf[i] = f050;
                f618Buf[i] = f618;
                f764Buf[i] = f764;
                f786Buf[i] = f786;
            }
            else
            {
                f214Buf[i] = 0;
                f236Buf[i] = 0;
                f382Buf[i] = 0;
                f050Buf[i] = 0;
                f618Buf[i] = 0;
                f764Buf[i] = 0;
                f786Buf[i] = 0;
            }

            if (ShowATRPivot)
            {
                atrR5[i] = atrR5e;
                atrR4[i] = atrR4e;
                atrR3[i] = atrR3e;
                atrR2[i] = atrR2e;
                atrR1[i] = atrR1e;
                atrS1[i] = atrS1e;
                atrS2[i] = atrS2e;
                atrS3[i] = atrS3e;
                atrS4[i] = atrS4e;
                atrS5[i] = atrS5e;
            }
            else
            {
                atrR5[i] = 0;
                atrR4[i] = 0;
                atrR3[i] = 0;
                atrR2[i] = 0;
                atrR1[i] = 0;
                atrS1[i] = 0;
                atrS2[i] = 0;
                atrS3[i] = 0;
                atrS4[i] = 0;
                atrS5[i] = 0;
            }

            return 0;

        }

        bool WeekFirst(int i)
        {
            if ((int)_time[i - 1].DayOfWeek - (int)_time[i].DayOfWeek >= 2)
                return (true);
            else
                return _time[i].DayOfWeek == DayOfWeek.Monday;
        }

        bool MonthFirst(int i)
        {
            if (_time[i - 1].Day - _time[i].Day > 20)
                return (true);
            else
                return _time[i].Day == 1;
        }

        bool Month2Span(int i)
        {
            int mon = _time[i].Month;
            if (_Period == (int)ENUM_TIMEFRAMES.PERIOD_MN1 && _time[i].Day == 1 && (mon == 1 || mon == 3 || mon == 5 || mon == 7 || mon == 9 || mon == 11))
                return (true);
            else
            if (_time[i - 1].Day - _time[i].Day > 20 && (mon == 1 || mon == 3 || mon == 5 || mon == 7 || mon == 9 || mon == 11))
                return (true);
            else
                return (false);
        }

        bool Month3Span(int i)
        {
            int mon = _time[i].Month;
            if (_Period == (int)ENUM_TIMEFRAMES.PERIOD_MN1 && _time[i].Day == 1 && (mon == 1 || mon == 4 || mon == 7 || mon == 10))
                return (true);
            else
            if (_time[i - 1].Day - _time[i].Day > 20 && (mon == 1 || mon == 4 || mon == 7 || mon == 10))
                return (true);
            else
                return (false);
        }

        bool Month4Span(int i)
        {
            int mon = _time[i].Month;
            if (_Period == (int)ENUM_TIMEFRAMES.PERIOD_MN1 && _time[i].Day == 1 && (mon == 1 || mon == 5 || mon == 9))
                return (true);
            else
            if (_time[i - 1].Day - _time[i].Day > 20 && (mon == 1 || mon == 5 || mon == 9))
                return (true);
            else
                return (false);
        }

        bool Month6Span(int i)
        {
            int mon = _time[i].Month;
            if (_Period == (int)ENUM_TIMEFRAMES.PERIOD_MN1 && _time[i].Day == 1 && (mon == 1 || mon == 7))
                return (true);
            else
            if (_time[i - 1].Day - _time[i].Day > 20 && (mon == 1 || mon == 7))
                return (true);
            else
                return (false);
        }

        bool Year1Span(int i)
        {
            int mon = _time[i].Month;
            if (_Period == (int)ENUM_TIMEFRAMES.PERIOD_MN1 && _time[i].Day == 1 && mon == 1)
                return (true);
            else
            if (_time[i - 1].Day - _time[i].Day > 20 && mon == 1)
                return (true);
            else
                return (false);
        }

        bool Year2Span(int i)
        {
            int mon = _time[i].Month;
            int even = _time[i].Year % 2;
            if (_Period == (int)ENUM_TIMEFRAMES.PERIOD_MN1 && _time[i].Day == 1 && mon == 1 && even == 0)
                return (true);
            else
            if (_time[i - 1].Day - _time[i].Day > 20 && mon == 1 && even == 0)
                return (true);
            else
                return (false);
        }

        bool Year5Span(int i)
        {
            int mon = _time[i].Month;
            int even = _time[i].Year % 5;
            if (_Period == (int)ENUM_TIMEFRAMES.PERIOD_MN1 && _time[i].Day == 1 && mon == 1 && even == 0)
                return (true);
            else
            if (_time[i - 1].Day - _time[i].Day > 20 && mon == 1 && even == 0)
                return (true);
            else
                return (false);
        }


    }
}
