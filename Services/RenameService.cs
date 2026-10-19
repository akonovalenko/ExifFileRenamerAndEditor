using ExifFileRenamer;
using ExifImageRenamer.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExifImageRenamer.Services
{
    /// <summary>
    /// Performs batch file rename operations and records a journal of moved files to support undo.
    /// </summary>
    /// <remarks>Processes each provided file entry, attempting to move the file to its new path, updating
    /// per-file status, and recording successful moves in a journal. Accepts a cancellation predicate that aborts the
    /// remaining work and sets the batch result as canceled, and an optional progress callback invoked after each file
    /// with (completed, total). The Undo operation accepts a journal of (originalPath, newPath) pairs, restores files
    /// by moving them back when the new path exists and the original path does not, returns the count of restored
    /// files, and continues on errors to restore the remainder of the batch.</remarks>
    internal sealed class RenameService
    {
        public RenameBatchResult Rename(
            IEnumerable<ProcessingFileInfo> files,
            Func<bool> cancellationRequested,
            Action<int, int> progressChanged = null)
        {
            if (files == null) throw new ArgumentNullException(nameof(files));
            var batch = new List<ProcessingFileInfo>(files);
            var result = new RenameBatchResult();
            int total = batch.Count;

            for (int i = 0; i < total; i++)
            {
                if (cancellationRequested?.Invoke() == true)
                {
                    result.Canceled = true;
                    break;
                }

                var file = batch[i];
                try
                {
                    File.Move(file.FullName, file.NewFileFullName);
                    result.Journal.Add(new KeyValuePair<string, string>(file.FullName, file.NewFileFullName));
                    file.CommitRename();
                    file.Status = "renamed";
                    result.RenamedCount++;
                }
                catch (Exception ex)
                {
                    file.Status = ex.Message;
                    result.FailedCount++;
                }

                progressChanged?.Invoke(i + 1, total);
            }

            return result;
        }

        
        /// <summary>
        /// Restores files recorded in the journal by moving each backup file (value) to its original path (key).
        /// </summary>
        /// <remarks>Returns 0 if journal is null. Entries are skipped if the backup file does not exist
        /// or the target path already exists. Exceptions thrown while moving individual files are caught and processing
        /// continues.</remarks>
        /// <param name="journal">Sequence of key/value pairs mapping original file path (key) to backup file path (value) to restore.</param>
        /// <returns>The number of files successfully restored.</returns>
        public int Undo(IEnumerable<KeyValuePair<string, string>> journal)
        {
            if (journal == null) return 0;
            int restored = 0;
            foreach (var item in journal)
            {
                try
                {
                    if (!File.Exists(item.Value) || File.Exists(item.Key)) continue;
                    File.Move(item.Value, item.Key);
                    restored++;
                }
                catch
                {
                    // Continue restoring the rest of the batch.
                }
            }
            return restored;
        }
    }
}
