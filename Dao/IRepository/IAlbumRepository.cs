using System.Collections.Generic;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IAlbumRepository
    {
        Album GetRandom();

        Album Get(long id);

        void Delete(long id);

        List<Album> GetAll();

        void Save(Album album);
    }
}