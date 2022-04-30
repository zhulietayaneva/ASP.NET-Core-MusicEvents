using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicEvents.Data;
using MusicEvents.Data.Models;
using MusicEvents.Infrastructure;
using MusicEvents.Models.Songs;
using MusicEvents.Services.Organizers;
using MusicEvents.Services.Songs;

namespace MusicEvents.Controllers
{
    public class SongsController : Controller
    {

        private readonly MusicEventsDbContext data;
        private readonly IOrganizerService organizers;
        private readonly ISongService songs;

        public SongsController(MusicEventsDbContext data, IOrganizerService organizers, ISongService songs)
        {
            this.data = data;
            this.organizers = organizers;
            this.songs = songs;
        }

        [Authorize]
        public IActionResult Add(int artistid)
        {
            if (!organizers.IsOrganizer(User.GetId()))
            {
                return RedirectToAction(nameof(OrganizersController.Create), "Organizers");
            }

           return View(this.songs.Add(artistid));
        }
         
        [Authorize]
        [HttpPost]
        public IActionResult Add(int artistid, AddSongFormModel model )
        {
            var artists = data.Artists.ToList();
            var genres = data.Genres.ToList();
            model.Genres = genres;
            model.Artists = artists;

            if (!organizers.IsOrganizer(User.GetId()))
            {
                return RedirectToAction(nameof(OrganizersController.Create), "Organizers");
            }
            if (!this.data.Genres.Any(c => c.Id == model.GenreId))
            {
                this.ModelState.AddModelError(nameof(model.GenreId), "Select a valid genre");

            }
            if (!this.data.Artists.Any(c => c.Id == model.ArtistId))
            {
                this.ModelState.AddModelError(nameof(model.GenreId), "Select a valid artist");

            }
            if (!ModelState.IsValid)
            {
                model.Genres = genres;
                model.Artists = artists;
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                
                return View(model);

            }

            songs.Add(artistid, model.SongName, model.GenreId, model.SongURL);

            return RedirectToAction("Details", "Artists", new { artistid });
        }

        [Authorize]
        public IActionResult Delete(int id,int artistid)
        {
            songs.Delete(id);
            return RedirectToAction("Details","Artists", new { artistid });
        }
        
    }
}
