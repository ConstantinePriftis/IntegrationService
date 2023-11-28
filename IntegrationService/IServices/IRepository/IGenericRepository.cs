using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace IntegrationService.Services.Repository
{
    public interface IGenericRepository<T> where T : class, new()
    {
        IQueryable<T> GetAll();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        IQueryable<T> Any(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Delete(T entity);
        void AddOrUpdate(T entity);
        //void AddOrUpdateRange(IEnumerable<T> entities);
        void AddOrUpdateRange(IEnumerable<T> entities);
        void Update(T entity);
        EntityState GetEntityState(T entity);
        DatabaseFacade GetDb();
        DbContext GetContext();
        int Save();
    }
}
