using Microsoft.AspNetCore.Mvc;
using MusicEvents.Data;
using MusicEvents.Models;
using MusicEvents.Models.Songs;
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

            var songs =
                this.data
                .Songs
                .Select(a => new ShowSongsInIndexModel
                {
                     ArtistsShow= String.Join(", ",a.Artists.Select(a=>a.ArtistName)),
                     Genre=a.Genre.GenreName,
                     SongName= a.SongName,
                     SongURL= a.SongURL,
                     Id= a.Id,


                })
                .OrderByDescending(a => a.Id)
                .Take(3)
                .ToList();


       

            return View(songs);
         
        }

       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}