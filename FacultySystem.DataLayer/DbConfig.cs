using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.DataLayer
{
    public class DbConfig : DbConfiguration
    {
        public DbConfig()
        {
            DbInterception.Add(new YeKeInterceptor());
        }
    }
}
