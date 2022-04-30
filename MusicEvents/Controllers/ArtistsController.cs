using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicEvents.Data;
using MusicEvents.Infrastructure;
using MusicEvents.Models.Artists;
using MusicEvents.Services.Artists;
using MusicEvents.Services.Countries;
using MusicEvents.Services.Organizers;

namespace MusicEvents.Controllers
{
    public class ArtistsController:Controller
    {
        private readonly MusicEventsDbContext data;
        private readonly IArtistService artists;
        private readonly ICountryService countries;
        private readonly IOrganizerService organizers;

        public ArtistsController(MusicEventsDbContext data, IArtistService artists, ICountryService countries, IOrganizerService organizers)
        {
            this.artists = artists;
            this.data = data;
            this.countries = countries;
            this.organizers = organizers;
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!organizers.IsOrganizer(User.GetId()))
            {
                return RedirectToAction(nameof(OrganizersController.Create), "Organizers");
            }

            var res = new AddArtistFormModel
            {
                Countries = countries.GetCountries(),
                Genres = data.Genres.ToList(),
                BirthDate = DateTime.UtcNow.Date,

            };
            return View(res);
        }


        [HttpPost]
        [Authorize]
        public IActionResult Add(AddArtistFormModel model)
        {
            if (!organizers.IsOrganizer(User.GetId()))
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

            artists.Add(model.ArtistName,
                        model.Biography,
                        model.BirthDate,
                        model.CountryId,
                        model.GenreId,
                        model.ImageURL);



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
            
            query.Countries = countries.GetCountries();
            query.TotalArtists = artists.TotalArtists;
            query.Artists = artists.Artists;
            query.Genres = data.Genres.ToList();
            return View(query);
        }


        [Authorize]
        public IActionResult Edit(int id)
        {
            if (!organizers.IsOrganizer(User.GetId()))
            {
                return RedirectToAction(nameof(OrganizersController.Create), "Organizers");
            }
            return View(artists.Edit(id));
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id, AddArtistFormModel a)
        {
            if (!organizers.IsOrganizer(User.GetId()))
            {
                return RedirectToAction(nameof(OrganizersController.Create), "Organizers");
            }
            artists.Edit(a, id);
            return RedirectToAction("Details","Artists",new { id});
        }

        public IActionResult Details(int artistid)
        { 
            return View(artists.Details(artistid));
        }
       
        [Authorize]
        public IActionResult Delete(int id)
        {
            if (!organizers.IsOrganizer(User.GetId()))
            {
                return RedirectToAction(nameof(OrganizersController.Create), "Organizers");
            }
            var ev = this.data.Artists.Where(e => e.Id == id).FirstOrDefault();
            data.Remove(ev);
            data.SaveChanges();

            return RedirectToAction(nameof(All));
        }
    }
}
