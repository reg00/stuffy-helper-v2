namespace StuffyHelper.Common.Configurations;

public class StuffyConfiguration
{
    public const string DefaultSection = "StuffyHelper";

    public AuthorizationConfiguration AuthorizationConfiguration { get; set; } = new();

    public FrontEndConfiguration Frontend { get; set; } = new();
    
    public Dictionary<string, string> Endpoints { get; set; } = new();
}