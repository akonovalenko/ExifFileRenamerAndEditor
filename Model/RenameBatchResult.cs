using System.Collections.Generic;

namespace ExifImageRenamer.Model
{
    /// <summary>
    /// Represents the outcome of executing a batch of rename operations, including a chronological journal of attempted
    /// renames and summary counts.
    /// </summary>
    /// <remarks>Journal contains ordered pairs of source and target names or error details for each attempted
    /// rename. RenamedCount and FailedCount report the number of successful and failed operations respectively.
    /// Canceled is true when the batch was aborted before completing all entries.</remarks>
    internal sealed class RenameBatchResult
    {
        public List<KeyValuePair<string, string>> Journal { get; } = new List<KeyValuePair<string, string>>();
        public int RenamedCount { get; set; }
        public int FailedCount { get; set; }
        public bool Canceled { get; set; }
    }
}
