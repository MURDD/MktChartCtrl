using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketSeries
{
    /// <summary>
    /// https://www.sevendata.co.jp/shihyou/technical/pf.html
    /// </summary>
    public class PFPatterns
    {
        public PFDataSeries _series = null;

        public enum PFPatternType
        {
            DoubleTop,
            DoubleBottom,
            TripleTop,
            TripleBottom,
            BullishCatapult,
            BearishCatapult,
            UpwardReversal,
            DownwardReversal,
            BullishPennant,
            BearishPennant,
            BullSignal,
            BearSignal,
            FallAndUpper,
            RiseAndLower,
            SpreadTripleTop,
            SpreadTripleBottom,
            BroadeningTop,
            BroadeningBottom,
            DiagonalTripleTop,
            DiagonalTripleBottom,
            BullShakeout,
            BearShakeout
        }

        public bool useDoubleTop { get; set; }
        public bool useDoubleBottom { get; set; }
        public bool useTripleTop { get; set; }
        public bool useTripleBottom { get; set; }
        public bool useBullishCatapult { get; set; }
        public bool useBearishCatapult { get; set; }
        public bool useUpwardReversal { get; set; }
        public bool useDownwardReversal { get; set; }
        public bool useBullishPennant { get; set; }
        public bool useBearishPennant { get; set; }
        public bool useBullSignal { get; set; }
        public bool useBearSignal { get; set; }
        public bool useFallAndUpper { get; set; }
        public bool useRiseAndLower { get; set; }
        public bool useSpreadTripleTop { get; set; }
        public bool useSpreadTripleBottom { get; set; }
        public bool useBroadeningTop { get; set; }
        public bool useBroadeningBottom { get; set; }
        public bool useDiagonalTripleTop { get; set; }
        public bool useDiagonalTripleBottom { get; set; }
        public bool useBullShakeout { get; set; }
        public bool useBearShakeout { get; set; }

        public List<PFPatterns.PFPatternType> Verify(int index)
        {
            List<PFPatternType> ret = new List<PFPatternType>();

            if (useDoubleTop && isDoubleTop(index)) { ret.Add(PFPatternType.DoubleTop); }
            if (useDoubleBottom && isDoubleBottom(index)) { ret.Add(PFPatternType.DoubleBottom); }
            if (useTripleTop && isTripleTop(index)) { ret.Add(PFPatternType.TripleTop); }
            if (useTripleBottom && isTripleBottom(index)) { ret.Add(PFPatternType.TripleBottom); }
            if (useBullishCatapult && isBullishCatapult(index)) { ret.Add(PFPatternType.BullishCatapult); }
            if (useBearishCatapult && isBearishCatapult(index)) { ret.Add(PFPatternType.BearishCatapult); }
            if (useUpwardReversal && isUpwardReversal(index)) { ret.Add(PFPatternType.UpwardReversal); }
            if (useDownwardReversal && isDownwardReversal(index)) { ret.Add(PFPatternType.DownwardReversal); }
            if (useBullishPennant && isBullishPennant(index)) { ret.Add(PFPatternType.BullishPennant); }
            if (useBearishPennant && isBearishPennant(index)) { ret.Add(PFPatternType.BearishPennant); }
            if (useBullSignal && isBullSignal(index)) { ret.Add(PFPatternType.BullSignal); }
            if (useBearSignal && isBearSignal(index)) { ret.Add(PFPatternType.BearSignal); }
            if (useFallAndUpper && isFallAndUpper(index)) { ret.Add(PFPatternType.FallAndUpper); }
            if (useRiseAndLower && isRiseAndLower(index)) { ret.Add(PFPatternType.RiseAndLower); }
            if (useSpreadTripleTop && isSpreadTripleTop(index)) { ret.Add(PFPatternType.SpreadTripleTop); }
            if (useSpreadTripleBottom && isSpreadTripleBottom(index)) { ret.Add(PFPatternType.SpreadTripleBottom); }
            if (useBroadeningTop && isBroadeningTop(index)) { ret.Add(PFPatternType.BroadeningTop); }
            if (useBroadeningBottom && isBroadeningBottom(index)) { ret.Add(PFPatternType.BroadeningBottom); }
            if (useDiagonalTripleTop && isDiagonalTripleTop(index)) { ret.Add(PFPatternType.DiagonalTripleTop); }
            if (useDiagonalTripleBottom && isDiagonalTripleBottom(index)) { ret.Add(PFPatternType.DiagonalTripleBottom); }
            if (useBullShakeout && isBullShakeout(index)) { ret.Add(PFPatternType.BullShakeout); }
            if (useBearShakeout && isBearShakeout(index)) { ret.Add(PFPatternType.BearShakeout); }

            return ret;
        }

        public PFPatterns()
        {
            useDoubleTop = false;
            useDoubleBottom = false;
            useTripleTop = false;
            useTripleBottom = false;
            useBullishCatapult = false;
            useBearishCatapult = false;
            useUpwardReversal = false;
            useDownwardReversal = false;
            useBullishPennant = false;
            useBearishPennant = false;
            useBullSignal = false;
            useBearSignal = false;
            useFallAndUpper = false;
            useRiseAndLower = false;
            useSpreadTripleTop = false;
            useSpreadTripleBottom = false;
            useBroadeningTop = false;
            useBroadeningBottom = false;
            useDiagonalTripleTop = false;
            useDiagonalTripleBottom = false;
            useBullShakeout = false;
            useBearShakeout = false;
        }

        public string GetPatternNameText(List<PFPatterns.PFPatternType> value)
        {
            List<string> ret = new List<string>();

            foreach (var n in value)
                ret.Add(Enum.GetName(typeof(PFPatternType), n));

            return string.Join(",", ret);
        }

        public List<string> GetPatternNames(List<PFPatterns.PFPatternType> value)
        {
            List<string> ret = new List<string>();

            foreach (var n in value)
                ret.Add(Enum.GetName(typeof(PFPatternType), n));

            //return string.Join(",",ret);
            return ret;
        }

        public List<string> GetPatternNum(List<PFPatterns.PFPatternType> value)
        {
            List<string> ret = new List<string>();

            foreach (var n in value)
                ret.Add(((int)n).ToString());

            //return string.Join(",",ret);
            return ret;
        }

        /// <summary>
        /// 価格変動中のダブルトップ型
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, 1:買いシグナル</returns>
        public bool isDoubleTop(int index)
        {
            if (index < 2) return false;
            if (_series == null) return false;
            if (_series.Count < 3) return false;

            PFDataSet barNow = _series[index];
            PFDataSet barPrev1 = _series[index - 1];
            PFDataSet barPrev2 = _series[index - 2];

            bool[] cond = new bool[3];

            cond[0] = barPrev2.High >= barPrev1.High;
            cond[1] = barPrev2.Low < barPrev1.Low;
            cond[2] = barPrev2.High < barNow.High;

            if (cond.Where(t => t).Count() == cond.Length)
                return true;

            return false;
        }

        /// <summary>
        /// 価格変動中のダブルボトム型
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, -1:売りシグナル</returns>
        public bool isDoubleBottom(int index)
        {
            if (index < 2) return false;
            if (_series == null) return false;
            if (_series.Count < 3) return false;

            PFDataSet barNow = _series[index];
            PFDataSet barPrev1 = _series[index - 1];
            PFDataSet barPrev2 = _series[index - 2];

            bool[] cond = new bool[3];

            cond[0] = barPrev2.Low <= barPrev1.Low;
            cond[1] = barPrev2.High > barPrev1.High;
            cond[2] = barPrev2.Low > barNow.Low;

            if (cond.Where(t => t).Count() == cond.Length)
                return true;

            return false;
        }

        /// <summary>
        /// トリプルトップ型
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, 1:買いシグナル</returns>
        public bool isTripleTop(int index)
        {
            if (index < 4) return false;
            if (_series == null) return false;
            if (_series.Count < 5) return false;

            PFDataSet barNow = _series[index];
            PFDataSet barPrev1 = _series[index - 1];
            PFDataSet barPrev2 = _series[index - 2];
            PFDataSet barPrev3 = _series[index - 3];
            PFDataSet barPrev4 = _series[index - 4];

            bool[] cond = new bool[4];

            cond[0] = barPrev4.High == barPrev2.High;
            cond[1] = barPrev4.High > barPrev3.High;
            cond[2] = barPrev2.High > barPrev1.High;
            cond[3] = barPrev2.High < barNow.High;

            if (cond.Where(t => t).Count() == cond.Length)
                return true;

            return false;
        }

        /// <summary>
        /// トリプルボトム型
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, -1:売りシグナル</returns>
        public bool isTripleBottom(int index)
        {
            if (index < 4) return false;
            if (_series == null) return false;
            if (_series.Count < 5) return false;

            PFDataSet barNow = _series[index];
            PFDataSet barPrev1 = _series[index - 1];
            PFDataSet barPrev2 = _series[index - 2];
            PFDataSet barPrev3 = _series[index - 3];
            PFDataSet barPrev4 = _series[index - 4];

            bool[] cond = new bool[4];

            cond[0] = barPrev4.Low == barPrev2.Low;
            cond[1] = barPrev4.Low < barPrev3.Low;
            cond[2] = barPrev2.Low < barPrev1.Low;
            cond[3] = barPrev2.Low > barNow.Low;

            if (cond.Where(t => t).Count() == cond.Length)
                return true;

            return false;
        }

        /// <summary>
        /// 強気のカタパルト型
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, 1:買いシグナル</returns>
        public bool isBullishCatapult(int index)
        {
            if (index < 6) return false;
            if (_series == null) return false;
            if (_series.Count < 7) return false;

            PFDataSet barNow = _series[index];
            PFDataSet barPrev1 = _series[index - 1];
            PFDataSet barPrev2 = _series[index - 2];
            PFDataSet barPrev3 = _series[index - 3];
            PFDataSet barPrev4 = _series[index - 4];
            PFDataSet barPrev5 = _series[index - 5];
            PFDataSet barPrev6 = _series[index - 6];

            bool[] cond = new bool[7];

            cond[0] = barPrev6.High == barPrev4.High;
            cond[1] = barPrev6.High > barPrev5.High;
            cond[2] = barPrev4.High > barPrev3.High;
            cond[3] = barPrev4.High < barPrev2.High;
            cond[4] = barPrev2.High > barPrev1.High;
            cond[5] = barPrev4.High < barPrev1.High;
            cond[6] = barPrev2.High < barNow.High;

            if (cond.Where(t => t).Count() == cond.Length)
                return true;

            return false;
        }

        /// <summary>
        /// 弱気のカタパルト型
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, -1:売りシグナル</returns>
        public bool isBearishCatapult(int index)
        {
            if (index < 6) return false;
            if (_series == null) return false;
            if (_series.Count < 7) return false;

            PFDataSet barNow = _series[index];
            PFDataSet barPrev1 = _series[index - 1];
            PFDataSet barPrev2 = _series[index - 2];
            PFDataSet barPrev3 = _series[index - 3];
            PFDataSet barPrev4 = _series[index - 4];
            PFDataSet barPrev5 = _series[index - 5];
            PFDataSet barPrev6 = _series[index - 6];

            bool[] cond = new bool[7];

            cond[0] = barPrev6.Low == barPrev4.Low;
            cond[1] = barPrev6.Low < barPrev5.Low;
            cond[2] = barPrev4.Low < barPrev3.Low;
            cond[3] = barPrev4.Low > barPrev2.Low;
            cond[4] = barPrev2.Low < barPrev1.Low;
            cond[5] = barPrev4.Low > barPrev1.Low;
            cond[6] = barPrev2.Low > barNow.Low;

            if (cond.Where(t => t).Count() == cond.Length)
                return true;

            return false;
        }

        /// <summary>
        /// 高ね切り下げ後の逆転上昇型
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, 1:買いシグナル</returns>
        public bool isUpwardReversal(int index)
        {
            if (index < 4) return false;
            if (_series == null) return false;
            if (_series.Count < 5) return false;

            PFDataSet barNow = _series[index];
            PFDataSet barPrev1 = _series[index - 1];
            PFDataSet barPrev2 = _series[index - 2];
            PFDataSet barPrev3 = _series[index - 3];
            PFDataSet barPrev4 = _series[index - 4];

            bool[] cond = new bool[4];

            cond[0] = barPrev4.High > barPrev3.High;
            cond[1] = barPrev3.High >= barPrev2.High;
            cond[2] = barPrev1.High < barNow.High;
            cond[3] = barPrev2.High < barNow.High;

            if (cond.Where(t => t).Count() == cond.Length)
                return true;

            return false;
        }

        /// <summary>
        /// 安値切り上げ後の逆転下降型
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, -1:売りシグナル</returns>
        public bool isDownwardReversal(int index)
        {
            if (index < 4) return false;
            if (_series == null) return false;
            if (_series.Count < 5) return false;

            PFDataSet barNow = _series[index];
            PFDataSet barPrev1 = _series[index - 1];
            PFDataSet barPrev2 = _series[index - 2];
            PFDataSet barPrev3 = _series[index - 3];
            PFDataSet barPrev4 = _series[index - 4];

            bool[] cond = new bool[4];

            cond[0] = barPrev4.Low < barPrev3.Low;
            cond[1] = barPrev3.Low <= barPrev2.Low;
            cond[2] = barPrev1.Low > barNow.Low;
            cond[3] = barPrev2.Low > barNow.Low;

            if (cond.Where(t => t).Count() == cond.Length)
                return true;

            return false;
        }
        
        /// <summary>
        /// 買い三角形型
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, 1:買いシグナル</returns>
        public bool isBullishPennant(int index)
        {
            if (index < 5) return false;
            if (_series == null) return false;
            if (_series.Count < 6) return false;

            PFDataSet barNow = _series[index];
            PFDataSet barPrev1 = _series[index - 1];
            PFDataSet barPrev2 = _series[index - 2];
            PFDataSet barPrev3 = _series[index - 3];
            PFDataSet barPrev4 = _series[index - 4];
            PFDataSet barPrev5 = _series[index - 5];

            bool[] cond = new bool[5];

            var blck = _series.BoxPipSize * _series.PipSize;

            cond[0] = barPrev5.Low == barPrev3.Low;
            cond[1] = barPrev3.Low == barPrev1.Low;
            cond[2] = barPrev5.High > barPrev3.High;
            cond[3] = barPrev3.High > barPrev1.High;
            cond[4] = barPrev1.High < barNow.High;

            if (cond.Where(t => t).Count() == cond.Length)
                return true;

            return false;
        }

        /// <summary>
        /// 売り三角形型
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, -1:売りシグナル</returns>
        public bool isBearishPennant(int index)
        {
            if (index < 5) return false;
            if (_series == null) return false;
            if (_series.Count < 6) return false;

            PFDataSet barNow = _series[index];
            PFDataSet barPrev1 = _series[index - 1];
            PFDataSet barPrev2 = _series[index - 2];
            PFDataSet barPrev3 = _series[index - 3];
            PFDataSet barPrev4 = _series[index - 4];
            PFDataSet barPrev5 = _series[index - 5];

            bool[] cond = new bool[5];

            var blck = _series.BoxPipSize * _series.PipSize;

            cond[0] = barPrev5.High == barPrev3.High;
            cond[1] = barPrev3.High == barPrev1.High;
            cond[2] = barPrev5.Low < barPrev3.Low;
            cond[3] = barPrev3.Low < barPrev1.Low;
            cond[4] = barPrev1.Low > barNow.Low;

            if (cond.Where(t => t).Count() == cond.Length)
                return true;

            return false;
        }

        /// <summary>
        /// 強気信号型
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, 1:買いシグナル</returns>
        public bool isBullSignal(int index)
        {
            if (index < 3) return false;
            if (_series == null) return false;
            if (_series.Count < 4) return false;

            PFDataSet barNow = _series[index];
            PFDataSet barPrev1 = _series[index - 1];
            PFDataSet barPrev2 = _series[index - 2];
            PFDataSet barPrev3 = _series[index - 3];

            bool[] cond = new bool[4];

            cond[0] = barPrev3.Low < barPrev1.Low;
            cond[1] = barPrev2.Low < barNow.Low;
            cond[2] = barPrev3.Low < barNow.Low;
            cond[3] = barPrev2.High < barNow.High;

            if (cond.Where(t => t).Count() == cond.Length)
                return true;

            return false;
        }

        /// <summary>
        /// 弱気信号型
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, -1:売りシグナル</returns>
        public bool isBearSignal(int index)
        {
            if (index < 3) return false;
            if (_series == null) return false;
            if (_series.Count < 4) return false;

            PFDataSet barNow = _series[index];
            PFDataSet barPrev1 = _series[index - 1];
            PFDataSet barPrev2 = _series[index - 2];
            PFDataSet barPrev3 = _series[index - 3];

            bool[] cond = new bool[4];

            cond[0] = barPrev3.High > barPrev1.High;
            cond[1] = barPrev2.High > barNow.High;
            cond[2] = barPrev3.High > barNow.High;
            cond[3] = barPrev2.Low > barNow.Low;

            if (cond.Where(t => t).Count() == cond.Length)
                return true;

            return false;
        }

        /// <summary>
        /// 上値切り下げの上ブレイク
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, 1:買いシグナル</returns>
        public bool isFallAndUpper(int index)
        {
            if (index < 4) return false;
            if (_series == null) return false;
            if (_series.Count < 5) return false;

            PFDataSet barNow = _series[index];
            PFDataSet barPrev1 = _series[index - 1];
            PFDataSet barPrev2 = _series[index - 2];
            PFDataSet barPrev3 = _series[index - 3];
            PFDataSet barPrev4 = _series[index - 4];

            bool[] cond = new bool[4];

            cond[0] = barPrev4.High > barPrev2.High;
            cond[1] = barPrev3.High > barPrev1.High;
            cond[2] = barPrev4.High > barPrev1.High;
            cond[3] = barPrev2.High < barNow.High;

            if (cond.Where(t => t).Count() == cond.Length)
                return true;

            return false;
        }

        /// <summary>
        /// 下値切り上げの下ブレイク
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, -1:売りシグナル</returns>
        public bool isRiseAndLower(int index)
        {
            if (index < 4) return false;
            if (_series == null) return false;
            if (_series.Count < 5) return false;

            PFDataSet barNow = _series[index];
            PFDataSet barPrev1 = _series[index - 1];
            PFDataSet barPrev2 = _series[index - 2];
            PFDataSet barPrev3 = _series[index - 3];
            PFDataSet barPrev4 = _series[index - 4];

            bool[] cond = new bool[4];

            cond[0] = barPrev4.Low < barPrev2.Low;
            cond[1] = barPrev3.Low < barPrev1.Low;
            cond[2] = barPrev4.Low < barPrev1.Low;
            cond[3] = barPrev2.Low > barNow.Low;

            if (cond.Where(t => t).Count() == cond.Length)
                return true;

            return false;
        }

        /// <summary>
        /// 幅の広いトリプルトップ型
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, 1:買いシグナル</returns>
        public bool isSpreadTripleTop(int index)
        {
            //double hi = _series[index - 1].High;
            //double lo = _series[index - 1].Low;

            //for (int i = 1; i < 20; i++)
            //{
            //    _series[index - i].High
            //}

            return false;
        }

        /// <summary>
        /// 幅の広いトリプルボトム型
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, -1:売りシグナル</returns>
        public bool isSpreadTripleBottom(int index)
        {
            return false;
        }

        /// <summary>
        /// 上抜け末広がり型
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, 1:買いシグナル</returns>
        public bool isBroadeningTop(int index)
        {
            return false;
        }

        /// <summary>
        /// 下抜け末広がり型
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, -1:売りシグナル</returns>
        public bool isBroadeningBottom(int index)
        {
            return false;
        }

        /// <summary>
        /// 対角線トリプルトップ
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, 1:買いシグナル</returns>
        public bool isDiagonalTripleTop(int index)
        {
            return false;
        }

        /// <summary>
        /// 対角線トリプルボトム
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, -1:売りシグナル</returns>
        public bool isDiagonalTripleBottom(int index)
        {
            return false;
        }

        /// <summary>
        /// 強気のシェイクアウト
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, 1:買いシグナル</returns>
        public bool isBullShakeout(int index)
        {
            return false;
        }

        /// <summary>
        /// 弱気のシェイクアウト
        /// </summary>
        /// <param name="index"></param>
        /// <param name="index"></param>
        /// <returns>0:なし, -1:売りシグナル</returns>
        public bool isBearShakeout(int index)
        {
            return false;
        }

    }
}
