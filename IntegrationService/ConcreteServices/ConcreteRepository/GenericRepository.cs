using IntegrationService.Data.DbContexts;
using IntegrationService.Services.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace IntegrationService.ConcreteRepository
{
    public class GenericRepository<T> :
    IGenericRepository<T> where T : class,new()
    {
        private readonly ApplicationDbContext context;
        private DbSet<T> _entities;

        public GenericRepository(ApplicationDbContext context)
        {
            this.context = context;
            _entities = context.Set<T>();
        }
        public IQueryable<T> GetAll()
        {

            IQueryable<T> query = _entities.AsQueryable();
            return query;
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {

            IQueryable<T> query = _entities.Where(predicate);
            return query;
        }
       
        public void Add(T entity)
        {
            _entities.Add(entity);
        }

        public void Delete(T entity)
        {
            _entities.Remove(entity);
        }
        public void AddOrUpdate(T entity)
        {
            var entry = context.Entry(entity);
            switch (entry.State)
            {
                case EntityState.Detached:
                    context.Add(entity);
                    break;
                case EntityState.Modified:
                    context.Update(entity);
                    break;
                case EntityState.Added:
                    context.Add(entity);
                    break;
                case EntityState.Unchanged: 
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public void AddOrUpdateRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                var entry = context.Entry(entity);
                switch (entry.State)
                {
                    case EntityState.Detached:
                        context.Add(entity);
                        break;
                    case EntityState.Modified:
                        var modifiedEntity = entity as IEntityWithModifiedOn;
                        if (modifiedEntity != null)
                        {
                            modifiedEntity.ModifiedOn = DateTime.Now;
                        }
                        context.Update(entity);
                        break;
                    case EntityState.Added:
                        context.Add(entity);
                        break;
                    case EntityState.Unchanged: 
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        public void Update(T entity)
        {
            _entities.Update(entity);

        }

        public int Save()
        {
            var Saved = context.SaveChanges();
            return Saved;

        }

        public void AddRange(IEnumerable<T> entities)
        {
            _entities?.AddRange(entities);
        }

        public DatabaseFacade GetDb()
        {
            return context.Database;
        }
        public DbContext GetContext()
        {
            return context;
        }

        public IQueryable<T> Any(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _entities.Where(predicate);
            return query;
        }

        public EntityState GetEntityState(T entity)
        {
            var entry = context.Entry(entity);
            return entry.State;
        }

        //public void AddOrUpdateRange(IEnumerable<T> entities)
        //{
        //    throw new NotImplementedException();
        //}

        public interface IEntityWithModifiedOn
        {
            DateTime ModifiedOn { get; set; }
        }
    }
}
