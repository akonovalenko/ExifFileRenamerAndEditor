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
            folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            laImagesCount = new System.Windows.Forms.ToolStripStatusLabel();
            pbTotal = new System.Windows.Forms.ToolStripProgressBar();
            buCancelProcess = new System.Windows.Forms.ToolStripDropDownButton();
            stlaState = new System.Windows.Forms.ToolStripStatusLabel();
            fontDialog1 = new System.Windows.Forms.FontDialog();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            browseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            previewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            undoRenameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparatorExif = new System.Windows.Forms.ToolStripSeparator();
            editExifToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            shiftDatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            addExifToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            removeExifToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            aboutProgramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            panelControls = new System.Windows.Forms.Panel();
            gbActions = new System.Windows.Forms.GroupBox();
            chSkipUnprocessed = new System.Windows.Forms.CheckBox();
            buRename = new System.Windows.Forms.Button();
            buAnalyze = new System.Windows.Forms.Button();
            edFileNameTemplateSample = new System.Windows.Forms.TextBox();
            laExample = new System.Windows.Forms.Label();
            gbFileNamePattern = new System.Windows.Forms.GroupBox();
            chUseSoftwareName = new System.Windows.Forms.CheckBox();
            chImageFilePrefix = new System.Windows.Forms.CheckBox();
            chUseImageSize = new System.Windows.Forms.CheckBox();
            chImageOrientation = new System.Windows.Forms.CheckBox();
            chUseCameraName = new System.Windows.Forms.CheckBox();
            chSuffixDelimiter = new System.Windows.Forms.CheckBox();
            chCounter = new System.Windows.Forms.CheckBox();
            chDateTimePrefix = new System.Windows.Forms.CheckBox();
            edCounterSuffix = new System.Windows.Forms.TextBox();
            edFilePrefix = new System.Windows.Forms.TextBox();
            cbDateTimeStampFormat = new System.Windows.Forms.ComboBox();
            edSuffixDelimiter = new System.Windows.Forms.TextBox();
            gbImagesDirectory = new System.Windows.Forms.GroupBox();
            chIncludeSubdirectories = new System.Windows.Forms.CheckBox();
            label1 = new System.Windows.Forms.Label();
            cbFileTypes = new System.Windows.Forms.ComboBox();
            laPath = new System.Windows.Forms.TextBox();
            buOpenPath = new System.Windows.Forms.Button();
            buRefresh = new System.Windows.Forms.Button();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            gbFileslist = new System.Windows.Forms.GroupBox();
            dataGridViewFiles = new System.Windows.Forms.DataGridView();
            panelSelectButtons = new System.Windows.Forms.Panel();
            buSelectAll = new System.Windows.Forms.Button();
            buDeselectAll = new System.Windows.Forms.Button();
            gbViewImage = new System.Windows.Forms.GroupBox();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            lbFileInfo = new System.Windows.Forms.ListBox();
            paExifButtons = new System.Windows.Forms.TableLayoutPanel();
            buEditExif = new System.Windows.Forms.Button();
            buAddExif = new System.Windows.Forms.Button();
            buDeleteExif = new System.Windows.Forms.Button();
            statusStrip1.SuspendLayout();
            menuStrip1.SuspendLayout();
            panelControls.SuspendLayout();
            gbActions.SuspendLayout();
            gbFileNamePattern.SuspendLayout();
            gbImagesDirectory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            gbFileslist.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewFiles).BeginInit();
            panelSelectButtons.SuspendLayout();
            gbViewImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            paExifButtons.SuspendLayout();
            SuspendLayout();
            // 
            // folderBrowserDialog1
            // 
            folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabel1, laImagesCount, pbTotal, buCancelProcess, stlaState });
            statusStrip1.Location = new System.Drawing.Point(0, 684);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            statusStrip1.Size = new System.Drawing.Size(864, 30);
            statusStrip1.TabIndex = 10;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new System.Drawing.Size(98, 24);
            toolStripStatusLabel1.Text = "Images count";
            // 
            // laImagesCount
            // 
            laImagesCount.Name = "laImagesCount";
            laImagesCount.Size = new System.Drawing.Size(17, 24);
            laImagesCount.Text = "0";
            // 
            // pbTotal
            // 
            pbTotal.Name = "pbTotal";
            pbTotal.Size = new System.Drawing.Size(400, 22);
            pbTotal.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // buCancelProcess
            // 
            buCancelProcess.AutoSize = false;
            buCancelProcess.Image = (System.Drawing.Image)resources.GetObject("buCancelProcess.Image");
            buCancelProcess.ImageTransparentColor = System.Drawing.Color.Magenta;
            buCancelProcess.Name = "buCancelProcess";
            buCancelProcess.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            buCancelProcess.ShowDropDownArrow = false;
            buCancelProcess.Size = new System.Drawing.Size(84, 28);
            buCancelProcess.Text = "Cancel";
            buCancelProcess.ToolTipText = "Cancel current operation";
            buCancelProcess.Click += ToolStripDropDownButton1_Click;
            // 
            // stlaState
            // 
            stlaState.Name = "stlaState";
            stlaState.Size = new System.Drawing.Size(0, 24);
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new System.Drawing.Size(864, 28);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { browseToolStripMenuItem, closeToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // browseToolStripMenuItem
            // 
            browseToolStripMenuItem.Name = "browseToolStripMenuItem";
            browseToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O;
            browseToolStripMenuItem.Size = new System.Drawing.Size(193, 26);
            browseToolStripMenuItem.Text = "Browse";
            browseToolStripMenuItem.Click += BuOpenPath_Click;
            // 
            // closeToolStripMenuItem
            // 
            closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            closeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4;
            closeToolStripMenuItem.Size = new System.Drawing.Size(193, 26);
            closeToolStripMenuItem.Text = "Close";
            closeToolStripMenuItem.Click += CloseToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { refreshToolStripMenuItem, previewToolStripMenuItem, renameToolStripMenuItem, undoRenameToolStripMenuItem, toolStripSeparatorExif, editExifToolStripMenuItem, shiftDatesToolStripMenuItem, addExifToolStripMenuItem, removeExifToolStripMenuItem });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new System.Drawing.Size(49, 24);
            editToolStripMenuItem.Text = "Edit";
            // 
            // refreshToolStripMenuItem
            // 
            refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            refreshToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R;
            refreshToolStripMenuItem.Size = new System.Drawing.Size(267, 26);
            refreshToolStripMenuItem.Text = "Refresh";
            refreshToolStripMenuItem.Click += BuRefresh_Click;
            // 
            // previewToolStripMenuItem
            // 
            previewToolStripMenuItem.Name = "previewToolStripMenuItem";
            previewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            previewToolStripMenuItem.Size = new System.Drawing.Size(267, 26);
            previewToolStripMenuItem.Text = "Preview";
            previewToolStripMenuItem.Click += BuAnalyze_Click;
            // 
            // renameToolStripMenuItem
            // 
            renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            renameToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F9;
            renameToolStripMenuItem.Size = new System.Drawing.Size(267, 26);
            renameToolStripMenuItem.Text = "Rename";
            renameToolStripMenuItem.Click += BuRename_Click;
            // 
            // undoRenameToolStripMenuItem
            // 
            undoRenameToolStripMenuItem.Enabled = false;
            undoRenameToolStripMenuItem.Name = "undoRenameToolStripMenuItem";
            undoRenameToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z;
            undoRenameToolStripMenuItem.Size = new System.Drawing.Size(267, 26);
            undoRenameToolStripMenuItem.Text = "Undo Last Rename";
            undoRenameToolStripMenuItem.Click += UndoRenameToolStripMenuItem_Click;
            // 
            // toolStripSeparatorExif
            // 
            toolStripSeparatorExif.Name = "toolStripSeparatorExif";
            toolStripSeparatorExif.Size = new System.Drawing.Size(264, 6);
            // 
            // editExifToolStripMenuItem
            // 
            editExifToolStripMenuItem.Enabled = false;
            editExifToolStripMenuItem.Name = "editExifToolStripMenuItem";
            editExifToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E;
            editExifToolStripMenuItem.Size = new System.Drawing.Size(267, 26);
            editExifToolStripMenuItem.Text = "Edit EXIF...";
            editExifToolStripMenuItem.Click += BuEditExif_Click;
            // 
            // shiftDatesToolStripMenuItem
            // 
            shiftDatesToolStripMenuItem.Name = "shiftDatesToolStripMenuItem";
            shiftDatesToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T;
            shiftDatesToolStripMenuItem.Size = new System.Drawing.Size(267, 26);
            shiftDatesToolStripMenuItem.Text = "Shift Dates...";
            shiftDatesToolStripMenuItem.Click += ShiftDatesToolStripMenuItem_Click;
            // 
            // addExifToolStripMenuItem
            // 
            addExifToolStripMenuItem.Name = "addExifToolStripMenuItem";
            addExifToolStripMenuItem.Size = new System.Drawing.Size(267, 26);
            addExifToolStripMenuItem.Text = "Add EXIF to Files...";
            addExifToolStripMenuItem.Click += AddExifToolStripMenuItem_Click;
            // 
            // removeExifToolStripMenuItem
            // 
            removeExifToolStripMenuItem.Name = "removeExifToolStripMenuItem";
            removeExifToolStripMenuItem.Size = new System.Drawing.Size(267, 26);
            removeExifToolStripMenuItem.Text = "Remove EXIF...";
            removeExifToolStripMenuItem.Click += RemoveExifToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { aboutToolStripMenuItem, aboutProgramToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            aboutToolStripMenuItem.Size = new System.Drawing.Size(157, 26);
            aboutToolStripMenuItem.Text = "Help...";
            aboutToolStripMenuItem.Click += HelpToolStripMenuItem_Click;
            // 
            // aboutProgramToolStripMenuItem
            // 
            aboutProgramToolStripMenuItem.Name = "aboutProgramToolStripMenuItem";
            aboutProgramToolStripMenuItem.Size = new System.Drawing.Size(157, 26);
            aboutProgramToolStripMenuItem.Text = "About...";
            aboutProgramToolStripMenuItem.Click += AboutProgramToolStripMenuItem_Click;
            // 
            // panelControls
            // 
            panelControls.AutoSize = true;
            panelControls.Controls.Add(gbActions);
            panelControls.Controls.Add(gbFileNamePattern);
            panelControls.Controls.Add(gbImagesDirectory);
            panelControls.Dock = System.Windows.Forms.DockStyle.Top;
            panelControls.Location = new System.Drawing.Point(0, 28);
            panelControls.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            panelControls.Name = "panelControls";
            panelControls.Size = new System.Drawing.Size(864, 274);
            panelControls.TabIndex = 54;
            // 
            // gbActions
            // 
            gbActions.Controls.Add(chSkipUnprocessed);
            gbActions.Controls.Add(buRename);
            gbActions.Controls.Add(buAnalyze);
            gbActions.Controls.Add(edFileNameTemplateSample);
            gbActions.Controls.Add(laExample);
            gbActions.Dock = System.Windows.Forms.DockStyle.Top;
            gbActions.Location = new System.Drawing.Point(0, 206);
            gbActions.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            gbActions.Name = "gbActions";
            gbActions.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            gbActions.Size = new System.Drawing.Size(864, 68);
            gbActions.TabIndex = 54;
            gbActions.TabStop = false;
            gbActions.Text = "Step 3 - Preview result and go rename";
            // 
            // chSkipUnprocessed
            // 
            chSkipUnprocessed.AutoSize = true;
            chSkipUnprocessed.Checked = true;
            chSkipUnprocessed.CheckState = System.Windows.Forms.CheckState.Checked;
            chSkipUnprocessed.Location = new System.Drawing.Point(712, 29);
            chSkipUnprocessed.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            chSkipUnprocessed.Name = "chSkipUnprocessed";
            chSkipUnprocessed.Size = new System.Drawing.Size(146, 24);
            chSkipUnprocessed.TabIndex = 54;
            chSkipUnprocessed.Text = "Skip unprocessed";
            chSkipUnprocessed.UseVisualStyleBackColor = true;
            // 
            // buRename
            // 
            buRename.Location = new System.Drawing.Point(621, 22);
            buRename.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            buRename.Name = "buRename";
            buRename.Size = new System.Drawing.Size(85, 40);
            buRename.TabIndex = 51;
            buRename.Text = "Rename";
            buRename.UseVisualStyleBackColor = true;
            buRename.Click += BuRename_Click;
            // 
            // buAnalyze
            // 
            buAnalyze.Location = new System.Drawing.Point(528, 22);
            buAnalyze.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            buAnalyze.Name = "buAnalyze";
            buAnalyze.Size = new System.Drawing.Size(85, 40);
            buAnalyze.TabIndex = 50;
            buAnalyze.Text = "Preview";
            buAnalyze.UseVisualStyleBackColor = true;
            buAnalyze.Click += BuAnalyze_Click;
            // 
            // edFileNameTemplateSample
            // 
            edFileNameTemplateSample.BackColor = System.Drawing.SystemColors.Control;
            edFileNameTemplateSample.Location = new System.Drawing.Point(145, 28);
            edFileNameTemplateSample.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            edFileNameTemplateSample.Name = "edFileNameTemplateSample";
            edFileNameTemplateSample.ReadOnly = true;
            edFileNameTemplateSample.Size = new System.Drawing.Size(309, 27);
            edFileNameTemplateSample.TabIndex = 49;
            // 
            // laExample
            // 
            laExample.AutoSize = true;
            laExample.Location = new System.Drawing.Point(13, 32);
            laExample.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            laExample.Name = "laExample";
            laExample.Size = new System.Drawing.Size(115, 20);
            laExample.TabIndex = 48;
            laExample.Text = "Result file name";
            // 
            // gbFileNamePattern
            // 
            gbFileNamePattern.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            gbFileNamePattern.Controls.Add(chUseSoftwareName);
            gbFileNamePattern.Controls.Add(chImageFilePrefix);
            gbFileNamePattern.Controls.Add(chUseImageSize);
            gbFileNamePattern.Controls.Add(chImageOrientation);
            gbFileNamePattern.Controls.Add(chUseCameraName);
            gbFileNamePattern.Controls.Add(chSuffixDelimiter);
            gbFileNamePattern.Controls.Add(chCounter);
            gbFileNamePattern.Controls.Add(chDateTimePrefix);
            gbFileNamePattern.Controls.Add(edCounterSuffix);
            gbFileNamePattern.Controls.Add(edFilePrefix);
            gbFileNamePattern.Controls.Add(cbDateTimeStampFormat);
            gbFileNamePattern.Controls.Add(edSuffixDelimiter);
            gbFileNamePattern.Dock = System.Windows.Forms.DockStyle.Top;
            gbFileNamePattern.Location = new System.Drawing.Point(0, 92);
            gbFileNamePattern.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            gbFileNamePattern.Name = "gbFileNamePattern";
            gbFileNamePattern.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            gbFileNamePattern.Size = new System.Drawing.Size(864, 114);
            gbFileNamePattern.TabIndex = 55;
            gbFileNamePattern.TabStop = false;
            gbFileNamePattern.Text = "Step 2 - adjust file name pattern settings";
            // 
            // chUseSoftwareName
            // 
            chUseSoftwareName.AutoSize = true;
            chUseSoftwareName.Location = new System.Drawing.Point(676, 68);
            chUseSoftwareName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            chUseSoftwareName.Name = "chUseSoftwareName";
            chUseSoftwareName.Size = new System.Drawing.Size(90, 24);
            chUseSoftwareName.TabIndex = 53;
            chUseSoftwareName.Text = "Software";
            chUseSoftwareName.UseVisualStyleBackColor = true;
            chUseSoftwareName.CheckedChanged += ChSoftware_CheckedChanged;
            // 
            // chImageFilePrefix
            // 
            chImageFilePrefix.AutoSize = true;
            chImageFilePrefix.Checked = true;
            chImageFilePrefix.CheckState = System.Windows.Forms.CheckState.Checked;
            chImageFilePrefix.Location = new System.Drawing.Point(16, 29);
            chImageFilePrefix.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            chImageFilePrefix.Name = "chImageFilePrefix";
            chImageFilePrefix.Size = new System.Drawing.Size(96, 24);
            chImageFilePrefix.TabIndex = 52;
            chImageFilePrefix.Text = "File prefix";
            chImageFilePrefix.UseVisualStyleBackColor = true;
            chImageFilePrefix.CheckedChanged += ChImageFilePrefix_CheckedChanged;
            // 
            // chUseImageSize
            // 
            chUseImageSize.AutoSize = true;
            chUseImageSize.Location = new System.Drawing.Point(512, 68);
            chUseImageSize.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            chUseImageSize.Name = "chUseImageSize";
            chUseImageSize.Size = new System.Drawing.Size(102, 24);
            chUseImageSize.TabIndex = 45;
            chUseImageSize.Text = "Image size";
            chUseImageSize.UseVisualStyleBackColor = true;
            chUseImageSize.CheckedChanged += ChUseImageSize_CheckedChanged;
            // 
            // chImageOrientation
            // 
            chImageOrientation.AutoSize = true;
            chImageOrientation.Location = new System.Drawing.Point(512, 29);
            chImageOrientation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            chImageOrientation.Name = "chImageOrientation";
            chImageOrientation.Size = new System.Drawing.Size(150, 24);
            chImageOrientation.TabIndex = 44;
            chImageOrientation.Text = "Image orientation";
            chImageOrientation.UseVisualStyleBackColor = true;
            chImageOrientation.CheckedChanged += ChImageOrientation_CheckedChanged;
            // 
            // chUseCameraName
            // 
            chUseCameraName.AutoSize = true;
            chUseCameraName.Location = new System.Drawing.Point(676, 29);
            chUseCameraName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            chUseCameraName.Name = "chUseCameraName";
            chUseCameraName.Size = new System.Drawing.Size(123, 24);
            chUseCameraName.TabIndex = 43;
            chUseCameraName.Text = "Camera name";
            chUseCameraName.UseVisualStyleBackColor = true;
            chUseCameraName.CheckedChanged += ChUseCameraName_CheckedChanged;
            // 
            // chSuffixDelimiter
            // 
            chSuffixDelimiter.AutoSize = true;
            chSuffixDelimiter.Checked = true;
            chSuffixDelimiter.CheckState = System.Windows.Forms.CheckState.Checked;
            chSuffixDelimiter.Location = new System.Drawing.Point(395, 29);
            chSuffixDelimiter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            chSuffixDelimiter.Name = "chSuffixDelimiter";
            chSuffixDelimiter.Size = new System.Drawing.Size(93, 24);
            chSuffixDelimiter.TabIndex = 34;
            chSuffixDelimiter.Text = "Delimiter";
            chSuffixDelimiter.UseVisualStyleBackColor = true;
            chSuffixDelimiter.CheckedChanged += ChSuffixDelimiter_CheckedChanged;
            // 
            // chCounter
            // 
            chCounter.AutoSize = true;
            chCounter.Checked = true;
            chCounter.CheckState = System.Windows.Forms.CheckState.Checked;
            chCounter.Location = new System.Drawing.Point(284, 31);
            chCounter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            chCounter.Name = "chCounter";
            chCounter.Size = new System.Drawing.Size(83, 24);
            chCounter.TabIndex = 33;
            chCounter.Text = "Counter";
            chCounter.UseVisualStyleBackColor = true;
            chCounter.CheckedChanged += ChCounter_CheckedChanged;
            // 
            // chDateTimePrefix
            // 
            chDateTimePrefix.AutoSize = true;
            chDateTimePrefix.Checked = true;
            chDateTimePrefix.CheckState = System.Windows.Forms.CheckState.Checked;
            chDateTimePrefix.Location = new System.Drawing.Point(152, 31);
            chDateTimePrefix.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            chDateTimePrefix.Name = "chDateTimePrefix";
            chDateTimePrefix.Size = new System.Drawing.Size(130, 24);
            chDateTimePrefix.TabIndex = 32;
            chDateTimePrefix.Text = "Datetime mark";
            chDateTimePrefix.UseVisualStyleBackColor = true;
            chDateTimePrefix.CheckedChanged += ChDateTimePrefix_CheckedChanged;
            // 
            // edCounterSuffix
            // 
            edCounterSuffix.Location = new System.Drawing.Point(284, 65);
            edCounterSuffix.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            edCounterSuffix.Name = "edCounterSuffix";
            edCounterSuffix.Size = new System.Drawing.Size(92, 27);
            edCounterSuffix.TabIndex = 31;
            edCounterSuffix.Text = "000";
            edCounterSuffix.TextChanged += EdCounterSuffix_TextChanged;
            // 
            // edFilePrefix
            // 
            edFilePrefix.Location = new System.Drawing.Point(16, 65);
            edFilePrefix.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            edFilePrefix.Name = "edFilePrefix";
            edFilePrefix.Size = new System.Drawing.Size(119, 27);
            edFilePrefix.TabIndex = 25;
            edFilePrefix.Text = "Image";
            edFilePrefix.TextChanged += EdFilePrefix_TextChanged;
            // 
            // cbDateTimeStampFormat
            // 
            cbDateTimeStampFormat.FormattingEnabled = true;
            cbDateTimeStampFormat.Items.AddRange(new object[] { "yyyymmdd", "yymmdd", "yyyymm", "yymm", "yyyymmdd hhmmss", "yyyymmdd hhmm", "yymmdd hhmmss", "yymmdd hhmm" });
            cbDateTimeStampFormat.Location = new System.Drawing.Point(153, 65);
            cbDateTimeStampFormat.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            cbDateTimeStampFormat.Name = "cbDateTimeStampFormat";
            cbDateTimeStampFormat.Size = new System.Drawing.Size(112, 28);
            cbDateTimeStampFormat.TabIndex = 27;
            cbDateTimeStampFormat.Text = "yyyymmdd";
            cbDateTimeStampFormat.SelectedIndexChanged += CbDateTimeStampFormat_SelectedIndexChanged;
            // 
            // edSuffixDelimiter
            // 
            edSuffixDelimiter.Location = new System.Drawing.Point(395, 65);
            edSuffixDelimiter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            edSuffixDelimiter.Name = "edSuffixDelimiter";
            edSuffixDelimiter.Size = new System.Drawing.Size(76, 27);
            edSuffixDelimiter.TabIndex = 29;
            edSuffixDelimiter.Text = "_";
            edSuffixDelimiter.TextChanged += EdSuffixDelimiter_TextChanged;
            // 
            // gbImagesDirectory
            // 
            gbImagesDirectory.Controls.Add(chIncludeSubdirectories);
            gbImagesDirectory.Controls.Add(label1);
            gbImagesDirectory.Controls.Add(cbFileTypes);
            gbImagesDirectory.Controls.Add(laPath);
            gbImagesDirectory.Controls.Add(buOpenPath);
            gbImagesDirectory.Controls.Add(buRefresh);
            gbImagesDirectory.Dock = System.Windows.Forms.DockStyle.Top;
            gbImagesDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
            gbImagesDirectory.Location = new System.Drawing.Point(0, 0);
            gbImagesDirectory.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            gbImagesDirectory.Name = "gbImagesDirectory";
            gbImagesDirectory.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            gbImagesDirectory.Size = new System.Drawing.Size(864, 92);
            gbImagesDirectory.TabIndex = 55;
            gbImagesDirectory.TabStop = false;
            gbImagesDirectory.Text = "Step 1 - choose an images directory";
            // 
            // chIncludeSubdirectories
            // 
            chIncludeSubdirectories.AutoSize = true;
            chIncludeSubdirectories.Location = new System.Drawing.Point(676, 45);
            chIncludeSubdirectories.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            chIncludeSubdirectories.Name = "chIncludeSubdirectories";
            chIncludeSubdirectories.Size = new System.Drawing.Size(168, 21);
            chIncludeSubdirectories.TabIndex = 36;
            chIncludeSubdirectories.Text = "Include subdirectories";
            chIncludeSubdirectories.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(395, 15);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(101, 17);
            label1.TabIndex = 35;
            label1.Text = "File extensions";
            // 
            // cbFileTypes
            // 
            cbFileTypes.FormattingEnabled = true;
            cbFileTypes.Items.AddRange(new object[] { "*.*", "jpg; jpeg;", "png", "tif; tiff" });
            cbFileTypes.Location = new System.Drawing.Point(395, 41);
            cbFileTypes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            cbFileTypes.Name = "cbFileTypes";
            cbFileTypes.Size = new System.Drawing.Size(111, 25);
            cbFileTypes.TabIndex = 34;
            // 
            // laPath
            // 
            laPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
            laPath.Location = new System.Drawing.Point(16, 40);
            laPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            laPath.Name = "laPath";
            laPath.Size = new System.Drawing.Size(316, 23);
            laPath.TabIndex = 30;
            laPath.Text = "<< no selection >>";
            laPath.TextChanged += laPath_TextChanged;
            laPath.Leave += laPath_Leave;
            // 
            // buOpenPath
            // 
            buOpenPath.Location = new System.Drawing.Point(336, 38);
            buOpenPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            buOpenPath.Name = "buOpenPath";
            buOpenPath.Size = new System.Drawing.Size(32, 32);
            buOpenPath.TabIndex = 29;
            buOpenPath.Text = "...";
            buOpenPath.UseVisualStyleBackColor = true;
            buOpenPath.Click += BuOpenPath_Click;
            // 
            // buRefresh
            // 
            buRefresh.AutoEllipsis = true;
            buRefresh.BackColor = System.Drawing.SystemColors.Info;
            buRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            buRefresh.Image = ExifImageRenamer.Properties.Resources.img_icons816;
            buRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buRefresh.Location = new System.Drawing.Point(515, 38);
            buRefresh.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            buRefresh.Name = "buRefresh";
            buRefresh.Size = new System.Drawing.Size(112, 32);
            buRefresh.TabIndex = 33;
            buRefresh.Text = "Refresh";
            buRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            buRefresh.UseVisualStyleBackColor = false;
            buRefresh.Click += BuRefresh_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            splitContainer1.Location = new System.Drawing.Point(0, 302);
            splitContainer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(gbFileslist);
            splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(gbViewImage);
            splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            splitContainer1.Size = new System.Drawing.Size(864, 382);
            splitContainer1.SplitterDistance = 380;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 55;
            // 
            // gbFileslist
            // 
            gbFileslist.Controls.Add(dataGridViewFiles);
            gbFileslist.Controls.Add(panelSelectButtons);
            gbFileslist.Dock = System.Windows.Forms.DockStyle.Fill;
            gbFileslist.Location = new System.Drawing.Point(0, 0);
            gbFileslist.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            gbFileslist.Name = "gbFileslist";
            gbFileslist.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            gbFileslist.Size = new System.Drawing.Size(380, 382);
            gbFileslist.TabIndex = 30;
            gbFileslist.TabStop = false;
            gbFileslist.Text = "Files List";
            // 
            // dataGridViewFiles
            // 
            dataGridViewFiles.AllowUserToAddRows = false;
            dataGridViewFiles.AllowUserToDeleteRows = false;
            dataGridViewFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridViewFiles.Location = new System.Drawing.Point(4, 67);
            dataGridViewFiles.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            dataGridViewFiles.MultiSelect = false;
            dataGridViewFiles.Name = "dataGridViewFiles";
            dataGridViewFiles.RowHeadersWidth = 51;
            dataGridViewFiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dataGridViewFiles.ShowEditingIcon = false;
            dataGridViewFiles.Size = new System.Drawing.Size(372, 310);
            dataGridViewFiles.TabIndex = 0;
            dataGridViewFiles.SelectionChanged += DataGridView1_SelectionChanged;
            // 
            // panelSelectButtons
            // 
            panelSelectButtons.Controls.Add(buSelectAll);
            panelSelectButtons.Controls.Add(buDeselectAll);
            panelSelectButtons.Dock = System.Windows.Forms.DockStyle.Top;
            panelSelectButtons.Location = new System.Drawing.Point(4, 25);
            panelSelectButtons.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            panelSelectButtons.Name = "panelSelectButtons";
            panelSelectButtons.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            panelSelectButtons.Size = new System.Drawing.Size(372, 42);
            panelSelectButtons.TabIndex = 1;
            // 
            // buSelectAll
            // 
            buSelectAll.AutoSize = true;
            buSelectAll.Location = new System.Drawing.Point(8, 4);
            buSelectAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            buSelectAll.Name = "buSelectAll";
            buSelectAll.Size = new System.Drawing.Size(98, 38);
            buSelectAll.TabIndex = 0;
            buSelectAll.Text = "Select All";
            buSelectAll.Click += BuSelectAll_Click;
            // 
            // buDeselectAll
            // 
            buDeselectAll.AutoSize = true;
            buDeselectAll.Location = new System.Drawing.Point(114, 4);
            buDeselectAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            buDeselectAll.Name = "buDeselectAll";
            buDeselectAll.Size = new System.Drawing.Size(98, 38);
            buDeselectAll.TabIndex = 1;
            buDeselectAll.Text = "Deselect All";
            buDeselectAll.Click += BuDeselectAll_Click;
            // 
            // gbViewImage
            // 
            gbViewImage.Controls.Add(splitContainer2);
            gbViewImage.Dock = System.Windows.Forms.DockStyle.Fill;
            gbViewImage.Location = new System.Drawing.Point(0, 0);
            gbViewImage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            gbViewImage.Name = "gbViewImage";
            gbViewImage.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            gbViewImage.Size = new System.Drawing.Size(479, 382);
            gbViewImage.TabIndex = 53;
            gbViewImage.TabStop = false;
            gbViewImage.Text = "Image preview";
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.Location = new System.Drawing.Point(4, 25);
            splitContainer2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(pictureBox1);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(lbFileInfo);
            splitContainer2.Panel2.Controls.Add(paExifButtons);
            splitContainer2.Size = new System.Drawing.Size(471, 352);
            splitContainer2.SplitterDistance = 212;
            splitContainer2.SplitterWidth = 6;
            splitContainer2.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBox1.Location = new System.Drawing.Point(0, 0);
            pictureBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(471, 212);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // lbFileInfo
            // 
            lbFileInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            lbFileInfo.FormattingEnabled = true;
            lbFileInfo.Location = new System.Drawing.Point(0, 0);
            lbFileInfo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            lbFileInfo.Name = "lbFileInfo";
            lbFileInfo.Size = new System.Drawing.Size(471, 92);
            lbFileInfo.TabIndex = 0;
            // 
            // paExifButtons
            // 
            paExifButtons.ColumnCount = 3;
            paExifButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            paExifButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            paExifButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            paExifButtons.Controls.Add(buEditExif, 0, 0);
            paExifButtons.Controls.Add(buAddExif, 1, 0);
            paExifButtons.Controls.Add(buDeleteExif, 2, 0);
            paExifButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            paExifButtons.Location = new System.Drawing.Point(0, 92);
            paExifButtons.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            paExifButtons.Name = "paExifButtons";
            paExifButtons.RowCount = 1;
            paExifButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            paExifButtons.Size = new System.Drawing.Size(471, 42);
            paExifButtons.TabIndex = 1;
            // 
            // buEditExif
            // 
            buEditExif.Dock = System.Windows.Forms.DockStyle.Fill;
            buEditExif.Enabled = false;
            buEditExif.Location = new System.Drawing.Point(0, 2);
            buEditExif.Margin = new System.Windows.Forms.Padding(0, 2, 3, 0);
            buEditExif.Name = "buEditExif";
            buEditExif.Size = new System.Drawing.Size(157, 40);
            buEditExif.TabIndex = 1;
            buEditExif.Text = "Edit EXIF...";
            buEditExif.UseVisualStyleBackColor = true;
            buEditExif.Click += BuEditExif_Click;
            // 
            // buAddExif
            // 
            buAddExif.Dock = System.Windows.Forms.DockStyle.Fill;
            buAddExif.Enabled = false;
            buAddExif.Location = new System.Drawing.Point(160, 2);
            buAddExif.Margin = new System.Windows.Forms.Padding(0, 2, 3, 0);
            buAddExif.Name = "buAddExif";
            buAddExif.Size = new System.Drawing.Size(152, 40);
            buAddExif.TabIndex = 2;
            buAddExif.Text = "Add EXIF...";
            buAddExif.UseVisualStyleBackColor = true;
            buAddExif.Click += AddExifToolStripMenuItem_Click;
            // 
            // buDeleteExif
            // 
            buDeleteExif.Dock = System.Windows.Forms.DockStyle.Fill;
            buDeleteExif.Enabled = false;
            buDeleteExif.Location = new System.Drawing.Point(315, 2);
            buDeleteExif.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            buDeleteExif.Name = "buDeleteExif";
            buDeleteExif.Size = new System.Drawing.Size(156, 40);
            buDeleteExif.TabIndex = 3;
            buDeleteExif.Text = "Delete EXIF...";
            buDeleteExif.UseVisualStyleBackColor = true;
            buDeleteExif.Click += RemoveExifToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(864, 714);
            Controls.Add(splitContainer1);
            Controls.Add(panelControls);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Name = "Form1";
            Text = "EXIF Image Renamer and Editor (choose a folder, adjust settings, press Preview, then Rename)";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            panelControls.ResumeLayout(false);
            gbActions.ResumeLayout(false);
            gbActions.PerformLayout();
            gbFileNamePattern.ResumeLayout(false);
            gbFileNamePattern.PerformLayout();
            gbImagesDirectory.ResumeLayout(false);
            gbImagesDirectory.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            gbFileslist.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewFiles).EndInit();
            panelSelectButtons.ResumeLayout(false);
            panelSelectButtons.PerformLayout();
            gbViewImage.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            paExifButtons.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

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
        private System.Windows.Forms.Panel panelSelectButtons;
        private System.Windows.Forms.Button buSelectAll;
        private System.Windows.Forms.Button buDeselectAll;
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

