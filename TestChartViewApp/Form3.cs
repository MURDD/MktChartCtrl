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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private Color ColorPicker(Color clr)
        {
            ColorDialog colorDialog1 = new ColorDialog();
            colorDialog1.Color = clr;
            colorDialog1.AllowFullOpen = true;
            colorDialog1.FullOpen = true;
            colorDialog1.AnyColor = true;
            colorDialog1.SolidColorOnly = true;
            colorDialog1.ShowHelp = true;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color tmp = colorDialog1.Color;
                colorDialog1.Dispose();
                return tmp;
            }
            return clr;
        }

        private void Background_Click(object sender, EventArgs e)
        {
            Background.BackColor = ColorPicker(Background.BackColor);
        }

        private void Foreground_Click(object sender, EventArgs e)
        {
            Foreground.BackColor = ColorPicker(Foreground.BackColor);
        }

        private void Grid_Click(object sender, EventArgs e)
        {
            Grid.BackColor = ColorPicker(Grid.BackColor);
        }

        private void PeriodSeparators_Click(object sender, EventArgs e)
        {
            PeriodSeparators.BackColor = ColorPicker(PeriodSeparators.BackColor);
        }

        private void BullOutline_Click(object sender, EventArgs e)
        {
            BullOutline.BackColor = ColorPicker(BullOutline.BackColor);
        }

        private void BearOutline_Click(object sender, EventArgs e)
        {
            BearOutline.BackColor = ColorPicker(BearOutline.BackColor);
        }

        private void BullFill_Click(object sender, EventArgs e)
        {
            BullFill.BackColor = ColorPicker(BullFill.BackColor);
        }

        private void BearFill_Click(object sender, EventArgs e)
        {
            BearFill.BackColor = ColorPicker(BearFill.BackColor);
        }
    }
}
