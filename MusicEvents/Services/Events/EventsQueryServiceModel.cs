using MusicEvents.Data.Models;

namespace MusicEvents.Services.Events
{
    public class EventsQueryServiceModel
    {
        public int CurrentPage { get; set; }
        public int TotalEvents { get; set; }
        public int EventsPerPage { get; set; }

        public ICollection<EventServiceModel> Events { get; set; }

    }
}
