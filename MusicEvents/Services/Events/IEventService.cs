using MusicEvents.Data.Models;
using MusicEvents.Models;
using MusicEvents.Models.Events;

namespace MusicEvents.Services.Events
{
    public interface IEventService
    {
        public EventsQueryServiceModel All(string searchTerm, int countryId, int cityId, EventSorting sorting, int currentPage, int eventsPerPage,string userId);

        public EventsQueryServiceModel MyEvents(string searchTerm, int countryId, int cityId, EventSorting sorting, int currentPage, int eventsPerPage, string userId);

        public AddEventFormModel Add();

        public void Add(string eventName, string venue, string? description, string ImgUrl, DateTime time, int countryId, int cityId, string userId, int artistId);

        public void Delete(int id);

        public EventProfileModel Details(int id);

        public AddEventFormModel Edit(int id);

        public Event? Edit(int id, string evName, string ImgUrl, int cityId, DateTime time, string venue, string? description, int countryId);

        public IEnumerable<Event> GetEvents();

    }
}
