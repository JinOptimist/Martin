using System.Collections.Generic;
using System.Web.UI.WebControls;
using Dao.Model;

namespace Martin.ViewModel
{
    public class SongViewModel
    {
        public Song Song { get; set; }

        public List<AlbumForListBox> AlbumsList { get; set; }
    }

    public class AlbumForListBox
    {
        public AlbumForListBox(Album album)
        {
            Id = album.Id;
            Name = album.Name;
        }

        public long Id { get; set; }

        public string Name { get; set; }
    }
}