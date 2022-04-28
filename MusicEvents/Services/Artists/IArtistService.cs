using MusicEvents.Models;
using MusicEvents.Models.Artists;

namespace MusicEvents.Services.Artists
{
    public interface IArtistService
    {
        public ArtistsQueryServiceModel All(string searchTerm, int countryId, ArtistSorting sorting, int currentPage, int artistsPerPage, int genreId);

        public void Add(string artistName, string? biography, DateTime birthDate, int countryId, int genreId, string imageUrl);

        public AddArtistFormModel Edit(int id);

        public void Edit(AddArtistFormModel a, int id);
        public ArtistProfileModel Details(int artistid);

    }
}
