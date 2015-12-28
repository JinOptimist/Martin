using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly MartinContext _db = new MartinContext();

        public Artist Get(long id)
        {
            return _db.Artist.SingleOrDefault(x => x.Id == id);
        }

        public void Delete(long id)
        {
            _db.Artist.Remove(Get(id));
            _db.SaveChanges();
        }

        public List<Artist> GetAll()
        {
            return _db.Artist.ToList();
        }

        public void Save(Artist artist)
        {
            if (artist.Id > 0)
            {
                _db.Artist.Attach(artist);
                _db.Entry(artist).State = EntityState.Modified;
                _db.SaveChanges();
                return;
            }

            _db.Artist.Add(artist);
            _db.SaveChanges();
        }

        public Artist CheckPassword(Artist artist)
        {
            return _db.Artist.SingleOrDefault(x => x.Name == artist.Name && x.Password == artist.Password);
        }
    }
}