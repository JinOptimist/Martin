using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dao.Model
{
    public class Album
    {
        
        public virtual long Id { get; set; }

        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual string Description { get; set; }

        [Required]
        public virtual string CoverFileName { get; set; }

        [Required]
        public virtual string BackgroundFileName { get; set; }

        [Required]
        public virtual string TextColor { get; set; }

        public virtual List<Song> Songs { get; set; }
    }
}