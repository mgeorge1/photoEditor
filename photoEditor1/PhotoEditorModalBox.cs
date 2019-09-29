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

namespace photoEditor1
{
    public partial class PhotoEditorModalBox : Form
    {
        private string filePath;
        private Bitmap myImage;
        public PhotoEditorModalBox(String newFilePath)
        {
            InitializeComponent();
            filePath = newFilePath;
            pathLabel.Text = filePath;
            FileInfo file = new FileInfo(newFilePath);
            file.IsReadOnly = false;

        }
        
        private void InvertColors()
        {
            for(int y = 0; y < myImage.Height; y++)
            {
                for(int x = 0; x < myImage.Width; x++)
                {
                    Color color = myImage.GetPixel(x, y);
                    int newRed = Math.Abs(color.R - 255);
                    int newGreen = Math.Abs(color.G - 255);
                    int newBlue = Math.Abs(color.B - 255);
                    Color newColor = Color.FromArgb(newRed, newGreen, newBlue);
                    myImage.SetPixel(x, y, newColor);
                }
            }
            pictureBox.Image = (Image)myImage;
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
            InvertColors();
        }
    }
}
