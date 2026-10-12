using System;
using System.IO;
using ExifFileRenamer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExifRenamer.Tests
{
    [TestClass]
    public class RenameBatchValidatorTests
    {
        private string _testDir;

        [TestInitialize]
        public void Setup()
        {
            _testDir = Path.Combine(Path.GetTempPath(), "ExifRenamerRenameTests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_testDir);
        }

        [TestCleanup]
        public void Cleanup()
        {
            try { Directory.Delete(_testDir, true); } catch { }
        }

        private ProcessingFileInfo CreateFile(string name, string targetName)
        {
            var source = Path.Combine(_testDir, name);
            File.WriteAllText(source, string.Empty);
            var info = new ProcessingFileInfo(new FileInfo(source));
            info.Selected = true;
            info.NewFileFullName = Path.Combine(_testDir, targetName);
            return info;
        }

        [TestMethod]
        public void FindConflicts_ExistingTarget_IsReported()
        {
            var source = CreateFile("a.jpg", "b.jpg");
            File.WriteAllText(Path.Combine(_testDir, "b.jpg"), string.Empty);

            var conflicts = RenameBatchValidator.FindConflicts(new[] { source });

            Assert.AreEqual(1, conflicts.Count);
            StringAssert.Contains(conflicts[0], "Target file already exists");
        }

        [TestMethod]
        public void FindConflicts_DuplicateTargets_AreReported()
        {
            var first = CreateFile("a.jpg", "same.jpg");
            var second = CreateFile("b.jpg", "same.jpg");

            var conflicts = RenameBatchValidator.FindConflicts(new[] { first, second });

            Assert.AreEqual(1, conflicts.Count);
            StringAssert.Contains(conflicts[0], "Multiple files would be renamed");
        }

        [TestMethod]
        public void FindConflicts_SourceTargetSame_IsIgnored()
        {
            var source = CreateFile("a.jpg", "a.jpg");

            var conflicts = RenameBatchValidator.FindConflicts(new[] { source });

            Assert.AreEqual(0, conflicts.Count);
        }
        [TestMethod]
        public void FindConflicts_TargetDirectory_IsReported()
        {
            var source = CreateFile("source.jpg", "target.jpg");
            Directory.CreateDirectory(Path.Combine(_testDir, "target.jpg"));

            var conflicts = RenameBatchValidator.FindConflicts(new[] { source });

            Assert.AreEqual(1, conflicts.Count);
            StringAssert.Contains(conflicts[0], "Target path is a directory");
        }

        [TestMethod]
        public void FindConflicts_DuplicateTargets_AreComparedCaseInsensitively()
        {
            var first = CreateFile("first.jpg", "Target.jpg");
            var second = CreateFile("second.jpg", "target.JPG");

            var conflicts = RenameBatchValidator.FindConflicts(new[] { first, second });

            Assert.AreEqual(1, conflicts.Count);
            StringAssert.Contains(conflicts[0], "Multiple files would be renamed");
        }

        [TestMethod]
        public void FindConflicts_NullAndBlankTargets_AreIgnored()
        {
            var source = CreateFile("source.jpg", "unused.jpg");
            source.NewFileFullName = null;
            var another = CreateFile("another.jpg", "ignored.jpg");
            another.NewFileFullName = " ";

            var conflicts = RenameBatchValidator.FindConflicts(new[] { null, source, another });

            Assert.AreEqual(0, conflicts.Count);
        }

    }
}
