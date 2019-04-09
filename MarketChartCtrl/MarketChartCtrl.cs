using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using MarketDataSeries;

namespace MarketChartCtrl
{
    //[LicenseProvider(typeof(LicFileLicenseProvider))]
    public class MarketChartCtrl : Control, IDisposable
    {
        //private License license = null;

        // 隠蔽する
        [BrowsableAttribute(false), EditorBrowsable(EditorBrowsableState.Never)]
        private new Image BackgroundImage { get; set; }

        private bool _mouseSelectMode = false;
        public bool MouseSelectMode
        {
            get { return _mouseSelectMode; }
            set
            {
                _mouseSelectMode = value;
                if (!value)
                {
                    //_mSelect.Initialize();
                    Refresh();
                }
            }
        }

        public bool ShowGridLine
        {
            get
            {
                return frameControler.ShowGridLine;
            }
            set
            {
                frameControler.ShowGridLine = value;
                Refresh();

            }
        }

        public bool IndicatorDrawOuter
        {
            get
            {
                return frameControler.IndicatorDrawOuter;
            }
            set
            {
                frameControler.IndicatorDrawOuter = value;
                Refresh();

            }
        }

        public IPositions Positions { get; set; }
        public IPendingOrders PendingOrders { get; set; }

        private MarketData _dataSource;
        public MarketData DataSource
        {
            get
            {
                return _dataSource;
            }
            set
            {

                _dataSource = value;
                if (value == null)
                {
                    //_dataSource.OpenTime = null;
                    //_dataSource.Open = null;
                    //_dataSource.High = null;
                    //_dataSource.Low = null;
                    //_dataSource.Close = null;
                    hScrollBar.Maximum = 0;
                    return;
                }

                // 
                FitLast();

                Refresh();
            }
        }

        public void FitLast(bool move = true)
        {
            int emp = frameControler.ViewMaxBarCount - 20;

            if (_dataSource == null)
                return;

            if (_dataSource.OpenTime.Count > emp)
                hScrollBar.Maximum = _dataSource.OpenTime.Count - emp;
            else
                hScrollBar.Maximum = 0;

            if (move)
                hScrollBar.Value = hScrollBar.Maximum;

        }

        private int _ViewPos;
        private int _DataPos;
        public int ViewPos { get { return _ViewPos; } }
        public int DataPos { get { return _DataPos; } }
        public int ScrollPos { get { return hScrollBar.Value; } }

        public int ElementSize { get { return Scaling.Size; } }
        public int ElementMargin { get { return Scaling.Margin; } }

        public int MaxBarNumber { get { return frameControler.ViewMaxBarCount; } }

        private FrameControler frameControler;
        public FrameControler Frames { get { return frameControler; } }

        internal DoubleBuffer myDoubleBuffer;
        private DragMover dragMover;
        private MouseSelectMode _mSelect = new MouseSelectMode();
        //public MouseSelectMode MouseSelecter => _mSelect;
        public MouseSelectMode MouseSelecter
        {
            get
            {
                return _mSelect;
            }
        }

        private int intialResizeStopper;

        public MarketChartCtrl()
        {
            //license = LicenseManager.Validate(typeof(MarketChartCtrl), this);

            InitializeControls();
        }

        ~MarketChartCtrl()
        {
            //if (license != null)
            //{
            //    license.Dispose();
            //    license = null;
            //}
        }

        public new Font Font
        {
            get
            {
                return frameControler.ViewFont;
            }
            set
            {
                Font bkFont = frameControler.ViewFont;
                frameControler.ViewFont = value;
                if (bkFont != value)
                    OnFontChanged(new EventArgs());
            }
        }

        public ChartColors Colors { get { return frameControler.Colors; } }
        public ChartScaling Scaling { get { return frameControler.Scaling; } }

        public double PriceTickSize { get { return frameControler.TickSize; } set { frameControler.TickSize = value; Refresh(); } }
        public double ViewPriceTickSize { get { return frameControler.ViewTickSize; } set { frameControler.ViewTickSize = value; Refresh(); } }
        public string PriceFormat { get { return frameControler.Format; } set { frameControler.Format = value; Refresh();  } }
        public void SetPriceFormat(int nInt, int ndec)
        {
            if (ndec > 0)
                frameControler.Format = new String('0', nInt) + "." + new String('0', ndec);
            else
                frameControler.Format = new String('0', nInt);
            Refresh();
        }

        public bool ShowLeftPrice
        {
            get
            {
                return frameControler.ShowLeftNumber;
            }
            set
            {
                frameControler.ShowLeftNumber = value;
                frameControler.OnResize();
                Refresh();
            }
        }

        public BarStyle BarStyle
        {
            get
            {
                return frameControler.BarStyle;
            }
            set
            {
                frameControler.BarStyle = value;
                //frameControler.OnResize();
                Refresh();
            }
        }

        private void InitializeControls()
        {
            intialResizeStopper = 0;
            Width = 1;
            Height = 1;

            // 最初に作成する
            myDoubleBuffer = new DoubleBuffer(this);
            dragMover = new DragMover();

            this.hScrollBar = new ChartHScrollBar();
            this.hScrollBar.Top = this.Height - this.hScrollBar.Height;
            this.hScrollBar.Left = 0;
            this.hScrollBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            this.hScrollBar.Width = this.Width - 55;

            this.Controls.Add(this.hScrollBar);

            this.hScrollBar.Scroll += this.HandleScrollBarScroll;

            frameControler = new FrameControler();
            frameControler.Parent = this;
            frameControler.MeasureStrAreatWidth();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (intialResizeStopper < 1)
            {
                // 最初のwidth, 1回をするー
                intialResizeStopper++;
                return;
            }

            // リサイズ時は再取得する

            if (myDoubleBuffer != null)
                myDoubleBuffer.Dispose();

            myDoubleBuffer = new DoubleBuffer(this);

            if (frameControler != null)
            {
                frameControler.SetSize(this.Width, this.Height - hScrollBar.Height);
                //frameControler.CalcBarsView();
                frameControler.OnResize();
            }

            if (hScrollBar != null && _dataSource != null && _dataSource.OpenTime.Count > 0)
            {
                FitLast();
            }

            Refresh();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            // mouse wheel events only arrive at the parent control
            if (this.Parent != null)
            {
                this.Parent.MouseWheel -= this.HandleMouseWheel;
            }
            base.OnParentChanged(e);
            if (this.Parent != null)
            {
                this.Parent.MouseWheel += this.HandleMouseWheel;
            }
        }

        #region HScrollBar

        private class ChartHScrollBar : HScrollBar
        {
            protected override void OnValueChanged(EventArgs e)
            {
                base.OnValueChanged(e);
                // setting the scroll position programmatically shall raise Scroll
                this.OnScroll(new ScrollEventArgs(ScrollEventType.EndScroll, this.Value));

                //System.Diagnostics.Debug.WriteLine(this.Value.ToString());
            }
        }

        private ScrollBar hScrollBar;

        internal ScrollBar HScrollBar
        {
            get { return this.hScrollBar; }
        }

        public event ScrollEventHandler Scroll;

        protected virtual void OnScroll(ScrollEventArgs e)
        {
            var handler = this.Scroll;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Event handler for the Scroll event of either scroll bar.
        /// </summary>
        private void HandleScrollBarScroll(object sender, ScrollEventArgs e)
        {
            OnScroll(e);

            this.AutoScrollOffset = new System.Drawing.Point(-this.HScrollBar.Value, 0);

            // ダブルバッファリング時の書き換えはRefreshを使うこと
            this.Refresh();
        }

        #endregion

        #region MouseWheel

        private void HandleMouseWheel(object sender, MouseEventArgs e)
        {
            this.HandleMouseWheel(e);
        }

        /// <summary>
        /// Specifies how the control reacts to mouse wheel events.
        /// Can be overridden to adjust the scroll speed with the mouse wheel.
        /// </summary>
        protected virtual void HandleMouseWheel(MouseEventArgs e)
        {
            // The scroll difference is calculated so that with the default system setting
            // of 3 lines per scroll incremenet,
            // one scroll will offset the scroll bar value by LargeChange / 4
            // i.e. a quarter of the thumb size
            ScrollBar scrollBar;
            scrollBar = this.HScrollBar;

            var minimum = 0;
            var maximum = scrollBar.Maximum - scrollBar.LargeChange + 1;
            if (maximum <= 0)
            {
                // happens when the entire area is visible
                return;
            }
            var value = scrollBar.Value - (int)(e.Delta * scrollBar.LargeChange / (120.0 * 12.0 / SystemInformation.MouseWheelScrollLines));
            scrollBar.Value = Math.Min(Math.Max(value, minimum), maximum);
        }

        #endregion

        #region MouseAction

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            // マウス位置により動作を分別
            if (frameControler.SeparatorHitTest(e.Y))
            {
                // Mouse Separator Mode
                this.Cursor = Cursors.HSplit;
                frameControler.SeparatorOn(e);
            }
            else
            {
                this.Cursor = Cursors.Default;

                if (_mouseSelectMode)
                {
                    // Mouse Select Mode
                    _mSelect.Initialize();
                    _mSelect.MouseDown(e, hScrollBar.Value);
                }
                else
                if (!hScrollBar.Focused)
                {
                    // Mouse Drag Mode
                    dragMover.MouseDown(e, hScrollBar.Value);
                }
            }
            frameControler.SeparatorOn(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            // 無条件に初期化する
            frameControler.SeparatorOff(e);

            if (_mouseSelectMode)
            {
                // Mouse Select Mode
                _mSelect.MouseUp(e);
            }
            else
            {
                // Mouse Drag Mode
                dragMover.MouseUp(e);
            }

            // マウス位置により動作を分別
            if (frameControler.SeparatorHitTest(e.Y))
            {
                this.Cursor = Cursors.HSplit;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // マウス位置により動作を分別
            if (frameControler.SeparatorHitTest(e.Y) || frameControler.isSeparatorOn())
                this.Cursor = Cursors.HSplit;
            else
                this.Cursor = Cursors.Default;

            if (frameControler.isSeparatorOn())
            {
                // Mouse Separator Mode
                frameControler.SeparatorDrag(e.Y);
                Refresh();
            }
            else
            {
                if (!_mouseSelectMode)
                {
                    // Mouse Drag Mode
                    if (dragMover.isDragging())
                    {
                        ScrollBar scrollBar = this.HScrollBar;

                        int maximum = scrollBar.Maximum - scrollBar.LargeChange;
                        if (maximum <= 0)
                        {
                            // happens when the entire area is visible
                            return;
                        }

                        // direction changer
                        int dragw = dragMover.MouseMove(e);

                        if (dragw < hScrollBar.Minimum)
                            dragw = hScrollBar.Minimum;

                        // hScrollBar.Maximum - hScrollBar.LargeChange が右端の左側最大位置
                        if (dragw >= hScrollBar.Maximum - hScrollBar.LargeChange)
                            dragw = hScrollBar.Maximum - hScrollBar.LargeChange + 1;

                        hScrollBar.Value = dragw;

                        // 3747
                        //System.Diagnostics.Debug.WriteLine(hScrollBar.Value.ToString());

                        return;
                    }
                    else
                    {
                        //XYLabelString = "X:" + e.X.ToString() + ", Y:" + e.Y.ToString() + ", ScrollPos:"+ hScrollBar.Value.ToString();

                        if (Scaling.FullWidth > 0)
                        {
                            // FrameControler の開始位置設定で加算した (Scaling.Margin + 1) を X軸から 引いてから計算すること
                            int startPoint = frameControler.GetMainChart().GraphRect.Left + Scaling.Margin;

                            //_ViewPos = (int)Math.Truncate((decimal)(e.X - (Scaling.Margin + 1)) / Scaling.FullWidth);
                            _ViewPos = (int)Math.Truncate((decimal)(e.X - (startPoint)) / Scaling.FullWidth);
                            _DataPos = hScrollBar.Value + ViewPos;
                        }
                    }
                }
                else
                {
                    // Mouse Select Mode
                    _mSelect.MouseMove(e);
                    Refresh();

                    if (Scaling.FullWidth > 0)
                    {
                        // FrameControler の開始位置設定で加算した (Scaling.Margin + 1) を X軸から 引いてから計算すること
                        int startPoint = frameControler.GetMainChart().GraphRect.Left + Scaling.Margin;

                        //_ViewPos = (int)Math.Truncate((decimal)(e.X - (Scaling.Margin + 1)) / Scaling.FullWidth);
                        _ViewPos = (int)Math.Truncate((decimal)(e.X - (startPoint)) / Scaling.FullWidth);
                        _DataPos = hScrollBar.Value + ViewPos;
                    }
                }
            }
        }

        #endregion

        #region Drawing

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            Refresh();
        }

        public override void Refresh()
        {
            if (frameControler == null)
                return;

            if (myDoubleBuffer == null)
                return;

            if (hScrollBar.Value >= 0)
                frameControler.Paint(myDoubleBuffer.Graphics, hScrollBar.Value);

            if (_mouseSelectMode)
            {
                _mSelect.SetTopBottom(frameControler.GetMainChart().Top + 1, frameControler.GetMainChart().GraphRect.Height + 1);
                _mSelect.Scaling = frameControler.Scaling;

                Pen penGray = new Pen(Color.Gray, 1) { DashStyle = DashStyle.Dot };
                myDoubleBuffer.Graphics.DrawRectangle(penGray, _mSelect.x1, _mSelect.y1, _mSelect.x2, _mSelect.y2);
                penGray.Dispose();
            }

            myDoubleBuffer.Refresh();
        }

        void SetIndexLeft(int value)
        {
            //if (OpenTime == null)
            //    return;

            //if (OpenTime->Count < 1)
            //    return;

            //visibleSeriesCount = frameControler->GetDrawAreaWidth() / frameControler->GetElementWidth();

            //if (frameControler->GetElementWidth() > 0)
            //{
            //	if (Series.Count() >= visibleSeriesCount)
            //		shScrollBar.Value = (Series.Count() - _visibleSeriesCount + 18) * renderer.DrawWidth;  //最大値の設定 
            //}
            //else
            //	shScrollBar.Value = 0;
        }

        #endregion

        public void UpdateBar(DateTime openTime, double open, double high, double low, double close, double vol)
        {
            if (_dataSource == null)
                return;

            int idx = _dataSource.OpenTime.Count - 1;
            for (; idx > 0; idx--)
            {
                if (_dataSource.OpenTime[idx].CompareTo(openTime) == 0)
                {
                    break;
                }
                else if (_dataSource.OpenTime.Last().CompareTo(openTime) < 0)
                {
                    idx = -1;
                    break;
                }
                else if (_dataSource.OpenTime[idx].CompareTo(openTime) < 0)
                {
                    idx = -2;
                    break;
                }
            }

            if (_dataSource.Open.Count > _dataSource.OpenTime.Count)
                ((DataSeries)_dataSource.Open)._item.RemoveRange(_dataSource.OpenTime.Count, _dataSource.Open.Count - _dataSource.OpenTime.Count);

            if (_dataSource.High.Count > _dataSource.OpenTime.Count)
                ((DataSeries)_dataSource.High)._item.RemoveRange(_dataSource.OpenTime.Count, _dataSource.High.Count - _dataSource.OpenTime.Count);

            if (_dataSource.Low.Count > _dataSource.OpenTime.Count)
                ((DataSeries)_dataSource.Low)._item.RemoveRange(_dataSource.OpenTime.Count, _dataSource.Low.Count - _dataSource.OpenTime.Count);

            if (_dataSource.Close.Count > _dataSource.OpenTime.Count)
                ((DataSeries)_dataSource.Close)._item.RemoveRange(_dataSource.OpenTime.Count, _dataSource.Close.Count - _dataSource.OpenTime.Count);

            if (_dataSource.Volume.Count > _dataSource.OpenTime.Count)
                ((DataSeries)_dataSource.Volume)._item.RemoveRange(_dataSource.OpenTime.Count, _dataSource.Volume.Count - _dataSource.OpenTime.Count);

            if (idx > -2)
            {
                //if (idx.Count() > 1)
                if (idx >= 0)
                {
                    //int ix = idx.First();
                    _dataSource.Open[idx] = open;
                    _dataSource.High[idx] = high;
                    _dataSource.Low[idx] = low;
                    _dataSource.Close[idx] = close;
                    _dataSource.Volume[idx] = vol;
                }
                else
                {
                    _dataSource.OpenTime.Add(openTime);
                    _dataSource.Open.Add(open);
                    _dataSource.High.Add(high);
                    _dataSource.Low.Add(low);
                    _dataSource.Close.Add(close);
                    _dataSource.Volume.Add(vol);
                }
            }

            FitLast();

            Refresh();
            Application.DoEvents();
        }

        #region Framing

        public void Tick(DateTime openTime, double ask, double bid)
        {
            if (_dataSource == null)
                return;

            _dataSource.Ask = ask;
            _dataSource.Bid = bid;

            int i = _dataSource.OpenTime.Count - 1;

            FitLast();

            Refresh();
            Application.DoEvents();
        }

        //public void SetSeries(cAlgo.API.TimeSeries opentime, cAlgo.API.DataSeries open, cAlgo.API.DataSeries high, cAlgo.API.DataSeries low, cAlgo.API.DataSeries close)
        //{
        //    MarketDataSeries dataSource = new MarketDataSeries();
        //    dataSource.OpenTime = new TimeSeries();
        //    dataSource.Open = new DataSeries();
        //    dataSource.High = new DataSeries();
        //    dataSource.Low = new DataSeries();
        //    dataSource.Close = new DataSeries();

        //    dataSource.OpenTime.AlgoData = opentime;
        //    dataSource.Open.AlgoData = open;
        //    dataSource.High.AlgoData = high;
        //    dataSource.Low.AlgoData = low;
        //    dataSource.Close.AlgoData = close;

        //    int imax = Math.Max(dataSource.OpenTime.Count,
        //               Math.Max(dataSource.Open.Count,
        //               Math.Max(dataSource.High.Count,
        //               Math.Max(dataSource.Low.Count,
        //                        dataSource.Close.Count)))) - 1;

        //    _dataSource = dataSource;

        //    // バーの最大本数からスクロールバーの最大値を計算する
        //    //int  fcw = frameControler.GetElementWidth();
        //    hScrollBar.Maximum = imax;

        //    Refresh();
        //}

        public void Clear()
        {
            frameControler.Clear();
            //frameControler.OpenTime = null;
            //frameControler.Open = null;
            //frameControler.High = null;
            //frameControler.Low = null;
            //frameControler.Close = null;
            hScrollBar.Maximum = 0;

            Refresh();
        }

        #endregion
    }
}
