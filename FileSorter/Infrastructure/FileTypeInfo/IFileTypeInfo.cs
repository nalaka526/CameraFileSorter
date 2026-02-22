namespace ImageFileSorter.Infrastructure.FileTypeInfo
{
    internal interface IFileTypeInfo
    {
        public string FileTypeName { get; }

        public string FileExtensions { get; }

        public DateTime GetFileCreatedDateTime(IEnumerable<MetadataExtractor.Directory> directories);
    }
}
