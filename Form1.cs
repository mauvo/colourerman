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
        public Form1()
        {
            InitializeComponent();
            this.pictureBox1.Location = new Point(10, 20);
            this.pictureBox1.Width = 624;
            this.pictureBox1.Height = 452;
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            
            this.pictureBox2.Location = new Point(0, 0);
            this.pictureBox2.Width = 624;
            this.pictureBox2.Height = 452;
            this.pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;

            this.pictureBox4.Location = new Point(0, 0);
            this.pictureBox4.Width = 624;
            this.pictureBox4.Height = 452;
            this.pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;

            this.pictureBox6.Location = new Point(0, 0);
            this.pictureBox6.Width = 624;
            this.pictureBox6.Height = 452;
            this.pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;

            this.pictureBox1.Controls.Add(this.pictureBox2);
            this.pictureBox2.Controls.Add(this.pictureBox4);
            this.pictureBox4.Controls.Add(this.pictureBox6);

        }

        private void UpdatePreview()
        {
            UpdatePrimary();
            UpdateSecondary();
            UpdateTertiary();

            pictureBox1.Refresh();
        }

        private void UpdateTertiary()
        {
            var tertiaryColor = pictureBox7.BackColor;

            var bmp = new Bitmap(Layers.tertiary);
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    var pixel = bmp.GetPixel(i, j);
                    if (pixel.A == 0)
                        continue;
                    var brightness = pixel.GetBrightness();
                    Color color;
                    if (brightness > 0.5)
                        color = tertiaryColor.Lerp(Color.White, brightness - 0.5f);
                    else
                        color = tertiaryColor.Lerp(Color.Black, 0.5f - brightness);

                    bmp.SetPixel(i, j, color);
                }
            }

            this.pictureBox6.Image = bmp;
        }

        private void UpdateSecondary()
        {
            var secondaryColor = pictureBox5.BackColor;
            
            var bmp = new Bitmap(Layers.secondary);
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    var pixel = bmp.GetPixel(i, j);
                    if (pixel.A == 0)
                        continue;
                    var brightness = pixel.GetBrightness();
                    Color color;
                    if (brightness > 0.5)
                        color = secondaryColor.Lerp(Color.White, brightness - 0.5f);
                    else
                        color = secondaryColor.Lerp(Color.Black, 0.5f - brightness);

                    bmp.SetPixel(i, j, color);
                }
            }

            this.pictureBox4.Image = bmp;
        }

        private void UpdatePrimary()
        { 
            var primaryColor = pictureBox3.BackColor;
            
            var bmp = new Bitmap(Layers.primary);
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    var pixel = bmp.GetPixel(i, j);
                    if (pixel.A == 0)
                        continue;
                    var brightness = pixel.GetBrightness();
                    var color = Color.FromArgb((int)(primaryColor.R * brightness), (int)(primaryColor.G * brightness), (int)(primaryColor.B*brightness));
                    bmp.SetPixel(i, j, color);
                }
            }

            this.pictureBox2.Image = bmp;
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
                var bmp = new Bitmap(pictureBox1.Image);
                OverlayBitmap(bmp, new Bitmap(pictureBox2.Image));
                OverlayBitmap(bmp, new Bitmap(pictureBox4.Image));
                OverlayBitmap(bmp, new Bitmap(pictureBox6.Image));

                bmp.Save((System.IO.FileStream)saveFileDialog1.OpenFile(), System.Drawing.Imaging.ImageFormat.Png);
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
