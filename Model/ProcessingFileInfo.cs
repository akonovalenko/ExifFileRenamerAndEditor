using ExifFileRenamer.Model;
using System;
using System.IO;

namespace ExifFileRenamer
{
    /// <summary>
    /// Image file information
    /// </summary>
    /// <author>Alexey Konovalenko, aldev@ukr.net</author>
    /// <created>created 28-dec-2008</created>
    /// <version>version 1.0.0.0</version>
    internal class ProcessingFileInfo : IComparable, IProcessingFileInfo
    {

        #region Private fields

        private readonly ExifInfo _exifInfo;
        private readonly ImageInfo _imageInfo;
        private readonly FileSystemInfo _sourceFileInfo;
        private string _newName;

        #endregion

        #region Public properties

        public string ErrorText { get; set; }

        public bool IsBitmapImage { get; set; }

        public bool IsExifImage { get; set; }

        public ExifInfo Exif { get { return this._exifInfo; } }

        public ImageInfo Bitmap { get { return this._imageInfo; } }

        public DateTime? CorrectedDateTime { get; set; }

        public string Status { get; internal set; }

        public string NewFileFullName { get { return _newName; } set { _newName = value; } }

        //public bool ShouldBeSkipped { get { return this.IsBitmapImage == false && this.IsExifImage == false;  } }

        public string DirectoryName { get { return this._sourceFileInfo.DirectoryName; } }

        public string Extension { get { return this._sourceFileInfo.Extension; } }

        public string OriginalNamePart { get { return this._sourceFileInfo.OriginalNamePart; } }

        public DateTime CreatedOn { get { return this._sourceFileInfo.CreatedOn; } }

        public long SizeInBytes { get { return this._sourceFileInfo.SizeInBytes; } }

        public string FullName { get { return this._sourceFileInfo.FullName; } }

        public string ProcessingErrors { get; internal set; }

        /* Binding field */
        public string SourceFileName { get { return _sourceFileInfo.Name; } }

        /* Binding field */
        public string TargetFileName
        {
            get
            {
                if (string.IsNullOrEmpty(_newName))
                    return string.Empty;
                else
                    return _newName.Substring(_newName.LastIndexOf(Constants.BACK_SLASH_CHAR) + 1, _newName.Length - _newName.LastIndexOf(Constants.BACK_SLASH_CHAR) - 1);
            }
        }

        /* Binding field */
        public string ImageDateTime
        {
            get
            {
                return this._exifInfo == null
                    ? this._sourceFileInfo.CreatedOn.ToString()
                    : this._exifInfo.OriginalDateTime.ToString();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessingFileInfo"/> class.
        /// </summary>
        /// <param name="fiFile">The fi file.</param>
        /// <param name="exif">The file EXIF data</param>
        public ProcessingFileInfo(FileInfo fiFile, ExifInfo exif, string error) : this(fiFile)
        {
            this._exifInfo = exif;
            this.IsExifImage = true;
            this.Status = "Image with EXIF";
            this.ErrorText = error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessingFileInfo"/> class.
        /// </summary>
        /// <param name="fiFile">The fi file.</param>
        public ProcessingFileInfo(FileInfo fiFile, ImageInfo imageInfo, string error) : this(fiFile)
        {
            this._imageInfo = imageInfo;
            this.IsBitmapImage = true;
            this.Status = "Image non EXIF";
            this.ErrorText = error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessingFileInfo"/> class.
        /// </summary>
        /// <param name="fiFile">The fi file.</param>
        public ProcessingFileInfo(FileInfo fiFile)
        {
            this._sourceFileInfo = new FileSystemInfo(fiFile);
            this.Status = Constants.NOT_RECOGNIZED;
        }

        #endregion

        #region Object methods

        public void UpdateName(string name)
        {
            this._newName = this.DirectoryName + Constants.BACK_SLASH_CHAR + name + this.Extension;
        }

        public void SetDefaultStatus()
        {
            if (this.IsExifImage)
            {
                this.Status = "Image with EXIF";
            }
            else if (this.IsBitmapImage)
            {
                this.Status = "Image non EXIF";
            }
            else
            {
                this.Status = Constants.NOT_RECOGNIZED;
            }
        }

        //public int Compare(ProcessingFileInfo x, ProcessingFileInfo y)
        //{
        //    return String.Compare(x.Exif.OriginalDateTime.ToString(), y.Exif.OriginalDateTime.ToString());
        //}

        /// <summary>
        /// Entity comparer
        /// </summary>
        /// <param name="obj">The entity to compare</param>
        /// <returns>0 if enities are equals/returns>
        /// <exception cref="ArgumentException"></exception>
        public int CompareTo(object obj)
        {
            var other = obj as ProcessingFileInfo;
            if (other == null)
                throw new ArgumentException("object is not a ProcessingFileInfo");

            var thisDate = (this.IsExifImage && this._exifInfo != null && this._exifInfo.OriginalDateTime != default)
                ? this._exifInfo.OriginalDateTime
                : this.CreatedOn;
            var otherDate = (other.IsExifImage && other._exifInfo != null && other._exifInfo.OriginalDateTime != default)
                ? other._exifInfo.OriginalDateTime
                : other.CreatedOn;

            var dateCompare = thisDate.CompareTo(otherDate);
            return dateCompare != 0 ? dateCompare : this._sourceFileInfo.Name.CompareTo(other._sourceFileInfo.Name);
        }

        #endregion

    }

}
