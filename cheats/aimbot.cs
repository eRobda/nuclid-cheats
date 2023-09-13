using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace our_first_menu.cheats
{
    public partial class aimbot : Form
    {
        public aimbot()
        {
            InitializeComponent();
        }

        void ChangeColor(CheckBox control)
        {
            if (control.Checked)
                control.ForeColor = Color.FromArgb(3, 132, 252);
            else
                control.ForeColor = Color.FromArgb(80, 80, 80);
        }

        void ChangeColor(Label lbl, Color col)
        {
            lbl.ForeColor = col;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            ChangeColor(checkBox3);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            ChangeColor((CheckBox)checkBox4);
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            globals.aimbotFOV = trackBar2.Value;
            label5.Text = trackBar2.Value.ToString();
        }

        private void trackBar2_MouseDown(object sender, MouseEventArgs e)
        {
            ChangeColor(label5, Color.FromArgb(3, 132, 252));
        }

        private void trackBar2_MouseUp(object sender, MouseEventArgs e)
        {
            ChangeColor(label5, Color.FromArgb(80,80,80));
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            globals.aimbotSmooth = trackBar1.Value;
            label1.Text = trackBar1.Value.ToString();
        }

        private void trackBar1_MouseDown(object sender, MouseEventArgs e)
        {
            ChangeColor(label1, Color.FromArgb(3, 132, 252));
        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            ChangeColor(label1, Color.FromArgb(80, 80, 80));
        }
    }
}
