using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicEvents.Data;
using MusicEvents.Data.Models;
using MusicEvents.Infrastructure;
using MusicEvents.Models.Artists;

namespace MusicEvents.Controllers
{
    public class ArtistsController:Controller
    {
        private readonly MusicEventsDbContext data;

        public ArtistsController(MusicEventsDbContext data)
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


            var res = new AddArtistFormModel
            {
                Countries = data.Countries.ToList(),
                Genres = data.Genres.ToList(),
                BirthDate = DateTime.UtcNow.Date,

            };
            return View(res);
        }


        [HttpPost]
        [Authorize]
        public IActionResult Add(AddArtistFormModel model)
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

            if (!this.data.Countries.Any(c => c.Id == model.CountryId))
            {
                this.ModelState.AddModelError(nameof(model.CountryId), "Select a valid place");

            }

            if (!ModelState.IsValid)
            {
                model.Countries = data.Countries.ToList();
                model.Genres = data.Genres.ToList();

                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);

            }


            var artists = data.Artists.Select(a => a.ArtistName).ToList();

            var curr = new Artist
            {
                ArtistName = model.ArtistName,
                Biography=model.Biography,
                BirthDate=model.BirthDate,
                CountryId=model.CountryId,
                GenreId =model.GenreId,
                ImageURL=model.ImageURL,

            };


            this.data.Artists.Add(curr);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        private bool UserIsOrganizer()
               => this.data //!
               .Organizers
               .Any(d => d.UserId ==
               this.User.GetId());


        public IActionResult All()
        {
            //var events = this.data
            //.Artists
            //.OrderByDescending(e => e.Id)
            //.Select(e => new AllArtistsFormModel
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


        public IActionResult Delete(int id)
        {
            var ev = this.data.Artists.Where(e => e.Id == id).FirstOrDefault();
            data.Remove(ev);
            data.SaveChanges();

            return RedirectToAction(nameof(All));
        }
    }
}
