using MusicEvents.Data.Models;

namespace MusicEvents.Services.Genres
{
    public interface IGenreService
    {
        public IEnumerable<Genre> GetGenres();
    }
}
