using System.Collections.Generic;
using System.Web.UI.WebControls;
using Dao.Model;

namespace Martin.ViewModel
{
    public class SongViewModel
    {
        public string Name { get; set; }

        public string Lyrics { get; set; }

        public string Mp3FileName { get; set; }

        public Album Album { get; set; }

        public List<AlbumForListBox> AlbumsList { get; set; }

        public Song CreateSong()
        {
            var song = new Song
            {
                Name = Name,
                Album = Album, 
                Lyrics = Lyrics, 
                Mp3FileName = Mp3FileName
            };
            return song;
        }
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