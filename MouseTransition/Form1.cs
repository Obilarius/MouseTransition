using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MouseTransition
{
    public partial class Settings : Form
    {
        public Size monitor01 = new Size(1920, 600);
        public Size monitor02 = new Size(1920, 1200);

        Thread loopThread;

        public Settings()
        {
            InitializeComponent();

            var _allScreens = Screen.AllScreens;

            if (_allScreens[0].Bounds.X == 0)
            {
                monitor01.Width = _allScreens[0].Bounds.Width;
                monitor01.Height = _allScreens[0].Bounds.Height;
                monitor02.Width = _allScreens[1].Bounds.Width;
                monitor02.Height = _allScreens[1].Bounds.Height;
            }
            else if (_allScreens[1].Bounds.X == 0)
            {
                monitor01.Width = _allScreens[1].Bounds.Width;
                monitor01.Height = _allScreens[1].Bounds.Height;
                monitor02.Width = _allScreens[0].Bounds.Width;
                monitor02.Height = _allScreens[0].Bounds.Height;
            }

            txt_m01width.Text = Convert.ToString(monitor01.Width);
            txt_m01height.Text = Convert.ToString(monitor01.Height);
            txt_m02width.Text = Convert.ToString(monitor02.Width);
            txt_m02height.Text = Convert.ToString(monitor02.Height);

            loopThread = new Thread(Loop);
            loopThread.Start();
        }

        public int ConvertRange (int originalStart, int originalEnd, int newStart, int newEnd, int value)
        {

            int originalDiff = originalEnd - originalStart;
            int newDiff = newEnd - newStart;
            float ratio = (float)newDiff / (float)originalDiff;
            int newProduct = Convert.ToInt16(value * ratio);
            int finalValue = newProduct + newStart;
            return finalValue;

        }

        public void Loop ()
        {
            var last = new Point();

            while (true)
            {
                var c = Control.MousePosition;
                //lbl_out.Text = "X: " + c.X + " - Y: " + c.Y;
                //Console.WriteLine("X: " + c.X + " - Y: " + c.Y);

                if (last.X >= monitor01.Width && c.X <= monitor01.Width)
                {
                    //this.Cursor = new Cursor(Cursor.Current.Handle);
                    int newY = ConvertRange(0, monitor02.Height, 0, monitor01.Height, c.Y);

                    //Console.WriteLine("Alt: " + c.Y + " - Neu: " + newY);

                    Cursor.Position = new Point(c.X, newY);
                }

                if (last.X <= monitor01.Width && c.X >= monitor01.Width)
                {
                    //this.Cursor = new Cursor(Cursor.Current.Handle);
                    int newY = ConvertRange(0, monitor01.Height, 0, monitor02.Height, c.Y);

                    //Console.WriteLine("Alt: " + c.Y + " - Neu: " + newY);

                    Cursor.Position = new Point(c.X, newY);
                }

                last.X = c.X;
                last.Y = c.Y;

                Thread.Sleep(3);
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                monitor01.Width = Convert.ToInt16(txt_m01width.Text);
                monitor01.Height = Convert.ToInt16(txt_m01height.Text);
                monitor02.Width = Convert.ToInt16(txt_m02width.Text);
                monitor02.Height = Convert.ToInt16(txt_m02height.Text);

                notifyIcon1.Icon = SystemIcons.Application;
                notifyIcon1.BalloonTipText = "MouseTransition has minimized to tray";
                notifyIcon1.ShowBalloonTip(1000);
                this.Hide();
            }
            else if (this.WindowState == FormWindowState.Normal)
            {
                txt_m01width.Text = Convert.ToString(monitor01.Width);
                txt_m01height.Text = Convert.ToString(monitor01.Height);
                txt_m02width.Text = Convert.ToString(monitor02.Width);
                txt_m02height.Text = Convert.ToString(monitor02.Height);
            }
        }

        private void showSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loopThread.Abort();
            Application.Exit();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
