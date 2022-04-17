using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicEvents.Data;
using MusicEvents.Data.Models;
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

        public IActionResult Add() {

          

            var res = new EventAddFormModel
            {
                Countries = data.Countries.ToList(),
                Time = DateTime.UtcNow.Date,

            };
            return View(res);
        } 


        [HttpPost]
        public IActionResult Add(EventAddFormModel model)
        {
            //if (!this.data.Countries.Any(c => c.Id == model.CountryId))
            //{
            //    this.ModelState.AddModelError(nameof(model.CountryId), "Category");

            //}

            if (!ModelState.IsValid)
            {
                model.Countries = data.Countries.ToList();                
                model.Cities = data.Cities.ToList();

                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);

            }

            var curr = new Event
            {
                EventName = model.EventName,
                Venue = model.Venue,
                Description = model.Description == null ? "" : model.Description,
                ImgURL = model.ImgURL,
                Time = model.Time

            };


           // this.data.Events.Add(curr);
           // this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }



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
