using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Image_Viewer
{
    public partial class Form1 : Form
    {
        private string[] imageFiles;
        private int currentIndex = 0;
        private float zoomFactor = 1.0f;
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonLoadFolder_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderBrowserDialog.SelectedPath;
                    imageFiles = Directory.GetFiles(selectedPath, "*.*")
                      .Where(file => file.EndsWith(".jpg") || file.EndsWith(".png") || file.EndsWith(".bmp"))
                      .ToArray();

                    if (imageFiles.Length > 0)
                    {
                        currentIndex = 0; LoadImage(imageFiles[currentIndex]);
                    }
                    else
                    {
                        MessageBox.Show("В выбранной папке нет изображений.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void LoadImage(string filePath)
        {
            var image = Image.FromFile(filePath); pictureBox.Image = image; zoomFactor = 1.0f; AdjustImageSize(image);
        }

        private void AdjustImageSize(Image image)
        {
            if (image != null)
            {
                int newWidth = (int)(image.Width * zoomFactor);
                int newHeight = (int)(image.Height * zoomFactor);

                if (newWidth <= 0 || newHeight <= 0)
                {
                    pictureBox.Image = image;
                    pictureBox.SizeMode = PictureBoxSizeMode.Zoom; return;
                }

                if (newWidth > pictureBox.Width || newHeight > pictureBox.Height)
                {
                    float widthRatio = (float)pictureBox.Width / image.Width;
                    float heightRatio = (float)pictureBox.Height / image.Height;
                    float minRatio = Math.Min(widthRatio, heightRatio);

                    newWidth = (int)(image.Width * minRatio);
                    newHeight = (int)(image.Height * minRatio);
                }

                pictureBox.Image = new Bitmap(image, newWidth, newHeight); pictureBox.SizeMode = PictureBoxSizeMode.Zoom; pictureBox.Refresh();
            }
        }


        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (imageFiles != null && imageFiles.Length > 0)
            {
                currentIndex = (currentIndex + 1) % imageFiles.Length; LoadImage(imageFiles[currentIndex]);
            }
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            if (imageFiles != null && imageFiles.Length > 0)
            {
                currentIndex = (currentIndex - 1 + imageFiles.Length) % imageFiles.Length; LoadImage(imageFiles[currentIndex]);
            }
        }

        private void numericUpDownZoom_ValueChanged(object sender, EventArgs e)
        {
            zoomFactor = (float)numericUpDownZoom.Value; if (pictureBox.Image != null)
            {
                AdjustImageSize(pictureBox.Image);
            }
        }
    }
}
