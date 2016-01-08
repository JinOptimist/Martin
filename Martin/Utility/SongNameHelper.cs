using Dao.Model;

namespace Martin.Utility
{
    public static class SongNameHelper
    {
        public static string FullSongNamePattern = "{0}. Martin S. - {1}";

        public static string FullSongName(this Song song)
        {
            return string.Format(FullSongNamePattern, song.Album.Songs.Count - song.Order + 1, song.Name);
        }
    }
}