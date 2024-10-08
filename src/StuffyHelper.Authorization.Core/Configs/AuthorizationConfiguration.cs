﻿namespace StuffyHelper.Authorization.Core.Configs
{
    public class AuthorizationConfiguration
    {
        public const string DefaultSectionName = "Authorization";
        public string ConnectionString { get; init; } = string.Empty;

        public JWTOptions JWT { get; init; } = new();
    }

    public class JWTOptions
    {
        public string ValidAudience { get; init; } = string.Empty;
        public string ValidIssuer { get; init; } = string.Empty;
        public string Secret { get; init; } = string.Empty;
        public int TokenExpireInHours { get; init; }
    }
}
