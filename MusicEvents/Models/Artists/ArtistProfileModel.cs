using MusicEvents.Data.Models;
using MusicEvents.Models.Events;

namespace MusicEvents.Models.Artists
{
    public class ArtistProfileModel
    {
        public int Id { get; set; }
        public string ArtistName { get; set; }
        public string? Biography { get; set; }
        public  string CountryName { get; set; }
        public int NumberOfEvents => Events.Count();
        public string GenreName { get; set; }
        public string ImageUrl { get; set; }
        public IEnumerable<Event> Events{ get; set; } =new List<Event>();
        public IEnumerable<Song> Songs{ get; set; } = new List<Song>();



    }
}
