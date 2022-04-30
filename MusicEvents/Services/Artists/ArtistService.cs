using MusicEvents.Controllers;
using MusicEvents.Data;
using MusicEvents.Data.Models;
using MusicEvents.Models;
using MusicEvents.Models.Artists;
using MusicEvents.Services.Countries;
using MusicEvents.Services.Organizers;

namespace MusicEvents.Services.Artists
{
    public class ArtistService : IArtistService
    {
        private readonly MusicEventsDbContext data;
        private readonly ICountryService countries;
        private readonly IOrganizerService organizers;

        public ArtistService(MusicEventsDbContext data, ICountryService countries, IOrganizerService organizers)
        {
            this.data = data;
            this.countries = countries;
            this.organizers = organizers;
        }

        public ArtistsQueryServiceModel All(string searchTerm, int countryId, ArtistSorting sorting, int currentPage, int artistsPerPage, int genreId)
        {
            var artistsQuery = data.Artists.AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                artistsQuery =
                    artistsQuery
                    .Where(e => e.ArtistName.Contains(searchTerm.ToLower()));
            }

            if (artistsQuery.Any(e => e.CountryId == countryId))
            {
                artistsQuery =
                    artistsQuery
                    .Where(e => e.CountryId == countryId);
            }
            else if (countryId != 0)
            {
                return new ArtistsQueryServiceModel { CurrentPage = currentPage, Artists = new List<ArtistServiceModel>(), TotalArtists = 0, ArtistsPerPage = artistsPerPage };
            }

            if (artistsQuery.Any(e => e.GenreId == genreId))
            {
                artistsQuery =
                    artistsQuery
                    .Where(e => e.GenreId == genreId);
            }
            else if (genreId != 0)
            {
                return new ArtistsQueryServiceModel { CurrentPage = currentPage, Artists = new List<ArtistServiceModel>(), TotalArtists = 0, ArtistsPerPage = artistsPerPage };
            }

            artistsQuery = sorting switch
            {
                ArtistSorting.TotalEvents => artistsQuery.OrderByDescending(a => a.Events.Count).Where(e => e.Events.Count > 0),
                ArtistSorting.Id => artistsQuery.OrderByDescending(a => a.Id),
                ArtistSorting.Name or _ => artistsQuery.OrderBy(a => a.ArtistName)
            };

            var artists = artistsQuery
                .Skip((currentPage - 1) * artistsPerPage)
                .Take(artistsPerPage)
             .Select(e => new ArtistServiceModel
             {
                 Id = e.Id,
                 CountryName = e.Country.CountryName,
                 ArtistName = e.ArtistName,
                 GenreName = e.Genre.GenreName,
                 Biography = e.Biography,
                 ImageUrl = e.ImageURL,
                 NumberOfEvents = e.Events.Count

             })
             .ToList();

             var totalArtists = artistsQuery.Count();
            
             return new ArtistsQueryServiceModel { CurrentPage = currentPage, Artists = artists, TotalArtists = totalArtists, ArtistsPerPage = artistsPerPage };
        }


        public void Add(string artistName, string? biography, DateTime birthDate, int countryId, int genreId, string imageUrl)
        {
            
            var curr = new Artist
            {
                ArtistName = artistName,
                Biography = biography,
                BirthDate = birthDate,
                CountryId = countryId,
                GenreId = genreId,
                ImageURL = imageUrl
            };

            this.data.Artists.Add(curr);
            this.data.SaveChanges();

        }

        public AddArtistFormModel Edit(int id)
        {
            var artistForm = this.data.Artists.Where(a => a.Id == id)
                        .Select(a => new AddArtistFormModel
                        {
                            ArtistName = a.ArtistName,
                            Biography = a.Biography,
                            BirthDate = a.BirthDate,
                            CountryId = a.CountryId,
                            GenreId = a.GenreId,
                            ImageURL = a.ImageURL


                        }).FirstOrDefault();

            artistForm.Genres = data.Genres.ToList();
            artistForm.Countries = countries.GetCountries();

            return artistForm;
        }

        public void Edit(AddArtistFormModel a, int id)
        {
            var artist = this.data.Artists.Find(id);

            artist.CountryId = a.CountryId;
            artist.ArtistName = a.ArtistName;
            artist.Biography = a.Biography;
            artist.BirthDate = a.BirthDate;
            artist.GenreId = a.GenreId;
            artist.ImageURL = a.ImageURL;


            this.data.SaveChanges();
        }

        public ArtistProfileModel Details(int artistid)
        {
            var artist = this.data.Artists.Where(a => a.Id == artistid).First();
            var songs = data.Songs.Where(s => s.Artists.Select(a => a.Id).Contains(artistid)).ToList();
            artist.Genre = data.Genres.Where(g => g.Id == artist.GenreId).FirstOrDefault();
            var eventsOfCurrArtist = data.Events.Where(e => e.Artists
                                                            .Select(a => a.Id).Where(a => a == artist.Id).First()
                                                            == artist.Id).OrderBy(e => e.Time).ToList();

            var country = countries.GetCountries().First(c => c.Id == artist.CountryId);

            var res = new ArtistProfileModel { ArtistName = artist.ArtistName, ImageUrl = artist.ImageURL, Biography = artist.Biography, Events = artist.Events, GenreName = artist.Genre.GenreName, Id = artist.Id, Songs = songs, CountryName = country.CountryName };

            return res;
        }
    }
}
