using MusicEvents.Data.Models;

namespace MusicEvents.Models.Songs
{
    public class ShowSongsInIndexModel
    {
        public int Id { get; set; }
        public string SongName { get; set; }

        public string Genre { get; set; }
       
        public string SongURL { get; set; }

        public string ArtistsShow { get; set; }
  
    }
}
