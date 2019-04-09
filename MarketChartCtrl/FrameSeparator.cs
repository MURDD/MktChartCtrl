using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

namespace MarketChartCtrl
{
    public abstract class FrameSeparator : FrameSizing
    {
        private bool _SeparatorMove;
        private FrameView upper;
        private FrameView lower;
        private int startY;
        internal abstract void OnResize();

        internal FrameSeparator() : base()
        {
            upper = null;
            lower = null;

            _SeparatorMove = false;
        }

        internal bool SeparatorHitTest(int Y)
        {
            foreach(var el in _item)
            {
                int top = el.Top;

                if (top == 0)
                    continue;

                if (top - 6 < Y && Y < top + 3)
                    return true;
            }
            return false;
        }

        internal bool isSeparatorOn()
        {
            return _SeparatorMove;
        }

        internal void SeparatorOn(MouseEventArgs e)
        {
            if (_SeparatorMove)
                return;

            FrameView tmp = null;

            // 対象位置をここで保管格納する
            foreach(var el in _item)
            { 
                int top = el.Top;

                if (top > 0)
                {
                    if (top - 6 <= e.Y && e.Y <= top + 3)
                    {
                        _SeparatorMove = true;
                        upper = tmp;
                        lower = el;
                        startY = e.Y;
                        return;
                    }
                }

                // 上側として使用する可能性のフレーム
                tmp = el;
            }
        }

        internal void SeparatorDrag(int Y)
        {
            if (!_SeparatorMove)
                return;

            // 上部高さ限度チェック
            int rangeTop = upper.Top;
            int upHeightNew = Y - rangeTop;
            if (upHeightNew < 30)
                return;

            // 下部高さ限度チェック
            int rangeBtm = lower.Top + lower.Height;
            int loHeightNew = rangeBtm - (Y + 1);
            if (loHeightNew < 30)
                return;

            upper.Height = upHeightNew;
            int loTopNew = rangeTop + upHeightNew;

            lower.SetTopHeight(loTopNew, rangeBtm - loTopNew);
            OnResize();
        }

        internal void SeparatorOff(MouseEventArgs e)
        {
            _SeparatorMove = false;
            upper = null;
            lower = null;

            FrameRegulation();
        }

    }
}
