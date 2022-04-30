using Microsoft.Extensions.Caching.Memory;
using MusicEvents.Data;
using MusicEvents.Data.Models;

namespace MusicEvents.Services.Cities
{
    public class CityService:ICityService
    {
        private readonly MusicEventsDbContext data;
        private readonly IMemoryCache cache;

        public CityService(MusicEventsDbContext data, IMemoryCache cache)
        {
            this.data = data;
            this.cache = cache;

        }


        public IEnumerable<City> GetCitties()
        {

            return cache.GetOrCreate("GetCitiesCache",
                                 cacheEntry =>
                                 {
                                     var cities = data.Cities.ToList();
                                     var options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(50));
                                     return cache.Set("GetCitiesCache", cities, options);
                                 }
                                 );


        }


    }
}
