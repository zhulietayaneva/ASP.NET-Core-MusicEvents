using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicEvents.Data;
using MusicEvents.Data.Models;
using MusicEvents.Infrastructure;
using MusicEvents.Models.Artists;
using MusicEvents.Services.Artists;

namespace MusicEvents.Controllers
{
    public class ArtistsController:Controller
    {
        private readonly MusicEventsDbContext data;
        private readonly IArtistService artists;

        public ArtistsController(MusicEventsDbContext data, IArtistService artists)
        {
            this.artists = artists;
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
            if (!this.data.Genres.Any(c => c.Id == model.GenreId))
            {
                this.ModelState.AddModelError(nameof(model.GenreId), "Select a valid genre");

            }

            if (!ModelState.IsValid)
            {
                model.Countries = data.Countries.ToList();
                model.Genres = data.Genres.ToList();

                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
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


        public IActionResult All([FromQuery]AllArtistsQueryModel query) 
        {
            var artists = this.artists.All(query.SearchTerm,
                                           query.CountryId,
                                           query.SortingType,
                                           query.CurrentPage,
                                           AllArtistsQueryModel.ArtistsPerPage,
                                           query.GenreId);



            query.Countries = data.Countries.ToList();
            query.TotalArtists = artists.TotalArtists;
            query.Artists = artists.Artists;
            query.Genres = data.Genres.ToList();
            return View(query);
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
           

            var artistForm = this.data.Artists.Where(a => a.Id == id)
                            .Select(a => new AddArtistFormModel
                            {
                                ArtistName=a.ArtistName,
                                Biography=a.Biography,
                                BirthDate=a.BirthDate,
                                CountryId=a.CountryId,
                                GenreId=a.GenreId,
                                ImageURL=a.ImageURL


                            }).FirstOrDefault();

            artistForm.Genres = data.Genres.ToList();
            artistForm.Countries = data.Countries.ToList();

            return View(artistForm);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id, AddArtistFormModel a)
        {

            var artist = this.data.Artists.Find(id);

            if (artist == null)
            {
                return BadRequest();
            }
            


            artist.CountryId=a.CountryId;
            artist.ArtistName=a.ArtistName;
            artist.Biography=a.Biography;
            artist.BirthDate = a.BirthDate;
            artist.GenreId=a.GenreId;
            artist.ImageURL=a.ImageURL;
      

            this.data.SaveChanges();
            return RedirectToAction(nameof(All));


        }




        private bool UserIsOrganizer()
               => this.data //!
               .Organizers
               .Any(d => d.UserId ==
               this.User.GetId());


        public IActionResult Details(ArtistProfileModel model,int id)
        {
            var artist = this.data.Artists.Where(a => a.Id == model.Id).First();
           
            artist.Genre = data.Genres.Where(g => g.Id == artist.GenreId).FirstOrDefault();
            var eventsOfCurrArtist = data.Events.Where(e => e.Artists
                                                            .Select(a => a.Id).Where(a => a == artist.Id).First()
                                                            == artist.Id).OrderBy(e => e.Time).ToList();
                                                            
                                    

            var res = new ArtistProfileModel { ArtistName = artist.ArtistName, ImageUrl = artist.ImageURL, Biography = artist.Biography, Events = artist.Events, GenreName = artist.Genre.GenreName, Id = artist.Id };


            return View(res);
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
