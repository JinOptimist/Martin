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

        public void Delete(long id)
        {
            var song = Get(id);
            var albumId = song.Album.Id;
            _db.Song.Remove(song);
            _db.SaveChanges();
            Reorder(albumId);
        }

        public void Order(long id, bool up)
        {
            NewOrder(id, up);
        }

        public List<Song> GetAllForAlbum(long albumId)
        {
            return _db.Song.Where(x => x.Album.Id == albumId).OrderByDescending(x => x.Order).ToList();
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

        public void Reorder(long albumId)
        {
            var i = 1;
            var songs = _db.Song.Where(x=>x.Album.Id == albumId).OrderBy(x => x.Order);
            foreach (var song in songs)
            {
                song.Order = i++;
                _db.Song.Attach(song);
                _db.Entry(song).State = EntityState.Modified;
            }

            _db.SaveChanges();
        }

        public int Count(long albumId)
        {
            return _db.Song.Count(x => x.Album.Id == albumId);
        }

        private Song GetByOrder(long albumId, int songOrder)
        {
            return _db.Song.SingleOrDefault(x => x.Album.Id == albumId && x.Order == songOrder);
        }

        private void NewOrder(long id, bool goUp)
        {
            var song = Get(id);
            var oldOrder = song.Order;
            var newOrder = goUp ? oldOrder + 1 : oldOrder - 1;

            if (newOrder < 1 || newOrder > Count(song.Album.Id))
            {
                return;
            }

            var songToRelocate = GetByOrder(song.Album.Id, newOrder);
            if (songToRelocate != null)
            {
                songToRelocate.Order = oldOrder;

                _db.Song.Attach(songToRelocate);
                _db.Entry(songToRelocate).State = EntityState.Modified;
            }

            song.Order = newOrder;
            _db.Song.Attach(song);
            _db.Entry(song).State = EntityState.Modified;

            _db.SaveChanges();
        }
    }
}