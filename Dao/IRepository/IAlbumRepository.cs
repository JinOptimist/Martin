using System.Collections.Generic;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IAlbumRepository
    {
        Album GetRandom();

        Album Get(long id);

        Album GetByOrder(int order);

        void Order(long id, bool up);

        void Delete(long id);

        List<Album> GetAll();

        void Save(Album album);

        int Count();

        void Reorder();
    }
}