using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Dao.Model;

namespace Dao
{
    public class MartinContext : DbContext
    {
        public MartinContext() : base("name=MartinContext")
        {
            //Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Album> Album { get; set; }

        public DbSet<Song> Song { get; set; }

        public DbSet<StaticContent> StaticContent { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Album>().HasMany(u => u.Songs).WithRequired(t => t.Album);

            //modelBuilder.Entity<UserFromVk>().HasMany(u => u.FriendIds).WithRequired(t => t.UserFromVk);
            //modelBuilder.Entity<Course>()
            //    .HasMany(c => c.Instructors).WithMany(i => i.Courses)
            //    .Map(t => t.MapLeftKey("CourseID")
            //        .MapRightKey("InstructorID")
            //        .ToTable("CourseInstructor"));
            //modelBuilder.Entity<Department>().MapToStoredProcedures();
        }
    }
}