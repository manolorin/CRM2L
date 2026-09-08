using Orders.Shared.Entities;

namespace Orders.Backend.Data;

public class SeedDb
{
    private readonly DataContext _context;

    public SeedDb(DataContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await CheckCategoriesAsync();
        await CheckCountriesAsync();
    }   


    public async Task CheckCategoriesAsync()
    {
        if (!_context.Categories.Any())
        {
            _context.Categories.Add(new Category { Name = "Espectáculos" });
            _context.Categories.Add(new Category { Name = "Financiera" });
            _context.Categories.Add(new Category { Name = "Politica" });
            await _context.SaveChangesAsync();
        }
    }

    public async Task CheckCountriesAsync()
    {
        if (!_context.Countries.Any())
        {
            _context.Countries.Add(new Country { Name = "Angola" });
            _context.Countries.Add(new Country { Name = "Argelia" });
            _context.Countries.Add(new Country { Name = "Belice" });
            await _context.SaveChangesAsync();
        }
    }   

}
