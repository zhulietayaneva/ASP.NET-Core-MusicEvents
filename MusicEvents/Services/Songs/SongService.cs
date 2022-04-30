using MusicEvents.Data;
using MusicEvents.Data.Models;
using MusicEvents.Models.Songs;

namespace MusicEvents.Services.Songs
{
    public class SongService : ISongService
    {
        private readonly MusicEventsDbContext data;
       

        public SongService(MusicEventsDbContext data)
        {
           this.data = data;
        }

        public AddSongFormModel Add(int artistid)
        {
            return new AddSongFormModel
            {
                Genres = data.Genres.ToList(),
                Artists = data.Artists.ToList(),
                ArtistId = artistid
            };
        }
        public void Add(int artistid,string songName,int genreId,string songUrl)
        {
            var artist = data.Artists.First(a => a.Id == artistid);
            var curr = new Song
            {
                SongName = songName,
                Artists = new List<Artist>() { artist },
                GenreId = genreId,
                SongURL = songUrl
            };

            data.Songs.Add(curr);
            data.SaveChanges();

        }
        public void Delete(int id)
        {
            var song = this.data.Songs.Where(e => e.Id == id).FirstOrDefault();
            data.Remove(song);
            data.SaveChanges();
        }
        public IEnumerable<Song> GetSongs()
        {
            return data.Songs.ToList();
        }
    }
}
