namespace Dao.Model
{
    public class Song
    {
        public virtual long Id { get; set; }

        public virtual string Name { get; set; }

        //Text of record
        public virtual string Lyrics { get; set; }

        public virtual string Mp3FileName { get; set; }

        public virtual Album Album { get; set; }
    }
}