using Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Api.Services.DBService
{
    public class DbService : IDbService
    {
        private readonly DBContextModel _dbContext;

        public DbService(DBContextModel dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public T FirstOrDefault<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, Object>>[] eagerProperties) where T : class
        {
            IQueryable<T> query = _dbContext.Set<T>().AsQueryable();
            query = eagerProperties.Aggregate(query, (current, e) => current.Include(e));
            return query.Where(predicate).FirstOrDefault();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Update<T>(T entity) where T : class
        {
           _dbContext.Set<T>().Update(entity);
            _dbContext.SaveChanges();
        }

        Task<int> IDbService.SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> QueryAll<TEntity>(params Expression<Func<TEntity, object>>[] eagerProperties) where TEntity : class
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>().AsQueryable();
            query = eagerProperties.Aggregate(query, (current, e) => current.Include(e));

            return query;
        }

        public IQueryable<TEntity> Query<TEntity>(Expression<Func<TEntity, Boolean>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties) where TEntity : class
        {
            return QueryAll(eagerProperties).Where(predicate);
        }
    }
}
