using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicEvents.Data;
using MusicEvents.Data.Models;
using MusicEvents.Infrastructure;
using MusicEvents.Models.Events;
using MusicEvents.Services.Cities;
using MusicEvents.Services.Countries;
using MusicEvents.Services.Events;
using MusicEvents.Services.Organizers;

namespace MusicEvents.Controllers
{
    public class EventsController : Controller
    {
        private readonly MusicEventsDbContext data;
        private readonly IEventService events;
        private readonly ICountryService countries;
        private readonly ICityService cities;
        private readonly IOrganizerService organizers;

        public EventsController(MusicEventsDbContext data, IEventService events, ICountryService countries, ICityService cities, IOrganizerService organizers)
        {
            this.data = data;
            this.events = events;
            this.countries = countries;
            this.cities = cities;
            this.organizers = organizers;
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!organizers.IsOrganizer(User.GetId()))
            {
                return RedirectToAction(nameof(OrganizersController.Create), "Organizers");
            }

            var res = new AddEventFormModel
            {
                Countries = countries.GetCountries(),
                Artists = data.Artists.OrderBy(a => a.ArtistName).ToList(),
                Time = DateTime.UtcNow.Date,

            };
            return View(res);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddEventFormModel model)
        {
            var userId = this.data
                             .Organizers
                             .Where(o => o.UserId == this.User.GetId())
                             .Select(o => o.Id)
                             .FirstOrDefault();

            if (!organizers.IsOrganizer(User.GetId()))
            {
                return RedirectToAction(nameof(OrganizersController.Create), "Organizers");
            }

            if (!this.cities.GetCitties().Any(c => c.Id == model.CityId) || !this.countries.GetCountries().Any(c => c.Id == model.CountryId))
            {
                this.ModelState.AddModelError(nameof(model.CountryId), "Select a valid place");

            }

            if (!ModelState.IsValid)
            {
                model.Countries = countries.GetCountries();

                model.Artists = data.Artists.ToList();

                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);

            }


            var curr = new Event
            {
                EventName = model.EventName,
                Venue = model.Venue,
                Description = model.Description,
                ImgURL = model.ImgURL,
                Time = model.Time,
                CountryId = model.CountryId,
                CityId = model.CityId,
                OrganizerId = userId,
                Artists = new List<Artist>() { data.Artists.Where(a => a.Id == model.ArtistId).First() }

            };


            this.data.Events.Add(curr);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }
       
        public IActionResult All([FromQuery] AllEventsQueryModel query)
        { 
            var userId = User.Identity.Name==null ? null : User.GetId();
            var events = this.events.All(query.SearchTerm,
                                         query.CountryId,
                                         query.CityId,
                                         query.SortingType,
                                         query.CurrentPage,
                                         AllEventsQueryModel.EventsPerPage,
                                         userId);

            var cities = this.cities.GetCitties().Where(c => c.CountryId == query.CountryId);
            query.Countries = countries.GetCountries();
            query.Cities = cities.Count() == 0 ? new List<City>() : cities;
            query.TotalEvents = events.TotalEvents;
            query.Events = events.Events;
            return View(query);
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var ev = this.data.Events.Where(e => e.Id == id).FirstOrDefault();
            data.Remove(ev);
            data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        public IActionResult Details(int id)
        {
            var artists = data.Artists.Where(e => e.Events.Select(e => e.Id).Contains(id));
            var ev = this.data.Events.Where(a => a.Id == id).First();

           
            

            var res = new EventProfileModel
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

            return View(res);

        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            if (!organizers.IsOrganizer(User.GetId()))
            {
                return RedirectToAction(nameof(OrganizersController.Create), "Organizers");
            }
            var artists = data.Artists.Where(e => e.Events.Select(e => e.Id).Contains(id)).ToList();

            var eventForm = this.data.Events.Where(e => e.Id == id)
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

            return View(eventForm);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id, AddEventFormModel e)
        {
            if (!organizers.IsOrganizer(User.GetId()))
            {
                return RedirectToAction(nameof(OrganizersController.Create), "Organizers");
            }

            var evData = this.data.Events.Find(id);

            if (evData == null)
            {
                return BadRequest();
            }
            var artists = data.Artists.Where(e => e.Events.Select(e => e.Id).Contains(id)).ToList();


            evData.EventName = e.EventName;
            evData.ImgURL = e.ImgURL;
            evData.CityId = e.CityId;
            evData.Artists = artists;
            evData.Time = e.Time;
            evData.Venue = e.Venue;
            evData.Description = e.Description;
            evData.CountryId = e.CountryId;

            this.data.SaveChanges();
            return RedirectToAction(nameof(All));


        }

        [Authorize]
        public IActionResult MyEvents([FromQuery] AllEventsQueryModel query)
        {
            var events = this.events.MyEvents(query.SearchTerm,
                                              query.CountryId,
                                              query.CityId,
                                              query.SortingType,
                                              query.CurrentPage,
                                              AllEventsQueryModel.EventsPerPage,
                                              User.GetId());

            var cities = this.cities.GetCitties().Where(c => c.CountryId == query.CountryId);
            query.Countries = countries.GetCountries();
            query.Cities = cities.Count() == 0 ? new List<City>() : cities;
            query.TotalEvents = events.TotalEvents;
            query.Events = events.Events;

            return View(query);
        }
        public IActionResult GetPartialArtists()
        {
            return PartialView("_ArtistsPartial");
        }

        public JsonResult GetCities(int CountryId)
        {
            var res = cities.GetCitties().Where(c => c.CountryId == CountryId).ToList();
            var t = Json(new SelectList(res, "Id", "CityName"));
            return t;
        }


    }
}
