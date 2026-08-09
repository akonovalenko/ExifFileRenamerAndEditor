using System;

namespace ExifFileRenamer
{
    /// <summary>
    /// Represents information about a file being processed, 
    /// including its source and target names, status, and any errors encountered during processing.
    /// </summary>
    internal interface IProcessingFileInfo
    {
        string ErrorText { get; set; }
        bool Selected { get; set; }
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