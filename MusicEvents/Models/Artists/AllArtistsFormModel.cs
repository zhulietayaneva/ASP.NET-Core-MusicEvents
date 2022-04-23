using MusicEvents.Data.Models;

namespace MusicEvents.Models.Artists
{
    public class AllArtistsFormModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Event> NumberOfEvents { get; set; }
        public string Description { get; set; }
        public string GenreName { get; set; }
        public string ImageUrl { get; set; }
    }
}
