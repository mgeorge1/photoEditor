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
        private Bitmap transformedBitmap;
        private bool isCancelled = false;
        private ProgressDialogBox progressDialogBox;

        public PhotoEditorModalBox(String newFilePath)
        {
            InitializeComponent();
            filePath = newFilePath;
            FileInfo file = new FileInfo(newFilePath);
            file.IsReadOnly = false;
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            myImage = new Bitmap(filePath);
            pictureBox.Image = (Image)myImage;

        }
        
        private async Task InvertColors()
        {
            UseWaitCursor = true;

            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            

            await Task.Run(() =>
            {
                double num = 0;
                double percentChange = 1.0 / transformedBitmap.Width / transformedBitmap.Height;
                double percentage = 0;
                int percentageNumber = 1;
                double count = 0;
                for (int y = 0; y < transformedBitmap.Height; y++)
                {
                    for (int x = 0; x < transformedBitmap.Width; x++)
                    {
                        Color color = transformedBitmap.GetPixel(x, y);
                        int newRed = Math.Abs(color.R - 255);
                        int newGreen = Math.Abs(color.G - 255);
                        int newBlue = Math.Abs(color.B - 255);

                        Color newColor = Color.FromArgb(newRed, newGreen, newBlue);
                        transformedBitmap.SetPixel(x, y, newColor);

                        if (token.IsCancellationRequested)
                        {
                            y = transformedBitmap.Height;
                            x = transformedBitmap.Width;
                            break;
                        }
                        count++;
                        percentage += (percentChange * 100);
                        if(percentageNumber < percentage)
                        {
                            num = count / (transformedBitmap.Height * transformedBitmap.Width);
                            try
                            {
                                Invoke((Action)delegate ()
                                {
                                    progressDialogBox.ProgressValue = (int)(num * 100);
                                });
                            }
                            catch (ObjectDisposedException)
                            {

                            }
                            percentageNumber++;
                        }
                        
                    }
                }
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
                double num = 0;
                double percentChange = 1.0 / transformedBitmap.Width / transformedBitmap.Height;
                double percentage = 0;
                int percentageNumber = 1;
                double count = 0;
                for (int y = 0; y < transformedBitmap.Height; y++)
                {
                    for (int x = 0; x < transformedBitmap.Width; x++)
                    {
                        Color color = transformedBitmap.GetPixel(x, y);
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
                        transformedBitmap.SetPixel(x, y, newColor);

                        if (token.IsCancellationRequested)
                        {
                            y = transformedBitmap.Height;
                            x = transformedBitmap.Width;
                            break;
                        }

                        count++;
                        percentage += (percentChange * 100);
                        if (percentageNumber < percentage)
                        {
                            num = count / (transformedBitmap.Height * transformedBitmap.Width);
                            try
                            {
                                Invoke((Action)delegate ()
                                {
                                    progressDialogBox.ProgressValue = (int)(num * 100);
                                });
                            }
                            catch (ObjectDisposedException)
                            {

                            }
                            percentageNumber++;
                        }
                    }
                }
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
                double num = 0;
                double percentChange = 1.0 / transformedBitmap.Width / transformedBitmap.Height;
                double percentage = 0;
                int percentageNumber = 1;
                double count = 0;
                for (int y = 0; y < transformedBitmap.Height; y++)
                {
                    for (int x = 0; x < transformedBitmap.Width; x++)
                    {
                        Color color = transformedBitmap.GetPixel(x, y);
                        double average = (color.R + color.G + color.B) / 3;
                        double percentageColor = average / 255;

                        int newRed = (int)(pickedColor.R * percentageColor);
                        int newGreen = (int)(pickedColor.G * percentageColor);
                        int newBlue = (int)(pickedColor.B * percentageColor);

                        Color newColor = Color.FromArgb(newRed, newGreen, newBlue);
                        transformedBitmap.SetPixel(x, y, newColor);

                        if (token.IsCancellationRequested)
                        {
                            y = transformedBitmap.Height;
                            x = transformedBitmap.Width;
                            break;
                        }

                        count++;
                        percentage += (percentChange * 100);
                        if (percentageNumber < percentage)
                        {
                            num = count / (transformedBitmap.Height * transformedBitmap.Width);
                            try
                            {
                                Invoke((Action)delegate ()
                                {
                                    progressDialogBox.ProgressValue = (int)(num * 100);
                                });
                            }
                            catch (ObjectDisposedException)
                            {

                            }
                            percentageNumber++;
                        }
                    }
                }
            }, token);

            UseWaitCursor = false;
        }

        private async void TransformPhoto(string selectedTransformation, Color color)
        {
            progressDialogBox = new ProgressDialogBox();
            progressDialogBox.Canceled += new EventHandler<EventArgs>(CancelOnProgressDialogPressed);
            progressDialogBox.Show();
            Image clone = (Image)pictureBox.Image.Clone();
            transformedBitmap = (Bitmap)clone;

            cancelButton.Enabled = false;
            saveButton.Enabled = false;
            invertButton.Enabled = false;
            colorButton.Enabled = false;
            brightnessSlider.Enabled = false;

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
            progressDialogBox.Close();

            cancelButton.Enabled = true;
            saveButton.Enabled = true;
            invertButton.Enabled = true;
            colorButton.Enabled = true;
            brightnessSlider.Enabled = true;
            this.BringToFront();

            if (!isCancelled)
            {
                pictureBox.Image = transformedBitmap;
            }
        }

        private void CancelOnProgressDialogPressed(object sender, EventArgs e)
        {
            isCancelled = true;
            cancellationTokenSource.Cancel();
        }

        private void PhotoEditorModalBox_Load(object sender, EventArgs e)
        {
                              
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            //File.Replace()
            //var image = pictureBox.Image;
            //pictureBox.Image = null;
            //File.Delete(filePath);
            //image.Save(filePath, ImageFormat.Jpeg);
            pictureBox.Image.Save("IHATETHIS.jpeg", ImageFormat.Jpeg);
            this.Close();
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
