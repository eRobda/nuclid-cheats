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
using System.Runtime.InteropServices;

namespace our_first_menu
{
    public partial class Form1 : Form
    {

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(Keys vKey);

        static bool showing = true;

        private Form activeMenu;

        hacks hacks = new hacks();
        Form2 form2 = new Form2();

        public Form1()
        {
            InitializeComponent();
            form2.Show();

            #region window setup

            //make the window transparent
            this.BackColor = Color.Wheat;
            this.TransparencyKey = Color.Wheat;

            //make the window borderless
            this.FormBorderStyle = FormBorderStyle.None;

            //make the window start in top left corner
            this.StartPosition = FormStartPosition.Manual;

            //make the window topmost
            this.TopMost = true;


            CheckForIllegalCrossThreadCalls = false;

            Thread shm = new Thread(ShowHideMenu);
            shm.Start();

            #endregion

            //hacks
            Thread TriggerBotThread = new Thread(hacks.TriggerBot);
            TriggerBotThread.Start();

            Thread GlowThread = new Thread(hacks.Glow);
            GlowThread.Start();

            Thread AimbotThread = new Thread(hacks.Aimbot);
            AimbotThread.Start();

            label1.Parent = label7;
        }

        void ShowHideMenu()
        {
            while (true)
            {
                if(GetAsyncKeyState(Keys.Insert) < 0 && showing == true) //than hide it
                {
                    this.Hide();
                    //form2.Hide();
                    showing = false;
                    Thread.Sleep(20);
                }
                else if (GetAsyncKeyState(Keys.Insert) < 0 && showing == false) // than show it
                {
                    this.Show();
                    //form2.Show();
                    showing = true;
                    Thread.Sleep(20);
                }
                else if (GetAsyncKeyState(Keys.Delete) < 0)
                {
                    Environment.Exit(0);
                    Application.Exit();
                }

                Thread.Sleep(70);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //end all created threads
            Environment.Exit(0);
            Application.Exit();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            globals.triggerBot = !globals.triggerBot;
        }
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            globals.triggerBotDelay = trackBar1.Value;
            label2.Text = trackBar1.Value.ToString() + " ms";
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            globals.glow = !globals.glow;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            globals.aimbot = !globals.aimbot;
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            globals.aimbotFOV = trackBar2.Value;
            label5.Text = trackBar2.Value.ToString();
        }

        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            globals.aimbotSmooth = trackBar3.Value;
            label6.Text = trackBar3.Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            form2.DrawCircle(globals.aimbotFOV);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {            globals.fovCircle = !globals.fovCircle;
        }


        private void label9_MouseDown(object sender, MouseEventArgs e)
        {
            cheats.aimbot aim = new cheats.aimbot();
            aim.FormBorderStyle = FormBorderStyle.None;
            aim.TopLevel = false;
            aim.Dock = DockStyle.Fill;
            panel_main.Controls.Add(aim);
        }
    }
    
}
