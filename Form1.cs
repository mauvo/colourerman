using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ColourerMan
{
    public partial class Form1 : Form
    {
        private Bitmap preview;

        public Form1()
        {
            InitializeComponent();
            this.pictureBox1.Location = new Point(10, 20);
            this.pictureBox1.Width = 624;
            this.pictureBox1.Height = 452;

            UpdatePreview();

        }

        private Bitmap Scale(Bitmap orig)
        {
            var scaled = new Bitmap(orig.Width * 4, orig.Height * 4);

            for (int x = 0; x < orig.Width; x++)
            {
                for (int y = 0; y < orig.Height; y++)
                {
                    var sx = x * 4;
                    var sy = y * 4;
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            scaled.SetPixel(sx + i, sy + j, orig.GetPixel(x, y));
                        }
                    }
                }
            }
            return scaled;
        }

        private void UpdatePreview()
        {
            preview = new Bitmap(Layers._base);

            UpdatePrimary(preview);
            UpdateSecondary(preview);
            UpdateTertiary(preview);

            pictureBox1.Image = Scale(preview);
        }

        private void UpdateTertiary(Bitmap preview)
        {
            var tertiaryColor = pictureBox7.BackColor;

            var layer = Layers.tertiary;
            for (int i = 0; i < layer.Width; i++)
            {
                for (int j = 0; j < layer.Height; j++)
                {
                    var pixel = layer.GetPixel(i, j);
                    if (pixel.A == 0)
                        continue;
                    var brightness = pixel.GetBrightness();
                    Color color;
                    if (brightness > 0.5)
                        color = tertiaryColor.Lerp(Color.White, brightness - 0.5f);
                    else
                        color = tertiaryColor.Lerp(Color.Black, 0.5f - brightness);

                    preview.SetPixel(i, j, color);
                }
            }
        }

        private void UpdateSecondary(Bitmap preview)
        {
            var secondaryColor = pictureBox5.BackColor;

            var layer = Layers.secondary;
            for (int i = 0; i < layer.Width; i++)
            {
                for (int j = 0; j < layer.Height; j++)
                {
                    var pixel = layer.GetPixel(i, j);
                    if (pixel.A == 0)
                        continue;
                    var brightness = pixel.GetBrightness();
                    Color color;
                    if (brightness > 0.5)
                        color = secondaryColor.Lerp(Color.White, brightness - 0.5f);
                    else
                        color = secondaryColor.Lerp(Color.Black, 0.5f - brightness);

                    preview.SetPixel(i, j, color);
                }
            }
        }

        private void UpdatePrimary(Bitmap preview)
        { 
            var primaryColor = pictureBox3.BackColor;

            var layer = Layers.primary;
            for (int i = 0; i < layer.Width; i++)
            {
                for (int j = 0; j < layer.Height; j++)
                {
                    var pixel = layer.GetPixel(i, j);
                    if (pixel.A == 0)
                        continue;
                    var brightness = pixel.GetBrightness();
                    var color = Color.FromArgb((int)(primaryColor.R * brightness), (int)(primaryColor.G * brightness), (int)(primaryColor.B*brightness));
                    preview.SetPixel(i, j, color);
                }
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox3.BackColor = colorDialog1.Color;
                UpdatePreview();
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox5.BackColor = colorDialog1.Color;
                UpdatePreview();
            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox7.BackColor = colorDialog1.Color;
                UpdatePreview();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                preview.Save((System.IO.FileStream)saveFileDialog1.OpenFile(), System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        private void OverlayBitmap(Bitmap underlay, Bitmap overlay)
        {
            for (int i = 0; i < underlay.Width; i++)
            {
                for (int j = 0; j < underlay.Height; j++)
                {
                    var pixel = overlay.GetPixel(i, j);
                    if (pixel.A == 0)
                        continue;

                    underlay.SetPixel(i, j, Color.FromArgb(pixel.R, pixel.G, pixel.B));
                }
            }
        }
    }
}
