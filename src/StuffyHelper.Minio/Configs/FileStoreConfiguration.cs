namespace StuffyHelper.Minio.Configs
{
    internal sealed class FileStoreConfiguration
    {
        public const string DefaultSection = "FileStore" ;

        /// <summary>
        /// Folder to store files
        /// </summary>
        public string ContainerName { get; set; } = string.Empty;
    }
}
