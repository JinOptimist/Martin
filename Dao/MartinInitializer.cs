using System.Collections.Generic;
using System.Data.Entity;
using Dao.Model;

namespace Dao
{
    public class MartinInitializer : DropCreateDatabaseIfModelChanges<MartinContext>
    {
        protected override void Seed(MartinContext context)
        {
            //var artist = new Artist {Name = "Jin", Password = "881205", MegaAdmin = true};
            //context.Artist.Add(artist);
            //context.SaveChanges();
        }
    }
}