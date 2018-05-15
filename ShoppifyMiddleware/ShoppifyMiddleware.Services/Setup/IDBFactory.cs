using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppifyMiddleware.DataBase;
namespace ShoppifyMiddleware.Services.Setup
{
    public interface IDbFactory
    {
         ShoppifyDatabaseConnection Init();
    }

    public class DbFactory : IDbFactory
    {
        ShoppifyDatabaseConnection dbContext;

        public ShoppifyDatabaseConnection Init()
        {
            return dbContext ?? (dbContext = new ShoppifyDatabaseConnection());
        }
    }
}
