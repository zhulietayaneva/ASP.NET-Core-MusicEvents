using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicEvents.Data;
using MusicEvents.Data.Models;
using MusicEvents.Infrastructure;
using MusicEvents.Models.Events;
using MusicEvents.Services.Events;

namespace MusicEvents.Controllers
{
    public class EventsController : Controller
    {
        private readonly MusicEventsDbContext data;
        private readonly IEventService events;

        

        public EventsController(MusicEventsDbContext data, IEventService events)
        {
            this.data = data;
            this.events = events;
        }

        [Authorize]
        public IActionResult Add()
        {

            if (!this.UserIsOrganizer())
            {
                return RedirectToAction(nameof(OrganizersController.Create), "Organizers");
            }


    
            var res = new AddEventFormModel
            {
                Countries = data.Countries.ToList(),
                Artists=data.Artists.OrderBy(a=>a.ArtistName).ToList(),
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
            

            if (!this.UserIsOrganizer())
            {
                return RedirectToAction(nameof(OrganizersController.Create), "Organizers");
            }

            if (!this.data.Cities.Any(c => c.Id == model.CityId) || !this.data.Countries.Any(c => c.Id == model.CountryId))
            {
                this.ModelState.AddModelError(nameof(model.CountryId), "Select a valid place");

            }

            if (!ModelState.IsValid)
            {
                model.Countries = data.Countries.ToList();
                model.Cities = data.Cities.ToList();
                model.Artists = data.Artists.ToList();

                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);

            }


            var artists = data.Artists.Select(a=>a.ArtistName).ToList();

            var curr = new Event
            {
                EventName = model.EventName,
                Venue = model.Venue,
                Description = model.Description == null ? "" : model.Description,
                ImgURL = model.ImgURL,
                Time = model.Time,
                CountryId = model.CountryId,
                CityId = model.CityId,
                OrganizerId = userId,
                Artists= new List<Artist>() { data.Artists.Where(a=>a.Id==model.ArtistId).First() }

            };


            this.data.Events.Add(curr);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

         private bool UserIsOrganizer()
                =>this.data //!
                .Organizers
                .Any(d => d.UserId==
                this.User.GetId());


      
        public IActionResult All([FromQuery] AllEventsQueryModel query)
        {
            var events = this.events.All(query.SearchTerm,
                                         query.CountryId,
                                         query.CityId,
                                         query.SortingType,
                                         query.CurrentPage,
                                         AllEventsQueryModel.EventsPerPage);



            query.Countries = data.Countries.ToList();
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
            var artists = data.Artists.Where(e=>e.Events.Select(e=>e.Id).Contains(id));
            var ev = this.data.Events.Where(a => a.Id == id).First();

            var country = data.Countries.First(c => c.Id == ev.CountryId);
            var city = data.Cities.First(c => c.Id == ev.CityId);
            //var artists = data.Artists.ToList();


            var res = new EventProfileModel {
                EventName = ev.EventName,
                ImgURL = ev.ImgURL,
                Description=ev.Description,
                Artists=artists,
                Id = ev.Id,
                CityName=ev.City.CityName,
                CountryName=ev.Country.CountryName,
                Time=ev.Time, 
                Venue=ev.Venue 
            };

            return View(res);

        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = this.data
           .Organizers
           .Where(o => o.UserId == this.User.GetId())
           .Select(o => o.Id)
           .FirstOrDefault();

             
            if (!this.UserIsOrganizer())
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
                                CountryId=e.CountryId,
                                 

                            }).FirstOrDefault();

            eventForm.Cities = data.Cities.Where(c=>c.CountryId==eventForm.CountryId).ToList();
            eventForm.Countries = data.Countries.ToList();

            return View(eventForm);
        }

        [Authorize]
        [HttpPost] 
        public IActionResult Edit(int id, AddEventFormModel e)
        {

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


        public IActionResult GetPartialArtists()
        {
            

            return PartialView("_ArtistsPartial");
        }


        public JsonResult GetCities(int CountryId)
        {
         
            var res=data.Cities.Where(c => c.CountryId == CountryId).ToList();
            var t= Json(new SelectList(res,"Id","CityName"));
            return t;
        }

        public JsonResult GetCountries()
        {
            return Json(new SelectList(data.Countries.ToList(), "Id", "CountryName"));
        }

    }
}
