namespace Dao.Model
{
    public class StaticContent
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public TypeStaticContent TypeStaticContent { get; set; }
    }

    public enum TypeStaticContent
    {
        About,
        Donate
    }
}