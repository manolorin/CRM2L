using Microsoft.AspNetCore.Identity;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Backend.Repositories.Interfaces;

public interface IProspectsRepository
{
    Task<ActionResponse<Prospect>> AddAsync(Prospect prospect);
    Task<IEnumerable<Prospect>> GetComboAsync();
    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
    Task<ActionResponse<IEnumerable<Prospect>>> GetAsync(PaginationDTO pagination);
    Task<ActionResponse<Prospect>> GetAsync(int id);
    Task<ActionResponse<Prospect>> UpdateAsync(Prospect prospect);

}
