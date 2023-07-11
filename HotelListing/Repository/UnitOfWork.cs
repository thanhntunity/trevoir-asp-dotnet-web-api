using HotelListing.Data;
using HotelListing.IRepository;

namespace HotelListing.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;
    private IGenericRepository<Country> countries;
    private IGenericRepository<Hotel> hotels;

    public UnitOfWork(DataContext context)
    {
        _context = context;
    }
    public IGenericRepository<Country> CountryRepository => countries ??= new GenericRepository<Country>(_context);

    public IGenericRepository<Hotel> HotelRepository => hotels ??= new GenericRepository<Hotel>(_context);

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
}
