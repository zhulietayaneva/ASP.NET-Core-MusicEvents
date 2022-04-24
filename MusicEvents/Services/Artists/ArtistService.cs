using MusicEvents.Data;
using MusicEvents.Models;

namespace MusicEvents.Services.Artists
{
    public class ArtistService : IArtistService
    {
        private readonly MusicEventsDbContext data;

        public ArtistService(MusicEventsDbContext data)
        {
            this.data = data;
        }

        public ArtistsQueryServiceModel All(string searchTerm, int countryId,  ArtistSorting sorting, int currentPage, int artistsPerPage,int genreId)
        {
            var artistsQuery = data.Artists.AsQueryable();
            var countries = data.Countries.ToList();
            var totalArtists = this.data.Artists.Count();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                artistsQuery =
                    artistsQuery
                    .Where(e => e.ArtistName.Contains(searchTerm.ToLower()));
            }

            if (artistsQuery.Any(a => a.CountryId == countryId))
            {
                artistsQuery =
                    artistsQuery
                    .Where(e => e.CountryId == countryId);
            }

            if (artistsQuery.Any(a => a.GenreId == genreId))
            {
                artistsQuery =
                    artistsQuery
                    .Where(e => e.GenreId == genreId);
            }

            artistsQuery = sorting switch
            {
                ArtistSorting.TotalEvents => artistsQuery.OrderBy(a=>a.Events.Count),
                ArtistSorting.Id  => artistsQuery.OrderByDescending(a => a.Id),
                ArtistSorting.Name or _ => artistsQuery.OrderBy(a => a.ArtistName)
            };

            var artists = artistsQuery
                .Skip((currentPage - 1) * artistsPerPage)
                .Take(artistsPerPage)
             .Select(e => new ArtistServiceModel
             {
                 Id = e.Id,
                 CountryName = e.Country.CountryName,
                 ArtistName=e.ArtistName,
                 GenreName=e.Genre.GenreName,       
                 Biography=e.Biography,
                 ImageUrl=e.ImageURL,
                 NumberOfEvents=e.Events.Count

             })
             .ToList();


            return new ArtistsQueryServiceModel { CurrentPage = currentPage, Artists = artists, TotalArtists = totalArtists, ArtistsPerPage = artistsPerPage, GenreId=genreId};
        }
    }
}
