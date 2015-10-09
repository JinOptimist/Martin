using System.Collections.Generic;
using System.Data.Entity;
using Dao.Model;

namespace Dao
{
    public class MartinInitializer : DropCreateDatabaseIfModelChanges<MartinContext>
    {
        protected override void Seed(MartinContext context)
        {
            var albums = new List<Album>
            {
                new Album {Name = "Carson", Id = 1},
                new Album {Name = "Meredith", Id = 2},
                new Album {Name = "Arturo", Id = 3},
                new Album {Name = "Gytis", Id = 4},
                new Album {Name = "Yan", Id = 5},
                new Album {Name = "Peggy", Id = 6},
                new Album {Name = "Laura", Id = 7},
                new Album {Name = "Nino", Id = 8}
            };

            context.Album.AddRange(albums);
            context.SaveChanges();
        }
    }
}