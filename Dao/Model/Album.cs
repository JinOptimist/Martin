using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dao.Model
{
    public class Album
    {
        public virtual long Id { get; set; }

        [Required]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string ReadmeInArchive { get; set; }

        [Required]
        public virtual string CoverFileName { get; set; }

        [Required]
        public virtual string BackgroundFileName { get; set; }

        [Required]
        public virtual string TextColor { get; set; }

        [Required]
        public virtual int Order { get; set; }

        public virtual List<Song> Songs { get; set; }
    }
}