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
            var countryList = data.Countries.ToList();
            cache.Set("GetCountriesCache", countryList);
            return cache.Get<IEnumerable<Country>>("GetCountriesCache");
        }
    }
}
