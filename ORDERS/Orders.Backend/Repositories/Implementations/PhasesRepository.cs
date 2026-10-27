using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Helpers;
using Orders.Backend.Repositories.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Backend.Repositories.Implementations;

public class PhasesRepository : GenericRepository<Phase>, IPhasesRepository
{
    private readonly DataContext _context;
    public PhasesRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Phase>> GetComboAsync()
    {
        return await _context.Phases
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public override async Task<ActionResponse<IEnumerable<Phase>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Phases.AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<Phase>>
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
        var queryable = _context.Phases.AsQueryable();

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
}
