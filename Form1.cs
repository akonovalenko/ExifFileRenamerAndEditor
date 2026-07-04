using ExifFileRenamer.Model;
using ExifImageRenamer.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExifFileRenamer
{
    public partial class Form1 : Form
    {
        #region Private fields
        private ApplicationState appState;
        private FileNameTemplateFactory _fileNameTemplate;
        private BackgroundWorker refreshBgWorker;
        private BackgroundWorker renameBgWorker;
        private BackgroundWorker analyzeBgWorker;
        private BackgroundWorker exifBatchBgWorker;
        private List<KeyValuePair<string, string>> _lastRenameBatch; // old full name → new full name
        private ExifInfoExtractorService _exifInfoExtractor;
        private ImageInfoExtractorService _imageInfoExtractor;
        private long _currentProcessedCount;
        private int _lastReportedPercent;
        private readonly ToolTip _toolTip = new ToolTip();
        #endregion

        #region Initialization

        public Form1()
        {
            this.InitializeComponent();
            this.InitializeUserComponents();
        }

        private void InitializeUserComponents()
        {
            this._fileNameTemplate = new FileNameTemplateFactory(
            chDateTimePrefix.Checked,
            chSuffixDelimiter.Checked,
            chCounter.Checked,
            chImageFilePrefix.Checked,
            chUseCameraName.Checked,
            chUseSoftwareName.Checked,
            chImageOrientation.Checked,
            chUseImageSize.Checked,
            edFilePrefix.Text,
            cbDateTimeStampFormat.Text,
            edSuffixDelimiter.Text,
            edCounterSuffix.Text,
            "MakeName",
                "ModelName",
                "Software",
                "Orientation",
                "100x100"
            );
            this._fileNameTemplate.PropertyChanged += FileNameTemplate_PropertyChanged;

            this._exifInfoExtractor = new ExifInfoExtractorService();
            this._imageInfoExtractor = new ImageInfoExtractorService();
            this.appState = ApplicationState.GetInstance();

            this.FileNameTemplate_PropertyChanged(null, null);
            this.InitializeDataGrid();

            // Tooltips
            _toolTip.SetToolTip(buRefresh,  "Reload file list from the selected folder (Ctrl+R)");
            _toolTip.SetToolTip(buOpenPath,  "Browse for images folder (Ctrl+O)");
            _toolTip.SetToolTip(buAnalyze,   "Preview new file names based on current settings (F5)");
            _toolTip.SetToolTip(buRename,    "Rename files according to preview (F9)");
            _toolTip.SetToolTip(buEditExif,  "Edit EXIF fields of the selected image and save to file (Ctrl+E)");
            _toolTip.SetToolTip(buAddExif,   "Add EXIF metadata to files that have none (date from file time, camera, artist...)");
            _toolTip.SetToolTip(buDeleteExif, "Remove ALL EXIF metadata from the selected files");

            // Button colors: Refresh = teal, Preview = blue, Rename = green
            buRefresh.FlatStyle = FlatStyle.Flat;
           // buRefresh.Image = CreateRefreshIcon(Color.White);
            buAnalyze.UseVisualStyleBackColor = false;
            buAnalyze.BackColor = Color.FromArgb(0, 120, 215);
            buAnalyze.ForeColor = Color.White;
            buRename.UseVisualStyleBackColor = false;
            buRename.BackColor = Color.FromArgb(16, 137, 62);
            buRename.ForeColor = Color.White;

            // Status bar: stretch state label to avoid truncation
            stlaState.Spring = true;

            // Info panel: alternating row colors
            lbFileInfo.DrawMode = DrawMode.OwnerDrawFixed;
            lbFileInfo.DrawItem += LbFileInfo_DrawItem;
        }

        #endregion

        #region Datagrid

        private void InitializeDataGrid()
        {
            dataGridViewFiles.AutoGenerateColumns = false;
            dataGridViewFiles.Columns.Clear();
            dataGridViewFiles.RowHeadersVisible = false;
            dataGridViewFiles.ColumnHeadersVisible = true;

            var colOldName = AddColumn("SourceFileName", "Old file name");
            colOldName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colOldName.FillWeight = 35;
            colOldName.MinimumWidth = 120;

            var colNewName = AddColumn("TargetFileName", "New file name");
            colNewName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colNewName.FillWeight = 35;
            colNewName.MinimumWidth = 120;

            var colDateTime = AddColumn("ImageDateTime", "Image DateTime");
            colDateTime.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colDateTime.FillWeight = 20;
            colDateTime.MinimumWidth = 130;

            var colStatus = AddColumn("Status", "Status");
            colStatus.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colStatus.FillWeight = 10;
            colStatus.MinimumWidth = 80;

            dataGridViewFiles.Columns.Add(colOldName);
            dataGridViewFiles.Columns.Add(colNewName);
            dataGridViewFiles.Columns.Add(colDateTime);
            dataGridViewFiles.Columns.Add(colStatus);

            dataGridViewFiles.CellToolTipTextNeeded += DataGridViewFiles_CellToolTipTextNeeded;
            dataGridViewFiles.CellFormatting += DataGridViewFiles_CellFormatting;
        }

        private void DataGridViewFiles_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var fi = dataGridViewFiles.Rows[e.RowIndex].DataBoundItem as ProcessingFileInfo;
            if (fi == null || fi.IsExifImage)
                return;
            if (fi.IsBitmapImage)
            {
                // картинки без EXIF — светло-серый фон: кандидаты на Add EXIF
                e.CellStyle.BackColor = Color.FromArgb(235, 235, 235);
            }
            else
            {
                // не изображения (mp4 и т.п.) — бледно-красный фон: EXIF добавить нельзя
                e.CellStyle.BackColor = Color.FromArgb(252, 228, 228);
            }
        }

        DataGridViewTextBoxColumn AddColumn(string FieldName, string Caption)
        {
            return new DataGridViewTextBoxColumn
            {
                DataPropertyName = FieldName,
                HeaderText = Caption,
                Name = Caption,
                ReadOnly = true
            };
        }

        private void DataGridViewFiles_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var fi = dataGridViewFiles.Rows[e.RowIndex].DataBoundItem as ProcessingFileInfo;
            if (fi == null) return;
            e.ToolTipText = fi.FullName + "\nStatus: " + fi.Status;
        }

        #endregion

        #region Form events
        private void BuClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.LoadSettings();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }
        #endregion

        #region Settings
        private void SaveSettings()
        {
            var settings = new Settings
            {
                Width = Width,
                Height = Height,
                Top = Top,
                Left = Left,
                SelectedPath = folderBrowserDialog1.SelectedPath,
                FileTypesSelectedIndex = cbFileTypes.SelectedIndex
            };
            this.appState.SaveSettings(settings);
        }

        private void LoadSettings()
        {
            var settings = this.appState.Settings;
            if (settings != null)
            {
                Width = settings.Width;
                Height = settings.Height;
                Top = settings.Top;
                Left = settings.Left;
                folderBrowserDialog1.SelectedPath = settings.SelectedPath;
                laPath.Text = folderBrowserDialog1.SelectedPath;
                cbFileTypes.SelectedIndex = settings.FileTypesSelectedIndex >= 0
                    ? settings.FileTypesSelectedIndex
                    : 0;
            }
            else
            {
                cbFileTypes.SelectedIndex = 0; // default: *.*
            }
            this.cbFileTypes.SelectedIndexChanged += new System.EventHandler(this.CbFileTypes_SelectedIndexChanged);
        }
        #endregion

        #region Refresh worker

        private void RunRefreshWorker()
        {
            refreshBgWorker = new BackgroundWorker();
            refreshBgWorker.DoWork += new DoWorkEventHandler(RefreshBgWorker_DoWork);
            refreshBgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RefreshBgWorker_RunWorkerCompleted);
            refreshBgWorker.ProgressChanged += new ProgressChangedEventHandler(UpdateProgressValue);
            refreshBgWorker.ProgressChanged += new ProgressChangedEventHandler(UpdateImagesCountLabel);
            refreshBgWorker.WorkerReportsProgress = true;
            refreshBgWorker.WorkerSupportsCancellation = true;
            refreshBgWorker.RunWorkerAsync();
        }

        private void BuOpenPath_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if ((result == DialogResult.OK) || (result == DialogResult.Yes))
            {
                laPath.Text = folderBrowserDialog1.SelectedPath;
                this.RunRefreshWorker();
            }
        }

        private void BuRefresh_Click(object sender, EventArgs e)
        {
            this.RunRefreshWorker();
        }

        void RefreshBgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            _currentProcessedCount = 0;
            _lastReportedPercent = -1;
            this.UpdateFileList();
        }

        void RefreshBgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.WorkerCompleted((x) =>
            {
                refreshBgWorker.Dispose();
                this.StatisticsStopProcess();
            }, e);
        }

        #endregion

        #region File processing

        /// <summary>
        /// Extracts info for one file. Thread-safe: called concurrently from Parallel.For.
        /// Returns null when the file does not match the extension filter.
        /// </summary>
        internal ProcessingFileInfo ProcessFile(FileInfo fi, string[] extFile, int length)
        {
            try
            {
                if (Array.IndexOf(extFile, fi.Extension.TrimStart('.').ToUpper()) < 0 && extFile[0] != "*.*")
                    return null;

                ProcessingFileInfo fileInfo;
                if (this._exifInfoExtractor.TryExtractExifInfo(fi.FullName, out ExifInfo exif, out string errorExif))
                    fileInfo = new ProcessingFileInfo(fi, exif, errorExif);
                else if (this._imageInfoExtractor.TryExtractImageInfo(fi.FullName, out ImageInfo img, out string errorImage))
                    fileInfo = new ProcessingFileInfo(fi, img, errorImage);
                else
                    fileInfo = new ProcessingFileInfo(fi);

                var count = Interlocked.Increment(ref _currentProcessedCount);
                if (length > 0)
                {
                    // report only when the percent value changes to avoid flooding the UI queue
                    var percent = (int)(count * 100L / length);
                    if (Interlocked.Exchange(ref _lastReportedPercent, percent) != percent || count == length)
                        refreshBgWorker.ReportProgress(percent, string.Format("{0}/{1}", count, length));
                }
                return fileInfo;
            }
            catch (Exception ex)
            {
                return new ProcessingFileInfo(fi) { ErrorText = ex.ToString() };
            }
        }

        void UpdateFileList()
        {
            string selectedPath = null;
            string[] extFile = null;
            bool includeSubdirs = false;

            Invoke(new Action(() =>
            {
                dataGridViewFiles.DataSource = null;
                appState.ImagesFiles.Clear();
                selectedPath = folderBrowserDialog1.SelectedPath;
                extFile = cbFileTypes.Text.ToUpper().Split(';');
                includeSubdirs = chIncludeSubdirectories.Checked;
            }));

            if (string.IsNullOrEmpty(selectedPath) || !Directory.Exists(selectedPath))
                return;

            var di = new DirectoryInfo(selectedPath);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            FileInfo[] files = di.GetFiles("*", includeSubdirs
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly);

            Invoke(new Action(() => this.StatisticsStartProcess(files.Length, $"Start processing {files.Length} images")));

            // extraction is I/O + decode bound: process files on all CPU cores;
            // the indexed results array keeps the original file order
            var results = new ProcessingFileInfo[files.Length];
            var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
            Parallel.For(0, files.Length, parallelOptions, (i, loopState) =>
            {
                if (refreshBgWorker.CancellationPending)
                {
                    loopState.Stop();
                    return;
                }
                results[i] = this.ProcessFile(files[i], extFile, files.Length);
            });

            foreach (var fileInfo in results)
            {
                if (fileInfo != null)
                    appState.ImagesFiles.Add(fileInfo);
            }

            var source = new BindingSource { DataSource = appState.ImagesFiles };
            watch.Stop();
            var elapsedSec = watch.Elapsed.TotalSeconds;

            Invoke(new Action(() =>
            {
                dataGridViewFiles.DataSource = typeof(List<ProcessingFileInfo>);
                dataGridViewFiles.DataSource = source;
                laImagesCount.Text = appState.ImagesFiles.Count.ToString();
                stlaState.Text = $"{appState.ImagesFiles.Count} files processed in {elapsedSec:0.##} seconds.";
            }));
        }

        #endregion

        #region Picture preview

        private void ShowPicture(string fileName)
        {
            var oldImage = pictureBox1.Image;
            pictureBox1.Image = null;
            oldImage?.Dispose();

            var bytes = File.ReadAllBytes(fileName);
            using (var ms = new MemoryStream(bytes))
            using (var img = Image.FromStream(ms))
            {
                var bmp = new Bitmap(img);
                ApplyExifOrientation(bmp);
                pictureBox1.Image = bmp;
            }
        }

        private static void ApplyExifOrientation(Bitmap bmp)
        {
            const int ORIENTATION_TAG = 274;
            try
            {
                var prop = bmp.GetPropertyItem(ORIENTATION_TAG);
                if (prop?.Value == null || prop.Value.Length < 2) return;
                var orientation = BitConverter.ToInt16(prop.Value, 0);
                switch (orientation)
                {
                    case 2: bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);   break;
                    case 3: bmp.RotateFlip(RotateFlipType.Rotate180FlipNone); break;
                    case 4: bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);   break;
                    case 5: bmp.RotateFlip(RotateFlipType.Rotate270FlipX);    break;
                    case 6: bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);  break;
                    case 7: bmp.RotateFlip(RotateFlipType.Rotate90FlipX);     break;
                    case 8: bmp.RotateFlip(RotateFlipType.Rotate270FlipNone); break;
                }
            }
            catch (ArgumentException) { } // tag not present in this image
        }

        // Показываем все поля EXIF при чтении, в том числе пустые.
        private void AddInfo(string caption, string value)
        {
            lbFileInfo.Items.Add(caption + " - " + value);
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            buEditExif.Enabled = false;
            buAddExif.Enabled = false;
            buDeleteExif.Enabled = false;
            editExifToolStripMenuItem.Enabled = false;
            if ((dataGridViewFiles.RowCount < 1) || (dataGridViewFiles.SelectedRows.Count < 1)) return;
            ProcessingFileInfo fi = (ProcessingFileInfo)dataGridViewFiles.SelectedRows[0].DataBoundItem;
            buEditExif.Enabled = fi.IsExifImage;                       // редактирование — только где EXIF есть
            buDeleteExif.Enabled = fi.IsExifImage;
            buAddExif.Enabled = !fi.IsExifImage && fi.IsBitmapImage;   // добавление — только где EXIF нет
            editExifToolStripMenuItem.Enabled = buEditExif.Enabled;
            try
            {
                lbFileInfo.Items.Clear();

                lbFileInfo.Items.Add("Size, byte - " + fi.SizeInBytes);
                if (fi.IsExifImage)
                {
                    var exif = fi.Exif;
                    lbFileInfo.Items.Add("File create date time - " + fi.CreatedOn);
                    lbFileInfo.Items.Add("---   EXIF Info   ---");
                    // показываем все поля EXIF, включая пустые
                    bool hasDate = exif.OriginalDateTime != default(DateTime);
                    AddInfo("Date", hasDate ? exif.OriginalDateTime.ToShortDateString() : string.Empty);
                    AddInfo("Time", hasDate ? exif.OriginalDateTime.ToLongTimeString() : string.Empty);
                    AddInfo("Description", exif.Description);
                    AddInfo("Make", exif.MakeName);
                    AddInfo("Model", exif.ModelName);
                    AddInfo("Software", exif.Software);
                    AddInfo("Image size (X:Y)", exif.Width + " : " + exif.Height);
                    AddInfo("ResolutionUnit", exif.ResolutionUnit);
                    AddInfo("Resolution (X:Y)", exif.XResolution + " : " + exif.YResolution);
                    AddInfo("Orientation", exif.Orientation);
                    AddInfo("Exif Version", exif.ExifVersion);
                    AddInfo("Focal Length", exif.FocalLength);
                    AddInfo("Exposure", exif.ExposureTime);
                    AddInfo("Aperture", exif.FNumber);
                    AddInfo("ISO", exif.ISOSpeed);
                    AddInfo("Flash", exif.Flash);
                    AddInfo("Exposure program", exif.ExposureProgram);
                    AddInfo("Metering mode", exif.MeteringMode);
                    AddInfo("Exposure bias", exif.ExposureBias);
                    AddInfo("White balance", exif.WhiteBalance);
                    AddInfo("Exposure mode", exif.ExposureMode);
                    AddInfo("Focal (35mm eq.)", exif.FocalLengthIn35mm);
                    AddInfo("Scene type", exif.SceneCaptureType);
                    AddInfo("Color space", exif.ColorSpace);
                    AddInfo("GPS", string.IsNullOrEmpty(exif.GpsLatitude) ? string.Empty : exif.GpsLatitude + ", " + exif.GpsLongitude);
                    AddInfo("Artist", exif.Artist);
                    AddInfo("Copyright", exif.Copyright);
                }
                else if (fi.IsBitmapImage)
                {
                    lbFileInfo.Items.Add("File create date time - " + fi.Bitmap.OriginalDateTime);
                    lbFileInfo.Items.Add("Image size (X:Y) " + fi.Bitmap.Width + " : " + fi.Bitmap.Height);
                    lbFileInfo.Items.Add("Resolution dpi (V:H) " + fi.Bitmap.VerticalResolution + " : " + fi.Bitmap.HorizontalResolution);
                    lbFileInfo.Items.Add("Pixel format - " + fi.Bitmap.PixelFormat);
                }
                else
                {
                    lbFileInfo.Items.Add("File with extension [" + fi.Extension + "] not recognized as image");
                }

                if (fi.IsBitmapImage || fi.IsExifImage)
                {
                    this.ShowPicture(fi.FullName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Image Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        //private static Bitmap CreateRefreshIcon(Color color)
        //{
        //    var bmp = new Bitmap(16, 16, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        //    using (var g = Graphics.FromImage(bmp))
        //    {
        //        g.Clear(Color.Transparent);
        //        g.SmoothingMode = SmoothingMode.AntiAlias;
        //        using (var pen = new Pen(color, 2.0f))
        //        {
        //            pen.CustomEndCap = new AdjustableArrowCap(3f, 3f, true);
        //            g.DrawArc(pen, 1, 1, 14, 14, 100f, 300f);
        //        }
        //    }
        //    return bmp;
        //}

        #region EXIF editing

        private void BuEditExif_Click(object sender, EventArgs e)
        {
            if (dataGridViewFiles.SelectedRows.Count < 1) return;
            var fi = dataGridViewFiles.SelectedRows[0].DataBoundItem as ProcessingFileInfo;
            if (fi == null || !fi.IsExifImage) return;

            using (var editForm = new ExifEditForm(fi.SourceFileName, fi.Exif))
            {
                if (editForm.ShowDialog(this) != DialogResult.OK)
                    return;

                var writer = new ExifWriterService();
                if (!writer.TrySaveExifInfo(fi.FullName, editForm.GetValues(), out string errors))
                {
                    MessageBox.Show(errors, "EXIF save error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.ReloadFileInfo(fi);
            }
        }

        private void ReloadFileInfo(ProcessingFileInfo fi)
        {
            var index = appState.ImagesFiles.IndexOf(fi);
            if (index < 0) return;

            var updated = this.CreateReloadedFileInfo(fi.FullName);
            appState.ImagesFiles[index] = updated;
            dataGridViewFiles.Refresh();
            this.DataGridView1_SelectionChanged(dataGridViewFiles, EventArgs.Empty);
            stlaState.Text = string.Format("EXIF saved: {0}", updated.SourceFileName);
        }

        /// <summary>Re-extracts file info from disk. Thread-safe: extractors are stateless.</summary>
        private ProcessingFileInfo CreateReloadedFileInfo(string fullName)
        {
            var systemFileInfo = new FileInfo(fullName);
            if (this._exifInfoExtractor.TryExtractExifInfo(fullName, out ExifInfo exif, out string errorExif))
                return new ProcessingFileInfo(systemFileInfo, exif, errorExif);
            if (this._imageInfoExtractor.TryExtractImageInfo(fullName, out ImageInfo img, out string errorImage))
                return new ProcessingFileInfo(systemFileInfo, img, errorImage);
            return new ProcessingFileInfo(systemFileInfo);
        }

        #endregion

        #region Info panel draw

        private void LbFileInfo_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            var backColor = selected
                ? SystemColors.Highlight
                : (e.Index % 2 == 0 ? Color.White : Color.FromArgb(235, 240, 250));
            var textColor = selected ? SystemColors.HighlightText : SystemColors.ControlText;
            using (var brush = new SolidBrush(backColor))
                e.Graphics.FillRectangle(brush, e.Bounds);
            using (var brush = new SolidBrush(textColor))
                e.Graphics.DrawString(lbFileInfo.Items[e.Index].ToString(), e.Font, brush,
                    e.Bounds.X + 2f, e.Bounds.Y + 1f);
            e.DrawFocusRectangle();
        }

        #endregion

        #region Analyze worker

        private void BuAnalyze_Click(object sender, EventArgs e)
        {
            if (appState.ImagesFiles == null || appState.ImagesFiles.Count < 1)
            {
                MessageBox.Show("Error! List of source files is empty!\nPlease,click [...] button for select folder with images!");
                return;
            }

            analyzeBgWorker = new BackgroundWorker();
            analyzeBgWorker.DoWork += new DoWorkEventHandler(AnalyzeImages_BgWorker_DoWork);
            analyzeBgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BgWorker_RunWorkerCompleted);
            analyzeBgWorker.ProgressChanged += new ProgressChangedEventHandler(UpdateProgressValue);
            analyzeBgWorker.WorkerReportsProgress = true;
            analyzeBgWorker.WorkerSupportsCancellation = true;
            analyzeBgWorker.RunWorkerAsync();
        }

        private void AnalyzeImages_BgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            this.AnalyzeImages();
        }

        private void BgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.WorkerCompleted((x) =>
            {
                dataGridViewFiles.Refresh();
                analyzeBgWorker.Dispose();
                this.StatisticsStopProcess();
            }, e);
        }

        private void AnalyzeImages()
        {
            if (appState.ImagesFiles == null)
                return;

            var watch = System.Diagnostics.Stopwatch.StartNew();
            Invoke(new Action(() => stlaState.Text = "Sorting files."));

            (appState.ImagesFiles as List<IProcessingFileInfo>).Sort();

            int totalCount = appState.ImagesFiles.Count;
            Invoke(new Action(() => this.StatisticsStartProcess(totalCount, "Analyzing images info.")));

            bool skipUnprocessed = false;
            Invoke(new Action(() => skipUnprocessed = chSkipUnprocessed.Checked));

            var fileNameFactory = new FileNameFactory();
            int count = 1;

            for (int i = 0; i < totalCount; i++)
            {
                if (!appState.ImagesFiles[i].IsExifImage && skipUnprocessed)
                    continue;

                fileNameFactory.Create(this._fileNameTemplate, appState.ImagesFiles[i], count++);
                analyzeBgWorker.ReportProgress(totalCount > 0 ? i * 100 / totalCount : 0);

                if (analyzeBgWorker.CancellationPending)
                    break;
            }

            watch.Stop();
            var elapsedSec = watch.Elapsed.TotalSeconds;
            Invoke(new Action(() => stlaState.Text = $"{totalCount} files processed in {elapsedSec:0.##} seconds."));
        }

        #endregion

        #region Rename worker

        private void BuRename_Click(object sender, EventArgs e)
        {
            renameBgWorker = new BackgroundWorker();
            renameBgWorker.DoWork += new DoWorkEventHandler(RenameWorkerDoWork);
            renameBgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RenameWorkerCompleted);
            renameBgWorker.ProgressChanged += new ProgressChangedEventHandler(UpdateProgressValue);
            renameBgWorker.WorkerReportsProgress = true;
            renameBgWorker.WorkerSupportsCancellation = true;
            renameBgWorker.RunWorkerAsync();
        }

        private void RenameFiles()
        {
            bool skipUnprocessed = false;
            Invoke(new Action(() => skipUnprocessed = chSkipUnprocessed.Checked));

            var journal = new List<KeyValuePair<string, string>>();
            int count = 0;
            for (int i = 0; i < appState.ImagesFiles.Count; i++)
            {
                ProcessingFileInfo fi = (ProcessingFileInfo)appState.ImagesFiles[i];
                if ((!fi.IsExifImage && skipUnprocessed) || fi.Status == Constants.NO_CHANGES)
                    continue;

                try
                {
                    System.IO.File.Move(fi.FullName, fi.NewFileFullName);
                    journal.Add(new KeyValuePair<string, string>(fi.FullName, fi.NewFileFullName));
                    fi.Status = "renamed";
                    renameBgWorker.ReportProgress(count++);
                    if (renameBgWorker.CancellationPending)
                        break;
                }
                catch (Exception ex)
                {
                    fi.Status = ex.Message;
                }
            }
            if (journal.Count > 0)
                _lastRenameBatch = journal;
        }

        void RenameWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            RenameFiles();
        }

        void RenameWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            undoRenameToolStripMenuItem.Enabled = _lastRenameBatch != null && _lastRenameBatch.Count > 0;

            if (e.Error != null)
                MessageBox.Show(e.Error.Message);
            else if (e.Cancelled)
                MessageBox.Show("Canceled");
            else
                BuRefresh_Click(sender, e);

            renameBgWorker.Dispose();
            this.StatisticsStopProcess();
        }

        private void UndoRenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var batch = _lastRenameBatch;
            if (batch == null || batch.Count == 0)
                return;

            if (MessageBox.Show(this,
                string.Format("Restore the original names of {0} renamed file(s)?", batch.Count),
                "Undo Last Rename", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            int restored = 0;
            for (int i = batch.Count - 1; i >= 0; i--)
            {
                try
                {
                    File.Move(batch[i].Value, batch[i].Key);
                    restored++;
                }
                catch
                {
                    // file was moved, renamed again or locked — reported in the summary below
                }
            }

            _lastRenameBatch = null;
            undoRenameToolStripMenuItem.Enabled = false;
            stlaState.Text = string.Format("Undo: restored {0} of {1} file name(s).", restored, batch.Count);
            if (restored < batch.Count)
                MessageBox.Show(this,
                    string.Format("{0} file(s) could not be restored (moved, renamed again or locked).", batch.Count - restored),
                    "Undo Last Rename", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            BuRefresh_Click(sender, e);
        }

        #endregion

        #region EXIF batch operations (shift dates, remove EXIF)

        private void ShiftDatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var all = this.GetFilesWithExifDate(false);
            if (all.Count == 0)
            {
                MessageBox.Show(this, "There are no files with an EXIF date in the list.\nLoad a folder first (Ctrl+R).",
                    "Shift EXIF Dates", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selected = this.GetFilesWithExifDate(true);
            var sample = (selected.Count > 0 ? selected : all)[0].Exif.OriginalDateTime;

            using (var shiftForm = new TimeShiftForm(all.Count, selected.Count, sample))
            {
                if (shiftForm.ShowDialog(this) != DialogResult.OK)
                    return;

                var targets = shiftForm.SelectedOnly ? selected : all;
                var offset = shiftForm.Offset;
                var writer = new ExifWriterService();
                this.RunExifBatchWorker(targets, fi =>
                {
                    var values = new ExifEditValues { OriginalDateTime = fi.Exif.OriginalDateTime + offset };
                    return writer.TrySaveExifInfo(fi.FullName, values, out string error) ? null : error;
                }, "Shifting EXIF dates");
            }
        }

        private void RemoveExifToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var targets = this.GetFilesWithExif(true);
            if (targets.Count == 0)
            {
                MessageBox.Show(this, "Select the files to clean in the list first\n(Ctrl+A selects all; only files with EXIF are processed).",
                    "Remove EXIF", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(this,
                string.Format("Remove ALL EXIF metadata from {0} selected file(s)?\nThis cannot be undone.", targets.Count),
                "Remove EXIF", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            var writer = new ExifWriterService();
            this.RunExifBatchWorker(targets, fi =>
                writer.TryRemoveExifInfo(fi.FullName, out string error) ? null : error,
                "Removing EXIF metadata");
        }

        private void AddExifToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var targets = this.GetFilesWithoutExif(true);
            if (targets.Count == 0)
            {
                MessageBox.Show(this, "Select the files without EXIF metadata in the list first.\nFiles that already have EXIF are not affected by this operation.",
                    "Add EXIF to Files", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var addForm = new AddExifForm(targets.Count))
            {
                if (addForm.ShowDialog(this) != DialogResult.OK)
                    return;

                var request = addForm.GetRequest();   // snapshot: живёт дольше формы
                var writer = new ExifWriterService();
                this.RunExifBatchWorker(targets, fi =>
                {
                    var values = request.CreateValuesFor(new FileInfo(fi.FullName));
                    return writer.TrySaveExifInfo(fi.FullName, values, out string error) ? null : error;
                }, "Adding EXIF metadata");
            }
        }

        private List<ProcessingFileInfo> GetFilesWithoutExif(bool selectedOnly)
        {
            var result = new List<ProcessingFileInfo>();
            if (selectedOnly)
            {
                foreach (DataGridViewRow row in dataGridViewFiles.SelectedRows)
                {
                    var fi = row.DataBoundItem as ProcessingFileInfo;
                    if (fi != null && !fi.IsExifImage && fi.IsBitmapImage)
                        result.Add(fi);
                }
                result.Reverse();
            }
            else if (appState.ImagesFiles != null)
            {
                foreach (var item in appState.ImagesFiles)
                {
                    var fi = item as ProcessingFileInfo;
                    if (fi != null && !fi.IsExifImage && fi.IsBitmapImage)
                        result.Add(fi);
                }
            }
            return result;
        }

        private List<ProcessingFileInfo> GetFilesWithExif(bool selectedOnly)
        {
            var result = new List<ProcessingFileInfo>();
            if (selectedOnly)
            {
                foreach (DataGridViewRow row in dataGridViewFiles.SelectedRows)
                {
                    var fi = row.DataBoundItem as ProcessingFileInfo;
                    if (fi != null && fi.IsExifImage)
                        result.Add(fi);
                }
                result.Reverse(); // SelectedRows enumerates in reverse order
            }
            else if (appState.ImagesFiles != null)
            {
                foreach (var item in appState.ImagesFiles)
                {
                    var fi = item as ProcessingFileInfo;
                    if (fi != null && fi.IsExifImage)
                        result.Add(fi);
                }
            }
            return result;
        }

        private List<ProcessingFileInfo> GetFilesWithExifDate(bool selectedOnly)
        {
            var result = this.GetFilesWithExif(selectedOnly);
            result.RemoveAll(fi => fi.Exif == null || fi.Exif.OriginalDateTime == default(DateTime));
            return result;
        }

        /// <summary>
        /// Runs an EXIF write operation over a list of files on a background worker.
        /// The operation returns null on success or an error text for the Status column.
        /// </summary>
        private void RunExifBatchWorker(List<ProcessingFileInfo> targets, Func<ProcessingFileInfo, string> operation, string operationName)
        {
            var worker = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            exifBatchBgWorker = worker;
            worker.ProgressChanged += UpdateProgressValue;

            worker.DoWork += (s, e) =>
            {
                int failed = 0;
                for (int i = 0; i < targets.Count; i++)
                {
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                    var target = targets[i];
                    var error = operation(target);
                    if (error != null)
                    {
                        failed++;
                        target.Status = error;
                    }
                    else
                    {
                        // перечитываем только изменённый файл (в фоне) и подменяем его в списке
                        var updated = this.CreateReloadedFileInfo(target.FullName);
                        Invoke(new Action(() =>
                        {
                            var index = appState.ImagesFiles.IndexOf(target);
                            if (index >= 0)
                                appState.ImagesFiles[index] = updated;
                        }));
                    }
                    worker.ReportProgress((i + 1) * 100 / targets.Count);
                }
                e.Result = failed;
            };

            worker.RunWorkerCompleted += (s, e) =>
            {
                this.StatisticsStopProcess();
                exifBatchBgWorker = null;
                worker.Dispose();

                dataGridViewFiles.Refresh();
                this.DataGridView1_SelectionChanged(dataGridViewFiles, EventArgs.Empty);

                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.Message);
                }
                else if (!e.Cancelled && e.Result is int failed && failed > 0)
                {
                    stlaState.Text = string.Format("{0}: {1} of {2} file(s) failed.", operationName, failed, targets.Count);
                    MessageBox.Show(this,
                        string.Format("{0} of {1} file(s) failed — see the Status column.", failed, targets.Count),
                        operationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    stlaState.Text = string.Format("{0}: {1} file(s) updated.", operationName, targets.Count);
                }
            };

            this.StatisticsStartProcess(100, operationName + "...");
            worker.RunWorkerAsync();
        }

        #endregion

        #region Workers common actions

        private void WorkerCompleted(Action<RunWorkerCompletedEventArgs> action, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                MessageBox.Show(e.Error.Message);
            else if (e.Cancelled)
                MessageBox.Show("Canceled");
            action.Invoke(e);
        }

        void UpdateProgressValue(object sender, ProgressChangedEventArgs e)
        {
            pbTotal.Value = Math.Min(e.ProgressPercentage, pbTotal.Maximum);
        }

        void UpdateImagesCountLabel(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState != null)
                laImagesCount.Text = e.UserState.ToString();
        }

        public void StatisticsStartProcess(int max, string message)
        {
            buCancelProcess.Enabled = true;
            pbTotal.Maximum = max > 0 ? max : 1;
            pbTotal.Value = 0;
            panelControls.Enabled = false;
            stlaState.Text = message;
        }

        public void StatisticsStopProcess()
        {
            pbTotal.Value = 0;
            buCancelProcess.Enabled = false;
            panelControls.Enabled = true;
        }

        #endregion

        #region Template controls

        private void ChDateTimePrefix_CheckedChanged(object sender, EventArgs e)
        {
            var status = chDateTimePrefix.Checked;
            cbDateTimeStampFormat.Enabled = status;
            this._fileNameTemplate.UseDateTimePart = status;
            if (status)
                this._fileNameTemplate.DateTimePart = cbDateTimeStampFormat.Text;
        }

        private void ChCounter_CheckedChanged(object sender, EventArgs e)
        {
            var status = chCounter.Checked;
            edCounterSuffix.Enabled = chCounter.Checked;
            this._fileNameTemplate.UseCounterSuffix = chCounter.Checked;
            if (status)
                this._fileNameTemplate.CounterSuffix = edCounterSuffix.Text;
        }

        private void ChSuffixDelimiter_CheckedChanged(object sender, EventArgs e)
        {
            var status = chSuffixDelimiter.Checked;
            edSuffixDelimiter.Enabled = status;
            this._fileNameTemplate.UseSuffixDelimiter = status;
            if (status)
                this._fileNameTemplate.SuffixDelimiter = edSuffixDelimiter.Text;
        }

        private void ChUseCameraName_CheckedChanged(object sender, EventArgs e)
        {
            this._fileNameTemplate.UseCameraName = chUseCameraName.Checked;
        }

        private void ChUseImageSize_CheckedChanged(object sender, EventArgs e)
        {
            this._fileNameTemplate.UseImageSize = chUseImageSize.Checked;
        }

        private void ChImageOrientation_CheckedChanged(object sender, EventArgs e)
        {
            this._fileNameTemplate.UseImageOrientation = chImageOrientation.Checked;
        }

        private void EdFilePrefix_TextChanged(object sender, EventArgs e)
        {
            this._fileNameTemplate.ImageFileNamePrefix = edFilePrefix.Text;
        }

        private void CbDateTimeStampFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._fileNameTemplate.DateTimePart = cbDateTimeStampFormat.Text;
        }

        private void EdCounterSuffix_TextChanged(object sender, EventArgs e)
        {
            this._fileNameTemplate.CounterSuffix = edCounterSuffix.Text;
        }

        private void EdSuffixDelimiter_TextChanged(object sender, EventArgs e)
        {
            this._fileNameTemplate.SuffixDelimiter = edSuffixDelimiter.Text;
        }

        private void FileNameTemplate_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            edFileNameTemplateSample.Text = this._fileNameTemplate.Build();
        }

        #endregion

        #region Cancel

        private void ToolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            if (refreshBgWorker != null && refreshBgWorker.IsBusy)
                refreshBgWorker.CancelAsync();
            if (analyzeBgWorker != null && analyzeBgWorker.IsBusy)
                analyzeBgWorker.CancelAsync();
            if (renameBgWorker != null && renameBgWorker.IsBusy)
                renameBgWorker.CancelAsync();
            if (exifBatchBgWorker != null && exifBatchBgWorker.IsBusy)
                exifBatchBgWorker.CancelAsync();
        }

        #endregion

        private void ChImageFilePrefix_CheckedChanged(object sender, EventArgs e)
        {
            var status = chImageFilePrefix.Checked;
            edFilePrefix.Enabled = status;
            this._fileNameTemplate.UseImageFilePrefixPart = status;
            if (status)
                this._fileNameTemplate.ImageFileNamePrefix = edFilePrefix.Text;
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var helpForm = new HelpForm())
                helpForm.ShowDialog(this);
        }

        private void AboutProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var aboutForm = new AboutForm())
                aboutForm.ShowDialog(this);
        }

        private void ChSoftware_CheckedChanged(object sender, EventArgs e)
        {
            this._fileNameTemplate.UseSoftwareName = chUseSoftwareName.Checked;
        }

        private void CbFileTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox cb)
            {
                if (appState.FileTypesSelectedIndex != cb.SelectedIndex)
                {
                    BuRefresh_Click(sender, e);
                    appState.FileTypesSelectedIndex = cb.SelectedIndex;
                }
            }
        }

        private void laPath_TextChanged(object sender, EventArgs e)
        {
        }

        private void laPath_Leave(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = laPath.Text;
        }
    }
}
