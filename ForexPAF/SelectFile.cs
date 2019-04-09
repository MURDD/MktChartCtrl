using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ForexPAF
{
    public partial class SelectFile : Form
    {
        public SelectFile()
        {
            InitializeComponent();
        }

        public string filename = "";
        public int left = 0;
        public int right = 0;
        public double tick = 0.0;
        public double viewtick = 0.0;
        private void button2_Click(object sender, EventArgs e)
        {
            filename = textBox1.Text;
            left = (int)numericUpDown1.Value;
            right = (int)numericUpDown2.Value;
            tick = double.Parse(textBox3.Text);
            viewtick = double.Parse(textBox4.Text);
        }
}
}
