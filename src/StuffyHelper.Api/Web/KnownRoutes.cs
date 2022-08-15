namespace StuffyHelper.Api.Web
{
    public static class KnownRoutes
    {
        private const string DefaultRouteSegment = "api";

        private const string AuthRoute = $"{DefaultRouteSegment}/{AuthRouteSegment}";
        
        private const string AuthRouteSegment = "auth";
        private const string EventsSegment = "events";
        private const string ParticipantsSegment = "participants";
        private const string PurchasesSegment = "purchases";

        private const string EventIdRouteSegment = $"{{{KnownActionParameterNames.EventId}}}";
        private const string ParticipantIdRouteSegment = $"{{{KnownActionParameterNames.ParticipantId}}}";
        private const string PurchaseIdRouteSegment = $"{{{KnownActionParameterNames.PurchaseId}}}";


        public const string RolesRoute = $"{AuthRoute}/roles";
        public const string LoginRoute = $"{AuthRoute}/login";
        public const string LogoutRoute = $"{AuthRoute}/logout";
        public const string IsAdminRoute = $"{AuthRoute}/is-admin";
        public const string AccountRoute = $"{AuthRoute}/account";
        public const string UserLoginsRoute = $"{AuthRoute}/users";

        public const string AddEventRoute = $"{DefaultRouteSegment}/{EventsSegment}";
        public const string GetEventRoute = $"{AddEventRoute}/{EventIdRouteSegment}";
        public const string GetEventsRoute = AddEventRoute;
        public const string DeleteEventRoute = GetEventRoute;
        public const string UpdateEventRoute = GetEventRoute;

        public const string AddParticipantRoute = $"{GetEventRoute}/{ParticipantsSegment}";
        public const string GetParticipantRoute = $"{AddParticipantRoute}/{ParticipantIdRouteSegment}";
        public const string GetParticipantsRoute = AddParticipantRoute;
        public const string DeleteParticipantRoute = GetParticipantRoute;
        public const string UpdateParticipantRoute = GetParticipantRoute;

        public const string AddPurchaseRoute = $"{GetEventRoute}/{PurchasesSegment}";
        public const string GetPurchaseRoute = $"{AddPurchaseRoute}/{PurchaseIdRouteSegment}";
        public const string GetPurchasesRoute = AddPurchaseRoute;
        public const string DeletePurchaseRoute = GetPurchaseRoute;
        public const string UpdatePurchaseRoute = GetPurchaseRoute;
    }
}
