using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ezOverLay;

namespace our_first_menu
{
    public partial class Form2 : Form
    {
        public static Form2 Instance;
        public Form2()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Instance = this;
            ez ez = new ez();
            ez.SetInvi(this);
            ez.DoStuff("Counter-Strike: Global Offensive - Direct3D 9", this);

        }

        public void RefreshPanel()
        {
            panel1.Refresh();
        }

        public void DrawCircle(float fov)
        {
        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (!globals.fovCircle)
                return;
            Graphics g = e.Graphics;
            float fov = globals.aimbotFOV * 24;
            g.DrawEllipse(new Pen(Color.Red), (Screen.PrimaryScreen.Bounds.Width / 2) - (fov / 2), (Screen.PrimaryScreen.Bounds.Height / 2) - (fov / 2), fov , fov );
        }
    }
}
