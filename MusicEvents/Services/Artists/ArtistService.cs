using MusicEvents.Data;
using MusicEvents.Data.Models;
using MusicEvents.Models;
using MusicEvents.Models.Artists;
using MusicEvents.Services.Countries;
using MusicEvents.Services.Events;
using MusicEvents.Services.Genres;
using MusicEvents.Services.Songs;

namespace MusicEvents.Services.Artists
{
    public class ArtistService : IArtistService
    {
        private readonly MusicEventsDbContext data;
        private readonly ICountryService countries;
        private readonly IGenreService genres;
        private readonly ISongService songs;
        private readonly IEventService events;


        public ArtistService(MusicEventsDbContext data, ICountryService countries, IGenreService genres, ISongService songs, IEventService events)
        {
            this.data = data;
            this.countries = countries;
            this.genres = genres;
            this.songs = songs;
            this.events = events;
        }

        public ArtistsQueryServiceModel All(string searchTerm, int countryId, ArtistSorting sorting, int currentPage, int artistsPerPage, int genreId)
        {
            var artistsQuery = GetArtists().AsQueryable();
            
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

        public AddArtistFormModel Add()
        {
            return new AddArtistFormModel
            {
                Countries = countries.GetCountries(),
                Genres = genres.GetGenres().ToList(),
                BirthDate = DateTime.UtcNow.Date
            };
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
            var artistForm = GetArtists().Where(a => a.Id == id)
                        .Select(a => new AddArtistFormModel
                        {
                            ArtistName = a.ArtistName,
                            Biography = a.Biography,
                            BirthDate = a.BirthDate,
                            CountryId = a.CountryId,
                            GenreId = a.GenreId,
                            ImageURL = a.ImageURL

                        }).FirstOrDefault();

            artistForm.Genres = genres.GetGenres().ToList();
            artistForm.Countries = countries.GetCountries();

            return artistForm;
        }
        public void Edit(int id,int countryId,string artistName,string? bio,DateTime birthDate,int genreId,string ImgURL)
        {
            var artist = GetArtistById(id);

            artist.CountryId = countryId;
            artist.ArtistName = artistName;
            artist.Biography = bio;
            artist.BirthDate = birthDate;
            artist.GenreId = genreId;
            artist.ImageURL = ImgURL;

            this.data.SaveChanges();
        }
        public ArtistProfileModel Details(int artistid)
        {
            var artist = GetArtists().Where(a => a.Id == artistid).First();
            var songs = this.songs.GetSongs().Where(s => s.Artists.Select(a => a.Id).Contains(artistid)).ToList();
            var eventsOfCurrArtist = events.GetEvents().Where(e => e.Artists
                                                            .Select(a => a.Id).Where(a => a == artist.Id).First()
                                                            == artist.Id).OrderBy(e => e.Time).ToList();

            var country = countries.GetCountries().First(c => c.Id == artist.CountryId);

            var res = new ArtistProfileModel { ArtistName = artist.ArtistName, ImageUrl = artist.ImageURL, Biography = artist.Biography, Events = artist.Events, GenreName = artist.Genre.GenreName, Id = artist.Id, Songs = songs, CountryName = country.CountryName };

            return res;
        }
        public IEnumerable<Artist> GetArtists()
        {
            return this.data.Artists.ToList();
        }
        public Artist? GetArtistById(int id)
        {
            return this.data.Artists.Find(id);
        }
    }
}
