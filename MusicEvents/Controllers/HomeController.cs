using Microsoft.AspNetCore.Mvc;
using MusicEvents.Data;
using MusicEvents.Models;
using System.Diagnostics;

namespace MusicEvents.Controllers
{

    public class HomeController : Controller
    {
        private readonly MusicEventsDbContext data;

        public HomeController(MusicEventsDbContext data)
        {
            this.data = data;
        }

        public IActionResult Index()
        {
            //var events = this.data
            //.Events
            //.OrderByDescending(e => e.Id)
            //.Select(e => new AllEventsFormModel
            //{
            //    Id = e.Id,
            //    CityName = e.City.CityName,
            //    CountryName = e.Country.CountryName,
            //    Artists = String.Join(", ", e.Artists.Select(a => a.ArtistName)),
            //    Description = e.Description,
            //    EventName = e.EventName,
            //    ImgURL = e.ImgURL,
            //    Time = e.Time,
            //    Venue = e.Venue,


            //})
            //.ToList();

            return View();
        }

       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}