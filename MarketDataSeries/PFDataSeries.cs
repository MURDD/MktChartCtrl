using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MarketDataSeries.PFPatterns;
using System.Reflection;

namespace MarketDataSeries
{
    /*
     * チャートにて上げは "×"、下げは "〇"
     * そのほか、図表には、数字1〜9と文字A、B、Cが表示されます。 
     * これらは1月から9月の1月から9月までの月を表します。 
     * A、B、Cは、10月、11月、12月に使用され、チャートの枠内のスペースを節約します。
     * 
     */
    public class NewBarEventArgs : EventArgs
    {
        public int barIndex;
        public int direction;
        public double high;
        public double low;
    }

    public class NewBoxEventArgs : EventArgs
    {
        public int barIndex;
        public int direction;
        public double high;
        public double low;
        public int boxIndex;
    }

    public class PFDataSeries : List<PFDataSet>
    {
        public delegate void NewBarEventHandler(object sender, NewBarEventArgs e);
        public delegate void NewBoxEventHandler(object sender, NewBoxEventArgs e);

        public NewBarEventHandler OnNewBar;
        public NewBoxEventHandler OnNewBox;

        // PFパターンチェッカー一覧
        public List<_PFPatternTypes> PatternTypes = new List<_PFPatternTypes>();
        public bool PatternVerify { get; set; }

        // データソース
        MarketData _dataSource = null;
        public MarketData DataSource
        {
            get { return _dataSource; }
            set
            {
                _dataSource = value;
                Refresh();
            }
        }

        // 銘柄
        public string Symbol { get { return DataSource.Symbol; } }

        // 元データの時間サイズ
        public TimeFrame TimeFrame { get { return DataSource.TimeFrame; } }

        // 転換枠数 (default 3 box)
        private int _reversalBox = 3;
        public int ReversalBox { get { return _reversalBox; } set { _reversalBox = value; } }

        // 枠サイズ (default 5 pips)
        private int _boxPipSize = 5;
        public int BoxPipSize { get { return _boxPipSize; } set { _boxPipSize = value; _box = (decimal)NormalizeDouble(_boxPipSize * _pipSize); } }

        // PipSize (ex. 0.01, 0.0001)
        private double _pipSize = 0.00;
        public double PipSize { get { return _pipSize; } set { _pipSize = value; _box = (decimal)NormalizeDouble(_boxPipSize * _pipSize); Refresh(); } }

        private decimal _box = 0;

        public PFDataSeries(int boxPipSize, int reversalBox, double pipSize)
        {
            Initialize(boxPipSize, reversalBox, pipSize);
        }

        public PFDataSeries(MarketData ds, int boxPipSize, int reversalBox, double pipSize)
        {
            PatternVerify = false;
            Initialize(boxPipSize, reversalBox, pipSize);
            _dataSource = ds;
        }

        public void Initialize(int boxPipSize, int reversalBox, double pipSize)
        {
            _boxPipSize = boxPipSize;
            _reversalBox = reversalBox;
            _pipSize = pipSize;

            if (PatternVerify)
            {
                // PFパターンチェッカーの型一覧作成
                Assembly assembly = Assembly.GetExecutingAssembly();

                List<Type> list = new List<Type>();
                foreach (var t in assembly.GetTypes().Where(t => t.Namespace == "MarketSeries.PFPatterns"))
                {
                    if (t.IsSubclassOf(typeof(_PFPatternTypes)))
                        list.Add(t);
                }

                //var list = Assembly.GetExecutingAssembly()
                //                   .GetTypes()
                //                   .Where(t => t.Namespace == this.GetType().Namespace)
                //                   .Where(t => t == typeof(IPFPatternTypes))
                //                   .Select(t => t).ToList();

                // PFパターンチェッカーのインスタンス化
                foreach (var ct in list)
                    PatternTypes.Add((_PFPatternTypes)Activator.CreateInstance(ct));
            }

            _box = (decimal)NormalizeDouble(_boxPipSize * _pipSize);
        }

        // PFパターンチェッカーの名前一覧
        static public List<string> GetPatternNames()
        {
            // 実行時のアセンブリ情報を取得する。
            Assembly assembly = Assembly.GetExecutingAssembly();

            List<string> lst = new List<string>();
            foreach (var t in assembly.GetTypes().Where(t => t.Namespace == "MarketSeries.PFPatterns"))
            {
                if (t.IsSubclassOf(typeof(_PFPatternTypes)))
                    lst.Add(t.Name);
            }
            return lst;

            //return assembly.GetTypes().Where(t => t.Namespace == "MarketSeries.PFPatterns")
            //                          .Where(t => t.IsSubclassOf(typeof(PFPatternTypes))
            //                          .Select(t => t.Name).ToList();
        }

        private List<_PFPatternTypes> Verify(int index)
        {
            if (!PatternVerify)
                return null;

            List<_PFPatternTypes> ret = new List<_PFPatternTypes>();

            foreach (_PFPatternTypes pt in PatternTypes)
                if (pt.Verify(index, this))
                    ret.Add(pt);

            return ret;
        }

        public void Refresh()
        {
            this.Clear();
            if (_dataSource != null)
            {
                int mx = _dataSource.OpenTime.Count;
                for (int i = 0; i < mx; i++)
                    Update(_dataSource.OpenTime[i], _dataSource.Open[i], _dataSource.High[i], _dataSource.Low[i], _dataSource.Close[i]);

                //foreach (var r in TimeLine)
                //{
                //    //System.Diagnostics.Debug.WriteLine(r.Bar.First().OpenDate.ToString() + "," + r.Direction.ToString() + "," + r.High.ToString("0.000") + "," + r.Low.ToString("0.000"));
                //    System.Diagnostics.Debug.WriteLine(r.Bar.First().OpenDate.ToString() + "," + r.Direction.ToString() + "," + r.High.ToString() + "," + r.Low.ToString());
                //}
            }
        }

        public double NormalizeDouble(double value)
        {
            return double.Parse(value.ToString("0.00000"));
        }

        public decimal NormalizeLow(double Low)
        {
            if ((decimal)_box == 0)
                return 0;

            // 安値の剰余
            if ((decimal)Low % (decimal)_box == 0)
                return (decimal)Low;

            return (decimal)Low + ((decimal)_box - ((decimal)Low % (decimal)_box));
        }

        public decimal NormalizeHigh(double High)
        {
            if ((decimal)_box == 0)
                return 0;

            // 高値の剰余
            return (decimal)High - ((decimal)High % (decimal)_box);
        }

        public void AddBoxHigh(DateTime time, double value)
        {
            var t = this[this.Count - 1];
            var r = NormalizeLow(t[t.Count - 1].High);

            if (r == (decimal)value)
                return;

            bool newbar = false;

            if (t[t.Count - 1].Direction < 0)
            {
                t = new PFDataSet();
                this.Add(t);
                newbar = true;
            }

            for (; (double)(r + _box) <= value; r = r + _box)
            {
                // BOXを追加する
                var bx = new PFDataBox() { OpenTime = time, High = (double)(r + _box), Low = (double)(r), Direction = 1, Patterns = null };
                t.Add(bx);

                // BOX追加後の状態からパターン判定を実行する
                var pfp = Verify(this.Count - 1);
                if (t.Count > 0)
                    for (int i = 0; i < t.Count - 1; i++)
                        if (t[i].Patterns != null)
                            pfp.RemoveAll(t[i].Patterns.Contains);
                t.Last().Patterns = pfp;

                if (newbar && OnNewBar != null)
                {
                    newbar = false;
                    NewBarEventArgs e = new NewBarEventArgs();
                    e.barIndex = this.Count - 1;
                    e.direction = 1;
                    e.high = (double)(r + _box);
                    e.low = (double)(r);
                    OnNewBar(this, e);
                }
                if (OnNewBox != null)
                {
                    NewBoxEventArgs e = new NewBoxEventArgs();
                    e.barIndex = this.Count - 1;
                    e.direction = 1;
                    e.high = (double)(r + _box);
                    e.low = (double)(r);
                    e.boxIndex = t.Count - 1;
                    OnNewBox(this, e);
                }
            }
        }

        public void AddBoxLow(DateTime time, double value)
        {
            var t = this[this.Count - 1];
            var r = NormalizeLow(t[t.Count - 1].Low);

            if (r == (decimal)value)
                return;

            bool newbar = false;

            if (t[t.Count - 1].Direction > 0)
            {
                t = new PFDataSet();
                this.Add(t);
                newbar = true;
            }

            for (; (double)(r - _box) >= value; r = r - _box)
            {
                // BOXを追加する
                var bx = new PFDataBox() { OpenTime = time, High = (double)(r), Low = (double)(r - _box), Direction = -1, Patterns = null };
                t.Add(bx);

                // BOX追加後の状態からパターン判定を実行する
                var pfp = Verify(this.Count - 1);
                if (t.Count > 0)
                    for (int i = 0; i < t.Count - 1; i++)
                        if (t[i].Patterns != null)
                            pfp.RemoveAll(t[i].Patterns.Contains);
                t.Last().Patterns = pfp;

                if (newbar && OnNewBar != null)
                {
                    newbar = false;
                    NewBarEventArgs e = new NewBarEventArgs();
                    e.barIndex = this.Count - 1;
                    e.direction = 1;
                    e.high = (double)(r);
                    e.low = (double)(r - _box);
                    OnNewBar(this, e);
                }
                if (OnNewBox != null)
                {
                    NewBoxEventArgs e = new NewBoxEventArgs();
                    e.barIndex = this.Count - 1;
                    e.direction = 1;
                    e.high = (double)(r);
                    e.low = (double)(r - _box);
                    e.boxIndex = t.Count - 1;
                    OnNewBox(this, e);
                }
            }
        }

        // tick : Box価格シリーズ更新
        public void TickUpdate(DateTime openTime, double ask, double bid)
        {
            // bid使用
            double price = bid;

            if (this.Count > 0)
            {
                var t = this[this.Count - 1];

                if (t.Direction > 0)
                {
                    // 反転書き込み有無チェック
                    bool hanten = NormalizeHigh(t.High) - (decimal)(_reversalBox * _box) >= (decimal)NormalizeLow(price);

                    // 継続書き込み有無チェック
                    bool keizoku = NormalizeHigh(t.High) + (decimal)_box <= (decimal)(price);

                    // 反転時の追加判定
                    if (hanten)
                    {
                        var g = NormalizeLow(price);
                        var j = (decimal)((decimal)_reversalBox * (decimal)_box);
                        var k = g + j;
                        if (k <= (decimal)price)
                            keizoku = true;
                        else
                            keizoku = false;
                    }

                    if (hanten)
                        AddBoxLow(openTime, (double)NormalizeLow(price));

                    if (keizoku)
                        AddBoxHigh(openTime, (double)NormalizeHigh(price));

                }
                else if (t.Direction < 0)
                {
                    // 反転書き込み有無チェック
                    bool hanten = (decimal)NormalizeLow(t.Low) + (decimal)(_reversalBox * _box) <= (decimal)NormalizeHigh(price);

                    // 継続書き込み有無チェック
                    bool keizoku = (decimal)NormalizeLow(t.Low) - (decimal)_box >= (decimal)(price);

                    // 反転時の追加判定
                    if (hanten)
                    {
                        var g = NormalizeHigh(price);
                        var j = (decimal)((decimal)_reversalBox * (decimal)_box);
                        var k = g - j;
                        if (k >= (decimal)price)
                            keizoku = true;
                        else
                            keizoku = false;
                    }

                    if (hanten)
                        AddBoxHigh(openTime, (double)NormalizeHigh(price));

                    if (keizoku)
                        AddBoxLow(openTime, (double)NormalizeLow(price));
                }
            }
        }

        // Box価格シリーズ更新
        public void Update(DateTime time, double Open, double High, double Low, double Close)
        {
            if (this.Count > 0)
            {
                //var t = TimeLine.Last();
                var t = this[this.Count - 1];

                if (t.Direction > 0)
                {

                    // 反転書き込み有無チェック
                    bool hanten = NormalizeHigh(t.High) - (decimal)(_reversalBox * _box) > (decimal)NormalizeLow(Low);

                    // 継続書き込み有無チェック
                    bool keizoku = NormalizeHigh(t.High) + (decimal)_box < (decimal)NormalizeHigh(High);

                    // 反転時の追加判定
                    if (hanten)
                    {
                        var g = NormalizeLow(Low);
                        var j = (decimal)((decimal)_reversalBox * (decimal)_box);
                        var k = g + j;
                        if (k <= (decimal)High)
                            keizoku = true;
                        else
                            keizoku = false;
                    }

                    // 中央値
                    var c = (High + Low) / 2;

                    // 中央判定が上げのとき
                    if (Close > c)
                    {
                        // 上げバーのとき、上げで継続していたら先に反転下げバーとBox追加
                        if (hanten)
                            AddBoxLow(time, (double)NormalizeLow(Low));

                        // 上げバーのとき、継続ありだった場合
                        if (keizoku)
                            AddBoxHigh(time, (double)NormalizeHigh(High));
                    }
                    // 上げバーで、中央判定が下げのとき
                    else if (Close <= c)
                    {
                        // 先に継続バーのBox追加
                        if (keizoku)
                            AddBoxHigh(time, (double)NormalizeHigh(High));

                        // 後から反転バーとBox追加
                        if (hanten)
                            AddBoxLow(time, (double)NormalizeLow(Low));
                    }

                    //if (TimeLine.Last().Bar.Count == 0)
                    if (this[this.Count - 1].Count == 0)
                    {
                        throw new Exception("Zero");
                    }
                }
                else if (t.Direction < 0)
                {
                    // 反転書き込み有無チェック
                    bool hanten = (decimal)NormalizeLow(t.Low) + (decimal)(_reversalBox * _box) < (decimal)NormalizeHigh(High);

                    // 継続書き込み有無チェック
                    bool keizoku = (decimal)NormalizeLow(t.Low) - (decimal)_box > (decimal)NormalizeLow(Low);

                    // 反転時の追加判定
                    if (hanten)
                    {
                        var g = NormalizeHigh(High);
                        var j = (decimal)((decimal)_reversalBox * (decimal)_box);
                        var k = g - j;
                        if ( k >= (decimal)Low)
                            keizoku = true;
                        else
                            keizoku = false;
                    }

                    // 中央値
                    var c = (High + Low) / 2;

                    // 中央判定が下げのとき
                    if (Close < c)
                    {
                        // 下げバーのとき、下げで継続していたら先に反転上げバーとBox追加
                        if (hanten)
                            AddBoxHigh(time, (double)NormalizeHigh(High));

                        // 下げバーのとき、継続ありだった場合
                        if (keizoku)
                            AddBoxLow(time, (double)NormalizeLow(Low));
                    }
                    // 下げバーで、中央判定が上げのとき
                    else if (Close >= c)
                    {
                        // 先に継続バーのBox追加
                        if (keizoku)
                            AddBoxLow(time, (double)NormalizeLow(Low));

                        // 後から反転バーとBox追加
                        if (hanten)
                            AddBoxHigh(time, (double)NormalizeHigh(High));
                    }

                    //if (TimeLine.Last().Bar.Count == 0)
                    if (this[this.Count - 1].Count == 0)
                    {
                        throw new Exception("Zero");
                    }
                }
                else
                {
                    if ((decimal)t.High + _box <= (decimal)High)
                        //AddBoxHigh(time, (double)NormalizeHigh(t.Bar.Last().High));
                        AddBoxHigh(time, (double)NormalizeHigh(t[t.Count - 1].High));

                    if ((decimal)t.Low - _box >= (decimal)Low)
                        //AddBoxLow(time, (double)NormalizeLow(t.Bar.Last().Low));
                        AddBoxLow(time, (double)NormalizeLow(t[t.Count - 1].Low));
                }
            }
            else
            {
                // 高値の剰余処理後
                decimal gh = NormalizeHigh(High);

                // 安値の剰余処理後
                decimal gl = NormalizeLow(Low);

                // 高安逆転した場合
                if (gh < gl)
                {
                    decimal tmp = gh;
                    gh = gl;
                    gl = tmp;

                    // 最初のバーを追加する
                    var t = new PFDataSet();
                    t.Add(new PFDataBox() { OpenTime = time, High = (double)gh, Low = (double)gl, Direction = 1 });
                    this.Add(t);

                }
                // 高安同一の場合
                else if (gh == gl)
                {
                    gh += (decimal)_box;

                    // 最初のバーを追加する
                    var t = new PFDataSet();
                    t.Add(new PFDataBox() { OpenTime = time, High = (double)(gh + _box), Low = (double)gl, Direction = 1 });
                    this.Add(t);

                }
                else if (Open < Close)
                {
                    // 最初のバーを追加する
                    var t = new PFDataSet();

                    var r = (double)gl;
                    for (; r + (double)_box <= High; r += (double)_box)
                        t.Add(new PFDataBox() { OpenTime = time, High = (double)gh, Low = (double)gl, Direction = 1 });
                }
                else
                {
                    // 最初のバーを追加する
                    var t = new PFDataSet();

                    var r = (double)gh;
                    for (; r - (double)_box >= Low; r -= (double)_box)
                        t.Add(new PFDataBox() { OpenTime = time, High = r, Low = r - (double)(_box), Direction = -1 });
                }

            }
        }
    }
}
