using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExifFileRenamer
{
    /// <summary>
    /// Dialog for shifting the EXIF date/time of a batch of files by a fixed offset
    /// (e.g. camera clock was wrong or set to another time zone).
    /// </summary>
    internal class TimeShiftForm : Form
    {
        private readonly NumericUpDown edDays = new NumericUpDown();
        private readonly NumericUpDown edHours = new NumericUpDown();
        private readonly NumericUpDown edMinutes = new NumericUpDown();
        private readonly RadioButton rbAll = new RadioButton();
        private readonly RadioButton rbSelected = new RadioButton();
        private readonly Label laPreview = new Label();
        private readonly DateTime _sampleDate;

        public TimeShiftForm(int allCount, int selectedCount, DateTime sampleDate)
        {
            _sampleDate = sampleDate;
            this.InitializeLayout(allCount, selectedCount);
            this.UpdatePreview(null, EventArgs.Empty);
        }

        public TimeSpan Offset
        {
            get
            {
                return new TimeSpan((int)edDays.Value, (int)edHours.Value, (int)edMinutes.Value, 0);
            }
        }

        public bool SelectedOnly { get { return rbSelected.Checked; } }

        private void InitializeLayout(int allCount, int selectedCount)
        {
            Text = "Shift EXIF Dates";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(360, 226);

            var laHint = new Label
            {
                Text = "Shift the date taken of the files by a fixed offset\n(negative values move the date back):",
                Location = new Point(16, 12),
                Size = new Size(330, 32)
            };

            var laDays = new Label { Text = "Days", Location = new Point(16, 52), AutoSize = true };
            var laHours = new Label { Text = "Hours", Location = new Point(126, 52), AutoSize = true };
            var laMinutes = new Label { Text = "Minutes", Location = new Point(236, 52), AutoSize = true };

            SetupNumeric(edDays, -3650, 3650, new Point(16, 70));
            SetupNumeric(edHours, -23, 23, new Point(126, 70));
            SetupNumeric(edMinutes, -59, 59, new Point(236, 70));

            laPreview.Location = new Point(16, 100);
            laPreview.Size = new Size(330, 18);
            laPreview.ForeColor = SystemColors.GrayText;

            rbAll.Text = string.Format("All files with an EXIF date ({0})", allCount);
            rbAll.Location = new Point(16, 124);
            rbAll.AutoSize = true;
            rbAll.Checked = true;

            rbSelected.Text = string.Format("Selected files only ({0})", selectedCount);
            rbSelected.Location = new Point(16, 146);
            rbSelected.AutoSize = true;
            rbSelected.Enabled = selectedCount > 0;
            if (selectedCount > 1)
                rbSelected.Checked = true;

            var buApply = new Button
            {
                Text = "Apply",
                DialogResult = DialogResult.OK,
                BackColor = Color.FromArgb(16, 137, 62),
                ForeColor = Color.White,
                UseVisualStyleBackColor = false,
                Size = new Size(90, 28),
                Location = new Point(ClientSize.Width - 196, ClientSize.Height - 40)
            };
            var buCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Size = new Size(90, 28),
                Location = new Point(ClientSize.Width - 100, ClientSize.Height - 40)
            };

            Controls.Add(laHint);
            Controls.Add(laDays);
            Controls.Add(laHours);
            Controls.Add(laMinutes);
            Controls.Add(edDays);
            Controls.Add(edHours);
            Controls.Add(edMinutes);
            Controls.Add(laPreview);
            Controls.Add(rbAll);
            Controls.Add(rbSelected);
            Controls.Add(buApply);
            Controls.Add(buCancel);
            AcceptButton = buApply;
            CancelButton = buCancel;
        }

        private void SetupNumeric(NumericUpDown editor, int min, int max, Point location)
        {
            editor.Minimum = min;
            editor.Maximum = max;
            editor.Location = location;
            editor.Size = new Size(90, 22);
            editor.TextAlign = HorizontalAlignment.Right;
            editor.ValueChanged += UpdatePreview;
        }

        private void UpdatePreview(object sender, EventArgs e)
        {
            laPreview.Text = string.Format("Example: {0:yyyy-MM-dd HH:mm:ss}  →  {1:yyyy-MM-dd HH:mm:ss}",
                _sampleDate, _sampleDate + Offset);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK && Offset == TimeSpan.Zero)
            {
                MessageBox.Show(this, "The offset is zero — nothing to shift.", Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
            }
            base.OnFormClosing(e);
        }
    }
}
