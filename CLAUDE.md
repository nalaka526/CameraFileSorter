# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Run

```powershell
# Build
dotnet build "FileSorter/Image File Sorter.csproj"

# Run (Windows Forms app — requires Windows desktop)
dotnet run --project "FileSorter/Image File Sorter.csproj"

# Publish (ClickOnce)
dotnet publish "FileSorter/Image File Sorter.csproj" -c Release
```

There are no automated tests in this project.

## Architecture

This is a Windows Forms (.NET 8, `net8.0-windows7.0`) desktop app that copies camera files into date-based folder hierarchies using EXIF/metadata dates rather than filesystem dates.

**Data flow:**
1. `ImageSorterForm` (UI) — user picks source/target folders and options, fires `BackgroundWorker`
2. `Session` — carries config (paths, separator, year/month folder flags) and the `BackgroundWorker` reference; all progress reporting flows through it via `UserState` messages
3. `FileSorter.Sort()` — iterates files in the source directory, delegates date extraction to `FileReader`
4. `FileReader.GetCreatedDateTime()` — uses `MetadataExtractor` to read embedded metadata; dispatches to the correct `IFileTypeInfo` implementation based on detected file type
5. Files are **copied** (not moved) to `<target>/<year?>/<month?>/<YYYY-MM-DD>/` or to `NotHandled/` (unsupported type) / `Failed/` (no valid date)

**Adding support for a new file type:**
- Implement `IFileTypeInfo` (in `Infrastructure/FileTypeInfo/`) — provide `FileTypeName` (matches `MetadataExtractor`'s detected type string), `FileExtentions` (substring match), and `GetFileCreatedDateTime`
- Register the new class in `FileReader`'s static `fileTypeInfos` list

**Key design notes:**
- `FileTypeName` detection uses `MetadataExtractor`'s `FileTypeDirectory.TagDetectedFileTypeName` — the string must match exactly what the library returns (e.g. `"JPEG"`, `"MP4"`)
- `FileExtentions` is a single concatenated string checked with `Contains` (e.g. `".jpg.jpeg"`) — not a collection
- Folder nesting (year/month subdirectories) is optional and controlled per-session; month folder requires year folder to be enabled (`ChkYearFolder_CheckedChanged` enforces this in the UI)
- Progress messages are inserted at index 0 in the log list (newest-first display)
