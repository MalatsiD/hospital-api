using System.Linq.Expressions;

namespace Hospital_API.Data.Abstract
{
    public interface IEntityBaseRepository<T> where T : class, new()
    {
        T Add(T entity);
        IEnumerable<T> AddBulk(IEnumerable<T> entities);
        bool Update(T entity);
        IEnumerable<T> UpdateBulk(IEnumerable<T> entities);
        IQueryable<T> GetAll();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        bool Delete(T entity);
        bool DeleteWhere(Expression<Func<T, bool>> predicate);
        void Commit();
    }
}
