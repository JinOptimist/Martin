using System.Collections.Generic;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IAlbumRepository
    {
        List<Album> GetAlbums();
    }
}