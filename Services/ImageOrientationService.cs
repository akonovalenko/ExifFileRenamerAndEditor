using System;
using System.Drawing;

namespace ExifImageRenamer.Services
{
    /// <summary>
    /// Provides methods to apply EXIF Orientation metadata to <see cref="System.Drawing.Bitmap"/> instances.
    /// </summary>
    /// <remarks>Reads the EXIF Orientation tag (ID 274) and performs corresponding RotateFlip operations.
    /// Images without a readable Orientation tag are left unchanged. Throws ArgumentNullException if the bitmap
    /// argument is null.</remarks>
    internal static class ImageOrientationService
    {
        /// <summary>
        /// Apply the image's EXIF Orientation (tag 274) to the provided Bitmap by performing the required rotation
        /// and/or flip.
        /// </summary>
        /// <remarks>If the image has no readable Orientation tag or the tag value is absent, no
        /// transformation is performed. Images with unreadable Orientation information are ignored and left
        /// unchanged.</remarks>
        /// <param name="bitmap">Bitmap to modify; rotated and/or flipped according to its EXIF Orientation tag.</param>
        /// <exception cref="ArgumentNullException">Thrown when bitmap is null.</exception>
        public static void ApplyExifOrientation(Bitmap bitmap)
        {
            if (bitmap == null) throw new ArgumentNullException(nameof(bitmap));

            try
            {
                const int OrientationTag = 274;
                var property = bitmap.GetPropertyItem(OrientationTag);
                if (property?.Value == null || property.Value.Length < 2) return;

                switch (BitConverter.ToUInt16(property.Value, 0))
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
            catch (ArgumentException)
            {
                // Images without a readable Orientation tag need no transformation.
            }
        }
    }
}
