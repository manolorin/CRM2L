using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Repositories.Interfaces;
using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Backend.Repositories.Implementations;

public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
{
    private readonly DataContext _context;

    public CountriesRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async override  Task<ActionResponse<IEnumerable<Country>>> GetAsync()
    {
        var countries =await  _context.Countries
                            .Include(x => x.States)
                            .ToArrayAsync();
        return new ActionResponse<IEnumerable<Country>>()
        {
            WasSuccess = true,
            Result = countries
        };
    }

    public async override Task<ActionResponse<Country>> GetAsync(int id)
    {
        var country =await _context.Countries
                    .Include(x => x.States!)
                    .ThenInclude(s => s.Cities)
                    .FirstOrDefaultAsync(x => x.Id == id);
        if (country == null)
        {
            return new ActionResponse<Country>
            {
                
                Message = "Registro no encontrado"
            };  
        }

        return new ActionResponse<Country>
        {
            WasSuccess = true,
            Result = country
        };  
    }
}
