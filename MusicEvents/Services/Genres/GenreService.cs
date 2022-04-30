using Microsoft.Extensions.Caching.Memory;
using MusicEvents.Data;
using MusicEvents.Data.Models;

namespace MusicEvents.Services.Genres
{
    public class GenreService:IGenreService
    {
        private readonly MusicEventsDbContext data;
        private readonly IMemoryCache cache;
        public GenreService(MusicEventsDbContext data, IMemoryCache cache)
        {
            this.data = data;
            this.cache = cache;
        }

        public IEnumerable<Genre> GetGenres()
        {
            return cache.GetOrCreate("GetGenresCache",
                                cacheEntry =>
                                {
                                    var genres = data.Genres.ToList();
                                    var options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(50));
                                    return cache.Set("GetGenresCache", genres, options);
                                });

        }
    }
}
