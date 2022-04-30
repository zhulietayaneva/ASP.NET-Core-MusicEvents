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
            
            return View(events.Add());
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddEventFormModel model)
        {
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

            events.Add(model.EventName,
                       model.Venue,
                       model.Description,
                       model.ImgURL,
                       model.Time,
                       model.CountryId,
                       model.CityId,
                       User.GetId(),
                       model.ArtistId);

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
            events.Delete(id);
            return RedirectToAction(nameof(All));
        }

        public IActionResult Details(int id)
        {
            return View(events.Details(id));
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            if (!organizers.IsOrganizer(User.GetId()))
            {
                return RedirectToAction(nameof(OrganizersController.Create), "Organizers");
            }
            
            return View(events.Edit(id));
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id, AddEventFormModel e)
        {
            if (!organizers.IsOrganizer(User.GetId()))
            {
                return RedirectToAction(nameof(OrganizersController.Create), "Organizers");
            }

            var evData=events.Edit(id,e.EventName,e.ImgURL,e.CityId,e.Time,e.Venue,e.Description,e.CountryId);

            if (evData == null)
            {
                return BadRequest();
            }
            
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
