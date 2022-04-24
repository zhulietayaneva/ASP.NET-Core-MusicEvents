using MusicEvents.Models;

namespace MusicEvents.Services.Artists
{
    public interface IArtistService
    {
        public ArtistsQueryServiceModel All(string searchTerm, int countryId, ArtistSorting sorting, int currentPage, int artistsPerPage, int genreId);

    }
}
