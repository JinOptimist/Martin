using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace Dao
{
    public class MartinConfiguration : DbConfiguration
    {
        public MartinConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
        }
    }
}