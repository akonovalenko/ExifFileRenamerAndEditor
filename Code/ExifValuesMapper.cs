using ExifFileRenamer.Model;
using ExifImageRenamer.Model;

namespace ExifFileRenamer.Code
{
    /// <summary>
    /// Provides methods to create ExifEditValues instances from an AddExifRequest or from individual EXIF-related
    /// control values.
    /// </summary>
    /// <remarks>Centralizes mapping logic for EXIF editing and preserves null inputs; callers are responsible
    /// for input validation.</remarks>
    internal static class ExifValuesMapper
    {
        /// <summary>
        /// Creates an ExifEditValues instance populated from the provided AddExifRequest.
        /// </summary>
        /// <remarks>Maps FixedDate to OriginalDateTime and copies Description, MakeName, ModelName,
        /// Artist, Copyright, and WriteFullTemplate.</remarks>
        /// <param name="req">AddExifRequest whose form values are used to populate the new ExifEditValues.</param>
        /// <returns>An ExifEditValues populated with values from req.</returns>
        public static ExifEditValues FromAddExifFormValues(AddExifRequest req)
        {
            return new ExifEditValues
            {
                OriginalDateTime = req.FixedDate,
                Description = req.Description,
                MakeName = req.MakeName,
                ModelName = req.ModelName,
                Artist = req.Artist,
                Copyright = req.Copyright,
                WriteFullTemplate = req.WriteFullTemplate
            };
        }

        /// <summary>
        /// Creates an ExifEditValues instance populated with the provided metadata values.
        /// </summary>
        /// <remarks>Values are assigned directly to the corresponding ExifEditValues properties; no
        /// validation is performed.</remarks>
        /// <param name="originalDateTime">Original date and time of the image, or null if unspecified.</param>
        /// <param name="description">Image description.</param>
        /// <param name="make">Camera manufacturer name.</param>
        /// <param name="model">Camera model name.</param>
        /// <param name="software">Software used to process or edit the image.</param>
        /// <param name="artist">Artist or creator name.</param>
        /// <param name="copyright">Copyright notice to embed.</param>
        /// <param name="writeFullTemplate">Whether to write the full EXIF template.</param>
        /// <returns>A new ExifEditValues instance containing the provided values.</returns>
        public static ExifEditValues FromControls(System.DateTime? originalDateTime, string description, string make, string model, string software, string artist, string copyright, bool writeFullTemplate)
        {
            return new ExifEditValues
            {
                OriginalDateTime = originalDateTime,
                Description = description,
                MakeName = make,
                ModelName = model,
                Software = software,
                Artist = artist,
                Copyright = copyright,
                WriteFullTemplate = writeFullTemplate
            };
        }
    }
}
