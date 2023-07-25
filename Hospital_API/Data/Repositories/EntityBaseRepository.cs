using Hospital_API.Data.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Hospital_API.Data.Repositories
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : class, new()
    {
        private readonly MyDbContext _context;

        public EntityBaseRepository(MyDbContext context)
        {
            _context = context;
        }

        public T Add(T entity)
        {
            _context.Entry<T>(entity);
            _context.Set<T>().Add(entity);

            return entity;
        }

        public IEnumerable<T> AddBulk(IEnumerable<T> entities)
        {
            foreach(var entity in entities) 
            {
                _context.Entry<T>(entity);
                _context.Set<T>().Add(entity);
            }

            return entities;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().AsQueryable().Where(predicate);
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsQueryable();
        }

        public bool Update(T entity)
        {
            EntityEntry entry = _context.Entry<T>(entity);
            entry.State = EntityState.Modified;

            return true;
        }

        public IEnumerable<T> UpdateBulk(IEnumerable<T> entities)
        {
            foreach(var entity in entities)
            {
                EntityEntry entry = _context.Entry<T>(entity);
                entry.State = EntityState.Modified;
            }

            return entities;
        }

        public virtual bool Delete(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Deleted;

            return true;
        }

        public virtual bool DeleteWhere(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> entities = _context.Set<T>().Where(predicate);

            foreach (var entity in entities)
            {
                _context.Entry<T>(entity).State = EntityState.Deleted;
            }

            return true;
        }
    }
}
