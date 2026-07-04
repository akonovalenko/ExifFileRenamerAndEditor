using System;
using System.IO;

namespace ExifFileRenamer
{
    /// <summary>
    /// A file system information
    /// </summary>
    class FileSystemInfo
    {
        #region Private fields

        private string _name;
        private string _fullFileName;
        private string _extension;
        private string _directoryName;
        private DateTime _creationDateTime;
        private long _sizeInBytes;

        #endregion

        #region Public propeties

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Extension
        {
            get { return _extension; }
            set { _extension = value; }
        }

        public string FullName
        {
            get { return _fullFileName; }
            set { _fullFileName = value; }
        }

        public string DirectoryName
        {
            get { return _directoryName; }
            set { _directoryName = value; }
        }

        public long SizeInBytes
        {
            get { return _sizeInBytes; }
            set { _sizeInBytes = value; }
        }

        public DateTime CreatedOn
        {
            get { return _creationDateTime; }
            set { _creationDateTime = value; }
        }

        public string OriginalNamePart
        {
            get
            {
                if (string.IsNullOrEmpty(_fullFileName))
                {
                    return string.Empty;
                }
                else
                {
                    if (_fullFileName.EndsWith(this._extension))
                    {
                        return _fullFileName.Substring(0, _fullFileName.LastIndexOf(this._extension));
                    }
                    else
                        return this._name;
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemInfo"/> class.
        /// </summary>
        /// <param name="fi">The file info data <see cref="FileInfo"/>.</param>
        public FileSystemInfo(FileInfo fi)
        {
            this._name = fi.Name;
            this._fullFileName = fi.FullName;
            this._extension = fi.Extension;
            this._directoryName = fi.DirectoryName;
            this._creationDateTime = fi.CreationTime;
            this._sizeInBytes = fi.Length;
        }

        #endregion

    }
}
