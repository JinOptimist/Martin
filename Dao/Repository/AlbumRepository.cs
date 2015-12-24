using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly MartinContext _db = new MartinContext();

        public Album GetRandom()
        {
            return _db.Album.First();
        }

        public Album Get(long id)
        {
            return _db.Album.SingleOrDefault(x => x.Id == id);
        }

        public void Delete(long id)
        {
            _db.Album.Remove(Get(id));
            _db.SaveChanges();
        }

        public List<Album> GetAll()
        {
            return _db.Album.OrderBy(x => x.Order).ToList();
        }

        public void Save(Album album)
        {
            if (album.Id > 0)
            {
                _db.Album.Attach(album);
                _db.Entry(album).State = EntityState.Modified;
                _db.SaveChanges();
                return;
            }

            _db.Album.Add(album);
            _db.SaveChanges();
        }
    }
}