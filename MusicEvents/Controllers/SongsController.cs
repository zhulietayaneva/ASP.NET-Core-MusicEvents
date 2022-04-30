using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicEvents.Data;
using MusicEvents.Data.Models;
using MusicEvents.Infrastructure;
using MusicEvents.Models.Songs;
using MusicEvents.Services.Organizers;

namespace MusicEvents.Controllers
{
    public class SongsController : Controller
    {

        private readonly MusicEventsDbContext data;
        private readonly IOrganizerService organizers;

        public SongsController(MusicEventsDbContext data)
        {
            this.data = data;
        }

        [Authorize]
        public IActionResult Add(int artistid)
        {
            if (!organizers.IsOrganizer(User.GetId()))
            {
                return RedirectToAction(nameof(OrganizersController.Create), "Organizers");
            }

            var res = new AddSongFormModel
            {
                Genres = data.Genres.ToList(),
                Artists= data.Artists.ToList(),
                ArtistId = artistid

            };
            return View(res);
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

            var artist = data.Artists.First(a=>a.Id==model.ArtistId);
            var curr = new Song
            {
                SongName = model.SongName,
                Artists = new List<Artist>() { artist },
                GenreId = model.GenreId,
                SongURL = model.SongURL
            };


            data.Songs.Add(curr);
            data.SaveChanges();

            return RedirectToAction("Details", "Artists", new { artistid });
        }

        [Authorize]
        public IActionResult Delete(int id,int artistid)
        {
            var song = this.data.Songs.Where(e => e.Id == id).FirstOrDefault();
            data.Remove(song);
            data.SaveChanges();

            return RedirectToAction("Details","Artists", new { artistid });
        }

        
    }
}
