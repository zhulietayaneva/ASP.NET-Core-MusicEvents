using MusicEvents.Data.Models;

namespace MusicEvents.Services.Countries
{
    public interface ICountryService
    {

        public IEnumerable<Country> GetCountries();

    }
}
