using MusicEvents.Data.Models;
using MusicEvents.Models.Songs;

namespace MusicEvents.Services.Songs
{
    public interface ISongService
    {
        public AddSongFormModel Add(int artistid);
        public void Add(int artistid, string songName, int genreId, string songUrl);
        public void Delete(int id);
        public IEnumerable<Song> GetSongs();

    }
}
