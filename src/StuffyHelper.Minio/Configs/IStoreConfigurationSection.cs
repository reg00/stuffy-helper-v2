namespace StuffyHelper.Minio.Configs
{
    public interface IStoreConfigurationSection
    {
        string ConfigurationSectionName { get; }

        string BucketConfigurationName { get; }
    }
}
