using MusicEvents.Data.Models;
using MusicEvents.Models.Events;

namespace MusicEvents.Models.Artists
{
    public class ArtistProfileModel
    {
        public int Id { get; set; }
        public string ArtistName { get; set; }
        public string? Biography { get; set; }
        public int NumberOfEvents => Events.Count;
        public string GenreName { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<Event> Events{ get; set; }=new List<Event>();



    }
}
