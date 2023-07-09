using HotelListing.Data;
using HotelListing.IRepository;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly DataContext _context;
    private readonly DbSet<T> _db;

    public GenericRepository(DataContext context)
    {
        _context = context;
        _db = _context.Set<T>();
    }

    public async Task Delete(int id)
    {
        var entity = await _db.FindAsync(id);
        _db.Remove(entity);
    }

    public void DeleteRange(IEnumerable<T> entities)
    {
        _db.RemoveRange(entities);
    }

    public async Task<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> expression, List<string>? includes)
    {
        IQueryable<T> query = _db;
        if (includes is not null)
        {
            foreach (var includeProperty in includes)
            {
                query = query.Include(includeProperty);
            }
        }

        return await query.AsNoTracking().FirstOrDefaultAsync(expression);
    }

    public async Task<IList<T>> GetAll(System.Linq.Expressions.Expression<Func<T, bool>>? expression, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy, List<string>? includes)
    {
        IQueryable<T> query = _db;

        if (expression is not null)
        {
            query = query.Where(expression);
        }

        if (includes is not null)
        {
            foreach (var includeProperty in includes)
            {
                query = query.Include(includeProperty);
            }
        }

        if (orderBy is not null)
        {
            query = orderBy(query);
        }

        return await query.AsNoTracking().ToListAsync();
    }

    public async Task Insert(T entity)
    {
        await _db.AddAsync(entity);
    }

    public async Task InsertRange(IEnumerable<T> entities)
    {
        await _db.AddRangeAsync(entities);
    }

    public void Update(T entity)
    {
       _db.Attach(entity);
       _context.Entry(entity).State = EntityState.Modified;
    }
}
