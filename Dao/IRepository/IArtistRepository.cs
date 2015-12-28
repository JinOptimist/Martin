using System.Collections.Generic;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IArtistRepository
    {
        Artist Get(long id);

        void Delete(long id);

        List<Artist> GetAll();

        void Save(Artist artist);

        Artist CheckPassword(Artist artist);
    }
}