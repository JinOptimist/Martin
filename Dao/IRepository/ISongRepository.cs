using System.Collections.Generic;
using Dao.Model;

namespace Dao.IRepository
{
    public interface ISongRepository
    {
        Song Get(long id);

        void Delete(long id);

        List<Song> GetAllForAlbum(long albumId);

        List<Song> GetAll();

        void Save(Song song);
    }
}