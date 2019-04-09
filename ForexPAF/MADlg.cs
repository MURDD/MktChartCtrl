using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ForexPAF
{
    public partial class MADlg : Form
    {
        public MADlg()
        {
            InitializeComponent();

            cmbMethod.SelectedIndex = 0;
            cmbPrice.SelectedIndex = 0;
        }

        private void MADlg_Load(object sender, EventArgs e)
        {
        }

        private void label5_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = lblColor.BackColor;
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lblColor.BackColor = colorDialog1.Color;
            }
        }
    }
}
