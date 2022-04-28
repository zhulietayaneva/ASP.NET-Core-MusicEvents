using MusicEvents.Data.Models;

namespace MusicEvents.Services.Cities
{
    public interface ICityService
    {
        public IEnumerable<City> GetCitties();
    }
}
