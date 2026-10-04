using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ExifFileRenamer
{
    /// <summary>
    /// File name template generator
    /// </summary>
    /// <author>Alexey Konovalenko</author>
    /// <created>28/12/2008</created>
    /// <version>1.1.1.0</version>
    public class FileNameTemplateFactory : INotifyPropertyChanged
    {
        #region Public events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private fields

        private bool _useImageFilePrefixPart;
        private bool _useDateTimePart;
        private bool _useSuffixDelimiter;
        private bool _useCounterSuffix;
        private bool _useCameraName;
        private bool _useImageOrientation;
        private bool _useImageSize;
        private bool _useSoftWareName;

        private string _imageFileNamePrefix;
        private string _dateTimePart;
        private string _suffixDelimiter;
        private string _counterSuffix;
        private string _cameraName;
        private string _softWareName;
        private string _imageOrientation;
        private string _imageSize;

        #endregion

        #region Public properties

        public bool UseImageFilePrefixPart
        {
            get => this._useImageFilePrefixPart;
            set => SetProperty(ref this._useImageFilePrefixPart, value);
        }

        public bool UseDateTimePart
        {
            get => this._useDateTimePart;
            set => SetProperty(ref this._useDateTimePart, value);
        }

        public bool UseSuffixDelimiter
        {
            get => this._useSuffixDelimiter;
            set => SetProperty(ref this._useSuffixDelimiter, value);
        }

        public bool UseCounterSuffix
        {
            get => this._useCounterSuffix;
            set => SetProperty(ref this._useCounterSuffix, value);
        }

        public string ImageFileNamePrefix
        {
            get => this._imageFileNamePrefix;
            set => SetProperty(ref this._imageFileNamePrefix, value);
        }

        public bool UseCameraName
        {
            get => this._useCameraName;
            set => SetProperty(ref this._useCameraName, value);
        }

        public bool UseSoftwareName
        {
            get => this._useSoftWareName;
            set => SetProperty(ref this._useSoftWareName, value);
        }

        public bool UseImageOrientation
        {
            get => this._useImageOrientation;
            set => SetProperty(ref this._useImageOrientation, value);
        }

        public bool UseImageSize
        {
            get => this._useImageSize;
            set => SetProperty(ref this._useImageSize, value);
        }

        public string DateTimePart
        {
            get => this._dateTimePart;
            set => SetProperty(ref this._dateTimePart, value);
        }

        public string SuffixDelimiter
        {
            get => this._suffixDelimiter;
            set => SetProperty(ref this._suffixDelimiter, value);
        }

        public string CounterSuffix
        {
            get => this._counterSuffix;
            set => SetProperty(ref this._counterSuffix, value);
        }

        public string CameraName
        {
            get => this._cameraName;
            set => SetProperty(ref this._cameraName, value);
        }

        public string SoftWareName
        {
            get => this._softWareName;
            set => SetProperty(ref this._softWareName, value);
        }

        public string ImageOrientation
        {
            get => this._imageOrientation;
            set => SetProperty(ref this._imageOrientation, value);
        }

        public string ImageSize
        {
            get => this._imageSize;
            set => SetProperty(ref this._imageSize, value);
        }

        #endregion

        #region Constructors

        public FileNameTemplateFactory(
            bool useDateTimePart,
            bool useSuffixDelimiter,
            bool useCounterSuffix,
            bool useImageFilePrefixPart,
            bool useCameraName,
            bool useSoftWareName,
            bool useImageOrientation,
            bool useImageSize,
            string imageFileNamePrefix,
            string dateTimePart,
            string suffixDelimiter,
            string counterSuffix,
            string makeName,
            string modelName,
            string softWareName,
            string imageOrientation,
            string imageSize)
        {
            this._useImageFilePrefixPart = useImageFilePrefixPart;
            this._useDateTimePart = useDateTimePart;
            this._useSuffixDelimiter = useSuffixDelimiter;
            this._useCounterSuffix = useCounterSuffix;
            this._useCameraName = useCameraName;
            this._useSoftWareName = useSoftWareName;
            this._useImageOrientation = useImageOrientation;
            this._useImageSize = useImageSize;

            this._imageFileNamePrefix = imageFileNamePrefix;
            this._dateTimePart = dateTimePart;
            this._suffixDelimiter = suffixDelimiter;
            this._counterSuffix = counterSuffix;
            this._cameraName = $"{makeName}-{modelName}";
            this._softWareName = softWareName;
            this._imageOrientation = imageOrientation;
            this._imageSize = imageSize;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Builds the file name template based on the selected parts and their values.
        /// </summary>
        /// <returns>The built file name template.</returns>
        public string Build()
        {
            var parts = new List<string>();

            AddPart(parts, this._useImageFilePrefixPart, this._imageFileNamePrefix);
            AddPart(parts, this._useDateTimePart, this._dateTimePart);
            AddPart(parts, this._useCounterSuffix, this._counterSuffix);
            AddPart(parts, this._useCameraName, this._cameraName);
            AddPart(parts, this._useSoftWareName, this._softWareName);
            AddPart(parts, this._useImageOrientation, this._imageOrientation);
            AddPart(parts, this._useImageSize, this._imageSize);

            return string.Join(this._useSuffixDelimiter ? this._suffixDelimiter ?? string.Empty : string.Empty, parts);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Adds a part to the collection if it is enabled and has a non-empty value.
        /// </summary>
        /// <param name="parts">The collection to which the part will be added.</param>
        /// <param name="enabled">Indicates whether the part is enabled.</param>
        /// <param name="value">The value of the part.</param>
        private static void AddPart(ICollection<string> parts, bool enabled, string value)
        {
            if (enabled && !string.IsNullOrWhiteSpace(value))
                parts.Add(value);
        }

        /// <summary>
        /// Raises the PropertyChanged event for the specified property name.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the value of a property and raises the PropertyChanged event if the value has changed.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="field">The field backing the property.</param>
        /// <param name="value">The new value for the property.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        private void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }

        #endregion
    }
}
