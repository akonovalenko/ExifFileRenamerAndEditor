namespace ExifFileRenamer
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.laImagesCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.pbTotal = new System.Windows.Forms.ToolStripProgressBar();
            this.buCancelProcess = new System.Windows.Forms.ToolStripDropDownButton();
            this.stlaState = new System.Windows.Forms.ToolStripStatusLabel();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.browseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.previewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoRenameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorExif = new System.Windows.Forms.ToolStripSeparator();
            this.editExifToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shiftDatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addExifToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeExifToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutProgramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelControls = new System.Windows.Forms.Panel();
            this.gbActions = new System.Windows.Forms.GroupBox();
            this.chSkipUnprocessed = new System.Windows.Forms.CheckBox();
            this.buRename = new System.Windows.Forms.Button();
            this.buAnalyze = new System.Windows.Forms.Button();
            this.edFileNameTemplateSample = new System.Windows.Forms.TextBox();
            this.laExample = new System.Windows.Forms.Label();
            this.gbFileNamePattern = new System.Windows.Forms.GroupBox();
            this.chUseSoftwareName = new System.Windows.Forms.CheckBox();
            this.chImageFilePrefix = new System.Windows.Forms.CheckBox();
            this.chUseImageSize = new System.Windows.Forms.CheckBox();
            this.chImageOrientation = new System.Windows.Forms.CheckBox();
            this.chUseCameraName = new System.Windows.Forms.CheckBox();
            this.chSuffixDelimiter = new System.Windows.Forms.CheckBox();
            this.chCounter = new System.Windows.Forms.CheckBox();
            this.chDateTimePrefix = new System.Windows.Forms.CheckBox();
            this.edCounterSuffix = new System.Windows.Forms.TextBox();
            this.edFilePrefix = new System.Windows.Forms.TextBox();
            this.cbDateTimeStampFormat = new System.Windows.Forms.ComboBox();
            this.edSuffixDelimiter = new System.Windows.Forms.TextBox();
            this.gbImagesDirectory = new System.Windows.Forms.GroupBox();
            this.chIncludeSubdirectories = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbFileTypes = new System.Windows.Forms.ComboBox();
            this.laPath = new System.Windows.Forms.TextBox();
            this.buOpenPath = new System.Windows.Forms.Button();
            this.buRefresh = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gbFileslist = new System.Windows.Forms.GroupBox();
            this.dataGridViewFiles = new System.Windows.Forms.DataGridView();
            this.gbViewImage = new System.Windows.Forms.GroupBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lbFileInfo = new System.Windows.Forms.ListBox();
            this.paExifButtons = new System.Windows.Forms.TableLayoutPanel();
            this.buEditExif = new System.Windows.Forms.Button();
            this.buAddExif = new System.Windows.Forms.Button();
            this.buDeleteExif = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panelControls.SuspendLayout();
            this.gbActions.SuspendLayout();
            this.gbFileNamePattern.SuspendLayout();
            this.gbImagesDirectory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gbFileslist.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFiles)).BeginInit();
            this.gbViewImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.paExifButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.laImagesCount,
            this.pbTotal,
            this.buCancelProcess,
            this.stlaState});
            this.statusStrip1.Location = new System.Drawing.Point(0, 442);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(648, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(79, 17);
            this.toolStripStatusLabel1.Text = "Images count";
            // 
            // laImagesCount
            // 
            this.laImagesCount.Name = "laImagesCount";
            this.laImagesCount.Size = new System.Drawing.Size(13, 17);
            this.laImagesCount.Text = "0";
            // 
            // pbTotal
            // 
            this.pbTotal.Name = "pbTotal";
            this.pbTotal.Size = new System.Drawing.Size(300, 16);
            this.pbTotal.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // buCancelProcess
            // 
            this.buCancelProcess.AutoSize = false;
            this.buCancelProcess.Image = ((System.Drawing.Image)(resources.GetObject("buCancelProcess.Image")));
            this.buCancelProcess.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buCancelProcess.Name = "buCancelProcess";
            this.buCancelProcess.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.buCancelProcess.ShowDropDownArrow = false;
            this.buCancelProcess.Size = new System.Drawing.Size(84, 20);
            this.buCancelProcess.Text = "Cancel";
            this.buCancelProcess.ToolTipText = "Cancel current operation";
            this.buCancelProcess.Click += new System.EventHandler(this.ToolStripDropDownButton1_Click);
            // 
            // stlaState
            // 
            this.stlaState.Name = "stlaState";
            this.stlaState.Size = new System.Drawing.Size(0, 17);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(648, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.browseToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // browseToolStripMenuItem
            // 
            this.browseToolStripMenuItem.Name = "browseToolStripMenuItem";
            this.browseToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.browseToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.browseToolStripMenuItem.Text = "Browse";
            this.browseToolStripMenuItem.Click += new System.EventHandler(this.BuOpenPath_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.CloseToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem,
            this.previewToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.undoRenameToolStripMenuItem,
            this.toolStripSeparatorExif,
            this.editExifToolStripMenuItem,
            this.shiftDatesToolStripMenuItem,
            this.addExifToolStripMenuItem,
            this.removeExifToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.BuRefresh_Click);
            // 
            // previewToolStripMenuItem
            // 
            this.previewToolStripMenuItem.Name = "previewToolStripMenuItem";
            this.previewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.previewToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.previewToolStripMenuItem.Text = "Preview";
            this.previewToolStripMenuItem.Click += new System.EventHandler(this.BuAnalyze_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F9;
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.BuRename_Click);
            // 
            // undoRenameToolStripMenuItem
            // 
            this.undoRenameToolStripMenuItem.Enabled = false;
            this.undoRenameToolStripMenuItem.Name = "undoRenameToolStripMenuItem";
            this.undoRenameToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoRenameToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.undoRenameToolStripMenuItem.Text = "Undo Last Rename";
            this.undoRenameToolStripMenuItem.Click += new System.EventHandler(this.UndoRenameToolStripMenuItem_Click);
            // 
            // toolStripSeparatorExif
            // 
            this.toolStripSeparatorExif.Name = "toolStripSeparatorExif";
            this.toolStripSeparatorExif.Size = new System.Drawing.Size(211, 6);
            // 
            // editExifToolStripMenuItem
            // 
            this.editExifToolStripMenuItem.Enabled = false;
            this.editExifToolStripMenuItem.Name = "editExifToolStripMenuItem";
            this.editExifToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.editExifToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.editExifToolStripMenuItem.Text = "Edit EXIF...";
            this.editExifToolStripMenuItem.Click += new System.EventHandler(this.BuEditExif_Click);
            // 
            // shiftDatesToolStripMenuItem
            // 
            this.shiftDatesToolStripMenuItem.Name = "shiftDatesToolStripMenuItem";
            this.shiftDatesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.shiftDatesToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.shiftDatesToolStripMenuItem.Text = "Shift Dates...";
            this.shiftDatesToolStripMenuItem.Click += new System.EventHandler(this.ShiftDatesToolStripMenuItem_Click);
            // 
            // addExifToolStripMenuItem
            // 
            this.addExifToolStripMenuItem.Name = "addExifToolStripMenuItem";
            this.addExifToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.addExifToolStripMenuItem.Text = "Add EXIF to Files...";
            this.addExifToolStripMenuItem.Click += new System.EventHandler(this.AddExifToolStripMenuItem_Click);
            // 
            // removeExifToolStripMenuItem
            // 
            this.removeExifToolStripMenuItem.Name = "removeExifToolStripMenuItem";
            this.removeExifToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.removeExifToolStripMenuItem.Text = "Remove EXIF...";
            this.removeExifToolStripMenuItem.Click += new System.EventHandler(this.RemoveExifToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.aboutProgramToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.aboutToolStripMenuItem.Text = "Help...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.HelpToolStripMenuItem_Click);
            // 
            // aboutProgramToolStripMenuItem
            // 
            this.aboutProgramToolStripMenuItem.Name = "aboutProgramToolStripMenuItem";
            this.aboutProgramToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.aboutProgramToolStripMenuItem.Text = "About...";
            this.aboutProgramToolStripMenuItem.Click += new System.EventHandler(this.AboutProgramToolStripMenuItem_Click);
            // 
            // panelControls
            // 
            this.panelControls.AutoSize = true;
            this.panelControls.Controls.Add(this.gbActions);
            this.panelControls.Controls.Add(this.gbFileNamePattern);
            this.panelControls.Controls.Add(this.gbImagesDirectory);
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControls.Location = new System.Drawing.Point(0, 24);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(648, 178);
            this.panelControls.TabIndex = 54;
            // 
            // gbActions
            // 
            this.gbActions.Controls.Add(this.chSkipUnprocessed);
            this.gbActions.Controls.Add(this.buRename);
            this.gbActions.Controls.Add(this.buAnalyze);
            this.gbActions.Controls.Add(this.edFileNameTemplateSample);
            this.gbActions.Controls.Add(this.laExample);
            this.gbActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbActions.Location = new System.Drawing.Point(0, 134);
            this.gbActions.Name = "gbActions";
            this.gbActions.Size = new System.Drawing.Size(648, 44);
            this.gbActions.TabIndex = 54;
            this.gbActions.TabStop = false;
            this.gbActions.Text = "Step 3 - Preview result and go rename";
            // 
            // chSkipUnprocessed
            // 
            this.chSkipUnprocessed.AutoSize = true;
            this.chSkipUnprocessed.Checked = true;
            this.chSkipUnprocessed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chSkipUnprocessed.Location = new System.Drawing.Point(534, 19);
            this.chSkipUnprocessed.Name = "chSkipUnprocessed";
            this.chSkipUnprocessed.Size = new System.Drawing.Size(111, 17);
            this.chSkipUnprocessed.TabIndex = 54;
            this.chSkipUnprocessed.Text = "Skip unprocessed";
            this.chSkipUnprocessed.UseVisualStyleBackColor = true;
            // 
            // buRename
            // 
            this.buRename.Location = new System.Drawing.Point(466, 15);
            this.buRename.Name = "buRename";
            this.buRename.Size = new System.Drawing.Size(64, 26);
            this.buRename.TabIndex = 51;
            this.buRename.Text = "Rename";
            this.buRename.UseVisualStyleBackColor = true;
            this.buRename.Click += new System.EventHandler(this.BuRename_Click);
            // 
            // buAnalyze
            // 
            this.buAnalyze.Location = new System.Drawing.Point(396, 15);
            this.buAnalyze.Name = "buAnalyze";
            this.buAnalyze.Size = new System.Drawing.Size(64, 26);
            this.buAnalyze.TabIndex = 50;
            this.buAnalyze.Text = "Preview";
            this.buAnalyze.UseVisualStyleBackColor = true;
            this.buAnalyze.Click += new System.EventHandler(this.BuAnalyze_Click);
            // 
            // edFileNameTemplateSample
            // 
            this.edFileNameTemplateSample.BackColor = System.Drawing.SystemColors.Control;
            this.edFileNameTemplateSample.Location = new System.Drawing.Point(109, 18);
            this.edFileNameTemplateSample.Name = "edFileNameTemplateSample";
            this.edFileNameTemplateSample.ReadOnly = true;
            this.edFileNameTemplateSample.Size = new System.Drawing.Size(233, 20);
            this.edFileNameTemplateSample.TabIndex = 49;
            // 
            // laExample
            // 
            this.laExample.AutoSize = true;
            this.laExample.Location = new System.Drawing.Point(10, 21);
            this.laExample.Name = "laExample";
            this.laExample.Size = new System.Drawing.Size(82, 13);
            this.laExample.TabIndex = 48;
            this.laExample.Text = "Result file name";
            // 
            // gbFileNamePattern
            // 
            this.gbFileNamePattern.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gbFileNamePattern.Controls.Add(this.chUseSoftwareName);
            this.gbFileNamePattern.Controls.Add(this.chImageFilePrefix);
            this.gbFileNamePattern.Controls.Add(this.chUseImageSize);
            this.gbFileNamePattern.Controls.Add(this.chImageOrientation);
            this.gbFileNamePattern.Controls.Add(this.chUseCameraName);
            this.gbFileNamePattern.Controls.Add(this.chSuffixDelimiter);
            this.gbFileNamePattern.Controls.Add(this.chCounter);
            this.gbFileNamePattern.Controls.Add(this.chDateTimePrefix);
            this.gbFileNamePattern.Controls.Add(this.edCounterSuffix);
            this.gbFileNamePattern.Controls.Add(this.edFilePrefix);
            this.gbFileNamePattern.Controls.Add(this.cbDateTimeStampFormat);
            this.gbFileNamePattern.Controls.Add(this.edSuffixDelimiter);
            this.gbFileNamePattern.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbFileNamePattern.Location = new System.Drawing.Point(0, 60);
            this.gbFileNamePattern.Name = "gbFileNamePattern";
            this.gbFileNamePattern.Size = new System.Drawing.Size(648, 74);
            this.gbFileNamePattern.TabIndex = 55;
            this.gbFileNamePattern.TabStop = false;
            this.gbFileNamePattern.Text = "Step 2 - adjust file name pattern settings";
            // 
            // chUseSoftwareName
            // 
            this.chUseSoftwareName.AutoSize = true;
            this.chUseSoftwareName.Location = new System.Drawing.Point(507, 44);
            this.chUseSoftwareName.Name = "chUseSoftwareName";
            this.chUseSoftwareName.Size = new System.Drawing.Size(68, 17);
            this.chUseSoftwareName.TabIndex = 53;
            this.chUseSoftwareName.Text = "Software";
            this.chUseSoftwareName.UseVisualStyleBackColor = true;
            this.chUseSoftwareName.CheckedChanged += new System.EventHandler(this.ChSoftware_CheckedChanged);
            // 
            // chImageFilePrefix
            // 
            this.chImageFilePrefix.AutoSize = true;
            this.chImageFilePrefix.Checked = true;
            this.chImageFilePrefix.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chImageFilePrefix.Location = new System.Drawing.Point(12, 19);
            this.chImageFilePrefix.Name = "chImageFilePrefix";
            this.chImageFilePrefix.Size = new System.Drawing.Size(70, 17);
            this.chImageFilePrefix.TabIndex = 52;
            this.chImageFilePrefix.Text = "File prefix";
            this.chImageFilePrefix.UseVisualStyleBackColor = true;
            this.chImageFilePrefix.CheckedChanged += new System.EventHandler(this.ChImageFilePrefix_CheckedChanged);
            // 
            // chUseImageSize
            // 
            this.chUseImageSize.AutoSize = true;
            this.chUseImageSize.Location = new System.Drawing.Point(384, 44);
            this.chUseImageSize.Name = "chUseImageSize";
            this.chUseImageSize.Size = new System.Drawing.Size(76, 17);
            this.chUseImageSize.TabIndex = 45;
            this.chUseImageSize.Text = "Image size";
            this.chUseImageSize.UseVisualStyleBackColor = true;
            this.chUseImageSize.CheckedChanged += new System.EventHandler(this.ChUseImageSize_CheckedChanged);
            // 
            // chImageOrientation
            // 
            this.chImageOrientation.AutoSize = true;
            this.chImageOrientation.Location = new System.Drawing.Point(384, 19);
            this.chImageOrientation.Name = "chImageOrientation";
            this.chImageOrientation.Size = new System.Drawing.Size(107, 17);
            this.chImageOrientation.TabIndex = 44;
            this.chImageOrientation.Text = "Image orientation";
            this.chImageOrientation.UseVisualStyleBackColor = true;
            this.chImageOrientation.CheckedChanged += new System.EventHandler(this.ChImageOrientation_CheckedChanged);
            // 
            // chUseCameraName
            // 
            this.chUseCameraName.AutoSize = true;
            this.chUseCameraName.Location = new System.Drawing.Point(507, 19);
            this.chUseCameraName.Name = "chUseCameraName";
            this.chUseCameraName.Size = new System.Drawing.Size(91, 17);
            this.chUseCameraName.TabIndex = 43;
            this.chUseCameraName.Text = "Camera name";
            this.chUseCameraName.UseVisualStyleBackColor = true;
            this.chUseCameraName.CheckedChanged += new System.EventHandler(this.ChUseCameraName_CheckedChanged);
            // 
            // chSuffixDelimiter
            // 
            this.chSuffixDelimiter.AutoSize = true;
            this.chSuffixDelimiter.Checked = true;
            this.chSuffixDelimiter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chSuffixDelimiter.Location = new System.Drawing.Point(296, 19);
            this.chSuffixDelimiter.Name = "chSuffixDelimiter";
            this.chSuffixDelimiter.Size = new System.Drawing.Size(66, 17);
            this.chSuffixDelimiter.TabIndex = 34;
            this.chSuffixDelimiter.Text = "Delimiter";
            this.chSuffixDelimiter.UseVisualStyleBackColor = true;
            this.chSuffixDelimiter.CheckedChanged += new System.EventHandler(this.ChSuffixDelimiter_CheckedChanged);
            // 
            // chCounter
            // 
            this.chCounter.AutoSize = true;
            this.chCounter.Checked = true;
            this.chCounter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chCounter.Location = new System.Drawing.Point(213, 20);
            this.chCounter.Name = "chCounter";
            this.chCounter.Size = new System.Drawing.Size(63, 17);
            this.chCounter.TabIndex = 33;
            this.chCounter.Text = "Counter";
            this.chCounter.UseVisualStyleBackColor = true;
            this.chCounter.CheckedChanged += new System.EventHandler(this.ChCounter_CheckedChanged);
            // 
            // chDateTimePrefix
            // 
            this.chDateTimePrefix.AutoSize = true;
            this.chDateTimePrefix.Checked = true;
            this.chDateTimePrefix.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chDateTimePrefix.Location = new System.Drawing.Point(114, 20);
            this.chDateTimePrefix.Name = "chDateTimePrefix";
            this.chDateTimePrefix.Size = new System.Drawing.Size(94, 17);
            this.chDateTimePrefix.TabIndex = 32;
            this.chDateTimePrefix.Text = "Datetime mark";
            this.chDateTimePrefix.UseVisualStyleBackColor = true;
            this.chDateTimePrefix.CheckedChanged += new System.EventHandler(this.ChDateTimePrefix_CheckedChanged);
            // 
            // edCounterSuffix
            // 
            this.edCounterSuffix.Location = new System.Drawing.Point(213, 42);
            this.edCounterSuffix.Name = "edCounterSuffix";
            this.edCounterSuffix.Size = new System.Drawing.Size(70, 20);
            this.edCounterSuffix.TabIndex = 31;
            this.edCounterSuffix.Text = "000";
            this.edCounterSuffix.TextChanged += new System.EventHandler(this.EdCounterSuffix_TextChanged);
            // 
            // edFilePrefix
            // 
            this.edFilePrefix.Location = new System.Drawing.Point(12, 42);
            this.edFilePrefix.Name = "edFilePrefix";
            this.edFilePrefix.Size = new System.Drawing.Size(90, 20);
            this.edFilePrefix.TabIndex = 25;
            this.edFilePrefix.Text = "Image";
            this.edFilePrefix.TextChanged += new System.EventHandler(this.EdFilePrefix_TextChanged);
            // 
            // cbDateTimeStampFormat
            // 
            this.cbDateTimeStampFormat.FormattingEnabled = true;
            this.cbDateTimeStampFormat.Items.AddRange(new object[] {
            "yyyymmdd",
            "yymmdd",
            "yyyymm",
            "yymm",
            "yyyymmdd hhmmss",
            "yyyymmdd hhmm",
            "yymmdd hhmmss",
            "yymmdd hhmm"});
            this.cbDateTimeStampFormat.Location = new System.Drawing.Point(115, 42);
            this.cbDateTimeStampFormat.Name = "cbDateTimeStampFormat";
            this.cbDateTimeStampFormat.Size = new System.Drawing.Size(85, 21);
            this.cbDateTimeStampFormat.TabIndex = 27;
            this.cbDateTimeStampFormat.Text = "yyyymmdd";
            this.cbDateTimeStampFormat.SelectedIndexChanged += new System.EventHandler(this.CbDateTimeStampFormat_SelectedIndexChanged);
            // 
            // edSuffixDelimiter
            // 
            this.edSuffixDelimiter.Location = new System.Drawing.Point(296, 42);
            this.edSuffixDelimiter.Name = "edSuffixDelimiter";
            this.edSuffixDelimiter.Size = new System.Drawing.Size(58, 20);
            this.edSuffixDelimiter.TabIndex = 29;
            this.edSuffixDelimiter.Text = "_";
            this.edSuffixDelimiter.TextChanged += new System.EventHandler(this.EdSuffixDelimiter_TextChanged);
            // 
            // gbImagesDirectory
            // 
            this.gbImagesDirectory.Controls.Add(this.chIncludeSubdirectories);
            this.gbImagesDirectory.Controls.Add(this.label1);
            this.gbImagesDirectory.Controls.Add(this.cbFileTypes);
            this.gbImagesDirectory.Controls.Add(this.laPath);
            this.gbImagesDirectory.Controls.Add(this.buOpenPath);
            this.gbImagesDirectory.Controls.Add(this.buRefresh);
            this.gbImagesDirectory.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbImagesDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbImagesDirectory.Location = new System.Drawing.Point(0, 0);
            this.gbImagesDirectory.Name = "gbImagesDirectory";
            this.gbImagesDirectory.Padding = new System.Windows.Forms.Padding(2);
            this.gbImagesDirectory.Size = new System.Drawing.Size(648, 60);
            this.gbImagesDirectory.TabIndex = 55;
            this.gbImagesDirectory.TabStop = false;
            this.gbImagesDirectory.Text = "Step 1 - choose an images directory";
            // 
            // chIncludeSubdirectories
            // 
            this.chIncludeSubdirectories.AutoSize = true;
            this.chIncludeSubdirectories.Location = new System.Drawing.Point(507, 31);
            this.chIncludeSubdirectories.Name = "chIncludeSubdirectories";
            this.chIncludeSubdirectories.Size = new System.Drawing.Size(129, 17);
            this.chIncludeSubdirectories.TabIndex = 36;
            this.chIncludeSubdirectories.Text = "Include subdirectories";
            this.chIncludeSubdirectories.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(296, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 35;
            this.label1.Text = "File extensions";
            // 
            // cbFileTypes
            // 
            this.cbFileTypes.FormattingEnabled = true;
            this.cbFileTypes.Items.AddRange(new object[] {
            "*.*",
            "jpg; jpeg;",
            "png",
            "tif; tiff"});
            this.cbFileTypes.Location = new System.Drawing.Point(296, 27);
            this.cbFileTypes.Name = "cbFileTypes";
            this.cbFileTypes.Size = new System.Drawing.Size(84, 21);
            this.cbFileTypes.TabIndex = 34;
            // 
            // laPath
            // 
            this.laPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.laPath.Location = new System.Drawing.Point(12, 26);
            this.laPath.Name = "laPath";
            this.laPath.Size = new System.Drawing.Size(238, 20);
            this.laPath.TabIndex = 30;
            this.laPath.Text = "<< no selection >>";
            this.laPath.TextChanged += new System.EventHandler(this.laPath_TextChanged);
            this.laPath.Leave += new System.EventHandler(this.laPath_Leave);
            // 
            // buOpenPath
            // 
            this.buOpenPath.Location = new System.Drawing.Point(252, 24);
            this.buOpenPath.Name = "buOpenPath";
            this.buOpenPath.Size = new System.Drawing.Size(28, 26);
            this.buOpenPath.TabIndex = 29;
            this.buOpenPath.Text = "...";
            this.buOpenPath.UseVisualStyleBackColor = true;
            this.buOpenPath.Click += new System.EventHandler(this.BuOpenPath_Click);
            // 
            // buRefresh
            // 
            this.buRefresh.AutoEllipsis = true;
            this.buRefresh.BackColor = System.Drawing.SystemColors.Info;
            this.buRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buRefresh.Image = global::ExifImageRenamer.Properties.Resources.img_icons816;
            this.buRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buRefresh.Location = new System.Drawing.Point(386, 24);
            this.buRefresh.Name = "buRefresh";
            this.buRefresh.Size = new System.Drawing.Size(84, 26);
            this.buRefresh.TabIndex = 33;
            this.buRefresh.Text = "Refresh";
            this.buRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buRefresh.UseVisualStyleBackColor = false;
            this.buRefresh.Click += new System.EventHandler(this.BuRefresh_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 202);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gbFileslist);
            this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gbViewImage);
            this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.Size = new System.Drawing.Size(648, 240);
            this.splitContainer1.SplitterDistance = 380;
            this.splitContainer1.TabIndex = 55;
            // 
            // gbFileslist
            // 
            this.gbFileslist.Controls.Add(this.dataGridViewFiles);
            this.gbFileslist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbFileslist.Location = new System.Drawing.Point(0, 0);
            this.gbFileslist.Name = "gbFileslist";
            this.gbFileslist.Size = new System.Drawing.Size(380, 240);
            this.gbFileslist.TabIndex = 30;
            this.gbFileslist.TabStop = false;
            this.gbFileslist.Text = "Files List";
            // 
            // dataGridViewFiles
            // 
            this.dataGridViewFiles.AllowUserToAddRows = false;
            this.dataGridViewFiles.AllowUserToDeleteRows = false;
            this.dataGridViewFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewFiles.Location = new System.Drawing.Point(3, 16);
            this.dataGridViewFiles.MultiSelect = false;
            this.dataGridViewFiles.Name = "dataGridViewFiles";
            this.dataGridViewFiles.ReadOnly = true;
            this.dataGridViewFiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewFiles.ShowEditingIcon = false;
            this.dataGridViewFiles.Size = new System.Drawing.Size(374, 221);
            this.dataGridViewFiles.TabIndex = 0;
            this.dataGridViewFiles.SelectionChanged += new System.EventHandler(this.DataGridView1_SelectionChanged);
            // 
            // gbViewImage
            // 
            this.gbViewImage.Controls.Add(this.splitContainer2);
            this.gbViewImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbViewImage.Location = new System.Drawing.Point(0, 0);
            this.gbViewImage.Name = "gbViewImage";
            this.gbViewImage.Size = new System.Drawing.Size(264, 240);
            this.gbViewImage.TabIndex = 53;
            this.gbViewImage.TabStop = false;
            this.gbViewImage.Text = "Image preview";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 16);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.pictureBox1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.lbFileInfo);
            this.splitContainer2.Panel2.Controls.Add(this.paExifButtons);
            this.splitContainer2.Size = new System.Drawing.Size(258, 221);
            this.splitContainer2.SplitterDistance = 134;
            this.splitContainer2.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(258, 134);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // lbFileInfo
            // 
            this.lbFileInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbFileInfo.FormattingEnabled = true;
            this.lbFileInfo.Location = new System.Drawing.Point(0, 0);
            this.lbFileInfo.Name = "lbFileInfo";
            this.lbFileInfo.Size = new System.Drawing.Size(258, 55);
            this.lbFileInfo.TabIndex = 0;
            // 
            // paExifButtons
            // 
            this.paExifButtons.ColumnCount = 3;
            this.paExifButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.paExifButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.paExifButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.paExifButtons.Controls.Add(this.buEditExif, 0, 0);
            this.paExifButtons.Controls.Add(this.buAddExif, 1, 0);
            this.paExifButtons.Controls.Add(this.buDeleteExif, 2, 0);
            this.paExifButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.paExifButtons.Location = new System.Drawing.Point(0, 55);
            this.paExifButtons.Name = "paExifButtons";
            this.paExifButtons.RowCount = 1;
            this.paExifButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.paExifButtons.Size = new System.Drawing.Size(258, 28);
            this.paExifButtons.TabIndex = 1;
            // 
            // buEditExif
            // 
            this.buEditExif.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buEditExif.Enabled = false;
            this.buEditExif.Location = new System.Drawing.Point(0, 2);
            this.buEditExif.Margin = new System.Windows.Forms.Padding(0, 2, 2, 0);
            this.buEditExif.Name = "buEditExif";
            this.buEditExif.Size = new System.Drawing.Size(85, 26);
            this.buEditExif.TabIndex = 1;
            this.buEditExif.Text = "Edit EXIF...";
            this.buEditExif.UseVisualStyleBackColor = true;
            this.buEditExif.Click += new System.EventHandler(this.BuEditExif_Click);
            // 
            // buAddExif
            // 
            this.buAddExif.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buAddExif.Enabled = false;
            this.buAddExif.Location = new System.Drawing.Point(87, 2);
            this.buAddExif.Margin = new System.Windows.Forms.Padding(0, 2, 2, 0);
            this.buAddExif.Name = "buAddExif";
            this.buAddExif.Size = new System.Drawing.Size(83, 26);
            this.buAddExif.TabIndex = 2;
            this.buAddExif.Text = "Add EXIF...";
            this.buAddExif.UseVisualStyleBackColor = true;
            this.buAddExif.Click += new System.EventHandler(this.AddExifToolStripMenuItem_Click);
            // 
            // buDeleteExif
            // 
            this.buDeleteExif.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buDeleteExif.Enabled = false;
            this.buDeleteExif.Location = new System.Drawing.Point(172, 2);
            this.buDeleteExif.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.buDeleteExif.Name = "buDeleteExif";
            this.buDeleteExif.Size = new System.Drawing.Size(86, 26);
            this.buDeleteExif.TabIndex = 3;
            this.buDeleteExif.Text = "Delete EXIF...";
            this.buDeleteExif.UseVisualStyleBackColor = true;
            this.buDeleteExif.Click += new System.EventHandler(this.RemoveExifToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 464);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panelControls);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "Form1";
            this.Text = "EXIF Image Renamer and Editor (choose a folder, adjust settings, press Preview, t" +
    "hen Rename)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panelControls.ResumeLayout(false);
            this.gbActions.ResumeLayout(false);
            this.gbActions.PerformLayout();
            this.gbFileNamePattern.ResumeLayout(false);
            this.gbFileNamePattern.PerformLayout();
            this.gbImagesDirectory.ResumeLayout(false);
            this.gbImagesDirectory.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.gbFileslist.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFiles)).EndInit();
            this.gbViewImage.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.paExifButtons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel laImagesCount;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.Panel panelControls;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox gbFileslist;
        private System.Windows.Forms.DataGridView dataGridViewFiles;
        private System.Windows.Forms.GroupBox gbViewImage;
        private System.Windows.Forms.ToolStripProgressBar pbTotal;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripDropDownButton buCancelProcess;
        private System.Windows.Forms.GroupBox gbFileNamePattern;
        private System.Windows.Forms.CheckBox chUseImageSize;
        private System.Windows.Forms.CheckBox chImageOrientation;
        private System.Windows.Forms.CheckBox chUseCameraName;
        private System.Windows.Forms.CheckBox chSuffixDelimiter;
        private System.Windows.Forms.CheckBox chCounter;
        private System.Windows.Forms.CheckBox chDateTimePrefix; 
        private System.Windows.Forms.TextBox edCounterSuffix;
        private System.Windows.Forms.TextBox edFilePrefix;
        private System.Windows.Forms.ComboBox cbDateTimeStampFormat;
        private System.Windows.Forms.TextBox edSuffixDelimiter;
        private System.Windows.Forms.GroupBox gbActions;
        private System.Windows.Forms.GroupBox gbImagesDirectory;
        private System.Windows.Forms.TextBox laPath;
        private System.Windows.Forms.Button buOpenPath;
        private System.Windows.Forms.Button buRefresh;
        private System.Windows.Forms.ToolStripStatusLabel stlaState;
        private System.Windows.Forms.ToolStripMenuItem browseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem previewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoRenameToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorExif;
        private System.Windows.Forms.ToolStripMenuItem editExifToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shiftDatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addExifToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeExifToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutProgramToolStripMenuItem;
        private System.Windows.Forms.CheckBox chImageFilePrefix;
        private System.Windows.Forms.CheckBox chSkipUnprocessed;
        private System.Windows.Forms.Button buRename;
        private System.Windows.Forms.Button buAnalyze;
        private System.Windows.Forms.TextBox edFileNameTemplateSample;
        private System.Windows.Forms.Label laExample;
        private System.Windows.Forms.ComboBox cbFileTypes;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListBox lbFileInfo;
        private System.Windows.Forms.Button buEditExif;
        private System.Windows.Forms.Button buAddExif;
        private System.Windows.Forms.Button buDeleteExif;
        private System.Windows.Forms.TableLayoutPanel paExifButtons;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chIncludeSubdirectories;
        private System.Windows.Forms.CheckBox chUseSoftwareName;
    }
}

