namespace StuffyHelper.Common.Configurations;

/// <summary>
/// Front end configuration
/// </summary>
public class FrontEndConfiguration
{
    /// <summary>
    /// Endpoint of service
    /// </summary>
    // TODO: Перенести в Endpoints
    public Uri Endpoint { get; init; } = new Uri("about:blank");
}
