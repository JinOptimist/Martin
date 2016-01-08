using Dao.Model;

namespace Martin.Utility
{
    public static class NameHelper
    {
        public static string FullSongNamePattern = "{0}. Martin S. - {1}";

        public static string FullSongName(this Song song)
        {
            return string.Format(FullSongNamePattern, song.Album.Songs.Count - song.Order + 1, song.Name);
        }

        public static string CuteUrl(this string url)
        {
            while (url.Contains("--"))
            {
                url = url.Replace("--", "-");
            }

            return url;
        }
    }
}