using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MarketDataSeries;
using MarketDataSeries.Indicators;
using MarketChartCtrl;
using MarketChartCtrl.IndicatorRenderItem;

namespace TestChartViewApp
{
    public partial class Form1 : Form
    {
        public ChartColors ChartColors { get; set; }

        //MarketChartCtrl.MarketChartCtrl marketChartViewCtrl1;
        public Form1()
        {
            InitializeComponent();

            //ChartCtrl p = new ChartCtrl();
            //p.Parent = this;
            //p.Location = new Point(218, 408);
            //p.Size = new Size(1162, 459);
            //p.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            //p.Visible = true;
            //p.Refresh();

            checkBox2.Checked = marketChartViewCtrl1.BarStyle == MarketChartCtrl.BarStyle.Candle;

            marketChartViewCtrl1.SetPriceFormat(3, 2);
            marketChartViewCtrl1.PriceTickSize = 0.001;
            marketChartViewCtrl1.ViewPriceTickSize = 0.01;

            numericUpDown2.Value = marketChartViewCtrl1.ElementSize;
            numericUpDown3.Value = marketChartViewCtrl1.ElementMargin;

            listBox1.Items.Add("MA");
            listBox1.Items.Add("ATR");
            listBox1.SelectedIndex = 0;
            ListingFrame();

            label4.Text = "";
            label5.Text = "";
            label6.Text = "";
            label7.Text = "";
            label8.Text = "";
            label9.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (ChartColors != null)
            {
                marketChartViewCtrl1.Colors.Background.Color = ChartColors.Background.Color;
                marketChartViewCtrl1.Colors.Foreground.Color = ChartColors.Foreground.Color;
                marketChartViewCtrl1.Colors.Grid.Color = ChartColors.Grid.Color;
                marketChartViewCtrl1.Colors.PeriodSeparators.Color = ChartColors.PeriodSeparators.Color;
                marketChartViewCtrl1.Colors.BullOutline.Color = ChartColors.BullOutline.Color;
                marketChartViewCtrl1.Colors.BearOutline.Color = ChartColors.BearOutline.Color;
                marketChartViewCtrl1.Colors.BullFill.Color = ChartColors.BullFill.Color;
                marketChartViewCtrl1.Colors.BearFill.Color = ChartColors.BearFill.Color;

                marketChartViewCtrl1.Colors.WinningDeal.Color = ChartColors.WinningDeal.Color;
                marketChartViewCtrl1.Colors.LosingDeal.Color = ChartColors.LosingDeal.Color;
                marketChartViewCtrl1.Colors.AskLine.Color = ChartColors.AskLine.Color;
                marketChartViewCtrl1.Colors.BidLine.Color = ChartColors.BidLine.Color;

                marketChartViewCtrl1.Colors.BuyPosition.Color = ChartColors.BuyPosition.Color;
                marketChartViewCtrl1.Colors.SellPosition.Color = ChartColors.SellPosition.Color;
                marketChartViewCtrl1.Colors.BuyPositionStoploss.Color = ChartColors.BuyPositionStoploss.Color;
                marketChartViewCtrl1.Colors.SellPositionStoploss.Color = ChartColors.SellPositionStoploss.Color;
                marketChartViewCtrl1.Colors.BuyPositionTakeprofit.Color = ChartColors.BuyPositionTakeprofit.Color;
                marketChartViewCtrl1.Colors.SellPositionTakeprofit.Color = ChartColors.SellPositionTakeprofit.Color;

                marketChartViewCtrl1.Colors.BuyOrder.Color = ChartColors.BuyOrder.Color;
                marketChartViewCtrl1.Colors.SellOrder.Color = ChartColors.SellOrder.Color;
                marketChartViewCtrl1.Colors.BuyOrderStoploss.Color = ChartColors.BuyOrderStoploss.Color;
                marketChartViewCtrl1.Colors.SellOrderStoploss.Color = ChartColors.SellOrderStoploss.Color;
                marketChartViewCtrl1.Colors.BuyOrderTakeprofit.Color = ChartColors.BuyOrderTakeprofit.Color;
                marketChartViewCtrl1.Colors.SellOrderTakeprofit.Color = ChartColors.SellOrderTakeprofit.Color;

                marketChartViewCtrl1.Refresh();
            }
        }

        string[] tp = { "Top", "Main", "Bottom" };
        private void ListingFrame()
        {
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            int i = 0;
            int fi = 0;
            int k = 0;
            foreach (var v in marketChartViewCtrl1.Frames)
            {
                if (fi == 1 && !v.isMainFrame)
                {
                    ++fi;
                    k = 0;
                }

                if (v.isMainFrame)
                {
                    ++fi;
                    k = 0;
                }

                string s = i.ToString() + "-" + tp[fi] + (v.isMainFrame ? "" : k.ToString());
                listBox2.Items.Add(s);
                ++i;
                ++k;
            }
        }

        private void ListingIndicator()
        {
            listBox3.Items.Clear();
            
            if (listBox2.SelectedIndex < 0)
                return;

            string s = listBox2.Items[listBox2.SelectedIndex].ToString();
            var sl = s.Split('-');
            int x = int.Parse(sl[0]);
            int i = 0;

            if (marketChartViewCtrl1.Frames.Count > x)
                foreach (var el in marketChartViewCtrl1.Frames[x])
                {
                    string g = i.ToString() + "-" + el.Name;
                    listBox3.Items.Add(g);
                }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (data == null)
                return;

            if (data.Close.Count < 0)
                return;

            MarketChartCtrl.FrameView frm = null;

            if (radioButton2.Checked)
            {
                int pos = (int)numericUpDown1.Value;

                if (marketChartViewCtrl1.Frames.Count < pos)
                    pos = marketChartViewCtrl1.Frames.Count;

                if (0 > pos)
                    pos = 0;

                frm = marketChartViewCtrl1.Frames.Add(pos);
            }
            else
            {
                frm = marketChartViewCtrl1.Frames[marketChartViewCtrl1.Frames.MainFrameIndex];
            }

            if (frm != null)
            {
                string s = listBox1.Items[listBox1.SelectedIndex].ToString();

                if (s == "MA")
                {
                    var x = new MovingAverage(MaType.Simple, 6, data.Close);
                    x.fill();
                    //frm.Add(new IndicatorRenderLine("ma6", x.Result));
                    frm.Add(new IndicatorRenderPoint("ma6", x.Result));
                }
                else
                if (s == "ATR")
                {
                    var x = new AverageTrueRange(6, data.Close, data.High, data.Low);
                    x.fill();
                    frm.Add(new IndicatorRenderLine("atrL6", x.Result));
                    frm.Add(new IndicatorRenderHistogram("atrH6", x.Result));
                }
            }
            ListingFrame();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex < 0)
                return;

            var s = listBox2.Items[listBox2.SelectedIndex].ToString();
            var sl = s.Split('-');
            int x = int.Parse(sl[0]);

            marketChartViewCtrl1.Frames.RemoveAt(x);
            ListingFrame();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            marketChartViewCtrl1.Frames.Add(0);
            ListingFrame();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            marketChartViewCtrl1.Frames.Add(marketChartViewCtrl1.Frames.Count);
            ListingFrame();

        }

        MarketData data;
        private void button7_Click(object sender, EventArgs e)
        {
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            marketChartViewCtrl1.ShowLeftPrice = checkBox1.Checked;
            ListingFrame();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                marketChartViewCtrl1.BarStyle = MarketChartCtrl.BarStyle.Candle;
            else
                marketChartViewCtrl1.BarStyle = MarketChartCtrl.BarStyle.Bar;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //btnCalc.Enabled = false;
            //btnClipBrd.Enabled = false;
            //btnMA.Enabled = false;
            //btnColor.Enabled = false;
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

                Form2 f = new Form2();
                f.textBox1.Text = ofd.FileName;

                var ret = NFormat.Get(f.textBox1.Text);

                if (ret != null)
                {
                    f.numericUpDown1.Value = ret.Item1;
                    f.numericUpDown2.Value = ret.Item2;
                    f.textBox2.Text = new string('0', ret.Item1) + "." + new string('0', ret.Item2);
                    f.textBox3.Text = "0." + new string('0', ret.Item2 - 1) + "1";
                    f.textBox4.Text = "0." + new string('0', ret.Item2 - 2) + "1";
                }


                if (f.ShowDialog() != DialogResult.OK)
                    return;

                if (null!=data)
                    data.clear();
                marketChartViewCtrl1.Clear();

                marketChartViewCtrl1.SetPriceFormat(f.left, f.right);
                marketChartViewCtrl1.PriceTickSize = f.tick;
                marketChartViewCtrl1.ViewPriceTickSize = f.viretick;

                LoadCsvFile(f.filename);
                marketChartViewCtrl1.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "PnFChartView", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCsvFile(string filename)
        {
            try
            {
                Text = filename;

                var time = new TimeSeries();
                var open = new DataSeries();
                var high = new DataSeries();
                var low = new DataSeries();
                var close = new DataSeries();
                var vol = new DataSeries();

                // csvファイルを開く
                using (var sr = new System.IO.StreamReader(filename))
                {
                    // skip
                    sr.ReadLine();

                    // ストリームの末尾まで繰り返す
                    while (!sr.EndOfStream)
                    {
                        // ファイルから一行読み込む
                        var line = sr.ReadLine();

                        // 読み込んだ一行をカンマ毎に分けて配列に格納する
                        var values = line.Split(',');
                        if (values.Length < 6)
                            return;

                        time.Add(DateTime.Parse(values[0]));
                        open.Add(double.Parse(values[1]));
                        high.Add(double.Parse(values[2]));
                        low.Add(double.Parse(values[3]));
                        close.Add(double.Parse(values[4]));
                        vol.Add(double.Parse(values[5]));
                    }
                    sr.Close();
                }
                data = new MarketData("", time, open, high, low, close, vol);
            }
            catch (System.Exception e)
            {
                // ファイルを開くのに失敗したとき
                System.Console.WriteLine(e.Message);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var c = new MACH(data.High, data.Low, data.Close);
            c.fill();
            marketChartViewCtrl1.Frames[marketChartViewCtrl1.Frames.MainFrameIndex].Add(c.Indicators);
            marketChartViewCtrl1.Refresh();
            ListingFrame();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            for (int i = marketChartViewCtrl1.Frames.Count - 1; i >= 0; i--)
                marketChartViewCtrl1.Frames.RemoveAt(i);
            marketChartViewCtrl1.Frames[marketChartViewCtrl1.Frames.MainFrameIndex].Clear();
            marketChartViewCtrl1.Refresh();

            ListingFrame();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            marketChartViewCtrl1.Scaling.Size = (int)numericUpDown2.Value;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            marketChartViewCtrl1.Scaling.Margin = (int)numericUpDown3.Value;
        }

        private void listBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            ListingIndicator();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            marketChartViewCtrl1.Clear();
            ListingFrame();
            data.clear();
            Text = "　Select Csv File ...";
        }

        private void marketChartViewCtrl1_MouseMove(object sender, MouseEventArgs e)
        {
            int sp = marketChartViewCtrl1.ScrollPos;
            int dp = marketChartViewCtrl1.DataPos;

            if (marketChartViewCtrl1.ViewPos < 0)
            {
                sp = -1;
                dp = -1;
            }

            if (marketChartViewCtrl1.ViewPos >= marketChartViewCtrl1.MaxBarNumber)
            {
                sp = -1;
                dp = -1;
            }

            label4.Text = "X:" + e.X.ToString();
            label5.Text = "Y:" + e.Y.ToString();
            label6.Text = "ScrolPos:" + sp.ToString();
            label7.Text = "ViewPos:" + marketChartViewCtrl1.ViewPos.ToString();
            label8.Text = "DataPos:" + dp.ToString();
            if (null!=data)
                label9.Text = "T: " + data.OpenTime[dp].ToString() + "  "
                            + "O: " + data.Open[dp].ToString() + "  "
                            + "H: " + data.High[dp].ToString() + "  "
                            + "L: " + data.Low[dp].ToString() + "  "
                            + "C: " + data.Close[dp].ToString();
            else
                label9.Text ="";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form3 frm = new Form3();

            frm.Background.BackColor = marketChartViewCtrl1.Colors.Background.Color;
            frm.Foreground.BackColor = marketChartViewCtrl1.Colors.Foreground.Color;
            frm.Grid.BackColor = marketChartViewCtrl1.Colors.Grid.Color;
            frm.PeriodSeparators.BackColor = marketChartViewCtrl1.Colors.PeriodSeparators.Color;
            frm.BullOutline.BackColor = marketChartViewCtrl1.Colors.BullOutline.Color;
            frm.BearOutline.BackColor = marketChartViewCtrl1.Colors.BearOutline.Color;
            frm.BullFill.BackColor = marketChartViewCtrl1.Colors.BullFill.Color;
            frm.BearFill.BackColor = marketChartViewCtrl1.Colors.BearFill.Color;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                marketChartViewCtrl1.Colors.Background.Color = frm.Background.BackColor;
                marketChartViewCtrl1.Colors.Foreground.Color = frm.Foreground.BackColor;
                marketChartViewCtrl1.Colors.Grid.Color = frm.Grid.BackColor;
                marketChartViewCtrl1.Colors.PeriodSeparators.Color = frm.PeriodSeparators.BackColor;
                marketChartViewCtrl1.Colors.BullOutline.Color = frm.BullOutline.BackColor;
                marketChartViewCtrl1.Colors.BearOutline.Color = frm.BearOutline.BackColor;
                marketChartViewCtrl1.Colors.BullFill.Color = frm.BullFill.BackColor;
                marketChartViewCtrl1.Colors.BearFill.Color = frm.BearFill.BackColor;
                marketChartViewCtrl1.Refresh();
            }
        }

        private void button12_Click_1(object sender, EventArgs e)
        {

        }

        private void chkShowGrid_CheckedChanged(object sender, EventArgs e)
        {
            marketChartViewCtrl1.ShowGridLine = chkShowGrid.Checked;
        }
    }
}
