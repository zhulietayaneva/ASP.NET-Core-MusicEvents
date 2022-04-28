using MusicEvents.Data;
using MusicEvents.Models;
using MusicEvents.Services.Countries;

namespace MusicEvents.Services.Events
{
    public class EventService : IEventService
    {
        private readonly MusicEventsDbContext data;
        private readonly ICountryService countries;

        public EventService(MusicEventsDbContext data, ICountryService countries)
        {
            this.data = data;
            this.countries = countries;
        }

        public EventsQueryServiceModel All(string searchTerm,int countryId, int cityId,EventSorting sorting,int currentPage,int eventsPerPage)
        {
            
                var eventsQuery = data.Events.AsQueryable();
                var countries = this.countries.GetCountries();
                

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    eventsQuery =
                        eventsQuery
                        .Where(e => e.EventName.Contains(searchTerm.ToLower()));
                }

                if (eventsQuery.Any(e=>e.CountryId==countryId))
                {
                    eventsQuery =
                        eventsQuery
                        .Where(e => e.CountryId == countryId);
                }
                else if (countryId != 0)
                {
                    return new EventsQueryServiceModel { CurrentPage = currentPage, Events = new List<EventServiceModel>(), TotalEvents = 0, EventsPerPage = eventsPerPage };
                }
                

                if (eventsQuery.Any(a => a.CityId == cityId))
                {
                    eventsQuery =
                        eventsQuery
                        .Where(e => e.CityId == cityId);
                }
                 else if (cityId != 0)
                {
                    return new EventsQueryServiceModel { CurrentPage = currentPage, Events = new List<EventServiceModel>(), TotalEvents = 0, EventsPerPage = eventsPerPage };
                }



            eventsQuery = sorting switch
                {
                    EventSorting.Date => eventsQuery.OrderBy(g => g.Time),
                    EventSorting.EventName => eventsQuery.OrderBy(g => g.EventName),
                    EventSorting.Id or _ => eventsQuery.OrderByDescending(a => a.Id)
                };

                var events = eventsQuery
                    .Skip((currentPage - 1) * eventsPerPage)
                    .Take(eventsPerPage)
                 .Select(e => new EventServiceModel
                 {
                     Id = e.Id,
                     CityName = e.City.CityName,
                     CountryName = e.Country.CountryName,
                     Artists = String.Join(", ", e.Artists.Select(a => a.ArtistName)),
                     Description = e.Description,
                     EventName = e.EventName,
                     ImgURL = e.ImgURL,
                     Time = e.Time,
                     Venue = e.Venue
                 })
                 .ToList();
            var totalEvents = eventsQuery.Count();

            return new EventsQueryServiceModel { CurrentPage = currentPage, Events = events, TotalEvents = totalEvents, EventsPerPage=eventsPerPage};
            
        }

    }
}
