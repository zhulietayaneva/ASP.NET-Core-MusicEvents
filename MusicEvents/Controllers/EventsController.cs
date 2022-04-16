using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Add() => View(new EventAddFormModel
        {
            Countries = CountryList(),

        });


        [HttpPost]
        public IActionResult Add(EventAddFormModel model)
        {
            //if (!this.data.Countries.Any(c => c.Id == model.CountryId))
            //{
            //    this.ModelState.AddModelError(nameof(model.CountryId), "Category");

            //}

            if (!ModelState.IsValid)
            {
                model.Countries = CountryList();
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);

            }

            var curr = new Event
            {
                EventName = model.EventName,
                Venue = model.Venue,
                Description = model.Description==null?"":model.Description,
                ImgURL = model.ImgURL,

            };


           // this.data.Events.Add(curr);
           // this.data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public static List<string> CountryList()
        {
            //Creating list
            List<string> Culturelist = new List<string>();
            //getting the specific CultureInfo from CultureInfo class
            CultureInfo[] getCultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo getCulture in getCultureInfo)
            {
                //creating the object of RegionInfo class
                RegionInfo GetRegionInfo = new RegionInfo(getCulture.LCID);
                //adding each county Name into the arraylist
                if (!(Culturelist.Contains(GetRegionInfo.EnglishName)))
                {
                    Culturelist.Add(GetRegionInfo.EnglishName);
                }
            }
            // sorting array by using sort method to get countries in order
            Culturelist.Sort();
            //returning country list
            return Culturelist;
        }

        public IActionResult GetPartialArtists()
        {
            

            return PartialView("_ArtistsPartial");
        }
    }
}
