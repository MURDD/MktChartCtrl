using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

namespace PFChartCtrl
{
    public class MouseSelectMode
    {
        private bool _dragging = false;

        private int MarkX = -1, MarkY = -1;
        private int CurrX = -1, CurrY = -1;

        public int x1 { get { return Math.Min(MarkX, CurrX); } }
        public int x2 { get { return Math.Max(MarkX, CurrX) - x1; } }

        public int y1 { get { return Math.Min(MarkY, CurrY); } }
        public int y2 { get { return Math.Max(MarkY, CurrY) - y1; } }

        public ChartScaling Scaling { get; set; }

        public void Initialize()
        {
            MarkX = -1; MarkY = -1;
            CurrX = -1; CurrY = -1;
        }

        public void SetTopBottom(int top, int bottom)
        {
            MarkY = top; //  frameController.GetMainChart().Top + 1;
            CurrY = bottom; //  frameController.GetMainChart().GraphRect.Height + 1;
        }

        public bool isDragging()
        {
            return _dragging;
        }

        public void MouseDown(MouseEventArgs e, int ScrollbarLeftValue)
        {
            _dragging = true;
            MarkX = e.X;

            MarkX = MarkX - (MarkX % Scaling.FullWidth) - Scaling.Center + 1;

            MarkY = e.Y;
        }

        public void MouseMove(MouseEventArgs e)
        {
            if (!_dragging) return;
            if (MarkX < 0) return;
            CurrX = e.X;

            CurrX = CurrX - (CurrX % Scaling.FullWidth) - Scaling.Center + 1;

            CurrY = e.Y;
        }

        public void MouseUp(MouseEventArgs e)
        {
            _dragging = false;
            CurrX = CurrX + 1;
        }
    }
}