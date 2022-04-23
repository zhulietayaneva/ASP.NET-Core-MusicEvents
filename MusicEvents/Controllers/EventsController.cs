using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicEvents.Data;
using MusicEvents.Data.Models;
using MusicEvents.Infrastructure;
using MusicEvents.Models.Artists;
using MusicEvents.Models.Events;
using System.Globalization;

namespace MusicEvents.Controllers
{
    public class EventsController : Controller
    {
        private readonly MusicEventsDbContext data;
        

        public EventsController(MusicEventsDbContext data)
        {
            this.data = data;
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
              


        public IActionResult All()
        {
             var events = this.data
             .Events
             .OrderByDescending(e => e.Id)
             .Select(e => new AllEventsFormModel
             {
                 Id = e.Id,
                 CityName=e.City.CityName,
                 CountryName=e.Country.CountryName,
                 Artists=String.Join(", ",e.Artists.Select(a=>a.ArtistName)),
                 Description=e.Description,
                 EventName=e.EventName,
                 ImgURL=e.ImgURL,
                 Time = e.Time,
                 Venue=e.Venue,
                 

             })
             .ToList();

            return View(events);
        }
        [Authorize]
        public IActionResult Delete(int id)
        {
            var ev = this.data.Events.Where(e => e.Id == id).FirstOrDefault();
            data.Remove(ev);
            data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        public IActionResult Details()
        {
            return View();

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
