namespace StuffyHelper.Common.Configurations;

/// <summary>
/// Stuffy configuration (aggregate all configs)
/// </summary>
public class StuffyConfiguration
{
    /// <summary>
    /// Default section name
    /// </summary>
    public const string DefaultSection = "StuffyHelper";

    /// <summary>
    /// Authorization configuration
    /// </summary>
    public AuthorizationConfiguration Authorization { get; init; } = new();
    
    /// <summary>
    /// Front end configuration
    /// </summary>
    public FrontEndConfiguration Frontend { get; init; } = new();

    /// <summary>
    /// Email service configuration
    /// </summary>
    public EmailServiceConfiguration EmailService { get; init; } = new();
    
    /// <summary>
    /// Public endpoints
    /// </summary>
    public Dictionary<string, string> Endpoints { get; init; } = new();
}