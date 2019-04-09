using System;
using System.Drawing;
using System.Windows.Forms;

namespace PFChartCtrl
{
    class DoubleBuffer : IDisposable
    {
        private BufferedGraphics _Buffer;
        private Control _Control;
        public Graphics Graphics { get { return _Buffer.Graphics; } }

        public DoubleBuffer(Control control)
        {
            _Control = control;

            // This example assumes the existence of a form called control.
            System.Drawing.BufferedGraphicsContext currentContext;

            // Gets a reference to the current BufferedGraphicsContext
            currentContext = BufferedGraphicsManager.Current;

            // Creates a BufferedGraphics instance associated with control, and with 
            // dimensions the same size as the drawing surface of control.
            _Buffer = currentContext.Allocate(control.CreateGraphics(), control.DisplayRectangle);

            _Control.Paint += new System.Windows.Forms.PaintEventHandler(this.Paint);
        }

        ~DoubleBuffer()
        {
            Dispose();
        }

        // Dispose = デストラクタ：~DoubleBuffer()
        // デストラクタでマネージドなリソースを解放する
        public void Dispose()
        {
            //_Buffer?.Dispose();
            if (_Buffer != null)
                _Buffer.Dispose();
            _Buffer = null;
            _Control.Paint -= new System.Windows.Forms.PaintEventHandler(this.Paint);
        }

        private void Paint(Object sender, PaintEventArgs e)
        {
            Refresh();
        }

        public void Refresh()
        {
            //_Buffer?.Render();
            if (_Buffer != null)
                _Buffer.Render();
        }
    }
}
