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
        private const string PurchaseUsagesSegment = "purchase-usages";
        private const string ShoppingsSegment = "shoppings";
        private const string PurchaseTagsSegment = "purchase-tags";
        private const string UnitTypesSegment = "unit-types";
        private const string MediaSegment = "media";
        private const string FormFileSegment = "form-file";
        private const string PresignedUrlSegment = "presigned-url";
        private const string MetadataSegment = "metadata";
        private const string RequestSegment = "requests";
        private const string FriendsSegment = "friends";


        private const string EventIdRouteSegment = $"{{{KnownActionParameterNames.EventId}}}";
        private const string ParticipantIdRouteSegment = $"{{{KnownActionParameterNames.ParticipantId}}}";
        private const string PurchaseIdRouteSegment = $"{{{KnownActionParameterNames.PurchaseId}}}";
        private const string PurchaseUsageIdRouteSegment = $"{{{KnownActionParameterNames.PurchaseUsageId}}}";
        private const string ShoppingIdRouteSegment = $"{{{KnownActionParameterNames.ShoppingId}}}";
        private const string PurchaseTagIdRouteSegment = $"{{{KnownActionParameterNames.PurchaseTagId}}}";
        private const string UnitTypeIdRouteSegment = $"{{{KnownActionParameterNames.UnitTypeId}}}";
        private const string MediaIdRouteSegment = $"{{{KnownActionParameterNames.MediaId}}}";
        private const string RequestIdRouteSegment = $"{{{KnownActionParameterNames.RequestId}}}";


        public const string RegisterRoute = $"{AuthRoute}/register";
        public const string RolesRoute = $"{AuthRoute}/roles";
        public const string LoginRoute = $"{AuthRoute}/login";
        public const string EmailConfirmRoute = $"{AuthRoute}/email-confirm";
        public const string EditUserRoute = $"{AuthRoute}/edit";
        public const string LogoutRoute = $"{AuthRoute}/logout";
        public const string IsAdminRoute = $"{AuthRoute}/is-admin";
        public const string AccountRoute = $"{AuthRoute}/account";
        public const string UserLoginsRoute = $"{AuthRoute}/users";
        public const string ResetPasswordRoute = $"{AuthRoute}/reset-password";
        public const string ResetPasswordConfirmRoute = $"{AuthRoute}/reset-password-confirm";

        public const string AddEventRoute = $"{DefaultRouteSegment}/{EventsSegment}";
        public const string GetEventRoute = $"{AddEventRoute}/{EventIdRouteSegment}";
        public const string GetEventsRoute = AddEventRoute;
        public const string DeleteEventRoute = GetEventRoute;
        public const string UpdateEventRoute = GetEventRoute;
        public const string UpdateEventPrimalMediaRoute = $"{GetEventRoute}/photo";
        public const string DeleteEventPrimalMediaRoute = $"{GetEventRoute}/photo";
        public const string CompleteEventRoute = $"{GetEventRoute}/complete";
        public const string ReopenEventRoute = $"{GetEventRoute}/reopen";

        public const string AddParticipantRoute = $"{DefaultRouteSegment}/{ParticipantsSegment}";
        public const string GetParticipantRoute = $"{AddParticipantRoute}/{ParticipantIdRouteSegment}";
        public const string GetParticipantsRoute = AddParticipantRoute;
        public const string DeleteParticipantRoute = GetParticipantRoute;
        public const string UpdateParticipantRoute = GetParticipantRoute;

        public const string AddPurchaseRoute = $"{DefaultRouteSegment}/{PurchasesSegment}";
        public const string GetPurchaseRoute = $"{AddPurchaseRoute}/{PurchaseIdRouteSegment}";
        public const string GetPurchasesRoute = AddPurchaseRoute;
        public const string DeletePurchaseRoute = GetPurchaseRoute;
        public const string UpdatePurchaseRoute = GetPurchaseRoute;

        public const string AddPurchaseUsageRoute = $"{DefaultRouteSegment}/{PurchaseUsagesSegment}";
        public const string GetPurchaseUsageRoute = $"{AddPurchaseUsageRoute}/{PurchaseUsageIdRouteSegment}";
        public const string GetPurchaseUsagesRoute = AddPurchaseUsageRoute;
        public const string DeletePurchaseUsageRoute = GetPurchaseUsageRoute;
        public const string UpdatePurchaseUsageRoute = GetPurchaseUsageRoute;

        public const string AddShoppingRoute = $"{DefaultRouteSegment}/{ShoppingsSegment}";
        public const string GetShoppingRoute = $"{AddShoppingRoute}/{ShoppingIdRouteSegment}";
        public const string GetShoppingsRoute = AddShoppingRoute;
        public const string DeleteShoppingRoute = GetShoppingRoute;
        public const string UpdateShoppingRoute = GetShoppingRoute;

        public const string AddPurchaseTagRoute = $"{DefaultRouteSegment}/{PurchaseTagsSegment}";
        public const string GetPurchaseTagRoute = $"{AddPurchaseTagRoute}/{PurchaseTagIdRouteSegment}";
        public const string GetPurchaseTagsRoute = AddPurchaseTagRoute;
        public const string DeletePurchaseTagRoute = GetPurchaseTagRoute;
        public const string UpdatePurchaseTagRoute = GetPurchaseTagRoute;

        public const string AddUnitTypeRoute = $"{DefaultRouteSegment}/{UnitTypesSegment}";
        public const string GetUnitTypeRoute = $"{AddUnitTypeRoute}/{UnitTypeIdRouteSegment}";
        public const string GetUnitTypesRoute = AddUnitTypeRoute;
        public const string DeleteUnitTypeRoute = GetUnitTypeRoute;
        public const string UpdateUnitTypeRoute = GetUnitTypeRoute;

        private const string GetMediaRouteSegment = $"{DefaultRouteSegment}/{MediaSegment}/{MediaIdRouteSegment}";
        public const string RetrieveMediaFromFileRoute = $"{GetMediaRouteSegment}/{FormFileSegment}";
        public const string RetrieveMediaPresignedUrlRoute = $"{GetMediaRouteSegment}/{PresignedUrlSegment}";
        public const string GetMediaMetadataRoute = $"{GetMediaRouteSegment}/{MetadataSegment}";
        public const string GetMediasMetadatasRoute = $"{DefaultRouteSegment}/{MediaSegment}/{MetadataSegment}";
        public const string StoreMediaFormFileRoute = $"{DefaultRouteSegment}/{MediaSegment}/{FormFileSegment}";
        public const string StoreMediaPresignedUrlRoute = $"{DefaultRouteSegment}/{MediaSegment}/{PresignedUrlSegment}";
        public const string DeleteMediaRoute = GetMediaRouteSegment;

        private const string RequestRoute = $"{DefaultRouteSegment}/{RequestSegment}";
        public const string AddRequestRoute = RequestRoute;
        public const string AcceptRequestRoute = $"{GetRequestRoute}/accept";
        public const string GetRequestRoute = $"{RequestRoute}/{RequestIdRouteSegment}";
        public const string GetSendedRequestsRoute = $"{RequestRoute}/sended";
        public const string GetIncomingRequestsRoute = $"{RequestRoute}/incoming";
        public const string DeleteRequestRoute = GetRequestRoute;
        public const string UpdateRequestRoute = GetRequestRoute;

        public const string GetFriendsRoute = $"{DefaultRouteSegment}/{FriendsSegment}";
    }
}
