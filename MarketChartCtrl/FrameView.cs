using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using MarketChartCtrl.IndicatorRenderItem;

namespace MarketChartCtrl
{
    internal struct ValueMaxMinRange
    {
        internal double valueMax;
        internal double valueMin;
        internal double Height;
    }

    public abstract class FrameView : IEnumerable<IndicatorRender>
    {
        //****************************************************************

        internal abstract ValueMaxMinRange GetMaxMinRange(int start, int count);
        internal abstract void Paint(System.Drawing.Graphics graphics);
        internal abstract void DrawMeasure(System.Drawing.Graphics graphics, ValueMaxMinRange range, int fontHeight);
        internal abstract void Paint(System.Drawing.Graphics graphics, int idx, int startPoint, int graphIdx, ref List<int> TimeLines);

        #region IndicatorMethod

        protected List<IndicatorRender> _item = new List<IndicatorRender>();

        public string Comment { get; set; }

        public IndicatorRender this[int index]
        {
            get
            {
                return _item[index];
            }
        }

        public int Count { get { return _item.Count; } }

        public IEnumerator<IndicatorRender> GetEnumerator()
        {
            foreach (var elm in _item) yield return elm;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void Add(params IndicatorRender[] rdrs)
        {
            if (rdrs == null)
                return;

            for (int i = 0; i < rdrs.Length; i++)
            {
                _item.Add(rdrs[i]);
            }
        }

        public void Add(IndicatorRender rdrs)
        {
            if (rdrs == null)
                return;

            _item.Add(rdrs);
            Parent.Parent.Refresh();
        }

        public int IndexOf(string name)
        {
            if (name == null)
                return -1;

            int ic = -1;
            for (int i = 0; i < _item.Count; i++)
                if (_item[i].Name == name)
                    ic = i;

            if (ic > -1)
            {
                return ic;
            }
            return -1;
        }

        public bool Remove(IndicatorRender item)
        {
            if (item == null)
                return false;

            bool r = _item.Remove(item);
            Parent.Parent.Refresh();
            return r;
        }

        public bool Remove(string name)
        {
            if (name == null)
                return false;

            int ic = -1;
            for (int i = 0; i < _item.Count; i++)
                if (_item[i].Name == name)
                    ic = i;

            if (ic > -1)
            {
                _item.RemoveAt(ic);
                Parent.Parent.Refresh();
                return true;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            if (index < 0)
                return;

            _item.RemoveAt(index);
            Parent.Parent.Refresh();
        }

        public void Clear()
        {
            _item.Clear();
        }

        #endregion

        // 上下毎のマージン
        private const int MarginTopBottom = 4;

        public bool isMainFrame { get { return _main; } }
        internal double TickSize { get; set; }
        internal double ViewTickSize { get; set; }
        public ChartColors Colors;

        internal ChartScaling scaling;

        private bool _main;

        protected int _width;
        protected int _height;
        protected int _top;

        private string _strFormat;
        internal string NumberFormat { get { return _strFormat; } set { _strFormat = value; } }

        protected Rectangle graphRect;
        internal Rectangle GraphRect { get { return graphRect; } }

        private ChartScaling _scaling;
        protected ChartColors _colors;

        internal FrameControler Parent { get; set; }

        internal FrameView(bool main, ChartScaling scaling, ChartColors colors, FrameControler parent)
        {
            _main = main;
            _scaling = scaling;
            _colors = colors;
            Parent = parent;
        }

        internal int Width { get { return _width; } }
        internal int Height 
        {
            get 
            {
                return _height; 
            } 
            set 
            {
                _height = value; 
            }
        }
        internal int Top { get { return _top; } set { _top = value; } }

        internal void SetTopHeight(int top, int height)
        {
            Top = top;
            Height = height;
        }

        internal void SetSize(int width, int height, int top)
        {
            _width = width;
            _height = height;
            _top = top;
        }

        internal void PreSetSize(int width, int height)
        {
            _width = width;
            _height = height;
        }

        protected int fntHalf;
        internal int topText;
        internal int btmText;
        internal int pxHeight;
        internal void OnResize(int fontHeight)
        {
            fntHalf = (int)Math.Ceiling((double)fontHeight / 2);

            //****************************************************************
            // 目盛りのちょい線位置計算
            int tpLine = _top + 1;
            int bmLine = _top + _height - 1;

            // メインビューは下部日時TEXT領域のフォント高さを引く
            if (_main)
                bmLine -= fontHeight;

            topText = tpLine;
            btmText = bmLine - fontHeight;

            //****************************************************************
            // 物理デバイスでの描画域の高さ (上下2 pixel分を引く)
            pxHeight = bmLine - tpLine - (MarginTopBottom * 2);

            int leftMargin = 1;
            if (Parent.ShowLeftNumber)
                leftMargin = (int)Parent.NumberStrSize.Width;

            graphRect = new Rectangle(leftMargin, tpLine, _width - (int)Parent.NumberStrSize.Width - leftMargin, bmLine - tpLine);
        }

        //****************************************************************
        // 画面上の縦位置変換計算ルーチン
        protected double btmValue;
        protected double liveUnit;
        internal int ValueToViewYPosition(double value)
        {
            // 入力された値(価格など)から一番下の基準値(価格など)を引いてRectトップからの位置を計算する
            double tp = value - btmValue;
            if (liveUnit == 0) return 0;
            int liveWire = (int)Math.Round(tp / TickSize * liveUnit, 0, MidpointRounding.AwayFromZero);
            return graphRect.Bottom - liveWire - MarginTopBottom;
        }
        //****************************************************************

            
    }
}
