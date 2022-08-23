namespace ImageFileSorter.Infrastructure
{
    internal static class LogHelper
    {
        #region Session
        public static string GetSessionStartMessage()
            => "Sorting started";

        public static string GetSourceFolderPathMessage(string sourceFolderPath)
            => $"Source folder : {sourceFolderPath}";

        public static string GetSTargetFolderPathMessage(string targetFolderPath)
            => $"Target folder : {targetFolderPath}";
        
        public static string GetSessionCancelMessage()
            => "Error occured";

        public static string GetSessionErrorMessage()
            => "Sorting canceled";

        public static string GetSessionSucessMessage()
            => "Sorting completed";

        #endregion

        #region Validations

        public static string GetValidationEmptyFolderPathsMessage()
            => "Empty folder locations";

        public static string GetValidationInvalidFolderPathsMessage()
            => "Invalid Source & Target folder";

        public static string GetValidationInvalidSourceFolderPathMessage(string sourceFolderPath)
            => $"'{sourceFolderPath}' is not a valid directory.";

        #endregion

        #region File

        public static string GetFileProcessingStartMessage(int fileIndex, string fileName)
            => $"{DateTime.Now:HH:mm:ss:fff} : File {fileIndex} : {fileName}";

        public static string GetFileProcessingFailMessage(int fileIndex)
            => $"{DateTime.Now:HH:mm:ss:fff} : File {fileIndex} : Sorting failed";

        public static string GetFileProcessingSucessMessage(int fileIndex, string destinationFolder)
            => $"{DateTime.Now:HH:mm:ss:fff} : File {fileIndex} : Copied to '{destinationFolder}'";

        public static string GetFileProcessingErrorMessage(int fileIndex)
            => $"{DateTime.Now:HH:mm:ss:fff} : File {fileIndex} : Error occured while sorting file";

        #endregion

        #region Other

        public static string GetSeperaotor()
            => "-----------------------------------------------------------------------------------------------";

        #endregion


    }
}
