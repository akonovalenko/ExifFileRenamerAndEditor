using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ExifImageRenamer.Forms
{
    /// <summary>
    /// Provides a resizable Windows Form that loads an image from a file into a PictureBox, applies EXIF orientation,
    /// and supports closing with the Escape key.
    /// </summary>
    /// <remarks>Sets the form title to the file name, centers the window relative to its parent, enforces a
    /// minimum size, uses a dark background, reads file bytes into a MemoryStream to preserve image data, and disposes
    /// the displayed image when the form is disposed.</remarks>
    internal sealed class ImagePreviewForm : Form
    {
        private readonly PictureBox _pictureBox;

        /// <summary>
        /// Initializes a new ImagePreviewForm that loads the image at the specified path and displays it in a zoomed
        /// PictureBox.
        /// </summary>
        /// <remarks>Configures form size, appearance, and Escape key handling; reads the file into
        /// memory, creates a Bitmap, applies EXIF orientation, and assigns it to the PictureBox.</remarks>
        /// <param name="fileName">Path of the image file to load and display.</param>
        public ImagePreviewForm(string fileName)
        {
            Text = Path.GetFileName(fileName);
            StartPosition = FormStartPosition.CenterParent;
            Width = 1000;
            Height = 750;
            MinimumSize = new Size(400, 300);
            KeyPreview = true;
            BackColor = Color.FromArgb(32, 32, 32);

            this._pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = BackColor,
                TabStop = false
            };
            Controls.Add(this._pictureBox);
            KeyDown += (sender, e) => {
                if (e.KeyCode == Keys.Escape)
                {
                    Close();
                }
            };

            var bytes = File.ReadAllBytes(fileName);
            using (var stream = new MemoryStream(bytes))
            using (var image = Image.FromStream(stream))
            {
                var bitmap = new Bitmap(image);
                ApplyExifOrientation(bitmap);
                this._pictureBox.Image = bitmap;
            }
        }

        /// <summary>
        /// Releases unmanaged resources and, when disposing is true, disposes the PictureBox image and clears the
        /// PictureBox.Image reference.
        /// </summary>
        /// <remarks>Overrides Dispose to dispose the cached Image and clear the PictureBox.Image before
        /// invoking base.Dispose(disposing).</remarks>
        /// <param name="disposing">true to dispose managed resources (including the PictureBox image); false to release only unmanaged
        /// resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var image = this._pictureBox?.Image;
                if (this._pictureBox != null)
                {
                    this._pictureBox.Image = null;
                }
                image?.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Apply the EXIF Orientation (tag 0x0112 / 274) to a Bitmap by performing the corresponding RotateFlip
        /// operations.
        /// </summary>
        /// <remarks>If the orientation property is missing or invalid, no changes are made.
        /// ArgumentException thrown while retrieving EXIF data is ignored. Handles EXIF orientation values 2–8; value 1
        /// is treated as no rotation.</remarks>
        /// <param name="bitmap">Bitmap to adjust in place according to its EXIF Orientation tag.</param>
        private static void ApplyExifOrientation(Bitmap bitmap)
        {
            const int OrientationTag = 274;
            try
            {
                var property = bitmap.GetPropertyItem(OrientationTag);
                if (property?.Value == null || property.Value.Length < 2) return;
                switch (BitConverter.ToInt16(property.Value, 0))
                {
                    case 2: bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX); break;
                    case 3: bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone); break;
                    case 4: bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY); break;
                    case 5: bitmap.RotateFlip(RotateFlipType.Rotate270FlipX); break;
                    case 6: bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone); break;
                    case 7: bitmap.RotateFlip(RotateFlipType.Rotate90FlipX); break;
                    case 8: bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone); break;
                }
            }
            catch (ArgumentException) { }
        }
    }
}
