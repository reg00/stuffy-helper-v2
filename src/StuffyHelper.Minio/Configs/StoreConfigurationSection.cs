namespace StuffyHelper.Minio.Configs
{
    internal sealed class MinioBlobStoreConfigurationSection : StoreConfigurationSection
    {
        public MinioBlobStoreConfigurationSection()
            : base(Constants.BlobStoreConfigurationSection, Constants.BlobBucketConfigurationName)
        {
        }
    }

    internal class StoreConfigurationSection : IStoreConfigurationSection
    {
        internal StoreConfigurationSection(string sectionName, string name)
        {
            ConfigurationSectionName = sectionName;
            BucketConfigurationName = name;
        }

        public string BucketConfigurationName { get; }

        public string ConfigurationSectionName { get; }
    }
}
