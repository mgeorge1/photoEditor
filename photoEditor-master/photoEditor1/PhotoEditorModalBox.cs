using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace photoEditor1
{
    public partial class PhotoEditorModalBox : Form
    {
        private string filePath;
        private Bitmap myImage;
        private CancellationTokenSource cancellationTokenSource;
        public PhotoEditorModalBox(String newFilePath)
        {
            InitializeComponent();
            filePath = newFilePath;
            pathLabel.Text = filePath;
            FileInfo file = new FileInfo(newFilePath);
            file.IsReadOnly = false;

        }
        
        private async Task InvertColors()
        {
            UseWaitCursor = true;

            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;

            await Task.Run(() =>
            {
                for (int y = 0; y < myImage.Height; y++)
                {
                    for (int x = 0; x < myImage.Width; x++)
                    {
                        Color color = myImage.GetPixel(x, y);
                        int newRed = Math.Abs(color.R - 255);
                        int newGreen = Math.Abs(color.G - 255);
                        int newBlue = Math.Abs(color.B - 255);

                        Color newColor = Color.FromArgb(newRed, newGreen, newBlue);
                        myImage.SetPixel(x, y, newColor);

                        if (token.IsCancellationRequested)
                            break;
                    }
                }
                pictureBox.Image = (Image)myImage;
            }, token);

            UseWaitCursor = false;
        }

        private async Task ChangeBrightness()
        {
            int amount = Convert.ToInt32(2 * (50 - brightnessSlider.Value) * 0.01 * 255);

            UseWaitCursor = true;

            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;

            await Task.Run(() =>
            {
                for (int y = 0; y < myImage.Height; y++)
                {
                    for (int x = 0; x < myImage.Width; x++)
                    {
                        Color color = myImage.GetPixel(x, y);
                        int newRed = color.R - amount;
                        int newGreen = color.G - amount;
                        int newBlue = color.B - amount;

                        if (newRed < 0)
                            newRed = 0;
                        else if (newRed > 255)
                            newRed = 255;

                        if (newGreen < 0)
                            newGreen = 0;
                        else if (newGreen > 255)
                            newGreen = 255;

                        if (newBlue < 0)
                            newBlue = 0;
                        else if (newBlue > 255)
                            newBlue = 255;

                        Color newColor = Color.FromArgb(newRed, newGreen, newBlue);
                        myImage.SetPixel(x, y, newColor);
                    }
                }
                pictureBox.Image = (Image)myImage;
            }, token);

            UseWaitCursor = false;
        }

        private async Task ChangeColor(Color pickedColor)
        {
            UseWaitCursor = true;

            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;

            await Task.Run(() =>
            {
                for (int y = 0; y < myImage.Height; y++)
                {
                    for (int x = 0; x < myImage.Width; x++)
                    {
                        Color color = myImage.GetPixel(x, y);
                        double average = (color.R + color.G + color.B) / 3;
                        double percentage = average / 255;

                        int newRed = (int)(pickedColor.R * percentage);
                        int newGreen = (int)(pickedColor.G * percentage);
                        int newBlue = (int)(pickedColor.B * percentage);

                        Color newColor = Color.FromArgb(newRed, newGreen, newBlue);
                        myImage.SetPixel(x, y, newColor);
                    }
                }
                pictureBox.Image = (Image)myImage;
            }, token);

            UseWaitCursor = false;
        }

        private async void TransformPhoto(string selectedTransformation, Color color)
        {
            var transformedBitmap = pictureBox.Image;

            if (selectedTransformation == "invert")
            {
                await InvertColors();
            }
            else if (selectedTransformation == "changeBrightness")
            {
                await ChangeBrightness();
            }
            else if(selectedTransformation == "changeColor")
            {
                await ChangeColor(color);
            }
        }

        private void PhotoEditorModalBox_Load(object sender, EventArgs e)
        {
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            myImage = new Bitmap(filePath);
            pictureBox.Image = (Image)myImage;                   
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            //File.Replace()
            var image = myImage;
            pictureBox.Image = null;
            File.Delete(filePath);
            image.Save(filePath, ImageFormat.Jpeg);
        }

        private void InvertButton_Click(object sender, EventArgs e)
        {
            Color color = Color.White;
            TransformPhoto("invert", color);
        }

        private void ColorButton_Click(object sender, EventArgs e)
        {
            // TransformPhoto("changeColor");
            Color selectedColor;
            ColorDialog colorDialog = new ColorDialog();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                selectedColor = colorDialog.Color;
                TransformPhoto("changeColor", selectedColor);
            }
           
        }

        private void brightnessSlider_MouseUp(object sender, MouseEventArgs e)
        {
            Color color = Color.White;
            TransformPhoto("changeBrightness", color);
        }
    }
}
