namespace MusicEvents.Models.Api
{
    public class AllEventsApiResponseModel
    {
        public int CurrentPage { get; set; } 
        public int TotalEvents { get; set; }
        public ICollection<EventsResponseModel> Events { get; set; }
    }
}
