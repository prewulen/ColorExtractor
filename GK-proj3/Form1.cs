using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GK_proj3
{
    public partial class Form1 : Form
    {
        Bitmap Input;
        Bitmap Output1;
        Bitmap Output2;
        Bitmap Output3;

        public Form1()
        {
            InitializeComponent();
            Input = new Bitmap("G:\\osu piosnki\\gameover_cut.png");
            Output1 = new Bitmap("G:\\osu piosnki\\gameover_cut.png");
            Output2 = new Bitmap("G:\\osu piosnki\\gameover_cut.png");
            Output3 = new Bitmap("G:\\osu piosnki\\gameover_cut.png");
            pictureBox1.Image = Input;
            openFileDialog1.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";
        }

        private void ChangeImageB_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Input = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = Input;
                pictureBox2.Image = null;
                pictureBox3.Image = null;
                pictureBox4.Image = null;
                Output1 = new Bitmap(Input.Width, Input.Height);
                Output2 = new Bitmap(Input.Width, Input.Height);
                Output3 = new Bitmap(Input.Width, Input.Height);
            }
        }

        private void ProcessGreyScale()
        {
            for (int i = 0; i < Input.Width; i++)
                for (int j = 0; j < Input.Height; j++)
                    Input.SetPixel(i, j, RGB2GreyScale(Input.GetPixel(i, j)));
        }

        private Color RGB2GreyScale(Color c)
        {
            int g = (int)((0.2126D * (c.R)) + (0.7152D * (c.G)) + (0.0722D * (c.B)));
            return Color.FromArgb(g,g,g);
        }

        private void ProcessHSV()
        {
            for (int i = 0; i < Input.Width; i++)
                for (int j = 0; j < Input.Height; j++)
                    HSVPixel(i, j);
            pictureBox2.Image = Output1;
            pictureBox3.Image = Output2;
            pictureBox4.Image = Output3;
        }

        private void HSVPixel(int x, int y)
        {
            Color c = Input.GetPixel(x, y);
            double r = c.R / 255D;
            double g = c.G / 255D;
            double b = c.B / 255D;
            double h = 0, s = 0, v = 0;
            double delta;

            double min;
            double max;
            if (r <= g && r <= b)
            {
                min = r;
                if (b >= g)
                {
                    max = b;
                    delta = max - min;
                    if (delta != 0)
                    {
                        h = 60.0D * (((r - g) / delta) + 4);
                    }
                }
                else
                {
                    max = g;
                    delta = max - min;
                    if (delta != 0)
                    {
                        h = 60.0D * (((b - r) / delta) + 2);
                    }
                }
            }
            else if (g <= b && g <= r)
            {
                min = g;
                if (r >= b)
                {
                    max = r;
                    delta = max - min;
                    if (delta != 0)
                    {
                        h = 60.0D * (((g - b) / delta));
                    }
                }
                else
                {
                    max = b;
                    delta = max - min;
                    if (delta != 0)
                    {
                        h = 60.0D * (((r - g) / delta) + 4);
                    }
                }
            }
            else
            {
                min = b;
                if (r >= g)
                {
                    max = r;
                    delta = max - min;
                    if (delta != 0)
                    {
                        h = 60.0D * (((g - b) / delta));
                    }
                }
                else
                {
                    max = g;
                    delta = max - min;
                    if (delta != 0)
                    {
                        h = 60.0D * (((b - r) / delta) + 2);
                    }
                }
            }
            if (delta == 0)
                h = 0;
            if (max == 0)
                s = 0;
            else
                s = delta / max;
            v = max;
            if (h < 0)
                h += 360;
            h = h * 255D / 360D;
            s = s * 255D;
            v = v * 255D;
            if (h > 255) h = 255;
            if (s > 255) s = 255;
            if (v > 255) v = 255;
            if (h < 0) h = 0;
            if (s < 0) s = 0;
            if (v < 0) v = 0;

            Output1.SetPixel(x, y, Color.FromArgb((int)h, (int)h, (int)h));
            Output2.SetPixel(x, y, Color.FromArgb((int)s, (int)s, (int)s));
            Output3.SetPixel(x, y, Color.FromArgb((int)v, (int)v, (int)v));

            
        }

        private void ProcessYCbCr()
        {

        }

        private void ProcessLab()
        {

        }

        private void SeparateColorsB_Click(object sender, EventArgs e)
        {
            if (YCbCrRadio.Checked)
            {
                ProcessYCbCr();
            }
            else if (HSVRadio.Checked)
            {
                ProcessHSV();
            }
            else if (LabRadio.Checked)
            {
                ProcessLab();
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {

            ProcessGreyScale();
            pictureBox1.Image = Input;
        }
    }
}
