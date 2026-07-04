using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ExifFileRenamer
{
    /// <summary>
    /// File name template generator
    /// </summary>
    /// <author>Alexey Konovalenko</author>
    /// <created>28/12/2008</created>
    /// <version>1.0.0.0</version>
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
            get => _useImageFilePrefixPart;
            set => SetProperty(ref _useImageFilePrefixPart, value);
        }

        public bool UseDateTimePart
        {
            get => _useDateTimePart;
            set => SetProperty(ref _useDateTimePart, value);
        }

        public bool UseSuffixDelimiter
        {
            get => _useSuffixDelimiter;
            set => SetProperty(ref _useSuffixDelimiter, value);
        }

        public bool UseCounterSuffix
        {
            get => _useCounterSuffix;
            set => SetProperty(ref _useCounterSuffix, value);
        }

        public string ImageFileNamePrefix
        {
            get => _imageFileNamePrefix;
            set => SetProperty(ref _imageFileNamePrefix, value);
        }

        public bool UseCameraName
        {
            get => _useCameraName;
            set => SetProperty(ref _useCameraName, value);
        }

        public bool UseSoftwareName
        {
            get => _useSoftWareName;
            set => SetProperty(ref _useSoftWareName, value);
        }

        public bool UseImageOrientation
        {
            get => _useImageOrientation;
            set => SetProperty(ref _useImageOrientation, value);
        }

        public bool UseImageSize
        {
            get => _useImageSize;
            set => SetProperty(ref _useImageSize, value);
        }

        public string DateTimePart
        {
            get => _dateTimePart;
            set => SetProperty(ref _dateTimePart, value);
        }

        public string SuffixDelimiter
        {
            get => _suffixDelimiter;
            set => SetProperty(ref _suffixDelimiter, value);
        }

        public string CounterSuffix
        {
            get => _counterSuffix;
            set => SetProperty(ref _counterSuffix, value);
        }

        public string CameraName
        {
            get => _cameraName;
            set => SetProperty(ref _cameraName, value);
        }

        public string SoftWareName
        {
            get => _softWareName;
            set => SetProperty(ref _softWareName, value);
        }

        public string ImageOrientation
        {
            get => _imageOrientation;
            set => SetProperty(ref _imageOrientation, value);
        }

        public string ImageSize
        {
            get => _imageSize;
            set => SetProperty(ref _imageSize, value);
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
            _useImageFilePrefixPart = useImageFilePrefixPart;
            _useDateTimePart = useDateTimePart;
            _useSuffixDelimiter = useSuffixDelimiter;
            _useCounterSuffix = useCounterSuffix;
            _useCameraName = useCameraName;
            _useSoftWareName = useSoftWareName;
            _useImageOrientation = useImageOrientation;
            _useImageSize = useImageSize;

            _imageFileNamePrefix = imageFileNamePrefix;
            _dateTimePart = dateTimePart;
            _suffixDelimiter = suffixDelimiter;
            _counterSuffix = counterSuffix;
            _cameraName = $"{makeName}-{modelName}";
            _softWareName = softWareName;
            _imageOrientation = imageOrientation;
            _imageSize = imageSize;
        }

        #endregion

        #region Public methods

        public string Build()
        {
            var sbNewName = new StringBuilder();

            if (_useImageFilePrefixPart)
            {
                sbNewName.Append(_imageFileNamePrefix);
            }

            if (_useDateTimePart)
            {
                if (_useSuffixDelimiter)
                    sbNewName.Append(_suffixDelimiter);
                sbNewName.Append(_dateTimePart);
            }

            if (_useCounterSuffix)
            {
                if (_useSuffixDelimiter)
                    sbNewName.Append(_suffixDelimiter);
                sbNewName.Append(_counterSuffix);
            }

            if (_useCameraName)
            {
                if (_useSuffixDelimiter)
                    sbNewName.Append(_suffixDelimiter);
                sbNewName.Append(_cameraName);
            }

            if (_useSoftWareName)
            {
                if (_useSuffixDelimiter)
                    sbNewName.Append(_suffixDelimiter);
                sbNewName.Append(_softWareName);
            }

            if (_useImageOrientation)
            {
                if (_useSuffixDelimiter)
                    sbNewName.Append(_suffixDelimiter);
                sbNewName.Append(_imageOrientation);
            }

            if (_useImageSize)
            {
                if (_useSuffixDelimiter)
                    sbNewName.Append(_suffixDelimiter);
                sbNewName.Append(_imageSize);
            }

            return sbNewName.ToString();
        }

        #endregion

        #region Private methods

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
