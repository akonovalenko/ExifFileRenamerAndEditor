using System;
using System.Windows.Forms;

namespace ExifFileRenamer
{
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
            PopulateContent();
        }

        private void PopulateContent()
        {
            rtbOverview.Text =
"EXIF Image Renamer and Editor\r\n" +
"=============================\r\n\r\n" +
"Renames photos by their EXIF metadata and edits EXIF fields — losslessly\r\n" +
"for JPEG. Renaming is a 3-step workflow:\r\n\r\n" +
"Step 1 — Choose a folder\r\n" +
"  Click the \"...\" button next to the path field, or use File > Browse (Ctrl+O).\r\n" +
"  Enable \"Include subdirectories\" to scan all nested folders recursively.\r\n" +
"  Choose a file type filter: all files (*.*), JPG, PNG, or TIFF.\r\n\r\n" +
"Step 2 — Adjust the naming pattern\r\n" +
"  Toggle the components you want included in the file name:\r\n\r\n" +
"    File prefix         Custom text prepended to every name (e.g. \"Vacation_\").\r\n" +
"    Datetime mark       EXIF DateTimeOriginal in the chosen format.\r\n" +
"    Counter             Sequential number; padding follows the Counter field\r\n" +
"                        length (e.g. \"000\" → 001, 002, …).\r\n" +
"    Delimiter           Character inserted between every active component.\r\n" +
"    Camera name         {Make}-{Model} read from EXIF tags.\r\n" +
"    Software            Editing software name from EXIF.\r\n" +
"    Image orientation   EXIF orientation value (1–8).\r\n" +
"    Image size          Pixel dimensions (e.g. 4000x3000).\r\n\r\n" +
"  The \"Result file name\" field updates instantly as you adjust settings.\r\n\r\n" +
"Step 3 — Preview, then Rename\r\n" +
"  Click Preview (F5) to see the proposed new name for every file in the list.\r\n" +
"  Check the Status column: EXIF / No EXIF / No changes / error message.\r\n" +
"  Enable \"Skip unprocessed\" to exclude files that could not be read.\r\n" +
"  Click Rename (F9) to apply all changes to disk.\r\n\r\n" +
"Extra — View and edit EXIF\r\n" +
"  Select a file to see its full EXIF details and a thumbnail in the right\r\n" +
"  panel. Click \"Edit EXIF...\" under the details to change the date taken,\r\n" +
"  description, camera, software, artist, or copyright and save the file.\r\n" +
"  See the \"EXIF Editing\" tab for details.";

            rtbShortcuts.Text =
"Action                              Shortcut\r\n" +
"─────────────────────────────────────────────────\r\n" +
"Browse for a folder                 Ctrl+O\r\n" +
"Refresh file list                   Ctrl+R\r\n" +
"Preview proposed names              F5\r\n" +
"Rename files                        F9\r\n" +
"Undo last rename                    Ctrl+Z\r\n" +
"Edit EXIF of the selected file      Ctrl+E\r\n" +
"Shift EXIF dates (batch)            Ctrl+T\r\n" +
"Open this Help window               F1\r\n" +
"Cancel current background op.      Cancel  (status bar)\r\n" +
"Exit application                    Alt+F4\r\n";

            rtbTemplate.Text =
"Component           Description                                  Example\r\n" +
"──────────────────────────────────────────────────────────────────────────────\r\n" +
"File prefix         Fixed text at the start of the name          Vacation_\r\n" +
"Datetime mark       EXIF DateTimeOriginal; if absent the file     20240615\r\n" +
"                    is renamed without a date component\r\n" +
"Counter             Sequential number; zero-padded to the        001  0042\r\n" +
"                    digit count of the Counter field value\r\n" +
"Delimiter           Separator between every active component      _  or  -\r\n" +
"                    (can be set to empty)\r\n" +
"Camera name         {Make}-{Model} from EXIF                     Canon-EOS5D\r\n" +
"Software            Editing software from EXIF tag 305           AdobeLightroom\r\n" +
"Image orientation   EXIF orientation tag (274), values 1–8       1\r\n" +
"Image size          Pixel dimensions                             4000x3000\r\n" +
"\r\n" +
"Datetime formats:\r\n" +
"  yyyymmdd          →  20240615\r\n" +
"  yymmdd            →  240615\r\n" +
"  yyyymm            →  202406\r\n" +
"  yymm              →  2406\r\n" +
"  yyyymmdd hhmmss   →  20240615 102030\r\n" +
"  yyyymmdd hhmm     →  20240615 1020\r\n" +
"  yymmdd hhmmss     →  240615 102030\r\n" +
"  yymmdd hhmm       →  240615 1020\r\n" +
"\r\n" +
"Example with all components active:\r\n" +
"  Vacation_20240615_001_Canon-EOS5D_4000x3000.jpg\r\n" +
"  │prefix │ │date │ │ctr│ │camera     │ │size    │";

            rtbExifEdit.Text =
"EXIF Editing\r\n" +
"============\r\n\r\n" +
"Select an image in the file list and use the buttons under the details panel:\r\n" +
"  Edit EXIF...    enabled for images that CONTAIN EXIF metadata\r\n" +
"  Add EXIF...     enabled for images WITHOUT EXIF metadata\r\n" +
"  Delete EXIF...  enabled for images that CONTAIN EXIF metadata\r\n\r\n" +
"Editable fields\r\n" +
"  Date taken       written to DateTimeOriginal (36867), DateTimeDigitized\r\n" +
"                   (36868) and ModifyDate (306) — the same date the renamer\r\n" +
"                   uses for the Datetime name component\r\n" +
"  Description      ImageDescription (270)\r\n" +
"  Camera make      Make (271)\r\n" +
"  Camera model     Model (272)\r\n" +
"  Software         Software (305)\r\n" +
"  Artist           Artist (315)\r\n" +
"  Copyright        Copyright (33432)\r\n\r\n" +
"Editing rules\r\n" +
"  • Clear a field completely to REMOVE that tag from the file.\r\n" +
"  • Uncheck \"Date taken\" to leave all three date tags untouched.\r\n" +
"  • To WRITE EXIF into files that have none (scans, screenshots), use\r\n" +
"    \"Add EXIF...\" — see Batch tools below.\r\n\r\n" +
"How files are saved\r\n" +
"  • JPEG is saved LOSSLESSLY: only the metadata (APP1) segment is rewritten,\r\n" +
"    the compressed image data is copied byte for byte. No re-encoding, no\r\n" +
"    quality loss, and untouched tags (GPS, maker notes, thumbnail) survive.\r\n" +
"  • Text is stored as UTF-8, so non-Latin characters are supported.\r\n" +
"  • Saving goes through a temp file with an atomic replace — if anything\r\n" +
"    fails, the original file is left intact. File creation time is preserved.\r\n" +
"  • Non-JPEG formats (PNG, TIFF) are re-saved via the Windows imaging\r\n" +
"    pipeline (GDI+).\r\n\r\n" +
"After saving, the file list row and the details panel refresh automatically,\r\n" +
"so a changed date immediately affects the Preview (F5) result.\r\n\r\n" +
"Batch tools (Edit menu)\r\n" +
"  Shift Dates... (Ctrl+T)\r\n" +
"    Moves the date taken of many files at once by a fixed offset (days,\r\n" +
"    hours, minutes; negative values move back). Use it when the camera\r\n" +
"    clock was wrong or set to another time zone. Applies to all files\r\n" +
"    with an EXIF date or to the selected rows only.\r\n\r\n" +
"  Add EXIF to Files...\r\n" +
"    Writes EXIF into the SELECTED images that have none (scans, messenger\r\n" +
"    photos, screenshots); Ctrl+A selects all, files with EXIF are skipped.\r\n" +
"    The date taken can be filled from the file creation or last write\r\n" +
"    time — after that the files can be renamed by date. Optional text\r\n" +
"    tags (camera, artist, copyright) are written to the whole batch.\r\n" +
"    By default only the fields you filled are written; tick \"Write full\r\n" +
"    EXIF template\" to also add standard tags (orientation, resolution,\r\n" +
"    color space, EXIF version, software) with default values.\r\n\r\n" +
"  Remove EXIF...\r\n" +
"    Strips ALL metadata from the selected files (Ctrl+A selects all) —\r\n" +
"    useful before publishing photos. For JPEG the image data is not\r\n" +
"    re-encoded; this operation cannot be undone.";

            rtbTips.Text =
"Tips & Reminders\r\n" +
"================\r\n\r\n" +
"• Always run Preview (F5) before Rename (F9). If the result is not what you\r\n" +
"  expected, Edit > Undo Last Rename (Ctrl+Z) restores the previous names of\r\n" +
"  the last batch (until the application is closed).\r\n\r\n" +
"• Files with no EXIF date are shown with status \"No EXIF\". They are still\r\n" +
"  included in the batch and renamed; the Datetime component is simply omitted\r\n" +
"  from their new name.\r\n\r\n" +
"• Use \"Skip unprocessed\" to exclude any file that could not be loaded or read.\r\n" +
"  Useful when a folder contains a mix of supported and unsupported formats.\r\n\r\n" +
"• For a meaningful sequential counter, enable the Datetime component too.\r\n" +
"  The renamer sorts all files by their EXIF date before assigning numbers,\r\n" +
"  so the counter reliably reflects chronological order.\r\n\r\n" +
"• Camera name and software values are written verbatim from EXIF — they may\r\n" +
"  contain spaces, slashes, or brand-specific formatting.\r\n\r\n" +
"• Set the Delimiter field to an empty string if you want components joined\r\n" +
"  without any separator character.\r\n\r\n" +
"• If a rename fails (e.g. the target file already exists), the Status column\r\n" +
"  shows the error for that file only. The rest of the batch continues\r\n" +
"  unaffected.\r\n\r\n" +
"• The last browsed folder and window position are saved automatically\r\n" +
"  and restored on the next launch.\r\n\r\n" +
"• A photo has no EXIF date or a wrong one? Fix it via \"Edit EXIF...\" before\r\n" +
"  renaming — the corrected date is picked up by the next Preview (F5).\r\n\r\n" +
"• EXIF editing of JPEG files is lossless (metadata only, no re-encoding),\r\n" +
"  so it is safe to run on original photos.";

            rtbLicense.Text =
"EXIF Image Renamer and Editor — License\r\n" +
"=======================================\r\n\r\n" +
"This software is free to use for PERSONAL, EDUCATIONAL and other\r\n" +
"NON-COMMERCIAL purposes.\r\n\r\n" +
"COMMERCIAL USE IS NOT PERMITTED under this free license. \"Commercial use\"\r\n" +
"includes, but is not limited to:\r\n" +
"  • use by or within a company or other for-profit organization;\r\n" +
"  • use as part of a paid product or service;\r\n" +
"  • use that directly or indirectly generates revenue.\r\n\r\n" +
"To use this software for any commercial purpose you must obtain a separate\r\n" +
"commercial license from the author. Please get in touch first:\r\n" +
"  Author:  Alexey Konovalenko\r\n" +
"  E-mail:  aldev@ukr.net\r\n\r\n" +
"You are responsible for the files you process and for keeping backups of\r\n" +
"your originals.\r\n\r\n" +
"THE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR\r\n" +
"IMPLIED. THE AUTHOR IS NOT LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY\r\n" +
"ARISING FROM THE USE OF THE SOFTWARE.\r\n\r\n" +
"© Alexey Konovalenko. All rights reserved.";
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
