using MusicEvents.Models;

namespace MusicEvents.Services.Events
{
    public interface IEventService
    {
        public EventsQueryServiceModel All(string searchTerm, int countryId, int cityId, EventSorting sorting, int currentPage, int eventsPerPage,string userId);

        public EventsQueryServiceModel MyEvents(string searchTerm, int countryId, int cityId, EventSorting sorting, int currentPage, int eventsPerPage, string userId);

    }
}
