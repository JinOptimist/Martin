using System.Collections.Generic;
using Dao.Model;

namespace Martin.ViewModel
{
    public class AlbumsHomeViewModel
    {
        public List<Album> Albums { get; set; }

        public long AlbumId { get; set; }
    }
}