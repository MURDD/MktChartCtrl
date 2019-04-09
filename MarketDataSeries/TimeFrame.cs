using System;
using System.Collections.Generic;
using System.Text;

namespace MarketDataSeries
{
    // 概要:
    //     Contains supported timeframe values from Minute 1 to Monthly.
    public class TimeFrame
    {
        //
        // 概要:
        //     1 Minute Timeframe
        public static TimeFrame Minute = new TimeFrame(1, "Minute");
        //
        // 概要:
        //     2 Minute Timeframe
        public static TimeFrame Minute2 = new TimeFrame(2, "Minute2");
        //
        // 概要:
        //     3 Minute Timeframe
        public static TimeFrame Minute3 = new TimeFrame(3, "Minute3");
        //
        // 概要:
        //     4 Minute Timeframe
        public static TimeFrame Minute4 = new TimeFrame(4, "Minute4");
        //
        // 概要:
        //     5 Minute Timeframe
        public static TimeFrame Minute5 = new TimeFrame(5, "Minute5");
        //
        // 概要:
        //     6 Minute Timeframe
        public static TimeFrame Minute6 = new TimeFrame(6, "Minute6");
        //
        // 概要:
        //     7 Minute Timeframe
        public static TimeFrame Minute7 = new TimeFrame(7, "Minute7");
        //
        // 概要:
        //     8 Minute Timeframe
        public static TimeFrame Minute8 = new TimeFrame(8, "Minute8");
        //
        // 概要:
        //     9 Minute Timeframe
        public static TimeFrame Minute9 = new TimeFrame(9, "Minute9");
        //
        // 概要:
        //     10 Minute Timeframe
        public static TimeFrame Minute10 = new TimeFrame(10, "Minute10");
        //
        // 概要:
        //     15 Minute Timeframe
        public static TimeFrame Minute15 = new TimeFrame(15, "Minute15");
        //
        // 概要:
        //     20 Minute Timeframe
        public static TimeFrame Minute20 = new TimeFrame(20, "Minute20");
        //
        // 概要:
        //     30 Minute Timeframe
        public static TimeFrame Minute30 = new TimeFrame(30, "Minute30");
        //
        // 概要:
        //     45 Minute Timeframe
        public static TimeFrame Minute45 = new TimeFrame(45, "Minute45");
        //
        // 概要:
        //     1 hour Timeframe
        public static TimeFrame Hour = new TimeFrame(60, "Hour");
        //
        // 概要:
        //     2 hour Timeframe
        public static TimeFrame Hour2 = new TimeFrame(60 * 2, "Hour2");
        //
        // 概要:
        //     3 hour Timeframe
        public static TimeFrame Hour3 = new TimeFrame(60 * 3, "Hour3");
        //
        // 概要:
        //     4 hour Timeframe
        public static TimeFrame Hour4 = new TimeFrame(60 * 4, "Hour4");
        //
        // 概要:
        //     6 hour Timeframe
        public static TimeFrame Hour6 = new TimeFrame(60 * 6, "Hour6");
        //
        // 概要:
        //     8 hour Timeframe
        public static TimeFrame Hour8 = new TimeFrame(60 * 8, "Hour8");
        //
        // 概要:
        //     12 hour Timeframe
        public static TimeFrame Hour12 = new TimeFrame(60 * 12, "Hour12");
        // 概要:
        //     Daily Timeframe
        public static TimeFrame Daily = new TimeFrame(1440, "Daily");
        //
        // 概要:
        //     2 day Timeframe
        public static TimeFrame Day2 = new TimeFrame(1440 * 2, "Day2");
        //
        // 概要:
        //     3 day Timeframe
        public static TimeFrame Day3 = new TimeFrame(1440 * 3, "Day3");
        //
        // 概要:
        //     Weekly Timeframe
        public static TimeFrame Weekly = new TimeFrame(10080, "Weekly");
        //
        // 概要:
        //     Monthly Timeframe
        public static TimeFrame Monthly = new TimeFrame(43200, "Monthly");

        private int _minute = 0;
        private string _vname = "";
        private TimeFrame(int minute, string vname)
        {
            _minute = minute;
            _vname = vname;
        }

        public static bool operator <(TimeFrame x, TimeFrame y) { return x._minute < y._minute; }
        public static bool operator <=(TimeFrame x, TimeFrame y) { return x._minute <= y._minute; }
        public static bool operator >(TimeFrame x, TimeFrame y) { return x._minute > y._minute; }
        public static bool operator >=(TimeFrame x, TimeFrame y) { return x._minute >= y._minute; }

        // 概要:
        //     Convert the TimeFrame property to a string
        //
        // 戻り値:
        //     the string representation of the timeframe
        public override string ToString() { return this._vname; }

        public int FrameMinute { get { return _minute; } }
    }
}
