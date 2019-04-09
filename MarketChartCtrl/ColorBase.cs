using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MarketChartCtrl
{
    public abstract class ColorBase
    {
        public Color Color { get { return GetColor(); } set { SetColor(value); } }

        public static Color ColorFromCOLORREF(ulong color)
        {
            ulong r = color & 0x000000FF;
            ulong g = (color & 0x0000FF00) >> 8;
            ulong b = (color & 0x00FF0000) >> 16;
            return System.Drawing.Color.FromArgb((int)r, (int)g, (int)b);
        }

        internal abstract Color GetColor();
        internal abstract void SetColor(Color value);
    };

    public class ChartBrush : ColorBase
    {
        internal ChartBrush() { brush = null;  }
        internal SolidBrush brush;

        public void SetColor(int r, int g, int b)
        {
            SetColor(Color.FromArgb(r, g, b));
        }

        public void SetColor(ulong value)
        {
            SetColor(ColorFromCOLORREF(value));
        }

        internal override void SetColor(Color value)
        {
            if (brush != null)
                brush.Color = value;
            else
                brush = new SolidBrush(value);
        }

        internal override Color GetColor()
        {
            return brush.Color;
        }
    };

    public class ChartPen : ColorBase
    {
        internal ChartPen() { pen = null; }
        internal Pen pen;

        public void SetColor(int r, int g, int b)
        {
            SetColor(Color.FromArgb(r, g, b));
        }

        public void SetColor(ulong value)
        {
            SetColor(ColorFromCOLORREF(value));
        }

        internal override void SetColor(Color value)
        {
            if (pen != null)
                pen.Color = value;
            else
                pen = new Pen(value);
        }

        internal override Color GetColor()
        {
            return pen.Color;
        }
    };

    public class ChartPenBrush : ColorBase
    {
        internal ChartPenBrush() { pen = null; brush = null; }
        internal Pen pen;
        internal SolidBrush brush;
        internal SolidBrush xorbrush;

        public void SetColor(int r, int g, int b)
        {
            SetColor(Color.FromArgb(r, g, b));
        }

        public void SetColor(ulong value)
        {
            SetColor(ColorFromCOLORREF(value));
        }

        internal override void SetColor(Color value)
        {
            if (pen != null)
                pen.Color = value;
            else
                pen = new Pen(value);

            if (brush != null)
                brush.Color = value;
            else
                brush = new SolidBrush(value);

            // 反転色ブラシ
            xorbrush = new SolidBrush(Color.FromArgb(value.ToArgb() ^ 0xffffff));
        }

        internal override Color GetColor()
        {
            return brush.Color;
        }
    };

}
