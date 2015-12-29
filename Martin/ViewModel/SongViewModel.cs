﻿using System.Collections.Generic;
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

        public Song CreateSongModel()
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
}