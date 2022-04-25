using MusicEvents.Data.Models;
using MusicEvents.Models.Artists;

namespace MusicEvents.Models.Events
{
    public class EventProfileModel
    {

        public int Id { get; set; }
        public string EventName { get; set; }

        public string Venue { get; set; }

        public string ImgURL { get; set; }

        public string Description { get; set; }

        public string CountryName { get; set; }
      

        public string CityName { get; set; }
      

        public DateTime Time { get; set; }

        public IEnumerable<Artist> Artists { get; set; } = new List<Artist>();
    }
}
