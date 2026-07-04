# EXIF Image Renamer and Editor

A Windows desktop application (.NET Framework 4.8) for organizing photo collections by EXIF metadata: batch-renames image files using a fully customizable naming template with live preview, and views & edits EXIF fields — JPEG editing is lossless (metadata only, no re-encoding).

![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.8-512BD4) ![Platform](https://img.shields.io/badge/platform-Windows-0078D4) ![License](https://img.shields.io/badge/license-MIT-green)

---

## What it does

Point it at a folder, build a naming template by toggling components on and off, preview the result in a grid, then rename with one click. Files that already have the target name are skipped; failed renames are flagged per-row in the grid so nothing is silently lost.

Beyond renaming, the detail panel shows the full EXIF of the selected file, and the **Edit EXIF** dialog lets you fix a wrong date taken, add a description or copyright, or attach EXIF to scanned photos that have none — with the corrected date immediately driving the rename preview.

---

## Features

- **Live preview** — the grid shows the proposed new name for every file before anything is renamed (Analyze / F5)
- **Customizable template** — combine any subset of: date/time, sequential counter, custom prefix, camera make+model, editing software, orientation, image dimensions
- **Full EXIF extraction** — reads date, camera, lens, exposure, GPS, color space, and more; displayed in a detail panel for the selected file
- **EXIF editing** — edit date taken, description, camera make/model, software, artist, and copyright of the selected file and save back to disk (Edit EXIF button under the detail panel)
- **Batch date shift** — fix a wrong camera clock or time zone: shift the date taken of all (or selected) files by days/hours/minutes in one operation (Ctrl+T)
- **Add EXIF to files that have none** — batch-write EXIF into scans, messenger photos, and screenshots; the date taken can be taken from the file creation/write time so the files become renamable by date
- **Strip EXIF** — remove all metadata from selected files before publishing; lossless for JPEG
- **Undo rename** — Ctrl+Z restores the original names of the last rename batch
- **Recursive folder scan** — optional checkbox includes all subdirectories
- **Background processing** — file loading, analysis, and renaming run on separate worker threads with a progress bar and cancellation; EXIF extraction is parallelized across all CPU cores
- **Settings persistence** — last folder, file type filter, and window state saved to the Windows Registry
- **Image preview** — thumbnail of the selected file with EXIF-correct orientation applied

---

## Naming template

Components can be freely combined and reordered:

| Component | Example output |
|---|---|
| Custom prefix | `Vacation_` |
| Date/time | `20240615`, `20240615_102030`, `yymmdd`, … |
| Sequential counter | `001`, `0042` (configurable padding) |
| Delimiter | `-`, `_`, space |
| Camera | `Canon-EOS5D`, `Apple-iPhone14Pro` |
| Software | `AdobeLightroom` |
| Orientation | `1` … `8` |
| Image size | `4000x3000` |

**Examples:**
```
20240615-001-Canon-EOS5D.jpg
Vacation_20240615_102030_0001.jpg
IMG_240615-042-4000x3000.jpg
```

---

## EXIF data read

The following EXIF fields are extracted and shown in the detail panel:

- **Date/time** — `DateTimeOriginal` (tag 36867), fallback to `ModifyDate` (tag 306)
- **Camera** — Make, Model
- **Exposure** — ExposureTime, FNumber, ISO, ExposureProgram, ExposureMode, ExposureBias, Flash, MeteringMode, WhiteBalance
- **Optics** — FocalLength, FocalLengthIn35mm
- **Image** — Width, Height, XResolution, YResolution, ColorSpace, PixelFormat, Orientation
- **Metadata** — Software, Description, Artist, Copyright, ExifVersion, SceneCaptureType
- **GPS** — Latitude, Longitude (parsed from DMS)

---

## EXIF editing

Select a file in the grid and click **Edit EXIF…** under the detail panel. Editable fields:

| Field | EXIF tags written |
|---|---|
| Date taken | `DateTimeOriginal` (36867), `DateTimeDigitized` (36868), `ModifyDate` (306) |
| Description | `ImageDescription` (270) |
| Camera make / model | `Make` (271) / `Model` (272) |
| Software | `Software` (305) |
| Artist | `Artist` (315) |
| Copyright | `Copyright` (33432) |

Notes:

- Clearing a field removes the tag from the file; unchecking **Date taken** leaves date tags untouched
- EXIF can also be **added** to JPEG files that had none
- Saving is done via a temp file with an atomic replace — the original is never lost on failure; file creation time is preserved
- **JPEG editing is lossless**: only the APP1 (EXIF) metadata segment is rewritten, compressed image data is copied byte for byte — no re-encoding, no quality loss. Untouched tags (maker notes, GPS, thumbnail) keep their original offsets and stay intact
- Text tags are stored as UTF-8 (non-Latin characters supported)
- Non-JPEG formats (PNG, TIFF) are re-saved via GDI+ property items

Batch operations in the **Edit** menu:

- **Shift Dates… (Ctrl+T)** — move the date taken of all files (or the selected ones) by a fixed offset of days/hours/minutes; the dialog shows a live example of the resulting date
- **Add EXIF to Files…** — write EXIF into the selected images that have none (Ctrl+A selects all; files with EXIF are skipped): date taken from file creation time / last write time / a fixed date, plus optional text tags (camera, artist, copyright) for the whole batch. By default only the filled fields are written; an optional "full EXIF template" checkbox also adds standard tags (orientation, resolution, color space, EXIF version, software) with default values
- **Remove EXIF…** — strip all metadata from the selected files (lossless for JPEG); asks for confirmation, cannot be undone

---

## File grid columns

| Column | Description |
|---|---|
Row background colors: **light gray** — images without EXIF metadata (candidates for **Add EXIF…**); **pale red** — files that are not images (e.g. mp4), EXIF cannot be added to them.

| Old name | Current file name on disk |
| New name | Proposed name per the template |
| DateTime | EXIF original date/time |
| Status | `EXIF` / `No EXIF` / `No changes` / error message |

---

## Keyboard shortcuts

| Key | Action |
|---|---|
| Ctrl+R | Refresh file list |
| F5 | Analyze — preview new names |
| F9 | Rename — apply changes |
| Ctrl+Z | Undo last rename |
| Ctrl+E | Edit EXIF of the selected file |
| Ctrl+T | Shift EXIF dates (batch) |

---

## Requirements

- Windows 7 / 10 / 11
- .NET Framework 4.8

---

## Architecture

```
ExifRenamer
├── Code/
│   ├── ExifInfoExtractorService   — reads EXIF PropertyItems via System.Drawing
│   ├── ExifWriterService          — saves edited EXIF (routes JPEG → lossless, other → GDI+)
│   ├── JpegExifRewriter           — lossless APP1/TIFF-IFD rewriter for JPEG files
│   ├── ImageInfoExtractorService  — reads bitmap dimensions and pixel format
│   ├── FileNameTemplateFactory    — builds the naming template (INotifyPropertyChanged)
│   ├── FileNameFactory            — applies the template to a concrete file
│   ├── ApplicationState           — singleton file list shared across workers
│   └── RegistrySettingsProvider   — persists settings to HKCU\Software\AlexK
├── Model/                         — ExifInfo, ExifEditValues, ImageInfo, ProcessingFileInfo, Settings
├── Interfaces/                    — IProcessingFileInfo, ISettingsProvider
├── ExifEditForm.cs                — modal EXIF edit dialog
├── TimeShiftForm.cs               — batch date shift dialog
├── Tests/                         — MSTest unit tests for the EXIF rewriter and writer
└── Form1.cs                       — main UI with BackgroundWorker threads
                                     (refresh, analyze, rename, EXIF batch)
```

Run the tests with `dotnet test Tests\ExifRenamer.Tests.csproj`.

EXIF reading uses .NET's built-in `System.Drawing.Image.PropertyItems`; EXIF writing for JPEG is done by a custom lossless rewriter that patches the APP1 (TIFF/IFD) segment directly — no third-party EXIF libraries required.

# How lossless JPEG editing works

`JpegExifRewriter` walks the JPEG segment chain and replaces only the APP1 (`Exif\0\0`) segment; everything from the SOS marker onward — the compressed image data — is copied byte for byte. Inside the TIFF block, existing value data is never moved: entries are patched in place when the new value fits, larger values are appended, and when a tag is added or removed only the IFD table itself is relocated to the end of the block. Absolute offsets referenced by untouched entries (maker notes, GPS IFD, thumbnail) therefore remain valid. Both byte orders (II/MM) are supported.
