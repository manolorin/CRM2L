using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Helpers;
using Orders.Backend.Repositories.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Backend.Repositories.Implementations;

public class ContactsRepository: GenericRepository<Contact>, IContactsRepository
{
    private readonly DataContext _context;

    public ContactsRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<ActionResponse<Contact>> AddAsync(Contact contact)
    {
        _context.Add(contact);

        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<Contact>
            {
                WasSuccess = true,
                Result = contact
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<Contact>
            {
                Message = "No se puede agregar contacto"
            };
        }
    }

    public async Task<IEnumerable<Contact>> GetComboAsync(int prospectId)
    {
        return await _context.Contacts
            .Where(p => p.ProspectId == prospectId)
            .OrderBy(c => c.FullName)
            .ToListAsync();
    }

    public override async Task<ActionResponse<IEnumerable<Contact>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Contacts
            .Where(x => x.Prospect!.Id == pagination.Id)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<Contact>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.Name)
                .ThenBy(x => x.LastName)
                .Paginate(pagination)
                .ToListAsync()
        };
    }

    public override async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Contacts
            .Where(x => x.Prospect!.Id == pagination.Id)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();
        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }


    public override async Task<ActionResponse<IEnumerable<Contact>>> GetAsync()
    {
        var contacts = await _context.Contacts
            .Include(x => x.Prospect)
            .Include(x => x.City)
            .ThenInclude(x => x.State)
            .ThenInclude(x => x.Country)
            .ToListAsync();
        return new ActionResponse<IEnumerable<Contact>>
        {
            WasSuccess = true,
            Result = contacts
        };
    }

    public override async Task<ActionResponse<Contact>> GetAsync(int id)
    {
        var contact = await _context.Contacts
            .Include(x => x.Prospect)
            .Include(x => x.City)
            .ThenInclude(x => x.State)
            .ThenInclude(x => x.Country)
             .FirstOrDefaultAsync(x => x.Id == id);

        if (contact == null)
        {
            return new ActionResponse<Contact>
            {
                WasSuccess = false,
                Message = "Contacto no existe"
            };
        }

        return new ActionResponse<Contact>
        {
            WasSuccess = true,
            Result = contact
        };
    }

    public override async Task<ActionResponse<Contact>> UpdateAsync(Contact contact)
    {
        _context.Update(contact);

        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<Contact>
            {
                WasSuccess = true,
                Result = contact
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<Contact>
            {
                Message = "No se puede actualizar contacto"
            };
        }
    }
}
