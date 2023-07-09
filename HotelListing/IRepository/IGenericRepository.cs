using System.Linq.Expressions;

namespace HotelListing.IRepository;

public interface IGenericRepository<T> where T : class
{
    Task<IList<T>> GetAll(Expression<Func<T, bool>>? expression, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy, List<string>? includes);
    Task<T> Get(Expression<Func<T, bool>> expression, List<string>? includes);
    Task Insert(T entity);
    Task InsertRange(IEnumerable<T> entities);
    Task Delete(int id);
    void DeleteRange(IEnumerable<T> entities);
    void Update(T entity);
}
