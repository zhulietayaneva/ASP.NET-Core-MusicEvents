using MusicEvents.Data;
using MusicEvents.Data.Models;
using MusicEvents.Models;
using MusicEvents.Models.Events;
using MusicEvents.Services.Artists;
using MusicEvents.Services.Cities;
using MusicEvents.Services.Countries;
using MusicEvents.Services.Organizers;

namespace MusicEvents.Services.Events
{
    public class EventService : IEventService
    {
        private readonly MusicEventsDbContext data;
        private readonly IOrganizerService organizers;
        private readonly ICountryService countries;
        private readonly ICityService cities;
        private readonly IArtistService artists;

        public EventService(MusicEventsDbContext data, ICountryService countries, IOrganizerService organizers, ICityService cities, IArtistService artists)
        {
            this.data = data;
            this.countries = countries;
            this.organizers = organizers;
            this.cities = cities;
            this.artists = artists;
        }
        public EventsQueryServiceModel All(string searchTerm,int countryId, int cityId,EventSorting sorting,int currentPage,int eventsPerPage,string? userId)
        {
            
                var eventsQuery = GetEvents().AsQueryable();
                
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
                     Venue = e.Venue,
                     IsOrganizer=organizers.IsEvOrganizer(e.Id,userId)
                 })
                 .ToList();

                 var totalEvents = eventsQuery.Count();

                 return new EventsQueryServiceModel { CurrentPage = currentPage, Events = events, TotalEvents = totalEvents, EventsPerPage=eventsPerPage };
            
        }
        public EventsQueryServiceModel MyEvents(string searchTerm, int countryId, int cityId, EventSorting sorting, int currentPage, int eventsPerPage, string userId)
        {
            var eventsQuery = GetEvents().Where(e => e.OrganizerId == organizers.GetOrganizerId(userId)).AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                eventsQuery =
                    eventsQuery
                    .Where(e => e.EventName.Contains(searchTerm.ToLower()));
            }

            if (eventsQuery.Any(e => e.CountryId == countryId))
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
                 Venue = e.Venue,
               
             })
             .ToList();

             var totalEvents = eventsQuery.Count();

             return  new EventsQueryServiceModel { CurrentPage = currentPage, Events = events, TotalEvents = totalEvents, EventsPerPage = eventsPerPage };
        }
        public AddEventFormModel Add()
        {
            return new AddEventFormModel
            {
                Countries = countries.GetCountries(),
                Artists = artists.GetArtists().OrderBy(a => a.ArtistName).ToList(),
                Time = DateTime.UtcNow.Date
            };
        }
        public void Add(string eventName,string venue,string? description, string ImgUrl,DateTime time, int countryId,int cityId, string userId,int artistId)
        {
            var curr = new Event
            {
                EventName = eventName,
                Venue = venue,
                Description = description,
                ImgURL = ImgUrl,
                Time = time,
                CountryId = countryId,
                CityId = cityId,
                OrganizerId = organizers.GetOrganizerId(userId),
                Artists = new List<Artist>() { artists.GetArtists().Where(a => a.Id == artistId).First() }
            };


            this.data.Events.Add(curr);
            this.data.SaveChanges();
        }
        public void Delete(int id)
        {
            var ev = GetEvents().Where(e => e.Id == id).FirstOrDefault();
            data.Remove(ev);
            data.SaveChanges();
        }
        public EventProfileModel Details(int id)
        {
            var artists = this.artists.GetArtists().Where(e => e.Events.Select(e => e.Id).Contains(id));
            var ev = GetEvents().Where(a => a.Id == id).First();
            return new EventProfileModel
            {
                EventName = ev.EventName,
                ImgURL = ev.ImgURL,
                Description = ev.Description,
                Artists = artists,
                Id = ev.Id,
                CityName = ev.City.CityName,
                CountryName = ev.Country.CountryName,
                Time = ev.Time,
                Venue = ev.Venue
            };
        }
        public AddEventFormModel Edit(int id)
        {
            var artists = this.artists.GetArtists().Where(e => e.Events.Select(e => e.Id).Contains(id)).ToList();

            var eventForm = GetEvents().Where(e => e.Id == id)
                                            .Select(e => new AddEventFormModel
                                            {
                                                EventName = e.EventName,
                                                ImgURL = e.ImgURL,
                                                CityId = e.CityId,
                                                Artists = artists,
                                                Time = e.Time,
                                                Venue = e.Venue,
                                                Description = e.Description,
                                                CountryId = e.CountryId

                                            }).FirstOrDefault();

            eventForm.Cities = cities.GetCitties().Where(c => c.CountryId == eventForm.CountryId).ToList();
            eventForm.Countries = countries.GetCountries();

            return eventForm;
        }
        public Event? Edit(int id, string evName,string ImgUrl,int cityId,DateTime time,string venue,string? description, int countryId)
        {
            var evData = GetEvents().FirstOrDefault(e=>e.Id==id);
            if (evData==null)
            {
                return null;
            }
            var artists = this.artists.GetArtists().Where(e => e.Events.Select(e => e.Id).Contains(id)).ToList();


            evData.EventName = evName;
            evData.ImgURL = ImgUrl;
            evData.CityId = cityId;
            evData.Artists = artists;
            evData.Time = time;
            evData.Venue = venue;
            evData.Description = description;
            evData.CountryId = countryId;

            this.data.SaveChanges();
            return evData;
        }
        public IEnumerable<Event> GetEvents()
        {
            return this.data.Events.ToList();
        }

    }
}
