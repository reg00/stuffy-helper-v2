namespace StuffyHelper.Core.Features.Shopping
{
    public class UpsertShoppingEntry
    {
        public DateTime ShoppingDate { get; set; }
        public Guid ParticipantId { get; set; }
        public Guid EventId { get; set; }
        public string Check { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public ShoppingEntry ToCommonEntry()
        {
            return new ShoppingEntry()
            {
                Check = Check,
                ParticipantId = ParticipantId,
                EventId = EventId,
                ShoppingDate = ShoppingDate,
                Description = Description,
                IsActive = true
            };
        }
    }
}
