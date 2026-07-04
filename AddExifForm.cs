using ExifFileRenamer.Model;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ExifFileRenamer
{
    internal enum AddExifDateSource
    {
        None,
        CreationTime,
        LastWriteTime,
        Fixed
    }

    /// <summary>
    /// Snapshot of the dialog values: survives the dialog disposal, so the
    /// background batch worker can safely build per-file EXIF values from it.
    /// </summary>
    internal class AddExifRequest
    {
        public AddExifDateSource DateSource;
        public DateTime FixedDate;
        public string Description;
        public string MakeName;
        public string ModelName;
        public string Artist;
        public string Copyright;
        public bool WriteFullTemplate;

        public ExifEditValues CreateValuesFor(FileInfo file)
        {
            DateTime? date = null;
            switch (DateSource)
            {
                case AddExifDateSource.CreationTime: date = file.CreationTime; break;
                case AddExifDateSource.LastWriteTime: date = file.LastWriteTime; break;
                case AddExifDateSource.Fixed: date = FixedDate; break;
            }
            return new ExifEditValues
            {
                OriginalDateTime = date,
                Description = NullIfEmpty(Description),
                MakeName = NullIfEmpty(MakeName),
                ModelName = NullIfEmpty(ModelName),
                Artist = NullIfEmpty(Artist),
                Copyright = NullIfEmpty(Copyright),
                WriteFullTemplate = WriteFullTemplate
            };
        }

        /// <summary>Empty field in the ADD dialog means "do not write" (null), not "remove tag" ("").</summary>
        private static string NullIfEmpty(string value)
        {
            return string.IsNullOrEmpty(value) ? null : value;
        }
    }

    /// <summary>
    /// Dialog for batch-adding EXIF metadata to image files that have none
    /// (scans, messenger downloads, screenshots). The date taken can be filled
    /// from the file system dates so the files become renamable by date.
    /// </summary>
    internal class AddExifForm : Form
    {
        private readonly RadioButton rbDateCreated = new RadioButton();
        private readonly RadioButton rbDateModified = new RadioButton();
        private readonly RadioButton rbDateFixed = new RadioButton();
        private readonly RadioButton rbDateNone = new RadioButton();
        private readonly DateTimePicker dtFixed = new DateTimePicker();
        private readonly TextBox edDescription = new TextBox();
        private readonly TextBox edMake = new TextBox();
        private readonly TextBox edModel = new TextBox();
        private readonly TextBox edArtist = new TextBox();
        private readonly TextBox edCopyright = new TextBox();
        private readonly CheckBox chFullTemplate = new CheckBox();
        private readonly ToolTip _toolTip = new ToolTip();

        public AddExifForm(int targetCount)
        {
            this.InitializeLayout(targetCount);
        }

        public AddExifRequest GetRequest()
        {
            var source = AddExifDateSource.None;
            if (rbDateCreated.Checked) source = AddExifDateSource.CreationTime;
            else if (rbDateModified.Checked) source = AddExifDateSource.LastWriteTime;
            else if (rbDateFixed.Checked) source = AddExifDateSource.Fixed;

            return new AddExifRequest
            {
                DateSource = source,
                FixedDate = dtFixed.Value,
                Description = edDescription.Text.Trim(),
                MakeName = edMake.Text.Trim(),
                ModelName = edModel.Text.Trim(),
                Artist = edArtist.Text.Trim(),
                Copyright = edCopyright.Text.Trim(),
                WriteFullTemplate = chFullTemplate.Checked
            };
        }

        private void InitializeLayout(int targetCount)
        {
            Text = string.Format("Add EXIF to Files ({0})", targetCount);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(400, 424);

            var laHint = new Label
            {
                Text = string.Format(
                    "Writes EXIF metadata into {0} selected file(s) without EXIF\n(scans, messenger photos) — e.g. to rename them by date.", targetCount),
                Location = new Point(16, 12),
                Size = new Size(370, 32)
            };

            var gbDate = new GroupBox { Text = "Date taken", Location = new Point(16, 50), Size = new Size(368, 118) };
            rbDateCreated.Text = "File creation time";
            rbDateCreated.Location = new Point(12, 20);
            rbDateCreated.AutoSize = true;
            rbDateCreated.Checked = true;
            rbDateModified.Text = "File last write time";
            rbDateModified.Location = new Point(12, 42);
            rbDateModified.AutoSize = true;
            rbDateFixed.Text = "Fixed:";
            rbDateFixed.Location = new Point(12, 64);
            rbDateFixed.AutoSize = true;
            rbDateFixed.CheckedChanged += (s, e) => dtFixed.Enabled = rbDateFixed.Checked;
            dtFixed.Format = DateTimePickerFormat.Custom;
            dtFixed.CustomFormat = "yyyy-MM-dd  HH:mm:ss";
            dtFixed.ShowUpDown = true;
            dtFixed.Enabled = false;
            dtFixed.Location = new Point(78, 62);
            dtFixed.Size = new Size(170, 22);
            rbDateNone.Text = "Do not set the date";
            rbDateNone.Location = new Point(12, 88);
            rbDateNone.AutoSize = true;
            gbDate.Controls.Add(rbDateCreated);
            gbDate.Controls.Add(rbDateModified);
            gbDate.Controls.Add(rbDateFixed);
            gbDate.Controls.Add(dtFixed);
            gbDate.Controls.Add(rbDateNone);

            var gbText = new GroupBox { Text = "Optional text tags (empty = not written)", Location = new Point(16, 176), Size = new Size(368, 128) };
            AddRow(gbText, "Description", edDescription, 0);
            AddRow(gbText, "Camera make", edMake, 1);
            AddRow(gbText, "Camera model", edModel, 2);
            AddRow(gbText, "Artist", edArtist, 3);
            // Copyright помещаем в ту же сетку пятой строкой — компактно в 2 колонки не влезает,
            // поэтому просто увеличиваем высоту группы под 5 строк
            gbText.Height = 152;
            AddRow(gbText, "Copyright", edCopyright, 4);

            var buApply = new Button
            {
                Text = "Add EXIF",
                DialogResult = DialogResult.OK,
                BackColor = Color.FromArgb(16, 137, 62),
                ForeColor = Color.White,
                UseVisualStyleBackColor = false,
                Size = new Size(110, 28),
                Location = new Point(ClientSize.Width - 216, ClientSize.Height - 40)
            };
            var buCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Size = new Size(90, 28),
                Location = new Point(ClientSize.Width - 100, ClientSize.Height - 40)
            };

            chFullTemplate.Text = "Write full EXIF template (all standard tags)";
            chFullTemplate.Location = new Point(16, 334);
            chFullTemplate.AutoSize = true;
            _toolTip.SetToolTip(chFullTemplate,
                "Unchecked: write only the fields above.\n" +
                "Checked: also add standard EXIF tags (orientation, resolution, color space,\n" +
                "EXIF version, software) with default values, so the file gets a complete EXIF block.");

            Controls.Add(laHint);
            Controls.Add(gbDate);
            Controls.Add(gbText);
            Controls.Add(chFullTemplate);
            Controls.Add(buApply);
            Controls.Add(buCancel);
            AcceptButton = buApply;
            CancelButton = buCancel;
        }

        private static void AddRow(GroupBox group, string caption, TextBox editor, int row)
        {
            var label = new Label
            {
                Text = caption,
                Location = new Point(12, 22 + row * 25),
                AutoSize = true
            };
            editor.Location = new Point(110, 19 + row * 25);
            editor.Size = new Size(244, 22);
            group.Controls.Add(label);
            group.Controls.Add(editor);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK && !chFullTemplate.Checked && rbDateNone.Checked
                && edDescription.Text.Trim().Length == 0 && edMake.Text.Trim().Length == 0
                && edModel.Text.Trim().Length == 0 && edArtist.Text.Trim().Length == 0
                && edCopyright.Text.Trim().Length == 0)
            {
                MessageBox.Show(this, "Nothing to write: no date source, all text fields are empty,\nand the full template option is off.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
            }
            base.OnFormClosing(e);
        }
    }
}
