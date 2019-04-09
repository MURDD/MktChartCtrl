using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestChartViewApp
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
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

            textBox1.Text = ofd.FileName;

            var ret = NFormat.Get(textBox1.Text);

            if (ret != null)
            {
                numericUpDown1.Value = ret.Item1;
                numericUpDown2.Value = ret.Item2;
                textBox2.Text = new string('0',ret.Item1) + "." + new string('0', ret.Item2);
                textBox3.Text = "0." + new string('0', ret.Item2 - 1) + "1";
                textBox4.Text = "0." + new string('0', ret.Item2 - 2) + "1";
            }
        }

        public string filename = "";
        public int left = 0;
        public int right = 0;
        public double tick = 0.0;
        public double viretick = 0.0;
        private void button2_Click(object sender, EventArgs e)
        {
            filename = textBox1.Text;
            left = (int)numericUpDown1.Value;
            right = (int)numericUpDown2.Value;
            tick = double.Parse(textBox3.Text);
            viretick = double.Parse(textBox4.Text);
        }
}
}
