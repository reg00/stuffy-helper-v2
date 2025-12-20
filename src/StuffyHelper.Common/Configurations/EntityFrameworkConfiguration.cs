namespace StuffyHelper.Common.Configurations;

/// <summary>
/// Entity framework configuration
/// </summary>
public class EntityFrameworkConfiguration
{
    public const string DefaultSectionName = "EntityFramework";

    /// <summary>
    /// Database provider (PosgreSQL, MSSQLServer...)
    /// </summary>
    public string DbProvider { get; set; } = string.Empty;

    /// <summary>
    /// Full connection string to database
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;
}