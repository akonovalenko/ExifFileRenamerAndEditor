using System;

namespace ExifFileRenamer
{
    /// <summary>
    /// Image file name factory
    /// </summary>
    /// <author>Alexey Konovalenko</created>
    /// <created>28/12/2008</created>
    internal class FileNameFactory
    {

        #region Public methods

        // Creates a new file name for image nased on EXIF information

        public void Create(FileNameTemplateFactory appFileNameTemplate, IProcessingFileInfo imageFile, int fileNumber)
        {

            var fileItem = imageFile as ProcessingFileInfo;
            if (fileItem == null)
                return;

            var dateTime = this.CreateExifDateTime(appFileNameTemplate, fileItem);

            var counterLength = appFileNameTemplate.CounterSuffix?.Length ?? 4;
            var fNumber = fileNumber.ToString().PadLeft(counterLength, '0');

            var imageSize = string.Empty;
            if (fileItem.IsExifImage
                && !string.IsNullOrEmpty(fileItem.Exif?.Width)
                && !string.IsNullOrEmpty(fileItem.Exif?.Height))
            {
                imageSize = string.Format("{0}x{1}", fileItem.Exif.Width, fileItem.Exif.Height);
            }
            else if (fileItem.Bitmap != null)
            {
                imageSize = string.Format("{0}x{1}", fileItem.Bitmap.Width, fileItem.Bitmap.Height);
            }

            var fileNameTemplate = new FileNameTemplateFactory(
                appFileNameTemplate.UseDateTimePart,
                appFileNameTemplate.UseSuffixDelimiter,
                appFileNameTemplate.UseCounterSuffix,
                appFileNameTemplate.UseImageFilePrefixPart,
                appFileNameTemplate.UseCameraName,
                appFileNameTemplate.UseSoftwareName,
                appFileNameTemplate.UseImageOrientation,
                appFileNameTemplate.UseImageSize,

                appFileNameTemplate.ImageFileNamePrefix,
                dateTime,
                appFileNameTemplate.SuffixDelimiter,
                fNumber,
                fileItem.Exif?.MakeName,
                fileItem.Exif?.ModelName,
                fileItem.Exif?.Software,
                fileItem.Exif?.Orientation,
                imageSize
            );

            var result = fileNameTemplate.Build();

            if (string.IsNullOrEmpty(result))
            {
                result = fileItem.OriginalNamePart;
            }

            fileItem.UpdateName(result);

            if (fileItem.TargetFileName == fileItem.SourceFileName)
            {
                fileItem.Status = Constants.NO_CHANGES;
            }
            else
            {
                fileItem.SetDefaultStatus();
            }
        }

        #endregion

        #region Private methods

        private string GetFilledByZeroNumber(int i, int totalWidth)
        {
            return this.GetFilledNumber(i, totalWidth, '0');
        }

        private string GetFilledNumber(int i, int totalWidth, char character)
        {
            return i.ToString().PadLeft(totalWidth, character);
        }

        private string CreateExifDateTime(FileNameTemplateFactory fnTemplate, IProcessingFileInfo imageFile)
        {
            string dateTime = string.Empty;
            var fileItem = imageFile as ProcessingFileInfo;
            if (fileItem == null)
                return dateTime;

            if (fnTemplate.UseDateTimePart)
            {
                int year, month, day, hour, minute, second;

                if (!fileItem.IsExifImage)
                {
                    year = fileItem.CreatedOn.Year;
                    month = fileItem.CreatedOn.Month;
                    day = fileItem.CreatedOn.Day;
                    hour = fileItem.CreatedOn.Hour;
                    minute = fileItem.CreatedOn.Minute;
                    second = fileItem.CreatedOn.Second;
                }
                else
                {
                    DateTime photoCreatedDateTime;
                    //if (this._fileNameCorrector != null)
                    //{
                    //   photoCreatedDateTime= this._fileNameCorrector.GetCorectedTime(this._imageFileInfo.Exif);
                    //}
                    //else 
                    photoCreatedDateTime = fileItem.Exif.OriginalDateTime;

                    year = photoCreatedDateTime.Year;
                    month = photoCreatedDateTime.Month;
                    day = photoCreatedDateTime.Day;
                    hour = photoCreatedDateTime.Hour;
                    minute = photoCreatedDateTime.Minute;
                    second = photoCreatedDateTime.Second;
                }
                switch (fnTemplate.DateTimePart)
                {
                    case "yyyymmdd":
                        dateTime = (year.ToString() + GetFilledByZeroNumber(month, 2) + GetFilledByZeroNumber(day, 2));
                        break;
                    case "yymmdd":
                        dateTime = (year.ToString().Substring(2, 2) + GetFilledByZeroNumber(month, 2) + GetFilledByZeroNumber(day, 2));
                        break;
                    case "yyyymm":
                        dateTime = (year.ToString() + GetFilledByZeroNumber(month, 2));
                        break;
                    case "yymm":
                        dateTime = (year.ToString().Substring(2, 2) + GetFilledByZeroNumber(month, 2));
                        break;
                    case "yyyymmdd hhmmss":
                        dateTime = (year.ToString() + GetFilledByZeroNumber(month, 2) + GetFilledByZeroNumber(day, 2) + " " + GetFilledByZeroNumber(hour, 2) + GetFilledByZeroNumber(minute, 2) + GetFilledByZeroNumber(second, 2));
                        break;
                    case "yyyymmdd hhmm":
                        dateTime = (year.ToString() + GetFilledByZeroNumber(month, 2) + GetFilledByZeroNumber(day, 2) + " " + GetFilledByZeroNumber(hour, 2) + GetFilledByZeroNumber(minute, 2));
                        break;
                    case "yymmdd hhmmss":
                        dateTime = (year.ToString().Substring(2, 2) + GetFilledByZeroNumber(month, 2) + GetFilledByZeroNumber(day, 2) + " " + GetFilledByZeroNumber(hour, 2) + GetFilledByZeroNumber(minute, 2) + GetFilledByZeroNumber(second, 2));
                        break;
                    case "yymmdd hhmm":
                        dateTime = (year.ToString().Substring(2, 2) + GetFilledByZeroNumber(month, 2) + GetFilledByZeroNumber(day, 2) + " " + GetFilledByZeroNumber(hour, 2) + GetFilledByZeroNumber(minute, 2));
                        break;
                    default:
                        dateTime = (year.ToString() + GetFilledByZeroNumber(month, 2) + GetFilledByZeroNumber(day, 2));
                        break;
                }
            }
            return dateTime;
        }

        #endregion

    }

}
