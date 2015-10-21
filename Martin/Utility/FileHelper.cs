using System.IO;

namespace Martin.Utility
{
    public class FileHelper
    {
        public static void SaveHtmlFile(string name, string content)
        {
            var path = string.Format("{0} {1}", "Path", name);
            using (var text = File.CreateText(path))
            {
                text.WriteLine(content);
            }
        } 
    }
}