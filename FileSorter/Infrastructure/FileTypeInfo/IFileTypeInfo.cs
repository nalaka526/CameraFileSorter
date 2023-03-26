namespace ImageFileSorter.Infrastructure.FileTypeInfo
{
    internal interface IFileTypeInfo
    {
        public string FileTypeName { get; }

        public string FileExtentions { get; }

        public DateTime GetFileCreatedDateTime(IEnumerable<MetadataExtractor.Directory> directories);
    }
}
