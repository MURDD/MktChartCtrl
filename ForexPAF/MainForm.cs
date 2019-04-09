using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using MarketDataSeries;
using MarketDataSeries.PFPatterns;
using PFChartCtrl.IndicatorRenderItem;
using System.Runtime.Serialization.Json;

namespace ForexPAF
{
    public partial class MainForm : Form
    {
        public static MainForm Instance;
        public static Settings settings;

        MarketDataSeries.MarketData data;
        PFDataSeries pfDataSource;

        #region Clip Board

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern bool BitBlt(IntPtr hdcDest,
             int nXDest, int nYDest, int nWidth, int nHeight,
             IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

        private const int SRCCOPY = 0xCC0020;

        public Bitmap CaptureControl(Control ctrl)
        {
            Graphics g = ctrl.CreateGraphics();
            Bitmap img = new Bitmap(ctrl.ClientRectangle.Width,
                ctrl.ClientRectangle.Height, g);
            Graphics memg = Graphics.FromImage(img);
            IntPtr dc1 = g.GetHdc();
            IntPtr dc2 = memg.GetHdc();
            BitBlt(dc2, 0, 0, img.Width, img.Height, dc1, 0, 0, SRCCOPY);
            g.ReleaseHdc(dc1);
            memg.ReleaseHdc(dc2);
            memg.Dispose();
            g.Dispose();
            return img;
        }
        #endregion

        private SystemMenu systemMenu;

        const uint WM_SIZE = 0x0005;
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint wMsg, IntPtr wParam, IntPtr lParam);

        public MainForm()
        {
            InitializeComponent();
            Instance = this;

            // Create instance and connect it with the Form
            systemMenu = new SystemMenu(this);

            // Define commands and handler methods
            // (Deferred until HandleCreated if it's too early)
            // IDs are counted internally, separator is optional
            systemMenu.AddCommand("&About…", OnSysMenuAbout, true);
        }

        protected override void WndProc(ref Message msg)
        {
            base.WndProc(ref msg);

            // Let it know all messages so it can handle WM_SYSCOMMAND
            // (This method is inlined)
            systemMenu.HandleMessage(ref msg);
        }

        // Handle menu command click
        private void OnSysMenuAbout()
        {
            using (Version frm = new Version())
            {
                frm.ShowDialog();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";

            bool loaded = Settings.Load();
            Settings settings = Settings.Instance;

            cmbScale.Items.Clear();
            for (int i = 1; i <= 7; i++)
                cmbScale.Items.Add(i.ToString());

            cmbMargin.Items.Clear();
            for (int i = 1; i <= 5; i++)
                cmbMargin.Items.Add(i.ToString());
            cmbMargin.SelectedIndex = 0;

            cmbScale.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMargin.DropDownStyle = ComboBoxStyle.DropDownList;

            numBox.Value = settings.PointAndFigure.BoxPips;
            numReversal.Value = settings.PointAndFigure.RevarsalBox;
            cmbScale.SelectedIndex = cmbScale.Items.IndexOf(settings.Showing.Scaling.ToString());
            cmbMargin.SelectedIndex = cmbMargin.Items.IndexOf(settings.Showing.Margin.ToString());
            showLeftToolStripMenuItem.Checked = settings.Showing.ShowLeft;
            barStyleToolStripMenuItem.Checked = settings.Showing.BarStyle;
            gridLineToolStripMenuItem.Checked = settings.Showing.GridLine;
            simpleLineToolStripMenuItem.Checked = settings.Showing.SimpleLine;

            pfChartCtrl1.BoxPipSize = (int)numBox.Value;
            pfChartCtrl1.ReversalBox = (int)numReversal.Value;
            pfChartCtrl1.ShowLeftPrice = showLeftToolStripMenuItem.Checked;
            pfChartCtrl1.BarStyle = barStyleToolStripMenuItem.Checked;
            pfChartCtrl1.ShowGridLine = gridLineToolStripMenuItem.Checked;
            pfChartCtrl1.ShowLine = simpleLineToolStripMenuItem.Checked;
            pfChartCtrl1.Scaling.Size = int.Parse(cmbScale.Items[cmbScale.SelectedIndex].ToString());
            pfChartCtrl1.Scaling.Margin = int.Parse(cmbMargin.Items[cmbMargin.SelectedIndex].ToString());

            Top = settings.WindowPosition.Top;
            Left = settings.WindowPosition.Left;
            Width = settings.WindowPosition.Width;
            Height = settings.WindowPosition.Height;

            try
            {
                if (loaded)
                {
                    pfChartCtrl1.Colors.Background.Color = ColorTranslator.FromHtml(settings.ChartColors.Background.Replace("#", "0x"));
                    pfChartCtrl1.Colors.Foreground.Color = ColorTranslator.FromHtml(settings.ChartColors.Foreground.Replace("#", "0x"));
                    pfChartCtrl1.Colors.Grid.Color = ColorTranslator.FromHtml(settings.ChartColors.Grid.Replace("#", "0x"));
                    pfChartCtrl1.Colors.PeriodSeparators.Color = ColorTranslator.FromHtml(settings.ChartColors.PeriodSeparators.Replace("#", "0x"));
                    pfChartCtrl1.Colors.BullOutline.Color = ColorTranslator.FromHtml(settings.ChartColors.BullOutline.Replace("#", "0x"));
                    pfChartCtrl1.Colors.BearOutline.Color = ColorTranslator.FromHtml(settings.ChartColors.BearOutline.Replace("#", "0x"));
                    pfChartCtrl1.Colors.BullFill.Color = ColorTranslator.FromHtml(settings.ChartColors.BullFill.Replace("#", "0x"));
                    pfChartCtrl1.Colors.BearFill.Color = ColorTranslator.FromHtml(settings.ChartColors.BearFill.Replace("#", "0x"));
                }
                else
                {
                    settings.ChartColors.Background = ColorTranslator.ToHtml(pfChartCtrl1.Colors.Background.Color);
                    settings.ChartColors.Foreground = ColorTranslator.ToHtml(pfChartCtrl1.Colors.Foreground.Color);
                    settings.ChartColors.Grid = ColorTranslator.ToHtml(pfChartCtrl1.Colors.Grid.Color);
                    settings.ChartColors.PeriodSeparators = ColorTranslator.ToHtml(pfChartCtrl1.Colors.PeriodSeparators.Color);
                    settings.ChartColors.BullOutline = ColorTranslator.ToHtml(pfChartCtrl1.Colors.BullOutline.Color);
                    settings.ChartColors.BearOutline = ColorTranslator.ToHtml(pfChartCtrl1.Colors.BearOutline.Color);
                    settings.ChartColors.BullFill = ColorTranslator.ToHtml(pfChartCtrl1.Colors.BullFill.Color);
                    settings.ChartColors.BearFill = ColorTranslator.ToHtml(pfChartCtrl1.Colors.BearFill.Color);
                }

                // Clear the ContextMenuStrip control's Items collection.
                editToolStripMenuItem1.DropDown.Items.Clear();
                removeToolStripMenuItem.DropDown.Items.Clear();

                // Add Indicator MA
                foreach (var ma in settings.Indicators)
                {
                    Color color = ColorTranslator.FromHtml(ma.Color.Replace("#", "0x"));
                    string maName = MA.GetName(ma.Method, ma.PriceType, ma.Period);

                    var c = new MA(pfChartCtrl1.PFDataSource, maName, color);
                    c.PriceType = ma.PriceType;
                    c.Method = ma.Method;
                    c.Period = ma.Period;

                    c.Fill();
                    pfChartCtrl1.Frames.Frame.Add(c);

                    // Populate the ContextMenuStrip control with its default items.
                    editToolStripMenuItem1.DropDown.Items.Add(MA.GetName(ma.Method, ma.PriceType, ma.Period));
                    removeToolStripMenuItem.DropDown.Items.Add(MA.GetName(ma.Method, ma.PriceType, ma.Period));
                }
            }
            catch (Exception ex)
            {
            }

            pfChartCtrl1.Dock = DockStyle.Fill;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Instance.Indicators.Clear();
            foreach (MA ma in pfChartCtrl1.Frames.Frame)
            {
                MAConfig m = new MAConfig()
                {
                    Method = ma.Method,
                    PriceType = ma.PriceType,
                    Period = ma.Period,
                    Color = ColorTranslator.ToHtml(ma.Color)
                };
                Settings.Instance.Indicators.Add(m);
            }

            Settings.Instance.ChartColors.Background = ColorTranslator.ToHtml(pfChartCtrl1.Colors.Background.Color);
            Settings.Instance.ChartColors.Foreground = ColorTranslator.ToHtml(pfChartCtrl1.Colors.Foreground.Color);
            Settings.Instance.ChartColors.Grid = ColorTranslator.ToHtml(pfChartCtrl1.Colors.Grid.Color);
            Settings.Instance.ChartColors.PeriodSeparators = ColorTranslator.ToHtml(pfChartCtrl1.Colors.PeriodSeparators.Color);
            Settings.Instance.ChartColors.BullOutline = ColorTranslator.ToHtml(pfChartCtrl1.Colors.BullOutline.Color);
            Settings.Instance.ChartColors.BearOutline = ColorTranslator.ToHtml(pfChartCtrl1.Colors.BearOutline.Color);
            Settings.Instance.ChartColors.BullFill = ColorTranslator.ToHtml(pfChartCtrl1.Colors.BullFill.Color);
            Settings.Instance.ChartColors.BearFill = ColorTranslator.ToHtml(pfChartCtrl1.Colors.BearFill.Color);

            Settings.Instance.PointAndFigure.BoxPips = (int)numBox.Value;
            Settings.Instance.PointAndFigure.RevarsalBox = (int)numReversal.Value;
            Settings.Instance.Showing.Scaling = int.Parse(cmbScale.Items[cmbScale.SelectedIndex].ToString());
            Settings.Instance.Showing.Margin = int.Parse(cmbMargin.Items[cmbMargin.SelectedIndex].ToString());
            Settings.Instance.Showing.ShowLeft = showLeftToolStripMenuItem.Checked;
            Settings.Instance.Showing.BarStyle = barStyleToolStripMenuItem.Checked;
            Settings.Instance.Showing.GridLine = gridLineToolStripMenuItem.Checked;
            Settings.Instance.Showing.SimpleLine = simpleLineToolStripMenuItem.Checked;

            Settings.Instance.WindowPosition.Top = MainForm.Instance.Top;
            Settings.Instance.WindowPosition.Left = MainForm.Instance.Left;
            Settings.Instance.WindowPosition.Width = MainForm.Instance.Width;
            Settings.Instance.WindowPosition.Height = MainForm.Instance.Height;

            Settings.Save();
        }

        private void Enabler(bool value)
        {
            btnRefresh.Enabled = value;
            numBox.Enabled = value;
            numReversal.Enabled = value;

            cmbScale.Enabled = value;
            cmbMargin.Enabled = value;
            showLeftToolStripMenuItem.Enabled = value;
            barStyleToolStripMenuItem.Enabled = value;
            gridLineToolStripMenuItem.Enabled = value;
            simpleLineToolStripMenuItem.Enabled = value;

            fileToolStripMenuItem.Enabled = value;
            editToolStripMenuItem.Enabled = value;
            viewToolStripMenuItem.Enabled = value;
            helpToolStripMenuItem.Enabled = value;
        }

        private void pfChartCtrl1_MouseMove(object sender, MouseEventArgs e)
        {
            int sp = pfChartCtrl1.ScrollPos;
            int dp = pfChartCtrl1.DataPos;

            if (pfChartCtrl1.ViewPos < 0)
            {
                sp = -1;
                dp = -1;
            }

            if (pfChartCtrl1.ViewPos >= pfChartCtrl1.MaxBarNumber)
            {
                sp = -1;
                dp = -1;
            }

            if (null != pfChartCtrl1)
                if (null != pfChartCtrl1.PFDataSource)
                    if (dp >= pfChartCtrl1.PFDataSource.Count)
                    {
                        sp = -1;
                        dp = -1;
                    }

            if (dp == -1)
                return;

            if (null != pfChartCtrl1)
                if (null != pfChartCtrl1.PFDataSource)
                {
                    var o = pfChartCtrl1.PFDataSource[dp].Where(t => t.Low <= pfChartCtrl1.ViewPeice && pfChartCtrl1.ViewPeice <= t.High);

                    if (o != null)
                    {
                        if (o.Count() > 0)
                            toolStripStatusLabel1.Text = "Top: " + pfChartCtrl1.PFDataSource[dp].High.ToString() + "  "
                                        + "Bottom: " + pfChartCtrl1.PFDataSource[dp].Low.ToString() + "  "
                                        + "Box [Time: " + o.First().OpenTime.ToString() + "  "
                                        + "High: " + o.First().High.ToString() + "  "
                                        + "Low: " + o.First().Low.ToString() + "]";
                        else
                            toolStripStatusLabel1.Text = "Top: " + pfChartCtrl1.PFDataSource[dp].High.ToString() + "  "
                                        + "Bottom: " + pfChartCtrl1.PFDataSource[dp].Low.ToString() + "  "
                                        + "Time[Begin: " + pfChartCtrl1.PFDataSource[dp].First().OpenTime.ToString() + ", "
                                        + "End: " + pfChartCtrl1.PFDataSource[dp].Last().OpenTime.ToString() + "]";
                    }
                }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pfDataSource != null)
                using (Bitmap bmp = CaptureControl(pfChartCtrl1))
                {
                    

                    //描画先とするImageオブジェクトを作成する
                    Bitmap canvas = new Bitmap(bmp.Width, bmp.Height + 16 - pfChartCtrl1.HScrollbarHeight + 3);

                    //ImageオブジェクトのGraphicsオブジェクトを作成する
                    Graphics g = Graphics.FromImage(canvas);

                    //フォントオブジェクトの作成
                    Font fnt = new Font("MS UI Gothic", 10);

                    string s = Application.ProductName + ":  Box: " + pfDataSource.BoxPipSize.ToString() + ", Rev: " + pfDataSource.ReversalBox + "  [" + Text + "]";

                    //文字列を表示
                    g.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 62, 62)), 0, 0, bmp.Width, 16);
                    g.DrawString(s, fnt, Brushes.White, 2, 2);
                    g.DrawImage(bmp, 0, 17);
                    g.DrawRectangle(new Pen(Color.Black), 0, 0, canvas.Width - 1, canvas.Height - 1);

                    Clipboard.SetData(DataFormats.Bitmap, canvas);
                    bmp.Dispose();

                    //リソースを解放する
                    fnt.Dispose();
                    g.Dispose();

                }
        }

        private void colorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectColor frm = new SelectColor();

            frm.Background.BackColor = pfChartCtrl1.Colors.Background.Color;
            frm.Foreground.BackColor = pfChartCtrl1.Colors.Foreground.Color;
            frm.Grid.BackColor = pfChartCtrl1.Colors.Grid.Color;
            frm.PeriodSeparators.BackColor = pfChartCtrl1.Colors.PeriodSeparators.Color;
            frm.BullOutline.BackColor = pfChartCtrl1.Colors.BullOutline.Color;
            frm.BearOutline.BackColor = pfChartCtrl1.Colors.BearOutline.Color;
            frm.BullFill.BackColor = pfChartCtrl1.Colors.BullFill.Color;
            frm.BearFill.BackColor = pfChartCtrl1.Colors.BearFill.Color;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                pfChartCtrl1.Colors.Background.Color = frm.Background.BackColor;
                pfChartCtrl1.Colors.Foreground.Color = frm.Foreground.BackColor;
                pfChartCtrl1.Colors.Grid.Color = frm.Grid.BackColor;
                pfChartCtrl1.Colors.PeriodSeparators.Color = frm.PeriodSeparators.BackColor;
                pfChartCtrl1.Colors.BullOutline.Color = frm.BullOutline.BackColor;
                pfChartCtrl1.Colors.BearOutline.Color = frm.BearOutline.BackColor;
                pfChartCtrl1.Colors.BullFill.Color = frm.BullFill.BackColor;
                pfChartCtrl1.Colors.BearFill.Color = frm.BearFill.BackColor;
                pfChartCtrl1.Refresh();

            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Enabler(false);
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();

                ofd.Filter = "CSV File(*.csv)|*.csv";
                ofd.FilterIndex = 2;
                ofd.RestoreDirectory = true;
                ofd.CheckFileExists = true;
                ofd.CheckPathExists = true;

                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                toolStripStatusLabel1.Text = "Checking Files ...";
                toolStripStatusLabel1.Invalidate();
                Application.DoEvents();

                var ldr = CsvLoader.GetInstance(ofd.FileName);
                var typ = CsvLoader.TypeCheck(ofd.FileName);

                if (typ == CsvLoader.CSVFormat.NONE)
                {
                    MessageBox.Show("Unsupported Format.");
                    return;
                }

                SelectFile f = new SelectFile();
                f.textBox1.Text = ofd.FileName;

                var ret = ldr.NFormat(f.textBox1.Text);

                if (ret != null)
                {
                    f.numericUpDown1.Value = ret.Item2;
                    f.numericUpDown2.Value = ret.Item3;
                    f.textBox2.Text = new string('0', ret.Item2) + "." + new string('0', ret.Item3);
                    f.textBox3.Text = "0." + new string('0', ret.Item3 - 1) + "1";
                    f.textBox4.Text = "0." + new string('0', ret.Item3 - 2) + "1";
                    f.checkBox1.Checked = typ == CsvLoader.CSVFormat.MT4;
                    f.checkBox2.Checked = typ == CsvLoader.CSVFormat.MT5;
                }

                if (f.ShowDialog() != DialogResult.OK)
                {
                    toolStripStatusLabel1.Text = "";
                    toolStripStatusLabel1.Invalidate();
                    Application.DoEvents();
                    return;
                }

                foreach (var o in pfChartCtrl1.Frames.Frame)
                    ((MA)o).data.Clear();
                if (data != null) data.clear();
                pfChartCtrl1.Clear();

                toolStripStatusLabel1.Text = "Now Loading ...";
                toolStripStatusLabel1.Invalidate();
                Application.DoEvents();

                Text = f.filename;

                data = ldr.Load(f.filename);

                pfChartCtrl1.SetPriceFormat(f.left, f.right);
                pfChartCtrl1.PriceTickSize = f.tick;
                pfChartCtrl1.ViewPriceTickSize = f.viewtick;
                pfChartCtrl1.PipSize = f.tick * 10;

                toolStripStatusLabel1.Text = "Now work in process ...";
                toolStripStatusLabel1.Invalidate();
                Application.DoEvents();

                pfDataSource = new MarketDataSeries.PFDataSeries(data, (int)numBox.Value, (int)numReversal.Value, f.tick * 10);
                pfDataSource.Refresh();

                pfChartCtrl1.PFDataSource = pfDataSource;

                foreach (var o in pfChartCtrl1.Frames.Frame)
                {
                    ((MA)o).Fill(pfDataSource);
                }

                //SendMessage(pfChartCtrl1.Handle, WM_SIZE, IntPtr.Zero, IntPtr.Zero);

                pfChartCtrl1.Loaded();

                toolStripStatusLabel1.Text = "Ready.";
                toolStripStatusLabel1.Invalidate();
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ForexPAF", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Enabler(true);
            }
        }

        private void refreshToolStripButton_Click(object sender, EventArgs e)
        {
            if (pfDataSource == null)
                return;

            pfDataSource.BoxPipSize = (int)numBox.Value;
            pfDataSource.ReversalBox = (int)numReversal.Value;

            foreach (var o in pfChartCtrl1.Frames.Frame)
                ((MA)o).data.Clear();

            toolStripStatusLabel1.Text = "Now work in process ...";
            toolStripStatusLabel1.Invalidate();
            Application.DoEvents();

            pfDataSource.Clear();
            pfChartCtrl1.Refresh();
            Application.DoEvents();
            pfDataSource.Refresh();

            foreach (var o in pfChartCtrl1.Frames.Frame)
            {
                ((MA)o).Fill();
            }

            pfChartCtrl1.Loaded();

            toolStripStatusLabel1.Text = "Ready.";
            toolStripStatusLabel1.Invalidate();
            Application.DoEvents();
        }

        private void versionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Version frm = new Version())
            {
                frm.ShowDialog();
            }
        }

        private void showLeftToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            pfChartCtrl1.ShowLeftPrice = showLeftToolStripMenuItem.Checked;
        }

        private void barStyleToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            pfChartCtrl1.BarStyle = barStyleToolStripMenuItem.Checked;
        }

        private void gridLineToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            pfChartCtrl1.ShowGridLine = gridLineToolStripMenuItem.Checked;
        }

        private void simpleLineToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            pfChartCtrl1.ShowLine = simpleLineToolStripMenuItem.Checked;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int priceType = 0;
            int method = 0;
            int period = 7;
            Color color = Color.Green;

        input:
            using (MADlg dlg = new MADlg())
            {
                dlg.numPeriod.Value = period;
                dlg.cmbMethod.SelectedIndex = 0;
                dlg.cmbPrice.SelectedIndex = 0;
                dlg.lblColor.BackColor = color;

                if (dlg.ShowDialog() == DialogResult.Cancel)
                    return;

                period = (int)dlg.numPeriod.Value;
                method = dlg.cmbMethod.SelectedIndex;
                priceType = dlg.cmbPrice.SelectedIndex;
                color = dlg.lblColor.BackColor;
            }

            if (pfChartCtrl1.Frames.Frame.Where(t => ((MA)t).Period == period && ((MA)t).PriceType == priceType && ((MA)t).Method == method).Any())
            {
                // registered
                MessageBox.Show("Registered.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                goto input;
            }

            string maName = MA.GetName(method, priceType, period);
            var c = new MA(pfChartCtrl1.PFDataSource, maName, color);
            c.PriceType = priceType;
            c.Method = method;
            c.Period = period;

            c.Fill();
            pfChartCtrl1.Frames.Frame.Add(c);
            pfChartCtrl1.Refresh();

            // 追加
            //MovingAverage m = new MovingAverage()
            //{
            //    Method = method,
            //    PriceType = priceType,
            //    Period = period,
            //    Color = ColorTranslator.ToHtml(color)
            //};
            //Settings.Instance.Indicators.Add(m);

        }

        private void mAToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            /* EDIT */
            // Clear the ContextMenuStrip control's Items collection.
            editToolStripMenuItem1.DropDown.Items.Clear();

            // Populate the ContextMenuStrip control with its default items.
            foreach (MA item in pfChartCtrl1.Frames.Frame)
                editToolStripMenuItem1.DropDown.Items.Add(MA.GetName(item.Method, item.PriceType, item.Period), null, EditMovingAverage);

            /* REMOVE */
            // Clear the ContextMenuStrip control's Items collection.
            removeToolStripMenuItem.DropDown.Items.Clear();

            // Populate the ContextMenuStrip control with its default items.
            foreach (MA item in pfChartCtrl1.Frames.Frame)
                removeToolStripMenuItem.DropDown.Items.Add(MA.GetName(item.Method, item.PriceType, item.Period), null, RemoveMovingAverage);

            if (removeToolStripMenuItem.DropDown.Items.Count > 0)
            {
                removeToolStripMenuItem.DropDown.Items.Add("-");
                removeToolStripMenuItem.DropDown.Items.Add("Remove All", null, RemoveAllMovingAverage);
            }
        }

        private void EditMovingAverage(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            int i = pfChartCtrl1.Frames.Frame.IndexOf(item.Text);
            if (i < 0)
            {
                return;
            }

            var ind = (MA)pfChartCtrl1.Frames.Frame[i];

            using (MADlg dlg = new MADlg())
            {
                dlg.numPeriod.Value = ind.Period;
                dlg.cmbMethod.SelectedIndex = ind.Method;
                dlg.cmbPrice.SelectedIndex = ind.PriceType;
                dlg.lblColor.BackColor = ind.Color;

                if (dlg.ShowDialog() == DialogResult.Cancel)
                    return;

                string old = ind.Name;

                ind.Period = (int)dlg.numPeriod.Value;
                ind.Method = dlg.cmbMethod.SelectedIndex;
                ind.PriceType = dlg.cmbPrice.SelectedIndex;
                ind.Color = dlg.lblColor.BackColor;

                string maName = MA.GetName(ind.Method, ind.PriceType, ind.Period);
                ind.Name = maName;

                ind.Fill();
                pfChartCtrl1.Refresh();
            }

        }

        private void RemoveAllMovingAverage(object sender, EventArgs e)
        {
            for (int i = pfChartCtrl1.Frames.Frame.Count - 1; i>=0; i--)
                pfChartCtrl1.Frames.Frame.RemoveAt(i);
        }

        private void RemoveMovingAverage(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            int i = pfChartCtrl1.Frames.Frame.IndexOf(item.Text);
            if (i < 0)
            {
                return;
            }

            string name = pfChartCtrl1.Frames.Frame[i].Name;

            // remove
            pfChartCtrl1.Frames.Frame.Remove(item.Text);
        }

        private void cmbScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            pfChartCtrl1.Scaling.Size = int.Parse(cmbScale.Items[cmbScale.SelectedIndex].ToString());
            pfChartCtrl1.Refresh();
        }

        private void cmbMargin_SelectedIndexChanged(object sender, EventArgs e)
        {
            pfChartCtrl1.Scaling.Margin = int.Parse(cmbMargin.Items[cmbMargin.SelectedIndex].ToString());
            pfChartCtrl1.Refresh();
        }

        protected override bool ProcessCmdKey(ref Message message, Keys keys)
        {
            switch (keys)
            {
                case Keys.Control | Keys.O:
                  {
                      openToolStripMenuItem_Click(null, null);
                      return true;
                  }
                case Keys.Control | Keys.C:
                  {
                      copyToolStripMenuItem_Click(null, null);
                      return true;
                  }
                case Keys.Control | Keys.A:
                  {
                      addToolStripMenuItem_Click(null, null);
                      return true;
                  }
                case Keys.Control | Keys.L:
                  {
                      colorsToolStripMenuItem_Click(null, null);
                      return true;
                  }
            }
            return base.ProcessCmdKey(ref message, keys);
        }
    }
}
