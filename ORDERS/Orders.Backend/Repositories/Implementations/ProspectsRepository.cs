using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Helpers;
using Orders.Backend.Repositories.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Backend.Repositories.Implementations;

public class ProspectsRepository : GenericRepository<Prospect>, IProspectsRepository
{
    private readonly DataContext _context;

    public ProspectsRepository(DataContext context): base(context)
    {
        _context = context;
    }

    public override async Task<ActionResponse<Prospect>> AddAsync(Prospect prospect)
    {
        _context.Add(prospect);

        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<Prospect>
            {
                WasSuccess = true,
                Result = prospect
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<Prospect>
            {
                Message = "No se puede agregar registro"
            };
        }
    }

   

    public override async Task<ActionResponse<Prospect>> GetAsync(int Id)
    {

        var prospect = await _context.Prospects
            .Include(p => p.City!)
            .ThenInclude(c => c.State!)
            .ThenInclude(s => s.Country)
            .FirstOrDefaultAsync(x => x.Id == Id);

        if (prospect == null)
        {
            return new ActionResponse<Prospect>
            {
                WasSuccess = false,
                Message = "Prospecto no existe"
            };
        }

        return new ActionResponse<Prospect>
        {
            WasSuccess = true,
            Result = prospect
        };

    }

    public async Task<IEnumerable<Prospect>> GetComboAsync()
    {
        return await _context.Prospects
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public override async Task<ActionResponse<IEnumerable<Prospect>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Prospects
             .Include(c => c.Contacts)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<Prospect>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.Name)
                .Paginate(pagination)
                .ToListAsync()
        };
    }

    public override async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Prospects.AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        try
        {
            double count = await queryable.CountAsync();
            return new ActionResponse<int>
            {
                WasSuccess = true,
                Result = (int)count
            };
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public override async Task<ActionResponse<Prospect>> UpdateAsync(Prospect prospect)
    {
        _context.Update(prospect);

        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<Prospect>
            {
                WasSuccess = true,
                Result = prospect
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<Prospect>
            {
                Message = "No se puede actualizar registro"
            };
        }
    }


}