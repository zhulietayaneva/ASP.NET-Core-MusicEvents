using Microsoft.AspNetCore.Mvc;
using MusicEvents.Data;
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

        public IActionResult Add() => View(new EventAddFormModel { Countries = CountryList(),
            
        });


        [HttpPost]
        public IActionResult Add(EventAddFormModel model)
        {
            return View();
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

    }      
}
