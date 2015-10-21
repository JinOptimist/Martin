using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class SongRepository : ISongRepository
    {
        private readonly MartinContext _db = new MartinContext();

        public Song Get(long id)
        {
            return _db.Song.SingleOrDefault(x => x.Id == id);
        }

        public List<Song> GetAllForAlbum(long albumId)
        {
            return _db.Song.Where(x => x.Album.Id == albumId).ToList();
        }

        public List<Song> GetAll()
        {
            return _db.Song.ToList();
        }

        public void Save(Song song)
        {
            var album = _db.Album.FirstOrDefault(x => x.Id == song.Album.Id);
            song.Album = album;

            if (song.Id > 0)
            {
                _db.Song.Attach(song);
                _db.Entry(song).State = EntityState.Modified;
                _db.SaveChanges();
                return;
            }

            _db.Song.Add(song);
            _db.SaveChanges();
        }
    }
}