using System.Collections.Generic;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IStaticContentRepository
    {
        StaticContent Get(TypeStaticContent staticContent);

        List<StaticContent> GetAll();

        void Save(StaticContent content);
    }
}