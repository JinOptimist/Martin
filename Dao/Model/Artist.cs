using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dao.Model
{
    public class Artist
    {
        public virtual long Id { get; set; }

        [Required]
        [MaxLength(120)]
        [Index(IsUnique = true)]
        public virtual string Name { get; set; }

        [Required]
        public virtual string Password { get; set; }

        public virtual bool MegaAdmin { get; set; } 
    }
}