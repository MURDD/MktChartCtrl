using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

namespace PFChartCtrl
{
    class DragMover
    {

        private bool _dragging = false;
        private int _dragStart;

        private int _LeftValueXWhenMosuDown;

        public bool isDragging()
        {
            return _dragging;
        }

        public void MouseDown(MouseEventArgs e, int ScrollbarLeftValue)
        {
            _dragging = true;
            _dragStart = e.X;
            _LeftValueXWhenMosuDown = ScrollbarLeftValue;
        }

        public int MouseMove(MouseEventArgs e)
        {
            return _LeftValueXWhenMosuDown - ( e.X - _dragStart );
        }

        public void MouseUp(MouseEventArgs e)
        {
            _dragging = false;
        }
    }
}
