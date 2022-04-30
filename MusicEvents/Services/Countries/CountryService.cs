using Microsoft.Extensions.Caching.Memory;
using MusicEvents.Data;
using MusicEvents.Data.Models;
using MusicEvents.Services.Countries;

namespace MusicEvents.Services
{
    public class CountryService:ICountryService
    {
        private readonly MusicEventsDbContext data;
        private readonly IMemoryCache cache;

        public CountryService(MusicEventsDbContext data, IMemoryCache cache)
        {
            this.data = data;
            this.cache = cache;
            
        }


        public IEnumerable<Country> GetCountries()
        {

           return  cache.GetOrCreate("GetCountriesCache",
                                cacheEntry => 
                                {
                                    var countries = data.Countries.ToList();
                                    var options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(50));
                                    return cache.Set("GetCountriesCache", countries,options);

                           
                                }
                                );

            
        }
    }
}
