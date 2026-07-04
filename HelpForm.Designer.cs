namespace ExifFileRenamer
{
    partial class HelpForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabOverview;
        private System.Windows.Forms.TabPage tabShortcuts;
        private System.Windows.Forms.TabPage tabTemplate;
        private System.Windows.Forms.TabPage tabExifEdit;
        private System.Windows.Forms.TabPage tabTips;
        private System.Windows.Forms.TabPage tabLicense;
        private System.Windows.Forms.RichTextBox rtbOverview;
        private System.Windows.Forms.RichTextBox rtbShortcuts;
        private System.Windows.Forms.RichTextBox rtbTemplate;
        private System.Windows.Forms.RichTextBox rtbExifEdit;
        private System.Windows.Forms.RichTextBox rtbTips;
        private System.Windows.Forms.RichTextBox rtbLicense;
        private System.Windows.Forms.Button btnClose;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabOverview = new System.Windows.Forms.TabPage();
            this.rtbOverview = new System.Windows.Forms.RichTextBox();
            this.tabShortcuts = new System.Windows.Forms.TabPage();
            this.rtbShortcuts = new System.Windows.Forms.RichTextBox();
            this.tabTemplate = new System.Windows.Forms.TabPage();
            this.rtbTemplate = new System.Windows.Forms.RichTextBox();
            this.tabExifEdit = new System.Windows.Forms.TabPage();
            this.rtbExifEdit = new System.Windows.Forms.RichTextBox();
            this.tabTips = new System.Windows.Forms.TabPage();
            this.rtbTips = new System.Windows.Forms.RichTextBox();
            this.tabLicense = new System.Windows.Forms.TabPage();
            this.rtbLicense = new System.Windows.Forms.RichTextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabOverview.SuspendLayout();
            this.tabShortcuts.SuspendLayout();
            this.tabTemplate.SuspendLayout();
            this.tabExifEdit.SuspendLayout();
            this.tabTips.SuspendLayout();
            this.tabLicense.SuspendLayout();
            this.SuspendLayout();
            //
            // tabControl
            //
            this.tabControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.tabControl.Controls.Add(this.tabOverview);
            this.tabControl.Controls.Add(this.tabShortcuts);
            this.tabControl.Controls.Add(this.tabTemplate);
            this.tabControl.Controls.Add(this.tabExifEdit);
            this.tabControl.Controls.Add(this.tabTips);
            this.tabControl.Controls.Add(this.tabLicense);
            this.tabControl.Location = new System.Drawing.Point(8, 8);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(576, 390);
            this.tabControl.TabIndex = 0;
            //
            // tabOverview
            //
            this.tabOverview.Controls.Add(this.rtbOverview);
            this.tabOverview.Location = new System.Drawing.Point(4, 22);
            this.tabOverview.Name = "tabOverview";
            this.tabOverview.Padding = new System.Windows.Forms.Padding(4);
            this.tabOverview.Size = new System.Drawing.Size(568, 364);
            this.tabOverview.TabIndex = 0;
            this.tabOverview.Text = "Overview";
            //
            // rtbOverview
            //
            this.rtbOverview.BackColor = System.Drawing.SystemColors.Window;
            this.rtbOverview.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbOverview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbOverview.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rtbOverview.Name = "rtbOverview";
            this.rtbOverview.ReadOnly = true;
            this.rtbOverview.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbOverview.TabIndex = 0;
            this.rtbOverview.TabStop = false;
            //
            // tabShortcuts
            //
            this.tabShortcuts.Controls.Add(this.rtbShortcuts);
            this.tabShortcuts.Location = new System.Drawing.Point(4, 22);
            this.tabShortcuts.Name = "tabShortcuts";
            this.tabShortcuts.Padding = new System.Windows.Forms.Padding(4);
            this.tabShortcuts.Size = new System.Drawing.Size(568, 364);
            this.tabShortcuts.TabIndex = 1;
            this.tabShortcuts.Text = "Keyboard Shortcuts";
            //
            // rtbShortcuts
            //
            this.rtbShortcuts.BackColor = System.Drawing.SystemColors.Window;
            this.rtbShortcuts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbShortcuts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbShortcuts.Font = new System.Drawing.Font("Consolas", 9.5F);
            this.rtbShortcuts.Name = "rtbShortcuts";
            this.rtbShortcuts.ReadOnly = true;
            this.rtbShortcuts.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbShortcuts.TabIndex = 0;
            this.rtbShortcuts.TabStop = false;
            //
            // tabTemplate
            //
            this.tabTemplate.Controls.Add(this.rtbTemplate);
            this.tabTemplate.Location = new System.Drawing.Point(4, 22);
            this.tabTemplate.Name = "tabTemplate";
            this.tabTemplate.Padding = new System.Windows.Forms.Padding(4);
            this.tabTemplate.Size = new System.Drawing.Size(568, 364);
            this.tabTemplate.TabIndex = 2;
            this.tabTemplate.Text = "Naming Template";
            //
            // rtbTemplate
            //
            this.rtbTemplate.BackColor = System.Drawing.SystemColors.Window;
            this.rtbTemplate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbTemplate.Font = new System.Drawing.Font("Consolas", 9.5F);
            this.rtbTemplate.Name = "rtbTemplate";
            this.rtbTemplate.ReadOnly = true;
            this.rtbTemplate.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbTemplate.TabIndex = 0;
            this.rtbTemplate.TabStop = false;
            //
            // tabExifEdit
            //
            this.tabExifEdit.Controls.Add(this.rtbExifEdit);
            this.tabExifEdit.Location = new System.Drawing.Point(4, 22);
            this.tabExifEdit.Name = "tabExifEdit";
            this.tabExifEdit.Padding = new System.Windows.Forms.Padding(4);
            this.tabExifEdit.Size = new System.Drawing.Size(568, 364);
            this.tabExifEdit.TabIndex = 4;
            this.tabExifEdit.Text = "EXIF Editing";
            //
            // rtbExifEdit
            //
            this.rtbExifEdit.BackColor = System.Drawing.SystemColors.Window;
            this.rtbExifEdit.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbExifEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbExifEdit.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rtbExifEdit.Name = "rtbExifEdit";
            this.rtbExifEdit.ReadOnly = true;
            this.rtbExifEdit.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbExifEdit.TabIndex = 0;
            this.rtbExifEdit.TabStop = false;
            //
            // tabTips
            //
            this.tabTips.Controls.Add(this.rtbTips);
            this.tabTips.Location = new System.Drawing.Point(4, 22);
            this.tabTips.Name = "tabTips";
            this.tabTips.Padding = new System.Windows.Forms.Padding(4);
            this.tabTips.Size = new System.Drawing.Size(568, 364);
            this.tabTips.TabIndex = 3;
            this.tabTips.Text = "Tips";
            //
            // rtbTips
            //
            this.rtbTips.BackColor = System.Drawing.SystemColors.Window;
            this.rtbTips.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbTips.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbTips.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rtbTips.Name = "rtbTips";
            this.rtbTips.ReadOnly = true;
            this.rtbTips.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbTips.TabIndex = 0;
            this.rtbTips.TabStop = false;
            //
            // tabLicense
            //
            this.tabLicense.Controls.Add(this.rtbLicense);
            this.tabLicense.Location = new System.Drawing.Point(4, 22);
            this.tabLicense.Name = "tabLicense";
            this.tabLicense.Padding = new System.Windows.Forms.Padding(4);
            this.tabLicense.Size = new System.Drawing.Size(568, 364);
            this.tabLicense.TabIndex = 5;
            this.tabLicense.Text = "License";
            //
            // rtbLicense
            //
            this.rtbLicense.BackColor = System.Drawing.SystemColors.Window;
            this.rtbLicense.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbLicense.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLicense.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rtbLicense.Name = "rtbLicense";
            this.rtbLicense.ReadOnly = true;
            this.rtbLicense.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbLicense.TabIndex = 0;
            this.rtbLicense.TabStop = false;
            //
            // btnClose
            //
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(496, 408);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(88, 28);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            //
            // HelpForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(592, 444);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 380);
            this.Name = "HelpForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Help — EXIF Image Renamer and Editor";
            this.tabControl.ResumeLayout(false);
            this.tabOverview.ResumeLayout(false);
            this.tabShortcuts.ResumeLayout(false);
            this.tabTemplate.ResumeLayout(false);
            this.tabExifEdit.ResumeLayout(false);
            this.tabTips.ResumeLayout(false);
            this.tabLicense.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
