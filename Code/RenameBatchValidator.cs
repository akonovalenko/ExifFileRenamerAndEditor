using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExifFileRenamer
{
    /// <summary>
    /// Validates a batch of files to be renamed, checking for conflicts such as duplicate target names or existing files/directories.
    /// </summary>
    internal static class RenameBatchValidator
    {
        /// <summary>
        /// Finds conflicts in a batch of files to be renamed. A conflict occurs if multiple files would be renamed to the same target name, or if the target name already exists as a file or directory.
        /// </summary>
        /// <param name="files">The list of files to validate.</param>
        /// <returns>A list of conflict messages.</returns>
        public static IReadOnlyList<string> FindConflicts(IEnumerable<ProcessingFileInfo> files)
        {
            var candidates = files
                .Where(f => f != null && !string.IsNullOrWhiteSpace(f.NewFileFullName)
                    && !string.Equals(f.FullName, f.NewFileFullName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var conflicts = new List<string>();
            var targets = new Dictionary<string, ProcessingFileInfo>(StringComparer.OrdinalIgnoreCase);

            foreach (var file in candidates)
            {
                if (targets.TryGetValue(file.NewFileFullName, out var previous))
                {
                    conflicts.Add(string.Format("Multiple files would be renamed to '{0}': '{1}' and '{2}'.",
                        file.NewFileFullName, previous.FullName, file.FullName));
                }
                else
                {
                    targets[file.NewFileFullName] = file;
                }

                if (File.Exists(file.NewFileFullName))
                {
                    conflicts.Add(string.Format("Target file already exists: '{0}'.", file.NewFileFullName));
                }
                else if (Directory.Exists(file.NewFileFullName))
                {
                    conflicts.Add(string.Format("Target path is a directory: '{0}'.", file.NewFileFullName));
                }
            }

            return conflicts.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        }
    }
}
