using System;

namespace ExifFileRenamer.Model
{
    /// <summary>
    /// Editable EXIF fields collected from the edit dialog.
    /// String semantics: null — do not touch the tag, empty — remove the tag, otherwise — write the value.
    /// </summary>
    internal class ExifEditValues
    {
        /// <summary>Null — do not modify date tags.</summary>
        public DateTime? OriginalDateTime { get; set; }

        public string Description { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string Software { get; set; }
        public string Artist { get; set; }
        public string Copyright { get; set; }

        /// <summary>
        /// When true, also write a full set of standard EXIF tags (with default values)
        /// in addition to the fields above — so the file gets a complete EXIF block.
        /// Intended for adding EXIF to files that have none.
        /// </summary>
        public bool WriteFullTemplate { get; set; }
    }
}
