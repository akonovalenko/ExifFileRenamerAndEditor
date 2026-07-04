using ExifFileRenamer.Model;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExifFileRenamer
{
    /// <summary>
    /// Modal dialog for editing writable EXIF fields of a single image file.
    /// </summary>
    internal class ExifEditForm : Form
    {
        private readonly CheckBox chSetDateTaken = new CheckBox();
        private readonly DateTimePicker dtDateTaken = new DateTimePicker();
        private readonly TextBox edDescription = new TextBox();
        private readonly TextBox edMake = new TextBox();
        private readonly TextBox edModel = new TextBox();
        private readonly TextBox edSoftware = new TextBox();
        private readonly TextBox edArtist = new TextBox();
        private readonly TextBox edCopyright = new TextBox();

        public ExifEditForm(string fileName, ExifInfo current)
        {
            this.InitializeLayout(fileName);
            this.LoadValues(current);
        }

        /// <summary>
        /// Values entered by the user; empty text removes the corresponding tag.
        /// </summary>
        public ExifEditValues GetValues()
        {
            return new ExifEditValues
            {
                OriginalDateTime = chSetDateTaken.Checked ? dtDateTaken.Value : (DateTime?)null,
                Description = edDescription.Text.Trim(),
                MakeName = edMake.Text.Trim(),
                ModelName = edModel.Text.Trim(),
                Software = edSoftware.Text.Trim(),
                Artist = edArtist.Text.Trim(),
                Copyright = edCopyright.Text.Trim()
            };
        }

        private void LoadValues(ExifInfo current)
        {
            if (current == null)
                return;

            if (current.OriginalDateTime != default(DateTime))
            {
                chSetDateTaken.Checked = true;
                dtDateTaken.Value = current.OriginalDateTime;
            }
            edDescription.Text = current.Description;
            edMake.Text = current.MakeName;
            edModel.Text = current.ModelName;
            edSoftware.Text = current.Software;
            edArtist.Text = current.Artist;
            edCopyright.Text = current.Copyright;
        }

        private void InitializeLayout(string fileName)
        {
            Text = "Edit EXIF — " + fileName;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(420, 296);

            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                Padding = new Padding(10),
                AutoSize = false
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            chSetDateTaken.Text = "Date taken";
            chSetDateTaken.Dock = DockStyle.Fill;
            chSetDateTaken.CheckedChanged += (s, e) => dtDateTaken.Enabled = chSetDateTaken.Checked;

            dtDateTaken.Format = DateTimePickerFormat.Custom;
            dtDateTaken.CustomFormat = "yyyy-MM-dd  HH:mm:ss";
            dtDateTaken.ShowUpDown = true;
            dtDateTaken.Enabled = false;
            dtDateTaken.Dock = DockStyle.Fill;

            table.Controls.Add(chSetDateTaken);
            table.Controls.Add(dtDateTaken);
            AddRow(table, "Description", edDescription);
            AddRow(table, "Camera make", edMake);
            AddRow(table, "Camera model", edModel);
            AddRow(table, "Software", edSoftware);
            AddRow(table, "Artist", edArtist);
            AddRow(table, "Copyright", edCopyright);

            var note = new Label
            {
                Text = "Clear a field to remove the tag. JPEG is saved losslessly (metadata only, no re-encoding).",
                ForeColor = SystemColors.GrayText,
                Dock = DockStyle.Fill,
                AutoSize = false
            };
            table.Controls.Add(note);
            table.SetColumnSpan(note, 2);

            var buSave = new Button
            {
                Text = "Save",
                DialogResult = DialogResult.OK,
                BackColor = Color.FromArgb(16, 137, 62),
                ForeColor = Color.White,
                UseVisualStyleBackColor = false,
                Width = 90
            };
            var buCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Width = 90
            };

            var buttons = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Bottom,
                Height = 40,
                Padding = new Padding(0, 5, 10, 5)
            };
            buttons.Controls.Add(buCancel);
            buttons.Controls.Add(buSave);

            Controls.Add(table);
            Controls.Add(buttons);
            AcceptButton = buSave;
            CancelButton = buCancel;
        }

        private static void AddRow(TableLayoutPanel table, string caption, TextBox editor)
        {
            var label = new Label
            {
                Text = caption,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            editor.Dock = DockStyle.Fill;
            table.Controls.Add(label);
            table.Controls.Add(editor);
        }
    }
}
