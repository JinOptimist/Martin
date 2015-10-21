using System.Collections.Generic;

namespace Dao.Model
{
    public class Album
    {
        public virtual long Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string CoverFileName { get; set; }

        public virtual List<Song> Songs { get; set; }
    }
}