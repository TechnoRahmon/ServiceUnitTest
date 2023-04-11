using System.Linq.Expressions;

namespace Api.Services.DBService
{
    public interface IDbService
    {
        T FirstOrDefault<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, Object>>[] eagerProperties) where T : class;
        Task<int> SaveChangesAsync();
        void Update<T>(T entity) where T : class;
        IQueryable<TEntity> QueryAll<TEntity>(params Expression<Func<TEntity, object>>[] eagerProperties) where TEntity : class;
        IQueryable<TEntity> Query<TEntity>(Expression<Func<TEntity, Boolean>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties) where TEntity : class;
    }
}