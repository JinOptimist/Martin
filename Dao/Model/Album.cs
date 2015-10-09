namespace Dao.Model
{
    public class Album
    {
        public virtual long Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string ImgUrl { get; set; }
    }
}