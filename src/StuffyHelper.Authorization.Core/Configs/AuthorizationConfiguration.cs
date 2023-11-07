namespace StuffyHelper.Authorization.Core.Configs
{
    public class AuthorizationConfiguration
    {
        public const string DefaultSectionName = "Authorization";
        public string ConnectionString { get; set; } = string.Empty;

        public JWTOptions JWT { get; set; } = new();
    }

    public class JWTOptions
    {
        public string ValidAudience { get; set; } = string.Empty;
        public string ValidIssuer { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public int TokenExpireInHours { get; set; }
    }
}
