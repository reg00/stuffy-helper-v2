namespace StuffyHelper.Core.Features.Event
{
    public class UpsertEventEntry
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public bool IsCompleted { get; set; }


        public EventEntry ToCommonEntry(string userId)
        {
            return new EventEntry()
            {
                Name = Name,
                Description = Description,
                CreatedDate = DateTime.UtcNow,
                UserId = userId,
                EventDate = EventDate,
                IsCompleted = IsCompleted
            };
        }
    }
}
