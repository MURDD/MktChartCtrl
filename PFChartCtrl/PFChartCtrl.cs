using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using MarketDataSeries;
using MarketDataSeries.PFPatterns;

namespace PFChartCtrl
{
    public class PFChartCtrl : Control, IDisposable
    {
        // 隠蔽する
        [BrowsableAttribute(false), EditorBrowsable(EditorBrowsableState.Never)]
        private new Image BackgroundImage { get; set; }

        private PFDataSeries _dataSource;
        public PFDataSeries PFDataSource 
        {
            get 
            {
                return _dataSource;
            }
            set 
            {
                _dataSource = value;
                if (null != _dataSource)
                {
                    if (mktDataSource != null && _dataSource.DataSource == null)
                        _dataSource.DataSource = mktDataSource;
                    frameController.SetDataSource(_dataSource);
                }
            }
        }

        public int HScrollbarHeight { get { return this.hScrollBar.Height; } }

        private MarketData mktDataSource = null;
        public MarketData DataSource
        {
            get
            {
                return _dataSource != null ? _dataSource.DataSource : null;
            }
            set
            {
                mktDataSource = value;
                if (value == null)
                {
                    if (_dataSource != null)
                       _dataSource.DataSource = null;
                    return;
                }

                if (_dataSource != null)
                    _dataSource.DataSource = null;

                _dataSource = null;
                GC.Collect();

                hScrollBar.Maximum = 0;

                //_dataSource = new PFDataSeries(_boxPipSize, _reversalBox, _pipSize);
                if (_dataSource == null)
                    return;

                _dataSource.DataSource = value;
                frameController.SetDataSource(_dataSource);

                // 
                FitLast();

                Refresh();
            }
        }

        public void SetPFPatterns(List<string> pfPatterns)
        {
            foreach (var p in _dataSource.PatternTypes)
                p.Visible = pfPatterns.Contains(p.PatternName);

            Refresh();
        }

        public IPositions Positions { get; set; }
        public IPendingOrders PendingOrders { get; set; }

        public bool ShowGridLine
        {
            get
            {
                return frameController.GetMainChart().ShowGridLine;
            }
            set
            {
                frameController.GetMainChart().ShowGridLine = value;
                Refresh();

            }
        }

        public bool ShowLine
        {
            get
            {
                return frameController.GetMainChart().ShowLine;
            }
            set
            {
                frameController.GetMainChart().ShowLine = value;
                Refresh();

            }
        }

        public void FitLast(bool move = true)
        {
            frameController.UpdateViewMaxBars();
            int emp = frameController.ViewMaxBarCount - 15;

            if (null == _dataSource)
                return;

            if (_dataSource.Count > emp)
                hScrollBar.Maximum = _dataSource.Count - emp;
            else
                hScrollBar.Maximum = 0;

            if (move)
                hScrollBar.Value = hScrollBar.Maximum;
        }

        public bool BarStyle
        {
            get
            {
                return frameController.GetMainChart().BarStyle;
            }
            set
            {
                frameController.GetMainChart().BarStyle = value;
                Refresh();
            }
        }

        // 銘柄
        public string Symbol { get { return DataSource.Symbol; } }

        // 元データの時間サイズ
        public TimeFrame TimeFrame { get { return DataSource.TimeFrame; } }

        // 転換枠数 (default 3 box)
        private int _reversalBox = 0;
        public int ReversalBox
        {
            get
            {
                return _dataSource != null ? _dataSource.ReversalBox : _reversalBox;
            }
            set
            {
                if (_dataSource == null)
                    _reversalBox = value;
                else
                {
                    _dataSource.ReversalBox = value;
                    //hScrollBar.Maximum = _dataSource.Count - 1;
                    AdjustScrollMaximum();
                    FitLast();
                    Refresh();
                }
            }
        }

        // 枠サイズ (default 5 pips)
        private int _boxPipSize = 0;
        public int BoxPipSize
        {
            get
            {
                return _dataSource != null ? _dataSource.BoxPipSize : _boxPipSize;
            }
            set
            {
                if (_dataSource == null)
                    _boxPipSize = value;
                else
                {
                    _dataSource.BoxPipSize = value;
                    //hScrollBar.Maximum = _dataSource.Count - 1;
                    AdjustScrollMaximum();
                    _box = _dataSource.BoxPipSize * _dataSource.PipSize;
                    FitLast();
                    Refresh();
                }
            }
        }

        // PipSize (ex. 0.01, 0.0001)
        private double _pipSize = 0;
        public double PipSize
        {
            get
            {
                return _dataSource != null ? _dataSource.PipSize : _pipSize;
            }
            set
            {
                if (_dataSource == null)
                    _pipSize = value; else { _dataSource.PipSize = value;
                    //hScrollBar.Maximum = _dataSource.Count - 1;
                    AdjustScrollMaximum();
                    _box = _dataSource.BoxPipSize * _dataSource.PipSize;
                    Refresh();
                }
            }
        }

        private double _box = 0;

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

        private int _ViewPos;
        private double _ViewPrice;
        private int _DataPos;
        public int ViewPos { get { return _ViewPos; } }
        public double ViewPeice { get { return _ViewPrice; } }
        public int DataPos { get { return _DataPos; } }
        public int ScrollPos { get { return hScrollBar.Value; } }

        public int ElementSize { get { return Scaling.Size; } }
        public int ElementMargin { get { return Scaling.Margin; } }

        public int MaxBarNumber { get { return frameController.ViewMaxBarCount; } }

        private FrameController frameController;
        public FrameController Frames { get { return frameController; } }

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

        public PFChartCtrl()
        {
            //license = LicenseManager.Validate(typeof(MarketChartCtrl), this);

            InitializeControls();
        }

        ~PFChartCtrl()
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
                return frameController.ViewFont;
            }
            set
            {
                Font bkFont = frameController.ViewFont;
                frameController.ViewFont = value;
                if (bkFont != value)
                    OnFontChanged(new EventArgs());
            }
        }

        public ChartColors Colors { get { return frameController.Colors; } }
        public ChartScaling Scaling { get { return frameController.Scaling; } }

        public double PriceTickSize { get { return frameController.TickSize; } set { frameController.TickSize = value; Refresh(); } }
        public double ViewPriceTickSize { get { return frameController.ViewTickSize; } set { frameController.ViewTickSize = value; Refresh(); } }
        public string PriceFormat { get { return frameController.Format; } set { frameController.Format = value; Refresh(); } }
        public void SetPriceFormat(int nInt, int ndec)
        {
            if (ndec > 0)
                frameController.Format = new String('0', nInt) + "." + new String('0', ndec);
            else
                frameController.Format = new String('0', nInt);
            Refresh();
        }

        public bool ShowLeftPrice
        {
            get
            {
                return frameController.ShowLeftNumber;
            }
            set
            {
                frameController.ShowLeftNumber = value;
                frameController.OnResize();
                AdjustScrollMaximum();
                Refresh();
            }
        }

        private void AdjustScrollMaximum()
        {
            #region スクロール端の調整
            int emp = frameController.ViewMaxBarCount - 15;
            if (null != _dataSource)
                if (_dataSource.Count > emp)
                    hScrollBar.Maximum = _dataSource.Count - emp;
                else
                    hScrollBar.Maximum = 0;
            #endregion
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

            frameController = new FrameController();
            frameController.Parent = this;
            frameController.MeasureStrAreatWidth();
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

            if (frameController != null)
            {
                frameController.SetSize(this.Width, this.Height - hScrollBar.Height);
                //frameController.CalcBarsView();
                frameController.OnResize();
            }

            if (hScrollBar != null && _dataSource != null && _dataSource.Count > 0)
            {
                FitLast(false);
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

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

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

            this.Cursor = Cursors.Default;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            this.Cursor = Cursors.Default;

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
                        int startPoint = frameController.GetMainChart().GraphRect.Left + Scaling.Margin;

                        //_ViewPos = (int)Math.Truncate((decimal)(e.X - (Scaling.Margin + 1)) / Scaling.FullWidth);
                        _ViewPos = (int)Math.Truncate((decimal)(e.X - (startPoint)) / Scaling.FullWidth);
                        _DataPos = hScrollBar.Value + ViewPos;
                        _ViewPrice = frameController.GetMainChart().ValueFromViewYPosition(e.Y);
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
                    int startPoint = frameController.GetMainChart().GraphRect.Left + Scaling.Margin;

                    //_ViewPos = (int)Math.Truncate((decimal)(e.X - (Scaling.Margin + 1)) / Scaling.FullWidth);
                    _ViewPos = (int)Math.Truncate((decimal)(e.X - (startPoint)) / Scaling.FullWidth);
                    _DataPos = hScrollBar.Value + ViewPos;
                    _ViewPrice = frameController.GetMainChart().ValueFromViewYPosition(e.Y);
                }
            }
        }

        #endregion

        #region Drawing

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            Refresh();
        }

        public void Loaded()
        {
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

            if (frameController != null)
            {
                frameController.SetSize(this.Width, this.Height - hScrollBar.Height);
                //frameController.CalcBarsView();
                frameController.OnResize();
            }

            Refresh();

            if (hScrollBar != null && _dataSource != null && _dataSource.Count > 0)
            {
                FitLast(true);
            }

        }

        public FrameController.PaintCompletedEventHandler PaintCompleted;

        public override void Refresh()
        {
            if (frameController == null)
                return;

            if (myDoubleBuffer == null)
                return;

            if (hScrollBar.Value >= 0)
                frameController.Paint(myDoubleBuffer.Graphics, hScrollBar.Value);

            if (_mouseSelectMode)
            {
                _mSelect.SetTopBottom(frameController.GetMainChart().Top + 1, frameController.GetMainChart().GraphRect.Height + 1);
                //_mSelect.SetTopBottom(frameController.GetMainChart().Top + 1, frameController.GetMainChart().GraphRect.Height - HScrollBar.Height);
                _mSelect.Scaling = frameController.Scaling;

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

            //visibleSeriesCount = frameController->GetDrawAreaWidth() / frameController->GetElementWidth();

            //if (frameController->GetElementWidth() > 0)
            //{
            //	if (Series.Count() >= visibleSeriesCount)
            //		shScrollBar.Value = (Series.Count() - _visibleSeriesCount + 18) * renderer.DrawWidth;  //最大値の設定 
            //}
            //else
            //	shScrollBar.Value = 0;
        }

        #endregion

        #region Framing

        public void Tick(DateTime openTime, double ask, double bid, bool lastBar)
        {
            if (_dataSource == null)
                return;

            if (_dataSource.DataSource == null)
                return;

            _dataSource.DataSource.Ask = ask;
            _dataSource.DataSource.Bid = bid;

            int i = _dataSource.DataSource.OpenTime.Count - 1;
            _dataSource.TickUpdate(openTime, ask, bid);

            FitLast(lastBar);

            Refresh();
            Application.DoEvents();
        }

        //public void SetSeries(cAlgo.API.TimeSeries opentime, cAlgo.API.DataSeries open, cAlgo.API.DataSeries high, cAlgo.API.DataSeries low, cAlgo.API.DataSeries close)
        //{
        //    MarketDataSeries _mktdataSource = new MarketDataSeries();
        //    _mktdataSource.OpenTime.AlgoData = opentime;
        //    _mktdataSource.Open.AlgoData = open;
        //    _mktdataSource.High.AlgoData = high;
        //    _mktdataSource.Low.AlgoData = low;
        //    _mktdataSource.Close.AlgoData = close;

        //    DataSource = _mktdataSource;

        //    int imax = Math.Max(_mktdataSource.OpenTime.Count,
        //               Math.Max(_mktdataSource.Open.Count,
        //               Math.Max(_mktdataSource.High.Count,
        //               Math.Max(_mktdataSource.Low.Count,
        //                        _mktdataSource.Close.Count))));

        //    // バーの最大本数からスクロールバーの最大値を計算する
        //    //int  fcw = frameController.GetElementWidth();
        //    hScrollBar.Maximum = imax;

        //    Refresh();
        //}

        public void Clear()
        {
            if (_dataSource != null)
                _dataSource.DataSource = null;

            _dataSource = null;
            GC.Collect();

            if (!InvokeRequired)
                hScrollBar.Maximum = 0;

            if (InvokeRequired)
                Invoke(new Action(Refresh));
            else
                Refresh();
        }

        #endregion
    }
}