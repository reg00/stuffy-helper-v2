namespace StuffyHelper.Api.Web
{
    public static class KnownRoutes
    {
        private const string DefaultRouteSegment = "api";
        private const string AuthRouteSegment = "auth";

        private const string AuthRoute = $"{DefaultRouteSegment}/{AuthRouteSegment}";

        public const string RolesRoute = $"{AuthRoute}/roles";
        public const string LoginRoute = $"{AuthRoute}/login";
        public const string LogoutRoute = $"{AuthRoute}/logout";
        public const string IsAdminRoute = $"{AuthRoute}/is-admin";
        public const string AccountRoute = $"{AuthRoute}/account";
        public const string UserLoginsRoute = $"{AuthRoute}/users";
    }
}
