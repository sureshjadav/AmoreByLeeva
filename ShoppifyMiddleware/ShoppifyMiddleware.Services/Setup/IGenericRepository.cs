using ShoppifyMiddleware.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShoppifyMiddleware.Services.Setup
{

    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        T Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
        void Save();
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private ShoppifyDatabaseConnection dbContext;

        static IDbFactory DbFactory;
        //private IDbFactory DbFactory;
        //{
        //    get;
        //    private set;
        //}

        protected ShoppifyDatabaseConnection DbContext
        {
            get { return dbContext ?? (dbContext = DbFactory.Init()); }
        }

        public GenericRepository()
        {
            DbFactory = new DbFactory();
            dbContext = new ShoppifyDatabaseConnection();
        }

        //public GenericRepository(IDbFactory dbFactory)
        //{
        //    DbFactory = dbFactory;
        //}

        public virtual IQueryable<T> GetAll()
        {

            IQueryable<T> query = dbContext.Set<T>();
            return query;
        }
        public virtual IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {

            IQueryable<T> query = dbContext.Set<T>().Where(predicate);
            return query;
        }

        public virtual T Add(T entity)
        {
            dbContext.Set<T>().Add(entity);
            Save();
            return entity;
        }

        public virtual void Delete(T entity)
        {
            dbContext.Set<T>().Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            Save();
        }

        public virtual void Save()
        {
            dbContext.SaveChanges();
        }
    }
}
