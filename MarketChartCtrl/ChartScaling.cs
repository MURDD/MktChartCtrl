using System;
using System.Collections.Generic;
using System.Text;

namespace MarketChartCtrl
{
    public class ChartScaling
    {
    	private int _Size;
        private int _Margin;
        private int _Width;
        private int _Center;
        private FrameControler Parent;
        private int _fullWidth;

        public int FullWidth { get { return _fullWidth; } }

        internal ChartScaling(FrameControler parent)
        {
            Parent = parent;
            _Size = 1;
	        _Margin = 1;

            UpSize();
            DownSize();

            _fullWidth = (_Margin + _Width);

            FromSize();
        }

        public void UpSize()
        {
            if (_Size < 0)
                _Size = 1;
            else
                _Size++;
            FromSize();
            _fullWidth = (_Margin + _Width);
        }

        public void DownSize()
        {
            _Size--;
            if (_Size < 1)
                _Size = 1;
            FromSize();
            _fullWidth = (_Margin + _Width);
        }

        internal void FromSize()
        {
            _Width = _Size * 2 + 1;
            _Center = _Size + 1;
        }

        public int Margin
        {
            get
            {
                return _Margin;
            }
            set
            {
                if (value > 0)
                {
                    _Margin = value;

                    //System.Diagnostics.Debug.WriteLine("_Size : " + _Size.ToString() +  ",  _Margin : " + _Margin.ToString());

                    Parent.OnResize();

                    Parent.DrawChart();
                    _fullWidth = (_Margin + _Width);

                    Parent.Parent.FitLast(false);
                }
            }
        }

        public int Size
        {
            get
            {
                return _Size;
            }
            set
            {
                if (value < 1)
                    value = 1;

                _Size = value;

                //System.Diagnostics.Debug.WriteLine("_Size : " + _Size.ToString() + ",  _Margin : " + _Margin.ToString());

                // OnResize より先に FromSize を実行しないと、ワンテンポ遅れてサイズが反映されることになっていた
                FromSize();

                Parent.OnResize();
                Parent.DrawChart();
                _fullWidth = (Margin + _Width);

                Parent.Parent.FitLast(false);
            }
        }

        // 指定幅に表示できるバーの本数を返す
        internal int GetXMaxCount(int width)
        {
            return (int)System.Math.Truncate(1.0 * width / (_Margin + _Width));
        }

        // 指定バーの描画開始位置を返す
        internal int GetLeftX(int width, int index)
        {
            if (GetXMaxCount(width) < index)
                return -1;

            index++;

            int x = index * (_Margin + _Width) - _Width;

            return x;
        }

        public int Width { get{ return _Width; } }
        internal int Center { get{ return _Center; } }
    }
}
