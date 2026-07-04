using System;

namespace ExifFileRenamer
{
    internal interface IProcessingFileInfo
    {
        string ErrorText { get; set; }
        string ProcessingErrors { get; }
        string SourceFileName { get; }
        string Status { get; }
        string TargetFileName { get; }
        string ImageDateTime { get; }
        string DirectoryName { get; }
        string Extension { get; }
        string OriginalNamePart { get; }
        DateTime CreatedOn { get; }
        long SizeInBytes { get; }
        string FullName { get; }
        
        bool IsExifImage { get; }
        bool IsBitmapImage { get; }
    }
}