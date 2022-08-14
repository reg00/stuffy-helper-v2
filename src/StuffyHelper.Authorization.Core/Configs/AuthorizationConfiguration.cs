namespace StuffyHelper.Authorization.Core.Configs
{
    public class AuthorizationConfiguration
    {
        public const string DefaultSectionName = "Authorization";
        public string ConnectionString { get; set; }

        public JWTOptions JWT { get; set; }
    }

    public class JWTOptions
    {
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string Secret { get; set; }
        public int TokenExpireInHours { get; set; }
    }
}
