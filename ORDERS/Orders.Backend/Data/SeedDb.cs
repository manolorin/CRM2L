using Microsoft.EntityFrameworkCore;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.Entities;
using Orders.Shared.Enums;

namespace Orders.Backend.Data;

public class SeedDb
{
    private readonly DataContext _context;
    private readonly IUsersUnitOfWork _usersUnitOfWork;

    public SeedDb(DataContext context, IUsersUnitOfWork usersUnitOfWork )
    {
        _context = context;
        _usersUnitOfWork = usersUnitOfWork;
    }

    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await CheckCountriesFullAsync();
        await CheckCategoriesAsync();
        await CheckCountriesAsync();
        await CheckRolesAsync();
        await CheckUserAsync("Número INE", "Francisco", "Rincon", "frincon@simaqap.com", "8110279500", "Aramberri 503, Col.Lazaro Cardenas Amp", UserType.Admin);
    }

    private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, string address, UserType userType)
    {
        var user = await _usersUnitOfWork.GetUserAsync(email);
        if (user == null)
        {
            user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = email,
                PhoneNumber = phone,
                Address = address,
                Document = document,
                City = _context.Cities.FirstOrDefault(),
                UserType = userType,
            };

            await _usersUnitOfWork.AddUserAsync(user, "123456");
            await _usersUnitOfWork.AddUserToRoleAsync(user, userType.ToString());
        }

        return user;
    }


    private async Task CheckRolesAsync()
    {
        await _usersUnitOfWork.CheckRoleAsync(UserType.Admin.ToString());
        await _usersUnitOfWork.CheckRoleAsync(UserType.Aux.ToString());
        await _usersUnitOfWork.CheckRoleAsync(UserType.Dir.ToString());
        await _usersUnitOfWork.CheckRoleAsync(UserType.Leader.ToString());
        await _usersUnitOfWork.CheckRoleAsync(UserType.Seller.ToString());
    }

    public async Task CheckCountriesFullAsync()
    {
        if (!_context.Countries.Any())
        {
            var countriesSQLScript = System.IO.File.ReadAllText("Data/CountriesStatesCities.sql");
            _context.Database.SetCommandTimeout(300); // Set timeout to 5 minutes
            await _context.Database.ExecuteSqlRawAsync(countriesSQLScript);
        }
    }   


    public async Task CheckCategoriesAsync()
    {
        if (!_context.Categories.Any())
        {
            _context.Categories.Add(new Category { Name = "AAA" });
            _context.Categories.Add(new Category { Name = "BBB" });
            _context.Categories.Add(new Category { Name = "CCC" });
            _context.Categories.Add(new Category { Name = "DDD" });
            _context.Categories.Add(new Category { Name = "EEE" });
            _context.Categories.Add(new Category { Name = "Espectáculos" });
            _context.Categories.Add(new Category { Name = "Financiera" });
            _context.Categories.Add(new Category { Name = "Industria" });
            _context.Categories.Add(new Category { Name = "Politica" });
            _context.Categories.Add(new Category { Name = "Retail" });
            _context.Categories.Add(new Category { Name = "Salud" });
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
