using System.Collections.Generic;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IAlbumRepository
    {
        Album Get(long id);

        List<Album> GetAll();

        void Save(Album album);
    }
}