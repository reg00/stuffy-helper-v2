namespace StuffyHelper.Core.Features.Event
{
    public class UpsertEventEntry
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public bool IsCompleted { get; set; }
        public string UserId { get; set; }
        public bool IsActive { get; set; }


        public EventEntry ToCommonEntry()
        {
            return new EventEntry()
            {
                Name = Name,
                Description = Description,
                CreatedDate = DateTime.UtcNow,
                UserId = UserId,
                EventDate = EventDate,
                IsCompleted = IsCompleted,
                IsActive = true
            };
        }
    }
}
