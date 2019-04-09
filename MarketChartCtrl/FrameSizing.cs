using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MarketChartCtrl
{
    public abstract class FrameSizing : FrameControlerBase
    {
        private List<double> _szHight; // フレームの高さ比率格納バッファ

        protected int _Width;
        protected int _Height;
        internal int Width { get { return _Width; } set { SetSize(value, _Height); } }
        internal int Height { get { return _Height; } set { SetSize(_Width, value); } }

        internal FrameSizing()
        {
            _Width = 0;
            _Height = 0;
            //_flames = new List<FrameView>();
            _szHight = new List<double>();
        }

        internal void SetSize(int width, int height)
        {
            // MainChartのみの場合
            if (_item.Count == 1)
            {
                _Width = width;
                _Height = height;

                _item[0].SetSize(width, height, 0);

                return;
            }

            // Resizeイベントからの場合、更新前の高さ調査

            // 古い高さを新しい高さに変更
            _Width = width;
            _Height = height;

            int isum = 0;
            int i = 0;
            foreach (var el in _item)
            {
                int h = (int)(_Height * _szHight[i]);
                el.SetSize(_Width, h, isum);
                isum += h;
                ++i;
            }
        }

        internal void FrameRegulation()
        {
            // 通常は全体の高さ調整を実行する
            int isum = 0;          // 累積高さ
            List<int> sz1 = new List<int>();  // 高さ配列
            foreach (var el in _item)
            {
                int h = el.Height;
                isum += h;         // 累積加算
                sz1.Add(h);  // 配列格納
            }

            // 高さ比率計算
            _szHight.Clear();
            double hsum = 0.0;
            foreach (int el in sz1)
            {
                double h = (double)el / (double)isum;
                hsum += h;
                _szHight.Add(h);
            }

            // サイズ率で高さを割り当てる
            int i = 0;             // 添え字
            isum = 0;
            int main = -1;         // mainフレームの場所
            int maintop = 0;
            foreach (var el in _item)
            {
                int szNewHeight = (int)((1.0 * _Height) * _szHight[i]);

                //// 前後同値の場合、調整する
                //if (el->GetHeight() == szNewHeight)
                //	if (dirSize > 0)
                //		szNewHeight += 1;
                //	else
                //		if (dirSize < 0)
                //			szNewHeight -= 1;

                el.SetSize(_Width, szNewHeight, isum);

                if (el.isMainFrame)
                {
                    maintop = isum;
                    main = i;
                }
                isum += szNewHeight;
                ++i;
            }

            if (main >= 0)
                _item[main].SetSize(_Width, _item[main].Height + (_Height - isum), maintop);

        }
    }
}
