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
            Input = new Bitmap(GK_proj3.Properties.Resources.image);
            Output1 = new Bitmap(GK_proj3.Properties.Resources.image);
            Output2 = new Bitmap(GK_proj3.Properties.Resources.image);
            Output3 = new Bitmap(GK_proj3.Properties.Resources.image);
            pictureBox1.Image = Input;
            openFileDialog1.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";
            ColorPredefinedCB.Items.Add("sRGB");            //0
            ColorPredefinedCB.Items.Add("Adobe RGB");       //1
            ColorPredefinedCB.Items.Add("Apple RGB");       //2
            ColorPredefinedCB.Items.Add("CIE RGB");         //3
            ColorPredefinedCB.Items.Add("Wide Gamut");      //4
            ColorPredefinedCB.Items.Add("PAL/SECAM");       //5
            IlluminantCB.Items.Add("A");        //0
            IlluminantCB.Items.Add("B");        //1
            IlluminantCB.Items.Add("D50");      //2
            IlluminantCB.Items.Add("D55");      //3
            IlluminantCB.Items.Add("D65");      //4
            IlluminantCB.Items.Add("D75");      //5
            IlluminantCB.Items.Add("9300K");    //6
            IlluminantCB.Items.Add("E");        //7
            IlluminantCB.Items.Add("F2");       //8
            IlluminantCB.Items.Add("F7");       //9
            IlluminantCB.Items.Add("F11");      //10
            ColorPredefinedCB.SelectedIndex = 0;
            IlluminantCB.SelectedIndex = 0;
            UseCustomCB_CheckedChanged(null, null);
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
            label1.Text = "H";
            label2.Text = "S";
            label3.Text = "V";
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
            double h = 0, s, v;
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
                        h = 240D + (((r - g) * 60.0D / delta));
                    }
                }
                else
                {
                    max = g;
                    delta = max - min;
                    if (delta != 0)
                    {
                        h = 120D + (((b - r) * 60.0D / delta));
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
                        h = 0D + (((g - b) * 60.0D / delta));
                    }
                }
                else
                {
                    max = b;
                    delta = max - min;
                    if (delta != 0)
                    {
                        h = 240D + (((r - g) * 60.0D / delta));
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
                        h = 0D + (((g - b) * 60.0D / delta));
                    }
                }
                else
                {
                    max = g;
                    delta = max - min;
                    if (delta != 0)
                    {
                        h = 120D + (((b - r) * 60.0D / delta));
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
            for (int i = 0; i < Input.Width; i++)
                for (int j = 0; j < Input.Height; j++)
                    YCbCrPixel(i, j);
            label1.Text = "Y";
            label2.Text = "Cb";
            label3.Text = "Cr";
            pictureBox2.Image = Output1;
            pictureBox3.Image = Output2;
            pictureBox4.Image = Output3;
        }

        private void YCbCrPixel(int x, int y)
        {
            Color c = Input.GetPixel(x, y);
            double r = c.R;
            double g = c.G;
            double b = c.B;
            double Y = 16D + 65.738D * r / 256D + 129.057D * g / 256D + 25.064D * b / 256D;
            double Cb = 128D - 37.945D * r / 256D - 74.494D * g / 256D + 112.439D * b / 256D;
            double Cr = 128D + 112.439D * r / 256D - 94.154D * g / 256D - 18.285D * b / 256D;


            Output1.SetPixel(x, y, Color.FromArgb((int)Y, (int)Y, (int)Y));
            Output2.SetPixel(x, y, Color.FromArgb((int)127, (int)(255 - Cb), (int)Cb));
            Output3.SetPixel(x, y, Color.FromArgb((int)Cr, (int)(255 - Cr), (int)127));
        }

        private void ProcessLab()
        {
            double Xr = (double)(NumRX.Value / NumRY.Value);
            double Yr = 1D;
            double Zr = (double)((1M - NumRX.Value - NumRY.Value) / NumRY.Value);
            double Xg = (double)(NumGX.Value / NumGY.Value);
            double Yg = 1D;
            double Zg = (double)((1M - NumGX.Value - NumGY.Value) / NumGY.Value);
            double Xb = (double)(NumRX.Value / NumRY.Value);
            double Yb = 1D;
            double Zb = (double)((1M - NumBX.Value - NumBY.Value) / NumBY.Value);
            double Xw = (double)(NumWX.Value / NumWY.Value);
            double Yw = 1D;
            double Zw = (double)((1M - NumWX.Value - NumWY.Value) / NumWY.Value);

            //http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html
            //https://stackoverflow.com/questions/983999/simple-3x3-matrix-inverse-code-c
            double det = Xr * (Yg * Zb - Yb * Zg) -
                         Xg * (Yr * Zb - Yb * Zr) +
                         Xb * (Yr * Zg - Yg * Zr);
            det = 1 / det;
            double[,] XYZinv = new double[3, 3];
            XYZinv[0, 0] = (Yg * Zb - Yb * Zg) * det;
            XYZinv[0, 1] = (Xb * Zg - Xg * Zb) * det;
            XYZinv[0, 2] = (Xg * Yb - Xb * Yg) * det;
            XYZinv[1, 0] = (Yb * Zr - Yr * Zb) * det;
            XYZinv[1, 1] = (Xr * Zb - Xb * Zr) * det;
            XYZinv[1, 2] = (Yr * Xb - Xr * Yb) * det;
            XYZinv[2, 0] = (Yr * Zg - Yg * Zr) * det;
            XYZinv[2, 1] = (Zr * Xg - Zg * Xr) * det;
            XYZinv[2, 2] = (Xr * Yg - Xg * Yr) * det;

            double Sr = XYZinv[0, 0] * Xw + XYZinv[0, 1] * Xw + XYZinv[0, 2] * Xw;
            double Sg = XYZinv[1, 0] * Yw + XYZinv[1, 1] * Yw + XYZinv[1, 2] * Yw;
            double Sb = XYZinv[2, 0] * Zw + XYZinv[2, 1] * Zw + XYZinv[2, 2] * Zw;

            double[,] M = new double[3, 3];

            M[0, 0] = Sr * Xr;
            M[0, 1] = Sg * Xg;
            M[0, 2] = Sb * Xb;
            M[1, 0] = Sr * Yr;
            M[1, 1] = Sg * Yg;
            M[1, 2] = Sb * Yb;
            M[2, 0] = Sr * Zr;
            M[2, 1] = Sg * Zg;
            M[2, 2] = Sb * Zb;


            for (int i = 0; i < Input.Width; i++)
                for (int j = 0; j < Input.Height; j++)
                    LabPixel(i, j, M, Xw, Yw, Zw);
            label1.Text = "L";
            label2.Text = "a";
            label3.Text = "b";
            pictureBox2.Image = Output1;
            pictureBox3.Image = Output2;
            pictureBox4.Image = Output3;
        }

        private void LabPixel(int xpos, int ypos, double[,] M, double Xw, double Yw, double Zw)
        {
            Color c = Input.GetPixel(xpos, ypos);
            double red = c.R / 255D;
            double green = c.G / 255D;
            double blue = c.B / 255D;
            red = Math.Pow(red, (double)NumGamma.Value);
            green = Math.Pow(green, (double)NumGamma.Value);
            blue = Math.Pow(blue, (double)NumGamma.Value);
            double L;
            double a;
            double b;

            double x = M[0, 0] * red + M[0, 1] * red + M[0, 2] * red;
            double y = M[1, 0] * green + M[1, 1] * green + M[1, 2] * green;
            double z = M[2, 0] * blue + M[2, 1] * blue + M[2, 2] * blue;

            double xr = x / Xw;
            double yr = y / Yw;
            double zr = z / Zw;

            if (yr > 0.008856D)
            {
                L = 116D * Math.Pow(yr, 1.0D / 3.0D) - 16;
            }
            else
            {
                L = 903.3D * yr;
            }
            a = 500D * (Math.Pow(xr, 1.0D / 3.0D) - Math.Pow(yr, 1.0D / 3.0D));
            b = 200D * (Math.Pow(yr, 1.0D / 3.0D) - Math.Pow(zr, 1.0D / 3.0D));

            a += 127D;
            a = a * 256D / 255D;
            b += 127D;
            b = b * 256D / 255D;

            if (L > 255) L = 255;
            if (L < 0) L = 0;
            if (a > 255) a = 255;
            if (a < 0) a = 0;
            if (b > 255) b = 255;
            if (b < 0) b = 0;

            Output1.SetPixel(xpos, ypos, Color.FromArgb((int)L, (int)L, (int)L));
            Output2.SetPixel(xpos, ypos, Color.FromArgb((int)a, (int)(255 - a), (int)127));
            Output3.SetPixel(xpos, ypos, Color.FromArgb((int)b, (int)127, (int)(255 - b)));
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

        private void GreyB_Click(object sender, EventArgs e)
        {

            ProcessGreyScale();
            pictureBox1.Image = Input;
        }

        private void ColorProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(ColorPredefinedCB.SelectedIndex)
            {
                case 0:
                    ColorPredefinedChangeTo_sRGB();
                    IlluminantCB.SelectedIndex = 4;
                    break;
                case 1:
                    ColorPredefinedChangeTo_AdobeRGB();
                    IlluminantCB.SelectedIndex = 4;
                    break;
                case 2:
                    ColorPredefinedChangeTo_AppleRGB();
                    IlluminantCB.SelectedIndex = 4;
                    break;
                case 3:
                    ColorPredefinedChangeTo_CIERGB();
                    IlluminantCB.SelectedIndex = 7;
                    break;
                case 4:
                    ColorPredefinedChangeTo_WideGamut();
                    IlluminantCB.SelectedIndex = 2;
                    break;
                case 5:
                    ColorPredefinedChangeTo_PALSECAM();
                    IlluminantCB.SelectedIndex = 4;
                    break;
            }
            Illuminant_SelectedIndexChanged(null, null);
        }

        private void Illuminant_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (IlluminantCB.SelectedIndex)
            {
                case 0:
                    IlluminantChangeTo_A();
                    break;
                case 1:
                    IlluminantChangeTo_B();
                    break;
                case 2:
                    IlluminantChangeTo_D50();
                    break;
                case 3:
                    IlluminantChangeTo_D55();
                    break;
                case 4:
                    IlluminantChangeTo_D65();
                    break;
                case 5:
                    IlluminantChangeTo_D75();
                    break;
                case 6:
                    IlluminantChangeTo_9300K();
                    break;
                case 7:
                    IlluminantChangeTo_E();
                    break;
                case 8:
                    IlluminantChangeTo_F2();
                    break;
                case 9:
                    IlluminantChangeTo_F7();
                    break;
                case 10:
                    IlluminantChangeTo_F11();
                    break;
            }
        }

        private void UseCustomCB_CheckedChanged(object sender, EventArgs e)
        {
            if (UseCustomCB.Checked)
            {
                ColorPredefinedCB.Enabled = false;
                IlluminantCB.Enabled = false;
                NumRX.Enabled = true;
                NumRY.Enabled = true;
                NumGX.Enabled = true;
                NumGY.Enabled = true;
                NumBX.Enabled = true;
                NumBY.Enabled = true;
                NumWX.Enabled = true;
                NumWY.Enabled = true;
                NumGamma.Enabled = true;
            }
            else
            {
                ColorPredefinedCB.Enabled = true;
                IlluminantCB.Enabled = true;
                NumRX.Enabled = false;
                NumRY.Enabled = false;
                NumGX.Enabled = false;
                NumGY.Enabled = false;
                NumBX.Enabled = false;
                NumBY.Enabled = false;
                NumWX.Enabled = false;
                NumWY.Enabled = false;
                NumGamma.Enabled = false;
                ColorProfile_SelectedIndexChanged(null, null);
                Illuminant_SelectedIndexChanged(null, null);
            }
        }

        private void ColorPredefinedChangeTo_sRGB()
        {
            NumRX.Value = 0.6400M;
            NumRY.Value = 0.3300M;
            NumGX.Value = 0.3000M;
            NumGY.Value = 0.6000M;
            NumBX.Value = 0.1500M;
            NumBY.Value = 0.0600M;
            NumGamma.Value = 2.2000M;
        }

        private void ColorPredefinedChangeTo_AdobeRGB()
        {
            NumRX.Value = 0.6400M;
            NumRY.Value = 0.3300M;
            NumGX.Value = 0.2100M;
            NumGY.Value = 0.7100M;
            NumBX.Value = 0.1500M;
            NumBY.Value = 0.0600M;
            NumGamma.Value = 2.2000M;
        }

        private void ColorPredefinedChangeTo_AppleRGB()
        {
            NumRX.Value = 0.6250M;
            NumRY.Value = 0.3400M;
            NumGX.Value = 0.2800M;
            NumGY.Value = 0.5950M;
            NumBX.Value = 0.1550M;
            NumBY.Value = 0.0700M;
            NumGamma.Value = 1.8000M;
        }

        private void ColorPredefinedChangeTo_CIERGB()
        {
            NumRX.Value = 0.7350M;
            NumRY.Value = 0.2650M;
            NumGX.Value = 0.2740M;
            NumGY.Value = 0.7170M;
            NumBX.Value = 0.1670M;
            NumBY.Value = 0.0090M;
            NumGamma.Value = 2.2000M;
        }

        private void ColorPredefinedChangeTo_WideGamut()
        {
            NumRX.Value = 0.7347M;
            NumRY.Value = 0.2653M;
            NumGX.Value = 0.1152M;
            NumGY.Value = 0.8264M;
            NumBX.Value = 0.1566M;
            NumBY.Value = 0.0177M;
            NumGamma.Value = 1.2000M;
        }

        private void ColorPredefinedChangeTo_PALSECAM()
        {
            NumRX.Value = 0.6400M;
            NumRY.Value = 0.3300M;
            NumGX.Value = 0.2900M;
            NumGY.Value = 0.6000M;
            NumBX.Value = 0.1500M;
            NumBY.Value = 0.0600M;
            NumGamma.Value = 1.9500M;
        }

        private void IlluminantChangeTo_A()
        {
            NumWX.Value = 0.44757M;
            NumWY.Value = 0.40744M;
        }

        private void IlluminantChangeTo_B()
        {
            NumWX.Value = 0.34840M;
            NumWY.Value = 0.35160M;
        }

        private void IlluminantChangeTo_D50()
        {
            NumWX.Value = 0.34567M;
            NumWY.Value = 0.35850M;
        }

        private void IlluminantChangeTo_D55()
        {
            NumWX.Value = 0.33242M;
            NumWY.Value = 0.34743M;
        }

        private void IlluminantChangeTo_D65()
        {
            NumWX.Value = 0.31273M;
            NumWY.Value = 0.32902M;
        }

        private void IlluminantChangeTo_D75()
        {
            NumWX.Value = 0.29902M;
            NumWY.Value = 0.31485M;
        }

        private void IlluminantChangeTo_9300K()
        {
            NumWX.Value = 0.28480M;
            NumWY.Value = 0.29320M;
        }

        private void IlluminantChangeTo_E()
        {
            NumWX.Value = 0.33333M;
            NumWY.Value = 0.33333M;
        }

        private void IlluminantChangeTo_F2()
        {
            NumWX.Value = 0.37207M;
            NumWY.Value = 0.37512M;
        }

        private void IlluminantChangeTo_F7()
        {
            NumWX.Value = 0.31285M;
            NumWY.Value = 0.32918M;
        }

        private void IlluminantChangeTo_F11()
        {
            NumWX.Value = 0.38054M;
            NumWY.Value = 0.37691M;
        }
    }
}
