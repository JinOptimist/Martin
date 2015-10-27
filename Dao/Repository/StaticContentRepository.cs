using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class StaticContentRepository:IStaticContentRepository
    {
        private readonly MartinContext _db = new MartinContext();

        public StaticContent Get(TypeStaticContent staticContent)
        {
            return _db.StaticContent.SingleOrDefault(x => x.TypeStaticContent == staticContent);
        }

        public List<StaticContent> GetAll()
        {
            return _db.StaticContent.ToList();
        }

        public void Save(StaticContent content)
        {
            var oldRecord = _db.StaticContent.FirstOrDefault(x => x.TypeStaticContent == content.TypeStaticContent);

            if (oldRecord != null)
            {
                oldRecord.Body = content.Body;
                oldRecord.Title = content.Title;

                _db.StaticContent.Attach(oldRecord);
                _db.Entry(oldRecord).State = EntityState.Modified;
                _db.SaveChanges();
                return;
            }

            _db.StaticContent.Add(content);
            _db.SaveChanges();
        }
    }
}