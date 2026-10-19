using ExifFileRenamer;
using ExifFileRenamer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExifImageRenamer.Services
{
    /// <summary>
    /// Scans directories for image files, extracts EXIF and image metadata, and returns ProcessingFileInfo instances
    /// describing each processed file.
    /// </summary>
    /// <remarks>Constructed with optional ExifInfoExtractorService and ImageInfoExtractorService
    /// dependencies. CreateFileInfo attempts EXIF extraction first and falls back to image-level extraction, recording
    /// any extraction errors in ProcessingFileInfo.ErrorText; CreateFileInfo throws ArgumentNullException if fileInfo
    /// is null. Scan enumerates files in the specified directory (optionally including subdirectories), filters by
    /// extensions (null, empty, or "*.*" match all), supports cooperative cancellation via a Func<bool>, reports
    /// discovered file count and progress via callbacks, processes files in parallel using Environment.ProcessorCount
    /// threads, and returns only the ProcessingFileInfo entries for matching files. If directoryPath is null, empty, or
    /// does not exist, Scan returns an empty array.</remarks>
    internal sealed class FileScanService
    {
        private readonly ExifInfoExtractorService _exifExtractor;
        private readonly ImageInfoExtractorService _imageExtractor;

        /// <summary>
        /// Initializes a new instance of the FileScanService class using the provided extractors or default instances
        /// when null.
        /// </summary>
        /// <param name="exifExtractor">Optional ExifInfoExtractorService to use; if null, a new ExifInfoExtractorService is created.</param>
        /// <param name="imageExtractor">Optional ImageInfoExtractorService to use; if null, a new ImageInfoExtractorService is created.</param>
        public FileScanService(ExifInfoExtractorService exifExtractor = null, 
            ImageInfoExtractorService imageExtractor = null)
        {
            this._exifExtractor = exifExtractor ?? new ExifInfoExtractorService();
            this._imageExtractor = imageExtractor ?? new ImageInfoExtractorService();
        }

        /// <summary>
        /// Creates a ProcessingFileInfo for the specified file by attempting EXIF extraction first, then falling back
        /// to image extraction; sets ErrorText when metadata cannot be read.
        /// </summary>
        /// <remarks>EXIF extraction is attempted before image extraction. If EXIF extraction reports a
        /// metadata read failure, ErrorText is set to "Unable to read image metadata: <error>". If EXIF is absent and
        /// image extraction succeeds, the result contains ImageInfo; otherwise ErrorText contains any image extraction
        /// error.</remarks>
        /// <param name="fileInfo">The file to process.</param>
        /// <returns>A ProcessingFileInfo containing extracted ExifInfo or ImageInfo when available; ErrorText contains any
        /// metadata extraction error.</returns>
        /// <exception cref="ArgumentNullException">Thrown when fileInfo is null.</exception>
        public ProcessingFileInfo CreateFileInfo(FileInfo fileInfo)
        {
            if (fileInfo == null) throw new ArgumentNullException(nameof(fileInfo));

            if (this._exifExtractor.TryExtractExifInfo(fileInfo.FullName, out ExifInfo exif, out string exifError))
                return new ProcessingFileInfo(fileInfo, exif, exifError);

            // A metadata read failure is not evidence that the image has no EXIF.
            if (!string.IsNullOrWhiteSpace(exifError))
                return new ProcessingFileInfo(fileInfo) { ErrorText = $"Unable to read image metadata: {exifError}" };

            if (this._imageExtractor.TryExtractImageInfo(fileInfo.FullName, out ImageInfo image, out string imageError))
                return new ProcessingFileInfo(fileInfo, image, imageError);

            var result = new ProcessingFileInfo(fileInfo);
            if (!string.IsNullOrWhiteSpace(imageError)) result.ErrorText = imageError;
            return result;
        }

        /// <summary>
        /// Scans the specified directory for files that match the provided extensions.
        /// </summary>
        /// <remarks>Files are processed in parallel (MaxDegreeOfParallelism =
        /// Environment.ProcessorCount). Callbacks such as progressChanged may be invoked from worker threads and must
        /// be thread-safe. Cancellation is cooperative and may stop processing early.</remarks>
        /// <param name="directoryPath">Directory path to scan; if null, whitespace, or does not exist, an empty list is returned.</param>
        /// <param name="extensions">Array of file extensions to include (for example, ".txt"); comparisons use the file's Extension property.</param>
        /// <param name="includeSubdirectories">true to include subdirectories; false to search only the top-level directory.</param>
        /// <param name="cancellationRequested">Callback invoked to indicate whether scanning should stop; returning true requests cooperative cancellation.</param>
        /// <param name="filesDiscovered">Optional callback invoked once with the total number of files discovered before extension filtering.</param>
        /// <param name="progressChanged">Optional callback invoked when progress changes; parameters are the number of files processed and the total
        /// file count.</param>
        /// <returns>A read-only list of ProcessingFileInfo for files that matched the extensions. Each item may contain
        /// ErrorText for individual processing errors; an empty list is returned when the directory is invalid or
        /// contains no matching files.</returns>
        public IReadOnlyList<ProcessingFileInfo> Scan(
            string directoryPath,
            string[] extensions,
            bool includeSubdirectories,
            Func<bool> cancellationRequested,
            Action<int> filesDiscovered = null,
            Action<int, int> progressChanged = null)
        {
            if (string.IsNullOrWhiteSpace(directoryPath) || !Directory.Exists(directoryPath))
                return Array.Empty<ProcessingFileInfo>();

            var files = new DirectoryInfo(directoryPath).GetFiles("*", includeSubdirectories
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly);
            filesDiscovered?.Invoke(files.Length);

            var results = new ProcessingFileInfo[files.Length];
            int processed = 0;
            int lastPercent = -1;
            var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

            Parallel.For(0, files.Length, options, (index, loopState) =>
            {
                if (cancellationRequested?.Invoke() == true)
                {
                    loopState.Stop();
                    return;
                }

                var file = files[index];
                try
                {
                    if (MatchesExtension(file.Extension, extensions))
                    {
                        results[index] = CreateFileInfo(file);
                    }
                }
                catch (Exception ex)
                {
                    results[index] = new ProcessingFileInfo(file) { ErrorText = ex.ToString() };
                }

                int count = Interlocked.Increment(ref processed);
                if (files.Length > 0)
                {
                    int percent = count * 100 / files.Length;
                    if (Interlocked.Exchange(ref lastPercent, percent) != percent || count == files.Length)
                    {
                        progressChanged?.Invoke(count, files.Length);
                    }
                }
            });

            return results.Where(item => item != null).ToArray();
        }

        /// <summary>
        /// Determines whether a file extension matches one of the specified extensions.
        /// </summary>
        /// <remarks>The fileExtension value is trimmed of a leading '.' and converted with
        /// ToUpperInvariant before comparison. The extensions array is used as provided, so its entries should be
        /// normalized (no leading '.' and uppercased) for matching. Use "*.*" as the first element to accept any
        /// extension.</remarks>
        /// <param name="fileExtension">File extension to test; may include a leading '.' and may be null.</param>
        /// <param name="extensions">Allowed extensions to match against; if null, empty, or its first element is "*.*" every extension is
        /// accepted. Entries are compared against the normalized file extension (no leading '.' and uppercased via
        /// ToUpperInvariant).</param>
        /// <returns>True if extensions is null, empty, or its first element equals "*.*", or if the normalized file extension
        /// (no leading '.' and uppercased via ToUpperInvariant) is found in extensions; otherwise false.</returns>
        private static bool MatchesExtension(string fileExtension, string[] extensions)
        {
            if (extensions == null || extensions.Length == 0 || extensions[0] == "*.*")
            {
                return true;
            }
            string extension = (fileExtension ?? string.Empty).TrimStart('.').ToUpperInvariant();
            return Array.IndexOf(extensions, extension) >= 0;
        }
    }
}
