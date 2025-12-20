using Microsoft.Extensions.Configuration;

namespace StuffyHelper.Tests.Common;

public interface IConfigurationHelper
{
    T Get<T>(IConfigurationSection section);
}

public class ConfigurationHelper : IConfigurationHelper
{
    public T Get<T>(IConfigurationSection section) => section.Get<T>();
}