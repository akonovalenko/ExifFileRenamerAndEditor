using ExifFileRenamer.Model;
using System;
using System.Drawing;

namespace ExifFileRenamer
{

    /// <summary>
    /// Image file info extraction service
    /// </summary>
    /// <author>Alexey Konovalenko</created>
    /// <created>28/12/2008</created>
    /// <version>1.0.0.0</version>
    internal class ImageInfoExtractorService
    {

        /// <summary>
        /// Extracts the exif information.
        /// </summary>
        /// <param name="imageFileName">Name of the image file.</param>
        /// <returns>The instance of <see cref="EXIF"/></returns>
        public bool TryExtractImageInfo(string imageFileName, out ImageInfo result, out string errors)
        {
            result = new ImageInfo();
            bool isProcessed = false;
            errors = default;

            try
            {
                using (var bitmap = Image.FromFile(imageFileName))
                {
                    if (bitmap.PropertyItems != null)
                    {
                        result.Width = bitmap.Size.Width;
                        result.Height = bitmap.Size.Height;
                        result.HorizontalResolution = (int)bitmap.HorizontalResolution;
                        result.VerticalResolution = (int)bitmap.VerticalResolution;
                        result.PixelFormat = bitmap.PixelFormat.ToString();
                        isProcessed = result.OriginalDateTime != default;

                        isProcessed = true;
                    }//if
                }//using

            } //try
            catch (Exception ex)
            {
                isProcessed = false;
                errors = string.Format("Error source: {0}; Error message: {1};", ex.Source, ex);
            }
            return isProcessed;
        }

    }


}
